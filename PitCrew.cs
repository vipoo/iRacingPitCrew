// This file is part of iRacingPitCrew Application.
//
// Copyright 2014 Dean Netherton
// https://github.com/vipoo/iRacingPitCrew.Net
//
// iRacingPitCrew is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// iRacingPitCrew is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with iRacingPitCrew.  If not, see <http://www.gnu.org/licenses/>.

using System.Linq;
using iRacingPitCrew.Support;
using System.Diagnostics;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System;

namespace iRacingPitCrew
{
    public class PitCrew
    {
        SpeechRecognitionEngine recognizer;
        SpeechSynthesizer synthesizer;

        public void Start()
        {
            synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;
            synthesizer.Rate = -2;

            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();

            recognizer.LoadGrammar(ResetPitStop, "pit crew reset");

            recognizer.LoadGrammar(FuelStrategy, "pit crew fuel strategy");
            recognizer.LoadGrammar(RaceStatus, "pit crew race status");
            recognizer.LoadGrammar(TyreOff, "pit crew no tyre change");
            recognizer.LoadGrammar(TyreOff, "pit crew tyre change off");

            recognizer.LoadGrammar(TyreOn, "pit crew change all tyres");
            recognizer.LoadGrammar(TyreOn, "pit crew tyre change on");

            recognizer.LoadGrammar(SetFuel, g => {
                g.Append("pit crew set fuel");
                g.Append(new SemanticResultKey("fuel_amount", Number()));
            });

            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void ResetPitStop(RecognitionResult obj)
        {
            iRacingSDK.iRacing.PitCommand.Clear();
            synthesizer.Speak("No tyres fuel or windscreen cleaning at your next pit stop.");
        }

        void RaceStatus(RecognitionResult rr)
        {
            var d = iRacingSDK.iRacing.GetDataFeed().First();

            var session = d.SessionData.SessionInfo.Sessions[d.Telemetry.SessionNum];

            if (!session.IsRace )
            {
                synthesizer.Speak("You are not in a race");
                return;
            }

            if (session.IsLimitedTime)
            {
                var sessionTimeSpanRemaining = TimeSpan.FromSeconds(d.Telemetry.SessionTimeRemain);

                synthesizer.Speak(string.Format("There are {0} minutes remaining in this race.", (int)sessionTimeSpanRemaining.TotalMinutes));
            }
            else if( session.IsLimitedSessionLaps)
            {
                synthesizer.Speak(string.Format("There are {0} laps remaining in this race.", d.Telemetry.SessionLapsRemain));
            }

            synthesizer.Speak(string.Format("You {0} litres of fuel.", (int)d.Telemetry.FuelLevel));
        }

        void TyreOff(RecognitionResult rr)
        {
            synthesizer.Speak("Will not be changing tyres at next pit stop.");
            iRacingSDK.iRacing.PitCommand.ClearTireChange();
        }


        void TyreOn(RecognitionResult rr)
        {
            synthesizer.Speak("We will change your tyes at next pit stop.");
            iRacingSDK.iRacing.PitCommand.ChangeAllTyres();
        }

        void SetFuel(RecognitionResult rr)
        {
            var a = (int)rr.Semantics["fuel_amount"].Value;

            if (a == 0)
            {
                iRacingSDK.iRacing.PitCommand.SetFuel(1);
                synthesizer.Speak(string.Format("No fuel at your next pit stop.", a));
            }
            else
            {
                iRacingSDK.iRacing.PitCommand.SetFuel((int)a);
                synthesizer.Speak(string.Format("You will get {0} litres of fuel at next pit stop.", a));
            }
        }

        void FuelStrategy(RecognitionResult rr)
        {
            synthesizer.Speak("Fuel strategy is unknown.");
        }

        Choices Number()
        {
            var digits = new Choices();

            for (int i = 0; i < 101; i++)
                digits.Add(new SemanticResultValue(i.ToString(), i));

            return digits;
        }
    }
}

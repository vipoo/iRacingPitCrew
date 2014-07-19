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
        readonly DataCollector dataCollector;
        readonly SpeechRecognitionEngine recognizer;
        readonly SpeechSynthesizer synthesizer;

        public PitCrew(DataCollector dataCollector)
        {
            this.dataCollector = dataCollector;
            this.dataCollector.Connected += dataCollector_Connected;
            this.dataCollector.Disconnected += dataCollector_Disconnected;

            synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;
            synthesizer.Rate = -2;

            recognizer = new SpeechRecognitionEngine();
        }

        void dataCollector_Disconnected()
        {
            synthesizer.Speak("Disconnected from I Racing, your pit crew have gone away.");

        }   

        void dataCollector_Connected()
        {
            synthesizer.Speak("Connected to I Racing, your pit crew is standing by for your commands.");
        }

        public void Start()
        {
            recognizer.SetInputToDefaultAudioDevice();

            recognizer.LoadGrammar(ProcessPitCommand, ResetPitStop, "pit crew reset");
            recognizer.LoadGrammar(ProcessPitCommand, FuelStrategy, "pit crew fuel strategy");
            recognizer.LoadGrammar(ProcessPitCommand, RaceStatus, "pit crew race status");
            recognizer.LoadGrammar(ProcessPitCommand, TyreOff, "pit crew no tyre change");
            recognizer.LoadGrammar(ProcessPitCommand, TyreOff, "pit crew tyre change off");

            recognizer.LoadGrammar(ProcessPitCommand, TyreOn, "pit crew change all tyres");
            recognizer.LoadGrammar(ProcessPitCommand, TyreOn, "pit crew tyre change on");

            recognizer.LoadGrammar(ProcessPitCommand, SetFuel, g =>
            {
                g.Append("pit crew set fuel");
                g.Append(new SemanticResultKey("fuel_amount", Number()));
            });

            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            dataCollector.Start();
        }

        internal void Stop()
        {
            dataCollector.Stop();
        }

        bool ProcessPitCommand(RecognitionResult rr)
        {
            Trace.WriteLine(string.Format("Confidence is {0} percent", (int)(rr.Confidence * 100.0)));
            Trace.WriteLine(string.Format("Alt couunt is {0}", (int)(rr.Alternates.Count)));
            foreach (var alt in rr.Alternates)
                Trace.WriteLine(string.Format("{0}, {1}", alt.Confidence, alt.Text));

            if( rr.Confidence < 0.9)
            {
                Trace.WriteLine("Ignore bad match");
                return false;
            }
            if (dataCollector.IsConnectedToiRacing )
                return true;

            synthesizer.Speak("Disconnected from i racing.  Unable to process your command.");
            return false;
        }

        void ResetPitStop(RecognitionResult rr)
        {
            iRacingSDK.iRacing.PitCommand.Clear();
            synthesizer.Speak("No tyres fuel or windscreen cleaning at your next pit stop.");
        }

        void RaceStatus(RecognitionResult rr)
        {
            var d = dataCollector.Data;
            if( d == null)
            {
                synthesizer.Speak("Yet to received any data from iRacing");
                return;
            }

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
            synthesizer.Speak(string.Format("You are on lap {0}.", (int)d.Telemetry.FuelLevel));

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

            for (int i = 0; i < 121; i++)
                digits.Add(new SemanticResultValue(i.ToString(), i));

            return digits;
        }
    }
}

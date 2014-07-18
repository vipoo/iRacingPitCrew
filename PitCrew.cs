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

using iRacingPitCrew.Support;
using System.Diagnostics;
using System.Speech.Recognition;
using System.Speech.Synthesis;

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

            recognizer.LoadGrammar(FuelStrategy, "pit crew fuel strategy");
            recognizer.LoadGrammar(RaceStatus, "pit crew race status");
            recognizer.LoadGrammar(TyreOff, "pit crew no tyre change");
            recognizer.LoadGrammar(TyreOff, "pit crew tyre change off");

            recognizer.LoadGrammar(SetFuel, g => {
                g.Append("pit crew set fuel");
                g.Append(new SemanticResultKey("fuel_amount", Number()));
            });

            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        void RaceStatus(RecognitionResult rr)
        {
            Trace.WriteLine("Race status is unknown");
            synthesizer.Speak("Race status is unknown");
        }

        void TyreOff(RecognitionResult rr)
        {
            synthesizer.Speak("Will not be changing tyes at next pit stop.");
        }

        void SetFuel(RecognitionResult rr)
        {
            var a = (int)rr.Semantics["fuel_amount"].Value;

            if (a == 0)
            {
                iRacingSDK.iRacing.PitCommand.Clear();
                synthesizer.Speak("Clearing");
            }
            else
            {
                iRacingSDK.iRacing.PitCommand.SetFuel((int)a);
                synthesizer.Speak(string.Format("Setting Fuel to {0}", a));
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

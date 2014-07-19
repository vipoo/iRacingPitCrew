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

using System.Diagnostics;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace iRacingPitCrew.PitCrewCommands
{
    public abstract class PitCrewCommand
    {
        protected readonly SpeechRecognitionEngine recognizer;
        protected readonly DataCollector dataCollector;
        protected readonly SpeechSynthesizer synthesizer;

        public PitCrewCommand(SpeechRecognitionEngine recognizer, DataCollector dataCollector, System.Speech.Synthesis.SpeechSynthesizer synthesizer)
        {
            this.recognizer = recognizer;
            this.dataCollector = dataCollector;
            this.synthesizer = synthesizer;
        }

        public abstract void Start();

        protected bool ProcessCommand(RecognitionResult rr)
        {
            Trace.WriteLine("---------------------", "DEBUG");
            foreach (var alt in rr.Alternates)
                Trace.WriteLine(string.Format("{0:00.00}%, {1}", alt.Confidence * 100f, alt.Text), "DEBUG");

            if (rr.Confidence < 0.9)
            {
                Trace.WriteLine("Ignore bad match", "INFO");
                Trace.WriteLine("---------------------", "DEBUG");
                return false;
            }

            Trace.WriteLine("---------------------", "DEBUG");
            return true;
        }

        protected bool ProcessPitCommand(RecognitionResult rr)
        {
            if (!ProcessCommand(rr))
                return false;

            if (dataCollector.IsConnectedToiRacing)
                return true;

            synthesizer.SpeakAsync("Disconnected from i racing.  Unable to process your command.");
            return false;
        }

        protected Choices Number()
        {
            var digits = new Choices();

            for (int i = 0; i < 121; i++)
                digits.Add(new SemanticResultValue(i.ToString(), i));

            return digits;
        }
    }
}

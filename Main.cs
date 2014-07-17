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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace iRacingPitCrew
{
    public partial class Main : Form
    {
        SpeechRecognitionEngine recognizer;
        SpeechSynthesizer synthesizer;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            synthesizer = new SpeechSynthesizer();
            synthesizer.Volume = 100;
            synthesizer.Rate = -2;

            recognizer = new SpeechRecognitionEngine();
            recognizer.SetInputToDefaultAudioDevice();

            var grammarFuelStrategy = new Grammar(new GrammarBuilder("pit crew. fuel strategy"));
            grammarFuelStrategy.SpeechRecognized += grammarFuelStrategy_SpeechRecognized;

            var gb = new GrammarBuilder();
            gb.Append("pit crew. set fuel");
            gb.Append(new SemanticResultKey("fuel_amount", Number()));
            var grammarSetFuel = new Grammar(gb);
            grammarSetFuel.SpeechRecognized += grammarSetFuel_SpeechRecognized;

            var grammarTyreOff = new Grammar(new Choices( new GrammarBuilder("pit crew. no tyre change"), new GrammarBuilder("pit crew. tyre change off") ));
            grammarTyreOff.SpeechRecognized += grammarTyreOff_SpeechRecognized;

            var grammarRaceStatus = new Grammar(new GrammarBuilder("pit crew race status"));
            grammarRaceStatus.SpeechRecognized += grammarRaceStatus_SpeechRecognized;

            recognizer.LoadGrammar(grammarFuelStrategy);
            recognizer.LoadGrammar(grammarSetFuel);
            recognizer.LoadGrammar(grammarTyreOff);
            recognizer.LoadGrammar(grammarRaceStatus);

            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        void grammarRaceStatus_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Trace.WriteLine("Race status is unknown");
            synthesizer.Speak("Race status is unknown");
        }

        void grammarTyreOff_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            synthesizer.Speak("Will not be changing tyes at next pit stop.");
        }

        void grammarSetFuel_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var a = (int)e.Result.Semantics["fuel_amount"].Value;

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

        void grammarFuelStrategy_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
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

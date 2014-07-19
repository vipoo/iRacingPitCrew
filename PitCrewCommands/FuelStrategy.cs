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
using System;
using System.Speech.Recognition;

namespace iRacingPitCrew.PitCrewCommands
{
    public class FuelStrategy : PitCrewCommand
    {
        public FuelStrategy(SpeechRecognitionEngine recognizer, DataCollector dataCollector, System.Speech.Synthesis.SpeechSynthesizer synthesizer)
            : base(recognizer, dataCollector, synthesizer)
        {
        }

        public override void Start()
        {
            recognizer.LoadGrammar(ProcessPitCommand, Command, "pit crew fuel strategy");
        }

        void Command(RecognitionResult rr)
        {
            var session = dataCollector.Data.SessionData.SessionInfo.Sessions[dataCollector.Data.Telemetry.SessionNum];
            if( !session.IsRace )
            {
                recognizer.SetEnabled(false);

                synthesizer.SpeakAsync("What is your race length?");

                using (var rec = new SpeechRecognitionEngine())
                {
                    rec.LoadGrammar(ProcessCommand, rrr =>
                    {
                        var laps = (int)rrr.Semantics["amount"].Value;
                        synthesizer.SpeakAsync(string.Format("Your race length is {0}", rrr.Text));
                    }, g =>
                    {
                        g.Append(new SemanticResultKey("amount", Number()));
                        g.Append(new Choices(new GrammarBuilder("laps"), new GrammarBuilder("minutes")));
                    });

                    rec.SetInputToDefaultAudioDevice();

                    rec.Recognize(TimeSpan.FromSeconds(15));
                }

                recognizer.SetEnabled(true);
            }
            
            synthesizer.SpeakAsync(string.Format("Your average fuel usage is {0:0.00} litres per lap.", dataCollector.AverageFuelUsage));
        }
    }
}

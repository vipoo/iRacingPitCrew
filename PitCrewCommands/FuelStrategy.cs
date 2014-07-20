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
using System.Diagnostics;
using System.Speech.Recognition;
using System.Threading;
using System.Threading.Tasks;

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
            recognizer.SpeechRecognized += recognizer_SpeechRecognized;
            grammerFuelStrategy = recognizer.LoadGrammar(ProcessPitCommand, Command, "pit crew fuel strategy");
            grammarRaceLength = recognizer.LoadGrammar(ProcessCommand, GetRaceLength, g =>
            {
                g.Append(new SemanticResultKey("amount", Number()));
                g.Append(new Choices(new GrammarBuilder("laps"), new GrammarBuilder("minutes")));
            });
            grammarRaceLength.Enabled = false;
        }

        void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Grammar == grammerFuelStrategy)
                return;

            if (e.Result.Grammar != grammarRaceLength)
                DisableRaceLengthGrammar();
        }

        private void DisableRaceLengthGrammar(object state = null)
        {
            grammarRaceLength.Enabled = false;

            if (timer != null)
                timer.Dispose();
        }

        int raceLaps = 0;
        Grammar grammarRaceLength;
        Timer timer;
        Grammar grammerFuelStrategy;

        void Command(RecognitionResult rr)
        {
            var session = dataCollector.Data.SessionData.SessionInfo.Sessions[dataCollector.Data.Telemetry.SessionNum];
            if( !session.IsRace )
            {
                grammarRaceLength.Enabled = true;
                synthesizer.SpeakAsync("What is your race length?");

                timer = new Timer(DisableRaceLengthGrammar, null, 15000, Timeout.Infinite);
            }

            if (dataCollector.AverageFuelPerLap > 0)
            {
                synthesizer.SpeakAsync(string.Format("Your average fuel usage is {0:0.00} litres per lap.", dataCollector.AverageFuelPerLap));
                synthesizer.SpeakAsync(string.Format("You will need a total of {0:0.00} litres", dataCollector.AverageFuelPerLap * raceLaps));
            }

            if( dataCollector.AverageLapTime > 0 )
            {
                var t = TimeSpan.FromSeconds(dataCollector.AverageLapTime);
                
                synthesizer.SpeakAsync(string.Format("Your average lap time is {0:0}, {0:00.00}.", (int)t.TotalMinutes, (float)t.Seconds + ((float)t.Milliseconds)/1000f));
            }
        }

        void GetRaceLength(RecognitionResult rrr)
        {
            raceLaps = (int)rrr.Semantics["amount"].Value;
            synthesizer.SpeakAsync(string.Format("Your race length is {0}", rrr.Text));

            DisableRaceLengthGrammar();
        }
    }
}

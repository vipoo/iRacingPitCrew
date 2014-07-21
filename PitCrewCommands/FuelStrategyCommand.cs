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
using iRacingSDK.Support;
using System;
using System.Diagnostics;
using System.Speech.Recognition;
using System.Threading;

namespace iRacingPitCrew.PitCrewCommands
{
    public class FuelStrategyCommand : PitCrewCommand
    {
        public FuelStrategyCommand(SpeechRecognitionEngine recognizer, DataCollector dataCollector, System.Speech.Synthesis.SpeechSynthesizer synthesizer)
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
        private TimeSpan raceDuration;

        void Command(RecognitionResult rr)
        {
            if (dataCollector.AverageFuelPerLap <= 0 || dataCollector.AverageLapTimeSpan.TotalSeconds <= 0)
            {
                synthesizer.SpeakAsync("Do not have any fuel consumption or lap timing data.  You need to do 5 laps at least");
                return;
            }

            var session = dataCollector.Data.SessionData.SessionInfo.Sessions[dataCollector.Data.Telemetry.SessionNum];
            if( !session.IsRace )
            {
                grammarRaceLength.Enabled = true;
                synthesizer.SpeakAsync("What is your race length?");

                timer = new Timer(DisableRaceLengthGrammar, null, 15000, Timeout.Infinite);
            }
        }

        void GetRaceLength(RecognitionResult rrr)
        {
            DisableRaceLengthGrammar();

            if (rrr.Text.EndsWith("laps"))
            {
                raceLaps = (int)rrr.Semantics["amount"].Value;
                raceDuration = new TimeSpan();
            }
            else
            {
                raceDuration = TimeSpan.FromMinutes((int)rrr.Semantics["amount"].Value);
                raceLaps = 0;
            }
            synthesizer.SpeakAsync(string.Format("Your race length is {0}", rrr.Text));

            var t = dataCollector.AverageLapTimeSpan;

            Trace.WriteLine("Your average lap time is {0:0}, {1:0.00}.".F((int)t.TotalMinutes, (float)t.Seconds + ((float)t.Milliseconds) / 1000f));
            synthesizer.SpeakAsync("Your average lap time is {0:0}, {1:0.00}.".F((int)t.TotalMinutes, (float)t.Seconds + ((float)t.Milliseconds) / 1000f));

            Trace.WriteLine("Your average fuel usage is {0:0.00} litres per lap.".F(dataCollector.AverageFuelPerLap));
            synthesizer.SpeakAsync("Your average fuel usage is {0:0.00} litres per lap.".F(dataCollector.AverageFuelPerLap));

            if (raceLaps == 0)
            {
                var r = FuelStrategy.Calculate(raceDuration, dataCollector.AverageFuelPerLap, dataCollector.AverageLapTimeSpan);

                Trace.WriteLine("Estimating you will do {0} laps in {1} minutes".F(r.EstimatedNumberOfRaceLaps, r.RaceDuration.TotalMinutes));
                synthesizer.SpeakAsync("Estimating you will do {0} laps in {1} minutes".F(r.EstimatedNumberOfRaceLaps, (int)r.RaceDuration.TotalMinutes));

                Trace.WriteLine("For a {0} minute race, you will need a total of {1} litres".F((int)r.RaceDuration.TotalMinutes, r.TotalFuelRequired));
                synthesizer.SpeakAsync("For a {0} minute race, you will need a total of {1} litres".F((int)r.RaceDuration.TotalMinutes, r.TotalFuelRequired));
            }
            else
            {
                var r = FuelStrategy.Calculate(raceLaps, dataCollector.AverageFuelPerLap);

                Trace.WriteLine("For a {0} lap race, you will need a total of {1} litres".F(raceLaps, r.TotalFuelRequired));
                synthesizer.SpeakAsync("For a {0} lap race, you will need a total of {1} litres".F(raceLaps, r.TotalFuelRequired));
            }
        }
    }
}

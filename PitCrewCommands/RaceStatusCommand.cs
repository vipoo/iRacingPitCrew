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
using System.Speech.Recognition;
using System.Speech.Synthesis;
using iRacingSDK.Support;

namespace iRacingPitCrew.PitCrewCommands
{
    public class RaceStatusCommand : PitCrewCommand
    {
        readonly DataCollector dataCollector;

        public RaceStatusCommand(SpeechRecognitionEngine recognizer, SpeechSynthesizer synthesizer, Action recognized, DataCollector dataCollector)
            : base(recognizer, synthesizer, recognized)
        {
            this.dataCollector = dataCollector;
            SetGrammar("race status");
        }

        protected override void Command(RecognitionResult rr)
        {
            var d = dataCollector.Data;
            if (d == null)
            {
                SpeakAsync("Yet to received any data from iRacing");
                return;
            }

            var session = d.SessionData.SessionInfo.Sessions[d.Telemetry.SessionNum];

            if (!session.IsRace)
            {
                SpeakAsync("You are not in a race");
                return;
            }

            if (session.IsLimitedTime)
            {
                var sessionTimeSpanRemaining = TimeSpan.FromSeconds(d.Telemetry.SessionTimeRemain);

                SpeakAsync(string.Format("There are {0} minutes remaining in this race.", (int)sessionTimeSpanRemaining.TotalMinutes));

                var r = FuelStrategy.CalculateToFinish(
                    fuelLevel: d.Telemetry.FuelLevel,
                    remainingTime: sessionTimeSpanRemaining,
                    raceDuration: dataCollector.RaceDuration.TotalMinutes,
                    averageFuelBurnPerLap: dataCollector.AverageFuelPerLap,
                    averageLapTime: dataCollector.AverageLapTimeSpan,
                    fuelTankCapacity: (int)dataCollector.TankSize);

                SpeakAsync("There are about {0} laps remaining".F(r.EstimateLapsRemaining));
                SpeakAsync("You need an additional {0} litres to finish".F(r.TotalFuelRequired));
                SpeakAsync("At your next stop, you need {0} litres".F(r.TotalFuelRequiredAtNextStop));
                if (r.PitWindowOpened)
                    SpeakAsync("Your pit window is open");
                else
                    SpeakAsync("Your pit stop window will open in about {0} laps".F(r.LapsToPitWindow));
            }

            else if (session.IsLimitedSessionLaps)
            {
                SpeakAsync(string.Format("There are {0} laps remaining", d.Telemetry.SessionLapsRemain));
                
                var r = FuelStrategy.CalculateToFinish(
                    fuelLevel: d.Telemetry.FuelLevel,
                    remainingLaps: d.Telemetry.SessionLapsRemain,
                    raceDuration: dataCollector.RaceDuration.TotalMinutes,
                    averageFuelBurnPerLap: dataCollector.AverageFuelPerLap,
                    averageLapTime: dataCollector.AverageLapTimeSpan,
                    fuelTankCapacity: (int)dataCollector.TankSize);

                SpeakAsync("You need an additional {0} litres to finish".F(r.TotalFuelRequired));
                SpeakAsync("At your next stop, you need {0} litres".F(r.TotalFuelRequiredAtNextStop));
                if (r.PitWindowOpened)
                    SpeakAsync("Your pit window is open");
                else
                    SpeakAsync("Your pit stop window will open in about {0} laps".F(r.LapsToPitWindow));
            }
        }
    }
}

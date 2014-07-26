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
    public class SessionCommand : PitCrewCommand
    {
        readonly DataCollector dataCollector;

        public SessionCommand(SpeechRecognitionEngine recognizer, SpeechSynthesizer synthesizer, Action recognized, DataCollector dataCollector)
            : base(recognizer, synthesizer, recognized)
        {
            this.dataCollector = dataCollector;
            SetGrammar("session status");
        }

        protected override void Command(RecognitionResult rr)
        {
            var d = dataCollector.Data;
            if (d == null)
            {
                SpeakAsync("Yet to received any data from iRacing");
                return;
            }

            var session = d.Telemetry.Session;;

            SpeakAsync("You are in a {0} session.".F(session.SessionType));
            TimeSpan sessionTimeSpanRemaining = TimeSpan.MaxValue;

            if (session.IsLimitedTime)
            {
                sessionTimeSpanRemaining = TimeSpan.FromSeconds(d.Telemetry.SessionTimeRemain);
                SpeakAsync("There is {0} minutes remaining in the session".F((int)sessionTimeSpanRemaining.TotalMinutes));
            }

            var r = FuelStrategy.CalculateRemainingLapsForFuel(
                fuelLevel: d.Telemetry.FuelLevel,
                averageFuelBurnPerLap: dataCollector.AverageFuelPerLap,
                averageLapTime: dataCollector.AverageLapTimeSpan);
                
            SpeakAsync("You will run out of fuel in about {0} laps.".F(r.EstimateLapsRemaining));
            SpeakAsync("You will run out of fuel in about {0} minutes.".F((int)r.EstimateTimeRemaining.TotalMinutes));

        }
    }
}

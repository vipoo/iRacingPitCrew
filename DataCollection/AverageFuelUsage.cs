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

using iRacingSDK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace iRacingPitCrew.DataCollection
{
    public static class AverageFuelUsage
    {
        public static Func<DataSample, LapTransition, bool> Capture(Action<float> newAverage)
        {
            var last5LapFuelUsage = new List<float>();
            var lastFuelAmount = 0f;
            var averageFuelUsage = 0f;

            return (data, lapTransition) => 
            {
                var telemetry = data.Telemetry;
                var car = telemetry.CamCar;

                switch (lapTransition)
                {
                    case LapTransition.None:
                        lastFuelAmount = 0f;
                        last5LapFuelUsage.Clear();
                        break;

                    case LapTransition.StartFastLap:
                        lastFuelAmount = telemetry.FuelLevel;
                        break;

                    case LapTransition.CompletedFastLap:
                        var fuelUsed = lastFuelAmount - telemetry.FuelLevel;
                        lastFuelAmount = telemetry.FuelLevel;

                        if (last5LapFuelUsage.Count >= 5)
                            last5LapFuelUsage.RemoveAt(0);

                        last5LapFuelUsage.Add(fuelUsed);

                        averageFuelUsage = last5LapFuelUsage.Average();

                        Trace.WriteLine(string.Format("Finished lap with fuel burn of {0:0.00}. Avg: {1:0.00}", fuelUsed, averageFuelUsage), "INFO");

                        newAverage(averageFuelUsage);
                        break;
                }
                
                return true;
            };
        }
    }
}

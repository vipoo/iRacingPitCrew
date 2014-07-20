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
using iRacingSDK.Support;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace iRacingPitCrew.DataCollection
{
    public static class AverageTime
    {
        public static Func<DataSample, LapTransition, bool> Capture(Action<float> newAverage)
        {
            var last5Laps = new List<float>();

            return (data, lapTransition) =>
            {
                var telemetry = data.Telemetry;
                var car = telemetry.CamCar;

                switch (lapTransition)
                {
                    case LapTransition.None:
                        last5Laps.Clear();
                        break;

                    case LapTransition.StartFastLap:
                        break;

                    case LapTransition.CompletedFastLap:
                        if (last5Laps.Count >= 5)
                            last5Laps.RemoveAt(0);

                        last5Laps.Add(telemetry.LapLastLapTime);

                        var average = last5Laps.Average();

                        Trace.WriteLine("Finished lap with time: {0}.  Average of: {1}".F(telemetry.LapLastLapTime, average), "INFO");

                        newAverage(average);
                        break;
                }

                return true;
            };
        }
    }

}

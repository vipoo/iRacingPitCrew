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
using System.Diagnostics;

namespace iRacingPitCrew.DataCollection
{
    public static class OnEachLap
    {
        public static Func<DataSample, bool> Capture(Func<DataSample, LapTransition, bool> next)
        {
            var lastLapNumber = -1;
            var startNewSession = true;
            var onOutLap = false;
            var onFastLap = false;

            return data =>
            {
                var lapTransition = LapTransition.None;

                var telemetry = data.Telemetry;
                var car = telemetry.CamCar;

                var newLapStarted = car.Lap != lastLapNumber;
                lastLapNumber = car.Lap;

                if (car.TrackSurface == TrackLocation.NotInWorld)
                {
                    if (!startNewSession)
                        Trace.WriteLine("Car in NotInWorld");

                    lastLapNumber = -1;
                    startNewSession = true;
                    onOutLap = false;
                    onFastLap = false;
                }
                else if (startNewSession && car.TrackSurface == TrackLocation.OnTrack)
                {
                    Trace.WriteLine("Starting out lap", "INFO");
                    onOutLap = true;
                    startNewSession = false;
                    lapTransition = LapTransition.StartingOutLap;
                }
                else if (onOutLap && newLapStarted)
                {
                    Trace.WriteLine("Starting fast lap", "INFO");

                    onOutLap = false;
                    onFastLap = true;
                    lapTransition = LapTransition.StartFastLap;
                }
                else if (onFastLap && newLapStarted)
                {
                    Trace.WriteLine("Lap Completed", "INFO");

                    lapTransition = LapTransition.CompletedFastLap;
                }
                else
                    return true;

                return next(data, lapTransition);
            };
        }
    }
}

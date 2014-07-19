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
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace iRacingPitCrew
{
    public class DataCollector
    {
        Task process;
        bool requestCancel = false;
        DataSample lastestData;
        iRacingConnection iracing = new iRacingConnection();

        public event Action Connected
        {
            add { iracing.Connected += value;}
            remove { iracing.Connected -= value;}
        }
        
        public event Action Disconnected
        {
            add { iracing.Disconnected += value;}
            remove { iracing.Disconnected -= value;}
        }

        public bool IsConnectedToiRacing
        {
            get
            {
                return lastestData != null && lastestData.IsConnected;
            }
        }

        internal void Start()
        {
            if( process != null)
                return;

            lastestData = null;
            requestCancel = false;
            process = Task.Factory.StartNew(Process);
        }

        internal void Stop()
        {
            var p = process;
            process = null;

            if( p == null )
                return;

            requestCancel = true;
            p.Wait();
        }

        internal DataSample Data
        {
            get
            {
                return lastestData;
            }
        }

        float averageFuelUsage;

        public float AverageFuelUsage { get { return averageFuelUsage; } }

        void Process()
        {
            var last5LapFuelUsage = new List<float>();
            var lastLapNumber = -1;
            var lastFuelAmount = 0f;
            var startNewSession = true;
            var onOutLap = false;
            var onFastLap = false;

            foreach (var data in iracing.GetDataFeed())
            {
                if (requestCancel)
                    return;

                lastestData = data;

                if (!data.IsConnected)
                    continue;

                var telemetry = data.Telemetry;
                var car = telemetry.CamCar;

                if (car.TrackSurface == TrackLocation.NotInWorld)
                {
                    lastLapNumber = -1;
                    startNewSession = true;
                    onOutLap = false;
                    onFastLap = false;
                    lastFuelAmount = 0f;
                    last5LapFuelUsage.Clear();
                    continue;
                }

                var newLapStarted = car.Lap != lastLapNumber;
                lastLapNumber =  car.Lap;

                if( startNewSession && car.TrackSurface == TrackLocation.OnTrack)
                {
                    Trace.WriteLine("Starting out lap", "INFO");
                    onOutLap = true;
                    startNewSession = false;
                    continue;
                }

                if( onOutLap && newLapStarted)
                {
                    Trace.WriteLine("Starting fast lap", "INFO");

                    onOutLap = false;
                    onFastLap = true;
                    lastFuelAmount = telemetry.FuelLevel;
                    continue;
                }

                if( onFastLap && newLapStarted )
                {
                    Trace.WriteLine("Lap Completed", "INFO");

                    var fuelUsed = lastFuelAmount - telemetry.FuelLevel;
                    lastFuelAmount = telemetry.FuelLevel;

                    if (last5LapFuelUsage.Count >= 5)
                        last5LapFuelUsage.RemoveAt(0);

                    last5LapFuelUsage.Add(fuelUsed);

                    averageFuelUsage = last5LapFuelUsage.Average();

                    Trace.WriteLine(string.Format("Finished lap with fuel burn of {0}. Avg: {1}", fuelUsed, averageFuelUsage), "INFO");

                }
            }
        }
    }
}

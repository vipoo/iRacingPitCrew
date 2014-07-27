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

using iRacingPitCrew.DataCollection;
using iRacingSDK;
using iRacingSDK.Support;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace iRacingPitCrew
{
    public class DataCollector
    {
        Task process;
        bool requestCancel = false;
        iRacingConnection iracing = new iRacingConnection();

        public event Action Connected
        {
            add { iracing.Connected += value; }
            remove { iracing.Connected -= value; }
        }

        public event Action Disconnected
        {
            add { iracing.Disconnected += value; }
            remove { iracing.Disconnected -= value; }
        }

        public event Action<DataSample> NewSessionData
        {
            add { iracing.NewSessionData += value; }
            remove { iracing.NewSessionData -= value; }
        }

        CarConfigurations carConfigurations;

        public DataCollector(CarConfigurations carConfigurations)
        {
            this.carConfigurations = carConfigurations;
        }

        public bool IsConnectedToiRacing
        {
            get
            {
                return Data != null && Data.IsConnected;
            }
        }

        internal void Start()
        {
            if (process != null)
                return;

            Data = null;
            requestCancel = false;
            process = Task.Factory.StartNew(Process);
        }

        internal void Stop()
        {
            var p = process;
            process = null;

            if (p == null)
                return;

            requestCancel = true;
            p.Wait();
        }

        public DataSample Data { get; private set; }
        public float AverageFuelPerLap { get;  set; }
        public TimeSpan AverageLapTimeSpan { get; set; }

        public RaceDuration RaceDuration
        {
            set
            {
                value.WriteTo(carConfigurations[CurrentCarName]);
                carConfigurations.HaveChanged(CurrentCarName);
            }
            get
            {
                return RaceDuration.From(carConfigurations[CurrentCarName]);
            }
        }

        private string CurrentCarName
        {
            get { return Data.Telemetry.CamCar.CarPath; }
        }

        public int? TankSize
        {
            set
            {
                carConfigurations[CurrentCarName].TankSize = value;
                carConfigurations.HaveChanged(CurrentCarName);
            }
            get
            {
                return carConfigurations[CurrentCarName].TankSize;
            }
        } 

        void Process()
        {
            try
            {
                var averageFuelPerLap = AverageFuelUsage.Capture(avg => AverageFuelPerLap = avg);
                var onEachLap = OnEachLap.Capture(averageFuelPerLap);

                var averageLapTime = AverageTime.Capture(avg => AverageLapTimeSpan = avg);
                var onEachSessionLap = OnEachSessionLap.Capture(averageLapTime);
                var onEachSession = OnEachSessionUpdate.Capture(onEachSessionLap);

                var data = CaptureLatestData(EmitTo.All(onEachLap, onEachSession));
                var connected = ConnectedDataOnly(data);

                iracing.GetDataFeed().EmitTo(StopOnRequestCancel(connected));
            }
            catch(Exception e)
            {
                Trace.WriteLine("Non Recoverable error in DataCollector. {0}".F(e.Message), "INFO");
                Trace.WriteLine(e.StackTrace, "DEBUG");
            }
        }

        Func<DataSample, bool> ConnectedDataOnly(Func<DataSample, bool> next)
        {
            return data =>
            {
                if (data.IsConnected)
                    return next(data);

                return true;
            };
        }

        Func<DataSample, bool> StopOnRequestCancel(Func<DataSample, bool> next)
        {
            return data => !requestCancel && next(data);
        }

        Func<DataSample, bool> CaptureLatestData(Func<DataSample, bool> next)
        {
            return data => {
                Data = data;

                return next(data);
            };
        }
    }
}

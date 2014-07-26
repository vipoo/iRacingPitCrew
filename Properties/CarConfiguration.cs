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
using iRacingSDK.Support;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRacingSDK;
using iRacingPitCrew.Properties;
using System.Xml.Serialization;

namespace iRacingPitCrew
{
    public enum RaceType { Minutes, Laps};

    public struct RaceDuration
    {
        public readonly int Length;
        public readonly RaceType Type;
        public bool IsEmpty { get { return !isNotEmpty; } }
        readonly bool isNotEmpty;

        public RaceDuration(int length, RaceType type)
        {
            Length = length;
            Type = type;
            isNotEmpty = true;
        }

        public RaceDuration(int length, RaceType type, bool isEmpty)
        {
            Length = length;
            Type = type;
            isNotEmpty = !isEmpty;
        }

        public RaceDuration ForType(RaceType type)
        {
            return new RaceDuration(Length, type);
        }

        public RaceDuration ForLength(int length)
        {
            return new RaceDuration(length, Type);
        }

        public RaceDuration ForLength(int? length)
        {
            return length == null ? new RaceDuration() : new RaceDuration((int)length, Type);
        }

        public TimeSpan TotalMinutes { get { return Length.Minutes(); } }

        internal void WriteTo(CarConfiguration config)
        {
            config.RaceDuration_IsEmpty = IsEmpty;
            config.RaceDuration_Length = Length;
            config.RaceDuration_Type = Type;
        }

        internal static RaceDuration From(CarConfiguration config)
        {
            return new RaceDuration(config.RaceDuration_Length, config.RaceDuration_Type, config.RaceDuration_IsEmpty);
        }
    }

    public class CarConfiguration
    {
        public string CarName { get; set; }
        public int? TankSize { get; set; }
        public int RaceDuration_Length { get; set; }
        public RaceType RaceDuration_Type { get; set; }
        public bool RaceDuration_IsEmpty { get; set; }
        public float? FuelBurn { get; set; }

        public CarConfiguration()
        {
            RaceDuration_IsEmpty = true;
        }
    }

    public class CarConfigurations : List<CarConfiguration>
    {
        CrossThreadEvents<string> changed = new CrossThreadEvents<string>();

        internal event Action<string> Changed
        {
            add { changed.Event += value; }
            remove { changed.Event -= value; }
        }

        internal void HaveChanged(string carName)
        {
            Settings.Default.Save();
            changed.Invoke(carName);
        }

        public CarConfiguration this[string carName]
        {
            get
            {
                return this.FirstOrDefault(c => c.CarName == carName);
            }
        }

        public bool ContainsKey(string carName)
        {
            return this.Any(c => c.CarName == carName);
        }
    }
}

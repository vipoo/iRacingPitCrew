using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public RaceDuration ForType(RaceType type)
        {
            return new RaceDuration(Length, type);
        }

        public RaceDuration ForLength(int length)
        {
            return new RaceDuration(length, Type);
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
    }

    public class CarConfigurations : List<CarConfiguration>
    {
    }
}

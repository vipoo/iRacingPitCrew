using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRacingPitCrew
{
    public enum RaceType { Minutes, Laps};

    public class CarConfiguration
    {
        public string CarName { get; set; }
        public int? TankSize { get; set; }
        public int? RaceLength { get; set; }
        public float? FuelBurn { get; set; }
        public RaceType? RaceDuationType { get; set; }
    }

    public class CarConfigurations : List<CarConfiguration>
    {
    }
}

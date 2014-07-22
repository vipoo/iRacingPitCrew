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

using iRacingSDK.Support;
using System;
using System.Diagnostics;
namespace iRacingPitCrew.PitCrewCommands
{
    public static class FuelStrategy
    {
        public static FuelStrategyOption Calculate(int numberOfRaceLaps, double averageFuelBurnPerLap, double fuelTankCapacity)
        {
            var totalFuelRequired = GetTotalFuelRequired(numberOfRaceLaps, averageFuelBurnPerLap);

            var numberOfPitStops =(int)( totalFuelRequired / fuelTankCapacity);

            return new FuelStrategyOption(numberOfRaceLaps, averageFuelBurnPerLap, fuelTankCapacity, totalFuelRequired, numberOfPitStops);
        }

        public static RaceDurationFuelStrategyOption Calculate(TimeSpan raceDuration, float averageFuelBurnPerLap, TimeSpan averageLapTime)
        {
            var estimatedNumberOfRaceLaps =(int)( (raceDuration.TotalSeconds / averageLapTime.TotalSeconds) + 1);

            var totalFuelRequired = GetTotalFuelRequired(estimatedNumberOfRaceLaps, averageFuelBurnPerLap);

            return new RaceDurationFuelStrategyOption(raceDuration, averageFuelBurnPerLap, averageLapTime, estimatedNumberOfRaceLaps, totalFuelRequired);
        }

        static int GetTotalFuelRequired(int numberOfRaceLaps, double averageFuelBurnPerLap)
        {
            var totalFuelRequired = (int)Math.Ceiling((numberOfRaceLaps + 1) * averageFuelBurnPerLap);

            return totalFuelRequired = ((totalFuelRequired / 5) + 1) * 5;
        }
    }
}

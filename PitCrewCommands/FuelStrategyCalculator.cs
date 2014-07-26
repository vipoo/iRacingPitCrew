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
        public static FuelStrategyOption Calculate(int numberOfRaceLaps, double averageFuelBurnPerLap, int fuelTankCapacity)
        {
            var twoLapsOfFuel = averageFuelBurnPerLap * 2;

            var totalFuelRequired = GetTotalFuelRequired(numberOfRaceLaps, averageFuelBurnPerLap);

            var numberOfPitStops = GetTotalPitStops(fuelTankCapacity, totalFuelRequired, averageFuelBurnPerLap);

            return new FuelStrategyOption(numberOfRaceLaps, averageFuelBurnPerLap, fuelTankCapacity, totalFuelRequired, numberOfPitStops);
        }
            
        public static RaceDurationFuelStrategyOption Calculate(TimeSpan raceDuration, float averageFuelBurnPerLap, TimeSpan averageLapTime, int fuelTankCapacity)
        {
            var estimatedNumberOfRaceLaps =(int)( (raceDuration.TotalSeconds / averageLapTime.TotalSeconds) + 1);

            var totalFuelRequired = GetTotalFuelRequired(estimatedNumberOfRaceLaps, averageFuelBurnPerLap);

            var numberOfPitStops = GetTotalPitStops(fuelTankCapacity, totalFuelRequired, averageFuelBurnPerLap);

            return new RaceDurationFuelStrategyOption(raceDuration, averageFuelBurnPerLap, averageLapTime, estimatedNumberOfRaceLaps, totalFuelRequired, numberOfPitStops);
        }

        public static RaceCompletionRequirements CalculateToFinish(float fuelLevel, int remainingLaps, TimeSpan raceDuration, float averageFuelBurnPerLap, TimeSpan averageLapTime, int fuelTankCapacity)
        {
            var totalFuelRequired = GetTotalFuelRequired(remainingLaps, averageFuelBurnPerLap, fuelLevel);

            int numberOfPitStops = 0;
            int fuelAtNextStop;
            bool pitWindowOpened;
            float fuelToBurnUntilPitWindow;
            int lapsToPitWindow;

            if (totalFuelRequired > 0)
                numberOfPitStops = GetTotalPitStops(fuelTankCapacity, totalFuelRequired + (int)fuelLevel, averageFuelBurnPerLap, (int)fuelLevel);

            if (numberOfPitStops == 0)
            {
                fuelAtNextStop = 0;
                pitWindowOpened = false;
                fuelToBurnUntilPitWindow = 0;
                lapsToPitWindow = 0;
            }
            else if (numberOfPitStops == 1)
            {
                fuelAtNextStop = totalFuelRequired;

                pitWindowOpened = fuelLevel + fuelAtNextStop <= fuelTankCapacity;

                fuelToBurnUntilPitWindow = fuelLevel + fuelAtNextStop - fuelTankCapacity;

                lapsToPitWindow = Math.Max(0, (int)(fuelToBurnUntilPitWindow / averageFuelBurnPerLap));
            }
            else
            {
                fuelAtNextStop = fuelTankCapacity;

                pitWindowOpened = fuelLevel <= 2 * averageFuelBurnPerLap;

                fuelToBurnUntilPitWindow = fuelLevel - (2 * averageFuelBurnPerLap);

                lapsToPitWindow = Math.Max(0, (int)(fuelToBurnUntilPitWindow / averageFuelBurnPerLap));
            }

            return new RaceCompletionRequirements(numberOfPitStops, totalFuelRequired, fuelAtNextStop, pitWindowOpened, lapsToPitWindow, remainingLaps);
        }
        
        public static RaceCompletionRequirements CalculateToFinish(float fuelLevel, TimeSpan remainingTime, TimeSpan raceDuration, float averageFuelBurnPerLap, TimeSpan averageLapTime, int fuelTankCapacity)
        {
            var estimateLapsRemaning = (int)((remainingTime.TotalSeconds / averageLapTime.TotalSeconds) + 1);

            return CalculateToFinish(fuelLevel, estimateLapsRemaning, raceDuration, averageFuelBurnPerLap, averageLapTime, fuelTankCapacity);
        }

        static int GetTotalFuelRequired(int numberOfRaceLaps, double averageFuelBurnPerLap, float existingFuel = 0f)
        {
            var totalFuelRequired = (int)(Math.Ceiling((numberOfRaceLaps + 1) * averageFuelBurnPerLap) - existingFuel);

            return totalFuelRequired = ((totalFuelRequired / 5) + 1) * 5;
        }

        static int GetTotalPitStops(int fuelTankCapacity, int totalFuelRequired, double averageFuelBurnPerLap, int fuelLevel = -1)
        {
            if (fuelLevel == -1)
                fuelLevel = fuelTankCapacity;

            var twoLapsOfFuel = averageFuelBurnPerLap * 2;

            var fuelRefill = fuelTankCapacity - twoLapsOfFuel;

            var refillRequired = (double)totalFuelRequired - fuelLevel;

            return (int)Math.Ceiling(refillRequired / fuelRefill);
        }
    }
}

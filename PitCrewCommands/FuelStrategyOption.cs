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

namespace iRacingPitCrew.PitCrewCommands
{
    public struct SessionStatuseResults
    {
        public int EstimateLapsRemaining;
        public TimeSpan EstimateTimeRemaining;

        public SessionStatuseResults(int estimateLapsRemaining, TimeSpan estimateTimeRemaining )
        {
            EstimateLapsRemaining = estimateLapsRemaining;
            EstimateTimeRemaining = estimateTimeRemaining;
        }
    }
    
    public struct RaceCompletionRequirements
    {
        public readonly int NumberOfPitStops;
        public readonly int TotalFuelRequired;
        public readonly int TotalFuelRequiredAtNextStop;
        public bool PitWindowOpened;
        public int LapsToPitWindow;
        public int EstimateLapsRemaining;

        public RaceCompletionRequirements(int numberOfPitStops, int totalFuelRequired, int totalFuelRequiredAtNextStop, bool pitWindowOpened, int lapsToPitWindow, int estimateLapsRemaining)
        {
            NumberOfPitStops = numberOfPitStops;
            TotalFuelRequired = totalFuelRequired;
            TotalFuelRequiredAtNextStop = totalFuelRequiredAtNextStop;
            PitWindowOpened = pitWindowOpened;
            LapsToPitWindow = lapsToPitWindow;
            EstimateLapsRemaining = estimateLapsRemaining;
        }

        public override string ToString()
        {
            return "numberOfPitStops: {0}, totalFuelRequired: {1}, totalFuelRequiredAtNextStop: {2}, pitWindowOpened: {3}, lapsToPitWindow: {4}, estimateLapsRemaining: {5}".F(
                NumberOfPitStops, TotalFuelRequired, TotalFuelRequiredAtNextStop, PitWindowOpened, LapsToPitWindow, EstimateLapsRemaining);
        }
    }

    public struct FuelStrategyOption
    {
        public readonly int NumberOfRaceLaps;
        public readonly double AverageFuelBurnPerLap;
        public readonly double FuelTankCapacity;
        public readonly int TotalFuelRequired;
        public readonly int NumberOfPitStops;

        public FuelStrategyOption(int numberOfRaceLaps, double averageFuelBurnPerLap, double fuelTankCapacity, int totalFuelRequired, int numberOfPitStops)
        {
            NumberOfRaceLaps = numberOfRaceLaps;
            AverageFuelBurnPerLap = averageFuelBurnPerLap;
            TotalFuelRequired = totalFuelRequired;
            FuelTankCapacity = fuelTankCapacity;
            NumberOfPitStops = numberOfPitStops;
        }
    }

    public struct RaceDurationFuelStrategyOption
    {
        public readonly TimeSpan RaceDuration;
        public readonly double AverageFuelBurnPerLap;
        public readonly TimeSpan AverageLapTime;
        public readonly int EstimatedNumberOfRaceLaps;
        public readonly int TotalFuelRequired;
        public readonly int NumberOfPitStops;

        public RaceDurationFuelStrategyOption(System.TimeSpan raceDuration, float averageFuelBurnPerLap, System.TimeSpan averageLapTime, int estimatedNumberOfRaceLaps, int totalFuelRequired, int numberOfPitStops)
        {
            this.RaceDuration = raceDuration;
            this.AverageFuelBurnPerLap = averageFuelBurnPerLap;
            this.AverageLapTime = averageLapTime;
            this.EstimatedNumberOfRaceLaps = estimatedNumberOfRaceLaps;
            this.TotalFuelRequired = totalFuelRequired;
            this.NumberOfPitStops = numberOfPitStops;
        }
    }
}

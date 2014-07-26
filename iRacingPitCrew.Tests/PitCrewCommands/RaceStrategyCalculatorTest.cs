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
using iRacingPitCrew.PitCrewCommands;
using NUnit.Framework;
using System;

namespace iRacingPitCrew.Tests.PitCrewCommands
{
    [TestFixture]
    public class RaceStrategyCalculatorTest
    {
        [Test]
        public void ShouldCalculateForRaceRemaining()
        {
            var r = FuelStrategy.CalculateToFinish(
                fuelLevel: 10,
                remainingTime: 30.Minutes(),
                raceDuration: 60.Minutes(),
                averageFuelBurnPerLap: 1f,
                averageLapTime: 60.Seconds(),
                fuelTankCapacity: 30
            );

            Assert.That(r, Is.EqualTo(new RaceCompletionRequirements(
                numberOfPitStops: 1,
                totalFuelRequired: 25,
                totalFuelRequiredAtNextStop: 25,
                pitWindowOpened: false,
                lapsToPitWindow: 5,
                estimateLapsRemaining: 31))
            );
        }

        [Test]
        public void ShouldCalculateForRaceRemaining2()
        {
            var r = FuelStrategy.CalculateToFinish(
                fuelLevel: 29,
                remainingTime: 30.Minutes(),
                raceDuration: 60.Minutes(),
                averageFuelBurnPerLap: 1f,
                averageLapTime: 60.Seconds(),
                fuelTankCapacity: 30
            );

            Assert.That(r, Is.EqualTo(new RaceCompletionRequirements(
                numberOfPitStops: 1,
                totalFuelRequired: 5,
                totalFuelRequiredAtNextStop: 5,
                pitWindowOpened: false,
                lapsToPitWindow: 4,
                estimateLapsRemaining: 31))
            );
        }

        [Test]
        public void ShouldCalculateForRaceRemaining_WhenNoPitStopRequired()
        {
            var r = FuelStrategy.CalculateToFinish(
                fuelLevel: 20,
                remainingTime: 10.Minutes(),
                raceDuration: 60.Minutes(),
                averageFuelBurnPerLap: 1f,
                averageLapTime: 60.Seconds(),
                fuelTankCapacity: 30
            );

            Assert.That(r, Is.EqualTo(new RaceCompletionRequirements(
                numberOfPitStops: 0,
                totalFuelRequired: 0,
                totalFuelRequiredAtNextStop: 0,
                pitWindowOpened: false,
                lapsToPitWindow: 0,
                estimateLapsRemaining: 11))
            );
        }

        [Test]
        public void ShouldCalculateForRaceRemaining_WhenPitWindowOpen()
        {
            var r = FuelStrategy.CalculateToFinish(
                fuelLevel: 5,
                remainingTime: 24.Minutes(),
                raceDuration: 60.Minutes(),
                averageFuelBurnPerLap: 1f,
                averageLapTime: 60.Seconds(),
                fuelTankCapacity: 30
            );

            Assert.That(r, Is.EqualTo(new RaceCompletionRequirements(
                numberOfPitStops: 1,
                totalFuelRequired: 25,
                totalFuelRequiredAtNextStop: 25,
                pitWindowOpened: true,
                lapsToPitWindow: 0,
                estimateLapsRemaining: 25))
            );
        }

        [Test]
        public void ShouldCalculateForRaceRemaining_WhenTwoStopRequired()
        {
            var r = FuelStrategy.CalculateToFinish(
                fuelLevel: 15,
                remainingTime: 90.Minutes(),
                raceDuration: 120.Minutes(),
                averageFuelBurnPerLap: 1f,
                averageLapTime: 60.Seconds(),
                fuelTankCapacity: 30
            );

            Assert.That(r, Is.EqualTo(new RaceCompletionRequirements(
                numberOfPitStops: 3,
                totalFuelRequired: 90-15 + 5,
                totalFuelRequiredAtNextStop: 30,
                pitWindowOpened: false,
                lapsToPitWindow: 13,
                estimateLapsRemaining: 91))
            );
        }
    }
}

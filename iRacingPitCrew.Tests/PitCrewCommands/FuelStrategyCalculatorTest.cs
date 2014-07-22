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

namespace iRacingPitCrew.Tests.PitCrewCommands
{
    [TestFixture]
    public class FuelStrategyCalculatorTest
    {
        [Test]
        public void ShouldCalculateTotalFuelRequiredFor20Laps()
        {
            var r = FuelStrategy.Calculate(numberOfRaceLaps: 20, averageFuelBurnPerLap: 1.9d, fuelTankCapacity: 0);
            Assert.That(r.TotalFuelRequired, Is.EqualTo(45)); //( 20+1)*1.9 => 39.9 => 40 => then to nearest 5

            r = FuelStrategy.Calculate(numberOfRaceLaps: 20, averageFuelBurnPerLap: 2.4d, fuelTankCapacity: 0);
            Assert.That(r.TotalFuelRequired, Is.EqualTo(55)); //( 20+1)*2.4 => 50.4 => 51 then to nearest 5

            r = FuelStrategy.Calculate(numberOfRaceLaps: 20, averageFuelBurnPerLap: 2.380952380952381d, fuelTankCapacity: 0);
            Assert.That(r.TotalFuelRequired, Is.EqualTo(55)); //( 20+1)*2.38...  => 50.0 then to nearest 5

            r = FuelStrategy.Calculate(numberOfRaceLaps: 20, averageFuelBurnPerLap: 2.333333333333333, fuelTankCapacity: 0);
            Assert.That(r.TotalFuelRequired, Is.EqualTo(50));

            r = FuelStrategy.Calculate(numberOfRaceLaps: 20, averageFuelBurnPerLap: 2.333333333333334, fuelTankCapacity: 0);
            Assert.That(r.TotalFuelRequired, Is.EqualTo(55));

            r = FuelStrategy.Calculate(numberOfRaceLaps: 20, averageFuelBurnPerLap: 2.380952380952381d, fuelTankCapacity: 0);
            Assert.That(r.TotalFuelRequired, Is.EqualTo(55));
        }

        [Test]
        public void ShouldCalculateNumerOfPitsStopsForA20LapRace()
        {
            var r = FuelStrategy.Calculate(numberOfRaceLaps: 20, averageFuelBurnPerLap: 2d, fuelTankCapacity: 30d);
            Assert.That(r.NumberOfPitStops, Is.EqualTo(1)); 

        }

        [Test]
        public void ShouldCalculateEstimateNumberOfRaceLapsFor20Minutes()
        {
            var r = FuelStrategy.Calculate(raceDuration: 20.Minutes(), averageFuelBurnPerLap: 0f, averageLapTime: 60.Seconds());
            Assert.That(r.EstimatedNumberOfRaceLaps, Is.EqualTo(21));

            r = FuelStrategy.Calculate(raceDuration: 20.Minutes(), averageFuelBurnPerLap: 0f, averageLapTime: 55.Seconds());
            Assert.That(r.EstimatedNumberOfRaceLaps, Is.EqualTo(22));
        }

        [Test]
        public void ShouldCalculateTotalFuelRequiredFor20Minutes()
        {
            var r = FuelStrategy.Calculate(raceDuration: 20.Minutes(), averageFuelBurnPerLap: 1.9f, averageLapTime: 60.Seconds());

            Assert.That(r.TotalFuelRequired, Is.EqualTo(45)); //( 21+1)*1.9 =>39 then to nearest 5
        }
    }
}

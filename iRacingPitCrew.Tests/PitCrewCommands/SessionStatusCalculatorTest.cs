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
    public class SessionStatusCalculatorTest
    {
        [Test]
        public void ShouldCalculateRemainingFor20Litres()
        {
            var r = FuelStrategy.CalculateRemainingLapsForFuel(20, 2f, 60.Seconds());
            Assert.That(r.EstimateLapsRemaining, Is.EqualTo(10));
            Assert.That(r.EstimateTimeRemaining, Is.EqualTo(600.Seconds()));

            r = FuelStrategy.CalculateRemainingLapsForFuel(20, 1.35f, 60.Seconds());
            Assert.That(r.EstimateLapsRemaining, Is.EqualTo(14));
            Assert.That(r.EstimateTimeRemaining, Is.EqualTo((14*60).Seconds()));

        }
    }
}

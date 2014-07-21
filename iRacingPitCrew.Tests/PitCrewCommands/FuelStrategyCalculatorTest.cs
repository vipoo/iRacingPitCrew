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


using iRacingPitCrew.PitCrewCommands;
using NUnit.Framework;

namespace iRacingPitCrew.Tests.PitCrewCommands
{
    [TestFixture]
    public class FuelStrategyCalculatorTest
    {
        [Test]
        public void ShouldCalculatorTotalFuel()
        {
            var c = new FuelStrategyCalculator();

            var r = c.Calculator(20, 1.9d);

            Assert.That(r.TotalFuelRequired, Is.EqualTo(40)); //( 20+1)*1.9 =>39 then to nearest 5
        }


    }
}

﻿// This file is part of iRacingPitCrew Application.
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

using iRacingSDK;
using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace iRacingPitCrew.PitCrewCommands
{
    public class SetFuelCommand : PitCrewCommand
    {
        public SetFuelCommand(SpeechRecognitionEngine recognizer, SpeechSynthesizer synthesizer, Action recognized)
            : base(recognizer, synthesizer, recognized)
        {
            SetGrammar(g =>
            {
                g.Append("set fuel");
                g.Append(new SemanticResultKey("fuel_amount", FuelNumbers()));
                g.Append(new Choices("litres", "liters", "litre", "liter"));
            });
        }

        Choices FuelNumbers()
        {
            var digits = new Choices();

            for (int i = 5; i < 201; i += 5)
                digits.Add(new SemanticResultValue(i.ToString(), i));

            return digits;
        }

        protected override void Command(RecognitionResult rr)
        {
            var a = (int)rr.Semantics["fuel_amount"].Value;

            if (a == 0)
            {
                iRacing.PitCommand.SetFuel(1);
                SpeakAsync(string.Format("No fuel at your next pit stop.", a));
            }
            else
            {
                iRacing.PitCommand.SetFuel((int)a);
                SpeakAsync(string.Format("You will get {0} litres of fuel at next pit stop.", a));
            }
        }
    }
}

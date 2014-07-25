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

using iRacingPitCrew.Support;
using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace iRacingPitCrew.PitCrewCommands
{
    public class HelpCommand : PitCrewCommand
    {
        public HelpCommand(SpeechRecognitionEngine recognizer, SpeechSynthesizer synthesizer, Action recognized)
            : base(recognizer, synthesizer, recognized)
        {
            SetGrammar("help");
        }

        public const string HelpText = "Your pit crew commands are as follows.  Reset. Fuel Strategy.  Race Status.  No Tyre Change.  Change All Tyres.  Set Fuel 'amount'";

        protected override void Command(RecognitionResult rr)
        {
            SpeakAsync(HelpText);
        }
    }
}

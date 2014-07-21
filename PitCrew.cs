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
using iRacingPitCrew.Support;
using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace iRacingPitCrew
{
    public class PitCrew : PitCrewCommand
    {
        public const string HelpText = "Your pit crew commands are as follows.  Reset. Fuel Strategy.  Race Status.  No Tyre Change.  Change All Tyres.  Set Fuel 'amount'";

        readonly List<PitCrewCommand> pitCrewCommands = new List<PitCrewCommand>();

        public PitCrew(DataCollector dataCollector) : base(new SpeechRecognitionEngine(), dataCollector, new SpeechSynthesizer())
        {
            LogToVoiceListener.ToSpeech(synthesizer);

            recognizer.UpdateRecognizerSetting("CFGConfidenceRejectionThreshold", 90);

            this.dataCollector.Connected += dataCollector_Connected;
            this.dataCollector.Disconnected += dataCollector_Disconnected;

            synthesizer.Volume = 100;
            synthesizer.Rate = -2;

            pitCrewCommands.Add(new FuelStrategyCommand(recognizer, dataCollector, synthesizer));
            pitCrewCommands.Add(new SetFuelCommand(recognizer, dataCollector, synthesizer));
        }

        void dataCollector_Disconnected()
        {
            synthesizer.SpeakAsync("Disconnected from I Racing, your pit crew have gone away.");
        }

        void dataCollector_Connected()
        {
            synthesizer.SpeakAsync("Connected to I Racing, your pit crew is standing by.");
        }

        public override void Start()
        {
            recognizer.SetInputToDefaultAudioDevice();

            foreach (var pcc in pitCrewCommands)
                pcc.Start();

            recognizer.LoadGrammar(ProcessCommand, Debugging, g =>
            {
                g.Append("pit crew debug");
                g.Append(new Choices(new GrammarBuilder("on"), new GrammarBuilder("off")));
            });

            recognizer.LoadGrammar(ProcessCommand, HelpCommands, "pit crew help");
            recognizer.LoadGrammar(ProcessCommand, Shutup, "pit crew quite");
            recognizer.LoadGrammar(ProcessCommand, Shutup, "pit crew shut up");
            recognizer.LoadGrammar(ProcessCommand, Shutup, "pit crew shutup");
            recognizer.LoadGrammar(ProcessPitCommand, ResetPitStop, "pit crew reset");
            recognizer.LoadGrammar(ProcessPitCommand, RaceStatus, "pit crew race status");
            recognizer.LoadGrammar(ProcessPitCommand, TyreOff, "pit crew no tyre change");
            recognizer.LoadGrammar(ProcessPitCommand, TyreOff, "pit crew tyre change off");
            recognizer.LoadGrammar(ProcessPitCommand, TyreOn, "pit crew change all tyres");
            recognizer.LoadGrammar(ProcessPitCommand, TyreOn, "pit crew tyre change on");
            
            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            dataCollector.Start();
        }


        private void Debugging(RecognitionResult rr)
        {
            if( rr.Text == "pit crew debug on")
                LogToVoiceListener.Enable();
            else
                LogToVoiceListener.Disable();
        }

        private void Shutup(RecognitionResult obj)
        {
            synthesizer.SpeakAsyncCancelAll();
        }

        private void HelpCommands(RecognitionResult obj)
        {
            synthesizer.SpeakAsync(HelpText);
        }

        internal void Stop()
        {
            dataCollector.Stop();
        }

        void ResetPitStop(RecognitionResult rr)
        {
            iRacingSDK.iRacing.PitCommand.Clear();
            synthesizer.SpeakAsync("No tyres fuel or windscreen cleaning at your next pit stop.");
        }

        void RaceStatus(RecognitionResult rr)
        {
            var d = dataCollector.Data;
            if( d == null)
            {
                synthesizer.SpeakAsync("Yet to received any data from iRacing");
                return;
            }

            var session = d.SessionData.SessionInfo.Sessions[d.Telemetry.SessionNum];

            if (!session.IsRace )
            {
                synthesizer.SpeakAsync("You are not in a race");
                return;
            }

            if (session.IsLimitedTime)
            {
                var sessionTimeSpanRemaining = TimeSpan.FromSeconds(d.Telemetry.SessionTimeRemain);

                synthesizer.SpeakAsync(string.Format("There are {0} minutes remaining in this race.", (int)sessionTimeSpanRemaining.TotalMinutes));
            }
            else if( session.IsLimitedSessionLaps)
            {
                synthesizer.SpeakAsync(string.Format("There are {0} laps remaining in this race.", d.Telemetry.SessionLapsRemain));
            }

            synthesizer.SpeakAsync(string.Format("You {0} litres of fuel.", (int)d.Telemetry.FuelLevel));
            synthesizer.SpeakAsync(string.Format("You are on lap {0}.", (int)d.Telemetry.FuelLevel));
        }

        void TyreOff(RecognitionResult rr)
        {
            synthesizer.SpeakAsync("Will not be changing tyres at next pit stop.");
            iRacingSDK.iRacing.PitCommand.ClearTireChange();
        }

        void TyreOn(RecognitionResult rr)
        {
            synthesizer.SpeakAsync("We will change your tyes at next pit stop.");
            iRacingSDK.iRacing.PitCommand.ChangeAllTyres();
        }
    }
}

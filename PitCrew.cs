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
using iRacingPitCrew.Properties;
using iRacingPitCrew.Support;
using System;
using System.Collections.Generic;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;

namespace iRacingPitCrew
{
    public class PitCrew
    {
        readonly List<PitCrewCommand> pitCrewCommands = new List<PitCrewCommand>();
        readonly SpeechSynthesizer synthesizer;
        readonly DataCollector dataCollector;

        SpeechRecognitionEngine recognizer;
        DictationGrammar dicatationGrammar;
        Grammar pitCrewGrammar;
        Timer reenablePitCrewGrammarTimer;
        private System.Globalization.CultureInfo cultureInfo;

        public PitCrew(DataCollector dataCollector, System.Globalization.CultureInfo cultureInfo)
        {
            this.dataCollector = dataCollector;
            synthesizer = new SpeechSynthesizer();
            LogToVoiceListener.ToSpeech(synthesizer);

            this.dataCollector.Connected += dataCollector_Connected;
            this.dataCollector.Disconnected += dataCollector_Disconnected;

            BuildSpeechRecognitionEngine(cultureInfo);
        }

        public void Start()
        {
            synthesizer.Volume = 100;
            
            dataCollector.Start();
        }

        void dataCollector_Disconnected()
        {
            synthesizer.SpeakAsync("Disconnected from I Racing, your pit crew have gone away.");
        }

        void dataCollector_Connected()
        {
            synthesizer.SpeakAsync("Connected to I Racing, your pit crew is standing by.");
        }

        void PitCrewCommand(RecognitionResult obj)
        {
            pitCrewGrammar.Enabled = dicatationGrammar.Enabled = false;

            synthesizer.SpeakAsync("copy");

            foreach (var pcc in pitCrewCommands)
                pcc.Enable = true;

            reenablePitCrewGrammarTimer = new Timer(x => CommandRecognized(), null, 15000, Timeout.Infinite);
        }

        void CommandRecognized()
        {
            pitCrewGrammar.Enabled = dicatationGrammar.Enabled = true;

            foreach (var pcc in pitCrewCommands)
                pcc.Enable = false;

            if (reenablePitCrewGrammarTimer != null)
            {
                reenablePitCrewGrammarTimer.Dispose();
                reenablePitCrewGrammarTimer = null;
            }
        }

        void Shutup(RecognitionResult rr)
        {
            synthesizer.SpeakAsyncCancelAll();
        }

        public void Stop()
        {
            dataCollector.Stop();
        }

        void BuildSpeechRecognitionEngine(System.Globalization.CultureInfo cultureInfo)
        {
            this.cultureInfo = cultureInfo;

            if (recognizer != null)
                recognizer.Dispose();

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            recognizer = new SpeechRecognitionEngine(cultureInfo);

            recognizer.UpdateRecognizerSetting("CFGConfidenceRejectionThreshold", 90);
            recognizer.EndSilenceTimeout = TimeSpan.FromMilliseconds(100);
            recognizer.InitialSilenceTimeout = TimeSpan.FromMilliseconds(1000);

            pitCrewCommands.Add(new FuelStrategyCommand(recognizer, synthesizer, CommandRecognized, dataCollector));
            pitCrewCommands.Add(new RaceStatusCommand(recognizer, synthesizer, CommandRecognized, dataCollector));
            pitCrewCommands.Add(new SetFuelCommand(recognizer, synthesizer, CommandRecognized));
            pitCrewCommands.Add(new ResetCommand(recognizer, synthesizer, CommandRecognized));
            pitCrewCommands.Add(new DebuggingCommand(recognizer, synthesizer, CommandRecognized));
            pitCrewCommands.Add(new HelpCommand(recognizer, synthesizer, CommandRecognized));
            pitCrewCommands.Add(new TyreOffCommand(recognizer, synthesizer, CommandRecognized));
            pitCrewCommands.Add(new TyreOnCommand(recognizer, synthesizer, CommandRecognized));
            pitCrewCommands.Add(new CancelCommand(recognizer, synthesizer, CommandRecognized));
            pitCrewCommands.Add(new SessionCommand(recognizer, synthesizer, CommandRecognized, dataCollector));

            if( Settings.Default.ActivationFilter)
                recognizer.LoadGrammar(dicatationGrammar = new DictationGrammar("grammar:dictation#pronunciation"));

            pitCrewGrammar = recognizer.LoadGrammar(PitCrewCommand, "pit crew");
            recognizer.LoadGrammar(Shutup, g => { g.Append("pit crew"); g.Append(new Choices("quite", "shut up", "shutup")); });

            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }

        internal void ChangeCulture(System.Globalization.CultureInfo cultureInfo)
        {
            BuildSpeechRecognitionEngine(cultureInfo);
        }

        internal void ChangeActivationFilter()
        {
            BuildSpeechRecognitionEngine(cultureInfo);
        }
    }
}

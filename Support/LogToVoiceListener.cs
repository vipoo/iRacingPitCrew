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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Speech.Synthesis;

namespace iRacingPitCrew.Support
{
    public class LogToVoiceListener : TraceListener
    {
        static LogToVoiceListener logger;
        readonly SpeechSynthesizer synthesizer;
        public string FileName { get; internal set; }
        static bool enabled = false;

        LogToVoiceListener(SpeechSynthesizer synthesizer)
        {
            this.synthesizer= synthesizer;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }

        public override void WriteLine(string message, string category)
        {
            Write(message, category);
        }

        public override void Write(string message, string category)
        {
            base.Write(message, category);

            if (category == "INFO" && enabled)
                synthesizer.SpeakAsync(message);
        }

        internal static void ToSpeech(SpeechSynthesizer synthesizer)
        {
            logger = new LogToVoiceListener(synthesizer);
            Trace.Listeners.Add(logger);
        }

        internal static void Enable()
        {
            enabled = true;
            Trace.WriteLine("Logging turned on.", "INFO");
        }

        internal static void Disable()
        {
            Trace.WriteLine("Logging turned off.", "INFO");
            enabled = false;
        }
    }
}

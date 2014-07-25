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
using System.Speech.Recognition;

namespace iRacingPitCrew.Support
{
    public static class Recognition
    {
        public static bool ProcessCommand(RecognitionResult rr)
        {
            Trace.WriteLine("---------------------", "DEBUG");
            foreach (var alt in rr.Alternates)
                Trace.WriteLine(string.Format("{0:00.00}%, {1}", alt.Confidence * 100f, alt.Text), "DEBUG");

            if (rr.Confidence < 0.9)
            {
                Trace.WriteLine("Ignoring poor match", "INFO");
                Trace.WriteLine("---------------------", "DEBUG");
                return false;
            }

            Trace.WriteLine("---------------------", "DEBUG");
            return true;
        }

        public static Grammar LoadGrammar(this SpeechRecognitionEngine self, Action<RecognitionResult> speechReconized, string phrase)
        {
            var g = new Grammar(new GrammarBuilder(phrase));
            g.SpeechRecognized += (s, e) => SpeechReconized(self, speechReconized, e);

            self.LoadGrammar(g);

            return g;
        }

        public static Grammar LoadGrammar(this SpeechRecognitionEngine self, Action<RecognitionResult> speechReconized, Action<GrammarBuilder> builder)
        {
            var gb = new GrammarBuilder();
            builder(gb);

            var g = new Grammar(gb);
            g.SpeechRecognized += (s, e) => SpeechReconized(self, speechReconized, e);

            self.LoadGrammar(g);

            return g;
        }

        static void SpeechReconized(SpeechRecognitionEngine self, Action<RecognitionResult> speechReconized, SpeechRecognizedEventArgs e)
        {
            try
            {
                if( ProcessCommand(e.Result))
                    speechReconized(e.Result);
            }
            catch(Exception ex)
            {
                Trace.WriteLine("Error in speech recognition handler", "DEBUG");
                Trace.WriteLine(ex.Message, "DEBUG");
                Trace.WriteLine(ex.StackTrace, "DEBUG");
            }
        }
    }
}

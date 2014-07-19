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
    public static class SpeechRecognitionEngineExtension
    {
        public static void LoadGrammar(this SpeechRecognitionEngine self, Action<RecognitionResult> speechReconized, string phrase)
        {
            self.LoadGrammar(r => true, speechReconized, phrase);
        }

        public static void LoadGrammar(this SpeechRecognitionEngine self, Func<RecognitionResult, bool> reconizerGuard, Action<RecognitionResult> speechReconized, string phrase)
        {
            var g = new Grammar(new GrammarBuilder(phrase));
            g.SpeechRecognized += (s, e) => SpeechReconized(reconizerGuard, speechReconized, e);

            self.LoadGrammar(g);
        }

        public static void LoadGrammar(this SpeechRecognitionEngine self, Action<RecognitionResult> speechReconized, Action<GrammarBuilder> builder)
        {
            self.LoadGrammar(r => true, speechReconized, builder);
        }

        public static void LoadGrammar(this SpeechRecognitionEngine self, Func<RecognitionResult, bool> reconizerGuard, Action<RecognitionResult> speechReconized, Action<GrammarBuilder> builder)
        {
            var gb = new GrammarBuilder();
            builder(gb);

            var g = new Grammar(gb);
            g.SpeechRecognized += (s, e) => SpeechReconized(reconizerGuard, speechReconized, e);

            self.LoadGrammar(g);
        }

        static void SpeechReconized(Func<RecognitionResult, bool> reconizerGuard, Action<RecognitionResult> speechReconized, SpeechRecognizedEventArgs e)
        {
            try
            {
                if( reconizerGuard(e.Result))
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

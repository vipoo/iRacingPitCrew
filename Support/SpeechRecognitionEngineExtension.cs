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
using System.Collections.Generic;
using System.Diagnostics;
using System.Speech.Recognition;
using System.Linq;

namespace iRacingPitCrew.Support
{
    public static class SpeechRecognitionEngineExtension
    {
        static Dictionary<WeakReference, bool> engines = new Dictionary<WeakReference, bool>();

        public static void SetEnabled(this SpeechRecognitionEngine self, bool enabled)
        {
            var found = engines.Select( wr => new { Key = wr.Key, Value = wr.Value}).FirstOrDefault( wr => wr.Key.Target == self);
            
            if( found != null )
                engines.Remove( found.Key );

            engines.Add(new WeakReference(self), enabled);
        }

        public static bool IsEnabled(this SpeechRecognitionEngine self)
        {
            var found = engines.Select(wr => new { Key = wr.Key, Value = wr.Value }).FirstOrDefault(wr => wr.Key.Target == self);

            if (found == null)
                return true;

            return found.Value;
        }

        static void CleanEngines()
        {
            var found = engines.Select(wr => new { Key = wr.Key, Value = wr.Value }).FirstOrDefault(wr => wr.Key.Target == null);

            while( found != null )
            {
                engines.Remove(found.Key);

                found = engines.Select(wr => new { Key = wr.Key, Value = wr.Value }).FirstOrDefault(wr => wr.Key.Target == null);
            }
        }

        public static void LoadGrammar(this SpeechRecognitionEngine self, Action<RecognitionResult> speechReconized, string phrase)
        {
            self.LoadGrammar(r => true, speechReconized, phrase);
        }

        public static void LoadGrammar(this SpeechRecognitionEngine self, Func<RecognitionResult, bool> reconizerGuard, Action<RecognitionResult> speechReconized, string phrase)
        {
            var g = new Grammar(new GrammarBuilder(phrase));
            g.SpeechRecognized += (s, e) => SpeechReconized(self, reconizerGuard, speechReconized, e);

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
            g.SpeechRecognized += (s, e) => SpeechReconized(self, reconizerGuard, speechReconized, e);

            self.LoadGrammar(g);
        }

        static void SpeechReconized(SpeechRecognitionEngine self, Func<RecognitionResult, bool> reconizerGuard, Action<RecognitionResult> speechReconized, SpeechRecognizedEventArgs e)
        {
            try
            {
                if( self.IsEnabled() && reconizerGuard(e.Result))
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

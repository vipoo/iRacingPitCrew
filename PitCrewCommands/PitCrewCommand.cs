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
using System.Speech.Synthesis;
using System.Threading;
using iRacingPitCrew.Support;

namespace iRacingPitCrew.PitCrewCommands
{
    public abstract class PitCrewCommand
    {
        readonly SpeechSynthesizer synthesizer;
        readonly SpeechRecognitionEngine recognizer;
        readonly Action recognized;

        Timer timer;
        Grammar grammar;

        public PitCrewCommand(SpeechRecognitionEngine recognizer, SpeechSynthesizer synthesizer, Action recognized)
        {
            this.recognizer = recognizer;
            this.synthesizer = synthesizer;
            this.recognized = recognized;
        }

        public virtual bool Enable
        {
            get
            {
                return grammar.Enabled;
            }
            set
            {
                grammar.Enabled = value;
            }
        }

        protected void SetGrammar(string phrase)
        {
            grammar = recognizer.LoadGrammar(CommandRecognized, phrase);
            grammar.Enabled = false;
        }

        protected void SetGrammar(Action<GrammarBuilder> builder)
        {
            grammar = recognizer.LoadGrammar(CommandRecognized, builder);
            grammar.Enabled = false;
        }

        protected void SetGrammar(Choices choices)
        {
            grammar = recognizer.LoadGrammar(CommandRecognized, g => g.Append(choices));
            grammar.Enabled = false;
        }

        protected void SpeakAsync(string p)
        {
            synthesizer.SpeakAsync(p);
        }

        protected abstract void Command(RecognitionResult rr);

        protected Choices Numbers()
        {
            var digits = new Choices();

            for (int i = 0; i < 121; i++)
                digits.Add(new SemanticResultValue(i.ToString(), i));

            return digits;
        }

        protected Action AskQuestion(string question, Action<GrammarBuilder> matching, Action<RecognitionResult> answer, Action next)
        {
            var gb = new GrammarBuilder();
            matching(gb);

            var grammar = new Grammar(gb);
            grammar.Enabled = false;

            recognizer.LoadGrammar(grammar);

            grammar.SpeechRecognized += (s, e) =>
            {
                if (Recognition.ProcessCommand(e.Result))
                {
                    grammar.Enabled = false;
                    timer.Dispose();
                    answer(e.Result);
                    next();
                }
            };

            return () =>
            {
                grammar.Enabled = true;
                synthesizer.SpeakAsync(question);
                timer = new Timer(DisableGrammar, grammar, 15000, Timeout.Infinite);
            };
        }

        void CommandRecognized(RecognitionResult rr)
        {
            recognized();
            Command(rr);
        }

        void DisableGrammar(object state)
        {
            var grammar = (Grammar)state;
            grammar.Enabled = false;

            if (timer != null)
                timer.Dispose();
        }
    }
}

using System;

namespace ConsoleGameEngine
{
    public class Timer
    {
        private DateTime start;
        public TimeSpan Span { get; set; }

        public Timer()
        {
            start = DateTime.Now;
        }

        public bool Elapsed => DateTime.Now - start > Span;

        public void Reset()
        {
            start = DateTime.Now;
        }
    }
}
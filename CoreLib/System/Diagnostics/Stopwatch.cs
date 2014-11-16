namespace System.Diagnostics
{
    using System;

    public class Stopwatch
    {
        // Internal state.
        private DateTime start;
        private DateTime stop;
        private bool running;

        public Stopwatch()
        {
            stop = DateTime.Now;
            start = stop;
            running = false;
        }

        public void Start()
        {
            if (!running)
            {
                stop = DateTime.Now;
                start = stop;
                running = true;
            }
        }

        public void Stop()
        {
            if (running)
            {
                stop = DateTime.Now;
                running = false;
            }
        }

        public void Clear()
        {
            stop = DateTime.Now;
            start = stop;
        }

        public TimeSpan Elapsed
        {
            get
            {
                if (running)
                {
                    stop = DateTime.Now;
                }

                return stop - start;
            }
        }
    }
}

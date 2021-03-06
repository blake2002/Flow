using System;
using System.Collections.Generic;

namespace Flow.Impl
{
    /// <summary>
    /// A barrier that completes after a certain time.
    /// Barriers normally only complete when all of it contents are complete.
    /// </summary>
    internal class TimedBarrier : Barrier, ITimedBarrier
    {
        public event TimedOutHandler TimedOut;
        public ITimer Timer { get; }

        public bool HasTimedOut { get; set; }

        public TimedBarrier(IKernel kernel, TimeSpan span, IEnumerable<ITransient> contents)
        {
            Timer = kernel.Factory.OneShotTimer(span);
            Timer.Elapsed += (tr) =>
            {
                HasTimedOut = true;
                TimedOut?.Invoke(this);
                Complete();
            };

            foreach (var tr in contents)
                Add(tr);
        }
    }
}

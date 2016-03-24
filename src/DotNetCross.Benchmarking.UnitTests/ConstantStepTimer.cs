using System;

namespace DotNetCross.Benchmarking.UnitTests
{
    public sealed class ConstantStepTimer : ITimer
    {
        long _current = 0;

        public long Step { get; set; }

        public Ticks Now
        {
            get
            {
                _current += Step;
                return new Ticks(_current);
            }
        }
    }
}
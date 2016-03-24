using System;

namespace DotNetCross.Benchmarking.UnitTests
{
    public sealed class DateTimeTimer : ITimer
    {
        public Ticks Now => DateTime.UtcNow.Ticks;
    }
}
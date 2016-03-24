using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DotNetCross.Benchmarking
{
    public struct StopwatchTimer : ISpecTimer
    {
        public Ticks Now
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new Ticks(Stopwatch.GetTimestamp()); }
        }

        public TimerSpec Spec => new TimerSpec(Stopwatch.Frequency);
    }
}

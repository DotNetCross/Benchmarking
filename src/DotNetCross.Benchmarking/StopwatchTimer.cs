using System.Diagnostics;

namespace DotNetCross.Benchmarking
{
    public sealed class StopwatchTimer : ITimer
    {
        public Ticks Now => new Ticks(Stopwatch.GetTimestamp());

        public TimerSpec Spec => new TimerSpec(Stopwatch.Frequency);
    }
}

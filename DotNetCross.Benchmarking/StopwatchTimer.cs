using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCross.Benchmarking
{
    public class StopwatchTimer : ITimer
    {
        public long Now => Stopwatch.GetTimestamp();

        public TimerSpec Spec => new TimerSpec(Stopwatch.Frequency);
    }
}

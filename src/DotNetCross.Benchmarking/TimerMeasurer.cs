using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCross.Benchmarking
{
    public sealed class TimerMeasurer
    {
        public Ticks MeasureOverhead<TTimer>(TTimer timer)
            where TTimer : ITimer
        {
            // TODO: Clean this up, do we really need the whole warm-up thing...
            //       refactor constants etc.

            // JIT
            var diffJit = Measurer.MeasureMinDiffInsideLoop(timer, new NoOpAction(), 10);
            // Warm-up
            var diffWarmUp = Measurer.MeasureMinDiffInsideLoop(timer, new NoOpAction(), 100);
            // Measure
            return Measurer.MeasureMinDiffInsideLoop(timer, new NoOpAction(), 1000);
        }
    }
}

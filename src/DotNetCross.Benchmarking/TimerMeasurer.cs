using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DotNetCross.Benchmarking.Actions;

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

        public Ticks MeasurePrecision<TTimer>(TTimer timer)
            where TTimer : ITimer
        {
            // TODO: Make reliable, do proper monotonic search i.e. binary or something
            // TODO: Clean this up, do we really need the whole warm-up thing...
            //       refactor constants etc.
            var action = new DelegateAction(Delegates.NoOpAction);
            // JIT/Warmup
            var diffJit = Measurer.MeasureDiffOutsideLoop(timer, action, 100);

            Ticks diff = 0;
            var iterations1 = 0;
            var iterations2 = iterations1 * 10;
            while (diff == 0)
            {
                var diff1 = Measurer.MeasureDiffOutsideLoop(timer, action, iterations1);
                var diff2 = Measurer.MeasureDiffOutsideLoop(timer, action, iterations2);
                diff = Math.Max(0, diff2 - diff1);
                if (diff == 0)
                {
                    iterations2 *= 10;
                }
                // TODO: Handle searching backwards from diff to minimum diff
            }
            return diff;
        }
    }
}

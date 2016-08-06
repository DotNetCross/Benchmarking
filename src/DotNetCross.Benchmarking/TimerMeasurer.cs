using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            // Find minimal iterations before dependably getting a non-zero diff
            // Increase or decrease iterations to get close to zero
            bool hasBeenZero = false;
            bool hasBeenNonZero = false;
            int iterations = 100000; // Start guess
            int iterationsZero = iterations;
            int iterationsNonZero = iterations;
            while (!hasBeenZero || !hasBeenNonZero)
            {
                var diffNew = Measurer.MeasureDiffOutsideLoop(timer, action, iterations);
                if (diffNew <= 0)
                {
                    iterationsZero = iterations;
                    hasBeenZero = true;
                    iterations *= 2;
                }
                else
                {
                    iterationsNonZero = iterations;
                    hasBeenNonZero = true;
                    iterations = Math.Max(1, iterations / 2);
                }
            }
            // Find reliable non-zero iterations
            const int nonZeroTrials = 10;

            Ticks minDiff = 0;
            iterationsNonZero = Math.Max(1, iterationsNonZero / 2);
            while (minDiff <= 0)
            {
                iterationsNonZero *= 2;
                for (int i = 0; i < nonZeroTrials; i++)
                {
                    minDiff = Math.Min(minDiff, Measurer.MeasureDiffOutsideLoop(timer, action, iterationsNonZero));
                }
            }
            // Now we have upper bound (always non-zero)

            Ticks maxDiff = long.MaxValue;
            iterationsZero = iterationsNonZero * 2;
            while (maxDiff > 0 || iterationsNonZero == 0)
            {
                iterationsZero /= 2;
                for (int i = 0; i < nonZeroTrials; i++)
                {
                    maxDiff = Math.Min(maxDiff, Measurer.MeasureDiffOutsideLoop(timer, action, iterationsZero));
                }
            }
            // Now we have lower bound (always zero)

            // Keep moving the two closer...
            var currentIterationsZero = iterationsZero;
            var previousIterationsZero = 0;
            var currentIterationsNonZero = iterationsNonZero;
            var previousIterationsNonZero = 0;



            // Then find minimal increase in iterations for this to yield dependable diff of diffs


            Ticks diff = 0;
            var iterations1 = 0;
            var iterations2 = 1;
            var previousIterations2 = iterations2;
            int factor = 10;
            while (diff == 0 || previousIterations2 != iterations2)
            {
                var diff1 = Measurer.MeasureDiffOutsideLoop(timer, action, iterations1);
                var diff2 = Measurer.MeasureDiffOutsideLoop(timer, action, iterations2);
                diff = Math.Max(0, diff2 - diff1);
                if (diff == 0)
                {
                    previousIterations2 = iterations2;
                    iterations2 *= factor;
                }
                else
                {
                    iterations2 = (iterations2 + previousIterations2) / 2;
                    factor = Math.Max(2, factor / 2);
                }
                // TODO: Handle searching backwards from diff to minimum diff
            }
            Debug.WriteLine(iterations2);
            return diff;
        }
    }
}

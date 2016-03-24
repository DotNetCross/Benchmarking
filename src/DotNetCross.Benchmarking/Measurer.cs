using System;
using DotNetCross.Benchmarking.Actions;

namespace DotNetCross.Benchmarking
{
    public static class Measurer
    {
        public static Ticks MeasureMinDiffInsideLoop<TTimer, TAction>(
            TTimer timer, TAction action, int iterations)
            where TTimer : ITimer
            where TAction : IAction
        {
            var minTicks = new Ticks(long.MaxValue);
            for (int i = 0; i < iterations; i++)
            {
                var before = timer.Now;
                action.Invoke();
                var after = timer.Now;
                var diff = after - before;
                minTicks = Math.Min(minTicks, diff);
            }
            return minTicks;
        }

        public static Ticks MeasureDiffOutsideLoop<TTimer, TAction>(
            TTimer timer, TAction action, int iterations)
            where TTimer : ITimer
            where TAction : IAction
        {
            var before = timer.Now;
            for (int i = 0; i < iterations; i++)
            {
                action.Invoke();
            }
            var after = timer.Now;
            var diff = after - before;
            return diff;
        }
    }
}
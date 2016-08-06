using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DotNetCross.Benchmarking.UnitTests
{
    public class Brainstorm
    {
        readonly ITestOutputHelper _output;

        public Brainstorm(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void DeterminePrecisionOfTimer()
        {
            var timer = new StopwatchTimer();
            var timerSpec = timer.Spec;
            var jit = timer.Now;
            for (int i = 0; i < 100; i++)
            {
                var before = timer.Now;
                var after = timer.Now;
                while (after == before)
                { after = timer.Now; }
                var precision = after - before;
                // The call to Now itself takes time so precision is often many ticks e.g. 100
                _output.WriteLine(precision.ToString());
            }
        }

        [Fact]
        public void MeasureTimerOverhead_Stopwatch()
        {
            var minDiff = new TimerMeasurer().MeasureOverhead(new StopwatchTimer());
            // The call to Now itself takes time so precision is often many ticks e.g. 100
            _output.WriteLine(minDiff.ToString());
        }
        [Fact]
        public void MeasureTimerPrecision_Stopwatch()
        {
            for (int i = 0; i < 100; i++)
            {
                var precision = new TimerMeasurer().MeasurePrecision(new StopwatchTimer());
                _output.WriteLine(precision.ToString());
            }
        }
        [Fact]
        public void MeasureTimerPrecision_DateTime()
        {
            for (int i = 0; i < 10; i++)
            {
                var precision = new TimerMeasurer().MeasurePrecision(new DateTimeTimer());
                _output.WriteLine(precision.ToString());
            }
        }
        [Fact]
        public void MeasureTimerOverhead_QueryPerformanceCounter()
        {
            var minDiff = new TimerMeasurer().MeasureOverhead(new QueryPerformanceCounterTimer());
            // The call to Now itself takes time so precision is often many ticks e.g. 100
            _output.WriteLine(minDiff.ToString());
        }
    }
}
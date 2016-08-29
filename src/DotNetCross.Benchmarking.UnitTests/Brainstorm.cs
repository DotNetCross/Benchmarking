using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DotNetCross.Benchmarking.UnitTests
{
    public class Brainstorm
    {
        readonly ITestOutputHelper _output;
        // Ensure on LOH for minimal GC impact
        readonly Ticks[] m_globalTicks = new Ticks[(128 * 1024) / Marshal.SizeOf(typeof(Ticks))]; 
        readonly ArraySegment<Ticks> m_precisionMeasurements;
        int m_globalOffset = 0;

        public Brainstorm(ITestOutputHelper output)
        {
            _output = output;
            m_precisionMeasurements = new ArraySegment<Ticks>(m_globalTicks, m_globalOffset, 11);
            m_globalOffset += m_precisionMeasurements.Count;
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
            var minDiff = TimerMeasurer.MeasureOverhead(new StopwatchTimer());
            // The call to Now itself takes time so precision is often many ticks e.g. 100
            _output.WriteLine(minDiff.ToString());
        }

        [Fact]
        public void MeasureTimerPrecision_Stopwatch()
        {
            var timer = new StopwatchTimer();
            MeasureTimerPrecision(timer);
        }

        [Fact]
        public void MeasureTimerPrecision_QueryPerformanceCounter()
        {
            var timer = new QueryPerformanceCounterTimer();
            MeasureTimerPrecision(timer);
        }

        [Fact]
        public void MeasureTimerPrecision_DateTime()
        {
            var timer = new DateTimeTimer();
            MeasureTimerPrecision(timer);
        }

        private void MeasureTimerPrecision<TTimer>(TTimer timer)
            where TTimer : ITimer
        {
            for (int i = 0; i < 10; i++)
            {
                TimerMeasurer.MeasurePrecision(timer, m_precisionMeasurements);
                m_precisionMeasurements.Sort();
                var min = m_precisionMeasurements.First();
                var median = m_precisionMeasurements.Middle();
                var max = m_precisionMeasurements.Last();
                _output.WriteLine($"Precision {min} - {median} - {max} ticks");
            }
        }
        //[Fact]
        //public void MeasureTimerPrecision_Stopwatch()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        var precision = new TimerMeasurer().MeasurePrecision(new StopwatchTimer());
        //        _output.WriteLine(precision.ToString());
        //    }
        //}
        //[Fact]
        //public void MeasureTimerPrecision_DateTime()
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        var precision = new TimerMeasurer().MeasurePrecision(new DateTimeTimer());
        //        _output.WriteLine(precision.ToString());
        //    }
        //}
        //[Fact]
        //public void MeasureTimerOverhead_QueryPerformanceCounter()
        //{
        //    var minDiff = new TimerMeasurer().MeasureOverhead(new QueryPerformanceCounterTimer());
        //    // The call to Now itself takes time so precision is often many ticks e.g. 100
        //    _output.WriteLine(minDiff.ToString());
        //}
    }
}
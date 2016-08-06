using System;
using Xunit;

namespace DotNetCross.Benchmarking.UnitTests
{
    public class TimerMeasurerTest
    {
        public readonly TimerMeasurer _sut = new TimerMeasurer();

        [Fact]
        public void MeasureMinOverhead_ConstantStep()
        {
            long step = 17;
            var timer = new ConstantStepTimer() { Step = step };

            var m = _sut.MeasureOverhead(timer);

            Assert.Equal(step, m.Value);
        }

        [Fact]
        public void MeasureMinOverhead_Random()
        {
            int min = 3;
            var timer = new RandomStepTimer(17, 3, 42);

            var m = _sut.MeasureOverhead(timer);

            Assert.Equal(min, m.Value);
        }

        [Fact]
        public void MeasurePrecision_ConstantStep()
        {
            long step = 17;
            var timer = new ConstantStepTimer() { Step = step };
            var measurements = new Ticks[53];

            _sut.MeasurePrecision(timer, new ArraySegment<Ticks>(measurements));

            foreach (var m in measurements)
            { 
                Assert.Equal(step, m.Value);
            }
        }

        [Fact]
        public void MeasurePrecision_Random()
        {
            int min = 3;
            int max = 42;
            var timer = new RandomStepTimer(17, min, max);
            var measurements = new Ticks[53];

            _sut.MeasurePrecision(timer, new ArraySegment<Ticks>(measurements));

            foreach (var m in measurements)
            {
                Assert.InRange(m.Value, min, max);
            }
        }
    }
}

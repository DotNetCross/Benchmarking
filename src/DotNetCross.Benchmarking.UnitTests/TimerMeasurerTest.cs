using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

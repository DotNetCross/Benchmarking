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
        readonly ITestOutputHelper m_output;

        public Brainstorm(ITestOutputHelper output)
        {
            m_output = output;
        }

        [Fact]
        public void DeterminePrecisionViaTimer()
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
                m_output.WriteLine(precision.ToString());
            }
        }
    }
}

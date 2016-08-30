using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace DotNetCross.Benchmarking.UnitTests
{
    public class QuickPerfTest
    {
        byte[] m_src = new byte[128 * 1024];
        byte[] m_dst = new byte[128 * 1024];
        int m_count;
        int m_offset;
        private readonly ITestOutputHelper m_output;

        public QuickPerfTest(ITestOutputHelper output)
        {
            m_output = output;
        }
        

        [Fact]
        public void Run()
        {
            var measurements = QuickPerf.New(nameof(QuickPerfTest))
                .Params(new { Count = 2, Offset = 1 }, new { Count = 43, Offset = 1 })
                .ParamDetail(p => p.Count.ToString(), p => nameof(p.Count))
                .ParamDetail(p => p.Offset.ToString(), p => nameof(p.Offset))
                // TODO: Add Setup scope e.g. PerParams, PerMeasurement, PerIteration/PerOp 
                // TODO: Perhaps return object to test on instead/reuse is possible...
                .Setup(p => Setup(p.Count, p.Offset)) 
                .Measure(ArrayCopy, nameof(ArrayCopy))
                .Measure(SimpleLoop, nameof(SimpleLoop))
                .Run(t => m_output.WriteLine(t));
        }

        public void Setup(int count, int offset)
        {
            m_count = count;
            m_offset = offset;
        }

        public void ArrayCopy()
        {
            Array.Copy(m_src, m_offset, m_dst, m_offset, m_count);
        }

        public void SimpleLoop()
        {
            for (int i = 0; i < m_count; i++)
            {
                m_dst[i + m_offset] = m_src[i + m_offset];
            }
        }
    }
}

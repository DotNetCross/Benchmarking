using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetCross.Benchmarking.UnitTests
{
    class QuickPerfTest
    {
        [Fact]
        public void Run()
        {
            QuickPerf.New("name")
                .Params(new { Count = 2, Offset = 1 }, new { Count = 3, Offset = 1 })
                .ParamDetail(p => p.Count.ToString(), p => nameof(p.Count))
                .ParamDetail(p => p.Offset.ToString(), p => nameof(p.Offset))
                .Measure(p => MethodA(p.Count, p.Offset), nameof(MethodA))
                .Measure(p => MethodB(p.Count, p.Offset), nameof(MethodB))
                .Run();
        }

        public void MethodA(int count, int offset)
        {

        }

        public void MethodB(int count, int offset)
        {

        }
    }
}

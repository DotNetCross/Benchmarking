using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCross.Benchmarking
{
    public interface ITimer
    {
        Ticks Now { get; }
    }
    public interface ISpecTimer : ITimer
    { 
        TimerSpec Spec { get; }
    }
}

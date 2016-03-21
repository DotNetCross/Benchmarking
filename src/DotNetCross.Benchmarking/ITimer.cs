using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCross.Benchmarking
{
    public interface ITimer
    {
        TimerSpec Spec { get; }
        Ticks Now { get; }
    }
}

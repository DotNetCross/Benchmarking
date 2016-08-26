using System;

namespace DotNetCross.Benchmarking.UnitTests
{
    internal class QuickPerf
    {
        internal static QuickPerfConfig<Void> New(string name)
        {
            return new QuickPerfConfig<Void>(name);
        }
    }
}
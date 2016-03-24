using System.Runtime.CompilerServices;

namespace DotNetCross.Benchmarking
{
    public struct NoOpAction : IAction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke() { }
    }
}
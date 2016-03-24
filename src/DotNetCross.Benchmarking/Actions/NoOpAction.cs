using System.Runtime.CompilerServices;

namespace DotNetCross.Benchmarking.Actions
{
    public struct NoOpAction : IAction
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke() { }
    }
}
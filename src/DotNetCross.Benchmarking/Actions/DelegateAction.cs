using System;
using System.Runtime.CompilerServices;

namespace DotNetCross.Benchmarking.Actions
{
    public struct DelegateAction : IAction
    {
        readonly Action _action;

        public DelegateAction(Action action)
        {
            if (action == null) { throw new ArgumentNullException(nameof(action)); }
            _action = action;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Invoke() { _action(); }
    }
}
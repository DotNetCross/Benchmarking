using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace DotNetCross.Benchmarking
{
    public unsafe struct QueryPerformanceCounterTimer : ISpecTimer
    {
        private const string Kernel32 = "kernel32.dll";

        public Ticks Now
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                long ticks;
                QueryPerformanceCounter(out ticks);
                return ticks;
            }
        }

        public TimerSpec Spec
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                long frequency;
                QueryPerformanceFrequency(out frequency);
                return new TimerSpec(frequency);
            }
        }

        [DllImport(Kernel32), SuppressUnmanagedCodeSecurity]
        static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport(Kernel32), SuppressUnmanagedCodeSecurity]
        static extern bool QueryPerformanceFrequency(out long lpFrequency);
    }
}
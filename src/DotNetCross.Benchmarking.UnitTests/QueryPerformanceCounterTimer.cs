using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using DWORD = System.UInt32;

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
        static extern DWORD QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport(Kernel32), SuppressUnmanagedCodeSecurity]
        static extern DWORD QueryPerformanceFrequency(out long lpFrequency);
    }
}
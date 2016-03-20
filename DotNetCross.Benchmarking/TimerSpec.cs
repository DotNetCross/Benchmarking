namespace DotNetCross.Benchmarking
{
    public struct TimerSpec
    {
        public readonly long Frequency;

        public TimerSpec(long frequency)
        {
            Frequency = frequency;
        }
    }
}
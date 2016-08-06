namespace DotNetCross.Benchmarking
{
    public struct MinMaxTicks
    {
        public readonly Ticks Min;
        public readonly Ticks Max;

        public MinMaxTicks(Ticks min, Ticks max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}

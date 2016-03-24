using System;

namespace DotNetCross.Benchmarking.UnitTests
{
    public sealed class RandomStepTimer : ITimer
    {
        readonly Random _random;
        readonly int _minValue;
        readonly int _maxValue;
        long _current = 0;

        public RandomStepTimer(int seed, int minValue, int maxValue)
        {
            _random = new Random(seed);
            _minValue = minValue;
            _maxValue = maxValue;
        }

        public Ticks Now => _current += _random.Next(_minValue, _maxValue);
    }
}
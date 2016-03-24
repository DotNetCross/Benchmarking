using System;
using System.Diagnostics;
using System.Globalization;

namespace DotNetCross.Benchmarking
{
    [DebuggerDisplay("{Value}")]
    public struct Ticks
        : IComparable
        , IComparable<Ticks>
        , IEquatable<Ticks>
        , IFormattable
    {
        public readonly long Value;

        public Ticks(long ticks)
        {
            Value = ticks;
        }

        public static implicit operator long(Ticks t) => t.Value;
        public static implicit operator Ticks(long l) => new Ticks(l);

        /// <remarks>Note overflow/underflow may occur. TODO Might have to replace this with TicksSpan</remarks>
        public static Ticks operator -(Ticks t1, Ticks t2)
        {
            return new Ticks(t1.Value - t2.Value);
        }
        /// <remarks>Note overflow/underflow may occur. TODO Might have to replace this with TicksSpan</remarks>
        public static Ticks operator +(Ticks t1, Ticks t2)
        {
            return new Ticks(t1.Value + t2.Value);
        }
        public static bool operator <(Ticks t1, Ticks t2)
        {
            return t1.Value < t2.Value;
        }
        public static bool operator <=(Ticks t1, Ticks t2)
        {
            return t1.Value <= t2.Value;
        }
        public static bool operator ==(Ticks t1, Ticks t2)
        {
            return t1.Value == t2.Value;
        }
        public static bool operator !=(Ticks t1, Ticks t2)
        {
            return t1.Value != t2.Value;
        }
        public static bool operator >(Ticks t1, Ticks t2)
        {
            return t1.Value > t2.Value;
        }
        public static bool operator >=(Ticks t1, Ticks t2)
        {
            return t1.Value >= t2.Value;
        }

        public int CompareTo(Ticks other)
        {
            if (this < other)
            {
                return -1;
            }
            if (this > other)
            {
                return 1;
            }
            return 0;
        }
        public int CompareTo(object other)
        {
            if (other == null) { return -1; }

            if (other.GetType() != typeof(Ticks))
            { return -1; }

            return CompareTo((Ticks)other);

        }

        public bool Equals(Ticks other)
        {
            if (this.Value == other.Value)
            {   return true; }
            else
            {   return false; }
        }
        public override bool Equals(object other)
        {
            if (other == null) { return false; }

            if (other.GetType() != typeof(Ticks))
            { return false; }

            return Equals((Ticks)other);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString(CultureInfo.InvariantCulture);
        }

        public string ToString(string format)
        {
            return this.Value.ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return this.Value.ToString(format, formatProvider);
        }

        public static Ticks Parse(string timeString, IFormatProvider formatProvider)
        {
            return new Ticks(long.Parse(timeString, formatProvider));
        }
    }
}

using System;

namespace DotNetCross.Benchmarking
{
    public static class ArraySegmentExtensions
    {
        public static T Get<T>(this ArraySegment<T> a, int index)
        {
            return a.Array[a.Offset + index];
        }
        public static void Set<T>(this ArraySegment<T> a, int index, T value)
        {
            a.Array[a.Offset + index] = value;
        }

        public static void Sort<T>(this ArraySegment<T> a)
        {
            Array.Sort(a.Array, a.Offset, a.Count);
        }

        public static T First<T>(this ArraySegment<T> a) => a.Array[a.Offset];
        public static T Middle<T>(this ArraySegment<T> a) => a.Array[a.Offset + a.Count / 2];
        public static T Last<T>(this ArraySegment<T> a) => a.Array[a.Offset + a.Count - 1];
    }
}

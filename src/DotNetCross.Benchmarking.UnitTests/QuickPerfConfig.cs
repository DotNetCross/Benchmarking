using System;

namespace DotNetCross.Benchmarking.UnitTests
{
    public struct Void { }

    public class QuickPerfConfig<T>
    {
        readonly T[] m_params;

        public QuickPerfConfig(string name)
            : this(name, new T[0])
        { }

        public QuickPerfConfig(string name, T[] parameters)
        {
            if (name == null) { throw new ArgumentNullException(nameof(name)); }
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
            Name = name;
            m_params = parameters;
        }

        internal QuickPerfConfig<T> ParamDetail(Func<T, string> toValueText, Func<T, string> toName)
        {
            throw new NotImplementedException();
        }

        internal QuickPerfConfig<TParam> Params<TParam>(params TParam[] ps)
        {
            return new QuickPerfConfig<TParam>(Name, ps);
        }

        public string Name { get;}

        public QuickPerfConfig<T> Measure(Action<T> methodA, string name)
        {
            throw new NotImplementedException();
        }
    }

    public static class QuickPerfConfigExtensions
    {
        public static void Run<T>(this QuickPerfConfig<T> config)
        { }
    }
}
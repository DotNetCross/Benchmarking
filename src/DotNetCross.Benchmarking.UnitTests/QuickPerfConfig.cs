using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DotNetCross.Benchmarking.UnitTests
{
    public struct Void { }

    public class QuickPerfConfig<T>
    {
        static readonly Action IdleAction = () => { };
        readonly T[] m_params;
        struct ParamDetailFuncs
        {
            internal readonly Func<T, string> ToValueText;
            internal readonly Func<T, string> ToName;
            public ParamDetailFuncs(Func<T, string> toValueText, Func<T, string> toName)
            {
                if (toValueText == null) { throw new ArgumentNullException(nameof(toValueText)); }
                if (toName == null) { throw new ArgumentNullException(nameof(toName)); }
                ToValueText = toValueText;
                ToName = toName;
            }
        }
        readonly List<ParamDetailFuncs> m_paramDetails = new List<ParamDetailFuncs>();
        Action<T> m_setup = null;
        struct NamedMethod
        {
            internal readonly Action Method;
            internal readonly string Name;
            public NamedMethod(Action method, string name)
            {
                if (method == null) { throw new ArgumentNullException(nameof(method)); }
                if (name == null) { throw new ArgumentNullException(nameof(name)); }
                Method = method;
                Name = name;
            }
        }
        readonly List<NamedMethod> m_methods = new List<NamedMethod>();

        Ticks[] m_globalTicks = new Ticks[(128 * 1024) / Marshal.SizeOf(typeof(Ticks))];
        int m_globalOffset = 0;

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
        public string Name { get; }

        internal QuickPerfConfig<TParam> Params<TParam>(params TParam[] ps)
        {
            return new QuickPerfConfig<TParam>(Name, ps);
        }

        internal QuickPerfConfig<T> ParamDetail(Func<T, string> toValueText, Func<T, string> toName)
        {
            m_paramDetails.Add(new ParamDetailFuncs(toValueText, toName));
            return this;
        }

        internal QuickPerfConfig<T> Setup(Action<T> setup)
        {
            m_setup = setup;
            return this;
        }

        public QuickPerfConfig<T> Measure(Action method, string name)
        {
            m_methods.Add(new NamedMethod(method, name));
            return this;
        }

        public QuickPerfMeasurements Run(Action<string> log)//<T>(this QuickPerfConfig<T> config)
        {
            var timer = new StopwatchTimer();
            // Measure timer precision
            const int precisionMeasurementCount = 11;
            var precisionMeasurements = ReserveMeasurements(precisionMeasurementCount);
            TimerMeasurer.MeasurePrecision(timer, precisionMeasurements);
            precisionMeasurements.Sort();
            var min = precisionMeasurements.First();
            var median = precisionMeasurements.Middle();
            var max = precisionMeasurements.Last();
            log?.Invoke($"Precision {min} - {median} - {max} ticks");
            // foreach params
            //   foreach method
            //       // GC
            //       find iteration count to reach required precision
            //       // GC
            //       measure idle time
            //       
            //       foreach measurement
            //          // GC
            //          measure method

            return new QuickPerfMeasurements();
        }

        private ArraySegment<Ticks> ReserveMeasurements(int precisionMeasurementCount)
        {
            // TODO: Add check if outside array
            var thisOffset = m_globalOffset;
            m_globalOffset += precisionMeasurementCount;
            return new ArraySegment<Ticks>(m_globalTicks, thisOffset, precisionMeasurementCount);
        }
    }

    public static class QuickPerfConfigExtensions
    {
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using DotNetCross.Benchmarking.Actions;

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
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
            Thread.CurrentThread.Priority = ThreadPriority.Highest;

            var timer = new StopwatchTimer();
            var timerSpec = timer.Spec;
            Func<double, double> toNs = ticks => 1000 * 1000 * 1000 * ticks / (double)timerSpec.Frequency;

            const int MinTicksExtraMultiplier = 40;
            const int PrecisionDigits = 3;
            int precisionMultiplier = 1;
            for (int i = 0; i < PrecisionDigits; i++)
            {   precisionMultiplier *= 10; }
            const int precisionMeasurementCount = 11;
            const int measurementsPerParamPerMethodCount = 21;

            // Measure timer precision
            var precisionMeasurements = ReserveMeasurements(precisionMeasurementCount);
            TimerMeasurer.MeasurePrecision(timer, precisionMeasurements);
            precisionMeasurements.Sort();
            var min = precisionMeasurements.First();
            var median = precisionMeasurements.Middle();
            var max = precisionMeasurements.Last();
            log?.Invoke($"Precision {min} - {median} - {max} ticks");

            var minTicksForMeasurement = median * precisionMultiplier * MinTicksExtraMultiplier;

            foreach (var param in m_params)
            {
                // TODO: When to do
                m_setup?.Invoke(param);

                foreach (var namedMethod in m_methods)
                {
                    var method = namedMethod.Method;
                    var methodAction = new DelegateAction(method);
                    var idleAction = new DelegateAction(IdleAction);
                    // Pre-JIT and warmup
                    var warmupIdleTime = Measurer.MeasureDiffOutsideLoop(timer, idleAction, 3);
                    var warmupTime = Measurer.MeasureDiffOutsideLoop(timer, methodAction, 3);

                    ForceAndWaitForGarbageCollection();
                    var iterationCount = FindIterationCount(timer, method, minTicksForMeasurement, log);

                    //ForceAndWaitForGarbageCollection();
                    var idleTime = Measurer.MeasureDiffOutsideLoop(timer, idleAction, iterationCount);
                    log?.Invoke($"Idle Ticks: {idleTime} ns: {toNs(idleTime / (double)iterationCount)}");

                    var measurements = ReserveMeasurements(measurementsPerParamPerMethodCount);
                    ForceAndWaitForGarbageCollection();
                    for (int i = 0; i < measurements.Count; i++)
                    {
                        //ForceAndWaitForGarbageCollection();
                        var methodTime = Measurer.MeasureDiffOutsideLoop(timer, methodAction, iterationCount);
                        measurements.Set(i, methodTime);
                        log?.Invoke($"{namedMethod.Name} Ticks: {methodTime} ns: {toNs(methodTime / (double)iterationCount)}");
                    }

                }
            }
            return new QuickPerfMeasurements();
        }

        private int FindIterationCount(StopwatchTimer timer, Action method, long minTicksForMeasurement,
            Action<string> log)
        {
            var iterationCount = 1;
            var diff = Measurer.MeasureDiffOutsideLoop(timer, new DelegateAction(method), iterationCount);
            while (diff < minTicksForMeasurement || diff > minTicksForMeasurement * 2)
            {
                iterationCount = (int)((iterationCount * minTicksForMeasurement) / Math.Max(diff, 1));
                diff = Measurer.MeasureDiffOutsideLoop(timer, new DelegateAction(method), iterationCount);
                log?.Invoke($"Diff {diff} IterationCount {iterationCount}");
            }

            return iterationCount;
        }
        
        private static void ForceAndWaitForGarbageCollection()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
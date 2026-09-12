using NUnit.Framework;
using System;
using System.Linq;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;

namespace NotBura.Packages.Tests
{
    public sealed class NotQueryArrayTests
    {
        private int[] m_source;

        [OneTimeSetUp]
        public void SetUp()
        {
            m_source = Create();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            m_source = null!;
        }

        [Test, Performance]
        public void SpeedLinqWhereTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .Where(x => x % 2 == 0)
                    .Select(x => (float)x)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        [Test, Performance]
        public void SpeedQueryWhereSelectTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .AsQuery()
                    .Where(x => x % 2 == 0)
                    .Select(x => (float)x)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        [Test, Performance]
        public void SpeedQueryWhereTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .AsQuery()
                    .Where(x => x % 2 == 0)
                    .Select<Where<ManagedIterator<int>, int>, int, float>(x => (float)x)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        [Test, Performance]
        public void SpeedLinqSkipTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .Skip(1000)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        [Test, Performance]
        public void SpeedQuerySkipTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .AsQuery()
                    .Skip(1000)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        [Test, Performance]
        public void SpeedLinqTakeTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .Take(9000)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        [Test, Performance]
        public void SpeedQueryTakeTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .AsQuery()
                    .Take(9000)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        [Test, Performance]
        public void SpeedLinqSkipTakeTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .Skip(1000)
                    .Take(9000)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        [Test, Performance]
        public void SpeedQuerySkipTakeTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_source;
                var _query = _source
                    .AsQuery()
                    .Skip(1000)
                    .Take(9000)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        private static int[] Create()
        {
            var _result = new int[10_000];

            var _span = _result.AsSpan();
            for (int i = 0; i < _span.Length; ++i)
            {
                _span[i] = i;
            }

            return _result;
        }

        private static void Run(MethodMeasurement measurement)
        {
            measurement
                .WarmupCount(50)
                .IterationsPerMeasurement(100)
                .MeasurementCount(100)
                .GC()
                .Run();
        }
    }
}

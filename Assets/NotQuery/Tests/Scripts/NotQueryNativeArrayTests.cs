using NUnit.Framework;
using System.Linq;
using Unity.Collections;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;

namespace NotBura.Packages.Tests
{
    public sealed class NotQueryNativeArrayTests
    {
        private NativeArray<int> m_source;

        [OneTimeSetUp]
        public void SetUp()
        {
            m_source = Create();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            m_source.Dispose();
        }

        [Test, Performance]
        public void SpeedLinqWhereTest()
        {
            UnityEngine.Debug.Log($"{m_source.IsCreated}");
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
                    .Select<Where<NativeArrayIterator<int>, int>, int, float>(x => (float)x)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        private static NativeArray<int> Create()
        {
            var _result = new NativeArray<int>(10_000, Allocator.Persistent);

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

using NUnit.Framework;
using Unity.Collections;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;

namespace NotBura.Packages.Tests
{
    public sealed class NotQueryNativeListTests
    {
        private NativeList<int> m_source;

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
                    .Select<Where<NativeListIterator<int>, int>, int, float>(x => (float)x)
                    ;

                foreach (var _value in _query)
                {
                }
            }
        }

        private static NativeList<int> Create()
        {
            var _result = new NativeList<int>(10_000, Allocator.Persistent);

            for (int i = 0; i < 10_000; ++i)
            {
                _result.Add(i);
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

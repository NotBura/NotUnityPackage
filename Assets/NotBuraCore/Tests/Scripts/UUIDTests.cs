using NUnit.Framework;
using System;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;

namespace NotBura.Core.Tests
{
    public class UUIDTests
    {
        private const string TEXT   = "12345678-9999-4abc-dddd-2A22444455F5";
        private const ulong HIGH    = 0x1234_5678_9999_4abc;
        private const ulong LOW     = 0xdddd_2A22_4444_55F5;

        [Test]
        public void CheckCreateTest()
        {
            var _source = TEXT;
            var _fromSpan = UUID.FromCharSpan(_source);
            var _fromValue = new UUID(HIGH, LOW);

            var _lhs = _fromSpan.ToString().AsSpan();
            var _rhs = _fromSpan.ToString().AsSpan();

            Assert.IsTrue(_lhs.SequenceEqual(_rhs));
        }

        [Test, Performance]
        public void SpeedGUIDTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = TEXT;
                var _value = new Guid(_source);
            }
        }

        [Test, Performance]
        public void SpeedUUIDTest()
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = TEXT;
                var _value = UUID.FromCharSpan(_source);
            }
        }

        private static void Run(MethodMeasurement measurement)
        {
            measurement
                .WarmupCount(50)
                .IterationsPerMeasurement(10000)
                .MeasurementCount(100)
                .GC()
                .Run();
        }
    }
}

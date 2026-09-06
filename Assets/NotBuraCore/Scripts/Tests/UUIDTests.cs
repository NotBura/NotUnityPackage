using NUnit.Framework;
using System;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;
using UnityEngine;

namespace NotBura.Core.Tests
{
    public class UUIDTests
    {
        private const string SOURCE = "12345678-9999-4abc-dddd-222244445555";

        [Test]
        public void SimpleUUIDTest()
        {
            var source = SOURCE;
            var a = UUID.FromCharSpan(source);
            var b = new UUID(0x1234_5678_9999_4abc, 0xdddd_2222_4444_5555);
            var c = a.ToString();

            Debug.Log(@$"{a}
{b}
{c}");
        }

        [Test, Performance]
        public void SpeedGUIDTest()
        {
            var measurement = Measure.Method(Impl);
            Run(measurement);

            void Impl()
            {
                var source = SOURCE;
                for (int i = 0; i < 100_000; ++i)
                {
                    var a = new Guid(source);
                }
            }
        }

        [Test, Performance]
        public void SpeedUUIDTest()
        {
            var measurement = Measure.Method(Impl);
            Run(measurement);

            void Impl()
            {
                var source = SOURCE;
                for (int i = 0; i < 100_000; ++i)
                {
                    var a = UUID.FromCharSpan(source);
                }
            }
        }

        private void Run(MethodMeasurement measurement)
        {

        }
    }
}

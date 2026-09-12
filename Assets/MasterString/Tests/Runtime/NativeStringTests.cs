using NUnit.Framework;
using System;
using System.Text;
using Unity.Collections;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;
using UnityEngine;
using EncodingTypes = NotBura.Packages.NativeStringEncodingTypes;

namespace NotBura.Packages
{
    internal sealed class NativeStringTests
    {
        private string[] m_texts;

        [OneTimeSetUp]
        public void SetUp()
        {
            m_texts = TestsHelper.GetTexts();
        }

        [OneTimeTearDown]
        public void TearDowwn()
        {
            m_texts = null!;
        }

        [Test, TestCase(EncodingTypes.UTF16), TestCase(EncodingTypes.UTF8)]
        public void SimpleTableFromSourceTest(EncodingTypes encoding)
        {
            var _source = m_texts;

            using var _table = NativeStringTable.FromSource(_source, Allocator.Temp, encoding);

            Assert.AreEqual(_source.Length, _table.Length);

            var _sb = new StringBuilder();

            for (int i = 0; i < _table.Length; ++i)
            {
                var _item = _table[i];

                _sb.AppendLine(_item.ToString());

                using var _native = _item.ToNativeString(Allocator.Temp);

                var _lhs = _source[i].AsSpan();
                var _rhs = _native.AsSpan();
                Assert.IsTrue(_lhs.SequenceEqual(_rhs));

                _sb.AppendLine(_native.ToString());

                _sb.AppendLine();
            }

            Debug.Log(_sb.ToString());
        }

        [Performance]
        [Test, TestCase(EncodingTypes.UTF16), TestCase(EncodingTypes.UTF8)]
        public void SpeedTableFromSourceTest(EncodingTypes encoding)
        {
            var _measurement = Measure.Method(Impl);
            Run(_measurement);

            void Impl()
            {
                var _source = m_texts;

                using var _table = NativeStringTable.FromSource(_source, Allocator.Temp, encoding);

                for (int i = 0; i < _table.Length; ++i)
                {
                    var _item = _table[i];
                    _item.ToString();

                    //using var _native = _item.ToNativeString(Allocator.Temp);
                }
            }
        }

        private static void Run(MethodMeasurement measurement)
        {
            measurement
                .WarmupCount(50)
                .IterationsPerMeasurement(100)
                .MeasurementCount(50)
                .GC()
                .Run();
        }
    }
}

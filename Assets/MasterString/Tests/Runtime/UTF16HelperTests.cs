using NUnit.Framework;
using System.Text;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;

namespace NotBura.Packages
{
    internal sealed class UTF16HelperTests
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

        [Test]
        public void CheckResultByteCountEncodingEqualsHelperTest()
        {
            var _source = m_texts;
            var _lhs = GetByteCountEncoding(_source);
            var _rhs = GetByteCountHelper(_source);

            Assert.AreEqual(_lhs, _rhs);
        }

        [Test, Performance]
        public void SpeedByteCountHelperTest()
        {
            var _measurement = Measure.Method(() => GetByteCountHelper(m_texts));
            Run(_measurement);
        }

        [Test, Performance]
        public void SpeedByteCountEncodingTest()
        {
            var _measurement = Measure.Method(() => GetByteCountEncoding(m_texts));
            Run(_measurement);
        }

        private uint GetByteCountHelper(string[] source)
        {
            return UTF16Helper.ByteCount(source);
        }

        private uint GetByteCountEncoding(string[] source)
        {
            var _result = 0U;
            var _encoding = Encoding.Unicode;

            foreach (var _text in source)
            {
                _result += (uint)_encoding.GetByteCount(_text);
            }

            return _result;
        }

        private static void Run(MethodMeasurement measurement)
        {
            measurement
                .WarmupCount(50)
                .IterationsPerMeasurement(5000)
                .MeasurementCount(50)
                .GC()
                .Run();
        }
    }
}

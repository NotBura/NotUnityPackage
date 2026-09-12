using NUnit.Framework;
using System.Text;
using Unity.PerformanceTesting;
using Unity.PerformanceTesting.Measurements;

namespace NotBura.Packages
{
    internal sealed class UTF8HelperTests
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
            var _lhs = GetByteCountEncoding();
            var _rhs = GetByteCountHelper();

            Assert.AreEqual(_lhs, _rhs);
        }

        [Test, Performance]
        public void SpeedByteCountHelperTest()
        {
            var _measurement = Measure.Method(() => GetByteCountHelper());
            Run(_measurement);
        }

        [Test, Performance]
        public void SpeedByteCountEncodingTest()
        {
            var _measurement = Measure.Method(() => GetByteCountEncoding());
            Run(_measurement);
        }

        private uint GetByteCountHelper()
        {
            return UTF8Helper.ByteCount(m_texts);
        }

        private uint GetByteCountEncoding()
        {
            var _result = 0U;
            var _encoding = Encoding.UTF8;
            var _texts = m_texts;

            foreach (var _text in _texts)
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

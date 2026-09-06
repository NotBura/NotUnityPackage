using NUnit.Framework;
using UnityEngine;

namespace NotBura.Packages.Tests
{
    public sealed class RuntimeTests
    {
        [Test]
        public void Add()
        {
            using var core = NotVM.Create();

            var source = "1 + 2";
            var (instructions, frame, reader) = core.Compile(source);
            var result = core.Execute(instructions, frame, reader, 12);

            Debug.Log(result);
        }
    }
}


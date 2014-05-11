using System;
using Il2Native.Logic;
using NUnit.Framework;

namespace Ll2NativeTests
{
    [TestFixture]
    public class NUnitTests
    {
        private const string SourcePath = @"D:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";

        [Test]
        public void Test1()
        {
            Test(1);
        }

        [Test]
        public void Test2()
        {
            Test(2);
        }

        [Test]
        public void Test3()
        {
            Test(3);
        }

        private void Test(int number)
        {
            Il2Converter.Convert(string.Concat(SourcePath, string.Format("test-{0}.cs", number)), @"D:\Temp\IlCTests\");
        }
    }
}

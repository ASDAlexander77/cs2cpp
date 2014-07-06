namespace Ll2NativeTests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Il2Native.Logic;
    using System.IO;

    /// <summary>
    /// Summary description for MSTests
    /// </summary>
    [TestClass]
    public class MSTests
    {
        private const string SourcePath = @"D:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        private const string SourcePathCustom = @"D:\Temp\tests\";
        private const string OutputPath = @"D:\Temp\IlCTests\";
        //private const string CoreLibPath = @"C:\Dev\Temp\Il2Native\CoreLib\bin\Release\CoreLib.dll";
        private const string CoreLibPath = @"..\..\..\CoreLib\bin\Release\CoreLib.dll";

        public MSTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Test()
        {
            //Test(1);

            var newChars = new char[32];

            var @base = 10;
            var i = 30;
            var value = 0;

            if (value > 0)
            {
                for (; value > 0 && i > 0; --i, value /= @base)
                {
                    newChars[i] = "0123456789abcdef"[value % @base];
                }
            }
            else
            {
                newChars[i--] = "0123456789abcdef"[0];
            }

            var s = new String(newChars, i + 1, 30 - i + 1);
        }

        [TestMethod]
        public void TestGen()
        {
            TestGen(175);
        }

        [TestMethod]
        public void TestCustom()
        {
            TestCustom(1);
        }

        [TestMethod]
        public void TestCustomCompileAndRun()
        {
            TestCustomCompileAndRun(1);
        }

        [TestMethod]
        public void TestType()
        {
            Il2Converter.Convert(typeof(object), OutputPath);
        }

        [TestMethod]
        public void TestCoreLib()
        {
            Il2Converter.Convert(Path.GetFullPath(CoreLibPath), OutputPath);
        }

        [TestMethod]
        public void TestRunLlvm()
        {
            // 9, 10 - Decimal class
            var skip = new int[] { 10, 12, 16, 19, 20, 21, 26, 27, 28, 30, 33, 34, 35, 36, 37, 39, 40, 42, 43, 44, 45, 46, 49, 50, 52, 53, 55, 57 };

            foreach (var index in Enumerable.Range(1, 400).Where(n => !skip.Contains(n)))
            {
                RunInterpreter(index);
            }
        }

        [TestMethod]
        public void TestCompileAndRunLlvm()
        {
            // 9, 10 - Decimal class
            var skip = new int[] { 10, 12, 16, 19, 20, 21, 26, 27, 28, 30, 33, 34, 35, 36, 37, 39, 40, 42, 43, 44, 45, 46, 49, 50, 52, 53, 55, 57 };
            foreach (var index in Enumerable.Range(1, 400).Where(n => !skip.Contains(n)))
            {
                CompileAndRun(index);
            }
        }

        private void RunInterpreter(int index)
        {
            System.Diagnostics.Trace.WriteLine("Generating LLVM BC(ll) for " + index);

            Test(index);

            System.Diagnostics.Trace.WriteLine("Executing LLVM for " + index);

            var pi = new ProcessStartInfo();
            pi.WorkingDirectory = OutputPath;
            pi.FileName = "lli.exe";
            pi.Arguments = string.Format("{1}test-{0}.ll", index, OutputPath);

            var piProc = System.Diagnostics.Process.Start(pi);

            piProc.WaitForExit();

            Assert.AreEqual(0, piProc.ExitCode);
        }

        private void CompileAndRun(int index)
        {
            System.Diagnostics.Trace.WriteLine("Generating LLVM BC(ll) for " + index);

            Test(index);

            System.Diagnostics.Trace.WriteLine("Executing LLVM for " + index);

            /*
                call vcvars32.bat
                llc -mtriple i686-pc-win32 -filetype=obj mscorlib.ll
                llc -mtriple i686-pc-win32 -filetype=obj test-%1.ll
                link -defaultlib:libcmt -nodefaultlib:msvcrt.lib -nodefaultlib:libcd.lib -nodefaultlib:libcmtd.lib -nodefaultlib:msvcrtd.lib mscorlib.obj test-%1.obj /OUT:test-%1.exe
                del test-%1.obj
            */

            // to test working try/catch with g++ compilation
            // http://mingw-w64.sourceforge.net/download.php
            // Windows 32	DWARF	i686 - use this config to test exceptions on windows
            /*
                llc -mtriple i686-pc-mingw32 -filetype=obj mscorlib.ll
                llc -mtriple i686-pc-mingw32 -filetype=obj test-%1.ll
                g++.exe -o test-%1.exe mscorlib.o test-%1.o -lstdc++ -march=i686
                del test-%1.o
            */

            var pi = new ProcessStartInfo();
            pi.WorkingDirectory = OutputPath;
            pi.FileName = "ll.bat";
            pi.Arguments = string.Format("{0}", index);

            var piProc = System.Diagnostics.Process.Start(pi);

            piProc.WaitForExit();

            Assert.AreEqual(0, piProc.ExitCode);

            var piexec = new ProcessStartInfo();
            piexec.WorkingDirectory = OutputPath;
            piexec.FileName = string.Format("{1}test-{0}.exe", index, OutputPath);

            var piexecProc = System.Diagnostics.Process.Start(piexec);
            
            piexecProc.WaitForExit();

            Assert.AreEqual(0, piexecProc.ExitCode);

        }

        private void Test(int number)
        {
            Il2Converter.Convert(string.Concat(SourcePath, string.Format("test-{0}.cs", number)), OutputPath, new [] { "corelib:" + Path.GetFullPath(CoreLibPath) });
        }

        private void TestCustom(int number)
        {
            Il2Converter.Convert(string.Concat(SourcePathCustom, string.Format("test-{0}.cs", number)), OutputPath, new[] { "corelib:" + Path.GetFullPath(CoreLibPath) });
        }

        private void TestCustomCompileAndRun(int number)
        {
            Il2Converter.Convert(string.Concat(SourcePathCustom, string.Format("test-{0}.cs", number)), OutputPath, new[] { "corelib:" + Path.GetFullPath(CoreLibPath) });
        }

        private void TestGen(int number)
        {
            Il2Converter.Convert(string.Concat(SourcePath, string.Format("gtest-{0}.cs", number.ToString("000"))), OutputPath, new[] { "corelib:" + Path.GetFullPath(CoreLibPath) });
        }
    }
}

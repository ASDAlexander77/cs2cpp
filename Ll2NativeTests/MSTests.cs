namespace Ll2NativeTests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Il2Native.Logic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for MSTests
    /// </summary>
    [TestClass]
    public class MSTests
    {
#if _DISK_C_
        private const string SourcePath = @"C:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        private const string SourcePathCustom = @"C:\Temp\tests\";
        private const string OutputPath = @"C:\Temp\IlCTests\";
        private const string CoreLibPath = @"C:\Dev\Temp\Il2Native\CoreLib\bin\Release\CoreLib.dll";
#endif
#if _DISK_D_
        private const string SourcePath = @"D:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        private const string SourcePathCustom = @"D:\Temp\tests\";
        private const string OutputPath = @"D:\Temp\IlCTests\";
        private const string CoreLibPath = @"..\..\..\CoreLib\bin\Release\CoreLib.dll";
#endif

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
        public void TestCustomConvert()
        {
            Convert(1, SourcePathCustom, "test");
        }

        [TestMethod]
        public void TestCoreLib()
        {
            Il2Converter.Convert(Path.GetFullPath(CoreLibPath), OutputPath);
        }

        [TestMethod]
        public void TestCompileAndRunLlvm()
        {
            // 95 - init double in class to '0'
            // 99 - using GetType
            var skip = new int[] { 10, 19, 27, 28, 33, 36, 37, 39, 42, 43, 44, 45, 50, 52, 53, 57, 66, 67, 68, 74, 77, 83, 85, 91, 95, 99, 100, 101, 102 };
            foreach (var index in Enumerable.Range(1, 400).Where(n => !skip.Contains(n)))
            {
                CompileAndRun(index);
            }
        }

        [TestMethod]
        public void TestGenCompileAndRunLlvm()
        {
            // 95 - init double in class to '0'
            // 99 - using GetType
            var skip = new int[] { 0 };
            foreach (var index in Enumerable.Range(1, 400).Where(n => !skip.Contains(n)))
            {
                GenCompileAndRun(index);
            }
        }

        private void CompileAndRun(int index)
        {
            Trace.WriteLine("Generating LLVM BC(ll) for " + index);

            Convert(index);

            Trace.WriteLine("Executing LLVM for " + index);

            ExecCompile(index);
        }

        private void GenCompileAndRun(int index)
        {
            Trace.WriteLine("Generating LLVM BC(ll) for " + index);

            Convert(index, SourcePath, "gtest", "000");

            Trace.WriteLine("Executing LLVM for " + index);

            ExecCompile(index, "gtest", "000");
        }

        private static void ExecCompile(int index, string fileName = "test", string format = null)
        {
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
            pi.Arguments = string.Format("{1}-{0}", format == null ? index.ToString() : index.ToString(format), fileName);

            var piProc = Process.Start(pi);

            piProc.WaitForExit();

            Assert.AreEqual(0, piProc.ExitCode);

            var piexec = new ProcessStartInfo();
            piexec.WorkingDirectory = OutputPath;
            piexec.FileName = string.Format("{1}{2}-{0}.exe", format == null ? index.ToString() : index.ToString(format), OutputPath, fileName);

            var piexecProc = Process.Start(piexec);

            piexecProc.WaitForExit();

            Assert.AreEqual(0, piexecProc.ExitCode);
        }

        private void Convert(int number, string source = SourcePath, string fileName = "test", string format = null)
        {
            Il2Converter.Convert(
                string.Concat(source, string.Format("{1}-{0}.cs", format == null ? number.ToString() : number.ToString(format), fileName)), 
                OutputPath, 
                new[] { "corelib:" + Path.GetFullPath(CoreLibPath) });
        }
    }
}

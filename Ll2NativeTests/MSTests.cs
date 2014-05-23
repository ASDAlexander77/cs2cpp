namespace Ll2NativeTests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Il2Native.Logic;

    /// <summary>
    /// Summary description for MSTests
    /// </summary>
    [TestClass]
    public class MSTests
    {
        private const string SourcePath = @"D:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        private const string SourcePathCustom = @"D:\Temp\tests\";
        private const string OutputPath = @"D:\Temp\IlCTests\";

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
            Test(1);
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
        public void TestType()
        {
            Il2Converter.Convert(typeof(string), OutputPath);
            //Il2Converter.Convert(typeof(Type), OutputPath);
            //Il2Converter.Convert(typeof(object), OutputPath);
        }

        [TestMethod]
        public void TestCoreLib()
        {
            Il2Converter.Convert(@"D:\Temp\CorLib\bin\Release\CorLib.dll", OutputPath);
        }


        [TestMethod]
        public void TestRun()
        {
            // 9, 10 - Decimal class
            // 12 object class
            // 14 new keyword in function
            // 19 delegates
            // 26, 27 - delegate, order, imdelcared interfaces
            // 28 - object, hashtable
            // 30 - Explicit Interface
            // 33 - GetType()
            // 34, 35 - array of Objects
            // 36 - big mess with ldtoken etc
            // 37, 66 - array functionality
            // 38 - mess with arrays and 'Dup' commands
            // 43, 44, 45 - multi array
            // 46 - boxing/unboxing
            // 52 - arrays, enumerator
            // 57 UI classes (Button, EventHandler)
            // 59 - dynamic cast to Object
            // 61 - event, delegate

            // TO FIX:
            // 24  = = =  with Set/Get method, somehow it does not work. Priority now !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // 42  = = =  ++/-- mess
            // 49 string ops
            // 50, 67 missing file
            // 64 enum, namespace issue.
            // 66 simple multi arrays + static constructor
            // 68 ToString() + Console.WriteLine with 2 args
            // 69 extern function

            var skip = new int[] { 9, 10, 12, 14, 19, 24, 26, 27, 28, 30, 33, 34, 35, 36, 37, 38, 39, 40, 42, 43, 44, 45, 46, 49, 50, 52, 57, 59, 61, 64, 66, 67, 68, 69 };

            foreach (var index in Enumerable.Range(1, 400).Where(n => !skip.Contains(n)))
            {
                System.Diagnostics.Trace.WriteLine("Generating CPP for " + index);

                Test(index);

                System.Diagnostics.Trace.WriteLine("Compiling CPP for " + index);

                // compile and run test
                // ====== Natch content ====
                // call vcvars32.bat
                // del test-%1.exe
                // cl.exe test-%1.cpp
                // del test-%1.obj
                // test-%1.exe

                var pi = new ProcessStartInfo();
                pi.WorkingDirectory = OutputPath;
                pi.FileName = "c.bat";
                pi.Arguments = index.ToString();

                var piProc = System.Diagnostics.Process.Start(pi);

                piProc.WaitForExit();

                System.Diagnostics.Trace.WriteLine("Running EXE for " + index);

                // run test file
                var process = System.Diagnostics.Process.Start(string.Format("{1}test-{0}.exe", index, OutputPath));

                process.WaitForExit();

                Assert.AreEqual(0, process.ExitCode);
            }
        }

        [TestMethod]
        public void TestGenRun()
        {
            // 13, 17 - not compilable
            // 21 - string oper, GetType
            // 24 - boxing int
            // 25 - GetType
            // 28, 29 - use Object 
            // 30, 31 - can't be compiled

            var skip = new int[] { 13, 17, 21, 24, 25, 28, 29, 30, 31 };

            foreach (var index in Enumerable.Range(1, 472).Where(n => !skip.Contains(n)))
            {
                System.Diagnostics.Trace.WriteLine("Generating CPP for generic " + index);

                TestGen(index);

                System.Diagnostics.Trace.WriteLine("Compiling CPP for generic " + index);

                // compile and run test
                // ====== Natch content ====
                // call vcvars32.bat
                // del test-%1.exe
                // cl.exe test-%1.cpp
                // del test-%1.obj
                // test-%1.exe

                var pi = new ProcessStartInfo();
                pi.WorkingDirectory = OutputPath;
                pi.FileName = "cg.bat";
                pi.Arguments = index.ToString("000");

                var piProc = System.Diagnostics.Process.Start(pi);

                piProc.WaitForExit();

                System.Diagnostics.Trace.WriteLine("Running EXE for " + index);

                // run test file
                var process = System.Diagnostics.Process.Start(string.Format("{1}gtest-{0}.exe", index.ToString("000"), OutputPath));

                process.WaitForExit();

                Assert.AreEqual(0, process.ExitCode);
            }
        }

        [TestMethod]
        public void TestRunLlvm()
        {
            // 9, 10 - Decimal class
            var skip = new int[] { 9, 10, 12, 14, 15, 16, 18, 19, 20, 21, 26, 27, 28, 30, 32, 33, 34, 35, 36, 37, 39, 40, 42, 43, 44, 45, 46, 49, 50, 52, 53 };

            foreach (var index in Enumerable.Range(1, 400).Where(n => !skip.Contains(n)))
            {
                System.Diagnostics.Trace.WriteLine("Generating LLVM BC(il) for " + index);

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
        }

        private void Test(int number)
        {
            Il2Converter.Convert(string.Concat(SourcePath, string.Format("test-{0}.cs", number)), OutputPath);
        }

        private void TestCustom(int number)
        {
            Il2Converter.Convert(string.Concat(SourcePathCustom, string.Format("test-{0}.cs", number)), OutputPath);
        }

        private void TestGen(int number)
        {
            Il2Converter.Convert(string.Concat(SourcePath, string.Format("gtest-{0}.cs", number.ToString("000"))), OutputPath);
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MSTests.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Ll2NativeTests
{
    using System;
    using System.Collections.Generic;
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

        /// <summary>
        /// </summary>
        private const string SourcePath = @"C:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";

        /// <summary>
        /// </summary>
        private const string SourcePathCustom = @"C:\Temp\tests\";

        /// <summary>
        /// </summary>
        private const string OutputPath = @"C:\Temp\IlCTests\";

        /// <summary>
        /// </summary>
        private const string CoreLibPath = @"C:\Dev\Temp\Il2Native\CoreLib\bin\Release\CoreLib.dll";

        /// <summary>
        /// </summary>
        private const string OpenGlLibPath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonNativeCsLibraryForIl\bin\Release\BabylonNativeCsLibraryForIl.dll";

        /// <summary>
        /// </summary>
        private const string OpenGlExePath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonGlut\bin\Release\BabylonGlut.dll";

        /// <summary>
        /// </summary>
        private const string AndroidPath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonAndroid\bin\Android - Release\BabylonAndroid.dll";

        private const bool Llvm36Support = true;
#endif
#if _DISK_D_
        private const string SourcePath = @"D:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        private const string SourcePathCustom = @"D:\Temp\tests\";
        private const string OutputPath = @"D:\Temp\IlCTests\";
        private const string CoreLibPath = @"..\..\..\CoreLib\bin\Release\CoreLib.dll";
        private const string OpenGlLibPath = @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonNativeCsLibraryForIl\bin\Debug\BabylonNativeCsLibraryForIl.dll";
        private const string OpenGlExePath = @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonGlut\bin\Debug\BabylonGlut.dll";
        private const string AndroidPath = @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonAndroid\bin\Android - Release\BabylonAndroid.dll";

        private const bool Llvm36Support = false;
#endif

        /// <summary>
        /// </summary>
        private const bool UsingRoslyn = true;

        /// <summary>
        /// </summary>
        private const bool GcEnabled = true;

        /// <summary>
        /// </summary>
        private const bool GctorsEnabled = true;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        #endregion

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCustomConvert()
        {
            Convert(1, SourcePathCustom);
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCoreLib()
        {
            Il2Converter.Convert(Path.GetFullPath(CoreLibPath), OutputPath, GetConverterArgs(false));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestOpenGlLib()
        {
            // todo: 
            // 1) class Condition method _getEffectiveTarget when it is returning Object it does not cast Interface to an Object, to replicate the issue change returning type to Object
            // 2) the same as 1) but in InterpolateValueAction when saving value 'value = this._target[this._property]'
            Il2Converter.Convert(Path.GetFullPath(OpenGlLibPath), OutputPath, GetConverterArgs(true));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestOpenGlExe()
        {
            Il2Converter.Convert(Path.GetFullPath(OpenGlExePath), OutputPath, GetConverterArgs(true));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestAndroid()
        {
            Il2Converter.Convert(Path.GetFullPath(AndroidPath), OutputPath, GetConverterArgs(true));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCompile()
        {
            // test-400.cs 
            // %.r13 = load i32* %local1, align 4
            // %.r14 = load i32* %local1, align 4


            // 100 - using DllImport      
            // 251 - error CS0518: Predefined type 'System.Runtime.CompilerServices.IsVolatile' is not defined or imported
            // 294 - lock (Missing Monitor.Enter/Exit)
            // 300 - typeof of C[] (Array, will be fixed when using __Array__<T> implementation
            // 301 - typeof of Pointer type (*)
            // 304 - the same as 300
            // 305 - the same as 301
            // 324 - bug NEED TO BE FIXED.
            // 353 - does not have Main method
            // 444 - codepage 65001 is used (can't be compiled)
            // 524 - (Missing Monitor.Enter/Exit
            // 528 - using typeof(object[]) (Array, will be fixed when using __Array__<T> implementation
            // 550 - codepage 65001 is used (can't be compiled)
            // 551 - multiple definition of Int32 (but all issues are fixed)
            // 616 - test to compile Object (but it should be compiled without any Assembly reference)
            // 709 - get_OffsetStringData - required (NEED TO BE FIXED!!!!).
            var skip =
                new List<int>(
                    new[]
                        {
                            100, 251, 294, 300, 301, 304, 305, 353, 444, 482, 524, 528, 550, 551, 616, 709
                        });

            Debug.Listeners.Clear();

            // last 790
            foreach (var index in Enumerable.Range(1, 907).Where(n => !skip.Contains(n)))
            {
                Compile(index);
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCompileAndRunLlvm()
        {
            // 1) !!! NEED TO BE FIXED, Issue: dynamic_cast of a Struct
            // file in sscli20 co1367catch_block.cs can't be compiled (mismatch of types)

            // 10 - not compilable
            // 19 - using Thread class, Reflection
            // 28 - bug in execution (Hashtable)
            // 32 - multi array
            // 36 - bug in execution (NotImplemented)
            // 37 - multi array
            // 39 - using Attributes
            // 43 - multi array
            // 50 - missing
            // 52 - bug in execution (NotImplemented, ArrayList, Hashtable)
            // 57 - bug in execution (NotImplemented, EventHandler)
            // 67 - missing
            // 68 - using enum
            // 74 - using StreamReader
            // 77 - using enum
            // 85 - using UnmanagedType
            // 91 - using Reflection
            // 95 - NEED TO BE FIXED, init double in a class
            // 99 - using GetType
            // 100 - using DllImport      
            // 101 - using Reflection
            // 102 - using Reflection
            // 105 - IAsyncResult (NotImplemented)
            // 106 - IAsyncResult (NotImplemented) (missing)
            // 109 - DateTime.Now.ToString (NotImplemented)
            // 117 - not implemented Hashtable
            // 118 - not implemented Attribute
            // 120 - not implemented Attribute
            // 127 - IsDerined not implemented
            // 128 - using Attributes
            // 130 - not compilable (Debug Trace: (24,20): error CS0037: Cannot convert null to 'System.IntPtr' because it is a non-nullable value type)
            // 132 - Reflection
            // 135 - Reflection
            // 149 - Delegate.Combine (NotImplemented)
            // 157 - reflection, attributes
            // 158 - reflection, attributes
            // 171 - Roslyn can't handle it!!!
            // 174 - can't be compiled (21,3): error CS0103: The name 'comparer' does not exist in the current context
            // 177 - using Reflection
            // 178 - using Reflection
            // 180 - not compilable (9,38): error CS1503: Argument 1: cannot convert from 'System.Enum' to 'string'
            // 181 - using Reflection
            // 183 - using BeginInvoke
            // 187 - using Specialized Collections
            // 207 - Delegate.Combine (NotImplemented)
            // 209 - Delegate.Combine (NotImplemented)
            // 216 - Delegate.Combine (NotImplemented)
            // 219 - can't be compiled (22,26): error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes' and no extension method 'GetCustomAttributes' accepting a first argument of type 'System.Type' could be found (are you missing a using directive or an assembly reference?)
            // 220 - can't be compiled (8,26): error CS0234: The type or namespace name 'Specialized' does not exist in the namespace 'System.Collections' (are you missing an assembly reference?)
            // 229 - can't be compiled (3,26): error CS0234: The type or namespace name 'Specialized' does not exist in the namespace 'System.Collections' (are you missing an assembly reference?)
            // 230 - using Reflection
            // 231 - NEED TO BE FIXED (when "this" is null. it should throw an error (Null Reference)
            var skip =
                new List<int>(
                    new[]
                        {
                            10, 19, 28, 36, 39, 50, 52, 53, 57, 67, 68, 85, 91, 95, 99, 100, 101, 102, 105, 106, 107, 109, 115, 117, 118, 120,
                            127, 128, 130, 132, 135, 149, 157, 158, 171, 174, 177, 178, 180, 181, 183, 187, 207, 209, 216, 219, 220, 229, 230,
                            231
                        });

            if (UsingRoslyn)
            {
                // 49 - bug in execution
                // object o = -(2147483648); type is "int", not "long" in Roslyn
                skip.AddRange(new[] { 49, 129 });
            }

            foreach (var index in Enumerable.Range(229, 869).Where(n => !skip.Contains(n)))
            {
                CompileAndRun(index);
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestGenCompileAndRunLlvm()
        {
            // 40 - using T name in nested generic type which causes mess (not main concern now), Debug Trace: (46,19): warning CS0693: Type parameter 'T' has the same name as the type parameter from outer type 'Stack<T>'
            // 46 - Delegate.Combine not implemented
            // 47 - not compilable
            // 51 - bug in execution (NotImplemented)
            // 52 - using new() (NEED TO BE FIXED), Debug Trace: (9,10): error CS0656: Missing compiler required member 'System.Activator.CreateInstance'
            // 56 - bug in execution (NotImplemented)
            // 63 - Array.Length is not implemented
            // 65 - can't be compiled yet, Debug Trace: (39,22): error CS0311: The type 'string' cannot be used as type parameter 'T' in the generic type or method 'ComparablePair<T, U>'. There is no implicit reference conversion from 'string' to 'System.IComparable<string>'.
            // 66 - using typeof (typeof (Foo<>))
            // 72 - not implemented (DateTime to string)
            // 77 - file not found
            // 78 - not implemented
            // 96 - NEED TO BE FIXED: generic methods nested referencing
            // 99 - file not found
            // 102 - can't be compiled, Debug Trace: (18,5): error CS0315: The type 'int' cannot be used as type parameter 'T' in the generic type or method 'A<T>'. There is no boxing conversion from 'int' to 'System.IComparable'.
            // 109 - can't be compiled, Debug Trace: error CS0117: 'System.Array' does not contain a definition for 'Resize'
            // 110 - as 96 but additionally virtual and interface
            var skip = new[] { 40, 46, 47, 51, 52, 56, 63, 65, 66, 72, 77, 78, 96, 99, 102, 109, 110 };
            foreach (var index in Enumerable.Range(1, 400).Where(n => !skip.Contains(n)))
            {
                GenCompileAndRun(index);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="includeCoreLib">
        /// </param>
        /// <param name="roslyn">
        /// </param>
        /// <returns>
        /// </returns>
        private static string[] GetConverterArgs(bool includeCoreLib, bool roslyn = UsingRoslyn, bool gc = GcEnabled, bool gctors = GctorsEnabled, bool llvm36Support = Llvm36Support)
        {
            var args = new List<string>();
            if (includeCoreLib)
            {
                args.Add("corelib:" + Path.GetFullPath(CoreLibPath));
            }

            if (roslyn)
            {
                args.Add("roslyn");
            }

            if (!gc)
            {
                args.Add("gc-");
            }

            if (!gctors)
            {
                args.Add("gctors-");
            }

            if (llvm36Support)
            {
                args.Add("llvm36");
            }

            return args.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <param name="fileName">
        /// </param>
        /// <param name="format">
        /// </param>
        /// <param name="justCompile">
        /// </param>
        private static void ExecCompile(int index, string fileName = "test", string format = null, bool justCompile = false)
        {
            /*
                call vcvars32.bat
                llc -mtriple i686-pc-win32 -filetype=obj corelib.ll
                llc -mtriple i686-pc-win32 -filetype=obj test-%1.ll
                link -defaultlib:libcmt -nodefaultlib:msvcrt.lib -nodefaultlib:libcd.lib -nodefaultlib:libcmtd.lib -nodefaultlib:msvcrtd.lib corelib.obj test-%1.obj /OUT:test-%1.exe
                del test-%1.obj
            */

            // to test working try/catch with g++ compilation
            // http://mingw-w64.sourceforge.net/download.php
            // Windows 32	DWARF	i686 - use this config to test exceptions on windows
            // GC - http://www.hboehm.info/gc/ (use git and cmake to compile libgc-lib.a file
            /*
                llc -mtriple i686-pc-mingw32 -filetype=obj corelib.ll
                llc -mtriple i686-pc-mingw32 -filetype=obj test-%1.ll
                g++.exe -o test-%1.exe corelib.o test-%1.o -lstdc++ -march=i686
                del test-%1.o
            */

            // if GC Enabled
            /*
                llc -mtriple i686-pc-mingw32 -filetype=obj corelib.ll
                llc -mtriple i686-pc-mingw32 -filetype=obj test-%1.ll
                g++.exe -o test-%1.exe corelib.o test-%1.o -lstdc++ -lgc-lib -march=i686 -L .
                del test-%1.o 
             */

            // Android target - target triple = "armv7-none-linux-androideabi"

            var pi = new ProcessStartInfo();
            pi.WorkingDirectory = OutputPath;
            pi.FileName = "ll.bat";
            pi.Arguments = string.Format("{1}-{0}", format == null ? index.ToString() : index.ToString(format), fileName);

            var process = Process.Start(pi);

            process.WaitForExit();

            Assert.AreEqual(0, process.ExitCode);

            if (!justCompile)
            {
                var execProcess = new ProcessStartInfo();
                execProcess.WorkingDirectory = OutputPath;
                execProcess.FileName = string.Format("{1}{2}-{0}.exe", format == null ? index.ToString() : index.ToString(format), OutputPath, fileName);

                var execProcessProc = Process.Start(execProcess);

                execProcessProc.WaitForExit();

                Assert.AreEqual(0, execProcessProc.ExitCode);
            }
            else
            {
                Assert.IsTrue(File.Exists(Path.Combine(OutputPath, string.Format("{1}{2}-{0}.exe", format == null ? index.ToString() : index.ToString(format), OutputPath, fileName))));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        private static void Compile(int index)
        {
            Trace.WriteLine("Generating LLVM BC(ll) for " + index);

            try
            {
                Convert(index);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return;
            }

            Trace.WriteLine("Compiling LLVM for " + index);

            ExecCompile(index, "test", null, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        private static void CompileAndRun(int index)
        {
            Trace.WriteLine("Generating LLVM BC(ll) for " + index);

            Convert(index);

            Trace.WriteLine("Compiling/Executing LLVM for " + index);

            ExecCompile(index);
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="fileName">
        /// </param>
        /// <param name="format">
        /// </param>
        private static void Convert(int number, string source = SourcePath, string fileName = "test", string format = null)
        {
            Il2Converter.Convert(
                string.Concat(source, string.Format("{1}-{0}.cs", format == null ? number.ToString() : number.ToString(format), fileName)),
                OutputPath,
                GetConverterArgs(true));
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        private static void GenCompileAndRun(int index)
        {
            Trace.WriteLine("Generating LLVM BC(ll) for " + index);

            Convert(index, SourcePath, "gtest", "000");

            Trace.WriteLine("Executing LLVM for " + index);

            ExecCompile(index, "gtest", "000");
        }
    }
}
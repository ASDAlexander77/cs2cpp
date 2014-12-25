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
    using System.Runtime.ExceptionServices;
    using System.Runtime.InteropServices;
    using System.Threading;

    using Il2Native.Logic;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PdbReader;

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
        private const string CoreLibPdbPath = @"C:\Dev\Temp\Il2Native\CoreLib\bin\Release\CoreLib.pdb";
        private const string OpenGlLibPath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonNativeCsLibraryForIl\bin\Release\BabylonNativeCsLibraryForIl.dll";
        private const string OpenGlExePath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonGlut\bin\Release\BabylonGlut.dll";
        private const string AndroidPath = @"C:\Dev\BabylonNative\BabylonNativeCs\BabylonAndroid\bin\Android - Release\BabylonAndroid.dll";

        private const bool Llvm35Support = false;
        private const bool Llvm34Support = false;
        private const string OutputObjectFileExt = "obj";
        private const string Target = "i686-w64-mingw32";
#endif
#if _DISK_D_
        private const string SourcePath = @"D:\Temp\CSharpTranspilerExt\Mono-Class-Libraries\mcs\tests\";
        private const string SourcePathCustom = @"D:\Temp\tests\";
        private const string OutputPath = @"D:\Temp\IlCTests\";
        private const string CoreLibPath = @"..\..\..\CoreLib\bin\Release\CoreLib.dll";
        private const string CoreLibPdbPath = @"..\..\..\CoreLib\bin\Release\CoreLib.pdb";
        private const string OpenGlLibPath = @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonNativeCsLibraryForIl\bin\Debug\BabylonNativeCsLibraryForIl.dll";
        private const string OpenGlExePath = @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonGlut\bin\Debug\BabylonGlut.dll";
        private const string AndroidPath = @"D:\Developing\BabylonNative\BabylonNativeCs\BabylonAndroid\bin\Android - Release\BabylonAndroid.dll";
        private const string SscliSourcePath = @"D:\Temp\CSharpTranspilerExt\sscli20\tests\bcl\system\";

        private const bool Llvm35Support = false;
        private const bool Llvm34Support = false;
        private const string OutputObjectFileExt = "obj";
        private const string Target = "i686-w64-mingw32";
#endif

        /// <summary>
        /// </summary>
        private const bool Android = false;

        /// <summary>
        /// </summary>
        private const bool Emscripten = false;

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
        /// </summary>
        private const bool DebugInfo = false;

        /// <summary>
        /// </summary>
        private const bool MultiCore = true;

        /// <summary>
        /// ex. opt 'file'.ll -o 'file'.bc -O2
        /// </summary>
        private const bool CompileWithOptimization = false;

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
            Convert("test-1", SourcePathCustom);
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestMscorlibCompile()
        {
            //Debug.Listeners.Clear();
            Il2Converter.Convert(Path.GetFullPath(@"C:\Windows\Microsoft.NET\assembly\GAC_32\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll"), OutputPath, GetConverterArgs(false));
        }

        /// </summary>
        [TestMethod]
        public void TestSscli()
        {
            foreach (var file in Directory.EnumerateFiles(SscliSourcePath, "*.cs", SearchOption.AllDirectories))
            {
                CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", true);
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCoreLib()
        {
            Il2Converter.Convert(Path.GetFullPath(CoreLibPath), OutputPath, GetConverterArgs(false));

            if (CompileWithOptimization)
            {
                ExecCmd("opt", "CoreLib.ll -o CoreLib.bc -O2");
                ExecCmd("llc", string.Format("-filetype=obj -mtriple={0} CoreLib.bc", Target));
            }
            else
            {
                ExecCmd("llc", string.Format("-filetype=obj -mtriple={0} CoreLib.ll", Target));
            }
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
            // TODO: WARNING! order of obj files in g++ cmd line is important, CoreLib.obj should be last one

            // 10 - Double conversion (in CoreLib.dll some conversions are missing)
            // 100 - using DllImport      
            // 251 - error CS0518: Predefined type 'System.Runtime.CompilerServices.IsVolatile' is not defined or imported
            // 300 - typeof of C[] (Array, will be fixed when using __Array__<T> implementation
            // 301 - typeof of Pointer type (*)
            // 304 - the same as 300
            // 305 - the same as 301
            // 324 - bug NEED TO BE FIXED.
            // 353 - does not have Main method
            // 386 - Double conversion (in CoreLib.dll some conversions are missing)
            // 387 - Decimal conversion (in CoreLib.dll some conversions are missing)
            // 444 - codepage 65001 is used (can't be compiled)
            // 528 - using typeof(object[]) (Array, will be fixed when using __Array__<T> implementation
            // 535 - IntPtr conversion (in CoreLib.dll some conversions are missing)
            // 550 - codepage 65001 is used (can't be compiled)
            // 551 - multiple definition of Int32 (but all issues are fixed)
            // 616 - test to compile Object (but it should be compiled without any Assembly reference)
            // 631 - missing System_Decimal__op_UnaryNegation (the same issue as 596)
            // 709 - get_OffsetStringData - required (NEED TO BE FIXED!!!!).
            // 817 - redefinition of Int32
            // 864 - Decimal conversion (in CoreLib.dll some conversions are missing)
            var skip =
                new List<int>(
                    new[]
                        {
                            10, 100, 251, 300, 301, 304, 305, 353, 386, 387, 444, 482, 528, 535, 550, 551, 616, 631, 709, 817, 864
                        });

            Debug.Listeners.Clear();

            foreach (var index in Enumerable.Range(1, 907).Where(n => !skip.Contains(n)))
            {
                Compile(string.Format("test-{0}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestGenCompile()
        {
            // 66 - using typeof (typeof (Foo<>))

            var skip =
                new List<int>(
                    new[]
                        { 
                            66
                        });

            Debug.Listeners.Clear();

            foreach (var index in Enumerable.Range(1, 589).Where(n => !skip.Contains(n)))
            {
                Compile(string.Format("gtest-{0:000}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCompileAndRunLlvm()
        {
            // Bug of using struct references instead of copying them into stack (following example has a problem because value from Code.Ldloc1 overwriting the value 
            // of Code.Ldloc2 after storing value into Code.Ldloc1, we would not have an issue if we have copied the value of Code.Ldloc1 into stack and then read it later
            /*
             	static int Test ()
	            {
		            int? a = 5;
		            int? b = 5;

		            var d = b++ + ++a;

		            Console.WriteLine(a);		
		            Console.WriteLine(b);		
		            Console.WriteLine(d);		

		            return 0;
	            }
             */

            // 10 - Double conversion (in CoreLib.dll some conversions are missing)
            // 19 - using Thread class, Reflection
            // 32 - multi array
            // 36 - bug in execution (NotImplemented)
            // 37 - multi array
            // 39 - using Attributes
            // 43 - multi array
            // 50 - missing
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
            // 107 - IAsyncResult (NotImplemented)
            // 109 - DateTime.Now.ToString (NotImplemented)
            // 118 - not implemented Attribute
            // 120 - not implemented Attribute
            // 126 - calling ToString on Interface, (CONSIDER FIXING IT)
            // 127 - IsDerived not implemented
            // 128 - using Attributes
            // 132 - Reflection
            // 135 - Reflection
            // 157 - reflection, attributes
            // 158 - reflection, attributes
            // 174 - can't be compiled (21,3): error CS0103: The name 'comparer' does not exist in the current context
            // 177 - using Reflection
            // 178 - using Reflection
            // 180 - not compilable (9,38): error CS1503: Argument 1: cannot convert from 'System.Enum' to 'string'
            // 181 - using Reflection
            // 183 - using BeginInvoke
            // 187 - using Specialized Collections
            // 219 - can't be compiled (22,26): error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes' and no extension method 'GetCustomAttributes' accepting a first argument of type 'System.Type' could be found (are you missing a using directive or an assembly reference?)
            // 220 - can't be compiled (8,26): error CS0234: The type or namespace name 'Specialized' does not exist in the namespace 'System.Collections' (are you missing an assembly reference?)
            // 229 - can't be compiled (3,26): error CS0234: The type or namespace name 'Specialized' does not exist in the namespace 'System.Collections' (are you missing an assembly reference?)
            // 230 - using Reflection
            // 231 - NEED TO BE FIXED (when "this" is null. it should throw an error (Null Reference)
            // 232 - missing IConvertable
            // 233 - Reflection
            // 236 - Attributes
            // 238 - can't be compiled (5,10): error CS0246: The type or namespace name 'Conditional' could not be found (are you missing a using directive or an assembly reference?)
            // 239 - can't be compiled (5,10): error CS0246: The type or namespace name 'Conditional' could not be found (are you missing a using directive or an assembly reference?)
            // 240 - the same as 239
            // 247 - ArrayList - GetEnumator is not implemented
            // 250 - FieldsOffset attribute not implemented
            // 253 - System.Reflection
            // 254 - System.Reflection 
            // 263 - string with sbyte*
            // 266 - IConvertable 
            // 269 - ArgIterator
            // 273 - GetCustomAttributes
            // 276 - GetType.GetEvents(); (NotImplemented)
            // 279 - Enum ToString with Flags
            // 282 - error:  error CS1502: The best overloaded method match for 'System.Convert.ToDouble(string)' has some invalid arguments,  error CS1503: Argument 1: cannot convert from 'int' to 'string'
            // 286 - Xml (not implemented)
            // 287 - System.Type, GetConstructors, IsSealed
            // 295 - System.Reflection
            // 296 - GetElementType (NotImplemented)
            // 297 - System.Reflection
            // 300 - typeof(C[]) - will be fixed when __Array__<T> applied
            // 301 - typeof(x*) - type of pointer
            // 304 - typeof(C[]) - will be fixed when __Array__<T> applied
            // 305 - typeof(x*) - type of pointer
            // 308 - typeof(x*) - type of pointer
            // 311 - SecurityPermission
            // 313 - typeof(D).GetMethods - NotImplemented
            // 318 - EventHandlerList error CS0246: The type or namespace name 'EventHandlerList' could not be found (are you missing a using directive or an assembly reference?)
            // 319 - missing DecimalConstantAttribute
            // 329 - GetCustromAttributes
            // 330 - int to IFormattable
            // 349 - TypeAttributes
            // 352 - MarshalAs
            // 353 - no Main()
            // 358 - missing implementation of ToString('R') for double
            // 361 - missing Attribute
            // 362 - cycling Catch/Throw bug (NEED TO BE FIXED!!!)
            // 367 - GetFields not implemented
            // -----------
            // 32, 55, 74 - missing class
            // 37, 42, 43, 44, 45, 66 - multiarray
            // 77 - enum to string
            var skip =
                new List<int>(
                    new[]
                        {
                            10, 19, 32, 36, 37, 39, 42, 43, 44, 45, 49, 50, 53, 55, 66, 67, 68, 74, 77, 85, 91, 95, 99, 100, 101, 102, 105, 106, 107, 109, 115, 118, 120,
                            126, 127, 128, 132, 135, 157, 158, 174, 177, 178, 180, 181, 183, 187, 219, 220, 229, 230, 231, 232, 233, 236, 238, 239, 240, 
                            247, 250, 253, 254, 263, 266, 269, 273, 276, 279, 282, 286, 287, 295, 296, 297, 300, 301, 304, 305, 308, 311, 313, 318, 319, 329, 330,
                            349, 352, 353, 358, 361, 362, 367
                        });

            if (UsingRoslyn)
            {
                // object o = -(2147483648); type is "int", not "long" in Roslyn
                skip.AddRange(new[] { 129 });
            }

            foreach (var index in Enumerable.Range(1, 906).Where(n => !skip.Contains(n)))
            {
                CompileAndRun(string.Format("test-{0}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestGenCompileAndRunLlvm()
        {
            // 40 - using T name in nested generic type which causes mess (not main concern now), Debug Trace: (46,19): warning CS0693: Type parameter 'T' has the same name as the type parameter from outer type 'Stack<T>'
            // 47 - not compilable
            // 51 - bug in execution (NotImplemented)
            // 56 - bug in execution (NotImplemented)
            // 65 - can't be compiled yet, Debug Trace: (39,22): error CS0311: The type 'string' cannot be used as type parameter 'T' in the generic type or method 'ComparablePair<T, U>'. There is no implicit reference conversion from 'string' to 'System.IComparable<string>'.
            // 66 - using typeof (typeof (Foo<>))
            // 72 - not implemented (DateTime to string)
            // 77 - file not found
            // 78 - not implemented
            // 99 - file not found
            // 102 - can't be compiled, Debug Trace: (18,5): error CS0315: The type 'int' cannot be used as type parameter 'T' in the generic type or method 'A<T>'. There is no boxing conversion from 'int' to 'System.IComparable'.
            // 109 - can't be compiled, Debug Trace: error CS0117: 'System.Array' does not contain a definition for 'Resize'
            // 117 - "xxx is int[]" treated as "xxx is int": NEED TO BE FIXED (when __Array__<T> is used)
            // 119 - typeof pointer
            // 128 - Reflection
            // 143 - BIG BUG with using "++" on structures due to using struct references instead of using copied object in stack
            // 144 - cast string[] to IEnumerable<string> (not yet supported. NEED TO BE FIXED (when __Array__<T> is used)
            // 145 - using multiarray
            // 156 - can't compile (seems it is lib)
            // 161 - can't compile (seems it is lib)
            // 162 - GetType. findMember
            // 165 - cant be compiled (library)
            // 166 - cant be compiled (library)
            // 167 - Attribute.GetCustomAttributes
            // 171 - multiarray
            // 172 - cant be compiled (library)
            // 174 - cant be compiled (library)
            // 177 - cast IEnumerable<T> from Array
            // 180 - Attributes
            // 184 - Array.FindAll not implemented
            // 186 - Serialization, FileStream etc not implemented

            // 13, 17, 31, 47, 98 - with Libs
            // 53 - ValueType.ToString() not implemented

            var skip = new[]
                           {
                               13, 17, 31, 40, 47, 51, 53, 56, 65, 66, 72, 77, 78, 98, 99, 102, 109, 117, 119, 128, 143, 144, 145, 156, 
                               161, 162, 165, 166, 167, 171, 172, 174, 177, 180, 184, 186
                           };
            foreach (var index in Enumerable.Range(1, 400).Where(n => !skip.Contains(n)))
            {
                CompileAndRun(string.Format("gtest-{0:000}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestPdbReader()
        {
            Converter.Convert(CoreLibPdbPath, new DummySymbolWriter.DummySymbolWriter());
        }

        /// <summary>
        /// </summary>
        /// <param name="includeCoreLib">
        /// </param>
        /// <param name="roslyn">
        /// </param>
        /// <returns>
        /// </returns>
        private static string[] GetConverterArgs(bool includeCoreLib, bool roslyn = UsingRoslyn, bool gc = GcEnabled, bool gctors = GctorsEnabled, bool llvm35Support = Llvm35Support, bool llvm34Support = Llvm34Support, bool debugInfo = DebugInfo)
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

            if (llvm35Support)
            {
                args.Add("llvm35");
            }
            else if (llvm34Support)
            {
                args.Add("llvm34");
            }

            if (debugInfo)
            {
                args.Add("debug");
            }

            if (MultiCore)
            {
                args.Add("multi");
            }

            if (Android)
            {
                args.Add("android");
            }

            if (Emscripten)
            {
                args.Add("emscripten");
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
        private static void ExecCompile(string fileName, bool justCompile = false, bool opt = false)
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

            // if GC Enabled with optimization
            /*
                opt corelib.ll -o corelib.bc -O2
                opt test-%1.ll -o test-%1.bc -O2
                llc -mtriple i686-pc-mingw32 -filetype=obj corelib.bc
                llc -mtriple i686-pc-mingw32 -filetype=obj test-%1.bc
                g++.exe -o test-%1.exe corelib.o test-%1.o -lstdc++ -lgc-lib -march=i686 -L .
                del test-%1.o 
             */

            // Android target - target triple = "armv7-none-linux-androideabi"

            // compile CoreLib
            if (!File.Exists(Path.Combine(OutputPath, string.Concat("CoreLib.", OutputObjectFileExt))))
            {
                if (!File.Exists(Path.Combine(OutputPath, "CoreLib.ll")))
                {
                    Il2Converter.Convert(Path.GetFullPath(CoreLibPath), OutputPath, GetConverterArgs(false));
                }

                if (opt)
                {
                    ExecCmd("opt", "CoreLib.ll -o CoreLib.bc -O2");
                    ExecCmd("llc", string.Format("-filetype=obj -mtriple={0} CoreLib.bc", Target));
                }
                else
                {
                    ExecCmd("llc", string.Format("-filetype=obj -mtriple={0} CoreLib.ll", Target));
                }
            }

            // file obj
            if (opt)
            {
                ExecCmd("opt", string.Format("{0}.ll -o {0}.bc -O2", fileName));
                ExecCmd("llc", string.Format("-filetype=obj -mtriple={1} {0}.bc", fileName, Target));
            }
            else
            {
                ExecCmd("llc", string.Format("-filetype=obj -mtriple={1} {0}.ll", fileName, Target));
            }

            if (!justCompile)
            {
                // file exe
                ExecCmd("g++", string.Format("-o {0}.exe {0}.{1} CoreLib.{1} -lstdc++ -lgc-lib -march=i686 -L .", fileName, OutputObjectFileExt));

                // test execution
                ExecCmd(string.Format("{0}.exe", fileName));
            }
            else
            {
                Assert.IsTrue(File.Exists(Path.Combine(OutputPath, string.Format("{0}{1}.{2}", OutputPath, fileName, OutputObjectFileExt))));
            }
        }

        private static void ExecCmd(string fileName, string arguments = "", string workingDir = OutputPath)
        {
            var processStartInfo = new ProcessStartInfo();
            processStartInfo.WorkingDirectory = workingDir;
            processStartInfo.FileName = fileName;
            processStartInfo.Arguments = arguments;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            var processCoreLibObj = Process.Start(processStartInfo);
            processCoreLibObj.WaitForExit();
            Assert.AreEqual(0, processCoreLibObj.ExitCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        private static void Compile(string fileName, string source = SourcePath)
        {
            Trace.WriteLine("Generating LLVM BC(ll) for " + fileName);

            try
            {
                Convert(fileName, source);
            }
            catch (BadImageFormatException ex)
            {
                Debug.WriteLine(ex);
                return;
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine(ex);
                return;
            }

            Trace.WriteLine("Compiling LLVM for " + fileName);

            ExecCompile(fileName, justCompile: true);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        private static void CompileAndRun(string fileName, string source = SourcePath, bool ignoreBadFiles = false)
        {
            Trace.WriteLine("Generating LLVM BC(ll) for " + fileName);

            if (!ignoreBadFiles)
            {
                Convert(fileName, source);
            }
            else
            {
                try
                {
                    Convert(fileName, source);
                }
                catch (BadImageFormatException ex)
                {
                    Debug.WriteLine(ex);
                    return;
                }
                catch (FileNotFoundException ex)
                {
                    Debug.WriteLine(ex);
                    return;
                }
            }

            Trace.WriteLine("Compiling/Executing LLVM for " + fileName);

            ExecCompile(fileName, opt: CompileWithOptimization);
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
        private static void Convert(string fileName, string source = SourcePath)
        {
            Il2Converter.Convert(
                string.Concat(source, string.Format("{0}.cs", fileName)),
                OutputPath,
                GetConverterArgs(true));
        }
    }
}
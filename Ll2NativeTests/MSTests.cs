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
    using System.Threading.Tasks;

    using Il2Native.Logic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using PEAssemblyReader;

    using PdbReader;

    /// <summary>
    ///     Summary description for MSTests
    /// </summary>
    [TestClass]
    public class MSTests
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void GenerateTestFromMonoTests()
        {
            Debug.WriteLine(@"namespace Ll2NativeTests {");
            Debug.WriteLine(@"using System;");
            Debug.WriteLine(@"using System.Collections.Generic;");
            Debug.WriteLine(@"using System.Diagnostics;");
            Debug.WriteLine(@"using System.IO;");
            Debug.WriteLine(@"using System.Linq;");
            Debug.WriteLine(@"using Il2Native.Logic;");
            Debug.WriteLine(@"using Microsoft.VisualStudio.TestTools.UnitTesting;");
            Debug.WriteLine(@"using PdbReader;");

            var chars = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            var currentDir = "";
            var currentNamespace = "";
            var enumerateFiles =
                Directory.EnumerateFiles(CompilerHelper.SourcePath, "*.cs", SearchOption.AllDirectories).ToArray();
            Array.Sort(enumerateFiles);
            foreach (var file in enumerateFiles)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                var directoryName = Path.GetDirectoryName(file);
                var folderName = Path.GetFileName(directoryName);
                var subfolders =
                    directoryName.Substring(Math.Min(directoryName.Length, CompilerHelper.SourcePath.Length));
                var fileName = Path.GetFileName(file);

                // custom for Mono only
                string groupName;
                var minusIndex = fileName.IndexOfAny(chars);
                if (minusIndex > 0)
                {
                    groupName = fileName.Substring(0, minusIndex - 1);
                    if (fileName.IndexOf('-', minusIndex) >= 0)
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }

                var namespaceChain = subfolders + groupName;
                var groupper = directoryName + namespaceChain;

                if (currentDir != groupper)
                {
                    if (!string.IsNullOrEmpty(currentDir))
                    {
                        Debug.WriteLine(@"}");
                        Debug.WriteLine(@"");
                    }

                    if (currentNamespace != namespaceChain)
                    {
                        if (!string.IsNullOrEmpty(currentNamespace))
                        {
                            Debug.WriteLine(@"}");
                            Debug.WriteLine(@"");
                        }

                        Debug.WriteLine(@"namespace @" + namespaceChain.Replace("\\", ".@").Replace("-", "_") + " {");
                        currentNamespace = namespaceChain;
                    }

                    Debug.WriteLine(@"[TestClass]");
                    Debug.WriteLine(@"public class @" + folderName.Replace("-", "_") + " {");
                    Debug.WriteLine(@"[TestInitialize]");
                    Debug.WriteLine(@"public void Initialize() { ");
                    Debug.WriteLine(@"CompilerHelper.AssertUiEnabled(false);");
                    Debug.WriteLine(@"}");
                    Debug.WriteLine(@"");
                    Debug.WriteLine(@"[TestCleanup]");
                    Debug.WriteLine(@"public void Cleanup() { ");
                    Debug.WriteLine(@"CompilerHelper.AssertUiEnabled(true);");
                    Debug.WriteLine(@"}");
                    Debug.WriteLine(@"");

                    currentDir = groupper;
                }

                Debug.WriteLine(@"[TestMethod]");
                var testMethodName = fileNameWithoutExtension.Replace("-", "_");
                Debug.WriteLine(@"public void @" + testMethodName + "() {");
                Debug.WriteLine(
                    @"var file = Path.Combine(CompilerHelper.SourcePath, @""" + subfolders + @""", """ + fileName +
                    @""");");
                Debug.WriteLine(
                    @"CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + ""\\"", false, true);");
                Debug.WriteLine(@"}");
                Debug.WriteLine(@"");
            }

            Debug.WriteLine(@"}"); // class
            Debug.WriteLine(@"}"); // namespaces
            Debug.WriteLine(@"}"); // global namespace
        }

        [TestMethod]
        public void GenerateTestFromSscliTests()
        {
            Debug.WriteLine(@"namespace Ll2NativeTests {");
            Debug.WriteLine(@"using System;");
            Debug.WriteLine(@"using System.Collections.Generic;");
            Debug.WriteLine(@"using System.Diagnostics;");
            Debug.WriteLine(@"using System.IO;");
            Debug.WriteLine(@"using System.Linq;");
            Debug.WriteLine(@"using Il2Native.Logic;");
            Debug.WriteLine(@"using Microsoft.VisualStudio.TestTools.UnitTesting;");
            Debug.WriteLine(@"using PdbReader;");

            var currentDir = "";
            var currentNamespace = "";
            foreach (
                var file in
                    Directory.EnumerateFiles(CompilerHelper.SscliSourcePath, "*.cs", SearchOption.AllDirectories))
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                var directoryName = Path.GetDirectoryName(file);
                var folderName = Path.GetFileName(directoryName);
                var subfolders = directoryName.Substring(CompilerHelper.SscliSourcePath.Length);

                if (currentDir != directoryName)
                {
                    if (!string.IsNullOrEmpty(currentDir))
                    {
                        Debug.WriteLine(@"}");
                        Debug.WriteLine(@"");
                    }

                    if (currentNamespace != subfolders)
                    {
                        if (!string.IsNullOrEmpty(currentNamespace))
                        {
                            Debug.WriteLine(@"}");
                            Debug.WriteLine(@"");
                        }

                        Debug.WriteLine(@"namespace @" + subfolders.Replace("\\", ".@") + " {");
                        currentNamespace = subfolders;
                    }

                    Debug.WriteLine(@"[TestClass]");
                    Debug.WriteLine(@"public class @" + folderName + " {");
                    Debug.WriteLine(@"[TestInitialize]");
                    Debug.WriteLine(@"public void Initialize() { ");
                    Debug.WriteLine(@"CompilerHelper.AssertUiEnabled(false);");
                    Debug.WriteLine(@"}");
                    Debug.WriteLine(@"");
                    Debug.WriteLine(@"[TestCleanup]");
                    Debug.WriteLine(@"public void Cleanup() { ");
                    Debug.WriteLine(@"CompilerHelper.AssertUiEnabled(true);");
                    Debug.WriteLine(@"}");
                    Debug.WriteLine(@"");

                    currentDir = directoryName;
                }

                Debug.WriteLine(@"[TestMethod]");
                var testMethodName = (fileNameWithoutExtension.Length > 6
                    ? fileNameWithoutExtension.Insert(6, "_")
                    : fileNameWithoutExtension);
                Debug.WriteLine(@"public void @" + testMethodName + "() {");
                Debug.WriteLine(
                    @"var file = Path.Combine(CompilerHelper.SscliSourcePath, @""" + subfolders + @""", """ +
                    Path.GetFileName(file) + @""");");
                Debug.WriteLine(
                    @"CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + ""\\"", false, false);");
                Debug.WriteLine(@"}");
                Debug.WriteLine(@"");
            }

            Debug.WriteLine(@"}"); // class
            Debug.WriteLine(@"}"); // namespaces
            Debug.WriteLine(@"}"); // global namespace
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestAndroid()
        {
            Il2Converter.Convert(
                Path.GetFullPath(CompilerHelper.AndroidPath),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(true));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCompile()
        {
            // TODO: WARNING! order of obj files in g++ cmd line is important, CoreLib.obj should be last one

            // 10 - Double conversion (in CoreLib.dll some conversions are missing)
            // 100 - using DllImport      
            // 324 - bug NEED TO BE FIXED.
            // 353 - does not have Main method
            // 386 - Double conversion (in CoreLib.dll some conversions are missing)
            // 387 - Decimal conversion (in CoreLib.dll some conversions are missing)
            // 444 - codepage 65001 is used (can't be compiled)
            // 535 - IntPtr conversion (in CoreLib.dll some conversions are missing)
            // 550 - codepage 65001 is used (can't be compiled)
            // 551 - multiple definition of Int32 (but all issues are fixed)
            // 616 - test to compile Object (but it should be compiled without any Assembly reference)
            // 631 - missing System_Decimal__op_UnaryNegation (the same issue as 596)
            // 817 - redefinition of Int32
            // 864 - Decimal conversion (in CoreLib.dll some conversions are missing)
            var skip =
                new List<int>(
                    new[]
                    {
                        10,
                        100,
                        353,
                        386,
                        387,
                        444,
                        482,
                        535,
                        550,
                        551,
                        616,
                        631,
                        817,
                        864
                    });

            CompilerHelper.AssertUiEnabled(false);

            foreach (var index in Enumerable.Range(1, 907).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.Compile(string.Format("test-{0}", index));
            }

            CompilerHelper.AssertUiEnabled(true);
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
            // 36 - bug in execution (NotImplemented)
            // 39 - using Attributes
            // 74 - using StreamReader
            // 85 - using UnmanagedType
            // 91 - using Reflection
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
            // 167 - GetCustomAttributes
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
            // 250 - FieldsOffset attribute not implemented
            // 253 - System.Reflection
            // 254 - System.Reflection 
            // 263 - string with sbyte*
            // 266 - IConvertible in Enum
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
            // 300 - GetElementType (NotImplemented)
            // 301 - GetElementType (NotImplemented)
            // 304 - GetElementType (NotImplemented)
            // 305 - GetElementType (NotImplemented)
            // 308 - missing System.Security
            // 311 - SecurityPermission
            // 313 - typeof(D).GetMethods - NotImplemented
            // 318 - EventHandlerList error CS0246: The type or namespace name 'EventHandlerList' could not be found (are you missing a using directive or an assembly reference?)
            // 319 - missing DecimalConstantAttribute
            // 329 - GetCustromAttributes
            // 349 - TypeAttributes
            // 352 - MarshalAs
            // 353 - no Main()
            // 358 - missing implementation of ToString('R') for double
            // 361 - missing Attribute
            // 362 - cycling Catch/Throw bug (NEED TO BE FIXED!!!)
            // 367 - GetFields not implemented
            // 377 - lib with .IL file
            // 382 - using GetField
            // 388 - Xml
            // 389 - Xml, lib
            // 397 - using PropertyInfo etc
            // 399 - ArgIterator - NotImplemented (__arglist)
            // 414 - (12,28): error CS0507: 'BB.Data': cannot change access modifiers when overriding 'protected internal' inherited member 'AA.Data'
            // 418 - typeof (M3).Assembly.GetTypes
            // -----------
            // 32, 55, 74 - missing class

            // -----------------
            // 9 - decimal
            var skip =
                new List<int>(
                    new[]
                    {
                        9,
                        10,
                        19,
                        32,
                        36,
                        39,
                        53,
                        55,
                        74,
                        85,
                        91,
                        99,
                        100,
                        101,
                        102,
                        105,
                        106,
                        107,
                        109,
                        115,
                        118,
                        120,
                        126,
                        127,
                        128,
                        132,
                        135,
                        157,
                        158,
                        167,
                        174,
                        177,
                        178,
                        180,
                        181,
                        183,
                        187,
                        219,
                        220,
                        229,
                        230,
                        231,
                        232,
                        233,
                        236,
                        238,
                        239,
                        240,
                        250,
                        253,
                        254,
                        263,
                        266,
                        269,
                        273,
                        276,
                        279,
                        282,
                        286,
                        287,
                        295,
                        296,
                        297,
                        300,
                        301,
                        304,
                        305,
                        308,
                        311,
                        313,
                        318,
                        319,
                        329,
                        349,
                        352,
                        353,
                        358,
                        361,
                        362,
                        367,
                        377,
                        382,
                        388,
                        389,
                        397,
                        399,
                        414,
                        418
                    });

            if (CompilerHelper.UsingRoslyn)
            {
                // object o = -(2147483648); type is "int", not "long" in Roslyn
                skip.AddRange(new[] { 129 });
            }

            foreach (var index in Enumerable.Range(1, 906).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("test-{0}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCoreLib()
        {
            Il2Converter.Convert(
                Path.GetFullPath(CompilerHelper.CoreLibPath),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false));

            if (CompilerHelper.CompileWithOptimization)
            {
                CompilerHelper.ExecCmd("opt", "CoreLib.ll -o CoreLib.bc -O2");
                CompilerHelper.ExecCmd(
                    "llc",
                    string.Format("-filetype=obj -mtriple={0} CoreLib.bc", CompilerHelper.Target));
            }
            else
            {
                CompilerHelper.ExecCmd(
                    "llc",
                    string.Format("-filetype=obj -mtriple={0} CoreLib.ll", CompilerHelper.Target));
            }
        }


        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestMscolibCSNative()
        {
            // TODO: if you have undefined symbols, remove all linkodr_once and see which symbol is not defined

            Il2Converter.Convert(
                Path.GetFullPath(CompilerHelper.MscorlibPath),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false, stubs: true));

            ////if (CompilerHelper.CompileWithOptimization)
            ////{
            ////    CompilerHelper.ExecCmd("opt", "CoreLib.ll -o CoreLib.bc -O2");
            ////    CompilerHelper.ExecCmd(
            ////        "llc",
            ////        string.Format("-filetype=obj -mtriple={0} mscorlib.bc", CompilerHelper.Target));
            ////}
            ////else
            ////{
            ////    CompilerHelper.ExecCmd(
            ////        "llc",
            ////        string.Format("-filetype=obj -mtriple={0} mscorlib.ll", CompilerHelper.Target));
            ////}
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCustomConvert()
        {
            CompilerHelper.ConvertAll("test-1", CompilerHelper.SourcePathCustom);
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestGenCompile()
        {
            var skip = new List<int>();

            CompilerHelper.AssertUiEnabled(false);

            foreach (var index in Enumerable.Range(1, 589).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.Compile(string.Format("gtest-{0:000}", index));
            }

            CompilerHelper.AssertUiEnabled(true);
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
            // 72 - not implemented (DateTime to string)
            // 78 - not implemented
            // 102 - can't be compiled, Debug Trace: (18,5): error CS0315: The type 'int' cannot be used as type parameter 'T' in the generic type or method 'A<T>'. There is no boxing conversion from 'int' to 'System.IComparable'.
            // 109 - can't be compiled, Debug Trace: error CS0117: 'System.Array' does not contain a definition for 'Resize'
            // 117 - "xxx is int[]" treated as "xxx is int": NEED TO BE FIXED (when __Array__<T> is used)
            // 119 - typeof(x).Name (NotImplemeneted)
            // 128 - Reflection
            // 143 - BIG BUG with using "++" on structures due to using struct references instead of using copied object in stack
            // 162 - GetType. findMember
            // 165 - BUG in compiling (very cool bug, when you use the same specialized method in as generic method which causing issue to generate 2 the same methods)
            // 167 - Attribute.GetCustomAttributes
            // 177 - cast IEnumerable<T> from Array
            // 180 - Attributes
            // 184 - Array.FindAll not implemented
            // 186 - Serialization, FileStream etc not implemented
            // 188 - string does not have GetEnumerable on chars
            // 189 - object casted to IFoo and IFoo using Object functions (IFoo is not inherited from Object)
            // 196 - (19,10): error CS1061: 'System.Type' does not contain a definition for 'IsGenericType' and no extension method 'IsGenericType' accepting a first argument of type 'System.Type' could be found
            // 197 - (11,106): error CS1061: 'System.Type' does not contain a definition for 'MakeGenericType' and no extension method 'MakeGenericType' accepting a first argument of type 'System.Type' could be found
            // 205 - GetType of Generics
            // 207 - (10,18): error CS0117: 'System.Array' does not contain a definition for 'ForEach'
            // 214 - Attribute.GetCustomAttributes
            // 219 - GetMethod
            // 223 - GetMethod
            // 226 - GetField
            // 233 - ListChangedEventArgs not implemented
            // 236 - BUG in compiling 2 (very cool bug, when you use the same specialized method in as generic method which causing issue to generate 2 the same methods)
            // 237 - the same as 236
            // 239 - the same as 237, 236 etc
            // 243 - GetMethod
            // 262 - GetMethods
            // 278 - Type FullName not implemeneted fully
            // 282 - Type.Base is not implemeneted
            // 284 - 'System.Type' does not contain a definition for 'MakeArrayType'
            // 285 - Type.IsAssignableFrom is not implemeneted
            // 286 - GetCustomAttributes
            // 287 - List.ToString not implemented
            // 289 - GetConstructors not implemented
            // 296 - 'ObjectModel' does not exist in the namespace 'System.Collections'
            // 297 - 'RuntimeCompatibility' could not be found
            // 305 - GetConstructors not implemented
            // 311 - extern is used with DllImport
            // 316 - TODO: NEED TO BE FIXED (new T() is removed in the code, find out why), call !!0 [CoreLib]System.Activator::CreateInstance<!!T>() needs to be replaced with new !!T();
            // 53 - ValueType.ToString() not implemented

            var skip = new[]
            {
                40,
                51,
                53,
                56,
                65,
                72,
                78,
                102,
                109,
                117,
                119,
                128,
                143,
                156,
                161,
                162,
                165,
                167,
                177,
                180,
                184,
                186,
                188,
                189,
                196,
                197,
                205,
                207,
                214,
                219,
                223,
                226,
                233,
                236,
                237,
                239,
                243,
                262,
                278,
                282,
                284,
                285,
                286,
                287,
                289,
                296,
                297,
                305,
                311
            };
            foreach (var index in Enumerable.Range(1, 589).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-{0:000}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestMscorlibCompile()
        {
            // TODO: if you have undefined symbols, remove all linkodr_once and see which symbol is not defined

            // Do not forget to set MSCORLIB variable

            // you need to compile it with optimization, otherwise it will not be compiled as being to big
            /*
                IF NOT EXIST mscorlib.obj opt mscorlib.ll -o mscorlib.bc -O2
                IF NOT EXIST mscorlib.obj llc -filetype=obj -mtriple=i686-w64-mingw32 mscorlib.bc
                opt test-1.ll -o test-1.bc -O3 
                llc -filetype=obj -mtriple=i686-w64-mingw32 test-1.bc
                g++.exe -o test-1.exe mscorlib.obj test-1.obj -lstdc++ -lgc-lib -march=i686 -L .
                del test-1.obj
                del test-1.bc
             */

            // alternative way to compile
            /*
                llvm-link -o=test-1.bc test-1.ll mscorlib.ll
                llc -filetype=obj -mtriple=i686-w64-mingw32 test-1.bc
                g++.exe -o test-1.exe test-1.obj -lstdc++ -lgc-lib -march=i686 -L .
                del test-1.o
             */

            // WHAT TODO here
            // remove static dependancy on count of interfaces
            // adjust creating string as MSCORLIB does
            // adjust creating RuntimeType as MSCORLIB does

            Il2Converter.Convert(
                Path.GetFullPath(
                    @"C:\Windows\Microsoft.NET\assembly\GAC_32\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll"),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false, stubs: true));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestMscorlibCompile_TypeTest()
        {
            // Do not forget to set MSCORLIB variable

            Il2Converter.Convert(
                Path.GetFullPath(
                    @"C:\Windows\Microsoft.NET\assembly\GAC_32\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll"),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false),
                new[] { "System.Diagnostics.Tracing.EventProvider" });
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestMscorlibCompile_ReducedBuild()
        {
            // Do not forget to set MSCORLIB variable

            Il2Converter.Convert(
                //Path.GetFullPath(@"C:\Windows\Microsoft.NET\assembly\GAC_32\mscorlib\v4.0_4.0.0.0__b77a5c561934e089\mscorlib.dll"),
                CompilerHelper.MscorlibPath,
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false),
                new[]
                {
                    "System.Object",
                });
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestOpenGlExe()
        {
            Il2Converter.Convert(
                Path.GetFullPath(CompilerHelper.OpenGlExePath),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(true));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestOpenGlLib()
        {
            // todo: 
            // 1) class Condition method _getEffectiveTarget when it is returning Object it does not cast Interface to an Object, to replicate the issue change returning type to Object
            // 2) the same as 1) but in InterpolateValueAction when saving value 'value = this._target[this._property]'
            Il2Converter.Convert(
                Path.GetFullPath(CompilerHelper.OpenGlLibPath),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(true));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestPdbReader()
        {
            Converter.Convert(CompilerHelper.CoreLibPdbPath, new DummySymbolWriter.DummySymbolWriter());
        }

        /// </summary>
        [TestMethod]
        public void TestSscli()
        {
            foreach (
                var file in
                    Directory.EnumerateFiles(CompilerHelper.SscliSourcePath, "*.cs", SearchOption.AllDirectories))
            {
                CompilerHelper.CompileAndRun(
                    Path.GetFileNameWithoutExtension(file),
                    Path.GetDirectoryName(file) + "\\",
                    true);
            }
        }

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
    }
}
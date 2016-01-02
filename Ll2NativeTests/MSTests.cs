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

    using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

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
        [Ignore]
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
        [Ignore]
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

        [TestMethod]
        [Ignore]
        public void GenerateTestFromCorCLRTests()
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
                    Directory.EnumerateFiles(CompilerHelper.CoreCLRSourcePath, "*.cs", SearchOption.AllDirectories))
            {
                if (file.ToLowerInvariant().Contains("regress") || file.ToLowerInvariant().Contains("stress"))
                {
                    continue;
                }

                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                var directoryName = Path.GetDirectoryName(file);
                var folderName = Path.GetFileName(directoryName);
                var subfolders = directoryName.Substring(CompilerHelper.CoreCLRSourcePath.Length);
                var subfoldersEffective = subfolders;

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

                        currentNamespace = subfolders;
                        var testNamespaceName = currentNamespace;

                        testNamespaceName = testNamespaceName.Replace("\\0", "\\_0");
                        testNamespaceName = testNamespaceName.Replace("\\1", "\\_1");
                        testNamespaceName = testNamespaceName.Replace("\\2", "\\_2");
                        testNamespaceName = testNamespaceName.Replace("\\3", "\\_3");
                        testNamespaceName = testNamespaceName.Replace("\\4", "\\_4");
                        testNamespaceName = testNamespaceName.Replace("\\5", "\\_5");
                        testNamespaceName = testNamespaceName.Replace("\\6", "\\_6");
                        testNamespaceName = testNamespaceName.Replace("\\7", "\\_7");
                        testNamespaceName = testNamespaceName.Replace("\\8", "\\_8");
                        testNamespaceName = testNamespaceName.Replace("\\9", "\\_9");

                        Debug.WriteLine(@"namespace @" + testNamespaceName.Replace(".", "_").Replace("\\", ".@").Replace("-", "_") + " {");
                    }

                    var testClassName = folderName;
                    if (Char.IsDigit(testClassName[0]))
                    {
                        testClassName = "_" + folderName;
                    }

                    if (subfolders.EndsWith(folderName))
                    {
                        testClassName = "testclass_" + testClassName;
                    }

                    Debug.WriteLine(@"[TestClass]");
                    Debug.WriteLine(@"public class @" + testClassName.Replace(".", "_").Replace("-", "_") + " {");
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
                var testMethodName = fileNameWithoutExtension;
                if (Char.IsDigit(testMethodName[0]))
                {
                    testMethodName = "_" + fileNameWithoutExtension;
                }

                if (subfolders.EndsWith(fileNameWithoutExtension))
                {
                    testMethodName = "test_" + testMethodName;
                }

                Debug.WriteLine(@"public void @" + testMethodName.Replace(".", "_").Replace("-", "_") + "() {");
                Debug.WriteLine(
                    @"var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @""" + subfoldersEffective + @""", """ +
                    Path.GetFileName(file) + @""");");
                Debug.WriteLine(@"var testfolder = Path.Combine(CompilerHelper.CoreCLRSourcePath, @""Common\CoreCLRTestLibrary"");");
                Debug.WriteLine(
                    @"CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + ""\\"", false, true, returnCode: 100, additionalFilesFolder:testfolder, additionalFilesPattern: new[] { ""*.cs"" });");
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
        [Timeout(36000000)]
        public void Test_Mono_Tests()
        {
            // TODO: think about using IntPtr as "native int" when it is used as struct type to speed up execution and reduce some code

            // TODO: test-201: BUG with using field with the same name as struct causing issue (+274 for generics) +338 +625

            // TODO: gtest-named-04: calling function with incorrect order of passed arguments (right-to-left but should be left-to-right) (BUG: needs to be fixed)

            // TODO: finish
            // __arglist

            // TODO: finish
            // Nullable<T> to object when nullable is null, tests gtests-378 and 372

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

            // Bug, needs to be fixed, LoadLocal/SaveLocal used in Code.Dup
            // test-527 - for Roslyn
            /*
                  private int[] stack = new int[1];
                  private int cc;
                  public int fc;
                  private int sp;

                  fc = cc = bar();
                  fc = stack[sp++] = cc;
             */

            // 19 - using Thread class, Reflection
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
            // 127 - IsDefined not implemented
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
            // 201 - field name using t
            // 219 - can't be compiled (22,26): error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes' and no extension method 'GetCustomAttributes' accepting a first argument of type 'System.Type' could be found (are you missing a using directive or an assembly reference?)
            // 220 - can't be compiled (8,26): error CS0234: The type or namespace name 'Specialized' does not exist in the namespace 'System.Collections' (are you missing an assembly reference?)
            // 229 - can't be compiled (3,26): error CS0234: The type or namespace name 'Specialized' does not exist in the namespace 'System.Collections' (are you missing an assembly reference?)
            // 230 - using Reflection
            // 231 - NEED TO BE FIXED (when "this" is null. it should throw an error (Null Reference)
            // 233 - Reflection
            // 236 - Attributes
            // 238 - can't be compiled (5,10): error CS0246: The type or namespace name 'Conditional' could not be found (are you missing a using directive or an assembly reference?)
            // 239 - can't be compiled (5,10): error CS0246: The type or namespace name 'Conditional' could not be found (are you missing a using directive or an assembly reference?)
            // 240 - the same as 239
            // 250 - FieldsOffset attribute not implemented
            // 253 - System.Reflection
            // 254 - System.Reflection 
            // 266 - IConvertible in Enum
            // 269 - ArgIterator
            // 273 - GetCustomAttributes
            // 276 - GetType.GetEvents(); (NotImplemented)
            // 279 - Enum ToString with Flags
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
            // 319 - missing DecimalConstantAttribute, error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes'
            // 329 - GetCustromAttributes
            // 338 - field name used as class name
            // 349 - TypeAttributes
            // 352 - MarshalAs
            // 353 - no Main()
            // 358 - missing implementation of ToString('R') for double
            // 361 - missing Attribute
            // 367 - GetFields not implemented
            // 377 - lib with .IL file
            // 382 - using GetField
            // 388 - Xml
            // 389 - Xml, lib
            // 397 - using PropertyInfo etc
            // 399 - ArgIterator - NotImplemented (__arglist)
            // 414 - (12,28): error CS0507: 'BB.Data': cannot change access modifiers when overriding 'protected internal' inherited member 'AA.Data'
            // 418 - typeof (M3).Assembly.GetTypes
            // 419 - lib (which requires to compile DLL files)
            // 420 - lib (which uses files from 419)
            // 437 - missing type UnmanagedType
            // 438 - (3,14): error CS0234: The type or namespace name 'Security' does not exist
            // 443 - missing file
            // 444 - using UTF8
            // 449 - error CS0103: The name 'ThreadPool' does not exist in the current context
            // 451 - (10,20): error CS0030: Cannot convert type 'int' to 'System.IComparable'
            // 454 - error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes'
            // 458 - error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes'
            // 463 - error CS0246: The type or namespace name 'ExpandableObjectConverter' could not be found
            // 464 - error CS1061: 'System.Reflection.Assembly' does not contain a definition for 'GetManifestResourceNames' and no extension method 'GetManifestResourceNames'
            // 465 - error CS1061: 'System.Reflection.Assembly' does not contain a definition for 'GetManifestResourceNames' and no extension method 'GetManifestResourceNames'
            // 466 - not compilable
            // 468 - error CS0246: The type or namespace name 'ComImport' could not be found
            // 471 - error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes
            // 472 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'GetParameters'
            // 473 - error CS0619: 'EnumWrapper' is obsolete
            // 477 - error CS0234: The type or namespace name 'Conditional' does not exist in the namespace 'System.Diagnostics'
            // 478 - error CS0234: The type or namespace name 'Design' does not exist in the namespace 'System.ComponentModel' (TODO: can be fixed)
            // 483 - using IL file for Library
            // 485 - Overflow check, required -checked option
            // 489 - error CS0430: The extern alias 'FULL' was not specified in a /reference option
            // 492 - error CS1061: 'System.Reflection.MemberInfo' does not contain a definition for 'GetCustomAttributes'
            // 498 - error CS1061: 'System.Type' does not contain a definition for 'GetConstructors'
            // 500 - error CS1061: 'System.Reflection.FieldInfo' does not contain a definition 
            // 507 - error CS0103: The name 'MethodAttributes' does not exist in the current context
            // 513 - CS1061: 'System.Reflection.Assembly' does not contain a definition for 'GetManifestResourceNames'
            // 527 - bug for Dup values in Roslyn (TODO: investigate)
            // 530 - special chars used
            // 531 - library IL
            // 550 - used UTF8 in names
            // 551 - using Int32 as struct in System name space (conflict of names)
            // 555 - error CS0234: The type or namespace name 'Emit' does not exist in the namespace 'System.Reflection'
            // 562 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'CallingConvention'
            // 564 - can't be compiled
            // 567 - error CS0246: The type or namespace name 'PreserveSig' could not be found
            // 594 - Type.Assembly NotImplemented
            // 604 - Type.GetMethods NotImplemented
            // 606 - error CS1061: 'System.Type' does not contain a definition for 'GetProperties'
            // 607 - error CS0246: The type or namespace name 'AssemblyAlgorithmId' could not be found
            // 608 - error CS0266: Cannot implicitly convert type 'System.Enum' to 'System.IConvertible'
            // 616 - test system classes without fields. TODO: Can be fixed later
            // 617 - GetMethod for Type class NotImplemented
            // 621 - error CS0246: The type or namespace name 'FieldOffset'. TODO: have a look, we need to implement FieldOffset
            // 636 - (6,14): error CS0161: 'Foo.test_while(int)': not all code paths return a value
            // 637 - error CS1061: 'System.Reflection.MemberInfo' does not contain a definition for 'GetCustomAttributes'
            // 645 - (23,14): error CS0101: The namespace '<global namespace>' already contains a definition for 'C'. TODO: Investigate
            // 654 - ??? 
            // 657 - error CS0246: The type or namespace name 'Conditional' could not be found
            // 661 - error CS0246: The type or namespace name 'SummaryInfo' could not be found
            // 666 - unicode used in preprocessor
            // 671 - error CS0837: The first operand of an 'is' or 'as' operator may not be a lambda expression, anonymous method, or method group.
            // 678 - issue with comparing NaN to 0.0 in C code. TODO: Investigate
            // 679 - lib with DLLs
            // 684 - getElementType is not implemented (for Array clone)
            // 692 - TODO: Investigate, DateTime returns ArgumentOutOfRange 
            // 695 - error CS0246: The type or namespace name 'AssemblyDefinition' could not be found
            // 704 - DllImport is not implemented and ArgIterator
            // 715 - NetModule lib
            // 716 - error CS0103: The name 'TypeAttributes' does not exist
            // 721 - error CS0246: The type or namespace name 'MarshalAs' could not be found
            // 725 - The type or namespace name 'In' could not be found ([In])
            // 733 - Optimization causing issues with float const. TODO: Investigate if can be fixed as it works ok in C#
            // 734 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'GetMethodBody'
            // 739 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'Attributes'
            // 740 - error CS0246: The type or namespace name 'FieldOffset' could not be found. TODO: Needs to be implemented
            // 748 - error CS0029: Cannot implicitly convert type 'RealTest.Foo' to 'Test.Foo'
            // 759 - lib with IL
            // 760 - lib with IL
            // 769 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'Attributes'
            // 773 - error CS1061: 'System.Type' does not contain a definition for 'GetProperty'
            // 782 - using UTF8
            // 792 - lib with IL
            // 793 - test works but returns 1 as sizeof in C# and C++ have different result (maybe will be fixed in future)
            // 795 - error CS0246: The type or namespace name 'AppDomainSetup' could not be found
            // 801 - error CS0034: Operator '-' is ambiguous on operands of type 'C.E' and 'C'
            // 805 - lib with IL
            // 806 - error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes'
            // 810 - lib with IL
            // 811 - error CS0246: The type or namespace name 'ArgIterator' could not be found
            // 814 - Mono
            // 816 - error CS1061: 'System.AppDomain' does not contain a definition for 'TypeResolve'
            // 817 - error in generated C (conflict of System.Int32)
            // 819 - error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes'
            // 820 - using two Main
            // 823 - lib with IL
            // 825 - error CS0246: The type or namespace name 'Conditional' could not be found. TODO: Can be fixed
            // 826 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'Attributes' 
            // 831 - error CS0246: The type or namespace name 'Conditional' could not be found. TODO: Can be fixed
            // 846 - lib with IL
            // 847 - error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes'
            // 851 - lib with IL
            // 858 - lib with IL
            // 860 - error CS0117: 'System.Attribute' does not contain a definition for 'GetCustomAttribute'
            // 863 - NotImplemented __refanytype. TODO: Can be implemeneted
            // 868 - error CS1061: 'System.Reflection.Assembly' does not contain a definition for 'Location'
            // -----------
            // 32, 55, 74 - missing class

            var skip =
                new List<int>(
                    new[]
                    {
                        19,
                        32,
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
                        118,
                        120,
                        127,
                        128,
                        132,
                        135,
                        157,
                        158,
                        174,
                        177,
                        178,
                        180,
                        181,
                        183,
                        187,
                        201,
                        219,
                        220,
                        229,
                        230,
                        231,
                        233,
                        236,
                        238,
                        239,
                        240,
                        250,
                        253,
                        254,
                        266,
                        269,
                        273,
                        276,
                        279,
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
                        338,
                        349,
                        352,
                        353,
                        358,
                        361,
                        367,
                        377,
                        382,
                        388,
                        389,
                        397,
                        399,
                        414,
                        418,
                        419,
                        420,
                        437,
                        438,
                        443,
                        444,
                        449,
                        451,
                        454,
                        458,
                        463,
                        464,
                        465,
                        466,
                        468,
                        471,
                        472,
                        473,
                        477,
                        478,
                        483,
                        485,
                        489,
                        492,
                        498,
                        500,
                        507,
                        513,
                        527,
                        530,
                        531,
                        547,
                        550,
                        551,
                        555,
                        562,
                        564,
                        567,
                        594,
                        604,
                        606,
                        607,
                        608,
                        616,
                        617,
                        621,
                        636,
                        637,
                        645,
                        654,
                        657,
                        660,
                        661,
                        666,
                        671,
                        678,
                        679,
                        684,
                        692,
                        695,
                        704,
                        715,
                        716,
                        721,
                        725,
                        733,
                        734,
                        739,
                        740,
                        748,
                        759,
                        760,
                        769,
                        773,
                        782,
                        792,
                        793,
                        795,
                        801,
                        805,
                        806,
                        810,
                        811,
                        814,
                        816,
                        817,
                        819,
                        820,
                        823,
                        825,
                        826,
                        831,
                        846,
                        847,
                        851,
                        858,
                        860,
                        863,
                        868
                    });

            // MISSING ARRAY IN CORELIB
            // TODO: remove it
            skip.Add(45);
            // TODO: full namespace not correct
            skip.Add(126);
            
            if (CompilerHelper.UsingRoslyn)
            {
                // object o = -(2147483648); type is "int", not "long" in Roslyn
                skip.AddRange(new[] { 129 });
            }
            else
            {
                skip.AddRange(new[] { 499, 591 });
            }

            // TODO: remove when overflow ops are done
            skip.AddRange(new[] { 643 });

            //CompilerHelper.CompactMode = true;
            //CompilerHelper.Stubs = true;

            foreach (var index in Enumerable.Range(1, 869).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("test-{0}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        [Timeout(36000000)]
        public void Test_Mono_Tests_Anon()
        {
            // 34 - error CS0234: The type or namespace name 'Timers' does not exist in the namespace 'System' (are you missing an assembly reference?)
            // 42 - compiling with -Ofast causing C app to crash
            // 73 - error CS0234: The type or namespace name 'ThreadPool' does not exist in the namespace 'System.Threading' (are you missing an assembly reference?)
            // 119 - error CS0234: The type or namespace name 'RegularExpressions' does not exist
            // 122 - error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' ((33,15): error CS7043: Runtime library method 'System.Linq.Expressions.Expression.Constant(object, System.Type)' not found.)
            // 124 - error CS0315: The type 'ulong' cannot be used as type parameter 'T' in the generic type or method 'Test.NestedTypeMutate<T>()'. There is no boxing conversion from 'ulong' to 'System.IEquatable<ulong>'. (but it can be compiled with .NET)
            // 135 - GetFields - NotImplemeneted
            // 138 - error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' ((12,12): error CS1061: 'System.Linq.Expressions.Expression<System.Func<bool>>' does not contain a definition for 'Compile' and no extension method 'Compile' accepting a first argument of type )

            var skip = new List<int>(new[]
            {
                34,
                42,
                73,
                119,
                122,
                124,
                135,
                138
            });

            foreach (var index in Enumerable.Range(1, 171).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("test-anon-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCoreLib()
        {
            // you need to compile GC property to include GC_pthread_create etc.
            // read Readme.win32 how to compile it for MinGW

            Il2Converter.Convert(
                Path.GetFullPath(CompilerHelper.CoreLibPath),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false, stubs: true, split:true));

            ////CompilerHelper.ExecCmd(
            ////    "g++",
            ////    string.Format(
            ////        "-fno-rtti {0}-o CoreLib.obj -c CoreLib.cpp{1}",
            ////        CompilerHelper.CompileWithOptimization ? "-O2 " : string.Empty,
            ////        CompilerHelper.GcDebugEnabled ? " -I " + CompilerHelper.GcHeaders : string.Empty));
        }

        [TestMethod]
        public void TestBabylonGlut()
        {
            CompilerHelper.CompactMode = true;

            Il2Converter.Convert(
                Path.GetFullPath(@"D:\Developing\BabylonNative\BabylonNativeCs\BabylonGlut\bin\Release\BabylonGlut.dll"),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false, stubs: true, split: true));
        }


        /// <summary>
        /// </summary>
        /*
# typical cmake to build libraries
cmake_minimum_required(VERSION 2.8)

project (test) 

file(GLOB mscorlib_SRC
    "M:/mscorlib*.cpp"
)

file(GLOB system_private_uri_SRC
    "M:/System.Private.Uri*.cpp"
)

file(GLOB system_resources_resourceManager_SRC
    "M:/System.Resources.ResourceManager*.cpp"
)

file(GLOB system_collections_SRC
    "M:/System.Collections*.cpp"
)

file(GLOB system_diagnostics_debug_SRC
    "M:/System.Diagnostics.Debug*.cpp"
)

file(GLOB system_runtime_SRC
    "M:/System.Runtime*.cpp"
)

file(GLOB system_runtime_extensions_SRC
    "M:/System.Runtime.Extensions*.cpp"
)

file(GLOB system_linq_SRC
    "M:/System.Linq*.cpp"
)

file(GLOB test_SRC
    "M:/test-*.cpp"
)


if(DEBUG)		
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -march=native -std=c++11 -fno-rtti")
else()
	SET(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -march=native -std=c++11 -fno-rtti -O3")
endif()

include_directories("E:/Gits/bdwgc/include")
link_directories("M:/")

add_library(mscorlib ${mscorlib_SRC})
add_library(system_private_uri ${system_private_uri_SRC})
add_library(system_resources_resourceManager ${system_resources_resourceManager_SRC})
add_library(system_collections ${system_collections_SRC})
add_library(system_diagnostics_debug ${system_diagnostics_debug_SRC})
add_library(system_runtime ${system_runtime_SRC})
add_library(system_runtime_extensions ${system_runtime_extensions_SRC})
add_library(system_linq ${system_linq_SRC})
add_executable(test ${test_SRC})

target_link_libraries (test mscorlib system_private_uri system_resources_resourceManager system_collections system_diagnostics_debug system_runtime system_runtime_extensions system_linq "stdc++" "gcmt-lib")

         */
        [TestMethod]
        //[Ignore]
        public void TestMscolibCSNative()
        {
            Il2Converter.Convert(
                Path.GetFullPath(CompilerHelper.MscorlibPath),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false, stubs: true, split: true));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        //[Ignore]
        public void TestMscolibCSNative_Type()
        {            
            Il2Converter.Convert(
                Path.GetFullPath(CompilerHelper.MscorlibPath),
                CompilerHelper.OutputPath,
                CompilerHelper.GetConverterArgs(false, stubs: true, split: true),
                new[] { "System.Delegate" });
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestSystemLinq()
        {
            Il2Converter.Convert(
                            string.Format(@"{0}System.Private.Uri.dll", CompilerHelper.CoreCLRDlls),
                            CompilerHelper.OutputPath,
                            CompilerHelper.GetConverterArgs(false, stubs: true, split: false));

            Il2Converter.Convert(
                            string.Format(@"{0}System.Resources.ResourceManager.dll", CompilerHelper.CoreCLRDlls),
                            CompilerHelper.OutputPath,
                            CompilerHelper.GetConverterArgs(false, stubs: true, split: false));

            Il2Converter.Convert(
                            string.Format(@"{0}System.Collections.dll", CompilerHelper.CoreCLRDlls),
                            CompilerHelper.OutputPath,
                            CompilerHelper.GetConverterArgs(false, stubs: true, split: false));

            Il2Converter.Convert(
                            string.Format(@"{0}System.Diagnostics.Debug.dll", CompilerHelper.CoreCLRDlls),
                            CompilerHelper.OutputPath,
                            CompilerHelper.GetConverterArgs(false, stubs: true, split: false));

            Il2Converter.Convert(
                            string.Format(@"{0}System.Runtime.dll", CompilerHelper.CoreCLRDlls),
                            CompilerHelper.OutputPath,
                            CompilerHelper.GetConverterArgs(false, stubs: true, split: false));

            Il2Converter.Convert(
                            string.Format(@"{0}System.Runtime.Extensions.dll", CompilerHelper.CoreCLRDlls),
                            CompilerHelper.OutputPath,
                            CompilerHelper.GetConverterArgs(false, stubs: true, split: false));

            Il2Converter.Convert(
                            string.Format(@"{0}System.Linq.dll", CompilerHelper.CoreCLRDlls),
                            CompilerHelper.OutputPath,
                            CompilerHelper.GetConverterArgs(false, stubs: true, split: false));
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void TestCustomConvert()
        {
            //CompilerHelper.Mscorlib = true;
            //CompilerHelper.MscorlibPath = string.Format(@"{0}mscorlib.dll", CompilerHelper.CoreCLRDlls);
            //CompilerHelper.AddSystemLinq = true;
            //CompilerHelper.CompactMode = true;
            //CompilerHelper.Stubs = true;
            //CompilerHelper.Split = true;
            CompilerHelper.ConvertAll("test-1", CompilerHelper.SourcePathCustom);
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        [Timeout(36000000)]
        public void Test_Mono_GTests()
        {
            // 329 - DEBUG ASSERT - investigate

            // 56 - bug in execution (NotImplemented)
            // 65 - can't be compiled yet, Debug Trace: (39,22): error CS0311: The type 'string' cannot be used as type parameter 'T' in the generic type or method 'ComparablePair<T, U>'. There is no implicit reference conversion from 'string' to 'System.IComparable<string>'.
            // 72 - not implemented (DateTime to string)
            // 102 - can't be compiled, Debug Trace: (18,5): error CS0315: The type 'int' cannot be used as type parameter 'T' in the generic type or method 'A<T>'. There is no boxing conversion from 'int' to 'System.IComparable'.
            // 109 - can't be compiled, Debug Trace: error CS0117: 'System.Array' does not contain a definition for 'Resize'
            // 117 - "xxx is int[]" treated as "xxx is int": NEED TO BE FIXED (when __Array__<T> is used)
            // 128 - Reflection
            // 162 - GetType. findMember
            // 165 - BUG in compiling (very cool bug, when you use the same specialized method in as generic method which causing issue to generate 2 the same methods)
            // 167 - Attribute.GetCustomAttributes
            // 180 - Attributes
            // 184 - Array.FindAll not implemented
            // 186 - Serialization, FileStream etc not implemented
            // 188 - string does not have GetEnumerable on chars
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
            // 329 - TDOD: FIX IT. Conflict __check_this with unboxing of Nullable
            // 340 - error CS1061: 'System.Type' does not contain a definition for 'GetMember'
            // 341 - error CS0246: The type or namespace name 'SpecialNameAttribute' could not be found
            // 345 - error CS0246: The type or namespace name 'Conditional' could not be found
            // 349 - System.NativeType.GetAttributeFlagsImpl not implemented yet
            // 352 - error CS1061: 'System.Type' does not contain a definition for 'GetConstructors'
            // 380 - error CS1061: 'System.Reflection.FieldInfo' does not contain a definition for 'GetCustomAttributes'
            // 385 - error CS1061: 'System.Type' does not contain a definition for 'GetGenericArguments'
            // 391 - error CS0019: Operator '??' cannot be applied to operands of type '<null>' and '<null>'
            // 393 - error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes'
            // 408 - error CS0246: The type or namespace name 'MarshalAs' could not be found
            // 412 - error CS1061: 'System.Type' does not contain a definition for 'GenericParameterAttributes'
            // 418 - error CS1061: 'System.Type' does not contain a definition for 'GetProperties'
            // 421 - System.Type.IsValueType NotImplemented
            // 431 - redeclaration of ExtensionAttribute(System.Runtime.CompilerServices) from CoreLib#
            // 434 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'Attributes'
            // 437 - error: GetAttributes
            // 444 - error CS1061: 'System.Reflection.AssemblyName' does not contain a definition for 'Flags'
            // 447 - error CS0246: The type or namespace name 'TypeForwardedTo' could not be found
            // 449 - error CS0103: The name 'PortableExecutableKinds' does not exist in the current context
            // 450 - error CS0103: The name 'PortableExecutableKinds' does not exist in the current context
            // 456 - GetType().ToString() does not return "G`1+S[System.Int32]". TODO: Can be fixed
            // 464 - lib using IL
            // 499 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'GetMethodBody'
            // 500 - error CS0029: Cannot implicitly convert type 'int?' to 'System.IComparable'. TODO: Can be fixed 
            // 502 - GetType().ToString() does not return "A+N`1[System.Double]". TODO: Can be fixed
            // 503 - error CS1061: 'System.Type' does not contain a definition for 'GetCustomAttributes'
            // 507 - BUG in compiling 2 (very cool bug, when you use the same specialized method in as generic method which causing issue to generate 2 the same methods)
            // 511 - error CS1061: 'System.Reflection.MethodInfo' does not contain a definition for 'GetGenericArguments'
            // 512 - expected NullReferenceException
            // 528 - error CS0315: The type 'int' cannot be used as type parameter 'U' in the generic type or method 'GenericType<U>'. There is no boxing conversion from 'int' to 'System.IEquatable<int>'. TODO: Investigate
            // 529 - error CS0315: The type 'byte' cannot be used as type parameter 'V' in the generic type or method 'Base<V>'. There is no boxing conversion from 'byte' to 'System.IEquatable<byte>'.
            // 554 - error CS0246: The type or namespace name 'InterfaceMapping' could not be found
            // 555 - Bug with Nullable in 'is' operator. TODO: Need to be fixed
            // 564 - error CS1061: 'System.Reflection.MethodInfo'
            // 582 - error CS1061: 'System.Type' does not contain a definition for 'GetGenericArguments'
            // 583 - lib with IL

            // 53 - ValueType.ToString() not implemented

            var skip = new List<int>(new[]
            {
                56,
                65,
                72,
                102,
                109,
                117,
                128,
                161,
                162,
                165,
                167,
                180,
                184,
                186,
                188,
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
                311,
                329,
                340,
                341,
                345,
                349,
                352,
                380,
                385,
                391,
                393,
                408,
                412,
                418,
                421,
                431,
                434,
                437,
                444,
                447,
                449,
                450,
                456,
                464,
                499,
                500,
                502,
                503,
                507,
                511,
                512,
                528,
                529,
                554,
                555,
                564,
                582,
                583
            });

            foreach (var index in Enumerable.Range(1, 589).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-{0:000}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_Anon()
        {
            // 9 - compiling with -Ofast causing C app to crash
            // 19 - error CS0234: The type or namespace name 'RegularExpressions' does not exist in the namespace 'System.Text'
            // 22 - error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System'
            // 33 - StringBuilder enumerator is not impelemented. TODO: Investigate
            // 35 - GetFields - NotImplemeneted
            // 38 - error CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System'

            var skip = new List<int>(new[]
            {
                9,
                19,
                22,
                33,
                35,
                38
            });

            foreach (var index in Enumerable.Range(1, 56).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-anon-{0}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_AnonType()
        {
            // 13 - rror CS0234: The type or namespace name 'Linq' does not exist in the namespace 'System' (GetProprty not implemented)

            var skip = new List<int>(new[]
            {
                13
            });

            foreach (var index in Enumerable.Range(1, 13).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-anontype-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_Autoproperty()
        {
            // 3 - error CS1061: 'System.Reflection.FieldInfo' does not contain a definition for 'GetCustomAttributes'

            var skip = new List<int>(new[]
            {
                3
            });

            foreach (var index in Enumerable.Range(1, 7).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-autoproperty-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_Tests_Iter()
        {
            // 18 - Reflection NotImplemented
            // 23 - error CS0103: The name 'ThreadPool' does not exist in the current context

            var skip = new List<int>(new[]
            {
                18,
                23
            });

            foreach (var index in Enumerable.Range(1, 26).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("test-iter-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_Collectioninit()
        {
            // 

            var skip = new List<int>(new[]
            {
                0
            });

            foreach (var index in Enumerable.Range(1, 3).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-collectioninit-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_ExMethod()
        {
            // 1 - error CS1061: 'System.Type' does not contain a definition for 'IsDefined'
            // 12 - error CS0234: The type or namespace name 'Specialized' does not exist in the namespace 'System.Collections'
            // 15 - error CS0246: The type or namespace name 'ICustomAttributeProvider' could not be found
            // 26 - error CS0121: The call is ambiguous between the following methods or properties: 'Test2.Extensions.IsNullable(System.Type)' and 'test.TypeExtensions.IsNullable(System.Type)' (TODO: Review it)
            // 35 - Redeclaring attribute which used in Core lib
            // 45 - lib with IL
            // 46 - error CS1503: Argument 1: cannot convert from 'string' to 'char' (TODO: Investigate)

            var skip = new List<int>(new[]
            {
                1,
                12,
                15,
                26,
                35,
                45,
                46
            });

            foreach (var index in Enumerable.Range(1, 46).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-exmethod-{0:00}", index));
            }
        }


        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_FixedBuffer()
        {
            // 1 - error CS0246: The type or namespace name 'DefaultCharSet' could not be found
            // 8 - error CS1061: 'System.Type' does not contain a definition for 'StructLayoutAttribute', (TODO: finish it when StructLayoutAttribute is done)
            // 9 - error CS1061: 'System.Reflection.FieldInfo' does not contain a definition for 'GetCustomAttributes'

            var skip = new List<int>(new[]
            {
                1,
                8,
                9
            });

            CompilerHelper.AssertUiEnabled(false);

            foreach (var index in Enumerable.Range(1, 10).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-fixedbuffer-{0:00}", index));
            }

            CompilerHelper.AssertUiEnabled(true);
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_ImplicitArray()
        {
            foreach (var index in Enumerable.Range(1, 3))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-implicitarray-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_Initialize()
        {
            foreach (var index in Enumerable.Range(1, 12))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-initialize-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_Iter()
        {
            // 26 - error CS0246: The type or namespace name 'IteratorStateMachineAttribute' could not be found

            var skip = new List<int>(new[]
            {
                26
            });

            foreach (var index in Enumerable.Range(1, 29).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-iter-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_Lambda()
        {
            // 3 - error CS0117: 'System.TimeSpan' does not contain a definition for 'Parse' (TODO: Review it)
            // 4 - error CS0117: 'System.TimeSpan' does not contain a definition for 'Parse' (TODO: Review it)
            // 25 - (TODO: you can fix it if you have time), forcing conflict with generic params

            var skip = new List<int>(new[]
            {
                3,
                4,
                25
            });

            foreach (var index in Enumerable.Range(1, 31).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-lambda-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_Linq()
        {
            // 5 - BUG: in CoreLib, in C# Compare string depends on culture settings and can be case-sensitive or not (TODO: fix CoreLib)
            // 10 - error CS1930: The range variable 'a' has already been declared
            // 13 - error CS0246: The type or namespace name 'CollectionBase' could not be found (are you missing a using directive or an assembly reference?)
            // 14 - error CS0518: Predefined type 'System.Linq.Expressions.ParameterExpression' is not defined or imported
            // 22 - error CS0117: 'System.DateTime' does not contain a definition for 'TryParse'

            var skip = new List<int>(new[]
            {
                5,
                10,
                13,
                14,
                22
            });

            foreach (var index in Enumerable.Range(1, 28).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.AddSystemLinq = true;
                CompilerHelper.CompileAndRun(string.Format("gtest-linq-{0:00}", index));
                CompilerHelper.AddSystemLinq = false;
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_GTests_Named()
        {
            // 4 - Wrong order of calling parameters in methods call (TODO: fix it)

            var skip = new List<int>(new[]
            {
                4
            });

            foreach (var index in Enumerable.Range(1, 4).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-named-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        [Ignore]
        public void Test_Mono_GTests_Variance()
        {
            // BUG: 21 - generated code is not compilable

            // !!! Variance is not implemented

            // 10 - casting I<object> from I<string>
            // 12
            // 15
            // 19 - error CS0266: Cannot implicitly convert type 'System.Collections.Generic.IEnumerable<T>' to 'System.Collections.Generic.IEnumerable<U>'. An explicit conversion exists (are you missing a cast?)

            var skip = new List<int>(new[]
            {
                10,
                12,
                15,
                19
            });

            foreach (var index in Enumerable.Range(1, 21).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("gtest-variance-{0:0}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        public void Test_Mono_Tests_Async()
        {
            // 2 - WaitAll not implemented
            // 4 - WaitAll not implemented
            // 6 - TODO: it works if compiled in DEBUG, but crashes when compile in RELEASE
            // 10 - WaitAll not implemented
            // 11 - WaitAll not implemented
            // 12 - WaitAll not implemented
            // 13 - GetMethods not implemented
            // 14 - WaitAll not implemented
            // 15 - WaitAll not implemented
            // 16 - GetMethods not implemented
            // 17 - GetMethods not implemented
            // 18 - WaitAll not implemented

            var skip = new List<int>(new[]
            {
                2,
                4,
                6,
                10,
                11,
                12,
                13,
                14,
                15,
                16,
                17,
                18
            });

            foreach (var index in Enumerable.Range(1, 48).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("test-async-{0:00}", index));
            }
        }

        /// <summary>
        /// </summary>
        [TestMethod]
        [Ignore]
        public void Test_Mono_DTests()
        {
            // 0

            var skip = new List<int>(new[]
            {
                0
            });

            foreach (var index in Enumerable.Range(1, 58).Where(n => !skip.Contains(n)))
            {
                CompilerHelper.CompileAndRun(string.Format("dtest-{0:000}", index));
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
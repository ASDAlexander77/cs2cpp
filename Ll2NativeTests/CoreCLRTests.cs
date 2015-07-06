namespace Ll2NativeTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Il2Native.Logic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PdbReader;

    namespace @Common.@Coreclr_TestWrapper
    {
        [TestClass]
        public class @testclass_Coreclr_TestWrapper
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @CoreclrTestWrapperLib()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\Coreclr.TestWrapper", "CoreclrTestWrapperLib.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Common.@CoreCLRTestLibrary
    {
        [TestClass]
        public class @testclass_CoreCLRTestLibrary
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @CalendarHelpers()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "CalendarHelpers.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @EndianessChecker()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "EndianessChecker.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Env()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "Env.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generator()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "Generator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @GlobLocHelper()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "GlobLocHelper.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Logging()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "Logging.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @TestFramework()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "TestFramework.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Utilities()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "Utilities.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Exceptions.@Finalization
    {
        [TestClass]
        public class @testclass_Finalization
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @Finalizer()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Exceptions\Finalization", "Finalizer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Interop.@ICastable
    {
        [TestClass]
        public class @testclass_ICastable
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_Castable()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Interop\ICastable", "Castable.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Interop.@ReversePInvoke.@Marshalling
    {
        [TestClass]
        public class @testclass_Marshalling
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @MarshalBoolArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Interop\ReversePInvoke\Marshalling", "MarshalBoolArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@CodeGenBringUpTests
    {
        [TestClass]
        public class @testclass_CodeGenBringUpTests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @Add1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Add1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @addref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "addref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @And1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "And1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @AndRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AndRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Args4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Args4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Args5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Args5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @AsgAdd1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgAdd1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @AsgAnd1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgAnd1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @AsgOr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgOr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @AsgSub1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgSub1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @AsgXor1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgXor1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @BinaryRMW()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "BinaryRMW.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Call1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Call1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @CnsBool()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "CnsBool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @CnsLng1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "CnsLng1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblAdd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblAdd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblAddConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblAddConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblArea()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblArea.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblAvg2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblAvg2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblAvg6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblAvg6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblCall1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblCall1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblCall2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblCall2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblDist()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblDist.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblDiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblDiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblDivConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblDivConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblFillArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblFillArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblMulConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblMulConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblNeg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblNeg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblRem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblRem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblRoots()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblRoots.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblSub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblSub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblSubConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblSubConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DblVar()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblVar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @div1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "div1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @div2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "div2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @divref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "divref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Eq1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Eq1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FactorialRec()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FactorialRec.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FibLoop()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FibLoop.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FiboRec()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FiboRec.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPAdd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPAdd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPAddConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPAddConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPArea()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPArea.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPAvg2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPAvg2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPAvg6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPAvg6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPCall1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPCall1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPCall2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPCall2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPConvDbl2Lng()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvDbl2Lng.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPConvF2F()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvF2F.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPConvF2I()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvF2I.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPConvF2Lng()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvF2Lng.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPConvI2F()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvI2F.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPDist()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPDist.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPDiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPDiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPDivConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPDivConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPError()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPError.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPFillArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPFillArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPMath()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPMath.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPMulConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPMulConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPNeg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPNeg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPRem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPRem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPRoots()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPRoots.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPSmall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPSmall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPSub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPSub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPSubConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPSubConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FPVar()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPVar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Gcd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Gcd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Ge1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Ge1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Gt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Gt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Ind1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Ind1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @InitObj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "InitObj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @InstanceCalls()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "InstanceCalls.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @IntArraySum()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "IntArraySum.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @IntConv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "IntConv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Jmp1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Jmp1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrue1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrue1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueEqDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueEqDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueEqFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueEqFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueEqInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueEqInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueGeDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGeDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueGeFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGeFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueGeInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGeInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueGtDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGtDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueGtFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGtFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueGtInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGtInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueLeDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLeDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueLeFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLeFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueLeInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLeInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueLtDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLtDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueLtFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLtFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueLtInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLtInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueNeDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueNeDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueNeFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueNeFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JTrueNeInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueNeInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Le1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Le1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @LeftShift()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "LeftShift.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @LngConv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "LngConv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Localloc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Localloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @LocallocLarge()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "LocallocLarge.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @LongArgsAndReturn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "LongArgsAndReturn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Lt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Lt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @mul1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "mul1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @mul2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "mul2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @mul3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "mul3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @mul4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "mul4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Ne1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Ne1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @NegRMW()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "NegRMW.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @NestedCall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "NestedCall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @NotAndNeg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "NotAndNeg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @NotRMW()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "NotRMW.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ObjAlloc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "ObjAlloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @OpMembersOfStructLocal()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "OpMembersOfStructLocal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Or1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Or1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @OrRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "OrRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @rem1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "rem1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @RightShiftRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "RightShiftRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @StaticCalls()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "StaticCalls.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @StaticValueField()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "StaticValueField.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @StructFldAddr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "StructFldAddr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @StructInstMethod()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "StructInstMethod.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Sub1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Sub1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SubRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "SubRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Swap()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Swap.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Switch()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Switch.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Unbox()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Unbox.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Xor1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Xor1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @XorRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "XorRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@Arrays
    {
        [TestClass]
        public class @testclass_Arrays
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @complex1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\Arrays", "complex1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @complex2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\Arrays", "complex2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @simple1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\Arrays", "simple1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @simple2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\Arrays", "simple2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@CheckedCtor
    {
        [TestClass]
        public class @testclass_CheckedCtor
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Base_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Base_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Base_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Base_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Base_6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Peer_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Peer_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Peer_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Peer_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Generic_Test_CSharp_Peer_6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Test_CSharp_Base_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Base_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Test_CSharp_Base_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Base_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Test_CSharp_Base_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Base_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Test_CSharp_Base_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Base_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Test_CSharp_Peer_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Peer_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Test_CSharp_Peer_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Peer_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Test_CSharp_Peer_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Peer_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Test_CSharp_Peer_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Peer_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@cmov
    {
        [TestClass]
        public class @testclass_cmov
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @Bool_And_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Bool_And_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Bool_No_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Bool_No_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Bool_Or_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Bool_Or_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Bool_Xor_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Bool_Xor_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Double_And_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Double_And_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Double_No_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Double_No_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Double_Or_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Double_Or_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Double_Xor_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Double_Xor_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Float_And_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Float_And_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Float_No_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Float_No_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Float_Or_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Float_Or_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Float_Xor_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Float_Xor_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Int_And_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Int_And_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Int_No_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Int_No_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Int_Or_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Int_Or_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Int_Xor_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Int_Xor_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@coverage.@flowgraph
    {
        [TestClass]
        public class @testclass_flowgraph
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @xaddmuly()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\flowgraph", "xaddmuly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@coverage.@oldtests
    {
        [TestClass]
        public class @testclass_oldtests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @_33objref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "33objref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @cse1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "cse1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @cse2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "cse2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lclfldadd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclfldadd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lclflddiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclflddiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lclfldmul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclfldmul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lclfldrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclfldrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lclfldsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclfldsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@intrinsic.@interlocked
    {
        [TestClass]
        public class @testclass_interlocked
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @cmpxchg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "cmpxchg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @cse_cmpxchg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "cse_cmpxchg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @IntrinsicTest_Overflow()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "IntrinsicTest_Overflow.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @nullchecksuppress()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "nullchecksuppress.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @regalloc1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "regalloc1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @regalloc2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "regalloc2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@intrinsic.@pow
    {
        [TestClass]
        public class @testclass_pow
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @pow0()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\pow", "pow0.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @pow1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\pow", "pow1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @pow2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\pow", "pow2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @pow3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\pow", "pow3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@lifetime
    {
        [TestClass]
        public class @testclass_lifetime
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @lifetime1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\lifetime", "lifetime1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lifetime2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\lifetime", "lifetime2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@localloc
    {
        [TestClass]
        public class @testclass_localloc
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @localloc3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\localloc", "localloc3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@newarr
    {
        [TestClass]
        public class @testclass_newarr
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_newarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\newarr", "newarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@shift
    {
        [TestClass]
        public class @testclass_shift
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @int16()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "int16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @int32()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "int32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @int64()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "int64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @uint16()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "uint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @uint32()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "uint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @uint64()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "uint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @uint8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "uint8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@StrAccess
    {
        [TestClass]
        public class @testclass_StrAccess
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @straccess1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StrAccess", "straccess1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @straccess2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StrAccess", "straccess2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @straccess3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StrAccess", "straccess3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @straccess4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StrAccess", "straccess4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@StructPromote
    {
        [TestClass]
        public class @testclass_StructPromote
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @SP1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP1a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP1a2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1a2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP1b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP1c()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1c.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP1d()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1d.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP2a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP2a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP2b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP2b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SP2c()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP2c.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SpAddr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SpAddr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SpAddrAT()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SpAddrAT.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@TypedReference
    {
        [TestClass]
        public class @testclass_TypedReference
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_TypedReference()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\TypedReference", "TypedReference.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Directed.@UnrollLoop
    {
        [TestClass]
        public class @testclass_UnrollLoop
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @Dev10_846218()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\UnrollLoop", "Dev10_846218.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @loop1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\UnrollLoop", "loop1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @loop4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\UnrollLoop", "loop4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Arrays.@ConstructedTypes.@Jagged
    {
        [TestClass]
        public class @testclass_Jagged
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class01_instance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class01_instance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class01_static()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class01_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class06()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class07()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struc01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struc01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "Struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct01_instance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "Struct01_instance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct01_static()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct01_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct06()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct07()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Arrays.@ConstructedTypes.@MultiDim
    {
        [TestClass]
        public class @testclass_MultiDim
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class01_Instance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "class01_Instance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class01_static()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "class01_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct01_Instance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "struct01_Instance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct01_static()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "struct01_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Arrays.@TypeParameters.@Jagged
    {
        [TestClass]
        public class @testclass_Jagged
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\TypeParameters\Jagged", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\TypeParameters\Jagged", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Arrays.@TypeParameters.@MultiDim
    {
        [TestClass]
        public class @testclass_MultiDim
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\TypeParameters\MultiDim", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\TypeParameters\MultiDim", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@ConstrainedCall
    {
        [TestClass]
        public class @testclass_ConstrainedCall
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @class1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "class1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "class2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @vt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "vt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @vt2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "vt2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @vt3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "vt3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @vt4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "vt4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Constraints
    {
        [TestClass]
        public class @testclass_Constraints
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @Call_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Call_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Call_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Call_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Convert_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Convert_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Convert_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Convert_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Transitive_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Transitive_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Transitive_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Transitive_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Conversions.@Boxing
    {
        [TestClass]
        public class @testclass_Boxing
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @box_unbox01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Boxing", "box_unbox01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Conversions.@Reference
    {
        [TestClass]
        public class @testclass_Reference
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @GenToGen01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToGen01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @GenToGen02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToGen02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @GenToGen03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToGen03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @GenToNonGen01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToNonGen01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @GenToNonGen02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToNonGen02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @GenToNonGen03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToNonGen03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @NonGenToGen01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "NonGenToGen01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @NonGenToGen02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "NonGenToGen02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @NonGenToGen03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "NonGenToGen03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Coverage
    {
        [TestClass]
        public class @testclass_Coverage
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @chaos55915408cs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Coverage", "chaos55915408cs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @chaos56200037cs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Coverage", "chaos56200037cs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @chaos65204782cs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Coverage", "chaos65204782cs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Exceptions
    {
        [TestClass]
        public class @testclass_Exceptions
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @general_class_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "general_class_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @general_class_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "general_class_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @general_struct_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "general_struct_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @general_struct_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "general_struct_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @specific_class_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_class_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @specific_class_instance02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_class_instance02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @specific_class_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_class_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @specific_class_static02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_class_static02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @specific_struct_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_struct_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @specific_struct_instance02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_struct_instance02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @specific_struct_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_struct_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @specific_struct_static02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_struct_static02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Fields
    {
        [TestClass]
        public class @testclass_Fields
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @instance_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Instantiation.@Classes
    {
        [TestClass]
        public class @testclass_Classes
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @BaseClass01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "BaseClass01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @BaseClass02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "BaseClass02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @BaseClass03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "BaseClass03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Baseclass04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "Baseclass04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Baseclass05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "Baseclass05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "class03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Instantiation.@Interfaces
    {
        [TestClass]
        public class @testclass_Interfaces
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @Class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Class03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Class04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Class05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Instantiation.@Structs
    {
        [TestClass]
        public class @testclass_Structs
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Structs", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Structs", "struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Structs", "struct03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Locals
    {
        [TestClass]
        public class @testclass_Locals
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @instance_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@MemberAccess
    {
        [TestClass]
        public class @testclass_MemberAccess
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @class_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "class_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "class_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @interface_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "interface_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @interface_class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "interface_class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @interface_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "interface_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @interface_struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "interface_struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "struct_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "struct_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Parameters
    {
        [TestClass]
        public class @testclass_Parameters
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @instance_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @instance_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @static_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@Typeof
    {
        [TestClass]
        public class @testclass_Typeof
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @class03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "class03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @dynamicTypes()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "dynamicTypes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @objectBoxing()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "objectBoxing.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @refTypesdynamic()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "refTypesdynamic.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "Struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "struct03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @valueTypeBoxing()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "valueTypeBoxing.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Generics.@TypeParameters
    {
        [TestClass]
        public class @testclass_TypeParameters
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @default_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\TypeParameters", "default_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @default_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\TypeParameters", "default_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@jit64.@gc.@misc
    {
        [TestClass]
        public class @testclass_misc
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @_148343()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "148343.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @_9param()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "9param.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @_9_and_alloca2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "9_and_alloca2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @eh1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "eh1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @fgtest1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "fgtest1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @fgtest2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "fgtest2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @funclet()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "funclet.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @gcparaminreg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "gcparaminreg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ret_struct_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "ret_struct_test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ret_struct_test4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "ret_struct_test4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @simple1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "simple1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct1_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct1_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct1_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct1_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct1_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct1_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct2_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct2_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct2_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct2_5_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2_5_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct3_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct3_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct3_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct3_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct3_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct3_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct4_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct4_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct4_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct4_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct4_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct4_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct5_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct5_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct5_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct5_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct5_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct5_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct6_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct6_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct6_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct6_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct6_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct6_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct7_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct7_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct9()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct9.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @struct9_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct9_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp1_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp1_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp1_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp1_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp1_6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp2_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp2_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp2_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp2_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp2_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp2_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp2_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp2_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp3_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp3_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp4_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp4_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp5_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp5_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfp6_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp6_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfpseh5_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfpseh5_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structfpseh6_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfpseh6_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structref1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structref1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret1_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret1_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret1_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret1_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret2_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret2_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret2_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret2_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret2_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret2_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret3_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret3_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret3_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret3_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret3_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret3_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret4_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret4_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret4_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret4_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret4_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret4_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret5_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret5_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret5_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret5_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret5_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret5_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret6_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret6_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret6_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret6_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structret6_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret6_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structva1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structva1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "test2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "test3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_noalloca()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "test_noalloca.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @vbil()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "vbil.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@jit64.@opt.@cg.@CGRecurse
    {
        [TestClass]
        public class @testclass_CGRecurse
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @CGRecurseAAA()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\CGRecurse", "CGRecurseAAA.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @CGRecurseAAC()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\CGRecurse", "CGRecurseAAC.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @CGRecurseACA()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\CGRecurse", "CGRecurseACA.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @CGRecurseACC()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\CGRecurse", "CGRecurseACC.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@jit64.@opt.@cse
    {
        [TestClass]
        public class @testclass_cse
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @arrayexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "arrayexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @arrayexpr2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "arrayexpr2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @fieldexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "fieldexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @fieldexpr1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "fieldexpr1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @fieldexpr2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "fieldexpr2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @fieldExprUnchecked1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "fieldExprUnchecked1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @HugeArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "HugeArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @HugeArray1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "HugeArray1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @hugeexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "hugeexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @HugeField1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "HugeField1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @HugeField2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "HugeField2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @hugeSimpleExpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "hugeSimpleExpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @mixedexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "mixedexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @pointerexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "pointerexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @pointerexpr1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "pointerexpr1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @simpleexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @simpleexpr1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @simpleexpr2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @simpleexpr3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @simpleexpr4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @staticFieldExpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "staticFieldExpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @staticFieldExpr1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "staticFieldExpr1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @staticFieldExprUnchecked1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "staticFieldExprUnchecked1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @volatilefield()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "volatilefield.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @volatilestaticfield()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "volatilestaticfield.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@jit64.@opt.@inl
    {
        [TestClass]
        public class @testclass_inl
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @caninline()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\inl", "caninline.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@jit64.@opt.@lim
    {
        [TestClass]
        public class @testclass_lim
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @lim_002()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\lim", "lim_002.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@jit64.@opt.@lur
    {
        [TestClass]
        public class @testclass_lur
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @lur_02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\lur", "lur_02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@jit64.@opt.@osr
    {
        [TestClass]
        public class @testclass_osr
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @osr001()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\osr", "osr001.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@jit64.@opt.@rngchk
    {
        [TestClass]
        public class @testclass_rngchk
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @ArrayBound()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "ArrayBound.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ArrayWith2Loops()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "ArrayWith2Loops.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ArrayWithFunc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "ArrayWithFunc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @BadMatrixMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "BadMatrixMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @JaggedArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "JaggedArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @MatrixMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "MatrixMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @SimpleArray_01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "SimpleArray_01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Arrays.@lcs
    {
        [TestClass]
        public class @testclass_lcs
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_lcs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcs2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcs2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcsbas()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsbas.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcsbox()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsbox.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcsmax()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsmax.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcsmixed()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsmixed.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcsval()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsval.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcsvalbox()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsvalbox.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Arrays.@misc
    {
        [TestClass]
        public class @testclass_misc
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @arrres()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\misc", "arrres.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @gcarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\misc", "gcarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @selfref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\misc", "selfref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@AsgOp.@i4
    {
        [TestClass]
        public class @testclass_i4
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_i4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\i4", "i4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @i4flat()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\i4", "i4flat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@AsgOp.@i8
    {
        [TestClass]
        public class @testclass_i8
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_i8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\i8", "i8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @i8flat()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\i8", "i8flat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@AsgOp.@r4
    {
        [TestClass]
        public class @testclass_r4
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_r4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\r4", "r4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r4flat()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\r4", "r4flat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@AsgOp.@r8
    {
        [TestClass]
        public class @testclass_r8
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_r8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\r8", "r8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r8flat()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\r8", "r8flat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Boxing.@misc
    {
        [TestClass]
        public class @testclass_misc
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @enum()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Boxing\misc", "enum.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @nestval()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Boxing\misc", "nestval.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @tailjump()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Boxing\misc", "tailjump.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@casts.@coverage
    {
        [TestClass]
        public class @testclass_coverage
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @castclass_call()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "castclass_call.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @castclass_ldarg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "castclass_ldarg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @castclass_ldloc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "castclass_ldloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @castclass_newobj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "castclass_newobj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @isinst_call()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "isinst_call.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @isinst_ldarg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "isinst_ldarg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @isinst_ldloc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "isinst_ldloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @isinst_newobj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "isinst_newobj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@casts.@iface
    {
        [TestClass]
        public class @testclass_iface
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @iface1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\iface", "iface1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@casts.@ilseq
    {
        [TestClass]
        public class @testclass_ilseq
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @commonBase()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\ilseq", "commonBase.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@casts.@SEH
    {
        [TestClass]
        public class @testclass_SEH
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @cast_throw()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\SEH", "cast_throw.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @throw()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\SEH", "throw.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@cctor.@simple
    {
        [TestClass]
        public class @testclass_simple
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @precise1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\cctor\simple", "precise1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @precise1b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\cctor\simple", "precise1b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @precise2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\cctor\simple", "precise2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @precise4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\cctor\simple", "precise4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Coverage
    {
        [TestClass]
        public class @testclass_Coverage
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @b433189()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Coverage", "b433189.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@divrem.@div
    {
        [TestClass]
        public class @testclass_div
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @decimaldiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "decimaldiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @i4div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "i4div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @i8div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "i8div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @negSignedMod()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "negSignedMod.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @overlddiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "overlddiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r4div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "r4div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r8div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "r8div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @u4div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "u4div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @u8div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "u8div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@divrem.@rem
    {
        [TestClass]
        public class @testclass_rem
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @decimalrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "decimalrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @i4rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "i4rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @i8rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "i8rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @overldrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "overldrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r4rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "r4rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r8rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "r8rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @u4rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "u4rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @u8rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "u8rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@eh.@interactions
    {
        [TestClass]
        public class @testclass_interactions
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @ehSO()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\eh\interactions", "ehSO.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @volatileFromFinally()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\eh\interactions", "volatileFromFinally.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@explicit.@basic
    {
        [TestClass]
        public class @testclass_basic
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @refarg_c()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_c.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @refarg_f4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_f4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @refarg_f8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_f8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @refarg_i1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_i1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @refarg_i2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_i2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @refarg_i4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_i4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @refarg_o()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @refarg_s()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_s.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@explicit.@misc
    {
        [TestClass]
        public class @testclass_misc
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @explicit1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @explicit2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @explicit3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @explicit4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @explicit5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @explicit6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @explicit7()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit7.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @explicit8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@explicit.@rotate
    {
        [TestClass]
        public class @testclass_rotate
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @rotarg_double()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\rotate", "rotarg_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @rotarg_float()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\rotate", "rotarg_float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @rotarg_objref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\rotate", "rotarg_objref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @rotarg_valref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\rotate", "rotarg_valref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@flowgraph.@bug614098
    {
        [TestClass]
        public class @testclass_bug614098
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @intToByte()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug614098", "intToByte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@flowgraph.@bug619534
    {
        [TestClass]
        public class @testclass_bug619534
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @ehCodeMotion()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug619534", "ehCodeMotion.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @finallyclone()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug619534", "finallyclone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @twoEndFinallys()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug619534", "twoEndFinallys.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@flowgraph.@bug621705
    {
        [TestClass]
        public class @testclass_bug621705
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @ptuple_lost()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug621705", "ptuple_lost.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@flowgraph.@bug647189
    {
        [TestClass]
        public class @testclass_bug647189
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @ssa_tuIsAddr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug647189", "ssa_tuIsAddr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@flowgraph.@dev10_bug675304
    {
        [TestClass]
        public class @testclass_dev10_bug675304
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @arrayDim()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug675304", "arrayDim.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @osrAddovershot()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug675304", "osrAddovershot.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@flowgraph.@dev10_bug679008
    {
        [TestClass]
        public class @testclass_dev10_bug679008
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @castClassEH()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "castClassEH.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @GCOverReporting()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "GCOverReporting.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @sealedCastVariance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "sealedCastVariance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @singleRefField()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "singleRefField.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @zeroInitStackSlot()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "zeroInitStackSlot.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@flowgraph.@dev10_bug723489
    {
        [TestClass]
        public class @testclass_dev10_bug723489
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @qMarkColon()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug723489", "qMarkColon.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@FPtrunc
    {
        [TestClass]
        public class @testclass_FPtrunc
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @convr4a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\FPtrunc", "convr4a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @convr8a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\FPtrunc", "convr8a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@inlining.@bug505642
    {
        [TestClass]
        public class @testclass_bug505642
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\inlining\bug505642", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@int64.@arrays
    {
        [TestClass]
        public class @testclass_arrays
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @lcs_long()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\arrays", "lcs_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcs_ulong()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\arrays", "lcs_ulong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@int64.@misc
    {
        [TestClass]
        public class @testclass_misc
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @binop()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\misc", "binop.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @box()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\misc", "box.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@int64.@signed
    {
        [TestClass]
        public class @testclass_signed
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @s_addsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_addsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @s_ldc_div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldc_div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @s_ldc_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldc_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @s_ldc_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldc_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @s_ldfld_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldfld_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @s_ldfld_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldfld_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @s_ldsfld_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldsfld_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @s_ldsfld_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldsfld_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @s_muldiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_muldiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@int64.@superlong
    {
        [TestClass]
        public class @testclass_superlong
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_superlong()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\superlong", "superlong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@int64.@unsigned
    {
        [TestClass]
        public class @testclass_unsigned
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @addsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "addsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ldc_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldc_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ldc_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldc_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ldfld_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldfld_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ldfld_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldfld_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ldsfld_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldsfld_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ldsfld_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldsfld_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @muldiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "muldiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Invoke.@_25params
    {
        [TestClass]
        public class @testclass__25params
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @_25param1a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\25params", "25param1a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @_25param2a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\25params", "25param2a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @_25param3a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\25params", "25param3a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Invoke.@callvirt
    {
        [TestClass]
        public class @testclass_callvirt
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\callvirt", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Invoke.@ctor
    {
        [TestClass]
        public class @testclass_ctor
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @val_ctor()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\ctor", "val_ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Invoke.@deep
    {
        [TestClass]
        public class @testclass_deep
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_deep()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\deep", "deep.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Invoke.@fptr
    {
        [TestClass]
        public class @testclass_fptr
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @recurse()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\fptr", "recurse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Invoke.@implicit
    {
        [TestClass]
        public class @testclass_implicit
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @obj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\implicit", "obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Invoke.@SEH
    {
        [TestClass]
        public class @testclass_SEH
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @catchfinally()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\SEH", "catchfinally.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @catchfinally_tail()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\SEH", "catchfinally_tail.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@MDArray.@basics
    {
        [TestClass]
        public class @testclass_basics
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @classarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "classarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @doublearr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "doublearr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @jaggedarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "jaggedarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @stringarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "stringarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "structarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@MDArray.@DataTypes
    {
        [TestClass]
        public class @testclass_DataTypes
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @bool()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @byte()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "byte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @char()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @decimal()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "decimal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @double()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @float()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @int()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @long()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @sbyte()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "sbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @short()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "short.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @uint()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "uint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ulong()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "ulong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ushort()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "ushort.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@MDArray.@GaussJordan
    {
        [TestClass]
        public class @testclass_GaussJordan
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @classarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\GaussJordan", "classarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @jaggedarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\GaussJordan", "jaggedarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @plainarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\GaussJordan", "plainarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\GaussJordan", "structarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@MDArray.@InnerProd
    {
        [TestClass]
        public class @testclass_InnerProd
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @classarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "classarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @doublearr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "doublearr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @intarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "intarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @jaggedarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "jaggedarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @stringarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "stringarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @structarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "structarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@NaN
    {
        [TestClass]
        public class @testclass_NaN
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @arithm32()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "arithm32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @arithm64()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "arithm64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r4NaNadd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNadd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r4NaNdiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNdiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r4NaNmul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNmul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r4NaNrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r4NaNsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r8NaNadd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNadd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r8NaNdiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNdiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r8NaNmul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNmul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r8NaNrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @r8NaNsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@nonvirtualcall
    {
        [TestClass]
        public class @testclass_nonvirtualcall
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @classic()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "classic.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @delegate()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "delegate.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @generics()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "generics.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @generics2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "generics2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @tailcall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "tailcall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @valuetype()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "valuetype.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@Overflow
    {
        [TestClass]
        public class @testclass_Overflow
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @FloatInfinitiesToInt()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Overflow", "FloatInfinitiesToInt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @FloatOvfToInt2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Overflow", "FloatOvfToInt2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@refany
    {
        [TestClass]
        public class @testclass_refany
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @array1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "array1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @array2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "array2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @format()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "format.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @gcreport()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "gcreport.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "lcs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @native()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "native.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @virtcall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "virtcall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@tailcall_v4
    {
        [TestClass]
        public class @testclass_tailcall_v4
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @delegateParamCallTarget()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\tailcall_v4", "delegateParamCallTarget.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@VT.@callconv
    {
        [TestClass]
        public class @testclass_callconv
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @call()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\callconv", "call.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @jumper()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\callconv", "jumper.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @jumps()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\callconv", "jumps.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @vtret()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\callconv", "vtret.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@VT.@etc
    {
        [TestClass]
        public class @testclass_etc
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @ctor_recurse()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "ctor_recurse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @gc_nested()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "gc_nested.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @han2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "han2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @han3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "han3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @han3_ctor()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "han3_ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @han3_ref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "han3_ref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @hanoi()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "hanoi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @knight()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "knight.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @nested()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "nested.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@VT.@identity
    {
        [TestClass]
        public class @testclass_identity
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @accum()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\identity", "accum.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @vcall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\identity", "vcall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@VT.@port
    {
        [TestClass]
        public class @testclass_port
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @lcs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\port", "lcs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @lcs_gcref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\port", "lcs_gcref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@Methodical.@xxobj.@operand
    {
        [TestClass]
        public class @testclass_operand
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @refanyval()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\xxobj\operand", "refanyval.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @unbox()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\xxobj\operand", "unbox.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@opt.@Inline
    {
        [TestClass]
        public class @testclass_Inline
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @args1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "args1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @args2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "args2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @args3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "args3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @array()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "array.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ArrayOfStructs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "ArrayOfStructs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @debug()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "debug.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @deepcall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "deepcall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DelegInstanceFtn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "DelegInstanceFtn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @DelegStaticFtn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "DelegStaticFtn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @GenericStructs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "GenericStructs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ifelse()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "ifelse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @indexer()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "indexer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @inline()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "inline.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @InlineThrow()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "InlineThrow.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_DelegateStruct()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_DelegateStruct.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_GenericMethods()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_GenericMethods.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_Handler()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_Handler.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @inline_Many()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "inline_Many.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_MultipleReturn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_MultipleReturn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_NewObj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_NewObj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_NormalizeStack()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_NormalizeStack.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @inline_Recursion()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "inline_Recursion.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_RecursiveMethod()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_RecursiveMethod.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_RecursiveMethod21()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_RecursiveMethod21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_SideAffects()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_SideAffects.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_STARG()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_STARG.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Inline_Vars()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_Vars.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @interfaceCall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "interfaceCall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @interfaceProperty()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "interfaceProperty.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @mathfunc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "mathfunc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @mthdimpl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "mthdimpl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @property()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "property.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ReturnStruct_Method()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "ReturnStruct_Method.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @size()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "size.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @StructAsParam_Method()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "StructAsParam_Method.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @StructInClass()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "StructInClass.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @Struct_Opcodes()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Struct_Opcodes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @throwTest()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "throwTest.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @trycatch()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "trycatch.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Loader.@typeinitialization
    {
        [TestClass]
        public class @testclass_typeinitialization
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @circularcctorthreethreads03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Loader\typeinitialization", "circularcctorthreethreads03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @managed.@Compilation
    {
        [TestClass]
        public class @testclass_Compilation
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_Compilation()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"managed\Compilation", "Compilation.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @HelloWorld()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"managed\Compilation", "HelloWorld.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Threading.@ThreadStatics
    {
        [TestClass]
        public class @testclass_ThreadStatics
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @ThreadStatic01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ThreadStatic02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ThreadStatic03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ThreadStatic05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @ThreadStatic06()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }
    }
}


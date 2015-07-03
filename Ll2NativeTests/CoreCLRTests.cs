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
    namespace @Common.@tests_Coreclr_TestWrapper
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
            public void @test_CoreclrTestWrapperLib()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\Coreclr.TestWrapper", "CoreclrTestWrapperLib.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Common.@tests_CoreCLRTestLibrary
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
            public void @test_CalendarHelpers()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "CalendarHelpers.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_EndianessChecker()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "EndianessChecker.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Env()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "Env.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generator()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "Generator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GlobLocHelper()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "GlobLocHelper.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Logging()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "Logging.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_TestFramework()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "TestFramework.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Utilities()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Common\CoreCLRTestLibrary", "Utilities.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Exceptions.@tests_Finalization
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
            public void @test_Finalizer()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Exceptions\Finalization", "Finalizer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @GC.@tests_Stress.@tests_Framework
    {
        [TestClass]
        public class @testclass_Framework
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
            public void @test_DetourHelpers()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Framework", "DetourHelpers.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_key_v1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Framework", "key_v1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_LoaderClass()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Framework", "LoaderClass.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ReliabilityConfiguration()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Framework", "ReliabilityConfiguration.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ReliabilityFramework()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Framework", "ReliabilityFramework.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ReliabilityTest()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Framework", "ReliabilityTest.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ReliabilityTestSet()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Framework", "ReliabilityTestSet.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_RFLogging()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Framework", "RFLogging.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @GC.@tests_Stress.@tests_Tests
    {
        [TestClass]
        public class @testclass_Tests
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
            public void @test_573277()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "573277.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_allocationwithpins()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "allocationwithpins.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_b115557()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "b115557.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_bestfit_finalize()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "bestfit-finalize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_concurrentspin2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "concurrentspin2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_doubLinkStay()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "doubLinkStay.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ExpandHeap()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "ExpandHeap.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GCQueue()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "GCQueue.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GCSimulator()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "GCSimulator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GCVariant()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "GCVariant.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_LeakGenThrd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "LeakGenThrd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_MulDimJagAry()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "MulDimJagAry.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_pinstress()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "pinstress.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_plug()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "plug.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_PlugGaps()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "PlugGaps.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SingLinkStay()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "SingLinkStay.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_StressAllocator()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "StressAllocator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ThdTreeGrowingObj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"GC\Stress\Tests", "ThdTreeGrowingObj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Interop.@tests_ICastable
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

    namespace @Interop.@tests_ReversePInvoke.@tests_Marshalling
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
            public void @test_MarshalBoolArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Interop\ReversePInvoke\Marshalling", "MarshalBoolArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_CodeGenBringUpTests
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
            public void @test_Add1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Add1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_addref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "addref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_And1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "And1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_AndRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AndRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Args4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Args4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Args5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Args5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_AsgAdd1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgAdd1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_AsgAnd1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgAnd1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_AsgOr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgOr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_AsgSub1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgSub1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_AsgXor1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "AsgXor1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_BinaryRMW()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "BinaryRMW.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Call1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Call1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_CnsBool()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "CnsBool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_CnsLng1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "CnsLng1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblAdd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblAdd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblAddConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblAddConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblArea()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblArea.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblAvg2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblAvg2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblAvg6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblAvg6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblCall1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblCall1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblCall2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblCall2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblDist()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblDist.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblDiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblDiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblDivConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblDivConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblFillArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblFillArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblMulConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblMulConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblNeg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblNeg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblRem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblRem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblRoots()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblRoots.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblSub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblSub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblSubConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblSubConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DblVar()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "DblVar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_div1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "div1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_div2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "div2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_divref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "divref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Eq1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Eq1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FactorialRec()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FactorialRec.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FibLoop()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FibLoop.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FiboRec()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FiboRec.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPAdd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPAdd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPAddConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPAddConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPArea()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPArea.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPAvg2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPAvg2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPAvg6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPAvg6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPCall1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPCall1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPCall2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPCall2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPConvDbl2Lng()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvDbl2Lng.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPConvF2F()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvF2F.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPConvF2I()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvF2I.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPConvF2Lng()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvF2Lng.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPConvI2F()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPConvI2F.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPDist()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPDist.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPDiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPDiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPDivConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPDivConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPError()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPError.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPFillArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPFillArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPMath()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPMath.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPMulConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPMulConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPNeg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPNeg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPRem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPRem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPRoots()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPRoots.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPSmall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPSmall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPSub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPSub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPSubConst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPSubConst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FPVar()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "FPVar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Gcd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Gcd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Ge1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Ge1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Gt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Gt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Ind1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Ind1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_InitObj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "InitObj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_InstanceCalls()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "InstanceCalls.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_IntArraySum()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "IntArraySum.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_IntConv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "IntConv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Jmp1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Jmp1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrue1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrue1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueEqDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueEqDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueEqFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueEqFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueEqInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueEqInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueGeDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGeDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueGeFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGeFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueGeInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGeInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueGtDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGtDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueGtFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGtFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueGtInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueGtInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueLeDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLeDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueLeFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLeFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueLeInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLeInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueLtDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLtDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueLtFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLtFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueLtInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueLtInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueNeDbl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueNeDbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueNeFP()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueNeFP.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JTrueNeInt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "JTrueNeInt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Le1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Le1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_LeftShift()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "LeftShift.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_LngConv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "LngConv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Localloc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Localloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_LocallocLarge()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "LocallocLarge.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_LongArgsAndReturn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "LongArgsAndReturn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Lt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Lt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_mul1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "mul1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_mul2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "mul2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_mul3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "mul3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_mul4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "mul4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Ne1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Ne1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_NegRMW()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "NegRMW.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_NestedCall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "NestedCall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_NotAndNeg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "NotAndNeg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_NotRMW()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "NotRMW.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ObjAlloc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "ObjAlloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_OpMembersOfStructLocal()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "OpMembersOfStructLocal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Or1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Or1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_OrRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "OrRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_rem1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "rem1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_RightShiftRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "RightShiftRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_StaticCalls()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "StaticCalls.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_StaticValueField()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "StaticValueField.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_StructFldAddr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "StructFldAddr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_StructInstMethod()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "StructInstMethod.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Sub1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Sub1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SubRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "SubRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Swap()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Swap.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Switch()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Switch.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Unbox()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Unbox.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Xor1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "Xor1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_XorRef()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\CodeGenBringUpTests", "XorRef.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_Arrays
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
            public void @test_complex1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\Arrays", "complex1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_complex2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\Arrays", "complex2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_simple1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\Arrays", "simple1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_simple2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\Arrays", "simple2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_CheckedCtor
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
            public void @test_Generic_Test_CSharp_Base_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Base_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Base_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Base_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Base_6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Base_6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Peer_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Peer_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Peer_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Peer_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Generic_Test_CSharp_Peer_6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Generic_Test_CSharp_Peer_6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Test_CSharp_Base_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Base_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Test_CSharp_Base_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Base_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Test_CSharp_Base_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Base_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Test_CSharp_Base_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Base_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Test_CSharp_Peer_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Peer_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Test_CSharp_Peer_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Peer_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Test_CSharp_Peer_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Peer_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Test_CSharp_Peer_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\CheckedCtor", "Test_CSharp_Peer_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_cmov
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
            public void @test_Bool_And_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Bool_And_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Bool_No_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Bool_No_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Bool_Or_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Bool_Or_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Bool_Xor_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Bool_Xor_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Double_And_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Double_And_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Double_No_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Double_No_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Double_Or_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Double_Or_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Double_Xor_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Double_Xor_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Float_And_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Float_And_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Float_No_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Float_No_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Float_Or_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Float_Or_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Float_Xor_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Float_Xor_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Int_And_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Int_And_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Int_No_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Int_No_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Int_Or_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Int_Or_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Int_Xor_Op()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\cmov", "Int_Xor_Op.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_coverage.@tests_flowgraph
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
            public void @test_xaddmuly()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\flowgraph", "xaddmuly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_coverage.@tests_oldtests
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
            public void @test_33objref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "33objref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_cse1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "cse1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_cse2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "cse2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lclfldadd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclfldadd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lclflddiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclflddiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lclfldmul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclfldmul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lclfldrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclfldrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lclfldsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\coverage\oldtests", "lclfldsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_intrinsic.@tests_interlocked
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
            public void @test_cmpxchg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "cmpxchg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_cse_cmpxchg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "cse_cmpxchg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_IntrinsicTest_Overflow()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "IntrinsicTest_Overflow.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_nullchecksuppress()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "nullchecksuppress.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_regalloc1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "regalloc1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_regalloc2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\interlocked", "regalloc2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_intrinsic.@tests_pow
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
            public void @test_pow0()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\pow", "pow0.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_pow1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\pow", "pow1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_pow2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\pow", "pow2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_pow3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\intrinsic\pow", "pow3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_lifetime
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
            public void @test_lifetime1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\lifetime", "lifetime1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lifetime2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\lifetime", "lifetime2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_localloc
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
            public void @test_localloc3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\localloc", "localloc3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_newarr
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

    namespace @JIT.@tests_Directed.@tests_shift
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
            public void @test_int16()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "int16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_int32()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "int32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_int64()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "int64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_uint16()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "uint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_uint32()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "uint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_uint64()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "uint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_uint8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\shift", "uint8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_StrAccess
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
            public void @test_straccess1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StrAccess", "straccess1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_straccess2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StrAccess", "straccess2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_straccess3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StrAccess", "straccess3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_straccess4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StrAccess", "straccess4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_StructPromote
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
            public void @test_SP1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP1a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP1a2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1a2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP1b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP1c()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1c.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP1d()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP1d.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP2a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP2a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP2b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP2b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SP2c()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SP2c.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SpAddr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SpAddr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SpAddrAT()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\StructPromote", "SpAddrAT.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Directed.@tests_TypedReference
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

    namespace @JIT.@tests_Directed.@tests_UnrollLoop
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
            public void @test_Dev10_846218()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\UnrollLoop", "Dev10_846218.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_loop1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\UnrollLoop", "loop1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_loop4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Directed\UnrollLoop", "loop4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Arrays.@tests_ConstructedTypes.@tests_Jagged
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
            public void @test_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class01_instance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class01_instance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class01_static()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class01_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class06()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class07()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "class07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struc01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struc01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "Struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct01_instance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "Struct01_instance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct01_static()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct01_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct06()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct07()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\Jagged", "struct07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Arrays.@tests_ConstructedTypes.@tests_MultiDim
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
            public void @test_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class01_Instance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "class01_Instance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class01_static()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "class01_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct01_Instance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "struct01_Instance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct01_static()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\ConstructedTypes\MultiDim", "struct01_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Arrays.@tests_TypeParameters.@tests_Jagged
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
            public void @test_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\TypeParameters\Jagged", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\TypeParameters\Jagged", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Arrays.@tests_TypeParameters.@tests_MultiDim
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
            public void @test_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\TypeParameters\MultiDim", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Arrays\TypeParameters\MultiDim", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_ConstrainedCall
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
            public void @test_class1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "class1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "class2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vt1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "vt1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vt2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "vt2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vt3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "vt3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vt4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\ConstrainedCall", "vt4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Constraints
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
            public void @test_Call_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Call_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Call_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Call_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Convert_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Convert_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Convert_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Convert_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Transitive_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Transitive_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Transitive_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Constraints", "Transitive_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Conversions.@tests_Boxing
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
            public void @test_box_unbox01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Boxing", "box_unbox01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Conversions.@tests_Reference
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
            public void @test_GenToGen01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToGen01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GenToGen02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToGen02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GenToGen03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToGen03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GenToNonGen01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToNonGen01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GenToNonGen02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToNonGen02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GenToNonGen03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "GenToNonGen03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_NonGenToGen01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "NonGenToGen01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_NonGenToGen02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "NonGenToGen02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_NonGenToGen03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Conversions\Reference", "NonGenToGen03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Coverage
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
            public void @test_chaos55915408cs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Coverage", "chaos55915408cs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_chaos56200037cs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Coverage", "chaos56200037cs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_chaos65204782cs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Coverage", "chaos65204782cs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Exceptions
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
            public void @test_general_class_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "general_class_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_general_class_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "general_class_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_general_struct_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "general_struct_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_general_struct_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "general_struct_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_specific_class_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_class_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_specific_class_instance02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_class_instance02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_specific_class_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_class_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_specific_class_static02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_class_static02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_specific_struct_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_struct_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_specific_struct_instance02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_struct_instance02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_specific_struct_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_struct_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_specific_struct_static02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Exceptions", "specific_struct_static02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Fields
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
            public void @test_instance_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "instance_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Fields", "static_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Instantiation.@tests_Classes
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
            public void @test_BaseClass01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "BaseClass01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_BaseClass02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "BaseClass02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_BaseClass03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "BaseClass03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Baseclass04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "Baseclass04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Baseclass05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "Baseclass05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Classes", "class03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Instantiation.@tests_Interfaces
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
            public void @test_Class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Class03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Class04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Class05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Class05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct04()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Interfaces", "Struct05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Instantiation.@tests_Structs
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
            public void @test_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Structs", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Structs", "struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Instantiation\Structs", "struct03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Locals
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
            public void @test_instance_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "instance_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Locals", "static_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_MemberAccess
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
            public void @test_class_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "class_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "class_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_interface_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "interface_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_interface_class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "interface_class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_interface_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "interface_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_interface_struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "interface_struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct_instance01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "struct_instance01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct_static01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\MemberAccess", "struct_static01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Parameters
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
            public void @test_instance_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_instance_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "instance_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_assignment_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_assignment_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_assignment_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_assignment_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_equalnull_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_equalnull_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_equalnull_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_equalnull_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_passing_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_passing_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_static_passing_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Parameters", "static_passing_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_Typeof
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
            public void @test_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "class02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_class03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "class03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_dynamicTypes()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "dynamicTypes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_objectBoxing()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "objectBoxing.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_refTypesdynamic()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "refTypesdynamic.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "Struct02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "struct03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_valueTypeBoxing()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\Typeof", "valueTypeBoxing.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Generics.@tests_TypeParameters
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
            public void @test_default_class01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\TypeParameters", "default_class01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_default_struct01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Generics\TypeParameters", "default_struct01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_gc.@tests_misc
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
            public void @test_148343()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "148343.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_9param()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "9param.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_9_and_alloca2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "9_and_alloca2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_eh1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "eh1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_fgtest1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "fgtest1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_fgtest2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "fgtest2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_funclet()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "funclet.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_gcparaminreg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "gcparaminreg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ret_struct_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "ret_struct_test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ret_struct_test4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "ret_struct_test4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_simple1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "simple1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct1_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct1_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct1_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct1_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct1_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct1_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct2_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct2_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct2_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct2_5_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct2_5_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct3_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct3_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct3_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct3_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct3_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct3_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct4_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct4_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct4_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct4_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct4_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct4_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct5_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct5_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct5_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct5_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct5_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct5_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct6_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct6_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct6_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct6_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct6_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct6_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct7_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct7_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct9()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct9.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_struct9_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "struct9_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp1_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp1_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp1_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp1_5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp1_6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp1_6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp2_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp2_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp2_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp2_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp2_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp2_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp2_4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp2_4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp3_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp3_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp4_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp4_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp5_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp5_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfp6_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfp6_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfpseh5_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfpseh5_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structfpseh6_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structfpseh6_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structref1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structref1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret1_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret1_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret1_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret1_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret2_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret2_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret2_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret2_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret2_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret2_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret3_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret3_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret3_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret3_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret3_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret3_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret4_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret4_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret4_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret4_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret4_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret4_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret5_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret5_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret5_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret5_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret5_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret5_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret6_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret6_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret6_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret6_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structret6_3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structret6_3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structva1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "structva1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_test2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "test2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_test3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "test3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_test_noalloca()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "test_noalloca.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vbil()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\misc", "vbil.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_gc.@tests_regress.@tests_vswhidbey
    {
        [TestClass]
        public class @testclass_vswhidbey
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
            public void @test_143837()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\regress\vswhidbey", "143837.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_339415()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\gc\regress\vswhidbey", "339415.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_opt.@tests_cg.@tests_CGRecurse
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
            public void @test_CGRecurseAAA()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\CGRecurse", "CGRecurseAAA.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_CGRecurseAAC()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\CGRecurse", "CGRecurseAAC.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_CGRecurseACA()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\CGRecurse", "CGRecurseACA.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_CGRecurseACC()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\CGRecurse", "CGRecurseACC.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_opt.@tests_cg.@tests_cgstress
    {
        [TestClass]
        public class @testclass_cgstress
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
            public void @test_CgStress1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\cgstress", "CgStress1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_CgStress2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\cgstress", "CgStress2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_CgStress3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cg\cgstress", "CgStress3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_opt.@tests_cse
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
            public void @test_arrayexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "arrayexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_arrayexpr2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "arrayexpr2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_fieldexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "fieldexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_fieldexpr1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "fieldexpr1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_fieldexpr2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "fieldexpr2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_fieldExprUnchecked1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "fieldExprUnchecked1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_HugeArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "HugeArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_HugeArray1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "HugeArray1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_hugeexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "hugeexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_HugeField1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "HugeField1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_HugeField2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "HugeField2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_hugeSimpleExpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "hugeSimpleExpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_mixedexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "mixedexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_pointerexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "pointerexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_pointerexpr1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "pointerexpr1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_simpleexpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_simpleexpr1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_simpleexpr2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_simpleexpr3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_simpleexpr4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "simpleexpr4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_staticFieldExpr1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "staticFieldExpr1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_staticFieldExpr1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "staticFieldExpr1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_staticFieldExprUnchecked1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "staticFieldExprUnchecked1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_volatilefield()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "volatilefield.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_volatilestaticfield()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\cse", "volatilestaticfield.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_opt.@tests_inl
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
            public void @test_caninline()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\inl", "caninline.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_opt.@tests_lim
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
            public void @test_lim_002()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\lim", "lim_002.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_opt.@tests_lur
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
            public void @test_lur_02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\lur", "lur_02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_opt.@tests_osr
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
            public void @test_osr001()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\osr", "osr001.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_opt.@tests_rngchk
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
            public void @test_ArrayBound()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "ArrayBound.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ArrayWith2Loops()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "ArrayWith2Loops.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ArrayWithFunc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "ArrayWithFunc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_BadMatrixMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "BadMatrixMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_JaggedArray()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "JaggedArray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_MatrixMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "MatrixMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_RngchkStress1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "RngchkStress1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_RngchkStress2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "RngchkStress2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_RngchkStress3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "RngchkStress3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_SimpleArray_01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\opt\rngchk", "SimpleArray_01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_asurt.@tests_143616
    {
        [TestClass]
        public class @testclass_143616
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
            public void @test_foo()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\asurt\143616", "foo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_ddb.@tests_103087
    {
        [TestClass]
        public class @testclass_103087
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
            public void @test_103087()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\ddb\103087", "103087.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_ddb.@tests_113574
    {
        [TestClass]
        public class @testclass_113574
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
            public void @test_113574()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\ddb\113574", "113574.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_ddb.@tests_132977
    {
        [TestClass]
        public class @testclass_132977
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
            public void @test_132977()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\ddb\132977", "132977.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_ndpw.@tests_160545
    {
        [TestClass]
        public class @testclass_160545
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
            public void @test_simple()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\ndpw\160545", "simple.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_ndpw.@tests_21015
    {
        [TestClass]
        public class @testclass_21015
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
            public void @test_interior_pointer()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\ndpw\21015", "interior_pointer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_102964
    {
        [TestClass]
        public class @testclass_102964
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\102964", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_329169
    {
        [TestClass]
        public class @testclass_329169
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\329169", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_373472
    {
        [TestClass]
        public class @testclass_373472
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\373472", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_471729
    {
        [TestClass]
        public class @testclass_471729
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\471729", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_517867
    {
        [TestClass]
        public class @testclass_517867
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\517867", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_524070
    {
        [TestClass]
        public class @testclass_524070
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
            public void @test_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\524070", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_test2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\524070", "test2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_528315
    {
        [TestClass]
        public class @testclass_528315
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
            public void @test_simple_repro()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\528315", "simple-repro.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_538615
    {
        [TestClass]
        public class @testclass_538615
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
            public void @test_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\538615", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_539509
    {
        [TestClass]
        public class @testclass_539509
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
            public void @test_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\539509", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_541067
    {
        [TestClass]
        public class @testclass_541067
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
            public void @test_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\541067", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_543229
    {
        [TestClass]
        public class @testclass_543229
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\543229", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_549880
    {
        [TestClass]
        public class @testclass_549880
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\549880", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_601425
    {
        [TestClass]
        public class @testclass_601425
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
            public void @test_stret()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\601425", "stret.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_jit64.@tests_regress.@tests_vsw.@tests_610378
    {
        [TestClass]
        public class @testclass_610378
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
            public void @test_BigFrame()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\jit64\regress\vsw\610378", "BigFrame.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Arrays.@tests_lcs
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
            public void @test_lcs2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcs2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcsbas()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsbas.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcsbox()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsbox.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcsmax()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsmax.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcsmixed()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsmixed.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcsval()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsval.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcsvalbox()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\lcs", "lcsvalbox.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Arrays.@tests_misc
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
            public void @test_arrres()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\misc", "arrres.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_gcarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\misc", "gcarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_selfref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Arrays\misc", "selfref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_AsgOp.@tests_i4
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
            public void @test_i4flat()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\i4", "i4flat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_AsgOp.@tests_i8
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
            public void @test_i8flat()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\i8", "i8flat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_AsgOp.@tests_r4
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
            public void @test_r4flat()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\r4", "r4flat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_AsgOp.@tests_r8
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
            public void @test_r8flat()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\AsgOp\r8", "r8flat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Boxing.@tests_misc
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
            public void @test_enum()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Boxing\misc", "enum.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_nestval()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Boxing\misc", "nestval.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_tailjump()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Boxing\misc", "tailjump.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_casts.@tests_coverage
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
            public void @test_castclass_call()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "castclass_call.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_castclass_ldarg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "castclass_ldarg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_castclass_ldloc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "castclass_ldloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_castclass_newobj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "castclass_newobj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_isinst_call()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "isinst_call.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_isinst_ldarg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "isinst_ldarg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_isinst_ldloc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "isinst_ldloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_isinst_newobj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\coverage", "isinst_newobj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_casts.@tests_iface
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
            public void @test_iface1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\iface", "iface1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_casts.@tests_ilseq
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
            public void @test_commonBase()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\ilseq", "commonBase.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_casts.@tests_SEH
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
            public void @test_cast_throw()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\SEH", "cast_throw.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_throw()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\casts\SEH", "throw.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_cctor.@tests_simple
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
            public void @test_precise1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\cctor\simple", "precise1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_precise1b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\cctor\simple", "precise1b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_precise2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\cctor\simple", "precise2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_precise4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\cctor\simple", "precise4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Coverage
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
            public void @test_b433189()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Coverage", "b433189.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_divrem.@tests_div
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
            public void @test_decimaldiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "decimaldiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_i4div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "i4div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_i8div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "i8div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_negSignedMod()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "negSignedMod.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_overlddiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "overlddiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r4div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "r4div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r8div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "r8div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_u4div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "u4div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_u8div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\div", "u8div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_divrem.@tests_rem
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
            public void @test_decimalrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "decimalrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_i4rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "i4rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_i8rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "i8rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_overldrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "overldrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r4rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "r4rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r8rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "r8rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_u4rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "u4rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_u8rem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\divrem\rem", "u8rem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_eh.@tests_interactions
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
            public void @test_ehSO()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\eh\interactions", "ehSO.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_volatileFromFinally()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\eh\interactions", "volatileFromFinally.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_explicit.@tests_basic
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
            public void @test_refarg_c()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_c.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_refarg_f4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_f4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_refarg_f8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_f8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_refarg_i1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_i1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_refarg_i2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_i2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_refarg_i4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_i4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_refarg_o()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_refarg_s()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\basic", "refarg_s.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_explicit.@tests_misc
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
            public void @test_explicit1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_explicit2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_explicit3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_explicit4()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_explicit5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_explicit6()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_explicit7()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit7.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_explicit8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\misc", "explicit8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_explicit.@tests_rotate
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
            public void @test_rotarg_double()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\rotate", "rotarg_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_rotarg_float()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\rotate", "rotarg_float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_rotarg_objref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\rotate", "rotarg_objref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_rotarg_valref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\explicit\rotate", "rotarg_valref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_flowgraph.@tests_bug614098
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
            public void @test_intToByte()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug614098", "intToByte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_flowgraph.@tests_bug619534
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
            public void @test_ehCodeMotion()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug619534", "ehCodeMotion.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_finallyclone()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug619534", "finallyclone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_twoEndFinallys()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug619534", "twoEndFinallys.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_flowgraph.@tests_bug621705
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
            public void @test_ptuple_lost()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug621705", "ptuple_lost.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_flowgraph.@tests_bug647189
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
            public void @test_ssa_tuIsAddr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\bug647189", "ssa_tuIsAddr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_flowgraph.@tests_dev10_bug675304
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
            public void @test_arrayDim()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug675304", "arrayDim.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_osrAddovershot()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug675304", "osrAddovershot.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_flowgraph.@tests_dev10_bug679008
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
            public void @test_castClassEH()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "castClassEH.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GCOverReporting()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "GCOverReporting.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_sealedCastVariance()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "sealedCastVariance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_singleRefField()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "singleRefField.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_zeroInitStackSlot()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug679008", "zeroInitStackSlot.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_flowgraph.@tests_dev10_bug723489
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
            public void @test_qMarkColon()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\flowgraph\dev10_bug723489", "qMarkColon.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_FPtrunc
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
            public void @test_convr4a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\FPtrunc", "convr4a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_convr8a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\FPtrunc", "convr8a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_inlining.@tests_bug505642
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\inlining\bug505642", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_int64.@tests_arrays
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
            public void @test_lcs_long()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\arrays", "lcs_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcs_ulong()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\arrays", "lcs_ulong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_int64.@tests_misc
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
            public void @test_binop()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\misc", "binop.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_box()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\misc", "box.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_int64.@tests_signed
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
            public void @test_s_addsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_addsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_s_ldc_div()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldc_div.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_s_ldc_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldc_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_s_ldc_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldc_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_s_ldfld_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldfld_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_s_ldfld_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldfld_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_s_ldsfld_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldsfld_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_s_ldsfld_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_ldsfld_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_s_muldiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\signed", "s_muldiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_int64.@tests_superlong
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

    namespace @JIT.@tests_Methodical.@tests_int64.@tests_unsigned
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
            public void @test_addsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "addsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ldc_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldc_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ldc_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldc_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ldfld_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldfld_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ldfld_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldfld_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ldsfld_mul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldsfld_mul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ldsfld_mulovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "ldsfld_mulovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_muldiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\int64\unsigned", "muldiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Invoke.@tests_25params
    {
        [TestClass]
        public class @testclass_25params
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
            public void @test_25param1a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\25params", "25param1a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_25param2a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\25params", "25param2a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_25param3a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\25params", "25param3a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Invoke.@tests_callvirt
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
            public void @test_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\callvirt", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Invoke.@tests_ctor
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
            public void @test_val_ctor()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\ctor", "val_ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Invoke.@tests_deep
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

    namespace @JIT.@tests_Methodical.@tests_Invoke.@tests_fptr
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
            public void @test_recurse()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\fptr", "recurse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Invoke.@tests_implicit
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
            public void @test_obj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\implicit", "obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Invoke.@tests_SEH
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
            public void @test_catchfinally()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\SEH", "catchfinally.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_catchfinally_tail()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Invoke\SEH", "catchfinally_tail.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_MDArray.@tests_basics
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
            public void @test_classarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "classarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_doublearr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "doublearr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_jaggedarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "jaggedarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_stringarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "stringarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\basics", "structarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_MDArray.@tests_DataTypes
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
            public void @test_bool()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_byte()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "byte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_char()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_decimal()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "decimal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_double()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_float()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_int()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_long()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_sbyte()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "sbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_short()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "short.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_uint()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "uint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ulong()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "ulong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ushort()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\DataTypes", "ushort.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_MDArray.@tests_GaussJordan
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
            public void @test_classarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\GaussJordan", "classarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_jaggedarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\GaussJordan", "jaggedarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_plainarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\GaussJordan", "plainarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\GaussJordan", "structarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_MDArray.@tests_InnerProd
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
            public void @test_classarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "classarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_doublearr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "doublearr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_intarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "intarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_jaggedarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "jaggedarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_stringarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "stringarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_structarr()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\MDArray\InnerProd", "structarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_NaN
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
            public void @test_arithm32()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "arithm32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_arithm64()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "arithm64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r4NaNadd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNadd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r4NaNdiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNdiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r4NaNmul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNmul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r4NaNrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r4NaNsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r4NaNsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r8NaNadd()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNadd.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r8NaNdiv()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNdiv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r8NaNmul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNmul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r8NaNrem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNrem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_r8NaNsub()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\NaN", "r8NaNsub.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_nonvirtualcall
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
            public void @test_classic()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "classic.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_delegate()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "delegate.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_generics()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "generics.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_generics2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "generics2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_tailcall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "tailcall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_valuetype()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\nonvirtualcall", "valuetype.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_Overflow
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
            public void @test_FloatInfinitiesToInt()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Overflow", "FloatInfinitiesToInt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_FloatOvfToInt2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\Overflow", "FloatOvfToInt2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_refany
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
            public void @test_array1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "array1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_array2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "array2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_format()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "format.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_gcreport()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "gcreport.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "lcs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_native()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "native.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_virtcall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\refany", "virtcall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_tailcall_v4
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
            public void @test_delegateParamCallTarget()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\tailcall_v4", "delegateParamCallTarget.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_VT.@tests_callconv
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
            public void @test_call()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\callconv", "call.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_jumper()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\callconv", "jumper.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_jumps()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\callconv", "jumps.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vtret()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\callconv", "vtret.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_VT.@tests_etc
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
            public void @test_ctor_recurse()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "ctor_recurse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_gc_nested()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "gc_nested.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_han2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "han2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_han3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "han3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_han3_ctor()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "han3_ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_han3_ref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "han3_ref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_hanoi()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "hanoi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_knight()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "knight.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_nested()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\etc", "nested.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_VT.@tests_identity
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
            public void @test_accum()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\identity", "accum.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vcall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\identity", "vcall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_VT.@tests_port
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
            public void @test_lcs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\port", "lcs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_lcs_gcref()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\VT\port", "lcs_gcref.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Methodical.@tests_xxobj.@tests_operand
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
            public void @test_refanyval()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\xxobj\operand", "refanyval.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_unbox()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Methodical\xxobj\operand", "unbox.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_opt.@tests_Inline
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
            public void @test_args1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "args1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_args2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "args2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_args3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "args3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_array()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "array.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ArrayOfStructs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "ArrayOfStructs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_debug()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "debug.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_deepcall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "deepcall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DelegInstanceFtn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "DelegInstanceFtn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DelegStaticFtn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "DelegStaticFtn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_GenericStructs()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "GenericStructs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ifelse()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "ifelse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_indexer()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "indexer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_inline()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "inline.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_InlineThrow()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "InlineThrow.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_DelegateStruct()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_DelegateStruct.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_GenericMethods()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_GenericMethods.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_Handler()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_Handler.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_inline_Many()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "inline_Many.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_MultipleReturn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_MultipleReturn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_NewObj()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_NewObj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_NormalizeStack()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_NormalizeStack.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_inline_Recursion()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "inline_Recursion.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_RecursiveMethod()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_RecursiveMethod.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_RecursiveMethod21()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_RecursiveMethod21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_SideAffects()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_SideAffects.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_STARG()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_STARG.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Inline_Vars()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Inline_Vars.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_interfaceCall()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "interfaceCall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_interfaceProperty()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "interfaceProperty.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_mathfunc()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "mathfunc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_mthdimpl()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "mthdimpl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_property()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "property.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ReturnStruct_Method()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "ReturnStruct_Method.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_size()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "size.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_StructAsParam_Method()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "StructAsParam_Method.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_StructInClass()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "StructInClass.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Struct_Opcodes()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "Struct_Opcodes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_throwTest()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "throwTest.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_trycatch()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\opt\Inline", "trycatch.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_clr_x64_JIT.@tests_v4_0.@tests_DevDiv34372
    {
        [TestClass]
        public class @testclass_DevDiv34372
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
            public void @test_overRepLocalOpt()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\clr-x64-JIT\v4.0\DevDiv34372", "overRepLocalOpt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_EJIT.@tests_V1_M09_5_PDC.@tests_b12008
    {
        [TestClass]
        public class @testclass_b12008
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
            public void @test_b12008()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-EJIT\V1-M09.5-PDC\b12008", "b12008.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_EJIT.@tests_V1_M11_Beta1.@tests_b40089
    {
        [TestClass]
        public class @testclass_b40089
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
            public void @test_b40089()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-EJIT\V1-M11-Beta1\b40089", "b40089.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_EJIT.@tests_V1_M11_Beta1.@tests_b40138
    {
        [TestClass]
        public class @testclass_b40138
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
            public void @test_b40138()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-EJIT\V1-M11-Beta1\b40138", "b40138.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_EJIT.@tests_V1_M11_Beta1.@tests_b45679
    {
        [TestClass]
        public class @testclass_b45679
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
            public void @test_b45679()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-EJIT\V1-M11-Beta1\b45679", "b45679.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_EJIT.@tests_V1_M12_Beta2.@tests_b46847
    {
        [TestClass]
        public class @testclass_b46847
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
            public void @test_b46847()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-EJIT\V1-M12-Beta2\b46847", "b46847.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_dev10.@tests_b392262
    {
        [TestClass]
        public class @testclass_b392262
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
            public void @test_b392262()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\dev10\b392262", "b392262.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_dev10.@tests_b400791
    {
        [TestClass]
        public class @testclass_b400791
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
            public void @test_b400971()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\dev10\b400791", "b400971.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_dev10.@tests_b402658
    {
        [TestClass]
        public class @testclass_b402658
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
            public void @test_b402658()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\dev10\b402658", "b402658.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_dev11.@tests_DevDiv_376412
    {
        [TestClass]
        public class @testclass_DevDiv_376412
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
            public void @test_DevDiv_376412()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\dev11\DevDiv_376412", "DevDiv_376412.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b13170
    {
        [TestClass]
        public class @testclass_b13170
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
            public void @test_b13170()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b13170", "b13170.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b13178
    {
        [TestClass]
        public class @testclass_b13178
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
            public void @test_b13178()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b13178", "b13178.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b13647
    {
        [TestClass]
        public class @testclass_b13647
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
            public void @test_b13647()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b13647", "b13647.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14057
    {
        [TestClass]
        public class @testclass_b14057
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
            public void @test_b14057()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14057", "b14057.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14059
    {
        [TestClass]
        public class @testclass_b14059
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
            public void @test_b14059()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14059", "b14059.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14277
    {
        [TestClass]
        public class @testclass_b14277
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
            public void @test_b14277()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14277", "b14277.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14314
    {
        [TestClass]
        public class @testclass_b14314
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
            public void @test_b14314()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14314", "b14314.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14367
    {
        [TestClass]
        public class @testclass_b14367
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
            public void @test_b14367()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14367", "b14367.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14396
    {
        [TestClass]
        public class @testclass_b14396
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
            public void @test_b14396()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14396", "b14396.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14422
    {
        [TestClass]
        public class @testclass_b14422
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
            public void @test_b14422()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14422", "b14422.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14428
    {
        [TestClass]
        public class @testclass_b14428
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
            public void @test_b14428()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14428", "b14428.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14443
    {
        [TestClass]
        public class @testclass_b14443
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
            public void @test_b14443()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14443", "b14443.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14616
    {
        [TestClass]
        public class @testclass_b14616
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
            public void @test_b14616()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14616", "b14616.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14624
    {
        [TestClass]
        public class @testclass_b14624
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
            public void @test_b14624()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14624", "b14624.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14640
    {
        [TestClass]
        public class @testclass_b14640
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
            public void @test_b14640()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14640", "b14640.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b14673
    {
        [TestClass]
        public class @testclass_b14673
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
            public void @test_b14673()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b14673", "b14673.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b15155
    {
        [TestClass]
        public class @testclass_b15155
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
            public void @test_b15155()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b15155", "b15155.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b15468
    {
        [TestClass]
        public class @testclass_b15468
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
            public void @test_b15468()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b15468", "b15468.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b15526
    {
        [TestClass]
        public class @testclass_b15526
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
            public void @test_b15526()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b15526", "b15526.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b15783
    {
        [TestClass]
        public class @testclass_b15783
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
            public void @test_b15783()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b15783", "b15783.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b15786
    {
        [TestClass]
        public class @testclass_b15786
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
            public void @test_b15786()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b15786", "b15786.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b15797
    {
        [TestClass]
        public class @testclass_b15797
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
            public void @test_b15797()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b15797", "b15797.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b15864
    {
        [TestClass]
        public class @testclass_b15864
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
            public void @test_b15864()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b15864", "b15864.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09.@tests_b16294
    {
        [TestClass]
        public class @testclass_b16294
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
            public void @test_b16294()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09\b16294", "b16294.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b11490
    {
        [TestClass]
        public class @testclass_b11490
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
            public void @test_b11490()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b11490", "b11490.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b12399
    {
        [TestClass]
        public class @testclass_b12399
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
            public void @test_b12399()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b12399", "b12399.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b12624
    {
        [TestClass]
        public class @testclass_b12624
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
            public void @test_b12624()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b12624", "b12624.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b13569
    {
        [TestClass]
        public class @testclass_b13569
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
            public void @test_b13569()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b13569", "b13569.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b14716
    {
        [TestClass]
        public class @testclass_b14716
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
            public void @test_b14716()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b14716", "b14716.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b15728
    {
        [TestClass]
        public class @testclass_b15728
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
            public void @test_b15728()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b15728", "b15728.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b16238
    {
        [TestClass]
        public class @testclass_b16238
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
            public void @test_b16238()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b16238", "b16238.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b16328
    {
        [TestClass]
        public class @testclass_b16328
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
            public void @test_b16328()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b16328", "b16328.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b16335
    {
        [TestClass]
        public class @testclass_b16335
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
            public void @test_b16335()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b16335", "b16335.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b16423
    {
        [TestClass]
        public class @testclass_b16423
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
            public void @test_b16423()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b16423", "b16423.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b20913
    {
        [TestClass]
        public class @testclass_b20913
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
            public void @test_b20913()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b20913", "b20913.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b22290
    {
        [TestClass]
        public class @testclass_b22290
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
            public void @test_b22290()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b22290", "b22290.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b24727
    {
        [TestClass]
        public class @testclass_b24727
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
            public void @test_b24727()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b24727", "b24727.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b24728
    {
        [TestClass]
        public class @testclass_b24728
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
            public void @test_b24728()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b24728", "b24728.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b25647
    {
        [TestClass]
        public class @testclass_b25647
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
            public void @test_b25647()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b25647", "b25647.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b26558
    {
        [TestClass]
        public class @testclass_b26558
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
            public void @test_b26558()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b26558", "b26558.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b26560
    {
        [TestClass]
        public class @testclass_b26560
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
            public void @test_b26560()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b26560", "b26560.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b26732
    {
        [TestClass]
        public class @testclass_b26732
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
            public void @test_b26732()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b26732", "b26732.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b26863
    {
        [TestClass]
        public class @testclass_b26863
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
            public void @test_b26863()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b26863", "b26863.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b27811
    {
        [TestClass]
        public class @testclass_b27811
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
            public void @test_b27811()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b27811", "b27811.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b27819
    {
        [TestClass]
        public class @testclass_b27819
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
            public void @test_b27819()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b27819", "b27819.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b27824
    {
        [TestClass]
        public class @testclass_b27824
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
            public void @test_b27824()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b27824", "b27824.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b28037
    {
        [TestClass]
        public class @testclass_b28037
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
            public void @test_b28037()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b28037", "b28037.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b28042
    {
        [TestClass]
        public class @testclass_b28042
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
            public void @test_b28042()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b28042", "b28042.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b28776
    {
        [TestClass]
        public class @testclass_b28776
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
            public void @test_b28776()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b28776", "b28776.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b28787
    {
        [TestClass]
        public class @testclass_b28787
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
            public void @test_b28787()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b28787", "b28787.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b28790
    {
        [TestClass]
        public class @testclass_b28790
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
            public void @test_b28790()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b28790", "b28790.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b30126
    {
        [TestClass]
        public class @testclass_b30126
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
            public void @test_b30126()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b30126", "b30126.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b30128
    {
        [TestClass]
        public class @testclass_b30128
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
            public void @test_b30128()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b30128", "b30128.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b30630
    {
        [TestClass]
        public class @testclass_b30630
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
            public void @test_b30630()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b30630", "b30630.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b31732
    {
        [TestClass]
        public class @testclass_b31732
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
            public void @test_b31732()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b31732", "b31732.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b31748
    {
        [TestClass]
        public class @testclass_b31748
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
            public void @test_b31748()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b31748", "b31748.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b31749
    {
        [TestClass]
        public class @testclass_b31749
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
            public void @test_b31749()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b31749", "b31749.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b31763
    {
        [TestClass]
        public class @testclass_b31763
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
            public void @test_b31763()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b31763", "b31763.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b31912
    {
        [TestClass]
        public class @testclass_b31912
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
            public void @test_b31912()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b31912", "b31912.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b32303
    {
        [TestClass]
        public class @testclass_b32303
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
            public void @test_b32303()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b32303", "b32303.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b32345
    {
        [TestClass]
        public class @testclass_b32345
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
            public void @test_b32345()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b32345", "b32345.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b32560
    {
        [TestClass]
        public class @testclass_b32560
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
            public void @test_b32560()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b32560", "b32560.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M09_5_PDC.@tests_b32801
    {
        [TestClass]
        public class @testclass_b32801
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
            public void @test_b32801()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M09.5-PDC\b32801", "b32801.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b02051
    {
        [TestClass]
        public class @testclass_b02051
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
            public void @test_b02051()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b02051", "b02051.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b02076
    {
        [TestClass]
        public class @testclass_b02076
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
            public void @test_b02076()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b02076", "b02076.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b05477
    {
        [TestClass]
        public class @testclass_b05477
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
            public void @test_b05477()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b05477", "b05477.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b06464
    {
        [TestClass]
        public class @testclass_b06464
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
            public void @test_b06464()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b06464", "b06464.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b06680
    {
        [TestClass]
        public class @testclass_b06680
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
            public void @test_b06680()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b06680", "b06680.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b06859
    {
        [TestClass]
        public class @testclass_b06859
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
            public void @test_b06859()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b06859", "b06859.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b06924
    {
        [TestClass]
        public class @testclass_b06924
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
            public void @test_b06924()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b06924", "b06924.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b08944
    {
        [TestClass]
        public class @testclass_b08944
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
            public void @test_b08944a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b08944", "b08944a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_b08944b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b08944", "b08944b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b09287
    {
        [TestClass]
        public class @testclass_b09287
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
            public void @test_b09287()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b09287", "b09287.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b09452
    {
        [TestClass]
        public class @testclass_b09452
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
            public void @test_b09452()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b09452", "b09452.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b13330
    {
        [TestClass]
        public class @testclass_b13330
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
            public void @test_b13330()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b13330", "b13330.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M10.@tests_b13466
    {
        [TestClass]
        public class @testclass_b13466
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
            public void @test_b13466()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M10\b13466", "b13466.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b30586
    {
        [TestClass]
        public class @testclass_b30586
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
            public void @test_b30586()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b30586", "b30586.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b31878
    {
        [TestClass]
        public class @testclass_b31878
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
            public void @test_b31878()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b31878", "b31878.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b34945
    {
        [TestClass]
        public class @testclass_b34945
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
            public void @test_b34945()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b34945", "b34945.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b36274
    {
        [TestClass]
        public class @testclass_b36274
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
            public void @test_b36274()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b36274", "b36274.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b36332
    {
        [TestClass]
        public class @testclass_b36332
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
            public void @test_b36332()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b36332", "b36332.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b36470
    {
        [TestClass]
        public class @testclass_b36470
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
            public void @test_b36470()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b36470", "b36470.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b36471
    {
        [TestClass]
        public class @testclass_b36471
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
            public void @test_b36471()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b36471", "b36471.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b36472
    {
        [TestClass]
        public class @testclass_b36472
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
            public void @test_b36472()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b36472", "b36472.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b37131
    {
        [TestClass]
        public class @testclass_b37131
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
            public void @test_b37131()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b37131", "b37131.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b37598
    {
        [TestClass]
        public class @testclass_b37598
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
            public void @test_b37598()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b37598", "b37598.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b37608
    {
        [TestClass]
        public class @testclass_b37608
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
            public void @test_b37608()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b37608", "b37608.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b37636
    {
        [TestClass]
        public class @testclass_b37636
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
            public void @test_b37636()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b37636", "b37636.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b38403
    {
        [TestClass]
        public class @testclass_b38403
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
            public void @test_b38403()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b38403", "b38403.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b38556
    {
        [TestClass]
        public class @testclass_b38556
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
            public void @test_b38556()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b38556", "b38556.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b39217
    {
        [TestClass]
        public class @testclass_b39217
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
            public void @test_b39217()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b39217", "b39217.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b39224
    {
        [TestClass]
        public class @testclass_b39224
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
            public void @test_b39224()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b39224", "b39224.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b39951
    {
        [TestClass]
        public class @testclass_b39951
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
            public void @test_b39951()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b39951", "b39951.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b40141
    {
        [TestClass]
        public class @testclass_b40141
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
            public void @test_b40141()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b40141", "b40141.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b40216
    {
        [TestClass]
        public class @testclass_b40216
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
            public void @test_b40216()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b40216", "b40216.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b40221
    {
        [TestClass]
        public class @testclass_b40221
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
            public void @test_b40221()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b40221", "b40221.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b40496
    {
        [TestClass]
        public class @testclass_b40496
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
            public void @test_b40496()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b40496", "b40496.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b40521
    {
        [TestClass]
        public class @testclass_b40521
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
            public void @test_b40521()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b40521", "b40521.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b41063
    {
        [TestClass]
        public class @testclass_b41063
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
            public void @test_b41063()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b41063", "b41063.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b41234
    {
        [TestClass]
        public class @testclass_b41234
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
            public void @test_b41234()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b41234", "b41234.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b41391
    {
        [TestClass]
        public class @testclass_b41391
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
            public void @test_b41391()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b41391", "b41391.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b41470
    {
        [TestClass]
        public class @testclass_b41470
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
            public void @test_b41470()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b41470", "b41470.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b41488
    {
        [TestClass]
        public class @testclass_b41488
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
            public void @test_b41488()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b41488", "b41488.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b41495
    {
        [TestClass]
        public class @testclass_b41495
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
            public void @test_b41495()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b41495", "b41495.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b41621
    {
        [TestClass]
        public class @testclass_b41621
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
            public void @test_b41621()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b41621", "b41621.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b41918
    {
        [TestClass]
        public class @testclass_b41918
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
            public void @test_b41918()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b41918", "b41918.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b42009
    {
        [TestClass]
        public class @testclass_b42009
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
            public void @test_b42009()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b42009", "b42009.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b42013
    {
        [TestClass]
        public class @testclass_b42013
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
            public void @test_b42013()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b42013", "b42013.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b42918
    {
        [TestClass]
        public class @testclass_b42918
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
            public void @test_b42918()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b42918", "b42918.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b42929
    {
        [TestClass]
        public class @testclass_b42929
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
            public void @test_b42929()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b42929", "b42929.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b43010
    {
        [TestClass]
        public class @testclass_b43010
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
            public void @test_b43010()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b43010", "b43010.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b43313
    {
        [TestClass]
        public class @testclass_b43313
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
            public void @test_b43313()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b43313", "b43313.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b43719
    {
        [TestClass]
        public class @testclass_b43719
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
            public void @test_b43719()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b43719", "b43719.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b43958
    {
        [TestClass]
        public class @testclass_b43958
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
            public void @test_b43958()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b43958", "b43958.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b44193
    {
        [TestClass]
        public class @testclass_b44193
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
            public void @test_b44193()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b44193", "b44193.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b44297
    {
        [TestClass]
        public class @testclass_b44297
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
            public void @test_b44297()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b44297", "b44297.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b44410
    {
        [TestClass]
        public class @testclass_b44410
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
            public void @test_b44410()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b44410", "b44410.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b45015
    {
        [TestClass]
        public class @testclass_b45015
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
            public void @test_b45015()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b45015", "b45015.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b45259
    {
        [TestClass]
        public class @testclass_b45259
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
            public void @test_b45259()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b45259", "b45259.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b45270
    {
        [TestClass]
        public class @testclass_b45270
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
            public void @test_b45270()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b45270", "b45270.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b45458
    {
        [TestClass]
        public class @testclass_b45458
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
            public void @test_b45458()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b45458", "b45458.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b45535
    {
        [TestClass]
        public class @testclass_b45535
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
            public void @test_b45535()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b45535", "b45535.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b46170
    {
        [TestClass]
        public class @testclass_b46170
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
            public void @test_b46170()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b46170", "b46170.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b46629
    {
        [TestClass]
        public class @testclass_b46629
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
            public void @test_b46629()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b46629", "b46629.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b46641
    {
        [TestClass]
        public class @testclass_b46641
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
            public void @test_b46641()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b46641", "b46641.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b46649
    {
        [TestClass]
        public class @testclass_b46649
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
            public void @test_b46649()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b46649", "b46649.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b46867
    {
        [TestClass]
        public class @testclass_b46867
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
            public void @test_b46867()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b46867", "b46867.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b47047
    {
        [TestClass]
        public class @testclass_b47047
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
            public void @test_b47047()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b47047", "b47047.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b48248
    {
        [TestClass]
        public class @testclass_b48248
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
            public void @test_b48248()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b48248", "b48248.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b48797
    {
        [TestClass]
        public class @testclass_b48797
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
            public void @test_b48797()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b48797", "b48797.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b48805
    {
        [TestClass]
        public class @testclass_b48805
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
            public void @test_b48805()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b48805", "b48805.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b48864
    {
        [TestClass]
        public class @testclass_b48864
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
            public void @test_b48864()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b48864", "b48864.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b48872
    {
        [TestClass]
        public class @testclass_b48872
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
            public void @test_b48872()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b48872", "b48872.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b48990
    {
        [TestClass]
        public class @testclass_b48990
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
            public void @test_b48990a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b48990", "b48990a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_b48990b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b48990", "b48990b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b49318
    {
        [TestClass]
        public class @testclass_b49318
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
            public void @test_b49318()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b49318", "b49318.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b49322
    {
        [TestClass]
        public class @testclass_b49322
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
            public void @test_b49322()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b49322", "b49322.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M11_Beta1.@tests_b49717
    {
        [TestClass]
        public class @testclass_b49717
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
            public void @test_b49717()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M11-Beta1\b49717", "b49717.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b31182
    {
        [TestClass]
        public class @testclass_b31182
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
            public void @test_b31182()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b31182", "b31182.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b31745
    {
        [TestClass]
        public class @testclass_b31745
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
            public void @test_b31745()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b31745", "b31745.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b31746
    {
        [TestClass]
        public class @testclass_b31746
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
            public void @test_b31746()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b31746", "b31746.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b31762
    {
        [TestClass]
        public class @testclass_b31762
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
            public void @test_b31762()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b31762", "b31762.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b31903
    {
        [TestClass]
        public class @testclass_b31903
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
            public void @test_b31903()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b31903", "b31903.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b31917
    {
        [TestClass]
        public class @testclass_b31917
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
            public void @test_b31917()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b31917", "b31917.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b37646
    {
        [TestClass]
        public class @testclass_b37646
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
            public void @test_b37646()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b37646", "b37646.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b38269
    {
        [TestClass]
        public class @testclass_b38269
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
            public void @test_b38269()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b38269", "b38269.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b41852
    {
        [TestClass]
        public class @testclass_b41852
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
            public void @test_b41852()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b41852", "b41852.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b47975
    {
        [TestClass]
        public class @testclass_b47975
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
            public void @test_b47975()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b47975", "b47975.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b48929
    {
        [TestClass]
        public class @testclass_b48929
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
            public void @test_b48929()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b48929", "b48929.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b49809
    {
        [TestClass]
        public class @testclass_b49809
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
            public void @test_b49809()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b49809", "b49809.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b50042
    {
        [TestClass]
        public class @testclass_b50042
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
            public void @test_b50042()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b50042", "b50042.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b50145
    {
        [TestClass]
        public class @testclass_b50145
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
            public void @test_b50145()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b50145", "b50145.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_b50145a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b50145", "b50145a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_b50145b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b50145", "b50145b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_b50145c()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b50145", "b50145c.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b50535
    {
        [TestClass]
        public class @testclass_b50535
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
            public void @test_b50535()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b50535", "b50535.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b51463
    {
        [TestClass]
        public class @testclass_b51463
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
            public void @test_b51463()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b51463", "b51463.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b51469
    {
        [TestClass]
        public class @testclass_b51469
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
            public void @test_b51469()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b51469", "b51469.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b51565
    {
        [TestClass]
        public class @testclass_b51565
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
            public void @test_b51565()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b51565", "b51565.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b51870
    {
        [TestClass]
        public class @testclass_b51870
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
            public void @test_b51870()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b51870", "b51870.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b52572
    {
        [TestClass]
        public class @testclass_b52572
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
            public void @test_b52572()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b52572", "b52572.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b52578
    {
        [TestClass]
        public class @testclass_b52578
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
            public void @test_b52578()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b52578", "b52578.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b52746
    {
        [TestClass]
        public class @testclass_b52746
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
            public void @test_b52746()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b52746", "b52746.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b52760
    {
        [TestClass]
        public class @testclass_b52760
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
            public void @test_b52760()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b52760", "b52760.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b52838
    {
        [TestClass]
        public class @testclass_b52838
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
            public void @test_b52838()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b52838", "b52838.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b52839
    {
        [TestClass]
        public class @testclass_b52839
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
            public void @test_b52839()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b52839", "b52839.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b54667
    {
        [TestClass]
        public class @testclass_b54667
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
            public void @test_b54667()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b54667", "b54667.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b55197
    {
        [TestClass]
        public class @testclass_b55197
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
            public void @test_b55197()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b55197", "b55197.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b56149
    {
        [TestClass]
        public class @testclass_b56149
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
            public void @test_b56149()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b56149", "b56149.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b56154
    {
        [TestClass]
        public class @testclass_b56154
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
            public void @test_b56154()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b56154", "b56154.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b56159
    {
        [TestClass]
        public class @testclass_b56159
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
            public void @test_b56159()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b56159", "b56159.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b59297
    {
        [TestClass]
        public class @testclass_b59297
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
            public void @test_b59297()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b59297", "b59297.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b60723
    {
        [TestClass]
        public class @testclass_b60723
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
            public void @test_b60723()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b60723", "b60723.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b61028
    {
        [TestClass]
        public class @testclass_b61028
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
            public void @test_b61028()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b61028", "b61028.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b61515
    {
        [TestClass]
        public class @testclass_b61515
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
            public void @test_b61515()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b61515", "b61515.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b61640
    {
        [TestClass]
        public class @testclass_b61640
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
            public void @test_b61640()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b61640", "b61640.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b62498
    {
        [TestClass]
        public class @testclass_b62498
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
            public void @test_b62498()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b62498", "b62498.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b62555
    {
        [TestClass]
        public class @testclass_b62555
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
            public void @test_b62555()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b62555", "b62555.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b62892
    {
        [TestClass]
        public class @testclass_b62892
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
            public void @test_b62892()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b62892", "b62892.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b63183
    {
        [TestClass]
        public class @testclass_b63183
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
            public void @test_b63183()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b63183", "b63183.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b68361
    {
        [TestClass]
        public class @testclass_b68361
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
            public void @test_b68361()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b68361", "b68361.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b68634
    {
        [TestClass]
        public class @testclass_b68634
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
            public void @test_b68634()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b68634", "b68634.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b71099
    {
        [TestClass]
        public class @testclass_b71099
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
            public void @test_b71099()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b71099", "b71099.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b71120
    {
        [TestClass]
        public class @testclass_b71120
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
            public void @test_b71120()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b71120", "b71120.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b71135
    {
        [TestClass]
        public class @testclass_b71135
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
            public void @test_b71135()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b71135", "b71135.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b71155
    {
        [TestClass]
        public class @testclass_b71155
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
            public void @test_b71155()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b71155", "b71155.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b71231
    {
        [TestClass]
        public class @testclass_b71231
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
            public void @test_b71231()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b71231", "b71231.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b72136
    {
        [TestClass]
        public class @testclass_b72136
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
            public void @test_b72136()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b72136", "b72136.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b72996
    {
        [TestClass]
        public class @testclass_b72996
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
            public void @test_b72996()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b72996", "b72996.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b73921
    {
        [TestClass]
        public class @testclass_b73921
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
            public void @test_b73921()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b73921", "b73921.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b74182
    {
        [TestClass]
        public class @testclass_b74182
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
            public void @test_b74182()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b74182", "b74182.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b75250
    {
        [TestClass]
        public class @testclass_b75250
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
            public void @test_b75250()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b75250", "b75250.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b75509
    {
        [TestClass]
        public class @testclass_b75509
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
            public void @test_b75509()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b75509", "b75509.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b76590
    {
        [TestClass]
        public class @testclass_b76590
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
            public void @test_b76590()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b76590", "b76590.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b77707
    {
        [TestClass]
        public class @testclass_b77707
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
            public void @test_b77707()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b77707", "b77707.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b77713
    {
        [TestClass]
        public class @testclass_b77713
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
            public void @test_b77713()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b77713", "b77713.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b78392
    {
        [TestClass]
        public class @testclass_b78392
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
            public void @test_b78392()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b78392", "b78392.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b79418
    {
        [TestClass]
        public class @testclass_b79418
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
            public void @test_b79418()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b79418", "b79418.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b80045
    {
        [TestClass]
        public class @testclass_b80045
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
            public void @test_b80045()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b80045", "b80045.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b82866
    {
        [TestClass]
        public class @testclass_b82866
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
            public void @test_b82866()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b82866", "b82866.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b83690
    {
        [TestClass]
        public class @testclass_b83690
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
            public void @test_b83690()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b83690", "b83690.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b84836
    {
        [TestClass]
        public class @testclass_b84836
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
            public void @test_b84836()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b84836", "b84836.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M12_Beta2.@tests_b91377
    {
        [TestClass]
        public class @testclass_b91377
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
            public void @test_b91377()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M12-Beta2\b91377", "b91377.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b87284
    {
        [TestClass]
        public class @testclass_b87284
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
            public void @test_b87284()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b87284", "b87284.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b88712
    {
        [TestClass]
        public class @testclass_b88712
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
            public void @test_b88712()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b88712", "b88712.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b88793
    {
        [TestClass]
        public class @testclass_b88793
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
            public void @test_b88793()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b88793", "b88793.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b88797
    {
        [TestClass]
        public class @testclass_b88797
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
            public void @test_b88797()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b88797", "b88797.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b89277
    {
        [TestClass]
        public class @testclass_b89277
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
            public void @test_b89277()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b89277", "b89277.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b89279
    {
        [TestClass]
        public class @testclass_b89279
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
            public void @test_b89279()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b89279", "b89279.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b89506
    {
        [TestClass]
        public class @testclass_b89506
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
            public void @test_b89506()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b89506", "b89506.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b89600
    {
        [TestClass]
        public class @testclass_b89600
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
            public void @test_b89600()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b89600", "b89600.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b89946
    {
        [TestClass]
        public class @testclass_b89946
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
            public void @test_b89946()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b89946", "b89946.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b91189
    {
        [TestClass]
        public class @testclass_b91189
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
            public void @test_b91189()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b91189", "b91189.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b91230
    {
        [TestClass]
        public class @testclass_b91230
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
            public void @test_b91230()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b91230", "b91230.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b91248
    {
        [TestClass]
        public class @testclass_b91248
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
            public void @test_b91248()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b91248", "b91248.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b91855
    {
        [TestClass]
        public class @testclass_b91855
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
            public void @test_b91855()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b91855", "b91855.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b91859
    {
        [TestClass]
        public class @testclass_b91859
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
            public void @test_b91859()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b91859", "b91859.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b91867
    {
        [TestClass]
        public class @testclass_b91867
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
            public void @test_b91867()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b91867", "b91867.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b91917
    {
        [TestClass]
        public class @testclass_b91917
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
            public void @test_b91917()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b91917", "b91917.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b92568
    {
        [TestClass]
        public class @testclass_b92568
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
            public void @test_b92568()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b92568", "b92568.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b92614
    {
        [TestClass]
        public class @testclass_b92614
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
            public void @test_b92614()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b92614", "b92614.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b92693
    {
        [TestClass]
        public class @testclass_b92693
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
            public void @test_b92693()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b92693", "b92693.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M13_RTM.@tests_b93027
    {
        [TestClass]
        public class @testclass_b93027
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
            public void @test_b93027()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M13-RTM\b93027", "b93027.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_M14_SP1.@tests_b119538
    {
        [TestClass]
        public class @testclass_b119538
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
            public void @test_b119538a()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M14-SP1\b119538", "b119538a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_b119538b()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-M14-SP1\b119538", "b119538b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_QFE.@tests_b148815
    {
        [TestClass]
        public class @testclass_b148815
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
            public void @test_testclass()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1-QFE\b148815", "testclass.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_1_M1_Beta1.@tests_b119294
    {
        [TestClass]
        public class @testclass_b119294
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
            public void @test_b119294()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.1-M1-Beta1\b119294", "b119294.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_1_M1_Beta1.@tests_b130333
    {
        [TestClass]
        public class @testclass_b130333
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
            public void @test_byteshift()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.1-M1-Beta1\b130333", "byteshift.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_1_M1_Beta1.@tests_b140711
    {
        [TestClass]
        public class @testclass_b140711
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
            public void @test_b140711()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.1-M1-Beta1\b140711", "b140711.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_Beta1.@tests_b102879
    {
        [TestClass]
        public class @testclass_b102879
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
            public void @test_dblinf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-Beta1\b102879", "dblinf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_Beta1.@tests_b124232
    {
        [TestClass]
        public class @testclass_b124232
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
            public void @test_genisinst()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-Beta1\b124232", "genisinst.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_Beta1.@tests_b169333
    {
        [TestClass]
        public class @testclass_b169333
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
            public void @test_b169333()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-Beta1\b169333", "b169333.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_Beta1.@tests_b178119
    {
        [TestClass]
        public class @testclass_b178119
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
            public void @test_hugemthfrm()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-Beta1\b178119", "hugemthfrm.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_Beta1.@tests_b178128
    {
        [TestClass]
        public class @testclass_b178128
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
            public void @test_hugestruct()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-Beta1\b178128", "hugestruct.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_Beta1.@tests_b180381
    {
        [TestClass]
        public class @testclass_b180381
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
            public void @test_constrained1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-Beta1\b180381", "constrained1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_Beta1.@tests_b191926
    {
        [TestClass]
        public class @testclass_b191926
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
            public void @test_ParamLimit()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-Beta1\b191926", "ParamLimit.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_Beta1.@tests_b219940
    {
        [TestClass]
        public class @testclass_b219940
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
            public void @test_struct01_gen()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-Beta1\b219940", "struct01_gen.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M01.@tests_b00735
    {
        [TestClass]
        public class @testclass_b00735
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
            public void @test_b00735()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M01\b00735", "b00735.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M01.@tests_b02345
    {
        [TestClass]
        public class @testclass_b02345
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
            public void @test_b02345()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M01\b02345", "b02345.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M01.@tests_b07211
    {
        [TestClass]
        public class @testclass_b07211
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
            public void @test_byteshift()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M01\b07211", "byteshift.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M01.@tests_b07900
    {
        [TestClass]
        public class @testclass_b07900
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
            public void @test_rpPasses()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M01\b07900", "rpPasses.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M01.@tests_b08020
    {
        [TestClass]
        public class @testclass_b08020
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
            public void @test_bbHndIndex()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M01\b08020", "bbHndIndex.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M01.@tests_b11762
    {
        [TestClass]
        public class @testclass_b11762
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
            public void @test_b11762()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M01\b11762", "b11762.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M01.@tests_b16386
    {
        [TestClass]
        public class @testclass_b16386
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
            public void @test_gtnop()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M01\b16386", "gtnop.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M01.@tests_b16399
    {
        [TestClass]
        public class @testclass_b16399
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
            public void @test_gtIsValid64RsltMul()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M01\b16399", "gtIsValid64RsltMul.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M02.@tests_b19171
    {
        [TestClass]
        public class @testclass_b19171
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
            public void @test_jmp2blk()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M02\b19171", "jmp2blk.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V1_2_M02.@tests_b30251
    {
        [TestClass]
        public class @testclass_b30251
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
            public void @test_b578931()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V1.2-M02\b30251", "b578931.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V2_0_Beta2.@tests_b102533
    {
        [TestClass]
        public class @testclass_b102533
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
            public void @test_DeadBlock()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V2.0-Beta2\b102533", "DeadBlock.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V2_0_Beta2.@tests_b268908
    {
        [TestClass]
        public class @testclass_b268908
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
            public void @test_vars2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V2.0-Beta2\b268908", "vars2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V2_0_Beta2.@tests_b320147
    {
        [TestClass]
        public class @testclass_b320147
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
            public void @test_1086745236()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V2.0-Beta2\b320147", "1086745236.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V2_0_Beta2.@tests_b321799
    {
        [TestClass]
        public class @testclass_b321799
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
            public void @test_repro()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V2.0-Beta2\b321799", "repro.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V2_0_Beta2.@tests_b338014
    {
        [TestClass]
        public class @testclass_b338014
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
            public void @test_vsw338014()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V2.0-Beta2\b338014", "vsw338014.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V2_0_Beta2.@tests_b405223
    {
        [TestClass]
        public class @testclass_b405223
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
            public void @test_b405223()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V2.0-Beta2\b405223", "b405223.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V2_0_Beta2.@tests_b416667
    {
        [TestClass]
        public class @testclass_b416667
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
            public void @test_b416667()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V2.0-Beta2\b416667", "b416667.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_V2_0_RTM.@tests_b369916
    {
        [TestClass]
        public class @testclass_b369916
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
            public void @test_b369916()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\V2.0-RTM\b369916", "b369916.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b106272
    {
        [TestClass]
        public class @testclass_b106272
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
            public void @test_b106272()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b106272", "b106272.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b152292
    {
        [TestClass]
        public class @testclass_b152292
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
            public void @test_b152292()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b152292", "b152292.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b565808
    {
        [TestClass]
        public class @testclass_b565808
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
            public void @test_b565808()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b565808", "b565808.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b589202
    {
        [TestClass]
        public class @testclass_b589202
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
            public void @test_b589202()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b589202", "b589202.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b598034
    {
        [TestClass]
        public class @testclass_b598034
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
            public void @test_b598034()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b598034", "b598034.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b598649
    {
        [TestClass]
        public class @testclass_b598649
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
            public void @test_b598649()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b598649", "b598649.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b602004
    {
        [TestClass]
        public class @testclass_b602004
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
            public void @test_Inline_Vars2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b602004", "Inline_Vars2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b608066
    {
        [TestClass]
        public class @testclass_b608066
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
            public void @test_b608066()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b608066", "b608066.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b608198
    {
        [TestClass]
        public class @testclass_b608198
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
            public void @test_b608198()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b608198", "b608198.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b609280
    {
        [TestClass]
        public class @testclass_b609280
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
            public void @test_b609280()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b609280", "b609280.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b610562
    {
        [TestClass]
        public class @testclass_b610562
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
            public void @test_b610562()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b610562", "b610562.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b610750
    {
        [TestClass]
        public class @testclass_b610750
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
            public void @test_32vs64()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b610750", "32vs64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_b610750()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b610750", "b610750.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b611219
    {
        [TestClass]
        public class @testclass_b611219
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
            public void @test_b611219()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b611219", "b611219.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_b72218
    {
        [TestClass]
        public class @testclass_b72218
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
            public void @test_b72218()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\b72218", "b72218.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_DDB.@tests_b121938
    {
        [TestClass]
        public class @testclass_b121938
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
            public void @test_ConstToString()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\DDB\b121938", "ConstToString.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_DDB.@tests_b151497
    {
        [TestClass]
        public class @testclass_b151497
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
            public void @test_NullCheckBoxedValuetypeReturn()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\DDB\b151497", "NullCheckBoxedValuetypeReturn.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_DDB.@tests_b158861
    {
        [TestClass]
        public class @testclass_b158861
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
            public void @test_b158861()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\DDB\b158861", "b158861.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_DDB.@tests_b163200
    {
        [TestClass]
        public class @testclass_b163200
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
            public void @test_b163200()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\DDB\b163200", "b163200.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_DDB.@tests_b170362
    {
        [TestClass]
        public class @testclass_b170362
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
            public void @test_b170362()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\DDB\b170362", "b170362.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_DDB.@tests_b33183
    {
        [TestClass]
        public class @testclass_b33183
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
            public void @test_b33183()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\DDB\b33183", "b33183.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_CLR_x86_JIT.@tests_v2_1.@tests_DDB.@tests_b49778
    {
        [TestClass]
        public class @testclass_b49778
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
            public void @test_b49778()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\CLR-x86-JIT\v2.1\DDB\b49778", "b49778.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_dev10_94677
    {
        [TestClass]
        public class @testclass_dev10_94677
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
            public void @test_loopvt()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\dev10_94677", "loopvt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_dev11_13912
    {
        [TestClass]
        public class @testclass_dev11_13912
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
            public void @test_dev11_13912()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\dev11_13912", "dev11_13912.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_dev11_165544
    {
        [TestClass]
        public class @testclass_dev11_165544
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
            public void @test_seqpts()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\dev11_165544", "seqpts.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_Dev11_457559
    {
        [TestClass]
        public class @testclass_Dev11_457559
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
            public void @test_Dev11_457559()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\Dev11_457559", "Dev11_457559.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_Dev11_5437
    {
        [TestClass]
        public class @testclass_Dev11_5437
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
            public void @test_Dev11_5437()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\Dev11_5437", "Dev11_5437.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_Dev11_617302
    {
        [TestClass]
        public class @testclass_Dev11_617302
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
            public void @test_Dev11_617302()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\Dev11_617302", "Dev11_617302.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_Dev11_646049
    {
        [TestClass]
        public class @testclass_Dev11_646049
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
            public void @test_Dev11_646049()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\Dev11_646049", "Dev11_646049.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_DevDiv2_10623
    {
        [TestClass]
        public class @testclass_DevDiv2_10623
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
            public void @test_DevDiv2_10623()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\DevDiv2_10623", "DevDiv2_10623.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev11.@tests_DevDiv2_8863
    {
        [TestClass]
        public class @testclass_DevDiv2_8863
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
            public void @test_DevDiv2_8863()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev11\DevDiv2_8863", "DevDiv2_8863.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_Dev14.@tests_DevDiv_876169
    {
        [TestClass]
        public class @testclass_DevDiv_876169
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
            public void @test_DevDiv_876169()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\Dev14\DevDiv_876169", "DevDiv_876169.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_JitBlue.@tests_DevDiv_794115
    {
        [TestClass]
        public class @testclass_DevDiv_794115
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
            public void @test_DevDiv_794115()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\JitBlue\DevDiv_794115", "DevDiv_794115.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_JitBlue.@tests_DevDiv_794631
    {
        [TestClass]
        public class @testclass_DevDiv_794631
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
            public void @test_DevDiv_794631()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\JitBlue\DevDiv_794631", "DevDiv_794631.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_JitBlue.@tests_DevDiv_815940
    {
        [TestClass]
        public class @testclass_DevDiv_815940
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
            public void @test_DevDiv_815940()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\JitBlue\DevDiv_815940", "DevDiv_815940.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_JitBlue.@tests_DevDiv_816617
    {
        [TestClass]
        public class @testclass_DevDiv_816617
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
            public void @test_DevDiv_816617()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\JitBlue\DevDiv_816617", "DevDiv_816617.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_M00.@tests_b100336
    {
        [TestClass]
        public class @testclass_b100336
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
            public void @test_emptytryfinally()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b100336", "emptytryfinally.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_M00.@tests_b103846
    {
        [TestClass]
        public class @testclass_b103846
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
            public void @test_d()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b103846", "d.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_M00.@tests_b111130
    {
        [TestClass]
        public class @testclass_b111130
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
            public void @test_makework()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b111130", "makework.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_M00.@tests_b115253
    {
        [TestClass]
        public class @testclass_b115253
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
            public void @test_hello2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b115253", "hello2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_M00.@tests_b119026
    {
        [TestClass]
        public class @testclass_b119026
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
            public void @test_bug()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b119026", "bug.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_charbug()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b119026", "charbug.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_M00.@tests_b140298
    {
        [TestClass]
        public class @testclass_b140298
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b140298", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_M00.@tests_b141358
    {
        [TestClass]
        public class @testclass_b141358
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b141358", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_M00.@tests_b99219
    {
        [TestClass]
        public class @testclass_b99219
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
            public void @test_b99219()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\M00\b99219", "b99219.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_Beta1.@tests_b102615
    {
        [TestClass]
        public class @testclass_b102615
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
            public void @test_test1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-Beta1\b102615", "test1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_Beta1.@tests_b102860
    {
        [TestClass]
        public class @testclass_b102860
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
            public void @test_structret1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-Beta1\b102860", "structret1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_Beta1.@tests_b102887
    {
        [TestClass]
        public class @testclass_b102887
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
            public void @test_struct5_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-Beta1\b102887", "struct5_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_Beta1.@tests_b124409
    {
        [TestClass]
        public class @testclass_b124409
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
            public void @test_ericcprop3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-Beta1\b124409", "ericcprop3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_Beta1.@tests_b91074
    {
        [TestClass]
        public class @testclass_b91074
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
            public void @test_pack8()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-Beta1\b91074", "pack8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M01.@tests_b10789
    {
        [TestClass]
        public class @testclass_b10789
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
            public void @test_switch()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M01\b10789", "switch.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M01.@tests_b10790
    {
        [TestClass]
        public class @testclass_b10790
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
            public void @test_locals10K()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M01\b10790", "locals10K.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M01.@tests_b10827
    {
        [TestClass]
        public class @testclass_b10827
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
            public void @test_MT_DEATH()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M01\b10827", "MT_DEATH.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M01.@tests_b19112
    {
        [TestClass]
        public class @testclass_b19112
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
            public void @test_divbyte()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M01\b19112", "divbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_divshort()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M01\b19112", "divshort.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b102729
    {
        [TestClass]
        public class @testclass_b102729
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
            public void @test_b102729()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b102729", "b102729.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b102763
    {
        [TestClass]
        public class @testclass_b102763
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
            public void @test_gcparamonstack()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b102763", "gcparamonstack.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b102886
    {
        [TestClass]
        public class @testclass_b102886
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
            public void @test_ovf()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b102886", "ovf.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b10828
    {
        [TestClass]
        public class @testclass_b10828
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
            public void @test_redundant()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b10828", "redundant.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b19394
    {
        [TestClass]
        public class @testclass_b19394
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
            public void @test_stringArray114()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b19394", "stringArray114.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b21015
    {
        [TestClass]
        public class @testclass_b21015
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
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b21015", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b26496
    {
        [TestClass]
        public class @testclass_b26496
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
            public void @test__1d6bgof()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b26496", "_1d6bgof.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b27978
    {
        [TestClass]
        public class @testclass_b27978
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
            public void @test_gcparaminreg()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b27978", "gcparaminreg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b27980
    {
        [TestClass]
        public class @testclass_b27980
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
            public void @test_struct1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b27980", "struct1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b28077
    {
        [TestClass]
        public class @testclass_b28077
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
            public void @test_struct5_2()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b28077", "struct5_2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b28141
    {
        [TestClass]
        public class @testclass_b28141
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
            public void @test_structret1_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b28141", "structret1_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V1_2_M02.@tests_b29343
    {
        [TestClass]
        public class @testclass_b29343
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
            public void @test_test_hfa12()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V1.2-M02\b29343", "test.hfa12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V2_0_Beta2.@tests_b184799
    {
        [TestClass]
        public class @testclass_b184799
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
            public void @test_b184799()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V2.0-Beta2\b184799", "b184799.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V2_0_Beta2.@tests_b302558
    {
        [TestClass]
        public class @testclass_b302558
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
            public void @test__aopst1l()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V2.0-Beta2\b302558", "_aopst1l.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V2_0_Beta2.@tests_b309539
    {
        [TestClass]
        public class @testclass_b309539
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
            public void @test__ba6c0ou()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V2.0-Beta2\b309539", "_ba6c0ou.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V2_0_Beta2.@tests_b309548
    {
        [TestClass]
        public class @testclass_b309548
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
            public void @test__hngh669()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V2.0-Beta2\b309548", "_hngh669.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V2_0_Beta2.@tests_b311420
    {
        [TestClass]
        public class @testclass_b311420
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
            public void @test_b311420()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V2.0-Beta2\b311420", "b311420.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V2_0_Beta2.@tests_b360587
    {
        [TestClass]
        public class @testclass_b360587
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
            public void @test_repro()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V2.0-Beta2\b360587", "repro.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @JIT.@tests_Regression.@tests_VS_ia64_JIT.@tests_V2_0_RTM.@tests_b539509
    {
        [TestClass]
        public class @testclass_b539509
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
            public void @test_b539509()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"JIT\Regression\VS-ia64-JIT\V2.0-RTM\b539509", "b539509.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Loader.@tests_regressions.@tests_classloader
    {
        [TestClass]
        public class @testclass_classloader
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
            public void @test_main()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Loader\regressions\classloader", "main.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vsw307137()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Loader\regressions\classloader", "vsw307137.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Loader.@tests_regressions.@tests_classloader_generics
    {
        [TestClass]
        public class @testclass_classloader_generics
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
            public void @test_repro237932()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Loader\regressions\classloader-generics", "repro237932.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_StaticsProblem5()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Loader\regressions\classloader-generics", "StaticsProblem5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_vsw514968()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Loader\regressions\classloader-generics", "vsw514968.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Loader.@tests_regressions.@tests_polyrec
    {
        [TestClass]
        public class @testclass_polyrec
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
            public void @test_polyrec()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Loader\regressions\polyrec", "polyrec.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Loader.@tests_typeinitialization
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
            public void @test_circularcctorthreethreads03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Loader\typeinitialization", "circularcctorthreethreads03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @managed.@tests_Compilation
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
            public void @test_HelloWorld()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"managed\Compilation", "HelloWorld.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Regressions.@tests_assemblyref
    {
        [TestClass]
        public class @testclass_assemblyref
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
            public void @test_assem()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\assemblyref", "assem.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_test()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\assemblyref", "test.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Regressions.@tests_common
    {
        [TestClass]
        public class @testclass_common
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
            public void @test_AboveStackLimit()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "AboveStackLimit.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ArrayCopy()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "ArrayCopy.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_avtest()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "avtest.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_CompEx()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "CompEx.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_date()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "date.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_DisableTransparencyEnforcement()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "DisableTransparencyEnforcement.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_interlock()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "interlock.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Marshal()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "Marshal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_pow3()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "pow3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_test1307()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "test1307.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_testClass()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "testClass.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_testInterface()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "testInterface.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ThreadCulture()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "ThreadCulture.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_Timer()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "Timer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ToLower()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "ToLower.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_unsafe()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\common", "unsafe.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Regressions.@tests_expl_double
    {
        [TestClass]
        public class @testclass_expl_double
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
            public void @test_body_double()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\expl_double", "body_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_expl_double_1()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Regressions\expl_double", "expl_double_1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }

    }

    namespace @Threading.@tests_ThreadStatics
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
            public void @test_ThreadStatic01()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ThreadStatic02()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ThreadStatic03()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ThreadStatic05()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

            [TestMethod]
            public void @test_ThreadStatic06()
            {
                var file = Path.Combine(CompilerHelper.CoreCLRSourcePath, @"Threading\ThreadStatics", "ThreadStatic06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false, returnCode: 100);
            }

        }
    }
}


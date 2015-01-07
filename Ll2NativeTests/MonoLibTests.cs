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

    namespace @dtest
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_001()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-001.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_002()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-002.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_003()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-003.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_004()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-004.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_005()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-005.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_006()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-006.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_007()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-007.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_008()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-008.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_009()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-009.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_010()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-010.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_011()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-011.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_012()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-012.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_013()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-013.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_014()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-014.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_015()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-015.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_016()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-016.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_017()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-017.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_018()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-018.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_019()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-019.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_020()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-020.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_021()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-021.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_022()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-022.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_023()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-023.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_024()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-024.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_025()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-025.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_026()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-026.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_027()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-027.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_028()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-028.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_029()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-029.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_030()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-030.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_031()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-031.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_032()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-032.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_033()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-033.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_034()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-034.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_035()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-035.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_036()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-036.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_037()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-037.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_038()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-038.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_039()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-039.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_040()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-040.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_041()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-041.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_042()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-042.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_043()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-043.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_044()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-044.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_045()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-045.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_046()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-046.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_047()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-047.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_048()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-048.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_049()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-049.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_050()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-050.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_051()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-051.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_052()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-052.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_053()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-053.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_054()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-054.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_055()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-055.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_056()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-056.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_057()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-057.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_058()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest", "dtest-058.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_anontype
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_anontype_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-anontype", "dtest-anontype-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_cls
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_cls_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-cls", "dtest-cls-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_collectioninit
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_collectioninit_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-collectioninit", "dtest-collectioninit-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_error
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_error_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-error", "dtest-error-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_error_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-error", "dtest-error-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_error_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-error", "dtest-error-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_error_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-error", "dtest-error-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_etree
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_etree_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-etree", "dtest-etree-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_etree_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-etree", "dtest-etree-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_friend
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_friend_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-friend", "dtest-friend-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_implicitarray
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_implicitarray_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-implicitarray", "dtest-implicitarray-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_iter
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_iter_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-iter", "dtest-iter-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_named
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_named_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-named", "dtest-named-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @dtest_named_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-named", "dtest-named-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @dtest_optional
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @dtest_optional_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"dtest-optional", "dtest-optional-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_001()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-001.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_002()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-002.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_003()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-003.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_004()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-004.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_005()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-005.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_006()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-006.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_007()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-007.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_008()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-008.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_009()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-009.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_010()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-010.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_011()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-011.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_012()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-012.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_013()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-013.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_014()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-014.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_015()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-015.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_016()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-016.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_017()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-017.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_018()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-018.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_019()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-019.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_020()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-020.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_021()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-021.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_022()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-022.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_023()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-023.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_024()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-024.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_025()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-025.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_026()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-026.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_027()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-027.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_028()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-028.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_029()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-029.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_030()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-030.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_031()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-031.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_032()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-032.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_033()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-033.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_034()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-034.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_035()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-035.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_036()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-036.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_037()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-037.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_038()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-038.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_039()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-039.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_040()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-040.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_041()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-041.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_042()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-042.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_043()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-043.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_044()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-044.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_045()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-045.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_046()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-046.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_047()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-047.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_048()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-048.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_049()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-049.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_050()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-050.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_051()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-051.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_052()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-052.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_053()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-053.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_054()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-054.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_055()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-055.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_056()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-056.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_057()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-057.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_058()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-058.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_059()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-059.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_060()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-060.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_061()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-061.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_062()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-062.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_063()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-063.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_064()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-064.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_065()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-065.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_066()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-066.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_067()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-067.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_068()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-068.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_069()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-069.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_070()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-070.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_071()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-071.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_072()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-072.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_073()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-073.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_074()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-074.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_075()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-075.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_076()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-076.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_078()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-078.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_079()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-079.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_080()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-080.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_081()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-081.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_082()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-082.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_083()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-083.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_084()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-084.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_085()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-085.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_086()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-086.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_087()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-087.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_088()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-088.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_089()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-089.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_090()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-090.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_091()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-091.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_092()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-092.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_093()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-093.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_094()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-094.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_095()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-095.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_096()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-096.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_097()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-097.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_098()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-098.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_100()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-100.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_101()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-101.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_102()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-102.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_103()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-103.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_104()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-104.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_105()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-105.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_106()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-106.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_107()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-107.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_108()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-108.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_109()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-109.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_110()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-110.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_111()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-111.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_112()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-112.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_113()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-113.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_114()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-114.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_115()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-115.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_116()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-116.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_117()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-117.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_118()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-118.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_119()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-119.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_120()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-120.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_121()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-121.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_122()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-122.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_123()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-123.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_124()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-124.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_125()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-125.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_126()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-126.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_127()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-127.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_128()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-128.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_129()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-129.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_130()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-130.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_131()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-131.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_132()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-132.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_133()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-133.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_134()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-134.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_135()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-135.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_136()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-136.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_137()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-137.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_138()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-138.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_139()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-139.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_140()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-140.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_141()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-141.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_142()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-142.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_143()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-143.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_144()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-144.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_145()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-145.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_146()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-146.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_147()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-147.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_148()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-148.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_149()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-149.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_150()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-150.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_151()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-151.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_152()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-152.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_153()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-153.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_154()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-154.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_155()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-155.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_156()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-156.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_157()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-157.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_158()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-158.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_159()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-159.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_160()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-160.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_161()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-161.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_162()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-162.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_163()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-163.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_164()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-164.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_165()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-165.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_166()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-166.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_167()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-167.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_168()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-168.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_169()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-169.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_170()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-170.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_171()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-171.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_172()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-172.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_173()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-173.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_174()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-174.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_175()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-175.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_176()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-176.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_177()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-177.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_178()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-178.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_179()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-179.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_180()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-180.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_181()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-181.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_182()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-182.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_183()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-183.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_184()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-184.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_185()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-185.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_186()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-186.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_187()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-187.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_188()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-188.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_189()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-189.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_190()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-190.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_191()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-191.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_192()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-192.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_193()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-193.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_194()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-194.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_195()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-195.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_196()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-196.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_197()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-197.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_198()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-198.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_199()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-199.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_200()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-200.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_201()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-201.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_202()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-202.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_203()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-203.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_204()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-204.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_205()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-205.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_206()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-206.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_207()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-207.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_208()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-208.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_209()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-209.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_210()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-210.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_211()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-211.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_212()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-212.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_213()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-213.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_214()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-214.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_215()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-215.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_216()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-216.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_217()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-217.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_218()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-218.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_219()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-219.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_220()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-220.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_221()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-221.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_222()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-222.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_223()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-223.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_224()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-224.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_225()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-225.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_226()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-226.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_227()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-227.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_228()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-228.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_229()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-229.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_230()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-230.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_231()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-231.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_232()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-232.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_233()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-233.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_234()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-234.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_235()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-235.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_236()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-236.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_237()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-237.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_238()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-238.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_239()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-239.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_240()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-240.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_241()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-241.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_242()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-242.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_243()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-243.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_244()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-244.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_245()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-245.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_246()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-246.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_247()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-247.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_248()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-248.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_249()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-249.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_250()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-250.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_251()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-251.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_252()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-252.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_253()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-253.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_254()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-254.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_255()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-255.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_256()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-256.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_257()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-257.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_258()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-258.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_259()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-259.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_260()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-260.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_261()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-261.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_262()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-262.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_263()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-263.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_264()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-264.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_265()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-265.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_266()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-266.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_267()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-267.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_268()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-268.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_269()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-269.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_270()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-270.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_271()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-271.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_272()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-272.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_273()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-273.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_274()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-274.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_275()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-275.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_276()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-276.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_277()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-277.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_278()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-278.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_279()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-279.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_280()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-280.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_281()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-281.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_282()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-282.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_283()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-283.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_284()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-284.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_285()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-285.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_286()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-286.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_287()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-287.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_288()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-288.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_289()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-289.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_290()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-290.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_291()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-291.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_292()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-292.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_293()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-293.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_294()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-294.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_295()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-295.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_296()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-296.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_297()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-297.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_298()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-298.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_299()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-299.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_300()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-300.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_301()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-301.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_302()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-302.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_303()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-303.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_304()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-304.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_305()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-305.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_306()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-306.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_307()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-307.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_308()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-308.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_309()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-309.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_310()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-310.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_311()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-311.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_312()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-312.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_313()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-313.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_314()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-314.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_315()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-315.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_316()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-316.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_317()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-317.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_318()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-318.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_319()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-319.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_320()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-320.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_321()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-321.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_322()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-322.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_323()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-323.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_324()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-324.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_325()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-325.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_326()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-326.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_327()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-327.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_328()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-328.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_329()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-329.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_330()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-330.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_331()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-331.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_332()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-332.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_333()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-333.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_334()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-334.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_335()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-335.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_336()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-336.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_337()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-337.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_338()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-338.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_339()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-339.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_340()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-340.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_341()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-341.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_342()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-342.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_343()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-343.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_344()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-344.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_345()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-345.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_346()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-346.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_347()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-347.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_348()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-348.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_349()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-349.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_350()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-350.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_351()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-351.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_352()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-352.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_353()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-353.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_354()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-354.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_355()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-355.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_356()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-356.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_357()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-357.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_358()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-358.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_359()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-359.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_360()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-360.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_361()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-361.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_362()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-362.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_363()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-363.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_364()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-364.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_365()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-365.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_366()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-366.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_367()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-367.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_368()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-368.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_369()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-369.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_370()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-370.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_371()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-371.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_372()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-372.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_373()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-373.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_374()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-374.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_375()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-375.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_376()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-376.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_377()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-377.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_378()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-378.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_379()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-379.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_380()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-380.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_381()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-381.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_382()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-382.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_383()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-383.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_384()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-384.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_385()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-385.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_386()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-386.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_387()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-387.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_388()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-388.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_389()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-389.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_390()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-390.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_391()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-391.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_392()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-392.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_393()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-393.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_394()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-394.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_395()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-395.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_396()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-396.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_397()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-397.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_398()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-398.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_399()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-399.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_400()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-400.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_401()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-401.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_402()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-402.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_403()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-403.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_404()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-404.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_405()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-405.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_406()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-406.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_407()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-407.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_408()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-408.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_409()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-409.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_410()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-410.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_411()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-411.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_412()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-412.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_413()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-413.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_414()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-414.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_415()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-415.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_416()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-416.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_417()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-417.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_418()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-418.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_419()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-419.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_420()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-420.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_421()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-421.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_422()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-422.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_423()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-423.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_424()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-424.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_425()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-425.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_426()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-426.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_427()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-427.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_428()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-428.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_429()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-429.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_430()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-430.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_431()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-431.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_432()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-432.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_433()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-433.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_434()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-434.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_435()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-435.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_436()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-436.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_438()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-438.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_439()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-439.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_440()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-440.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_441()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-441.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_442()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-442.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_443()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-443.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_444()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-444.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_445()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-445.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_446()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-446.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_447()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-447.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_448()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-448.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_449()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-449.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_450()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-450.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_451()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-451.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_452()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-452.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_453()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-453.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_454()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-454.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_455()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-455.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_456()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-456.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_457()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-457.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_458()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-458.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_459()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-459.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_460()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-460.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_461()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-461.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_462()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-462.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_463()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-463.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_464()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-464.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_465()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-465.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_466()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-466.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_467()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-467.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_468()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-468.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_469()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-469.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_470()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-470.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_471()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-471.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_472()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-472.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_473()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-473.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_474()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-474.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_475()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-475.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_476()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-476.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_477()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-477.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_478()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-478.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_479()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-479.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_480()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-480.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_481()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-481.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_482()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-482.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_483()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-483.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_484()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-484.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_485()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-485.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_486()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-486.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_487()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-487.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_488()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-488.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_489()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-489.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_490()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-490.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_491()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-491.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_492()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-492.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_493()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-493.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_494()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-494.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_495()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-495.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_496()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-496.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_497()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-497.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_498()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-498.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_499()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-499.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_500()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-500.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_501()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-501.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_502()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-502.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_503()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-503.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_504()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-504.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_505()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-505.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_506()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-506.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_507()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-507.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_508()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-508.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_509()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-509.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_510()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-510.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_511()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-511.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_512()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-512.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_513()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-513.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_514()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-514.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_515()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-515.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_516()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-516.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_517()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-517.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_518()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-518.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_519()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-519.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_520()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-520.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_521()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-521.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_522()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-522.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_523()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-523.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_524()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-524.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_525()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-525.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_526()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-526.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_527()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-527.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_528()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-528.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_529()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-529.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_530()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-530.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_531()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-531.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_532()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-532.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_533()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-533.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_534()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-534.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_535()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-535.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_536()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-536.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_537()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-537.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_538()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-538.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_539()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-539.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_540()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-540.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_541()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-541.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_542()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-542.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_543()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-543.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_544()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-544.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_545()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-545.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_546()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-546.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_547()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-547.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_548()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-548.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_549()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-549.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_550()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-550.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_551()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-551.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_552()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-552.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_553()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-553.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_554()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-554.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_555()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-555.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_556()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-556.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_557()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-557.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_558()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-558.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_559()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-559.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_560()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-560.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_561()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-561.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_562()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-562.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_563()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-563.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_564()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-564.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_565()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-565.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_566()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-566.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_567()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-567.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_568()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-568.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_569()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-569.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_570()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-570.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_571()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-571.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_572()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-572.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_573()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-573.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_574()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-574.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_575()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-575.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_576()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-576.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_577()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-577.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_578()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-578.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_579()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-579.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_580()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-580.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_581()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-581.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_582()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-582.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_583()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-583.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_584()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-584.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_585()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-585.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_586()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-586.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_587()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-587.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_588()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-588.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_589()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest", "gtest-589.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_anontype
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_anontype_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_anontype_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-anontype", "gtest-anontype-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_autoproperty
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_autoproperty_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-autoproperty", "gtest-autoproperty-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_autoproperty_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-autoproperty", "gtest-autoproperty-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_autoproperty_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-autoproperty", "gtest-autoproperty-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_autoproperty_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-autoproperty", "gtest-autoproperty-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_autoproperty_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-autoproperty", "gtest-autoproperty-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_autoproperty_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-autoproperty", "gtest-autoproperty-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_autoproperty_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-autoproperty", "gtest-autoproperty-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_collectioninit
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_collectioninit_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-collectioninit", "gtest-collectioninit-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_collectioninit_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-collectioninit", "gtest-collectioninit-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_collectioninit_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-collectioninit", "gtest-collectioninit-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_etree
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_etree_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_etree_29()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-etree", "gtest-etree-29.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_exmethod
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_exmethod_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_29()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-29.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_30()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-30.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_31()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-31.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_32()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_33()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-33.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_34()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-34.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_35()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-35.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_36()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-36.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_37()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-37.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_38()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-38.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_39()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-39.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_40()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-40.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_41()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-41.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_42()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-42.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_43()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-43.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_44()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-44.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_45()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-45.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_exmethod_46()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-exmethod", "gtest-exmethod-46.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_fixedbuffer
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_fixedbuffer_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-fixedbuffer", "gtest-fixedbuffer-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_friend
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_friend_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_friend_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-friend", "gtest-friend-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_implicitarray
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_implicitarray_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-implicitarray", "gtest-implicitarray-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_implicitarray_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-implicitarray", "gtest-implicitarray-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_implicitarray_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-implicitarray", "gtest-implicitarray-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_initialize
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_initialize_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_initialize_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-initialize", "gtest-initialize-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_iter
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_iter_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_iter_29()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-iter", "gtest-iter-29.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_lambda
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_lambda_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_29()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-29.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_30()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-30.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_lambda_31()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-lambda", "gtest-lambda-31.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_linq
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_linq_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_linq_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-linq", "gtest-linq-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_named
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_named_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-named", "gtest-named-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_named_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-named", "gtest-named-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_named_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-named", "gtest-named-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_named_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-named", "gtest-named-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_optional
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_optional_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_optional_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-optional", "gtest-optional-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_partial
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_partial_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-partial", "gtest-partial-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_partial_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-partial", "gtest-partial-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_partial_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-partial", "gtest-partial-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_partial_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-partial", "gtest-partial-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_partial_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-partial", "gtest-partial-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_partial_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-partial", "gtest-partial-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_var
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_var_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-var", "gtest-var-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @gtest_variance
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @gtest_variance_1()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_2()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_3()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_4()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_5()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_6()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_7()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-7.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_8()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @gtest_variance_9()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"gtest-variance", "gtest-variance-9.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @support
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @support_353()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"support", "support-353.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @support_361()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"support", "support-361.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @support_388()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"support", "support-388.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @support_389()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"support", "support-389.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @support_test_debug
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @support_test_debug_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"support-test-debug", "support-test-debug-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @support_xml
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @support_xml_067()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"support-xml", "support-xml-067.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_1()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-1.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_100()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-100.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_101()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-101.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_102()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-102.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_103()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-103.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_104()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-104.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_105()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-105.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_107()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-107.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_108()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-108.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_109()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-109.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_110()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-110.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_111()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-111.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_112()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-112.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_113()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-113.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_114()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-114.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_115()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-115.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_116()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-116.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_117()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-117.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_118()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-118.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_119()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-119.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_120()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-120.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_121()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-121.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_122()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-122.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_123()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-123.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_124()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-124.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_125()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-125.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_126()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-126.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_127()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-127.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_128()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-128.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_129()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-129.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_130()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-130.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_131()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-131.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_132()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-132.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_133()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-133.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_134()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-134.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_135()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-135.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_136()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-136.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_137()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-137.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_138()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-138.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_139()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-139.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_140()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-140.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_141()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-141.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_142()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-142.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_143()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-143.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_144()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-144.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_145()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-145.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_146()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-146.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_147()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-147.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_148()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-148.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_149()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-149.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_150()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-150.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_151()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-151.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_152()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-152.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_153()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-153.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_154()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-154.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_155()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-155.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_156()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-156.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_157()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-157.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_158()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-158.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_159()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-159.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_160()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-160.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_161()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-161.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_162()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-162.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_163()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-163.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_164()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-164.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_165()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-165.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_166()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-166.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_167()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-167.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_168()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-168.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_169()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-169.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_170()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-170.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_171()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-171.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_172()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-172.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_173()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-173.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_174()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-174.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_175()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-175.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_176()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-176.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_177()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-177.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_178()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-178.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_179()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-179.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_180()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-180.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_181()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-181.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_182()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-182.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_183()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-183.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_184()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-184.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_185()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-185.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_186()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-186.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_187()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-187.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_188()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-188.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_189()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-189.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_190()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-190.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_191()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-191.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_192()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-192.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_193()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-193.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_194()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-194.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_195()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-195.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_196()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-196.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_197()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-197.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_198()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-198.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_199()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-199.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_2()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_200()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-200.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_201()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-201.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_202()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-202.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_203()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-203.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_204()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-204.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_205()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-205.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_206()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-206.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_207()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-207.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_208()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-208.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_209()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-209.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_210()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-210.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_211()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-211.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_212()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-212.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_213()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-213.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_214()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-214.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_215()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-215.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_216()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-216.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_217()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-217.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_218()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-218.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_219()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-219.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_220()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-220.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_221()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-221.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_222()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-222.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_223()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-223.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_224()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-224.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_225()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-225.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_226()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-226.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_227()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-227.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_228()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-228.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_229()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-229.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_230()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-230.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_231()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-231.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_232()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-232.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_233()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-233.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_234()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-234.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_235()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-235.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_236()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-236.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_237()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-237.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_238()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-238.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_239()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-239.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_240()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-240.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_241()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-241.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_242()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-242.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_243()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-243.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_244()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-244.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_245()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-245.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_246()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-246.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_247()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-247.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_248()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-248.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_249()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-249.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_250()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-250.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_251()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-251.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_252()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-252.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_253()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-253.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_254()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-254.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_255()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-255.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_256()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-256.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_257()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-257.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_258()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-258.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_259()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-259.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_260()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-260.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_261()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-261.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_262()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-262.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_263()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-263.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_264()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-264.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_265()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-265.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_266()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-266.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_267()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-267.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_268()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-268.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_269()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-269.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_270()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-270.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_271()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-271.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_272()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-272.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_273()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-273.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_274()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-274.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_275()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-275.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_276()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-276.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_277()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-277.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_278()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-278.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_279()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-279.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_280()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-280.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_281()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-281.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_282()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-282.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_283()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-283.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_284()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-284.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_285()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-285.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_286()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-286.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_287()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-287.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_288()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-288.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_289()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-289.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_29()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-29.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_290()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-290.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_291()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-291.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_292()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-292.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_293()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-293.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_294()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-294.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_295()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-295.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_296()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-296.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_297()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-297.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_298()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-298.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_299()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-299.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_3()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-3.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_30()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-30.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_300()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-300.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_301()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-301.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_302()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-302.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_303()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-303.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_304()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-304.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_305()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-305.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_306()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-306.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_307()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-307.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_308()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-308.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_309()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-309.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_31()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-31.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_310()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-310.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_311()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-311.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_312()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-312.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_313()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-313.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_314()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-314.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_315()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-315.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_316()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-316.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_317()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-317.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_318()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-318.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_319()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-319.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_32()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_320()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-320.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_321()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-321.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_322()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-322.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_323()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-323.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_324()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-324.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_325()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-325.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_326()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-326.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_327()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-327.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_328()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-328.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_329()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-329.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_33()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-33.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_330()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-330.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_331()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-331.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_332()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-332.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_333()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-333.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_334()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-334.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_335()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-335.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_336()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-336.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_337()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-337.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_338()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-338.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_339()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-339.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_34()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-34.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_340()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-340.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_341()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-341.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_342()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-342.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_343()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-343.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_344()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-344.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_345()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-345.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_346()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-346.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_347()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-347.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_348()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-348.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_349()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-349.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_35()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-35.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_350()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-350.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_351()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-351.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_352()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-352.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_353()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-353.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_354()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-354.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_355()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-355.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_356()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-356.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_357()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-357.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_358()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-358.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_359()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-359.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_36()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-36.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_360()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-360.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_361()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-361.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_362()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-362.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_363()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-363.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_364()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-364.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_365()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-365.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_366()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-366.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_367()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-367.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_368()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-368.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_369()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-369.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_37()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-37.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_370()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-370.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_371()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-371.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_372()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-372.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_373()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-373.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_374()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-374.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_375()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-375.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_376()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-376.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_377()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-377.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_378()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-378.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_379()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-379.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_38()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-38.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_380()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-380.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_381()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-381.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_382()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-382.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_383()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-383.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_384()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-384.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_385()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-385.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_386()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-386.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_387()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-387.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_388()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-388.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_389()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-389.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_39()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-39.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_390()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-390.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_391()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-391.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_392()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-392.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_393()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-393.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_394()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-394.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_395()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-395.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_396()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-396.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_397()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-397.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_398()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-398.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_399()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-399.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_4()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-4.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_40()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-40.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_400()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-400.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_401()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-401.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_402()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-402.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_403()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-403.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_404()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-404.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_405()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-405.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_406()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-406.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_407()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-407.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_408()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-408.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_409()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-409.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_41()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-41.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_410()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-410.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_411()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-411.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_412()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-412.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_413()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-413.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_414()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-414.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_415()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-415.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_416()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-416.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_417()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-417.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_418()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-418.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_419()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-419.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_42()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-42.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_420()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-420.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_421()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-421.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_422()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-422.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_423()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-423.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_424()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-424.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_425()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-425.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_426()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-426.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_427()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-427.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_428()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-428.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_429()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-429.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_43()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-43.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_430()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-430.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_431()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-431.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_432()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-432.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_433()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-433.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_434()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-434.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_435()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-435.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_436()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-436.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_437()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-437.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_438()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-438.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_439()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-439.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_44()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-44.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_440()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-440.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_441()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-441.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_442()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-442.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_444()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-444.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_445()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-445.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_446()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-446.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_447()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-447.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_448()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-448.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_449()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-449.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_45()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-45.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_450()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-450.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_451()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-451.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_452()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-452.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_453()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-453.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_454()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-454.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_455()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-455.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_456()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-456.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_457()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-457.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_458()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-458.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_459()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-459.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_46()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-46.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_460()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-460.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_461()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-461.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_462()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-462.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_463()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-463.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_464()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-464.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_465()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-465.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_466()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-466.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_467()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-467.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_469()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-469.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_47()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-47.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_470()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-470.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_471()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-471.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_472()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-472.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_473()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-473.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_474()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-474.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_475()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-475.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_476()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-476.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_477()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-477.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_478()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-478.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_479()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-479.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_48()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-48.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_480()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-480.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_481()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-481.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_482()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-482.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_483()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-483.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_484()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-484.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_485()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-485.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_486()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-486.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_487()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-487.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_488()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-488.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_489()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-489.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_49()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-49.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_490()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-490.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_491()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-491.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_492()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-492.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_493()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-493.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_494()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-494.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_495()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-495.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_496()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-496.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_497()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-497.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_498()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-498.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_499()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-499.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_5()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-5.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_500()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-500.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_501()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-501.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_502()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-502.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_503()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-503.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_504()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-504.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_505()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-505.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_506()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-506.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_507()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-507.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_508()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-508.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_509()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-509.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_51()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-51.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_510()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-510.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_511()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-511.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_512()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-512.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_513()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-513.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_514()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-514.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_515()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-515.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_516()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-516.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_517()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-517.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_518()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-518.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_519()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-519.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_52()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-52.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_520()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-520.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_521()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-521.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_522()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-522.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_523()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-523.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_524()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-524.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_525()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-525.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_526()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-526.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_527()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-527.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_528()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-528.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_529()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-529.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_53()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-53.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_530()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-530.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_531()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-531.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_532()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-532.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_533()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-533.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_534()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-534.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_535()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-535.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_536()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-536.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_537()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-537.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_538()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-538.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_539()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-539.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_54()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-54.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_540()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-540.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_541()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-541.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_542()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-542.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_543()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-543.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_544()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-544.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_545()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-545.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_546()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-546.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_547()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-547.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_548()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-548.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_549()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-549.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_55()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-55.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_550()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-550.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_551()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-551.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_552()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-552.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_553()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-553.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_554()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-554.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_555()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-555.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_556()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-556.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_557()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-557.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_558()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-558.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_559()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-559.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_56()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-56.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_560()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-560.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_561()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-561.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_562()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-562.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_563()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-563.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_564()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-564.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_565()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-565.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_566()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-566.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_567()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-567.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_568()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-568.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_569()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-569.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_57()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-57.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_570()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-570.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_571()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-571.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_572()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-572.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_573()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-573.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_574()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-574.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_575()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-575.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_576()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-576.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_577()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-577.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_578()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-578.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_579()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-579.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_58()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-58.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_580()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-580.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_581()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-581.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_582()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-582.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_583()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-583.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_584()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-584.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_585()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-585.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_586()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-586.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_587()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-587.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_588()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-588.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_589()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-589.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_59()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-59.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_590()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-590.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_591()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-591.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_592()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-592.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_593()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-593.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_594()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-594.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_595()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-595.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_596()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-596.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_597()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-597.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_598()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-598.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_599()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-599.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_6()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-6.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_60()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-60.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_600()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-600.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_601()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-601.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_602()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-602.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_603()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-603.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_604()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-604.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_605()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-605.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_606()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-606.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_607()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-607.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_608()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-608.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_609()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-609.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_61()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-61.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_610()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-610.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_611()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-611.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_612()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-612.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_613()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-613.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_614()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-614.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_615()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-615.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_616()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-616.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_617()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-617.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_618()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-618.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_619()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-619.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_62()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-62.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_620()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-620.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_621()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-621.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_622()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-622.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_623()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-623.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_624()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-624.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_625()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-625.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_626()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-626.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_627()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-627.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_628()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-628.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_629()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-629.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_63()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-63.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_630()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-630.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_631()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-631.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_632()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-632.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_633()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-633.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_634()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-634.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_635()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-635.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_636()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-636.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_637()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-637.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_638()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-638.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_639()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-639.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_64()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_640()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-640.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_641()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-641.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_642()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-642.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_643()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-643.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_644()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-644.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_645()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-645.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_646()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-646.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_647()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-647.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_648()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-648.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_649()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-649.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_65()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-65.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_650()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-650.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_651()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-651.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_652()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-652.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_653()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-653.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_654()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-654.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_655()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-655.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_656()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-656.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_657()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-657.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_658()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-658.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_659()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-659.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_66()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-66.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_660()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-660.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_661()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-661.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_662()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-662.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_663()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-663.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_664()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-664.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_665()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-665.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_666()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-666.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_667()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-667.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_668()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-668.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_669()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-669.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_670()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-670.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_671()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-671.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_672()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-672.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_673()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-673.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_674()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-674.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_675()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-675.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_676()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-676.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_677()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-677.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_678()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-678.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_679()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-679.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_68()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-68.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_680()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-680.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_681()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-681.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_682()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-682.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_683()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-683.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_684()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-684.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_685()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-685.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_686()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-686.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_687()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-687.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_688()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-688.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_689()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-689.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_69()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-69.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_690()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-690.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_691()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-691.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_692()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-692.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_693()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-693.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_694()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-694.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_695()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-695.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_696()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-696.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_697()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-697.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_698()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-698.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_699()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-699.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_7()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-7.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_70()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-70.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_700()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-700.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_701()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-701.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_702()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-702.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_703()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-703.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_704()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-704.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_705()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-705.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_706()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-706.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_707()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-707.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_708()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-708.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_709()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-709.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_71()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-71.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_710()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-710.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_711()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-711.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_712()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-712.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_713()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-713.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_714()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-714.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_715()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-715.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_716()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-716.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_717()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-717.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_718()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-718.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_719()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-719.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_72()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-72.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_720()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-720.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_721()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-721.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_722()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-722.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_723()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-723.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_724()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-724.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_725()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-725.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_726()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-726.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_727()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-727.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_728()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-728.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_729()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-729.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_73()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-73.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_730()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-730.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_731()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-731.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_732()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-732.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_733()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-733.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_734()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-734.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_735()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-735.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_736()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-736.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_737()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-737.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_738()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-738.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_739()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-739.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_74()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-74.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_740()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-740.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_741()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-741.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_742()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-742.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_743()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-743.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_744()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-744.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_745()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-745.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_746()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-746.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_747()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-747.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_748()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-748.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_749()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-749.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_75()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-75.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_750()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-750.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_751()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-751.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_752()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-752.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_753()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-753.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_754()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-754.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_755()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-755.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_756()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-756.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_757()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-757.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_758()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-758.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_759()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-759.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_76()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-76.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_760()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-760.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_761()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-761.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_762()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-762.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_763()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-763.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_764()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-764.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_765()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-765.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_766()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-766.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_767()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-767.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_768()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-768.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_769()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-769.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_77()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-77.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_770()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-770.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_771()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-771.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_772()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-772.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_773()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-773.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_774()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-774.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_775()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-775.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_776()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-776.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_777()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-777.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_778()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-778.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_779()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-779.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_78()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-78.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_780()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-780.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_781()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-781.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_782()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-782.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_783()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-783.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_784()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-784.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_785()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-785.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_786()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-786.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_787()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-787.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_788()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-788.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_789()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-789.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_79()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-79.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_790()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-790.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_791()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-791.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_792()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-792.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_793()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-793.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_794()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-794.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_795()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-795.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_796()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-796.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_797()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-797.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_798()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-798.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_799()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-799.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_8()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-8.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_80()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-80.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_800()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-800.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_801()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-801.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_802()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-802.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_803()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-803.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_804()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-804.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_805()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-805.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_806()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-806.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_807()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-807.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_808()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-808.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_809()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-809.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_81()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-81.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_810()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-810.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_811()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-811.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_812()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-812.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_813()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-813.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_814()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-814.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_815()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-815.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_816()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-816.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_817()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-817.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_818()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-818.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_819()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-819.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_82()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-82.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_820()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-820.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_821()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-821.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_822()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-822.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_823()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-823.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_824()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-824.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_825()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-825.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_826()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-826.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_827()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-827.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_828()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-828.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_829()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-829.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_83()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-83.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_830()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-830.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_831()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-831.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_832()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-832.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_833()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-833.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_834()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-834.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_835()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-835.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_836()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-836.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_837()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-837.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_838()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-838.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_839()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-839.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_84()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-84.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_840()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-840.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_841()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-841.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_842()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-842.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_843()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-843.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_844()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-844.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_845()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-845.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_846()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-846.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_847()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-847.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_848()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-848.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_849()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-849.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_85()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-85.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_850()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-850.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_851()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-851.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_852()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-852.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_853()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-853.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_854()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-854.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_855()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-855.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_856()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-856.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_857()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-857.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_858()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-858.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_86()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-86.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_860()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-860.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_861()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-861.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_862()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-862.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_863()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-863.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_864()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-864.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_865()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-865.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_866()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-866.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_867()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-867.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_868()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-868.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_869()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-869.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_87()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-87.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_88()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-88.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_89()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-89.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_9()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-9.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_90()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-90.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_91()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-91.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_92()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-92.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_93()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-93.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_94()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-94.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_95()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-95.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_96()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-96.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_97()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-97.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_98()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-98.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_99()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test", "test-99.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_anon
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_anon_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_100()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-100.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_101()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-101.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_102()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-102.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_103()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-103.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_104()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-104.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_105()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-105.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_106()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-106.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_107()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-107.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_108()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-108.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_109()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-109.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_110()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-110.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_111()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-111.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_112()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-112.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_113()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-113.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_114()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-114.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_115()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-115.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_116()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-116.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_117()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-117.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_118()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-118.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_119()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-119.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_120()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-120.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_121()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-121.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_122()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-122.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_123()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-123.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_124()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-124.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_125()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-125.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_126()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-126.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_127()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-127.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_128()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-128.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_129()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-129.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_130()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-130.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_131()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-131.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_132()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-132.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_133()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-133.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_134()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-134.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_135()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-135.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_136()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-136.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_137()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-137.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_138()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-138.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_139()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-139.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_140()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-140.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_141()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-141.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_142()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-142.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_143()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-143.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_144()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-144.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_145()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-145.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_146()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-146.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_147()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-147.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_148()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-148.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_149()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-149.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_150()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-150.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_151()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-151.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_152()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-152.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_153()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-153.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_154()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-154.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_155()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-155.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_156()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-156.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_157()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-157.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_158()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-158.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_159()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-159.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_160()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-160.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_161()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-161.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_162()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-162.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_163()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-163.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_164()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-164.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_165()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-165.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_166()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-166.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_167()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-167.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_168()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-168.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_169()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-169.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_170()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-170.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_171()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-171.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_29()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-29.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_30()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-30.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_31()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-31.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_32()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_33()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-33.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_34()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-34.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_35()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-35.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_36()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-36.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_37()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-37.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_38()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-38.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_39()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-39.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_40()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-40.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_41()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-41.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_42()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-42.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_43()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-43.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_44()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-44.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_45()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-45.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_46()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-46.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_47()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-47.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_48()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-48.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_49()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-49.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_50()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-50.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_51()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-51.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_52()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-52.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_53()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-53.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_54()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-54.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_55()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-55.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_56()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-56.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_57()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-57.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_58()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-58.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_59()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-59.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_60()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-60.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_61()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-61.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_62()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-62.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_63()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-63.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_64()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_65()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-65.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_66()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-66.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_67()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-67.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_68()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-68.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_69()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-69.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_70()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-70.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_71()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-71.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_72()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-72.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_73()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-73.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_74()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-74.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_75()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-75.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_76()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-76.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_77()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-77.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_78()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-78.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_79()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-79.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_80()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-80.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_81()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-81.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_82()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-82.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_83()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-83.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_84()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-84.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_85()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-85.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_86()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-86.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_87()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-87.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_88()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-88.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_89()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-89.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_90()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-90.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_91()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-91.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_92()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-92.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_93()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-93.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_94()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-94.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_95()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-95.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_96()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-96.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_97()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-97.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_98()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-98.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_anon_99()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-anon", "test-anon-99.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_async
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_async_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_29()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-29.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_30()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-30.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_31()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-31.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_32()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_33()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-33.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_34()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-34.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_35()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-35.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_36()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-36.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_37()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-37.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_38()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-38.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_39()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-39.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_40()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-40.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_41()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-41.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_42()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-42.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_43()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-43.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_44()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-44.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_45()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-45.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_46()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-46.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_47()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-47.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_async_48()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-async", "test-async-48.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_cls
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_cls_00()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-00.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_cls_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-cls", "test-cls-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_com
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_com_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-com", "test-com-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_com_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-com", "test-com-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_com_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-com", "test-com-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_debug
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_debug_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_debug_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-debug", "test-debug-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_externalias
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_externalias_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_externalias_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_externalias_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_externalias_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_externalias_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_externalias_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_externalias_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_externalias_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_externalias_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-externalias", "test-externalias-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_iter
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_iter_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_iter_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-iter", "test-iter-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_named
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_named_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-named", "test-named-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_named_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-named", "test-named-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_named_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-named", "test-named-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_named_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-named", "test-named-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_named_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-named", "test-named-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_named_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-named", "test-named-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_named_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-named", "test-named-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_partial
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_partial_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_04()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-04.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_10()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_11()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-11.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_12()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-12.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_13()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-13.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_14()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-14.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_15()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-15.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_16()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_17()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-17.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_18()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-18.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_19()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-19.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_20()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-20.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_21()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-21.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_22()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-22.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_23()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-23.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_24()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-24.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_25()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-25.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_26()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-26.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_27()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-27.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_28()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-28.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_29()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-29.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_30()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-30.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_partial_31()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-partial", "test-partial-31.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_var
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_var_01()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-var", "test-var-01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_var_02()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-var", "test-var-02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_var_03()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-var", "test-var-03.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_var_05()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-var", "test-var-05.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_var_06()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-var", "test-var-06.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_var_07()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-var", "test-var-07.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_var_08()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-var", "test-var-08.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_var_09()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-var", "test-var-09.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }

    }

    namespace @test_xml
    {
        [TestClass]
        public class @tests
        {
            [TestInitialize]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false);
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @test_xml_001()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-001.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_002()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-002.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_003()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-003.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_004()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-004.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_005()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-005.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_006()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-006.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_007()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-007.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_008()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-008.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_009()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-009.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_010()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-010.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_011()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-011.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_012()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-012.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_013()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-013.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_014()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-014.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_015()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-015.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_016()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-016.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_017()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-017.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_018()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-018.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_019()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-019.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_020()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-020.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_021()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-021.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_022()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-022.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_023()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-023.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_024()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-024.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_025()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-025.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_026()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-026.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_027()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-027.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_028()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-028.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_029()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-029.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_030()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-030.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_031()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-031.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_032()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-032.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_033()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-033.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_034()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-034.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_035()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-035.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_036()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-036.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_037()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-037.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_038()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-038.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_039()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-039.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_040()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-040.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_041()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-041.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_042()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-042.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_043()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-043.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_044()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-044.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_045()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-045.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_046()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-046.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_047()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-047.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_048()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-048.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_049()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-049.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_050()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-050.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_051()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-051.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_052()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-052.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_053()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-053.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_054()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-054.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_055()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-055.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_056()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-056.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_057()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-057.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_058()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-058.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_059()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-059.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_060()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-060.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_061()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-061.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_062()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-062.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_063()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-063.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_064()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-064.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_065()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-065.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_066()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-066.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

            [TestMethod]
            public void @test_xml_067()
            {
                var file = Path.Combine(CompilerHelper.SourcePath, @"test-xml", "test-xml-067.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false);
            }

        }
    }
}


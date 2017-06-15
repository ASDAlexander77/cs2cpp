namespace Sscli.Ll2NativeTests
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using global::Ll2NativeTests;

    namespace Sscli.@argiterator
    {
        [TestClass]
        public class @argiterator
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1827_argiterator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"argiterator", "co1827argiterator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6661_getnextarg()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"argiterator", "co6661getnextarg.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6662_getnextargtype()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"argiterator", "co6662getnextargtype.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6663_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"argiterator", "co6663equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6664_getremainingcount()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"argiterator", "co6664getremainingcount.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6665_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"argiterator", "co6665gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@array
    {
        [TestClass]
        public class @array
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1078_clear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co1078clear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1262_copy()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co1262copy.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1592_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co1592clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3742_createinstance_type_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3742createinstance_type_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3743_createinstance_type_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3743createinstance_type_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3744_createinstance_type_iii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3744createinstance_type_iii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3745_createinstance_type_iarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3745createinstance_type_iarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3746_createinstance_type_iarr_iarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3746createinstance_type_iarr_iarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3747_getvalue_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3747getvalue_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3748_getvalue_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3748getvalue_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3749_getvalue_iii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3749getvalue_iii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3750_setvalue_vi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3750setvalue_vi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3751_setvalue_vii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3751setvalue_vii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3752_setvalue_viii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3752setvalue_viii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3753_getvalue_iarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3753getvalue_iarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3754_setvalue_viarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3754setvalue_viarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3756_copy_aai()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3756copy_aai.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3757_binarysearch_oo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3757binarysearch_oo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3758_binarysearch_oiio()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3758binarysearch_oiio.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3759_binarysearch_ooi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3759binarysearch_ooi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3760_binarysearch_oiioi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3760binarysearch_oiioi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3761_indexof_oo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3761indexof_oo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3762_indexof_ooi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3762indexof_ooi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3763_indexof_ooii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3763indexof_ooii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3764_lastindexof_oo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3764lastindexof_oo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3765_lastindexof_ooi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3765lastindexof_ooi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3766_lastindexof_ooii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3766lastindexof_ooii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3767_reverse_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3767reverse_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3768_reverse_oii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3768reverse_oii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3769_sort_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3769sort_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3770_sort_oii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3770sort_oii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3771_sort_oiii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3771sort_oiii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3772_sort_oi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3772sort_oi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3773_sort_oo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3773sort_oo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3774_sort_ooi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3774sort_ooi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3775_sort_ooii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3775sort_ooii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3776_sort_ooiii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3776sort_ooiii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3777_aslist()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3777aslist.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3795_copyto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3795copyto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3796_getenumerator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3796getenumerator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3797_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co3797get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4385_getupperbound()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co4385getupperbound.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4386_getlowerbound()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co4386getlowerbound.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5126_getlength()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co5126getlength.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5127_getlength_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co5127getlength_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5149_get_rank()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co5149get_rank.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6001_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co6001gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6002_gettype()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co6002gettype.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6003_isfixedsize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co6003isfixedsize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6004_isreadonly()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co6004isreadonly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6005_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co6005issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6006_syncroot()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co6006syncroot.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6007_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"array", "co6007tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@attribute
    {
        [TestClass]
        public class @attribute
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co4752_getcustomattributes_meminfo_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"attribute", "co4752getcustomattributes_meminfo_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4763_getcustomattributes_meminfo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"attribute", "co4763getcustomattributes_meminfo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4764_isdefined_meminfo_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"attribute", "co4764isdefined_meminfo_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4765_getcustomattributes_paraminfo_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"attribute", "co4765getcustomattributes_paraminfo_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4766_getcustomattributes_paraminfo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"attribute", "co4766getcustomattributes_paraminfo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4767_isdefined_paraminfo_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"attribute", "co4767isdefined_paraminfo_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4768_getcustomattributes_meminfo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"attribute", "co4768getcustomattributes_meminfo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4769_getcustomattributes_paraminfo_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"attribute", "co4769getcustomattributes_paraminfo_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@bitconverter
    {
        [TestClass]
        public class @bitconverter
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1230_getbytes_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1230getbytes_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1231_getbytes_short()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1231getbytes_short.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1232_getbytes_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1232getbytes_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1233_getbytes_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1233getbytes_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1235_toint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1235toint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1237_toint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1237toint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1239_toint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1239toint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1241_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1241tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1243_toboolean()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1243toboolean.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1245_getbytes_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co1245getbytes_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3431_getbytes_float()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co3431getbytes_float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3432_todouble()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co3432todouble.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3433_tosingle()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co3433tosingle.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3434_tostring_bytearr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co3434tostring_bytearr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3436_tostring_bytearrint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co3436tostring_bytearrint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3437_tochar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co3437tochar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5370_getbytes_uint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co5370getbytes_uint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5371_getbytes_ulong()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co5371getbytes_ulong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5372_getbytes_ushort()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co5372getbytes_ushort.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5373_touint16_ubarr_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co5373touint16_ubarr_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5374_touint32_ubarr_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co5374touint32_ubarr_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5375_touint64_ubarr_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co5375touint64_ubarr_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5452_getbytes_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co5452getbytes_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9101_doubletoint64bits_dbl()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co9101doubletoint64bits_dbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9102_int64bitstodouble_i64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"bitconverter", "co9102int64bitstodouble_i64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@boolean
    {
        [TestClass]
        public class @boolean
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co4214_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"boolean", "co4214gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4215_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"boolean", "co4215equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4216_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"boolean", "co4216parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4217_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"boolean", "co4217tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4218_tostring_boolean()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"boolean", "co4218tostring_boolean.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5026_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"boolean", "co5026compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5712_iconvertible_boolean()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"boolean", "co5712iconvertible_boolean.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6000_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"boolean", "co6000parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@buffer
    {
        [TestClass]
        public class @buffer
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3829_blockcopy()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"buffer", "co3829blockcopy.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3830_bytelength_a()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"buffer", "co3830bytelength_a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3831_getbyte_ai()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"buffer", "co3831getbyte_ai.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3832_setbyte_aibyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"buffer", "co3832setbyte_aibyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@byte
    {
        [TestClass]
        public class @byte
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5035_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5035compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5036_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5036equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5038_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5038gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5039_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5039tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5040_tostring_byte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5040tostring_byte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5041_parse_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5041parse_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5042_parse_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5042parse_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5043_parse_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5043parse_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5044_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5044tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5045_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5045tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5117_format_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5117format_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5713_iconvertible_byte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co5713iconvertible_byte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6001_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co6001parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7055_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co7055gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8637_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"byte", "co8637parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@char
    {
        [TestClass]
        public class @char
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @char_g_ettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_scontrol_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_iscontrol_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_sdigit_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_isdigit_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_sletterordigit_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_isletterordigit_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_sletter_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_isletter_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_slower_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_islower_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_snumber_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_isnumber_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_snumber_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_isnumber_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_spunctuation_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_ispunctuation_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_sseparator_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_isseparator_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_sseparator_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_isseparator_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_ssurrogate_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_issurrogate_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_ssurrogate_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_issurrogate_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_ssymbol_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_issymbol_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_ssymbol_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_issymbol_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_supper_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_isupper_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @char_i_swhitespace_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "char_iswhitespace_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4233_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4233gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4234_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4234equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4235_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4235tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4236_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4236tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4237_isdigit()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4237isdigit.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4269_isletter()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4269isletter.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4270_iswhitespace()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4270iswhitespace.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4271_toupper()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4271toupper.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4272_tolower()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4272tolower.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4273_isupper()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4273isupper.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4274_islower()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4274islower.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4275_ispunctuation()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4275ispunctuation.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4276_isisocontrol()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co4276isisocontrol.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5326_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co5326compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5327_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co5327parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5714_iconvertible_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co5714iconvertible_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7054_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co7054gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8002_tolower_char_cultinfo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co8002tolower_char_cultinfo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8003_toupper_char_cultinfo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co8003toupper_char_cultinfo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8854_argumentchecking()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "co8854argumentchecking.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @getnum_ericvalue_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "getnumericvalue_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @getnum_ericvalue_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "getnumericvalue_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @getuni_codecategory_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "getunicodecategory_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @getuni_codecategory_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"char", "getunicodecategory_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@charenumerator
    {
        [TestClass]
        public class @charenumerator
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8563_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"charenumerator", "co8563clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8564_get_current()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"charenumerator", "co8564get_current.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8565_movenext()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"charenumerator", "co8565movenext.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8566_reset()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"charenumerator", "co8566reset.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@arraylist
    {
        [TestClass]
        public class @arraylist
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1501_add_stress()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co1501add_stress.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1703_getenumerator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co1703getenumerator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1719_copyto_ai()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co1719copyto_ai.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1720_contains_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co1720contains_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1722_clear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co1722clear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1723_remove_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co1723remove_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1724_indexof_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co1724indexof_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1730_adapter_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co1730adapter_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2192_sort()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2192sort.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2450_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2450ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2452_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2452ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2454_ctor_collection()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2454ctor_collection.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2458_addrange_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2458addrange_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2460_binarysearch_iioi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2460binarysearch_iioi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2462_get_capacity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2462get_capacity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2464_copyto_iaii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2464copyto_iaii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2466_get_item()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2466get_item.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2468_getenumerator_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2468getenumerator_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2470_indexof_oii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2470indexof_oii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2472_insert_io()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2472insert_io.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2474_insertrange_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2474insertrange_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2476_lastindexof_oii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2476lastindexof_oii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2478_removeat_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2478removeat_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2480_removerange()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2480removerange.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2482_reverse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2482reverse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2484_set_item()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2484set_item.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2486_set_capacity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2486set_capacity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2488_setrange()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2488setrange.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2490_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2490get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2494_trimtosize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co2494trimtosize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3905_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3905clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3906_copyto_a()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3906copyto_a.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3907_fixedsize_il()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3907fixedsize_il.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3908_fixedsize_al()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3908fixedsize_al.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3909_get_isreadonly()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3909get_isreadonly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3910_readonly_al()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3910readonly_al.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3911_readonly_il()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3911readonly_il.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3919_synchronized_il()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3919synchronized_il.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3920_get_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3920get_issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3928_lastindexof_oi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3928lastindexof_oi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3929_lastindexof_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3929lastindexof_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3930_indexof_oi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3930indexof_oi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3931_repeat_objint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3931repeat_objint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3932_reverse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3932reverse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3933_sort_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3933sort_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3934_sort()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3934sort.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3935_toarray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3935toarray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3936_toarray_tp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3936toarray_tp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3966_wrappertests()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co3966wrappertests.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8597_binarysearch_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co8597binarysearch_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8598_binarysearch_obj_ic()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co8598binarysearch_obj_ic.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8599_get_isfixedsize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co8599get_isfixedsize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8600_getrange_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\arraylist", "co8600getrange_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@bitarray
    {
        [TestClass]
        public class @bitarray
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1550_and()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1550and.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1551_or()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1551or.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1552_xor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1552xor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1553_not()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1553not.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1554_get_length()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1554get_length.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1555_get()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1555get.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1556_set()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1556set.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1557_setall()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1557setall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1558_set_length()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co1558set_length.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3118_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3118ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3120_ctor_int_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3120ctor_int_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3122_ctor_byte_array()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3122ctor_byte_array.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3124_ctor_intarray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3124ctor_intarray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3126_ctor_bitarray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3126ctor_bitarray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3969_ctor_boolarray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3969ctor_boolarray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3970_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3970get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3971_get_isreadonly()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3971get_isreadonly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3972_get_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3972get_issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3974_get_item()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3974get_item.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3975_set_item()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3975set_item.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3976_getenumerator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co3976getenumerator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8822_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\bitarray", "co8822clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@caseinsensitivecomparer
    {
        [TestClass]
        public class @caseinsensitivecomparer
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8601_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\caseinsensitivecomparer", "co8601ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8602_ctor_ci()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\caseinsensitivecomparer", "co8602ctor_ci.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8603_get_default()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\caseinsensitivecomparer", "co8603get_default.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8604_compare_oo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\caseinsensitivecomparer", "co8604compare_oo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@caseinsensitivehashcodeprovider
    {
        [TestClass]
        public class @caseinsensitivehashcodeprovider
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3951_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\caseinsensitivehashcodeprovider", "co3951ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3952_gethashcode_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\caseinsensitivehashcodeprovider", "co3952gethashcode_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8605_ctor_ci()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\caseinsensitivehashcodeprovider", "co8605ctor_ci.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8606_getdefault()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\caseinsensitivehashcodeprovider", "co8606getdefault.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@collectionbase
    {
        [TestClass]
        public class @collectionbase
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3995_allmethods()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\collectionbase", "co3995allmethods.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@comparer
    {
        [TestClass]
        public class @comparer
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1672_compare_oo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\comparer", "co1672compare_oo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@dictionaryentry
    {
        [TestClass]
        public class @dictionaryentry
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8607_get_key()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\dictionaryentry", "co8607get_key.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8608_get_value()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\dictionaryentry", "co8608get_value.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8610_set_value()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\dictionaryentry", "co8610set_value.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@hashtable
    {
        [TestClass]
        public class @hashtable
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1657_copyto_ai()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co1657copyto_ai.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1658_ctor_df()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co1658ctor_df.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1659_ctor_ifii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co1659ctor_ifii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1694_remove_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co1694remove_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2350_remove()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co2350remove.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3914_get_isreadonly()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co3914get_isreadonly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3923_get_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co3923get_issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3941_ctor_intihpic()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co3941ctor_intihpic.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3942_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co3942clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3949_getobjectdata_sersc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co3949getobjectdata_sersc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3950_ondeserialization_objde()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co3950ondeserialization_objde.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4307_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co4307ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4308_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co4308ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4309_ctor_int_float()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co4309ctor_int_float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4310_add_stress()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co4310add_stress.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4311_ctor_dictionary()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co4311ctor_dictionary.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8611_ctor_iii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co8611ctor_iii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8612_ctor_isii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co8612ctor_isii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8613_get_isfixedsize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\hashtable", "co8613get_isfixedsize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@queue
    {
        [TestClass]
        public class @queue
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1700_ctor_int_float()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co1700ctor_int_float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1702_dequeue()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co1702dequeue.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1704_enqueue()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co1704enqueue.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1708_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co1708get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1725_allsigns_queue_copycat()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co1725allsigns_queue_copycat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3100_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co3100ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3102_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co3102ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3106_peek()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co3106peek.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3110_toarray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co3110toarray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6078_synchronized_queue()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co6078synchronized_queue.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8614_contains_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co8614contains_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8615_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co8615clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8823_trimtosize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\queue", "co8823trimtosize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@readonlycollectionbase
    {
        [TestClass]
        public class @readonlycollectionbase
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3997_allmethods()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\readonlycollectionbase", "co3997allmethods.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@sortedlist
    {
        [TestClass]
        public class @sortedlist
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3718_getvaluelist()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co3718getvaluelist.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3916_get_isreadonly()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co3916get_isreadonly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3924_synchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co3924synchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3925_get_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co3925get_issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3953_multimethods()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co3953multimethods.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3954_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co3954clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4321_ctor_icomparer()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4321ctor_icomparer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4322_ctor_icomparer_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4322ctor_icomparer_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4323_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4323ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4324_ctor_dictionary()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4324ctor_dictionary.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4326_containskey()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4326containskey.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4327_containsvalue()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4327containsvalue.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4328_getobject()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4328getobject.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4329_clear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4329clear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4330_getbyindex_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4330getbyindex_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4331_indexofkey()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4331indexofkey.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4332_indexofvalue()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4332indexofvalue.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4334_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4334get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4335_get_keys()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4335get_keys.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4336_get_values()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4336get_values.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4338_getenumerator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4338getenumerator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4339_getkeylist()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4339getkeylist.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4340_remove_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4340remove_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4341_removeat_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4341removeat_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4345_trimtosize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4345trimtosize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4346_ctor_dictionary_icomparer()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4346ctor_dictionary_icomparer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4347_setobject_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4347setobject_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4348_getkey()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co4348getkey.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8616_get_capacity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co8616get_capacity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8617_set_capacity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co8617set_capacity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8618_get_isfixedsize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\sortedlist", "co8618get_isfixedsize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@specialized.@bitvector32
    {
        [TestClass]
        public class @bitvector32
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8662_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8662ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8663_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8663ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8664_ctor_bitvector32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8664ctor_bitvector32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8665_createmask()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8665createmask.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8666_createmask_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8666createmask_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8667_createsection_short()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8667createsection_short.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8668_createsection_short_section()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8668createsection_short_section.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8669_equals_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8669equals_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8670_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8670gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8671_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8671tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8672_tostring_bv()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8672tostring_bv.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8673_get_data()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8673get_data.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8674_get_item_section()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8674get_item_section.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8675_get_item_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8675get_item_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8676_set_item_section_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8676set_item_section_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8677_set_item_int_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\bitvector32", "co8677set_item_int_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@specialized.@hybriddictionary
    {
        [TestClass]
        public class @hybriddictionary
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8699_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8699ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8700_ctor_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8700ctor_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8701_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8701ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8702_ctor_int_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8702ctor_int_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8703_add_obj_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8703add_obj_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8704_clear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8704clear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8705_contains_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8705contains_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8709_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8709get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8710_get_isfixedsize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8710get_isfixedsize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8711_get_isreadonly()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8711get_isreadonly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8712_get_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8712get_issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8713_get_item_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8713get_item_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8714_get_keys()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8714get_keys.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8715_get_syncroot()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8715get_syncroot.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8716_get_values()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8716get_values.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8717_set_item_obj_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8717set_item_obj_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8773_performance()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\hybriddictionary", "co8773performance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@specialized.@listdictionary
    {
        [TestClass]
        public class @listdictionary
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8682_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8682ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8683_ctor_icomp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8683ctor_icomp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8684_add_obj_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8684add_obj_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8685_clear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8685clear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8686_contains_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8686contains_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8687_copyto_array_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8687copyto_array_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8689_remove_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8689remove_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8690_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8690get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8691_get_isfixedsize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8691get_isfixedsize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8692_get_isreadonly()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8692get_isreadonly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8693_get_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8693get_issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8694_get_item_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8694get_item_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8695_set_item_obj_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8695set_item_obj_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8696_get_keys()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8696get_keys.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8698_get_values()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8698get_values.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8772_performance()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\listdictionary", "co8772performance.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@specialized.@namevaluecollection
    {
        [TestClass]
        public class @namevaluecollection
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8718_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8718ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8719_ctor_nvc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8719ctor_nvc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8720_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8720ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8721_ctor_int_nvc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8721ctor_int_nvc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8722_add_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8722add_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8723_clear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8723clear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8724_copyto_array_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8724copyto_array_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8725_get_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8725get_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8726_get_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8726get_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8727_getkey_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8727getkey_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8728_getvalues_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8728getvalues_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8729_getvalues_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8729getvalues_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8730_haskeys()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8730haskeys.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8731_remove_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8731remove_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8732_set_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8732set_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8733_get_allkeys()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8733get_allkeys.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8734_get_item_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8734get_item_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8735_get_item_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8735get_item_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8736_set_item_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8736set_item_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8774_ctor_ihcp_ic()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8774ctor_ihcp_ic.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8775_ctor_int_ihcp_ic()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8775ctor_int_ihcp_ic.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8776_add_nvc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\namevaluecollection", "co8776add_nvc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@specialized.@stringcollection
    {
        [TestClass]
        public class @stringcollection
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8737_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8737ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8738_add_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8738add_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8739_addrange_stra()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8739addrange_stra.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8740_clear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8740clear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8741_contains_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8741contains_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8742_copyto_stra_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8742copyto_stra_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8743_getenumerator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8743getenumerator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8744_indexof_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8744indexof_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8745_insert_int_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8745insert_int_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8746_remove_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8746remove_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8747_removeat_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8747removeat_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8748_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8748get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8749_get_isreadonly()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8749get_isreadonly.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8750_get_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8750get_issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8751_get_item()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8751get_item.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8753_set_item()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringcollection", "co8753set_item.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@specialized.@stringdictionary
    {
        [TestClass]
        public class @stringdictionary
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8754_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8754ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8755_add_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8755add_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8756_clear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8756clear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8757_containskey_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8757containskey_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8758_containsvalue_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8758containsvalue_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8759_copyto_array_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8759copyto_array_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8760_getenumerator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8760getenumerator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8761_remove_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8761remove_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8762_get_count()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8762get_count.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8763_get_issynchronized()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8763get_issynchronized.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8764_get_item_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8764get_item_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8765_get_keys()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8765get_keys.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8767_get_values()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8767get_values.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8768_set_item_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringdictionary", "co8768set_item_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@specialized.@stringenumerator
    {
        [TestClass]
        public class @stringenumerator
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8769_movenext()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringenumerator", "co8769movenext.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8770_reset()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringenumerator", "co8770reset.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8771_get_current()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\specialized\stringenumerator", "co8771get_current.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@collections.@stack
    {
        [TestClass]
        public class @stack
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1726_allsigns_stack_copycat()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\stack", "co1726allsigns_stack_copycat.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3955_toarray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\stack", "co3955toarray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8619_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\stack", "co8619clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8620_contains_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"collections\stack", "co8620contains_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@convert
    {
        [TestClass]
        public class @convert
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1565_tobyte_allsigns()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co1565tobyte_allsigns.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6053_toboolean_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6053toboolean_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6054_tobyte_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6054tobyte_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6055_tochar_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6055tochar_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6057_todatetime_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6057todatetime_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6058_todecimal_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6058todecimal_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6059_todouble_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6059todouble_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6060_toint16_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6060toint16_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6061_toint32_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6061toint32_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6062_toint64_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6062toint64_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6064_tosbyte_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6064tosbyte_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6065_tosingle_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6065tosingle_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6067_touint16_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6067touint16_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6068_touint32_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6068touint32_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6069_touint64_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co6069touint64_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7063_isdbnull()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co7063isdbnull.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7064_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co7064gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8638_changetype_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co8638changetype_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8639_frombase64chararray_charr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co8639frombase64chararray_charr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8640_frombase64string_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co8640frombase64string_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8641_tobase64chararray_btarr_ii_charr_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co8641tobase64chararray_btarr_ii_charr_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8642_tobase64string_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co8642tobase64string_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8826_boxedobjectcheck()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"convert", "co8826boxedobjectcheck.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@datetime
    {
        [TestClass]
        public class @datetime
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @bug()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "bug.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1500_get_dayofweek()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1500get_dayofweek.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1504_get_dayofyear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1504get_dayofyear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1507_get_day()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1507get_day.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1509_get_date()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1509get_date.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1511_get_hour()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1511get_hour.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1512_addminutes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1512addminutes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1513_addmonths()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1513addmonths.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1514_addhours()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1514addhours.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1515_addseconds()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1515addseconds.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1516_addmilliseconds()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1516addmilliseconds.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1517_addyears()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1517addyears.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1518_addticks()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1518addticks.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1519_ctor_iiiiii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1519ctor_iiiiii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1520_touniversaltime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1520touniversaltime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1521_tolongdatestring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1521tolongdatestring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1522_get_now()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1522get_now.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1523_get_minute()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1523get_minute.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1524_get_month()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1524get_month.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1525_get_second()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1525get_second.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1526_get_year()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1526get_year.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1527_get_millisecond()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co1527get_millisecond.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3582_adddays()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co3582adddays.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3671_ctor_iii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co3671ctor_iii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3673_ctor_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co3673ctor_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3867_op_comparisonoperators()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co3867op_comparisonoperators.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5030_get_timeofday()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5030get_timeofday.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5031_get_today()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5031get_today.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5051_add()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5051add.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5052_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5052tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5053_isleapyear()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5053isleapyear.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5054_equals_dt_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5054equals_dt_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5055_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5055equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5056_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5056compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5057_compare()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5057compare.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5058_daysinmonth()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5058daysinmonth.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5059_get_ticks()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5059get_ticks.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5060_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5060gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5061_subtract_ts()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5061subtract_ts.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5062_subtract_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5062subtract_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5063_toshorttimestring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5063toshorttimestring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5064_toshortdatestring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5064toshortdatestring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5065_tolongtimestring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5065tolongtimestring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5066_tolocaltime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5066tolocaltime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5067_fromoadate()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5067fromoadate.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5068_tooadate()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5068tooadate.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5069_fromfiletime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5069fromfiletime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5070_tofiletime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5070tofiletime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5303_parse_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5303parse_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5306_tostring_static()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5306tostring_static.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5330_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5330tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5381_ctor_iiiiiid()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5381ctor_iiiiiid.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5506_parseexact_str_str_dtfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5506parseexact_str_str_dtfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5715_iconvertible_datetime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co5715iconvertible_datetime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6008_fromstring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co6008fromstring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7053_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co7053gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7056_getdatetimeformats()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co7056getdatetimeformats.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7057_getdatetimeformats_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co7057getdatetimeformats_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7058_getdatetimeformats_iserviceobjectprovider()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co7058getdatetimeformats_iserviceobjectprovider.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7059_getdatetimeformats_char_isp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co7059getdatetimeformats_char_isp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7060_ctor_iii_calendar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co7060ctor_iii_calendar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7061_ctor_iiiiiii_calendar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co7061ctor_iiiiiii_calendar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7062_ctor_iiiiii_calendar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co7062ctor_iiiiii_calendar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8567_get_utcnow()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co8567get_utcnow.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8569_parseexact_str_stra_ifp_dts()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co8569parseexact_str_stra_ifp_dts.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8570_parse_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co8570parse_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8571_parse_ifp_dts()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co8571parse_ifp_dts.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8572_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"datetime", "co8572tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@dbnull
    {
        [TestClass]
        public class @dbnull
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co7018_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7018tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7019_touint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7019touint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7020_touint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7020touint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7021_touint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7021touint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7022_totype_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7022totype_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7023_tosingle()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7023tosingle.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7024_tosbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7024tosbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7025_toint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7025toint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7026_toint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7026toint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7027_toint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7027toint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7028_todouble()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7028todouble.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7029_todecimal()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7029todecimal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7030_todatetime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7030todatetime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7031_tochar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7031tochar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7032_tobyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7032tobyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7033_toboolean()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7033toboolean.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7035_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7035gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7036_equals_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"dbnull", "co7036equals_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@decimal
    {
        [TestClass]
        public class @decimal
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3542_add()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3542add.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3543_op_addition()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3543op_addition.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3545_ctor_float()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3545ctor_float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3546_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3546ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3547_ctor_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3547ctor_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3550_ctor_intarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3550ctor_intarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3552_compare()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3552compare.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3554_divide()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3554divide.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3555_op_division()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3555op_division.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3556_op_unaryplus()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3556op_unaryplus.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3557_op_increment()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3557op_increment.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3558_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3558gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3559_equals_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3559equals_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3560_equals_decdec()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3560equals_decdec.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3561_op_equality()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3561op_equality.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3562_getbits()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3562getbits.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3565_multiply()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3565multiply.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3566_op_multiply()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3566op_multiply.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3567_negate()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3567negate.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3568_op_unarynegation()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3568op_unarynegation.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3569_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3569parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3570_round()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3570round.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3571_subtract()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3571subtract.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3572_op_subtraction()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3572op_subtraction.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3573_op_decrement()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3573op_decrement.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3574_truncate()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3574truncate.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3575_toint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3575toint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3576_op_explicit_returns_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3576op_explicit_returns_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3577_toint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3577toint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3578_op_explicit_returns_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3578op_explicit_returns_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3579_tobyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3579tobyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3624_toint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3624toint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3674_ctor_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3674ctor_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3680_todouble()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3680todouble.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3681_tosbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3681tosbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3682_tosingle()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3682tosingle.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3683_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3683tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3684_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3684tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3685_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3685tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3686_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3686tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3687_format_str_iformatprovider()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3687format_str_iformatprovider.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3688_parse_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3688parse_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3689_parse_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3689parse_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3866_op_greaterthan_etall()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co3866op_greaterthan_etall.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5314_touint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5314touint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5315_touint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5315touint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5316_touint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5316touint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5384_floor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5384floor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5385_ctor_uint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5385ctor_uint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5386_ctor_ulong()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5386ctor_ulong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5387_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5387compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5470_remainder_dec_dec()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5470remainder_dec_dec.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5716_iconvertible_decimal()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co5716iconvertible_decimal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7052_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co7052gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7084_ctor_int32_int32_int32_boolean_byte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co7084ctor_int32_int32_int32_boolean_byte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8574_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"decimal", "co8574parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@delegate
    {
        [TestClass]
        public class @delegate
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1162_invoke()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co1162invoke.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1901_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co1901clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3152_getmethod()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co3152getmethod.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3154_gettarget()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co3154gettarget.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3156_getinvocationlist()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co3156getinvocationlist.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3158_combine()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co3158combine.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3160_combine()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co3160combine.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3164_dynamicinvoke()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co3164dynamicinvoke.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3871_op_comparisonoperators()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"delegate", "co3871op_comparisonoperators.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@double
    {
        [TestClass]
        public class @double
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1645_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co1645parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3448_isinfinity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co3448isinfinity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3449_isnegativeinfinity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co3449isnegativeinfinity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3452_ispositiveinfinity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co3452ispositiveinfinity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3453_isnan()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co3453isnan.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4262_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co4262ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4264_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co4264gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4265_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co4265equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4267_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co4267tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5003_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5003compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5004_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5004tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5012_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5012tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5014_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5014tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5019_parse_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5019parse_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5024_parse_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5024parse_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5105_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5105ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5119_format_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5119format_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5717_iconvertible_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co5717iconvertible_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6002_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co6002parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7051_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co7051gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8824_tryparse_str_ns_if_dbl()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"double", "co8824tryparse_str_ns_if_dbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@enum
    {
        [TestClass]
        public class @enum
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3876_getunderlyingtype_tp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3876getunderlyingtype_tp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3877_isdefined_tp_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3877isdefined_tp_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3878_getvalues_tp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3878getvalues_tp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3879_parse_tp_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3879parse_tp_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3880_format()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3880format.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3881_compareto_o()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3881compareto_o.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3898_iconvertible_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3898iconvertible_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3899_toobject_all()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3899toobject_all.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3900_format_tpobjstr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3900format_tpobjstr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3901_tostring_strifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3901tostring_strifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3962_parse_tpstrbln()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co3962parse_tpstrbln.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8504_equals_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8504equals_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8505_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8505gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8506_getname_typeobj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8506getname_typeobj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8507_getnames_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8507getnames_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8508_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8508tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8509_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8509tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8510_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8510tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8511_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8511gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8825_iconvertible()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"enum", "co8825iconvertible.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@environment
    {
        [TestClass]
        public class @environment
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1051_getenvironmentvariable()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co1051getenvironmentvariable.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1480_get_tickcount()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co1480get_tickcount.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5324_set_exitcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co5324set_exitcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5325_get_exitcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co5325get_exitcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5483_getenvironmentvariables()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co5483getenvironmentvariables.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5521_set_currentdirectory_dir()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co5521set_currentdirectory_dir.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5522_get_currentdirectory()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co5522get_currentdirectory.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7010_get_newline()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co7010get_newline.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7012_get_version()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co7012get_version.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7013_get_workingset()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co7013get_workingset.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7014_get_systemdirectory()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co7014get_systemdirectory.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7015_get_stacktrace()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co7015get_stacktrace.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7016_getlogicaldrives()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co7016getlogicaldrives.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8518_getcommandlineargs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"environment", "co8518getcommandlineargs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@eventargs
    {
        [TestClass]
        public class @eventargs
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5429_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"eventargs", "co5429ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@exception
    {
        [TestClass]
        public class @exception
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1367_catch_block()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co1367catch_block.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1369_finally_block()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co1369finally_block.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2206_get_message()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co2206get_message.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2208_get_stacktrace()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co2208get_stacktrace.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2212_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co2212tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3265_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co3265ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3266_ctor_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co3266ctor_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5220_get_source()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co5220get_source.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5221_get_targetsite()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co5221get_targetsite.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7085_set_source()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"exception", "co7085set_source.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@guid
    {
        [TestClass]
        public class @guid
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1183_equals_dupl2()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co1183equals_dupl2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1184_ctor_default_dupl2()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co1184ctor_default_dupl2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1186_ctor_string()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co1186ctor_string.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1187_tostring_dupl2()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co1187tostring_dupl2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1188_gethashcode_dupl2()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co1188gethashcode_dupl2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1189_ctor_4221arr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co1189ctor_4221arr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3870_op_comparisonoperators()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co3870op_comparisonoperators.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5321_ctor_bytarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co5321ctor_bytarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5322_tobytearray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co5322tobytearray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5323_ctor_issuuuuuuuu()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co5323ctor_issuuuuuuuu.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7074_ctor_uint32_uint16_uint16_uint16_bbbbbbbb()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co7074ctor_uint32_uint16_uint16_uint16_bbbbbbbb.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8521_compareto_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co8521compareto_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8575_newguid()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co8575newguid.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8576_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co8576tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8577_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"guid", "co8577tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@int16
    {
        [TestClass]
        public class @int16
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1622_cons_eq_gv_ghc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co1622cons_eq_gv_ghc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5000_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5000compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5001_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5001parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5006_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5006tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5007_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5007tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5016_parse_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5016parse_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5021_parse_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5021parse_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5106_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5106ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5120_format_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5120format_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5718_iconvertible_int16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co5718iconvertible_int16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6003_fromstring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co6003fromstring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7050_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co7050gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8578_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co8578tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8579_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int16", "co8579parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@int32
    {
        [TestClass]
        public class @int32
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1115_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co1115parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1117_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co1117compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1612_cons_eq_gv_ghc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co1612cons_eq_gv_ghc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1613_tostring_int_static_dupl2()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co1613tostring_int_static_dupl2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5008_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co5008tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5009_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co5009tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5017_parse_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co5017parse_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5022_parse_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co5022parse_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5107_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co5107ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5121_format_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co5121format_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5719_iconvertible_int32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co5719iconvertible_int32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6004_fromstring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co6004fromstring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7049_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co7049gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8580_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co8580tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8581_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int32", "co8581parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@int64
    {
        [TestClass]
        public class @int64
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1118_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co1118compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1632_cons_eq_gv_ghc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co1632cons_eq_gv_ghc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5002_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co5002parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5010_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co5010tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5011_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co5011tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5018_parse_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co5018parse_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5023_parse_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co5023parse_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5108_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co5108ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5122_format_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co5122format_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5720_iconvertible_int64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co5720iconvertible_int64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6005_fromstring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co6005fromstring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7048_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co7048gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8582_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co8582tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8583_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"int64", "co8583parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@intptr
    {
        [TestClass]
        public class @intptr
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8522_ctor_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8522ctor_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8523_ctor_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8523ctor_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8524_ctor_void()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8524ctor_void.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8525_equals_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8525equals_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8526_get_size()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8526get_size.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8527_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8527gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8528_operator_multi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8528operator_multi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8529_toint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8529toint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8530_toint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8530toint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8531_topointer()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8531topointer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8532_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"intptr", "co8532tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@binaryreader
    {
        [TestClass]
        public class @binaryreader
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5631_ctor_stream()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5631ctor_stream.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5632_ctor_stream_enc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5632ctor_stream_enc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5633_close()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5633close.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5634_get_basestream()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5634get_basestream.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5635_peekchar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5635peekchar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5636_read()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5636read.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5637_read_charr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5637read_charr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5638_readchar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5638readchar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5639_readchars()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5639readchars.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5640_readbytes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5640readbytes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5641_readboolean()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5641readboolean.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5642_readdouble()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5642readdouble.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5643_readint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5643readint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5644_readint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5644readint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5645_readint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5645readint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5646_readsingle()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5646readsingle.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5647_readuint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5647readuint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5648_readuint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5648readuint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5649_readuint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5649readuint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5650_readsbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5650readsbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5651_readstring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5651readstring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5653_readbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5653readbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5654_read_barr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co5654read_barr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9300_readdecimal()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binaryreader", "co9300readdecimal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@binarywriter
    {
        [TestClass]
        public class @binarywriter
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5536_ctor_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5536ctor_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5537_write_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5537write_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5538_write_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5538write_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5539_write_dbl()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5539write_dbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5540_write_i16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5540write_i16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5541_write_i32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5541write_i32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5542_write_i64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5542write_i64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5543_write_sgl()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5543write_sgl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5544_write_ui16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5544write_ui16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5545_write_ui32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5545write_ui32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5546_write_ui64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5546write_ui64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5547_write_sbyt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5547write_sbyt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5548_write_byt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5548write_byt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5549_write_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5549write_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5550_writestring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5550writestring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5552_write_barr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5552write_barr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5553_write_carr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5553write_carr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5554_write_carr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5554write_carr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5561_seek_i_so()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5561seek_i_so.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5738_get_basestream()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5738get_basestream.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5739_flush()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5739flush.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5740_ctor_stream_enc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5740ctor_stream_enc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5741_close()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5741close.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5742_write_barr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co5742write_barr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9301_write_dcml()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\binarywriter", "co9301write_dcml.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@bufferedstream
    {
        [TestClass]
        public class @bufferedstream
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5602_ctor_stream()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5602ctor_stream.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5603_ctor_stream_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5603ctor_stream_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5604_close()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5604close.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5605_flush()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5605flush.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5606_get_canread()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5606get_canread.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5607_get_canwrite()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5607get_canwrite.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5608_get_canseek()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5608get_canseek.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5609_setlength()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5609setlength.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5610_get_length()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5610get_length.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5611_set_position()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5611set_position.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5612_get_position()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5612get_position.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5614_seek_i64_so()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5614seek_i64_so.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5615_write_barr_i_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5615write_barr_i_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5616_read_barr_i_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5616read_barr_i_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5743_writebyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5743writebyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5744_readbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\bufferedstream", "co5744readbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@directory
    {
        [TestClass]
        public class @directory
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5512_getlogicaldrives()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5512getlogicaldrives.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5513_createdirectory_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5513createdirectory_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5656_get_isdirectory()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5656get_isdirectory.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5657_get_isfile()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5657get_isfile.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5658_exists_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5658exists_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5663_delete_str_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5663delete_str_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5664_delete_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5664delete_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5669_getdirectoryroot_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5669getdirectoryroot_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5670_getdirectories_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5670getdirectories_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5671_getfiles_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5671getfiles_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5672_getfiles_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5672getfiles_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5674_getdirectories_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5674getdirectories_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5736_setcurrentdirectory_dir()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5736setcurrentdirectory_dir.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5737_getcurrentdirectory()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5737getcurrentdirectory.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5751_getparent_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5751getparent_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5759_move_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co5759move_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9025_getcreationtime_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co9025getcreationtime_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9026_setcreationtime_str_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co9026setcreationtime_str_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9027_getlastaccesstime_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co9027getlastaccesstime_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9028_setlastaccesstime_str_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co9028setlastaccesstime_str_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9029_getlastwritetime_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co9029getlastwritetime_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9030_setlastwritetime_str_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co9030setlastwritetime_str_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9034_getfilesystementries_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co9034getfilesystementries_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9035_getfilesystementries_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directory", "co9035getfilesystementries_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@directoryinfo
    {
        [TestClass]
        public class @directoryinfo
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5510_ctor_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5510ctor_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5511_get_fullname()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5511get_fullname.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5516_createsubdirectory_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5516createsubdirectory_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5517_delete()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5517delete.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5518_get_creationtime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5518get_creationtime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5519_set_attributes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5519set_attributes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5520_get_attributes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5520get_attributes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5523_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5523tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5525_moveto_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5525moveto_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5659_exists()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5659exists.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5660_get_name()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5660get_name.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5662_delete_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5662delete_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5665_get_parent()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5665get_parent.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5666_get_root()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5666get_root.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5667_getdirectories()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5667getdirectories.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5668_getfiles()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5668getfiles.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5673_getfiles_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5673getfiles_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5675_getdirectories_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5675getdirectories_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5676_getfilesysteminfos()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5676getfilesysteminfos.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5677_getfilesysteminfos_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5677getfilesysteminfos_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5748_get_lastaccesstime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5748get_lastaccesstime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5749_get_lastwritetime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5749get_lastwritetime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5767_refresh()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co5767refresh.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9031_setlastaccesstime_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co9031setlastaccesstime_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9032_setcreationtime_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co9032setcreationtime_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9033_setlastwritetime_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co9033setlastwritetime_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9040_create()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co9040create.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9041_extension()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\directoryinfo", "co9041extension.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@file
    {
        [TestClass]
        public class @file
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5586_copy_str_str_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5586copy_str_str_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5587_copy_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5587copy_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5589_create_str_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5589create_str_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5683_exists_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5683exists_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5686_appendtext_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5686appendtext_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5687_changeextension_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5687changeextension_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5688_createtext_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5688createtext_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5690_delete_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5690delete_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5692_opentext_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5692opentext_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5710_move_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5710move_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5754_openread_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5754openread_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5755_openwrite_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co5755openwrite_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9001_getattributes_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9001getattributes_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9002_setattributes_str_attrs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9002setattributes_str_attrs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9003_getcreationtime_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9003getcreationtime_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9004_setcreationtime_str_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9004setcreationtime_str_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9005_getlastaccesstime_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9005getlastaccesstime_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9006_setlastaccesstime_str_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9006setlastaccesstime_str_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9007_getlastwritetime_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9007getlastwritetime_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9008_setlastwritetime_str_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9008setlastwritetime_str_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9009_open_str_fm()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9009open_str_fm.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9010_open_str_fm_fa()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9010open_str_fm_fa.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9011_open_str_fm_fa_fs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\file", "co9011open_str_fm_fa_fs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@fileinfo
    {
        [TestClass]
        public class @fileinfo
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5530_ctor_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5530ctor_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5531_copyto_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5531copyto_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5585_copyto_str_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5585copyto_str_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5588_create_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5588create_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5682_delete()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5682delete.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5685_appendtext()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5685appendtext.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5689_createtext()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5689createtext.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5691_opentext()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5691opentext.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5693_open_fm()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5693open_fm.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5694_get_name()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5694get_name.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5697_get_directoryname()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5697get_directoryname.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5698_get_directory()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5698get_directory.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5699_get_length()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5699get_length.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5700_set_attributes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5700set_attributes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5701_get_attributes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5701get_attributes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5703_get_creationtime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5703get_creationtime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5704_get_lastaccesstime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5704get_lastaccesstime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5705_get_lastwritetime()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5705get_lastwritetime.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5706_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5706tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5708_open_fm_fa()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5708open_fm_fa.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5709_open_fm_fa_fs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5709open_fm_fa_fs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5711_moveto_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5711moveto_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5752_openread()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5752openread.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5753_openwrite()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5753openwrite.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5768_refresh()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co5768refresh.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9012_create()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co9012create.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9013_set_creationtime_dt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co9013set_creationtime_dt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9014_exists()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co9014exists.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9015_extension()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co9015extension.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9016_fullname()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\fileinfo", "co9016fullname.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@filestream
    {
        [TestClass]
        public class @filestream
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5535_ctor_i_fa()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5535ctor_i_fa.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5575_ctor_str_fm()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5575ctor_str_fm.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5576_set_position()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5576set_position.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5577_get_position()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5577get_position.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5578_setlength_i64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5578setlength_i64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5579_get_canread()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5579get_canread.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5580_get_canwrite()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5580get_canwrite.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5581_get_length()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5581get_length.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5715_write_barr_i_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5715write_barr_i_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5716_read_barr_i_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5716read_barr_i_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5718_close()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5718close.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5719_get_canseek()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5719get_canseek.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5721_readbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5721readbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5722_seek_i64_so()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5722seek_i64_so.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5723_flush()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5723flush.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5724_ctor_str_fm_fa()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5724ctor_str_fm_fa.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5725_ctor_str_fm_fa_fs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5725ctor_str_fm_fa_fs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5726_ctor_str_fm_fa_fs_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5726ctor_str_fm_fa_fs_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5728_endwrite()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5728endwrite.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5745_writebyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5745writebyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5760_ctor_i_fa_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5760ctor_i_fa_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5761_ctor_i_fa_b_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5761ctor_i_fa_b_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5762_ctor_i_fa_b_i_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "co5762ctor_i_fa_b_i_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @filest_ream01()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "filestream01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @filest_ream_lock()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "filestream_lock.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @filest_ream_unlock()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\filestream", "filestream_unlock.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@memorystream
    {
        [TestClass]
        public class @memorystream
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1801_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1801ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1802_ctor_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1802ctor_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1803_ctor_bb()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1803ctor_bb.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1804_ctor_bii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1804ctor_bii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1805_ctor_biib()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1805ctor_biib.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1806_ctor_biibb()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1806ctor_biibb.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1807_ctor_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1807ctor_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1808_flush()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1808flush.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1809_get_canread()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1809get_canread.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1810_get_canseek()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1810get_canseek.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1811_get_canwrite()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1811get_canwrite.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1812_close()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1812close.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1814_get_position()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1814get_position.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1815_set_position()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1815set_position.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1816_getbuffer()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1816getbuffer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1821_write_bii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co1821write_bii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5731_get_length()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5731get_length.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5732_read_barr_i_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5732read_barr_i_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5734_seek_i64_so()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5734seek_i64_so.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5735_setlength_i64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5735setlength_i64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5746_writebyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5746writebyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5757_set_capacity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5757set_capacity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5758_get_capacity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5758get_capacity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5765_toarray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5765toarray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5766_writeto_strm()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "co5766writeto_strm.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @memory_stream01()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "memorystream01.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @memory_stream02()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\memorystream", "memorystream02.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@path
    {
        [TestClass]
        public class @path
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5702_hasextension_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co5702hasextension_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9050_changeextension_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9050changeextension_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9051_combine_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9051combine_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9052_getdirectoryname_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9052getdirectoryname_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9053_getextension_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9053getextension_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9054_getfilename_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9054getfilename_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9055_getfilenwextension_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9055getfilenwextension_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9056_getfullpath_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9056getfullpath_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9057_getpathroot_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9057getpathroot_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9058_gettempfilename()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9058gettempfilename.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9059_gettemppath()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9059gettemppath.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9061_ispathrooted_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9061ispathrooted_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9062_altdirectoryseparatorchar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9062altdirectoryseparatorchar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9063_directoryseparatorchar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9063directoryseparatorchar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9064_invalidpathchars()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9064invalidpathchars.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9065_pathseparator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9065pathseparator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9066_volumeseparatorchar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\path", "co9066volumeseparatorchar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@stream
    {
        [TestClass]
        public class @stream
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3965_streammethods()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stream", "co3965streammethods.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@streamreader
    {
        [TestClass]
        public class @streamreader
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5617_ctor_stream()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5617ctor_stream.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5618_ctor_stream_enc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5618ctor_stream_enc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5619_ctor_stream_enc_b_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5619ctor_stream_enc_b_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5620_ctor_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5620ctor_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5621_ctor_str_enc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5621ctor_str_enc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5622_ctor_str_enc_b_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5622ctor_str_enc_b_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5623_close()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5623close.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5624_get_basestream()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5624get_basestream.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5625_getencoding()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5625getencoding.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5626_read()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5626read.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5627_peek()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5627peek.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5628_read_charr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5628read_charr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5629_readtoend()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5629readtoend.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5630_readline()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co5630readline.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9302_ctor_stream_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co9302ctor_stream_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9303_ctor_stream_enc_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co9303ctor_stream_enc_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9304_ctor_str_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co9304ctor_str_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9305_ctor_str_enc_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "co9305ctor_str_enc_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @stream_read()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamreader", "streamread.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@streamwriter
    {
        [TestClass]
        public class @streamwriter
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5560_ctor_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5560ctor_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5563_write_ch()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5563write_ch.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5564_write_charr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5564write_charr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5565_write_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5565write_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5591_set_autoflush()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5591set_autoflush.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5592_get_autoflush()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5592get_autoflush.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5593_get_basestream()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5593get_basestream.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5594_ctor_str_b()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5594ctor_str_b.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5595_close()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5595close.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5596_flush()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5596flush.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5597_ctor_str_b_enc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5597ctor_str_b_enc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5598_ctor_str_b_enc_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5598ctor_str_b_enc_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5599_ctor_stream()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5599ctor_stream.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5600_ctor_stream_enc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5600ctor_stream_enc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5601_ctor_enc_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\streamwriter", "co5601ctor_enc_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@stringreader
    {
        [TestClass]
        public class @stringreader
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5566_ctor_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringreader", "co5566ctor_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5567_read()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringreader", "co5567read.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5568_peek()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringreader", "co5568peek.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5569_readtoend()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringreader", "co5569readtoend.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5570_readline()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringreader", "co5570readline.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5571_read_charr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringreader", "co5571read_charr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5572_close()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringreader", "co5572close.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@io.@stringwriter
    {
        [TestClass]
        public class @stringwriter
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5555_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co5555ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5556_ctor_sb()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co5556ctor_sb.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5557_write_ch()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co5557write_ch.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5558_write_charr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co5558write_charr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5559_write_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co5559write_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5561_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co5561tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5562_getstringbuilder()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co5562getstringbuilder.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9400_writeline_charr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co9400writeline_charr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9401_write_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co9401write_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9402_writeline_bool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"io\stringwriter", "co9402writeline_bool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@math
    {
        [TestClass]
        public class @math
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co4045_abs_byte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4045abs_byte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4046_abs_short()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4046abs_short.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4047_abs_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4047abs_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4048_abs_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4048abs_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4049_abs_float()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4049abs_float.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4050_abs_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4050abs_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4051_max_sbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4051max_sbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4052_max_short()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4052max_short.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4053_max_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4053max_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4054_max_single()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4054max_single.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4055_max_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4055max_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4057_min_sbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4057min_sbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4058_min_short()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4058min_short.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4059_min_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4059min_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4060_min_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4060min_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4061_min_single()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4061min_single.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4062_min_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4062min_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4063_ieeeremainder()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4063ieeeremainder.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4064_log()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4064log.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4065_acos()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4065acos.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4066_asin()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4066asin.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4067_atan()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4067atan.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4068_atan2()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4068atan2.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4069_ceiling_dbl()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4069ceiling_dbl.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4070_cos()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4070cos.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4071_exp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4071exp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4072_floor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4072floor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4073_log10()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4073log10.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4074_pow()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4074pow.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4075_round()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4075round.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4077_sin()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4077sin.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4078_sqrt()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4078sqrt.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4079_tan()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4079tan.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4082_log_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4082log_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4647_max_ubyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4647max_ubyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4648_min_ubyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co4648min_ubyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5396_max_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co5396max_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5397_max_uint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co5397max_uint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5398_max_uint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co5398max_uint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5399_max_uint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co5399max_uint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5400_min_uint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co5400min_uint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5401_min_uint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co5401min_uint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5402_min_uint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co5402min_uint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7000_sign_int32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7000sign_int32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7001_sign_int64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7001sign_int64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7002_sign_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7002sign_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7003_sign_decimal()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7003sign_decimal.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7005_cosh_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7005cosh_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7006_sinh_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7006sinh_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7007_tanh_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7007tanh_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7008_log_double()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7008log_double.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7066_sign_int16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7066sign_int16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7067_sign_sbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co7067sign_sbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8533_round_dbl_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"math", "co8533round_dbl_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@random
    {
        [TestClass]
        public class @random
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co4238_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"random", "co4238ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4239_ctor_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"random", "co4239ctor_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4240_next()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"random", "co4240next.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4241_next_i_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"random", "co4241next_i_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4242_next_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"random", "co4242next_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4243_nextdouble()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"random", "co4243nextdouble.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4244_nextbytes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"random", "co4244nextbytes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5380_sample()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"random", "co5380sample.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@formatterconverter
    {
        [TestClass]
        public class @formatterconverter
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3889_to_most()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\formatterconverter", "co3889to_most.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@formatterservices
    {
        [TestClass]
        public class @formatterservices
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3856_getserializablemembers_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\formatterservices", "co3856getserializablemembers_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3857_getserializablemembers_type_sc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\formatterservices", "co3857getserializablemembers_type_sc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3858_getobjectdata()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\formatterservices", "co3858getobjectdata.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3860_populateobjectmembers()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\formatterservices", "co3860populateobjectmembers.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3956_gettypefromassembly_asmstr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\formatterservices", "co3956gettypefromassembly_asmstr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@objectidgenerator
    {
        [TestClass]
        public class @objectidgenerator
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5240_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectidgenerator", "co5240ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5265_hasid_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectidgenerator", "co5265hasid_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@objectmanager
    {
        [TestClass]
        public class @objectmanager
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3861_recordfixup()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co3861recordfixup.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3862_recorddelayedfixup()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co3862recorddelayedfixup.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3890_recordarrayelementfixup_lil()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co3890recordarrayelementfixup_lil.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3891_recordarrayelementfixup_lial()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co3891recordarrayelementfixup_lial.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3892_dofixups()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co3892dofixups.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3893_getobject_lng()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co3893getobject_lng.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3894_registerobject_oiser()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co3894registerobject_oiser.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5244_registerobject_obj_lng()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co5244registerobject_obj_lng.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8626_ctor_is()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co8626ctor_is.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8627_raisedeserializationevent()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co8627raisedeserializationevent.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8628_registerobject_oisim()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\objectmanager", "co8628registerobject_oisim.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@pseudomlformatter
    {
        [TestClass]
        public class @pseudomlformatter
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1011_serialize()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\pseudomlformatter", "co1011serialize.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@serializationentry
    {
        [TestClass]
        public class @serializationentry
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8630_get_name()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationentry", "co8630get_name.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8631_get_value()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationentry", "co8631get_value.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8632_get_objecttype()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationentry", "co8632get_objecttype.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@serializationinfo
    {
        [TestClass]
        public class @serializationinfo
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3847_get_fulltypename()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3847get_fulltypename.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3848_get_membercount()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3848get_membercount.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3849_getvalue_str_tp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3849getvalue_str_tp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3851_getenumerator()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3851getenumerator.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3884_ctor_tp_if()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3884ctor_tp_if.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3885_addvalue_most()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3885addvalue_most.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3888_get_most()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3888get_most.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3959_get_assemblyname()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3959get_assemblyname.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3960_set_assemblyname()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co3960set_assemblyname.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8633_set_fulltypename()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co8633set_fulltypename.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8634_settype_type()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfo", "co8634settype_type.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@serializationinfoenumerator
    {
        [TestClass]
        public class @serializationinfoenumerator
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3852_movenext()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfoenumerator", "co3852movenext.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3853_get_name()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfoenumerator", "co3853get_name.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3854_get_value()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfoenumerator", "co3854get_value.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3957_get_current()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfoenumerator", "co3957get_current.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3958_get_objecttype()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfoenumerator", "co3958get_objecttype.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8635_reset()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\serializationinfoenumerator", "co8635reset.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@streamingcontext
    {
        [TestClass]
        public class @streamingcontext
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5247_ctor_scs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\streamingcontext", "co5247ctor_scs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5248_ctor_scs_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\streamingcontext", "co5248ctor_scs_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5249_get_state()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\streamingcontext", "co5249get_state.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5250_get_context()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\streamingcontext", "co5250get_context.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5251_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\streamingcontext", "co5251equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5252_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\streamingcontext", "co5252gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@runtime.@serialization.@surrogateselector
    {
        [TestClass]
        public class @surrogateselector
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5246_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\surrogateselector", "co5246ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5285_addsurrogate_typ_iss()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\surrogateselector", "co5285addsurrogate_typ_iss.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5286_getsurrogate_typ_sc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\surrogateselector", "co5286getsurrogate_typ_sc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5287_chainselector()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\surrogateselector", "co5287chainselector.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5288_removesurrogate_typ()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\surrogateselector", "co5288removesurrogate_typ.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8636_getnextselector()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"runtime\serialization\surrogateselector", "co8636getnextselector.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@sbyte
    {
        [TestClass]
        public class @sbyte
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3658_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co3658compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3661_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co3661tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3662_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co3662tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3663_parse_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co3663parse_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3664_parse_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co3664parse_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3665_format_str_iformatprovider()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co3665format_str_iformatprovider.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4223_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co4223equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4225_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co4225tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4226_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co4226gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4227_tostring_byte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co4227tostring_byte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4228_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co4228parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5725_iconvertible_sbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co5725iconvertible_sbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6006_fromstring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co6006fromstring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7047_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co7047gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8584_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co8584tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8585_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"sbyte", "co8585parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@single
    {
        [TestClass]
        public class @single
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1120_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co1120compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3447_isinfinity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co3447isinfinity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3450_isnegativeinfinity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co3450isnegativeinfinity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3451_ispositiveinfinity()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co3451ispositiveinfinity.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3454_isnan()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co3454isnan.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4250_parse()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co4250parse.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4251_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co4251equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4253_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co4253gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4257_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co4257tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4258_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co4258tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5013_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co5013tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5015_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co5015tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5020_parse_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co5020parse_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5025_parse_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co5025parse_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5123_format_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co5123format_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5724_iconvertible_single()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co5724iconvertible_single.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6007_fromstring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co6007fromstring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7046_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co7046gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8586_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co8586tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8587_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"single", "co8587parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@string
    {
        [TestClass]
        public class @string
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1000_toupper()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1000toupper.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1010_tolower()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1010tolower.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1022_get_length()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1022get_length.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1030_compare_strstrbool()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1030compare_strstrbool.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, additionalFilesFolder: Path.Combine(CompilerHelper.SscliSourcePath, @"..\..\utilities"), additionalFilesPattern: new[] { "genstrings.cs" });
            }

            [TestMethod]
            public void @co1090_indexof_charint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1090indexof_charint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1091_indexof_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1091indexof_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1100_equals_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1100equals_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1126_substring_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1126substring_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1130_get_chars()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1130get_chars.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1150_endswith_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1150endswith_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1160_lastindexof_charint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1160lastindexof_charint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1161_lastindexof_chararrint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1161lastindexof_chararrint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1454_ctor_strings()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1454ctor_strings.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1490_tochararray_intint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1490tochararray_intint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co1491_get_chars_chararrii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co1491get_chars_chararrii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2345_replace_cc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co2345replace_cc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2346_replace_ss()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co2346replace_ss.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3038_trimend_charr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3038trimend_charr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3039_trimstart_charr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3039trimstart_charr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3040_trim_charr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3040trim_charr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3041_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3041tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3042_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3042gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3043_split_charr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3043split_charr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3044_join_strstrarrii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3044join_strstrarrii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3045_join_strstrarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3045join_strstrarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3130_ctor_strstr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3130ctor_strstr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3140_equals_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3140equals_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3146_ctor_chararr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3146ctor_chararr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3148_ctor_chararrintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3148ctor_chararrintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3408_compare_strstr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3408compare_strstr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3409_compare_strintstrintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3409compare_strintstrintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3411_compareto_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3411compareto_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3416_indexof_charintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3416indexof_charintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3417_indexof_chararr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3417indexof_chararr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3418_indexof_chararrint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3418indexof_chararrint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3419_indexof_chararrintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3419indexof_chararrintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3420_indexof_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3420indexof_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3421_indexof_strint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3421indexof_strint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3422_indexof_strintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3422indexof_strintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3423_lastindexof_charintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3423lastindexof_charintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3424_lastindexof_chararr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3424lastindexof_chararr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3425_lastindexof_chararrintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3425lastindexof_chararrintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3426_lastindexof_char()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3426lastindexof_char.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3427_startswith_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3427startswith_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3428_trim()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3428trim.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3440_replace_charchar()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3440replace_charchar.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3441_compare_strintstrintintboolloc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3441compare_strintstrintintboolloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3442_tolower_loc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3442tolower_loc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3443_toupper_loc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3443toupper_loc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3445_compareordinal_strstr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3445compareordinal_strstr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3446_compareordinal_strintstrintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3446compareordinal_strintstrintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3709_compare_strstrboolloc()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3709compare_strstrboolloc.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3710_substring_intint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3710substring_intint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3711_lastindexof_strint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3711lastindexof_strint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3712_format_strobjarrifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3712format_strobjarrifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3713_lastindexof_strintint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co3713lastindexof_strintint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4000_ctor_strarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4000ctor_strarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4825_isinterned()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4825isinterned.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4827_ctor_sbyte_int_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4827ctor_sbyte_int_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4828_ctor_sbyte()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4828ctor_sbyte.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4829_concat_string_string_string()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4829concat_string_string_string.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4830_concat_string_string()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4830concat_string_string.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4832_ctor_char_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4832ctor_char_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4833_copy()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4833copy.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co4834_equals_string_string()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co4834equals_string_string.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5050_format_objs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5050format_objs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5100_format_str_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5100format_str_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5101_format_str_4objs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5101format_str_4objs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5102_format_str_3objs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5102format_str_3objs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5103_format_str_2objs()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5103format_str_2objs.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5150_compareto_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5150compareto_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5151_equals_str_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5151equals_str_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5152_tochararray()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5152tochararray.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5153_split_charr_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5153split_charr_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5154_remove_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5154remove_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5155_insert_is()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5155insert_is.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5156_lastindexof_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5156lastindexof_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5463_concat_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5463concat_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5464_concat_oo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5464concat_oo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5465_concat_ooo()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5465concat_ooo.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5467_concat_oarr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5467concat_oarr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5472_copyto_i_charr_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5472copyto_i_charr_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5504_ctor_charptr()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5504ctor_charptr.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5800_intern()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co5800intern.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9301_padleft()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co9301padleft.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9302_padleft_ch_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co9302padleft_ch_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9303_padright_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co9303padright_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co9304_padright_ch_i()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"string", "co9304padright_ch_i.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@timespan
    {
        [TestClass]
        public class @timespan
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co1528_ctor_iiii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co1528ctor_iiii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2900_ctor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2900ctor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2902_ctor_long()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2902ctor_long.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2904_ctor_iii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2904ctor_iii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2910_add()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2910add.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2912_compare()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2912compare.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2914_compareto()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2914compareto.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2916_get_days()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2916get_days.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2918_duration()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2918duration.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2920_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2920equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2922_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2922equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2924_get_days()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2924get_days.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2926_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2926gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2928_get_hours()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2928get_hours.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2930_frommilliseconds()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2930frommilliseconds.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2932_get_minutes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2932get_minutes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2934_get_seconds()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2934get_seconds.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2936_get_ticks()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2936get_ticks.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2938_get_totaldays()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2938get_totaldays.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2940_get_totalhours()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2940get_totalhours.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2944_get_totalminutes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2944get_totalminutes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2948_fromhours()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2948fromhours.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2952_fromminutes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2952fromminutes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2954_negate()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2954negate.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2956_fromseconds()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2956fromseconds.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2958_subtract()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2958subtract.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2960_fromticks()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2960fromticks.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co2962_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co2962tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3868_op_comparisonoperators()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co3868op_comparisonoperators.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5337_tostring_ts()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co5337tostring_ts.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5338_parse_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co5338parse_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5339_fromdays()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co5339fromdays.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5340_get_totalminutes()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co5340get_totalminutes.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5341_get_totalseconds()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co5341get_totalseconds.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6009_fromstring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co6009fromstring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8588_ctor_iiiii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timespan", "co8588ctor_iiiii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@timezone
    {
        [TestClass]
        public class @timezone
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5453_getcurrenttimezone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"timezone", "co5453getcurrenttimezone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@uint16
    {
        [TestClass]
        public class @uint16
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5723_iconvertible_uint16()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co5723iconvertible_uint16.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6012_compareto_ob()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6012compareto_ob.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6013_equals_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6013equals_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6014_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6014tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6015_format_u16_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6015format_u16_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6016_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6016tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6017_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6017gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6019_parse_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6019parse_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6020_parse_str_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6020parse_str_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6021_parse_str_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6021parse_str_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6022_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6022tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6023_tostring_uint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co6023tostring_uint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7045_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co7045gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7070_fromstring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co7070fromstring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8589_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co8589tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8590_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint16", "co8590parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@uint32
    {
        [TestClass]
        public class @uint32
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5722_iconvertible_uint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co5722iconvertible_uint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6026_compareto_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6026compareto_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6027_equals_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6027equals_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6028_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6028tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6029_format_u32_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6029format_u32_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6030_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6030tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6031_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6031gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6033_parse_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6033parse_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6034_parse_str_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6034parse_str_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6035_parse_str_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6035parse_str_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6036_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6036tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6037_tostring_uint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co6037tostring_uint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7044_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co7044gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7069_fromstring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co7069fromstring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8591_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co8591tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8592_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint32", "co8592parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@uint64
    {
        [TestClass]
        public class @uint64
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5721_iconvertible_uint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co5721iconvertible_uint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6040_compareto_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6040compareto_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6041_equals_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6041equals_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6042_tostring_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6042tostring_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6043_format_u64_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6043format_u64_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6044_tostring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6044tostring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6045_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6045gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6047_parse_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6047parse_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6048_parse_str_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6048parse_str_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6049_parse_str_int_nfi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6049parse_str_int_nfi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6050_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6050tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co6051_tostring_ulong()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co6051tostring_ulong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7043_gettypecode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co7043gettypecode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7071_fromstring_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co7071fromstring_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8593_tostring_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co8593tostring_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8594_parse_str_ifp()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uint64", "co8594parse_str_ifp.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@uintptr
    {
        [TestClass]
        public class @uintptr
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co8534_ctor_uint()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8534ctor_uint.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8535_ctor_ulong()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8535ctor_ulong.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8536_ctor_void()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8536ctor_void.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8537_equals_obj()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8537equals_obj.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8538_get_size()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8538get_size.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8539_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8539gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8540_operator_multi()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8540operator_multi.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8541_topointer()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8541topointer.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8542_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8542tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8543_touint32()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8543touint32.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8544_touint64()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"uintptr", "co8544touint64.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@valuetype
    {
        [TestClass]
        public class @valuetype
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co3873_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"valuetype", "co3873tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3874_equals()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"valuetype", "co3874equals.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co3875_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"valuetype", "co3875gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }

    }

    namespace Sscli.@version
    {
        [TestClass]
        public class @version
        {
            [TestInitialize]
            [Timeout(36000000)]
            public void Initialize()
            {
                CompilerHelper.AssertUiEnabled(false); CompilerHelper.DownloadTestsAndBuildCoreLib("sscli");
            }

            [TestCleanup]
            public void Cleanup()
            {
                CompilerHelper.AssertUiEnabled(true);
            }

            [TestMethod]
            public void @co5473_clone()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5473clone.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5474_get_build()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5474get_build.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5475_set_build()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5475set_build.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5476_get_major()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5476get_major.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5477_set_major()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5477set_major.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5478_get_minor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5478get_minor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5479_set_minor()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5479set_minor.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5480_get_revision()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5480get_revision.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co5481_set_revision()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co5481set_revision.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7037_ctor_iii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co7037ctor_iii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7038_ctor_ii()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co7038ctor_ii.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7039_ctor_str()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co7039ctor_str.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7040_compareto_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co7040compareto_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7041_equals_object()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co7041equals_object.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co7042_tostring()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co7042tostring.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8595_gethashcode()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co8595gethashcode.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

            [TestMethod]
            public void @co8596_tostring_int()
            {
                var file = Path.Combine(CompilerHelper.SscliSourcePath, @"version", "co8596tostring_int.cs");
                CompilerHelper.CompileAndRun(Path.GetFileNameWithoutExtension(file), Path.GetDirectoryName(file) + "\\", false, false);
            }

        }
    }
}


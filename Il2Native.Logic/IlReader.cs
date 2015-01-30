// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IlReader.cs" company="">
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class IlReader
    {
        /// <summary>
        /// </summary>
        private static readonly IDictionary<Code, OpCode> OpCodesMap = new SortedDictionary<Code, OpCode>();

        /// <summary>
        /// </summary>
        private static IDictionary<IType, IEnumerable<IMethod>> genMethodSpec;

        /// <summary>
        /// </summary>
        private readonly IDictionary<AssemblyIdentity, AssemblySymbol> cache = new Dictionary<AssemblyIdentity, AssemblySymbol>();

        private readonly bool isDll;

        /// <summary>
        /// </summary>
        private readonly Lazy<IEnumerable<IType>> lazyAllReferencedTypes;

        /// <summary>
        /// </summary>
        private readonly Lazy<IEnumerable<IType>> lazyAllTypes;

        /// <summary>
        /// </summary>
        private readonly Lazy<IEnumerable<IType>> lazyTypes;

        /// <summary>
        /// </summary>
        private readonly IList<UnifiedAssembly<AssemblySymbol>> unifiedAssemblies = new List<UnifiedAssembly<AssemblySymbol>>();

        /// <summary>
        /// </summary>
        private ISet<IMethod> calledMethods;

        /// <summary>
        /// </summary>
        private ISet<IMethod> usedGenericSpecialiazedMethods;

        /// <summary>
        /// </summary>
        private ISet<IType> usedGenericSpecialiazedTypes;

        /// <summary>
        /// </summary>
        private ISet<IField> usedStaticFieldsToRead;

        /// <summary>
        /// </summary>
        private ISet<IType> usedStructTypes;

        /// <summary>
        /// </summary>
        private ISet<IType> usedTypes;

        /// <summary>
        /// </summary>
        static IlReader()
        {
            OpCodesMap[Code.Nop] = OpCodesEmit.Nop;
            OpCodesMap[Code.Break] = OpCodesEmit.Break;
            OpCodesMap[Code.Ldarg_0] = OpCodesEmit.Ldarg_0;
            OpCodesMap[Code.Ldarg_1] = OpCodesEmit.Ldarg_1;
            OpCodesMap[Code.Ldarg_2] = OpCodesEmit.Ldarg_2;
            OpCodesMap[Code.Ldarg_3] = OpCodesEmit.Ldarg_3;
            OpCodesMap[Code.Ldloc_0] = OpCodesEmit.Ldloc_0;
            OpCodesMap[Code.Ldloc_1] = OpCodesEmit.Ldloc_1;
            OpCodesMap[Code.Ldloc_2] = OpCodesEmit.Ldloc_2;
            OpCodesMap[Code.Ldloc_3] = OpCodesEmit.Ldloc_3;
            OpCodesMap[Code.Stloc_0] = OpCodesEmit.Stloc_0;
            OpCodesMap[Code.Stloc_1] = OpCodesEmit.Stloc_1;
            OpCodesMap[Code.Stloc_2] = OpCodesEmit.Stloc_2;
            OpCodesMap[Code.Stloc_3] = OpCodesEmit.Stloc_3;
            OpCodesMap[Code.Ldarg_S] = OpCodesEmit.Ldarg_S;
            OpCodesMap[Code.Ldarga_S] = OpCodesEmit.Ldarga_S;
            OpCodesMap[Code.Starg_S] = OpCodesEmit.Starg_S;
            OpCodesMap[Code.Ldloc_S] = OpCodesEmit.Ldloc_S;
            OpCodesMap[Code.Ldloca_S] = OpCodesEmit.Ldloca_S;
            OpCodesMap[Code.Stloc_S] = OpCodesEmit.Stloc_S;
            OpCodesMap[Code.Ldnull] = OpCodesEmit.Ldnull;
            OpCodesMap[Code.Ldc_I4_M1] = OpCodesEmit.Ldc_I4_M1;
            OpCodesMap[Code.Ldc_I4_0] = OpCodesEmit.Ldc_I4_0;
            OpCodesMap[Code.Ldc_I4_1] = OpCodesEmit.Ldc_I4_1;
            OpCodesMap[Code.Ldc_I4_2] = OpCodesEmit.Ldc_I4_2;
            OpCodesMap[Code.Ldc_I4_3] = OpCodesEmit.Ldc_I4_3;
            OpCodesMap[Code.Ldc_I4_4] = OpCodesEmit.Ldc_I4_4;
            OpCodesMap[Code.Ldc_I4_5] = OpCodesEmit.Ldc_I4_5;
            OpCodesMap[Code.Ldc_I4_6] = OpCodesEmit.Ldc_I4_6;
            OpCodesMap[Code.Ldc_I4_7] = OpCodesEmit.Ldc_I4_7;
            OpCodesMap[Code.Ldc_I4_8] = OpCodesEmit.Ldc_I4_8;
            OpCodesMap[Code.Ldc_I4_S] = OpCodesEmit.Ldc_I4_S;
            OpCodesMap[Code.Ldc_I4] = OpCodesEmit.Ldc_I4;
            OpCodesMap[Code.Ldc_I8] = OpCodesEmit.Ldc_I8;
            OpCodesMap[Code.Ldc_R4] = OpCodesEmit.Ldc_R4;
            OpCodesMap[Code.Ldc_R8] = OpCodesEmit.Ldc_R8;
            OpCodesMap[Code.Dup] = OpCodesEmit.Dup;
            OpCodesMap[Code.Pop] = OpCodesEmit.Pop;
            OpCodesMap[Code.Jmp] = OpCodesEmit.Jmp;
            OpCodesMap[Code.Call] = OpCodesEmit.Call;
            OpCodesMap[Code.Calli] = OpCodesEmit.Calli;
            OpCodesMap[Code.Ret] = OpCodesEmit.Ret;
            OpCodesMap[Code.Br_S] = OpCodesEmit.Br_S;
            OpCodesMap[Code.Brfalse_S] = OpCodesEmit.Brfalse_S;
            OpCodesMap[Code.Brtrue_S] = OpCodesEmit.Brtrue_S;
            OpCodesMap[Code.Beq_S] = OpCodesEmit.Beq_S;
            OpCodesMap[Code.Bge_S] = OpCodesEmit.Bge_S;
            OpCodesMap[Code.Bgt_S] = OpCodesEmit.Bgt_S;
            OpCodesMap[Code.Ble_S] = OpCodesEmit.Ble_S;
            OpCodesMap[Code.Blt_S] = OpCodesEmit.Blt_S;
            OpCodesMap[Code.Bne_Un_S] = OpCodesEmit.Bne_Un_S;
            OpCodesMap[Code.Bge_Un_S] = OpCodesEmit.Bge_Un_S;
            OpCodesMap[Code.Bgt_Un_S] = OpCodesEmit.Bgt_Un_S;
            OpCodesMap[Code.Ble_Un_S] = OpCodesEmit.Ble_Un_S;
            OpCodesMap[Code.Blt_Un_S] = OpCodesEmit.Blt_Un_S;
            OpCodesMap[Code.Br] = OpCodesEmit.Br;
            OpCodesMap[Code.Brfalse] = OpCodesEmit.Brfalse;
            OpCodesMap[Code.Brtrue] = OpCodesEmit.Brtrue;
            OpCodesMap[Code.Beq] = OpCodesEmit.Beq;
            OpCodesMap[Code.Bge] = OpCodesEmit.Bge;
            OpCodesMap[Code.Bgt] = OpCodesEmit.Bgt;
            OpCodesMap[Code.Ble] = OpCodesEmit.Ble;
            OpCodesMap[Code.Blt] = OpCodesEmit.Blt;
            OpCodesMap[Code.Bne_Un] = OpCodesEmit.Bne_Un;
            OpCodesMap[Code.Bge_Un] = OpCodesEmit.Bge_Un;
            OpCodesMap[Code.Bgt_Un] = OpCodesEmit.Bgt_Un;
            OpCodesMap[Code.Ble_Un] = OpCodesEmit.Ble_Un;
            OpCodesMap[Code.Blt_Un] = OpCodesEmit.Blt_Un;
            OpCodesMap[Code.Switch] = OpCodesEmit.Switch;
            OpCodesMap[Code.Ldind_I1] = OpCodesEmit.Ldind_I1;
            OpCodesMap[Code.Ldind_U1] = OpCodesEmit.Ldind_U1;
            OpCodesMap[Code.Ldind_I2] = OpCodesEmit.Ldind_I2;
            OpCodesMap[Code.Ldind_U2] = OpCodesEmit.Ldind_U2;
            OpCodesMap[Code.Ldind_I4] = OpCodesEmit.Ldind_I4;
            OpCodesMap[Code.Ldind_U4] = OpCodesEmit.Ldind_U4;
            OpCodesMap[Code.Ldind_I8] = OpCodesEmit.Ldind_I8;
            OpCodesMap[Code.Ldind_I] = OpCodesEmit.Ldind_I;
            OpCodesMap[Code.Ldind_R4] = OpCodesEmit.Ldind_R4;
            OpCodesMap[Code.Ldind_R8] = OpCodesEmit.Ldind_R8;
            OpCodesMap[Code.Ldind_Ref] = OpCodesEmit.Ldind_Ref;
            OpCodesMap[Code.Stind_Ref] = OpCodesEmit.Stind_Ref;
            OpCodesMap[Code.Stind_I1] = OpCodesEmit.Stind_I1;
            OpCodesMap[Code.Stind_I2] = OpCodesEmit.Stind_I2;
            OpCodesMap[Code.Stind_I4] = OpCodesEmit.Stind_I4;
            OpCodesMap[Code.Stind_I8] = OpCodesEmit.Stind_I8;
            OpCodesMap[Code.Stind_R4] = OpCodesEmit.Stind_R4;
            OpCodesMap[Code.Stind_R8] = OpCodesEmit.Stind_R8;
            OpCodesMap[Code.Add] = OpCodesEmit.Add;
            OpCodesMap[Code.Sub] = OpCodesEmit.Sub;
            OpCodesMap[Code.Mul] = OpCodesEmit.Mul;
            OpCodesMap[Code.Div] = OpCodesEmit.Div;
            OpCodesMap[Code.Div_Un] = OpCodesEmit.Div_Un;
            OpCodesMap[Code.Rem] = OpCodesEmit.Rem;
            OpCodesMap[Code.Rem_Un] = OpCodesEmit.Rem_Un;
            OpCodesMap[Code.And] = OpCodesEmit.And;
            OpCodesMap[Code.Or] = OpCodesEmit.Or;
            OpCodesMap[Code.Xor] = OpCodesEmit.Xor;
            OpCodesMap[Code.Shl] = OpCodesEmit.Shl;
            OpCodesMap[Code.Shr] = OpCodesEmit.Shr;
            OpCodesMap[Code.Shr_Un] = OpCodesEmit.Shr_Un;
            OpCodesMap[Code.Neg] = OpCodesEmit.Neg;
            OpCodesMap[Code.Not] = OpCodesEmit.Not;
            OpCodesMap[Code.Conv_I1] = OpCodesEmit.Conv_I1;
            OpCodesMap[Code.Conv_I2] = OpCodesEmit.Conv_I2;
            OpCodesMap[Code.Conv_I4] = OpCodesEmit.Conv_I4;
            OpCodesMap[Code.Conv_I8] = OpCodesEmit.Conv_I8;
            OpCodesMap[Code.Conv_R4] = OpCodesEmit.Conv_R4;
            OpCodesMap[Code.Conv_R8] = OpCodesEmit.Conv_R8;
            OpCodesMap[Code.Conv_U4] = OpCodesEmit.Conv_U4;
            OpCodesMap[Code.Conv_U8] = OpCodesEmit.Conv_U8;
            OpCodesMap[Code.Callvirt] = OpCodesEmit.Callvirt;
            OpCodesMap[Code.Cpobj] = OpCodesEmit.Cpobj;
            OpCodesMap[Code.Ldobj] = OpCodesEmit.Ldobj;
            OpCodesMap[Code.Ldstr] = OpCodesEmit.Ldstr;
            OpCodesMap[Code.Newobj] = OpCodesEmit.Newobj;
            OpCodesMap[Code.Castclass] = OpCodesEmit.Castclass;
            OpCodesMap[Code.Isinst] = OpCodesEmit.Isinst;
            OpCodesMap[Code.Conv_R_Un] = OpCodesEmit.Conv_R_Un;
            OpCodesMap[Code.Unbox] = OpCodesEmit.Unbox;
            OpCodesMap[Code.Throw] = OpCodesEmit.Throw;
            OpCodesMap[Code.Ldfld] = OpCodesEmit.Ldfld;
            OpCodesMap[Code.Ldflda] = OpCodesEmit.Ldflda;
            OpCodesMap[Code.Stfld] = OpCodesEmit.Stfld;
            OpCodesMap[Code.Ldsfld] = OpCodesEmit.Ldsfld;
            OpCodesMap[Code.Ldsflda] = OpCodesEmit.Ldsflda;
            OpCodesMap[Code.Stsfld] = OpCodesEmit.Stsfld;
            OpCodesMap[Code.Stobj] = OpCodesEmit.Stobj;
            OpCodesMap[Code.Conv_Ovf_I1_Un] = OpCodesEmit.Conv_Ovf_I1_Un;
            OpCodesMap[Code.Conv_Ovf_I2_Un] = OpCodesEmit.Conv_Ovf_I2_Un;
            OpCodesMap[Code.Conv_Ovf_I4_Un] = OpCodesEmit.Conv_Ovf_I4_Un;
            OpCodesMap[Code.Conv_Ovf_I8_Un] = OpCodesEmit.Conv_Ovf_I8_Un;
            OpCodesMap[Code.Conv_Ovf_U1_Un] = OpCodesEmit.Conv_Ovf_U1_Un;
            OpCodesMap[Code.Conv_Ovf_U2_Un] = OpCodesEmit.Conv_Ovf_U2_Un;
            OpCodesMap[Code.Conv_Ovf_U4_Un] = OpCodesEmit.Conv_Ovf_U4_Un;
            OpCodesMap[Code.Conv_Ovf_U8_Un] = OpCodesEmit.Conv_Ovf_U8_Un;
            OpCodesMap[Code.Conv_Ovf_I_Un] = OpCodesEmit.Conv_Ovf_I_Un;
            OpCodesMap[Code.Conv_Ovf_U_Un] = OpCodesEmit.Conv_Ovf_U_Un;
            OpCodesMap[Code.Box] = OpCodesEmit.Box;
            OpCodesMap[Code.Newarr] = OpCodesEmit.Newarr;
            OpCodesMap[Code.Ldlen] = OpCodesEmit.Ldlen;
            OpCodesMap[Code.Ldelema] = OpCodesEmit.Ldelema;
            OpCodesMap[Code.Ldelem_I1] = OpCodesEmit.Ldelem_I1;
            OpCodesMap[Code.Ldelem_U1] = OpCodesEmit.Ldelem_U1;
            OpCodesMap[Code.Ldelem_I2] = OpCodesEmit.Ldelem_I2;
            OpCodesMap[Code.Ldelem_U2] = OpCodesEmit.Ldelem_U2;
            OpCodesMap[Code.Ldelem_I4] = OpCodesEmit.Ldelem_I4;
            OpCodesMap[Code.Ldelem_U4] = OpCodesEmit.Ldelem_U4;
            OpCodesMap[Code.Ldelem_I8] = OpCodesEmit.Ldelem_I8;
            OpCodesMap[Code.Ldelem_I] = OpCodesEmit.Ldelem_I;
            OpCodesMap[Code.Ldelem_R4] = OpCodesEmit.Ldelem_R4;
            OpCodesMap[Code.Ldelem_R8] = OpCodesEmit.Ldelem_R8;
            OpCodesMap[Code.Ldelem_Ref] = OpCodesEmit.Ldelem_Ref;
            OpCodesMap[Code.Stelem_I] = OpCodesEmit.Stelem_I;
            OpCodesMap[Code.Stelem_I1] = OpCodesEmit.Stelem_I1;
            OpCodesMap[Code.Stelem_I2] = OpCodesEmit.Stelem_I2;
            OpCodesMap[Code.Stelem_I4] = OpCodesEmit.Stelem_I4;
            OpCodesMap[Code.Stelem_I8] = OpCodesEmit.Stelem_I8;
            OpCodesMap[Code.Stelem_R4] = OpCodesEmit.Stelem_R4;
            OpCodesMap[Code.Stelem_R8] = OpCodesEmit.Stelem_R8;
            OpCodesMap[Code.Stelem_Ref] = OpCodesEmit.Stelem_Ref;
            OpCodesMap[Code.Ldelem] = OpCodesEmit.Ldelem;
            OpCodesMap[Code.Stelem] = OpCodesEmit.Stelem;
            OpCodesMap[Code.Unbox_Any] = OpCodesEmit.Unbox_Any;
            OpCodesMap[Code.Conv_Ovf_I1] = OpCodesEmit.Conv_Ovf_I1;
            OpCodesMap[Code.Conv_Ovf_U1] = OpCodesEmit.Conv_Ovf_U1;
            OpCodesMap[Code.Conv_Ovf_I2] = OpCodesEmit.Conv_Ovf_I2;
            OpCodesMap[Code.Conv_Ovf_U2] = OpCodesEmit.Conv_Ovf_U2;
            OpCodesMap[Code.Conv_Ovf_I4] = OpCodesEmit.Conv_Ovf_I4;
            OpCodesMap[Code.Conv_Ovf_U4] = OpCodesEmit.Conv_Ovf_U4;
            OpCodesMap[Code.Conv_Ovf_I8] = OpCodesEmit.Conv_Ovf_I8;
            OpCodesMap[Code.Conv_Ovf_U8] = OpCodesEmit.Conv_Ovf_U8;
            OpCodesMap[Code.Refanyval] = OpCodesEmit.Refanyval;
            OpCodesMap[Code.Ckfinite] = OpCodesEmit.Ckfinite;
            OpCodesMap[Code.Mkrefany] = OpCodesEmit.Mkrefany;
            OpCodesMap[Code.Ldtoken] = OpCodesEmit.Ldtoken;
            OpCodesMap[Code.Conv_U2] = OpCodesEmit.Conv_U2;
            OpCodesMap[Code.Conv_U1] = OpCodesEmit.Conv_U1;
            OpCodesMap[Code.Conv_I] = OpCodesEmit.Conv_I;
            OpCodesMap[Code.Conv_Ovf_I] = OpCodesEmit.Conv_Ovf_I;
            OpCodesMap[Code.Conv_Ovf_U] = OpCodesEmit.Conv_Ovf_U;
            OpCodesMap[Code.Add_Ovf] = OpCodesEmit.Add_Ovf;
            OpCodesMap[Code.Add_Ovf_Un] = OpCodesEmit.Add_Ovf_Un;
            OpCodesMap[Code.Mul_Ovf] = OpCodesEmit.Mul_Ovf;
            OpCodesMap[Code.Mul_Ovf_Un] = OpCodesEmit.Mul_Ovf_Un;
            OpCodesMap[Code.Sub_Ovf] = OpCodesEmit.Sub_Ovf;
            OpCodesMap[Code.Sub_Ovf_Un] = OpCodesEmit.Sub_Ovf_Un;
            OpCodesMap[Code.Endfinally] = OpCodesEmit.Endfinally;
            OpCodesMap[Code.Leave] = OpCodesEmit.Leave;
            OpCodesMap[Code.Leave_S] = OpCodesEmit.Leave_S;
            OpCodesMap[Code.Stind_I] = OpCodesEmit.Stind_I;
            OpCodesMap[Code.Conv_U] = OpCodesEmit.Conv_U;
            OpCodesMap[Code.Arglist] = OpCodesEmit.Arglist;
            OpCodesMap[Code.Ceq] = OpCodesEmit.Ceq;
            OpCodesMap[Code.Cgt] = OpCodesEmit.Cgt;
            OpCodesMap[Code.Cgt_Un] = OpCodesEmit.Cgt_Un;
            OpCodesMap[Code.Clt] = OpCodesEmit.Clt;
            OpCodesMap[Code.Clt_Un] = OpCodesEmit.Clt_Un;
            OpCodesMap[Code.Ldftn] = OpCodesEmit.Ldftn;
            OpCodesMap[Code.Ldvirtftn] = OpCodesEmit.Ldvirtftn;
            OpCodesMap[Code.Ldarg] = OpCodesEmit.Ldarg;
            OpCodesMap[Code.Ldarga] = OpCodesEmit.Ldarga;
            OpCodesMap[Code.Starg] = OpCodesEmit.Starg;
            OpCodesMap[Code.Ldloc] = OpCodesEmit.Ldloc;
            OpCodesMap[Code.Ldloca] = OpCodesEmit.Ldloca;
            OpCodesMap[Code.Stloc] = OpCodesEmit.Stloc;
            OpCodesMap[Code.Localloc] = OpCodesEmit.Localloc;
            OpCodesMap[Code.Endfilter] = OpCodesEmit.Endfilter;
            OpCodesMap[Code.Unaligned] = OpCodesEmit.Unaligned;
            OpCodesMap[Code.Volatile] = OpCodesEmit.Volatile;
            OpCodesMap[Code.Tail] = OpCodesEmit.Tailcall;
            OpCodesMap[Code.Initobj] = OpCodesEmit.Initobj;
            OpCodesMap[Code.Constrained] = OpCodesEmit.Constrained;
            OpCodesMap[Code.Cpblk] = OpCodesEmit.Cpblk;
            OpCodesMap[Code.Initblk] = OpCodesEmit.Initblk;

            // OpCodesMap[Code.No] = OpCodesEmit.No;
            OpCodesMap[Code.Rethrow] = OpCodesEmit.Rethrow;
            OpCodesMap[Code.Sizeof] = OpCodesEmit.Sizeof;
            OpCodesMap[Code.Refanytype] = OpCodesEmit.Refanytype;
            OpCodesMap[Code.Readonly] = OpCodesEmit.Readonly;
        }

        /// <summary>
        /// </summary>
        public IlReader()
        {
            this.lazyTypes = new Lazy<IEnumerable<IType>>(() => this.ReadTypes());
            this.lazyAllTypes = new Lazy<IEnumerable<IType>>(() => this.ReadTypes(true));
            this.lazyAllReferencedTypes = new Lazy<IEnumerable<IType>>(() => this.ReadTypes(true, true));
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="args">
        /// </param>
        public IlReader(string[] source, string[] args)
            : this()
        {
            this.Sources = source;
            this.FirstSource = this.Sources.First();

            var coreLibPathArg = args != null ? args.FirstOrDefault(a => a.StartsWith("corelib:")) : null;
            this.CoreLibPath = coreLibPathArg != null ? coreLibPathArg.Substring("corelib:".Length) : null;
            this.UsingRoslyn = args != null && args.Any(a => a == "roslyn");
            this.isDll = this.FirstSource.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase);
            this.DefaultDllLocations = this.isDll ? Path.GetDirectoryName(Path.GetFullPath(this.FirstSource)) : null;
            this.DebugInfo = args != null && args.Contains("debug");
            if (!this.isDll)
            {
                this.SourceFilePath = Path.GetFullPath(this.FirstSource);
            }
        }

        /// <summary>
        /// </summary>
        public static IDictionary<IType, IEnumerable<IMethod>> GenericMethodSpecializations
        {
            set
            {
                genMethodSpec = value;
            }
        }

        /// <summary>
        /// </summary>
        public string AssemblyQualifiedName
        {
            get
            {
                return this.Assembly.Assembly.Identity.Name;
            }
        }

        /// <summary>
        /// </summary>
        public ISet<IMethod> CalledMethods
        {
            get
            {
                return this.calledMethods;
            }

            set
            {
                this.calledMethods = value;
            }
        }

        /// <summary>
        /// </summary>
        public string CoreLibPath { get; set; }

        public bool DebugInfo { get; private set; }

        /// <summary>
        /// </summary>
        public string DefaultDllLocations { get; private set; }

        public string DllFilePath { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsCoreLib
        {
            get
            {
                return !this.Assembly.Assembly.AssemblyReferences.Any();
            }
        }

        /// <summary>
        /// </summary>
        public string ModuleName
        {
            get
            {
                return this.Assembly.ManifestModule.Name;
            }
        }

        public string PdbFilePath { get; private set; }

        public string SourceFilePath { get; private set; }

        /// <summary>
        /// </summary>
        public ISet<IMethod> UsedGenericSpecialiazedMethods
        {
            get
            {
                return this.usedGenericSpecialiazedMethods;
            }

            set
            {
                this.usedGenericSpecialiazedMethods = value;
            }
        }

        /// <summary>
        /// </summary>
        public ISet<IType> UsedGenericSpecialiazedTypes
        {
            get
            {
                return this.usedGenericSpecialiazedTypes;
            }

            set
            {
                this.usedGenericSpecialiazedTypes = value;
            }
        }

        /// <summary>
        /// </summary>
        public ISet<IField> UsedStaticFieldsToRead
        {
            get
            {
                return this.usedStaticFieldsToRead;
            }

            set
            {
                this.usedStaticFieldsToRead = value;
            }
        }

        /// <summary>
        /// </summary>
        public ISet<IType> UsedStructTypes
        {
            get
            {
                return this.usedStructTypes;
            }

            set
            {
                this.usedStructTypes = value;
            }
        }

        /// <summary>
        /// </summary>
        public ISet<IType> UsedTypes
        {
            get
            {
                return this.usedTypes;
            }

            set
            {
                this.usedTypes = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool UsingRoslyn { get; set; }

        /// <summary>
        /// </summary>
        protected AssemblyMetadata Assembly { get; private set; }

        protected string FirstSource { get; private set; }

        /// <summary>
        /// </summary>
        protected string[] Sources { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IConstructor> Constructors(IType type)
        {
            return type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IField> Fields(IType type)
        {
            return type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IMethod> Methods(IType type, ITypeResolver typeResolver)
        {
            return Methods(
                type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, typeResolver);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="flags">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IMethod> Methods(IType type, BindingFlags flags, ITypeResolver typeResolver)
        {
            foreach (var method in type.GetMethods(flags).Where(m => !m.IsGenericMethodDefinition))
            {
                yield return method;
            }

            // append C# native compiler infrastructure methods
            yield return new SynthesizedGetSizeMethod(type, typeResolver);

            // append internal methods
            yield return new SynthesizedGetTypeMethod(type, typeResolver);

            if (type.ToNormal().IsEnum)
            {
                yield return new SynthesizedGetHashCodeMethod(type, typeResolver);
            }

            // append specialized methods
            IEnumerable<IMethod> genMethodSpecializationForType = null;
            if (!genMethodSpec.TryGetValue(type, out genMethodSpecializationForType))
            {
                yield break;
            }

            // return Generic Method Specializations for a type
            foreach (var method in
                genMethodSpecializationForType)
            {
                yield return method;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IMethod> MethodsOriginal(IType type)
        {
            return type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> AllReferencedTypes()
        {
            return this.lazyAllReferencedTypes.Value;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<string> AllReferences()
        {
            return this.AllReferencesHelper(this.Assembly);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> AllTypes()
        {
            return this.lazyAllTypes.Value;
        }

        public IEnumerable<IType> CompileSourceWithRoslyn(params string[] source)
        {
            var baseName = Path.GetRandomFileName();
            var nameDll = baseName + ".dll";

            var syntaxTrees =
                source.Select(s => CSharpSyntaxTree.ParseText(new StreamReader(s).ReadToEnd(), new CSharpParseOptions(LanguageVersion.Experimental)));

            var coreLibRefAssembly = string.IsNullOrWhiteSpace(this.CoreLibPath)
                                         ? new MetadataImageReference(new FileStream(typeof(int).Assembly.Location, FileMode.Open, FileAccess.Read))
                                         : new MetadataImageReference(new FileStream(this.CoreLibPath, FileMode.Open, FileAccess.Read));

            var options =
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithAllowUnsafe(true)
                                                                                 .WithOptimizations(!this.DebugInfo)
                                                                                 .WithRuntimeMetadataVersion("4.5");

            var compilation = CSharpCompilation.Create(nameDll, syntaxTrees, new[] { coreLibRefAssembly }, options);

            using (var dllStream = new MemoryStream())
            {
                var result = compilation.Emit(peStream: dllStream);
                if (result.Diagnostics.Length > 0)
                {
                    foreach (var diagnostic in result.Diagnostics.Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error))
                    {
                        throw new InvalidOperationException(diagnostic.GetMessage());
                    }
                }

                // Successful Compile
                var fromImageStream = AssemblyMetadata.CreateFromImageStream(dllStream);
                var compileSourceWithRoslyn = this.LoadAssemblySymbol(fromImageStream);
                return this.ReadTypes(compileSourceWithRoslyn);
            }
        }

        /// <summary>
        /// </summary>
        public void Load()
        {
            this.Assembly = !this.isDll
                                ? this.UsingRoslyn ? this.CompileWithRoslyn(this.Sources) : this.Compile(this.Sources)
                                : AssemblyMetadata.CreateFromImageStream(new FileStream(this.FirstSource, FileMode.Open, FileAccess.Read));

            if (this.isDll)
            {
                this.DllFilePath = this.FirstSource;
                this.PdbFilePath = Path.ChangeExtension(this.FirstSource, "pdb");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <param name="stackCall">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IConstructor ctor, IGenericContext genericContext, Queue<IMethod> stackCall = null)
        {
            if (ctor == null)
            {
                yield break;
            }

            foreach (var opCode in this.OpCodes(ctor.GetMethodBody(), ctor.Module, genericContext, stackCall))
            {
                yield return opCode;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <param name="stackCall">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IMethod method, IGenericContext genericContext, Queue<IMethod> stackCall = null)
        {
            if (method == null)
            {
                yield break;
            }

            foreach (var opCode in this.OpCodes(method.GetMethodBody(), method.Module, genericContext, stackCall))
            {
                yield return opCode;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="methodBody">
        /// </param>
        /// <param name="module">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <param name="stackCall">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IMethodBody methodBody, IModule module, IGenericContext genericContext, Queue<IMethod> stackCall)
        {
            if (!methodBody.HasBody)
            {
                yield break;
            }

            var extended = false;
            var startAddress = 0;
            var currentAddress = 0;
            var ilAsByteArray = methodBody.GetILAsByteArray();
            var enumerator = ilAsByteArray.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var @byte = (byte)enumerator.Current;
                if (@byte == 0xFE)
                {
                    extended = true;
                    continue;
                }

                var code = (Code)(extended ? (@byte + 0xE1) : @byte);
                extended = false;

                var opCode = OpCodesMap[code];

                startAddress = currentAddress;
                currentAddress += opCode.Size;

                switch (code)
                {
                    case Code.Br_S:
                    case Code.Beq_S:
                    case Code.Brtrue_S:
                    case Code.Brfalse_S:
                    case Code.Blt_S:
                    case Code.Blt_Un_S:
                    case Code.Bgt_S:
                    case Code.Bgt_Un_S:
                    case Code.Bge_S:
                    case Code.Bge_Un_S:
                    case Code.Ble_S:
                    case Code.Ble_Un_S:
                    case Code.Bne_Un_S:
                    case Code.Ldc_I4_S:
                    case Code.Ldloc_S:
                    case Code.Ldloca_S:
                    case Code.Stloc_S:
                    case Code.Leave_S:
                    case Code.Ldarg_S:
                    case Code.Starg_S:
                    case Code.Ldarga_S:

                        // read token, next 
                        var token = ReadInt32ShortForm(enumerator, ref currentAddress);
                        var @int32 = token;
                        yield return new OpCodeInt32Part(opCode, startAddress, currentAddress, @int32);
                        continue;
                    case Code.Br:
                    case Code.Beq:
                    case Code.Brtrue:
                    case Code.Brfalse:
                    case Code.Blt:
                    case Code.Blt_Un:
                    case Code.Bgt:
                    case Code.Bgt_Un:
                    case Code.Bge:
                    case Code.Bge_Un:
                    case Code.Ble:
                    case Code.Ble_Un:
                    case Code.Bne_Un:
                    case Code.Ldc_I4:
                    case Code.Ldloc:
                    case Code.Stloc:
                    case Code.Leave:
                    case Code.Starg:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        @int32 = token;
                        yield return new OpCodeInt32Part(opCode, startAddress, currentAddress, @int32);
                        continue;
                    case Code.Ldc_I8:

                        // read token, next 
                        var bytes = ReadBytes(enumerator, 8, ref currentAddress);
                        var @int64 = BitConverter.ToInt64(bytes, 0);
                        yield return new OpCodeInt64Part(opCode, startAddress, currentAddress, @int64);
                        continue;
                    case Code.Ldc_R4:

                        // read token, next 
                        bytes = ReadBytes(enumerator, 4, ref currentAddress);
                        var @single = BitConverter.ToSingle(bytes, 0);
                        yield return new OpCodeSinglePart(opCode, startAddress, currentAddress, @single);
                        continue;
                    case Code.Ldc_R8:

                        // read token, next 
                        bytes = ReadBytes(enumerator, 8, ref currentAddress);
                        var @double = BitConverter.ToDouble(bytes, 0);
                        yield return new OpCodeDoublePart(opCode, startAddress, currentAddress, @double);
                        continue;
                    case Code.Ldstr:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var @string = module.ResolveString(token);
                        yield return new OpCodeStringPart(opCode, startAddress, currentAddress, @string);
                        continue;
                    case Code.Newobj:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var constructor = module.ResolveMember(token, genericContext) as IConstructor;
                        this.AddGenericSpecializedType(constructor.DeclaringType);
                        this.AddGenericSpecializedMethod(constructor, stackCall);
                        foreach (var methodParameter in constructor.GetParameters())
                        {
                            this.AddStructType(methodParameter.ParameterType);
                        }

                        this.AddUsedType(constructor.DeclaringType);
                        this.AddCalledMethod(constructor);

                        yield return new OpCodeConstructorInfoPart(opCode, startAddress, currentAddress, constructor);
                        continue;
                    case Code.Call:
                    case Code.Callvirt:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var method = module.ResolveMethod(token, genericContext);
                        this.AddGenericSpecializedType(method.DeclaringType);
                        this.AddGenericSpecializedMethod(method, stackCall);

                        this.AddStructType(method.ReturnType);
                        foreach (var methodParameter in method.GetParameters())
                        {
                            this.AddStructType(methodParameter.ParameterType);
                        }

                        this.AddUsedType(method.DeclaringType);
                        this.AddCalledMethod(method);

                        yield return new OpCodeMethodInfoPart(opCode, startAddress, currentAddress, method);
                        continue;

                    case Code.Ldftn:
                    case Code.Ldvirtftn:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        method = module.ResolveMethod(token, genericContext);
                        this.AddGenericSpecializedType(method.DeclaringType);
                        this.AddGenericSpecializedMethod(method, stackCall);

                        this.AddUsedType(method.DeclaringType);

                        yield return new OpCodeMethodInfoPart(opCode, startAddress, currentAddress, method);
                        continue;
                    case Code.Stfld:
                    case Code.Stsfld:
                    case Code.Ldfld:
                    case Code.Ldflda:
                    case Code.Ldsfld:
                    case Code.Ldsflda:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var field = module.ResolveField(token, genericContext);

                        this.AddGenericSpecializedType(field.FieldType);
                        this.AddGenericSpecializedType(field.DeclaringType);
                        this.AddUsedType(field.DeclaringType);

                        if (code == Code.Ldsfld || code == Code.Ldsflda)
                        {
                            this.AddUsedStaticFieldToRead(field);
                        }

                        yield return new OpCodeFieldInfoPart(opCode, startAddress, currentAddress, field);
                        continue;
                    case Code.Ldtoken: // can it be anything?

                        token = ReadInt32(enumerator, ref currentAddress);

                        var resolvedToken = module.ResolveToken(token, genericContext);

                        var typeToken = resolvedToken as IType;
                        if (typeToken != null)
                        {
                            this.AddUsedType(typeToken);

                            yield return new OpCodeTypePart(opCode, startAddress, currentAddress, typeToken);
                            continue;
                        }

                        var fieldMember = resolvedToken as IField;
                        if (fieldMember != null)
                        {
                            this.AddUsedType(fieldMember.DeclaringType);

                            yield return new OpCodeFieldInfoPart(opCode, startAddress, currentAddress, fieldMember);
                            continue;
                        }

                        var methodMember = resolvedToken as IMethod;
                        if (methodMember != null)
                        {
                            this.AddUsedType(methodMember.DeclaringType);

                            yield return new OpCodeMethodInfoPart(opCode, startAddress, currentAddress, methodMember);
                            continue;
                        }

                        yield return new OpCodeInt32Part(opCode, startAddress, currentAddress, token);
                        continue;
                    case Code.Newarr:
                    case Code.Ldelem:
                    case Code.Stelem:
                    case Code.Ldelema:
                    case Code.Box:
                    case Code.Unbox:
                    case Code.Unbox_Any:
                    case Code.Castclass:
                    case Code.Initobj:
                    case Code.Isinst:
                    case Code.Ldobj:
                    case Code.Stobj:
                    case Code.Constrained:
                    case Code.Sizeof:
                    case Code.Mkrefany:
                    case Code.Refanyval:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var type = module.ResolveType(token, genericContext);
                        this.AddGenericSpecializedType(type);
                        if (code == Code.Box)
                        {
                            this.AddStructType(type);
                        }

                        this.AddUsedType(type);

                        yield return new OpCodeTypePart(opCode, startAddress, currentAddress, type);
                        continue;
                    case Code.Switch:

                        var ints = new List<int>();
                        var count = ReadInt32(enumerator, ref currentAddress);
                        for (var i = 0; i < count; i++)
                        {
                            ints.Add(ReadInt32(enumerator, ref currentAddress));
                        }

                        yield return new OpCodeLabelsPart(opCode, startAddress, currentAddress, ints.ToArray());
                        continue;

                    default:
                        yield return new OpCodePart(opCode, startAddress, currentAddress);
                        continue;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> Types()
        {
            return this.lazyTypes.Value;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private static IEnumerable<IType> EnumAllNestedTypes(IType type)
        {
            foreach (var nestedType in type.GetNestedTypes())
            {
                yield return nestedType;
                foreach (var subNestedType in EnumAllNestedTypes(nestedType))
                {
                    yield return subNestedType;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        private static IEnumerable<NamespaceSymbol> GetAllNamespaces(NamespaceSymbol source)
        {
            yield return source;
            foreach (var namespaceSymbolSub in
                source.GetNamespaceMembers().SelectMany(namespaceSymbolSub => GetAllNamespaces(namespaceSymbolSub)))
            {
                yield return namespaceSymbolSub;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyMetadata">
        /// </param>
        /// <returns>
        /// </returns>
        private static PEAssemblySymbol GetAssemblySymbol(AssemblyMetadata assemblyMetadata)
        {
            return new PEAssemblySymbol(assemblyMetadata.Assembly, DocumentationProvider.Default, isLinked: false, importOptions: MetadataImportOptions.All);
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="size">
        /// </param>
        /// <param name="shift">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static byte[] ReadBytes(IEnumerator source, int size, ref int shift)
        {
            var b = new byte[size];
            for (var i = 0; i < size; i++)
            {
                if (source.MoveNext())
                {
                    b[i] = (byte)source.Current;
                }
                else
                {
                    throw new InvalidOperationException("Could not read bytes");
                }
            }

            shift += size;

            return b;
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="shift">
        /// </param>
        /// <returns>
        /// </returns>
        private static int ReadInt32(IEnumerator source, ref int shift)
        {
            return BitConverter.ToInt32(ReadBytes(source, 4, ref shift), 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="shift">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        private static int ReadInt32ShortForm(IEnumerator source, ref int shift)
        {
            if (source.MoveNext())
            {
                shift++;
                return (byte)source.Current;
            }

            throw new InvalidOperationException("Could not read a short for of int32");
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        private void AddCalledMethod(IMethod method)
        {
            if (this.calledMethods == null || method == null)
            {
                return;
            }

            this.calledMethods.Add(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="stackCall">
        /// </param>
        private void AddGenericSpecializedMethod(IMethod method, Queue<IMethod> stackCall)
        {
            if (this.usedGenericSpecialiazedMethods == null || method == null)
            {
                return;
            }

            if (!method.IsGenericMethod)
            {
                if (method.DeclaringType.IsGenericType && !stackCall.Contains(method))
                {
                    this.DiscoverRequiredTypesAndMethodsInMethod(method, stackCall);
                }

                return;
            }

            if (method.IsGenericMethodDefinition || method.DeclaringType.IsGenericTypeDefinition || this.usedGenericSpecialiazedMethods.Contains(method))
            {
                return;
            }

            Debug.Assert(!method.IsGenericMethodDefinition, "Generic Method Definition can't be used here");
            Debug.Assert(!method.DeclaringType.IsGenericTypeDefinition, "Generic Type Definition can't be used here");

            this.usedGenericSpecialiazedMethods.Add(method);

            this.DiscoverRequiredTypesAndMethodsInMethod(method, stackCall);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void AddGenericSpecializedType(IType type)
        {
            if (this.usedGenericSpecialiazedTypes == null || type == null)
            {
                return;
            }

            if (type.IsArray || type.IsPointer || type.IsGenericTypeDefinition || !type.IsGenericType)
            {
                return;
            }

            this.usedGenericSpecialiazedTypes.Add(type.ToBareType().ToNormal());
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void AddStructType(IType type)
        {
            if (this.usedStructTypes == null || type == null || !type.IsStructureType())
            {
                return;
            }

            this.usedStructTypes.Add(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        private void AddUsedStaticFieldToRead(IField field)
        {
            if (this.usedStaticFieldsToRead == null || field == null || !field.IsStatic)
            {
                return;
            }

            this.usedStaticFieldsToRead.Add(field);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void AddUsedType(IType type)
        {
            if (this.usedTypes == null || type == null)
            {
                return;
            }

            this.usedTypes.Add(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyMetadata">
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<string> AllReferencesHelper(AssemblyMetadata assemblyMetadata)
        {
            yield return assemblyMetadata.Assembly.Identity.Name;

            foreach (var reference in this.LoadReferences(assemblyMetadata).Names)
            {
                foreach (var referenceName in this.AllReferencesHelper(this.GetAssemblyMetadata(reference)))
                {
                    yield return referenceName;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sources">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private AssemblyMetadata Compile(string[] sources)
        {
            var language = "CSharp";
            var codeProvider = CodeDomProvider.IsDefinedLanguage(language) ? CodeDomProvider.CreateProvider(language) : null;
            if (codeProvider == null)
            {
                throw new NotSupportedException(string.Format("language '{0}' is not supported", language));
            }

            var path = Path.GetTempPath();
            var filename = Path.GetRandomFileName();

            var outDll = Path.Combine(path, string.Concat(filename, ".dll"));
            var outPdb = Path.Combine(path, string.Concat(filename, ".pdb"));

            var parameters = CodeDomProvider.GetCompilerInfo(language).CreateDefaultCompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = false;
            parameters.CompilerOptions =
                string.Concat(
                    string.Format("/optimize{0} /unsafe+{1}", this.DebugInfo ? "-" : "+", this.DebugInfo ? " /debug:full" : string.Empty), 
                    string.IsNullOrWhiteSpace(this.CoreLibPath) ? string.Empty : string.Format(" /nostdlib+ /r:\"{0}\"", this.CoreLibPath));
            parameters.OutputAssembly = outDll;

            var results = codeProvider.CompileAssemblyFromFile(parameters, sources);

            if (results.Errors.Count > 0)
            {
                Console.WriteLine(@"Errors/Warnings:");
                foreach (CompilerError compilerError in results.Errors)
                {
                    Console.WriteLine(compilerError);
                }
            }

            this.DllFilePath = outDll;
            this.PdbFilePath = outPdb;

            // Successful Compile
            return AssemblyMetadata.CreateFromImageStream(new FileStream(results.PathToAssembly, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblyMetadata CompileWithRoslyn(string[] source)
        {
            var baseName = Path.GetRandomFileName();
            var nameDll = baseName + ".dll";
            var namePdb = baseName + ".pdb";
            var outDll = Path.Combine(Path.GetTempPath(), nameDll);
            var outPdb = Path.Combine(Path.GetTempPath(), namePdb);

            var syntaxTrees =
                source.Select(s => CSharpSyntaxTree.ParseText(new StreamReader(s).ReadToEnd(), new CSharpParseOptions(LanguageVersion.Experimental)));

            var coreLibRefAssembly = string.IsNullOrWhiteSpace(this.CoreLibPath)
                                         ? new MetadataImageReference(new FileStream(typeof(int).Assembly.Location, FileMode.Open, FileAccess.Read))
                                         : new MetadataImageReference(new FileStream(this.CoreLibPath, FileMode.Open, FileAccess.Read));

            var options =
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithAllowUnsafe(true)
                                                                                 .WithOptimizations(!this.DebugInfo)
                                                                                 .WithRuntimeMetadataVersion("4.5");

            var compilation = CSharpCompilation.Create(nameDll, syntaxTrees, new[] { coreLibRefAssembly }, options);

            using (var dllStream = new FileStream(outDll, FileMode.OpenOrCreate))
            {
                using (var pdbStream = new FileStream(outPdb, FileMode.OpenOrCreate))
                {
                    var result = compilation.Emit(peStream: dllStream, pdbFilePath: outPdb, pdbStream: pdbStream);

                    if (result.Diagnostics.Length > 0)
                    {
                        Console.WriteLine(@"Errors/Warnings:");
                        foreach (var diagnostic in result.Diagnostics)
                        {
                            Console.WriteLine(diagnostic);
                        }
                    }
                }
            }

            this.DllFilePath = outDll;
            this.PdbFilePath = outPdb;

            // Successful Compile
            return AssemblyMetadata.CreateFromImageStream(new FileStream(outDll, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="stackCall">
        /// </param>
        private void DiscoverRequiredTypesAndMethodsInMethod(IMethod method, Queue<IMethod> stackCall)
        {
            stackCall.Enqueue(method);

            // add all generic types in parameters
            foreach (var parameter in method.GetParameters())
            {
                this.AddGenericSpecializedType(parameter.ParameterType);
            }

            // add return type
            this.AddGenericSpecializedType(method.ReturnType);

            // disover it again in specialized method
            method.DiscoverRequiredTypesAndMethodsInMethodBody(this.usedGenericSpecialiazedTypes, this.usedGenericSpecialiazedMethods, null, stackCall);

            stackCall.Dequeue();
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblySymbol">
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<IType> EnumAllTypes(PEAssemblySymbol assemblySymbol)
        {
            Debug.Assert(assemblySymbol != null, "missing assembly");

            foreach (var module in assemblySymbol.Modules)
            {
                var peModuleSymbol = module as PEModuleSymbol;
                foreach (var metadataTypeAdapter in
                    from symbol in GetAllNamespaces(peModuleSymbol.GlobalNamespace).SelectMany(n => n.GetTypeMembers()) select new MetadataTypeAdapter(symbol))
                {
                    yield return metadataTypeAdapter;
                    foreach (var nestedType in EnumAllNestedTypes(metadataTypeAdapter))
                    {
                        yield return nestedType;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyIdentity">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblyMetadata GetAssemblyMetadata(AssemblyIdentity assemblyIdentity)
        {
            var resolveReferencePath = this.ResolveReferencePath(assemblyIdentity);
            if (string.IsNullOrWhiteSpace(resolveReferencePath))
            {
                return null;
            }

            return AssemblyMetadata.CreateFromImageStream(new FileStream(resolveReferencePath, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblySymbol LoadAssemblySymbol(AssemblyIdentity identity)
        {
            AssemblySymbol symbol;
            if (this.cache.TryGetValue(identity, out symbol))
            {
                return symbol;
            }

            var assemblyMetadata = this.GetAssemblyMetadata(identity);
            if (assemblyMetadata != null)
            {
                return this.LoadAssemblySymbol(assemblyMetadata);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyMetadata">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblySymbol LoadAssemblySymbol(AssemblyMetadata assemblyMetadata)
        {
            AssemblySymbol symbol;
            if (this.cache.TryGetValue(assemblyMetadata.Assembly.Identity, out symbol))
            {
                return symbol;
            }

            var assemblySymbol = GetAssemblySymbol(assemblyMetadata);

            this.cache[assemblyMetadata.Assembly.Identity] = assemblySymbol;
            this.unifiedAssemblies.Add(new UnifiedAssembly<AssemblySymbol>(assemblySymbol, assemblyMetadata.Assembly.Identity));

            var moduleReferences = this.LoadReferences(assemblyMetadata);
            foreach (var module in assemblySymbol.Modules)
            {
                module.SetReferences(moduleReferences);
            }

            this.SetCorLib(assemblySymbol);

            return assemblySymbol;
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblySymbol LoadAssemblySymbolOrMissingAssemblySymbol(AssemblyIdentity identity)
        {
            AssemblySymbol symbol;
            if (this.cache.TryGetValue(identity, out symbol))
            {
                return symbol;
            }

            var peAssemblySymbol = this.LoadAssemblySymbol(identity);
            if (peAssemblySymbol != null)
            {
                return peAssemblySymbol;
            }

            return new MissingAssemblySymbol(identity);
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyMetadata">
        /// </param>
        /// <returns>
        /// </returns>
        private ModuleReferences<AssemblySymbol> LoadReferences(AssemblyMetadata assemblyMetadata)
        {
            var peReferences = ImmutableArray.CreateRange(assemblyMetadata.Assembly.AssemblyReferences.Select(this.LoadAssemblySymbolOrMissingAssemblySymbol));

            var moduleReferences = new ModuleReferences<AssemblySymbol>(
                assemblyMetadata.Assembly.AssemblyReferences, peReferences, ImmutableArray.CreateRange(this.unifiedAssemblies));

            return moduleReferences;
        }

        /// <summary>
        /// </summary>
        /// <param name="readAll">
        /// </param>
        /// <param name="ignoreCurrent">
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<IType> ReadTypes(bool readAll = false, bool ignoreCurrent = false)
        {
            var assemblySymbol = this.LoadAssemblySymbol(this.Assembly);
            foreach (var type in this.ReadTypes(assemblySymbol, readAll, ignoreCurrent))
            {
                yield return type;
            }
        }

        private IEnumerable<IType> ReadTypes(AssemblySymbol assemblySymbol, bool readAll = false, bool ignoreCurrent = false)
        {
            if (!ignoreCurrent)
            {
                // 3) Load Types
                foreach (var metadataTypeAdapter in this.EnumAllTypes(assemblySymbol as PEAssemblySymbol))
                {
                    yield return metadataTypeAdapter;
                }
            }

            if (readAll)
            {
                var moduleReferences = this.LoadReferences(this.Assembly);
                foreach (var moduleAssemblySymbol in moduleReferences.Symbols)
                {
                    foreach (var metadataTypeAdapter in this.EnumAllTypes(moduleAssemblySymbol as PEAssemblySymbol))
                    {
                        yield return metadataTypeAdapter;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyIdentity">
        /// </param>
        /// <returns>
        /// </returns>
        private string ResolveReferencePath(AssemblyIdentity assemblyIdentity)
        {
            if (assemblyIdentity.Name == "CoreLib")
            {
                return this.CoreLibPath;
            }

            if (assemblyIdentity.Name == "mscorlib")
            {
                return typeof(int).Assembly.Location;
            }

            var dllFileName = string.Concat(assemblyIdentity.Name, ".dll");
            if (File.Exists(dllFileName))
            {
                return Path.GetFullPath(dllFileName);
            }

            if (!string.IsNullOrWhiteSpace(this.DefaultDllLocations))
            {
                var dllFullName = Path.Combine(this.DefaultDllLocations, dllFileName);
                if (File.Exists(dllFullName))
                {
                    return dllFullName;
                }
            }

            Debug.Fail("Not implemented yet");
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblySymbol">
        /// </param>
        private void SetCorLib(PEAssemblySymbol assemblySymbol)
        {
            if (!assemblySymbol.Assembly.AssemblyReferences.Any())
            {
                // this is the core lib
                assemblySymbol.SetCorLibrary(assemblySymbol);
                return;
            }

            var loadedRefAssemblies = from assemblyIdentity in assemblySymbol.Assembly.AssemblyReferences select this.LoadAssemblySymbol(assemblyIdentity);
            foreach (var loadedRefAssemblySymbol in loadedRefAssemblies)
            {
                var peRefAssembly = loadedRefAssemblySymbol as PEAssemblySymbol;
                if (peRefAssembly != null && !peRefAssembly.Assembly.AssemblyReferences.Any())
                {
                    assemblySymbol.SetCorLibrary(loadedRefAssemblySymbol);
                    return;
                }
            }

            Debug.Fail("CoreLib not set");
        }
    }
}
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
    using Gencode.SynthesizedMethods.MultiDimArray;
    using Gencode.SynthesizedMethods.SingleDimArray;
    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode;
    using Il2Native.Logic.Gencode.SynthesizedMethods;
    using Il2Native.Logic.Gencode.SynthesizedMethods.Enum;
    using Il2Native.Logic.Gencode.SynthesizedMethods.String;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class IlReader : IIlReader
    {
        /// <summary>
        /// </summary>
        private static readonly OpCode[] OpCodesMap = new OpCode[256];

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
        private ISet<IMethod> _calledMethods;

        /// <summary>
        /// </summary>
        private IDictionary<int, string> _usedStrings;

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
        private ISet<IType> _usedStructTypes;

        /// <summary>
        /// </summary>
        private ISet<IType> _usedArrayTypes;

        /// <summary>
        /// </summary>
        private ISet<IType> usedTypes;

        /// <summary>
        /// </summary>
        static IlReader()
        {
            OpCodesMap[(int)Code.Nop] = OpCodesEmit.Nop;
            OpCodesMap[(int)Code.Break] = OpCodesEmit.Break;
            OpCodesMap[(int)Code.Ldarg_0] = OpCodesEmit.Ldarg_0;
            OpCodesMap[(int)Code.Ldarg_1] = OpCodesEmit.Ldarg_1;
            OpCodesMap[(int)Code.Ldarg_2] = OpCodesEmit.Ldarg_2;
            OpCodesMap[(int)Code.Ldarg_3] = OpCodesEmit.Ldarg_3;
            OpCodesMap[(int)Code.Ldloc_0] = OpCodesEmit.Ldloc_0;
            OpCodesMap[(int)Code.Ldloc_1] = OpCodesEmit.Ldloc_1;
            OpCodesMap[(int)Code.Ldloc_2] = OpCodesEmit.Ldloc_2;
            OpCodesMap[(int)Code.Ldloc_3] = OpCodesEmit.Ldloc_3;
            OpCodesMap[(int)Code.Stloc_0] = OpCodesEmit.Stloc_0;
            OpCodesMap[(int)Code.Stloc_1] = OpCodesEmit.Stloc_1;
            OpCodesMap[(int)Code.Stloc_2] = OpCodesEmit.Stloc_2;
            OpCodesMap[(int)Code.Stloc_3] = OpCodesEmit.Stloc_3;
            OpCodesMap[(int)Code.Ldarg_S] = OpCodesEmit.Ldarg_S;
            OpCodesMap[(int)Code.Ldarga_S] = OpCodesEmit.Ldarga_S;
            OpCodesMap[(int)Code.Starg_S] = OpCodesEmit.Starg_S;
            OpCodesMap[(int)Code.Ldloc_S] = OpCodesEmit.Ldloc_S;
            OpCodesMap[(int)Code.Ldloca_S] = OpCodesEmit.Ldloca_S;
            OpCodesMap[(int)Code.Stloc_S] = OpCodesEmit.Stloc_S;
            OpCodesMap[(int)Code.Ldnull] = OpCodesEmit.Ldnull;
            OpCodesMap[(int)Code.Ldc_I4_M1] = OpCodesEmit.Ldc_I4_M1;
            OpCodesMap[(int)Code.Ldc_I4_0] = OpCodesEmit.Ldc_I4_0;
            OpCodesMap[(int)Code.Ldc_I4_1] = OpCodesEmit.Ldc_I4_1;
            OpCodesMap[(int)Code.Ldc_I4_2] = OpCodesEmit.Ldc_I4_2;
            OpCodesMap[(int)Code.Ldc_I4_3] = OpCodesEmit.Ldc_I4_3;
            OpCodesMap[(int)Code.Ldc_I4_4] = OpCodesEmit.Ldc_I4_4;
            OpCodesMap[(int)Code.Ldc_I4_5] = OpCodesEmit.Ldc_I4_5;
            OpCodesMap[(int)Code.Ldc_I4_6] = OpCodesEmit.Ldc_I4_6;
            OpCodesMap[(int)Code.Ldc_I4_7] = OpCodesEmit.Ldc_I4_7;
            OpCodesMap[(int)Code.Ldc_I4_8] = OpCodesEmit.Ldc_I4_8;
            OpCodesMap[(int)Code.Ldc_I4_S] = OpCodesEmit.Ldc_I4_S;
            OpCodesMap[(int)Code.Ldc_I4] = OpCodesEmit.Ldc_I4;
            OpCodesMap[(int)Code.Ldc_I8] = OpCodesEmit.Ldc_I8;
            OpCodesMap[(int)Code.Ldc_R4] = OpCodesEmit.Ldc_R4;
            OpCodesMap[(int)Code.Ldc_R8] = OpCodesEmit.Ldc_R8;
            OpCodesMap[(int)Code.Dup] = OpCodesEmit.Dup;
            OpCodesMap[(int)Code.Pop] = OpCodesEmit.Pop;
            OpCodesMap[(int)Code.Jmp] = OpCodesEmit.Jmp;
            OpCodesMap[(int)Code.Call] = OpCodesEmit.Call;
            OpCodesMap[(int)Code.Calli] = OpCodesEmit.Calli;
            OpCodesMap[(int)Code.Ret] = OpCodesEmit.Ret;
            OpCodesMap[(int)Code.Br_S] = OpCodesEmit.Br_S;
            OpCodesMap[(int)Code.Brfalse_S] = OpCodesEmit.Brfalse_S;
            OpCodesMap[(int)Code.Brtrue_S] = OpCodesEmit.Brtrue_S;
            OpCodesMap[(int)Code.Beq_S] = OpCodesEmit.Beq_S;
            OpCodesMap[(int)Code.Bge_S] = OpCodesEmit.Bge_S;
            OpCodesMap[(int)Code.Bgt_S] = OpCodesEmit.Bgt_S;
            OpCodesMap[(int)Code.Ble_S] = OpCodesEmit.Ble_S;
            OpCodesMap[(int)Code.Blt_S] = OpCodesEmit.Blt_S;
            OpCodesMap[(int)Code.Bne_Un_S] = OpCodesEmit.Bne_Un_S;
            OpCodesMap[(int)Code.Bge_Un_S] = OpCodesEmit.Bge_Un_S;
            OpCodesMap[(int)Code.Bgt_Un_S] = OpCodesEmit.Bgt_Un_S;
            OpCodesMap[(int)Code.Ble_Un_S] = OpCodesEmit.Ble_Un_S;
            OpCodesMap[(int)Code.Blt_Un_S] = OpCodesEmit.Blt_Un_S;
            OpCodesMap[(int)Code.Br] = OpCodesEmit.Br;
            OpCodesMap[(int)Code.Brfalse] = OpCodesEmit.Brfalse;
            OpCodesMap[(int)Code.Brtrue] = OpCodesEmit.Brtrue;
            OpCodesMap[(int)Code.Beq] = OpCodesEmit.Beq;
            OpCodesMap[(int)Code.Bge] = OpCodesEmit.Bge;
            OpCodesMap[(int)Code.Bgt] = OpCodesEmit.Bgt;
            OpCodesMap[(int)Code.Ble] = OpCodesEmit.Ble;
            OpCodesMap[(int)Code.Blt] = OpCodesEmit.Blt;
            OpCodesMap[(int)Code.Bne_Un] = OpCodesEmit.Bne_Un;
            OpCodesMap[(int)Code.Bge_Un] = OpCodesEmit.Bge_Un;
            OpCodesMap[(int)Code.Bgt_Un] = OpCodesEmit.Bgt_Un;
            OpCodesMap[(int)Code.Ble_Un] = OpCodesEmit.Ble_Un;
            OpCodesMap[(int)Code.Blt_Un] = OpCodesEmit.Blt_Un;
            OpCodesMap[(int)Code.Switch] = OpCodesEmit.Switch;
            OpCodesMap[(int)Code.Ldind_I1] = OpCodesEmit.Ldind_I1;
            OpCodesMap[(int)Code.Ldind_U1] = OpCodesEmit.Ldind_U1;
            OpCodesMap[(int)Code.Ldind_I2] = OpCodesEmit.Ldind_I2;
            OpCodesMap[(int)Code.Ldind_U2] = OpCodesEmit.Ldind_U2;
            OpCodesMap[(int)Code.Ldind_I4] = OpCodesEmit.Ldind_I4;
            OpCodesMap[(int)Code.Ldind_U4] = OpCodesEmit.Ldind_U4;
            OpCodesMap[(int)Code.Ldind_I8] = OpCodesEmit.Ldind_I8;
            OpCodesMap[(int)Code.Ldind_I] = OpCodesEmit.Ldind_I;
            OpCodesMap[(int)Code.Ldind_R4] = OpCodesEmit.Ldind_R4;
            OpCodesMap[(int)Code.Ldind_R8] = OpCodesEmit.Ldind_R8;
            OpCodesMap[(int)Code.Ldind_Ref] = OpCodesEmit.Ldind_Ref;
            OpCodesMap[(int)Code.Stind_Ref] = OpCodesEmit.Stind_Ref;
            OpCodesMap[(int)Code.Stind_I1] = OpCodesEmit.Stind_I1;
            OpCodesMap[(int)Code.Stind_I2] = OpCodesEmit.Stind_I2;
            OpCodesMap[(int)Code.Stind_I4] = OpCodesEmit.Stind_I4;
            OpCodesMap[(int)Code.Stind_I8] = OpCodesEmit.Stind_I8;
            OpCodesMap[(int)Code.Stind_R4] = OpCodesEmit.Stind_R4;
            OpCodesMap[(int)Code.Stind_R8] = OpCodesEmit.Stind_R8;
            OpCodesMap[(int)Code.Add] = OpCodesEmit.Add;
            OpCodesMap[(int)Code.Sub] = OpCodesEmit.Sub;
            OpCodesMap[(int)Code.Mul] = OpCodesEmit.Mul;
            OpCodesMap[(int)Code.Div] = OpCodesEmit.Div;
            OpCodesMap[(int)Code.Div_Un] = OpCodesEmit.Div_Un;
            OpCodesMap[(int)Code.Rem] = OpCodesEmit.Rem;
            OpCodesMap[(int)Code.Rem_Un] = OpCodesEmit.Rem_Un;
            OpCodesMap[(int)Code.And] = OpCodesEmit.And;
            OpCodesMap[(int)Code.Or] = OpCodesEmit.Or;
            OpCodesMap[(int)Code.Xor] = OpCodesEmit.Xor;
            OpCodesMap[(int)Code.Shl] = OpCodesEmit.Shl;
            OpCodesMap[(int)Code.Shr] = OpCodesEmit.Shr;
            OpCodesMap[(int)Code.Shr_Un] = OpCodesEmit.Shr_Un;
            OpCodesMap[(int)Code.Neg] = OpCodesEmit.Neg;
            OpCodesMap[(int)Code.Not] = OpCodesEmit.Not;
            OpCodesMap[(int)Code.Conv_I1] = OpCodesEmit.Conv_I1;
            OpCodesMap[(int)Code.Conv_I2] = OpCodesEmit.Conv_I2;
            OpCodesMap[(int)Code.Conv_I4] = OpCodesEmit.Conv_I4;
            OpCodesMap[(int)Code.Conv_I8] = OpCodesEmit.Conv_I8;
            OpCodesMap[(int)Code.Conv_R4] = OpCodesEmit.Conv_R4;
            OpCodesMap[(int)Code.Conv_R8] = OpCodesEmit.Conv_R8;
            OpCodesMap[(int)Code.Conv_U4] = OpCodesEmit.Conv_U4;
            OpCodesMap[(int)Code.Conv_U8] = OpCodesEmit.Conv_U8;
            OpCodesMap[(int)Code.Callvirt] = OpCodesEmit.Callvirt;
            OpCodesMap[(int)Code.Cpobj] = OpCodesEmit.Cpobj;
            OpCodesMap[(int)Code.Ldobj] = OpCodesEmit.Ldobj;
            OpCodesMap[(int)Code.Ldstr] = OpCodesEmit.Ldstr;
            OpCodesMap[(int)Code.Newobj] = OpCodesEmit.Newobj;
            OpCodesMap[(int)Code.Castclass] = OpCodesEmit.Castclass;
            OpCodesMap[(int)Code.Isinst] = OpCodesEmit.Isinst;
            OpCodesMap[(int)Code.Conv_R_Un] = OpCodesEmit.Conv_R_Un;
            OpCodesMap[(int)Code.Unbox] = OpCodesEmit.Unbox;
            OpCodesMap[(int)Code.Throw] = OpCodesEmit.Throw;
            OpCodesMap[(int)Code.Ldfld] = OpCodesEmit.Ldfld;
            OpCodesMap[(int)Code.Ldflda] = OpCodesEmit.Ldflda;
            OpCodesMap[(int)Code.Stfld] = OpCodesEmit.Stfld;
            OpCodesMap[(int)Code.Ldsfld] = OpCodesEmit.Ldsfld;
            OpCodesMap[(int)Code.Ldsflda] = OpCodesEmit.Ldsflda;
            OpCodesMap[(int)Code.Stsfld] = OpCodesEmit.Stsfld;
            OpCodesMap[(int)Code.Stobj] = OpCodesEmit.Stobj;
            OpCodesMap[(int)Code.Conv_Ovf_I1_Un] = OpCodesEmit.Conv_Ovf_I1_Un;
            OpCodesMap[(int)Code.Conv_Ovf_I2_Un] = OpCodesEmit.Conv_Ovf_I2_Un;
            OpCodesMap[(int)Code.Conv_Ovf_I4_Un] = OpCodesEmit.Conv_Ovf_I4_Un;
            OpCodesMap[(int)Code.Conv_Ovf_I8_Un] = OpCodesEmit.Conv_Ovf_I8_Un;
            OpCodesMap[(int)Code.Conv_Ovf_U1_Un] = OpCodesEmit.Conv_Ovf_U1_Un;
            OpCodesMap[(int)Code.Conv_Ovf_U2_Un] = OpCodesEmit.Conv_Ovf_U2_Un;
            OpCodesMap[(int)Code.Conv_Ovf_U4_Un] = OpCodesEmit.Conv_Ovf_U4_Un;
            OpCodesMap[(int)Code.Conv_Ovf_U8_Un] = OpCodesEmit.Conv_Ovf_U8_Un;
            OpCodesMap[(int)Code.Conv_Ovf_I_Un] = OpCodesEmit.Conv_Ovf_I_Un;
            OpCodesMap[(int)Code.Conv_Ovf_U_Un] = OpCodesEmit.Conv_Ovf_U_Un;
            OpCodesMap[(int)Code.Box] = OpCodesEmit.Box;
            OpCodesMap[(int)Code.Newarr] = OpCodesEmit.Newarr;
            OpCodesMap[(int)Code.Ldlen] = OpCodesEmit.Ldlen;
            OpCodesMap[(int)Code.Ldelema] = OpCodesEmit.Ldelema;
            OpCodesMap[(int)Code.Ldelem_I1] = OpCodesEmit.Ldelem_I1;
            OpCodesMap[(int)Code.Ldelem_U1] = OpCodesEmit.Ldelem_U1;
            OpCodesMap[(int)Code.Ldelem_I2] = OpCodesEmit.Ldelem_I2;
            OpCodesMap[(int)Code.Ldelem_U2] = OpCodesEmit.Ldelem_U2;
            OpCodesMap[(int)Code.Ldelem_I4] = OpCodesEmit.Ldelem_I4;
            OpCodesMap[(int)Code.Ldelem_U4] = OpCodesEmit.Ldelem_U4;
            OpCodesMap[(int)Code.Ldelem_I8] = OpCodesEmit.Ldelem_I8;
            OpCodesMap[(int)Code.Ldelem_I] = OpCodesEmit.Ldelem_I;
            OpCodesMap[(int)Code.Ldelem_R4] = OpCodesEmit.Ldelem_R4;
            OpCodesMap[(int)Code.Ldelem_R8] = OpCodesEmit.Ldelem_R8;
            OpCodesMap[(int)Code.Ldelem_Ref] = OpCodesEmit.Ldelem_Ref;
            OpCodesMap[(int)Code.Stelem_I] = OpCodesEmit.Stelem_I;
            OpCodesMap[(int)Code.Stelem_I1] = OpCodesEmit.Stelem_I1;
            OpCodesMap[(int)Code.Stelem_I2] = OpCodesEmit.Stelem_I2;
            OpCodesMap[(int)Code.Stelem_I4] = OpCodesEmit.Stelem_I4;
            OpCodesMap[(int)Code.Stelem_I8] = OpCodesEmit.Stelem_I8;
            OpCodesMap[(int)Code.Stelem_R4] = OpCodesEmit.Stelem_R4;
            OpCodesMap[(int)Code.Stelem_R8] = OpCodesEmit.Stelem_R8;
            OpCodesMap[(int)Code.Stelem_Ref] = OpCodesEmit.Stelem_Ref;
            OpCodesMap[(int)Code.Ldelem] = OpCodesEmit.Ldelem;
            OpCodesMap[(int)Code.Stelem] = OpCodesEmit.Stelem;
            OpCodesMap[(int)Code.Unbox_Any] = OpCodesEmit.Unbox_Any;
            OpCodesMap[(int)Code.Conv_Ovf_I1] = OpCodesEmit.Conv_Ovf_I1;
            OpCodesMap[(int)Code.Conv_Ovf_U1] = OpCodesEmit.Conv_Ovf_U1;
            OpCodesMap[(int)Code.Conv_Ovf_I2] = OpCodesEmit.Conv_Ovf_I2;
            OpCodesMap[(int)Code.Conv_Ovf_U2] = OpCodesEmit.Conv_Ovf_U2;
            OpCodesMap[(int)Code.Conv_Ovf_I4] = OpCodesEmit.Conv_Ovf_I4;
            OpCodesMap[(int)Code.Conv_Ovf_U4] = OpCodesEmit.Conv_Ovf_U4;
            OpCodesMap[(int)Code.Conv_Ovf_I8] = OpCodesEmit.Conv_Ovf_I8;
            OpCodesMap[(int)Code.Conv_Ovf_U8] = OpCodesEmit.Conv_Ovf_U8;
            OpCodesMap[(int)Code.Refanyval] = OpCodesEmit.Refanyval;
            OpCodesMap[(int)Code.Ckfinite] = OpCodesEmit.Ckfinite;
            OpCodesMap[(int)Code.Mkrefany] = OpCodesEmit.Mkrefany;
            OpCodesMap[(int)Code.Ldtoken] = OpCodesEmit.Ldtoken;
            OpCodesMap[(int)Code.Conv_U2] = OpCodesEmit.Conv_U2;
            OpCodesMap[(int)Code.Conv_U1] = OpCodesEmit.Conv_U1;
            OpCodesMap[(int)Code.Conv_I] = OpCodesEmit.Conv_I;
            OpCodesMap[(int)Code.Conv_Ovf_I] = OpCodesEmit.Conv_Ovf_I;
            OpCodesMap[(int)Code.Conv_Ovf_U] = OpCodesEmit.Conv_Ovf_U;
            OpCodesMap[(int)Code.Add_Ovf] = OpCodesEmit.Add_Ovf;
            OpCodesMap[(int)Code.Add_Ovf_Un] = OpCodesEmit.Add_Ovf_Un;
            OpCodesMap[(int)Code.Mul_Ovf] = OpCodesEmit.Mul_Ovf;
            OpCodesMap[(int)Code.Mul_Ovf_Un] = OpCodesEmit.Mul_Ovf_Un;
            OpCodesMap[(int)Code.Sub_Ovf] = OpCodesEmit.Sub_Ovf;
            OpCodesMap[(int)Code.Sub_Ovf_Un] = OpCodesEmit.Sub_Ovf_Un;
            OpCodesMap[(int)Code.Endfinally] = OpCodesEmit.Endfinally;
            OpCodesMap[(int)Code.Leave] = OpCodesEmit.Leave;
            OpCodesMap[(int)Code.Leave_S] = OpCodesEmit.Leave_S;
            OpCodesMap[(int)Code.Stind_I] = OpCodesEmit.Stind_I;
            OpCodesMap[(int)Code.Conv_U] = OpCodesEmit.Conv_U;
            OpCodesMap[(int)Code.Arglist] = OpCodesEmit.Arglist;
            OpCodesMap[(int)Code.Ceq] = OpCodesEmit.Ceq;
            OpCodesMap[(int)Code.Cgt] = OpCodesEmit.Cgt;
            OpCodesMap[(int)Code.Cgt_Un] = OpCodesEmit.Cgt_Un;
            OpCodesMap[(int)Code.Clt] = OpCodesEmit.Clt;
            OpCodesMap[(int)Code.Clt_Un] = OpCodesEmit.Clt_Un;
            OpCodesMap[(int)Code.Ldftn] = OpCodesEmit.Ldftn;
            OpCodesMap[(int)Code.Ldvirtftn] = OpCodesEmit.Ldvirtftn;
            OpCodesMap[(int)Code.Ldarg] = OpCodesEmit.Ldarg;
            OpCodesMap[(int)Code.Ldarga] = OpCodesEmit.Ldarga;
            OpCodesMap[(int)Code.Starg] = OpCodesEmit.Starg;
            OpCodesMap[(int)Code.Ldloc] = OpCodesEmit.Ldloc;
            OpCodesMap[(int)Code.Ldloca] = OpCodesEmit.Ldloca;
            OpCodesMap[(int)Code.Stloc] = OpCodesEmit.Stloc;
            OpCodesMap[(int)Code.Localloc] = OpCodesEmit.Localloc;
            OpCodesMap[(int)Code.Endfilter] = OpCodesEmit.Endfilter;
            OpCodesMap[(int)Code.Unaligned] = OpCodesEmit.Unaligned;
            OpCodesMap[(int)Code.Volatile] = OpCodesEmit.Volatile;
            OpCodesMap[(int)Code.Tail] = OpCodesEmit.Tailcall;
            OpCodesMap[(int)Code.Initobj] = OpCodesEmit.Initobj;
            OpCodesMap[(int)Code.Constrained] = OpCodesEmit.Constrained;
            OpCodesMap[(int)Code.Cpblk] = OpCodesEmit.Cpblk;
            OpCodesMap[(int)Code.Initblk] = OpCodesEmit.Initblk;

            // OpCodesMap[(int)Code.No] = OpCodesEmit.No;
            OpCodesMap[(int)Code.Rethrow] = OpCodesEmit.Rethrow;
            OpCodesMap[(int)Code.Sizeof] = OpCodesEmit.Sizeof;
            OpCodesMap[(int)Code.Refanytype] = OpCodesEmit.Refanytype;
            OpCodesMap[(int)Code.Readonly] = OpCodesEmit.Readonly;
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
        public ITypeResolver TypeResolver { get; set; }

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
                return this._calledMethods;
            }

            set
            {
                this._calledMethods = value;
            }
        }

        /// <summary>
        /// </summary>
        public IDictionary<int, string> UsedStrings
        {
            get
            {
                return this._usedStrings;
            }

            set
            {
                this._usedStrings = value;
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
                return this._usedStructTypes;
            }

            set
            {
                this._usedStructTypes = value;
            }
        }

        /// <summary>
        /// </summary>
        public ISet<IType> UsedArrayTypes
        {
            get
            {
                return this._usedArrayTypes;
            }

            set
            {
                this._usedArrayTypes = value;
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
        public static IEnumerable<IConstructor> Constructors(IType type, ITypeResolver typeResolver)
        {
            return Constructors(type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, typeResolver);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IField> Fields(IType type, ITypeResolver typeResolver)
        {
            return Fields(type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, typeResolver);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IMethod> Methods(IType type, ITypeResolver typeResolver, bool excludeSpecializations = false)
        {
            return Methods(
                type,
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance,
                typeResolver,
                excludeSpecializations);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="flags">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IField> Fields(IType type, BindingFlags flags, ITypeResolver typeResolver)
        {
            Debug.Assert(type != null);

            foreach (var field in type.GetFields(flags))
            {
                yield return field;
            }

            // extra fields
            var normal = type.ToNormal();
            if (normal.IsEnum)
            {
                foreach (var field in EnumGen.GetFields(normal, typeResolver))
                {
                    yield return field;
                }                    
            } 
            else if (type.IsMultiArray)
            {
                // append methods or MultiArray
                foreach (var field in ArrayMultiDimensionGen.GetFields(type, typeResolver)) 
                {
                    yield return field;
                }
            }
            else if (type.IsArray)
            {
                // append methods or MultiArray
                foreach (var field in ArraySingleDimensionGen.GetFields(type, typeResolver))
                {
                    yield return field;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="flags">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IConstructor> Constructors(IType type, BindingFlags flags, ITypeResolver typeResolver)
        {
            Debug.Assert(type != null);

            foreach (var constructor in type.GetConstructors(flags).Where(m => !m.IsGenericMethodDefinition))
            {
                yield return constructor;
            }

            // append methods or MultiArray
            if (type.IsMultiArray)
            {
                yield return new SynthesizedMultiDimArrayCtorMethod(type, typeResolver);
            }
            else if (type.IsArray)
            {
                yield return new SynthesizedSingleDimArrayCtorMethod(type, typeResolver);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="flags">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IMethod> Methods(IType type, BindingFlags flags, ITypeResolver typeResolver, bool excludeSpecializations = false)
        {
            Debug.Assert(type != null);

            if (!excludeSpecializations)
            {
                foreach (var method in type.GetMethods(flags).Where(m => !m.IsGenericMethodDefinition))
                {
                    yield return method;
                }
            }
            else
            {
                foreach (var method in type.GetMethods(flags))
                {
                    yield return method;
                }
            }

            if (!excludeSpecializations)
            {
                // TODO: remove if and write code to get info from following synth methods
                // append C# native compiler infrastructure methods
                yield return new SynthesizedGetSizeMethod(type, typeResolver);

                // append internal methods
                yield return new SynthesizedGetTypeMethod(type, typeResolver);
            }

            var normal = type.ToNormal();
            if (normal.IsEnum)
            {
                yield return new SynthesizedEnumGetHashCodeMethod(type, typeResolver);
                yield return new SynthesizedEnumToStringMethod(type, typeResolver);
            }

            // append methods or MultiArray
            if (type.IsMultiArray)
            {
                yield return new SynthesizedMultiDimArrayGetMethod(type, typeResolver);
                yield return new SynthesizedMultiDimArraySetMethod(type, typeResolver);
                yield return new SynthesizedMultiDimArrayAddressMethod(type, typeResolver);
            }
            else if (type.IsArray)
            {
                yield return new SynthesizedSingleDimArrayIListGetEnumeratorMethod(type, typeResolver);
                yield return new SynthesizedSingleDimArrayIListGetCountMethod(type, typeResolver);
                yield return new SynthesizedSingleDimArrayIListGetItemMethod(type, typeResolver);
                yield return new SynthesizedSingleDimArrayIListSetItemMethod(type, typeResolver);
            }
            else if (type.IsString)
            {
                yield return new SynthesizedStrLenMethod(typeResolver);
                yield return new SynthesizedCtorSBytePtrMethod(typeResolver);
                yield return new SynthesizedCtorSBytePtrStartLengthMethod(typeResolver);
                yield return new SynthesizedCtorSBytePtrStartLengthEncodingMethod(typeResolver);
            }

            if (excludeSpecializations)
            {
                yield break;
            }

            // append specialized methods
            IEnumerable<IMethod> genMethodSpecializationForType = null;
            if (genMethodSpec == null || !genMethodSpec.TryGetValue(type, out genMethodSpecializationForType))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public IEnumerable<IType> CompileSourceWithRoslyn(params string[] source)
        {
            var nameDll = "__unrestricted__.dll";

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

            foreach (var opCode in this.OpCodes(method.GetMethodBody(genericContext), method.Module, genericContext, stackCall))
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

                var opCode = OpCodesMap[(int)code];

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
                    case Code.Ldarg:
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
                        this.AddString(token, @string);
                        yield return new OpCodeStringPart(opCode, startAddress, currentAddress, new KeyValuePair<int, string>(token, @string));
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
                        this.AddCalledMethod(new SynthesizedNewMethod(constructor.DeclaringType, this.TypeResolver));
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
                        this.AddGenericSpecializedType(method.ReturnType);
                        foreach (var methodParameter in method.GetParameters())
                        {
                            this.AddStructType(methodParameter.ParameterType);
                            this.AddGenericSpecializedType(methodParameter.ParameterType);
                        }

                        this.AddUsedType(method.DeclaringType);
                        if (code == Code.Call)
                        {
                            this.AddCalledMethod(method);
                        }

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
                        Debug.Assert(field != null);
                        this.AddGenericSpecializedType(field.FieldType);
                        this.AddGenericSpecializedType(field.DeclaringType);
                        this.AddUsedType(field.FieldType);
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
                        this.AddUsedType(type);
                        if (code == Code.Box)
                        {
                            this.AddStructType(type);
                        }

                        if (code == Code.Newarr || code == Code.Ldelem || code == Code.Stelem)
                        {
                            this.AddArrayType(type.ToArrayType(1));
                        }

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
            if (this._calledMethods == null || method == null)
            {
                return;
            }

            this._calledMethods.Add(method);
        }

        private void AddString(int token, string usedString)
        {
            if (this._usedStrings == null || usedString == null)
            {
                return;
            }

            this._usedStrings[token] = usedString;
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
            if (this._usedStructTypes == null || type == null || !type.IsStructureType())
            {
                return;
            }

            this._usedStructTypes.Add(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void AddArrayType(IType type)
        {
            if (this._usedArrayTypes == null || type == null || !type.IsArray)
            {
                return;
            }

            this._usedArrayTypes.Add(type);
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
            this.AddArrayType(type);
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
            var srcFileName = Path.GetFileNameWithoutExtension(FirstSource);
            var baseName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            var nameDll = srcFileName + "_" + baseName + ".dll";
            var namePdb = srcFileName + "_" + baseName + ".pdb";
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
            method.DiscoverStructsArraysSpecializedTypesAndMethodsInMethodBody(
                this.usedGenericSpecialiazedTypes,
                this.usedGenericSpecialiazedMethods,
                null,
                this._usedArrayTypes,
                stackCall);

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
                    if (moduleAssemblySymbol is MissingAssemblySymbol)
                    {
                        throw new Exception(string.Format("Assembly '{0}' is missing", moduleAssemblySymbol));
                    }

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
                if (!string.IsNullOrWhiteSpace(this.CoreLibPath))
                {
                    return this.CoreLibPath;
                }

                Debug.Assert(false, "you are using mscorlib from .NET");

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
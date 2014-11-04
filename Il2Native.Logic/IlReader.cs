// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IlReader.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Il2Native.Logic.CodeParts;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using Microsoft.CSharp;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;
    using System.Diagnostics;

    /// <summary>
    /// </summary>
    public class IlReader
    {
        /// <summary>
        /// </summary>
        private static readonly IDictionary<Code, OpCode> opCodesMap = new SortedDictionary<Code, OpCode>();

        /// <summary>
        /// </summary>
        private static IDictionary<IType, IEnumerable<IMethod>> genMethodSpec;

        /// <summary>
        /// </summary>
        private readonly Lazy<IEnumerable<IType>> lazyTypes;

        /// <summary>
        /// </summary>
        private readonly Lazy<IEnumerable<IType>> lazyAllTypes;

        /// <summary>
        /// </summary>
        private readonly Lazy<IEnumerable<IType>> lazyAllReferencedTypes;

        /// <summary>
        /// </summary>
        private HashSet<IMethod> usedGenericSpecialiazedMethods;

        /// <summary>
        /// </summary>
        private HashSet<IType> usedGenericSpecialiazedTypes;

        /// <summary>
        /// </summary>
        private HashSet<IType> usedStructTypes;

        /// <summary>
        /// </summary>
        private HashSet<IType> usedTypes;

        /// <summary>
        /// </summary>
        private readonly HashSet<IField> usedStaticFieldsToRead = new HashSet<IField>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IMethod> calledMethods = new HashSet<IMethod>();

        /// <summary>
        /// </summary>
        private readonly Dictionary<AssemblyIdentity, AssemblySymbol> cache = new Dictionary<AssemblyIdentity, AssemblySymbol>();

        /// <summary>
        /// </summary>
        private readonly List<UnifiedAssembly<AssemblySymbol>> unifiedAssemblies = new List<UnifiedAssembly<AssemblySymbol>>();

        /// <summary>
        /// </summary>
        static IlReader()
        {
            opCodesMap[Code.Nop] = OpCodesEmit.Nop;
            opCodesMap[Code.Break] = OpCodesEmit.Break;
            opCodesMap[Code.Ldarg_0] = OpCodesEmit.Ldarg_0;
            opCodesMap[Code.Ldarg_1] = OpCodesEmit.Ldarg_1;
            opCodesMap[Code.Ldarg_2] = OpCodesEmit.Ldarg_2;
            opCodesMap[Code.Ldarg_3] = OpCodesEmit.Ldarg_3;
            opCodesMap[Code.Ldloc_0] = OpCodesEmit.Ldloc_0;
            opCodesMap[Code.Ldloc_1] = OpCodesEmit.Ldloc_1;
            opCodesMap[Code.Ldloc_2] = OpCodesEmit.Ldloc_2;
            opCodesMap[Code.Ldloc_3] = OpCodesEmit.Ldloc_3;
            opCodesMap[Code.Stloc_0] = OpCodesEmit.Stloc_0;
            opCodesMap[Code.Stloc_1] = OpCodesEmit.Stloc_1;
            opCodesMap[Code.Stloc_2] = OpCodesEmit.Stloc_2;
            opCodesMap[Code.Stloc_3] = OpCodesEmit.Stloc_3;
            opCodesMap[Code.Ldarg_S] = OpCodesEmit.Ldarg_S;
            opCodesMap[Code.Ldarga_S] = OpCodesEmit.Ldarga_S;
            opCodesMap[Code.Starg_S] = OpCodesEmit.Starg_S;
            opCodesMap[Code.Ldloc_S] = OpCodesEmit.Ldloc_S;
            opCodesMap[Code.Ldloca_S] = OpCodesEmit.Ldloca_S;
            opCodesMap[Code.Stloc_S] = OpCodesEmit.Stloc_S;
            opCodesMap[Code.Ldnull] = OpCodesEmit.Ldnull;
            opCodesMap[Code.Ldc_I4_M1] = OpCodesEmit.Ldc_I4_M1;
            opCodesMap[Code.Ldc_I4_0] = OpCodesEmit.Ldc_I4_0;
            opCodesMap[Code.Ldc_I4_1] = OpCodesEmit.Ldc_I4_1;
            opCodesMap[Code.Ldc_I4_2] = OpCodesEmit.Ldc_I4_2;
            opCodesMap[Code.Ldc_I4_3] = OpCodesEmit.Ldc_I4_3;
            opCodesMap[Code.Ldc_I4_4] = OpCodesEmit.Ldc_I4_4;
            opCodesMap[Code.Ldc_I4_5] = OpCodesEmit.Ldc_I4_5;
            opCodesMap[Code.Ldc_I4_6] = OpCodesEmit.Ldc_I4_6;
            opCodesMap[Code.Ldc_I4_7] = OpCodesEmit.Ldc_I4_7;
            opCodesMap[Code.Ldc_I4_8] = OpCodesEmit.Ldc_I4_8;
            opCodesMap[Code.Ldc_I4_S] = OpCodesEmit.Ldc_I4_S;
            opCodesMap[Code.Ldc_I4] = OpCodesEmit.Ldc_I4;
            opCodesMap[Code.Ldc_I8] = OpCodesEmit.Ldc_I8;
            opCodesMap[Code.Ldc_R4] = OpCodesEmit.Ldc_R4;
            opCodesMap[Code.Ldc_R8] = OpCodesEmit.Ldc_R8;
            opCodesMap[Code.Dup] = OpCodesEmit.Dup;
            opCodesMap[Code.Pop] = OpCodesEmit.Pop;
            opCodesMap[Code.Jmp] = OpCodesEmit.Jmp;
            opCodesMap[Code.Call] = OpCodesEmit.Call;
            opCodesMap[Code.Calli] = OpCodesEmit.Calli;
            opCodesMap[Code.Ret] = OpCodesEmit.Ret;
            opCodesMap[Code.Br_S] = OpCodesEmit.Br_S;
            opCodesMap[Code.Brfalse_S] = OpCodesEmit.Brfalse_S;
            opCodesMap[Code.Brtrue_S] = OpCodesEmit.Brtrue_S;
            opCodesMap[Code.Beq_S] = OpCodesEmit.Beq_S;
            opCodesMap[Code.Bge_S] = OpCodesEmit.Bge_S;
            opCodesMap[Code.Bgt_S] = OpCodesEmit.Bgt_S;
            opCodesMap[Code.Ble_S] = OpCodesEmit.Ble_S;
            opCodesMap[Code.Blt_S] = OpCodesEmit.Blt_S;
            opCodesMap[Code.Bne_Un_S] = OpCodesEmit.Bne_Un_S;
            opCodesMap[Code.Bge_Un_S] = OpCodesEmit.Bge_Un_S;
            opCodesMap[Code.Bgt_Un_S] = OpCodesEmit.Bgt_Un_S;
            opCodesMap[Code.Ble_Un_S] = OpCodesEmit.Ble_Un_S;
            opCodesMap[Code.Blt_Un_S] = OpCodesEmit.Blt_Un_S;
            opCodesMap[Code.Br] = OpCodesEmit.Br;
            opCodesMap[Code.Brfalse] = OpCodesEmit.Brfalse;
            opCodesMap[Code.Brtrue] = OpCodesEmit.Brtrue;
            opCodesMap[Code.Beq] = OpCodesEmit.Beq;
            opCodesMap[Code.Bge] = OpCodesEmit.Bge;
            opCodesMap[Code.Bgt] = OpCodesEmit.Bgt;
            opCodesMap[Code.Ble] = OpCodesEmit.Ble;
            opCodesMap[Code.Blt] = OpCodesEmit.Blt;
            opCodesMap[Code.Bne_Un] = OpCodesEmit.Bne_Un;
            opCodesMap[Code.Bge_Un] = OpCodesEmit.Bge_Un;
            opCodesMap[Code.Bgt_Un] = OpCodesEmit.Bgt_Un;
            opCodesMap[Code.Ble_Un] = OpCodesEmit.Ble_Un;
            opCodesMap[Code.Blt_Un] = OpCodesEmit.Blt_Un;
            opCodesMap[Code.Switch] = OpCodesEmit.Switch;
            opCodesMap[Code.Ldind_I1] = OpCodesEmit.Ldind_I1;
            opCodesMap[Code.Ldind_U1] = OpCodesEmit.Ldind_U1;
            opCodesMap[Code.Ldind_I2] = OpCodesEmit.Ldind_I2;
            opCodesMap[Code.Ldind_U2] = OpCodesEmit.Ldind_U2;
            opCodesMap[Code.Ldind_I4] = OpCodesEmit.Ldind_I4;
            opCodesMap[Code.Ldind_U4] = OpCodesEmit.Ldind_U4;
            opCodesMap[Code.Ldind_I8] = OpCodesEmit.Ldind_I8;
            opCodesMap[Code.Ldind_I] = OpCodesEmit.Ldind_I;
            opCodesMap[Code.Ldind_R4] = OpCodesEmit.Ldind_R4;
            opCodesMap[Code.Ldind_R8] = OpCodesEmit.Ldind_R8;
            opCodesMap[Code.Ldind_Ref] = OpCodesEmit.Ldind_Ref;
            opCodesMap[Code.Stind_Ref] = OpCodesEmit.Stind_Ref;
            opCodesMap[Code.Stind_I1] = OpCodesEmit.Stind_I1;
            opCodesMap[Code.Stind_I2] = OpCodesEmit.Stind_I2;
            opCodesMap[Code.Stind_I4] = OpCodesEmit.Stind_I4;
            opCodesMap[Code.Stind_I8] = OpCodesEmit.Stind_I8;
            opCodesMap[Code.Stind_R4] = OpCodesEmit.Stind_R4;
            opCodesMap[Code.Stind_R8] = OpCodesEmit.Stind_R8;
            opCodesMap[Code.Add] = OpCodesEmit.Add;
            opCodesMap[Code.Sub] = OpCodesEmit.Sub;
            opCodesMap[Code.Mul] = OpCodesEmit.Mul;
            opCodesMap[Code.Div] = OpCodesEmit.Div;
            opCodesMap[Code.Div_Un] = OpCodesEmit.Div_Un;
            opCodesMap[Code.Rem] = OpCodesEmit.Rem;
            opCodesMap[Code.Rem_Un] = OpCodesEmit.Rem_Un;
            opCodesMap[Code.And] = OpCodesEmit.And;
            opCodesMap[Code.Or] = OpCodesEmit.Or;
            opCodesMap[Code.Xor] = OpCodesEmit.Xor;
            opCodesMap[Code.Shl] = OpCodesEmit.Shl;
            opCodesMap[Code.Shr] = OpCodesEmit.Shr;
            opCodesMap[Code.Shr_Un] = OpCodesEmit.Shr_Un;
            opCodesMap[Code.Neg] = OpCodesEmit.Neg;
            opCodesMap[Code.Not] = OpCodesEmit.Not;
            opCodesMap[Code.Conv_I1] = OpCodesEmit.Conv_I1;
            opCodesMap[Code.Conv_I2] = OpCodesEmit.Conv_I2;
            opCodesMap[Code.Conv_I4] = OpCodesEmit.Conv_I4;
            opCodesMap[Code.Conv_I8] = OpCodesEmit.Conv_I8;
            opCodesMap[Code.Conv_R4] = OpCodesEmit.Conv_R4;
            opCodesMap[Code.Conv_R8] = OpCodesEmit.Conv_R8;
            opCodesMap[Code.Conv_U4] = OpCodesEmit.Conv_U4;
            opCodesMap[Code.Conv_U8] = OpCodesEmit.Conv_U8;
            opCodesMap[Code.Callvirt] = OpCodesEmit.Callvirt;
            opCodesMap[Code.Cpobj] = OpCodesEmit.Cpobj;
            opCodesMap[Code.Ldobj] = OpCodesEmit.Ldobj;
            opCodesMap[Code.Ldstr] = OpCodesEmit.Ldstr;
            opCodesMap[Code.Newobj] = OpCodesEmit.Newobj;
            opCodesMap[Code.Castclass] = OpCodesEmit.Castclass;
            opCodesMap[Code.Isinst] = OpCodesEmit.Isinst;
            opCodesMap[Code.Conv_R_Un] = OpCodesEmit.Conv_R_Un;
            opCodesMap[Code.Unbox] = OpCodesEmit.Unbox;
            opCodesMap[Code.Throw] = OpCodesEmit.Throw;
            opCodesMap[Code.Ldfld] = OpCodesEmit.Ldfld;
            opCodesMap[Code.Ldflda] = OpCodesEmit.Ldflda;
            opCodesMap[Code.Stfld] = OpCodesEmit.Stfld;
            opCodesMap[Code.Ldsfld] = OpCodesEmit.Ldsfld;
            opCodesMap[Code.Ldsflda] = OpCodesEmit.Ldsflda;
            opCodesMap[Code.Stsfld] = OpCodesEmit.Stsfld;
            opCodesMap[Code.Stobj] = OpCodesEmit.Stobj;
            opCodesMap[Code.Conv_Ovf_I1_Un] = OpCodesEmit.Conv_Ovf_I1_Un;
            opCodesMap[Code.Conv_Ovf_I2_Un] = OpCodesEmit.Conv_Ovf_I2_Un;
            opCodesMap[Code.Conv_Ovf_I4_Un] = OpCodesEmit.Conv_Ovf_I4_Un;
            opCodesMap[Code.Conv_Ovf_I8_Un] = OpCodesEmit.Conv_Ovf_I8_Un;
            opCodesMap[Code.Conv_Ovf_U1_Un] = OpCodesEmit.Conv_Ovf_U1_Un;
            opCodesMap[Code.Conv_Ovf_U2_Un] = OpCodesEmit.Conv_Ovf_U2_Un;
            opCodesMap[Code.Conv_Ovf_U4_Un] = OpCodesEmit.Conv_Ovf_U4_Un;
            opCodesMap[Code.Conv_Ovf_U8_Un] = OpCodesEmit.Conv_Ovf_U8_Un;
            opCodesMap[Code.Conv_Ovf_I_Un] = OpCodesEmit.Conv_Ovf_I_Un;
            opCodesMap[Code.Conv_Ovf_U_Un] = OpCodesEmit.Conv_Ovf_U_Un;
            opCodesMap[Code.Box] = OpCodesEmit.Box;
            opCodesMap[Code.Newarr] = OpCodesEmit.Newarr;
            opCodesMap[Code.Ldlen] = OpCodesEmit.Ldlen;
            opCodesMap[Code.Ldelema] = OpCodesEmit.Ldelema;
            opCodesMap[Code.Ldelem_I1] = OpCodesEmit.Ldelem_I1;
            opCodesMap[Code.Ldelem_U1] = OpCodesEmit.Ldelem_U1;
            opCodesMap[Code.Ldelem_I2] = OpCodesEmit.Ldelem_I2;
            opCodesMap[Code.Ldelem_U2] = OpCodesEmit.Ldelem_U2;
            opCodesMap[Code.Ldelem_I4] = OpCodesEmit.Ldelem_I4;
            opCodesMap[Code.Ldelem_U4] = OpCodesEmit.Ldelem_U4;
            opCodesMap[Code.Ldelem_I8] = OpCodesEmit.Ldelem_I8;
            opCodesMap[Code.Ldelem_I] = OpCodesEmit.Ldelem_I;
            opCodesMap[Code.Ldelem_R4] = OpCodesEmit.Ldelem_R4;
            opCodesMap[Code.Ldelem_R8] = OpCodesEmit.Ldelem_R8;
            opCodesMap[Code.Ldelem_Ref] = OpCodesEmit.Ldelem_Ref;
            opCodesMap[Code.Stelem_I] = OpCodesEmit.Stelem_I;
            opCodesMap[Code.Stelem_I1] = OpCodesEmit.Stelem_I1;
            opCodesMap[Code.Stelem_I2] = OpCodesEmit.Stelem_I2;
            opCodesMap[Code.Stelem_I4] = OpCodesEmit.Stelem_I4;
            opCodesMap[Code.Stelem_I8] = OpCodesEmit.Stelem_I8;
            opCodesMap[Code.Stelem_R4] = OpCodesEmit.Stelem_R4;
            opCodesMap[Code.Stelem_R8] = OpCodesEmit.Stelem_R8;
            opCodesMap[Code.Stelem_Ref] = OpCodesEmit.Stelem_Ref;
            opCodesMap[Code.Ldelem] = OpCodesEmit.Ldelem;
            opCodesMap[Code.Stelem] = OpCodesEmit.Stelem;
            opCodesMap[Code.Unbox_Any] = OpCodesEmit.Unbox_Any;
            opCodesMap[Code.Conv_Ovf_I1] = OpCodesEmit.Conv_Ovf_I1;
            opCodesMap[Code.Conv_Ovf_U1] = OpCodesEmit.Conv_Ovf_U1;
            opCodesMap[Code.Conv_Ovf_I2] = OpCodesEmit.Conv_Ovf_I2;
            opCodesMap[Code.Conv_Ovf_U2] = OpCodesEmit.Conv_Ovf_U2;
            opCodesMap[Code.Conv_Ovf_I4] = OpCodesEmit.Conv_Ovf_I4;
            opCodesMap[Code.Conv_Ovf_U4] = OpCodesEmit.Conv_Ovf_U4;
            opCodesMap[Code.Conv_Ovf_I8] = OpCodesEmit.Conv_Ovf_I8;
            opCodesMap[Code.Conv_Ovf_U8] = OpCodesEmit.Conv_Ovf_U8;
            opCodesMap[Code.Refanyval] = OpCodesEmit.Refanyval;
            opCodesMap[Code.Ckfinite] = OpCodesEmit.Ckfinite;
            opCodesMap[Code.Mkrefany] = OpCodesEmit.Mkrefany;
            opCodesMap[Code.Ldtoken] = OpCodesEmit.Ldtoken;
            opCodesMap[Code.Conv_U2] = OpCodesEmit.Conv_U2;
            opCodesMap[Code.Conv_U1] = OpCodesEmit.Conv_U1;
            opCodesMap[Code.Conv_I] = OpCodesEmit.Conv_I;
            opCodesMap[Code.Conv_Ovf_I] = OpCodesEmit.Conv_Ovf_I;
            opCodesMap[Code.Conv_Ovf_U] = OpCodesEmit.Conv_Ovf_U;
            opCodesMap[Code.Add_Ovf] = OpCodesEmit.Add_Ovf;
            opCodesMap[Code.Add_Ovf_Un] = OpCodesEmit.Add_Ovf_Un;
            opCodesMap[Code.Mul_Ovf] = OpCodesEmit.Mul_Ovf;
            opCodesMap[Code.Mul_Ovf_Un] = OpCodesEmit.Mul_Ovf_Un;
            opCodesMap[Code.Sub_Ovf] = OpCodesEmit.Sub_Ovf;
            opCodesMap[Code.Sub_Ovf_Un] = OpCodesEmit.Sub_Ovf_Un;
            opCodesMap[Code.Endfinally] = OpCodesEmit.Endfinally;
            opCodesMap[Code.Leave] = OpCodesEmit.Leave;
            opCodesMap[Code.Leave_S] = OpCodesEmit.Leave_S;
            opCodesMap[Code.Stind_I] = OpCodesEmit.Stind_I;
            opCodesMap[Code.Conv_U] = OpCodesEmit.Conv_U;
            opCodesMap[Code.Arglist] = OpCodesEmit.Arglist;
            opCodesMap[Code.Ceq] = OpCodesEmit.Ceq;
            opCodesMap[Code.Cgt] = OpCodesEmit.Cgt;
            opCodesMap[Code.Cgt_Un] = OpCodesEmit.Cgt_Un;
            opCodesMap[Code.Clt] = OpCodesEmit.Clt;
            opCodesMap[Code.Clt_Un] = OpCodesEmit.Clt_Un;
            opCodesMap[Code.Ldftn] = OpCodesEmit.Ldftn;
            opCodesMap[Code.Ldvirtftn] = OpCodesEmit.Ldvirtftn;
            opCodesMap[Code.Ldarg] = OpCodesEmit.Ldarg;
            opCodesMap[Code.Ldarga] = OpCodesEmit.Ldarga;
            opCodesMap[Code.Starg] = OpCodesEmit.Starg;
            opCodesMap[Code.Ldloc] = OpCodesEmit.Ldloc;
            opCodesMap[Code.Ldloca] = OpCodesEmit.Ldloca;
            opCodesMap[Code.Stloc] = OpCodesEmit.Stloc;
            opCodesMap[Code.Localloc] = OpCodesEmit.Localloc;
            opCodesMap[Code.Endfilter] = OpCodesEmit.Endfilter;
            opCodesMap[Code.Unaligned] = OpCodesEmit.Unaligned;
            opCodesMap[Code.Volatile] = OpCodesEmit.Volatile;
            opCodesMap[Code.Tail] = OpCodesEmit.Tailcall;
            opCodesMap[Code.Initobj] = OpCodesEmit.Initobj;
            opCodesMap[Code.Constrained] = OpCodesEmit.Constrained;
            opCodesMap[Code.Cpblk] = OpCodesEmit.Cpblk;
            opCodesMap[Code.Initblk] = OpCodesEmit.Initblk;

            // OpCodesMap[Code.No] = OpCodesEmit.No;
            opCodesMap[Code.Rethrow] = OpCodesEmit.Rethrow;
            opCodesMap[Code.Sizeof] = OpCodesEmit.Sizeof;
            opCodesMap[Code.Refanytype] = OpCodesEmit.Refanytype;
            opCodesMap[Code.Readonly] = OpCodesEmit.Readonly;
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
        public IlReader(string source, string[] args)
            : this()
        {
            this.Source = source;

            var coreLibPathArg = args != null ? args.FirstOrDefault(a => a.StartsWith("corelib:")) : null;
            this.CoreLibPath = coreLibPathArg != null ? coreLibPathArg.Substring("corelib:".Length) : null;
            this.UsingRoslyn = args != null && args.Any(a => a == "roslyn");
            this.DefaultDllLocations = this.Source.EndsWith(".dll") ? Path.GetDirectoryName(Path.GetFullPath(this.Source)) : null;
        }

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

        public bool IsCoreLib
        {
            get
            {
                return !this.Assembly.Assembly.AssemblyReferences.Any();
            }
        }

        /// <summary>
        /// </summary>
        public string CoreLibPath { get; set; }

        /// <summary>
        /// </summary>
        public string ModuleName
        {
            get
            {
                return this.Assembly.ManifestModule.Name;
            }
        }

        /// <summary>
        /// </summary>
        public HashSet<IMethod> UsedGenericSpecialiazedMethods
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
        public HashSet<IType> UsedGenericSpecialiazedTypes
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
        public HashSet<IType> UsedStructTypes
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
        public HashSet<IType> UsedTypes
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
        public HashSet<IField> UsedStaticFieldsToRead
        {
            get
            {
                return this.usedStaticFieldsToRead;
            }
        }

        /// <summary>
        /// </summary>
        public HashSet<IMethod> CalledMethods
        {
            get
            {
                return this.calledMethods;
            }
        }

        /// <summary>
        /// </summary>
        public bool UsingRoslyn { get; set; }

        public string DefaultDllLocations { get; private set; }

        /// <summary>
        /// </summary>
        protected AssemblyMetadata Assembly { get; private set; }

        /// <summary>
        /// </summary>
        protected string Source { get; private set; }

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

        public static IEnumerable<IMethod> MethodsOriginal(IType type)
        {
            return type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IMethod> Methods(IType type)
        {
            foreach (
                var method in
                    type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                        .Where(m => !m.IsGenericMethodDefinition))
            {
                yield return method;
            }

            // append specialized methods
            IEnumerable<IMethod> genMethodSpecializationForType = null;
            if (!genMethodSpec.TryGetValue(type, out genMethodSpecializationForType))
            {
                yield break;
            }

            foreach (
                var method in
                    genMethodSpecializationForType)
            {
                yield return method;
            }
        }

        /// <summary>
        /// </summary>
        public void Load()
        {
            this.Assembly = this.Source.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase)
                                ? this.UsingRoslyn ? this.CompileWithRoslyn(this.Source) : this.Compile(this.Source)
                                : AssemblyMetadata.CreateFromImageStream(new FileStream(this.Source, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void Load(Type type)
        {
            this.Assembly = AssemblyMetadata.CreateFromImageStream(new FileStream(type.Module.Assembly.Location, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IConstructor ctor, IGenericContext genericContext)
        {
            if (ctor == null)
            {
                yield break;
            }

            foreach (var opCode in this.OpCodes(ctor.GetMethodBody(), ctor.Module, genericContext))
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
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IMethod method, IGenericContext genericContext)
        {
            if (method == null)
            {
                yield break;
            }

            foreach (var opCode in this.OpCodes(method.GetMethodBody(), method.Module, genericContext))
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
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IMethodBody methodBody, IModule module, IGenericContext genericContext)
        {
            if (methodBody == null)
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

                var opCode = opCodesMap[code];

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

                        AddUsedType(constructor.DeclaringType);
                        AddCalledMethod(constructor);

                        yield return new OpCodeConstructorInfoPart(opCode, startAddress, currentAddress, constructor);
                        continue;
                    case Code.Call:
                    case Code.Callvirt:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var method = module.ResolveMethod(token, genericContext);
                        this.AddGenericSpecializedType(method.DeclaringType);
                        this.AddGenericSpecializedMethod(method);
                        foreach (var methodParameter in method.GetParameters())
                        {
                            this.AddStructType(methodParameter.ParameterType);
                        }

                        AddUsedType(method.DeclaringType);
                        AddCalledMethod(method);

                        yield return new OpCodeMethodInfoPart(opCode, startAddress, currentAddress, method);
                        continue;

                    case Code.Ldftn:
                    case Code.Ldvirtftn:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        method = module.ResolveMethod(token, genericContext);
                        this.AddGenericSpecializedType(method.DeclaringType);
                        this.AddGenericSpecializedMethod(method);

                        AddUsedType(method.DeclaringType);

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
                            AddUsedType(typeToken);

                            yield return new OpCodeTypePart(opCode, startAddress, currentAddress, typeToken);
                            continue;
                        }

                        var fieldMember = resolvedToken as IField;
                        if (fieldMember != null)
                        {
                            AddUsedType(fieldMember.DeclaringType);

                            yield return new OpCodeFieldInfoPart(opCode, startAddress, currentAddress, fieldMember);
                            continue;
                        }

                        var methodMember = resolvedToken as IMethod;
                        if (methodMember != null)
                        {
                            AddUsedType(methodMember.DeclaringType);

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

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var type = module.ResolveType(token, genericContext);
                        this.AddGenericSpecializedType(type);
                        if (code == Code.Box)
                        {
                            this.AddStructType(type);
                        }

                        AddUsedType(type);

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

        public IEnumerable<IType> AllTypes()
        {
            return this.lazyAllTypes.Value;
        }

        public IEnumerable<IType> AllReferencedTypes()
        {
            return this.lazyAllReferencedTypes.Value;
        }

        public IEnumerable<string> AllReferences()
        {
            return this.AllReferencesHelper(this.Assembly);
        }

        public IEnumerable<string> AllReferencesHelper(AssemblyMetadata assemblyMetadata)
        {
            yield return assemblyMetadata.Assembly.Identity.Name;

            foreach (var reference in this.LoadReferences(assemblyMetadata).Names)
            {
                foreach (var referenceName in this.AllReferencesHelper(GetAssemblyMetadata(reference)))
                {
                    yield return referenceName;
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
            foreach (var namespaceSymbolSub in source.GetNamespaceMembers().SelectMany(namespaceSymbolSub => GetAllNamespaces(namespaceSymbolSub)))
            {
                yield return namespaceSymbolSub;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <param name="map">
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
        private void AddGenericSpecializedMethod(IMethod method)
        {
            if (this.usedGenericSpecialiazedMethods == null || method == null || !method.IsGenericMethod)
            {
                return;
            }

            this.usedGenericSpecialiazedMethods.Add(method);

            // add all generic types in parameters
            foreach (var parameter in method.GetParameters())
            {
                AddGenericSpecializedType(parameter.ParameterType);
            }

            // disover it again in specialized method
            method.DiscoverRequiredTypesAndMethods(usedGenericSpecialiazedTypes, usedGenericSpecialiazedMethods, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void AddGenericSpecializedType(IType type)
        {
            if (this.usedGenericSpecialiazedTypes == null || type == null || type.IsGenericTypeDefinition || !type.IsGenericType)
            {
                return;
            }

            this.usedGenericSpecialiazedTypes.Add(type);
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

        private void AddUsedStaticFieldToRead(IField field)
        {
            if (field == null || !field.IsStatic)
            {
                return;
            }

            this.usedStaticFieldsToRead.Add(field);
        }

        private void AddCalledMethod(IMethod method)
        {
            if (method == null)
            {
                return;
            }

            this.calledMethods.Add(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private AssemblyMetadata Compile(string source)
        {
            var codeProvider = new CSharpCodeProvider();
            var icc = codeProvider.CreateCompiler();
            var outDll = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".dll");

            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = false;
            parameters.CompilerOptions = string.Concat(
                "/optimize+ /unsafe+", string.IsNullOrWhiteSpace(this.CoreLibPath) ? string.Empty : string.Format(" /nostdlib+ /r:\"{0}\"", this.CoreLibPath));
            parameters.OutputAssembly = outDll;

            // parameters.CompilerOptions = "/optimize-";
            var results = icc.CompileAssemblyFromFile(parameters, source);

            if (results.Errors.Count > 0)
            {
                foreach (CompilerError compilerError in results.Errors)
                {
                    throw new Exception(compilerError.ErrorText);
                }
            }

            // Successful Compile
            return AssemblyMetadata.CreateFromImageStream(new FileStream(results.PathToAssembly, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblyMetadata CompileWithRoslyn(string source)
        {
            var baseName = Path.GetRandomFileName();
            var nameDll = baseName + ".dll";
            var namePdb = baseName + ".pdb";
            var outDll = Path.Combine(Path.GetTempPath(), nameDll);
            var outPdb = Path.Combine(Path.GetTempPath(), namePdb);

            var syntaxTree = CSharpSyntaxTree.ParseText(new StreamReader(source).ReadToEnd());

            var coreLibRefAssembly = string.IsNullOrWhiteSpace(this.CoreLibPath)
                                         ? new MetadataImageReference(new FileStream(typeof(int).Assembly.Location, FileMode.Open, FileAccess.Read))
                                         : new MetadataImageReference(new FileStream(this.CoreLibPath, FileMode.Open, FileAccess.Read));

            var options =
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithAllowUnsafe(true)
                                                                                 .WithOptimizations(true)
                                                                                 .WithRuntimeMetadataVersion("4.5");

            var compilation = CSharpCompilation.Create(nameDll, new[] { syntaxTree }, new[] { coreLibRefAssembly }, options);

            using (var dllStream = new FileStream(outDll, FileMode.OpenOrCreate))
            using (var pdbStream = new FileStream(outPdb, FileMode.OpenOrCreate))
            {
                var result = compilation.Emit(peStream: dllStream, pdbFilePath: outPdb, pdbStream: pdbStream);
                foreach (var diagnostic in result.Diagnostics)
                {
                    System.Diagnostics.Trace.WriteLine(diagnostic);
                }
            }

            // Successful Compile
            return AssemblyMetadata.CreateFromImageStream(new FileStream(outDll, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private IEnumerable<IType> ReadTypes(bool readAll = false, bool ignoreCurrent = false)
        {
            var assemblySymbol = this.LoadAssemblySymbol(this.Assembly);

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

        private IEnumerable<IType> EnumAllTypes(PEAssemblySymbol assemblySymbol)
        {
            Debug.Assert(assemblySymbol != null, "missing assembly");

            foreach (var module in assemblySymbol.Modules)
            {
                var peModuleSymbol = module as PEModuleSymbol;
                foreach (var metadataTypeAdapter in from symbol in GetAllNamespaces(peModuleSymbol.GlobalNamespace).SelectMany(n => n.GetTypeMembers())
                                                    select new MetadataTypeAdapter(symbol))
                {
                    yield return metadataTypeAdapter;
                    foreach (var nestedType in EnumAllNestedTypes(metadataTypeAdapter))
                    {
                        yield return nestedType;
                    }
                }
            }
        }

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

        private ModuleReferences<AssemblySymbol> LoadReferences(AssemblyMetadata assemblyMetadata)
        {
            var peReferences = ImmutableArray.CreateRange(assemblyMetadata.Assembly.AssemblyReferences.Select(this.LoadAssemblySymbolOrMissingAssemblySymbol));

            var moduleReferences = new ModuleReferences<AssemblySymbol>(
                assemblyMetadata.Assembly.AssemblyReferences,
                peReferences,
                ImmutableArray.CreateRange(this.unifiedAssemblies));

            return moduleReferences;
        }

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

        private static PEAssemblySymbol GetAssemblySymbol(AssemblyMetadata assemblyMetadata)
        {
            return new PEAssemblySymbol(assemblyMetadata.Assembly, DocumentationProvider.Default, isLinked: false, importOptions: MetadataImportOptions.All);
        }

        private AssemblyMetadata GetAssemblyMetadata(AssemblyIdentity assemblyIdentity)
        {
            var resolveReferencePath = this.ResolveReferencePath(assemblyIdentity);
            if (string.IsNullOrWhiteSpace(resolveReferencePath))
            {
                return null;
            }

            return AssemblyMetadata.CreateFromImageStream(new FileStream(resolveReferencePath, FileMode.Open, FileAccess.Read));
        }

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
    }
}
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
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using Microsoft.CSharp;
    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    public class IlReader
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static IDictionary<Code, OpCode> OpCodesMap = new SortedDictionary<Code, OpCode>();

        #endregion

        #region Constructors and Destructors

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
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        public IlReader(string source)
        {
            this.Source = source;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public string ModuleName
        {
            get
            {
                return this.Assembly.ManifestModule.Name;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        protected AssemblyMetadata Assembly { get; private set; }

        /// <summary>
        /// </summary>
        protected string Source { get; private set; }

        #endregion

        #region Public Methods and Operators

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
        public void Load()
        {
            this.Assembly = this.Source.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase) ? Compile(this.Source) : AssemblyMetadata.CreateFromFile(this.Source);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void Load(Type type)
        {
            this.Assembly = AssemblyMetadata.CreateFromFile(type.Module.Assembly.Location);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IEnumerable<IMethod> Methods(IType type)
        {
            return type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IConstructor ctor, IType[] typeGenerics, IType[] methodGenerics)
        {
            if (ctor == null)
            {
                yield break;
            }

            foreach (var opCode in this.OpCodes(ctor.GetMethodBody(), ctor.Module, typeGenerics, methodGenerics))
            {
                yield return opCode;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IMethod method, IType[] typeGenerics, IType[] methodGenerics)
        {
            if (method == null)
            {
                yield break;
            }

            foreach (var opCode in this.OpCodes(method.GetMethodBody(), method.Module, typeGenerics, methodGenerics))
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
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> OpCodes(IMethodBody methodBody, IModule module, IType[] typeGenerics, IType[] methodGenerics)
        {
            if (methodBody == null)
            {
                yield break;
            }

            var extended = false;
            int startAddress = 0;
            int currentAddress = 0;
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
                        var constructor = module.ResolveMember(token, typeGenerics, methodGenerics) as IConstructor;
                        yield return new OpCodeConstructorInfoPart(opCode, startAddress, currentAddress, constructor);
                        continue;
                    case Code.Call:
                    case Code.Callvirt:
                    case Code.Ldftn:
                    case Code.Ldvirtftn:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var member = module.ResolveMethod(token, typeGenerics, methodGenerics);
                        yield return new OpCodeMethodInfoPart(opCode, startAddress, currentAddress, member);
                        continue;
                    case Code.Stfld:
                    case Code.Stsfld:
                    case Code.Ldfld:
                    case Code.Ldflda:
                    case Code.Ldsfld:
                    case Code.Ldsflda:

                        // read token, next 
                        token = ReadInt32(enumerator, ref currentAddress);
                        var field = module.ResolveField(token, typeGenerics, methodGenerics);
                        yield return new OpCodeFieldInfoPart(opCode, startAddress, currentAddress, field);
                        continue;
                    case Code.Ldtoken: // can it be anything?
                        token = ReadInt32(enumerator, ref currentAddress);
                        @int32 = token;
                        yield return new OpCodeInt32Part(opCode, startAddress, currentAddress, @int32);
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
                        var type = module.ResolveType(token, typeGenerics, methodGenerics);
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

        private static AssemblySymbol MapAssemblyIdentityToResolvedSymbol(AssemblyIdentity identity, Dictionary<AssemblyIdentity, AssemblySymbol> map)
        {
            AssemblySymbol symbol;
            return map.TryGetValue(identity, out symbol) ? symbol : new MissingAssemblySymbol(identity);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<IType> Types()
        {
            var assemblySymbol = new PEAssemblySymbol(this.Assembly.Assembly, DocumentationProvider.Default, isLinked: false, importOptions: MetadataImportOptions.All);

            // TODO: find mscorlib
            // 1) set corelib
            bool coreLibSet = false;
            var referencedAssembliesByIdentity = new Dictionary<AssemblyIdentity, AssemblySymbol>();
            var unifiedAssemblies = new List<UnifiedAssembly<AssemblySymbol>>();
            foreach (var assemblyIdentity in this.Assembly.Assembly.AssemblyReferences)
            {
                if (assemblyIdentity.Name == "mscorlib")
                {
                    var coreAssembly = AssemblyMetadata.CreateFromFile(typeof(int).Assembly.Location);
                    var coreAssemblySymbol = new PEAssemblySymbol(coreAssembly.Assembly, DocumentationProvider.Default, isLinked: false, importOptions: MetadataImportOptions.All);
                    coreAssemblySymbol.SetCorLibrary(coreAssemblySymbol);

                    assemblySymbol.SetCorLibrary(coreAssemblySymbol);
                    
                    referencedAssembliesByIdentity[coreAssemblySymbol.Identity] = coreAssemblySymbol;
                    unifiedAssemblies.Add(new UnifiedAssembly<AssemblySymbol>(coreAssemblySymbol, coreAssemblySymbol.Identity));
                    coreLibSet = true;
                    continue;
                }

                // TODO: finish it, loading Assebly (find it by DLL name etc)
            }

            if (!coreLibSet)
            {
                assemblySymbol.SetCorLibrary(assemblySymbol);
            }

            // 2) set references
            var peReferences = this.Assembly.Assembly.AssemblyReferences.SelectAsArray(MapAssemblyIdentityToResolvedSymbol, referencedAssembliesByIdentity);
            var moduleReferences = new ModuleReferences<AssemblySymbol>(
                this.Assembly.Assembly.AssemblyReferences, peReferences, ImmutableArray.CreateRange(unifiedAssemblies));
            assemblySymbol.Modules[0].SetReferences(moduleReferences);

            // 3) Load Types
            foreach (var module in assemblySymbol.Modules)
            {
                var typeWithNamespaces = module.TypeWithNamespaceNames.ToArray();
                foreach (var typeWithNamespace in typeWithNamespaces)
                {
                    var metadataTypeName = MetadataTypeName.FromNamespaceAndTypeName(typeWithNamespace.Value, typeWithNamespace.Key);
                    var symbol = module.LookupTopLevelMetadataType(ref metadataTypeName);
                    yield return new MetadataTypeAdapter(symbol as TypeSymbol);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        private static AssemblyMetadata Compile(string source)
        {
            var codeProvider = new CSharpCodeProvider();
            var icc = codeProvider.CreateCompiler();
            var outDll = Path.Combine(Path.GetTempPath(), Path.GetTempFileName() + ".dll");

            var parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = false;
            parameters.CompilerOptions = "/optimize+ /unsafe+";

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
            return AssemblyMetadata.CreateFromFile(results.PathToAssembly);
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
                return (int)(byte)source.Current;
            }

            throw new InvalidOperationException("Could not read a short for of int32");
        }

        #endregion
    }
}
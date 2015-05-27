// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LlvmWriter.cs" company="">
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.DebugInfo;
    using Il2Native.Logic.Exceptions;
    using Il2Native.Logic.Gencode;
    using Il2Native.Logic.Gencode.InlineMethods;
    using Il2Native.Logic.Gencode.SynthesizedMethods;
    using Il2Native.Logic.Properties;
    using Microsoft.Win32;
    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class CWriter : BaseWriter, ICodeWriter
    {
        /// <summary>
        /// </summary>
        public static int PointerSize = 4;

        /// <summary>
        /// </summary>
        public static string VTable = "__vtbl";

        /// <summary>
        /// </summary>
        public Stack<CatchOfFinallyClause> catchScopes = new Stack<CatchOfFinallyClause>();

        /// <summary>
        /// append notSpecialization
        /// </summary>
        public string declarationPrefix = "static ";

        /// <summary>
        /// </summary>
        public bool landingPadVariablesAreWritten;

        /// <summary>
        /// </summary>
        public readonly ISet<IMethod> methodsHaveDefinition = new NamespaceContainer<IMethod>();

        /// <summary>
        /// </summary>
        public bool needToWriteUnreachable;

        /// <summary>
        /// </summary>
        public bool needToWriteUnwindException;

        /// <summary>
        /// </summary>
        public Stack<TryClause> tryScopes = new Stack<TryClause>();

        /// <summary>
        /// </summary>
        public readonly ISet<int> stringTokenDefinitionWritten = new HashSet<int>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> indexByFieldInfo = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<int, IMethod> methodsByToken = new SortedDictionary<int, IMethod>();

        /// <summary>
        /// </summary>
        private string outputFile;

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> poisitionByFieldInfo = new SortedDictionary<string, int>();

        public DebugInfoGenerator debugInfoGenerator;

        private IList<IMethod> externDeclarations = new List<IMethod>();

        /// <summary>
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <param name="args">
        /// </param>
        public CWriter(string fileName, string fileExt, string sourceFilePath, string pdbFilePath, string[] args)
        {
            this.SetSettings(fileName, fileExt, sourceFilePath, pdbFilePath, args);
        }

        /// <summary>
        /// </summary>
        public bool IsHeader { get; set; }

        /// <summary>
        /// </summary>
        public bool IsSplit { get; set; }

        /// <summary>
        /// </summary>
        public string SplitNamespace { get; set; }

        /// <summary>
        /// </summary>
        public string FileHeader { get; set; }

        /// <summary>
        /// </summary>
        public IEnumerable<string> AllReferences
        {
            get
            {
                return this.IlReader.AllReferences();
            }
        }

        /// <summary>
        /// </summary>
        public bool DebugInfo { get; private set; }

        /// <summary>
        /// </summary>
        public bool DebugInfoNoLineGenerating { get; private set; }

        /// <summary>
        /// </summary>
        public bool Gc { get; private set; }

        /// <summary>
        /// </summary>
        public bool Unsafe { get; private set; }

        /// <summary>
        /// </summary>
        public bool GcDebug { get; private set; }

        /// <summary>
        /// </summary>
        public bool Gctors { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsCoreLib
        {
            get
            {
                return this.IlReader.IsCoreLib;
            }
        }

        public IDictionary<int, IMethod> MethodsByToken
        {
            get
            {
                return this.methodsByToken;
            }
        }

        /// <summary>
        /// </summary>
        public CIndentedTextWriter Output { get; private set; }

        /// <summary>
        /// </summary>
        public bool Stubs { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="currentType">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <param name="nextCurrentType">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public static int FindInterfaceIndexForOneStep(IType currentType, IType @interface, out IType nextCurrentType)
        {
            nextCurrentType = currentType;
            var found = false;
            var interfaceIndex = -1;
            foreach (var subInterface in currentType.GetInterfaces().ToList())
            {
                interfaceIndex++;

                if (subInterface.TypeEquals(@interface))
                {
                    nextCurrentType = null;
                    found = true;
                    break;
                }

                if (subInterface.GetAllInterfaces().Contains(@interface))
                {
                    nextCurrentType = subInterface;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new KeyNotFoundException("type can't be found");
            }

            return interfaceIndex;
        }

        public static IType FindInterfacePathForOneStep(IType currentType, IType @interface, out IType nextCurrentType)
        {
            nextCurrentType = currentType;
            var found = false;
            IType interfacePath = null;
            foreach (var subInterface in currentType.GetInterfaces().ToList())
            {
                interfacePath = subInterface;

                if (subInterface.TypeEquals(@interface))
                {
                    nextCurrentType = null;
                    found = true;
                    break;
                }

                if (subInterface.GetAllInterfaces().Contains(@interface))
                {
                    nextCurrentType = subInterface;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new KeyNotFoundException("type can't be found");
            }

            return interfacePath;
        }

        /// <summary>
        ///     if true - suppress ; at the end of line
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="firstLevel">
        /// </param>
        public void ActualWrite(CIndentedTextWriter writer, OpCodePart opCode, bool firstLevel = false)
        {
            if (opCode.Result != null)
            {
                return;
            }

            var processAsSeparateStatement = this.ProcessAsSeparateStatement(opCode);

            this.WriteTryBegins(writer, opCode);
            this.WriteCatchBegins(opCode);

            if (firstLevel && !processAsSeparateStatement)
            {
                return;
            }

            if (opCode.Result != null)
            {
                this.WriteTemporaryExpressionResult(opCode);
            }

            var processed = false;
            if (opCode.UsedByAlternativeValues != null)
            {
                processed = this.WriteStartOfPhiValues(writer, opCode, firstLevel);
            }

            if (!processed)
            {
                this.ActualWriteOpCode(writer, opCode);
            }

            if (firstLevel && opCode.UsedByAlternativeValues != null)
            {
                this.WriteEndOfPhiValues(writer, opCode);
            }

            if (firstLevel)
            {
                this.Output.Write(";");
            }

            this.WriteCatchFinallyEnd(writer, opCode);
            this.WriteCatchFinnallyCleanUpEnd(opCode);
            this.WriteTryEnds(opCode);
            this.WriteExceptionHandlersProlog(opCode);

            if (firstLevel)
            {
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// Temporaryly disabled as it not pointing to correct position yet.
        /// </summary>
        private void WriteDebugLine()
        {
            if (!this.debugInfoGenerator.CurrentDebugLine.HasValue)
            {
                return;
            }

            if (this.debugInfoGenerator.SourceFilePathChanged)
            {
                if (string.IsNullOrEmpty(this.debugInfoGenerator.SourceFilePath))
                {
                    Debug.Assert(false, "Source file is not provided");
                    return;
                }

                this.Output.WriteLine("#line {0} \"{1}\"", this.debugInfoGenerator.CurrentDebugLine, this.debugInfoGenerator.SourceFilePath);

                this.debugInfoGenerator.SourceFilePathChanged = false;
            }
            else
            {
                this.Output.WriteLine("#line {0}", this.debugInfoGenerator.CurrentDebugLine);
            }

            this.debugInfoGenerator.CurrentDebugLineNew = false;
        }

        private void WriteTemporaryExpressionResult(OpCodePart opCode)
        {
            opCode.Result.Type.WriteTypePrefix(this);
            this.Output.Write(" ");
            WriteResult(opCode.Result);
            this.Output.WriteLine(";");
            WriteResult(opCode.Result);
            this.Output.Write(" = ");
        }

        private bool ProcessAsSeparateStatement(OpCodePart opCode)
        {
            if (opCode.UsedBy == null)
            {
                return true;
            }

            if (opCode.UsedByAlternativeValues != null)
            {
                return true;
            }

            bool isVirtualCall;
            if (this.OpCodeWithVariableDeclaration(opCode, out isVirtualCall))
            {
                if (isVirtualCall)
                {
                    var methodDeclarationType = this.EstimatedResultOf(opCode).Type;
                    if (methodDeclarationType.IsInterface)
                    {
                        var address = opCode.AddressStart;
                        if (address == 0)
                        {
                            address = opCode.GetHashCode();
                            if (opCode.UsedBy != null)
                            {
                                address += opCode.UsedBy.OpCode.AddressStart;
                            }

                            if (opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0)
                            {
                                address += opCode.OpCodeOperands[0].AddressStart;
                            }
                        }

                        opCode.Result = new FullyDefinedReference(string.Concat("__expr", address), methodDeclarationType);
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private bool OpCodeWithVariableDeclaration(OpCodePart opCode, out bool isVirtualCall)
        {
            isVirtualCall = false;

            switch (opCode.ToCode())
            {
                case Code.Dup:
                case Code.Newobj:
                case Code.Newarr:
                case Code.Ldftn:
                case Code.Ldvirtftn:
                case Code.Localloc:
                case Code.Mkrefany:
                case Code.Constrained:
                case Code.Refanytype:
                    return true;
                case Code.Ldtoken:
                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    return opCodeFieldInfoPart != null && opCodeFieldInfoPart.Operand.FieldType != null &&
                           (opCodeFieldInfoPart.Operand.FieldType.IsStaticArrayInit ||
                            opCodeFieldInfoPart.Operand.GetFieldRVAData() != null);
                case Code.Call:
                    var opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;
                    return ActivatorGen.IsActivatorFunction(opCodeMethodInfoPart.Operand);
            }

            if (opCode.UsedByAlternativeValues == null && IsVirtualCallThisExpression(opCode))
            {
                isVirtualCall = true;
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public void ActualWriteOpCode(CIndentedTextWriter writer, OpCodePart opCode)
        {
            var code = opCode.ToCode();
            var firstOpCodeOperand = opCode != null && opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0 ? opCode.OpCodeOperands[0] : null;
            switch (code)
            {
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                    var asString = code.ToString();
                    this.Output.Write(asString.Substring(asString.Length - 1, 1));
                    break;
                case Code.Ldc_I4_M1:
                    this.Output.Write("-1");
                    break;
                case Code.Ldc_I4:
                    var opCodeInt32 = opCode as OpCodeInt32Part;
                    if (opCodeInt32.Operand > int.MaxValue || opCodeInt32.Operand < int.MinValue)
                    {
                        this.Output.Write(opCodeInt32.Operand);
                        this.Output.Write("L");
                    }
                    else
                    {
                        this.Output.Write(opCodeInt32.Operand);
                    }

                    break;
                case Code.Ldc_I4_S:
                    opCodeInt32 = opCode as OpCodeInt32Part;
                    this.Output.Write(opCodeInt32.Operand > 127 ? -(256 - opCodeInt32.Operand) : opCodeInt32.Operand);
                    break;
                case Code.Ldc_I8:
                    var opCodeInt64 = opCode as OpCodeInt64Part;
                    this.Output.Write(opCodeInt64.Operand);
                    this.Output.Write("L");
                    break;
                case Code.Ldc_R4:
                    var opCodeSingle = opCode as OpCodeSinglePart;

                    if (float.IsPositiveInfinity(opCodeSingle.Operand))
                    {
                        this.Output.Write("1.0f/0.0f");
                    }
                    else if (float.IsNegativeInfinity(opCodeSingle.Operand))
                    {
                        this.Output.Write("-1.0f/0.0f");
                    }
                    else if (float.IsNaN(opCodeSingle.Operand))
                    {
                        this.Output.Write("0.0f/0.0f");
                    }
                    else
                    {
                        this.Output.Write(opCodeSingle.Operand);
                        if (Math.Floor(opCodeSingle.Operand) == opCodeSingle.Operand && !opCodeSingle.Operand.ToString().Contains("E"))
                        {
                            this.Output.Write(".0");
                        }

                        this.Output.Write("f");
                    }

                    break;
                case Code.Ldc_R8:
                    var opCodeDouble = opCode as OpCodeDoublePart;
                    if (double.IsPositiveInfinity(opCodeDouble.Operand))
                    {
                        this.Output.Write("1.0/0.0");
                    }
                    else if (double.IsNegativeInfinity(opCodeDouble.Operand))
                    {
                        this.Output.Write("-1.0/0.0");
                    }
                    else if (double.IsNaN(opCodeDouble.Operand))
                    {
                        this.Output.Write("0.0/0.0");
                    }
                    else
                    {
                        this.Output.Write(opCodeDouble.Operand);
                        if (Math.Floor(opCodeDouble.Operand) == opCodeDouble.Operand && !opCodeDouble.Operand.ToString().Contains("E"))
                        {
                            this.Output.Write(".0");
                        }
                    }

                    break;
                case Code.Ldstr:
                    var opCodeString = opCode as OpCodeStringPart;
                    var stringType = this.System.System_String;
                    var stringToken = opCodeString.Operand.Key;
                    var strType = this.WriteToString(() => stringType.WriteTypePrefix(this));
                    this.Output.Write("({1}) &_s{0}_", stringToken, strType);
                    break;
                case Code.Ldnull:
                    this.Output.Write("0/*null*/");
                    break;

                case Code.Arglist:

                    // TODO: it really does not do anything. you need to use VA_START, VA_END, VA_ARG in ArgInterator class
                    this.Output.Write("System_RuntimeArgumentHandle()/*undef*/");
                    break;

                case Code.Ldtoken:

                    // TODO: finish loading Token  
                    var opCodeTypePart = opCode as OpCodeTypePart;
                    if (opCodeTypePart != null)
                    {
                        var tokenType = opCodeTypePart.Operand;

                        // special case
                        if (tokenType.IsVirtualTableImplementation)
                        {
                            this.Output.Write(
                                tokenType.InterfaceOwner != null
                                    ? tokenType.InterfaceOwner.GetVirtualInterfaceTableNameReference(tokenType, this)
                                    : tokenType.GetVirtualTableNameReference(this));

                            break;
                        }

                        this.Output.Write("System_RuntimeTypeHandle()/*undef*/");
                    }

                    var opCodeFieldInfoPartToken = opCode as OpCodeFieldInfoPart;
                    if (opCodeFieldInfoPartToken != null)
                    {
                        var constBytes = opCodeFieldInfoPartToken.Operand.ConstantValue as IConstBytes;
                        if (constBytes != null)
                        {
                            var typeString = this.WriteToString(() => System.System_Byte.ToArrayType(1).WriteTypePrefix(this));
                            this.Output.Write("({1}) &{0}", constBytes.Reference, typeString);
                            break;
                        }

                        if (opCodeFieldInfoPartToken.Operand.FieldType.IsStaticArrayInit || opCodeFieldInfoPartToken.Operand.GetFieldRVAData() != null)
                        {
                            // TODO: can be repeated (improve it, reduce using opCode.AddressStart here) 
                            System.System_RuntimeFieldHandle.WriteTypePrefix(this);
                            var tokenVar = string.Format("_token{0}{1}", opCodeFieldInfoPartToken.Operand.FieldType.Token, opCode.AddressStart);
                            this.Output.WriteLine(" {0};", tokenVar);
                            this.Output.Write("{0}.", tokenVar);
                            this.WriteFieldAccessLeftExpression(
                                this.Output,
                                System.System_RuntimeFieldHandle,
                                System.System_RuntimeFieldHandle.GetFieldByName("fieldAddress", this, true),
                                null);
                            this.Output.Write(" = (");
                            System.System_Byte.ToPointerType().WriteTypePrefix(this);
                            this.Output.Write(") &");
                            this.WriteStaticFieldName(opCodeFieldInfoPartToken.Operand);

                            if (opCodeFieldInfoPartToken.Operand.FieldType.IsStaticArrayInit)
                            {
                                this.Output.Write(" + sizeof(Void*)");
                            }

                            this.Output.WriteLine(";");

                            this.Output.Write("{0}.", tokenVar);
                            this.WriteFieldAccessLeftExpression(
                                this.Output,
                                System.System_RuntimeFieldHandle,
                                System.System_RuntimeFieldHandle.GetFieldByName("fieldSize", this, true),
                                null);
                            this.Output.Write(" = ");
                            if (opCodeFieldInfoPartToken.Operand.FieldType.IsStaticArrayInit)
                            {
                                this.Output.Write(opCodeFieldInfoPartToken.Operand.FieldType.GetStaticArrayInitSize());
                            }
                            else
                            {
                                this.Output.Write("sizeof(");
                                opCodeFieldInfoPartToken.Operand.FieldType.WriteTypePrefix(this);
                                this.Output.Write(")");
                            }

                            opCode.Result = new FullyDefinedReference(tokenVar, System.System_RuntimeFieldHandle);
                            break;
                        }

                        this.Output.Write("System_RuntimeFieldHandle()/*undef*/");
                    }

                    // to support direct method address loading
                    var opCodeMethodInfoPartToken = opCode as OpCodeMethodInfoPart;
                    if (opCodeMethodInfoPartToken != null)
                    {
                        // hack to be able to initialize finalizer
                        if (opCodeMethodInfoPartToken.Operand is SynthesizedFinalizerWrapperMethod)
                        {
                            this.Output.Write("&");
                            this.WriteMethodDefinitionName(this.Output, opCodeMethodInfoPartToken.Operand);
                            break;
                        }

                        this.Output.Write("System_RuntimeMethodHandle()/*undef*/");
                    }

                    break;
                case Code.Localloc:

                    var varName = string.Format("_alloc{0}", opCode.AddressStart);
                    System.System_Byte.ToPointerType().WriteTypePrefix(this);
                    writer.WriteLine(" {0};", varName);
                    this.UnaryOper(writer, opCode, string.Format("{0} = (::Byte*) alloca(", varName));
                    writer.WriteLine(");");

                    // do not remove, otherwise stackoverflow
                    opCode.Result = new FullyDefinedReference(varName, System.System_Byte.ToPointerType());

                    this.WriteMemSet(opCode, firstOpCodeOperand);

                    break;
                case Code.Ldfld:

                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    // we wait when opCode.DestinationName is set;
                    this.WriteFieldAccess(opCodeFieldInfoPart);

                    break;
                case Code.Ldflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    if (!opCodeFieldInfoPart.Operand.HasFixedElementField)
                    {
                        this.Output.Write("&");
                        this.WriteFieldAccess(opCodeFieldInfoPart);
                    }
                    else
                    {
                        this.Output.Write("(");
                        opCodeFieldInfoPart.Operand.FieldType.ToPointerType().WriteTypePrefix(this);
                        this.Output.Write(")");
                        this.WriteOperandResultOrActualWrite(this.Output, opCodeFieldInfoPart, 0);
                    }

                    break;
                case Code.Ldsfld:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.WriteStaticFieldName(opCodeFieldInfoPart.Operand, specialization: ApplyGeneric.Specialization);

                    break;
                case Code.Ldsflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.Output.Write("&");
                    this.WriteStaticFieldName(opCodeFieldInfoPart.Operand, specialization: ApplyGeneric.Specialization);

                    break;
                case Code.Stfld:

                    this.FieldAccessAndSaveToField(opCode as OpCodeFieldInfoPart);

                    break;
                case Code.Stsfld:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    var destinationName = WriteToString(() => WriteStaticFieldName(opCodeFieldInfoPart.Operand, specialization: ApplyGeneric.Specialization));

                    var operandType = opCodeFieldInfoPart.Operand.FieldType;
                    var reference = new FullyDefinedReference(destinationName, operandType);

                    this.WriteSave(opCode, operandType, 0, reference);

                    break;

                case Code.Ldobj:
                    opCodeTypePart = opCode as OpCodeTypePart;
                    Debug.Assert(opCodeTypePart != null);
                    if (opCodeTypePart != null)
                    {
                        this.LoadObject(opCodeTypePart, 0);
                    }
                    break;

                case Code.Stobj:
                    opCodeTypePart = opCode as OpCodeTypePart;
                    SaveObject(opCodeTypePart, 1);
                    break;

                case Code.Cpobj:
                    // TODO: finish it properly
                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.SaveIndirect(this.Output, opCodeTypePart, opCodeTypePart.Operand);
                    break;

                case Code.Ldlen:
                    this.LoadElement(writer, opCode, "length");
                    break;

                case Code.Ldelem:
                case Code.Ldelem_I:
                case Code.Ldelem_I1:
                case Code.Ldelem_I2:
                case Code.Ldelem_I4:
                case Code.Ldelem_I8:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Ldelem_U1:
                case Code.Ldelem_U2:
                case Code.Ldelem_U4:
                case Code.Ldelema:

                    this.LoadElement(writer, opCode);
                    break;

                case Code.Stelem:
                case Code.Stelem_I:
                case Code.Stelem_I1:
                case Code.Stelem_I2:
                case Code.Stelem_I4:
                case Code.Stelem_I8:
                case Code.Stelem_R4:
                case Code.Stelem_R8:
                case Code.Stelem_Ref:

                    this.SaveElement(writer, opCode);
                    break;

                case Code.Ldind_I:
                case Code.Ldind_I1:
                case Code.Ldind_I2:
                case Code.Ldind_I4:
                case Code.Ldind_I8:
                case Code.Ldind_R4:
                case Code.Ldind_R8:
                case Code.Ldind_Ref:
                case Code.Ldind_U1:
                case Code.Ldind_U2:
                case Code.Ldind_U4:

                    this.LoadIndirect(writer, opCode);
                    break;

                case Code.Stind_I:
                case Code.Stind_I1:
                case Code.Stind_I2:
                case Code.Stind_I4:
                case Code.Stind_I8:
                case Code.Stind_R4:
                case Code.Stind_R8:
                case Code.Stind_Ref:

                    this.SaveIndirect(writer, opCode);
                    break;

                case Code.Call:
                case Code.Callvirt:
                    var opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;
                    var methodBase = opCodeMethodInfoPart.Operand;

                    if (methodBase.DeclaringType.IsStructureType() && methodBase.IsConstructor)
                    {
                        // convert value to object
                        // TODO: Review next line, it seems not needed anymore
                        methodBase.DeclaringType.ToClass().WriteCallInitObjectMethod(this, opCodeMethodInfoPart);
                        this.Output.WriteLine(";");
                    }

                    this.WriteCall(
                        opCodeMethodInfoPart,
                        methodBase,
                        code == Code.Callvirt,
                        methodBase.CallingConvention.HasFlag(CallingConventions.HasThis),
                        false,
                        null,
                        this.tryScopes.Count > 0 ? this.tryScopes.Peek() : null);

                    break;
                case Code.Add:
                    this.BinaryOper(writer, opCode, " + ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Add_Ovf:
                    // TODO: finish it
                    //this.WriteOverflowWithThrow(writer, opCode, "sadd");
                    this.BinaryOper(writer, opCode, " + ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Add_Ovf_Un:
                    // TODO: finish it
                    //this.WriteOverflowWithThrow(writer, opCode, "uadd");
                    this.BinaryOper(writer, opCode, " + ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Mul:
                    this.BinaryOper(writer, opCode, " * ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Mul_Ovf:
                    // TODO: finish it
                    //this.WriteOverflowWithThrow(writer, opCode, "smul");
                    this.BinaryOper(writer, opCode, " * ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Mul_Ovf_Un:
                    // TODO: finish it
                    //this.WriteOverflowWithThrow(writer, opCode, "umul");
                    this.BinaryOper(writer, opCode, " * ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Sub:
                    this.BinaryOper(writer, opCode, " - ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Sub_Ovf:
                    // TODO: finish it
                    //this.WriteOverflowWithThrow(writer, opCode, "ssub");
                    this.BinaryOper(writer, opCode, " - ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Sub_Ovf_Un:
                    // TODO: finish it
                    //this.WriteOverflowWithThrow(writer, opCode, "usub");
                    this.BinaryOper(writer, opCode, " - ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Div:
                case Code.Div_Un:

                    if (this.IsFloatingPointOp(opCode) || this.Unsafe || GetIntegerValueFromOpCode(opCode.OpCodeOperands[1]) > 0)
                    {
                        this.BinaryOper(
                            writer, opCode, " / ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes, unsigned: opCode.ToCode() == Code.Div_Un);
                    }
                    else
                    {
                        this.BinaryOper(
                            writer, opCode, " / __check_divide(", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes, unsigned: opCode.ToCode() == Code.Div_Un);
                        this.Output.Write(")");
                    }

                    break;
                case Code.Rem:
                case Code.Rem_Un:
                    if (this.IsFloatingPointOp(opCode))
                    {
                        writer.Write("fmod");
                        this.BinaryOper(writer, opCode, ", ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    }
                    else
                    {
                        this.BinaryOper(writer, opCode, " % ", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes, unsigned: opCode.ToCode() == Code.Rem_Un);
                    }

                    break;
                case Code.And:
                    this.BinaryOper(writer, opCode, " & ", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Or:
                    this.BinaryOper(writer, opCode, " | ", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Xor:
                    this.BinaryOper(writer, opCode, " ^ ", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Shl:
                    this.BinaryOper(writer, opCode, " << ", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Shr:
                case Code.Shr_Un:
                    this.BinaryOper(writer, opCode, " >> ", OperandOptions.AdjustIntTypes, unsigned: opCode.ToCode() == Code.Shr_Un);
                    break;
                case Code.Not:
                    this.UnaryOper(writer, opCode, "~");
                    break;
                case Code.Neg:
                    this.UnaryOper(writer, opCode, "-");
                    break;

                case Code.Dup:

                    // if this is virtual copy of Dup then process Dup from operand
                    var dupVar = string.Concat("_dup", opCode.AddressStart);

                    if (opCode.Result == null)
                    {
                        if (!opCode.IsVirtual())
                        {
                            this.WriteVariable(opCode, "_dup");

                            this.WriteOperandResultOrActualWrite(writer, opCode, 0);

                            // do not remove next live, it contains _dup variable
                            opCode.Result = new FullyDefinedReference(dupVar, opCode.RequiredOutgoingType);
                        }
                        else
                        {
                            opCode.Result = opCode.OpCodeOperands[0].Result;
                        }
                    }
                    else
                    {
                        this.WriteResultOrActualWrite(writer, opCode);
                    }

                    break;

                case Code.Box:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    var type = opCodeTypePart.Operand;
                    if (type.IsValueType)
                    {
                        type.WriteCallBoxObjectMethod(this, opCode);
                    }
                    else if (type.IsPointer)
                    {
                        this.System.System_Int32.WriteCallBoxObjectMethod(this, opCode);
                    }
                    else
                    {
                        UnaryOper(writer, opCode, 0, string.Empty, type);
                    }

                    break;

                case Code.Unbox:
                case Code.Unbox_Any:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    type = opCodeTypePart.Operand;
                    if (type.IsValueType || type.IsStructureType())
                    {
                        type.WriteCallUnboxObjectMethod(this, opCode);
                    }
                    else if (type.IsPointer)
                    {
                        this.System.System_Int32.WriteCallUnboxObjectMethod(this, opCode);
                    }
                    else if (!this.WriteCast(opCodeTypePart, opCodeTypePart.OpCodeOperands[0], opCodeTypePart.Operand, true))
                    {
                        this.WriteResultOrActualWrite(this.Output, opCodeTypePart.OpCodeOperands[0]);
                    }

                    break;
                case Code.Ret:
                    this.WriteReturn(writer, opCode, this.MethodReturnType);
                    break;
                case Code.Stloc:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                case Code.Stloc_S:

                    code = opCode.ToCode();
                    asString = code.ToString();
                    var index = 0;
                    if (code == Code.Stloc_S || code == Code.Stloc)
                    {
                        index = (opCode as OpCodeInt32Part).Operand;
                    }
                    else
                    {
                        index = int.Parse(asString.Substring(asString.Length - 1));
                    }

                    var localType = this.LocalInfo[index].LocalType;

                    var destination = new FullyDefinedReference(this.GetLocalVarName(index), localType);
                    var estResult = this.EstimatedResultOf(firstOpCodeOperand);
                    if (IsStructSave(localType, estResult))
                    {
                        this.WriteSavePrimitiveIntoStructure(opCode, firstOpCodeOperand.Result, destination);
                    }
                    else
                    {
                        this.WriteSave(opCode, localType, 0, destination);
                    }

                    break;
                case Code.Ldloc:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldloc_S:
                    asString = code.ToString();

                    if (opCode.Any(Code.Ldloc_S, Code.Ldloc))
                    {
                        index = (opCode as OpCodeInt32Part).Operand;
                    }
                    else
                    {
                        index = int.Parse(asString.Substring(asString.Length - 1));
                    }

                    destinationName = this.GetLocalVarName(index);

                    this.Output.Write(destinationName);

                    break;
                case Code.Ldloca:
                case Code.Ldloca_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;
                    this.Output.Write("&" + this.GetLocalVarName(index));

                    break;
                case Code.Ldarg:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldarg_S:
                    asString = code.ToString();
                    if (opCode.Any(Code.Ldarg_S, Code.Ldarg))
                    {
                        opCodeInt32 = opCode as OpCodeInt32Part;
                        index = opCodeInt32.Operand;
                    }
                    else
                    {
                        index = int.Parse(asString.Substring(asString.Length - 1));
                    }

                    if (this.HasMethodThis && index == 0)
                    {
                        this.Output.Write(this.GetThisName());
                    }
                    else
                    {
                        var parameterIndex = index - (this.HasMethodThis ? 1 : 0);
                        var parameter = this.Parameters[parameterIndex];

                        destinationName = this.GetArgVarName(parameter, index);
                        this.Output.Write(destinationName);
                    }

                    break;

                case Code.Ldarga:
                case Code.Ldarga_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;

                    writer.Write("&");

                    if (this.HasMethodThis && index == 0)
                    {
                        writer.Write(this.GetThisName());
                    }
                    else
                    {
                        var parameterIndex = index - (this.HasMethodThis ? 1 : 0);
                        var parameter = this.Parameters[parameterIndex];
                        writer.Write(this.GetArgVarName(parameter, index));
                    }

                    break;

                case Code.Starg:
                case Code.Starg_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;
                    var actualIndex = index - (this.HasMethodThis ? 1 : 0);

                    var argType = this.GetArgType(index);
                    destination = new FullyDefinedReference(this.GetArgVarName(actualIndex, index), this.GetArgType(index));
                    estResult = this.EstimatedResultOf(firstOpCodeOperand);
                    if (IsStructSave(argType, estResult))
                    {
                        this.WriteSavePrimitiveIntoStructure(opCode, firstOpCodeOperand.Result, destination);
                    }
                    else
                    {
                        this.WriteSave(opCode, argType, 0, destination);
                    }

                    break;

                case Code.Ldftn:

                    opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;

                    System.System_IntPtr.WriteTypePrefix(this);
                    var ptrVar = string.Format("_ptr{0}", opCode.AddressStart);
                    this.Output.WriteLine(" {0};", ptrVar);
                    this.Output.Write("{0}.", ptrVar);
                    this.WriteFieldAccessLeftExpression(
                        this.Output,
                        System.System_IntPtr,
                        System.System_IntPtr.GetFieldByFieldNumber(0, this),
                        null);
                    this.Output.Write(" = (::Byte*) &");
                    this.WriteMethodDefinitionName(this.Output, opCodeMethodInfoPart.Operand);
                    this.Output.WriteLine(";");

                    opCode.Result = new FullyDefinedReference(ptrVar, System.System_IntPtr);

                    break;

                case Code.Ldvirtftn:

                    opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;

                    var methodInfo = opCodeMethodInfoPart.Operand;

                    System.System_IntPtr.WriteTypePrefix(this);
                    ptrVar = string.Format("_ptr{0}", opCode.AddressStart);
                    this.Output.WriteLine(" {0};", ptrVar);
                    this.Output.Write("{0}.", ptrVar);
                    this.WriteFieldAccessLeftExpression(
                        this.Output,
                        System.System_IntPtr,
                        System.System_IntPtr.GetFieldByFieldNumber(0, this),
                        null);

                    this.Output.Write(" = ");
                    this.WriteFunctionAddressForVirtualMethod(writer, methodInfo, opCodeMethodInfoPart);
                    this.Output.WriteLine(";");

                    opCode.Result = new FullyDefinedReference(ptrVar, System.System_IntPtr);

                    break;

                case Code.Beq:
                case Code.Beq_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:

                    bool unsigned = false;

                    // we need to invert all comare command
                    var oper = string.Empty;
                    switch (opCode.ToCode())
                    {
                        case Code.Beq:
                        case Code.Beq_S:
                            oper = " == ";
                            break;
                        case Code.Bne_Un:
                        case Code.Bne_Un_S:
                            oper = " != ";
                            unsigned = true;
                            break;
                        case Code.Blt:
                        case Code.Blt_S:
                            oper = " < ";
                            break;
                        case Code.Blt_Un:
                        case Code.Blt_Un_S:
                            oper = " < ";
                            unsigned = true;
                            break;
                        case Code.Ble:
                        case Code.Ble_S:
                            oper = " <= ";
                            break;
                        case Code.Ble_Un:
                        case Code.Ble_Un_S:
                            oper = " <= ";
                            unsigned = true;
                            break;
                        case Code.Bgt:
                        case Code.Bgt_S:
                            oper = " > ";
                            break;
                        case Code.Bgt_Un:
                        case Code.Bgt_Un_S:
                            oper = " > ";
                            unsigned = true;
                            break;
                        case Code.Bge:
                        case Code.Bge_S:
                            oper = " >= ";
                            break;
                        case Code.Bge_Un:
                        case Code.Bge_Un_S:
                            oper = " >= ";
                            unsigned = true;
                            break;
                    }

                    writer.Write("if ");

                    this.BinaryOper(
                        writer,
                        opCode,
                        oper,
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes,
                        this.System.System_Boolean,
                        unsigned);

                    writer.Write(string.Concat(" goto a", opCode.JumpAddress()));

                    break;
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:

                    oper = opCode.Any(Code.Brtrue, Code.Brtrue_S) ? string.Empty : "!";

                    this.UnaryOper(writer, opCode, "if (" + oper);

                    writer.Write(string.Concat(") goto a", opCode.JumpAddress()));

                    break;
                case Code.Br:
                case Code.Br_S:

                    writer.Write(string.Concat("goto a", opCode.JumpAddress()));
                    break;
                case Code.Leave:
                case Code.Leave_S:

                    var upperLevelExceptionHandlingClause = this.GetUpperLevelExceptionHandlingClause();
                    this.WriteLeave(opCode, this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null, upperLevelExceptionHandlingClause);

                    break;
                case Code.Ceq:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " == ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes,
                        this.System.System_Boolean);

                    break;
                case Code.Clt:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " < ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes,
                        this.System.System_Boolean);

                    break;
                case Code.Clt_Un:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " < ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes,
                        this.System.System_Boolean,
                        true);

                    break;
                case Code.Cgt:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " > ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes,
                        this.System.System_Boolean);

                    break;
                case Code.Cgt_Un:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " > ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes,
                        this.System.System_Boolean,
                        true);
                    break;

                case Code.Conv_R4:
                case Code.Conv_R_Un:
                    this.WriteCCastOperand(opCode, 0, this.System.System_Single);
                    break;

                case Code.Conv_R8:
                    this.WriteCCastOperand(opCode, 0, this.System.System_Double);
                    break;

                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:

                    this.WriteCCastOperand(opCode, 0, this.System.System_SByte);
                    break;

                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                    this.WriteCCastOperand(opCode, 0, this.System.System_Byte);
                    break;

                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                    this.WriteCCastOperand(opCode, 0, this.System.System_Int16);
                    break;

                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                    this.WriteCCastOperand(opCode, 0, this.System.System_UInt16);
                    break;

                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                    this.WriteConvertToNativeInt(opCode);
                    break;

                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                    this.WriteCCastOperand(opCode, 0, this.System.System_Int32);
                    break;

                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                    this.WriteConvertToNativeInt(opCode);
                    break;

                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                    this.WriteCCastOperand(opCode, 0, this.System.System_UInt32);
                    break;

                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                    this.WriteCCastOperand(opCode, 0, this.System.System_Int64);
                    break;

                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    this.WriteCCastOperand(opCode, 0, this.System.System_UInt64);
                    break;

                case Code.Castclass:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    if (!this.WriteCast(opCodeTypePart, opCodeTypePart.OpCodeOperands[0], opCodeTypePart.Operand, true))
                    {
                        this.WriteResultOrActualWrite(this.Output, opCodeTypePart.OpCodeOperands[0]);
                    }

                    break;

                case Code.Isinst:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    if (!this.WriteDynamicCast(writer, opCode, opCodeTypePart.OpCodeOperands[0], opCodeTypePart.Operand.ToClass()))
                    {
                        this.WriteResultOrActualWrite(this.Output, opCodeTypePart.OpCodeOperands[0]);
                    }

                    break;

                case Code.Newobj:

                    // to support settings exceptions
                    if (opCode.ReadExceptionFromStack)
                    {
                        var catchOfFinallyClause = this.catchScopes.First(c => opCode.AddressStart >= c.Offset);
                        opCode.Result =
                            new ConstValue(
                                ExceptionHandlingGen.GetExceptionCaseVariable(catchOfFinallyClause),
                                catchOfFinallyClause.Catch ?? System.System_Exception);
                        break;
                    }

                    var opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;
                    if (opCodeConstructorInfoPart != null)
                    {
                        this.WriteNewObject(opCodeConstructorInfoPart);
                    }

                    break;

                case Code.Newarr:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteNewSingleArray(opCodeTypePart);
                    break;

                case Code.Initobj:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteInit(opCode, opCodeTypePart.Operand);
                    break;

                case Code.Throw:

                    this.WriteThrow(opCode, this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);

                    break;

                case Code.Rethrow:

                    this.WriteRethrow(
                        opCode,
                        this.catchScopes.Count > 0 ? this.catchScopes.Peek() : null,
                        this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);
                    break;

                case Code.Endfilter:
                    writer.Write("// Endfinally");
                    break;

                case Code.Endfinally:
                    writer.Write("// Endfinally");
                    break;

                case Code.Pop:
                    writer.WriteLine("// pop");
                    this.UnaryOper(writer, opCode, 0, string.Empty);
                    break;

                case Code.Constrained:
                    opCodeTypePart = opCode as OpCodeTypePart;

                    // TODO: not fully implemented, https://msdn.microsoft.com/library/system.reflection.emit.opcodes.constrained.aspx
                    // not implemented case when you need to box value type, read carefully documentation

                    var @class = opCodeTypePart.Operand.ToClass();
                    this.WriteVariableDeclare(opCode, @class, "_constr");
                    var constrVar = this.WriteVariable(opCode, "_constr");

                    var opCodeNone = OpCodePart.CreateNop;

                    if (opCodeTypePart.Operand.IsStructureType())
                    {
                        // TODO: IntPtr hack, cast operation (review it)
                        var isIntPtrOrUIntPtr = opCodeTypePart.Operand.IsIntPtrOrUIntPtr();
                        if (isIntPtrOrUIntPtr)
                        {
                            this.WriteCCastOnly(@class);
                        }

                        WriteOperandResultOrActualWrite(this.Output, opCode, 0);
                        opCode.Result = new FullyDefinedReference(constrVar, @class);

                        if (!isIntPtrOrUIntPtr)
                        {
                            this.Output.WriteLine(";");
                            // TODO: this is hack to init structure if it is from Class member (ideally you need to initialize it when you call new on Class)
                            @class.WriteCallInitObjectMethod(this, opCode);
                        }
                    }
                    else if (opCodeTypePart.Operand.IsValueType())
                    {
                        opCodeNone.OpCodeOperands = new[]
                        {
                            new OpCodeTypePart(OpCodesEmit.Ldobj, 0, 0, opCodeTypePart.Operand)
                            {
                                OpCodeOperands = new[]
                                {
                                    opCode.OpCodeOperands[0]
                                }
                            },
                        };

                        @class.WriteCallBoxObjectMethod(this, opCodeNone);
                    }
                    else
                    {
                        opCodeNone.OpCodeOperands = new[] { opCode.OpCodeOperands[0] };
                        LoadIndirect(writer, opCodeNone, opCodeTypePart.Operand);
                    }

                    opCode.Result = new FullyDefinedReference(constrVar, @class);

                    break;

                case Code.Switch:

                    var opCodeLabels = opCode as OpCodeLabelsPart;

                    this.UnaryOper(writer, opCode, "switch (");
                    writer.WriteLine(")");
                    writer.Write("{");
                    writer.Indent++;
                    writer.WriteLine(string.Empty);

                    index = 0;
                    foreach (var label in opCodeLabels.Operand)
                    {
                        writer.WriteLine("case {0}: goto a{1};", index, opCodeLabels.JumpAddress(index++));
                    }

                    writer.Indent--;
                    writer.Write("}");

                    break;

                case Code.Sizeof:
                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.Output.Write("sizeof(");
                    if (!opCodeTypePart.Operand.IsPointer || opCodeTypePart.Operand.UseAsClass)
                    {
                        opCodeTypePart.Operand.WriteTypeWithoutModifiers(this);
                    }
                    else
                    {
                        opCodeTypePart.Operand.WriteTypePrefix(this);
                    }

                    this.Output.Write(")");
                    break;

                case Code.Mkrefany:

                    var mkRefVar = this.WriteVariableDeclare(opCode, System.System_TypedReference, "_mkref");
                    opCode.Result = new FullyDefinedReference(mkRefVar, System.System_TypedReference);
                    this.Output.Write("{0}.Value.m_value = (Void*)", mkRefVar);
                    this.WriteOperandResultOrActualWrite(this.Output, opCode, 0);

                    break;

                case Code.Refanytype:

                    System.System_RuntimeTypeHandle.WriteTypePrefix(this);
                    var refAnyTypeVar = string.Format("_refanytype{0}", opCode.AddressStart);
                    this.Output.Write(" {0}", refAnyTypeVar);

                    ////this.WriteOperandResultOrActualWrite(this.Output, opCode, 0);
                    ////this.Output.Write(".Type.m_value");

                    opCode.Result = new FullyDefinedReference(refAnyTypeVar, System.System_RuntimeTypeHandle);

                    break;

                case Code.Refanyval:

                    this.WriteOperandResultOrActualWrite(this.Output, opCode, 0);
                    this.Output.Write(".Value.m_value");

                    break;

                case Code.Nop:
                    writer.Write("// nop");
                    break;

                case Code.Initblk:
                    this.WriteMemSet(opCode.OpCodeOperands[0], opCode.OpCodeOperands[1], opCode.OpCodeOperands[2]);
                    break;

                case Code.Cpblk:
                    this.WriteMemCopy(opCode.OpCodeOperands[0], opCode.OpCodeOperands[1], opCode.OpCodeOperands[2]);
                    break;

                case Code.Jmp:
                    opCodeInt32 = opCode as OpCodeInt32Part;
                    writer.Write("goto a{0}", opCodeInt32.Operand);
                    break;

                case Code.Ckfinite:
                    throw new NotImplementedException();
            }
        }

        private void WriteFunctionAddressForVirtualMethod(CIndentedTextWriter writer, IMethod methodInfo, OpCodeMethodInfoPart opCodeMethodInfoPart)
        {
            IType thisType;
            bool hasThisArgument;
            OpCodePart opCodeFirstOperand;
            ReturnResult resultOfFirstOperand;
            bool isIndirectMethodCall;
            IType ownerOfExplicitInterface;
            IType requiredType;
            methodInfo.FunctionCallProlog(
                opCodeMethodInfoPart,
                true,
                true,
                this,
                out thisType,
                out hasThisArgument,
                out opCodeFirstOperand,
                out resultOfFirstOperand,
                out isIndirectMethodCall,
                out ownerOfExplicitInterface,
                out requiredType);

            if (isIndirectMethodCall)
            {
                this.Output.Write("(::Void*)");
                this.GenerateVirtualCall(opCodeMethodInfoPart, methodInfo, thisType, opCodeFirstOperand, resultOfFirstOperand, ref requiredType);
            }
            else
            {
                this.Output.Write("(::Byte*) &");
                this.WriteMethodDefinitionName(writer, methodInfo);
            }
        }

        private bool IsStructSave(IType localType, ReturnResult estResult)
        {
            if (!localType.IsStructureType() || localType.IsByRef || localType.TypeEquals(estResult.Type))
            {
                return false;
            }

            var fieldByFieldNumber = localType.GetFieldByFieldNumber(0, this);
            if (fieldByFieldNumber == null)
            {
                return false;
            }

            return fieldByFieldNumber.FieldType.TypeEquals(estResult.Type);
        }

        private string WriteVariableDeclare(OpCodePart opCode, IType type, string name)
        {
            var variable = string.Format("{0}{1}", name, opCode.AddressStart);
            type.WriteTypePrefix(this);
            this.Output.WriteLine(string.Concat(" ", variable, ";"));
            return variable;
        }

        private string WriteVariable(OpCodePart opCode, string name)
        {
            var variable = string.Format("{0}{1}", name, opCode.AddressStart);
            this.Output.Write(string.Concat(variable, " = "));
            return variable;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="op">
        /// </param>
        /// <param name="options">
        /// </param>
        /// <param name="resultType">
        /// </param>
        /// <param name="beforeSecondOperand">
        /// </param>
        public void BinaryOper(CIndentedTextWriter writer, OpCodePart opCode, string op, OperandOptions options = OperandOptions.None, IType resultType = null, bool unsigned = false)
        {
            var estimatedResultOperand0 = this.EstimatedResultOf(opCode.OpCodeOperands[0]);
            var estimatedResultOperand1 = this.EstimatedResultOf(opCode.OpCodeOperands[1]);

            if (options.HasFlag(OperandOptions.CastPointersToBytePointer))
            {
                if (estimatedResultOperand0.IsReference && estimatedResultOperand1.IsReference)
                {
                    writer.Write("((::Byte*)");
                    this.WriteOperandResultOrActualWrite(writer, opCode, 0);
                    writer.Write(op);
                    writer.Write("(::Byte*)");
                    this.WriteOperandResultOrActualWrite(writer, opCode, 1);
                    writer.Write(")");
                    return;
                }
            }

            writer.Write("(");
            if (unsigned)
            {
                this.WriteUnsigned(estimatedResultOperand0.Type);
            }

            this.WriteOperandResultOrActualWrite(writer, opCode, 0);
            writer.Write(op);
            if (unsigned)
            {
                this.WriteUnsigned(estimatedResultOperand1.Type);
            }

            this.WriteOperandResultOrActualWrite(writer, opCode, 1);
            writer.Write(")");
        }

        private void WriteUnsigned(IType type)
        {
            var bits = type.IntTypeBitSize();
            if (bits == 0)
            {
                return;
            }

            var unsignedType = this.GetUIntTypeByBitSize(bits);
            this.Output.Write("(");
            unsignedType.WriteTypePrefix(this);
            this.Output.Write(")");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public int CalculateFirstFieldPositionInType(IType type)
        {
            var index = 0;

            if (type.BaseType != null)
            {
                index++;
            }

            // add shift for virtual table
            if (type.IsRootOfVirtualTable(this))
            {
                index++;
            }

            // add shift for interfaces
            var interfacesCount = type.GetInterfacesExcludingBaseAllInterfaces().Count();

            index += interfacesCount;

            return index;
        }

        /// <summary>
        /// </summary>
        public void Close()
        {
            this.Output.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="arg">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetArgVarName(string name, int index)
        {
            ////return string.Format("{0}_{1}", name.CleanUpName(), index);
            return name.CleanUpName();
        }

        public int GetFieldIndex(IType type, string fieldName)
        {
            // find index
            int index;
            if (!this.indexByFieldInfo.TryGetValue(string.Concat(type.FullName, '.', fieldName), out index))
            {
                index = this.CalculateFieldIndex(type, fieldName);
            }

            return index;
        }

        public int GetFieldPosition(IType type, IField fieldInfo)
        {
            // find index
            int index;
            if (!this.poisitionByFieldInfo.TryGetValue(fieldInfo.GetFullName(), out index))
            {
                index = this.CalculateFieldPosition(type, fieldInfo);
            }

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetLocalVarName(int index)
        {
            if (this.DebugInfo)
            {
                return this.debugInfoGenerator.GetLocalNameByIndex(index);
            }

            return string.Concat("local", index);
        }

        /// <summary>
        /// </summary>
        /// <param name="arg">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetThisName()
        {
            return "__this";
        }

        public void LoadElement(
            CIndentedTextWriter writer, OpCodePart opCode, string field, IType type = null, OpCodePart fixedArrayIndex = null, bool actualLoad = true)
        {
            if (!actualLoad)
            {
                writer.Write("&");
            }

            var fieldByName = opCode.OpCodeOperands[0].RequiredOutgoingType.GetFieldByName(field, this);
            this.WriteFieldAccess(writer, opCode, fieldByName, fixedArrayIndex);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public void LoadIndirect(CIndentedTextWriter writer, OpCodePart opCode)
        {
            IType type = null;
            var loadingIntPtrFromVoidPtr = false;

            switch (opCode.ToCode())
            {
                case Code.Ldind_I:
                    type = this.GetTypeOfReference(opCode);
                    if (!type.IsPointer && !type.IsByRef && (type.IntTypeBitSize() == PointerSize * 8 || type.IsVoid()))
                    {
                        // using int as intptr
                        loadingIntPtrFromVoidPtr = true;
                        type = this.System.System_IntPtr;
                    }

                    break;
                case Code.Ldind_I1:
                    type = this.System.System_SByte;
                    break;
                case Code.Ldind_U1:
                    type = this.System.System_Byte;
                    break;
                case Code.Ldind_I2:
                    type = this.System.System_Int16;
                    break;
                case Code.Ldind_U2:
                    type = this.System.System_UInt16;
                    break;
                case Code.Ldind_I4:
                    type = this.System.System_Int32;
                    break;
                case Code.Ldind_U4:
                    type = this.System.System_UInt32;
                    break;
                case Code.Ldind_I8:
                    type = this.System.System_Int64;
                    break;
                case Code.Ldind_R4:
                    type = this.System.System_Single;
                    break;
                case Code.Ldind_R8:
                    type = this.System.System_Double;
                    break;
                case Code.Ldind_Ref:
                    type = this.GetTypeOfReference(opCode);
                    break;
            }

            Debug.Assert(!type.IsVoid());

            this.LoadIndirect(writer, opCode, type, loadingIntPtrFromVoidPtr);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        public void LoadIndirect(CIndentedTextWriter writer, OpCodePart opCode, IType type, bool loadingIntPtrFromVoidPtr = false)
        {
            // next code fixing issue with using Code.Ldind to load first value in value types
            var resultOfOperand0 = EstimatedResultOf(opCode.OpCodeOperands[0]);
            var isUsedAsClass = resultOfOperand0.Type.IsClass;
            if (isUsedAsClass)
            {
                resultOfOperand0 = new ReturnResult(resultOfOperand0.Type.ToNormal());
            }

            var isValueType = resultOfOperand0.Type.IsValueType;
            if (isValueType && isUsedAsClass && !resultOfOperand0.Type.IsStructureType())
            {
                // write first field access
                this.WriteFieldAccess(writer, opCode, resultOfOperand0.Type.GetFieldByFieldNumber(0, this));
            }
            else if (loadingIntPtrFromVoidPtr && type.IsIntPtrOrUIntPtr())
            {
                // write first field access
                var field = type.GetFieldByFieldNumber(0, this);
                writer.Write("((");
                type.ToPointerType().WriteTypePrefix(this);
                writer.Write(")");
                this.WriteResultOrActualWrite(writer, opCode.OpCodeOperands[0]);
                writer.Write(")->");
                this.WriteFieldAccessLeftExpression(writer, field.DeclaringType, field, null);
            }
            else
            {
                writer.Write("(*((");
                type.ToPointerType().WriteTypePrefix(this);
                writer.Write(")");
                this.WriteOperandResultOrActualWrite(writer, opCode, 0);
                writer.Write("))");
            }
        }

        public void ReadParameters(string sourceFilePath, string pdbFilePath, string[] args)
        {
            // custom settings
            this.Gc = args == null || !args.Contains("gc-");
            this.Gctors = false;

            this.Unsafe = args != null && args.Contains("safe-");

            this.Stubs = args != null && args.Contains("stubs");

            this.DebugInfo = args != null && args.Contains("debug");
            if (this.DebugInfo)
            {
                this.debugInfoGenerator = new DebugInfoGenerator(pdbFilePath, sourceFilePath);
            }

            this.DebugInfoNoLineGenerating = args != null && args.Contains("line-");

            this.GcDebug = args != null && args.Contains("gcdebug");

            // predefined settings
            if (args != null && args.Contains("android"))
            {
                this.Gctors = false;
            }
            else if (args != null && args.Contains("emscripten"))
            {
                this.Gc = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="fieldType">
        /// </param>
        /// <param name="valueOperand">
        /// </param>
        public void SaveToField(OpCodePart opCodePart, IType fieldType, int valueOperand = 1)
        {
            if (opCodePart.OpCodeOperands == null)
            {
                return;
            }

            var writer = this.Output;
            this.UnaryOper(writer, opCodePart, valueOperand, " = ", fieldType);
        }

        /// <summary>
        /// </summary>
        public override void StartProcess()
        {
            base.StartProcess();
            MethodBodyBank.Clear();
            this.landingPadVariablesAreWritten = false;
            this.needToWriteUnwindException = false;
            this.needToWriteUnreachable = false;
        }

        public void UnaryOper(CIndentedTextWriter writer, OpCodePart opCode, string op, IType resultType = null)
        {
            UnaryOper(writer, opCode, 0, op, resultType);
        }

        public void UnaryOper(CIndentedTextWriter writer, OpCodePart opCode, int operand, string op, IType resultType = null)
        {
            writer.Write(op);
            this.WriteOperandResultOrActualWrite(writer, opCode, operand);
        }

        /// <summary>
        /// </summary>
        /// <param name="rawText">
        /// </param>
        public void WriteRawText(string rawText)
        {
            this.Output.Write(rawText);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        private void WriteOpCode(OpCodePart opCode)
        {
            this.AddOpCode(opCode);
        }

        /// <summary>
        /// </summary>
        public void WriteAfterFields()
        {
            this.Output.Indent--;
            this.Output.WriteLine("};");

            EndPreprocessorIf(this.ThisType);
        }

        /// <summary>
        /// </summary>
        public void WriteBeforeFields()
        {
            var baseType = this.ThisType.BaseType;

            this.Output.WriteLine(" {");
            this.Output.Indent++;

            if (baseType != null)
            {
                baseType.WriteTypeWithoutModifiers(this);
                this.Output.WriteLine(" base;");
            }

            var index = 0;
            foreach (var @interface in this.ThisType.GetInterfacesExcludingBaseAllInterfaces())
            {
                @interface.WriteTypeWithoutModifiers(this);
                this.Output.Write(" ifce_");
                this.Output.Write(@interface.FullName.CleanUpName());
                this.Output.WriteLine(";");
                index++;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="opCodeOperand"></param>
        /// <param name="toType">
        /// </param>
        /// <param name="throwExceptionIfNull">
        /// </param>
        /// <param name="forceCast"></param>
        /// <param name="fromType">
        /// </param>
        public bool WriteDynamicCast(CIndentedTextWriter writer, OpCodePart opCodePart, OpCodePart opCodeOperand, IType toType, bool throwExceptionIfNull = false, bool forceCast = false)
        {
            var fromTypeOriginal = this.EstimatedResultOf(opCodeOperand);
            var fromType = fromTypeOriginal.Type.IsPointer || fromTypeOriginal.Type.IsByRef
                ? new ReturnResult(fromTypeOriginal.Type.GetElementType())
                : fromTypeOriginal;
            if (fromType.Type.TypeEquals(toType) && !forceCast)
            {
                return false;
            }

            Debug.Assert(!fromTypeOriginal.Type.IsVoid());
            Debug.Assert(!fromType.Type.IsVoid());
            Debug.Assert(!fromTypeOriginal.Type.IsPrimitiveType());
            Debug.Assert(!fromTypeOriginal.Type.IsStructureType());
            Debug.Assert(!fromTypeOriginal.Type.IsEnum);
            Debug.Assert(!fromType.Type.IsEnum);
            Debug.Assert(!fromType.Type.IsGenericTypeDefinition);
            Debug.Assert(fromType.Type.ToNormal().NormalizeType().TypeNotEquals(toType.ToNormal().NormalizeType()));

            writer.Write("((");
            toType.WriteTypePrefix(this);
            writer.Write(") __dynamic_cast_null_test{0}(", throwExceptionIfNull ? "_throw" : string.Empty);

            this.WriteResultOrActualWrite(writer, opCodeOperand);

            writer.Write(", (Void*) &{0}", fromType.Type.GetRttiInfoName(this));
            writer.Write(", (Void*) &{0}", toType.GetRttiInfoName(this));
            writer.Write(", {0}))", CalculateDynamicCastInterfaceIndex(fromType.Type, toType));

            return true;
        }

        /// <summary>
        /// </summary>
        public void WriteEnd()
        {
            if (IsHeader)
            {
                this.WriteGctorsForwardDeclarations();
            }
            else
            {
                this.WriteGlobalConstructors();
            }

            if (this.MainMethod != null)
            {
                this.WriteMainFunction();
            }

            if (this.IsHeader)
            {
                this.EndPreprocessorIf();
            }

            this.Output.Close();
        }

        public void WriteEndOfPhiValues(CIndentedTextWriter writer, OpCodePart opCode)
        {
            var usedByAlternativeValues = opCode.UsedByAlternativeValues;
            while (usedByAlternativeValues.UsedByAlternativeValues != null)
            {
                usedByAlternativeValues = usedByAlternativeValues.UsedByAlternativeValues;
            }

            var addressStart = GetPhiValuesAddressStart(usedByAlternativeValues);
            if (opCode.Result != null)
            {
                this.Output.WriteLine(";");
                this.Output.Write("_phi{0} = ", addressStart);
                this.WriteResultOrActualWrite(this.Output, opCode);
            }

            opCode.Result = new FullyDefinedReference(
                "_phi" + addressStart, usedByAlternativeValues.RequiredOutgoingType);
        }

        public int GetPhiValuesAddressStart(PhiNodes phiNodes)
        {
            var firstOpCode = phiNodes.Values[0];
            while (firstOpCode.AddressEnd == 0)
            {
                firstOpCode = firstOpCode.OpCodeOperands.FirstOrDefault();
            }

            return firstOpCode.AddressStart;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeFieldInfoPart">
        /// </param>
        public void WriteFieldAccess(OpCodeFieldInfoPart opCodeFieldInfoPart)
        {
            var writer = this.Output;

            var operand = opCodeFieldInfoPart.OpCodeOperands[0];
            var operandEstimatedResultOf = this.EstimatedResultOf(operand);
            var operandType = operandEstimatedResultOf.Type;
            var effectiveType = operandType;

            writer.Write("(");

            if (effectiveType.IsValueType)
            {
                if (operandEstimatedResultOf.Type.IntTypeBitSize() == PointerSize * 8)
                {
                    effectiveType = opCodeFieldInfoPart.Operand.DeclaringType;
                    this.WriteCCastOnly(effectiveType.ToPointerType());
                }
                else if (!effectiveType.IsByRef)
                {
                    effectiveType = effectiveType.ToClass();
                }
            }
            else if (effectiveType.IsPointer)
            {
                effectiveType = opCodeFieldInfoPart.Operand.DeclaringType;
                this.WriteCCastOnly(effectiveType.ToPointerType());
            }

            this.WriteResultOrActualWrite(writer, opCodeFieldInfoPart.OpCodeOperands[0]);

            writer.Write(")");
            writer.Write(!operandEstimatedResultOf.Type.IsStructureType() ? "->" : ".");

            effectiveType = effectiveType.IsByRef ? effectiveType.GetElementType() : effectiveType;
            if (opCodeFieldInfoPart.Operand.DeclaringType.IsInterface)
            {
                this.WriteInterfacePath(effectiveType, opCodeFieldInfoPart.Operand.DeclaringType, opCodeFieldInfoPart.Operand);
            }
            else
            {
                this.WriteFieldPath(effectiveType, opCodeFieldInfoPart.Operand);
            }
        }

        /// <summary>
        ///     fixing issue with Code.Ldind when you need to load first field value
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="index">
        /// </param>
        public void WriteFieldAccess(CIndentedTextWriter writer, OpCodePart opCodePart, IField field, OpCodePart fixedArrayElementIndex = null)
        {
            var operandEstimatedResultOf = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
            var classType = operandEstimatedResultOf.Type.IsPointer || operandEstimatedResultOf.Type.IsByRef
                ? operandEstimatedResultOf.Type.GetElementType().ToClass()
                : operandEstimatedResultOf.Type.ToClass();

            writer.Write("(");

            this.WriteResultOrActualWrite(writer, opCodePart.OpCodeOperands[0]);

            writer.Write(")");

            writer.Write(!operandEstimatedResultOf.Type.IsStructureType() ? "->" : ".");

            this.WriteFieldAccessLeftExpression(writer, classType, field, fixedArrayElementIndex);
        }

        private void WriteFieldAccessLeftExpression(CIndentedTextWriter writer, IType classType, IField field, OpCodePart fixedArrayElementIndex)
        {
            if (field.DeclaringType.IsInterface)
            {
                this.WriteInterfacePath(classType, field.DeclaringType, field);
            }
            else
            {
                this.WriteFieldPath(classType, field);
            }

            if (fixedArrayElementIndex != null)
            {
                writer.Write("[");
                this.WriteResultOrActualWrite(writer, fixedArrayElementIndex);
                writer.Write("]");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="classType">
        /// </param>
        /// <param name="fieldContainerType">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="valueReference">
        /// </param>
        /// <returns>
        /// </returns>
        public IField WriteFieldAccess(OpCodePart opCodePart, IType classType, IType fieldContainerType, int index, FullyDefinedReference valueReference)
        {
            var writer = this.Output;

            var field = fieldContainerType.GetFieldByFieldNumber(index, this);
            if (field == null)
            {
                return null;
            }

            writer.Write(valueReference);
            if (valueReference.Type.IsStructureType())
            {
                writer.Write(".");
            }
            else
            {
                writer.Write("->");
            }

            this.WriteFieldPath(classType, field);

            return field;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="classType">
        /// </param>
        /// <param name="fieldInfo">
        /// </param>
        public void WriteFieldPath(IType classType, IField fieldInfo)
        {
            var writer = this.Output;

            var targetType = fieldInfo.DeclaringType;
            var type = classType.ToNormal();

            while (type.TypeNotEquals(targetType))
            {
                type = type.BaseType;
                if (type == null)
                {
                    Debug.Assert(false, "could not find base class");
                    break;
                }

                // first index is base type index
                writer.Write("base.");
            }

            writer.Write(fieldInfo.Name.CleanUpName());
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        public void WriteField(IField field)
        {
            if (field.IsStatic || field.IsConst)
            {
                this.WriteStaticField(field, false);
                return;
            }

            this.WriteFieldType(field);
            this.Output.Write(' ');
            this.Output.Write(field.Name.CleanUpName());

            if (field.IsFixed)
            {
                this.Output.Write("[{0}]", field.FixedSize);
            }

            this.Output.WriteLine(';');
        }

        /// <summary>
        /// </summary>
        /// <param name="fieldType">
        /// </param>
        public void WriteFieldType(IType fieldType)
        {
            Debug.Assert(!fieldType.IsGenericParameter);

            fieldType.WriteTypePrefix(this);
        }

        public void WriteFieldType(IField field)
        {
            if (field.IsFixed)
            {
                this.WriteFieldType(field.FieldType.GetElementType());
            }
            else
            {
                this.WriteFieldType(field.FieldType);
            }
        }

        // TODO: doNotEstimateResult is hack
        public void WriteInterfaceAccess(OpCodePart opCodePart, IType classType, IType interfaceType, bool doNotEstimateResult = false)
        {
            var writer = this.Output;

            var operand = opCodePart;
            var operandResultCalc = this.EstimatedResultOf(operand);
            var operandType = operandResultCalc.Type;
            var effectiveType = !doNotEstimateResult ? operandType : classType;
            if (effectiveType.IsValueType)
            {
                if (operandResultCalc.Type.IntTypeBitSize() == PointerSize * 8)
                {
                    effectiveType = classType;
                    this.WriteCCastOnly(effectiveType.ToPointerType());
                }
                else if (!effectiveType.IsByRef)
                {
                    effectiveType = effectiveType.ToClass();
                }
            }
            else if (effectiveType.IsPointer)
            {
                effectiveType = classType;
                this.WriteCCastOnly(effectiveType.ToPointerType());
            }

            this.WriteResultOrActualWrite(writer, operand);
            writer.Write("->");
            effectiveType = effectiveType.IsByRef ? effectiveType.GetElementType() : effectiveType;
            this.WriteInterfacePath(effectiveType, interfaceType, null /*interfaceType.GetFieldByName(CWriter.VTable, this)*/);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="label">
        /// </param>
        public void WriteLabel(CIndentedTextWriter writer, string label)
        {
            writer.Indent--;
            writer.WriteLine(string.Concat(label, ':'));
            writer.Indent++;
        }

        public enum ApplyGeneric
        {
            NotApplied,
            Declaration,
            Specialization
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        public void WriteMethodDefinitionName(CIndentedTextWriter writer, IMethod methodBase, IType ownerOfExplicitInterface = null, bool shortName = true, bool excludeNamespace = false, ApplyGeneric specialization = ApplyGeneric.Specialization)
        {
            this.WriteMethodDefinitionNameNoPrefix(writer, methodBase, ownerOfExplicitInterface, shortName, excludeNamespace, specialization);
        }

        public void WriteMethodDefinitionNameNoPrefix(CIndentedTextWriter writer, IMethod methodBase, IType ownerOfExplicitInterface = null, bool shortName = true, bool excludeNamespace = false, ApplyGeneric specialization = ApplyGeneric.Specialization)
        {
            if (methodBase.DeclaringType == null)
            {
                writer.Write(methodBase.Name);
            }
            else
            {
                ////var name = shortName ? methodBase.GetMethodName(ownerOfExplicitInterface) : methodBase.GetFullMethodName(ownerOfExplicitInterface);
                ////writer.Write(name);

                var name = shortName ? methodBase.GetMethodName(ownerOfExplicitInterface) : methodBase.GetFullMethodName(ownerOfExplicitInterface);
                var ns = methodBase.Namespace;
                if (!excludeNamespace && ns != null)
                {
                    if (specialization != ApplyGeneric.NotApplied)
                    {
                        writer.Write("::");
                    }

                    if (!string.IsNullOrWhiteSpace(ns))
                    {
                        writer.Write(ns.Replace(".", "::"));
                        writer.Write("::");
                    }

                    writer.Write("Fn_");
                    this.WriteClassName(methodBase.DeclaringType);
                    writer.Write("::");
                }

                writer.Write(name);
                if (!excludeNamespace)
                {
                    switch (specialization)
                    {
                        case ApplyGeneric.Declaration:
                            this.WriteTemplateSpecializationDeclaration(methodBase.DeclaringType);
                            break;
                        case ApplyGeneric.Specialization:
                            WriteTemplateSpecializationDefinition(methodBase);
                            break;
                    }
                }
            }
        }

        public override bool GetGcSupport()
        {
            return this.Gc;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        private void WriteMethodEnd(IMethod method, IGenericContext genericContext)
        {
            var rest = this.PrepareWritingMethodBody();
            this.WriteMethodBeginning(method, genericContext);
            this.WriteMethodBody(rest);
            this.WritePostMethodEnd(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="parameterInfos">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="returnType">
        /// </param>
        /// <param name="noArgumentName">
        /// </param>
        /// <param name="varArgs">
        /// </param>
        public void WriteMethodParamsDef(
            CIndentedTextWriter writer, IMethod method, bool hasThis, IType thisType, IType returnType, bool noArgumentName = false)
        {
            var parameterInfos = method.GetParameters();

            writer.Write("(");

            var hasParameterWritten = false;

            var start = hasThis ? 1 : 0;

            if (hasThis)
            {
                thisType.ToClass().WriteTypePrefix(this, true);
                hasParameterWritten = true;

                if (!noArgumentName)
                {
                    writer.Write(" {0}", this.GetThisName());
                }
            }

            var index = start;
            var parameterIndex = start;
            var isAnonymousDelegate = method.IsAnonymousDelegate;
            var parameters = parameterInfos;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (isAnonymousDelegate)
                    {
                        isAnonymousDelegate = false;
                    }
                    else
                    {
                        if (hasParameterWritten)
                        {
                            writer.Write(", ");
                        }

                        parameter.ParameterType.WriteTypePrefix(this);
                        hasParameterWritten = true;

                        if (!noArgumentName)
                        {
                            writer.Write(" ");
                            writer.Write(GetArgVarName(parameter, parameterIndex));
                        }
                    }

                    index++;
                    parameterIndex++;
                }
            }

            if (method.CallingConvention.HasFlag(CallingConventions.VarArgs))
            {
                if (hasParameterWritten)
                {
                    writer.Write(", ");
                }

                writer.Write("...");
            }

            writer.Write(")");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        /// <param name="thisType">
        /// </param>
        public void WriteMethodPointerType(CIndentedTextWriter writer, IMethod methodBase, IType thisType = null, bool asStatic = false, bool withName = false, bool shortName = true, string suffix = null, bool excludeNamespace = false)
        {
            this.WriteMethodReturnType(writer, methodBase);
            writer.Write("(*");
            if (withName)
            {
                this.WriteMethodDefinitionNameNoPrefix(writer, methodBase, shortName: shortName, excludeNamespace: excludeNamespace);
            }

            if (!string.IsNullOrEmpty(suffix))
            {
                writer.Write(suffix);
            }

            writer.Write(")");

            this.WriteMethodParameters(writer, methodBase, thisType, asStatic);
        }

        public void WriteMethodParameters(CIndentedTextWriter writer, IMethod methodBase, IType thisType = null, bool asStatic = false)
        {
            writer.Write("(");

            var hasThis = !methodBase.IsStatic && !asStatic;
            if (hasThis)
            {
                (thisType ?? methodBase.DeclaringType.ToClass()).WriteTypePrefix(this);
            }

            var isAnonymousDelegate = methodBase.IsAnonymousDelegate;
            var index = 0;
            var parameters = methodBase.GetParameters();
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (isAnonymousDelegate)
                    {
                        isAnonymousDelegate = false;
                        continue;
                    }

                    if (index > 0 || hasThis)
                    {
                        writer.Write(", ");
                    }

                    parameter.ParameterType.WriteTypePrefix(this);
                    index++;
                }
            }

            if (methodBase.CallingConvention.HasFlag(CallingConventions.VarArgs))
            {
                if (index > 0 || hasThis)
                {
                    writer.Write(", ");
                }

                writer.Write("...");
            }

            writer.Write(")");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="method">
        /// </param>
        public void WriteMethodReturnType(CIndentedTextWriter writer, IMethod method)
        {
            method.ReturnType.WriteTypePrefix(this);
            writer.Write(" ");
        }

        public void WriteBeforeMethods(IType type)
        {
            this.Output.Write("namespace Fn_");
            this.WriteClassName(type);
            this.Output.Indent++;
            this.Output.WriteLine(" { ");
        }

        public void WriteAfterMethods(IType type)
        {
            this.Output.Indent--;
            this.Output.WriteLine("}");
        }

        public void WriteMethod(IMethod method, IMethod methodOpCodeHolder, IGenericContext genericMethodContext)
        {
            if (method is IConstructor && method.IsStatic)
            {
                this.IlReader.StaticConstructors.Add(method);
            }

            this.WriteMethodStart(method, genericMethodContext);

            foreach (var ilCode in IlReader.OpCodes(methodOpCodeHolder ?? method, genericMethodContext))
            {
                this.WriteOpCode(ilCode);
            }

            this.WriteMethodEnd(method, genericMethodContext);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        private void WriteMethodStart(IMethod method, IGenericContext genericContext)
        {
            this.StartProcess();

            Debug.Assert(!method.IsGenericMethodDefinition);

            if (method.Token.HasValue)
            {
                Debug.Assert(genericContext == null || !method.DeclaringType.IsGenericTypeDefinition);
                this.methodsByToken[method.Token.Value] = method;
            }

            this.ReadMethodInfo(method, genericContext);

            var isMain = IsMain(method);

            // check if main
            if (isMain)
            {
                this.MainMethod = method;
            }

            if (method.IsAbstract || method.IsSkipped())
            {
                return;
            }

            // to gather all info about method which we need
            this.IlReader.UsedStrings = new SortedDictionary<int, string>();
            this.IlReader.UsedConstBytes = new List<IConstBytes>();
            this.IlReader.CalledMethods = new NamespaceContainer<MethodKey>();
            this.IlReader.UsedArrayTypes = new NamespaceContainer<IType>();
        }

        private static bool IsMain(IMethod method)
        {
            return method.IsStatic && method.CallingConvention.HasFlag(CallingConventions.Standard) && method.Name.Equals("Main");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="typeName">
        /// </param>
        /// <returns>
        /// </returns>
        public FullyDefinedReference WriteNewCallingDefaultConstructor(CWriter cWriter, OpCodePart addressOpCode, string typeName)
        {
            var typeToCreate = this.ResolveType(typeName);
            return this.WriteNewCallingDefaultConstructor(cWriter, addressOpCode, typeToCreate);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="typeToCreate">
        /// </param>
        /// <returns>
        /// </returns>
        public FullyDefinedReference WriteNewCallingDefaultConstructor(CWriter cWriter, OpCodePart addressOpCode, IType typeToCreate, bool noNewLines = false)
        {
            var writer = cWriter.Output;

            if (!noNewLines)
            {
                writer.WriteLine(string.Empty);
            }

            // find constructor
            var constructorInfo = Logic.IlReader.Constructors(typeToCreate, cWriter).FirstOrDefault(c => !c.GetParameters().Any());

            OpCodePart opCodeNewInstance = null;
            if (constructorInfo != null)
            {
                opCodeNewInstance = new OpCodeConstructorInfoPart(OpCodesEmit.Newobj, addressOpCode.AddressStart, 0, constructorInfo);
                this.WriteNewObject((OpCodeConstructorInfoPart)opCodeNewInstance);
            }
            else
            {
                // we just need to create object without calling consturctor on it
                opCodeNewInstance = OpCodePart.CreateNop;
                this.WriteNewObject(opCodeNewInstance, typeToCreate);
            }

            if (!noNewLines)
            {
                writer.WriteLine(string.Empty);
            }

            return opCodeNewInstance.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="firstParameterType">
        /// </param>
        /// <param name="firstParameterOpCode">
        /// </param>
        /// <returns>
        /// </returns>
        public void WriteNewWithCallingConstructor(
            OpCodePart opCode, IType type, IType firstParameterType, OpCodePart firstParameterOpCode, FullyDefinedReference predefinedObjectReference = null)
        {
            // find constructor
            var constructorInfo =
                Logic.IlReader.Constructors(type, this)
                     .FirstOrDefault(c => c.GetParameters().Count() == 1 && c.GetParameters().First().ParameterType.TypeEquals(firstParameterType));

            ////Debug.Assert(constructorInfo != null, "Could not find required constructor");
            type.WriteCallNewObjectMethod(this, opCode);

            opCode.OpCodeOperands = new[] { firstParameterOpCode };

            if (constructorInfo != null)
            {
                this.WriteCallConstructor(opCode, constructorInfo);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="firstParameterType">
        /// </param>
        /// <param name="firstParameterValue">
        /// </param>
        /// <returns>
        /// </returns>
        public FullyDefinedReference WriteNewWithCallingConstructor(
            OpCodePart opCode, IType type, IType firstParameterType, FullyDefinedReference firstParameterValue)
        {
            // find constructor
            var constructorInfo = Logic.IlReader.FindConstructor(type, firstParameterType, this);

            ////Debug.Assert(constructorInfo != null, "Could not find required constructor");
            type.WriteCallNewObjectMethod(this, opCode);

            var opCodeOperandWithFunctionRef = OpCodePart.CreateNop;
            opCodeOperandWithFunctionRef.Result = firstParameterValue;
            opCode.OpCodeOperands = new[] { opCodeOperandWithFunctionRef };

            if (constructorInfo != null)
            {
                this.WriteCallConstructor(opCode, constructorInfo);
            }

            return opCode.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="index">
        /// </param>
        public void WriteOperandResultOrActualWrite(CIndentedTextWriter writer, OpCodePart opCode, int index)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            var operand = opCode.OpCodeOperands[index];
            this.WriteResultOrActualWrite(writer, operand);
        }

        public void WritePostDeclarations(IType type)
        {
            if (!(type.IsGenericType || type.IsArray) && this.AssemblyQualifiedName != type.AssemblyQualifiedName)
            {
                return;
            }

            if (!type.IsPrivateImplementationDetails)
            {
                type.WriteRttiDefinition(this);

                this.WriteVirtualTableImplementations(type);

                ////StartPreprocessorIf(type, "DP");

                WriteVirtualTable(type);

                ////EndPreprocessorIf(type);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        public void WriteResult(OpCodePart opCode)
        {
            Debug.Assert(opCode.Result != null && !string.IsNullOrWhiteSpace(opCode.Result.ToString()));

            // write number of method
            this.Output.Write(opCode.Result);
        }

        /// <summary>
        /// </summary>
        /// <param name="reference">
        /// </param>
        public void WriteResult(FullyDefinedReference reference)
        {
            if (reference == null)
            {
                return;
            }

            if (!(reference is IncrementalResult))
            {
                // write number of method
                this.Output.Write(reference);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="operand">
        /// </param>
        public void WriteResultOrActualWrite(CIndentedTextWriter writer, OpCodePart operand)
        {
            this.ActualWrite(writer, operand);
            this.WriteResult(operand.Result);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="methodReturnType">
        /// </param>
        public void WriteReturn(CIndentedTextWriter writer, OpCodePart opCode, IType methodReturnType)
        {
            writer.Write("return");
            if (!methodReturnType.IsVoid())
            {
                this.UnaryOper(writer, opCode, " ", methodReturnType);
            }
        }

        /// <summary>
        /// </summary>
        public void WriteStart()
        {
            this.Output = new CIndentedTextWriter(new StreamWriter(this.outputFile));

            if (this.IsHeader)
            {
                StartPreprocessorIf(this.AssemblyQualifiedName.CleanUpName(), "H");
            }

            if (this.DebugInfo && !this.debugInfoGenerator.StartGenerating())
            {
                this.DebugInfo = false;
            }

            if (this.DebugInfo && !this.DebugInfoNoLineGenerating)
            {
                this.WriteDebugLine();
            }

            if (this.IsHeader && this.IsCoreLib)
            {
                // declarations
                this.Output.WriteLine(Resources.c_declarations);
                this.Output.WriteLine(string.Empty);

                if (this.Gc)
                {
                    this.Output.WriteLine(this.GcDebug ? Resources.gc_declarations_debug : Resources.gc_declarations);
                    this.Output.WriteLine(string.Empty);
                }
            }

            if (!this.IsHeader)
            {
                this.Output.WriteLine("#include \"{0}.h\"", this.FileHeader);
            }
            else
            {
                foreach (var reference in this.IlReader.References())
                {
                    this.Output.WriteLine("#include \"{0}.h\"", reference);
                }
            }

            this.Output.WriteLine(string.Empty);

            VirtualTableGen.Clear();
            TypeGen.Clear();
        }

        public bool WriteStartOfPhiValues(CIndentedTextWriter writer, OpCodePart opCode, bool firstLevel)
        {
            if (!firstLevel)
            {
                return false;
            }

            var usedByAlternativeValues = opCode.UsedByAlternativeValues;
            while (usedByAlternativeValues.UsedByAlternativeValues != null)
            {
                usedByAlternativeValues = usedByAlternativeValues.UsedByAlternativeValues;
            }

            var firstOpCode = usedByAlternativeValues.Values[0];
            var addressStart = GetPhiValuesAddressStart(usedByAlternativeValues);
            if (opCode == firstOpCode)
            {
                usedByAlternativeValues.RequiredOutgoingType.WriteTypePrefix(this);
                this.Output.WriteLine(" _phi{0};", addressStart);
            }

            bool isVirtualCall;
            if (opCode.Result == null && !OpCodeWithVariableDeclaration(opCode, out isVirtualCall))
            {
                this.Output.Write("_phi{0} = ", addressStart);
            }

            return false;
        }

        public void WriteInterfaceToObjectCast(CIndentedTextWriter writer, OpCodePart opCode, IType toType)
        {
            writer.Write("(");
            this.WriteCCastOnly(toType);
            writer.Write("__interface_to_object(");
            this.WriteResultOrActualWrite(writer, opCode);
            writer.Write("))");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="exceptionName">
        /// </param>
        public void WriteThrowException(CWriter cWriter, string exceptionName)
        {
            var writer = cWriter.Output;

            // throw InvalidCast result
            writer.WriteLine(string.Empty);

            var opCodeThrow = new OpCodePart(OpCodesEmit.Throw, 0, 0);

            var invalidCastExceptionType = this.ResolveType(exceptionName);

            // find constructor
            var constructorInfo = Logic.IlReader.Constructors(invalidCastExceptionType, cWriter).First(c => !c.GetParameters().Any());

            var opCodeNewInstance = new OpCodeConstructorInfoPart(OpCodesEmit.Newobj, 0, 0, constructorInfo);
            opCodeThrow.OpCodeOperands = new[] { opCodeNewInstance };

            this.WriteNewObject(opCodeNewInstance);

            writer.WriteLine(string.Empty);

            this.WriteThrow(opCodeThrow, this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);
        }

        /// <summary>
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <returns>
        /// </returns>
        public string WriteToString(Action action)
        {
            var output = this.Output;

            var sb = new StringBuilder();
            this.Output = new CIndentedTextWriter(new StringWriter(sb));

            action();

            this.Output = output;
            return sb.ToString();
        }

        public void WriteForwardTypeDeclaration(IType type, IGenericContext genericContext)
        {
            this.WriteTypeNamespaceStart(type);

            this.WriteTemplateDeclaration(type);

            this.Output.Write("struct ");

            this.WriteClassName(type);

            this.Output.WriteLine(";");

            this.WriteTypeNamespaceEnd(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteTypeStart(IType type, IGenericContext genericContext)
        {
            this.Output.WriteLine(string.Empty);

            this.ReadTypeInfo(type);

#if EXTRA_VALIDATION
            Debug.Assert(!type.IsGenericTypeDefinition);
#endif

            this.WriteTypeNamespaceStart(type);

            this.StartPreprocessorIf(type, "D");

            this.WriteTemplateDeclaration(type);

            this.Output.Write("struct ");
            //type.ToClass().WriteTypeName(this.Output, false);

            this.WriteClassName(type);

            this.externDeclarations.Clear();
        }

        private void WriteClassName(IType type)
        {
            var currentType = type.IsArray ? type.GetElementType() : type;
            if (currentType.IsNested)
            {
                var sb = new StringBuilder();

                while (currentType != null && currentType.IsNested)
                {
                    currentType = currentType.DeclaringType;
                    if (currentType != null)
                    {
                        sb.Insert(0, string.Concat(currentType.Name.CleanUpName(), "_"));
                    }
                }

                this.Output.Write(sb.ToString());
            }

            type.WriteTypeName(this.Output, false, true, true, true);
        }

        public void WriteTypeEnd(IType type)
        {
            this.WriteTypeNamespaceEnd(type);

            // write all extern declarations
            foreach (var externMethod in this.externDeclarations)
            {
                this.ReadMethodInfo(externMethod, null);
                this.WriteMethodProlog(externMethod, externDecl: true);
                this.Output.WriteLine(";");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fromType">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <returns>
        /// </returns>
        private static int CalculateDynamicCastInterfaceIndex(IType fromType, IType toType)
        {
            if (!fromType.IsInterface && !toType.IsInterface)
            {
                if (toType.IsDerivedFrom(fromType))
                {
                    return 0;
                }

                return -2;
            }

            if (!fromType.IsInterface && toType.IsInterface)
            {
                return -2;
            }

            var allInterfaces = toType.GetAllInterfaces();
            if (fromType.IsInterface && !toType.IsInterface && !allInterfaces.Contains(fromType))
            {
                return -2;
            }

            if (fromType.IsInterface && !toType.IsInterface && allInterfaces.Contains(fromType))
            {
                // caluclate interfaceRouteIndex
                var interfaceRouteIndex = 0;
                var index = 1; // + BaseType
                foreach (var @interface in toType.GetInterfaces())
                {
                    if (@interface.GetAllInterfaces().Contains(fromType))
                    {
                        interfaceRouteIndex = index;
                        break;
                    }

                    index++;
                }

                if (interfaceRouteIndex >= 0 && toType.GetInterfaces().Contains(fromType))
                {
                    return -3;
                }

                return interfaceRouteIndex * PointerSize;
            }

            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        private static List<int> FindInterfaceIndexes(IType type, IType @interface, int index)
        {
            var indexes = new List<int>();

            var currentType = type;

            var baseCount = 0;
            while (currentType.BaseType != null && currentType.BaseType.GetAllInterfaces().Contains(@interface))
            {
                // add base index;
                indexes.Add(0);
                baseCount++;
                currentType = currentType.BaseType;
            }

            while (currentType != null)
            {
                var interfaceIndex = FindInterfaceIndexForOneStep(currentType, @interface, out currentType);
                var indexToAdd = indexes.Count > baseCount ? interfaceIndex : index + interfaceIndex;
                indexes.Add(indexToAdd);
            }

            return indexes;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        private static List<string> FindInterfacePath(IType type, IType @interface)
        {
            var indexes = new List<string>();

            var currentType = type;

            var baseCount = 0;
            while (currentType.BaseType != null && currentType.BaseType.GetAllInterfaces().Contains(@interface))
            {
                // add base index;
                indexes.Add("base");
                baseCount++;
                currentType = currentType.BaseType;
            }

            while (currentType != null)
            {
                var interfacePath = FindInterfacePathForOneStep(currentType, @interface, out currentType);
                indexes.Add(interfacePath.ToString().CleanUpName());
            }

            return indexes;
        }

        private int CalculateFieldIndex(IType type, string fieldName)
        {
            var list = Logic.IlReader.Fields(type, this).Where(t => !t.IsStatic).ToList();
            var index = 0;

            while (index < list.Count && !list[index].Name.Equals(fieldName))
            {
                index++;
            }

            Debug.Assert(index != list.Count, string.Format("Could not find field {0} in type {1}", fieldName, type));
            if (index == list.Count)
            {
                throw new KeyNotFoundException();
            }

            // no shift needed, it will be applied in WriteFieldAccess
            ////index += this.CalculateFirstFieldPositionInType(type);
            this.indexByFieldInfo[string.Concat(type.FullName, '.', fieldName)] = index;

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="fieldInfo">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        private int CalculateFieldPosition(IType type, IField fieldInfo)
        {
            var list = Logic.IlReader.Fields(type, this).Where(t => !t.IsStatic).ToList();
            var index = 0;

            while (index < list.Count && list[index].NameNotEquals(fieldInfo))
            {
                index++;
            }

            if (index == list.Count)
            {
                throw new KeyNotFoundException();
            }

            index += this.CalculateFirstFieldPositionInType(type);

            this.poisitionByFieldInfo[fieldInfo.GetFullName()] = index;

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeFieldInfoPart">
        /// </param>
        private void FieldAccessAndSaveToField(OpCodeFieldInfoPart opCodeFieldInfoPart)
        {
            this.WriteFieldAccess(opCodeFieldInfoPart);

            var fieldType = opCodeFieldInfoPart.Operand.FieldType;

            this.SaveToField(opCodeFieldInfoPart, fieldType);
        }

        /// <summary>
        /// </summary>
        /// <param name="parameterIndex">
        /// </param>
        /// <param name="argIndex">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetArgVarName(int parameterIndex, int argIndex)
        {
            return this.GetArgVarName(this.Parameters[parameterIndex], argIndex);
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetArgVarName(IParameter parameter, int index)
        {
            return this.GetArgVarName(parameter.Name, index);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private string GetGlobalConstructorsFunctionName()
        {
            return this.GetGlobalConstructorsFunctionName(this.AssemblyQualifiedName);
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyQualifiedName">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetGlobalConstructorsFunctionName(string assemblyQualifiedName)
        {
            return string.Concat("_gctors_for_", assemblyQualifiedName.CleanUpName());
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void LoadElement(CIndentedTextWriter writer, OpCodePart opCode)
        {
            var actualLoad = true;
            IType type = null;
            switch (opCode.ToCode())
            {
                case Code.Ldelem_I:

                    // type = this.System.System_Int32;
                    type = this.GetTypeOfReference(opCode);
                    break;
                case Code.Ldelem_I1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.System.System_SByte;
                    var result = this.EstimatedResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.GetElementType();
                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = this.System.System_SByte;
                    }

                    break;
                case Code.Ldelem_I2:
                    type = this.System.System_Int16;
                    break;
                case Code.Ldelem_I4:
                    type = this.System.System_Int32;
                    break;
                case Code.Ldelem_U1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.System.System_Byte;
                    result = this.EstimatedResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.GetElementType();
                    break;
                case Code.Ldelem_U2:
                    type = this.System.System_UInt16;
                    break;
                case Code.Ldelem_U4:
                    type = this.System.System_UInt32;
                    break;
                case Code.Ldelem_I8:
                    type = this.System.System_Int64;
                    break;
                case Code.Ldelem_R4:
                    type = this.System.System_Single;
                    break;
                case Code.Ldelem_R8:
                    type = this.System.System_Double;
                    break;
                case Code.Ldelem:
                case Code.Ldelem_Ref:
                    type = this.GetTypeOfReference(opCode);
                    break;
                case Code.Ldelema:
                    actualLoad = false;
                    break;
            }

            Debug.Assert(!type.IsVoid());

            this.LoadElement(writer, opCode, "data", type, opCode.OpCodeOperands[1], actualLoad);
        }

        private void LoadObject(OpCodeTypePart opCodeType, int operandIndex)
        {
            var estimatedResult = EstimatedResultOf(opCodeType.OpCodeOperands[0]);
            if (estimatedResult.IsReference)
            {
                this.LoadIndirect(this.Output, opCodeType, opCodeType.Operand);
                return;
            }

            this.WriteOperandResultOrActualWrite(this.Output, opCodeType, operandIndex);
        }

        private void SaveObject(OpCodeTypePart opCodeTypePart, int operandIndex)
        {
            var estimatedResult = EstimatedResultOf(opCodeTypePart.OpCodeOperands[0]);
            if (estimatedResult.IsReference)
            {
                this.SaveIndirect(this.Output, opCodeTypePart, opCodeTypePart.Operand);
                return;
            }

            this.UnaryOper(this.Output, opCodeTypePart, 0, string.Empty, opCodeTypePart.Operand);
            this.UnaryOper(this.Output, opCodeTypePart, operandIndex, " = ", opCodeTypePart.Operand);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void SaveElement(CIndentedTextWriter writer, OpCodePart opCode)
        {
            IType type = null;

            switch (opCode.ToCode())
            {
                case Code.Stelem_I:

                    // type = this.System.System_Void.ToPointerType();
                    type = this.GetTypeOfReference(opCode);
                    break;
                case Code.Stelem_I1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.System.System_SByte;
                    var result = this.EstimatedResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.GetElementType();
                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = this.System.System_SByte;
                    }

                    break;
                case Code.Stelem_I2:
                    type = this.System.System_Int16;
                    break;
                case Code.Stelem_I4:
                    type = this.System.System_Int32;
                    break;
                case Code.Stelem_I8:
                    type = this.System.System_Int64;
                    break;
                case Code.Stelem_R4:
                    type = this.System.System_Single;
                    break;
                case Code.Stelem_R8:
                    type = this.System.System_Double;
                    break;
                case Code.Stelem:
                case Code.Stelem_Ref:
                    type = this.GetTypeOfReference(opCode);
                    break;
            }

            if (type.UseAsClass)
            {
                type = type.ToNormal();
            }

            Debug.Assert(!type.IsVoid());

            var estimatedResultOf = this.EstimatedResultOf(opCode.OpCodeOperands[0]);
            this.WriteFieldAccess(writer, opCode, estimatedResultOf.Type.GetFieldByName("data", this), opCode.OpCodeOperands[1]);

            var operandIndex = 2;

            writer.Write(" = ");

            this.WriteOperandResultOrActualWrite(writer, opCode, operandIndex);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void SaveIndirect(CIndentedTextWriter writer, OpCodePart opCode)
        {
            IType type = null;

            switch (opCode.ToCode())
            {
                case Code.Stind_I:
                    type = this.GetTypeOfReference(opCode);
                    break;
                case Code.Stind_I1:

                    ////type = this.System.System_Byte;

                    // it can be Bool or Byte, leave it null
                    ////type = this.System.System_SByte;
                    var result = this.EstimatedResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.HasElementType ? result.Type.GetElementType() : result.Type;
                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = this.System.System_SByte;
                    }

                    break;
                case Code.Stind_I2:
                    type = this.System.System_Int16;
                    break;
                case Code.Stind_I4:
                    type = this.System.System_Int32;
                    break;
                case Code.Stind_I8:
                    type = this.System.System_Int64;
                    break;
                case Code.Stind_R4:
                    type = this.System.System_Single;
                    break;
                case Code.Stind_R8:
                    type = this.System.System_Double;
                    break;
                case Code.Stind_Ref:
                    type = this.GetTypeOfReference(opCode);
                    break;
            }

            this.SaveIndirect(writer, opCode, type);
        }

        private void SaveIndirect(CIndentedTextWriter writer, OpCodePart opCode, IType type)
        {
            Debug.Assert(!type.IsVoid());

            writer.Write("*(");

            this.WriteCCastOnly(type.ToPointerType());

            var resultOfOperand0 = this.EstimatedResultOf(opCode.OpCodeOperands[0]);
            var destinationType = resultOfOperand0.Type;
            if (destinationType.IsByRef && destinationType.GetElementType().TypeNotEquals(type))
            {
                type = destinationType.GetElementType();
            }

            this.UnaryOper(writer, opCode, 0, string.Empty);
            this.UnaryOper(writer, opCode, 1, ") = ", type);
        }

        private void SetSettings(string fileName, string fileExt, string sourceFilePath, string pdbFilePath, string[] args)
        {
            var extension = Path.GetExtension(fileName);
            this.outputFile = extension != null && extension.Equals(string.Empty) ? fileName + fileExt : fileName;

            this.ReadParameters(sourceFilePath, pdbFilePath, args);
        }

        /// <summary>
        /// </summary>
        private void SortStaticConstructorsByUsage()
        {
            var staticConstructors = new Dictionary<IMethod, ISet<IType>>();
            foreach (var staticCtor in this.IlReader.StaticConstructors)
            {
                var methodWalker = new MethodsWalker(staticCtor, this);
                var reaquiredTypesWithStaticFields = methodWalker.DiscoverAllStaticFieldsDependencies();
                staticConstructors.Add(staticCtor, reaquiredTypesWithStaticFields);
            }

            // rebuild order
            var newStaticConstructors = new List<IMethod>();

            var countBefore = 0;
            do
            {
                countBefore = staticConstructors.Count;
                foreach (
                    var staticConstructorPair in
                        staticConstructors.Where(
                            staticConstructorPair => !staticConstructorPair.Value.Any(v => staticConstructors.Keys.Any(k => k.DeclaringType.TypeEquals(v))))
                                          .ToList())
                {
                    staticConstructors.Remove(staticConstructorPair.Key);
                    newStaticConstructors.Add(staticConstructorPair.Key);
                }
            }
            while (staticConstructors.Count > 0 && countBefore != staticConstructors.Count);

            Debug.Assert(staticConstructors.Keys.Count == 0, "Not All static constructors were resolved");

            // add rest as is
            newStaticConstructors.AddRange(staticConstructors.Keys);

            this.IlReader.StaticConstructors = newStaticConstructors;
        }

        /// <summary>
        /// </summary>
        private void WriteConstBytes(IConstBytes constBytes)
        {
            var bytes = constBytes.Data;

            this.Output.Write(this.declarationPrefix);
            this.Output.Write(
                "const struct {1} {0} = {3} {2}",
                constBytes.Reference,
                this.GetArrayTypeHeader(this.System.System_Byte, bytes.Length),
                this.GetArrayValuesHeader(this.System.System_Byte, bytes.Length, bytes.Length),
                "{");

            this.Output.Write("{ ");

            var index = 0;
            foreach (var b in bytes)
            {
                if (index > 0)
                {
                    this.Output.Write(", ");
                }

                this.Output.Write("{0}", (int)b);
                index++;
            }

            this.Output.WriteLine(" {0} {0};", '}');
        }

        /// <summary>
        /// </summary>
        private void WriteGctorsForwardDeclarations()
        {
            // get all references
            foreach (var reference in this.AllReferences.Reverse().Distinct())
            {
                this.Output.WriteLine("Void {0}();", this.GetGlobalConstructorsFunctionName(reference));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCatchBegins(OpCodePart opCode)
        {
            if (opCode.CatchOrFinallyBegin == null)
            {
                return;
            }

            var exceptionHandler = opCode.CatchOrFinallyBegin;
            opCode.CatchOrFinallyBegin = null;
            this.catchScopes.Push(exceptionHandler);

            if (exceptionHandler.Flags == ExceptionHandlingClauseOptions.Clause)
            {
                this.WriteCatchTest(exceptionHandler, exceptionHandler.Next);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        private void WriteCatchFinnallyCleanUpEnd(OpCodePart opCode)
        {
            if (opCode.CatchOrFinallyEnds == null)
            {
                return;
            }

            var eh = opCode.CatchOrFinallyEnds;
            opCode.CatchOrFinallyEnds = null;
            var ehPopped = this.catchScopes.Pop();
            Debug.Assert(ehPopped == eh, "Mismatch of exception handlers");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCatchFinallyEnd(CIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.CatchOrFinallyEnds == null)
            {
                return;
            }

            var ehs = opCode.CatchOrFinallyEnds;
            opCode.CatchOrFinallyEnds = null;
            writer.WriteLine(string.Empty);
            foreach (var eh in ehs)
            {
                var upperLevelExceptionHandlingClause = this.GetCurrentLevelExceptionHandlingClause();
                this.WriteCatchEnd(opCode, eh, upperLevelExceptionHandlingClause);
            }
        }

        private CatchOfFinallyClause GetCurrentLevelExceptionHandlingClause()
        {
            return this.tryScopes.Count > 0
                ? this.tryScopes.Peek()
                    .Catches.FirstOrDefault(c => c.Flags == ExceptionHandlingClauseOptions.Clause)
                  ?? this.tryScopes.Peek()
                      .Catches.FirstOrDefault(c => c.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                : null;
        }

        private CatchOfFinallyClause GetUpperLevelExceptionHandlingClause()
        {
            return this.tryScopes.Count > 1
                ? this.tryScopes.Skip(1).First()
                    .Catches.FirstOrDefault(c => c.Flags == ExceptionHandlingClauseOptions.Clause)
                  ?? this.tryScopes.Skip(1).First()
                      .Catches.FirstOrDefault(c => c.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                : null;
        }

        private void WriteConvertToNativeInt(OpCodePart opCode)
        {
            var intPtrOper = IntTypeRequired(opCode);
            var nativeIntType = intPtrOper ? this.System.System_Int32 : this.System.System_Void.ToPointerType();

            var estimatedResultOfOperand0 = this.EstimatedResultOf(opCode.OpCodeOperands[0]);
            if (!estimatedResultOfOperand0.Type.IsPointer && !estimatedResultOfOperand0.Type.IsByRef)
            {
                this.WriteCCastOperand(opCode, 0, nativeIntType);
            }
            else
            {
                this.WriteResultOrActualWrite(this.Output, opCode.OpCodeOperands[0]);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteExceptionHandlersProlog(OpCodePart opCode)
        {
            if (opCode.ExceptionHandlers == null)
            {
                return;
            }

            this.WriteCatchProlog(opCode);
        }

        /// <summary>
        /// </summary>
        private void WriteGlobalConstructors()
        {
            if (this.IsSplit && !string.IsNullOrWhiteSpace(this.SplitNamespace))
            {
                // for multi source we generate it only in empty namespace
                return;
            }

            // write global ctors caller
            this.Output.WriteLine(string.Empty);
            this.Output.WriteLine("Void {0}() {1}", this.GetGlobalConstructorsFunctionName(), "{");
            this.Output.Indent++;

            this.SortStaticConstructorsByUsage();

            if (this.Gc && this.IsCoreLib)
            {
                this.Output.WriteLine("GC_INIT();");
            }

            foreach (var staticCtor in this.IlReader.StaticConstructors)
            {
                WriteMethodDefinitionName(this.Output, staticCtor);
                this.Output.WriteLine("();");
            }

            this.Output.Indent--;
            this.Output.WriteLine("}");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="classType">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <exception cref="IndexOutOfRangeException">
        /// </exception>
        /// <returns>
        /// </returns>
        private void WriteInterfacePath(IType classType, IType @interface, IField fieldInfo)
        {
            var writer = this.Output;

            var type = classType;
            if (type.GetAllInterfaces().Contains(@interface))
            {
                while (!type.GetInterfacesExcludingBaseAllInterfaces().Any(i => i.TypeEquals(@interface) || i.GetAllInterfaces().Contains(@interface)))
                {
                    type = type.BaseType;
                    if (type == null)
                    {
                        // throw new IndexOutOfRangeException("Could not find an type");
                        break;
                    }

                    // first index is base type index
                    writer.Write("base.");
                }

                var path = FindInterfacePath(type, @interface);

                for (var i = 0; i < path.Count; i++)
                {
                    if (path[i] != "base")
                    {
                        writer.Write("ifce_");
                    }

                    writer.Write(path[i]);

                    if (fieldInfo != null || i < path.Count - 1)
                    {
                        writer.Write(".");
                    }
                }
            }

            if (fieldInfo != null)
            {
                writer.Write(fieldInfo.Name);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteLabels(CIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.JumpDestination != null && opCode.JumpDestination.Count > 0 && !opCode.JumpProcessed)
            {
                this.WriteLabel(writer, string.Concat("a", opCode.AddressStart));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="locals">
        /// </param>
        private void WriteLocalVariableDeclarations(IEnumerable<ILocalVariable> locals)
        {
            foreach (var local in locals)
            {
                local.LocalType.WriteTypePrefix(this);
                this.Output.Write(" ");
                this.Output.Write(this.GetLocalVarName(local.LocalIndex));
                this.Output.WriteLine(";");
            }
        }

        /// <summary>
        /// </summary>
        private void WriteMainFunction()
        {
            var ilCodeBuilder = new IlCodeBuilder();

            if (!this.Gctors)
            {
                // call all gtors manually
                foreach (var reference in this.AllReferences.Distinct().Reverse())
                {
                    ilCodeBuilder.Call(new SynthesizedMethodStringAdapter(this.GetGlobalConstructorsFunctionName(reference), string.Empty, System.System_Void));
                }
            }

            var mainSynthMethod = MainGen.GetMainMethodBody(ilCodeBuilder, this.MainMethod, this);

            this.WriteMethod(new SynthesizedMainMethod(mainSynthMethod, this.MainMethod, this), null, null);
        }

        private void WriteMethodBeginning(IMethod method, IGenericContext genericContext)
        {
            if (method.IsAbstract || method.IsSkipped())
            {
                return;
            }

            this.WriteMethodRequiredStringsAndConstArraysDefinitions();

            // after WriteMethodRequiredDeclatations which removed info about current method we need to reread info about method
            this.ReadMethodInfo(method, genericContext);

            // debug info
            this.WriteDebugInfoForMethod(method);

            if (this.WriteMethodProlog(method))
            {
                return;
            }

            bool isDelegateBodyFunctions = method.IsDelegateFunctionBody();
            // write local declarations
            var methodBodyBytes = method.ResolveMethodBody(genericContext);
            if (methodBodyBytes.HasBody)
            {
                this.Output.WriteLine(" {");
                this.Output.Indent++;

                this.WriteLocalVariableDeclarations(methodBodyBytes.LocalVariables);

                this.Output.StartMethodBody();
            }
            else if (isDelegateBodyFunctions)
            {
                this.WriteDelegateStubFunctionBody(method);
                this.Output.WriteLine(string.Empty);
            }
            else if (!this.Stubs)
            {
                this.Output.WriteLine(";");
            }
        }

        private void WriteDebugInfoForMethod(IMethod method)
        {
            if (!this.DebugInfo)
            {
                return;
            }

            if (method.Token.HasValue)
            {
                this.debugInfoGenerator.GenerateFunction(method.Token.Value);
                // to find first debug line of method
                this.ReadDbgLine(OpCodePart.CreateNop);
            }
            else
            {
                this.debugInfoGenerator.GenerateFunction(-1);
            }

            if (IsMain(method))
            {
                this.MainDebugInfoStartLine = this.debugInfoGenerator.CurrentDebugLine ?? 1;
            }
        }

        public bool WriteTemplateDeclaration(IType type)
        {
            if (type == null)
            {
                return false;
            }

            if (!type.IsGenericType && !type.IsGenericTypeDefinition && !type.IsArray)
            {
                return false;
            }

            this.Output.Write("template < typename _T_ > ");

            return true;
        }

        public void WriteTemplateDeclaration(IMethod method)
        {
            if (!this.WriteTemplateDeclaration(method.DeclaringType) && (method.IsGenericMethod || method.IsGenericMethodDefinition))
            {
                this.Output.Write("template < typename _T_ > ");                
            }
        }

        public void WriteTemplateSpecializationDeclaration(IType type)
        {
            if (!type.IsGenericType && !type.IsGenericTypeDefinition && !type.IsArray)
            {
                return;
            }

            this.Output.Write("<_T_>");
        }

        private bool WriteTemplateSpecializationDefinition(IType type)
        {
            if (!type.IsGenericType && !type.IsGenericTypeDefinition && !type.IsArray)
            {
                return false;
            }

            this.Output.Write("<void>");
            return true;
        }

        private void WriteTemplateSpecializationDefinition(IMethod method)
        {
            if (!WriteTemplateSpecializationDefinition(method.DeclaringType) && (method.IsGenericMethod || method.IsGenericMethodDefinition))
            {
                this.Output.Write("<void>");
            }
        }

        public void WriteTypeNamespaceStart(IType type)
        {
            if (!string.IsNullOrWhiteSpace(type.Namespace))
            {
                foreach (var part in type.Namespace.Split('.'))
                {
                    this.Output.Write("namespace ");
                    this.Output.Write(part);
                    this.Output.Indent++;
                    this.Output.Write(" { ");
                }
            }

            this.Output.WriteLine(string.Empty);
        }

        public void WriteTypeNamespaceEnd(IType type)
        {
            if (!string.IsNullOrWhiteSpace(type.Namespace))
            {
                foreach (var part in type.Namespace.Split('.'))
                {
                    this.Output.Indent--;
                    this.Output.WriteLine("}");
                }
            }
        }

        private bool WriteMethodProlog(IMethod method, bool excludeNamespace = false, bool externDecl = false, bool shortName = true)
        {
            var isDelegateBodyFunctions = method.IsDelegateFunctionBody();
            if ((method.IsAbstract || (this.NoBody && !this.Stubs)) && !isDelegateBodyFunctions)
            {
                if (!method.IsUnmanagedMethodReference && this.methodsHaveDefinition.Contains(method))
                {
                    return true;
                }

                if (!externDecl)
                {
                    if (method.IsUnmanagedDllImport || method.IsUnmanaged || method.IsUnmanagedMethodReference)
                    {
                        this.externDeclarations.Add(method);
                        return true;
                    }
                }
                else
                {
                    excludeNamespace = true;
                    this.WriteMethodExternPrefix(method);
                }
            }

            if (!externDecl && excludeNamespace)
            {
                WriteTemplateDeclaration(method);
                ////this.Output.Write("static ");
            }
            else if (!Stubs && NoBody && !externDecl)
            {
                return true;
            }
            else
            {
                WriteTemplateDeclaration(method);
            }

            if (method.DllImportData != null && method.DllImportData.CallingConvention == CallingConvention.StdCall)
            {
                this.Output.Write("__stdcall ");
            }

            // return type
            this.WriteMethodReturnType(this.Output, method);

            if (method.IsUnmanagedMethodReference)
            {
                this.Output.Write("(*");
            }

            // name
            this.WriteMethodDefinitionName(
                this.Output,
                method,
                shortName: shortName,
                excludeNamespace: excludeNamespace,
                specialization: ApplyGeneric.NotApplied);

            if (method.IsUnmanagedMethodReference)
            {
                this.Output.Write(")");
            }

            this.WriteMethodParamsDef(
                this.Output,
                method,
                this.HasMethodThis,
                this.ThisType,
                method.ReturnType,
                method.IsUnmanagedMethodReference);

            return false;
        }

        private void WriteMethodExternPrefix(IMethod method)
        {
            if (method.IsUnmanagedDllImport)
            {
                this.Output.Write("extern \"C\" __declspec( dllimport ) ");
            }
            else if (method.IsUnmanaged || method.IsUnmanagedMethodReference)
            {
                this.Output.Write("extern \"C\" ");
            }
        }

        private void ReadDbgLine(OpCodePart opCode)
        {
            if (!this.DebugInfo)
            {
                return;
            }

            this.debugInfoGenerator.ReadAndSetCurrentDebugLine(opCode.AddressStart);
        }

        /// <summary>
        /// </summary>
        /// <param name="endPart">
        /// </param>
        private void WriteMethodBody(IEnumerable<OpCodePart> rest)
        {
            // Temp hack to delcare all temp variables;
            foreach (var opCodePart in rest.Where(opCodePart => opCodePart.Any(Code.Dup)))
            {
                this.WriteVariableDeclare(opCodePart, opCodePart.RequiredOutgoingType, "_dup");
            }

            this.IterateMethodBodyOpCodes(rest);
        }

        private void IterateMethodBodyOpCodes(IEnumerable<OpCodePart> rest)
        {
            var enumerator = rest.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return;
            }

            var item = enumerator.Current;
            var currentAddress = -1;
            while (item != null)
            {
                // move next item, we need next code to write address lables properly
                if (item.AddressEnd != 0)
                {
                    currentAddress = item.AddressStart;
                }

                while (currentAddress >= enumerator.Current.AddressStart)
                {
                    this.ReadDbgLine(enumerator.Current);
                    this.WriteLabels(this.Output, enumerator.Current);

                    if (this.DebugInfo && !this.DebugInfoNoLineGenerating && this.debugInfoGenerator.CurrentDebugLineNew)
                    {
                        this.WriteDebugLine();
                    }

                    if (!enumerator.MoveNext())
                    {
                        break;
                    }
                }

                // we need to preprocess all virtual OpCodes before
                this.ActualWriteForVirtualOpCodes(item);

                this.ActualWrite(this.Output, item, true);

                Debug.Assert(item != item.Next, "circular reference detected");
                Debug.Assert(
                    item.Next == null || item.AddressEnd == 0 || item.Next.AddressEnd == 0 || item.AddressStart <= item.Next.AddressStart,
                    "circular reference detected");

                item = item.Next;
            }
        }

        private void ActualWriteForVirtualOpCodes(OpCodePart item)
        {
            if (item.OpCodeOperands == null)
            {
                return;
            }

            foreach (var opCodeOperand in item.OpCodeOperands.Where(opCodeOperand => opCodeOperand.IsVirtual()))
            {
                ActualWriteForVirtualOpCodes(opCodeOperand);

                bool isVirtualCall;
                if (this.OpCodeWithVariableDeclaration(opCodeOperand, out isVirtualCall))
                {
                    this.ActualWrite(this.Output, opCodeOperand, true);
                }
            }
        }

        public void WriteMethodForwardDeclaration(IMethod methodDecl, IType ownerOfExplicitInterface, IGenericContext genericContext)
        {
            this.WriteMethodForwardDeclarationIfNotWrittenYet(new MethodKey(methodDecl, ownerOfExplicitInterface), genericContext);
        }

        private void WriteMethodForwardDeclarationIfNotWrittenYet(MethodKey methodKey, IGenericContext genericContext)
        {
            var ctor = methodKey.Method as IConstructor;
            if (ctor != null)
            {
                this.ReadMethodInfo(ctor, genericContext);
                this.WriteMethodProlog(ctor, true);
                this.Output.WriteLine(";");
                return;
            }

            var method = methodKey.Method;
            if (method != null)
            {
                this.ReadMethodInfo(method, genericContext);
                if (!this.WriteMethodProlog(method, true))
                {
                    this.Output.WriteLine(";");
                }
            }
        }

        private void WriteMethodRequiredStringsAndConstArraysDefinitions()
        {
            // const strings
            foreach (var usedString in this.IlReader.UsedStrings)
            {
                this.WriteUnicodeString(usedString);
            }

            if (this.IlReader.UsedStrings.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
            }

            // const bytes
            foreach (var usedConstBytes in this.IlReader.UsedConstBytes)
            {
                this.WriteConstBytes(usedConstBytes);
            }

            if (this.IlReader.UsedConstBytes.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeConstructorInfoPart">
        /// </param>
        private void WriteNewObject(OpCodeConstructorInfoPart opCodeConstructorInfoPart)
        {
            var declaringType = opCodeConstructorInfoPart.Operand.DeclaringType;
            this.WriteNew(opCodeConstructorInfoPart, declaringType);
        }

        private void WriteNewObject(OpCodePart opCodePart, IType declaringType)
        {
            this.WriteNew(opCodePart, declaringType);
        }

        private void WriteNewSingleArray(OpCodeTypePart opCodeTypePart)
        {
            var arrayType = opCodeTypePart.Operand.ToArrayType(1);

            var objectReference = this.WriteVariableForNew(opCodeTypePart, arrayType, "_newarr");

            this.WriteNewWithCallingConstructor(opCodeTypePart, arrayType, this.System.System_Int32, opCodeTypePart.OpCodeOperands[0], objectReference);
        }

        public FullyDefinedReference WriteVariableForNew(OpCodePart opCodePart, IType type, string name = "_new")
        {
            // temp var
            type.WriteTypePrefix(this);
            var newVar = string.Format("{1}{0}", opCodePart.AddressStart, name);
            this.Output.WriteLine(" {0};", newVar);

            this.Output.Write("{0} = ", newVar);

            var objectReference = new FullyDefinedReference(newVar, type);
            opCodePart.Result = objectReference;
            return objectReference;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        private void WritePostMethodEnd(IMethod method)
        {
            var stubFunc = this.Stubs && this.NoBody && !method.IsAbstract && !method.IsSkipped() && !method.IsDelegateFunctionBody();
            if (stubFunc)
            {
                this.DefaultStub(method);
            }

            if (!this.NoBody)
            {
                this.Output.Indent--;
                this.Output.EndMethodBody();

                this.Output.WriteLine("}");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <param name="externalRef">
        /// </param>
        public void WriteStaticField(IField field, bool definition = true)
        {
            var fieldType = field.FieldType;

            if (!definition)
            {
                this.Output.Write("static ");
            }

            if (definition)
            {
                WriteTemplateDeclaration(field.DeclaringType);
            }

            fieldType.WriteTypePrefix(this);

            this.Output.Write(" ");
            this.WriteStaticFieldName(field, excludeNamespace: !definition, specialization: definition ? ApplyGeneric.Declaration : ApplyGeneric.NotApplied);
            if (definition)
            {
                this.WriteStaticFieldInitialization(field);
            }

            this.Output.WriteLine(";");
        }

        private void WriteStaticFieldInitialization(IField field)
        {
            var fieldType = field.FieldType;

            if (fieldType.IsStructureType())
            {
                this.Output.Write(" = ");
                if (fieldType.IsStaticArrayInit)
                {
                    var staticArrayInitSize = fieldType.GetStaticArrayInitSize();
                    this.Output.Write("{ (Void**) 0");
                    this.Output.Write(", { ");
                    var data = field.GetFieldRVAData();
                    var index = 0;
                    foreach (var b in data.Take(staticArrayInitSize))
                    {
                        if (index++ > 0)
                        {
                            this.Output.Write(", ");
                        }

                        this.Output.Write(b);
                    }

                    this.Output.Write("} }");
                }
                else
                {
                    fieldType.ToClass().WriteTypeWithoutModifiers(this);
                    this.Output.Write("()/*undef*/");
                }
            }
            else if (fieldType.IsValueType() && field.GetFieldRVAData() != null)
            {
                var data = field.GetFieldRVAData();
                this.Output.Write(" = ");
                switch (fieldType.IntTypeBitSize())
                {
                    case 8:
                        this.Output.Write(data[0]);
                        break;
                    case 16:
                        this.Output.Write(BitConverter.ToInt16(data, 0));
                        break;
                    case 32:
                        this.Output.Write(BitConverter.ToInt32(data, 0));
                        break;
                    case 64:
                        this.Output.Write(BitConverter.ToInt64(data, 0));
                        break;
                }
            }
            else
            {
                this.Output.Write(" = 0/*undef*/");
            }
        }

        private void WriteStaticFieldName(IField field, bool excludeNamespace = false, ApplyGeneric specialization = ApplyGeneric.Declaration)
        {
            var writer = this.Output;

            if (!excludeNamespace)
            {
                if (specialization == ApplyGeneric.Specialization)
                {
                    writer.Write("::");
                }

                var ns = field.DeclaringType.Namespace;
                if (!string.IsNullOrWhiteSpace(ns))
                {
                    writer.Write(ns.Replace(".", "::"));
                    writer.Write("::");
                }

                this.WriteClassName(field.DeclaringType);
                if (!excludeNamespace)
                {
                    switch (specialization)
                    {
                        case ApplyGeneric.Declaration:
                            WriteTemplateSpecializationDeclaration(field.DeclaringType);
                            break;
                        case ApplyGeneric.Specialization:
                            WriteTemplateSpecializationDefinition(field.DeclaringType);
                            break;
                    }
                }

                writer.Write("::");
            }

            this.Output.Write(field.Name.CleanUpName());
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteTryBegins(CIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.TryBegin == null || opCode.TryBegin.Count <= 0)
            {
                return;
            }

            var ehs = opCode.TryBegin.ToArray();
            opCode.TryBegin.Clear();
            Array.Sort(ehs);
            foreach (var eh in ehs.Reverse())
            {
                this.tryScopes.Push(eh);
                writer.WriteTry(eh);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteTryEnds(OpCodePart opCode)
        {
            if (opCode.TryEnd == null)
            {
                return;
            }

            var eh = opCode.TryEnd;
            opCode.TryEnd = null;
            var ehPopped = this.tryScopes.Pop();
            Debug.Assert(ehPopped == eh, "Mismatch of exception handlers");
        }

        public void StartPreprocessorIf(IType type, string prefix)
        {
            if (type.IsGenericType || type.IsGenericTypeDefinition || type.IsArray)
            {
                var fullName = type.FullName.CleanUpName();
                this.Output.Write("#ifndef {0}__", prefix);
                this.Output.WriteLine(fullName);
                this.Output.Write("#define {0}__", prefix);
                this.Output.WriteLine(fullName);
            }
        }

        public void EndPreprocessorIf(IType type)
        {
            if (type.IsGenericType || type.IsGenericTypeDefinition || type.IsArray)
            {
                this.Output.WriteLine("#endif");
            }
        }

        private void StartPreprocessorIf(string id, string prefix)
        {
            this.Output.Write("#ifndef {0}__{1}", prefix, id);
            this.Output.WriteLine(string.Empty);
            this.Output.Write("#define {0}__{1}", prefix, id);
            this.Output.WriteLine(string.Empty);
        }

        private void EndPreprocessorIf()
        {
            this.Output.WriteLine("#endif");
        }

        /// <summary>
        /// </summary>
        /// <param name="pair">
        /// </param>
        private void WriteUnicodeString(KeyValuePair<int, string> pair)
        {
            if (!this.stringTokenDefinitionWritten.Add(pair.Key))
            {
                return;
            }

            var align = pair.Value.Length % 2 == 0;

            this.Output.Write(this.declarationPrefix);
            this.Output.Write(
                "const struct {1} _s{0}_ = {3} {2}",
                pair.Key,
                this.GetStringTypeHeader(pair.Value.Length + (align ? 2 : 1)),
                this.GetStringValuesHeader(pair.Value.Length + (align ? 3 : 2), pair.Value.Length),
                "{");

            this.Output.Write("{ ");

            var index = 0;
            foreach (var c in pair.Value.ToCharArray())
            {
                if (index > 0)
                {
                    this.Output.Write(", ");
                }

                this.Output.Write("{0}", (int)c);
                index++;
            }

            if (index > 0)
            {
                this.Output.Write(", ");
            }

            if (align)
            {
                this.Output.Write("0, ");
            }

            this.Output.WriteLine("0 {0} {0};", '}');
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteVirtualTableImplementations(IType type)
        {
            // write VirtualTable
            if (type.IsInterface)
            {
                return;
            }

            this.StartPreprocessorIf(type, "VTIMPL");

            // TODO: review next line (use sizeof)
            var baseTypeSize = type.BaseType != null ? type.BaseType.GetTypeSize(this) : 0;

            var index = 0;
            if (type.HasAnyVirtualMethod(this))
            {
                var virtualTable = type.GetVirtualTable(this);

                virtualTable.WriteTableOfMethodsWithImplementation(this, type, 0, baseTypeSize);
                index++;
                this.Output.WriteLine(string.Empty);
            }

            foreach (var @interface in type.SelectAllTopAndAllNotFirstChildrenInterfaces(null).Distinct())
            {
                var current = type;
                IType typeContainingInterface = null;
                while (current != null && current.GetAllInterfaces().Contains(@interface))
                {
                    typeContainingInterface = current;
                    current = current.BaseType;
                }

                var baseTypeSizeOfTypeContainingInterface = typeContainingInterface.BaseType != null ? typeContainingInterface.BaseType.GetTypeSize(this) : 0;
                var interfaceIndex = FindInterfaceIndexes(typeContainingInterface, @interface, index).Sum();

                var virtualInterfaceTable = type.GetVirtualInterfaceTable(@interface, this);

                virtualInterfaceTable.WriteTableOfMethodsWithImplementation(this, type, interfaceIndex, baseTypeSizeOfTypeContainingInterface, @interface);

                this.Output.WriteLine(string.Empty);
            }

            this.EndPreprocessorIf(type);
        }

        private void WriteVirtualTable(IType typeParam)
        {
            if (typeParam.IsEnum)
            {
                return;
            }

            var table = typeParam.ToVirtualTable();
            var type = typeParam.ToNormal();

            var writer = this.Output;

            StartPreprocessorIf(table, "VTBL");

            WriteTypeNamespaceStart(table);

            writer.Write("struct ");
            WriteClassName(table);
            writer.WriteLine("{0} {1}", VTable, "{");
            writer.Indent++;

            if (!type.IsInterface)
            {
                var virtualTable = type.GetVirtualTable(this);

                foreach (var method in virtualTable.Select(v => v.Value))
                {
                    this.WriteMethodPointerType(writer, method, withName: true, shortName: false, excludeNamespace: true);
                    writer.WriteLine(";");
                }
            }
            else
            {
                var usedMethods = new HashSet<IMethod>();

                var virtualTable = type.GetVirtualInterfaceTableLayout(this);

                var index = 0;

                foreach (var method in virtualTable)
                {
                    var suffix = usedMethods.Contains(method) ? string.Concat("_redef_", index) : null;
                    this.WriteMethodPointerType(writer, method, withName: true, shortName: false, excludeNamespace: true, suffix: suffix);
                    writer.WriteLine(";");

                    usedMethods.Add(method);
                    index++;
                }

                foreach (var @interface in type.SelectAllTopAndAllNotFirstChildrenInterfaces(null).Skip(1))
                {
                    var virtualTableOfSecondaryInterface = @interface.GetVirtualInterfaceTableLayout(this);
                    foreach (var method in virtualTableOfSecondaryInterface)
                    {
                        var suffix = usedMethods.Contains(method) ? string.Concat("_redef_", index) : null;
                        this.WriteMethodPointerType(writer, method, withName: true, shortName: false, excludeNamespace: true, suffix: suffix);
                        writer.WriteLine(";");

                        usedMethods.Add(method);
                        index++;
                    }
                }
            }

            writer.Indent--;
            writer.WriteLine("};");

            WriteTypeNamespaceEnd(table);

            this.EndPreprocessorIf(table);
        }

        /// <summary>
        /// </summary>
        [Flags]
        [Obsolete("this must be removed")]
        public enum OperandOptions
        {
            /// <summary>
            /// </summary>
            None = 0,

            /// <summary>
            /// </summary>
            GenerateResult = 1,

            /// <summary>
            /// </summary>
            Template = 8,

            /// <summary>
            /// </summary>
            TypeIsInOperator = 16,

            /// <summary>
            /// </summary>
            AppendPointer = 64,

            /// <summary>
            /// </summary>
            IgnoreOperand = 128,

            /// <summary>
            /// </summary>
            DetectAndWriteTypeInSecondOperand = 256,

            /// <summary>
            /// </summary>
            CastPointersToBytePointer = 512,

            /// <summary>
            /// </summary>
            AdjustIntTypes = 1024
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="K">
        /// </typeparam>
        /// <typeparam name="V">
        /// </typeparam>
        public class Pair<K, V>
        {
            /// <summary>
            /// </summary>
            public K Key { get; set; }

            /// <summary>
            /// </summary>
            public V Value { get; set; }
        }
    }
}
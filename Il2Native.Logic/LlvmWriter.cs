// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LlvmWriter.cs" company="">
//   
// </copyright>
// <summary>
//   
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

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Exceptions;
    using Il2Native.Logic.Gencode;
    using Il2Native.Logic.Properties;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class LlvmWriter : BaseWriter, ICodeWriter
    {
        /// <summary>
        /// </summary>
        public static int PointerSize = 4;

        /// <summary>
        /// </summary>
        public Stack<CatchOfFinallyClause> catchScopes = new Stack<CatchOfFinallyClause>();

        /// <summary>
        /// </summary>
        public bool landingPadVariablesAreWritten;

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
        public readonly HashSet<IType> typeRttiDeclRequired = new HashSet<IType>();

        /// <summary>
        /// </summary>
        public readonly HashSet<IType> typeRttiPointerDeclRequired = new HashSet<IType>();

        /// <summary>
        /// </summary>
        private static int methodNumberIncremental;

        /// <summary>
        /// </summary>
        private int arrayIndexIncremental;

        /// <summary>
        /// </summary>
        private readonly IDictionary<int, byte[]> arrayStorage = new SortedDictionary<int, byte[]>();

        /// <summary>
        /// </summary>
        private int blockJumpAddressIncremental;

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> indexByFieldInfo = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IMethod> methodDeclRequired = new HashSet<IMethod>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IType> postDeclarationsProcessedTypes = new HashSet<IType>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IMethod> processedMethods = new HashSet<IMethod>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IType> processedRttiPointerTypes = new HashSet<IType>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IType> processedRttiTypes = new HashSet<IType>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IType> processedTypes = new HashSet<IType>();

        /// <summary>
        /// </summary>
        private int resultNumberIncremental;

        /// <summary>
        /// </summary>
        private LlvmIndentedTextWriter savedOutput;

        /// <summary>
        /// </summary>
        private readonly HashSet<IField> staticFieldExtrenalDeclRequired = new HashSet<IField>();

        /// <summary>
        /// </summary>
        private readonly StringBuilder storedText = new StringBuilder();

        /// <summary>
        /// </summary>
        private int stringIndexIncremental;

        /// <summary>
        /// </summary>
        private readonly IDictionary<int, string> stringStorage = new SortedDictionary<int, string>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IType> typeDeclRequired = new HashSet<IType>();

        /// <summary>
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <param name="args">
        /// </param>
        public LlvmWriter(string fileName, string[] args)
        {
            var extension = Path.GetExtension(fileName);
            var outputFile = extension != null && extension.Equals(string.Empty) ? fileName + ".ll" : fileName;
            this.Output = new LlvmIndentedTextWriter(new StreamWriter(outputFile));
            var targetArg = args != null ? args.FirstOrDefault(a => a.StartsWith("target:")) : null;
            this.Target = targetArg != null ? targetArg.Substring("target:".Length) : null;
            this.Gc = args != null && args.Contains("gc-") ? false : true;
            this.Gctors = args != null && args.Contains("gctors-") ? false : true;
        }

        /// <summary>
        /// </summary>
        public string Target { get; private set; }

        /// <summary>
        /// </summary>
        public bool Gc { get; private set; }

        /// <summary>
        /// </summary>
        public bool Gctors { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsCoreLib { get; private set; }

        /// <summary>
        /// </summary>
        public IEnumerable<string> AllReference { get; private set; }

        /// <summary>
        /// </summary>
        public LlvmIndentedTextWriter Output { get; private set; }

        /// <summary>
        /// if true - suppress ; at the end of line
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="firstLevel">
        /// </param>
        public void ActualWrite(LlvmIndentedTextWriter writer, OpCodePart opCode, bool firstLevel = false)
        {
            if (firstLevel)
            {
                this.WriteLabels(writer, opCode);
            }

            if (opCode.Any(Code.Leave, Code.Leave_S))
            {
                this.WriteCatchFinnallyEnd(writer, opCode);
            }

            this.WriteTryBegins(writer, opCode);
            this.WriteCatchBegins(writer, opCode);

            // process Phi Nodes
            if (opCode.AlternativeValues != null)
            {
                this.WritePhi(writer, opCode);
            }

            this.ActualWriteOpCode(writer, opCode);

            this.AdjustResultType(opCode);

            if (!opCode.Any(Code.Leave, Code.Leave_S))
            {
                this.WriteCatchFinnallyEnd(writer, opCode);
            }

            this.WriteCatchFinnallyCleanUpEnd(opCode);
            this.WriteTryEnds(writer, opCode);
            this.WriteExceptionHandlers(writer, opCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public void ActualWriteOpCode(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            var code = opCode.ToCode();
            switch (code)
            {
                case Code.Ldc_I4_0:
                    opCode.Result = opCode.UseAsNull
                                        ? new ConstValue(null, this.ResolveType("System.Void").ToPointerType())
                                        : new ConstValue(0, this.ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4_1:
                    opCode.Result = new ConstValue(1, this.ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                    var asString = code.ToString();
                    opCode.Result = new ConstValue(int.Parse(asString.Substring(asString.Length - 1, 1)), this.ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4_M1:
                    opCode.Result = new ConstValue(-1, this.ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4:
                    var opCodeInt32 = opCode as OpCodeInt32Part;
                    opCode.Result = new ConstValue(opCodeInt32.Operand, this.ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4_S:
                    opCodeInt32 = opCode as OpCodeInt32Part;
                    opCode.Result = new ConstValue(
                        opCodeInt32.Operand > 127 ? -(256 - opCodeInt32.Operand) : opCodeInt32.Operand, this.ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I8:
                    var opCodeInt64 = opCode as OpCodeInt64Part;
                    opCode.Result = new ConstValue(opCodeInt64.Operand, this.ResolveType("System.Int64"));
                    break;
                case Code.Ldc_R4:
                    var opCodeSingle = opCode as OpCodeSinglePart;

                    if (float.IsPositiveInfinity(opCodeSingle.Operand))
                    {
                        opCode.Result = new ConstValue("0x7FF0000000000000", this.ResolveType("System.Single"));
                    }
                    else if (float.IsNegativeInfinity(opCodeSingle.Operand))
                    {
                        opCode.Result = new ConstValue("0xFFF0000000000000", this.ResolveType("System.Single"));
                    }
                    else
                    {
                        var g = BitConverter.DoubleToInt64Bits(opCodeSingle.Operand);
                        opCode.Result = new ConstValue(string.Format("0x{0}", g.ToString("X")), this.ResolveType("System.Single"));
                    }

                    break;
                case Code.Ldc_R8:
                    var opCodeDouble = opCode as OpCodeDoublePart;
                    if (double.IsPositiveInfinity(opCodeDouble.Operand))
                    {
                        opCode.Result = new ConstValue("0x7FF0000000000000", this.ResolveType("System.Double"));
                    }
                    else if (double.IsNegativeInfinity(opCodeDouble.Operand))
                    {
                        opCode.Result = new ConstValue("0xFFF0000000000000", this.ResolveType("System.Double"));
                    }
                    else
                    {
                        var g = BitConverter.DoubleToInt64Bits(opCodeDouble.Operand);
                        opCode.Result = new ConstValue(string.Format("0x{0}", g.ToString("X")), this.ResolveType("System.Double"));
                    }

                    break;
                case Code.Ldstr:
                    var opCodeString = opCode as OpCodeStringPart;
                    var stringType = this.ResolveType("System.String");

                    // find constructor
                    var constructorInfo =
                        IlReader.Constructors(stringType)
                                .First(c => c.GetParameters().Count() == 1 && c.GetParameters().First().ParameterType.ToString() == "Char[]");

                    this.WriteNewWithoutCallingConstructor(opCode, stringType);
                    var stringIndex = this.GetStringIndex(opCodeString.Operand);

                    var dummyOpCodeWithStringIndex = OpCodePart.CreateNop;
                    dummyOpCodeWithStringIndex.Result =
                        new FullyDefinedReference(
                            string.Format(
                                "bitcast ([{1} x i16]* getelementptr inbounds ({2} i32, [{1} x i16] {3}* @.s{0}, i32 0, i32 1) to i16*)",
                                stringIndex,
                                opCodeString.Operand.Length + 1,
                                '{',
                                '}'),
                            stringType);

                    opCode.OpCodeOperands = new[] { dummyOpCodeWithStringIndex };

                    this.WriteCallConstructor(opCode, constructorInfo);

                    break;
                case Code.Ldnull:
                    opCode.Result = new ConstValue(null, this.ResolveType("System.Void").ToPointerType());
                    break;
                case Code.Ldtoken:

                    // TODO: finish loading Token  
                    ////var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    ////var data = opCodeFieldInfoPart.Operand.GetFieldRVAData();
                    opCode.Result = new ConstValue("undef", this.ResolveType("System.Object"));

                    break;
                case Code.Localloc:
                    writer.Write("alloca i32 ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(", align " + PointerSize);
                    break;
                case Code.Ldfld:

                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    // we wait when opCode.DestinationName is set;
                    var skip = opCodeFieldInfoPart.Operand.FieldType.IsStructureType() && opCode.Destination == null;
                    if (!skip)
                    {
                        this.WriteFieldAccess(writer, opCodeFieldInfoPart);
                        writer.WriteLine(string.Empty);

                        var memberAccessResultNumber = opCode.Result;
                        opCode.Result = null;
                        this.WriteLlvmLoad(opCode, memberAccessResultNumber.Type, memberAccessResultNumber);
                    }
                    else if (opCode.UsedBy.Any(Code.Box, Code.Call, Code.Callvirt))
                    {
                        // just load an address of a structure
                        this.WriteFieldAccess(writer, opCodeFieldInfoPart);
                        writer.WriteLine(string.Empty);
                    }

                    break;
                case Code.Ldflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.WriteFieldAccess(writer, opCodeFieldInfoPart);
                    var fieldLoadResult = opCodeFieldInfoPart.Result;

                    // convert return type of field to pointer of a field type
                    opCodeFieldInfoPart.Result = fieldLoadResult.ToPointerType();

                    break;
                case Code.Ldsfld:

                    IType castFrom;
                    IType intAdjustment;
                    bool intAdjustSecondOperand;
                    var operandType = this.DetectTypePrefix(
                        opCode, null, OperandOptions.TypeIsInOperator, out castFrom, out intAdjustment, out intAdjustSecondOperand);
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    var destinationName = string.Concat("@\"", opCodeFieldInfoPart.Operand.GetFullName(), '"');
                    if (!operandType.IsStructureType())
                    {
                        this.WriteLlvmLoad(opCode, operandType, new FullyDefinedReference(destinationName, opCodeFieldInfoPart.Operand.FieldType));
                    }

                    CheckIfExternalDeclarationIsRequired(opCodeFieldInfoPart.Operand);

                    break;
                case Code.Ldsflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    opCodeFieldInfoPart.Result = new FullyDefinedReference(
                        string.Concat("@\"", opCodeFieldInfoPart.Operand.GetFullName(), '"'), opCodeFieldInfoPart.Operand.FieldType.ToPointerType());

                    CheckIfExternalDeclarationIsRequired(opCodeFieldInfoPart.Operand);

                    break;
                case Code.Stfld:

                    this.FieldAccessAndSaveToField(opCode as OpCodeFieldInfoPart);

                    break;
                case Code.Stsfld:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    this.PreProcessOperand(writer, opCode, 0);

                    destinationName = string.Concat("@\"", opCodeFieldInfoPart.Operand.GetFullName(), '"');
                    operandType = opCodeFieldInfoPart.Operand.FieldType;

                    if (opCodeFieldInfoPart.Operand.FieldType.IsStructureType())
                    {
                        opCode.Destination = new FullyDefinedReference(destinationName, operandType);
                        var valueOp = opCode.OpCodeOperands[0];
                        if (!valueOp.HasResult)
                        {
                            valueOp.Destination = opCode.Destination;
                            this.ActualWriteOpCode(writer, valueOp);
                        }
                        else
                        {
                            this.WriteLlvmLoad(opCode, operandType, valueOp.Result);
                        }
                    }
                    else
                    {
                        this.ProcessOperator(
                            writer,
                            opCode,
                            "store",
                            operandType,
                            options: OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes,
                            operand1: 1,
                            operand2: -1);
                        this.PostProcessOperand(writer, opCode, 0);
                        writer.Write(", ");
                        operandType.WriteTypePrefix(writer);
                        writer.Write("* ");
                        writer.Write(destinationName);
                    }

                    CheckIfExternalDeclarationIsRequired(opCodeFieldInfoPart.Operand);

                    break;

                case Code.Ldobj:

                    // to support settings exceptions
                    if (opCode.ReadExceptionFromStack)
                    {
                        opCode.Result = new IncrementalResult(this.resultNumberIncremental, opCode.ReadExceptionFromStackType);
                        break;
                    }

                    var opCodeTypePart = opCode as OpCodeTypePart;

                    this.PreProcessOperand(writer, opCode, 0);

                    var firstOpResultType = opCode.OpCodeOperands[0].Result.Type;
                    if (opCode.Destination != null || (!firstOpResultType.UseAsClass && !firstOpResultType.IsByRef))
                    {
                        this.WriteLlvmLoad(opCode, opCodeTypePart.Operand, opCode.OpCodeOperands[0].Result);
                    }
                    else
                    {
                        opCode.Result = opCode.OpCodeOperands[0].Result;
                    }

                    break;

                case Code.Stobj:
                    opCodeTypePart = opCode as OpCodeTypePart;

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.WriteLine(string.Empty);

                    var ooperandIndex = 1;
                    this.SaveStruct(writer, opCode, ooperandIndex);

                    break;
                case Code.Ldlen:
                    this.WriteArrayGetLength(opCode);
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

                    // check if it is InitializeArray call
                    if (code == Code.Call && methodBase.IsItArrayInitialization())
                    {
                        this.WriteArrayInit(opCode);
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
                    var isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fadd" : "add", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "sadd");
                    break;
                case Code.Mul:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fmul" : "mul", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "smul");
                    break;
                case Code.Sub:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fsub" : "sub", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "ssub");
                    break;
                case Code.Div:
                case Code.Div_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fdiv" : "sdiv", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Rem:
                case Code.Rem_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "frem" : "srem", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.And:
                    this.BinaryOper(writer, opCode, "and", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Or:
                    this.BinaryOper(writer, opCode, "or", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Xor:
                    this.BinaryOper(writer, opCode, "xor", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Shl:
                    this.BinaryOper(writer, opCode, "shl", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Shr:
                case Code.Shr_Un:
                    this.BinaryOper(writer, opCode, "lshr", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Not:
                    var tempOper = opCode.OpCodeOperands;
                    var secondOperand = new OpCodePart(OpCodesEmit.Ldc_I4_M1, 0, 0);
                    this.ActualWrite(writer, secondOperand);
                    opCode.OpCodeOperands = new[] { tempOper[0], secondOperand };
                    this.BinaryOper(writer, opCode, "xor");
                    opCode.OpCodeOperands = tempOper;
                    break;
                case Code.Neg:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);

                    // TODO: should be removed in the future when Skip field is not used
                    if (opCode.OpCodeOperands[0].Result == null)
                    {
                        this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    }

                    tempOper = opCode.OpCodeOperands;

                    var firstOperand = isFloatingPoint
                                           ? new OpCodeDoublePart(OpCodesEmit.Ldc_R8, 0, 0, 0.0)
                                           : GetTypedIntZeroCode(opCode.OpCodeOperands[0].Result.Type);
                    this.ActualWrite(writer, firstOperand);
                    opCode.OpCodeOperands = new[] { firstOperand, tempOper[0] };

                    this.BinaryOper(
                        writer, opCode, isFloatingPoint ? "fsub" : "sub", OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    opCode.OpCodeOperands = tempOper;
                    break;
                case Code.Dup:

                    if (opCode.Destination != null)
                    {
                        opCode.OpCodeOperands[0].Destination = opCode.Destination;
                    }

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Box:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    var type = opCodeTypePart.Operand;
                    if (type.IsValueType())
                    {
                        type.WriteCallBoxObjectMethod(this, opCode);
                    }
                    else
                    {
                        this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                        opCodeTypePart.Result = opCodeTypePart.OpCodeOperands[0].Result.ToClassType();
                    }

                    break;

                case Code.Unbox:
                case Code.Unbox_Any:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    type = opCodeTypePart.Operand;
                    if (type.IsValueType())
                    {
                        type.WriteCallUnboxObjectMethod(this, opCode);
                    }
                    else
                    {
                        this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                        opCodeTypePart.Result = opCodeTypePart.OpCodeOperands[0].Result.ToNormalType();
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

                    if (localType.IsStructureType())
                    {
                        opCode.OpCodeOperands[0].Destination = new FullyDefinedReference(this.GetLocalVarName(index), localType);
                        this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    }
                    else
                    {
                        this.UnaryOper(writer, opCode, "store", localType, options: OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
                        writer.Write(", ");
                        this.WriteLlvmLocalVarAccess(index, true);
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

                    localType = this.LocalInfo[index].LocalType;

                    skip = this.LocalInfo[index].LocalType.IsStructureType() && opCode.Destination == null;
                    var definedReference = new FullyDefinedReference(destinationName, localType);
                    if (!skip)
                    {
                        this.WriteLlvmLoad(opCode, definedReference);
                    }
                    else
                    {
                        opCode.Result = definedReference;
                    }

                    break;
                case Code.Ldloca:
                case Code.Ldloca_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;
                    opCode.Result = new FullyDefinedReference(this.GetLocalVarName(index), this.LocalInfo[index].LocalType.ToPointerType());

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
                        this.ThisType.UseAsClass = true;
                        this.WriteLlvmLoad(opCode, new FullyDefinedReference("%this", this.ThisType), true, this.ThisType.IsStructureType());
                    }
                    else
                    {
                        var parameter = this.Parameters[index - (this.HasMethodThis ? 1 : 0)];

                        destinationName = GetArgVarName(parameter);

                        skip = parameter.ParameterType.IsStructureType() && opCode.Destination == null;
                        var fullyDefinedReference = new FullyDefinedReference(destinationName, parameter.ParameterType);
                        if (!skip)
                        {
                            this.WriteLlvmLoad(opCode, fullyDefinedReference);
                        }
                        else
                        {
                            opCode.Result = fullyDefinedReference;
                        }
                    }

                    break;

                case Code.Ldarga:
                case Code.Ldarga_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;

                    if (this.HasMethodThis && index == 0)
                    {
                        writer.Write("%this");
                        opCode.Result = new FullyDefinedReference("%this", this.ThisType);
                    }
                    else
                    {
                        var parameter = this.Parameters[index - (this.HasMethodThis ? 1 : 0)];
                        opCode.Result = new FullyDefinedReference(GetArgVarName(parameter), parameter.ParameterType.ToPointerType());
                    }

                    break;

                case Code.Starg:
                case Code.Starg_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;
                    var actualIndex = index - (this.HasMethodThis ? 1 : 0);
                    this.UnaryOper(
                        writer,
                        opCode,
                        "store",
                        this.Parameters[actualIndex].ParameterType,
                        options: OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
                    writer.Write(", ");
                    this.WriteLlvmArgVarAccess(writer, index - (this.HasMethodThis ? 1 : 0), true);

                    break;

                case Code.Ldftn:

                    opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;

                    var intPtrType = this.ResolveType("System.IntPtr");

                    // find constructor
                    constructorInfo = IlReader.Constructors(intPtrType)
                                              .First(c => c.GetParameters().Count() == 1 /* && c.GetParameters().First().ParameterType.ToString() == "Int"*/);

                    this.WriteNewWithoutCallingConstructor(opCode, intPtrType);

                    var convertString = this.WriteToString(
                        () =>
                        {
                            this.Output.Write("bitcast (");
                            this.WriteMethodPointerType(this.Output, opCodeMethodInfoPart.Operand);
                            this.Output.Write(" ");
                            this.Output.Write(this.GetFullMethodName(opCodeMethodInfoPart.Operand));
                            this.Output.Write(" to i8*)");
                        });
                    var dummyOpCodeWithIntToPtrConversion = OpCodePart.CreateNop;
                    dummyOpCodeWithIntToPtrConversion.Result = new FullyDefinedReference(convertString, intPtrType);

                    opCode.OpCodeOperands = new[] { dummyOpCodeWithIntToPtrConversion };

                    this.WriteCallConstructor(opCode, constructorInfo);

                    this.CheckIfExternalDeclarationIsRequired(opCodeMethodInfoPart.Operand);

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

                    // we need to invert all comare command
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    var oper = string.Empty;
                    switch (opCode.ToCode())
                    {
                        case Code.Beq:
                        case Code.Beq_S:
                            oper = isFloatingPoint ? "fcmp oeq" : "icmp eq";
                            break;
                        case Code.Bne_Un:
                        case Code.Bne_Un_S:
                            oper = isFloatingPoint ? "fcmp one" : "icmp ne";
                            break;
                        case Code.Blt:
                        case Code.Blt_S:
                            oper = isFloatingPoint ? "fcmp olt" : "icmp slt";
                            break;
                        case Code.Blt_Un:
                        case Code.Blt_Un_S:
                            oper = isFloatingPoint ? "fcmp ult" : "icmp ult";
                            break;
                        case Code.Ble:
                        case Code.Ble_S:
                            oper = isFloatingPoint ? "fcmp ole" : "icmp sle";
                            break;
                        case Code.Ble_Un:
                        case Code.Ble_Un_S:
                            oper = isFloatingPoint ? "fcmp ule" : "icmp ule";
                            break;
                        case Code.Bgt:
                        case Code.Bgt_S:
                            oper = isFloatingPoint ? "fcmp ogt" : "icmp sgt";
                            break;
                        case Code.Bgt_Un:
                        case Code.Bgt_Un_S:
                            oper = isFloatingPoint ? "fcmp ugt" : "icmp ugt";
                            break;
                        case Code.Bge:
                        case Code.Bge_S:
                            oper = isFloatingPoint ? "fcmp oge" : "icmp sge";
                            break;
                        case Code.Bge_Un:
                        case Code.Bge_Un_S:
                            oper = isFloatingPoint ? "fcmp uge" : "icmp uge";
                            break;
                    }

                    this.BinaryOper(
                        writer, opCode, oper, OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
                    if (!opCode.UseAsConditionalExpression)
                    {
                        writer.WriteLine(string.Empty);
                        this.WriteCondBranch(writer, opCode);
                    }

                    break;
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:

                    var forTrue = opCode.Any(Code.Brtrue, Code.Brtrue_S) ? "ne" : "eq";
                    var resultOf = this.ResultOf(opCode.OpCodeOperands[0]);

                    var opts = OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer;
                    this.UnaryOper(writer, opCode, "icmp " + forTrue, options: opts);

                    if (resultOf.Type.IsValueType() && !resultOf.Type.UseAsClass)
                    {
                        writer.WriteLine(", 0");
                    }
                    else
                    {
                        writer.WriteLine(", null");
                    }

                    if (!opCode.UseAsConditionalExpression)
                    {
                        this.WriteCondBranch(writer, opCode);
                    }

                    break;
                case Code.Br:
                case Code.Br_S:

                    writer.Write(string.Concat("br label %.a", opCode.JumpAddress()));

                    break;
                case Code.Leave:
                case Code.Leave_S:

                    writer.WriteLine("; Leave ");
                    if (this.tryScopes.Count > 0)
                    {
                        var tryClause = this.tryScopes.Peek();
                        var finallyClause = tryClause.Catches.FirstOrDefault(c => c.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally));
                        if (finallyClause != null)
                        {
                            finallyClause.FinallyJumps.Add(string.Concat(".a", opCode.JumpAddress()));
                            this.WriteFinallyLeave(finallyClause);
                        }
                        else
                        {
                            writer.Write(string.Concat("br label %.a", opCode.JumpAddress()));
                        }
                    }
                    else
                    {
                        writer.Write(string.Concat("br label %.a", opCode.JumpAddress()));
                    }

                    break;
                case Code.Ceq:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp oeq" : "icmp eq",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Clt:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp olt" : "icmp slt",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Clt_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp ult" : "icmp ult",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Cgt:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp ogt" : "icmp sgt",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Cgt_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp ugt" : "icmp ugt",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
                    break;

                case Code.Conv_R4:
                case Code.Conv_R_Un:
                    this.LlvmConvert(opCode, "fptrunc", "sitofp", "float", false, this.ResolveType("System.Single"));
                    break;

                case Code.Conv_R8:
                    this.LlvmConvert(opCode, "fpext", "sitofp", "double", false, this.ResolveType("System.Double"));
                    break;

                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                    this.LlvmConvert(opCode, "fptosi", "trunc", "i8", false, this.ResolveType("System.SByte"), this.ResolveType("System.Byte"));
                    break;

                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                    this.LlvmConvert(opCode, "fptoui", "trunc", "i8", false, this.ResolveType("System.SByte"), this.ResolveType("System.Byte"));
                    break;

                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptosi",
                        "trunc",
                        "i16",
                        false,
                        this.ResolveType("System.Int16"),
                        this.ResolveType("System.UInt16"),
                        this.ResolveType("System.Char"));
                    break;

                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptoui",
                        "trunc",
                        "i16",
                        false,
                        this.ResolveType("System.Int16"),
                        this.ResolveType("System.UInt16"),
                        this.ResolveType("System.Char"));
                    break;

                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                    this.LlvmConvert(opCode, "fptoui", "trunc", "i32", true, this.ResolveType("System.Int32"), this.ResolveType("System.UInt32"));
                    break;

                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                    this.LlvmConvert(opCode, "fptoui", "trunc", "i32", false, this.ResolveType("System.Int32"), this.ResolveType("System.UInt32"));
                    break;

                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                    this.LlvmConvert(opCode, "fptosi", "trunc", "i32", true, this.ResolveType("System.Int32"), this.ResolveType("System.UInt32"));
                    break;

                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                    this.LlvmConvert(opCode, "fptosi", "trunc", "i32", false, this.ResolveType("System.Int32"), this.ResolveType("System.UInt32"));
                    break;

                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                    this.LlvmConvert(opCode, "fptosi", "sext", "i64", false, this.ResolveType("System.Int64"), this.ResolveType("System.UInt64"));
                    break;

                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    this.LlvmConvert(opCode, "fptoui", "zext", "i64", false, this.ResolveType("System.Int64"), this.ResolveType("System.UInt64"));
                    break;

                case Code.Castclass:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.WriteLine(string.Empty);

                    this.WriteCast(opCodeTypePart, opCodeTypePart.OpCodeOperands[0].Result, opCodeTypePart.Operand, true);

                    break;

                case Code.Isinst:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.WriteLine(string.Empty);

                    var fromType = opCodeTypePart.OpCodeOperands[0].Result;
                    var toType = opCodeTypePart.Operand;

                    var dynamicCastRequired = false;
                    var castRequired = toType.IsClassCastRequired(opCodeTypePart.OpCodeOperands[0], out dynamicCastRequired);
                    if (dynamicCastRequired || !castRequired)
                    {
                        this.WriteDynamicCast(writer, opCodeTypePart, fromType, toType, true);
                    }
                    else
                    {
                        this.WriteCast(opCodeTypePart, opCodeTypePart.OpCodeOperands[0].Result, toType);
                    }

                    break;

                case Code.Newobj:

                    var opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;
                    this.WriteNewObject(opCodeConstructorInfoPart);

                    break;

                case Code.Newarr:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteNewArray(opCode, opCodeTypePart.Operand, opCode.OpCodeOperands[0]);

                    break;

                case Code.Initobj:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteInit(opCode, opCodeTypePart.Operand);

                    break;

                case Code.Throw:

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.WriteLine(string.Empty);
                    this.WriteThrow(opCode, this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);

                    break;

                case Code.Rethrow:

                    this.WriteRethrow(
                        opCode,
                        this.catchScopes.Count > 0 ? this.catchScopes.Peek() : null,
                        this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);

                    break;

                case Code.Endfilter:
                case Code.Endfinally:
                    break;

                case Code.Pop:
                    break;

                case Code.Constrained:

                    // to solve the problem with referencing ValueType and Class type in Generic type
                    opCodeTypePart = opCode as OpCodeTypePart;

                    // if this is Struct we already have an address in LLVM
                    if (!opCodeTypePart.Operand.IsStructureType())
                    {
                        var nextOp = opCode.NextOpCode(this);
                        var fullyDefinedReference = nextOp.OpCodeOperands[0].Result;
                        nextOp.OpCodeOperands[0].Result = null;
                        this.WriteLlvmLoad(nextOp.OpCodeOperands[0], opCodeTypePart.Operand, fullyDefinedReference);
                    }

                    break;

                case Code.Switch:

                    var opCodeLabels = opCode as OpCodeLabelsPart;

                    this.UnaryOper(writer, opCode, "switch");

                    var switchValueType = opCodeLabels.OpCodeOperands[0].Result.Type;

                    index = 0;
                    writer.Write(", label %.a{0} [ ", opCode.GroupAddressEnd);

                    foreach (var label in opCodeLabels.Operand)
                    {
                        switchValueType.WriteTypePrefix(writer);
                        writer.Write(" {0}, label %.a{1} ", index, opCodeLabels.JumpAddress(index++));
                    }

                    writer.WriteLine("]");

                    writer.Indent--;
                    writer.WriteLine(string.Concat(".a", opCode.GroupAddressEnd, ':'));
                    writer.Indent++;

                    opCode.NextOpCode(this).JumpProcessed = true;

                    break;
            }
        }

        private void WriteOverflowWithThrow(LlvmIndentedTextWriter writer, OpCodePart opCode, string @operator)
        {
            this.BinaryOper(
                writer,
                opCode,
                string.Concat("call { %R, i1 } @llvm.", @operator, ".with.overflow.%R("),
                OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes | OperandOptions.Template | OperandOptions.DetectAndWriteTypeInSecondOperand);
            writer.WriteLine(")");

            var result = opCode.Result;

            var testResult = this.WriteSetResultNumber(opCode, this.ResolveType("System.Boolean"));
            writer.Write("extractvalue { ");
            result.Type.WriteTypePrefix(writer);
            writer.Write(", i1 } ");
            WriteResult(result);
            writer.WriteLine(", 1");

            var returnValue = this.WriteSetResultNumber(opCode, result.Type);
            writer.Write("extractvalue { ");
            result.Type.WriteTypePrefix(writer);
            writer.Write(", i1 } ");
            WriteResult(result);
            writer.WriteLine(", 0");

            // throw exception
            this.WriteBranchSwitchToThrowOrPass(writer, opCode, testResult, "System.OverflowException", "arithm_overflow", "zero");
        }

        private void WriteNewObject(OpCodeConstructorInfoPart opCodeConstructorInfoPart, bool ignoreTestNullValue = false)
        {
            var declaringType = opCodeConstructorInfoPart.Operand.DeclaringType;

            this.CheckIfExternalDeclarationIsRequired(declaringType);

            this.WriteNew(opCodeConstructorInfoPart, declaringType, ignoreTestNullValue);

            if (opCodeConstructorInfoPart.Destination != null)
            {
                opCodeConstructorInfoPart.Result.Type.UseAsClass = false;
                this.WriteLlvmLoad(opCodeConstructorInfoPart, opCodeConstructorInfoPart.Result);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="destType">
        /// </param>
        /// <returns>
        /// </returns>
        public bool AdjustIntConvertableTypes(LlvmIndentedTextWriter writer, OpCodePart opCode, IType destType)
        {
            if (!opCode.HasResult)
            {
                return false;
            }

            if (!destType.IsPointer && !opCode.Result.Type.IsPointer && destType.IsIntValueTypeExtCastRequired(opCode.Result.Type))
            {
                this.LlvmIntConvert(opCode, destType.IsSignType() ? "sext" : "zext", "i" + destType.IntTypeBitSize());
                writer.WriteLine(string.Empty);
                return true;
            }

            if (!destType.IsPointer && !opCode.Result.Type.IsPointer && destType.IsIntValueTypeTruncCastRequired(opCode.Result.Type))
            {
                this.LlvmIntConvert(opCode, "trunc", "i" + destType.IntTypeBitSize());
                writer.WriteLine(string.Empty);
                return true;
            }

            // pointer to int, int to pointerf
            if (destType.IntTypeBitSize() > 0 && !destType.IsPointer && opCode.Result.Type.IsPointer)
            {
                this.LlvmIntConvert(opCode, "ptrtoint", "i" + destType.IntTypeBitSize());
                writer.WriteLine(string.Empty);
                return true;
            }

            if (opCode.Result.Type.IntTypeBitSize() > 0 && destType.IsPointer && !opCode.Result.Type.IsPointer)
            {
                this.LlvmIntConvert(opCode, "inttoptr", "i" + destType.GetElementType().IntTypeBitSize(true) + "*");
                writer.WriteLine(string.Empty);
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodBase">
        /// </param>
        public void CheckIfExternalDeclarationIsRequired(IMethod methodBase)
        {
            if (methodBase.Name.StartsWith("%"))
            {
                return;
            }

            if (methodBase.DeclaringType.AssemblyQualifiedName != this.AssemblyQualifiedName)
            {
                this.methodDeclRequired.Add(methodBase);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void CheckIfExternalDeclarationIsRequired(IType type)
        {
            if (type == null || type.AssemblyQualifiedName == this.AssemblyQualifiedName)
            {
                return;
            }

            this.typeDeclRequired.Add(type.ToBareType());
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        public void CheckIfExternalDeclarationIsRequired(IField field)
        {
            if (field == null || !field.IsStatic || field.DeclaringType.AssemblyQualifiedName == this.AssemblyQualifiedName || field.DeclaringType.IsGenericType)
            {
                return;
            }

            this.staticFieldExtrenalDeclRequired.Add(field);
        }

        /// <summary>
        /// </summary>
        public void Close()
        {
            this.Output.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        public void DisableWrite(bool value)
        {
            if (value)
            {
                this.savedOutput = this.Output;
                this.storedText.Clear();
                this.Output = new LlvmIndentedTextWriter(new StringWriter(this.storedText));
            }
            else
            {
                this.Output.Close();
                this.Output = this.savedOutput;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="data">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetArrayIndex(byte[] data)
        {
            var idx = ++this.arrayIndexIncremental;
            this.arrayStorage[idx] = data;
            return idx;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public int GetBlockJumpAddress()
        {
            return ++this.blockJumpAddressIncremental;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetDirectName(OpCodePart opCodePart)
        {
            var output = this.Output;

            var sb = new StringBuilder();
            this.Output = new LlvmIndentedTextWriter(new StringWriter(sb));

            this.ActualWrite(this.Output, opCodePart);

            this.Output = output;
            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetLocalVarName(int index)
        {
            return string.Concat("%local", index);
        }

        /// <summary>
        /// </summary>
        /// <param name="str">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetStringIndex(string str)
        {
            var idx = ++this.stringIndexIncremental;
            this.stringStorage[idx] = str;
            return idx;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsPostDeclarationsProcessed(IType type)
        {
            return this.postDeclarationsProcessedTypes.Contains(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsProcessed(IType type)
        {
            return this.processedTypes.Contains(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public void LoadIndirect(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.Result != null)
            {
                return;
            }

            IType type = null;

            switch (opCode.ToCode())
            {
                case Code.Ldind_I:
                    type = this.ResolveType("System.Int32");
                    break;
                case Code.Ldind_I1:
                    type = this.ResolveType("System.SByte");
                    break;
                case Code.Ldind_U1:
                    type = this.ResolveType("System.Byte");
                    break;
                case Code.Ldind_I2:
                    type = this.ResolveType("System.Int16");
                    break;
                case Code.Ldind_U2:
                    type = this.ResolveType("System.UInt16");
                    break;
                case Code.Ldind_I4:
                    type = this.ResolveType("System.Int32");
                    break;
                case Code.Ldind_U4:
                    type = this.ResolveType("System.UInt32");
                    break;
                case Code.Ldind_I8:
                    type = this.ResolveType("System.Int64");
                    break;
                case Code.Ldind_R4:
                    type = this.ResolveType("System.Single");
                    break;
                case Code.Ldind_R8:
                    type = this.ResolveType("System.Double");
                    break;
                case Code.Ldind_Ref:
                    type = this.GetTypeOfReference(opCode);
                    break;
            }

            this.LoadIndirect(writer, opCode, type);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        public void LoadIndirect(LlvmIndentedTextWriter writer, OpCodePart opCode, IType type)
        {
            FullyDefinedReference accessIndexResultNumber2;

            // next code fixing issue with using Code.Ldind to load first value in value types
            var resultOfOperand0 = opCode.OpCodeOperands[0].Result;
            var isUsedAsClass = resultOfOperand0 != null && resultOfOperand0.Type.UseAsClass;
            if (isUsedAsClass)
            {
                resultOfOperand0.Type.UseAsClass = false;
            }

            var isValueType = resultOfOperand0 != null && resultOfOperand0.Type.IsValueType;
            if (isValueType && isUsedAsClass)
            {
                resultOfOperand0.Type.UseAsClass = true;

                // write first field access
                this.WriteFieldAccess(writer, opCode, 1);
                writer.WriteLine(string.Empty);
                accessIndexResultNumber2 = opCode.Result;
                type = opCode.Result.Type;
            }
            else
            {
                if (isUsedAsClass)
                {
                    resultOfOperand0.Type.UseAsClass = true;
                }

                this.PreProcessOperand(writer, opCode, 0);
                accessIndexResultNumber2 = opCode.OpCodeOperands[0].Result;
            }

            opCode.Result = null;

            this.WriteLlvmLoad(opCode, type, accessIndexResultNumber2);

            if (!isUsedAsClass && resultOfOperand0 != null)
            {
                resultOfOperand0.Type.UseAsClass = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="operand">
        /// </param>
        /// <param name="detectAndWriteTypePrefix">
        /// </param>
        /// <param name="forcedType">
        /// </param>
        public void PostProcess(LlvmIndentedTextWriter writer, OpCodePart operand, bool detectAndWriteTypePrefix = false, IType forcedType = null)
        {
            writer.Write(' ');

            if (!operand.HasResult)
            {
                if (forcedType != null)
                {
                    forcedType.WriteTypePrefix(writer);
                    writer.Write(' ');
                }
                else if (detectAndWriteTypePrefix)
                {
                    IType castFrom;
                    IType intAdjustment;
                    bool intAdjustSecondOperand;
                    var effectiveType = this.DetectTypePrefix(
                        operand, null, OperandOptions.TypeIsInOperator, out castFrom, out intAdjustment, out intAdjustSecondOperand);
                    (effectiveType ?? this.ResolveType("System.Void")).WriteTypePrefix(writer);
                    writer.Write(' ');
                }

                this.ActualWrite(writer, operand);

                this.WriteResult(operand.Result);
            }
            else
            {
                if (forcedType != null)
                {
                    forcedType.WriteTypePrefix(writer);
                    writer.Write(' ');
                }
                else if (detectAndWriteTypePrefix)
                {
                    (operand.Result.Type ?? this.ResolveType("System.Void")).WriteTypePrefix(writer);
                    writer.Write(' ');
                }

                // TODO: you need to remove it after removing using field Skip
                if (operand.Result == null)
                {
                    this.ActualWrite(writer, operand);
                }

                this.WriteResult(operand.Result);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="detectAndWriteTypePrefix">
        /// </param>
        public void PostProcessOperand(LlvmIndentedTextWriter writer, OpCodePart opCode, int index, bool detectAndWriteTypePrefix = false)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            var operand = opCode.OpCodeOperands[index];
            this.PostProcess(writer, operand, detectAndWriteTypePrefix);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="operandOpCode">
        /// </param>
        /// <param name="options">
        /// </param>
        public void PreProcess(LlvmIndentedTextWriter writer, OpCodePart operandOpCode, OperandOptions options = OperandOptions.None)
        {
            // TODO: use it to sort out Result in future
            ////Debug.Assert(operandOpCode.HasResult || operandOpCode.Destination != null || operandOpCode.Any(Code.Nop), "Should have result");
            if (!operandOpCode.HasResult)
            {
                this.ActualWrite(writer, operandOpCode);
                writer.WriteLine(string.Empty);
                return;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="options">
        /// </param>
        public void PreProcessOperand(LlvmIndentedTextWriter writer, OpCodePart opCode, int index, OperandOptions options = OperandOptions.None)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            var operandOpCode = opCode.OpCodeOperands[index];
            this.PreProcess(writer, operandOpCode, options: options);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="op">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        /// <param name="resultType">
        /// </param>
        /// <param name="options">
        /// </param>
        /// <param name="operand1">
        /// </param>
        /// <param name="operand2">
        /// </param>
        public void ProcessOperator(
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            string op,
            IType requiredType = null,
            IType resultType = null,
            OperandOptions options = OperandOptions.None,
            int operand1 = 0,
            int operand2 = 1)
        {
            IType castFrom;
            IType intAdjustment;
            bool intAdjustSecondOperand;

            var effectiveType = this.DetectTypePrefix(
                opCode, requiredType, options, out castFrom, out intAdjustment, out intAdjustSecondOperand, operand1, operand2);

            effectiveType = this.ApplyTypeAdjustment(
                writer, opCode, effectiveType, castFrom, intAdjustment, intAdjustSecondOperand, ref resultType, operand1, operand2);

            this.WriteResultAndFirstOperandType(writer, opCode, op, requiredType, resultType, options, effectiveType);
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
            var writer = this.Output;

            if (opCodePart.OpCodeOperands == null)
            {
                return;
            }

            if (fieldType.IsStructureType())
            {
                opCodePart.Destination = opCodePart.Result;
                var valueOp = opCodePart.OpCodeOperands[valueOperand];
                if (!valueOp.HasResult)
                {
                    valueOp.Destination = opCodePart.Destination;
                    this.ActualWriteOpCode(writer, valueOp);
                }
                else
                {
                    this.WriteLlvmLoad(opCodePart, fieldType, valueOp.Result);
                }
            }
            else
            {
                var opts = OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes;

                this.ProcessOperator(writer, opCodePart, "store", fieldType, options: opts, operand1: valueOperand, operand2: -1);
                this.PostProcessOperand(writer, opCodePart, valueOperand);
                writer.Write(", ");
                opCodePart.Result.Type.WriteTypePrefix(writer);
                writer.Write("* ");
                this.WriteResult(opCodePart.Result);
            }
        }

        /// <summary>
        /// </summary>
        public override void StartProcess()
        {
            base.StartProcess();
            this.resultNumberIncremental = 0;
            this.blockJumpAddressIncremental = 0;
            this.landingPadVariablesAreWritten = false;
            this.needToWriteUnwindException = false;
            this.needToWriteUnreachable = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="op">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        /// <param name="resultType">
        /// </param>
        /// <param name="options">
        /// </param>
        public void UnaryOper(
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            string op,
            IType requiredType = null,
            IType resultType = null,
            OperandOptions options = OperandOptions.None)
        {
            this.UnaryOper(writer, opCode, 0, op, requiredType, resultType, options);
        }

        public string GetAllocator()
        {
            return this.Gc ? "GC_malloc" : "_Znwj";
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="operandIndex">
        /// </param>
        /// <param name="op">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        /// <param name="resultType">
        /// </param>
        /// <param name="options">
        /// </param>
        public void UnaryOper(
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            int operandIndex,
            string op,
            IType requiredType = null,
            IType resultType = null,
            OperandOptions options = OperandOptions.None)
        {
            this.PreProcessOperand(writer, opCode, operandIndex, options);

            this.ProcessOperator(writer, opCode, op, requiredType, resultType, options, operand1: operandIndex, operand2: -1);

            if (!options.HasFlag(OperandOptions.IgnoreOperand))
            {
                this.PostProcessOperand(writer, opCode, operandIndex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="rawText">
        /// </param>
        public void Write(string rawText)
        {
            this.Output.Write(rawText);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        public void Write(OpCodePart opCode)
        {
            this.AddOpCode(opCode);
        }

        /// <summary>
        /// </summary>
        public void WriteAfterConstructors()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        public void WriteAfterFields(int count)
        {
            this.Output.WriteLine(string.Empty);

            this.Output.Indent--;
            this.Output.WriteLine("}");
        }

        /// <summary>
        /// </summary>
        public void WriteAfterMethods()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="isThis">
        /// </param>
        public void WriteArgumentCopyDeclaration(string name, IType type, bool isThis = false)
        {
            if (!isThis && type.IsStructureType())
            {
                return;
            }

            if (isThis)
            {
                type.UseAsClass = true;
            }

            this.Output.Write("{0} = ", GetArgVarName(name));

            // for value types
            this.Output.Write("alloca ");
            type.WriteTypePrefix(this.Output, type.IsStructureType() || isThis);
            this.Output.Write(", align " + PointerSize);
            this.Output.WriteLine(string.Empty);

            this.Output.Write("store ");
            type.WriteTypePrefix(this.Output, type.IsStructureType() || isThis);
            this.Output.Write(" %\"arg.{0}\"", name);
            this.Output.Write(", ");
            type.WriteTypePrefix(this.Output, type.IsStructureType() || isThis);

            this.Output.Write("* {0}", GetArgVarName(name));
            this.Output.Write(", align " + PointerSize);
            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        public void WriteBeforeConstructors()
        {
            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        public void WriteBeforeFields(int count)
        {
            var baseType = this.ThisType.BaseType;

            this.CheckIfExternalDeclarationIsRequired(baseType);

            this.Output.WriteLine("{");
            this.Output.Indent++;

            // put virtual root table if type has no any base with virtual types
            if (this.ThisType.IsRootInterface())
            {
                this.Output.WriteLine("i32 (...)**");
            }
            else if (this.ThisType.IsRootOfVirtualTable())
            {
                this.Output.WriteLine("i32 (...)**");
            }

            if (baseType != null)
            {
                baseType.WriteTypeWithoutModifiers(this.Output);
            }

            var index = 0;
            foreach (var @interface in this.ThisType.GetInterfacesExcludingBaseAllInterfaces())
            {
                this.CheckIfExternalDeclarationIsRequired(@interface);

                this.Output.WriteLine(index == 0 && baseType == null ? string.Empty : ", ");
                @interface.WriteTypeWithoutModifiers(this.Output);
                index++;
            }
        }

        /// <summary>
        /// </summary>
        public void WriteBeforeMethods()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="compareResult">
        /// </param>
        /// <param name="trueLabel">
        /// </param>
        /// <param name="falseLabel">
        /// </param>
        public void WriteCondBranch(LlvmIndentedTextWriter writer, IncrementalResult compareResult, string trueLabel, string falseLabel)
        {
            writer.WriteLine("br i1 {0}, label %{1}, label %{2}", compareResult, trueLabel, falseLabel);
            this.WriteLabel(writer, trueLabel);
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteConstructorEnd(IConstructor ctor, IGenericContext genericContext)
        {
            this.WriteMethodBody();

            this.WritePostMethodEnd(ctor);
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteConstructorStart(IConstructor ctor, IGenericContext genericContext)
        {
            this.StartProcess();

            this.processedMethods.Add(ctor);

            this.ReadMethodInfo(ctor, genericContext);

            var isDelegateBodyFunctions = ctor.IsDelegateFunctionBody();
            if ((ctor.IsAbstract || this.NoBody) && !isDelegateBodyFunctions)
            {
                this.Output.Write("declare ");
            }
            else
            {
                this.Output.Write("define ");
                if (this.ThisType.IsGenericType)
                {
                    this.Output.Write("linkonce_odr ");
                }
            }

            this.Output.Write("void ");

            this.WriteMethodDefinitionName(this.Output, ctor);
            if (ctor.IsStatic)
            {
                this.StaticConstructors.Add(ctor);
            }

            this.WriteMethodParamsDef(this.Output, ctor.GetParameters(), this.HasMethodThis, this.ThisType, this.ResolveType("System.Void"));

            this.WriteMethodNumber();

            // write local declarations
            var methodBase = ctor.ResolveMethodBody(genericContext);
            if (methodBase != null)
            {
                this.Output.WriteLine(" {");
                this.Output.Indent++;
                this.WriteLocalVariableDeclarations(methodBase.LocalVariables);
                this.WriteArgumentCopyDeclarations(ctor.GetParameters(), this.HasMethodThis);

                this.Output.StartMethodBody();
            }
            else if (isDelegateBodyFunctions)
            {
                this.WriteDelegateFunctionBody(ctor);
            }

            methodNumberIncremental++;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        public void WriteConvertValueTypeToReferenceType(OpCodePart opCode, IType declaringType)
        {
            var writer = this.Output;

            writer.WriteLine(string.Empty);
            this.CheckIfExternalDeclarationIsRequired(declaringType);
            this.WriteNewWithoutCallingConstructor(opCode, declaringType);

            var newObjectResult = opCode.Result;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Copy data");

            // write access to a field
            this.PreProcessOperand(writer, opCode, 0);

            if (!declaringType.IsStructureType() && declaringType.FullName != "System.DateTime" && declaringType.FullName != "System.Decimal")
            {
                this.WriteFieldAccess(writer, opCode, declaringType.ToClass(), declaringType.ToClass(), 1, opCode.Result);
                writer.WriteLine(string.Empty);
            }

            var fieldType = declaringType;

            fieldType.UseAsClass = false;

            this.SaveToField(opCode, fieldType, 0);

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");

            opCode.Result = newObjectResult;
            opCode.Result.Type.UseAsClass = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="dest">
        /// </param>
        public void WriteCopyStruct(LlvmIndentedTextWriter writer, OpCodePart opCode, FullyDefinedReference source, FullyDefinedReference dest)
        {
            Debug.Assert(source.Type.TypeEquals(dest.Type));

            var storeResult = opCode.Result;

            this.WriteBitcast(opCode, dest);
            var op1 = opCode.Result;
            writer.WriteLine(string.Empty);
            this.WriteBitcast(opCode, source);
            var op2 = opCode.Result;
            writer.WriteLine(string.Empty);

            this.WriteMemCopy(dest.Type, op1, op2);

            opCode.Result = storeResult;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeTypePart">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="checkNull">
        /// </param>
        public void WriteDynamicCast(
            LlvmIndentedTextWriter writer, OpCodePart opCodeTypePart, FullyDefinedReference fromType, IType toType, bool checkNull = false, bool throwExceptionIfNull = false)
        {
            var effectiveFromType = fromType.ToDereferencedType();
            effectiveFromType.Type.UseAsClass = fromType.Type.UseAsClass;

            if (effectiveFromType.Type.TypeEquals(toType))
            {
                opCodeTypePart.Result = fromType;
                return;
            }

            if (checkNull)
            {
                var testNullResultNumber = this.WriteSetResultNumber(opCodeTypePart, this.ResolveType("System.Boolean"));
                writer.Write("icmp eq ");
                effectiveFromType.Type.WriteTypePrefix(writer);
                writer.WriteLine(" {0}, null", fromType);

                writer.WriteLine("br i1 {0}, label %.dynamic_cast_null{1}, label %.dynamic_cast_not_null{1}", testNullResultNumber, opCodeTypePart.AddressStart);

                writer.Indent--;
                writer.WriteLine(".dynamic_cast_not_null{0}:", opCodeTypePart.AddressStart);
                writer.Indent++;
            }

            this.WriteBitcast(opCodeTypePart, effectiveFromType, this.ResolveType("System.Byte"));
            writer.WriteLine(string.Empty);

            var firstCastToBytesResult = opCodeTypePart.Result;

            var dynamicCastResultNumber = this.WriteSetResultNumber(opCodeTypePart, this.ResolveType("System.Byte").ToPointerType());

            writer.Write("call i8* @__dynamic_cast(i8* {0}, i8* bitcast (", firstCastToBytesResult);
            effectiveFromType.Type.WriteRttiClassInfoDeclaration(writer);
            writer.Write("* @\"{0}\" to i8*), i8* bitcast (", effectiveFromType.Type.GetRttiInfoName());
            toType.WriteRttiClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*), i32 {1})", toType.GetRttiInfoName(), CalculateDynamicCastInterfaceIndex(effectiveFromType.Type, toType));
            writer.WriteLine(string.Empty);

            if (throwExceptionIfNull)
            {
                this.WriteTestNullValue(writer, opCodeTypePart, dynamicCastResultNumber, "System.InvalidCastException", "dynamic_cast");
            }

            toType.UseAsClass = true;
            this.WriteBitcast(opCodeTypePart, dynamicCastResultNumber, toType);

            var dynamicCastResult = opCodeTypePart.Result;

            if (checkNull)
            {
                writer.WriteLine(string.Empty);

                writer.WriteLine("br label %.dynamic_cast_end{0}", opCodeTypePart.AddressStart);

                writer.Indent--;
                writer.WriteLine(".dynamic_cast_null{0}:", opCodeTypePart.AddressStart);
                writer.Indent++;

                writer.WriteLine("br label %.dynamic_cast_end{0}", opCodeTypePart.AddressStart);

                var label = string.Concat("dynamic_cast_end", opCodeTypePart.AddressStart);

                writer.Indent--;
                writer.WriteLine(".{0}:", label);
                writer.Indent++;

                var testNullResultNumber = this.WriteSetResultNumber(opCodeTypePart, toType);
                writer.Write("phi ");
                toType.WriteTypePrefix(writer, true);
                writer.Write(
                    " [ {0}, {1} ], [ null, {2} ]",
                    dynamicCastResult,
                    string.Format("%.{1}{0}", opCodeTypePart.AddressStart, throwExceptionIfNull ? "dynamic_cast_result_not_null" : "dynamic_cast_not_null"),
                    string.Format("%.dynamic_cast_null{0}", opCodeTypePart.AddressStart));

                LlvmHelpersGen.SetCustomLabel(opCodeTypePart, label);
            }

            this.typeRttiDeclRequired.Add(effectiveFromType.Type);
            this.typeRttiDeclRequired.Add(toType);
        }

        // TODO: if DynamicCast does not work for an interface then something wrong with this value, (it should return value equals to the number of inheritance route * pointer size, 
        // if type can't be found in inheritance route it should return -2, if casts from type which inheritce more then one time it should return -3
        // for inheritance root for objects equals 0 (as it is all the time first)
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
                foreach (var interafce in toType.GetInterfaces())
                {
                    if (interafce.GetAllInterfaces().Contains(fromType))
                    {
                        interfaceRouteIndex = index;
                        break;
                    }

                    index++;
                }

                if (interfaceRouteIndex > 0 && toType.GetInterfaces().Contains(fromType))
                {
                    return -3;
                }

                return interfaceRouteIndex * LlvmWriter.PointerSize;
            }

            return 0;
        }

        public void WriteTestNullValue(LlvmIndentedTextWriter writer, OpCodePart opCodePart, IncrementalResult resultToTest, string exceptionName, string labelPrefix)
        {
            var testNullResultNumber = this.WriteSetResultNumber(opCodePart, this.ResolveType("System.Boolean"));
            opCodePart.Result = resultToTest;

            writer.Write("icmp eq ");
            writer.WriteLine("i8* {0}, null", resultToTest);

            this.WriteBranchSwitchToThrowOrPass(writer, opCodePart, testNullResultNumber, exceptionName, labelPrefix, "null");
        }

        public void WriteBranchSwitchToThrowOrPass(
            LlvmIndentedTextWriter writer,
            OpCodePart opCodePart,
            IncrementalResult testValueResultNumber,
            string exceptionName,
            string labelPrefix,
            string labelSuffix)
        {
            writer.WriteLine("br i1 {0}, label %.{2}_result_{3}{1}, label %.{2}_result_not_{3}{1}", testValueResultNumber, opCodePart.AddressStart, labelPrefix, labelSuffix);

            writer.WriteLine(string.Empty);

            writer.Indent--;
            writer.WriteLine(".{1}_result_{2}{0}:", opCodePart.AddressStart, labelPrefix, labelSuffix);
            writer.Indent++;

            // throw InvalidCast result
            writer.WriteLine(string.Empty);
            var opCodeThrow = new OpCodePart(OpCodesEmit.Throw, 0, 0);

            var invalidCastExceptionType = this.ResolveType(exceptionName);

            // find constructor
            var constructorInfo = IlReader.Constructors(invalidCastExceptionType).First(c => !c.GetParameters().Any());

            var opCodeNewInstance = new OpCodeConstructorInfoPart(OpCodesEmit.Newobj, 0, 0, constructorInfo);
            opCodeThrow.OpCodeOperands = new[] { opCodeNewInstance };

            this.WriteNewObject(opCodeNewInstance, true);

            writer.WriteLine(string.Empty);
            this.WriteThrow(opCodeThrow, this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);

            var label = string.Concat(labelPrefix, "_result_not_", labelSuffix, opCodePart.AddressStart);

            writer.Indent--;
            writer.WriteLine(".{0}:", label);
            writer.Indent++;

            LlvmHelpersGen.SetCustomLabel(opCodePart, label);
        }

        /// <summary>
        /// </summary>
        public void WriteEnd()
        {
            if (this.MainMethod != null)
            {
                this.WriteMainFunction();
            }

            this.WriteGlobalConstructors();

            this.WriteRequiredDeclarations();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeFieldInfoPart">
        /// </param>
        public void WriteFieldAccess(LlvmIndentedTextWriter writer, OpCodeFieldInfoPart opCodeFieldInfoPart)
        {
            writer.WriteLine("; Access to '{0}' field", opCodeFieldInfoPart.Operand.Name);

            var operand = this.ResultOf(opCodeFieldInfoPart.OpCodeOperands[0]);
            var opts = OperandOptions.GenerateResult;

            var effectiveType = operand.Type.IsPointer ? operand.Type.GetElementType() : operand.Type;
            if (effectiveType.IsValueType)
            {
                effectiveType.UseAsClass = true;
            }

            this.UnaryOper(writer, opCodeFieldInfoPart, "getelementptr inbounds", effectiveType, opCodeFieldInfoPart.Operand.FieldType, options: opts);

            this.CheckIfTypeIsRequiredForBody(effectiveType);

            this.WriteFieldIndex(writer, effectiveType, opCodeFieldInfoPart.Operand);
        }

        /// <summary>
        /// fixing issue with Code.Ldind when you need to load first field value
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="index">
        /// </param>
        public void WriteFieldAccess(LlvmIndentedTextWriter writer, OpCodePart opCodePart, int index)
        {
            writer.WriteLine("; Access to '#{0}' field", index);
            var operand = this.ResultOf(opCodePart.OpCodeOperands[0]);

            var classType = operand.Type;

            var opts = OperandOptions.GenerateResult;
            var fieldType = classType.GetFieldTypeByIndex(index);
            classType.UseAsClass = true;
            this.UnaryOper(writer, opCodePart, "getelementptr inbounds", classType, fieldType, options: opts);

            this.CheckIfTypeIsRequiredForBody(classType);

            this.WriteFieldIndex(writer, classType, classType, index);
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
        public bool WriteFieldAccess(
            LlvmIndentedTextWriter writer, OpCodePart opCodePart, IType classType, IType fieldContainerType, int index, FullyDefinedReference valueReference)
        {
            var fieldType = fieldContainerType.GetFieldTypeByIndex(index);
            if (fieldType == null)
            {
                return false;
            }

            this.WriteSetResultNumber(opCodePart, fieldType);

            writer.Write("getelementptr inbounds ");
            valueReference.Type.ToClass().WriteTypePrefix(writer);
            writer.Write(" ");
            writer.Write(valueReference);

            this.CheckIfTypeIsRequiredForBody(valueReference.Type);

            this.WriteFieldIndex(writer, classType, fieldContainerType, index);

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <param name="count">
        /// </param>
        public void WriteFieldEnd(IField field, int number, int count)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="classType">
        /// </param>
        /// <param name="fieldInfo">
        /// </param>
        public void WriteFieldIndex(LlvmIndentedTextWriter writer, IType classType, IField fieldInfo)
        {
            var targetType = fieldInfo.DeclaringType;
            var type = classType;

            // first element for pointer (IType* + 0)
            writer.Write(", i32 0");

            while (type.TypeNotEquals(targetType))
            {
                type = type.BaseType;
                if (type == null)
                {
                    Debug.Assert(false, "could not find base class");
                    break;
                }

                // first index is base type index
                writer.Write(", i32 0");
            }

            // find index
            int index;
            if (!this.indexByFieldInfo.TryGetValue(fieldInfo.GetFullName(), out index))
            {
                index = this.CalculateFieldIndex(fieldInfo, type);
            }

            writer.Write(", i32 ");
            writer.Write(index);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="classType">
        /// </param>
        /// <param name="fieldContainerType">
        /// </param>
        /// <param name="fieldIndex">
        /// </param>
        public void WriteFieldIndex(LlvmIndentedTextWriter writer, IType classType, IType fieldContainerType, int fieldIndex)
        {
            var targetType = fieldContainerType;
            var type = classType;

            // first element for pointer (IType* + 0)
            writer.Write(", i32 0");

            while (type.TypeNotEquals(targetType))
            {
                type = type.BaseType;
                if (type == null)
                {
                    break;
                }

                // first index is base type index
                writer.Write(", i32 0");
            }

            // find index
            writer.Write(", i32 ");
            writer.Write(fieldIndex);
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <param name="count">
        /// </param>
        public void WriteFieldStart(IField field, int number, int count)
        {
            if (field.IsStatic)
            {
                return;
            }

            this.WriteFieldType(field.FieldType);
        }

        /// <summary>
        /// </summary>
        /// <param name="fieldType">
        /// </param>
        public void WriteFieldType(IType fieldType)
        {
            this.Output.WriteLine(',');

            CheckIfExternalDeclarationIsRequired(fieldType);

            fieldType.WriteTypePrefix(this.Output, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <param name="count">
        /// </param>
        public void WriteForwardDeclaration(IType type, int number, int count)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="pointerToInterfaceVirtualTablePointersResultNumber">
        /// </param>
        public void WriteGetThisPointerFromInterfacePointer(
            LlvmIndentedTextWriter writer,
            OpCodePart opCodeMethodInfo,
            IMethod methodInfo,
            IType thisType,
            FullyDefinedReference pointerToInterfaceVirtualTablePointersResultNumber)
        {
            writer.WriteLine("; Get 'this' from Interface Virtual Table");

            this.WriteGetInterfaceOffsetToObjectRootPointer(writer, opCodeMethodInfo, methodInfo, thisType);

            writer.WriteLine(string.Empty);
            var offsetAddressAsIntResultNumber = opCodeMethodInfo.Result;

            // convert to i8*
            this.WriteSetResultNumber(opCodeMethodInfo, thisType);
            writer.Write("bitcast ");
            this.WriteMethodPointerType(writer, methodInfo, thisType);
            writer.Write("** ");
            this.WriteResult(pointerToInterfaceVirtualTablePointersResultNumber);
            writer.Write(" to i8*");
            writer.WriteLine(string.Empty);
            var pointerToInterfaceVirtualTablePointersAsBytePointerResultNumber = opCodeMethodInfo.Result;

            // get 'this' address
            var thisAddressFromInterfaceResultNumber = this.WriteSetResultNumber(opCodeMethodInfo, thisType);
            writer.Write("getelementptr i8* ");
            this.WriteResult(pointerToInterfaceVirtualTablePointersAsBytePointerResultNumber);
            writer.Write(", i32 ");
            this.WriteResult(offsetAddressAsIntResultNumber);
            writer.WriteLine(string.Empty);

            // adjust 'this' pointer
            this.WriteSetResultNumber(opCodeMethodInfo, thisType);
            writer.Write("bitcast i8* ");
            this.WriteResult(thisAddressFromInterfaceResultNumber);
            writer.Write(" to ");
            thisType.WriteTypePrefix(writer);
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <param name="interface">
        /// </param>
        public void WriteInterfaceAccess(LlvmIndentedTextWriter writer, OpCodePart opCode, IType declaringType, IType @interface)
        {
            var objectResult = opCode.Result;

            writer.WriteLine("; Get interface '{0}' of '{1}'", @interface, declaringType);

            declaringType.UseAsClass = true;

            this.ProcessOperator(
                writer, opCode, "getelementptr inbounds", declaringType, @interface, OperandOptions.TypeIsInOperator | OperandOptions.GenerateResult);
            writer.Write(' ');
            writer.Write(objectResult);

            this.CheckIfTypeIsRequiredForBody(declaringType);

            this.WriteInterfaceIndex(writer, declaringType, @interface);

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="label">
        /// </param>
        public void WriteLabel(LlvmIndentedTextWriter writer, string label)
        {
            writer.Indent--;
            writer.WriteLine(string.Concat(label, ':'));
            writer.Indent++;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        public void WriteMethodDefinitionName(LlvmIndentedTextWriter writer, IMethod methodBase, IType ownerOfExplicitInterface = null)
        {
            writer.Write(this.GetFullMethodName(methodBase, ownerOfExplicitInterface));
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteMethodEnd(IMethod method, IGenericContext genericContext)
        {
            this.WriteMethodBody();

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
        public void WriteMethodParamsDef(
            LlvmIndentedTextWriter writer, IEnumerable<IParameter> parameterInfos, bool hasThis, IType thisType, IType returnType, bool noArgumentName = false)
        {
            writer.Write("(");

            var hasParameterWritten = false;

            if (returnType.IsStructureType())
            {
                returnType.WriteTypePrefix(writer, returnType.IsStructureType());
                hasParameterWritten = true;

                if (!noArgumentName)
                {
                    writer.Write(" noalias sret %agg.result");
                }
            }

            var start = hasThis ? 1 : 0;

            if (hasThis)
            {
                if (returnType.IsStructureType())
                {
                    writer.Write(", ");
                }

                thisType.UseAsClass = true;

                thisType.WriteTypePrefix(writer, true);
                hasParameterWritten = true;

                if (!noArgumentName)
                {
                    writer.Write(" %arg.this");
                }
            }

            var index = start;
            foreach (var parameter in parameterInfos)
            {
                this.CheckIfExternalDeclarationIsRequired(parameter.ParameterType);

                if (hasParameterWritten)
                {
                    writer.Write(", ");
                }

                parameter.ParameterType.WriteTypePrefix(writer, parameter.ParameterType.IsStructureType());
                hasParameterWritten = true;

                if (parameter.ParameterType.IsStructureType())
                {
                    this.CheckIfTypeIsRequiredForBody(parameter.ParameterType);
                    if (!noArgumentName)
                    {
                        if (!parameter.IsOut)
                        {
                            writer.Write(" byval align " + PointerSize);
                        }

                        writer.Write(" %\"");
                    }
                }
                else if (!noArgumentName)
                {
                    writer.Write(" %\"arg.");
                }

                if (!noArgumentName)
                {
                    writer.Write(parameter.Name);
                    writer.Write("\"");
                }

                index++;
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
        public void WriteMethodPointerType(LlvmIndentedTextWriter writer, IMethod methodBase, IType thisType = null)
        {
            var methodInfo = methodBase;
            var isStructureType = methodInfo.ReturnType.IsStructureType();
            if (!isStructureType)
            {
                methodInfo.ReturnType.WriteTypePrefix(writer);
            }
            else
            {
                writer.Write("void");
            }

            writer.Write(" (");

            if (isStructureType)
            {
                methodInfo.ReturnType.WriteTypePrefix(writer, true);
            }

            var hasThis = !methodInfo.IsStatic;
            if (hasThis)
            {
                if (isStructureType)
                {
                    writer.Write(", ");
                }

                (thisType ?? methodInfo.DeclaringType).WriteTypePrefix(writer);
            }

            var index = 0;
            foreach (var parameter in methodBase.GetParameters())
            {
                if (index > 0 || hasThis || isStructureType)
                {
                    writer.Write(", ");
                }

                if (!parameter.ParameterType.IsStructureType())
                {
                    parameter.ParameterType.WriteTypePrefix(writer);
                }
                else
                {
                    parameter.ParameterType.ToClass().WriteTypePrefix(writer);
                }

                index++;
            }

            writer.Write(")*");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="method">
        /// </param>
        public void WriteMethodReturnType(LlvmIndentedTextWriter writer, IMethod method)
        {
            this.CheckIfExternalDeclarationIsRequired(method.ReturnType);

            if (!method.ReturnType.IsVoid() && !method.ReturnType.IsStructureType())
            {
                method.ReturnType.WriteTypePrefix(writer, false);
                writer.Write(" ");
            }
            else
            {
                this.Output.Write("void ");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteMethodStart(IMethod method, IGenericContext genericContext)
        {
            if (method.IsUnmanaged && this.processedMethods.Any(m => m.Name == method.Name))
            {
                return;
            }

            this.StartProcess();

            this.processedMethods.Add(method);

            this.ReadMethodInfo(method, genericContext);

            var isMain = method.IsStatic && method.CallingConvention.HasFlag(CallingConventions.Standard) && method.Name.Equals("Main");

            // check if main
            if (isMain)
            {
                this.MainMethod = method;
            }

            if (method.IsAbstract)
            {
                return;
            }

            var isDelegateBodyFunctions = method.IsDelegateFunctionBody();
            if ((method.IsAbstract || this.NoBody) && !isDelegateBodyFunctions)
            {
                if (!method.IsUnmanagedMethodReference)
                {
                    this.Output.Write("declare ");
                }
                else
                {
                    this.WriteMethodDefinitionName(this.Output, method);
                    this.Output.Write(" = ");
                }

                if (method.IsDllImport)
                {
                    this.Output.Write("dllimport ");
                }
                else if (method.IsUnmanagedMethodReference)
                {
                    this.Output.Write("external ");
                }

                if (method.IsUnmanagedMethodReference)
                {
                    this.Output.Write("global ");
                }

                if (!method.IsUnmanagedMethodReference && method.DllImportData != null && method.DllImportData.CallingConvention == CallingConvention.StdCall)
                {
                    this.Output.Write("x86_stdcallcc ");
                }
            }
            else
            {
                this.Output.Write("define ");
                if (this.ThisType.IsGenericType)
                {
                    this.Output.Write("linkonce_odr ");
                }
            }

            // return type
            this.WriteMethodReturnType(this.Output, method);

            // name
            if (!method.IsUnmanagedMethodReference)
            {
                this.WriteMethodDefinitionName(this.Output, method);
            }

            if (method.IsExternalLibraryMethod())
            {
                this.Output.Write("(...)");
            }
            else
            {
                this.WriteMethodParamsDef(
                    this.Output, method.GetParameters(), this.HasMethodThis, this.ThisType, method.ReturnType, method.IsUnmanagedMethodReference);
            }

            if (method.IsUnmanagedMethodReference)
            {
                this.Output.Write("*");
            }
            else
            {
                this.WriteMethodNumber();
            }

            // write local declarations
            var methodBodyBytes = method.ResolveMethodBody(genericContext);
            if (methodBodyBytes != null)
            {
                this.Output.WriteLine(" {");

                this.Output.Indent++;

                this.WriteLocalVariableDeclarations(methodBodyBytes.LocalVariables);
                this.WriteArgumentCopyDeclarations(method.GetParameters(), this.HasMethodThis);

                this.Output.StartMethodBody();
            }
            else
            {
                if (isDelegateBodyFunctions)
                {
                    this.WriteDelegateFunctionBody(method);
                }

                this.Output.WriteLine(string.Empty);
            }

            methodNumberIncremental++;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public void WritePhi(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.WriteLine(string.Empty);

            var firstValueWithRequiredType = opCode.AlternativeValues.Values.FirstOrDefault(v => v.RequiredResultType != null);
            var firstValueRequiredType = firstValueWithRequiredType != null ? firstValueWithRequiredType.RequiredResultType : null;

            var phiType = firstValueRequiredType
                          ?? (opCode.AlternativeValues.Values.FirstOrDefault(v => !(v.Result is ConstValue)) ?? opCode.AlternativeValues.Values.First()).Result
                                                                                                                                                        .Type;

            // adjust types of constants
            if (!phiType.IsValueType)
            {
                foreach (var val in opCode.AlternativeValues.Values.Where(v => v.Result is ConstValue && v.Any(Code.Ldc_I4_0)))
                {
                    val.Result = new ConstValue(null, this.ResolveType("System.Void").ToPointerType());
                }
            }

            // apply PHI is condition is complex
            var nopeCode = OpCodePart.CreateNop;
            this.ProcessOperator(writer, nopeCode, "phi", phiType, phiType, options: OperandOptions.GenerateResult);

            var count = opCode.AlternativeValues.Values.Count;
            for (var index = 0; index < count; index++)
            {
                if (index > 0)
                {
                    writer.Write(",");
                }

                var values = opCode.AlternativeValues.Values;
                this.WritePhiNodeLabel(
                    writer,
                    values[index].Result,
                    values[index],
                    values[index],
                    string.Concat("a", opCode.AlternativeValues.Labels[index]),
                    opCode.AlternativeValues.Labels[index]);
            }

            writer.WriteLine(string.Empty);

            opCode.AlternativeValues.Values.Last().Result = nopeCode.Result;

            // clear it after processing
            opCode.AlternativeValues = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void WritePostDeclarations(IType type)
        {
            if (!type.IsGenericType && this.AssemblyQualifiedName != type.AssemblyQualifiedName)
            {
                return;
            }

            if (this.postDeclarationsProcessedTypes.Contains(type))
            {
                return;
            }

            this.postDeclarationsProcessedTypes.Add(type);

            this.WriteStaticFieldDeclarations(type);
            this.WriteInterfaceVirtaulTables(type);

            this.Output.WriteLine(string.Empty);

            type.WriteRtti(this);

            this.processedRttiTypes.Add(type);
            this.processedRttiPointerTypes.Add(type);

            this.Output.WriteLine(string.Empty);

            type.WriteInitObjectMethod(this);

            var stored = type.UseAsClass;
            type.UseAsClass = false;

            var isEnum = type.IsEnum;
            var canBeBoxed = type.IsPrimitiveType() || type.IsStructureType() || isEnum;
            var canBeUnboxed = canBeBoxed;
            var excluded = type.FullName == "System.Enum" || type.FullName == "System.IntPtr" || type.FullName == "System.UIntPtr";

            if (canBeBoxed && !excluded)
            {
                type.WriteBoxMethod(this);
            }

            if (canBeUnboxed && !excluded)
            {
                type.WriteUnboxMethod(this);
            }

            if (isEnum)
            {
                type.WriteGetHashCodeMethod(this);
            }

            type.UseAsClass = stored;
        }

        /// <summary>
        /// </summary>
        public void WriteRequiredTypesForBody()
        {
            ////// get all required types for methods bodies
            ////var count = 0;
            ////do
            ////{
            ////    var requiredTypesForBodyToWrite = this.requiredTypesForBody.ToArray();
            ////    count = requiredTypesForBodyToWrite.Length;
            ////    foreach (var requiredType in requiredTypesForBodyToWrite)
            ////    {
            ////        this.WriteTypeDefinitionIfNotWrittenYet(requiredType);
            ////    }
            ////}
            ////while (count > 0 && this.requiredTypesForBody.Count != count);
            foreach (var requiredType in this.requiredTypesForBody.ToArray())
            {
                this.WriteTypeDefinitionIfNotWrittenYet(requiredType);
            }

            // clear types for next type
            this.requiredTypesForBody.Clear();
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
            Debug.Assert(reference != null && !string.IsNullOrWhiteSpace(reference.ToString()));

            // write number of method
            this.Output.Write(reference);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="methodReturnType">
        /// </param>
        public void WriteReturn(LlvmIndentedTextWriter writer, OpCodePart opCode, IType methodReturnType)
        {
            var opts = this.WriteReturnStruct(opCode, methodReturnType);

            writer.WriteLine(string.Empty);

            if (!methodReturnType.IsVoid())
            {
                this.UnaryOper(writer, opCode, "ret", methodReturnType, options: opts | OperandOptions.AdjustIntTypes);

                if (methodReturnType.IsStructureType())
                {
                    writer.Write("void");
                }
            }
            else
            {
                writer.Write("ret void");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="methodReturnType">
        /// </param>
        /// <returns>
        /// </returns>
        public OperandOptions WriteReturnStruct(OpCodePart opCode, IType methodReturnType)
        {
            var opts = OperandOptions.None;

            if (methodReturnType.IsStructureType())
            {
                var operands = opCode.OpCodeOperands;
                var opCodeOperand = operands[0];
                if (!opCodeOperand.HasResult)
                {
                    opCodeOperand.Destination = new FullyDefinedReference("%agg.result", methodReturnType);
                }
                else
                {
                    opCode.Destination = new FullyDefinedReference("%agg.result", methodReturnType);
                    this.WriteLlvmLoad(opCode, opCodeOperand.Result.ToNormalType());
                }

                opts |= OperandOptions.IgnoreOperand;
            }

            return opts;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public IncrementalResult WriteSetResultNumber(OpCodePart opCode, IType type)
        {
            var writer = this.Output;

            // write number of method
            writer.Write("%.r");
            writer.Write(++this.resultNumberIncremental);
            writer.Write(" = ");

            var llvmResult = new IncrementalResult(this.resultNumberIncremental, type);
            if (opCode != null)
            {
                opCode.Result = llvmResult;
            }

            return llvmResult;
        }

        /// <summary>
        /// </summary>
        /// <param name="moduleName">
        /// </param>
        /// <param name="assemblyName">
        /// </param>
        public void WriteStart(string moduleName, string assemblyName, bool isCoreLib, IEnumerable<string> allReference)
        {
            this.AssemblyQualifiedName = assemblyName;
            this.IsCoreLib = isCoreLib;
            this.AllReference = allReference;

            this.Output.WriteLine(
                "target datalayout = \"e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32\"");
            this.Output.WriteLine("target triple = \"{0}\"", string.IsNullOrWhiteSpace(this.Target) ? "i686-pc-win32" : this.Target);
            this.Output.WriteLine(string.Empty);

            if (this.Gctors)
            {
                // Global ctors
                this.Output.WriteLine(
                    "@llvm.global_ctors = appending global [1 x { i32, void ()* }] [{ i32, void ()* } { i32 65535, void ()* " + this.GetGlobalConstructorsFunctionName() + " }]");
                this.Output.WriteLine(string.Empty);
            }

            // declarations
            this.Output.WriteLine(Resources.llvm_declarations);
            this.Output.WriteLine(string.Empty);

            if (this.Gc)
            {
                this.Output.WriteLine(Resources.llvm_gc_declarations);
                this.Output.WriteLine(string.Empty);
            }

            this.StaticConstructors.Clear();
            VirtualTableGen.Clear();
            TypeGen.Clear();
        }

        /// <summary>
        /// </summary>
        public void WriteStoredText()
        {
            this.Output.Write(this.storedText);
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
            this.Output = new LlvmIndentedTextWriter(new StringWriter(sb));

            action();

            this.Output = output;
            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void WriteTypeEnd(IType type)
        {
            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteTypeStart(IType type, IGenericContext genericContext)
        {
            this.processedTypes.Add(type);

            // get all required types for type definition
            var requiredTypes = new List<IType>();
            Il2Converter.ProcessRequiredITypesForITypes(new[] { type }, new HashSet<IType>(), requiredTypes, null, null);
            foreach (var requiredType in requiredTypes)
            {
                this.WriteTypeDefinitionIfNotWrittenYet(requiredType);
            }

            var interfacesList = type.GetInterfaces();
            foreach (var @interface in interfacesList)
            {
                this.WriteTypeDefinitionIfNotWrittenYet(@interface);
            }

            this.ReadTypeInfo(type);

            this.WriteTypeDeclarationStart(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="intType">
        /// </param>
        /// <returns>
        /// </returns>
        private static OpCodePart GetTypedIntZeroCode(IType intType)
        {
            switch (intType.IntTypeBitSize())
            {
                case 64:
                    return new OpCodeInt64Part(OpCodesEmit.Ldc_I8, 0, 0, 0);
                default:
                    return new OpCodePart(OpCodesEmit.Ldc_I4_0, 0, 0);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="usedTypes">
        /// </param>
        private static void MethodsWalker(IMethod method, HashSet<IType> usedTypes)
        {
            var calledMethods = new HashSet<IMethod>();
            var readStaticFields = new HashSet<IField>();

            method.DiscoverMethod(usedTypes, calledMethods, readStaticFields);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        private void AdjustResultType(OpCodePart opCode)
        {
            // cast result if required
            if (opCode.RequiredResultType != null && opCode.Result != null && opCode.RequiredResultType.TypeNotEquals(opCode.Result.Type)
                && !(opCode.Result is ConstValue))
            {
                bool castRequired;
                bool intAdjustmentRequired;
                this.DetectConversion(opCode.Result.Type, opCode.RequiredResultType, out castRequired, out intAdjustmentRequired);

                if (castRequired)
                {
                    this.Output.WriteLine(string.Empty);
                    this.WriteCast(opCode, opCode.Result, opCode.RequiredResultType);
                }

                if (intAdjustmentRequired)
                {
                    this.Output.WriteLine(string.Empty);
                    this.AdjustIntConvertableTypes(this.Output, opCode, opCode.RequiredResultType);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="effectiveType">
        /// </param>
        /// <param name="castFrom">
        /// </param>
        /// <param name="intAdjustment">
        /// </param>
        /// <param name="intAdjustSecondOperand">
        /// </param>
        /// <param name="resultType">
        /// </param>
        /// <param name="operand1">
        /// </param>
        /// <param name="operand2">
        /// </param>
        /// <returns>
        /// </returns>
        private IType ApplyTypeAdjustment(
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            IType effectiveType,
            IType castFrom,
            IType intAdjustment,
            bool intAdjustSecondOperand,
            ref IType resultType,
            int operand1 = 0,
            int operand2 = 1)
        {
            if (castFrom != null && opCode.OpCodeOperands[operand1].HasResult)
            {
                this.WriteCast(opCode.OpCodeOperands[operand1], opCode.OpCodeOperands[operand1].Result, effectiveType);
            }

            if (intAdjustment != null && opCode.OpCodeOperands[operand1].HasResult)
            {
                var changeType = this.AdjustIntConvertableTypes(
                    writer,
                    opCode.OpCodeOperands[operand2 >= 0 && opCode.OpCodeOperands.Length > operand2 && intAdjustSecondOperand ? operand2 : operand1],
                    intAdjustment);

                if (changeType && resultType == null)
                {
                    resultType = intAdjustment;
                    effectiveType = intAdjustment;
                }
            }

            return effectiveType;
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
        private void BinaryOper(
            LlvmIndentedTextWriter writer, OpCodePart opCode, string op, OperandOptions options = OperandOptions.None, IType resultType = null)
        {
            if (opCode.HasResult)
            {
                return;
            }

            this.PreProcessOperand(writer, opCode, 0, options);
            this.PreProcessOperand(writer, opCode, 1, options);

            this.ProcessOperator(writer, opCode, op, options: options, resultType: resultType);

            this.PostProcessOperand(writer, opCode, 0);
            writer.Write(',');
            this.PostProcessOperand(writer, opCode, 1, options.HasFlag(OperandOptions.DetectAndWriteTypeInSecondOperand));
        }

        /// <summary>
        /// </summary>
        /// <param name="fieldInfo">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        private int CalculateFieldIndex(IField fieldInfo, IType type)
        {
            var list = IlReader.Fields(type).Where(t => !t.IsStatic).ToList();
            var index = 0;

            while (index < list.Count && list[index].NameNotEquals(fieldInfo))
            {
                index++;
            }

            if (index == list.Count)
            {
                throw new KeyNotFoundException();
            }

            index += this.CalculateFirstFieldIndex(type);

            this.indexByFieldInfo[fieldInfo.GetFullName()] = index;

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private int CalculateFirstFieldIndex(IType type)
        {
            var index = 0;

            if (type.BaseType != null)
            {
                index++;
            }

            // add shift for virtual table
            if (type.IsRootOfVirtualTable())
            {
                index++;
            }

            // add shift for interfaces
            index += type.GetInterfaces().Count();

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="sourceType">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        /// <param name="castRequired">
        /// </param>
        /// <param name="intAdjustmentRequired">
        /// </param>
        private void DetectConversion(IType sourceType, IType requiredType, out bool castRequired, out bool intAdjustmentRequired)
        {
            castRequired = false;
            intAdjustmentRequired = false;
            if (sourceType.TypeEquals(requiredType))
            {
                return;
            }

            var sourceTypePointer = sourceType.IsPointer;
            var requiredTypePointer = requiredType.IsPointer;
            if (sourceTypePointer && requiredTypePointer)
            {
                castRequired = true;
                return;
            }

            var sourceIntType = sourceType.IntTypeBitSize() > 0;
            var requiredIntType = requiredType.IntTypeBitSize() > 0;
            if (sourceIntType && requiredIntType)
            {
                if (requiredType.IsIntValueTypeExtCastRequired(sourceType) || requiredType.IsIntValueTypeTruncCastRequired(sourceType))
                {
                    intAdjustmentRequired = true;
                }

                // pointer to int, int to pointer
                if (!sourceType.IsByRef && !sourceType.IsPointer && sourceType.IntTypeBitSize() > 0 && requiredType.IsPointer)
                {
                    intAdjustmentRequired = true;
                }

                return;
            }

            if (sourceType.TypeEquals(this.ResolveType("System.Boolean")) && requiredType.TypeEquals(this.ResolveType("System.Byte")))
            {
                return;
            }

            castRequired = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        /// <param name="options">
        /// </param>
        /// <param name="castFrom">
        /// </param>
        /// <param name="intAdjustment">
        /// </param>
        /// <param name="intAdjustSecondOperand">
        /// </param>
        /// <param name="operand1">
        /// </param>
        /// <param name="operand2">
        /// </param>
        /// <returns>
        /// </returns>
        private IType DetectTypePrefix(
            OpCodePart opCode,
            IType requiredType,
            OperandOptions options,
            out IType castFrom,
            out IType intAdjustment,
            out bool intAdjustSecondOperand,
            int operand1 = 0,
            int operand2 = 1)
        {
            castFrom = null;
            intAdjustment = null;
            intAdjustSecondOperand = false;

            var res1 = options.HasFlag(OperandOptions.TypeIsInOperator)
                           ? this.ResultOf(opCode)
                           : opCode.OpCodeOperands != null && operand1 >= 0 && opCode.OpCodeOperands.Length > operand1
                                 ? this.ResultOf(opCode.OpCodeOperands[operand1])
                                 : null;

            var res2 = opCode.OpCodeOperands != null && operand2 >= 0 && opCode.OpCodeOperands.Length > operand2
                           ? this.ResultOf(opCode.OpCodeOperands[operand2])
                           : null;

            // write type
            var effectiveType = this.ResolveType("System.Void");

            var res1Pointer = options.HasFlag(OperandOptions.CastPointersToBytePointer) && res1 != null && res1.IsPointerAccessRequired;
            var res2Pointer = options.HasFlag(OperandOptions.CastPointersToBytePointer) && res2 != null && res2.IsPointerAccessRequired;
            var requiredTypePointer = options.HasFlag(OperandOptions.CastPointersToBytePointer) && requiredType != null && requiredType.IsPointer;

            if (res1Pointer && (res2Pointer || requiredTypePointer))
            {
                if (res2 != null && (!res2.IsConst || res1.IsConst))
                {
                    castFrom = res1.Type;
                    effectiveType = res2.Type;
                }
                else
                {
                    castFrom = res1.Type;
                    effectiveType = requiredTypePointer ? requiredType : res1.Type;
                }

                if (effectiveType.TypeEquals(castFrom))
                {
                    castFrom = null;
                }
            }
            else if (requiredType != null)
            {
                if (options.HasFlag(OperandOptions.AdjustIntTypes) && res1 != null && res1.Type != null && res2 != null && res2.Type != null
                    && res1.Type.TypeEquals(res2.Type) && res1.Type.TypeNotEquals(requiredType) && res1.Type.TypeEquals(this.ResolveType("System.Boolean"))
                    && requiredType.TypeEquals(this.ResolveType("System.Byte")))
                {
                    effectiveType = res1.Type;
                }
                else
                {
                    effectiveType = requiredType;
                }
            }
            else if (options.HasFlag(OperandOptions.TypeIsInOperator) || opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > operand1)
            {
                if (!(res1 == null || res1.IsConst) || res2 == null || res2.IsConst)
                {
                    effectiveType = res1.Type;
                }
                else
                {
                    effectiveType = res2.Type;
                }
            }

            if (res1 != null && res1.Type.TypeNotEquals(effectiveType)
                && (res1.Type.IsClass || res1.Type.IsArray || res1.Type.IsInterface || res1.Type.IsDelegate) && effectiveType.IsAssignableFrom(res1.Type))
            {
                castFrom = res1.Type;
            }

            if (options.HasFlag(OperandOptions.AdjustIntTypes))
            {
                var firstType = res1 != null && res1.Type != null && !res1.IsConst
                                    ? res1.Type
                                    : res2 != null && res2.Type != null && !res2.IsConst ? res2.Type : null;

                IType secondType = null;
                if (firstType != null)
                {
                    if (res2 != null && res2.Type != null && !res2.IsConst)
                    {
                        secondType = res2.Type;
                    }

                    if (requiredType != null)
                    {
                        secondType = requiredType;
                    }

                    if (secondType != null)
                    {
                        if (firstType.IsIntValueTypeExtCastRequired(secondType))
                        {
                            intAdjustSecondOperand = true;
                            intAdjustment = firstType;
                        }

                        if (secondType.IsIntValueTypeExtCastRequired(firstType))
                        {
                            intAdjustSecondOperand = false;
                            intAdjustment = secondType;
                        }

                        // pointer to int, int to pointer
                        if (!firstType.IsByRef && !firstType.IsPointer && firstType.IntTypeBitSize() > 0 && secondType.IsPointer)
                        {
                            intAdjustSecondOperand = true;
                            intAdjustment = firstType;

                            if (requiredType != null && requiredType.IsPointer)
                            {
                                intAdjustment = secondType;
                            }
                        }

                        if (!secondType.IsByRef && !secondType.IsPointer && secondType.IntTypeBitSize() > 0 && firstType.IsPointer)
                        {
                            intAdjustSecondOperand = false;
                            intAdjustment = secondType;

                            if (requiredType != null && requiredType.IsPointer)
                            {
                                intAdjustment = firstType;
                            }
                        }
                    }
                }
            }

            return effectiveType;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeFieldInfoPart">
        /// </param>
        private void FieldAccessAndSaveToField(OpCodeFieldInfoPart opCodeFieldInfoPart)
        {
            var writer = this.Output;

            this.PreProcessOperand(writer, opCodeFieldInfoPart, 1);
            this.WriteFieldAccess(writer, opCodeFieldInfoPart);
            writer.WriteLine(string.Empty);

            var fieldType = opCodeFieldInfoPart.Operand.FieldType;

            this.SaveToField(opCodeFieldInfoPart, fieldType);
        }

        /// <summary>
        /// </summary>
        /// <param name="firstOpCode">
        /// </param>
        /// <param name="lastOpCode">
        /// </param>
        /// <param name="stopAddress">
        /// </param>
        /// <returns>
        /// </returns>
        private string FindCustomLabel(OpCodePart firstOpCode, OpCodePart lastOpCode, int stopAddress)
        {
            if (lastOpCode == null)
            {
                return null;
            }

            var current = lastOpCode;
            while (current != null && firstOpCode.GroupAddressStart <= current.AddressStart && current.AddressStart >= stopAddress)
            {
                if (current.CreatedLabel != null)
                {
                    return current.CreatedLabel;
                }

                current = current.PreviousOpCode(this);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetArgVarName(int index)
        {
            return this.GetArgVarName(this.Parameters[index]);
        }

        /// <summary>
        /// </summary>
        /// <param name="parameter">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetArgVarName(IParameter parameter)
        {
            return GetArgVarName(parameter.Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetArgVarName(string name)
        {
            return string.Format("%\"{0}\"", name);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodBase">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetFullMethodName(IMethod methodBase, IType ownerOfExplicitInterface = null)
        {
            if (methodBase.IsUnmanaged || methodBase.IsDllImport)
            {
                return string.Concat('@', methodBase.Name.StartsWith("llvm_") ? methodBase.Name.Replace('_', '.') : methodBase.Name);
            }

            if (methodBase.ToString().StartsWith("%"))
            {
                return methodBase.ToString();
            }

            var sb = new StringBuilder();
            sb.Append("@\"");

            if (ownerOfExplicitInterface != null)
            {
                sb.Append(methodBase.ToString(ownerOfExplicitInterface));
            }
            else
            {
                sb.Append(methodBase);
            }

            sb.Append('"');

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        private IType GetTypeOfReference(OpCodePart opCode)
        {
            IType type = null;
            if (opCode.HasResult)
            {
                type = opCode.Result.Type;
            }
            else if (opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0)
            {
                var resultOf = this.ResultOf(opCode.OpCodeOperands[0]);
                type = resultOf.Type;
            }
            else
            {
                type = this.ResolveType("System.Byte").ToPointerType();
            }

            if (type.IsArray || type.IsByRef)
            {
                return type.GetElementType();
            }

            return type;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsFloatingPointOp(OpCodePart opCode)
        {
            return opCode.OpCodeOperands.Length > 0 && this.IsFloatingPointOpOperand(opCode.OpCodeOperands[0])
                   || opCode.OpCodeOperands.Length > 1 && this.IsFloatingPointOpOperand(opCode.OpCodeOperands[1]);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsFloatingPointOpOperand(OpCodePart opCode)
        {
            var op1ReturnResult = this.ResultOf(opCode);

            // TODO: result of unbox is null, fix it
            if (op1ReturnResult == null || op1ReturnResult.Type == null)
            {
                return false;
            }

            var op1IsReal = op1ReturnResult.Type.IsReal();
            return op1IsReal;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void LoadElement(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.Result != null)
            {
                return;
            }

            var actualLoad = true;
            IType type = null;
            switch (opCode.ToCode())
            {
                case Code.Ldelem_I:
                    type = this.ResolveType("System.Int32");
                    break;
                case Code.Ldelem_I1:
                    type = this.ResolveType("System.SByte");
                    break;
                case Code.Ldelem_I2:
                    type = this.ResolveType("System.Int16");
                    break;
                case Code.Ldelem_I4:
                    type = this.ResolveType("System.Int32");
                    break;
                case Code.Ldelem_U1:
                    type = this.ResolveType("System.Byte");
                    break;
                case Code.Ldelem_U2:
                    type = this.ResolveType("System.UInt16");
                    break;
                case Code.Ldelem_U4:
                    type = this.ResolveType("System.UInt32");
                    break;
                case Code.Ldelem_I8:
                    type = this.ResolveType("System.Int64");
                    break;
                case Code.Ldelem_R4:
                    type = this.ResolveType("System.Single");
                    break;
                case Code.Ldelem_R8:
                    type = this.ResolveType("System.Double");
                    break;
                case Code.Ldelem:
                case Code.Ldelem_Ref:
                    type = this.GetTypeOfReference(opCode);
                    break;
                case Code.Ldelema:
                    actualLoad = false;
                    break;
            }

            this.BinaryOper(
                writer,
                opCode,
                "getelementptr inbounds",
                OperandOptions.GenerateResult | OperandOptions.DetectAndWriteTypeInSecondOperand,
                type);

            this.CheckIfTypeIsRequiredForBody(opCode.OpCodeOperands[0].Result.Type);

            if (actualLoad)
            {
                writer.WriteLine(string.Empty);

                var accessIndexResultNumber = opCode.Result;
                opCode.Result = null;

                var isWaitingAllInfoToLoadStruct = type.IsStructureType() && opCode.Destination == null;
                if (!isWaitingAllInfoToLoadStruct)
                {
                    this.WriteLlvmLoad(opCode, type, accessIndexResultNumber);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void SaveElement(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            IType type = null;

            switch (opCode.ToCode())
            {
                case Code.Stelem_I:
                    type = this.ResolveType("System.Int32");
                    break;
                case Code.Stelem_I1:
                    type = this.ResolveType("System.SByte");
                    break;
                case Code.Stelem_I2:
                    type = this.ResolveType("System.Int16");
                    break;
                case Code.Stelem_I4:
                    type = this.ResolveType("System.Int32");
                    break;
                case Code.Stelem_I8:
                    type = this.ResolveType("System.Int64");
                    break;
                case Code.Stelem_R4:
                    type = this.ResolveType("System.Single");
                    break;
                case Code.Stelem_R8:
                    type = this.ResolveType("System.Double");
                    break;
                case Code.Stelem:
                case Code.Stelem_Ref:
                    type = this.GetTypeOfReference(opCode);
                    break;
            }

            this.BinaryOper(
                writer,
                opCode,
                "getelementptr inbounds",
                options: OperandOptions.GenerateResult | OperandOptions.DetectAndWriteTypeInSecondOperand,
                resultType: type);

            this.CheckIfTypeIsRequiredForBody(opCode.OpCodeOperands[0].Result.Type);

            writer.WriteLine(string.Empty);

            var operandIndex = 2;

            this.PreProcessOperand(writer, opCode, operandIndex);

            if (!type.IsStructureType())
            {
                this.AdjustIntConvertableTypes(writer, opCode.OpCodeOperands[operandIndex], type);

                this.ProcessOperator(writer, opCode, "store", type, operand1: 2, operand2: -1);
                this.PostProcessOperand(writer, opCode, operandIndex);

                writer.Write(", ");
                type.WriteTypePrefix(writer, type.IsStructureType());
                writer.Write("* {0}", opCode.Result);
            }
            else
            {
                this.SaveStructElement(writer, opCode, operandIndex, type);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void SaveIndirect(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            IType type = null;

            switch (opCode.ToCode())
            {
                case Code.Stind_I:
                    type = this.ResolveType("System.Int32");
                    break;
                case Code.Stind_I1:
                    type = this.ResolveType("System.Byte");
                    break;
                case Code.Stind_I2:
                    type = this.ResolveType("System.Int16");
                    break;
                case Code.Stind_I4:
                    type = this.ResolveType("System.Int32");
                    break;
                case Code.Stind_I8:
                    type = this.ResolveType("System.Int64");
                    break;
                case Code.Stind_R4:
                    type = this.ResolveType("System.Single");
                    break;
                case Code.Stind_R8:
                    type = this.ResolveType("System.Double");
                    break;
                case Code.Stind_Ref:
                    type = this.GetTypeOfReference(opCode);
                    break;
            }

            var destinationType = opCode.OpCodeOperands[0].Result.Type;
            if (destinationType.IsPointer && destinationType.GetElementType().TypeNotEquals(type))
            {
                // adjust destination type, cast pointer to pointer of type
                this.WriteBitcast(opCode, opCode.OpCodeOperands[0].Result, type);
                opCode.OpCodeOperands[0].Result = opCode.Result;
                destinationType = type.ToPointerType();
                writer.WriteLine(string.Empty);
            }
            else if (destinationType.IsByRef && destinationType.GetElementType().TypeNotEquals(type))
            {
                type = destinationType.GetElementType();
            }

            if (!destinationType.IsPointer && destinationType.IntTypeBitSize() >= (PointerSize * 8) && destinationType.IntTypeBitSize() != type.IntTypeBitSize()
                && !opCode.OpCodeOperands[0].Result.Type.IsPointer && !opCode.OpCodeOperands[0].Result.Type.IsByRef)
            {
                // adjust destination type, cast pointer to pointer of type
                this.WriteIntToPtr(opCode, opCode.OpCodeOperands[0].Result, type);
                opCode.OpCodeOperands[0].Result = opCode.Result;
                destinationType = type.ToPointerType();
                writer.WriteLine(string.Empty);
            }

            this.PreProcessOperand(writer, opCode, 0);

            this.UnaryOper(writer, opCode, 1, "store", type, options: OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
            writer.Write(", ");

            destinationType.WriteTypePrefix(writer, true);
            this.PostProcessOperand(writer, opCode, 0);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="ooperandIndex">
        /// </param>
        private void SaveStruct(LlvmIndentedTextWriter writer, OpCodePart opCode, int ooperandIndex)
        {
            if (!opCode.OpCodeOperands[ooperandIndex].HasResult)
            {
                // we expect to see Ldobj here, so we set DestinationName to copy it into reserved stack
                opCode.OpCodeOperands[ooperandIndex].Destination = opCode.OpCodeOperands[0].Result;
                this.ActualWrite(writer, opCode.OpCodeOperands[ooperandIndex]);
            }
            else if (opCode.OpCodeOperands[0].Result.Type.ToNormal().IsStructureType())
            {
                opCode.Destination = opCode.OpCodeOperands[0].Result;
                this.WriteLlvmLoad(opCode, opCode.OpCodeOperands[ooperandIndex].Result.ToNormalType());
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="operandIndex">
        /// </param>
        /// <param name="type">
        /// </param>
        private void SaveStructElement(LlvmIndentedTextWriter writer, OpCodePart opCode, int operandIndex, IType type)
        {
            // copy struct
            if (!opCode.OpCodeOperands[operandIndex].HasResult)
            {
                opCode.OpCodeOperands[operandIndex].Destination = opCode.Result;
                this.ActualWrite(writer, opCode.OpCodeOperands[operandIndex]);
            }
            else
            {
                opCode.Destination = opCode.Result;
                var fullyDefinedRef = opCode.OpCodeOperands[operandIndex].Result;
                this.WriteLlvmLoad(opCode, type, fullyDefinedRef);
            }
        }

        /// <summary>
        /// </summary>
        private void SortStaticConstructorsByUsage()
        {
            var staticConstructors = new Dictionary<IConstructor, HashSet<IType>>();
            foreach (var staticCtor in this.StaticConstructors)
            {
                var methodWalker = new MethodsWalker(staticCtor);
                var reaquiredTypesWithStaticFields = methodWalker.DiscoverAllStaticFieldsDependencies();
                staticConstructors.Add(staticCtor, reaquiredTypesWithStaticFields);
            }

            // rebuild order
            var newStaticConstructors = new List<IConstructor>();

            var countBefore = 0;
            do
            {
                countBefore = staticConstructors.Count;
                foreach (var staticConstructorPair in staticConstructors.ToList())
                {
                    if (staticConstructorPair.Value.Any(v => staticConstructors.Keys.Any(k => k.DeclaringType.TypeEquals(v))))
                    {
                        continue;
                    }

                    staticConstructors.Remove(staticConstructorPair.Key);
                    newStaticConstructors.Add(staticConstructorPair.Key);
                }
            }
            while (staticConstructors.Count > 0 && countBefore != staticConstructors.Count);

            Debug.Assert(staticConstructors.Keys.Count == 0, "Not All static constructors were resolved");

            // add rest as is
            newStaticConstructors.AddRange(staticConstructors.Keys);

            this.StaticConstructors = newStaticConstructors;
        }

        /// <summary>
        /// </summary>
        /// <param name="parametersInfo">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        private void WriteArgumentCopyDeclarations(IEnumerable<IParameter> parametersInfo, bool hasThis)
        {
            if (hasThis)
            {
                this.WriteArgumentCopyDeclaration("this", this.ThisType, true);
            }

            foreach (var parameterInfo in parametersInfo)
            {
                this.WriteArgumentCopyDeclaration(parameterInfo.Name, parameterInfo.ParameterType);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pair">
        /// </param>
        private void WriteArrayData(KeyValuePair<int, byte[]> pair)
        {
            this.Output.Write(
                "@.array{0} = private unnamed_addr constant {4} i32, [{2} x i8] {5} {4} i32 {3}, [{2} x i8] [",
                pair.Key,
                pair.Value,
                pair.Value.Length,
                pair.Value.Length,
                '{',
                '}');

            var index = 0;
            foreach (var b in pair.Value)
            {
                if (index > 0)
                {
                    this.Output.Write(", ");
                }

                this.Output.Write("i8 {0}", b);
                index++;
            }

            this.Output.WriteLine("] {0}, align 1", '}');
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCatchBegins(LlvmIndentedTextWriter writer, OpCodePart opCode)
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
                writer.WriteLine(string.Empty);
                this.WriteCatchTest(exceptionHandler, exceptionHandler.Next);
            }

            writer.WriteLine(string.Empty);

            this.WriteCatchBegin(exceptionHandler);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        private void WriteCatchFinnallyCleanUpEnd(OpCodePart opCode)
        {
            if (opCode.CatchOrFinallyEnd == null)
            {
                return;
            }

            var eh = opCode.CatchOrFinallyEnd;
            opCode.CatchOrFinallyEnd = null;
            var ehPopped = this.catchScopes.Pop();
            Debug.Assert(ehPopped == eh, "Mismatch of exception handlers");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCatchFinnallyEnd(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.CatchOrFinallyEnd == null)
            {
                return;
            }

            var eh = opCode.CatchOrFinallyEnd;
            opCode.CatchOrFinallyEnd = null;
            writer.WriteLine(string.Empty);
            this.WriteCatchEnd(opCode, eh, this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCondBranch(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.WriteLine("br i1 {0}, label %.a{1}, label %.a{2}", opCode.Result, opCode.JumpAddress(), opCode.AddressEnd);
            writer.Indent--;
            writer.WriteLine(string.Concat(".a", opCode.AddressEnd, ':'));
            writer.Indent++;

            opCode.NextOpCode(this).JumpProcessed = true;
        }

        /// <summary>
        /// </summary>
        private void WriteConstData()
        {
            // write set of strings
            foreach (var pair in this.stringStorage)
            {
                this.WriteUnicodeString(pair);
            }

            if (this.stringStorage.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                this.stringStorage.Clear();
            }

            // write set of array data
            foreach (var pair in this.arrayStorage)
            {
                this.WriteArrayData(pair);
            }

            if (this.arrayStorage.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                this.arrayStorage.Clear();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        private void WriteExceptionEnvironment(IMethod method)
        {
            if (this.needToWriteUnwindException)
            {
                this.needToWriteUnwindException = false;
                this.WriteUnwindException();
            }

            if (this.needToWriteUnreachable)
            {
                this.needToWriteUnreachable = false;
                this.WriteUnreachable();
            }

            if (method.ExceptionHandlingClauses.Any())
            {
                this.WriteResume();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteExceptionHandlers(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.ExceptionHandlers == null)
            {
                return;
            }

            writer.WriteLine(string.Empty);
            this.WriteCatchProlog(opCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        /// <param name="thisType">
        /// </param>
        private void WriteGetInterfaceOffsetToObjectRootPointer(LlvmIndentedTextWriter writer, OpCodePart opCode, IMethod methodBase, IType thisType = null)
        {
            this.WriteSetResultNumber(opCode, this.ResolveType("System.Int32").ToPointerType());
            writer.Write("bitcast ");
            this.WriteMethodPointerType(writer, methodBase, thisType);
            writer.Write("* ");
            this.WriteResult(opCode.OpCodeOperands[0].Result);
            writer.Write(" to ");
            this.ResolveType("System.Int32").ToPointerType().WriteTypePrefix(writer);
            writer.WriteLine(string.Empty);

            var res = opCode.Result;
            var offsetResult = this.WriteSetResultNumber(opCode, this.ResolveType("System.Int32"));
            writer.Write("getelementptr ");
            this.ResolveType("System.Int32").WriteTypePrefix(writer);
            writer.Write("* ");
            this.WriteResult(res);
            writer.WriteLine(", i32 -{0}", ObjectInfrastructure.FunctionsOffsetInVirtualTable);

            opCode.Result = null;
            this.WriteLlvmLoad(opCode, this.ResolveType("System.Int32"), offsetResult);
        }

        /// <summary>
        /// </summary>
        private void WriteGlobalConstructors()
        {
            // write global ctors caller
            this.Output.WriteLine(string.Empty);
            this.Output.WriteLine("define {2} void {0}() {1}", this.GetGlobalConstructorsFunctionName(), "{", this.Gctors ? "internal" : string.Empty);
            this.Output.Indent++;

            this.SortStaticConstructorsByUsage();

            if (this.Gc && this.IsCoreLib)
            {
                this.Output.WriteLine("call void @GC_init()");
            }

            foreach (var staticCtor in this.StaticConstructors)
            {
                this.Output.WriteLine("call void {0}()", this.GetFullMethodName(staticCtor));
            }

            this.Output.WriteLine("ret void");
            this.Output.Indent--;
            this.Output.WriteLine("}");
        }

        private string GetGlobalConstructorsFunctionName()
        {
            return GetGlobalConstructorsFunctionName(this.AssemblyQualifiedName);
        }

        private string GetGlobalConstructorsFunctionName(string assemblyQualifiedName)
        {
            return string.Concat("@\"Global Ctors for ", this.AssemblyQualifiedName, "\"");
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
        private bool WriteInterfaceIndex(LlvmIndentedTextWriter writer, IType classType, IType @interface)
        {
            var type = classType;
            if (!type.GetAllInterfaces().Contains(@interface))
            {
                return false;
            }

            // first element for pointer (IType* + 0)
            writer.Write(", i32 0");

            while (!type.GetInterfacesExcludingBaseAllInterfaces().Any(i => i.TypeEquals(@interface) || i.GetAllInterfaces().Contains(@interface)))
            {
                type = type.BaseType;
                if (type == null)
                {
                    // throw new IndexOutOfRangeException("Could not find an interface");
                    break;
                }

                // first index is base type index
                writer.Write(", i32 0");
            }

            // find index
            var index = 0;

            if (type.IsRootOfVirtualTable())
            {
                index++;
            }

            if (type.BaseType != null)
            {
                index++;
            }

            var indexes = FindInterfaceIndexes(type, @interface, index);

            foreach (var i in indexes)
            {
                writer.Write(", i32 ");
                writer.Write(i);
            }

            return true;
        }

        // TODO: here the bug with index, index is caluclated for derived class but need to be calculated and used for type where the interface belong to
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
                throw new KeyNotFoundException("interface can't be found");
            }

            return interfaceIndex;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteInterfaceVirtaulTables(IType type)
        {
            // write VirtualTable
            if (!type.IsInterface)
            {
                var baseTypeSize = type.BaseType != null ? type.BaseType.GetTypeSize() : 0;

                var index = 0;
                if (type.HasAnyVirtualMethod())
                {
                    this.Output.WriteLine(string.Empty);
                    this.Output.Write(type.GetVirtualTableName());
                    var virtualTable = type.GetVirtualTable(this);
                    virtualTable.WriteTableOfMethods(this, type, 0, baseTypeSize);

                    foreach (var methodInVirtualTable in virtualTable)
                    {
                        this.CheckIfExternalDeclarationIsRequired(methodInVirtualTable.Value);
                    }

                    index++;
                }

                foreach (var @interface in type.SelectAllTopAndAllNotFirstChildrenInterfaces())
                {
                    var current = type;
                    IType typeContainingInterface = null;
                    while (current != null && current.GetAllInterfaces().Contains(@interface))
                    {
                        typeContainingInterface = current;
                        current = current.BaseType;
                    }

                    var baseTypeSizeOfTypeContainingInterface = typeContainingInterface.BaseType != null ? typeContainingInterface.BaseType.GetTypeSize() : 0;
                    var interfaceIndex = FindInterfaceIndexes(typeContainingInterface, @interface, index).Sum();

                    this.Output.WriteLine(string.Empty);
                    this.Output.Write(type.GetVirtualInterfaceTableName(@interface));
                    var virtualInterfaceTable = type.GetVirtualInterfaceTable(@interface);
                    virtualInterfaceTable.WriteTableOfMethods(this, type, interfaceIndex, baseTypeSizeOfTypeContainingInterface);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteLabels(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.JumpDestination != null && opCode.JumpDestination.Count > 0 && !opCode.JumpProcessed)
            {
                var previousOpCode = opCode.PreviousOpCode(this);
                var splitBlock = previousOpCode == null
                                 || (previousOpCode != null
                                     && (previousOpCode.OpCode.FlowControl == FlowControl.Next || previousOpCode.OpCode.FlowControl == FlowControl.Call));

                // opCode.Skip to fix issue with using it in 'conditional expresions'
                if (splitBlock)
                {
                    // we need to fix issue with blocks in llvm http://zanopia.wordpress.com/2010/09/14/understanding-llvm-assembly-with-fractals-part-i/
                    writer.WriteLine(string.Concat("br label %.a", opCode.AddressStart));
                }

                if (splitBlock || !opCode.JumpProcessed)
                {
                    this.WriteLabel(writer, string.Concat(".a", opCode.AddressStart));
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="asReference">
        /// </param>
        private void WriteLlvmArgVarAccess(LlvmIndentedTextWriter writer, int index, bool asReference = false)
        {
            this.Parameters[index].ParameterType.WriteTypePrefix(writer, false);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(this.GetArgVarName(index));

            // TODO: optional do we need to calculate it propertly?
            writer.Write(", align " + PointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="locals">
        /// </param>
        private void WriteLocalVariableDeclarations(IEnumerable<ILocalVariable> locals)
        {
            foreach (var local in locals)
            {
                this.Output.Write(string.Format("%local{0} = ", local.LocalIndex));
                if (local.LocalType.IsPinned)
                {
                    this.WriteAlloca(this.ResolveType("System.Void").ToPointerType());
                }
                else
                {
                    this.WriteAlloca(local.LocalType);
                }

                this.Output.WriteLine(string.Empty);
            }
        }

        private void WriteCallGctors()
        {
            var processed = new List<string>();

            // get all references
            foreach (var reference in this.AllReference.Reverse())
            {
                if (processed.Contains(reference))
                {
                    continue;
                }

                processed.Add(reference);

                this.Output.WriteLine("call void " + GetGlobalConstructorsFunctionName(reference) + "();");
            }
        }

        /// <summary>
        /// </summary>
        private void WriteMainFunction()
        {
            this.Output.Write("define i32 @main()");

            this.WriteMethodNumber();

            this.Output.WriteLine(" {");

            this.Output.Indent++;

            if (!this.Gctors)
            {
                this.WriteCallGctors();
            }

            if (!this.MainMethod.ReturnType.IsVoid())
            {
                this.Output.Write("%1 = call i32 ");
            }
            else
            {
                this.Output.Write("call void ");
            }

            var parameters = this.MainMethod.GetParameters();

            this.WriteMethodDefinitionName(this.Output, this.MainMethod);
            this.Output.Write("(");

            var index = 0;
            foreach (var parameter in parameters)
            {
                if (index > 0)
                {
                    this.Output.Write(", ");
                }

                this.Output.Write("%\"System.String\"** null");

                index++;
            }

            this.Output.WriteLine(");");

            if (!this.MainMethod.ReturnType.IsVoid())
            {
                this.Output.WriteLine("ret i32 %1");
            }
            else
            {
                this.Output.WriteLine("ret i32 0");
            }

            this.Output.Indent--;
            this.Output.WriteLine("}");
        }

        /// <summary>
        /// </summary>
        /// <param name="endPart">
        /// </param>
        private void WriteMethodBody()
        {
            var rest = this.PrepareWritingMethodBody();

            foreach (var opCodePart in rest)
            {
                this.ActualWrite(this.Output, opCodePart, true);
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// </summary>
        private void WriteMethodNumber()
        {
            // write number of method
            this.Output.Write(" #");
            this.Output.Write(methodNumberIncremental);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="result">
        /// </param>
        /// <param name="startOpCode">
        /// </param>
        /// <param name="endOpCode">
        /// </param>
        /// <param name="label">
        /// </param>
        /// <param name="stopAddress">
        /// </param>
        private void WritePhiNodeLabel(
            LlvmIndentedTextWriter writer, FullyDefinedReference result, OpCodePart startOpCode, OpCodePart endOpCode, string label = "a", int stopAddress = 0)
        {
            var customLabel = this.FindCustomLabel(startOpCode, endOpCode, stopAddress);
            if (customLabel != null)
            {
                writer.Write(" [ {0}, %.{1} ]", result, customLabel);
            }
            else
            {
                writer.Write(" [ {0}, %.{1} ]", result, label);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        private void WritePostMethodEnd(IMethod method)
        {
            if (!this.NoBody)
            {
                this.WriteExceptionEnvironment(method);

                this.Output.EndMethodBody();

                this.Output.Indent--;
                this.Output.WriteLine("}");
            }

            this.Output.WriteLine(string.Empty);

            this.WriteConstData();
        }

        /// <summary>
        /// </summary>
        private void WriteRequiredDeclarations()
        {
            if (this.typeRttiDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (var rttiDecl in this.typeRttiDeclRequired.Where(rttiDecl => !this.processedRttiTypes.Contains(rttiDecl)))
                {
                    rttiDecl.WriteRttiClassInfoExternalDeclaration(this.Output);
                    this.Output.WriteLine(string.Empty);
                }
            }

            if (this.typeRttiPointerDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (
                    var rttiPointerDecl in this.typeRttiPointerDeclRequired.Where(rttiPointerDecl => !this.processedRttiPointerTypes.Contains(rttiPointerDecl)))
                {
                    rttiPointerDecl.WriteRttiPointerClassInfoExternalDeclaration(this.Output);
                    this.Output.WriteLine(string.Empty);
                }
            }

            if (this.typeDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (var opaqueType in this.typeDeclRequired.Where(opaqueType => !this.processedTypes.Contains(opaqueType) && !opaqueType.IsArray))
                {
                    opaqueType.UseAsClass = true;
                    this.WriteTypeDeclarationStart(opaqueType);
                    opaqueType.UseAsClass = false;

                    this.Output.WriteLine("opaque");
                }
            }

            if (this.methodDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (var externalMethodDecl in this.methodDeclRequired.Where(externalMethodDecl => !this.processedMethods.Contains(externalMethodDecl)))
                {
                    this.Output.Write("declare ");

                    var ctor = externalMethodDecl as IConstructor;
                    if (ctor != null)
                    {
                        this.ReadMethodInfo(ctor, null);
                        this.Output.Write("void ");
                        this.WriteMethodDefinitionName(this.Output, ctor);
                        this.WriteMethodParamsDef(this.Output, ctor.GetParameters(), this.HasMethodThis, this.ThisType, this.ResolveType("System.Void"));
                        this.Output.WriteLine(string.Empty);
                        continue;
                    }

                    var method = externalMethodDecl;
                    if (method != null)
                    {
                        this.ReadMethodInfo(method, null);
                        this.WriteMethodReturnType(this.Output, method);
                        this.WriteMethodDefinitionName(this.Output, method);
                        this.WriteMethodParamsDef(this.Output, method.GetParameters(), this.HasMethodThis, this.ThisType, method.ReturnType);
                        this.Output.WriteLine(string.Empty);
                        continue;
                    }
                }
            }

            if (this.staticFieldExtrenalDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);

                foreach (var staticFieldExtrenalDecl in this.staticFieldExtrenalDeclRequired)
                {
                    this.WriteStaticFieldDeclaration(staticFieldExtrenalDecl, true);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="op">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        /// <param name="resultType">
        /// </param>
        /// <param name="options">
        /// </param>
        /// <param name="effectiveType">
        /// </param>
        private void WriteResultAndFirstOperandType(
            LlvmIndentedTextWriter writer, OpCodePart opCode, string op, IType requiredType, IType resultType, OperandOptions options, IType effectiveType)
        {
            if ((opCode.OpCode.StackBehaviourPush != StackBehaviour.Push0 || options.HasFlag(OperandOptions.GenerateResult)) && op != "store")
            {
                var resultOf = this.ResultOf(opCode);
                this.WriteSetResultNumber(opCode, resultType ?? (resultOf != null ? resultOf.Type : requiredType));
            }

            if (options.HasFlag(OperandOptions.Template))
            {
                var parts = op.Split('%');
                var index = 0;
                foreach (var part in parts)
                {
                    var text = index++ == 0 ? part : part.Substring(1);
                    var code = part.First();

                    switch (code)
                    {
                        case 'R':
                            if (effectiveType != null)
                            {
                                effectiveType.WriteTypePrefix(writer);
                            }

                            break;
                    }

                    writer.Write(text);
                }
            }
            else
            {
                writer.Write(op);
                writer.Write(' ');
            }

            if (!options.HasFlag(OperandOptions.IgnoreOperand))
            {
                var type = effectiveType ?? this.ResolveType("System.Void");
                type.WriteTypePrefix(writer);
                if (options.HasFlag(OperandOptions.AppendPointer))
                {
                    writer.Write('*');
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <param name="externalRef">
        /// </param>
        private void WriteStaticFieldDeclaration(IField field, bool externalRef = false)
        {
            var isExternal = externalRef;
            if (isExternal)
            {
                this.Output.Write("@\"{0}\" = external global ", field.GetFullName());
            }
            else
            {
                this.Output.Write("@\"{0}\" = {1} global ", field.GetFullName(), field.DeclaringType.IsGenericType ? "linkonce_odr" : string.Empty);
            }

            field.FieldType.WriteTypePrefix(this.Output, false);

            if (!isExternal)
            {
                if (field.FieldType.IsStructureType())
                {
                    this.Output.WriteLine(" zeroinitializer, align {0}", PointerSize);
                }
                else
                {
                    this.Output.WriteLine(" undef");
                }
            }
            else
            {
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteStaticFieldDeclarations(IType type)
        {
            if (!type.IsEnum)
            {
                foreach (var field in IlReader.Fields(type).Where(f => f.IsStatic && !f.IsConst))
                {
                    this.WriteStaticFieldDeclaration(field);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteTryBegins(LlvmIndentedTextWriter writer, OpCodePart opCode)
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
                writer.WriteLine("; Try, start of scope");
                this.tryScopes.Push(eh);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteTryEnds(LlvmIndentedTextWriter writer, OpCodePart opCode)
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

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteTypeDeclarationStart(IType type)
        {
            this.Output.Write("%");

            type.WriteTypeName(this.Output, false);

            this.Output.Write(" = type ");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteTypeDefinitionIfNotWrittenYet(IType type)
        {
            if (this.IsProcessed(type))
            {
                return;
            }

            Il2Converter.WriteTypeDefinition(this, type, null);
            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="pair">
        /// </param>
        private void WriteUnicodeString(KeyValuePair<int, string> pair)
        {
            this.Output.Write(
                "@.s{0} = private unnamed_addr constant {4} i32, [{2} x i16] {5} {4} i32 {3}, [{2} x i16] [",
                pair.Key,
                pair.Value,
                pair.Value.Length + 1,
                pair.Value.Length,
                '{',
                '}');

            var index = 0;
            foreach (var c in pair.Value.ToCharArray())
            {
                if (index > 0)
                {
                    this.Output.Write(", ");
                }

                this.Output.Write("i16 {0}", (int)c);
                index++;
            }

            if (index > 0)
            {
                this.Output.Write(", ");
            }

            this.Output.WriteLine("i16 0] {0}, align 2", '}');
        }

        /// <summary>
        /// </summary>
        [Flags]
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
            AdjustIntTypes = 1024,
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
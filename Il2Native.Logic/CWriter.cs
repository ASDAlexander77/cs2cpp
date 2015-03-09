// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LlvmWriter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

//#define INLINE_RTTI_INFO

namespace Il2Native.Logic
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.InteropServices;
    using System.Text;
    using CodeParts;
    using Exceptions;
    using Gencode;
    using Gencode.InlineMethods;
    using Gencode.SynthesizedMethods;
    using PEAssemblyReader;
    using Properties;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class CWriter : BaseWriter, ICodeWriter
    {
        /// <summary>
        /// </summary>
        public static int PointerSize = 4;

        public int ByValAlign = PointerSize;

        /// <summary>
        /// </summary>
        public Stack<CatchOfFinallyClause> catchScopes = new Stack<CatchOfFinallyClause>();

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
        public readonly ISet<IType> typeRttiDeclRequired = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        public readonly ISet<IType> typeRttiPointerDeclRequired = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        public readonly ISet<IType> typeVirtualTablesRequired = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<int, byte[]> bytesStorage = new SortedDictionary<int, byte[]>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> indexByFieldInfo = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<int, IMethod> methodsByToken = new SortedDictionary<int, IMethod>();

        /// <summary>
        /// </summary>
        private readonly ISet<IType> typeTokenRequired = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> poisitionByFieldInfo = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private readonly ISet<IType> postDeclarationsProcessedTypes = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        private readonly ISet<IType> processedRttiPointerTypes = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        private readonly ISet<IType> processedRttiTypes = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        private readonly ISet<IType> processedTypes = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        private readonly ISet<IType> processedVirtualTablesRequired = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        private int blockJumpAddressIncremental;

        /// <summary>
        /// </summary>
        private int bytesIndexIncremental;

        /// <summary>
        /// </summary>
        private int resultNumberIncremental;

        /// <summary>
        /// </summary>
        private CIndentedTextWriter savedOutput;

        /// <summary>
        /// </summary>
        private readonly StringBuilder storedText = new StringBuilder();

        /// <summary>
        /// </summary>
        private int stringIndexIncremental;

        /// <summary>
        /// </summary>
        private readonly ISet<IType> forwardTypeDeclarationWritten = new NamespaceContainer<IType>();

        /// <summary>
        /// </summary>
        private readonly ISet<MethodKey> forwardMethodDeclarationWritten = new NamespaceContainer<MethodKey>();

        /// <summary>
        /// </summary>
        private readonly ISet<IField> forwardStaticDeclarationWritten = new NamespaceContainer<IField>();

        /// <summary>
        /// </summary>
        private string outputFile;

        /// <summary>
        /// append declaration
        /// </summary>
        private string declarationPrefix = "extern \"C\" ";

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
            AdjustIntTypes = 1024
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <param name="args">
        /// </param>
        public CWriter(string fileName, string sourceFilePath, string pdbFilePath, string[] args)
        {
            this.SetSettings(fileName, sourceFilePath, pdbFilePath, args);
        }

        /// <summary>
        /// </summary>
        public IEnumerable<string> AllReferences
        {
            get
            {
                return IlReader.AllReferences();
            }
        }

        /// <summary>
        /// </summary>
        public string DataLayout { get; private set; }

        /// <summary>
        /// </summary>
        public bool Gc { get; private set; }

        /// <summary>
        /// </summary>
        public bool Gctors { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsCoreLib
        {
            get
            {
                return IlReader.IsCoreLib;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsLlvm34OrLower { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsLlvm35 { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsLlvm36 { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsLlvm37 { get; private set; }

        /// <summary>
        /// </summary>
        public bool Stubs { get; private set; }

        public IDictionary<int, IMethod> MethodsByToken
        {
            get { return this.methodsByToken; }
        }

        public ISet<IType> TypeTokenRequired
        {
            get { return this.typeTokenRequired; }
        }

        /// <summary>
        /// </summary>
        public CIndentedTextWriter Output { get; private set; }

        /// <summary>
        /// </summary>
        public string Target { get; private set; }

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
                this.Output = new CIndentedTextWriter(new StringWriter(this.storedText));
            }
            else
            {
                this.Output.Close();
                this.Output = this.savedOutput;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsTypeDefinitionWritten(IType type)
        {
            return this.processedTypes.Contains(type);
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
            this.Output.Indent--;
            this.Output.WriteLine("};");
        }

        /// <summary>
        /// </summary>
        public void WriteAfterMethods()
        {
        }

        /// <summary>
        /// </summary>
        public void WriteBeforeConstructors()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        public void WriteBeforeFields(int count)
        {
            var baseType = ThisType.BaseType;

            this.Output.WriteLine("{");
            this.Output.Indent++;

            // put virtual root table if type has no any base with virtual types
            if (ThisType.IsRootInterface() || ThisType.IsRootOfVirtualTable(this))
            {
                this.Output.WriteLine("anyFn* vtable;");
            }

            if (baseType != null)
            {
                baseType.WriteTypeWithoutModifiers(this);
                this.Output.WriteLine(" base;");
            }

            var index = 0;
            foreach (var @interface in ThisType.GetInterfacesExcludingBaseAllInterfaces())
            {
                @interface.WriteTypeWithoutModifiers(this);
                this.Output.Write(" ");
                @interface.WriteTypeName(this.Output, false);
                this.Output.WriteLine(";");
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
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteConstructorEnd(IConstructor ctor, IGenericContext genericContext)
        {
            this.WriteMethodEnd(ctor, genericContext);
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteConstructorStart(IConstructor ctor, IGenericContext genericContext)
        {
            this.WriteMethodStart(ctor, genericContext);
        }

        // TODO: if DynamicCast does not work for an type then something wrong with this value, (it should return value equals to the number of inheritance route * pointer size, 
        // if type can't be found in inheritance route it should return -2, if casts from type which inheritce more then one time it should return -3
        // for inheritance root for objects equals 0 (as it is all the time first)

        /// <summary>
        /// </summary>
        public void WriteEnd()
        {
            if (MainMethod != null)
            {
                this.WriteMainFunction();
            }

            this.WriteGlobalConstructors();

            this.WriteRequiredDeclarations();

            this.Output.Close();
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

            fieldType.WriteTypePrefix(this, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteMethodEnd(IMethod method, IGenericContext genericContext)
        {
            this.WriteMethodBeginning(method, genericContext);
            this.WriteMethodBody(method);
            this.WritePostMethodEnd(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteMethodStart(IMethod method, IGenericContext genericContext, bool linkOnceOdr = false, bool noLocalVars = false)
        {
            this.StartProcess();

            Debug.Assert(!method.IsGenericMethodDefinition);
            Debug.Assert(genericContext == null || !method.DeclaringType.IsGenericTypeDefinition);

            if (method.Token.HasValue)
            {
                this.methodsByToken[method.Token.Value] = method;
            }

            ReadMethodInfo(method, genericContext);

            var isMain = method.IsStatic && method.CallingConvention.HasFlag(CallingConventions.Standard) &&
                         method.Name.Equals("Main");

            // check if main
            if (isMain)
            {
                MainMethod = method;
            }

            if (method.IsAbstract
                || method.IsSkipped())
            {
                return;
            }

            // to gather all info about method which we need
            IlReader.UsedStrings = new SortedDictionary<int, string>();
            IlReader.CalledMethods = new NamespaceContainer<IMethod>();
            IlReader.StaticFields = new NamespaceContainer<IField>();
        }

        private void WriteMethodBeginning(IMethod method, IGenericContext genericContext)
        {
            if (method.IsAbstract
                || method.IsSkipped())
            {
                return;
            }

            this.forwardMethodDeclarationWritten.Add(new MethodKey(method, null));
            WriteMethodRequiredDeclatations();

            // after WriteMethodRequiredDeclatations which removed info about current method we need to reread info about method
            ReadMethodInfo(method, genericContext);

            // extern "c"
            this.Output.Write(this.declarationPrefix);

            var isDelegateBodyFunctions = method.IsDelegateFunctionBody();
            if ((method.IsAbstract || (NoBody && !this.Stubs)) && !isDelegateBodyFunctions)
            {
                if (!method.IsUnmanagedMethodReference)
                {
                    if (this.methodsHaveDefinition.Contains(method))
                    {
                        return;
                    }
                }
                else
                {
                    this.WriteMethodDefinitionName(this.Output, method);
                    Debug.Assert(false, "Investigate");
                    this.Output.Write(" = ");
                }

                if (method.IsUnmanagedDllImport)
                {
                    this.Output.Write("__declspec( dllimport ) ");
                }
                else if (method.IsUnmanagedMethodReference)
                {
                    this.Output.Write("extern ");
                }

                if (method.IsUnmanagedMethodReference)
                {
                    this.Output.Write("global ");
                }

                if (!method.IsUnmanagedMethodReference && method.DllImportData != null &&
                    method.DllImportData.CallingConvention == CallingConvention.StdCall)
                {
                    this.Output.Write("__stdcall ");
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
                    this.Output,
                    method,
                    this.HasMethodThis,
                    ThisType,
                    method.ReturnType,
                    method.IsUnmanagedMethodReference,
                    method.CallingConvention.HasFlag(CallingConventions.VarArgs));
            }

            if (method.IsUnmanagedMethodReference)
            {
                this.Output.Write("*");
            }

            // write local declarations
            var methodBodyBytes = method.ResolveMethodBody(genericContext);
            if (methodBodyBytes.HasBody)
            {
                this.Output.WriteLine(" {");
                this.Output.Indent++;

                this.WriteLocalVariableDeclarations(methodBodyBytes.LocalVariables);

                this.Output.StartMethodBody();
            }
            else
            {
                if (isDelegateBodyFunctions)
                {
                    this.WriteDelegateFunctionBody(method);
                    this.Output.WriteLine(string.Empty);
                }
                else
                {
                    this.Output.WriteLine(";");
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void WritePostDeclarationsAndInternalDefinitions(IType type, bool staticOnly = false)
        {
            if (!(type.IsGenericType || type.IsArray) && this.AssemblyQualifiedName != type.AssemblyQualifiedName)
            {
                return;
            }

            if (!this.postDeclarationsProcessedTypes.Add(type))
            {
                return;
            }

            this.WriteStaticFieldDeclarations(type);

            if (staticOnly)
            {
                return;
            }

            this.WriteVirtualTables(type);

            this.processedVirtualTablesRequired.Add(type);

            this.Output.WriteLine(string.Empty);

            type.WriteRtti(this);

            this.processedRttiTypes.Add(type);
            this.processedRttiPointerTypes.Add(type);

            this.Output.WriteLine(string.Empty);

            // object oriented methods
            if (!type.IsVoid())
            {
                type.WriteNewObjectMethod(this);
            }

            type.WriteInitObjectMethod(this);

            var normalType = type.ToNormal();

            var isEnum = normalType.IsEnum;
            var canBeBoxed = normalType.IsPrimitiveType() || normalType.IsStructureType() || isEnum;
            var canBeUnboxed = canBeBoxed;
            var excluded = normalType.FullName == "System.Enum";

            if (canBeBoxed && !excluded)
            {
                normalType.WriteBoxMethod(this);
            }

            if (canBeUnboxed && !excluded)
            {
                normalType.WriteUnboxMethod(this);
            }

            normalType.WriteInternalGetTypeMethod(this);
            normalType.WriteInternalGetSizeMethod(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="moduleName">
        /// </param>
        /// <param name="assemblyName">
        /// </param>
        /// <param name="isCoreLib">
        /// </param>
        /// <param name="allReference">
        /// </param>
        public void WriteStart(IIlReader ilReader)
        {
            this.resultNumberIncremental = 0;

            this.Output = new CIndentedTextWriter(new StreamWriter(this.outputFile));

            this.IlReader = ilReader;

            // declarations
            this.Output.WriteLine(Resources.c_declarations);
            this.Output.WriteLine(string.Empty);

            if (this.Gc)
            {
                this.Output.WriteLine(Resources.gc_declarations);
                this.Output.WriteLine(string.Empty);
            }

            StaticConstructors.Clear();
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

            this.WriteTypeRequiredDefinitions(type);

            ReadTypeInfo(type);

            this.WriteTypeDeclarationStart(type);
        }

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
            if (firstLevel)
            {
                this.WriteLabels(writer, opCode);
            }

            if (opCode.UsedByAlternativeValues != null)
            {
                this.WriteStartOfPhiValues(writer, opCode);
            }

            if (opCode.Result != null)
            {
                return;
            }

            if (firstLevel && !opCode.Any(Code.Newobj, Code.Dup) && opCode.UsedBy != null && opCode.UsedByAlternativeValues == null)
            {
                return;
            }

            this.WriteTryBegins(writer, opCode);
            this.WriteCatchBegins(writer, opCode);

            this.ActualWriteOpCode(writer, opCode);

            opCode.ResultOpCode = opCode.Result;

            this.AdjustResultTypeToOutgoingType(opCode);

            this.WriteCatchFinnallyEnd(writer, opCode);

            this.WriteCatchFinnallyCleanUpEnd(opCode);
            this.WriteTryEnds(writer, opCode);
            this.WriteExceptionHandlersProlog(writer, opCode);

            opCode.ResultAtExit = opCode.Result;

            if (opCode.UsedByAlternativeValues != null)
            {
                this.WriteEndOfPhiValues(writer, opCode);
            }

            if (firstLevel)
            {
                this.Output.WriteLine(";");
            }
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
                    opCode.Result = opCode.UseAsNull
                        ? new ConstValue(null, this.System.System_Void.ToPointerType())
                        : new ConstValue(0, this.System.System_Int32);
                    break;
                case Code.Ldc_I4_1:
                    opCode.Result = new ConstValue(1, this.System.System_Int32);
                    break;
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                    var asString = code.ToString();
                    opCode.Result = new ConstValue(
                        int.Parse(asString.Substring(asString.Length - 1, 1)),
                       this.System.System_Int32);
                    break;
                case Code.Ldc_I4_M1:
                    opCode.Result = new ConstValue(-1, this.System.System_Int32);
                    break;
                case Code.Ldc_I4:
                    var opCodeInt32 = opCode as OpCodeInt32Part;
                    opCode.Result = new ConstValue(opCodeInt32.Operand, this.System.System_Int32);
                    break;
                case Code.Ldc_I4_S:
                    opCodeInt32 = opCode as OpCodeInt32Part;
                    opCode.Result = new ConstValue(
                        opCodeInt32.Operand > 127 ? -(256 - opCodeInt32.Operand) : opCodeInt32.Operand,
                       this.System.System_Int32);
                    break;
                case Code.Ldc_I8:
                    var opCodeInt64 = opCode as OpCodeInt64Part;
                    opCode.Result = new ConstValue(opCodeInt64.Operand, this.System.System_Int64);
                    break;
                case Code.Ldc_R4:
                    var opCodeSingle = opCode as OpCodeSinglePart;

                    if (float.IsPositiveInfinity(opCodeSingle.Operand))
                    {
                        opCode.Result = new ConstValue("0x7FF0000000000000", this.System.System_Single);
                    }
                    else if (float.IsNegativeInfinity(opCodeSingle.Operand))
                    {
                        opCode.Result = new ConstValue("0xFFF0000000000000", this.System.System_Single);
                    }
                    else
                    {
                        var g = BitConverter.DoubleToInt64Bits(opCodeSingle.Operand);
                        opCode.Result = new ConstValue(
                            string.Format("0x{0}", g.ToString("X")),
                           this.System.System_Single);
                    }

                    break;
                case Code.Ldc_R8:
                    var opCodeDouble = opCode as OpCodeDoublePart;
                    if (double.IsPositiveInfinity(opCodeDouble.Operand))
                    {
                        opCode.Result = new ConstValue("0x7FF0000000000000", this.System.System_Double);
                    }
                    else if (double.IsNegativeInfinity(opCodeDouble.Operand))
                    {
                        opCode.Result = new ConstValue("0xFFF0000000000000", this.System.System_Double);
                    }
                    else
                    {
                        var g = BitConverter.DoubleToInt64Bits(opCodeDouble.Operand);
                        opCode.Result = new ConstValue(
                            string.Format("0x{0}", g.ToString("X")),
                           this.System.System_Double);
                    }

                    break;
                case Code.Ldstr:
                    var opCodeString = opCode as OpCodeStringPart;
                    var stringType = this.System.System_String;
                    var stringToken = opCodeString.Operand.Key;
                    var strType = this.WriteToString(() => { stringType.WriteTypePrefix(this); });
                    opCode.Result =
                        new FullyDefinedReference(
                            string.Format("({1}) &_s{0}_", stringToken, strType),
                            stringType);
                    break;
                case Code.Ldnull:
                    opCode.Result = new ConstValue(null, this.System.System_Void.ToPointerType());
                    break;

                case Code.Arglist:

                    // TODO: it really does not do anything. you need to use VA_START, VA_END, VA_ARG in ArgInterator class
                    opCode.Result = new ConstValue("undef", this.System.System_Object);
                    break;

                case Code.Ldtoken:

                    // TODO: finish loading Token  
                    //var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    ////var data = opCodeFieldInfoPart.Operand.GetFieldRVAData();
                    opCode.Result = new ConstValue("undef", this.System.System_Object);

                    var opCodeTypePart = opCode as OpCodeTypePart;
                    if (opCodeTypePart != null)
                    {
                        this.typeTokenRequired.Add(opCodeTypePart.Operand);
                    }

                    break;
                case Code.Localloc:

                    writer.Write("alloca i8, ");
                    this.UnaryOper(
                        writer,
                        opCode,
                        "alloca i8, ",
                        this.GetIntTypeByByteSize(PointerSize));
                    writer.Write(", align 1");

                    writer.WriteLine(string.Empty);

                    this.WriteMemSet(opCode.Result, firstOpCodeOperand.Result, 1);

                    break;
                case Code.Ldfld:

                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    // we wait when opCode.DestinationName is set;
                    this.WriteFieldAccess(opCodeFieldInfoPart);

                    if (!opCodeFieldInfoPart.Operand.FieldType.IsStructureType())
                    {
                        var memberAccessResultNumber = opCode.Result;
                        opCode.Result = null;
                        this.WriteLlvmLoad(opCode, memberAccessResultNumber.Type, memberAccessResultNumber);
                    }

                    break;
                case Code.Ldflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    if (!opCodeFieldInfoPart.Operand.HasFixedElementField)
                    {
                        this.WriteFieldAccess(opCodeFieldInfoPart);
                        var fieldLoadResult = opCodeFieldInfoPart.Result;

                        // convert return type of the field to pointer of a field type (unless it is fixed data field)
                        opCodeFieldInfoPart.Result = !opCodeFieldInfoPart.Operand.IsFixed ? fieldLoadResult.ToPointerType() : fieldLoadResult;
                    }
                    else
                    {
                        opCodeFieldInfoPart.Result = opCodeFieldInfoPart.OpCodeOperands[0].Result.ToType(opCodeFieldInfoPart.Operand.FieldType.ToPointerType());
                    }

                    break;
                case Code.Ldsfld:

                    IType castFrom;
                    IType intAdjustment;
                    bool intAdjustSecondOperand;
                    var operandType = this.DetectTypePrefix(
                        opCode,
                        null,
                        OperandOptions.TypeIsInOperator,
                        out castFrom,
                        out intAdjustment,
                        out intAdjustSecondOperand);
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    var destinationName = opCodeFieldInfoPart.Operand.GetFullName().CleanUpName();
                    var reference = new FullyDefinedReference(destinationName, opCodeFieldInfoPart.Operand.FieldType);
                    this.WriteLlvmLoad(opCode, operandType, reference);

                    break;
                case Code.Ldsflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    opCodeFieldInfoPart.Result = new FullyDefinedReference(
                        "&" + opCodeFieldInfoPart.Operand.GetFullName().CleanUpName(),
                        opCodeFieldInfoPart.Operand.FieldType.ToPointerType());

                    break;
                case Code.Stfld:

                    this.FieldAccessAndSaveToField(opCode as OpCodeFieldInfoPart);

                    break;
                case Code.Stsfld:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    destinationName = opCodeFieldInfoPart.Operand.GetFullName().CleanUpName();
                    operandType = opCodeFieldInfoPart.Operand.FieldType;
                    reference = new FullyDefinedReference(destinationName, operandType);

                    this.WriteLlvmSave(opCode, operandType, 0, reference);

                    break;

                case Code.Ldobj:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    var resultOfOp0 = firstOpCodeOperand.Result;
                    var loadValueFromAddress = !opCodeTypePart.Operand.IsStructureType();
                    if (loadValueFromAddress)
                    {
                        this.WriteLlvmLoad(opCode, opCodeTypePart.Operand, resultOfOp0);
                    }
                    else
                    {
                        if (resultOfOp0.Type.IntTypeBitSize() == PointerSize * 8)
                        {
                            // using int as intptr
                            this.AdjustIntConvertableTypes(
                                writer,
                                firstOpCodeOperand,
                                opCodeTypePart.Operand.ToPointerType());
                            opCode.Result = firstOpCodeOperand.Result;
                        }
                        else
                        {
                            // should be address, so Pointer or IsByRef is accepted
                            Debug.Assert(resultOfOp0.Type.IsPointer || resultOfOp0.Type.IsByRef || resultOfOp0.Type.UseAsClass);
                            opCode.Result = resultOfOp0.ToType(resultOfOp0.Type.UseAsClass ? resultOfOp0.Type : resultOfOp0.Type.GetElementType());
                            if (opCode.Result.Type.IsVoid())
                            {
                                // in case we receive void* to load struct
                                opCode.Result = resultOfOp0;
                            }

                            Debug.Assert(!opCode.Result.Type.IsVoid());
                        }
                    }

                    break;

                case Code.Stobj:
                    this.SaveObject(opCode, 1, 0);
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

                    if (methodBase.DeclaringType.IsStructureType() && methodBase.IsConstructor)
                    {
                        this.ActualWrite(writer, opCodeMethodInfoPart.OpCodeOperands[0]);

                        // if we call constructor on struct we need to initialize object before
                        opCodeMethodInfoPart.Result = opCodeMethodInfoPart.OpCodeOperands[0].Result;
                        methodBase.DeclaringType.WriteCallInitObjectMethod(this, opCodeMethodInfoPart);
                        opCodeMethodInfoPart.Result = null;
                        // TODO: maybe here we need use ','?
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
                    this.BinaryOper(
                        writer,
                        opCode,
                        " + ",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Add_Ovf:
                    this.WriteOverflowWithThrow(writer, opCode, "sadd");
                    break;
                case Code.Add_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "uadd");
                    break;
                case Code.Mul:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " * ",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Mul_Ovf:
                    this.WriteOverflowWithThrow(writer, opCode, "smul");
                    break;
                case Code.Mul_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "umul");
                    break;
                case Code.Sub:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " - ",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Sub_Ovf:
                    this.WriteOverflowWithThrow(writer, opCode, "ssub");
                    break;
                case Code.Sub_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "usub");
                    break;
                case Code.Div:
                case Code.Div_Un:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " / ",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    break;
                case Code.Rem:
                case Code.Rem_Un:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " % ",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
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
                    this.BinaryOper(writer, opCode, " >> ", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Shr:
                case Code.Shr_Un:
                    this.BinaryOper(writer, opCode, " << ", OperandOptions.AdjustIntTypes);
                    break;
                case Code.Not:
                    this.UnaryOper(writer, opCode, "~");
                    break;
                case Code.Neg:
                    this.UnaryOper(writer, opCode, "-");
                    break;

                case Code.Dup:

                    this.ActualWrite(writer, firstOpCodeOperand);

                    opCode.Result = firstOpCodeOperand.UsedByAlternativeValues == null
                                        ? firstOpCodeOperand.ResultOpCode
                                        : firstOpCodeOperand.Result;

                    var dupVar = string.Format("_dup{0}", opCode.AddressStart);

                    opCode.Result.Type.WriteTypePrefix(this);
                    this.Output.WriteLine(string.Concat(" ", dupVar, ";"));

                    this.Output.Write(string.Concat(dupVar, " = "));

                    this.WriteResultOrActualWrite(writer, opCode);

                    opCode.Result = new FullyDefinedReference(dupVar, opCode.Result.Type);
                    firstOpCodeOperand.Result = opCode.Result;

                    break;

                case Code.Box:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    var type = opCodeTypePart.Operand;
                    if (type.IsValueType())
                    {
                        type.WriteCallBoxObjectMethod(this, opCode);
                    }
                    else if (type.IsPointer)
                    {
                        this.System.System_Int32.WriteCallBoxObjectMethod(this, opCode);
                    }
                    else
                    {
                        var resultOfOper0 = opCodeTypePart.OpCodeOperands[0].Result;
                        opCodeTypePart.Result = resultOfOper0.Type.IsStructureType() || resultOfOper0.Type.IsEnum ? resultOfOper0.ToClassType() : resultOfOper0;
                    }

                    break;

                case Code.Unbox:
                case Code.Unbox_Any:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    type = opCodeTypePart.Operand;
                    if (type.IsValueType() || type.IsStructureType())
                    {
                        type.WriteCallUnboxObjectMethod(this, opCode);
                    }
                    else if (type.IsPointer)
                    {
                        this.System.System_Int32.WriteCallUnboxObjectMethod(this, opCode);
                    }
                    else
                    {
                        this.WriteCast(
                            opCodeTypePart,
                            opCodeTypePart.OpCodeOperands[0].Result,
                            opCodeTypePart.Operand,
                            true);
                    }

                    break;
                case Code.Ret:
                    this.WriteReturn(writer, opCode, MethodReturnType);
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

                    var localType = LocalInfo[index].LocalType;

                    var destination = new FullyDefinedReference(this.GetLocalVarName(index), localType);
                    this.WriteLlvmSave(opCode, localType, 0, destination);

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
                    localType = LocalInfo[index].LocalType;
                    var definedReference = new FullyDefinedReference(destinationName, localType);
                    if (!localType.IsStructureType() || localType.IsByRef)
                    {
                        this.WriteLlvmLoad(opCode, localType, definedReference);
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
                    opCode.Result = new FullyDefinedReference(
                        "&" + this.GetLocalVarName(index),
                        LocalInfo[index].LocalType.ToPointerType());

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
                        var thisTypeAsClass = ThisType.ToClass();
                        this.WriteLlvmLoad(
                            opCode,
                            thisTypeAsClass,
                            new FullyDefinedReference(this.GetThisName(), thisTypeAsClass),
                            true,
                            true);
                    }
                    else
                    {
                        var parameterIndex = index - (HasMethodThis ? 1 : 0);
                        var parameter = Parameters[parameterIndex];

                        destinationName = this.GetArgVarName(parameter, index);
                        var fullyDefinedReference = new FullyDefinedReference(destinationName, parameter.ParameterType);
                        if (!parameter.ParameterType.IsStructureType())
                        {
                            this.WriteLlvmLoad(opCode, parameter.ParameterType, fullyDefinedReference);
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
                        writer.Write(this.GetThisName());
                        opCode.Result = new FullyDefinedReference(this.GetThisName(), ThisType);
                    }
                    else
                    {
                        var parameterIndex = index - (HasMethodThis ? 1 : 0);
                        var parameter = Parameters[parameterIndex];
                        opCode.Result = new FullyDefinedReference(
                            this.GetArgVarName(parameter, index),
                            parameter.ParameterType.ToPointerType());
                    }

                    break;

                case Code.Starg:
                case Code.Starg_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;
                    var actualIndex = index - (HasMethodThis ? 1 : 0);

                    var argType = this.GetArgType(index);
                    destination = new FullyDefinedReference(
                        this.GetArgVarName(actualIndex, index),
                        this.GetArgType(index));
                    if (argType.IsStructureType() && !argType.IsByRef)
                    {
                        if (firstOpCodeOperand.Result.Type.IsPrimitiveType())
                        {
                            this.WriteLlvmSavePrimitiveIntoStructure(opCode, firstOpCodeOperand.Result, destination);
                        }
                        else
                        {
                            opCode.Result = destination;
                            this.WriteLlvmLoad(opCode, argType, firstOpCodeOperand.Result);
                        }
                    }
                    else
                    {
                        this.WriteLlvmSave(opCode, argType, 0, destination);
                    }

                    break;

                case Code.Ldftn:

                    opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;

                    var intPtrType = this.System.System_IntPtr;
                    var voidPtrType = this.System.System_Void.ToPointerType();
                    var convertString = this.WriteToString(
                        () =>
                        {
                            this.Output.Write("bitcast (");
                            this.WriteMethodPointerType(this.Output, opCodeMethodInfoPart.Operand);
                            this.Output.Write(" ");
                            this.Output.Write(opCodeMethodInfoPart.Operand.GetFullMethodName());
                            this.Output.Write(" to i8*)");
                        });
                    var value = new FullyDefinedReference(convertString, this.System.System_Byte.ToPointerType());

                    this.WriteNewWithCallingConstructor(opCode, intPtrType, voidPtrType, value);

                    break;

                case Code.Ldvirtftn:

                    opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;

                    var methodInfo = opCodeMethodInfoPart.Operand;

                    IType thisType;
                    bool hasThisArgument;
                    OpCodePart opCodeFirstOperand;
                    ReturnResult resultOfFirstOperand;
                    bool isIndirectMethodCall;
                    IType ownerOfExplicitInterface;
                    IType requiredType;
                    methodInfo.WriteFunctionCallProlog(
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

                    FullyDefinedReference methodAddressResultNumber = null;
                    if (isIndirectMethodCall)
                    {
                        methodAddressResultNumber = this.GenerateVirtualCall(
                            opCodeMethodInfoPart,
                            methodInfo,
                            thisType,
                            opCodeFirstOperand,
                            resultOfFirstOperand,
                            ref requiredType);
                    }

                    // bitcast method function address to Byte*
                    this.SetResultNumber(opCode, this.System.System_Byte.ToPointerType());
                    writer.Write("bitcast ");
                    this.WriteMethodPointerType(writer, methodInfo, thisType);
                    writer.Write(" ");
                    if (isIndirectMethodCall)
                    {
                        this.WriteResult(methodAddressResultNumber);
                    }
                    else
                    {
                        this.WriteMethodDefinitionName(writer, methodInfo);
                    }

                    writer.Write(" to i8*");
                    writer.WriteLine(string.Empty);

                    methodAddressResultNumber = opCode.Result;
                    opCode.Result = null;

                    intPtrType = this.System.System_IntPtr;
                    voidPtrType = this.System.System_Void.ToPointerType();
                    this.WriteNewWithCallingConstructor(opCode, intPtrType, voidPtrType, methodAddressResultNumber);

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
                            break;
                        case Code.Blt:
                        case Code.Blt_S:
                            oper = " < ";
                            break;
                        case Code.Blt_Un:
                        case Code.Blt_Un_S:
                            oper = " < ";
                            break;
                        case Code.Ble:
                        case Code.Ble_S:
                            oper = " <= ";
                            break;
                        case Code.Ble_Un:
                        case Code.Ble_Un_S:
                            oper = " <= ";
                            break;
                        case Code.Bgt:
                        case Code.Bgt_S:
                            oper = " > ";
                            break;
                        case Code.Bgt_Un:
                        case Code.Bgt_Un_S:
                            oper = " > ";
                            break;
                        case Code.Bge:
                        case Code.Bge_S:
                            oper = " >= ";
                            break;
                        case Code.Bge_Un:
                        case Code.Bge_Un_S:
                            oper = " >= ";
                            break;
                    }

                    this.BinaryOper(
                        writer,
                        opCode,
                        oper,
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                       this.System.System_Boolean);

                    break;
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:

                    oper = opCode.Any(Code.Brtrue, Code.Brtrue_S) ? "" : "!";
                    var resultOf = EstimatedResultOf(firstOpCodeOperand);

                    var opts = OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer;
                    this.UnaryOper(
                        writer,
                        opCode,
                        "if (" + oper,
                       this.System.System_Boolean);

                    writer.Write(string.Concat(") goto a", opCode.JumpAddress()));

                    break;
                case Code.Br:
                case Code.Br_S:

                    writer.Write(string.Concat("goto a", opCode.JumpAddress()));
                    break;
                case Code.Leave:
                case Code.Leave_S:

                    this.WriteLeave(writer, opCode);

                    break;
                case Code.Ceq:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " == ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                       this.System.System_Boolean);

                    break;
                case Code.Clt:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " < ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                       this.System.System_Boolean);

                    break;
                case Code.Clt_Un:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " < ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                       this.System.System_Boolean);

                    break;
                case Code.Cgt:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " > ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                       this.System.System_Boolean);

                    break;
                case Code.Cgt_Un:
                    this.BinaryOper(
                        writer,
                        opCode,
                        " > ",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                       this.System.System_Boolean);
                    break;

                case Code.Conv_R4:
                case Code.Conv_R_Un:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_Single);
                    break;

                case Code.Conv_R8:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_Double);
                    break;

                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:

                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_SByte);
                    break;

                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_Byte);
                    break;

                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_Int16);
                    break;

                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_UInt16);
                    break;

                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:

                    this.WriteConvertToNativeInt(writer, opCode);
                    break;

                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_Int32);
                    break;

                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                    var intPtrOper = this.IntTypeRequired(opCode);
                    var nativeIntType = intPtrOper
                        ? this.System.System_Int32
                        : this.System.System_Void.ToPointerType();
                    this.ConvertCCast(
                        opCode,
                        0,
                        nativeIntType);
                    break;

                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_UInt32);
                    break;

                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_Int64);
                    break;

                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    this.ConvertCCast(
                        opCode,
                        0,
                       this.System.System_UInt64);
                    break;

                case Code.Castclass:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteCast(
                        opCodeTypePart,
                        opCodeTypePart.OpCodeOperands[0].Result,
                        opCodeTypePart.Operand,
                        true);
                    break;

                case Code.Isinst:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    var fromType = opCodeTypePart.OpCodeOperands[0].Result;
                    var toType = opCodeTypePart.Operand;

                    var dynamicCastRequired = false;
                    var castRequired = toType.IsClassCastRequired(
                        this,
                        opCodeTypePart.OpCodeOperands[0],
                        out dynamicCastRequired);
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

                    // to support settings exceptions
                    if (opCode.ReadExceptionFromStack)
                    {
                        opCode.Result = this.catchScopes.First().ExceptionResult;
                        break;
                    }

                    var opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;
                    if (opCodeConstructorInfoPart != null && !opCodeConstructorInfoPart.Operand.DeclaringType.IsString)
                    {
                        this.WriteNewObject(opCodeConstructorInfoPart);
                    }
                    else
                    {
                        // special string case
                        var stringCtorMethodBase = StringGen.GetCtorMethodByParameters(
                            System.System_String, opCodeConstructorInfoPart.Operand.GetParameters(), this);
                        var hasThis = stringCtorMethodBase.CallingConvention.HasFlag(CallingConventions.HasThis);

                        OpCodePart opCodeNope = opCodeConstructorInfoPart;
                        if (hasThis)
                        {
                            // insert 'This' as null
                            opCodeNope = OpCodePart.CreateNop;
                            var operands = new List<OpCodePart>(opCodeConstructorInfoPart.OpCodeOperands.Length);
                            operands.AddRange(opCodeConstructorInfoPart.OpCodeOperands);

                            var opCodeThis = OpCodePart.CreateNop;
                            opCodeThis.Result = new ConstValue(null, System.System_String);
                            operands.Insert(0, opCodeThis);

                            opCodeNope.OpCodeOperands = operands.ToArray();
                        }

                        this.WriteCall(
                            opCodeNope,
                            stringCtorMethodBase,
                            false,
                            hasThis,
                            false,
                            null,
                            this.tryScopes.Count > 0 ? this.tryScopes.Peek() : null);

                        if (hasThis)
                        {
                            opCodeConstructorInfoPart.Result = opCodeNope.Result;
                        }
                    }

                    break;

                case Code.Newarr:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    WriteNewWithCallingConstructor(
                        opCodeTypePart,
                        opCodeTypePart.Operand.ToArrayType(1),
                       this.System.System_Int32,
                        opCodeTypePart.OpCodeOperands[0].Result);

                    break;

                case Code.Initobj:

                    ActualWrite(writer, opCode.OpCodeOperands[0]);

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
                    break;

                case Code.Endfinally:
                    this.WriteEndFinally(writer, opCode);
                    break;

                case Code.Pop:
                    break;

                case Code.Constrained:

                    // to solve the problem with referencing ValueType and Class type in Generic type
                    opCodeTypePart = opCode as OpCodeTypePart;
                    var nextOp = opCode.Next;

                    // if this is Struct we already have an address in LLVM
                    if (!opCodeTypePart.Operand.IsStructureType())
                    {
                        var fullyDefinedReference = nextOp.OpCodeOperands[0].Result;
                        nextOp.OpCodeOperands[0].Result = null;
                        this.WriteLlvmLoad(nextOp.OpCodeOperands[0], opCodeTypePart.Operand, fullyDefinedReference);
                    }

                    var firstOperandResult = nextOp.OpCodeOperands[0].Result;
                    var isPrimitive = firstOperandResult.Type.IsPrimitiveTypeOrEnum();

                    if (isPrimitive)
                    {
                        // box it
                        writer.WriteLine("; Constrained: Box Primitive type for 'This' parameter");

                        // convert value to object
                        var opCodeMethodInfo = opCode.Next as OpCodeMethodInfoPart;
                        opCodeMethodInfo.Result = null;
                        var opCodeNone = OpCodePart.CreateNop;
                        opCodeNone.OpCodeOperands = new[] { opCodeMethodInfo.OpCodeOperands[0] };
                        firstOperandResult.Type.ToClass().WriteCallBoxObjectMethod(this, opCodeNone);
                        nextOp.OpCodeOperands[0].Result = opCodeNone.Result;
                        writer.WriteLine(string.Empty);
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
                        switchValueType.WriteTypePrefix(this);
                        writer.Write(" {0}, label %.a{1} ", index, opCodeLabels.JumpAddress(index++));
                    }

                    writer.WriteLine("]");

                    writer.WriteLine(string.Empty);

                    writer.Indent--;
                    writer.WriteLine(string.Concat(".a", opCode.GroupAddressEnd, ':'));
                    writer.Indent++;

                    opCode.Next.JumpProcessed = true;

                    break;

                case Code.Sizeof:
                    opCodeTypePart = opCode as OpCodeTypePart;
                    opCode.Result = new ConstValue(
                        opCodeTypePart.Operand.GetTypeSize(this),
                        this.GetIntTypeByByteSize(PointerSize));
                    break;

                case Code.Mkrefany:
                    ////var opResult = opCode.OpCodeOperands[0].Result;
                    ////opCode.Result = opResult;
                    var typedRefType = this.System.System_TypedReference;
                    this.SetResultNumber(opCode, typedRefType);
                    //this.WriteAlloca(typedRefType);
                    writer.WriteLine(string.Empty);

                    break;

                case Code.Refanytype:
                    ////var opResult = opCode.OpCodeOperands[0].Result;
                    ////opCode.Result = opResult;

                    opCode.Result = new ConstValue("undef", this.System.System_Object);

                    break;

                case Code.Refanyval:

                    typedRefType = firstOpCodeOperand.Result.Type;

                    var _targetFieldIndex = this.GetFieldIndex(typedRefType, "Value");
                    this.WriteFieldAccess(
                        opCode,
                        typedRefType,
                        typedRefType,
                        _targetFieldIndex,
                        firstOpCodeOperand.Result);
                    writer.WriteLine(string.Empty);
                    this.WriteFieldAccess(opCode, opCode.Result.Type, opCode.Result.Type, 0, opCode.Result);
                    writer.WriteLine(string.Empty);
                    this.WriteLlvmLoad(opCode, opCode.Result.Type, opCode.Result);
                    writer.WriteLine(string.Empty);

                    break;

                case Code.Nop:
                    writer.Write("// nop");
                    break;

                case Code.Initblk:
                case Code.Cpblk:
                case Code.Cpobj:
                case Code.Jmp:
                case Code.Ckfinite:
                    throw new NotImplementedException();
            }
        }

        private void WriteConvertToNativeInt(CIndentedTextWriter writer, OpCodePart opCode)
        {
            var intPtrOper = this.IntTypeRequired(opCode);
            var nativeIntType = intPtrOper
                ? this.System.System_Int32
                : this.System.System_Void.ToPointerType();

            this.ConvertCCast(
                opCode,
                nativeIntType);
        }

        public void AddRequiredRttiDeclaration(IType type)
        {
            if (type.IsByRef)
            {
                this.typeRttiDeclRequired.Add(type.GetElementType());
            }
            else
            {
                this.typeRttiDeclRequired.Add(type);
            }
        }

        public void AddRequiredVirtualTablesDeclaration(IType type)
        {
            if (type.IsByRef)
            {
                this.typeVirtualTablesRequired.Add(type.GetElementType());
            }
            else
            {
                this.typeVirtualTablesRequired.Add(type);
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
        public bool AdjustIntConvertableTypes(CIndentedTextWriter writer, OpCodePart opCode, IType destType)
        {
            if (opCode.Result is ConstValue)
            {
                opCode.Result = new ConstValue(opCode.Result.ToString(), destType);
                return false;
            }

            var sourceType = opCode.Result.Type;
            if (!destType.IsPointer && !sourceType.IsPointer &&
                destType.IsIntValueTypeExtCastRequired(sourceType))
            {
                this.ConvertCCast(
                    opCode,
                    destType);
                return true;
            }

            if (!destType.IsPointer && !sourceType.IsPointer &&
                destType.IsIntValueTypeTruncCastRequired(sourceType))
            {
                this.ConvertCCast(opCode, destType);
                return true;
            }

            // pointer to int, int to pointerf
            if (destType.IntTypeBitSize() > 0 && !destType.IsPointer && !destType.IsByRef &&
                (sourceType.IsPointer || sourceType.IsByRef))
            {
                this.ConvertCCast(opCode, destType);
                return true;
            }

            if (sourceType.IntTypeBitSize() > 0 && (destType.IsPointer || destType.IsByRef) &&
                !sourceType.IsPointer && !sourceType.IsByRef)
            {
                this.ConvertCCast(opCode, destType);
                return true;
            }

            if ((sourceType.IsPointer || sourceType.IsByRef) && (destType.IsPointer || destType.IsByRef)
                && sourceType.GetElementType().TypeNotEquals(destType.GetElementType()))
            {
                this.ConvertCCast(opCode, destType);
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        public void AdjustOperandResultTypeToIncomingType(OpCodePart opCode)
        {
            // cast result if required
            if (opCode.RequiredIncomingType != null && opCode.Result != null &&
                opCode.RequiredIncomingType.TypeNotEquals(opCode.Result.Type)
                && !(opCode.Result is ConstValue))
            {
                bool castRequired;
                bool intAdjustmentRequired;
                this.DetectConversion(
                    opCode.Result.Type,
                    opCode.RequiredIncomingType,
                    out castRequired,
                    out intAdjustmentRequired);

                if (castRequired)
                {
                    this.Output.WriteLine(string.Empty);
                    this.WriteCast(opCode, opCode.Result, opCode.RequiredIncomingType);
                }

                if (intAdjustmentRequired)
                {
                    this.Output.WriteLine(string.Empty);
                    this.AdjustIntConvertableTypes(this.Output, opCode, opCode.RequiredIncomingType);
                }
            }
        }

        public void AdjustResultTypeToOutgoingType(OpCodePart opCode)
        {
            this.AdjustResultTypeToOutgoingType(opCode, opCode.RequiredOutgoingType);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        public void AdjustResultTypeToOutgoingType(OpCodePart opCode, IType requiredOutgoingType)
        {
            // cast result if required
            if (requiredOutgoingType != null && opCode.Result != null &&
                requiredOutgoingType.TypeNotEquals(opCode.Result.Type)
                && !(opCode.Result is ConstValue))
            {
                bool castRequired;
                bool intAdjustmentRequired;
                this.DetectConversion(
                    opCode.Result.Type,
                    requiredOutgoingType,
                    out castRequired,
                    out intAdjustmentRequired);

                if (castRequired)
                {
                    this.WriteCast(opCode, opCode.Result, requiredOutgoingType);
                }

                if (intAdjustmentRequired)
                {
                    this.AdjustIntConvertableTypes(this.Output, opCode, requiredOutgoingType);
                }
            }
        }

        public void UnaryOper(
            CIndentedTextWriter writer,
            OpCodePart opCode,
            string op,
            IType resultType = null)
        {
            writer.Write(op);
            this.WriteOperandResult(writer, opCode, 0, false);
            SetResultNumber(opCode, opCode.OpCodeOperands[0].Result.Type);
        }

        public void UnaryOper(
            CIndentedTextWriter writer,
            OpCodePart opCode,
            int operand,
            string op,
            IType resultType = null)
        {
            writer.Write(op);
            this.WriteOperandResult(writer, opCode, operand, false);
            SetResultNumber(opCode, opCode.OpCodeOperands[operand].Result.Type);
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
        public void BinaryOper(
            CIndentedTextWriter writer,
            OpCodePart opCode,
            string op,
            OperandOptions options = OperandOptions.None,
            IType resultType = null)
        {
            writer.Write("(");
            this.WriteOperandResult(writer, opCode, 0);
            writer.Write(op);
            this.WriteOperandResult(
                writer,
                opCode,
                1,
                options.HasFlag(OperandOptions.DetectAndWriteTypeInSecondOperand));
            writer.Write(")");

            SetResultNumber(opCode, opCode.OpCodeOperands[0].Result.Type);
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
        /// <returns>
        /// </returns>
        public string GetAllocator()
        {
            return this.Gc ? "GC_malloc" : "malloc";
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
            return string.Format("{0}_{1}", name, index);
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
        /// <param name="data">
        /// </param>
        /// <returns>
        /// </returns>
        public int GetBytesIndex(byte[] data)
        {
            var idx = ++this.bytesIndexIncremental;
            this.bytesStorage[idx] = data;
            return idx;
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
            this.Output = new CIndentedTextWriter(new StringWriter(sb));

            this.ActualWrite(this.Output, opCodePart);

            this.Output = output;
            return sb.ToString();
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
            return "this_0";
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public void LoadIndirect(CIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.Result != null)
            {
                return;
            }

            IType type = null;

            switch (opCode.ToCode())
            {
                case Code.Ldind_I:
                    type = this.GetTypeOfReference(opCode);
                    if (type.IsVoid())
                    {
                        // ignore Ldind.i load of Void type
                        opCode.Result = opCode.OpCodeOperands[0].Result;
                        return;
                    }
                    if (!type.IsPointer && !type.IsByRef && type.IntTypeBitSize() == PointerSize * 8)
                    {
                        // using int as intptr
                        type = this.System.System_IntPtr;
                        this.AdjustIntConvertableTypes(writer, opCode.OpCodeOperands[0], type.ToPointerType());
                        opCode.Result = opCode.OpCodeOperands[0].Result;
                    }

                    break;
                case Code.Ldind_I1:
                    // it can be Bool or Byte, leave it null
                    ////type = this.System.System_SByte;
                    var result = EstimatedResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.HasElementType ? result.Type.GetElementType() : result.Type;
                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = this.System.System_SByte;
                    }
                    break;
                case Code.Ldind_U1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.System.System_Byte;
                    result = EstimatedResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.HasElementType ? result.Type.GetElementType() : result.Type;

                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = this.System.System_Byte;
                    }
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
        public void LoadIndirect(CIndentedTextWriter writer, OpCodePart opCode, IType type)
        {
            FullyDefinedReference accessIndexResultNumber2;

            var indirect = true;

            // next code fixing issue with using Code.Ldind to load first value in value types
            var resultOfOperand0 = opCode.OpCodeOperands[0].Result;
            var isUsedAsClass = resultOfOperand0 != null && resultOfOperand0.Type.UseAsClass;
            if (isUsedAsClass)
            {
                resultOfOperand0 = resultOfOperand0.ToNormalType();
            }

            var isValueType = resultOfOperand0 != null && resultOfOperand0.Type.IsValueType;
            if (isValueType && (isUsedAsClass || resultOfOperand0.Type.IsStructureType()))
            {
                // write first field access
                this.WriteFieldAccess(writer, opCode, 0);
                writer.WriteLine(string.Empty);
                accessIndexResultNumber2 = opCode.Result;
                type = opCode.Result.Type;

                // TODO: needs to be fixed, WriteFieldAccess should return Pointer type
                indirect = false;
            }
            else
            {
                accessIndexResultNumber2 = opCode.OpCodeOperands[0].Result;
            }

            if (opCode.Result != null && !opCode.Result.Type.ToDereferencedType().IsStructureType())
            {
                opCode.Result = null;
            }

            this.WriteLlvmLoad(opCode, type, accessIndexResultNumber2, indirect: indirect);
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

            if (fieldType.IsStructureType())
            {
                this.WriteLlvmLoad(opCodePart, fieldType, opCodePart.OpCodeOperands[valueOperand].Result);
            }
            else
            {
                var writer = this.Output;
                UnaryOper(writer, opCodePart, valueOperand, " = ", fieldType);
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

        public void ReadParameters(string[] args)
        {
            // custom settings
            var targetArg = args != null ? args.FirstOrDefault(a => a.StartsWith("target:")) : null;
            this.Target = targetArg != null ? targetArg.Substring("target:".Length) : null;
            this.Gc = args == null || !args.Contains("gc-");
            this.Gctors = args == null || !args.Contains("gctors-");
            this.IsLlvm37 = args != null && args.Contains("llvm37");
            //this.IsLlvm36 = !this.IsLlvm37 && args != null && args.Contains("llvm36");
            this.IsLlvm35 = !this.IsLlvm36 && args != null && args.Contains("llvm35");
            this.IsLlvm34OrLower = !this.IsLlvm35 && args != null && args.Contains("llvm34");
            this.IsLlvm36 = !this.IsLlvm37 && !this.IsLlvm35 && !this.IsLlvm34OrLower;
            this.Stubs = args != null && args.Contains("stubs");

            // prefefined settings
            if (args != null && args.Contains("android"))
            {
                this.Target = this.Target ?? "armv7-none-linux-androideabi";
                this.Gctors = false;
                this.ByValAlign = Math.Max(PointerSize, 8);
                ////this.IsLlvm35 = false;
                ////this.IsLlvm34OrLower = false;
            }
            else if (args != null && args.Contains("emscripten"))
            {
                this.Target = this.Target ?? "asmjs-unknown-emscripten";
                this.Gc = false;
                this.IsLlvm35 = false;
                this.IsLlvm34OrLower = true;

                this.DataLayout = @"e-p:32:32-i64:64-v128:32:128-n32-S128";
            }

            this.DataLayout = this.DataLayout
                              ?? @"e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32";
            this.Target = this.Target ?? "i686-w64-mingw32";
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="testValueResultNumber">
        /// </param>
        /// <param name="exceptionName">
        /// </param>
        /// <param name="labelPrefix">
        /// </param>
        /// <param name="labelSuffix">
        /// </param>
        /// <param name="action">
        /// </param>
        public void WriteBranchSwitchToExecute(
            CIndentedTextWriter writer,
            OpCodePart opCodePart,
            FullyDefinedReference testValueResultNumber,
            string exceptionName,
            string labelPrefix,
            string labelSuffix,
            Action action)
        {
            writer.WriteLine(
                "br i1 {0}, label %.{2}_result_{3}{1}, label %.{2}_result_not_{3}{1}",
                testValueResultNumber,
                opCodePart.AddressStart,
                labelPrefix,
                labelSuffix);

            writer.WriteLine(string.Empty);

            writer.Indent--;
            writer.WriteLine(".{1}_result_{2}{0}:", opCodePart.AddressStart, labelPrefix, labelSuffix);
            writer.Indent++;

            action();

            var label = string.Concat(labelPrefix, "_result_not_", labelSuffix, opCodePart.AddressStart);

            writer.Indent--;
            writer.WriteLine(".{0}:", label);
            writer.Indent++;

            CHelpersGen.SetCustomLabel(opCodePart, label);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="testValueResultNumber">
        /// </param>
        /// <param name="exceptionName">
        /// </param>
        /// <param name="labelPrefix">
        /// </param>
        /// <param name="labelSuffix">
        /// </param>
        public void WriteBranchSwitchToThrowOrPass(
            CWriter cWriter,
            OpCodePart opCodePart,
            FullyDefinedReference testValueResultNumber,
            string exceptionName,
            string labelPrefix,
            string labelSuffix)
        {
            var writer = cWriter.Output;

            this.WriteBranchSwitchToExecute(
                writer,
                opCodePart,
                testValueResultNumber,
                exceptionName,
                labelPrefix,
                labelSuffix,
                () => this.WriteThrowException(cWriter, exceptionName));
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
        public void WriteCondBranch(
            CIndentedTextWriter writer,
            IncrementalResult compareResult,
            string trueLabel,
            string falseLabel)
        {
            writer.WriteLine("br i1 {0}, label %{1}, label %{2}", compareResult, trueLabel, falseLabel);
            this.WriteLabel(writer, trueLabel);
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
            declaringType.WriteCallNewObjectMethod(this, opCode);

            var newObjectResult = opCode.Result;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Copy data");

            if (!declaringType.IsStructureType() && declaringType.FullName != "System.DateTime" &&
                declaringType.FullName != "System.Decimal")
            {
                this.WriteFieldAccess(
                    opCode,
                    declaringType.ToClass(),
                    declaringType.ToClass(),
                    0,
                    opCode.Result);
                writer.WriteLine(string.Empty);
            }

            var fieldType = declaringType;

            this.SaveToField(opCode, fieldType.ToNormal(), 0);

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");

            opCode.Result = newObjectResult;
            opCode.Result = opCode.Result.ToClassType();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="typeToCopy">
        /// </param>
        /// <param name="source">
        /// </param>
        /// <param name="dest">
        /// </param>
        public void WriteCopyStruct(
            CIndentedTextWriter writer,
            OpCodePart opCode,
            IType typeToCopy,
            FullyDefinedReference source,
            FullyDefinedReference dest)
        {
            this.WriteBitcast(opCode, dest);
            var op1 = opCode.Result;
            writer.WriteLine(string.Empty);
            this.WriteBitcast(opCode, source);
            var op2 = opCode.Result;
            writer.WriteLine(string.Empty);

            this.WriteMemCopy(typeToCopy, op1, op2);

            opCode.Result = dest;
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
        /// <param name="throwExceptionIfNull">
        /// </param>
        public void WriteDynamicCast(
            CIndentedTextWriter writer,
            OpCodePart opCodeTypePart,
            FullyDefinedReference fromType,
            IType toType,
            bool checkNull = false,
            bool throwExceptionIfNull = false)
        {
            var effectiveFromType = fromType.Type.UseAsClass
                ? fromType.ToDereferencedType().ToClassType()
                : fromType.ToDereferencedType();
            if (effectiveFromType.Type.TypeEquals(toType))
            {
                opCodeTypePart.Result = fromType;
                return;
            }

            Debug.Assert(!fromType.Type.IsVoid());
            Debug.Assert(!(fromType is ConstValue));
            Debug.Assert(!fromType.Type.IsPrimitiveType());
            Debug.Assert(!fromType.Type.IsStructureType());

            if (checkNull)
            {
                var testNullResultNumber = this.SetResultNumber(opCodeTypePart, this.System.System_Boolean);
                writer.Write("icmp eq ");
                fromType.Type.WriteTypePrefix(this);
                writer.WriteLine(" {0}, null", fromType);

                writer.WriteLine(
                    "br i1 {0}, label %.dynamic_cast_null{1}, label %.dynamic_cast_not_null{1}",
                    testNullResultNumber,
                    opCodeTypePart.AddressStart);

                writer.Indent--;
                writer.WriteLine(".dynamic_cast_not_null{0}:", opCodeTypePart.AddressStart);
                writer.Indent++;
            }

            this.WriteCCast(opCodeTypePart, fromType, this.System.System_Byte);
            writer.WriteLine(string.Empty);

            var firstCastToBytesResult = opCodeTypePart.Result;

            var dynamicCastResultNumber = this.SetResultNumber(
                opCodeTypePart,
               this.System.System_Byte.ToPointerType());

            writer.Write("call i8* @__dynamic_cast(i8* {0}, i8* bitcast (", firstCastToBytesResult);
            effectiveFromType.Type.WriteRttiClassInfoDeclaration(writer);
            writer.Write("* @\"{0}\" to i8*), i8* bitcast (", effectiveFromType.Type.GetRttiInfoName());
            toType.WriteRttiClassInfoDeclaration(writer);
            writer.WriteLine(
                "* @\"{0}\" to i8*), i32 {1})",
                toType.GetRttiInfoName(),
                CalculateDynamicCastInterfaceIndex(effectiveFromType.Type, toType));
            writer.WriteLine(string.Empty);

            if (throwExceptionIfNull)
            {
                this.WriteTestNullValueAndThrowException(
                    writer,
                    opCodeTypePart,
                    dynamicCastResultNumber,
                    "System.InvalidCastException",
                    "dynamic_cast");
            }

            var toClassType = toType.ToClass();
            this.WriteCCast(opCodeTypePart, dynamicCastResultNumber, toClassType);

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

                var testNullResultNumber = this.SetResultNumber(opCodeTypePart, toClassType);
                writer.Write("phi ");
                toClassType.WriteTypePrefix(this, true);
                writer.Write(
                    " [ {0}, {1} ], [ null, {2} ]",
                    dynamicCastResult,
                    string.Format(
                        "%.{1}{0}",
                        opCodeTypePart.AddressStart,
                        throwExceptionIfNull ? "dynamic_cast_result_not_null" : "dynamic_cast_not_null"),
                    string.Format("%.dynamic_cast_null{0}", opCodeTypePart.AddressStart));

                CHelpersGen.SetCustomLabel(opCodeTypePart, label);
            }

            this.AddRequiredRttiDeclaration(effectiveFromType.Type);
            this.AddRequiredRttiDeclaration(toType);
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
            var operandResultCalc = EstimatedResultOf(operand);
            var operandType = operandResultCalc.Type;
            var effectiveType = operandType;
            var asPointer = false;
            if (effectiveType.IsValueType)
            {
                if (operand.Result.Type.IntTypeBitSize() == PointerSize * 8)
                {
                    effectiveType = opCodeFieldInfoPart.Operand.DeclaringType;
                    this.ConvertCCast(operand, effectiveType.ToPointerType());

                    asPointer = true;

                    writer.WriteLine(string.Empty);
                }
                else if (!effectiveType.IsByRef)
                {
                    effectiveType = effectiveType.ToClass();
                }
            }
            else if (effectiveType.IsPointer)
            {
                effectiveType = opCodeFieldInfoPart.Operand.DeclaringType;
                this.ConvertCCast(operand, effectiveType.ToPointerType());

                asPointer = true;

                writer.WriteLine(string.Empty);
            }

            this.WriteResultOrActualWrite(writer, opCodeFieldInfoPart.OpCodeOperands[0]);
            writer.Write("->");
            this.WriteFieldIndex(effectiveType.IsByRef ? effectiveType.GetElementType() : effectiveType, opCodeFieldInfoPart.Operand);

            SetResultNumber(opCodeFieldInfoPart, opCodeFieldInfoPart.Operand.FieldType);
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
        public void WriteFieldAccess(CIndentedTextWriter writer, OpCodePart opCodePart, int index, FullyDefinedReference fixedArrayElementIndex = null)
        {
            var operand = EstimatedResultOf(opCodePart.OpCodeOperands[0]);

            var classType = operand.Type.ToClass();

            var opts = OperandOptions.GenerateResult;
            var field = classType.GetFieldByFieldNumber(index, this);
            var fieldType = field.FieldType;

            this.UnaryOper(writer, opCodePart, string.Empty, classType);

            writer.Write("->");

            this.WriteFieldIndex(writer, classType, classType, field, index, fixedArrayElementIndex);
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
            OpCodePart opCodePart,
            IType classType,
            IType fieldContainerType,
            int index,
            FullyDefinedReference valueReference)
        {
            var writer = this.Output;

            var field = fieldContainerType.GetFieldByFieldNumber(index, this);
            if (field == null)
            {
                return false;
            }

            var fieldType = field.FieldType;
            this.SetResultNumber(opCodePart, fieldType);

            writer.Write(valueReference);
            writer.Write("->");
            this.WriteFieldIndex(writer, classType, fieldContainerType, field, index);

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="classType">
        /// </param>
        /// <param name="fieldInfo">
        /// </param>
        public void WriteFieldIndex(IType classType, IField fieldInfo)
        {
            var writer = this.Output;

            var targetType = fieldInfo.DeclaringType;
            var type = classType;

            while (type.TypeNotEquals(targetType))
            {
                type = type.BaseType;
                if (type == null)
                {
                    Debug.Assert(false, "could not find base class");
                    break;
                }

                // first index is base type index
                writer.Write(".");
                type.WriteTypePrefix(this);
            }

            writer.Write(fieldInfo.Name);
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
        public void WriteFieldIndex(
            CIndentedTextWriter writer,
            IType classType,
            IType fieldContainerType,
            IField field,
            int fieldIndex,
            FullyDefinedReference fixedArrayElementIndex = null)
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
            writer.Write(fieldIndex + this.CalculateFirstFieldPositionInType(fieldContainerType));

            // if we loading fixed data we need to convert [ 0 x Ty ]* into Ty*
            if (field.IsFixed)
            {
                if (fixedArrayElementIndex != null)
                {
                    writer.Write(", ");
                    fixedArrayElementIndex.Type.WriteTypePrefix(this);
                    writer.Write(" {0}", fixedArrayElementIndex);
                }
                else
                {
                    writer.Write(", i32 0");
                }
            }
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
            CIndentedTextWriter writer,
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
            this.SetResultNumber(opCodeMethodInfo, thisType);
            writer.Write("bitcast ");
            this.WriteMethodPointerType(writer, methodInfo, thisType);
            writer.Write("** ");
            this.WriteResult(pointerToInterfaceVirtualTablePointersResultNumber);
            writer.Write(" to i8*");
            writer.WriteLine(string.Empty);
            var pointerToInterfaceVirtualTablePointersAsBytePointerResultNumber = opCodeMethodInfo.Result;

            // get 'this' address
            var thisAddressFromInterfaceResultNumber = this.SetResultNumber(opCodeMethodInfo, thisType);
            writer.Write("getelementptr i8* ");
            this.WriteResult(pointerToInterfaceVirtualTablePointersAsBytePointerResultNumber);
            writer.Write(", i32 ");
            this.WriteResult(offsetAddressAsIntResultNumber);
            writer.WriteLine(string.Empty);

            // adjust 'this' pointer
            this.SetResultNumber(opCodeMethodInfo, thisType);
            writer.Write("bitcast i8* ");
            this.WriteResult(thisAddressFromInterfaceResultNumber);
            writer.Write(" to ");
            thisType.WriteTypePrefix(this);
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringTypeIn">
        /// </param>
        /// <param name="interface">
        /// </param>
        public void WriteInterfaceAccess(
            CIndentedTextWriter writer,
            OpCodePart opCode,
            IType declaringTypeIn,
            IType @interface)
        {
            var objectResult = opCode.Result;

            writer.WriteLine("; Get type '{0}' of '{1}'", @interface, declaringTypeIn);

            var declaringType = declaringTypeIn.ToClass();

            writer.Write("getelementptr inbounds");

            writer.Write(' ');
            writer.Write(objectResult);

            this.WriteInterfaceIndex(writer, declaringType, @interface);

            writer.WriteLine(string.Empty);
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

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        public void WriteMethodDefinitionName(
            CIndentedTextWriter writer,
            IMethod methodBase,
            IType ownerOfExplicitInterface = null)
        {
            writer.Write(methodBase.GetFullMethodName(ownerOfExplicitInterface));
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
            CIndentedTextWriter writer,
            IMethod method,
            bool hasThis,
            IType thisType,
            IType returnType,
            bool noArgumentName = false,
            bool varArgs = false)
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
            foreach (var parameter in parameterInfos)
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

            if (varArgs)
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
        public void WriteMethodPointerType(CIndentedTextWriter writer, IMethod methodBase, IType thisType = null)
        {
            var methodInfo = methodBase;
            var isStructureType = methodInfo.ReturnType.IsStructureType();
            if (!isStructureType)
            {
                methodInfo.ReturnType.WriteTypePrefix(this);
            }
            else
            {
                writer.Write("void");
            }

            writer.Write(" (");

            if (isStructureType)
            {
                methodInfo.ReturnType.WriteTypePrefix(this, true);
            }

            var hasThis = !methodInfo.IsStatic;
            if (hasThis)
            {
                if (isStructureType)
                {
                    writer.Write(", ");
                }

                (thisType ?? methodInfo.DeclaringType.ToClass()).WriteTypePrefix(this);
            }

            var isAnonymousDelegate = methodBase.IsAnonymousDelegate;
            var index = 0;
            foreach (var parameter in methodBase.GetParameters())
            {
                if (isAnonymousDelegate)
                {
                    isAnonymousDelegate = false;
                    continue;
                }

                if (index > 0 || hasThis || isStructureType)
                {
                    writer.Write(", ");
                }

                if (!parameter.ParameterType.IsStructureType())
                {
                    parameter.ParameterType.WriteTypePrefix(this);
                }
                else
                {
                    parameter.ParameterType.ToClass().WriteTypePrefix(this);
                }

                index++;
            }

            if (methodInfo.CallingConvention.HasFlag(CallingConventions.VarArgs))
            {
                if (index > 0 || hasThis || isStructureType)
                {
                    writer.Write(", ");
                }

                writer.Write("...");
            }

            writer.Write(")*");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="method">
        /// </param>
        public void WriteMethodReturnType(CIndentedTextWriter writer, IMethod method)
        {
            if (!method.ReturnType.IsVoid())
            {
                method.ReturnType.WriteTypePrefix(this);
                writer.Write(" ");
            }
            else
            {
                this.Output.Write("void ");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="typeName">
        /// </param>
        /// <returns>
        /// </returns>
        public FullyDefinedReference WriteNewCallingDefaultConstructor(CWriter cWriter, string typeName)
        {
            var typeToCreate = ResolveType(typeName);
            return this.WriteNewCallingDefaultConstructor(cWriter, typeToCreate);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="typeToCreate">
        /// </param>
        /// <returns>
        /// </returns>
        public FullyDefinedReference WriteNewCallingDefaultConstructor(
            CWriter cWriter,
            IType typeToCreate)
        {
            CIndentedTextWriter writer = cWriter.Output;

            // throw InvalidCast result
            writer.WriteLine(string.Empty);

            // find constructor
            var constructorInfo = Logic.IlReader.Constructors(typeToCreate, cWriter).First(c => !c.GetParameters().Any());

            var opCodeNewInstance = new OpCodeConstructorInfoPart(OpCodesEmit.Newobj, 0, 0, constructorInfo);

            this.WriteNewObject(opCodeNewInstance);

            writer.WriteLine(string.Empty);

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
        /// <param name="firstParameterValue">
        /// </param>
        /// <returns>
        /// </returns>
        public FullyDefinedReference WriteNewWithCallingConstructor(
            OpCodePart opCode,
            IType type,
            IType firstParameterType,
            FullyDefinedReference firstParameterValue)
        {
            // find constructor
            var constructorInfo =
                Logic.IlReader.Constructors(type, this)
                    .FirstOrDefault(
                        c =>
                            c.GetParameters().Count() == 1 &&
                            c.GetParameters().First().ParameterType.TypeEquals(firstParameterType));

            ////Debug.Assert(constructorInfo != null, "Could not find required constructor");

            type.WriteCallNewObjectMethod(this, opCode);

            var dummyOpCodeWithStringIndex = OpCodePart.CreateNop;
            dummyOpCodeWithStringIndex.Result = firstParameterValue;

            opCode.OpCodeOperands = new[] { dummyOpCodeWithStringIndex };

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
        /// <param name="operand">
        /// </param>
        /// <param name="detectAndWriteTypePrefix">
        /// </param>
        /// <param name="forcedType">
        /// </param>
        public void WriteResultOrActualWrite(
            CIndentedTextWriter writer,
            OpCodePart operand,
            bool detectAndWriteTypePrefix = false,
            IType forcedType = null)
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
        /// <param name="index">
        /// </param>
        /// <param name="detectAndWriteTypePrefix">
        /// </param>
        public void WriteOperandResult(
            CIndentedTextWriter writer,
            OpCodePart opCode,
            int index,
            bool detectAndWriteTypePrefix = false)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            var operand = opCode.OpCodeOperands[index];
            this.WriteResultOrActualWrite(writer, operand, detectAndWriteTypePrefix);
        }

        public void WriteStartOfPhiValues(CIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode == opCode.UsedByAlternativeValues.Values[0])
            {
                opCode.RequiredOutgoingType.WriteTypePrefix(this);
                this.Output.WriteLine(" _phi{0};", opCode.UsedByAlternativeValues.Values[0].AddressStart);
            }

            if (opCode.Result == null)
            {
                this.Output.Write("_phi{0} = ", opCode.UsedByAlternativeValues.Values[0].AddressStart);
            }
        }

        public void WriteEndOfPhiValues(CIndentedTextWriter writer, OpCodePart opCode)
        {
            this.WriteResult(opCode);
            opCode.Result = new FullyDefinedReference("_phi" + opCode.UsedByAlternativeValues.Values[0].AddressStart, opCode.Result.Type);
        }

        [Obsolete]
        public void WritePhi(CIndentedTextWriter writer, OpCodePart opCode)
        {
            while (opCode.AlternativeValues != null && opCode.AlternativeValues.Count > 0)
            {
                var alternativeValues = opCode.AlternativeValues.Dequeue();
                WritePhi(writer, opCode, alternativeValues);
            }

            opCode.AlternativeValues = null;
        }


        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public void WritePhi(CIndentedTextWriter writer, OpCodePart opCode, PhiNodes alternativeValues)
        {
            if (alternativeValues.Values.Count != alternativeValues.Labels.Count)
            {
                // phi is not full
                return;
            }

            writer.WriteLine(string.Empty);

            var firstValueWithRequiredType =
                alternativeValues.Values.FirstOrDefault(
                    v => v.RequiredOutgoingType != null && !(v.ResultAtExit is ConstValue));

            var firstValueRequiredType = firstValueWithRequiredType != null
                ? firstValueWithRequiredType.RequiredOutgoingType
                : null;

            var phiType = firstValueRequiredType;
            if (phiType == null)
            {
                var firstNonConstValue =
                    alternativeValues.Values.FirstOrDefault(v => !(v.ResultAtExit is ConstValue));
                if (firstNonConstValue != null)
                {
                    phiType = firstNonConstValue.ResultAtExit.Type ?? firstNonConstValue.RequiredOutgoingType;
                }
            }

            if (phiType == null)
            {
                var firstValue = alternativeValues.Values.First();
                phiType = firstValue.RequiredOutgoingType ?? firstValue.ResultAtExit.Type;
            }

            var structUsed = false;
            if (phiType.IsStructureType())
            {
                phiType = phiType.ToClass();
                structUsed = true;
            }

            // adjust types of constants
            if (!phiType.IsValueType)
            {
                foreach (
                    var val in
                        alternativeValues.Values.Where(v => v.ResultAtExit is ConstValue && v.Any(Code.Ldc_I4_0)))
                {
                    val.ResultAtExit = new ConstValue(null, this.System.System_Void.ToPointerType());
                }
            }

            // apply PHI if condition is complex
            var nopeCode = OpCodePart.CreateNop;

            writer.Write("phi");

            var count = alternativeValues.Values.Count;
            for (var index = 0; index < count; index++)
            {
                if (index > 0)
                {
                    writer.Write(",");
                }

                var values = alternativeValues.Values;
                this.WritePhiNodeLabel(
                    writer,
                    values[index].ResultAtExit,
                    values[index],
                    values[index],
                    string.Concat("a", alternativeValues.Labels[index]),
                    index + 1 < count ? alternativeValues.Labels[index + 1] : opCode.AddressStart,
                    alternativeValues.Labels[index]);
            }

            writer.WriteLine(string.Empty);

            this.AdjustResultTypeToOutgoingType(nopeCode, RequiredIncomingType(opCode));

            var connectedValue = alternativeValues.Values.LastOrDefault(v => v.UsedBy != null);
            if (connectedValue == null)
            {
                connectedValue = alternativeValues.Values.Last(v => v.UsedByAlternativeValues != alternativeValues);
            }

            connectedValue.Result = structUsed
                ? nopeCode.Result.ToNormalType()
                : nopeCode.Result;
            connectedValue.ResultAtExit = connectedValue.Result;
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
        /// <param name="opCode">
        /// </param>
        /// <param name="methodReturnType">
        /// </param>
        public void WriteReturn(CIndentedTextWriter writer, OpCodePart opCode, IType methodReturnType)
        {
            var opts = this.WriteReturnStruct(opCode, methodReturnType);
            writer.Write("return");
            if (methodReturnType != null && !methodReturnType.IsVoid())
            {
                this.UnaryOper(writer, opCode, " ", methodReturnType);
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
                opCode.Result = new FullyDefinedReference("%agg.result", methodReturnType);
                this.WriteLlvmLoad(opCode, methodReturnType, opCodeOperand.Result.ToNormalType());

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
        public IncrementalResult SetResultNumber(OpCodePart opCode, IType type)
        {
            var llvmResult = new IncrementalResult(++this.resultNumberIncremental, type);
            if (opCode != null)
            {
                opCode.Result = llvmResult;
            }

            return llvmResult;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="resultToTest">
        /// </param>
        /// <returns>
        /// </returns>
        public IncrementalResult WriteTestNull(
            CIndentedTextWriter writer,
            OpCodePart opCodePart,
            FullyDefinedReference resultToTest)
        {
            var testNullResultNumber = this.SetResultNumber(opCodePart, this.System.System_Boolean);
            opCodePart.Result = resultToTest;

            writer.Write("icmp eq ");
            resultToTest.Type.WriteTypePrefix(this);
            writer.WriteLine(" {0}, null", resultToTest);
            return testNullResultNumber;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="resultToTest">
        /// </param>
        /// <param name="exceptionName">
        /// </param>
        /// <param name="labelPrefix">
        /// </param>
        public void WriteTestNullValueAndThrowException(
            CIndentedTextWriter writer,
            OpCodePart opCodePart,
            IncrementalResult resultToTest,
            string exceptionName,
            string labelPrefix)
        {
            var testNullResultNumber = this.WriteTestNull(writer, opCodePart, resultToTest);
            this.WriteBranchSwitchToThrowOrPass(
                this,
                opCodePart,
                testNullResultNumber,
                exceptionName,
                labelPrefix,
                "null");
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

            var invalidCastExceptionType = ResolveType(exceptionName);

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
        /// <param name="options">
        /// </param>
        /// <param name="operand1">
        /// </param>
        /// <param name="operand2">
        /// </param>
        /// <returns>
        /// </returns>
        private IType ApplyTypeAdjustment(
            CIndentedTextWriter writer,
            OpCodePart opCode,
            IType effectiveType,
            IType castFrom,
            IType intAdjustment,
            bool intAdjustSecondOperand,
            ref IType resultType,
            OperandOptions options,
            int operand1 = 0,
            int operand2 = 1)
        {
            if (!options.HasFlag(OperandOptions.TypeIsInOperator) &&
                (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0))
            {
                return effectiveType;
            }

            var operator1 = options.HasFlag(OperandOptions.TypeIsInOperator) ? opCode : opCode.OpCodeOperands[operand1];
            var operator2 = !options.HasFlag(OperandOptions.TypeIsInOperator)
                ? opCode.OpCodeOperands[operand2 >= 0 && opCode.OpCodeOperands.Length > operand2 && intAdjustSecondOperand
                        ? operand2
                        : operand1]
                : null;

            if (castFrom != null)
            {
                this.WriteCast(operator1, operator1.Result, effectiveType);
            }

            if (intAdjustment != null)
            {
                var changeType = this.AdjustIntConvertableTypes(writer, operator2, intAdjustment);

                if (changeType)
                {
                    effectiveType = intAdjustment;
                    if (resultType == null)
                    {
                        resultType = intAdjustment;
                    }
                }
            }

            return effectiveType;
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
        /// <param name="sourceType">
        /// </param>
        /// <param name="requiredType">
        /// </param>
        /// <param name="castRequired">
        /// </param>
        /// <param name="intAdjustmentRequired">
        /// </param>
        private void DetectConversion(
            IType sourceType,
            IType requiredType,
            out bool castRequired,
            out bool intAdjustmentRequired)
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
                if (requiredType.IsIntValueTypeExtCastRequired(sourceType) ||
                    requiredType.IsIntValueTypeTruncCastRequired(sourceType))
                {
                    intAdjustmentRequired = true;
                }

                // pointer to int, int to pointer
                if (!sourceType.IsByRef && !sourceType.IsPointer && sourceType.IntTypeBitSize() > 0 &&
                    requiredType.IsPointer)
                {
                    intAdjustmentRequired = true;
                }

                return;
            }

            if ((sourceType.IsByRef && requiredType.IsPointer || requiredType.IsByRef && sourceType.IsPointer)
                && sourceType.GetElementType().Equals(requiredType.GetElementType()))
            {
                return;
            }

            if ((sourceType.IsByRef && requiredType.IsPointer || requiredType.IsByRef && sourceType.IsClass)
                && sourceType.ToNormal().Equals(requiredType.GetElementType()))
            {
                return;
            }

            if (sourceIntType && requiredType.IsPointer || requiredIntType && sourceType.IsPointer)
            {
                intAdjustmentRequired = true;
                return;
            }

            // TODO: review it
            if (sourceType.TypeEquals(this.System.System_Boolean) &&
                requiredType.TypeEquals(this.System.System_Byte))
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
        [Obsolete]
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
                ? EstimatedResultOf(opCode)
                : opCode.OpCodeOperands != null && operand1 >= 0 && opCode.OpCodeOperands.Length > operand1
                    ? EstimatedResultOf(opCode.OpCodeOperands[operand1])
                    : null;

            var res2 = opCode.OpCodeOperands != null && operand2 >= 0 && opCode.OpCodeOperands.Length > operand2
                ? EstimatedResultOf(opCode.OpCodeOperands[operand2])
                : null;

            // write type
            IType effectiveType = null;

            var res1Pointer = options.HasFlag(OperandOptions.CastPointersToBytePointer) && res1 != null &&
                              res1.IsPointerAccessRequired;
            var res2Pointer = options.HasFlag(OperandOptions.CastPointersToBytePointer) && res2 != null &&
                              res2.IsPointerAccessRequired;
            var requiredTypePointer = options.HasFlag(OperandOptions.CastPointersToBytePointer) && requiredType != null &&
                                      (requiredType.IsPointer || requiredType.IsByRef);

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
                if (options.HasFlag(OperandOptions.AdjustIntTypes) && res1 != null && res1.Type != null && res2 != null &&
                    res2.Type != null
                    && res1.Type.TypeEquals(res2.Type) && res1.Type.TypeNotEquals(requiredType) &&
                    res1.Type.TypeEquals(this.System.System_Boolean)
                    && requiredType.TypeEquals(this.System.System_Byte))
                {
                    effectiveType = res1.Type;
                }
                else
                {
                    effectiveType = requiredType;
                }
            }
            else if (options.HasFlag(OperandOptions.TypeIsInOperator) ||
                     opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > operand1)
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
                && (res1.Type.IsClass || res1.Type.IsArray || res1.Type.IsInterface || res1.Type.IsDelegate) &&
                effectiveType.IsAssignableFrom(res1.Type))
            {
                castFrom = res1.Type;
            }

            if (options.HasFlag(OperandOptions.AdjustIntTypes))
            {
                var firstType = res1 != null && res1.Type != null && !res1.IsConst
                    ? res1.Type
                    : res2 != null && res2.Type != null && !res2.IsConst ? res2.Type : null;
                if (firstType != null)
                {
                    IType secondType = null;
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
                        if (!firstType.IsByRef && !firstType.IsPointer && firstType.IntTypeBitSize() > 0 &&
                            secondType.IsPointer)
                        {
                            intAdjustSecondOperand = true;
                            intAdjustment = firstType;

                            if (requiredType != null && requiredType.IsPointer)
                            {
                                intAdjustment = secondType;
                            }
                        }

                        if (!secondType.IsByRef && !secondType.IsPointer && secondType.IntTypeBitSize() > 0 &&
                            firstType.IsPointer)
                        {
                            intAdjustSecondOperand = false;
                            intAdjustment = secondType;

                            if (requiredType != null && requiredType.IsPointer)
                            {
                                intAdjustment = firstType;
                            }
                        }
                    }
                    else
                    {
                        if (options.HasFlag(OperandOptions.DetectAndWriteTypeInSecondOperand) && res2.IsConst)
                        {
                            intAdjustment = res1.Type;
                            intAdjustSecondOperand = true;
                        }

                        // if it is pointer operation with integer adjust it to integer
                        if (res1.Type.IsPointer && res2.IsConst)
                        {
                            intAdjustment = res2.Type;
                            intAdjustSecondOperand = false;
                        }

                        if (res2.Type.IsPointer && res1.IsConst)
                        {
                            intAdjustment = res1.Type;
                            intAdjustSecondOperand = true;
                        }
                    }

                    if (IsPointerArithmetic(opCode))
                    {
                        requiredType = this.GetIntTypeByByteSize(PointerSize);
                        if (res1.Type.IsPointer && res2.IsConst)
                        {
                            intAdjustment = requiredType;
                            intAdjustSecondOperand = false;
                        }

                        if (res2.Type.IsPointer && res1.IsConst)
                        {
                            intAdjustment = requiredType;
                            intAdjustSecondOperand = true;
                        }

                        if (res1.Type.IsPointer && res2.Type.IsPointer)
                        {
                            opCode.OpCodeOperands[operand1].RequiredIncomingType = requiredType;
                            opCode.OpCodeOperands[operand2].RequiredIncomingType = requiredType;
                            this.AdjustOperandResultTypeToIncomingType(opCode.OpCodeOperands[operand1]);
                            this.AdjustOperandResultTypeToIncomingType(opCode.OpCodeOperands[operand2]);

                            effectiveType = requiredType;
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
            this.WriteFieldAccess(opCodeFieldInfoPart);

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
        private string FindCustomLabel(OpCodePart firstOpCode, OpCodePart lastOpCode, int startAddress, int stopAddress)
        {
            if (lastOpCode == null)
            {
                return null;
            }

            string customLabel = null;
            var current = lastOpCode;
            if (startAddress > 0)
            {
                while (current != null && /*firstOpCode.GroupAddressStart <= current.AddressStart &&*/
                       current.AddressStart < startAddress)
                {
                    if (current.CreatedLabel != null)
                    {
                        customLabel = current.CreatedLabel;
                    }

                    if (current.OpCode.FlowControl == FlowControl.Branch ||
                        current.OpCode.FlowControl == FlowControl.Cond_Branch)
                    {
                        break;
                    }

                    current = current.Next;
                }
            }

            if (customLabel != null)
            {
                return customLabel;
            }

            current = lastOpCode;
            while (current != null && /*firstOpCode.GroupAddressStart <= current.AddressStart &&*/
                   current.AddressStart >= stopAddress)
            {
                if (current.CreatedLabel != null)
                {
                    return current.CreatedLabel;
                }

                current = current.Previous;
            }

            return null;
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
            return this.GetArgVarName(Parameters[parameterIndex], argIndex);
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
            return this.GetGlobalConstructorsFunctionName(Path.GetFileNameWithoutExtension(this.AssemblyQualifiedName));
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
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IntTypeRequired(OpCodePart opCode)
        {
            if (opCode == null || opCode.UsedBy == null)
            {
                return false;
            }

            if (opCode.UsedBy.Any(
                Code.Add,
                Code.Add_Ovf,
                Code.Add_Ovf_Un,
                Code.Sub,
                Code.Sub_Ovf,
                Code.Sub_Ovf_Un,
                Code.Mul,
                Code.Mul_Ovf,
                Code.Mul_Ovf_Un,
                Code.Div,
                Code.Div_Un))
            {
                return true;
            }

            if (opCode.UsedBy.OperandPosition == 1
                && opCode.UsedBy.Any(
                    Code.Ldelem,
                    Code.Ldelem_I,
                    Code.Ldelem_I1,
                    Code.Ldelem_I2,
                    Code.Ldelem_I4,
                    Code.Ldelem_I8,
                    Code.Ldelem_R4,
                    Code.Ldelem_R8,
                    Code.Ldelem_Ref,
                    Code.Ldelem_U1,
                    Code.Ldelem_U2,
                    Code.Ldelem_U4,
                    Code.Ldelema))
            {
                return true;
            }

            if (opCode.UsedBy.OperandPosition == 1
                && opCode.UsedBy.Any(
                    Code.Stelem,
                    Code.Stelem_I,
                    Code.Stelem_I1,
                    Code.Stelem_I2,
                    Code.Stelem_I4,
                    Code.Stelem_I8,
                    Code.Stelem_R4,
                    Code.Stelem_R8,
                    Code.Stelem_Ref))
            {
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
        private void LoadElement(CIndentedTextWriter writer, OpCodePart opCode)
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

                    // type = this.System.System_Int32;
                    type = this.GetTypeOfReference(opCode);
                    break;
                case Code.Ldelem_I1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.System.System_SByte;
                    var result = EstimatedResultOf(opCode.OpCodeOperands[0]);
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
                    result = EstimatedResultOf(opCode.OpCodeOperands[0]);
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

            if (opCode.OpCodeOperands[1].Result.Type.IsStructureType())
            {
                // load index from struct type
                this.WriteLlvmLoadPrimitiveFromStructure(opCode.OpCodeOperands[1], opCode.OpCodeOperands[1].Result);
                this.AdjustIntConvertableTypes(writer, opCode.OpCodeOperands[1], this.GetIntTypeByByteSize(PointerSize));
            }

            this.LoadElement(writer, opCode, "data", type, opCode.OpCodeOperands[1].Result, actualLoad);
        }

        public void LoadElement(CIndentedTextWriter writer, OpCodePart opCode, string field, IType type = null, FullyDefinedReference fixedArrayIndex = null, bool actualLoad = true)
        {
            this.WriteFieldAccess(
                writer,
                opCode,
                this.GetFieldIndex(opCode.OpCodeOperands[0].Result.Type, field),
                fixedArrayIndex);

            if (actualLoad)
            {
                writer.WriteLine(string.Empty);

                var accessIndexResultNumber = opCode.Result;
                opCode.Result = null;

                var effectiveType = type ?? accessIndexResultNumber.Type;
                if (!effectiveType.IsStructureType())
                {
                    this.WriteLlvmLoad(opCode, effectiveType, accessIndexResultNumber);
                }
                else
                {
                    opCode.Result = accessIndexResultNumber;
                }
            }
        }

        private void SetSettings(string fileName, string sourceFilePath, string pdbFilePath, string[] args)
        {
            var extension = Path.GetExtension(fileName);
            this.outputFile = extension != null && extension.Equals(string.Empty) ? fileName + ".cpp" : fileName;

            this.ReadParameters(args);
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
                    var result = EstimatedResultOf(opCode.OpCodeOperands[0]);
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

            this.WriteFieldAccess(writer, opCode, this.GetFieldIndex(opCode.OpCodeOperands[0].Result.Type, "data"), opCode.OpCodeOperands[1].Result);

            writer.WriteLine(string.Empty);

            var operandIndex = 2;

            if (!type.IsStructureType())
            {
                this.AdjustIntConvertableTypes(writer, opCode.OpCodeOperands[operandIndex], type);

                writer.Write("=");
                this.WriteOperandResult(writer, opCode, operandIndex);

                writer.Write(", ");
                type.WriteTypePrefix(this, type.IsStructureType());
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
                    var result = EstimatedResultOf(opCode.OpCodeOperands[0]);
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

            Debug.Assert(!type.IsVoid());

            var resultOfOperand0 = opCode.OpCodeOperands[0].Result;
            var destinationType = resultOfOperand0.Type;
            if (destinationType.IsPointer && destinationType.GetElementType().TypeNotEquals(type))
            {
                // adjust destination type, cast pointer to pointer of type
                this.WriteCCast(opCode, resultOfOperand0, type);
                opCode.OpCodeOperands[0].Result = opCode.Result;
                destinationType = type.ToPointerType();
                writer.WriteLine(string.Empty);
            }
            else if (destinationType.IsByRef && destinationType.GetElementType().TypeNotEquals(type))
            {
                type = destinationType.GetElementType();
            }

            if (!destinationType.IsPointer && !resultOfOperand0.Type.IsPointer && !resultOfOperand0.Type.IsByRef)
            {
                // adjust destination type, cast pointer to pointer of type
                this.WriteIntToPtr(opCode, resultOfOperand0, type);
                opCode.OpCodeOperands[0].Result = opCode.Result;
                destinationType = type.ToPointerType();
                writer.WriteLine(string.Empty);
            }

            if (!type.IsStructureType())
            {
                this.UnaryOper(writer, opCode, 1, "=", type);
                writer.Write(", ");

                destinationType.WriteTypePrefix(this);
                this.WriteOperandResult(writer, opCode, 0);
            }
            else
            {
                opCode.Result = resultOfOperand0;
                this.WriteLlvmLoad(opCode, type, opCode.OpCodeOperands[1].Result);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="operandIndex">
        /// </param>
        /// <param name="destinationIndex">
        /// </param>
        private void SaveObject(OpCodePart opCode, int operandIndex, int destinationIndex)
        {
            var operandResult = opCode.OpCodeOperands[operandIndex].Result.ToNormalType();
            if (!operandResult.Type.IsStructureType())
            {
                this.WriteLlvmSave(
                    opCode,
                    operandResult.Type,
                    operandIndex,
                    opCode.OpCodeOperands[destinationIndex].Result);
            }
            else
            {
                opCode.Result = opCode.OpCodeOperands[0].Result;
                this.WriteLlvmLoad(opCode, operandResult.Type, operandResult);
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
        private void SaveStructElement(CIndentedTextWriter writer, OpCodePart opCode, int operandIndex, IType type)
        {
            // copy struct
            this.WriteLlvmLoad(opCode, type, opCode.OpCodeOperands[operandIndex].Result);
        }

        /// <summary>
        /// </summary>
        private void SortStaticConstructorsByUsage()
        {
            var staticConstructors = new Dictionary<IConstructor, ISet<IType>>();
            foreach (var staticCtor in StaticConstructors)
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
                foreach (var staticConstructorPair in staticConstructors.Where(staticConstructorPair => !staticConstructorPair.Value.Any(
                    v => staticConstructors.Keys.Any(k => k.DeclaringType.TypeEquals(v)))).ToList())
                {
                    staticConstructors.Remove(staticConstructorPair.Key);
                    newStaticConstructors.Add(staticConstructorPair.Key);
                }
            }
            while (staticConstructors.Count > 0 && countBefore != staticConstructors.Count);

            Debug.Assert(staticConstructors.Keys.Count == 0, "Not All static constructors were resolved");

            // add rest as is
            newStaticConstructors.AddRange(staticConstructors.Keys);

            StaticConstructors = newStaticConstructors;
        }

        /// <summary>
        /// </summary>
        /// <param name="pair">
        /// </param>
        private void WriteBytesData(KeyValuePair<int, byte[]> pair)
        {
            this.Output.Write(
                "@.bytes{0} = private unnamed_addr constant {1} {3} {2}",
                pair.Key,
                this.GetArrayTypeHeader(this.System.System_Byte, pair.Value.Length),
                this.GetArrayValuesHeader(this.System.System_Byte, pair.Value.Length, pair.Value.Length),
                "{");

            this.Output.Write(" [");

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
        private void WriteCallGctors()
        {
            // get all references
            foreach (var reference in this.AllReferences.Reverse().Distinct())
            {
                this.Output.WriteLine(this.GetGlobalConstructorsFunctionName(reference) + "();");
            }
        }

        /// <summary>
        /// </summary>
        private void WriteCallGctorsDeclarations()
        {
            // get all references
            foreach (var reference in this.AllReferences.Skip(1).Reverse().Distinct())
            {
                this.Output.WriteLine(this.GetGlobalConstructorsFunctionName(reference) + "();");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCatchBegins(CIndentedTextWriter writer, OpCodePart opCode)
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
        private void WriteCatchFinnallyEnd(CIndentedTextWriter writer, OpCodePart opCode)
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
                var upperLevelExceptionHandlingClause = this.tryScopes.Count > 0
                    ? this.tryScopes.Peek()
                        .Catches.FirstOrDefault(c => c.Flags == ExceptionHandlingClauseOptions.Clause)
                      ?? this.tryScopes.Peek()
                          .Catches.FirstOrDefault(c => c.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                    : null;
                this.WriteCatchEnd(opCode, eh, upperLevelExceptionHandlingClause);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCondBranch(CIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.Write(
                "br i1 {0}, label %.a{1}, label %.a{2}",
                opCode.Result,
                opCode.JumpAddress(),
                opCode.AddressEnd);

            writer.WriteLine(string.Empty);

            writer.Indent--;
            writer.WriteLine(string.Concat(".a", opCode.AddressEnd, ':'));
            writer.Indent++;

            if (opCode.Next != null)
            {
                opCode.Next.JumpProcessed = true;
            }
        }

        /// <summary>
        /// </summary>
        private void WriteConstData()
        {
            // write set of array data
            foreach (var pair in this.bytesStorage)
            {
                this.WriteBytesData(pair);
            }

            if (this.bytesStorage.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                this.bytesStorage.Clear();
            }
        }

        public IEnumerable<OpCodePart> WriteCustomMethodPart(
            SynthesizedMethodDecorator constructedMethod,
            IGenericContext genericContext)
        {
            // save important vars
            var parameters = Parameters;
            var localInfo = LocalInfo;
            var hasMethodThis = HasMethodThis;
            var thisType = ThisType;

            // write custom part
            var ilReader = new IlReader();
            ilReader.TypeResolver = this;
            var baseWriter = new BaseWriter();
            baseWriter.Parameters = constructedMethod.GetParameters().ToArray();
            baseWriter.LocalInfo = constructedMethod.GetMethodBody().LocalVariables.ToArray();
            baseWriter.HasMethodThis = constructedMethod.CallingConvention.HasFlag(CallingConventions.HasThis);
            baseWriter.ThisType = ThisType;

            // sync important vars
            Parameters = baseWriter.Parameters;
            LocalInfo = baseWriter.LocalInfo;
            HasMethodThis = baseWriter.HasMethodThis;
            ThisType = baseWriter.ThisType;

            // start writing process
            baseWriter.Initialize(ThisType);
            baseWriter.StartProcess();
            foreach (var opCodePart in ilReader.OpCodes(constructedMethod, genericContext, null))
            {
                baseWriter.AddOpCode(opCodePart);
            }

            var rest = baseWriter.PrepareWritingMethodBody();
            foreach (var opCodePart in rest)
            {
                this.ActualWrite(this.Output, opCodePart, true);
                this.Output.WriteLine(string.Empty);
            }

            // restor important vars
            Parameters = parameters;
            LocalInfo = localInfo;
            HasMethodThis = hasMethodThis;
            ThisType = thisType;

            return rest;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteEndFinally(CIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.WriteLine("; EndFinally ");
            if (this.catchScopes.Count > 0)
            {
                var finallyClause =
                    this.catchScopes.FirstOrDefault(c => c.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally));
                if (finallyClause != null)
                {
                    this.WriteEndFinally(finallyClause);
                }
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

            if (method.GetMethodBody().ExceptionHandlingClauses.Any())
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
        private void WriteExceptionHandlersProlog(CIndentedTextWriter writer, OpCodePart opCode)
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
        private void WriteGetInterfaceOffsetToObjectRootPointer(
            CIndentedTextWriter writer,
            OpCodePart opCode,
            IMethod methodBase,
            IType thisType = null)
        {
            this.SetResultNumber(opCode, this.System.System_Int32.ToPointerType());
            writer.Write("bitcast ");
            this.WriteMethodPointerType(writer, methodBase, thisType);
            writer.Write("* ");
            this.WriteResult(opCode.OpCodeOperands[0].Result);
            writer.Write(" to ");
            this.System.System_Int32.ToPointerType().WriteTypePrefix(this);
            writer.WriteLine(string.Empty);

            var res = opCode.Result;
            var offsetResult = this.SetResultNumber(opCode, this.System.System_Int32);
            writer.Write("getelementptr ");
            this.System.System_Int32.WriteTypePrefix(this);
            writer.Write("* ");
            this.WriteResult(res);
            writer.WriteLine(", i32 -{0}", ObjectInfrastructure.FunctionsOffsetInVirtualTable);

            opCode.Result = null;
            this.WriteLlvmLoad(opCode, this.System.System_Int32, offsetResult);
        }

        /// <summary>
        /// </summary>
        private void WriteGlobalConstructors()
        {
            // write global ctors caller
            this.Output.WriteLine(string.Empty);
            this.Output.WriteLine("{2}void {0}() {1}", this.GetGlobalConstructorsFunctionName(), "{", declarationPrefix);
            this.Output.Indent++;

            this.SortStaticConstructorsByUsage();

            if (this.Gc && this.IsCoreLib)
            {
                this.Output.WriteLine("GC_init();");
            }

            foreach (var staticCtor in StaticConstructors)
            {
                this.Output.WriteLine("{0}();", staticCtor.GetFullMethodName());
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
        private bool WriteInterfaceIndex(CIndentedTextWriter writer, IType classType, IType @interface)
        {
            var type = classType;
            if (!type.GetAllInterfaces().Contains(@interface))
            {
                return false;
            }

            // first element for pointer (IType* + 0)
            writer.Write(", i32 0");

            while (
                !type.GetInterfacesExcludingBaseAllInterfaces()
                    .Any(i => i.TypeEquals(@interface) || i.GetAllInterfaces().Contains(@interface)))
            {
                type = type.BaseType;
                if (type == null)
                {
                    // throw new IndexOutOfRangeException("Could not find an type");
                    break;
                }

                // first index is base type index
                writer.Write(", i32 0");
            }

            // find index
            var index = 0;

            if (type.IsRootOfVirtualTable(this))
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

        // TODO: here the bug with index, index is caluclated for derived class but need to be calculated and used for type where the type belong to

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteVirtualTables(IType type, bool checkParamsAndReturn = false)
        {
            // write VirtualTable
            if (type.IsInterface)
            {
                return;
            }

            var baseTypeSize = type.BaseType != null ? type.BaseType.GetTypeSize(this) : 0;

            var index = 0;
            if (type.HasAnyVirtualMethod(this))
            {
                this.Output.WriteLine(string.Empty);
                this.Output.Write(type.GetVirtualTableName());
                var virtualTable = type.GetVirtualTable(this);
                virtualTable.WriteTableOfMethods(this, type, 0, baseTypeSize);

                foreach (var methodInVirtualTable in virtualTable)
                {
                }

                index++;
            }

            foreach (var @interface in type.SelectAllTopAndAllNotFirstChildrenInterfaces().Distinct())
            {
                var current = type;
                IType typeContainingInterface = null;
                while (current != null && current.GetAllInterfaces().Contains(@interface))
                {
                    typeContainingInterface = current;
                    current = current.BaseType;
                }

                var baseTypeSizeOfTypeContainingInterface = typeContainingInterface.BaseType != null
                    ? typeContainingInterface.BaseType.GetTypeSize(this)
                    : 0;
                var interfaceIndex = FindInterfaceIndexes(typeContainingInterface, @interface, index).Sum();

                this.Output.WriteLine(string.Empty);
                this.Output.Write(type.GetVirtualInterfaceTableName(@interface));
                var virtualInterfaceTable = type.GetVirtualInterfaceTable(@interface, this);
                virtualInterfaceTable.WriteTableOfMethods(
                    this,
                    type,
                    interfaceIndex,
                    baseTypeSizeOfTypeContainingInterface);

                foreach (var methodInVirtualTableOfInterface in virtualInterfaceTable)
                {
                }
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
                if (!opCode.JumpProcessed)
                {
                    this.WriteLabel(writer, string.Concat("a", opCode.AddressStart));
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteLeave(CIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.WriteLine("; Leave ");
            if (this.tryScopes.Count > 0)
            {
                var tryClause = this.tryScopes.Peek();
                var finallyClause =
                    tryClause.Catches.FirstOrDefault(c => c.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally));
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
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="argIndex">
        /// </param>
        /// <param name="asReference">
        /// </param>
        private void WriteLlvmArgVarAccess(
            CIndentedTextWriter writer,
            int index,
            int argIndex,
            bool asReference = false)
        {
            Parameters[index].ParameterType.WriteTypePrefix(this);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(this.GetArgVarName(index, argIndex));

            // TODO: optional do we need to calculate it propertly?
            writer.Write(", align " + PointerSize);
        }

        private FullyDefinedReference WriteLoadingArgumentsForMain(
            IMethod currentMethod,
            IGenericContext currentGenericContext)
        {
            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            MainGen.GetLoadingArgumentsMethodBody(
                MainMethod.ReturnType.IsVoid(),
                this,
                out code,
                out tokenResolutions,
                out locals,
                out parameters);

            var mainEntry = new SynthesizedStaticMethod(
                "main",
                MainMethod.DeclaringType,
                MainMethod.ReturnType,
                MainMethod.GetParameters());

            var constructedMethod = MethodBodyBank.GetMethodDecorator(
                mainEntry,
                code,
                tokenResolutions,
                locals,
                parameters);

            // actual write
            var opCodes = this.WriteCustomMethodPart(constructedMethod, currentGenericContext);
            return opCodes.First(op => op.Any(Code.Newarr)).Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="locals">
        /// </param>
        private void WriteLocalVariableDeclarations(IEnumerable<ILocalVariable> locals)
        {
            foreach (var local in locals)
            {
                this.GetEffectiveLocalType(local).WriteTypePrefix(this);
                this.Output.Write(" ");
                this.Output.Write(this.GetLocalVarName(local.LocalIndex));
                this.Output.WriteLine(";");
            }
        }

        /// <summary>
        /// </summary>
        private void WriteMainFunction()
        {
            var isVoid = MainMethod.ReturnType.IsVoid();
            var hasParameters = MainMethod.GetParameters().Any();

            if (isVoid)
            {
                var environmentType = this.ResolveType("System.Environment");
                var setExitCode = environmentType.GetMethodByName("set_ExitCode", this);
                var getExitCode = environmentType.GetMethodByName("get_ExitCode", this);
                this.forwardMethodDeclarationWritten.Add(new MethodKey(setExitCode, null));
                this.forwardMethodDeclarationWritten.Add(new MethodKey(getExitCode, null));
                this.WriteMethodForwardDeclaration(setExitCode, null);
                this.Output.WriteLine(";");
                this.WriteMethodForwardDeclaration(getExitCode, null);
                this.Output.WriteLine(";");
                this.Output.WriteLine(string.Empty);
            }

            if (!hasParameters)
            {
                this.Output.Write("{0}i32 main()", declarationPrefix);
            }
            else
            {
                this.Output.Write("{0}i32 main(i32 value_0, char** value_1)", declarationPrefix);
            }

            this.Output.WriteLine(" {");

            this.Output.Indent++;

            // create locals and args
            if (hasParameters)
            {
                this.System.System_String.ToArrayType(1).WriteTypePrefix(this);
                this.Output.WriteLine(" local0;");
            }

            this.Output.WriteLine("i32 local1;");

            if (!this.Gctors)
            {
                this.WriteCallGctors();
            }

            ////var result = hasParameters ? this.WriteLoadingArgumentsForMain(MainMethod, null) : null;

            if (isVoid)
            {
                var method = "Void_System_Environment_set_ExitCodeFInt32N";
                this.Output.WriteLine("{0}(0);", method);
            }

            if (!isVoid)
            {
                this.Output.Write("local1 = ");
            }

            this.WriteMethodDefinitionName(this.Output, MainMethod);
            this.Output.Write("(");

            if (hasParameters)
            {
                ////    result.Type.WriteTypePrefix(this);
                ////    this.Output.Write(" ");
                ////    this.WriteResult(result);
                this.Output.Write("0");
            }

            this.Output.WriteLine(");");

            if (isVoid)
            {
                var method = "Int32_System_Environment_get_ExitCodeFN";
                this.Output.WriteLine("return {0}();", method);
            }
            else
            {
                this.Output.WriteLine("return local1;");
            }

            this.Output.Indent--;
            this.Output.WriteLine("}");
        }

        /// <summary>
        /// </summary>
        /// <param name="endPart">
        /// </param>
        private void WriteMethodBody(IMethod method)
        {
            var rest = PrepareWritingMethodBody();

            foreach (var opCodePart in rest)
            {
                this.ActualWrite(this.Output, opCodePart, true);
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

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="operator">
        /// </param>
        private void WriteOverflowWithThrow(CIndentedTextWriter writer, OpCodePart opCode, string @operator)
        {
            this.BinaryOper(
                writer,
                opCode,
                string.Concat("call { %R, i1 } @llvm.", @operator, ".with.overflow.%R("),
                OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes | OperandOptions.Template |
                OperandOptions.DetectAndWriteTypeInSecondOperand);
            writer.WriteLine(")");

            var result = opCode.Result;

            var testResult = this.SetResultNumber(opCode, this.System.System_Boolean);
            writer.Write("extractvalue { ");
            result.Type.WriteTypePrefix(this);
            writer.Write(", i1 } ");
            this.WriteResult(result);
            writer.WriteLine(", 1");

            var returnValue = this.SetResultNumber(opCode, result.Type);
            writer.Write("extractvalue { ");
            result.Type.WriteTypePrefix(this);
            writer.Write(", i1 } ");
            this.WriteResult(result);
            writer.WriteLine(", 0");

            // throw exception
            this.WriteBranchSwitchToThrowOrPass(
                this,
                opCode,
                testResult,
                "System.OverflowException",
                "arithm_overflow",
                "zero");
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
            CIndentedTextWriter writer,
            FullyDefinedReference result,
            OpCodePart startOpCode,
            OpCodePart endOpCode,
            string label = "a",
            int startAddress = 0,
            int stopAddress = 0)
        {
            var customLabel = this.FindCustomLabel(startOpCode, endOpCode, startAddress, stopAddress);
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
            var stubFunc = (this.Stubs && NoBody && !method.IsAbstract && !method.IsSkipped() && !method.IsDelegateFunctionBody());
            if (stubFunc)
            {
                this.DefaultStub(method);
            }

            if (!NoBody)
            {
                this.WriteExceptionEnvironment(method);

                this.Output.Indent--;
                this.Output.EndMethodBody();

                this.Output.WriteLine("}");
            }

            this.Output.WriteLine(string.Empty);

            this.WriteConstData();
        }

        /// <summary>
        /// </summary>
        private void WriteRequiredDeclarations()
        {
            if (MainMethod != null && !this.Gctors)
            {
                this.Output.WriteLine(string.Empty);
                this.WriteCallGctorsDeclarations();
            }

            /*
            if (this.typeRttiPointerDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (
                    var rttiPointerDecl in
                        this.typeRttiPointerDeclRequired.Where(rdp => !this.processedRttiPointerTypes.Contains(rdp)))
                {
#if !INLINE_RTTI_INFO
                    if (!rttiPointerDecl.IsArray && !rttiPointerDecl.IsGenericType && !rttiPointerDecl.IsGenericTypeDefinition)
                    {
                        rttiPointerDecl.WriteRttiPointerClassInfoExternalDeclaration(this.Output);
                    }
                    else
                    {
                        rttiPointerDecl.WriteRttiPointerClassName(this.Output);
                        rttiPointerDecl.WriteRttiPointerClassInfo(this.Output);
                        this.AddRequiredRttiDeclaration(rttiPointerDecl);
                    }
#else
                    rttiPointerDecl.WriteRttiPointerClassName(this.Output);
                    rttiPointerDecl.WriteRttiPointerClassInfo(this.Output);
                    this.AddRequiredRttiDeclaration(rttiPointerDecl);
#endif
                    this.Output.WriteLine(string.Empty);
                }
            }

            if (this.typeRttiDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);

                bool any;
                do
                {
                    any = false;
                    foreach (
                        var rttiDecl in
                            this.typeRttiDeclRequired.ToList().Where(rd => !this.processedRttiTypes.Contains(rd)))
                    {
#if !INLINE_RTTI_INFO
                        if (!rttiDecl.IsArray && !rttiDecl.IsGenericType && !rttiDecl.IsGenericTypeDefinition)
                        {
                            rttiDecl.WriteRttiClassInfoExternalDeclaration(this.Output);
                        }
                        else
                        {
                            rttiDecl.WriteRttiClassName(this.Output);
                            rttiDecl.WriteRttiClassInfo(this);
                        }
#else
                        rttiDecl.WriteRttiClassName(this.Output);
                        rttiDecl.WriteRttiClassInfo(this);
#endif
                        this.Output.WriteLine(string.Empty);

                        this.processedRttiTypes.Add(rttiDecl);
                        any = true;
                    }
                } while (any);
            }

            if (this.typeVirtualTablesRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (
                    var typeVirtualTableRequired in
                        this.typeVirtualTablesRequired.Where(rdp => !this.processedVirtualTablesRequired.Contains(rdp)))
                {
                    this.WriteVirtualTables(typeVirtualTableRequired, true);
                    this.processedVirtualTablesRequired.Add(typeVirtualTableRequired);
                    this.Output.WriteLine(string.Empty);
                }
            }

            if (this.typeTokenRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);

                foreach (var loadToken in this.typeTokenRequired)
                {
                    // add type infrastructure
                    loadToken.WriteTypeStorageStaticField(this);
                    loadToken.WriteGetTypeStaticMethod(this);
                }
            }
            */
        }

        private void WriteMethodForwardDeclaration(IMethod methodDecl, IType ownerOfExplicitInterface)
        {
            var ctor = methodDecl as IConstructor;
            if (ctor != null)
            {
                ReadMethodInfo(ctor, null);
                // extern "c"
                this.Output.Write(this.declarationPrefix);
                this.WriteMethodReturnType(this.Output, ctor);
                this.WriteMethodDefinitionName(this.Output, ctor);
                this.WriteMethodParamsDef(
                    this.Output,
                    ctor,
                    this.HasMethodThis,
                    ThisType,
                    this.System.System_Void);
                return;
            }

            var method = methodDecl;
            if (method != null)
            {
                ReadMethodInfo(method, null);
                // extern "c"
                this.Output.Write(this.declarationPrefix);
                this.WriteMethodReturnType(this.Output, method);
                this.WriteMethodDefinitionName(this.Output, method, ownerOfExplicitInterface);
                this.WriteMethodParamsDef(
                    this.Output,
                    method,
                    HasMethodThis,
                    ownerOfExplicitInterface ?? ThisType,
                    method.ReturnType);
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
                this.forwardStaticDeclarationWritten.Add(field);
                this.Output.Write(declarationPrefix);
            }

            field.FieldType.WriteTypePrefix(this, false);

            // TODO: do not forget static generic
            this.Output.Write(" ");
            this.Output.Write(field.GetFullName().CleanUpName());

            this.Output.WriteLine(";");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteStaticFieldDeclarations(IType type)
        {
            if (type.IsEnum)
            {
                return;
            }

            foreach (
                var field in
                    Logic.IlReader.Fields(type, this)
                         .Where(f => f.IsStatic && (!f.IsConst || f.FieldType.IsStructureType())))
            {
                this.WriteStaticFieldDeclaration(field);
            }
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
        private void WriteTryEnds(CIndentedTextWriter writer, OpCodePart opCode)
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
#if EXTRA_VALIDATION
            Debug.Assert(!type.IsGenericTypeDefinition);
#endif

            this.Output.Write("{0}struct ", declarationPrefix);
            type.ToClass().WriteTypeName(this.Output, false);
            this.Output.Write(" ");
        }

        private void WriteTypeRequiredDefinitions(IType type)
        {
            foreach (
                var requiredDeclarationType in
                    Il2Converter.GetRequiredDeclarationTypes(type)
                                .Where(requiredType => !requiredType.IsGenericTypeDefinition)
                                .Where(requiredDeclarationType => this.forwardTypeDeclarationWritten.Add(requiredDeclarationType)))
            {
                this.WriteTypeDeclarationStart(requiredDeclarationType);
                this.Output.WriteLine(";");
            }

            foreach (var requiredType in Il2Converter.GetRequiredDefinitionTypes(type).Where(requiredType => !requiredType.IsGenericTypeDefinition))
            {
                this.WriteTypeDefinitionIfNotWrittenYet(requiredType);
            }
        }

        private void WriteMethodRequiredDeclatations()
        {
            // const strings
            foreach (var usedString in IlReader.UsedStrings)
            {
                WriteUnicodeString(usedString);
            }

            if (IlReader.UsedStrings.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
            }

            var any = false;
            // methods
            foreach (
                var requiredDeclarationMethod in
                    IlReader.CalledMethods
                        .Where(
                            requiredMethodDeclaration =>
                                this.forwardMethodDeclarationWritten.Add(new MethodKey(requiredMethodDeclaration, null))))
            {
                any = true;
                WriteMethodForwardDeclaration(requiredDeclarationMethod, null);
                this.Output.WriteLine(";");
            }

            if (any)
            {
                this.Output.WriteLine(string.Empty);
            }

            any = false;
            // static fields
            foreach (
                var requiredStaticFieldDeclaration in
                    IlReader.StaticFields
                        .Where(
                            requiredStaticFieldDeclaration =>
                                this.forwardStaticDeclarationWritten.Add(requiredStaticFieldDeclaration)))
            {
                any = true;
                this.WriteStaticFieldDeclaration(requiredStaticFieldDeclaration, true);
                this.Output.WriteLine(";");
            }

            if (any)
            {
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteTypeDefinitionIfNotWrittenYet(IType type)
        {
            if (this.IsTypeDefinitionWritten(type))
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
            this.Output.Write(declarationPrefix);
            this.Output.Write(
                "const struct {1} _s{0}_ = {3} {2}",
                pair.Key,
                this.GetStringTypeHeader(pair.Value.Length + 1),
                this.GetStringValuesHeader(pair.Value.Length + 1, pair.Value.Length),
                "{", "}");

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

            this.Output.WriteLine("0 {0} {0};", '}');
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
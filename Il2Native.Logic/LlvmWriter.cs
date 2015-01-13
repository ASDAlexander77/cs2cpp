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
    using CodeParts;
    using DebugInfo;
    using Exceptions;
    using Gencode;
    using Gencode.SynthesizedMethods;

    using Il2Native.Logic.Gencode.InlineMethods;

    using PEAssemblyReader;
    using Properties;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class LlvmWriter : BaseWriter, ICodeWriter
    {
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
        public static int PointerSize = 4;

        /// <summary>
        /// </summary>
        private static int methodNumberIncremental;

        /// <summary>
        /// </summary>
        public Stack<CatchOfFinallyClause> catchScopes = new Stack<CatchOfFinallyClause>();

        public DebugInfoGenerator debugInfoGenerator;

        /// <summary>
        /// </summary>
        public bool landingPadVariablesAreWritten;

        /// <summary>
        /// </summary>
        public readonly HashSet<IMethod> methodsHaveDefinition = new HashSet<IMethod>();

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
        private int blockJumpAddressIncremental;

        /// <summary>
        /// </summary>
        private int bytesIndexIncremental;

        /// <summary>
        /// </summary>
        private readonly IDictionary<int, byte[]> bytesStorage = new SortedDictionary<int, byte[]>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> indexByFieldInfo = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private readonly HashSet<MethodKey> methodDeclRequired = new HashSet<MethodKey>();

        private readonly IDictionary<int, IMethod> methodsByToken = new SortedDictionary<int, IMethod>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> poisitionByFieldInfo = new SortedDictionary<string, int>();

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
        public LlvmWriter(string fileName, string sourceFilePath, string pdbFilePath, string[] args)
        {
            var extension = Path.GetExtension(fileName);
            var outputFile = extension != null && extension.Equals(string.Empty) ? fileName + ".ll" : fileName;
            this.Output = new LlvmIndentedTextWriter(new StreamWriter(outputFile));

            // custom settings
            var targetArg = args != null ? args.FirstOrDefault(a => a.StartsWith("target:")) : null;
            this.Target = targetArg != null ? targetArg.Substring("target:".Length) : null;
            this.Gc = args == null || !args.Contains("gc-");
            this.Gctors = args == null || !args.Contains("gctors-");
            this.IsLlvm35 = args != null && args.Contains("llvm35");
            this.IsLlvm34OrLower = !this.IsLlvm35 && args != null && args.Contains("llvm34");
            this.DebugInfo = args != null && args.Contains("debug");
            if (this.DebugInfo)
            {
                this.debugInfoGenerator = new DebugInfoGenerator(pdbFilePath, sourceFilePath);
            }

            // prefefined settings
            if (args != null && args.Contains("android"))
            {
                this.Target = this.Target ?? "armv7-none-linux-androideabi";
                this.Gctors = false;
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

            this.DataLayout = this.DataLayout ??
                              @"e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32";
            this.Target = this.Target ?? "i686-w64-mingw32";
        }

        /// <summary>
        /// </summary>
        public IEnumerable<string> AllReference { get; private set; }

        /// <summary>
        /// </summary>
        public string DataLayout { get; private set; }

        /// <summary>
        /// </summary>
        public bool DebugInfo { get; private set; }

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
        public bool IsLlvm34OrLower { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsLlvm35 { get; private set; }

        public IDictionary<int, IMethod> MethodsByToken
        {
            get { return this.methodsByToken; }
        }

        /// <summary>
        /// </summary>
        public LlvmIndentedTextWriter Output { get; private set; }

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
            AddOpCode(opCode);
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
            var baseType = ThisType.BaseType;

            this.CheckIfExternalDeclarationIsRequired(baseType);

            this.Output.WriteLine("{");
            this.Output.Indent++;

            // put virtual root table if type has no any base with virtual types
            if (ThisType.IsRootInterface())
            {
                this.Output.WriteLine("i32 (...)**");
            }
            else if (ThisType.IsRootOfVirtualTable())
            {
                this.Output.WriteLine("i32 (...)**");
            }

            if (baseType != null)
            {
                baseType.WriteTypeWithoutModifiers(this.Output);
            }

            var index = 0;
            foreach (var @interface in ThisType.GetInterfacesExcludingBaseAllInterfaces())
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
        /// <param name="ctor">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteConstructorEnd(IConstructor ctor, IGenericContext genericContext)
        {
            this.WriteMethodBody(ctor);
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

            Debug.Assert(!ctor.IsGenericMethodDefinition);
            Debug.Assert(!ctor.DeclaringType.IsGenericTypeDefinition);

            this.processedMethods.Add(ctor);
            if (ctor.Token.HasValue)
            {
                this.methodsByToken[ctor.Token.Value] = ctor;
            }

            ReadMethodInfo(ctor, genericContext);

            var isDelegateBodyFunctions = ctor.IsDelegateFunctionBody();
            if ((ctor.IsAbstract || NoBody) && !isDelegateBodyFunctions)
            {
                this.Output.Write("declare ");
            }
            else
            {
                this.Output.Write("define ");
                if (ThisType.IsGenericType)
                {
                    this.Output.Write("linkonce_odr ");
                }
            }

            this.Output.Write("void ");

            this.WriteMethodDefinitionName(this.Output, ctor);
            if (ctor.IsStatic)
            {
                StaticConstructors.Add(ctor);
            }

            this.WriteMethodParamsDef(this.Output, ctor, HasMethodThis, ThisType, ResolveType("System.Void"));

            this.WriteMethodNumber();

            // write local declarations
            var methodBase = ctor.ResolveMethodBody(genericContext);
            if (methodBase.HasBody)
            {
                if (this.DebugInfo && ctor.Token.HasValue)
                {
                    this.debugInfoGenerator.GenerateFunction(ctor.Token.Value);
                    // to find first debug line of method
                    this.ReadDbgLine(OpCodePart.CreateNop);
                }

                this.Output.WriteLine(" {");
                this.Output.Indent++;
                this.WriteLocalVariableDeclarations(methodBase.LocalVariables);
                this.WriteArgumentCopyDeclarations(ctor, HasMethodThis);

                this.Output.StartMethodBody();
            }
            else if (isDelegateBodyFunctions)
            {
                this.WriteDelegateFunctionBody(ctor);
            }

            methodNumberIncremental++;
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

            if (this.DebugInfo)
            {
                this.debugInfoGenerator.WriteTo(this.Output);
            }
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

            this.Output.WriteLine(',');

            this.WriteFieldType(field);
        }

        public void WriteFieldType(IField field)
        {
            if (field.IsFixed)
            {
                this.Output.Write("[ {0}", field.FixedSize);
                this.Output.Write(" x ");
                this.WriteFieldType(field.FieldType.ToDereferencedType());
                this.Output.Write(" ]");
            }
            else
            {
                this.WriteFieldType(field.FieldType);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fieldType">
        /// </param>
        public void WriteFieldType(IType fieldType)
        {
            Debug.Assert(!fieldType.IsGenericParameter);

            this.CheckIfExternalDeclarationIsRequired(fieldType);

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
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteMethodEnd(IMethod method, IGenericContext genericContext)
        {
            this.WriteMethodBody(method);
            this.WritePostMethodEnd(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void WriteMethodStart(IMethod method, IGenericContext genericContext)
        {
            this.StartProcess();

            Debug.Assert(!method.IsGenericMethodDefinition);
            Debug.Assert(!method.DeclaringType.IsGenericTypeDefinition);

            if (method.Token.HasValue)
            {
                this.methodsByToken[method.Token.Value] = method;
            }

            ReadMethodInfo(method, genericContext);

            if (method.IsUnmanaged && this.processedMethods.Any(m => m.Name == method.Name))
            {
                return;
            }

            this.processedMethods.Add(method);

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

            var isDelegateBodyFunctions = method.IsDelegateFunctionBody();
            if ((method.IsAbstract || NoBody) && !isDelegateBodyFunctions)
            {
                if (!method.IsUnmanagedMethodReference)
                {
                    if (this.methodsHaveDefinition.Contains(method))
                    {
                        return;
                    }

                    this.Output.Write("declare ");
                }
                else
                {
                    this.WriteMethodDefinitionName(this.Output, method);
                    this.Output.Write(" = ");
                }

                if (method.IsUnmanagedDllImport)
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

                if (!method.IsUnmanagedMethodReference && method.DllImportData != null &&
                    method.DllImportData.CallingConvention == CallingConvention.StdCall)
                {
                    this.Output.Write("x86_stdcallcc ");
                }
            }
            else
            {
                this.Output.Write("define ");
                if (ThisType.IsGenericType || method.IsGenericMethod)
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
                    this.Output,
                    method,
                    HasMethodThis,
                    ThisType,
                    method.ReturnType,
                    method.IsUnmanagedMethodReference,
                    method.CallingConvention.HasFlag(CallingConventions.VarArgs));
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
            if (methodBodyBytes.HasBody)
            {
                if (this.DebugInfo && method.Token.HasValue)
                {
                    this.debugInfoGenerator.GenerateFunction(method.Token.Value);
                    // to find first debug line of method
                    this.ReadDbgLine(OpCodePart.CreateNop);
                }

                this.Output.WriteLine(" {");
                this.Output.Indent++;
                this.WriteLocalVariableDeclarations(methodBodyBytes.LocalVariables);
                this.WriteArgumentCopyDeclarations(method, HasMethodThis);

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
        /// <param name="type">
        /// </param>
        public void WritePostDeclarations(IType type)
        {
            if (!type.IsGenericType && AssemblyQualifiedName != type.AssemblyQualifiedName)
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

            // object oriented methods
            if (!type.IsVoid())
            {
                type.WriteNewObjectMethod(this);
            }

            type.WriteInitObjectMethod(this);
            type.WriteGetTypeStaticMethod(this);

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

            if (isEnum)
            {
                normalType.WriteGetHashCodeMethodForEnum(this);
            }

            var customGetType = new SynthesizedGetTypeMethod(type, this);
            if (normalType.BaseType == null ||
                !IlReader.Methods(normalType).Any(m => m.IsMatchingOverride(customGetType)))
            {
                normalType.WriteGetTypeMethod(this);
            }
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
            foreach (var requiredType in RequiredTypesForBody.ToArray())
            {
                this.WriteTypeDefinitionIfNotWrittenYet(requiredType);
            }

            // clear types for next type
            RequiredTypesForBody.Clear();
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
        public void WriteStart(string moduleName, string assemblyName, bool isCoreLib, IEnumerable<string> allReference)
        {
            AssemblyQualifiedName = assemblyName;
            this.IsCoreLib = isCoreLib;
            this.AllReference = allReference;

            this.Output.WriteLine("target datalayout = \"{0}\"", this.DataLayout);
            this.Output.WriteLine("target triple = \"{0}\"", this.Target);
            this.Output.WriteLine(string.Empty);

            if (this.Gctors)
            {
                // Global ctors
                this.Output.WriteLine(
                    "@llvm.global_ctors = appending global [1 x { i32, void ()* }] [{ i32, void ()* } { i32 65535, void ()* "
                    + this.GetGlobalConstructorsFunctionName() + " }]");
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

            StaticConstructors.Clear();
            VirtualTableGen.Clear();
            TypeGen.Clear();

            if (this.DebugInfo)
            {
                this.Output.WriteLine("declare void @llvm.dbg.declare(metadata, metadata, metadata) #0");
                this.Output.WriteLine(string.Empty);

                if (!this.debugInfoGenerator.StartGenerating(this))
                {
                    this.DebugInfo = false;
                }
            }
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

            // get all required types for type definition
            var requiredTypes = new List<IType>();
            ISet<IType> processedAlready = new HashSet<IType>();
            Il2Converter.ProcessRequiredITypesForITypes(
                new[] { type },
                new HashSet<IType>(),
                requiredTypes,
                null,
                null,
                processedAlready);
            foreach (var requiredType in requiredTypes.Where(requiredType => !requiredType.IsGenericTypeDefinition))
            {
                this.WriteTypeDefinitionIfNotWrittenYet(requiredType);
            }

            var interfacesList = type.GetInterfaces();
            foreach (var @interface in interfacesList)
            {
                this.WriteTypeDefinitionIfNotWrittenYet(@interface);
            }

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
        public void ActualWrite(LlvmIndentedTextWriter writer, OpCodePart opCode, bool firstLevel = false)
        {
            if (firstLevel)
            {
                this.WriteLabels(writer, opCode);
            }

            this.WriteTryBegins(writer, opCode);
            this.WriteCatchBegins(writer, opCode);

            // process Phi Nodes
            if (opCode.AlternativeValues != null)
            {
                this.WritePhi(writer, opCode);
            }

            this.ActualWriteOpCode(writer, opCode);

            this.AdjustResultTypeToOutgoingType(opCode);

            this.WriteCatchFinnallyEnd(writer, opCode);

            this.WriteCatchFinnallyCleanUpEnd(opCode);
            this.WriteTryEnds(writer, opCode);
            this.WriteExceptionHandlersProlog(writer, opCode);

            opCode.ResultAtExit = opCode.Result;
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
                        ? new ConstValue(null, ResolveType("System.Void").ToPointerType())
                        : new ConstValue(0, ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4_1:
                    opCode.Result = new ConstValue(1, ResolveType("System.Int32"));
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
                        ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4_M1:
                    opCode.Result = new ConstValue(-1, ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4:
                    var opCodeInt32 = opCode as OpCodeInt32Part;
                    opCode.Result = new ConstValue(opCodeInt32.Operand, ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I4_S:
                    opCodeInt32 = opCode as OpCodeInt32Part;
                    opCode.Result = new ConstValue(
                        opCodeInt32.Operand > 127 ? -(256 - opCodeInt32.Operand) : opCodeInt32.Operand,
                        ResolveType("System.Int32"));
                    break;
                case Code.Ldc_I8:
                    var opCodeInt64 = opCode as OpCodeInt64Part;
                    opCode.Result = new ConstValue(opCodeInt64.Operand, ResolveType("System.Int64"));
                    break;
                case Code.Ldc_R4:
                    var opCodeSingle = opCode as OpCodeSinglePart;

                    if (float.IsPositiveInfinity(opCodeSingle.Operand))
                    {
                        opCode.Result = new ConstValue("0x7FF0000000000000", ResolveType("System.Single"));
                    }
                    else if (float.IsNegativeInfinity(opCodeSingle.Operand))
                    {
                        opCode.Result = new ConstValue("0xFFF0000000000000", ResolveType("System.Single"));
                    }
                    else
                    {
                        var g = BitConverter.DoubleToInt64Bits(opCodeSingle.Operand);
                        opCode.Result = new ConstValue(
                            string.Format("0x{0}", g.ToString("X")),
                            ResolveType("System.Single"));
                    }

                    break;
                case Code.Ldc_R8:
                    var opCodeDouble = opCode as OpCodeDoublePart;
                    if (double.IsPositiveInfinity(opCodeDouble.Operand))
                    {
                        opCode.Result = new ConstValue("0x7FF0000000000000", ResolveType("System.Double"));
                    }
                    else if (double.IsNegativeInfinity(opCodeDouble.Operand))
                    {
                        opCode.Result = new ConstValue("0xFFF0000000000000", ResolveType("System.Double"));
                    }
                    else
                    {
                        var g = BitConverter.DoubleToInt64Bits(opCodeDouble.Operand);
                        opCode.Result = new ConstValue(
                            string.Format("0x{0}", g.ToString("X")),
                            ResolveType("System.Double"));
                    }

                    break;
                case Code.Ldstr:
                    var opCodeString = opCode as OpCodeStringPart;
                    var stringType = ResolveType("System.String");
                    var charType = ResolveType("System.Char");
                    var charArrayType = charType.ToArrayType(1);
                    var stringIndex = this.GetStringIndex(opCodeString.Operand);
                    var firstParameterValue =
                        new FullyDefinedReference(
                            this.GetArrayTypeReference(
                                string.Format("@.s{0}", stringIndex),
                                charType,
                                opCodeString.Operand.Length + 1),
                            charArrayType);

                    this.WriteNewWithCallingConstructor(opCode, stringType, charArrayType, firstParameterValue);

                    break;
                case Code.Ldnull:
                    opCode.Result = new ConstValue(null, ResolveType("System.Void").ToPointerType());
                    break;

                case Code.Arglist:

                    // TODO: it really does not do anything. you need to use VA_START, VA_END, VA_ARG in ArgInterator class
                    opCode.Result = new ConstValue("undef", ResolveType("System.Object"));
                    break;

                case Code.Ldtoken:

                    // TODO: finish loading Token  
                    ////var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    ////var data = opCodeFieldInfoPart.Operand.GetFieldRVAData();
                    opCode.Result = new ConstValue("undef", ResolveType("System.Object"));

                    break;
                case Code.Localloc:

                    this.UnaryOper(
                        writer,
                        opCode,
                        "alloca i8, ",
                        this.GetIntTypeByByteSize(PointerSize),
                        ResolveType("System.Byte").ToPointerType(),
                        OperandOptions.AdjustIntTypes | OperandOptions.CastPointersToBytePointer);
                    writer.Write(", align 1");

                    writer.WriteLine(string.Empty);

                    this.WriteMemSet(opCode.Result, opCode.OpCodeOperands[0].Result, 1);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Ldfld:

                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    // we wait when opCode.DestinationName is set;
                    this.WriteFieldAccess(writer, opCodeFieldInfoPart);
                    writer.WriteLine(string.Empty);

                    if (!opCodeFieldInfoPart.Operand.FieldType.IsStructureType())
                    {
                        var memberAccessResultNumber = opCode.Result;
                        opCode.Result = null;
                        this.WriteLlvmLoad(opCode, memberAccessResultNumber.Type, memberAccessResultNumber);

                        this.WriteDbgLine(opCode);
                    }

                    break;
                case Code.Ldflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    if (!opCodeFieldInfoPart.Operand.HasFixedElementField)
                    {
                        this.WriteFieldAccess(writer, opCodeFieldInfoPart);
                        var fieldLoadResult = opCodeFieldInfoPart.Result;

                        // convert return type of field to pointer of a field type
                        opCodeFieldInfoPart.Result = fieldLoadResult.ToPointerType();
                    }
                    else
                    {
                        writer = this.Output;
                        this.WriteSetResultNumber(opCodeFieldInfoPart, opCodeFieldInfoPart.Operand.FieldType.ToPointerType());
                        writer.Write("bitcast ");
                        this.WriteFieldType((opCodeFieldInfoPart.OpCodeOperands[0] as OpCodeFieldInfoPart).Operand);
                        writer.Write("* ");
                        this.WriteResult(opCodeFieldInfoPart.OpCodeOperands[0].Result);
                        writer.Write(" to ");
                        opCodeFieldInfoPart.Operand.FieldType.ToPointerType().WriteTypePrefix(writer, true);
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

                    var destinationName = string.Concat("@\"", opCodeFieldInfoPart.Operand.GetFullName(), '"');
                    var reference = new FullyDefinedReference(destinationName, opCodeFieldInfoPart.Operand.FieldType);
                    if (!operandType.IsStructureType())
                    {
                        this.WriteLlvmLoad(opCode, operandType, reference);

                        this.WriteDbgLine(opCode);
                    }
                    else
                    {
                        opCode.Result = reference;
                    }

                    this.CheckIfStaticFieldExternalDeclarationIsRequired(opCodeFieldInfoPart.Operand);

                    break;
                case Code.Ldsflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    opCodeFieldInfoPart.Result = new FullyDefinedReference(
                        string.Concat("@\"", opCodeFieldInfoPart.Operand.GetFullName(), '"'),
                        opCodeFieldInfoPart.Operand.FieldType.ToPointerType());

                    this.CheckIfStaticFieldExternalDeclarationIsRequired(opCodeFieldInfoPart.Operand);

                    break;
                case Code.Stfld:

                    this.FieldAccessAndSaveToField(opCode as OpCodeFieldInfoPart);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Stsfld:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    destinationName = string.Concat("@\"", opCodeFieldInfoPart.Operand.GetFullName(), '"');
                    operandType = opCodeFieldInfoPart.Operand.FieldType;
                    reference = new FullyDefinedReference(destinationName, operandType);

                    if (opCodeFieldInfoPart.Operand.FieldType.IsStructureType())
                    {
                        opCode.Result = reference;
                        this.WriteLlvmLoad(opCode, operandType, opCode.OpCodeOperands[0].Result);
                    }
                    else
                    {
                        this.WriteLlvmSave(opCode, operandType, 0, reference);
                    }

                    this.WriteDbgLine(opCode);

                    this.CheckIfStaticFieldExternalDeclarationIsRequired(opCodeFieldInfoPart.Operand);

                    break;

                case Code.Ldobj:

                    var opCodeTypePart = opCode as OpCodeTypePart;
                    var loadValueFromAddress = !opCodeTypePart.Operand.IsStructureType();
                    if (loadValueFromAddress)
                    {
                        this.WriteLlvmLoad(opCode, opCodeTypePart.Operand, opCode.OpCodeOperands[0].Result);

                        this.WriteDbgLine(opCode);
                    }
                    else
                    {
                        if (opCode.OpCodeOperands[0].Result.Type.IntTypeBitSize() == PointerSize * 8)
                        {
                            // using int as intptr
                            this.AdjustIntConvertableTypes(
                                writer,
                                opCode.OpCodeOperands[0],
                                opCodeTypePart.Operand.ToPointerType());
                            opCode.Result = opCode.OpCodeOperands[0].Result;
                        }
                        else
                        {
                            opCode.Result = opCode.OpCodeOperands[0].Result.ToDereferencedType();
                            if (opCode.Result.Type.IsVoid())
                            {
                                // in case we receive void* to load struct
                                opCode.Result = opCode.OpCodeOperands[0].Result;
                            }

                            Debug.Assert(!opCode.Result.Type.IsVoid());
                        }
                    }

                    break;

                case Code.Stobj:
                    this.SaveObject(opCode, 1, 0);

                    this.WriteDbgLine(opCode);

                    break;

                case Code.Ldlen:
                    this.WriteArrayGetLength(opCode);

                    this.WriteDbgLine(opCode);

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

                    this.WriteDbgLine(opCode);

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

                    this.WriteDbgLine(opCode);

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

                    this.WriteDbgLine(opCode);

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

                    this.WriteDbgLine(opCode);

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
                        // if we call constructor on struct we need to initialize object before
                        opCodeMethodInfoPart.Result = opCodeMethodInfoPart.OpCodeOperands[0].Result;
                        methodBase.DeclaringType.WriteCallInitObjectMethod(this, opCodeMethodInfoPart);
                        opCodeMethodInfoPart.Result = null;
                        this.Output.WriteLine(string.Empty);
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
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fadd" : "add",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Add_Ovf:
                    this.WriteOverflowWithThrow(writer, opCode, "sadd");
                    break;
                case Code.Add_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "uadd");
                    break;
                case Code.Mul:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fmul" : "mul",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Mul_Ovf:
                    this.WriteOverflowWithThrow(writer, opCode, "smul");
                    break;
                case Code.Mul_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "umul");
                    break;
                case Code.Sub:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fsub" : "sub",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Sub_Ovf:
                    this.WriteOverflowWithThrow(writer, opCode, "ssub");
                    break;
                case Code.Sub_Ovf_Un:
                    this.WriteOverflowWithThrow(writer, opCode, "usub");
                    break;
                case Code.Div:
                case Code.Div_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    var isUnsigned = opCode.IsUnsigned() || opCode.Any(Code.Div_Un);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fdiv" : isUnsigned ? "udiv" : "sdiv",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Rem:
                case Code.Rem_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    isUnsigned = opCode.IsUnsigned() || opCode.Any(Code.Rem_Un);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "frem" : isUnsigned ? "urem" : "srem",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.And:
                    this.BinaryOper(writer, opCode, "and", OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Or:
                    this.BinaryOper(writer, opCode, "or", OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Xor:
                    this.BinaryOper(writer, opCode, "xor", OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Shl:
                    this.BinaryOper(writer, opCode, "shl", OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Shr:
                case Code.Shr_Un:
                    this.BinaryOper(writer, opCode, "lshr", OperandOptions.AdjustIntTypes);

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Not:
                    var tempOper = opCode.OpCodeOperands;
                    var secondOperand = new OpCodePart(OpCodesEmit.Ldc_I4_M1, 0, 0);
                    this.ActualWrite(writer, secondOperand);
                    opCode.OpCodeOperands = new[] { tempOper[0], secondOperand };
                    this.BinaryOper(writer, opCode, "xor");
                    opCode.OpCodeOperands = tempOper;

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Neg:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);

                    tempOper = opCode.OpCodeOperands;

                    var firstOperand = OpCodePart.CreateNop;
                    firstOperand.Result = isFloatingPoint
                        ? new ConstValue("0.0", opCode.OpCodeOperands[0].Result.Type)
                        : new ConstValue(0, opCode.OpCodeOperands[0].Result.Type);
                    opCode.OpCodeOperands = new[] { firstOperand, tempOper[0] };

                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fsub" : "sub",
                        OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes);
                    opCode.OpCodeOperands = tempOper;

                    this.WriteDbgLine(opCode);

                    break;

                case Code.Dup:
                    opCode.Result = opCode.OpCodeOperands[0].Result;
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
                        opCodeTypePart.Result = opCodeTypePart.OpCodeOperands[0].Result.ToClassType();
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

                    this.WriteDbgLine(opCode);

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
                    if (localType.IsStructureType() && !localType.IsByRef)
                    {
                        firstOperand = opCode.OpCodeOperands[0];
                        if (firstOperand.Result.Type.IsPrimitiveType())
                        {
                            this.WriteLlvmSavePrimitiveIntoStructure(opCode, firstOperand.Result, destination);
                        }
                        else
                        {
                            opCode.Result = destination;
                            this.WriteLlvmLoad(opCode, localType, firstOperand.Result);
                        }
                    }
                    else
                    {
                        this.WriteLlvmSave(opCode, localType, 0, destination);
                    }

                    this.WriteDbgLine(opCode);

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

                        this.WriteDbgLine(opCode);
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
                        this.GetLocalVarName(index),
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

                    if (HasMethodThis && index == 0)
                    {
                        var thisTypeAsClass = ThisType.ToClass();
                        this.WriteLlvmLoad(
                            opCode,
                            thisTypeAsClass,
                            new FullyDefinedReference(this.GetThisName(), thisTypeAsClass),
                            true,
                            true);

                        this.WriteDbgLine(opCode);
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

                            this.WriteDbgLine(opCode);
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

                    if (HasMethodThis && index == 0)
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
                        firstOperand = opCode.OpCodeOperands[0];
                        if (firstOperand.Result.Type.IsPrimitiveType())
                        {
                            this.WriteLlvmSavePrimitiveIntoStructure(opCode, firstOperand.Result, destination);
                        }
                        else
                        {
                            opCode.Result = destination;
                            this.WriteLlvmLoad(opCode, argType, firstOperand.Result);
                        }
                    }
                    else
                    {
                        this.WriteLlvmSave(opCode, argType, 0, destination);
                    }

                    this.WriteDbgLine(opCode);

                    break;

                case Code.Ldftn:

                    opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;

                    var intPtrType = ResolveType("System.IntPtr");
                    var voidPtrType = ResolveType("System.Void").ToPointerType();
                    var convertString = this.WriteToString(
                        () =>
                        {
                            this.Output.Write("bitcast (");
                            this.WriteMethodPointerType(this.Output, opCodeMethodInfoPart.Operand);
                            this.Output.Write(" ");
                            this.Output.Write(opCodeMethodInfoPart.Operand.GetFullMethodName());
                            this.Output.Write(" to i8*)");
                        });
                    var value = new FullyDefinedReference(convertString, ResolveType("System.Byte").ToPointerType());

                    this.WriteNewWithCallingConstructor(opCode, intPtrType, voidPtrType, value);

                    this.CheckIfMethodExternalDeclarationIsRequired(opCodeMethodInfoPart.Operand);

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
                    this.WriteSetResultNumber(opCode, ResolveType("System.Byte").ToPointerType());
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

                    intPtrType = ResolveType("System.IntPtr");
                    voidPtrType = ResolveType("System.Void").ToPointerType();
                    this.WriteNewWithCallingConstructor(opCode, intPtrType, voidPtrType, methodAddressResultNumber);

                    this.CheckIfMethodExternalDeclarationIsRequired(opCodeMethodInfoPart.Operand);

                    this.WriteDbgLine(opCode);

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
                    var sign = opCode.IsUnsigned() ? "u" : "s";
                    var oper = string.Empty;
                    switch (opCode.ToCode())
                    {
                        case Code.Beq:
                        case Code.Beq_S:
                            oper = isFloatingPoint ? "fcmp ueq" : "icmp eq";
                            break;
                        case Code.Bne_Un:
                        case Code.Bne_Un_S:
                            oper = isFloatingPoint ? "fcmp une" : "icmp ne";
                            break;
                        case Code.Blt:
                        case Code.Blt_S:
                            oper = isFloatingPoint ? "fcmp ult" : "icmp {0}lt";
                            break;
                        case Code.Blt_Un:
                        case Code.Blt_Un_S:
                            oper = isFloatingPoint ? "fcmp ult" : "icmp ult";
                            break;
                        case Code.Ble:
                        case Code.Ble_S:
                            oper = isFloatingPoint ? "fcmp ule" : "icmp {0}le";
                            break;
                        case Code.Ble_Un:
                        case Code.Ble_Un_S:
                            oper = isFloatingPoint ? "fcmp ule" : "icmp ule";
                            break;
                        case Code.Bgt:
                        case Code.Bgt_S:
                            oper = isFloatingPoint ? "fcmp ugt" : "icmp {0}gt";
                            break;
                        case Code.Bgt_Un:
                        case Code.Bgt_Un_S:
                            oper = isFloatingPoint ? "fcmp ugt" : "icmp ugt";
                            break;
                        case Code.Bge:
                        case Code.Bge_S:
                            oper = isFloatingPoint ? "fcmp uge" : "icmp {0}ge";
                            break;
                        case Code.Bge_Un:
                        case Code.Bge_Un_S:
                            oper = isFloatingPoint ? "fcmp uge" : "icmp uge";
                            break;
                    }

                    this.BinaryOper(
                        writer,
                        opCode,
                        string.Format(oper, sign),
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                        ResolveType("System.Boolean"));

                    this.WriteDbgLine(opCode);

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
                    var resultOf = ResultOf(opCode.OpCodeOperands[0]);

                    var opts = OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer;
                    this.UnaryOper(writer, opCode, "icmp " + forTrue, options: opts);

                    if (resultOf.Type.IsValueType() && !resultOf.Type.UseAsClass)
                    {
                        writer.Write(", 0");
                    }
                    else
                    {
                        writer.Write(", null");
                    }

                    this.WriteDbgLine(opCode);

                    writer.WriteLine(string.Empty);

                    if (!opCode.UseAsConditionalExpression)
                    {
                        this.WriteCondBranch(writer, opCode);
                    }

                    break;
                case Code.Br:
                case Code.Br_S:

                    writer.Write(string.Concat("br label %.a", opCode.JumpAddress()));

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Leave:
                case Code.Leave_S:

                    this.WriteLeave(writer, opCode);

                    break;
                case Code.Ceq:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp ueq" : "icmp eq",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                        ResolveType("System.Boolean"));

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Clt:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    sign = opCode.IsUnsigned() ? "u" : "s";
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp ult" : string.Format("icmp {0}lt", sign),
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                        ResolveType("System.Boolean"));

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Clt_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp ult" : "icmp ult",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                        ResolveType("System.Boolean"));

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Cgt:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    sign = opCode.IsUnsigned() ? "u" : "s";
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp ugt" : string.Format("icmp {0}gt", sign),
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                        ResolveType("System.Boolean"));

                    this.WriteDbgLine(opCode);

                    break;
                case Code.Cgt_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(
                        writer,
                        opCode,
                        isFloatingPoint ? "fcmp ugt" : "icmp ugt",
                        OperandOptions.GenerateResult | OperandOptions.CastPointersToBytePointer |
                        OperandOptions.AdjustIntTypes,
                        ResolveType("System.Boolean"));

                    this.WriteDbgLine(opCode);

                    break;

                case Code.Conv_R4:
                case Code.Conv_R_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptrunc",
                        "sitofp",
                        ResolveType("System.Single"),
                        false,
                        ResolveType("System.Single"));
                    break;

                case Code.Conv_R8:
                    this.LlvmConvert(
                        opCode,
                        "fpext",
                        "sitofp",
                        ResolveType("System.Double"),
                        false,
                        ResolveType("System.Double"));
                    break;

                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptosi",
                        "trunc",
                        ResolveType("System.SByte"),
                        false,
                        ResolveType("System.SByte"),
                        ResolveType("System.Byte"));
                    break;

                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptoui",
                        "trunc",
                        ResolveType("System.Byte"),
                        false,
                        ResolveType("System.SByte"),
                        ResolveType("System.Byte"));
                    break;

                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptosi",
                        "trunc",
                        ResolveType("System.Int16"),
                        false,
                        ResolveType("System.Int16"),
                        ResolveType("System.UInt16"),
                        ResolveType("System.Char"));
                    break;

                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptoui",
                        "trunc",
                        ResolveType("System.UInt16"),
                        false,
                        ResolveType("System.Int16"),
                        ResolveType("System.UInt16"),
                        ResolveType("System.Char"));
                    break;

                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:

                    var intPtrOper = this.IntTypeRequired(opCode);
                    var nativeIntType = intPtrOper
                        ? ResolveType("System.Int32")
                        : ResolveType("System.Void").ToPointerType();

                    var requiredOutgoingType =
                        this.RequiredIncomingType(opCode.UsedBy.OpCode.Any(Code.Add) ? opCode.UsedBy.OpCode.UsedBy.OpCode : opCode.UsedBy.OpCode);
                    if (requiredOutgoingType.TypeEquals(ResolveType("System.Char").ToPointerType()) &&
                        opCode.OpCodeOperands[0].Result.Type.TypeEquals(ResolveType("System.String")))
                    {
                        // load address of first char of the string
                        this.WriteFieldAccess(writer, opCode, this.GetFieldIndex(ResolveType("System.String"), "chars"));

                        writer.WriteLine(string.Empty);
                        
                        var memberAccessResultNumber = opCode.Result;
                        opCode.Result = null;
                        this.WriteLlvmLoad(opCode, memberAccessResultNumber.Type, memberAccessResultNumber);

                        writer.WriteLine(string.Empty);
                        this.WriteBitcast(opCode, opCode.Result, ResolveType("System.Void").ToPointerType());
                    }
                    else
                    {
                        this.LlvmConvert(
                            opCode, "fptoui", "ptrtoint", nativeIntType, !intPtrOper, ResolveType("System.IntPtr"), ResolveType("System.UIntPtr"));
                    }
                    break;

                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptoui",
                        "trunc",
                        ResolveType("System.Int32"),
                        false,
                        ResolveType("System.Int32"),
                        ResolveType("System.UInt32"));
                    break;

                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                    intPtrOper = this.IntTypeRequired(opCode);
                    nativeIntType = intPtrOper
                        ? ResolveType("System.Int32")
                        : ResolveType("System.Void").ToPointerType();
                    this.LlvmConvert(
                        opCode,
                        "fptosi",
                        "trunc",
                        nativeIntType,
                        !intPtrOper,
                        ResolveType("System.IntPtr"),
                        ResolveType("System.UIntPtr"));
                    break;

                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptosi",
                        "trunc",
                        ResolveType("System.UInt32"),
                        false,
                        ResolveType("System.Int32"),
                        ResolveType("System.UInt32"));
                    break;

                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptosi",
                        "sext",
                        ResolveType("System.Int64"),
                        false,
                        ResolveType("System.Int64"),
                        ResolveType("System.UInt64"));
                    break;

                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    this.LlvmConvert(
                        opCode,
                        "fptoui",
                        "zext",
                        ResolveType("System.UInt64"),
                        false,
                        ResolveType("System.Int64"),
                        ResolveType("System.UInt64"));
                    break;

                case Code.Castclass:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteCast(
                        opCodeTypePart,
                        opCodeTypePart.OpCodeOperands[0].Result,
                        opCodeTypePart.Operand,
                        true);

                    this.WriteDbgLine(opCode);

                    break;

                case Code.Isinst:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    var fromType = opCodeTypePart.OpCodeOperands[0].Result;
                    var toType = opCodeTypePart.Operand;

                    var dynamicCastRequired = false;
                    var castRequired = toType.IsClassCastRequired(
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

                    this.WriteDbgLine(opCode);

                    break;

                case Code.Newobj:

                    // to support settings exceptions
                    if (opCode.ReadExceptionFromStack)
                    {
                        opCode.Result = this.catchScopes.First().ExceptionResult;
                        break;
                    }

                    var opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;
                    this.WriteNewObject(opCodeConstructorInfoPart);

                    break;

                case Code.Newarr:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteNewArray(opCode, opCodeTypePart.Operand, opCode.OpCodeOperands[0]);

                    this.WriteDbgLine(opCode);

                    break;

                case Code.Initobj:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteInit(opCode, opCodeTypePart.Operand);

                    this.WriteDbgLine(opCode);

                    break;

                case Code.Throw:

                    this.WriteThrow(opCode, this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);

                    break;

                case Code.Rethrow:

                    this.WriteRethrow(
                        opCode,
                        this.catchScopes.Count > 0 ? this.catchScopes.Peek() : null,
                        this.tryScopes.Count > 0 ? this.tryScopes.Peek().Catches.First() : null);

                    this.WriteDbgLine(opCode);

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
                        switchValueType.WriteTypePrefix(writer);
                        writer.Write(" {0}, label %.a{1} ", index, opCodeLabels.JumpAddress(index++));
                    }

                    writer.WriteLine("]");

                    this.WriteDbgLine(opCode);
                    writer.WriteLine(string.Empty);

                    writer.Indent--;
                    writer.WriteLine(string.Concat(".a", opCode.GroupAddressEnd, ':'));
                    writer.Indent++;

                    opCode.Next.JumpProcessed = true;

                    break;

                case Code.Sizeof:
                    opCodeTypePart = opCode as OpCodeTypePart;
                    opCode.Result = new ConstValue(
                        opCodeTypePart.Operand.GetTypeSize(),
                        this.GetIntTypeByByteSize(PointerSize));
                    break;

                case Code.Mkrefany:
                    //var opResult = opCode.OpCodeOperands[0].Result;
                    //opCode.Result = opResult;
                    var typedRefType = ResolveType("System.TypedReference");
                    this.WriteSetResultNumber(opCode, typedRefType);
                    this.WriteAlloca(typedRefType);
                    writer.WriteLine(string.Empty);

                    break;

                case Code.Refanytype:
                    //var opResult = opCode.OpCodeOperands[0].Result;
                    //opCode.Result = opResult;

                    opCode.Result = new ConstValue("undef", ResolveType("System.Object"));

                    break;

                case Code.Refanyval:

                    typedRefType = opCode.OpCodeOperands[0].Result.Type;

                    var _targetFieldIndex = this.GetFieldIndex(typedRefType, "Value");
                    this.WriteFieldAccess(
                        writer,
                        opCode,
                        typedRefType,
                        typedRefType,
                        _targetFieldIndex,
                        opCode.OpCodeOperands[0].Result);
                    writer.WriteLine(string.Empty);
                    this.WriteFieldAccess(writer, opCode, opCode.Result.Type, opCode.Result.Type, 0, opCode.Result);
                    writer.WriteLine(string.Empty);
                    this.WriteLlvmLoad(opCode, opCode.Result.Type, opCode.Result);
                    writer.WriteLine(string.Empty);

                    break;

                case Code.Initblk:
                case Code.Cpblk:
                case Code.Cpobj:
                case Code.Jmp:
                case Code.Ckfinite:
                    throw new NotImplementedException();
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
            if (opCode.Result is ConstValue)
            {
                return false;
            }

            if (!destType.IsPointer && !opCode.Result.Type.IsPointer &&
                destType.IsIntValueTypeExtCastRequired(opCode.Result.Type))
            {
                this.LlvmIntConvert(opCode, opCode.Result.Type.IsSignedType() && destType.IsSignedType() ? "sext" : "zext", destType);
                writer.WriteLine(string.Empty);
                return true;
            }

            if (!destType.IsPointer && !opCode.Result.Type.IsPointer &&
                destType.IsIntValueTypeTruncCastRequired(opCode.Result.Type))
            {
                this.LlvmIntConvert(opCode, "trunc", destType);
                writer.WriteLine(string.Empty);
                return true;
            }

            // pointer to int, int to pointerf
            if (destType.IntTypeBitSize() > 0 && !destType.IsPointer && !destType.IsByRef &&
                opCode.Result.Type.IsPointer)
            {
                this.LlvmIntConvert(opCode, "ptrtoint", destType);
                writer.WriteLine(string.Empty);
                return true;
            }

            if (opCode.Result.Type.IntTypeBitSize() > 0 && (destType.IsPointer || destType.IsByRef) &&
                !opCode.Result.Type.IsPointer)
            {
                this.LlvmIntConvert(opCode, "inttoptr", destType);
                writer.WriteLine(string.Empty);
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
            AdjustResultTypeToOutgoingType(opCode, opCode.RequiredOutgoingType);
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
                    this.Output.WriteLine(string.Empty);
                    this.WriteCast(opCode, opCode.Result, requiredOutgoingType);
                }

                if (intAdjustmentRequired)
                {
                    this.Output.WriteLine(string.Empty);
                    this.AdjustIntConvertableTypes(this.Output, opCode, requiredOutgoingType);
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
        /// <param name="options">
        /// </param>
        /// <param name="resultType">
        /// </param>
        /// <param name="beforeSecondOperand">
        /// </param>
        public void BinaryOper(
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            string op,
            OperandOptions options = OperandOptions.None,
            IType resultType = null,
            string beforeSecondOperand = null)
        {
            this.ProcessOperator(writer, opCode, op, options: options, resultType: resultType);

            this.WriteOperandResult(writer, opCode, 0);
            writer.Write(", ");
            if (beforeSecondOperand != null)
            {
                writer.Write(beforeSecondOperand);
            }

            this.WriteOperandResult(
                writer,
                opCode,
                1,
                options.HasFlag(OperandOptions.DetectAndWriteTypeInSecondOperand));
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
            if (type.IsRootOfVirtualTable())
            {
                index++;
            }

            // add shift for interfaces
            index += type.GetInterfacesExcludingBaseAllInterfaces().Count();

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodBase">
        /// </param>
        public void CheckIfMethodExternalDeclarationIsRequired(IMethod methodBase, IType ownerOfExplicitInterface = null)
        {
            if (methodBase.Name.StartsWith("%"))
            {
                return;
            }

            if (methodBase.AssemblyQualifiedName != AssemblyQualifiedName)
            {
                this.methodDeclRequired.Add(new MethodKey(methodBase, ownerOfExplicitInterface));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void CheckIfExternalDeclarationIsRequired(IType type)
        {
            if (type == null)
            {
                return;
            }

            if (type.IsMultiArray)
            {
                this.typeDeclRequired.Add(ResolveType("System.Array"));
            }

            if (type.AssemblyQualifiedName == AssemblyQualifiedName)
            {
                return;
            }

            this.typeDeclRequired.Add(type.ToBareType());
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        public void CheckIfStaticFieldExternalDeclarationIsRequired(IField field)
        {
            if (field == null || !field.IsStatic || field.DeclaringType.AssemblyQualifiedName == AssemblyQualifiedName ||
                field.DeclaringType.IsGenericType)
            {
                return;
            }

            this.staticFieldExtrenalDeclRequired.Add(field);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public string GetAllocator()
        {
            return this.Gc ? "GC_malloc" : "_Znwj";
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
        public string GetArgVarName(string name, int index, bool arg = false)
        {
            return string.Format("%\"{2}{1}.{0}\"", name, index, arg ? "arg." : string.Empty);
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
            this.Output = new LlvmIndentedTextWriter(new StringWriter(sb));

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
        /// <param name="arg">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetThisName(bool arg = false)
        {
            return string.Format("%\"{0}0.this\"", arg ? "arg." : string.Empty);
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
                    type = GetTypeOfReference(opCode);
                    if (type.IsVoid())
                    {
                        // ignore Ldind.i load of Void type
                        opCode.Result = opCode.OpCodeOperands[0].Result;
                        return;
                    }
                    if (!type.IsPointer && !type.IsByRef && type.IntTypeBitSize() == PointerSize * 8)
                    {
                        // using int as intptr
                        type = ResolveType("System.IntPtr");
                        this.AdjustIntConvertableTypes(writer, opCode.OpCodeOperands[0], type.ToPointerType());
                        opCode.Result = opCode.OpCodeOperands[0].Result;
                    }

                    break;
                case Code.Ldind_I1:
                    // it can be Bool or Byte, leave it null
                    ////type = this.ResolveType("System.SByte");
                    var result = ResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.HasElementType ? result.Type.GetElementType() : result.Type;
                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = ResolveType("System.SByte");
                    }
                    break;
                case Code.Ldind_U1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.ResolveType("System.Byte");
                    result = ResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.HasElementType ? result.Type.GetElementType() : result.Type;

                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = ResolveType("System.Byte");
                    }
                    break;
                case Code.Ldind_I2:
                    type = ResolveType("System.Int16");
                    break;
                case Code.Ldind_U2:
                    type = ResolveType("System.UInt16");
                    break;
                case Code.Ldind_I4:
                    type = ResolveType("System.Int32");
                    break;
                case Code.Ldind_U4:
                    type = ResolveType("System.UInt32");
                    break;
                case Code.Ldind_I8:
                    type = ResolveType("System.Int64");
                    break;
                case Code.Ldind_R4:
                    type = ResolveType("System.Single");
                    break;
                case Code.Ldind_R8:
                    type = ResolveType("System.Double");
                    break;
                case Code.Ldind_Ref:
                    type = GetTypeOfReference(opCode);
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
        public void LoadIndirect(LlvmIndentedTextWriter writer, OpCodePart opCode, IType type)
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
            if (isValueType && isUsedAsClass)
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
        /// <returns>
        /// </returns>
        public FullyDefinedReference ProcessOperator(
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
                opCode,
                requiredType,
                options,
                out castFrom,
                out intAdjustment,
                out intAdjustSecondOperand,
                operand1,
                operand2);

            effectiveType = this.ApplyTypeAdjustment(
                writer,
                opCode,
                effectiveType,
                castFrom,
                intAdjustment,
                intAdjustSecondOperand,
                ref resultType,
                options,
                operand1,
                operand2);

            var castResult = options.HasFlag(OperandOptions.TypeIsInOperator) ? opCode.Result : null;

            this.WriteResultAndFirstOperandType(writer, opCode, op, requiredType, resultType, options, effectiveType);

            return castResult;
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

                var opts = OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes;

                this.ProcessOperator(
                    writer,
                    opCodePart,
                    "store",
                    fieldType,
                    options: opts,
                    operand1: valueOperand,
                    operand2: -1);
                this.WriteOperandResult(writer, opCodePart, valueOperand);
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
            this.ProcessOperator(writer, opCode, op, requiredType, resultType, options, operandIndex, -1);

            if (!options.HasFlag(OperandOptions.IgnoreOperand))
            {
                this.WriteOperandResult(writer, opCode, operandIndex);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="typeIn">
        /// </param>
        /// <param name="isThis">
        /// </param>
        public void WriteArgumentCopyDeclaration(string name, int index, IType typeIn, bool isThis = false)
        {
            if (!isThis && typeIn.IsStructureType())
            {
                return;
            }

            var type = isThis ? typeIn.ToClass() : typeIn;

            var paramFullName = isThis ? this.GetThisName() : this.GetArgVarName(name, index);
            var paramArgFullName = isThis ? this.GetThisName(true) : this.GetArgVarName(name, index, true);
            this.Output.Write("{0} = ", paramFullName);

            // for value types
            this.Output.Write("alloca ");
            type.WriteTypePrefix(this.Output, type.IsStructureType() || isThis);
            this.Output.Write(", align " + PointerSize);
            this.Output.WriteLine(string.Empty);

            this.Output.Write("store ");
            type.WriteTypePrefix(this.Output, type.IsStructureType() || isThis);
            this.Output.Write(" {0}", paramArgFullName);
            this.Output.Write(", ");
            type.WriteTypePrefix(this.Output, type.IsStructureType() || isThis);

            this.Output.Write("* {0}", paramFullName);
            this.Output.Write(", align " + PointerSize);
            this.Output.WriteLine(string.Empty);

            if (this.DebugInfo && this.debugInfoGenerator.CurrentDebugLine.HasValue)
            {
                var effectiveName = isThis ? "this" : name;
                var effectiveType = type.IsStructureType() || isThis ? type.ToClass() : type;
                this.WriteDebugDeclare(effectiveName, effectiveType, paramFullName, DebugVariableType.Argument, index);
            }
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
            LlvmIndentedTextWriter writer,
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

            LlvmHelpersGen.SetCustomLabel(opCodePart, label);
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
        public void WriteBranchSwitchToThrowOrPass(
            LlvmIndentedTextWriter writer,
            OpCodePart opCodePart,
            FullyDefinedReference testValueResultNumber,
            string exceptionName,
            string labelPrefix,
            string labelSuffix)
        {
            this.WriteBranchSwitchToExecute(
                writer,
                opCodePart,
                testValueResultNumber,
                exceptionName,
                labelPrefix,
                labelSuffix,
                () => this.WriteThrowException(writer, exceptionName));
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
            LlvmIndentedTextWriter writer,
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
            this.CheckIfExternalDeclarationIsRequired(declaringType);
            declaringType.WriteCallNewObjectMethod(this, opCode);

            var newObjectResult = opCode.Result;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Copy data");

            if (!declaringType.IsStructureType() && declaringType.FullName != "System.DateTime" &&
                declaringType.FullName != "System.Decimal")
            {
                this.WriteFieldAccess(
                    writer,
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
            LlvmIndentedTextWriter writer,
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

        public void WriteDbgLine(OpCodePart opCode)
        {
            if (!this.DebugInfo)
            {
                return;
            }

            var line = this.debugInfoGenerator.CurrentDebugLine;
            if (line.HasValue)
            {
                this.Output.Write(", !dbg !{0}", line);
            }
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
            LlvmIndentedTextWriter writer,
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
                var testNullResultNumber = this.WriteSetResultNumber(opCodeTypePart, ResolveType("System.Boolean"));
                writer.Write("icmp eq ");
                fromType.Type.WriteTypePrefix(writer);
                writer.WriteLine(" {0}, null", fromType);

                writer.WriteLine(
                    "br i1 {0}, label %.dynamic_cast_null{1}, label %.dynamic_cast_not_null{1}",
                    testNullResultNumber,
                    opCodeTypePart.AddressStart);

                writer.Indent--;
                writer.WriteLine(".dynamic_cast_not_null{0}:", opCodeTypePart.AddressStart);
                writer.Indent++;
            }

            this.WriteBitcast(opCodeTypePart, fromType, ResolveType("System.Byte"));
            writer.WriteLine(string.Empty);

            var firstCastToBytesResult = opCodeTypePart.Result;

            var dynamicCastResultNumber = this.WriteSetResultNumber(
                opCodeTypePart,
                ResolveType("System.Byte").ToPointerType());

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
            this.WriteBitcast(opCodeTypePart, dynamicCastResultNumber, toClassType);

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

                var testNullResultNumber = this.WriteSetResultNumber(opCodeTypePart, toClassType);
                writer.Write("phi ");
                toClassType.WriteTypePrefix(writer, true);
                writer.Write(
                    " [ {0}, {1} ], [ null, {2} ]",
                    dynamicCastResult,
                    string.Format(
                        "%.{1}{0}",
                        opCodeTypePart.AddressStart,
                        throwExceptionIfNull ? "dynamic_cast_result_not_null" : "dynamic_cast_not_null"),
                    string.Format("%.dynamic_cast_null{0}", opCodeTypePart.AddressStart));

                LlvmHelpersGen.SetCustomLabel(opCodeTypePart, label);
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
        public void WriteFieldAccess(LlvmIndentedTextWriter writer, OpCodeFieldInfoPart opCodeFieldInfoPart)
        {
            writer.WriteLine("; Access to '{0}' field", opCodeFieldInfoPart.Operand.Name);

            var operand = opCodeFieldInfoPart.OpCodeOperands[0];
            var operandResultCalc = ResultOf(operand);
            var operandType = operandResultCalc.Type;
            var effectiveType = operandType.IsPointer ? operandType.GetElementType() : operandType;
            var asPointer = false;
            if (effectiveType.IsValueType)
            {
                if (operand.Result.Type.IntTypeBitSize() == PointerSize * 8)
                {
                    effectiveType = opCodeFieldInfoPart.Operand.DeclaringType;
                    this.LlvmConvert(operand, string.Empty, string.Empty, effectiveType.ToPointerType(), true);

                    asPointer = true;

                    writer.WriteLine(string.Empty);
                }
                else
                {
                    effectiveType = effectiveType.ToClass();
                }
            }
            else if (effectiveType.IsPointer)
            {
                effectiveType = opCodeFieldInfoPart.Operand.DeclaringType;
                this.WriteBitcast(operand, effectiveType.ToPointerType());

                asPointer = true;

                writer.WriteLine(string.Empty);
            }

            this.UnaryOper(
                writer,
                opCodeFieldInfoPart,
                "getelementptr inbounds",
                asPointer ? effectiveType.ToPointerType() : effectiveType,
                opCodeFieldInfoPart.Operand.FieldType,
                OperandOptions.GenerateResult);

            CheckIfTypeIsRequiredForBody(effectiveType);

            this.WriteFieldIndex(writer, effectiveType, opCodeFieldInfoPart.Operand);
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
        public void WriteFieldAccess(LlvmIndentedTextWriter writer, OpCodePart opCodePart, int index)
        {
            writer.WriteLine("; Access to '#{0}' field", index);
            var operand = ResultOf(opCodePart.OpCodeOperands[0]);

            var classType = operand.Type.ToClass();

            var opts = OperandOptions.GenerateResult;
            var fieldType = classType.GetFieldTypeByFieldNumber(index);

            this.UnaryOper(writer, opCodePart, "getelementptr inbounds", classType, fieldType, opts);

            CheckIfTypeIsRequiredForBody(classType);

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
            LlvmIndentedTextWriter writer,
            OpCodePart opCodePart,
            IType classType,
            IType fieldContainerType,
            int index,
            FullyDefinedReference valueReference)
        {
            var fieldType = fieldContainerType.GetFieldTypeByFieldNumber(index);
            if (fieldType == null)
            {
                return false;
            }

            this.WriteSetResultNumber(opCodePart, fieldType);

            writer.Write("getelementptr inbounds ");
            valueReference.Type.ToClass().WriteTypePrefix(writer);
            writer.Write(" ");
            writer.Write(valueReference);

            CheckIfTypeIsRequiredForBody(valueReference.Type);

            this.WriteFieldIndex(writer, classType, fieldContainerType, index);

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

            var index = this.GetFieldPosition(type, fieldInfo);

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
        public void WriteFieldIndex(
            LlvmIndentedTextWriter writer,
            IType classType,
            IType fieldContainerType,
            int fieldIndex)
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
        /// <param name="declaringTypeIn">
        /// </param>
        /// <param name="interface">
        /// </param>
        public void WriteInterfaceAccess(
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            IType declaringTypeIn,
            IType @interface)
        {
            var objectResult = opCode.Result;

            writer.WriteLine("; Get type '{0}' of '{1}'", @interface, declaringTypeIn);

            var declaringType = declaringTypeIn.ToClass();

            var castedResult = this.ProcessOperator(
                writer,
                opCode,
                "getelementptr inbounds",
                declaringType,
                @interface,
                OperandOptions.TypeIsInOperator | OperandOptions.GenerateResult);
            writer.Write(' ');
            writer.Write(castedResult ?? objectResult);

            CheckIfTypeIsRequiredForBody(declaringType);

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
        public void WriteMethodDefinitionName(
            LlvmIndentedTextWriter writer,
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
            LlvmIndentedTextWriter writer,
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

                thisType.ToClass().WriteTypePrefix(writer, true);
                hasParameterWritten = true;

                if (!noArgumentName)
                {
                    writer.Write(" {0}", this.GetThisName(true));
                }
            }

            var index = start;
            var parameterIndex = start;
            var isAnonymousDelegate = method.IsAnonymousDelegate;
            foreach (var parameter in parameterInfos)
            {
                this.CheckIfExternalDeclarationIsRequired(parameter.ParameterType);

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

                    parameter.ParameterType.WriteTypePrefix(writer, parameter.ParameterType.IsStructureType());
                    hasParameterWritten = true;

                    if (parameter.ParameterType.IsStructureType())
                    {
                        CheckIfTypeIsRequiredForBody(parameter.ParameterType);
                        if (!noArgumentName)
                        {
                            if (!parameter.IsOut && !parameter.IsRef)
                            {
                                writer.Write(" byval align " + PointerSize);
                            }

                            writer.Write(" %\"{0}.", parameterIndex);
                        }
                    }
                    else if (!noArgumentName)
                    {
                        writer.Write(" %\"arg.{0}.", parameterIndex);
                    }

                    if (!noArgumentName)
                    {
                        writer.Write(parameter.Name);
                        writer.Write("\"");
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

                (thisType ?? methodInfo.DeclaringType.ToClass()).WriteTypePrefix(writer);
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
                    parameter.ParameterType.WriteTypePrefix(writer);
                }
                else
                {
                    parameter.ParameterType.ToClass().WriteTypePrefix(writer);
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
        /// <param name="writer">
        /// </param>
        /// <param name="typeName">
        /// </param>
        /// <returns>
        /// </returns>
        public FullyDefinedReference WriteNewCallingDefaultConstructor(LlvmIndentedTextWriter writer, string typeName)
        {
            var typeToCreate = ResolveType(typeName);
            return this.WriteNewCallingDefaultConstructor(writer, typeToCreate);
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
            LlvmIndentedTextWriter writer,
            IType typeToCreate)
        {
            // throw InvalidCast result
            writer.WriteLine(string.Empty);

            // find constructor
            var constructorInfo = IlReader.Constructors(typeToCreate).First(c => !c.GetParameters().Any());

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
                IlReader.Constructors(type)
                    .FirstOrDefault(
                        c =>
                            c.GetParameters().Count() == 1 &&
                            c.GetParameters().First().ParameterType.TypeEquals(firstParameterType));

            //Debug.Assert(constructorInfo != null, "Could not find required constructor");

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
        public void WriteOperandResult(
            LlvmIndentedTextWriter writer,
            OpCodePart operand,
            bool detectAndWriteTypePrefix = false,
            IType forcedType = null)
        {
            writer.Write(' ');

            if (forcedType != null)
            {
                forcedType.WriteTypePrefix(writer);
                writer.Write(' ');
            }
            else if (detectAndWriteTypePrefix)
            {
                (operand.Result.Type ?? ResolveType("System.Void")).WriteTypePrefix(writer);
                writer.Write(' ');
            }

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
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            int index,
            bool detectAndWriteTypePrefix = false)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            var operand = opCode.OpCodeOperands[index];
            this.WriteOperandResult(writer, operand, detectAndWriteTypePrefix);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public void WritePhi(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.AlternativeValues.Values.Count != opCode.AlternativeValues.Labels.Count)
            {
                // phi is not full
                return;
            }

            writer.WriteLine(string.Empty);

            var firstValueWithRequiredType =
                opCode.AlternativeValues.Values.FirstOrDefault(
                    v => v.RequiredOutgoingType != null && !(v.ResultAtExit is ConstValue));

            var firstValueRequiredType = firstValueWithRequiredType != null
                ? firstValueWithRequiredType.RequiredOutgoingType
                : null;

            var phiType = firstValueRequiredType;
            if (phiType == null)
            {
                var firstNonConstValue = opCode.AlternativeValues.Values.FirstOrDefault(v => !(v.ResultAtExit is ConstValue));
                if (firstNonConstValue != null)
                {
                    phiType = firstNonConstValue.ResultAtExit.Type ?? firstNonConstValue.RequiredOutgoingType;
                }
            }

            if (phiType == null)
            {
                var firstValue = opCode.AlternativeValues.Values.First();
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
                        opCode.AlternativeValues.Values.Where(v => v.ResultAtExit is ConstValue && v.Any(Code.Ldc_I4_0)))
                {
                    val.ResultAtExit = new ConstValue(null, ResolveType("System.Void").ToPointerType());
                }
            }

            // apply PHI if condition is complex
            var nopeCode = OpCodePart.CreateNop;
            this.ProcessOperator(writer, nopeCode, "phi", phiType, phiType, OperandOptions.GenerateResult);

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
                    values[index].ResultAtExit,
                    values[index],
                    values[index],
                    string.Concat("a", opCode.AlternativeValues.Labels[index]),
                    opCode.AlternativeValues.Labels[index]);
            }

            writer.WriteLine(string.Empty);

            AdjustResultTypeToOutgoingType(nopeCode, RequiredIncomingType(opCode));

            opCode.AlternativeValues.Values.Last().Result = structUsed
                ? nopeCode.Result.ToNormalType()
                : nopeCode.Result;

            // clear it after processing
            opCode.AlternativeValues = null;
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
                this.UnaryOper(
                    writer,
                    opCode,
                    "ret",
                    methodReturnType,
                    options: opts | OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);

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
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="resultToTest">
        /// </param>
        /// <returns>
        /// </returns>
        public IncrementalResult WriteTestNull(
            LlvmIndentedTextWriter writer,
            OpCodePart opCodePart,
            FullyDefinedReference resultToTest)
        {
            var testNullResultNumber = this.WriteSetResultNumber(opCodePart, ResolveType("System.Boolean"));
            opCodePart.Result = resultToTest;

            writer.Write("icmp eq ");
            resultToTest.Type.WriteTypePrefix(writer);
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
            LlvmIndentedTextWriter writer,
            OpCodePart opCodePart,
            IncrementalResult resultToTest,
            string exceptionName,
            string labelPrefix)
        {
            var testNullResultNumber = this.WriteTestNull(writer, opCodePart, resultToTest);
            this.WriteBranchSwitchToThrowOrPass(
                writer,
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
        public void WriteThrowException(LlvmIndentedTextWriter writer, string exceptionName)
        {
            // throw InvalidCast result
            writer.WriteLine(string.Empty);

            var opCodeThrow = new OpCodePart(OpCodesEmit.Throw, 0, 0);

            var invalidCastExceptionType = ResolveType(exceptionName);

            // find constructor
            var constructorInfo = IlReader.Constructors(invalidCastExceptionType).First(c => !c.GetParameters().Any());

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
            this.Output = new LlvmIndentedTextWriter(new StringWriter(sb));

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
            LlvmIndentedTextWriter writer,
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
                ? opCode.OpCodeOperands[
                    operand2 >= 0 && opCode.OpCodeOperands.Length > operand2 && intAdjustSecondOperand
                        ? operand2
                        : operand1
                    ]
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
            var list = IlReader.Fields(type).Where(t => !t.IsStatic).ToList();
            var index = 0;

            while (index < list.Count && !list[index].Name.Equals(fieldName))
            {
                index++;
            }

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

            if (sourceIntType && requiredType.IsPointer || requiredIntType && sourceType.IsPointer)
            {
                intAdjustmentRequired = true;
                return;
            }

            // TODO: review it
            if (sourceType.TypeEquals(ResolveType("System.Boolean")) &&
                requiredType.TypeEquals(ResolveType("System.Byte")))
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
                ? ResultOf(opCode)
                : opCode.OpCodeOperands != null && operand1 >= 0 && opCode.OpCodeOperands.Length > operand1
                    ? ResultOf(opCode.OpCodeOperands[operand1])
                    : null;

            var res2 = opCode.OpCodeOperands != null && operand2 >= 0 && opCode.OpCodeOperands.Length > operand2
                ? ResultOf(opCode.OpCodeOperands[operand2])
                : null;

            // write type
            var effectiveType = ResolveType("System.Void");

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
                    res1.Type.TypeEquals(ResolveType("System.Boolean"))
                    && requiredType.TypeEquals(ResolveType("System.Byte")))
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

                    if (this.IsPointerArithmetic(opCode))
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
            var writer = this.Output;

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
            return this.GetGlobalConstructorsFunctionName(AssemblyQualifiedName);
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyQualifiedName">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetGlobalConstructorsFunctionName(string assemblyQualifiedName)
        {
            return string.Concat("@\"Global Ctors for ", assemblyQualifiedName, "\"");
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

                    // type = this.ResolveType("System.Int32");
                    type = GetTypeOfReference(opCode);
                    break;
                case Code.Ldelem_I1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.ResolveType("System.SByte");
                    var result = ResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.GetElementType();
                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = ResolveType("System.SByte");
                    }

                    break;
                case Code.Ldelem_I2:
                    type = ResolveType("System.Int16");
                    break;
                case Code.Ldelem_I4:
                    type = ResolveType("System.Int32");
                    break;
                case Code.Ldelem_U1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.ResolveType("System.Byte");
                    result = ResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.GetElementType();
                    break;
                case Code.Ldelem_U2:
                    type = ResolveType("System.UInt16");
                    break;
                case Code.Ldelem_U4:
                    type = ResolveType("System.UInt32");
                    break;
                case Code.Ldelem_I8:
                    type = ResolveType("System.Int64");
                    break;
                case Code.Ldelem_R4:
                    type = ResolveType("System.Single");
                    break;
                case Code.Ldelem_R8:
                    type = ResolveType("System.Double");
                    break;
                case Code.Ldelem:
                case Code.Ldelem_Ref:
                    type = GetTypeOfReference(opCode);
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

            this.BinaryOper(
                writer,
                opCode,
                "getelementptr inbounds",
                OperandOptions.GenerateResult | OperandOptions.DetectAndWriteTypeInSecondOperand,
                type,
                opCode.OpCodeOperands[0].Result.Type.IsArray ? "i32 0, i32 5," : null);

            CheckIfTypeIsRequiredForBody(opCode.OpCodeOperands[0].Result.Type);

            if (actualLoad)
            {
                writer.WriteLine(string.Empty);

                var accessIndexResultNumber = opCode.Result;
                opCode.Result = null;

                if (!type.IsStructureType())
                {
                    this.WriteLlvmLoad(opCode, type, accessIndexResultNumber);
                }
                else
                {
                    opCode.Result = accessIndexResultNumber;
                }
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

                    // type = this.ResolveType("System.Void").ToPointerType();
                    type = GetTypeOfReference(opCode);
                    break;
                case Code.Stelem_I1:

                    // it can be Bool or Byte, leave it null
                    ////type = this.ResolveType("System.SByte");
                    var result = ResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.GetElementType();
                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = ResolveType("System.SByte");
                    }

                    break;
                case Code.Stelem_I2:
                    type = ResolveType("System.Int16");
                    break;
                case Code.Stelem_I4:
                    type = ResolveType("System.Int32");
                    break;
                case Code.Stelem_I8:
                    type = ResolveType("System.Int64");
                    break;
                case Code.Stelem_R4:
                    type = ResolveType("System.Single");
                    break;
                case Code.Stelem_R8:
                    type = ResolveType("System.Double");
                    break;
                case Code.Stelem:
                case Code.Stelem_Ref:
                    type = GetTypeOfReference(opCode);
                    break;
            }

            if (type.UseAsClass)
            {
                type = type.ToNormal();
            }

            Debug.Assert(!type.IsVoid());

            this.BinaryOper(
                writer,
                opCode,
                "getelementptr inbounds",
                OperandOptions.GenerateResult | OperandOptions.DetectAndWriteTypeInSecondOperand,
                type,
                opCode.OpCodeOperands[0].Result.Type.IsArray ? "i32 0, i32 5," : null);

            CheckIfTypeIsRequiredForBody(opCode.OpCodeOperands[0].Result.Type);

            writer.WriteLine(string.Empty);

            var operandIndex = 2;

            if (!type.IsStructureType())
            {
                this.AdjustIntConvertableTypes(writer, opCode.OpCodeOperands[operandIndex], type);

                this.ProcessOperator(writer, opCode, "store", type, operand1: 2, operand2: -1);
                this.WriteOperandResult(writer, opCode, operandIndex);

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
                    type = GetTypeOfReference(opCode);
                    break;
                case Code.Stind_I1:
                    //type = this.ResolveType("System.Byte");

                    // it can be Bool or Byte, leave it null
                    ////type = this.ResolveType("System.SByte");
                    var result = ResultOf(opCode.OpCodeOperands[0]);
                    type = result.Type.HasElementType ? result.Type.GetElementType() : result.Type;
                    if (type.IsVoid() || type.IntTypeBitSize() > 8)
                    {
                        type = ResolveType("System.SByte");
                    }

                    break;
                case Code.Stind_I2:
                    type = ResolveType("System.Int16");
                    break;
                case Code.Stind_I4:
                    type = ResolveType("System.Int32");
                    break;
                case Code.Stind_I8:
                    type = ResolveType("System.Int64");
                    break;
                case Code.Stind_R4:
                    type = ResolveType("System.Single");
                    break;
                case Code.Stind_R8:
                    type = ResolveType("System.Double");
                    break;
                case Code.Stind_Ref:
                    type = GetTypeOfReference(opCode);
                    break;
            }

            Debug.Assert(!type.IsVoid());

            var resultOfOperand0 = opCode.OpCodeOperands[0].Result;
            var destinationType = resultOfOperand0.Type;
            if (destinationType.IsPointer && destinationType.GetElementType().TypeNotEquals(type))
            {
                // adjust destination type, cast pointer to pointer of type
                this.WriteBitcast(opCode, resultOfOperand0, type);
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

            this.UnaryOper(
                writer,
                opCode,
                1,
                "store",
                type,
                options: OperandOptions.CastPointersToBytePointer | OperandOptions.AdjustIntTypes);
            writer.Write(", ");

            destinationType.WriteTypePrefix(writer, true);
            this.WriteOperandResult(writer, opCode, 0);
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
        private void SaveStructElement(LlvmIndentedTextWriter writer, OpCodePart opCode, int operandIndex, IType type)
        {
            // copy struct
            this.WriteLlvmLoad(opCode, type, opCode.OpCodeOperands[operandIndex].Result);
        }

        /// <summary>
        /// </summary>
        private void SortStaticConstructorsByUsage()
        {
            var staticConstructors = new Dictionary<IConstructor, HashSet<IType>>();
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
                foreach (var staticConstructorPair in staticConstructors.ToList())
                {
                    if (staticConstructorPair.Value.Any(
                            v => staticConstructors.Keys.Any(k => k.DeclaringType.TypeEquals(v))))
                    {
                        continue;
                    }

                    staticConstructors.Remove(staticConstructorPair.Key);
                    newStaticConstructors.Add(staticConstructorPair.Key);
                }
            } while (staticConstructors.Count > 0 && countBefore != staticConstructors.Count);

            Debug.Assert(staticConstructors.Keys.Count == 0, "Not All static constructors were resolved");

            // add rest as is
            newStaticConstructors.AddRange(staticConstructors.Keys);

            StaticConstructors = newStaticConstructors;
        }

        /// <summary>
        /// </summary>
        /// <param name="parametersInfo">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        private void WriteArgumentCopyDeclarations(IMethod methodInfo, bool hasThis)
        {
            var parametersInfo = methodInfo.GetParameters();

            if (hasThis)
            {
                this.WriteArgumentCopyDeclaration(null, 0, ThisType, true);
            }

            var index = hasThis ? 1 : 0;
            var isAnonymousDelegate = methodInfo.IsAnonymousDelegate;
            foreach (var parameterInfo in parametersInfo)
            {
                if (isAnonymousDelegate)
                {
                    isAnonymousDelegate = false;
                }
                else
                {
                    this.WriteArgumentCopyDeclaration(parameterInfo.Name, index, parameterInfo.ParameterType);
                }

                index++;
            }
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
                this.GetArrayTypeHeader(ResolveType("System.Byte"), pair.Value.Length),
                this.GetArrayValuesHeader(ResolveType("System.Byte"), pair.Value.Length, pair.Value.Length),
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
            foreach (var reference in this.AllReference.Reverse().Distinct())
            {
                this.Output.WriteLine("call void " + this.GetGlobalConstructorsFunctionName(reference) + "();");
            }
        }

        /// <summary>
        /// </summary>
        private void WriteCallGctorsDeclarations()
        {
            // get all references
            foreach (var reference in this.AllReference.Skip(1).Reverse().Distinct())
            {
                this.Output.WriteLine("declare void " + this.GetGlobalConstructorsFunctionName(reference) + "();");
            }
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
        private void WriteCatchFinnallyEnd(LlvmIndentedTextWriter writer, OpCodePart opCode)
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
        private void WriteCondBranch(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            writer.Write(
                "br i1 {0}, label %.a{1}, label %.a{2}",
                opCode.Result,
                opCode.JumpAddress(),
                opCode.AddressEnd);

            this.WriteDbgLine(opCode);
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

        private void WriteDebugDeclare(
            string name,
            IType type,
            string variableOrArgumentName,
            DebugVariableType debugVariableType,
            int index = 0)
        {
            if (!this.debugInfoGenerator.CurrentDebugLine.HasValue)
            {
                return;
            }

            this.Output.Write("call void @llvm.dbg.declare(metadata !{");
            type.WriteTypePrefix(this.Output);
            this.Output.Write(string.Concat("* ", variableOrArgumentName, "}, "));
            this.debugInfoGenerator.DefineVariable(name, type, debugVariableType, index).WriteTo(this.Output);
            this.Output.Write(", ");
            this.debugInfoGenerator.DefineTagExpression().WriteTo(this.Output);
            this.Output.WriteLine("), !dbg !{0}", this.debugInfoGenerator.CurrentDebugLine);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteEndFinally(LlvmIndentedTextWriter writer, OpCodePart opCode)
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
        private void WriteExceptionHandlersProlog(LlvmIndentedTextWriter writer, OpCodePart opCode)
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
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            IMethod methodBase,
            IType thisType = null)
        {
            this.WriteSetResultNumber(opCode, ResolveType("System.Int32").ToPointerType());
            writer.Write("bitcast ");
            this.WriteMethodPointerType(writer, methodBase, thisType);
            writer.Write("* ");
            this.WriteResult(opCode.OpCodeOperands[0].Result);
            writer.Write(" to ");
            ResolveType("System.Int32").ToPointerType().WriteTypePrefix(writer);
            writer.WriteLine(string.Empty);

            var res = opCode.Result;
            var offsetResult = this.WriteSetResultNumber(opCode, ResolveType("System.Int32"));
            writer.Write("getelementptr ");
            ResolveType("System.Int32").WriteTypePrefix(writer);
            writer.Write("* ");
            this.WriteResult(res);
            writer.WriteLine(", i32 -{0}", ObjectInfrastructure.FunctionsOffsetInVirtualTable);

            opCode.Result = null;
            this.WriteLlvmLoad(opCode, ResolveType("System.Int32"), offsetResult);
        }

        /// <summary>
        /// </summary>
        private void WriteGlobalConstructors()
        {
            // write global ctors caller
            this.Output.WriteLine(string.Empty);
            this.Output.WriteLine(
                "define {2} void {0}() {1}",
                this.GetGlobalConstructorsFunctionName(),
                "{",
                this.Gctors ? "internal" : string.Empty);
            this.Output.Indent++;

            this.SortStaticConstructorsByUsage();

            if (this.Gc && this.IsCoreLib)
            {
                this.Output.WriteLine("call void @GC_init()");
            }

            foreach (var staticCtor in StaticConstructors)
            {
                this.Output.WriteLine("call void {0}()", staticCtor.GetFullMethodName());
            }

            this.Output.WriteLine("ret void");
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
        private bool WriteInterfaceIndex(LlvmIndentedTextWriter writer, IType classType, IType @interface)
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

        // TODO: here the bug with index, index is caluclated for derived class but need to be calculated and used for type where the type belong to

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
                        this.CheckIfMethodExternalDeclarationIsRequired(methodInVirtualTable.Value);
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
                        ? typeContainingInterface.BaseType.GetTypeSize()
                        : 0;
                    var interfaceIndex = FindInterfaceIndexes(typeContainingInterface, @interface, index).Sum();

                    this.Output.WriteLine(string.Empty);
                    this.Output.Write(type.GetVirtualInterfaceTableName(@interface));
                    var virtualInterfaceTable = type.GetVirtualInterfaceTable(@interface);
                    virtualInterfaceTable.WriteTableOfMethods(
                        this,
                        type,
                        interfaceIndex,
                        baseTypeSizeOfTypeContainingInterface);
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
                var previousOpCode = opCode.Previous;
                var splitBlock = previousOpCode == null
                                 || (previousOpCode != null
                                     && (previousOpCode.OpCode.FlowControl == FlowControl.Meta
                                         || previousOpCode.OpCode.FlowControl == FlowControl.Next
                                         || previousOpCode.OpCode.FlowControl == FlowControl.Call));

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
        /// <param name="opCode">
        /// </param>
        private void WriteLeave(LlvmIndentedTextWriter writer, OpCodePart opCode)
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
            LlvmIndentedTextWriter writer,
            int index,
            int argIndex,
            bool asReference = false)
        {
            Parameters[index].ParameterType.WriteTypePrefix(writer);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(this.GetArgVarName(index, argIndex));

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
                this.Output.Write("{0} = ", this.GetLocalVarName(local.LocalIndex));
                this.WriteAlloca(GetEffectiveLocalType(local));

                this.CheckIfExternalDeclarationIsRequired(local.LocalType);

                this.Output.WriteLine(string.Empty);
            }

            if (this.DebugInfo)
            {
                foreach (var local in locals)
                {
                    this.WriteDebugDeclare(
                        this.debugInfoGenerator.GetLocalNameByIndex(local.LocalIndex),
                        this.GetEffectiveLocalType(local),
                        this.GetLocalVarName(local.LocalIndex),
                        DebugVariableType.Auto);
                }
            }
        }

        /// <summary>
        /// </summary>
        private void WriteMainFunction()
        {
            var hasParameters = MainMethod.GetParameters().Any();
            if (!hasParameters)
            {
                this.Output.Write("define i32 @main()");
            }
            else
            {
                this.Output.Write("define i32 @main(i32 %\"arg.0.value\", i8** %\"arg.1.value\")");
            }

            this.WriteMethodNumber();

            this.Output.WriteLine(" {");

            this.Output.Indent++;

            // create locals and args
            if (hasParameters)
            {
                this.Output.WriteLine("%local0 = alloca { i8*, i8*, i8*, i32, i32, [ 0 x %\"System.String\"* ] }*, align 4");
                this.Output.WriteLine("%local1 = alloca i32, align 4");
                this.Output.WriteLine("%\"0.value\" = alloca i32, align 4");
                this.Output.WriteLine("store i32 %\"arg.0.value\", i32* %\"0.value\", align 4");
                this.Output.WriteLine("%\"1.value\" = alloca i8**, align 4");
                this.Output.WriteLine("store i8** %\"arg.1.value\", i8*** %\"1.value\", align 4");
            }

            if (!this.Gctors)
            {
                this.WriteCallGctors();
            }

            var result = hasParameters ? this.WriteLoadingArgumentsForMain(this.MainMethod, null) : null;

            if (!MainMethod.ReturnType.IsVoid())
            {
                this.Output.Write("%1 = call i32 ");
            }
            else
            {
                this.Output.Write("call void ");
            }

            this.WriteMethodDefinitionName(this.Output, MainMethod);
            this.Output.Write("(");

            if (hasParameters)
            {
                result.Type.WriteTypePrefix(this.Output);
                this.Output.Write(" ");
                this.WriteResult(result);
            }

            this.Output.WriteLine(");");

            if (!MainMethod.ReturnType.IsVoid())
            {
                this.Output.WriteLine("ret i32 %1");
            }
            else
            {
                var method = "Int32 System.Environment.get_ExitCode()";
                this.Output.WriteLine("%1 = call i32 @\"{0}\"()", method);
                this.Output.WriteLine("ret i32 %1");

                this.CheckIfMethodExternalDeclarationIsRequired(new SynthesizedMethodStringAdapter("get_ExitCode", "System.Environment", this.ResolveType("System.Int32")));
            }

            this.Output.Indent--;
            this.Output.WriteLine("}");
        }

        private FullyDefinedReference WriteLoadingArgumentsForMain(IMethod currentMethod, IGenericContext currentGenericContext)
        {
            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            MainGen.GetLoadingArgumentsMethodBody(this.MainMethod.ReturnType.IsVoid(), this, out code, out tokenResolutions, out locals, out parameters);

            var mainEntry = new SynthesizedStaticMethod("main", this.MainMethod.DeclaringType, this.MainMethod.ReturnType, this.MainMethod.GetParameters());
            var constructedMethod = MethodBodyBank.GetMethodDecorator(mainEntry, code, tokenResolutions, locals, parameters);

            // actual write
            var opCodes = this.WriteCustomMethodPart(constructedMethod, currentMethod, currentGenericContext);
            return opCodes.First(op => op.Any(Code.Newarr)).Result;
        }

        private IEnumerable<OpCodePart> WriteCustomMethodPart(SynthesizedMethodDecorator constructedMethod, IMethod currentMethod, IGenericContext currentGenericContext)
        {
            Debug.Assert(currentMethod != null, "Please provide current method to restore method context");

            this.ReadMethodInfo(constructedMethod, currentGenericContext);

            var ilReader = new IlReader();
            var baseWriter = new BaseWriter();
            baseWriter.ReadMethodInfo(constructedMethod, currentGenericContext);
            baseWriter.StartProcess();
            foreach (var opCodePart in ilReader.OpCodes(constructedMethod, currentGenericContext, null))
            {
                baseWriter.AddOpCode(opCodePart);
            }

            var rest = baseWriter.PrepareWritingMethodBody();
            foreach (var opCodePart in rest)
            {
                this.ActualWrite(this.Output, opCodePart, true);
                this.Output.WriteLine(string.Empty);
            }

            // restore context
            this.ReadMethodInfo(currentMethod, currentGenericContext);

            return rest;
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
                this.ReadDbgLine(opCodePart);
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
        /// <param name="opCodeConstructorInfoPart">
        /// </param>
        private void WriteNewObject(OpCodeConstructorInfoPart opCodeConstructorInfoPart)
        {
            var declaringType = opCodeConstructorInfoPart.Operand.DeclaringType;
            this.CheckIfExternalDeclarationIsRequired(declaringType);
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
        private void WriteOverflowWithThrow(LlvmIndentedTextWriter writer, OpCodePart opCode, string @operator)
        {
            this.BinaryOper(
                writer,
                opCode,
                string.Concat("call { %R, i1 } @llvm.", @operator, ".with.overflow.%R("),
                OperandOptions.GenerateResult | OperandOptions.AdjustIntTypes | OperandOptions.Template |
                OperandOptions.DetectAndWriteTypeInSecondOperand);
            writer.WriteLine(")");

            var result = opCode.Result;

            var testResult = this.WriteSetResultNumber(opCode, ResolveType("System.Boolean"));
            writer.Write("extractvalue { ");
            result.Type.WriteTypePrefix(writer);
            writer.Write(", i1 } ");
            this.WriteResult(result);
            writer.WriteLine(", 1");

            var returnValue = this.WriteSetResultNumber(opCode, result.Type);
            writer.Write("extractvalue { ");
            result.Type.WriteTypePrefix(writer);
            writer.Write(", i1 } ");
            this.WriteResult(result);
            writer.WriteLine(", 0");

            // throw exception
            this.WriteBranchSwitchToThrowOrPass(
                writer,
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
            LlvmIndentedTextWriter writer,
            FullyDefinedReference result,
            OpCodePart startOpCode,
            OpCodePart endOpCode,
            string label = "a",
            int stopAddress = 0)
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

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        private void WritePostMethodEnd(IMethod method)
        {
            if (!NoBody)
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
            if (MainMethod != null && !this.Gctors)
            {
                this.Output.WriteLine(string.Empty);
                this.WriteCallGctorsDeclarations();
            }

            if (this.typeRttiPointerDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (
                    var rttiPointerDecl in
                        this.typeRttiPointerDeclRequired.Where(rdp => !this.processedRttiPointerTypes.Contains(rdp)))
                {
                    ////rttiPointerDecl.WriteRttiPointerClassInfoExternalDeclaration(this.Output);
                    rttiPointerDecl.WriteRttiPointerClassName(this.Output);
                    rttiPointerDecl.WriteRttiPointerClassInfo(this.Output);
                    this.AddRequiredRttiDeclaration(rttiPointerDecl);
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
                        var rttiDecl in this.typeRttiDeclRequired.ToList().Where(rd => !this.processedRttiTypes.Contains(rd)))
                    {
                        ////rttiDecl.WriteRttiClassInfoExternalDeclaration(this.Output);
                        rttiDecl.WriteRttiClassName(this.Output);
                        rttiDecl.WriteRttiClassInfo(this);
                        this.Output.WriteLine(string.Empty);

                        this.processedRttiTypes.Add(rttiDecl);
                        any = true;
                    }
                } while (any);
            }

            if (this.typeDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (
                    var opaqueType in
                        this.typeDeclRequired.Where(ot => !this.processedTypes.Contains(ot) && !ot.IsArray))
                {
                    this.WriteTypeDeclarationStart(opaqueType.ToClass());
                    this.Output.WriteLine("opaque");
                }
            }

            if (this.methodDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (
                    var methodKey in this.methodDeclRequired.Where(m => !this.processedMethods.Contains(m.Method)))
                {
                    var externalMethodDecl = methodKey.Method;

                    this.Output.Write("declare ");

                    var ctor = externalMethodDecl as IConstructor;
                    if (ctor != null)
                    {
                        ReadMethodInfo(ctor, null);
                        this.Output.Write("void ");
                        this.WriteMethodDefinitionName(this.Output, ctor);
                        this.WriteMethodParamsDef(
                            this.Output,
                            ctor,
                            HasMethodThis,
                            ThisType,
                            ResolveType("System.Void"));
                        this.Output.WriteLine(string.Empty);
                        continue;
                    }

                    var method = externalMethodDecl;
                    if (method != null)
                    {
                        ReadMethodInfo(method, null);
                        this.WriteMethodReturnType(this.Output, method);
                        this.WriteMethodDefinitionName(this.Output, method, methodKey.OwnerOfExplicitInterface);
                        this.WriteMethodParamsDef(this.Output, method, HasMethodThis, methodKey.OwnerOfExplicitInterface ?? ThisType, method.ReturnType);
                        this.Output.WriteLine(string.Empty);
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
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            string op,
            IType requiredType,
            IType resultType,
            OperandOptions options,
            IType effectiveType)
        {
            if ((opCode.OpCode.StackBehaviourPush != StackBehaviour.Push0 ||
                 options.HasFlag(OperandOptions.GenerateResult)) && op != "store")
            {
                var resultOf = ResultOf(opCode);
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
                var type = effectiveType ?? ResolveType("System.Void");
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
                this.Output.Write(
                    "@\"{0}\" = {1} global ",
                    field.GetFullName(),
                    field.DeclaringType.IsGenericType ? "linkonce_odr" : string.Empty);
            }

            field.FieldType.WriteTypePrefix(this.Output, false);
            this.CheckIfExternalDeclarationIsRequired(field.FieldType);

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

            if (!isExternal && this.DebugInfo)
            {
                this.debugInfoGenerator.DefineGlobal(field);
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

            // add type infrastructure
            type.WriteTypeStorageStaticField(this);
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
            Debug.Assert(!type.IsGenericTypeDefinition);
            Debug.Assert(!type.IsArray);

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
                "@.s{0} = private unnamed_addr constant {1} {3} {2}",
                pair.Key,
                this.GetArrayTypeHeader(ResolveType("System.Char"), pair.Value.Length + 1),
                this.GetArrayValuesHeader(ResolveType("System.Char"), pair.Value.Length + 1, pair.Value.Length),
                "{");

            this.Output.Write(" [");

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
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
    using System.Text;

    using Il2Native.Logic.CodeParts;
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
        private static readonly IDictionary<string, int> SystemTypeSizes = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, string> SystemTypesToCTypes = new SortedDictionary<string, string>();

        /// <summary>
        /// </summary>
        private static int methodNumberIncremental;

        /// <summary>
        /// </summary>
        public static int pointerSize = 4;

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> indexByFieldInfo = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IMethod> methodDeclRequired = new HashSet<IMethod>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IMethod> processedMethods = new HashSet<IMethod>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IType> processedTypes = new HashSet<IType>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, int> sizeByType = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private readonly IList<IField> staticFieldsInfo = new List<IField>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<int, string> stringStorage = new SortedDictionary<int, string>();

        /// <summary>
        /// </summary>
        private readonly HashSet<IType> typeDeclRequired = new HashSet<IType>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, List<Pair<string, IMethod>>> virtualInterfaceTableByType =
            new SortedDictionary<string, List<Pair<string, IMethod>>>();

        /// <summary>
        /// </summary>
        private readonly IDictionary<string, List<Pair<string, IMethod>>> virtualTableByType = new SortedDictionary<string, List<Pair<string, IMethod>>>();

        /// <summary>
        /// </summary>
        private int resultNumberIncremental;

        /// <summary>
        /// </summary>
        private int stringIndexIncremental;

        /// <summary>
        /// </summary>
        public bool landingPadVariablesAreWritten;

        /// <summary>
        /// </summary>
        public bool needToWriteUnwindException;

        /// <summary>
        /// </summary>
        static LlvmWriter()
        {
            // to be removed
            SystemTypesToCTypes["String"] = "i8";
            SystemTypesToCTypes["String&"] = "i8*";

            SystemTypesToCTypes["Void"] = "void";
            SystemTypesToCTypes["Byte"] = "i8";
            SystemTypesToCTypes["SByte"] = "i8";
            SystemTypesToCTypes["Char"] = "i16";
            SystemTypesToCTypes["Int16"] = "i16";
            SystemTypesToCTypes["Int32"] = "i32";
            SystemTypesToCTypes["Int64"] = "i64";
            SystemTypesToCTypes["UInt16"] = "i16";
            SystemTypesToCTypes["UInt32"] = "i32";
            SystemTypesToCTypes["UInt64"] = "i64";
            SystemTypesToCTypes["Float"] = "float";
            SystemTypesToCTypes["Single"] = "float";
            SystemTypesToCTypes["Double"] = "double";
            SystemTypesToCTypes["Boolean"] = "i1";
            SystemTypesToCTypes["Byte&"] = "i8*";
            SystemTypesToCTypes["SByte&"] = "i8*";
            SystemTypesToCTypes["Char&"] = "i8*";
            SystemTypesToCTypes["Int16&"] = "i16*";
            SystemTypesToCTypes["Int32&"] = "i32*";
            SystemTypesToCTypes["Int64&"] = "i64*";
            SystemTypesToCTypes["IntPtr"] = "i32*";
            SystemTypesToCTypes["UIntPtr"] = "i32*";
            SystemTypesToCTypes["UInt16&"] = "i16**";
            SystemTypesToCTypes["UInt32&"] = "i32**";
            SystemTypesToCTypes["UInt64&"] = "i64*";
            SystemTypesToCTypes["Float&"] = "float*";
            SystemTypesToCTypes["Single&"] = "float*";
            SystemTypesToCTypes["Double&"] = "double*";
            SystemTypesToCTypes["Boolean&"] = "i1*";

            SystemTypeSizes["Void"] = 0;
            SystemTypeSizes["Byte"] = 1;
            SystemTypeSizes["SByte"] = 1;
            SystemTypeSizes["Char"] = 2;
            SystemTypeSizes["Int16"] = 2;
            SystemTypeSizes["Int32"] = pointerSize;
            SystemTypeSizes["Int64"] = 8;
            SystemTypeSizes["UInt16"] = 2;
            SystemTypeSizes["UInt32"] = pointerSize;
            SystemTypeSizes["UInt64"] = 8;
            SystemTypeSizes["Float"] = pointerSize;
            SystemTypeSizes["Single"] = pointerSize;
            SystemTypeSizes["Double"] = 8;
            SystemTypeSizes["Boolean"] = 1;
            SystemTypeSizes["Byte&"] = pointerSize;
            SystemTypeSizes["SByte&"] = pointerSize;
            SystemTypeSizes["Char&"] = pointerSize;
            SystemTypeSizes["Int16&"] = pointerSize;
            SystemTypeSizes["Int32&"] = pointerSize;
            SystemTypeSizes["Int64&"] = pointerSize;
            SystemTypeSizes["IntPtr"] = pointerSize;
            SystemTypeSizes["UIntPtr"] = pointerSize;
            SystemTypeSizes["UInt16&"] = pointerSize;
            SystemTypeSizes["UInt32&"] = pointerSize;
            SystemTypeSizes["UInt64&"] = pointerSize;
            SystemTypeSizes["Float&"] = pointerSize;
            SystemTypeSizes["Single&"] = pointerSize;
            SystemTypeSizes["Double&"] = pointerSize;
            SystemTypeSizes["Boolean&"] = pointerSize;
        }

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
            this.includeMiniCoreLib = args != null && args.Contains("includeMiniCore");
        }

        /// <summary>
        /// </summary>
        public bool includeMiniCoreLib { get; set; }

        /// <summary>
        /// </summary>
        protected LlvmIndentedTextWriter Output { get; private set; }

        /// <summary>
        /// </summary>
        public void Close()
        {
            this.Output.Close();
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
            ////this.Output.WriteLine("; {0}", opCode.OpCode.Name);
            this.Process(opCode);
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
        /// <param name="disablePostDeclarations">
        /// </param>
        public void WriteAfterFields(int count, bool disablePostDeclarations = false)
        {
            this.Output.WriteLine(string.Empty);

            this.Output.Indent--;
            this.Output.WriteLine("}");

            if (!disablePostDeclarations)
            {
                this.WritePostDeclarations();

                this.ThisType.WriteRtti(this.Output);
            }
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
            var baseType = this.ThisType.BaseType;

            this.CheckIfExternalDeclarationIsRequired(baseType);

            this.Output.WriteLine("{");
            this.Output.Indent++;

            // put virtual root table if type has no any base with virtual types
            if (this.ThisType.IsInterface)
            {
                this.Output.WriteLine("i32 (...)**");
            }
            else if (this.ThisType.IsRootOfVirtualTable())
            {
                this.Output.WriteLine("i32 (...)**");
            }

            if (baseType != null)
            {
                this.WriteTypeWithoutModifiers(this.Output, baseType);
            }

            var index = 0;
            foreach (var @interface in this.ThisType.GetInterfaces())
            {
                if (this.ThisType.BaseType != null && this.ThisType.BaseType.GetInterfaces().Contains(@interface))
                {
                    continue;
                }

                this.CheckIfExternalDeclarationIsRequired(@interface);

                this.Output.WriteLine(index == 0 && baseType == null ? string.Empty : ", ");
                this.WriteTypeWithoutModifiers(this.Output, @interface);
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
        public void WriteConstructorEnd(IConstructor ctor)
        {
            this.WriteMethodBody(string.Empty);

            if (!this.NoBody)
            {
                this.Output.Indent--;

                this.Output.WriteLine("}");
            }

            this.Output.WriteLine(string.Empty);
        }

        public void StartProcess()
        {
            base.StartProcess();
            this.resultNumberIncremental = 0;
            this.landingPadVariablesAreWritten = false;
            this.needToWriteUnwindException = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        public void WriteConstructorStart(IConstructor ctor)
        {
            this.processedMethods.Add(ctor);

            this.StartProcess();
            this.ReadMethodInfo(ctor);

            if (ctor.IsAbstract || ctor.GetMethodBody() == null)
            {
                this.Output.Write("declare ");
            }
            else
            {
                this.Output.Write("define ");
            }

            this.Output.Write("void ");

            // this.WriteMethodName(this.Output, ctor);
            this.WriteMethodDefinitionName(this.Output, ctor);
            if (ctor.IsStatic)
            {
                this.StaticConstructors.Add(ctor);
            }

            this.WriteMethodParamsDef(this.Output, ctor.GetParameters(), this.HasMethodThis, this.ThisType, TypeAdapter.FromType(typeof(void)));

            this.WriteMethodNumber();

            // write local declarations
            var methodBase = ctor.GetMethodBody();
            if (methodBase != null)
            {
                this.Output.WriteLine(" {");
                this.Output.Indent++;
                this.WriteLocalVariableDeclarations(methodBase.LocalVariables);
                this.WriteArgumentCopyDeclarations(ctor.GetParameters(), this.HasMethodThis);
            }

            methodNumberIncremental++;
        }

        /// <summary>
        /// </summary>
        public void WriteEnd()
        {
            if (this.MainMethod != null)
            {
                this.Output.Write("define i32 @main()");

                this.WriteMethodNumber();

                this.Output.WriteLine(" {");

                this.Output.Indent++;

                this.Output.Write("%1 = call i32 ");

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

                    this.Output.Write("i8** null");

                    index++;
                }

                this.Output.WriteLine(");");

                this.Output.WriteLine("ret i32 %1");

                this.Output.Indent--;
                this.Output.WriteLine("}");
            }

            // write global ctors caller
            this.Output.WriteLine(string.Empty);
            this.Output.WriteLine("define internal void @_GLOBAL_CTORS_EXECUTE_() {");
            this.Output.Indent++;
            foreach (var staticCtor in this.StaticConstructors)
            {
                this.Output.WriteLine("call void {0}()", this.GetFullMethodName(staticCtor));
            }

            this.Output.WriteLine("ret void");
            this.Output.Indent--;
            this.Output.WriteLine("}");

            if (!this.includeMiniCoreLib)
            {
                this.WriteRequiredDeclarations();
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
                if (!field.IsLiteral)
                {
                    this.staticFieldsInfo.Add(field);
                }

                return;
            }

            this.Output.WriteLine(',');

            this.WriteTypePrefix(this.Output, field.FieldType, false);

            // this.Output.Write(' ');
            // this.Output.Write(field.Name);
            // this.Output.WriteLine(';');
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
        public void WriteMethodEnd(IMethod method)
        {
            this.WriteMethodBody();

            if (!this.NoBody)
            {
                this.Output.Indent--;

                if (this.needToWriteUnwindException)
                {
                    this.needToWriteUnwindException = false;
                    this.WriteUnwindException(this.Output);
                }

                if (method.ExceptionHandlingClauses.Count() > 0)
                {
                    this.Output.WriteLine(".unreachable:");
                    this.Output.Indent++;
                    this.Output.WriteLine("unreachable");
                    this.Output.Indent--;
                }

                this.Output.WriteLine("}");
            }

            this.Output.WriteLine(string.Empty);

            // write set of strings
            foreach (var pair in this.stringStorage)
            {
                this.Output.WriteLine(
                    string.Format("@.s{0} = private unnamed_addr constant [{2} x i8] c\"{1}\\00\", align 1", pair.Key, pair.Value, pair.Value.Length + 1));
            }

            if (this.stringStorage.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
            }

            this.stringStorage.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        public void WriteMethodStart(IMethod method)
        {
            this.processedMethods.Add(method);

            this.StartProcess();

            var isMain = method.IsStatic && method.CallingConvention.HasFlag(CallingConventions.Standard) && method.Name.Equals("Main");

            // check if main
            if (isMain)
            {
                this.MainMethod = method;
            }

            if (method.IsGenericMethod)
            {
                ////this.Output.Write("template <");
                ////WriteGenericParameters(this.Output, method);
                ////this.Output.Write("> ");
            }

            if (method.IsAbstract || method.GetMethodBody() == null)
            {
                this.Output.Write("declare ");
            }
            else
            {
                this.Output.Write("define ");
            }

            this.ReadMethodInfo(method);

            this.WriteMethodReturnType(this.Output, method);

            this.WriteMethodDefinitionName(this.Output, method);

            this.WriteMethodParamsDef(this.Output, method.GetParameters(), this.HasMethodThis, this.ThisType, method.ReturnType);

            this.WriteMethodNumber();

            // write local declarations
            var methodBodyBytes = method.GetMethodBody();
            if (methodBodyBytes != null)
            {
                this.Output.WriteLine(" {");

                this.Output.Indent++;

                this.WriteLocalVariableDeclarations(methodBodyBytes.LocalVariables);
                this.WriteArgumentCopyDeclarations(method.GetParameters(), this.HasMethodThis);
            }

            methodNumberIncremental++;
        }

        /// <summary>
        /// </summary>
        /// <param name="moduleName">
        /// </param>
        public void WriteStart(string moduleName)
        {
            this.Output.WriteLine(
                "target datalayout = \"e-p:32:32:32-i1:8:8-i8:8:8-i16:16:16-i32:32:32-i64:64:64-f32:32:32-f64:64:64-f80:128:128-v64:64:64-v128:128:128-a0:0:64-f80:32:32-n8:16:32-S32\"");
            this.Output.WriteLine("target triple = \"i686-pc-win32\"");
            this.Output.WriteLine(string.Empty);

            // Global ctors
            this.Output.WriteLine(
                "@llvm.global_ctors = appending global [1 x { i32, void ()* }] [{ i32, void ()* } { i32 65535, void ()* @_GLOBAL_CTORS_EXECUTE_ }]");
            this.Output.WriteLine(string.Empty);

            // declarations
            this.Output.WriteLine(new String(Encoding.ASCII.GetChars(Resources.llvm_declarations)));
            this.Output.WriteLine(string.Empty);

            if (this.includeMiniCoreLib)
            {
                // mini core lib
                this.Output.WriteLine(new String(Encoding.ASCII.GetChars(Resources.llvm_mini_mscore_lib)));
                this.Output.WriteLine(string.Empty);
            }

            this.StaticConstructors.Clear();
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
        /// <param name="genericType">
        /// </param>
        public void WriteTypeStart(IType type, IType genericType)
        {
            this.processedTypes.Add(type);

            if (type.BaseType != null)
            {
                this.WriteTypeDefinitionIfNotWrittenYet(type.BaseType);
            }

            this.staticFieldsInfo.Clear();

            this.ReadTypeInfo(type, genericType);

            if (type.IsGenericType)
            {
                this.Output.Write("template <");
                WriteGenericParameters(this.Output, type);
                this.Output.Write("> ");
            }

            this.WriteTypeDeclarationStart(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetVirtualInterfaceTableName(IType type, IType @interface)
        {
            return string.Concat("@\"", type.FullName, " Virtual Table ", @interface.FullName, " Interface\"");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetVirtualTableName(IType type)
        {
            return string.Concat("@\"", type.FullName, " Virtual Table\"");
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsDirectValue(OpCodePart opCode)
        {
            if (opCode is OpCodeBlock)
            {
                return false;
            }

            if (opCode.Any(Code.Dup))
            {
                return this.IsDirectValue(opCode.OpCodeOperands[0]);
            }

            // TODO: when finish remove Ldtoken from the list of Direct Values and I think Ldstr as well
            return opCode.Any(
                Code.Ldc_I4_0,
                Code.Ldc_I4_1,
                Code.Ldc_I4_2,
                Code.Ldc_I4_3,
                Code.Ldc_I4_4,
                Code.Ldc_I4_5,
                Code.Ldc_I4_6,
                Code.Ldc_I4_7,
                Code.Ldc_I4_8,
                Code.Ldc_I4_M1,
                Code.Ldc_I4,
                Code.Ldc_I4_S,
                Code.Ldc_I8,
                Code.Ldc_R4,
                Code.Ldc_R8,
                Code.Ldstr,
                Code.Ldnull,
                Code.Ldtoken,
                Code.Ldsflda,
                Code.Ldloca,
                Code.Ldloca_S,
                Code.Ldarga,
                Code.Ldarga_S);
        }

        // TODO: you need to compare not only not but return type and parameters types
        private static bool IsOverrideForAnInterface(IMethod interfaceMember, IMethod publicMethod)
        {
            if (interfaceMember.FullName == publicMethod.Name)
            {
                return true;
            }

            if (interfaceMember.Name == publicMethod.Name)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="field">
        /// </param>
        /// <returns>
        /// </returns>
        private static string GetFullFieldName(IField field)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(field.DeclaringType.Namespace))
            {
                sb.Append(field.DeclaringType.Namespace);
                sb.Append('.');
            }

            sb.Append(field.DeclaringType.Name);
            sb.Append('.');
            sb.Append(field.Name);

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="isFloatingPoint">
        /// </param>
        /// <returns>
        /// </returns>
        private static OperandOptions GetOperandOptions(bool isFloatingPoint)
        {
            var operandOptions = OperandOptions.GenerateResult;
            if (isFloatingPoint)
            {
                operandOptions |= OperandOptions.ToFloat;
            }

            return operandOptions;
        }

        /// <summary>
        /// </summary>
        /// <param name="requiredType">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool IsClassCastRequired(IType requiredType, OpCodePart opCodePart)
        {
            return opCodePart.ResultNumber.HasValue && requiredType != opCodePart.ResultType && requiredType.IsAssignableFrom(opCodePart.ResultType);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="asReference">
        /// </param>
        /// <param name="refChar">
        /// </param>
        private static void WriteTypeModifiers(LlvmIndentedTextWriter writer, IType type, bool asReference, char refChar)
        {
            var effectiveType = type;

            do
            {
                var isReference = !effectiveType.IsPrimitive && !effectiveType.IsValueType;
                if ((isReference || asReference) && !effectiveType.IsGenericParameter && !effectiveType.IsArray && !effectiveType.IsByRef)
                {
                    writer.Write(refChar);
                }

                if (effectiveType.IsArray)
                {
                    writer.Write(refChar);
                }

                if (effectiveType.IsByRef)
                {
                    writer.Write(refChar);
                }

                if (effectiveType.HasElementType)
                {
                    effectiveType = effectiveType.GetElementType();
                }
                else
                {
                    break;
                }
            }
            while (effectiveType != null);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="used">
        /// </param>
        /// <param name="parameterInfos">
        /// </param>
        /// <param name="isVirtual">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        /// <param name="isCtor">
        /// </param>
        /// <param name="isDirectValue">
        /// </param>
        /// <param name="resultNumberForThis">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="resultNumberForReturn">
        /// </param>
        /// <param name="returnType">
        /// </param>
        private void ActualWrite(
            LlvmIndentedTextWriter writer,
            OpCodePart[] used,
            IEnumerable<IParameter> parameterInfos,
            bool @isVirtual,
            bool hasThis,
            bool isCtor,
            IList<bool> isDirectValue,
            int? resultNumberForThis,
            IType thisType,
            int? resultNumberForReturn,
            IType returnType)
        {
            writer.Write("(");

            var index = 0;

            var returnIsStruct = returnType != null && returnType.IsStructureType();

            // allocate space for structure if return type is structure
            if (returnIsStruct)
            {
                this.WriteTypePrefix(writer, returnType, returnType.IsStructureType());
                writer.Write(' ');
                if (resultNumberForReturn.HasValue)
                {
                    WriteResultNumber(resultNumberForReturn.Value);
                }
            }

            if (hasThis)
            {
                if (returnIsStruct)
                {
                    writer.Write(", ");
                }

                this.WriteTypePrefix(writer, thisType, thisType.IsStructureType());
                writer.Write(' ');
                if (resultNumberForThis.HasValue)
                {
                    WriteResultNumber(resultNumberForThis.Value);
                }
                else if (used != null && used.Length > 0)
                {
                    if (used[0].ResultNumber.HasValue)
                    {
                        WriteResultNumber(used[0].ResultNumber.Value);
                    }
                    else if (isDirectValue[0])
                    {
                        this.ActualWrite(writer, used[0]);
                    }
                }
            }

            foreach (var parameter in parameterInfos)
            {
                this.CheckIfExternalDeclarationIsRequired(parameter.ParameterType);

                if (hasThis || index > 0 || returnIsStruct)
                {
                    writer.Write(", ");
                }

                var effectiveIndex = index + (@isVirtual || (hasThis && !isCtor) ? 1 : 0);

                var parameterInput = used[effectiveIndex];

                // detect if *(pointer) is required for example structure
                // var parameterInputReturnResult = ResultOf(parameterInput);
                this.WriteTypePrefix(writer, parameter.ParameterType);
                if (parameter.ParameterType.IsStructureType())
                {
                    writer.Write(" byval align " + pointerSize);
                }

                writer.Write(' ');

                if (!isDirectValue[effectiveIndex])
                {
                    WriteResultNumber(used[effectiveIndex]);
                }
                else
                {
                    this.ActualWrite(writer, parameterInput);
                }

                index++;
            }

            writer.Write(")");
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
        private void ActualWrite(LlvmIndentedTextWriter writer, OpCodePart opCode, bool firstLevel = false)
        {
            if (firstLevel)
            {
                this.WriteCaseAndLabels(writer, opCode);
            }

            this.WriteTry(writer, opCode);

            var block = opCode as OpCodeBlock;
            if (block != null)
            {
                this.ActualWriteBlock(writer, block);
            }
            else
            {
                var skip = firstLevel && (this.IsDirectValue(opCode) || opCode.Skip);
                if (!skip)
                {
                    this.ActualWriteOpCode(writer, opCode);
                }
            }

            this.WriteCatchFinnally(writer, opCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="block">
        /// </param>
        private void ActualWriteBlock(LlvmIndentedTextWriter writer, OpCodeBlock block)
        {
            if (block.UseAsConditionalExpression)
            {
                // thos os a hack for return () ? a : b; expressions
                var expressionPart = -1;
                if (block.OpCodes[block.OpCodes.Length - 2].OpCode.FlowControl == FlowControl.Branch)
                {
                    expressionPart = 3;
                }
                else if (block.OpCodes[block.OpCodes.Length - 2].OpCode.FlowControl == FlowControl.Return)
                {
                    expressionPart = 2;
                }

                var lastCond = block.OpCodes.Length - expressionPart;
                for (var i = 0; i < lastCond - 1; i++)
                {
                    var current = block.OpCodes[i];
                    this.ActualWrite(writer, current);
                }

                var opCode1 = block.OpCodes[lastCond - 1];
                opCode1.UseAsConditionalExpression = true;
                var opCode2 = block.OpCodes[block.OpCodes.Length - 1];
                var opCode3 = (expressionPart == 2)
                                  ? block.OpCodes[block.OpCodes.Length - expressionPart].OpCodeOperands[0]
                                  : block.OpCodes[block.OpCodes.Length - expressionPart];

                // custom operand
                var directResult1 = this.PreProcess(writer, opCode1, OperandOptions.None);
                var directResult2 = this.PreProcess(writer, opCode2, OperandOptions.None);
                var directResult3 = this.PreProcess(writer, opCode3, OperandOptions.None);

                this.ProcessOperator(writer, block, "select", TypeAdapter.FromType(typeof(bool)), options: OperandOptions.GenerateResult);

                this.PostProcess(writer, opCode1, directResult1);
                writer.Write(',');
                this.PostProcess(writer, opCode2, directResult2, true);
                writer.Write(',');
                this.PostProcess(writer, opCode3, directResult3, true);

                block.ResultNumber = block.ResultNumber;

                return;
            }

            // just array
            this.ActualWriteBlockBody(writer, block);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="block">
        /// </param>
        /// <param name="skip">
        /// </param>
        /// <param name="reduce">
        /// </param>
        private void ActualWriteBlockBody(LlvmIndentedTextWriter writer, OpCodeBlock block, int skip = 0, int? reduce = null)
        {
            var query = reduce.HasValue ? block.OpCodes.Take(block.OpCodes.Length - reduce.Value).Skip(skip) : block.OpCodes.Skip(skip);

            foreach (var subOpCode in query)
            {
                this.ActualWrite(writer, subOpCode);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void ActualWriteOpCode(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            var code = opCode.ToCode();
            switch (code)
            {
                case Code.Ldc_I4_0:
                    writer.Write("0");
                    break;
                case Code.Ldc_I4_1:
                    writer.Write("1");
                    break;
                case Code.Ldc_I4_2:
                    writer.Write("2");
                    break;
                case Code.Ldc_I4_3:
                    writer.Write("3");
                    break;
                case Code.Ldc_I4_4:
                    writer.Write("4");
                    break;
                case Code.Ldc_I4_5:
                    writer.Write("5");
                    break;
                case Code.Ldc_I4_6:
                    writer.Write("6");
                    break;
                case Code.Ldc_I4_7:
                    writer.Write("7");
                    break;
                case Code.Ldc_I4_8:
                    writer.Write("8");
                    break;
                case Code.Ldc_I4_M1:
                    writer.Write("-1");
                    break;
                case Code.Ldc_I4:
                    var opCodeInt32 = opCode as OpCodeInt32Part;
                    writer.Write(opCodeInt32.Operand);
                    break;
                case Code.Ldc_I4_S:
                    opCodeInt32 = opCode as OpCodeInt32Part;

                    if (opCodeInt32.Operand > 127)
                    {
                        // negative
                        writer.Write(-(256 - opCodeInt32.Operand));
                    }
                    else
                    {
                        writer.Write(opCodeInt32.Operand);
                    }

                    break;
                case Code.Ldc_I8:
                    var opCodeInt64 = opCode as OpCodeInt64Part;
                    writer.Write(opCodeInt64.Operand);
                    break;
                case Code.Ldc_R4:
                    var opCodeSingle = opCode as OpCodeSinglePart;
                    writer.Write(opCodeSingle.Operand.ToString("F"));
                    break;
                case Code.Ldc_R8:
                    var opCodeDouble = opCode as OpCodeDoublePart;
                    writer.Write(opCodeDouble.Operand.ToString("F"));
                    break;
                case Code.Ldstr:
                    var opCodeString = opCode as OpCodeStringPart;
                    writer.Write(
                        string.Format(
                            "getelementptr inbounds ([{1} x i8]* @.s{0}, i32 0, i32 0)",
                            this.GetStringIndex(opCodeString.Operand),
                            opCodeString.Operand.Length + 1));
                    break;
                case Code.Ldnull:
                    writer.Write("null");
                    break;
                case Code.Ldtoken:

                    // TODO: finish loading Token
                    opCodeInt32 = opCode as OpCodeInt32Part;
                    writer.Write("undef");
                    break;
                case Code.Localloc:
                    writer.Write("alloca i32 ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(", align " + pointerSize);
                    break;
                case Code.Ldfld:

                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    var skip = opCodeFieldInfoPart.Operand.FieldType.IsStructureType() && opCode.DestinationName == null;
                    if (!skip)
                    {
                        this.WriteFieldAccess(writer, opCodeFieldInfoPart);
                        writer.WriteLine(string.Empty);
                        this.WriteLlvmLoad(writer, opCode, opCode.ResultType, string.Format("%.r{0}", opCode.ResultNumber ?? -1));
                    }

                    break;
                case Code.Ldflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.WriteFieldAccess(writer, opCodeFieldInfoPart);

                    break;
                case Code.Ldsfld:

                    IType castFrom;
                    var operandType = this.DetectTypePrefix(opCode, null, OperandOptions.TypeIsInOperator, out castFrom);
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.WriteLlvmLoad(writer, opCode, operandType, string.Concat("@\"", GetFullFieldName(opCodeFieldInfoPart.Operand), '"'));

                    break;
                case Code.Ldsflda:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    writer.Write(string.Concat("@\"", GetFullFieldName(opCodeFieldInfoPart.Operand), '"'));

                    break;
                case Code.Stfld:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    var directResult1 = this.PreProcessOperand(writer, opCode, 1);
                    this.WriteFieldAccess(writer, opCodeFieldInfoPart);
                    writer.WriteLine(string.Empty);

                    this.ProcessOperator(writer, opCode, "store", opCodeFieldInfoPart.Operand.FieldType, options: OperandOptions.TypeIsInSecondOperand);

                    this.PostProcessOperand(writer, opCode, 1, directResult1);
                    writer.Write(", ");

                    this.WriteTypePrefix(writer, opCode.ResultType);

                    // add reference
                    writer.Write("* ");

                    WriteResultNumber(opCode.ResultNumber ?? -1);

                    break;
                case Code.Stsfld:

                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;

                    directResult1 = this.PreProcessOperand(writer, opCode, 0);

                    operandType = opCodeFieldInfoPart.Operand.FieldType;

                    this.ProcessOperator(writer, opCode, "store", operandType, options: OperandOptions.TypeIsInSecondOperand);

                    this.PostProcessOperand(writer, opCode, 0, directResult1);
                    writer.Write(", ");

                    this.WriteTypePrefix(writer, operandType);

                    // add reference
                    writer.Write("* ");

                    writer.Write(string.Concat("@\"", GetFullFieldName(opCodeFieldInfoPart.Operand), '"'));
                    break;

                case Code.Ldobj:

                    var opCodeTypePart = opCode as OpCodeTypePart;

                    directResult1 = this.PreProcessOperand(writer, opCode, 0);

                    // opCode.OpCodeOperands[0].DestinationName = this.GetResultNumber(opCode.OpCodeOperands[0].ResultNumber ?? -1);
                    this.WriteLlvmLoad(writer, opCode, opCodeTypePart.Operand, this.GetResultNumber(opCode.OpCodeOperands[0].ResultNumber ?? -1));

                    break;

                case Code.Stobj:
                    opCodeTypePart = opCode as OpCodeTypePart;

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.WriteLine(string.Empty);
                    opCode.OpCodeOperands[1].DestinationName = this.GetResultNumber(opCode.OpCodeOperands[0].ResultNumber ?? -1);
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);

                    break;
                case Code.Ldlen:
                    this.WriteArrayGetLength(writer, opCode);
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
                case Code.Ldelema:

                    this.BinaryOper(writer, opCode, "getelementptr inbounds", options: OperandOptions.DetectTypeInSecondOperand);

                    var actualLoad = true;
                    IType type = null;
                    switch (opCode.ToCode())
                    {
                        case Code.Ldelem:
                        case Code.Ldelem_I:
                            type = TypeAdapter.FromType(typeof(int));
                            break;
                        case Code.Ldelem_I1:
                            type = TypeAdapter.FromType(typeof(byte));
                            break;
                        case Code.Ldelem_I2:
                            type = TypeAdapter.FromType(typeof(short));
                            break;
                        case Code.Ldelem_I4:
                            type = TypeAdapter.FromType(typeof(int));
                            break;
                        case Code.Ldelem_I8:
                            type = TypeAdapter.FromType(typeof(long));
                            break;
                        case Code.Ldelem_R4:
                            type = TypeAdapter.FromType(typeof(float));
                            break;
                        case Code.Ldelem_R8:
                            type = TypeAdapter.FromType(typeof(double));
                            break;
                        case Code.Ldelem_Ref:
                            type = this.GetTypeOfReference(opCode);
                            break;
                        case Code.Ldelema:
                            actualLoad = false;
                            break;
                    }

                    if (actualLoad)
                    {
                        writer.WriteLine(string.Empty);
                        this.WriteLlvmLoad(writer, opCode, type, this.GetResultNumber(opCode.ResultNumber ?? -1));
                    }

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

                    this.BinaryOper(writer, opCode, "getelementptr inbounds", options: OperandOptions.GenerateResult | OperandOptions.DetectTypeInSecondOperand);
                    writer.WriteLine(string.Empty);

                    type = null;
                    switch (opCode.ToCode())
                    {
                        case Code.Stelem:
                        case Code.Stelem_I:
                            type = TypeAdapter.FromType(typeof(int));
                            break;
                        case Code.Stelem_I1:
                            type = TypeAdapter.FromType(typeof(byte));
                            break;
                        case Code.Stelem_I2:
                            type = TypeAdapter.FromType(typeof(short));
                            break;
                        case Code.Stelem_I4:
                            type = TypeAdapter.FromType(typeof(int));
                            break;
                        case Code.Stelem_I8:
                            type = TypeAdapter.FromType(typeof(long));
                            break;
                        case Code.Stelem_R4:
                            type = TypeAdapter.FromType(typeof(float));
                            break;
                        case Code.Stelem_R8:
                            type = TypeAdapter.FromType(typeof(double));
                            break;
                        case Code.Stelem_Ref:
                            type = this.GetTypeOfReference(opCode);
                            break;
                    }

                    directResult1 = this.PreProcessOperand(writer, opCode, 2);
                    this.ProcessOperator(writer, opCode, "store", type);
                    this.PostProcessOperand(writer, opCode, 2, directResult1);
                    writer.Write(", ");
                    this.WriteTypePrefix(writer, type, type.IsStructureType());
                    writer.Write(string.Format("* {0}", this.GetResultNumber(opCode.ResultNumber ?? -1)));

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

                    type = null;
                    switch (opCode.ToCode())
                    {
                        case Code.Ldind_I:
                            type = TypeAdapter.FromType(typeof(int));
                            break;
                        case Code.Ldind_I1:
                        case Code.Ldind_U1:
                            type = TypeAdapter.FromType(typeof(byte));
                            break;
                        case Code.Ldind_I2:
                        case Code.Ldind_U2:
                            type = TypeAdapter.FromType(typeof(short));
                            break;
                        case Code.Ldind_I4:
                        case Code.Ldind_U4:
                            type = TypeAdapter.FromType(typeof(int));
                            break;
                        case Code.Ldind_I8:
                            type = TypeAdapter.FromType(typeof(long));
                            break;
                        case Code.Ldind_R4:
                            type = TypeAdapter.FromType(typeof(float));
                            break;
                        case Code.Ldind_R8:
                            type = TypeAdapter.FromType(typeof(double));
                            break;
                        case Code.Ldind_Ref:
                            type = this.GetTypeOfReference(opCode);
                            break;
                    }

                    directResult1 = this.PreProcessOperand(writer, opCode, 0);
                    this.WriteLlvmLoad(writer, opCode, type, this.GetResultNumber(opCode.OpCodeOperands[0].ResultNumber ?? -1));

                    break;
                case Code.Stind_I:
                case Code.Stind_I1:
                case Code.Stind_I2:
                case Code.Stind_I4:
                case Code.Stind_I8:
                case Code.Stind_R4:
                case Code.Stind_R8:
                case Code.Stind_Ref:

                    type = null;
                    switch (opCode.ToCode())
                    {
                        case Code.Stind_I:
                            type = TypeAdapter.FromType(typeof(int));
                            break;
                        case Code.Stind_I1:
                            type = TypeAdapter.FromType(typeof(byte));
                            break;
                        case Code.Stind_I2:
                            type = TypeAdapter.FromType(typeof(short));
                            break;
                        case Code.Stind_I4:
                            type = TypeAdapter.FromType(typeof(int));
                            break;
                        case Code.Stind_I8:
                            type = TypeAdapter.FromType(typeof(long));
                            break;
                        case Code.Stind_R4:
                            type = TypeAdapter.FromType(typeof(float));
                            break;
                        case Code.Stind_R8:
                            type = TypeAdapter.FromType(typeof(double));
                            break;
                        case Code.Stind_Ref:
                            type = this.GetTypeOfReference(opCode);
                            break;
                    }

                    directResult1 = this.PreProcessOperand(writer, opCode, 0);

                    this.UnaryOper(writer, opCode, 1, "store", type);
                    writer.Write(", ");
                    this.WriteTypePrefix(writer, opCode.OpCodeOperands[0].ResultType);
                    this.PostProcessOperand(writer, opCode, 0, directResult1);

                    break;
                case Code.Call:
                case Code.Callvirt:
                    var opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;
                    var methodBase = opCodeMethodInfoPart.Operand;
                    this.WriteCall(
                        writer,
                        opCodeMethodInfoPart,
                        methodBase,
                        code == Code.Callvirt,
                        methodBase.CallingConvention.HasFlag(CallingConventions.HasThis),
                        false,
                        null);
                    break;
                case Code.Add:
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                    var isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fadd" : "add", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.Mul:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fmul" : "mul", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.Sub:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fsub" : "sub", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.Div:
                case Code.Div_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fdiv" : "sdiv", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.Rem:
                case Code.Rem_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "frem" : "srem", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.And:
                    this.BinaryOper(writer, opCode, "and");
                    break;
                case Code.Or:
                    this.BinaryOper(writer, opCode, "or");
                    break;
                case Code.Xor:
                    this.BinaryOper(writer, opCode, "xor");
                    break;
                case Code.Shl:
                    this.BinaryOper(writer, opCode, "shl");
                    break;
                case Code.Shr:
                case Code.Shr_Un:
                    this.BinaryOper(writer, opCode, "shr");
                    break;
                case Code.Not:
                    var tempOper = opCode.OpCodeOperands;
                    opCode.OpCodeOperands = new[] { tempOper[0], new OpCodePart(OpCodesEmit.Ldc_I4_M1, 0, 0) };
                    this.BinaryOper(writer, opCode, "xor");
                    opCode.OpCodeOperands = tempOper;
                    break;
                case Code.Neg:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    tempOper = opCode.OpCodeOperands;
                    opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldc_I4_0, 0, 0), tempOper[0] };
                    this.BinaryOper(
                        writer, opCode, isFloatingPoint ? "fsub" : "sub", options: GetOperandOptions(isFloatingPoint) | OperandOptions.TypeIsInSecondOperand);
                    opCode.OpCodeOperands = tempOper;
                    break;
                case Code.Dup:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Box:
                    writer.WriteLine("; Boxing");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Unbox:
                case Code.Unbox_Any:
                    writer.WriteLine("; Unboxing");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Ret:

                    var opts = OperandOptions.None;

                    if (this.MethodReturnType.IsStructureType())
                    {
                        var operands = opCode.OpCodeOperands;
                        var opCodeOperand = operands[0];
                        opCodeOperand.DestinationName = "%agg.result";
                        opCodeOperand.DestinationType = this.MethodReturnType;

                        opts |= OperandOptions.IgnoreOperand;
                    }

                    this.UnaryOper(writer, opCode, "ret", this.MethodReturnType, opts);

                    if (this.MethodReturnType.IsStructureType())
                    {
                        writer.Write("void");
                    }

                    break;
                case Code.Stloc:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                case Code.Stloc_S:

                    code = opCode.ToCode();
                    var asString = code.ToString();
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
                        opCode.OpCodeOperands[0].DestinationName = this.GetLocalVarName(index);
                        this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    }
                    else
                    {
                        this.UnaryOper(writer, opCode, "store", localType);
                        writer.Write(", ");
                        this.WriteLlvmLocalVarAccess(writer, index, true);
                    }

                    break;
                case Code.Ldloc:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldloc_S:
                    asString = code.ToString();

                    if (code == Code.Ldloc_S || code == Code.Ldloc)
                    {
                        index = (opCode as OpCodeInt32Part).Operand;
                    }
                    else
                    {
                        index = int.Parse(asString.Substring(asString.Length - 1));
                    }

                    skip = this.LocalInfo[index].LocalType.IsStructureType() && opCode.DestinationName == null;
                    if (!skip)
                    {
                        this.WriteLlvmLoad(writer, opCode, this.LocalInfo[index].LocalType, string.Concat("%local", index));
                    }

                    break;
                case Code.Ldloca:
                case Code.Ldloca_S:

                    index = (opCode as OpCodeInt32Part).Operand;

                    // alloca generate pointer so we do not need to load value from pointer
                    writer.Write(string.Concat("%local", index));

                    break;
                case Code.Ldarg:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldarg_S:
                    asString = code.ToString();
                    index = 0;
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
                        this.WriteLlvmLoad(writer, opCode, this.ThisType, "%.this", structAsRef: this.ThisType.IsStructureType());
                    }
                    else
                    {
                        var parameter = this.Parameters[index - (this.HasMethodThis ? 1 : 0)];

                        skip = parameter.ParameterType.IsStructureType() && opCode.DestinationName == null;
                        if (!skip)
                        {
                            this.WriteLlvmLoad(writer, opCode, parameter.ParameterType, string.Concat("%.", parameter.Name));
                        }
                    }

                    break;

                case Code.Ldarga:
                case Code.Ldarga_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;

                    if (this.HasMethodThis && index == 0)
                    {
                        writer.Write("%.this");
                    }
                    else
                    {
                        var parameter = this.Parameters[index - (this.HasMethodThis ? 1 : 0)];
                        writer.Write(string.Concat("%.", parameter.Name));
                    }

                    break;

                case Code.Starg:
                case Code.Starg_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;
                    var actualIndex = index - (this.HasMethodThis ? 1 : 0);
                    this.UnaryOper(writer, opCode, "store", this.Parameters[actualIndex].ParameterType);
                    writer.Write(", ");
                    this.WriteLlvmArgVarAccess(writer, index - (this.HasMethodThis ? 1 : 0), true);

                    break;

                case Code.Ldftn:

                    // bitcast (i8* (i8*)* @"[I]System.String fn(System.String)" to i32*)
                    opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;

                    writer.Write("bitcast (");
                    this.WriteMethodPointerType(writer, opCodeMethodInfoPart.Operand);
                    writer.Write(" ");
                    writer.Write(this.GetFullMethodName(opCodeMethodInfoPart.Operand));
                    writer.Write(" to i32*)");

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

                    this.BinaryOper(writer, opCode, oper, GetOperandOptions(isFloatingPoint));
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

                    var forTure = opCode.Any(Code.Brtrue, Code.Brtrue_S) ? "ne" : "eq";
                    var resultOf = this.ResultOf(opCode.OpCodeOperands[0]);

                    this.UnaryOper(writer, opCode, "icmp " + forTure, options: OperandOptions.GenerateResult);

                    if (resultOf.IType.IsValueType())
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
                case Code.Ceq:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fcmp oeq" : "icmp eq", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.Clt:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fcmp olt" : "icmp slt", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.Clt_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fcmp ult" : "icmp ult", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.Cgt:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fcmp ogt" : "icmp sgt", GetOperandOptions(isFloatingPoint));
                    break;
                case Code.Cgt_Un:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    this.BinaryOper(writer, opCode, isFloatingPoint ? "fcmp ugt" : "icmp ugt", GetOperandOptions(isFloatingPoint));
                    break;

                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                    this.UnaryOper(writer, opCode, "trunc");
                    writer.Write(" to i32");
                    break;

                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                    this.UnaryOper(writer, opCode, "trunc");
                    writer.Write(" to u32");
                    break;

                case Code.Conv_R_Un:
                    resultOf = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (resultOf.IType.TypeNotEquals(typeof(double)))
                    {
                        this.UnaryOper(writer, opCode, "sitofp");
                        writer.Write(" to double");
                    }
                    else
                    {
                        opCode.ResultNumber = opCode.OpCodeOperands[0].ResultNumber;
                        opCode.ResultType = opCode.OpCodeOperands[0].ResultType;
                    }

                    break;

                case Code.Conv_R4:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    resultOf = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (resultOf.IType.TypeNotEquals(typeof(float)) && resultOf.IType.TypeNotEquals(typeof(Single)))
                    {
                        this.UnaryOper(writer, opCode, isFloatingPoint ? "fptrunc" : "sitofp");
                        writer.Write(" to float");
                    }
                    else
                    {
                        opCode.ResultNumber = opCode.OpCodeOperands[0].ResultNumber;
                        opCode.ResultType = opCode.OpCodeOperands[0].ResultType;
                    }

                    break;
                case Code.Conv_R8:
                    isFloatingPoint = this.IsFloatingPointOp(opCode);
                    resultOf = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (resultOf.IType.TypeNotEquals(typeof(double)))
                    {
                        this.UnaryOper(writer, opCode, isFloatingPoint ? "fpext" : "sitofp");
                        writer.Write(" to double");
                    }
                    else
                    {
                        opCode.ResultNumber = opCode.OpCodeOperands[0].ResultNumber;
                        opCode.ResultType = opCode.OpCodeOperands[0].ResultType;
                    }

                    break;

                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:

                    resultOf = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (resultOf.IType.TypeNotEquals(typeof(byte)) && resultOf.IType.TypeNotEquals(typeof(SByte)))
                    {
                        this.UnaryOper(writer, opCode, "trunc");
                        writer.Write(" to i8");
                    }
                    else
                    {
                        opCode.ResultNumber = opCode.OpCodeOperands[0].ResultNumber;
                        opCode.ResultType = opCode.OpCodeOperands[0].ResultType;
                    }

                    break;

                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:

                    resultOf = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (resultOf.IType.TypeNotEquals(typeof(short)))
                    {
                        this.UnaryOper(writer, opCode, "trunc");
                        writer.Write(" to i16");
                    }
                    else
                    {
                        opCode.ResultNumber = opCode.OpCodeOperands[0].ResultNumber;
                        opCode.ResultType = opCode.OpCodeOperands[0].ResultType;
                    }

                    break;

                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:

                    resultOf = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (resultOf.IType.TypeNotEquals(typeof(int)))
                    {
                        this.UnaryOper(writer, opCode, "trunc");
                        writer.Write(" to i32");
                    }
                    else
                    {
                        opCode.ResultNumber = opCode.OpCodeOperands[0].ResultNumber;
                        opCode.ResultType = opCode.OpCodeOperands[0].ResultType;
                    }

                    break;

                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                    this.UnaryOper(writer, opCode, "zext");
                    writer.Write(" to i64");
                    break;

                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                    this.UnaryOper(writer, opCode, "trunc");
                    writer.Write(" to i8");
                    break;

                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                    this.UnaryOper(writer, opCode, "trunc");
                    writer.Write(" to i16");
                    break;

                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                    this.UnaryOper(writer, opCode, "trunc");
                    writer.Write(" to i32");
                    break;

                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:

                    this.UnaryOper(writer, opCode, "zext");
                    writer.Write(" to i64");
                    break;

                case Code.Castclass:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.WriteLine(string.Empty);
                    this.WriteCast(
                        writer,
                        opCodeTypePart,
                        opCodeTypePart.OpCodeOperands[0].ResultType,
                        opCodeTypePart.OpCodeOperands[0].ResultNumber ?? -1,
                        opCodeTypePart.Operand);

                    break;

                case Code.Isinst:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.WriteLine(string.Empty);
                    this.WriteCast(
                        writer,
                        opCodeTypePart,
                        opCodeTypePart.OpCodeOperands[0].ResultType,
                        opCodeTypePart.OpCodeOperands[0].ResultNumber ?? -1,
                        opCodeTypePart.Operand);

                    break;

                case Code.Newobj:

                    var opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;
                    var declaringType = opCodeConstructorInfoPart.Operand.DeclaringType;

                    this.WriteNew(writer, opCode, declaringType);
                    this.WriteCallConstructor(writer, opCodeConstructorInfoPart);

                    break;

                case Code.Newarr:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    this.WriteNewArray(writer, opCode, opCodeTypePart.Operand, opCode.OpCodeOperands[0]);

                    break;

                case Code.Initobj:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    ////this.WriteNew(writer, opCode, opCodeTypePart.Operand);
                    writer.Write("; Initobj - TODO: finish");

                    break;

                case Code.Throw:

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.WriteLine(string.Empty);
                    this.WriteThrow(writer, opCode);

                    break;

                case Code.Rethrow:

                    writer.Write("throw");

                    break;

                case Code.Leave:
                case Code.Leave_S:
                case Code.Endfilter:
                case Code.Endfinally:
                    break;

                case Code.Pop:
                    break;

                case Code.Constrained:

                    // to solve the problem with referencing ValueType and Class type in Generic type
                    break;

                case Code.Switch:

                    var opCodeLabels = opCode as OpCodeLabelsPart;

                    this.UnaryOper(writer, opCode, "switch");

                    index = 0;
                    writer.Write(", label %.a{0} [ ", opCode.GroupAddressEnd);

                    foreach (var label in opCodeLabels.Operand)
                    {
                        writer.Write("i32 {0}, label %.a{1} ", index, opCodeLabels.JumpAddress(index++));
                    }

                    writer.WriteLine("]", opCode.GroupAddressEnd);

                    writer.Indent--;
                    writer.Write(string.Concat(".a", opCode.GroupAddressEnd, ':'));
                    writer.Indent++;

                    opCode.NextOpCode(this).JumpProcessed = true;

                    break;
            }
        }

        private void WriteCallConstructor(LlvmIndentedTextWriter writer, OpCodeConstructorInfoPart opCodeConstructorInfoPart)
        {
            if (opCodeConstructorInfoPart.ResultNumber.HasValue)
            {
                return;
            }

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Call Constructor");
            var methodBase = opCodeConstructorInfoPart.Operand;
            var resAlloc = opCodeConstructorInfoPart.ResultNumber;
            opCodeConstructorInfoPart.ResultNumber = null;
            this.WriteCall(writer, opCodeConstructorInfoPart, methodBase, opCodeConstructorInfoPart.ToCode() == Code.Callvirt, true, true, resAlloc);
            opCodeConstructorInfoPart.ResultNumber = resAlloc;
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
        private void BinaryOper(LlvmIndentedTextWriter writer, OpCodePart opCode, string op, OperandOptions options = OperandOptions.None)
        {
            if (opCode.ResultNumber.HasValue)
            {
                return;
            }

            var directResult1 = this.PreProcessOperand(writer, opCode, 0, options);
            var directResult2 = this.PreProcessOperand(writer, opCode, 1, options);

            this.ProcessOperator(writer, opCode, op, options: options);

            this.PostProcessOperand(writer, opCode, 0, directResult1);
            writer.Write(',');
            this.PostProcessOperand(writer, opCode, 1, directResult2, options.HasFlag(OperandOptions.DetectTypeInSecondOperand));
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <param name="virtualTable">
        /// </param>
        private void BuildVirtualInterfaceTable(IType thisType, IType @interface, List<Pair<string, IMethod>> virtualTable)
        {
            var allPublic = thisType.GetMethods(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

            // get all virtual methods in current type and replace or append
            virtualTable.AddRange(
                IlReader.Methods(@interface)
                        .Select(interfaceMember => allPublic.First(pub => IsOverrideForAnInterface(interfaceMember, pub)))
                        .Select(foundMethod => new Pair<string, IMethod> { Key = foundMethod.ToString(), Value = foundMethod }));
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="virtualTable">
        /// </param>
        private void BuildVirtualTable(IType thisType, List<Pair<string, IMethod>> virtualTable)
        {
            if (thisType.BaseType != null)
            {
                this.BuildVirtualTable(thisType.BaseType, virtualTable);
            }

            // get all virtual methods in current type and replace or append
            foreach (var virtualOrAbstractMethod in IlReader.Methods(thisType).Where(m => m.IsVirtual || m.IsAbstract))
            {
                this.CheckIfExternalDeclarationIsRequired(virtualOrAbstractMethod);

                if (virtualOrAbstractMethod.IsAbstract)
                {
                    virtualTable.Add(new Pair<string, IMethod> { Key = virtualOrAbstractMethod.ToString(), Value = virtualOrAbstractMethod });
                    continue;
                }

                // find method in virtual table
                var baseMethod = virtualTable.FirstOrDefault(m => m.Key == virtualOrAbstractMethod.ToString());
                if (baseMethod != null)
                {
                    baseMethod.Value = virtualOrAbstractMethod;
                    continue;
                }

                virtualTable.Add(new Pair<string, IMethod> { Key = virtualOrAbstractMethod.ToString(), Value = virtualOrAbstractMethod });
            }
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

            // add shift for virtual table
            if (type.IsRootOfVirtualTable())
            {
                index++;
            }

            // add shift for base type
            if (type.BaseType != null)
            {
                index++;
            }

            // add shift for interfaces
            if (type.BaseType == null)
            {
                index += type.GetInterfaces().Count();
            }
            else
            {
                var baseInterfaces = type.BaseType.GetInterfaces();
                index += type.GetInterfaces().Count(i => !baseInterfaces.Contains(i));
            }

            this.indexByFieldInfo[GetFullFieldName(fieldInfo)] = index;

            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private int CalculateSize(IType type)
        {
            if (type.IsInterface)
            {
                // i8** (...)
                return pointerSize;
            }

            if (type.IsEnum)
            {
                return this.GetTypeSize(type.GetEnumUnderlyingType());
            }

            var size = 0;

            // add shift for virtual table
            if (type.IsRootOfVirtualTable())
            {
                size += pointerSize;
            }

            if (type.BaseType != null)
            {
                size += this.GetTypeSize(type.BaseType);
            }

            // add shift for interfaces
            if (type.BaseType == null)
            {
                size += type.GetInterfaces().Count() * pointerSize;
            }
            else
            {
                var baseInterfaces = type.BaseType.GetInterfaces();
                size += type.GetInterfaces().Count(i => !baseInterfaces.Contains(i)) * pointerSize;
            }

            foreach (var field in IlReader.Fields(type).Where(t => !t.IsStatic).ToList())
            {
                if (field.FieldType.IsStructureType())
                {
                    size += this.GetTypeSize(field.FieldType);
                }

                var fieldSize = 0;
                if (field.FieldType.IsClass)
                {
                    // pointer size
                    size += pointerSize;
                }
                else if (field.FieldType.Namespace == "System" && SystemTypeSizes.TryGetValue(field.FieldType.Name, out fieldSize))
                {
                    size += fieldSize;
                }
                else
                {
                    size += this.GetTypeSize(field.FieldType);
                }
            }

            this.sizeByType[type.FullName] = size;

            return size;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodBase">
        /// </param>
        private void CheckIfExternalDeclarationIsRequired(IMethod methodBase)
        {
            var mi = methodBase;
            if (mi != null)
            {
                if (mi.DeclaringType.AssemblyQualifiedName != this.ThisType.AssemblyQualifiedName)
                {
                    this.methodDeclRequired.Add(methodBase);
                }

                return;
            }

            var ci = methodBase as IConstructor;
            if (ci != null)
            {
                if (ci.DeclaringType.AssemblyQualifiedName != this.ThisType.AssemblyQualifiedName)
                {
                    this.methodDeclRequired.Add(methodBase);
                }

                return;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void CheckIfExternalDeclarationIsRequired(IType type)
        {
            if (type != null)
            {
                if (type.AssemblyQualifiedName != this.ThisType.AssemblyQualifiedName)
                {
                    this.typeDeclRequired.Add(type);
                }
            }
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
        /// <returns>
        /// </returns>
        private IType DetectTypePrefix(OpCodePart opCode, IType requiredType, OperandOptions options, out IType castFrom)
        {
            castFrom = null;

            var res1 = options.HasFlag(OperandOptions.TypeIsInOperator)
                           ? this.ResultOf(opCode)
                           : opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0 ? this.ResultOf(opCode.OpCodeOperands[0]) : null;

            var res2 = opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 1 ? this.ResultOf(opCode.OpCodeOperands[1]) : null;

            // write type
            var effectiveType = TypeAdapter.FromType(typeof(void));

            if (requiredType != null)
            {
                effectiveType = requiredType;
            }
            else if (options.HasFlag(OperandOptions.TypeIsInOperator) || opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0)
            {
                if (!options.HasFlag(OperandOptions.TypeIsInSecondOperand) || res2 == null || (res2.IsConst ?? false))
                {
                    effectiveType = res1.IType;
                }
                else
                {
                    effectiveType = res2.IType;
                }
            }

            if (res1 != null && res1.IType != effectiveType && res1.IType.IsClass && effectiveType.IsAssignableFrom(res1.IType))
            {
                castFrom = res1.IType;
            }

            return effectiveType;
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetArgVarName(int index)
        {
            return string.Concat("%.", this.Parameters[index].Name);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodBase">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetFullMethodName(IMethod methodBase, IType ownerOfExplicitInterface = null)
        {
            var methodName = methodBase.ToString();

            var sb = new StringBuilder();
            sb.Append("@\"");

            if (ownerOfExplicitInterface != null)
            {
                var lookupTypeName = string.Concat(' ', methodBase.DeclaringType.Name, '.', methodBase.Name, '(');
                sb.Append(methodName.Insert(methodName.IndexOf(lookupTypeName) + 1, string.Concat(ownerOfExplicitInterface.FullName, '.')));
            }
            else
            {
                sb.Append(methodName);
            }

            sb.Append('"');

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        private string GetLocalVarName(int index)
        {
            return string.Concat("%local", index);
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <returns>
        /// </returns>
        public string GetResultNumber(int number)
        {
            return string.Concat("%.r", number);
        }

        /// <summary>
        /// </summary>
        /// <param name="str">
        /// </param>
        /// <returns>
        /// </returns>
        private int GetStringIndex(string str)
        {
            var idx = ++this.stringIndexIncremental;
            this.stringStorage[idx] = str;
            return idx;
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
            if (opCode.ResultType != null)
            {
                type = opCode.ResultType;
            }
            else if (opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0 && opCode.OpCodeOperands[0].ResultType != null)
            {
                type = opCode.OpCodeOperands[0].ResultType;
            }
            else
            {
                type = TypeAdapter.FromType(typeof(byte*));
            }

            if (type.IsArray || type.IsByRef)
            {
                return type.GetElementType();
            }

            return type;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private int GetTypeSize(IType type)
        {
            // find index
            int size;
            if (!this.sizeByType.TryGetValue(type.FullName, out size))
            {
                size = this.CalculateSize(type);
            }

            return size;
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <returns>
        /// </returns>
        private List<Pair<string, IMethod>> GetVirtualInterfaceTable(IType thisType, IType @interface)
        {
            List<Pair<string, IMethod>> virtualInterfaceTable;

            if (this.virtualInterfaceTableByType.TryGetValue(string.Concat(thisType.FullName, '+', @interface.FullName), out virtualInterfaceTable))
            {
                return virtualInterfaceTable;
            }

            virtualInterfaceTable = new List<Pair<string, IMethod>>();
            this.BuildVirtualInterfaceTable(thisType, @interface, virtualInterfaceTable);

            this.virtualInterfaceTableByType[thisType.FullName] = virtualInterfaceTable;

            return virtualInterfaceTable;
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        private int GetVirtualMethodIndex(IType thisType, IMethod methodInfo)
        {
            var virtualTable = this.GetVirtualTable(thisType);

            var index = 0;
            foreach (var virtualMethod in virtualTable.Select(v => v.Value))
            {
                if (virtualMethod.ToString() == methodInfo.ToString())
                {
                    return index;
                }

                index++;
            }

            throw new KeyNotFoundException("virtual method could not be found");
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <returns>
        /// </returns>
        private List<Pair<string, IMethod>> GetVirtualTable(IType thisType)
        {
            List<Pair<string, IMethod>> virtualTable;

            if (this.virtualTableByType.TryGetValue(thisType.FullName, out virtualTable))
            {
                return virtualTable;
            }

            virtualTable = new List<Pair<string, IMethod>>();
            this.BuildVirtualTable(thisType, virtualTable);

            this.virtualTableByType[thisType.FullName] = virtualTable;

            return virtualTable;
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
            if (op1ReturnResult == null || op1ReturnResult.IType == null)
            {
                return false;
            }

            var op1IsReal = op1ReturnResult.IType.IsReal();
            return op1IsReal;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="operand">
        /// </param>
        /// <param name="directResult">
        /// </param>
        /// <param name="detectAndWriteTypePrefix">
        /// </param>
        private void PostProcess(LlvmIndentedTextWriter writer, OpCodePart operand, bool directResult, bool detectAndWriteTypePrefix = false)
        {
            writer.Write(' ');

            if (directResult)
            {
                if (detectAndWriteTypePrefix)
                {
                    IType castFrom;
                    var effectiveType = this.DetectTypePrefix(operand, null, OperandOptions.TypeIsInOperator, out castFrom);
                    this.WriteTypePrefix(writer, effectiveType ?? TypeAdapter.FromType(typeof(void)));
                    writer.Write(' ');
                }

                this.ActualWrite(writer, operand);
            }
            else
            {
                if (detectAndWriteTypePrefix)
                {
                    this.WriteTypePrefix(writer, operand.ResultType ?? TypeAdapter.FromType(typeof(void)));
                    writer.Write(' ');
                }

                WriteResultNumber(operand.ResultNumber ?? -1);
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
        /// <param name="directResult">
        /// </param>
        /// <param name="detectAndWriteTypePrefix">
        /// </param>
        private void PostProcessOperand(LlvmIndentedTextWriter writer, OpCodePart opCode, int index, bool directResult, bool detectAndWriteTypePrefix = false)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            var operand = opCode.OpCodeOperands[index];
            this.PostProcess(writer, operand, directResult, detectAndWriteTypePrefix);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="operandOpCode">
        /// </param>
        /// <param name="options">
        /// </param>
        /// <returns>
        /// </returns>
        private bool PreProcess(LlvmIndentedTextWriter writer, OpCodePart operandOpCode, OperandOptions options = OperandOptions.None)
        {
            if (!this.IsDirectValue(operandOpCode))
            {
                if (!operandOpCode.ResultNumber.HasValue)
                {
                    this.ActualWrite(writer, operandOpCode);
                    writer.WriteLine(string.Empty);
                }

                return false;
            }

            return true;
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
        /// <returns>
        /// </returns>
        private bool PreProcessOperand(LlvmIndentedTextWriter writer, OpCodePart opCode, int index, OperandOptions options = OperandOptions.None)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return false;
            }

            var operandOpCode = opCode.OpCodeOperands[index];
            return this.PreProcess(writer, operandOpCode, options: options);
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
        /// <param name="options">
        /// </param>
        private void ProcessOperator(
            LlvmIndentedTextWriter writer, OpCodePart opCode, string op, IType requiredType = null, OperandOptions options = OperandOptions.None)
        {
            IType castFrom;
            var effectiveType = this.DetectTypePrefix(opCode, requiredType, options, out castFrom);
            if (castFrom != null && opCode.OpCodeOperands[0].ResultNumber.HasValue)
            {
                this.WriteCast(writer, opCode.OpCodeOperands[0], castFrom, opCode.OpCodeOperands[0].ResultNumber.Value, effectiveType);
            }

            if (opCode.OpCode.StackBehaviourPush != StackBehaviour.Push0 || options.HasFlag(OperandOptions.GenerateResult))
            {
                var resultOf = this.ResultOf(opCode);
                this.WriteSetResultNumber(opCode, resultOf != null ? resultOf.IType : null);
            }

            writer.Write(op);
            writer.Write(' ');

            if (!options.HasFlag(OperandOptions.NoTypePrefix) && !options.HasFlag(OperandOptions.IgnoreOperand))
            {
                this.WriteTypePrefix(writer, effectiveType ?? TypeAdapter.FromType(typeof(void)));
                if (options.HasFlag(OperandOptions.AppendPointer))
                {
                    writer.Write('*');
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="doNotConvert">
        /// </param>
        /// <returns>
        /// </returns>
        private string TypeToCType(IType type, bool doNotConvert = false)
        {
            var effectiveType = type;

            if (type.IsArray)
            {
                effectiveType = type.GetElementType();
            }

            if (!doNotConvert)
            {
                if (effectiveType.Namespace == "System")
                {
                    string ctype;
                    if (SystemTypesToCTypes.TryGetValue(effectiveType.Name, out ctype))
                    {
                        return ctype;
                    }
                }

                if (type.IsEnum)
                {
                    switch (this.GetTypeSize(type))
                    {
                        case 1:
                            return "i8";
                        case 2:
                            return "i16";
                        case 4:
                            return "i32";
                        case 8:
                            return "i64";
                    }
                }

                if (type.IsValueType && type.IsPrimitive)
                {
                    return type.Name.ToLowerInvariant();
                }
            }

            return string.Concat('"', type.FullName, '"');
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="op">
        /// </param>
        /// <param name="localType">
        /// </param>
        /// <param name="options">
        /// </param>
        public void UnaryOper(
            LlvmIndentedTextWriter writer, OpCodePart opCode, string op, IType localType = null, OperandOptions options = OperandOptions.None)
        {
            this.UnaryOper(writer, opCode, 0, op, localType, options);
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
        /// <param name="options">
        /// </param>
        private void UnaryOper(
            LlvmIndentedTextWriter writer,
            OpCodePart opCode,
            int operandIndex,
            string op,
            IType requiredType = null,
            OperandOptions options = OperandOptions.None)
        {
            var directResult1 = this.PreProcessOperand(writer, opCode, operandIndex, options);

            this.ProcessOperator(writer, opCode, op, requiredType, options);

            if (!options.HasFlag(OperandOptions.IgnoreOperand))
            {
                this.PostProcessOperand(writer, opCode, operandIndex, directResult1);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        private void WriteAlloca(LlvmIndentedTextWriter writer, IType type)
        {
            // for value types
            writer.Write("alloca ");
            this.WriteTypePrefix(writer, type);
            writer.Write(", align " + pointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="isThis">
        /// </param>
        private void WriteArgumentCopyDeclaration(string name, IType type, bool isThis = false)
        {
            if (!isThis && type.IsStructureType())
            {
                return;
            }

            this.Output.Write(string.Format("%.{0} = ", name));

            // for value types
            this.Output.Write("alloca ");
            this.WriteTypePrefix(this.Output, type, type.IsStructureType());
            this.Output.Write(", align " + pointerSize);
            this.Output.WriteLine(string.Empty);

            this.Output.Write("store ");
            this.WriteTypePrefix(this.Output, type, type.IsStructureType());
            this.Output.Write(string.Format(" %{0}", name));
            this.Output.Write(", ");
            this.WriteTypePrefix(this.Output, type, type.IsStructureType());

            this.Output.Write(string.Format("* %.{0}", name));
            this.Output.Write(", align " + pointerSize);
            this.Output.WriteLine(string.Empty);
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
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteArrayGetLength(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            this.WriteBitcast(writer, opCode, TypeAdapter.FromType(typeof(int)));
            writer.WriteLine(string.Empty);

            var res = opCode.ResultNumber;
            var resLen = WriteSetResultNumber(writer, opCode);
            writer.Write("getelementptr ");
            this.WriteTypePrefix(writer, TypeAdapter.FromType(typeof(int)));
            writer.Write("* ");
            WriteResultNumber(res ?? -1);
            writer.WriteLine(", i32 -1");

            this.WriteLlvmLoad(writer, opCode, TypeAdapter.FromType(typeof(int)), this.GetResultNumber(resLen));
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteGetInterfaceOffsetToObjectRootPointer(LlvmIndentedTextWriter writer, OpCodePart opCode, IMethod method)
        {
            WriteSetResultNumber(writer, opCode);
            writer.Write("bitcast ");
            this.WriteMethodPointerType(writer, method);
            writer.Write("* ");
            WriteResultNumber(opCode.OpCodeOperands[0].ResultNumber ?? -1);
            writer.Write(" to ");
            this.WriteTypePrefix(writer, TypeAdapter.FromType(typeof(int*)));
            writer.WriteLine(string.Empty);
            opCode.ResultType = TypeAdapter.FromType(typeof(int*));

            writer.WriteLine(string.Empty);

            var res = opCode.ResultNumber;
            var offset = WriteSetResultNumber(writer, opCode);
            writer.Write("getelementptr ");
            this.WriteTypePrefix(writer, TypeAdapter.FromType(typeof(int)));
            writer.Write("* ");
            WriteResultNumber(res ?? -1);
            writer.WriteLine(", i32 -1");

            this.WriteLlvmLoad(writer, opCode, TypeAdapter.FromType(typeof(int)), this.GetResultNumber(offset));
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="res">
        /// </param>
        /// <param name="toType">
        /// </param>
        private void WriteBitcast(LlvmIndentedTextWriter writer, OpCodePart opCode, int res, IType toType)
        {
            WriteSetResultNumber(writer, opCode);
            writer.Write("bitcast i8* ");
            WriteResultNumber(res);
            writer.Write(" to ");
            this.WriteTypePrefix(writer, toType, true);
            opCode.ResultType = toType;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="toType">
        /// </param>
        public void WriteBitcast(LlvmIndentedTextWriter writer, OpCodePart opCode, IType toType, OperandOptions options = OperandOptions.None)
        {
            this.UnaryOper(writer, opCode, "bitcast", options: options);
            writer.Write(" to ");
            this.WriteTypePrefix(writer, toType, true);
            opCode.ResultType = TypeAdapter.FromType(typeof(byte*));
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="name">
        /// </param>
        private void WriteBitcast(LlvmIndentedTextWriter writer, OpCodePart opCode, IType fromType, string name)
        {
            WriteSetResultNumber(writer, opCode);
            writer.Write("bitcast ");
            this.WriteTypePrefix(writer, fromType, true);
            writer.Write(" ");
            writer.Write(name);
            writer.Write(" to i8*");
            opCode.ResultType = TypeAdapter.FromType(typeof(byte*));
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="res">
        /// </param>
        /// <param name="custom">
        /// </param>
        private void WriteBitcast(LlvmIndentedTextWriter writer, OpCodePart opCode, IType fromType, int res, string custom)
        {
            WriteSetResultNumber(writer, opCode);
            writer.Write("bitcast ");
            this.WriteTypePrefix(writer, fromType, true);
            writer.Write(' ');
            WriteResultNumber(res);
            writer.Write(" to ");
            writer.Write(custom);

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        /// <param name="isVirtual">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        /// <param name="isCtor">
        /// </param>
        /// <param name="thisResultNumber">
        /// </param>
        private void WriteCall(
            LlvmIndentedTextWriter writer, OpCodePart opCodeMethodInfo, IMethod methodBase, bool isVirtual, bool hasThis, bool isCtor, int? thisResultNumber)
        {
            if (opCodeMethodInfo.ResultNumber.HasValue)
            {
                return;
            }

            this.CheckIfExternalDeclarationIsRequired(methodBase);

            var preProcessedOperandResults = new List<bool>();

            if (opCodeMethodInfo.OpCodeOperands != null)
            {
                var index = 0;
                foreach (var operand in opCodeMethodInfo.OpCodeOperands)
                {
                    preProcessedOperandResults.Add(this.PreProcessOperand(writer, opCodeMethodInfo, index));
                    index++;
                }
            }

            var methodInfo = methodBase;
            var thisType = methodBase.DeclaringType;

            var hasThisArgument = hasThis && opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0;
            var opCodeFirstOperand = opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0 ? opCodeMethodInfo.OpCodeOperands[0] : null;
            var resultOfirstOperand = opCodeFirstOperand != null ? this.ResultOf(opCodeFirstOperand) : null;

            var startsWithThis = hasThisArgument && opCodeFirstOperand.Any(Code.Ldarg_0);

            int? virtualMethodAddressResultNumber = null;
            var isInderectMethodCall = isVirtual && (methodBase.IsVirtual || (thisType.IsInterface && thisType.TypeEquals(resultOfirstOperand.IType)));

            var ownerOfExplicitInterface = isVirtual && thisType.IsInterface && thisType.TypeNotEquals(resultOfirstOperand.IType) ? resultOfirstOperand.IType : null;
            var requiredType = ownerOfExplicitInterface != null ? resultOfirstOperand.IType : null;
            if (requiredType != null)
            {
                thisType = requiredType;
            }

            if (isInderectMethodCall)
            {
                if (thisType.IsInterface && !resultOfirstOperand.IType.Equals(thisType))
                {
                    // we need to extract interface from an object
                    requiredType = thisType;
                }

                // get pointer to Virtual Table and call method
                // 1) get pointer to virtual table
                writer.WriteLine("; Get Virtual Table");
                this.UnaryOper(writer, opCodeMethodInfo, "bitcast", requiredType);
                writer.Write(" to ");
                this.WriteMethodPointerType(writer, methodInfo);
                writer.WriteLine("**");
                var pointerToInterfaceVirtualTablePointersResultNumber = opCodeMethodInfo.ResultNumber;

                // load pointer
                this.WriteSetResultNumber(opCodeMethodInfo);
                writer.Write("load ");
                this.WriteMethodPointerType(writer, methodInfo);
                writer.Write("** ");
                WriteResultNumber(pointerToInterfaceVirtualTablePointersResultNumber ?? -1);
                writer.WriteLine(string.Empty);
                var virtualTableOfMethodPointersResultNumber = opCodeMethodInfo.ResultNumber;

                // get address of a function
                this.WriteSetResultNumber(opCodeMethodInfo);
                writer.Write("getelementptr inbounds ");
                this.WriteMethodPointerType(writer, methodInfo);
                writer.Write("* ");
                WriteResultNumber(virtualTableOfMethodPointersResultNumber ?? -1);
                writer.WriteLine(", i64 {0}", this.GetVirtualMethodIndex(thisType, methodInfo));
                var pointerToFunctionPointerResultNumber = opCodeMethodInfo.ResultNumber;

                // load method address
                this.WriteSetResultNumber(opCodeMethodInfo);
                writer.Write("load ");
                this.WriteMethodPointerType(writer, methodInfo);
                writer.Write("* ");
                WriteResultNumber(pointerToFunctionPointerResultNumber ?? -1);
                writer.WriteLine(string.Empty);
                // remember virtual method address result
                virtualMethodAddressResultNumber = opCodeMethodInfo.ResultNumber;

                if (thisType.IsInterface)
                {
                    opCodeFirstOperand.ResultNumber = virtualTableOfMethodPointersResultNumber;

                    this.WriteGetThisPointerFromInterfacePointer(
                        writer, opCodeMethodInfo, methodInfo, thisType, pointerToInterfaceVirtualTablePointersResultNumber);

                    var thisPointerResultNumber = opCodeMethodInfo.ResultNumber;
                    // set ot for Call op code
                    opCodeMethodInfo.OpCodeOperands[0].ResultNumber = thisPointerResultNumber;
                }
            }

            // check if you need to cast this parameter
            if (hasThisArgument && IsClassCastRequired(thisType, opCodeFirstOperand))
            {
                this.WriteCast(writer, opCodeFirstOperand, opCodeFirstOperand.ResultType, opCodeFirstOperand.ResultNumber.Value, thisType);
                writer.WriteLine(string.Empty);
            }

            // check if you need to cast parameter
            if (opCodeMethodInfo.OpCodeOperands != null)
            {
                var index = startsWithThis && !isCtor ? 1 : 0;
                foreach (var parameter in methodBase.GetParameters())
                {
                    var operand = opCodeMethodInfo.OpCodeOperands[index];

                    if (IsClassCastRequired(parameter.ParameterType, operand))
                    {
                        this.WriteCast(writer, operand, operand.ResultType, operand.ResultNumber.Value, parameter.ParameterType);
                    }

                    index++;
                }
            }

            if (methodInfo != null && !methodInfo.ReturnType.IsVoid())
            {
                this.WriteSetResultNumber(opCodeMethodInfo, methodInfo.ReturnType);
            }

            // allocate space for structure if return type is structure
            if (methodInfo != null && methodInfo.ReturnType.IsStructureType())
            {
                this.WriteAlloca(writer, methodInfo.ReturnType);
                writer.WriteLine(string.Empty);
            }

            writer.Write("call ");

            if (methodInfo != null && !methodInfo.ReturnType.IsVoid() && !methodInfo.ReturnType.IsStructureType())
            {
                this.WriteTypePrefix(writer, methodInfo.ReturnType, false);

                this.CheckIfExternalDeclarationIsRequired(methodInfo.ReturnType);
            }
            else
            {
                // this is constructor
                writer.Write("void");
            }

            writer.Write(' ');

            if (isInderectMethodCall)
            {
                WriteResultNumber(virtualMethodAddressResultNumber ?? -1);
            }
            else
            {
                this.WriteMethodDefinitionName(writer, methodBase, ownerOfExplicitInterface);
            }

            this.ActualWrite(
                writer,
                opCodeMethodInfo.OpCodeOperands,
                methodBase.GetParameters(),
                isVirtual,
                hasThis,
                isCtor,
                preProcessedOperandResults,
                thisResultNumber,
                thisType,
                opCodeMethodInfo.ResultNumber,
                methodInfo != null ? methodInfo.ReturnType : null);
        }

        private void WriteGetThisPointerFromInterfacePointer(LlvmIndentedTextWriter writer, OpCodePart opCodeMethodInfo, IMethod methodInfo, IType thisType, int? pointerToInterfaceVirtualTablePointersResultNumber)
        {
            writer.WriteLine("; Get 'this' from Interface Virtual Table");

            this.WriteGetInterfaceOffsetToObjectRootPointer(writer, opCodeMethodInfo, methodInfo);

            writer.WriteLine(string.Empty);
            var offsetAddressAsIntResultNumber = opCodeMethodInfo.ResultNumber;

            // get 'this' address
            var thisAddressFromInterfaceResultNumber = this.WriteSetResultNumber(writer, opCodeMethodInfo);
            writer.Write("getelementptr ");
            this.WriteMethodPointerType(writer, methodInfo);
            writer.Write("** ");
            this.WriteResultNumber(pointerToInterfaceVirtualTablePointersResultNumber ?? -1);
            writer.Write(", i32 ");
            this.WriteResultNumber(offsetAddressAsIntResultNumber ?? -1);
            writer.WriteLine(string.Empty);

            // adjust 'this' pointer
            this.WriteSetResultNumber(writer, opCodeMethodInfo);
            writer.Write("bitcast ");
            this.WriteMethodPointerType(writer, methodInfo);
            writer.Write("** ");
            this.WriteResultNumber(thisAddressFromInterfaceResultNumber);
            writer.Write(" to ");
            this.WriteTypePrefix(writer, thisType);
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCaseAndLabels(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.JumpDestination != null && opCode.JumpDestination.Count > 0)
            {
                var previousOpCode = opCode.PreviousOpCode(this);
                var splitBlock = previousOpCode != null
                                 && (previousOpCode.OpCode.FlowControl == FlowControl.Next || previousOpCode.OpCode.FlowControl == FlowControl.Call);
                if (splitBlock)
                {
                    // we need to fix issue with blocks in llvm http://zanopia.wordpress.com/2010/09/14/understanding-llvm-assembly-with-fractals-part-i/
                    writer.WriteLine(string.Concat("br label %.a", opCode.AddressStart));
                }

                if (splitBlock || !opCode.JumpProcessed)
                {
                    writer.Indent--;
                    writer.WriteLine(string.Concat(".a", opCode.AddressStart, ':'));
                    writer.Indent++;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="res">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        private void WriteCast(LlvmIndentedTextWriter writer, OpCodePart opCode, IType fromType, int res, IType toType, bool appendReference = false)
        {
            if (!fromType.IsInterface && toType.IsInterface)
            {
                opCode.ResultNumber = res;
                this.WriteInterfaceAccess(writer, opCode, fromType, toType);
            }
            else
            {
                WriteSetResultNumber(writer, opCode);
                writer.Write("bitcast ");
                this.WriteTypePrefix(writer, fromType, true);
                writer.Write(' ');
                WriteResultNumber(res);
                writer.Write(" to ");
                this.WriteTypePrefix(writer, toType, true);
                if (appendReference)
                {
                    // result should be array
                    writer.Write('*');
                }
            }

            opCode.ResultType = toType;
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCatchFinnally(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.EndOfTry != null && opCode.EndOfTry.Count > 0)
            {
                foreach (var endOfTryId in opCode.EndOfTry)
                {
                    writer.WriteLine(string.Empty);
                    writer.Indent--;
                    writer.WriteLine("}");
                }
            }

            if (opCode.EndOfClausesOrFinal != null && opCode.EndOfClausesOrFinal.Count > 0)
            {
                foreach (var endOfCaluseId in opCode.EndOfClausesOrFinal)
                {
                    // writer.WriteLine(string.Empty);
                    writer.Indent--;
                    writer.WriteLine("}");
                }
            }

            if (opCode.ExceptionHandlers != null)
            {
                foreach (var exceptionHandler in opCode.ExceptionHandlers)
                {
                    if (exceptionHandler.Flags == ExceptionHandlingClauseOptions.Clause)
                    {
                        writer.Write("catch (");
                        this.WriteTypePrefix(writer, exceptionHandler.CatchType);
                        writer.Write(")");
                    }

                    if (exceptionHandler.Flags.HasFlag(ExceptionHandlingClauseOptions.Finally))
                    {
                        writer.Write("catch (...) {}");
                    }

                    writer.WriteLine(string.Empty);
                    writer.Write('{');
                    writer.Indent++;
                }
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
            writer.WriteLine(string.Format("br i1 %.r{0}, label %.a{1}, label %.a{2}", opCode.ResultNumber, opCode.JumpAddress(), opCode.GroupAddressEnd));
            writer.Indent--;
            writer.Write(string.Concat(".a", opCode.GroupAddressEnd, ':'));
            writer.Indent++;

            opCode.NextOpCode(this).JumpProcessed = true;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="sourceVarName">
        /// </param>
        /// <param name="desctVarName">
        /// </param>
        private void WriteCopyStruct(LlvmIndentedTextWriter writer, OpCodePart opCode, IType type, string sourceVarName, string desctVarName)
        {
            WriteBitcast(writer, opCode, type, desctVarName);
            var op1 = opCode.ResultNumber;
            writer.WriteLine(string.Empty);
            WriteBitcast(writer, opCode, type, sourceVarName);
            var op2 = opCode.ResultNumber;
            writer.WriteLine(string.Empty);

            this.WriteMemCopy(writer, type, op1, op2);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeFieldInfoPart">
        /// </param>
        private void WriteFieldAccess(LlvmIndentedTextWriter writer, OpCodeFieldInfoPart opCodeFieldInfoPart)
        {
            var operand = this.ResultOf(opCodeFieldInfoPart.OpCodeOperands[0]);
            var opts = OperandOptions.GenerateResult;
            if (operand.IType.IsStructureType())
            {
                opts |= OperandOptions.AppendPointer;
            }

            this.UnaryOper(writer, opCodeFieldInfoPart, "getelementptr inbounds", options: opts);
            this.WriteFieldIndex(writer, operand.IType, opCodeFieldInfoPart.Operand);
            opCodeFieldInfoPart.ResultType = opCodeFieldInfoPart.Operand.FieldType;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="classType">
        /// </param>
        /// <param name="fieldInfo">
        /// </param>
        private void WriteFieldIndex(LlvmIndentedTextWriter writer, IType classType, IField fieldInfo)
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
                    break;
                }

                // first index is base type index
                writer.Write(", i32 0");
            }

            // find index
            int index;
            if (!this.indexByFieldInfo.TryGetValue(GetFullFieldName(fieldInfo), out index))
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
        /// <param name="method">
        /// </param>
        private void WriteGenericParameters(LlvmIndentedTextWriter writer, IMethod method)
        {
            var i = 0;
            foreach (var generic in method.GetGenericArguments())
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }

                writer.Write("typename ");
                writer.Write(generic.Name);

                i++;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        private void WriteGenericParameters(LlvmIndentedTextWriter writer, IType type)
        {
            var index = type.Name.IndexOf('`');
            var level = int.Parse(type.Name.Substring(index + 1));

            for (var i = 0; i < level; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }

                writer.Write("typename ");

                IType generic = null;

                if (this.GenericMethodArguments != null)
                {
                    generic = this.GenericMethodArguments.Where(a => a.GenericParameterPosition == i).FirstOrDefault();
                }

                if (generic == null && this.TypeGenericArguments != null)
                {
                    generic = this.TypeGenericArguments.Where(a => a.GenericParameterPosition == i).FirstOrDefault();
                }

                if (generic != null)
                {
                    writer.Write(generic.Name);
                }
                else
                {
                    writer.Write("T");
                    writer.Write(i + 1);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        private void WriteInitObject(LlvmIndentedTextWriter writer, OpCodePart opCode, IType declaringType)
        {
            // Init Object From Here
            if (declaringType.HasAnyVirtualMethod())
            {
                var opCodeResult = opCode.ResultNumber;
                var opCodeType = opCode.ResultType;

                writer.WriteLine("; set virtual table");

                // initializw virtual table
                this.WriteCast(writer, opCode, declaringType, opCode.ResultNumber ?? -1, TypeAdapter.FromType(typeof(byte**)));
                writer.WriteLine(string.Empty);

                var virtualTable = this.GetVirtualTable(declaringType);

                writer.Write(
                    "store i8** getelementptr inbounds ([{0} x i8*]* {1}, i64 0, i64 1), i8*** ",
                    virtualTable.Count + 1,
                    this.GetVirtualTableName(declaringType));
                WriteResultNumber(opCode.ResultNumber ?? -1);
                writer.WriteLine(string.Empty);

                // restore
                opCode.ResultNumber = opCodeResult;
                opCode.ResultType = opCodeType;
            }

            // init all interfaces
            foreach (var @interface in declaringType.GetInterfaces())
            {
                var opCodeResult = opCode.ResultNumber;
                var opCodeType = opCode.ResultType;

                writer.WriteLine("; set virtual interface table");

                this.WriteInterfaceAccess(writer, opCode, declaringType, @interface);

                this.WriteCast(writer, opCode, @interface, opCode.ResultNumber ?? -1, TypeAdapter.FromType(typeof(byte**)));
                writer.WriteLine(string.Empty);

                var virtualInterfaceTable = this.GetVirtualInterfaceTable(declaringType, @interface);

                writer.Write(
                    "store i8** getelementptr inbounds ([{0} x i8*]* {1}, i64 0, i64 1), i8*** ",
                    virtualInterfaceTable.Count + 1,
                    this.GetVirtualInterfaceTableName(declaringType, @interface));
                WriteResultNumber(opCode.ResultNumber ?? -1);
                writer.WriteLine(string.Empty);

                // restore
                opCode.ResultNumber = opCodeResult;
                opCode.ResultType = opCodeType;
            }
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
        private void WriteInterfaceAccess(LlvmIndentedTextWriter writer, OpCodePart opCode, IType declaringType, IType @interface)
        {
            var objectResult = opCode.ResultNumber;

            writer.WriteLine("; Get interface '{0}' of '{1}'", @interface, declaringType);

            this.ProcessOperator(writer, opCode, "getelementptr inbounds", declaringType, OperandOptions.TypeIsInOperator);
            writer.Write(' ');
            writer.Write(this.GetResultNumber(objectResult ?? -1));
            this.WriteInterfaceIndex(writer, declaringType, @interface);
            writer.WriteLine(string.Empty);
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
        private void WriteInterfaceIndex(LlvmIndentedTextWriter writer, IType classType, IType @interface)
        {
            var type = classType;

            // first element for pointer (IType* + 0)
            writer.Write(", i32 0");

            while (!type.GetInterfaces().Contains(@interface) || type.BaseType.GetInterfaces().Contains(@interface))
            {
                type = type.BaseType;
                if (type == null)
                {
                    // break;
                    return;
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

            var found = false;
            foreach (var typeInterface in type.GetInterfaces())
            {
                if (typeInterface.NameEquals(@interface))
                {
                    found = true;
                    break;
                }

                index++;
            }

            if (!found)
            {
                throw new IndexOutOfRangeException("Could not find an interface");
            }

            writer.Write(", i32 ");
            writer.Write(index);
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
            this.WriteTypePrefix(writer, this.Parameters[index].ParameterType, false);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(this.GetArgVarName(index));

            // TODO: optional do we need to calculate it propertly?
            writer.Write(", align " + pointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="localVarName">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <param name="structAsRef">
        /// </param>
        private void WriteLlvmLoad(
            LlvmIndentedTextWriter writer, OpCodePart opCode, IType type, string localVarName, bool appendReference = true, bool structAsRef = false)
        {
            if (!type.IsStructureType() || structAsRef)
            {
                WriteSetResultNumber(opCode, type);

                // last part
                writer.Write("load ");
                this.WriteTypePrefix(writer, type, structAsRef);
                if (appendReference)
                {
                    // add reference to type
                    writer.Write('*');
                }

                writer.Write(' ');
                writer.Write(localVarName);

                // TODO: optional do we need to calculate it propertly?
                writer.Write(", align " + pointerSize);
            }
            else
            {
                Debug.Assert(opCode.DestinationName != null);
                this.WriteCopyStruct(writer, opCode, type, localVarName, opCode.DestinationName);
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
        private void WriteLlvmLocalVarAccess(LlvmIndentedTextWriter writer, int index, bool asReference = false)
        {
            this.WriteTypePrefix(writer, this.LocalInfo[index].LocalType, false);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(this.GetLocalVarName(index));

            // TODO: optional do we need to calculate it propertly?
            writer.Write(", align " + pointerSize);
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
                this.WriteAlloca(this.Output, local.LocalType);
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="op1">
        /// </param>
        /// <param name="op2">
        /// </param>
        private void WriteMemCopy(LlvmIndentedTextWriter writer, IType type, int? op1, int? op2)
        {
            writer.WriteLine(
                "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)",
                this.GetResultNumber(op1.Value),
                this.GetResultNumber(op2.Value),
                this.GetTypeSize(type),
                pointerSize /*Align*/);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="op1">
        /// </param>
        private void WriteMemSet(LlvmIndentedTextWriter writer, IType type, int? op1)
        {
            writer.Write(
                "call void @llvm.memset.p0i8.i32(i8* {0}, i8 0, i32 {1}, i32 {2}, i1 false)",
                this.GetResultNumber(op1.Value),
                this.GetTypeSize(type),
                pointerSize /*Align*/);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="memberInfo">
        /// </param>
        private void WriteMemberName(LlvmIndentedTextWriter writer, MemberInfo memberInfo)
        {
            writer.Write(string.Concat(memberInfo.DeclaringType.FullName + '.' + memberInfo.Name));
        }

        /// <summary>
        /// </summary>
        /// <param name="endPart">
        /// </param>
        private void WriteMethodBody(string endPart = null)
        {
            this.Output.StartMethodBody();

            var rest = this.PrepareWritingMethodBody();

            var i = 0;
            foreach (var opCodePart in rest)
            {
                this.ActualWrite(this.Output, opCodePart, true);
                i++;

                if (endPart != null && i == rest.Length)
                {
                    this.Output.Write(endPart);
                }

                this.Output.WriteLine(string.Empty);
            }

            this.Output.EndMethodBody();
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        private void WriteMethodDefinitionName(LlvmIndentedTextWriter writer, IMethod methodBase, IType ownerOfExplicitInterface = null)
        {
            writer.Write(this.GetFullMethodName(methodBase, ownerOfExplicitInterface));
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
        private void WriteMethodParamsDef(
            LlvmIndentedTextWriter writer, IEnumerable<IParameter> parameterInfos, bool hasThis, IType thisType, IType returnType, bool noArgumentName = false)
        {
            writer.Write("(");

            if (returnType.IsStructureType())
            {
                this.WriteTypePrefix(writer, returnType, true);
                writer.Write(" noalias sret %agg.result");
            }

            var start = hasThis ? 1 : 0;

            if (hasThis)
            {
                if (returnType.IsStructureType())
                {
                    writer.Write(", ");
                }

                this.WriteTypePrefix(writer, thisType, thisType.IsStructureType());
                if (!noArgumentName)
                {
                    writer.Write(" %this");
                }
            }

            var index = start;
            foreach (var parameter in parameterInfos)
            {
                if (hasThis || index > start || returnType.IsStructureType())
                {
                    writer.Write(", ");
                }

                this.WriteTypePrefix(writer, parameter.ParameterType, false);
                if (parameter.ParameterType.IsStructureType())
                {
                    writer.Write(" byval align " + pointerSize);
                    if (!noArgumentName)
                    {
                        writer.Write(" %.");
                    }
                }
                else if (!noArgumentName)
                {
                    writer.Write(" %");
                }

                if (!noArgumentName)
                {
                    writer.Write(parameter.Name);
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
        private void WriteMethodPointerType(LlvmIndentedTextWriter writer, IMethod methodBase)
        {
            var fullMethodName = this.GetFullMethodName(methodBase);
            var methodInfo = methodBase;
            this.WriteTypePrefix(writer, methodInfo.ReturnType);

            writer.Write(" (");

            var hasThis = !methodInfo.IsStatic;

            if (hasThis)
            {
                this.WriteTypePrefix(writer, methodInfo.DeclaringType);
            }

            var index = 0;
            foreach (var parameter in methodBase.GetParameters())
            {
                if (index > 0 || hasThis)
                {
                    writer.Write(", ");
                }

                this.WriteTypePrefix(writer, parameter.ParameterType);
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
        private void WriteMethodReturnType(LlvmIndentedTextWriter writer, IMethod method)
        {
            if (!method.ReturnType.IsVoid() && !method.ReturnType.IsStructureType())
            {
                this.WriteTypePrefix(writer, method.ReturnType, false);
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
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        private void WriteNew(LlvmIndentedTextWriter writer, OpCodePart opCode, IType declaringType)
        {
            if (opCode.ResultNumber.HasValue)
            {
                return;
            }

            writer.WriteLine("; New obj");

            var mallocResult = WriteSetResultNumber(writer, opCode);
            var size = this.GetTypeSize(declaringType);
            writer.WriteLine("call i8* @malloc(i32 {0})", size);
            this.WriteMemSet(writer, declaringType, mallocResult);
            writer.WriteLine(string.Empty);

            WriteBitcast(writer, opCode, mallocResult, declaringType);
            writer.WriteLine(string.Empty);

            var castResult = opCode.ResultNumber;

            this.WriteInitObject(writer, opCode, declaringType);

            // restore result and type
            opCode.ResultNumber = castResult;
            opCode.ResultType = declaringType;

            writer.WriteLine("; end of new obj");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <param name="length">
        /// </param>
        private void WriteNewArray(LlvmIndentedTextWriter writer, OpCodePart opCode, IType declaringType, OpCodePart length)
        {
            if (opCode.ResultNumber.HasValue)
            {
                return;
            }

            writer.WriteLine("; New array");

            var size = this.GetTypeSize(declaringType);
            this.UnaryOper(writer, opCode, "mul");
            writer.WriteLine(string.Format(", {0}", size));

            var resMul = opCode.ResultNumber;

            WriteSetResultNumber(writer, opCode);
            writer.Write("add i32 4, {0}", this.GetResultNumber(resMul ?? -1));
            writer.WriteLine(string.Empty);

            var resAdd = opCode.ResultNumber;

            var resAlloc = WriteSetResultNumber(writer, opCode);
            writer.Write("call i8* @malloc(i32 {0})", this.GetResultNumber(resAdd ?? -1));
            writer.WriteLine(string.Empty);

            this.WriteBitcast(writer, opCode, resAlloc, TypeAdapter.FromType(typeof(int)));
            writer.WriteLine(string.Empty);

            var opCodeTemp = OpCodePart.Nop;
            opCodeTemp.OpCodeOperands = opCode.OpCodeOperands;

            // save array size
            this.ProcessOperator(writer, opCodeTemp, "store");
            this.PostProcessOperand(writer, opCode, 0, !opCode.OpCodeOperands[0].ResultNumber.HasValue);
            writer.Write(", ");
            this.WriteTypePrefix(writer, TypeAdapter.FromType(typeof(int)));
            writer.Write("* ");
            WriteResultNumber(opCode.ResultNumber ?? -1);
            writer.WriteLine(string.Empty);

            var tempRes = opCode.ResultNumber.Value;
            var resGetArr = WriteSetResultNumber(writer, opCode);
            writer.Write("getelementptr ");

            // WriteTypePrefix(writer, declaringType);
            writer.Write("i32* ");
            WriteResultNumber(tempRes);
            writer.Write(", i32 1");

            if (declaringType != TypeAdapter.FromType(typeof(int)))
            {
                writer.WriteLine(string.Empty);
                this.WriteCast(writer, opCode, TypeAdapter.FromType(typeof(int)), resGetArr, declaringType, true);
            }

            writer.WriteLine("; end of new array");
        }

        /// <summary>
        /// </summary>
        private void WritePostDeclarations()
        {
            // after writing type you need to generate static members
            foreach (var field in this.staticFieldsInfo)
            {
                this.Output.Write("@\"{0}\" = global ", GetFullFieldName(field));
                this.WriteTypePrefix(this.Output, field.FieldType, false);
                this.Output.WriteLine(" undef");
            }

            // write VirtualTable
            if (!this.ThisType.IsInterface)
            {
                if (this.ThisType.HasAnyVirtualMethod())
                {
                    this.Output.WriteLine(string.Empty);
                    this.Output.Write(this.GetVirtualTableName(this.ThisType));
                    var virtualTable = this.GetVirtualTable(this.ThisType);
                    this.WriteTableOfMethods(virtualTable);
                }

                int index = 1;
                foreach (var @interface in this.ThisType.GetInterfaces())
                {
                    this.Output.WriteLine(string.Empty);
                    this.Output.Write(this.GetVirtualInterfaceTableName(this.ThisType, @interface));
                    var virtualInterfaceTable = this.GetVirtualInterfaceTable(this.ThisType, @interface);
                    this.WriteTableOfMethods(virtualInterfaceTable, index++);
                }
            }
        }

        /// <summary>
        /// </summary>
        private void WriteRequiredDeclarations()
        {
            if (this.typeDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (var opaqueType in this.typeDeclRequired)
                {
                    if (this.processedTypes.Contains(opaqueType))
                    {
                        continue;
                    }

                    this.WriteTypeDeclarationStart(opaqueType);
                    this.Output.WriteLine("opaque");
                }
            }

            if (this.methodDeclRequired.Count > 0)
            {
                this.Output.WriteLine(string.Empty);
                foreach (var externalMethodDecl in this.methodDeclRequired)
                {
                    if (this.processedMethods.Contains(externalMethodDecl))
                    {
                        continue;
                    }

                    this.Output.Write("declare ");

                    var ctor = externalMethodDecl as IConstructor;
                    if (ctor != null)
                    {
                        this.ReadMethodInfo(ctor);
                        this.Output.Write("void ");
                        this.WriteMethodDefinitionName(this.Output, ctor);
                        this.WriteMethodParamsDef(this.Output, ctor.GetParameters(), this.HasMethodThis, this.ThisType, TypeAdapter.FromType(typeof(void)));
                        this.Output.WriteLine(string.Empty);
                        continue;
                    }

                    var method = externalMethodDecl;
                    if (method != null)
                    {
                        this.ReadMethodInfo(method);
                        this.WriteMethodReturnType(this.Output, method);
                        this.WriteMethodDefinitionName(this.Output, method);
                        this.WriteMethodParamsDef(this.Output, method.GetParameters(), this.HasMethodThis, this.ThisType, method.ReturnType);
                        this.Output.WriteLine(string.Empty);
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        private void WriteResultNumber(OpCodePart opCode)
        {
            // write number of method
            this.Output.Write(this.GetResultNumber(opCode.ResultNumber ?? -1));
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        public void WriteResultNumber(int number)
        {
            // write number of method
            this.Output.Write(this.GetResultNumber(number));
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private int WriteSetResultNumber(OpCodePart opCode, IType type = null)
        {
            return this.WriteSetResultNumber(this.Output, opCode, type);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public int WriteSetResultNumber(LlvmIndentedTextWriter writer, OpCodePart opCode, IType type = null)
        {
            // write number of method
            writer.Write("%.r");
            writer.Write(++this.resultNumberIncremental);
            writer.Write(" = ");

            if (opCode != null)
            {
                opCode.ResultNumber = this.resultNumberIncremental;
                opCode.ResultType = type;
            }

            return this.resultNumberIncremental;
        }

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        private void WriteTableOfMethods(List<Pair<string, IMethod>> virtualTable, int interfaceIndex = 0)
        {
            this.Output.Write(
                " = linkonce_odr unnamed_addr constant [{0} x i8*] [i8* {1}",
                virtualTable.Count + 1,
                interfaceIndex == 0 ? "null" : string.Format("inttoptr (i32 -{0} to i8*)", interfaceIndex));

            // define virtual table
            foreach (var virtualMethod in virtualTable)
            {
                var method = virtualMethod.Value;

                // write method pointer
                this.Output.Write(", i8* bitcast (");

                // write pointer to method
                this.WriteMethodReturnType(this.Output, method);
                this.WriteMethodParamsDef(this.Output, method.GetParameters(), true, method.DeclaringType, method.ReturnType, true);
                this.Output.Write("* ");
                this.WriteMethodDefinitionName(this.Output, method);
                this.Output.Write(" to i8*)");
            }

            this.Output.WriteLine("]");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteTry(LlvmIndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.Try != null)
            {
                foreach (var tryId in opCode.Try)
                {
                    writer.WriteLine("try");
                    writer.WriteLine('{');
                    writer.Indent++;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteTypeDeclarationStart(IType type)
        {
            this.Output.Write("%");
            this.WriteTypeName(this.Output, type, true);

            this.Output.Write(" = type ");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteTypeDefinitionIfNotWrittenYet(IType type)
        {
            if (this.processedTypes.Contains(type))
            {
                return;
            }

            this.processedTypes.Add(type);

            Il2Converter.WriteTypeDefinition(this, type, null, true);
            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="doNotConvert">
        /// </param>
        private void WriteTypeName(LlvmIndentedTextWriter writer, IType type, bool doNotConvert = false)
        {
            var typeBaseName = this.TypeToCType(type, doNotConvert);

            // clean name
            if (typeBaseName.EndsWith("&"))
            {
                typeBaseName = typeBaseName.Substring(0, typeBaseName.Length - 1);
            }

            var index = typeBaseName.IndexOf('`');
            if (index >= 0)
            {
                var nameWithoutGeneric = typeBaseName.Substring(0, index);
                writer.Write(nameWithoutGeneric);
            }
            else
            {
                writer.Write(typeBaseName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="asReference">
        /// </param>
        /// <param name="doNotIncludeTypePrefixId">
        /// </param>
        /// <param name="refChar">
        /// </param>
        public void WriteTypePrefix(
            LlvmIndentedTextWriter writer, IType type, bool asReference = false, bool doNotIncludeTypePrefixId = false, char refChar = '*')
        {
            this.WriteTypeWithoutModifiers(writer, type, doNotIncludeTypePrefixId);
            WriteTypeModifiers(writer, type, asReference, refChar);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="doNotIncludeTypePrefixId">
        /// </param>
        private void WriteTypeWithoutModifiers(LlvmIndentedTextWriter writer, IType type, bool doNotIncludeTypePrefixId = false)
        {
            var effectiveType = type;

            while (effectiveType.HasElementType)
            {
                effectiveType = effectiveType.GetElementType();
            }

            // TODO: remove String test when you use real string class
            if (!doNotIncludeTypePrefixId && !effectiveType.IsPrimitiveType() && !effectiveType.IsVoid() && !effectiveType.IsEnum
                && !(effectiveType.Namespace == "System" && effectiveType.Name == "String"))
            {
                writer.Write('%');
            }

            // write base name
            this.WriteTypeName(writer, effectiveType);
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
            ToFloat = 2,

            /// <summary>
            /// </summary>
            ToInteger = 4,

            /// <summary>
            /// </summary>
            TypeIsInSecondOperand = 8,

            /// <summary>
            /// </summary>
            TypeIsInOperator = 16,

            /// <summary>
            /// </summary>
            NoTypePrefix = 32,

            /// <summary>
            /// </summary>
            AppendPointer = 64,

            /// <summary>
            /// </summary>
            IgnoreOperand = 128,

            /// <summary>
            /// </summary>
            DetectTypeInSecondOperand = 256,
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
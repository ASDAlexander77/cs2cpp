namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Il2Native.Logic.CodeParts;

    /// <summary>
    /// </summary>
    public class CppWriter : BaseWriter, ICodeWriter
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static IDictionary<string, string> systemTypesToCTypes = new SortedDictionary<string, string>();

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        private List<FieldInfo> fieldsInfo = new List<FieldInfo>();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        static CppWriter()
        {
            systemTypesToCTypes["Byte"] = "unsigned char";
            systemTypesToCTypes["SByte"] = "char";
            systemTypesToCTypes["Char"] = "wchar_t";
            systemTypesToCTypes["Int16"] = "short";
            systemTypesToCTypes["Int32"] = "int";
            systemTypesToCTypes["Int64"] = "long";
            systemTypesToCTypes["UInt16"] = "unsigned short";
            systemTypesToCTypes["UInt32"] = "unsigned int";
            systemTypesToCTypes["UInt64"] = "unsigned long";
            systemTypesToCTypes["Float"] = "float";
            systemTypesToCTypes["Single"] = "float";
            systemTypesToCTypes["Double"] = "double";
            systemTypesToCTypes["Boolean"] = "bool";
            systemTypesToCTypes["String"] = "string";
            systemTypesToCTypes["Byte&"] = "unsigned char";
            systemTypesToCTypes["SByte&"] = "char";
            systemTypesToCTypes["Char&"] = "wchar_t";
            systemTypesToCTypes["Int16&"] = "short";
            systemTypesToCTypes["Int32&"] = "int";
            systemTypesToCTypes["Int64&"] = "long";
            systemTypesToCTypes["IntPtr"] = "void*";
            systemTypesToCTypes["UInt16&"] = "unsigned short";
            systemTypesToCTypes["UInt32&"] = "unsigned int";
            systemTypesToCTypes["UInt64&"] = "unsigned long";
            systemTypesToCTypes["Float&"] = "float";
            systemTypesToCTypes["Single&"] = "float";
            systemTypesToCTypes["Double&"] = "double";
            systemTypesToCTypes["Boolean&"] = "bool";
            systemTypesToCTypes["String&"] = "string";
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">
        /// </param>
        public CppWriter(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var outputFile = extension != null && extension.Equals(string.Empty) ? fileName + ".cpp" : fileName;
            this.Output = new IndentedTextWriter(new StreamWriter(outputFile));
        }

        #endregion

        #region Enums

        /// <summary>
        /// </summary>
        private enum Ending
        {
            /// <summary>
            /// </summary>
            SeparatorWithNewLine, 

            /// <summary>
            /// </summary>
            NewLine, 

            /// <summary>
            /// </summary>
            NoEndings
        }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        protected IndentedTextWriter Output { get; private set; }

        #endregion

        #region Public Methods and Operators

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
            this.Output.WriteLine("// {0}", opCode.OpCode.Name);
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
        public void WriteAfterFields(int count)
        {
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
            this.Output.Indent--;
            this.Output.WriteLine(string.Empty);
            this.Output.Indent++;
        }

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        public void WriteBeforeFields(int count)
        {
            this.fieldsInfo.Clear();

            this.Output.Indent--;
            this.Output.WriteLine(string.Empty);
            this.Output.Indent++;
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
        public void WriteConstructorEnd(ConstructorInfo ctor)
        {
            this.WriteMethodBody(!ctor.IsStatic ? string.Empty : " 1");

            if (!this.NoBody)
            {
                this.Output.Indent--;

                this.Output.WriteLine("}");
            }

            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        public void WriteConstructorStart(ConstructorInfo ctor)
        {
            this.StartProcess();

            ReadMethodInfo(ctor);

            if (ctor.IsPublic || ctor.IsStatic)
            {
                this.Output.Write("public: ");
            }
            else if (ctor.IsPrivate)
            {
                this.Output.Write("private: ");
            }
            else
            {
                this.Output.Write("protected: ");
            }

            if (ctor.IsStatic)
            {
                this.Output.Write("static ");
            }

            if (ctor.IsVirtual)
            {
                this.Output.Write("virtual ");
            }

            // this.WriteMethodName(this.Output, ctor);
            if (!ctor.IsStatic)
            {
                this.WriteName(this.Output, ctor.DeclaringType);
            }
            else
            {
                this.StaticConstructors.Add(ctor);
                this.Output.Write("int _ctor_s");
            }

            this.WriteMethodParamsDef(this.Output, ctor.GetParameters(), this.HasMethodThis);

            // write local declarations
            var methodBase = ctor.GetMethodBody();
            if (methodBase != null)
            {
                this.Output.WriteLine(" {");
                this.Output.Indent++;
                this.WriteLocalVariableDeclarations(methodBase.LocalVariables);
            }
            else
            {
                this.Output.WriteLine(" = 0;");
            }
        }

        /// <summary>
        /// </summary>
        public void WriteEnd()
        {
            if (this.MainMethod != null)
            {
                this.Output.WriteLine("int main() {");
                this.Output.Indent++;

                this.Output.Write("return ");

                this.WriteMethodName(this.Output, this.MainMethod, true);

                this.Output.Write("(");

                var index = 0;
                foreach (var parameter in this.MainMethod.GetParameters())
                {
                    if (index > 0)
                    {
                        this.Output.Write(", ");
                    }

                    this.Output.Write("nullptr");

                    index++;
                }

                this.Output.WriteLine(");");

                this.Output.Indent--;
                this.Output.WriteLine("}");
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
        public void WriteFieldEnd(FieldInfo field, int number, int count)
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
        public void WriteFieldStart(FieldInfo field, int number, int count)
        {
            this.fieldsInfo.Add(field);

            if (field.IsPrivate)
            {
                this.Output.Write("private: ");
            }

            if (field.IsPublic)
            {
                this.Output.Write("public: ");
            }

            if (field.IsStatic)
            {
                this.Output.Write("static ");
            }

            this.WriteTypePrefix(this.Output, field.FieldType, false, true);
            this.Output.Write(' ');
            this.Output.Write(field.Name);
            this.WriteTypeSuffix(this.Output, field.FieldType, true);
            this.Output.WriteLine(';');
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="number">
        /// </param>
        /// <param name="count">
        /// </param>
        public void WriteForwardDeclaration(Type type, int number, int count)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        public void WriteMethodEnd(MethodInfo method)
        {
            this.WriteMethodBody();

            if (!this.NoBody)
            {
                this.Output.Indent--;
                this.Output.WriteLine("}");
            }

            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        public void WriteMethodStart(MethodInfo method)
        {
#if DEBUG
            this.Output.Write("// ");
            var methodBody = method.GetMethodBody();
            if (methodBody != null)
            {
                foreach (var codeByte in methodBody.GetILAsByteArray())
                {
                    this.Output.Write(codeByte.ToString("X"));
                    this.Output.Write(" ");
                }
            }

            this.Output.WriteLine(string.Empty);
#endif

            this.StartProcess();

            var isMain = method.IsStatic && method.CallingConvention.HasFlag(CallingConventions.Standard) && method.Name.Equals("Main");

            // check if main
            if (isMain)
            {
                this.MainMethod = method;
            }

            if (isMain)
            {
                this.Output.Write("public: ");
            }
            else if (method.IsPrivate)
            {
                this.Output.Write("private: ");
            }
            else if (method.IsPublic)
            {
                this.Output.Write("public: ");
            }
            else
            {
                this.Output.Write("protected: ");
            }

            if (method.IsGenericMethod)
            {
                this.Output.Write("template <");
                WriteGenericParameters(this.Output, method);
                this.Output.Write("> ");
            }

            if (method.IsStatic)
            {
                this.Output.Write("static ");
            }

            if (method.IsVirtual)
            {
                this.Output.Write("virtual ");
            }

            if (method.ReturnType.Name != "Void")
            {
                this.WriteTypePrefix(this.Output, method.ReturnType, false, true);
                this.WriteTypeSuffix(this.Output, method.ReturnType, true);
                this.Output.Write(" ");
            }
            else
            {
                this.Output.Write("void ");
            }

            this.WriteMethodName(this.Output, method, false);

            ReadMethodInfo(method);

            this.WriteMethodParamsDef(this.Output, method.GetParameters(), this.HasMethodThis);

            // write local declarations
            var methodBodyBytes = method.GetMethodBody();
            if (methodBodyBytes != null)
            {
                this.Output.WriteLine(" {");

                this.Output.Indent++;

                this.WriteLocalVariableDeclarations(methodBodyBytes.LocalVariables);
            }
            else
            {
                this.Output.WriteLine(" = 0;");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="moduleName">
        /// </param>
        public void WriteStart(string moduleName)
        {
            this.Output.WriteLine("#include <iostream>");
            this.Output.WriteLine("#include <sstream>");
            this.Output.WriteLine();
            this.Output.WriteLine("using namespace std;");
            this.Output.WriteLine();

            this.Output.WriteLine("class Object {");
            this.Output.WriteLine("};");
            this.Output.WriteLine(string.Empty);

            this.Output.WriteLine("class ValueType: public Object {");
            this.Output.WriteLine("};");
            this.Output.WriteLine(string.Empty);

            // Console Output
            this.Output.WriteLine("class Console {");
            this.Output.Indent++;
            this.Output.WriteLine("public: static void Write(string s) {");
            this.Output.Indent++;
            this.Output.WriteLine("cout << s.c_str();");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.WriteLine("public: static void WriteLine(string s) {");
            this.Output.Indent++;
            this.Output.WriteLine("cout << s.c_str() << endl;");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.WriteLine("public: static void Write(int i) {");
            this.Output.Indent++;
            this.Output.WriteLine("cout << i;");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.WriteLine("public: static void WriteLine(int i) {");
            this.Output.Indent++;
            this.Output.WriteLine("cout << i << endl;");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.Indent--;
            this.Output.WriteLine("};");
            this.Output.WriteLine(string.Empty);

            // String min functionality
            this.Output.WriteLine("class String {");
            this.Output.Indent++;
            this.Output.WriteLine("public: static bool op_Equality(string s1, string s2) {");
            this.Output.Indent++;
            this.Output.WriteLine("return s1.compare(s2) == 0;");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.WriteLine("public: static bool op_Inequality(string s1, string s2) {");
            this.Output.Indent++;
            this.Output.WriteLine("return s1.compare(s2) != 0;");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.WriteLine("public: static string Concat(string s1, string s2) {");
            this.Output.Indent++;
            this.Output.WriteLine("string r;");
            this.Output.WriteLine("r.append(s1);");
            this.Output.WriteLine("r.append(s2);");
            this.Output.WriteLine("return r;");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.WriteLine("public: static string Concat(string s1, int i) {");
            this.Output.Indent++;
            this.Output.WriteLine("string r;");
            this.Output.WriteLine("r.append(s1);");
            this.Output.WriteLine("stringstream ss;");
            this.Output.WriteLine("ss << i;");
            this.Output.WriteLine("r.append(ss.str());");
            this.Output.WriteLine("return r;");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.WriteLine("public: static string Concat(string s1, string s2, string s3) {");
            this.Output.Indent++;
            this.Output.WriteLine("string r;");
            this.Output.WriteLine("r.append(s1);");
            this.Output.WriteLine("r.append(s2);");
            this.Output.WriteLine("r.append(s3);");
            this.Output.WriteLine("return r;");
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.Indent--;
            this.Output.WriteLine("};");
            this.Output.WriteLine(string.Empty);

            this.Output.WriteLine("class Exception: public Object {");
            this.Output.WriteLine("};");
            this.Output.WriteLine(string.Empty);

            this.Output.WriteLine("class ArgumentException: public Exception {");
            this.Output.Indent++;
            this.Output.WriteLine("public: ArgumentException(string msg) {}");
            this.Output.Indent--;
            this.Output.WriteLine("};");
            this.Output.WriteLine(string.Empty);

            this.Output.WriteLine("class IDisposable {");
            this.Output.Indent++;
            this.Output.WriteLine("public: virtual void Dispose() = 0;");
            this.Output.Indent--;
            this.Output.WriteLine("};");
            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void WriteTypeEnd(Type type)
        {
            this.Output.Indent--;
            this.Output.WriteLine("};");
            this.Output.WriteLine(string.Empty);

            // after writing type you need to generate static members
            foreach (var field in this.fieldsInfo)
            {
                if (!field.IsStatic)
                {
                    continue;
                }

                this.WriteTypePrefix(this.Output, field.FieldType, false, true);
                this.Output.Write(' ');

                this.Output.Write(type);
                this.Output.Write("::");
                this.Output.Write(field.Name);
                this.WriteTypeSuffix(this.Output, field.FieldType, true);
                this.Output.WriteLine(';');
            }

            if (this.StaticConstructors != null)
            {
                this.Output.Write("int ");
                this.Output.Write(type);
                this.Output.Write("_ctor_s_initialized = ");
                this.Output.Write(type);
                this.Output.WriteLine("::_ctor_s();");
            }

            this.Output.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void WriteTypeStart(Type type, Type genericType)
        {
            this.ReadTypeInfo(type, genericType);

            if (type.IsGenericType)
            {
                this.Output.Write("template <");
                WriteGenericParameters(this.Output, type);
                this.Output.Write("> ");
            }

            this.Output.Write(type.IsValueType ? "struct " : "class ");

            this.WriteName(this.Output, type);

            var index = 0;

            var baseType = type.BaseType;
            if (baseType != null
                
                ////&& baseType.Namespace != "System"
                ////&& baseType.Name != "Object"
                )
            {
                if (index == 0)
                {
                    this.Output.Write(" :");
                }

                this.Output.Write(" public ");
                this.WriteTypeWithoutReferences(this.Output, baseType);

                index++;
            }

            foreach (var @interface in type.GetInterfaces())
            {
                if (type.BaseType.GetInterfaces().Contains(@interface))
                {
                    continue;
                }

                if (index == 0)
                {
                    this.Output.Write(" :");
                }

                if (index > 0)
                {
                    this.Output.Write(",");
                }

                this.Output.Write(" public ");
                this.WriteTypeWithoutReferences(this.Output, @interface);

                index++;
            }

            this.Output.WriteLine(" {");

            this.Output.Indent++;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="increment">
        /// </param>
        /// <param name="before">
        /// </param>
        private static void SaveIncDec(IndentedTextWriter writer, OpCodePart opCode, bool increment, bool before)
        {
            if (before)
            {
                writer.Write(increment ? "++" : "--");
            }

            SaveLocalIndex(writer, opCode);

            if (!before)
            {
                writer.Write(increment ? "++" : "--");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private static void SaveLocalIndex(IndentedTextWriter writer, OpCodePart opCode)
        {
            var code = opCode.ToCode();

            writer.Write("local");
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

            writer.Write(index);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private static string TypeToCType(Type type)
        {
            var effectiveType = type;

            if (type.IsArray)
            {
                effectiveType = type.GetElementType();
            }

            if (effectiveType.Namespace == "System")
            {
                string ctype;
                if (systemTypesToCTypes.TryGetValue(effectiveType.Name, out ctype))
                {
                    return ctype;
                }
            }

            if (type.IsValueType && type.IsPrimitive)
            {
                return type.Name.ToLowerInvariant();
            }

            return type.Name.Replace('.', '_');
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        private static void WriteParametersSuffix(IndentedTextWriter writer, MethodBase methodBase)
        {
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
        private void ActualWrite(IndentedTextWriter writer, OpCodePart[] used, IEnumerable<ParameterInfo> parameterInfos, bool @isVirtual, bool hasThis)
        {
            writer.Write("(");

            var index = 0;
            foreach (var parameter in parameterInfos)
            {
                if (index > 0)
                {
                    writer.Write(", ");
                }

                var effectiveIndex = index + (@isVirtual || hasThis ? 1 : 0);

                var parameterInput = used[effectiveIndex];

                // detect if *(pointer) is required for example structure
                var parameterInputReturnResult = this.ResultOf(parameterInput);
                if (parameterInputReturnResult != null && parameter.ParameterType.IsStructureType() && parameterInputReturnResult.IsPointerAccessRequired)
                {
                    writer.Write("*");
                }

                this.ActualWrite(writer, parameterInput);

                index++;
            }

            writer.Write(")");
        }

        /// <summary>
        /// if true - suppress ; at the end of line
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        private Ending ActualWrite(IndentedTextWriter writer, OpCodePart opCode)
        {
            var endings = Ending.SeparatorWithNewLine;

            if (opCode.Skip)
            {
                return Ending.NoEndings;
            }

            this.WriteCaseAndLabels(writer, opCode);
            this.WriteTry(writer, opCode);

            var block = opCode as OpCodeBlock;
            if (block != null)
            {
                endings = this.ActualWriteBlock(writer, block);
            }
            else
            {
                endings = this.ActualWriteOpCode(writer, opCode, endings);
            }

            endings = this.WriteCatchFinnally(writer, opCode, endings);

            return endings;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="block">
        /// </param>
        /// <returns>
        /// </returns>
        private Ending ActualWriteBlock(IndentedTextWriter writer, OpCodeBlock block)
        {
            if (block.UseAsEmpty)
            {
                this.ActualWriteBlockBody(writer, block);
                return Ending.NoEndings;
            }

            if (block.UseAsConditionalExpression)
            {
                writer.Write("(");

                // thos os a hack for return () ? a : b; expressions
                int expressionPart = -1;
                if (block.OpCodes[block.OpCodes.Length - 2].OpCode.FlowControl == FlowControl.Branch)
                {
                    expressionPart = 3;
                }
                else if (block.OpCodes[block.OpCodes.Length - 2].OpCode.FlowControl == FlowControl.Return)
                {
                    expressionPart = 2;
                }

                for (var i = 0; i < block.OpCodes.Length - expressionPart; i++)
                {
                    var current = block.OpCodes[i];

                    if (i > 0)
                    {
                        if (current.ConjunctionOrCondition)
                        {
                            writer.Write(" || ");
                        }

                        if (current.ConjunctionAndCondition)
                        {
                            writer.Write(" && ");
                        }
                    }

                    for (var k = 0; k < current.OpenRoundBrackets; k++)
                    {
                        writer.Write('(');
                    }

                    this.ActualWrite(writer, current);

                    for (var k = 0; k < current.CloseRoundBrackets; k++)
                    {
                        writer.Write(')');
                    }
                }

                writer.Write(") ? ");
                this.ActualWrite(writer, block.OpCodes[block.OpCodes.Length - 1]);
                writer.Write(" : ");
                if (expressionPart == 2)
                {
                    this.ActualWrite(writer, block.OpCodes[block.OpCodes.Length - expressionPart].OpCodeOperands[0]);
                }
                else
                {
                    this.ActualWrite(writer, block.OpCodes[block.OpCodes.Length - expressionPart]);
                }
            }

            // todo: if 'If' contains x == 1 || x == 2 somehow it thinks that this is 'Switch'
            if (block.UseAsIf)
            {
                writer.Write("if (");

                var count = 0;
                while (count < block.OpCodes.Length && block.OpCodes[count].UseAsIfWhileForSubCondition)
                {
                    var current = block.OpCodes[count];
                    for (var k = 0; k < current.OpenRoundBrackets; k++)
                    {
                        writer.Write('(');
                    }

                    if (count > 0)
                    {
                        if (current.ConjunctionAndCondition)
                        {
                            writer.Write(" && ");
                        }

                        if (current.ConjunctionOrCondition)
                        {
                            writer.Write(" || ");
                        }
                    }

                    this.ActualWrite(writer, current);

                    for (var k = 0; k < current.CloseRoundBrackets; k++)
                    {
                        writer.Write(')');
                    }

                    count++;
                }

                if (count == 0)
                {
                    count++;
                    this.ActualWrite(writer, block.OpCodes[0]);
                }

                writer.Write(") ");

                this.ActualWriteBlockBody(writer, block, count);

                return Ending.NoEndings;
            }

            if (block.UseAsElse)
            {
                writer.Write("else ");

                this.ActualWriteBlockBody(writer, block, 1);

                return Ending.NoEndings;
            }

            if (block.UseAsFor)
            {
                writer.Write("for (");
                writer.Write(";");
                writer.Write(";");
                writer.Write(") ");

                this.ActualWriteBlockBody(writer, block, 0, 1);

                return Ending.NoEndings;
            }

            if (block.UseAsDoWhile)
            {
                writer.Write("do ");

                this.ActualWriteBlockBody(writer, block, 0, 1);

                writer.Write("while (");
                this.ActualWrite(writer, block.OpCodes[block.OpCodes.Length - 1]);
                writer.Write(")");

                return Ending.SeparatorWithNewLine;
            }

            if (block.UseAsWhile)
            {
                var nestedOpCodeBlock = block.OpCodes.Last() as OpCodeBlock;

                if (nestedOpCodeBlock.UseAsFor)
                {
                    writer.Write("for (;");
                    this.ActualWrite(writer, nestedOpCodeBlock.OpCodes.Last());
                    writer.Write(";");
                    this.ActualWrite(writer, nestedOpCodeBlock.OpCodes.Skip(nestedOpCodeBlock.OpCodes.Length - 2).First());
                    writer.Write(") ");

                    this.ActualWriteBlockBody(writer, nestedOpCodeBlock, 0, 2);
                }
                else
                {
                    writer.Write("while (");
                    this.ActualWrite(writer, nestedOpCodeBlock.OpCodes.Last());
                    writer.Write(") ");

                    this.ActualWriteBlockBody(writer, nestedOpCodeBlock, 0, 1);
                }

                return Ending.NoEndings;
            }

            if (block.UseAsSwitch)
            {
                if (!block.UseAsIfElseSwitch)
                {
                    writer.Write("switch (");

                    var first = block.OpCodes.First();

                    if (first.Any(Code.Switch))
                    {
                        var switchOpCode = first as OpCodeLabelsPart;
                        if (switchOpCode.OpCodeOperands[0].OpCode.StackBehaviourPop == StackBehaviour.Pop1_pop1
                            && switchOpCode.OpCodeOperands[0].OpCodeOperands[1].OpCode.StackBehaviourPush == StackBehaviour.Pushi)
                        {
                            this.ActualWrite(writer, switchOpCode.OpCodeOperands[0].OpCodeOperands[0]);
                        }
                        else
                        {
                            this.ActualWrite(writer, first.OpCodeOperands[0]);
                        }
                    }
                    else
                    {
                        this.ActualWrite(writer, first.OpCodeOperands[0]);
                    }

                    writer.Write(") ");

                    this.ActualWriteBlockBody(writer, block);
                }
                else
                {
                    // switch by If/Else
                    var hasAny = false;
                    foreach (var subOpCode in block.OpCodes)
                    {
                        if (subOpCode.Cases != null)
                        {
                            if (hasAny)
                            {
                                writer.Indent--;
                                writer.Write("}");
                                writer.Write(" else ");
                            }

                            writer.Write("if (");

                            hasAny = true;

                            var index = 0;
                            foreach (var @case in subOpCode.Cases)
                            {
                                if (index > 0)
                                {
                                    writer.Write(" || ");
                                }

                                @case.Skip = false;
                                this.ActualWrite(writer, @case);
                                @case.Skip = true;

                                index++;
                            }

                            writer.WriteLine(") {");
                            writer.Indent++;
                        }

                        if (subOpCode.UseAsCaseBreak)
                        {
                            continue;
                        }

                        var endings = this.ActualWrite(writer, subOpCode);
                        this.WriteEndings(endings);
                    }

                    if (hasAny)
                    {
                        writer.Indent--;
                        writer.Write("}");
                    }

                    return Ending.NewLine;
                }

                return Ending.NoEndings;
            }

            return Ending.SeparatorWithNewLine;
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
        private void ActualWriteBlockBody(IndentedTextWriter writer, OpCodeBlock block, int skip = 0, int? reduce = null)
        {
            writer.WriteLine('{');
            writer.Indent++;

            var query = reduce.HasValue ? block.OpCodes.Take(block.OpCodes.Length - reduce.Value).Skip(skip) : block.OpCodes.Skip(skip);

            foreach (var subOpCode in query)
            {
                var endings = this.ActualWrite(writer, subOpCode);
                this.WriteEndings(endings);
            }

            writer.Indent--;
            writer.WriteLine('}');
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="endings">
        /// </param>
        /// <returns>
        /// </returns>
        private Ending ActualWriteOpCode(IndentedTextWriter writer, OpCodePart opCode, Ending endings)
        {
            var code = opCode.ToCode();
            switch (code)
            {
                case Code.Ldc_I4_0:
                    if (opCode.UseAsBoolean)
                    {
                        writer.Write("false");
                    }
                    else
                    {
                        writer.Write('0');
                    }

                    break;
                case Code.Ldc_I4_1:
                    if (opCode.UseAsBoolean)
                    {
                        writer.Write("true");
                    }
                    else
                    {
                        writer.Write('1');
                    }

                    break;
                case Code.Ldc_I4_2:
                    writer.Write('2');
                    break;
                case Code.Ldc_I4_3:
                    writer.Write('3');
                    break;
                case Code.Ldc_I4_4:
                    writer.Write('4');
                    break;
                case Code.Ldc_I4_5:
                    writer.Write('5');
                    break;
                case Code.Ldc_I4_6:
                    writer.Write('6');
                    break;
                case Code.Ldc_I4_7:
                    writer.Write('7');
                    break;
                case Code.Ldc_I4_8:
                    writer.Write('8');
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
                    writer.Write(opCodeSingle.Operand);
                    break;
                case Code.Ldc_R8:
                    var opCodeDouble = opCode as OpCodeDoublePart;
                    writer.Write(opCodeDouble.Operand);
                    break;
                case Code.Ldstr:
                    var opCodeString = opCode as OpCodeStringPart;
                    writer.Write('"');
                    writer.Write(opCodeString.Operand);
                    writer.Write('"');
                    break;
                case Code.Ldnull:
                    writer.Write("nullptr");
                    break;
                case Code.Ldfld:
                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    var isThis = this.WriteMemberAccess(writer, opCodeFieldInfoPart);
                    this.WriteMemberName(writer, opCodeFieldInfoPart.Operand, false, isThis);
                    break;
                case Code.Ldflda:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    isThis = this.WriteMemberAccess(writer, opCodeFieldInfoPart);
                    this.WriteMemberName(writer, opCodeFieldInfoPart.Operand, !isThis);
                    break;
                case Code.Ldsfld:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.WriteMemberName(writer, opCodeFieldInfoPart.Operand, true);
                    break;
                case Code.Stfld:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    isThis = this.WriteMemberAccess(writer, opCodeFieldInfoPart);
                    this.WriteMemberName(writer, opCodeFieldInfoPart.Operand, false, false);
                    writer.Write(" = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Stsfld:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    writer.Write(opCodeFieldInfoPart.Operand.Name);
                    writer.Write(" = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Stobj:
                    var opCodeTypePart = opCode as OpCodeTypePart;
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Ldlen:
                    writer.Write("sizeof(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")/sizeof(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write("[0])");
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
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write("[");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    writer.Write("]");
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
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write("[");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    writer.Write("] = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[2]);
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
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Stind_I:
                case Code.Stind_I1:
                case Code.Stind_I2:
                case Code.Stind_I4:
                case Code.Stind_I8:
                case Code.Stind_R4:
                case Code.Stind_R8:
                case Code.Stind_Ref:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Call:
                case Code.Callvirt:
                    var opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;
                    var methodBase = opCodeMethodInfoPart.Operand;
                    this.WriteCall(writer, opCodeMethodInfoPart, code == Code.Callvirt, methodBase.CallingConvention.HasFlag(CallingConventions.HasThis));
                    break;
                case Code.Add:
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" + ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Mul:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" * ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Sub:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" - ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Div:
                case Code.Div_Un:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" / ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Rem:
                case Code.Rem_Un:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" % ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.And:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" & ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Or:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" | ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Xor:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" ^ ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Shl:
                    writer.Write('(');
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" << ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    writer.Write(')');
                    break;
                case Code.Shr:
                case Code.Shr_Un:
                    writer.Write('(');
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" >> ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    writer.Write(')');
                    break;
                case Code.Not:
                    writer.Write("~");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Neg:
                    writer.Write("-");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Dup:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Box:
                case Code.Unbox:
                case Code.Unbox_Any:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Ret:
                    writer.Write("return");
                    if (opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0)
                    {
                        writer.Write(' ');

                        var requiredType = this.RequiredType(opCode);

                        var oper1 = opCode.OpCodeOperands[0];
                        var oper1ReturnResult = this.ResultOf(oper1);

                        if (!requiredType.IsPointerAccessRequired && oper1ReturnResult.IsPointerAccessRequired)
                        {
                            // *<referense of value type>
                            writer.Write("*");
                        }

                        this.ActualWrite(writer, oper1);
                    }

                    break;
                case Code.Stloc:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                case Code.Stloc_S:

                    this.SaveLocal(writer, opCode);

                    break;
                case Code.Ldloc:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldloc_S:
                case Code.Ldloca_S:
                    writer.Write("local");
                    var asString = code.ToString();

                    if (code == Code.Ldloca_S || code == Code.Ldloc_S || code == Code.Ldloc)
                    {
                        opCodeInt32 = opCode as OpCodeInt32Part;
                        writer.Write(opCodeInt32.Operand);
                    }
                    else
                    {
                        writer.Write(asString.Substring(asString.Length - 1));
                    }

                    break;
                case Code.Ldarg:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldarg_S:
                    asString = code.ToString();
                    var index = 0;
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
                        writer.Write("this");
                    }
                    else
                    {
                        writer.Write(this.ParameterInfo[index - (this.HasMethodThis ? 1 : 0)].Name);
                    }

                    break;

                case Code.Starg:
                case Code.Starg_S:

                    asString = code.ToString();
                    opCodeInt32 = opCode as OpCodeInt32Part;
                    index = opCodeInt32.Operand;

                    if (this.HasMethodThis && index == 0)
                    {
                        writer.Write("this");
                    }
                    else
                    {
                        writer.Write(this.ParameterInfo[index - (this.HasMethodThis ? 1 : 0)].Name);
                    }

                    writer.Write(" = ");

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);

                    break;

                case Code.Ldftn:

                    writer.Write('&');
                    var opCodeMethodPart = opCode as OpCodeMethodInfoPart;
                    this.WriteMethodName(writer, opCodeMethodPart.Operand, true);

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

                    if (opCode.UseAsConditionalBreak || opCode.UseAsConditionalContinue)
                    {
                        writer.Write("if (");
                    }

                    opCodeInt32 = opCode as OpCodeInt32Part;

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);

                    if (!opCode.InvertCondition)
                    {
                        // we need to invert all comare command
                        switch (code)
                        {
                            case Code.Beq:
                            case Code.Beq_S:
                                writer.Write(" == ");
                                break;
                            case Code.Bne_Un:
                            case Code.Bne_Un_S:
                                writer.Write(" != ");
                                break;
                            case Code.Blt:
                            case Code.Blt_S:
                            case Code.Blt_Un:
                            case Code.Blt_Un_S:
                                writer.Write(" < ");
                                break;
                            case Code.Ble:
                            case Code.Ble_S:
                            case Code.Ble_Un:
                            case Code.Ble_Un_S:
                                writer.Write(" <= ");
                                break;
                            case Code.Bgt:
                            case Code.Bgt_S:
                            case Code.Bgt_Un:
                            case Code.Bgt_Un_S:
                                writer.Write(" > ");
                                break;
                            case Code.Bge:
                            case Code.Bge_S:
                            case Code.Bge_Un:
                            case Code.Bge_Un_S:
                                writer.Write(" >= ");
                                break;
                        }
                    }
                    else
                    {
                        // we need to invert all comare command
                        switch (code)
                        {
                            case Code.Beq:
                            case Code.Beq_S:
                                writer.Write(" != ");
                                break;
                            case Code.Bne_Un:
                            case Code.Bne_Un_S:
                                writer.Write(" == ");
                                break;
                            case Code.Blt:
                            case Code.Blt_S:
                                writer.Write(" >= ");
                                break;
                            case Code.Ble:
                            case Code.Ble_S:
                                writer.Write(" > ");
                                break;
                            case Code.Bgt:
                            case Code.Bgt_S:
                                writer.Write(" <= ");
                                break;
                            case Code.Bge:
                            case Code.Bge_S:
                                writer.Write(" < ");
                                break;
                        }
                    }

                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);

                    if (opCode.UseAsConditionalBreak || opCode.UseAsConditionalContinue)
                    {
                        writer.WriteLine(") {");
                        writer.Indent++;
                        writer.WriteLine(opCode.UseAsConditionalBreak ? "break;" : "continue;");
                        writer.Indent--;
                        writer.WriteLine("}");
                        endings = Ending.NoEndings;
                    }

                    break;
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:

                    if (opCode.UseAsConditionalBreak || opCode.UseAsConditionalContinue)
                    {
                        writer.Write("if (");
                    }

                    opCodeInt32 = opCode as OpCodeInt32Part;

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);

                    var returnType = this.ResultOf(opCode.OpCodeOperands[0]);

                    if (code == Code.Brtrue || code == Code.Brtrue_S)
                    {
                        if (!opCode.InvertCondition)
                        {
                            writer.Write(" != ");
                        }
                        else
                        {
                            writer.Write(" == ");
                        }
                    }

                    if (code == Code.Brfalse || code == Code.Brfalse_S)
                    {
                        if (!opCode.InvertCondition)
                        {
                            writer.Write(" == ");
                        }
                        else
                        {
                            writer.Write(" != ");
                        }
                    }

                    if (returnType != null && returnType.IsPointerAccessRequired)
                    {
                        writer.Write("nullptr");
                    }
                    else if (returnType != null && returnType.IsTypeOf(typeof(bool)))
                    {
                        writer.Write("false");
                    }
                    else
                    {
                        writer.Write("0");
                    }

                    if (opCode.UseAsConditionalBreak || opCode.UseAsConditionalContinue)
                    {
                        writer.WriteLine(") {");
                        writer.Indent++;
                        writer.WriteLine(opCode.UseAsConditionalBreak ? "break;" : "continue;");
                        writer.Indent--;
                        writer.WriteLine("}");
                        endings = Ending.NoEndings;
                    }
                    else
                    {
                        endings = Ending.NewLine;
                    }

                    break;
                case Code.Br:
                case Code.Br_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;

                    if (opCode.UseAsBreak || opCode.UseAsCaseBreak)
                    {
                        writer.Write("break");
                        endings = Ending.SeparatorWithNewLine;
                    }

                    if (opCode.UseAsContinue)
                    {
                        writer.Write("continue");
                        endings = Ending.SeparatorWithNewLine;
                    }

                    if (opCode.UseAsElse)
                    {
                        writer.Write("else");
                        endings = Ending.NewLine;
                    }

                    break;
                case Code.Ceq:
                case Code.Clt:
                case Code.Clt_Un:
                case Code.Cgt:
                case Code.Cgt_Un:

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);

                    // we need to invert all comare command
                    switch (code)
                    {
                        case Code.Ceq:
                            writer.Write(" == ");
                            break;
                        case Code.Clt:
                        case Code.Clt_Un:
                            writer.Write(" < ");
                            break;
                        case Code.Cgt:
                        case Code.Cgt_Un:
                            writer.Write(" > ");
                            break;
                    }

                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;

                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                    writer.Write("(int) (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;

                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                    writer.Write("(unsigned int) (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;

                case Code.Conv_R_Un:
                    writer.Write("(float) (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;

                case Code.Conv_R4:
                case Code.Conv_R8:
                    writer.Write("(");
                    writer.Write(code == Code.Conv_R4 ? "float" : "double");
                    writer.Write(") (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;

                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:

                    bool isInt;
                    bool isUInt;
                    char size;

                    asString = code.ToString();
                    if (asString.StartsWith("Conv_Ovf_"))
                    {
                        isInt = asString["Conv_Ovf_".Length] == 'I';
                        isUInt = asString["Conv_Ovf_".Length] == 'U';
                        size = asString["Conv_Ovf_".Length + 1];
                    }
                    else
                    {
                        isInt = asString["Conv_".Length] == 'I';
                        isUInt = asString["Conv_".Length] == 'U';
                        size = asString["Conv_".Length + 1];
                    }

                    writer.Write("(");

                    if (isUInt)
                    {
                        writer.Write("unsigned ");
                    }

                    switch (size)
                    {
                        case '1':
                            writer.Write("char");
                            break;
                        case '2':
                            writer.Write("short");
                            break;
                        case '4':
                            writer.Write("int");
                            break;
                        case '8':
                            writer.Write("long");
                            break;
                    }

                    writer.Write(") (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");

                    break;

                case Code.Castclass:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    writer.Write("dynamic_cast<");
                    this.WriteTypePrefix(writer, opCodeTypePart.Operand, true);
                    writer.Write(">(");
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.Write(")");

                    break;

                case Code.Isinst:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    writer.Write("dynamic_cast<");
                    this.WriteTypePrefix(writer, opCodeTypePart.Operand, true);
                    writer.Write(">(");
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.Write(") != nullptr");

                    break;

                case Code.Newobj:

                    var opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;

                    var declaringType = opCodeConstructorInfoPart.Operand.DeclaringType;

                    writer.Write("new ");
                    this.WriteTypeWithoutReferences(writer, declaringType);
                    this.WriteCall(writer, opCode as OpCodeConstructorInfoPart, code == Code.Callvirt, false);

                    break;

                case Code.Newarr:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    writer.Write("new ");
                    writer.Write(TypeToCType(opCodeTypePart.Operand));
                    writer.Write("[");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write("]");

                    break;

                case Code.Initobj:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);

                    writer.Write(" = new ");
                    writer.Write(TypeToCType(opCodeTypePart.Operand));

                    break;

                case Code.Throw:

                    writer.Write("throw ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);

                    break;

                case Code.Rethrow:

                    writer.Write("throw");

                    break;

                case Code.Leave:
                case Code.Leave_S:
                case Code.Endfilter:
                case Code.Endfinally:
                    endings = Ending.NoEndings;
                    break;

                case Code.Pop:
                    endings = Ending.NoEndings;
                    break;

                case Code.Constrained:

                    // to solve the problem with referencing ValueType and Class type in Generic type
                    endings = Ending.NoEndings;
                    break;
            }

            return endings;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void SaveLocal(IndentedTextWriter writer, OpCodePart opCode)
        {
            SaveLocalIndex(writer, opCode);
            writer.Write(" = ");
            this.ActualWrite(writer, opCode.OpCodeOperands[0]);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeConstructorInfo">
        /// </param>
        /// <param name="isVirtual">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        private void WriteCall(IndentedTextWriter writer, OpCodeConstructorInfoPart opCodeConstructorInfo, bool @isVirtual, bool hasThis)
        {
            // temp hack
            var ctor = opCodeConstructorInfo.Operand;
            this.ActualWrite(writer, opCodeConstructorInfo.OpCodeOperands, ctor.GetParameters(), @isVirtual, hasThis);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="isVirtual">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        private void WriteCall(IndentedTextWriter writer, OpCodeMethodInfoPart opCodeMethodInfo, bool isVirtual, bool hasThis)
        {
            // temp hack
            var methodBase = opCodeMethodInfo.Operand;

            if (methodBase.Name == ".ctor")
            {
                writer.Write("// ");
            }

            if (isVirtual || hasThis)
            {
                this.WriteMemberAccess(writer, opCodeMethodInfo);
            }

            var startsWithThis = hasThis && opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0
                                 && opCodeMethodInfo.OpCodeOperands[0].Any(Code.Ldarg_0);

            this.WriteMethodName(writer, methodBase, methodBase.IsStatic, startsWithThis);

            this.ActualWrite(writer, opCodeMethodInfo.OpCodeOperands, methodBase.GetParameters(), isVirtual, hasThis);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void WriteCaseAndLabels(IndentedTextWriter writer, OpCodePart opCode)
        {
            if (opCode.Cases != null)
            {
                foreach (var @case in opCode.Cases)
                {
                    if (@case.Skip)
                    {
                        continue;
                    }

                    // write case
                    writer.Write("case ");
                    this.ActualWrite(writer, @case);
                    writer.WriteLine(":");
                }
            }

            if (opCode.DefaultCase)
            {
                // write case
                writer.WriteLine("default:");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="endings">
        /// </param>
        /// <returns>
        /// </returns>
        private Ending WriteCatchFinnally(IndentedTextWriter writer, OpCodePart opCode, Ending endings)
        {
            if (opCode.EndOfTry != null && opCode.EndOfTry.Count > 0)
            {
                this.WriteEndings(endings);

                foreach (var endOfTryId in opCode.EndOfTry)
                {
                    writer.WriteLine(string.Empty);
                    writer.Indent--;
                    writer.WriteLine("}");

                    endings = Ending.NewLine;
                }
            }

            if (opCode.EndOfClausesOrFinal != null && opCode.EndOfClausesOrFinal.Count > 0)
            {
                this.WriteEndings(endings);

                foreach (var endOfCaluseId in opCode.EndOfClausesOrFinal)
                {
                    // writer.WriteLine(string.Empty);
                    writer.Indent--;
                    writer.WriteLine("}");

                    endings = Ending.NewLine;
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

                endings = Ending.NewLine;
            }

            return endings;
        }

        /// <summary>
        /// </summary>
        /// <param name="endings">
        /// </param>
        private void WriteEndings(Ending endings)
        {
            switch (endings)
            {
                case Ending.SeparatorWithNewLine:
                    this.Output.WriteLine(";");
                    break;
                case Ending.NewLine:
                    this.Output.WriteLine(string.Empty);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="method">
        /// </param>
        private void WriteGenericParameters(IndentedTextWriter writer, MethodInfo method)
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
        private void WriteGenericParameters(IndentedTextWriter writer, Type type)
        {
            var index = type.Name.IndexOf('`');
            var level = int.Parse(type.Name.Substring(index + 1));

            for (int i = 0; i < level; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }

                writer.Write("typename ");

                Type generic = null;

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
        /// <param name="locals">
        /// </param>
        private void WriteLocalVariableDeclarations(IList<LocalVariableInfo> locals)
        {
            foreach (var local in locals)
            {
                this.WriteTypePrefix(this.Output, local.LocalType, true, true);
                this.Output.Write(" local{0}", local.LocalIndex);
                this.WriteTypeSuffix(this.Output, local.LocalType, true);
                this.Output.WriteLine(';');
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        private bool WriteMemberAccess(IndentedTextWriter writer, OpCodePart opCodePart)
        {
            var oper1 = opCodePart.OpCodeOperands[0];

            this.ActualWrite(writer, oper1);

            var oper1ReturnResult = this.ResultOf(oper1);
            var isThis = this.IsThis(oper1);
            if (!isThis && oper1ReturnResult != null && oper1ReturnResult.IsDotAccessRequired)
            {
                writer.Write(".");
            }
            else
            {
                writer.Write("->");
            }

            return isThis;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="memberInfo">
        /// </param>
        /// <param name="withNamespace">
        /// </param>
        /// <param name="ifDifferent">
        /// </param>
        private void WriteMemberName(IndentedTextWriter writer, MemberInfo memberInfo, bool withNamespace = false, bool ifDifferent = true)
        {
            if (withNamespace || ifDifferent && this.ThisType != memberInfo.DeclaringType)
            {
                this.WriteTypeWithoutReferences(writer, memberInfo.DeclaringType);
                writer.Write("::");
            }

            writer.Write(memberInfo.Name.Replace('.', '_'));
        }

        /// <summary>
        /// </summary>
        /// <param name="endPart">
        /// </param>
        private void WriteMethodBody(string endPart = null)
        {
            var rest = this.PrepareWritingMethodBody();

            var i = 0;
            foreach (var opCodePart in rest)
            {
                i++;
                var endings = this.ActualWrite(this.Output, opCodePart);

                if (endPart != null && i == rest.Length)
                {
                    this.Output.Write(endPart);
                }

                switch (endings)
                {
                    case Ending.SeparatorWithNewLine:
                        this.Output.WriteLine(";");
                        break;
                    case Ending.NewLine:
                        this.Output.WriteLine(string.Empty);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        /// <param name="withNamespace">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        private void WriteMethodName(IndentedTextWriter writer, MethodBase methodBase, bool withNamespace = false, bool hasThis = false)
        {
            this.WriteMemberName(writer, methodBase, withNamespace, hasThis);

            // add parameters suffixes
            WriteParametersSuffix(writer, methodBase);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="parameterInfos">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        private void WriteMethodParamsDef(IndentedTextWriter writer, IEnumerable<ParameterInfo> parameterInfos, bool hasThis)
        {
            writer.Write("(");

            var start = hasThis ? 1 : 0;
            var index = start;
            foreach (var parameter in parameterInfos)
            {
                if (index > start)
                {
                    writer.Write(", ");
                }

                this.WriteTypePrefix(writer, parameter.ParameterType, false);
                writer.Write(" ");
                writer.Write(parameter.Name);
                this.WriteTypeSuffix(writer, parameter.ParameterType);

                index++;
            }

            writer.Write(")");
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        private void WriteName(IndentedTextWriter writer, Type type)
        {
            var typeBaseName = TypeToCType(type);

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
        /// <param name="opCode">
        /// </param>
        private void WriteTry(IndentedTextWriter writer, OpCodePart opCode)
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
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="structAsReference">
        /// </param>
        /// <param name="isZeroArraySize">
        /// </param>
        private void WriteTypePrefix(IndentedTextWriter writer, Type type, bool structAsReference = true, bool isZeroArraySize = false)
        {
            var effectiveType = this.WriteTypeWithoutReferences(writer, type);

            var isReference = !effectiveType.IsPrimitive && !effectiveType.IsValueType;
            var isStructAsReference = structAsReference && effectiveType.IsLayoutSequential && !effectiveType.IsPrimitive;

            if ((isReference || isStructAsReference) && !type.IsGenericParameter)
            {
                if (effectiveType.Namespace != "System" || effectiveType.Name != "String")
                {
                    writer.Write("*");
                }
            }

            if (type.IsArray && isZeroArraySize)
            {
                writer.Write("*");
            }

            if (type.IsByRef)
            {
                writer.Write("&");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="isZeroArraySize">
        /// </param>
        private void WriteTypeSuffix(IndentedTextWriter writer, Type type, bool isZeroArraySize = false)
        {
            if (type.IsArray && !isZeroArraySize)
            {
                writer.Write("[]");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private Type WriteTypeWithoutReferences(IndentedTextWriter writer, Type type)
        {
            var effectiveType = type;

            if (type.HasElementType)
            {
                effectiveType = type.GetElementType();
            }

            // write base name
            this.WriteName(writer, effectiveType);

            // write generic parameters
            if (type.IsGenericType)
            {
                writer.Write('<');

                int count = 0;
                foreach (var genericParam in type.GenericTypeArguments)
                {
                    if (count > 0)
                    {
                        writer.Write(", ");
                    }

                    this.WriteTypePrefix(this.Output, genericParam, true, true);
                    this.WriteTypeSuffix(this.Output, genericParam, true);

                    count++;
                }

                writer.Write('>');
            }

            return effectiveType;
        }

        #endregion
    }
}
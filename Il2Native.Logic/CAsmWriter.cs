//#define USE_C_STRING
//#define IGNORE_GENERIC_INTERFACES
//#define IGNORE_INTERFACES
namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Runtime.Remoting;
    using System.Text;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Properties;

    /// <summary>
    /// </summary>
    public class CAsmWriter : BaseWriter, ICodeWriter
    {
        #region Static Fields

        /// <summary>
        /// </summary>
        private static IDictionary<string, string> systemTypesToCShortTypes = new SortedDictionary<string, string>();

        /// <summary>
        /// </summary>
        private static IDictionary<string, string> systemTypesToCTypes = new SortedDictionary<string, string>();

        private static IList<Tuple<string, string>> excludeMethods = new List<Tuple<string, string>>();

        private static IList<Tuple<string, string>> excludeMethodsBody = new List<Tuple<string, string>>();

        #endregion

        #region Fields

        /// <summary>
        /// </summary>
        private List<FieldInfo> fieldsInfo = new List<FieldInfo>();

        /// <summary>
        /// </summary>
        private string headerFile;

        /// <summary>
        /// </summary>
        private string lastNamespace;

        /// <summary>
        /// </summary>
        private bool namespaceClosed;

        /// <summary>
        /// </summary>
        private char[] specChars = new[] { '<', '>', '{', '}', '-', ',' };

        /// <summary>
        /// </summary>
        private char[] specCharsClean = new[] { '<', '>', '{', '}', '-', ' ', ',', '[', ']', '*' };

        /// <summary>
        /// </summary>
        private WritingMode writingMode;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        static CAsmWriter()
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
            systemTypesToCTypes["Void"] = "void";
#if USE_C_STRING
            systemTypesToCTypes["String"] = "string";
#endif
            systemTypesToCTypes["Byte&"] = "unsigned char";
            systemTypesToCTypes["SByte&"] = "char";
            systemTypesToCTypes["Char&"] = "wchar_t";
            systemTypesToCTypes["Int16&"] = "short";
            systemTypesToCTypes["Int32&"] = "int";
            systemTypesToCTypes["Int64&"] = "long";
            systemTypesToCTypes["IntPtr"] = "void*"; 
            systemTypesToCTypes["UIntPtr"] = "void*";
            systemTypesToCTypes["UInt16&"] = "unsigned short";
            systemTypesToCTypes["UInt32&"] = "unsigned int";
            systemTypesToCTypes["UInt64&"] = "unsigned long";
            systemTypesToCTypes["Float&"] = "float";
            systemTypesToCTypes["Single&"] = "float";
            systemTypesToCTypes["Double&"] = "double";
            systemTypesToCTypes["Boolean&"] = "bool";
#if USE_C_STRING
            systemTypesToCTypes["String&"] = "string";
#endif
            systemTypesToCTypes["Void*"] = "void*";

            systemTypesToCShortTypes["Byte"] = "uc";
            systemTypesToCShortTypes["SByte"] = "c";
            systemTypesToCShortTypes["Char"] = "wc";
            systemTypesToCShortTypes["Int16"] = "shr";
            systemTypesToCShortTypes["Int32"] = "i";
            systemTypesToCShortTypes["Int64"] = "l";
            systemTypesToCShortTypes["UInt16"] = "usrt";
            systemTypesToCShortTypes["UInt32"] = "ui";
            systemTypesToCShortTypes["UInt64"] = "ul";
            systemTypesToCShortTypes["Float"] = "f";
            systemTypesToCShortTypes["Single"] = "sgl";
            systemTypesToCShortTypes["Double"] = "dbl";
            systemTypesToCShortTypes["Boolean"] = "b";
            systemTypesToCShortTypes["String"] = "str";
            systemTypesToCShortTypes["Void"] = "void";
            systemTypesToCShortTypes["Byte&"] = "ruc";
            systemTypesToCShortTypes["SByte&"] = "rc";
            systemTypesToCShortTypes["Char&"] = "rwc";
            systemTypesToCShortTypes["Int16&"] = "rshr";
            systemTypesToCShortTypes["Int32&"] = "ri";
            systemTypesToCShortTypes["Int64&"] = "rl";
            systemTypesToCShortTypes["IntPtr"] = "riptr";
            systemTypesToCShortTypes["UIntPtr"] = "ruiptr";
            systemTypesToCShortTypes["IntPtr&"] = "rriptr";
            systemTypesToCShortTypes["UIntPtr&"] = "rruiptr";
            systemTypesToCShortTypes["UInt16&"] = "rushr";
            systemTypesToCShortTypes["UInt32&"] = "rui";
            systemTypesToCShortTypes["UInt64&"] = "rul";
            systemTypesToCShortTypes["Float&"] = "rf";
            systemTypesToCShortTypes["Single&"] = "rsgl";
            systemTypesToCShortTypes["Double&"] = "rdbl";
            systemTypesToCShortTypes["Boolean&"] = "rb";
            systemTypesToCShortTypes["String&"] = "rstr";
            systemTypesToCShortTypes["Void*"] = "ptr";

        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <param name="writingMode">
        /// </param>
        /// <param name="nestTypes">
        /// </param>
        public CAsmWriter(string fileName, WritingMode writingMode = WritingMode.Standart, bool nestTypes = true)
        {
            var extension = Path.GetExtension(fileName);
            var outputFile = extension != null && extension.Equals(string.Empty) ? fileName + (writingMode == WritingMode.HeaderOnly ? ".h" : ".cpp") : fileName;
            this.Output = new IndentedTextWriter(new StreamWriter(outputFile));

            this.writingMode = writingMode;
            this.headerFile = string.Concat(Path.GetFileNameWithoutExtension(fileName), ".h");

            this.DetectStatements = false;
        }

        #endregion

        #region Enums

        /// <summary>
        /// </summary>
        public enum WritingMode
        {
            /// <summary>
            /// </summary>
            Standart,

            /// <summary>
            /// </summary>
            HeaderOnly,

            /// <summary>
            /// </summary>
            CodeOnly
        }

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
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public string TypeToString(Type type)
        {
            var sw = new StringWriter();
            var iw = new IndentedTextWriter(sw);
            this.WriteTypePrefix(iw, type, false);
            this.WriteTypeSuffix(iw, type);
            iw.Close();
            return sw.ToString();
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
            if (this.writingMode == WritingMode.HeaderOnly)
            {
                return;
            }

            ////this.Output.WriteLine("// {0}", opCode.OpCode.Name);
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
            if (this.writingMode != WritingMode.CodeOnly)
            {
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// </summary>
        public void WriteBeforeConstructors()
        {
            if (this.writingMode == WritingMode.Standart)
            {
                this.Output.Indent--;
                this.Output.WriteLine(string.Empty);
                this.Output.Indent++;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="count">
        /// </param>
        public void WriteBeforeFields(int count)
        {
            this.fieldsInfo.Clear();

            if (count > 0 && this.writingMode == WritingMode.Standart)
            {
                this.Output.Indent--;
                this.Output.WriteLine(string.Empty);
                this.Output.Indent++;
            }

            if (this.writingMode != WritingMode.CodeOnly && ThisType.FullName == "System.Object")
            {
                this.Output.WriteLine("virtual ~" + GetTypeNameWithId(typeof(object)) + "() {}");
            }

            if (this.writingMode != WritingMode.CodeOnly && ThisType.IsEnum)
            {
                this.Output.Write("public: ");
                WriteName(this.Output, ThisType, true, false, true);
                this.Output.WriteLine("(int v) : value__(v) {};");
                this.Output.WriteLine("public: operator int() { return value__; }");
            }

            if (this.writingMode != WritingMode.CodeOnly && ThisType.IsPrimitiveType())
            {
                this.Output.Write("public: ");
                WriteName(this.Output, ThisType, true, false, true);
                this.Output.Write("(");
                WriteTypePrefix(this.Output, ThisType, false);
                WriteTypeSuffix(this.Output, ThisType);
                this.Output.WriteLine(" v) : m_value(v) {};");
                this.Output.Write("public: operator ");
                WriteTypePrefix(this.Output, ThisType, false);
                WriteTypeSuffix(this.Output, ThisType);
                this.Output.WriteLine("() { return m_value; }");
            }

            if (this.writingMode != WritingMode.CodeOnly && ThisType.IsValueType)
            {
                this.Output.Write("public: ");
                WriteName(this.Output, ThisType, true, false, true);
                this.Output.WriteLine("() {};");
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
        public void WriteConstructorEnd(ConstructorInfo ctor)
        {
            if (this.writingMode == WritingMode.HeaderOnly)
            {
                this.Output.WriteLine(";");
                this.WriteFactoryMethodNew(ctor);
                return;
            }

            this.WriteMethodBody(!ctor.IsStatic ? string.Empty : " 1");

            if (!this.NoBody)
            {
                this.Output.Indent--;

                this.Output.WriteLine("}");
            }

            this.Output.WriteLine(string.Empty);
            this.WriteFactoryMethodNew(ctor);
        }

        /// <summary>
        /// </summary>
        /// <param name="ctor">
        /// </param>
        public void WriteConstructorStart(ConstructorInfo ctor)
        {
            this.StartProcess();

            ReadMethodInfo(ctor);

            // write local declarations
            var methodBase = ctor.GetMethodBody();
            ////if (methodBase == null)
            ////{
            ////    return;
            ////}

            if (ctor.IsStatic)
            {
                // write type of static constuctor, it should return int
                this.Output.Write("static int ");
            }
            else
            {
                // to support ctor as Constructor
                this.Output.Write("void ");
            }

            // WriteName(this.Output, ctor.DeclaringType, true, false, true);
            this.WriteMethodName(this.Output, ctor, this.writingMode == WritingMode.CodeOnly);
            if (ctor.IsStatic)
            {
                this.StaticConstructors.Add(ctor);
            }

            if (!ctor.IsStatic)
            {
                this.WriteMethodParamsDef(this.Output, ctor.GetParameters(), this.HasMethodThis);
            }
            else
            {
                this.Output.Write("()");
            }

            if (this.writingMode == WritingMode.HeaderOnly)
            {
                return;
            }

            if (methodBase != null)
            {
                this.Output.WriteLine(" {");
                this.Output.Indent++;

                // write default initialization
                // after writing type you need to generate static members
                ////if (!ctor.IsStatic)
                ////{
                ////    foreach (var field in this.fieldsInfo)
                ////    {
                ////        if (field.IsStatic || !field.FieldType.IsValueType || !field.FieldType.IsPrimitiveType())
                ////        {
                ////            continue;
                ////        }

                ////        this.Output.Write("this->");
                ////        this.Output.Write(field.Name);
                ////        this.Output.Write(" = ");
                ////        this.Output.Write(Activator.CreateInstance(field.FieldType).ToString().ToLowerInvariant());
                ////        this.Output.WriteLine(';');
                ////    }
                ////}

                this.WriteLocalVariableDeclarations(methodBase.LocalVariables);
            }
            else
            {
                this.Output.WriteLine(ctor.IsAbstract ? " = 0;" : ";");
            }
        }

        /// <summary>
        /// </summary>
        public void WriteEnd()
        {
            if (this.MainMethod != null)
            {
                if (this.writingMode == WritingMode.HeaderOnly)
                {
                    return;
                }

                this.Output.WriteLine("int main() {");
                this.Output.Indent++;

                this.Output.Write("return ");

                this.WriteMethodName(this.Output, this.MainMethod, this.writingMode == WritingMode.CodeOnly);

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

            if (this.writingMode == WritingMode.CodeOnly)
            {
                return;
            }

            if (field.IsStatic)
            {
                this.Output.Write("static ");
            }

            this.WriteTypePrefix(this.Output, field.FieldType, false);
            this.Output.Write(' ');
            this.WriteFieldOrMethodName(this.Output, field.Name);
            this.WriteTypeSuffix(this.Output, field.FieldType);
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
            if (this.writingMode == WritingMode.CodeOnly)
            {
                return;
            }

            this.ReadTypeInfo(type, null);

            bool isLastType = count == (number + 1);

            if (type.IsPrimitiveType() || type.IsVoid() || type.IsConstructedGenericType)
            {
                if (isLastType)
                {
                    this.WriteNamespaceEnd(this.Output, this.lastNamespace);
                    this.lastNamespace = null;
                    this.Output.WriteLine(string.Empty);
                }

                return;
            }

            if (type.IsGenericType)
            {
                this.WriteTemplateDefintion(type);
            }

            this.Output.Write("struct ");

            this.WriteName(this.Output, type, true, false, true);
            if (type.IsGenericType)
            {
                WriteTemplate(type);
            }

            this.Output.WriteLine(";");

            if (isLastType)
            {
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        public void WriteMethodEnd(MethodInfo method)
        {
            if (SkipMethod(method))
            {
                return;
            }

            if (method.DeclaringType.IsInterface)
            {
                return;
            }

            if (!SkipMethodBody(method))
            {
                this.WriteMethodBody();
            }

            if (this.writingMode == WritingMode.HeaderOnly)
            {
                if (!this.NoBody)
                {
                    this.Output.WriteLine(";");
                }

                return;
            }

            if (this.NoBody && this.writingMode == WritingMode.CodeOnly)
            {
                return;
            }

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
            this.StartProcess();

            if (SkipMethod(method))
            {
                return;
            }

            if (method.DeclaringType.IsInterface)
            {
                return;
            }

            ////#if DEBUG
            ////            this.Output.Write("// ");
            ////            var methodBody = method.GetMethodBody();
            ////            if (methodBody != null)
            ////            {
            ////                foreach (var codeByte in methodBody.GetILAsByteArray())
            ////                {
            ////                    this.Output.Write(codeByte.ToString("X"));
            ////                    this.Output.Write(" ");
            ////                }
            ////            }

            ////            this.Output.WriteLine(string.Empty);
            ////#endif

            ReadMethodInfo(method);

            var isMain = method.IsStatic && method.CallingConvention.HasFlag(CallingConventions.Standard) && method.Name.Equals("Main");

            // check if main
            if (isMain)
            {
                this.MainMethod = method;
            }

            var methodBodyBytes = method.GetMethodBody();
            ////if (methodBodyBytes == null)
            ////{
            ////    return;
            ////}

            if (method.IsGenericMethod)
            {
                this.WriteTemplateDefinition(method);
            }

            if (method.IsStatic && this.writingMode != WritingMode.CodeOnly)
            {
                this.Output.Write("static ");
            }

            if (method.ReturnType.Name != "Void")
            {
                this.WriteTypePrefix(this.Output, method.ReturnType, false);
                this.WriteTypeSuffix(this.Output, method.ReturnType);
                this.Output.Write(" ");
            }
            else
            {
                this.Output.Write("void ");
            }

            this.WriteMethodName(this.Output, method, this.writingMode == WritingMode.CodeOnly);
            if (method.IsGenericMethod)
            {
                WriteTemplate(method);
            }

            this.WriteMethodParamsDef(this.Output, method.GetParameters(), this.HasMethodThis);

            // write local declarations
            if (methodBodyBytes != null)
            {
                if (this.writingMode == WritingMode.HeaderOnly)
                {
                    return;
                }

                this.Output.WriteLine(" {");

                this.Output.Indent++;

                this.WriteLocalVariableDeclarations(methodBodyBytes.LocalVariables);
            }
            else
            {
                //this.Output.WriteLine(method.IsAbstract ? " = 0;" : ";");
                this.Output.WriteLine(";");
            }
        }

        private static bool SkipMethod(MethodInfo method)
        {
            // TODO: hack to solve issue with double declaring
            foreach (var part in from exclude in excludeMethods where method.DeclaringType.FullName == exclude.Item1 select exclude.Item2.Split(';') into parts from part in parts select part)
            {
                if (part[part.Length - 1] == '*' && method.Name.StartsWith(part.Substring(0, part.Length - 1)))
                {
                    return true;
                }

                if (part == method.Name)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool SkipMethodBody(MethodInfo method)
        {
            // TODO: hack to solve issue with double declaring
            foreach (var part in from exclude in excludeMethodsBody where method.DeclaringType.FullName == exclude.Item1 select exclude.Item2.Split(';') into parts from part in parts select part)
            {
                if (part[part.Length - 1] == '*' && method.Name.StartsWith(part.Substring(0, part.Length - 1)))
                {
                    return true;
                }

                if (part == method.Name)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="moduleName">
        /// </param>
        public void WriteStart(string moduleName)
        {
            if (this.writingMode == WritingMode.CodeOnly)
            {
                if (this.headerFile != "mscorlib.h")
                {
                    this.Output.WriteLine("#include \"mscorlib.h\"");
                }
                else
                {
                }

                this.Output.Write("#include \"");
                this.Output.Write(this.headerFile);
                this.Output.WriteLine("\"");
                this.Output.WriteLine(string.Empty);

                var code = Resources.operations.Replace("System::Object", GetTypeNameWithId(typeof(object))).Replace("System::", string.Empty);
                this.Output.WriteLine(code);
                this.Output.WriteLine(string.Empty);

                return;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void WriteTypeEnd(Type type)
        {
            if (this.writingMode != WritingMode.CodeOnly)
            {
                this.Output.Indent--;
                this.Output.WriteLine("};");

                this.Output.WriteLine(string.Empty);
            }

            this.lastNamespace = null;

            if (this.writingMode == WritingMode.HeaderOnly)
            {
                return;
            }

            var index = 0;

            // after writing type you need to generate static members
            foreach (var field in this.fieldsInfo)
            {
                if (!field.IsStatic)
                {
                    continue;
                }

                this.WriteTypePrefix(this.Output, field.FieldType, false);
                this.Output.Write(' ');

                this.WriteTypeNoModifiers(this.Output, type, false, true);
                this.Output.Write("::");
                this.WriteFieldOrMethodName(this.Output, field.Name);
                this.WriteTypeSuffix(this.Output, field.FieldType);
                this.Output.WriteLine(';');

                index++;
            }

            if (this.StaticConstructors.Any())
            {
                var appendId = "";
                appendId = AppendId(this.StaticConstructors.First().DeclaringType, appendId);

                this.Output.Write("int ");
                WriteName(this.Output, type, true);

                this.Output.Write(string.Concat(this.StaticConstructors.First().MetadataToken, appendId));
                this.Output.Write("_static_ctor_initialized = ");
                this.Output.WriteLine(string.Concat("_cctor_", this.StaticConstructors.First().MetadataToken, appendId, "();"));
            }

            if (index > 0)
            {
                this.Output.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void WriteTypeStart(Type type, Type genericDefinition)
        {
            this.ReadTypeInfo(type, genericDefinition);

            if (this.writingMode == WritingMode.CodeOnly)
            {
                return;
            }

            if (type.IsGenericType)
            {
                this.WriteTemplateDefintion(type);
            }

            this.Output.Write("struct ");

            this.WriteName(this.Output, type, true, false, true);
            if (type.IsGenericType)
            {
                WriteTemplate(type);
            }

            var index = 0;

            var baseType = type.BaseType;
            if (baseType != null && baseType.FullName != "System.Enum")
            {
                if (index == 0)
                {
                    this.Output.Write(" :");
                }

                this.Output.Write(" public ");

                if (baseType.FullName == "System.Object")
                {
                    this.Output.Write("virtual ");
                }

                this.WriteTypeNoModifiers(this.Output, baseType, false, true);

                index++;
            }

            foreach (var @interface in type.GetInterfaces())
            {
                if (HasBaseTypeSameInterface(type, @interface, 0))
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
                this.WriteTypeNoModifiers(this.Output, @interface, false, true);

                index++;
            }

            if (index == 0 && type.IsInterface)
            {
                // add object reference
                this.Output.Write(": public virtual ");
                this.WriteTypeNoModifiers(this.Output, typeof(object), false, true);
            }

            this.Output.WriteLine(" {");

            // if (method.IsPublic)
            this.Output.WriteLine("public: ");

            this.Output.Indent++;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <param name="level">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool HasBaseTypeSameInterface(Type type, Type @interface, int level)
        {
            if (type == null)
            {
                return false;
            }

            foreach (var @interfaceOne in type.GetInterfaces())
            {
                if (level == 0 && ReferenceEquals(@interfaceOne, @interface))
                {
                    continue;
                }

                if (@interfaceOne == @interface)
                {
                    return true;
                }

                if (HasBaseTypeSameInterface(@interfaceOne, @interface, level + 1))
                {
                    return true;
                }
            }

            return HasBaseTypeSameInterface(type.BaseType, @interface, level + 1);
        }

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
            writer.Write("local");
            var index = GetLocalIndex(opCode);
            writer.Write(index);
        }

        private static int GetLocalIndex(OpCodePart opCode)
        {
            var code = opCode.ToCode();

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
            return index;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cleanName">
        /// </param>
        /// <returns>
        /// </returns>
        private static string TypeToCType(Type type, out bool converted, bool cleanName = false, bool doNotConvertName = false)
        {
            converted = false;

            var effectiveType = type;

            while (effectiveType.IsArray)
            {
                effectiveType = effectiveType.GetElementType();
            }

            if (!doNotConvertName && effectiveType.Namespace == "System")
            {
                string ctype;
                if (systemTypesToCTypes.TryGetValue(effectiveType.Name, out ctype))
                {
                    converted = true;
                    return ctype;
                }
            }

            return effectiveType.Name;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="parameterType">
        /// </param>
        /// <param name="any">
        /// </param>
        /// <param name="anyone">
        /// </param>
        private void WriteParameterSuffix(IndentedTextWriter writer, Type parameterType)
        {
            this.WriteName(writer, parameterType, true, true);
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
        private static void WriteTypeModifiers(IndentedTextWriter writer, Type type, bool structAsReference)
        {
            var isStructAsReference = structAsReference && type.IsStructureType();
            var isPointerType = type.IsPointer || (type.Namespace == "System" && (type.Name == "IntPtr" || type.Name == "UIntPtr"));

            if ((type.IsReference() || isStructAsReference || isPointerType || type.IsArray) && !type.IsGenericParameter)
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
        /// <param name="used">
        /// </param>
        /// <param name="parameterInfos">
        /// </param>
        /// <param name="isVirtual">
        /// </param>
        /// <param name="hasThis">
        /// </param>
        private void ActualWrite(IndentedTextWriter writer, OpCodePart thisMemberAccess, OpCodePart[] used, IEnumerable<ParameterInfo> parameterInfos, bool @isVirtual, bool hasThis)
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

                // TODO: temp hack for Int -> Enums
                if (parameter.ParameterType.IsEnum)
                {
                    writer.Write('(');
                    WriteTypeNoModifiers(writer, parameter.ParameterType);
                    writer.Write(')');
                }

                // detect if *(pointer) is required for example structure
                var parameterInputReturnResult = this.ResultOf(parameterInput);

                if (parameter.ParameterType.IsByRef && (parameterInputReturnResult == null || !parameterInputReturnResult.Type.IsByRef))
                {
                    //writer.Write('&');
                }

                if (parameterInputReturnResult != null && parameter.ParameterType.IsStructureType() && parameterInputReturnResult.IsPointerAccessRequired)
                {
                    writer.Write('*');
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

            if (block.UseAsNullCoalescingExpression)
            {
                writer.Write("nullCoalescing(");
                this.ActualWrite(writer, block.OpCodes[0].OpCodeOperands[0]);
                writer.Write(", ");
                this.ActualWrite(writer, block.OpCodes[block.OpCodes.Length - 1]);
                writer.Write(")");
            }

            if (block.UseAsLeadingIncDecExpression)
            {
                writer.Write(block.OpCodes[0].OpCodeOperands[0].Any(Code.Add) ? "++" : (block.OpCodes[1].OpCodeOperands[0].Any(Code.Sub) ? "--" : (string)null));
                this.ActualWrite(writer, block.OpCodes[0].OpCodeOperands[0].OpCodeOperands[0]);
            }

            if (block.UseAsIncDecExpression)
            {
                this.ActualWrite(writer, block.OpCodes[0].OpCodeOperands[0]);
                writer.Write(block.OpCodes[1].OpCodeOperands[0].Any(Code.Add) ? "++" : (block.OpCodes[1].OpCodeOperands[0].Any(Code.Sub) ? "--" : (string)null));
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

                    current.UseAsConditionalExpression = true;
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
                case Code.Nop:

                    if (opCode.ReadExceptionFromStack)
                    {
                        writer.Write("e");
                    }

                    break;

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
                    writer.Write(string.Concat("New", GetTypeId(typeof(string)) , GetTypeId(typeof(char*)), "(\""));
                    writer.Write(opCodeString.Operand.Replace(@"\", @"\\").Replace("\"", "\\\""));
                    writer.Write("\")");
                    break;
                case Code.Ldnull:
                    writer.Write("nullptr");
                    break;
                case Code.Ldtoken:
                    opCodeInt32 = opCode as OpCodeInt32Part;
                    // new RuntimeTypeHandle(new RuntimeType());
                    writer.Write("*New_33554749_33554734(New_33554734())");
                    break;
                case Code.Localloc:
                    writer.Write("new int[");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write("]");
                    break;
                case Code.Ldfld:
                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    var isThis = this.WriteMemberAccess(writer, opCodeFieldInfoPart);
                    this.WriteFieldName(writer, opCodeFieldInfoPart.Operand);
                    break;
                case Code.Ldflda:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    writer.Write("&(");
                    isThis = this.WriteMemberAccess(writer, opCodeFieldInfoPart);
                    this.WriteFieldName(writer, opCodeFieldInfoPart.Operand);
                    writer.Write(")");
                    break;
                case Code.Ldsfld:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.WriteFieldName(writer, opCodeFieldInfoPart.Operand, true);
                    break;
                case Code.Stfld:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    isThis = this.WriteMemberAccess(writer, opCodeFieldInfoPart);
                    this.WriteFieldName(writer, opCodeFieldInfoPart.Operand);
                    writer.Write(" = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Stsfld:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.WriteFieldName(writer, opCodeFieldInfoPart.Operand, true);
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
                    writer.Write("GetValue_100663705(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(", ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    writer.Write(")");
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
                    writer.Write("SetValue_100663712(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(", ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[2]);
                    writer.Write(", ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    writer.Write(")");
                    break;
                case Code.Ldind_I:
                    writer.Write("(int) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_I1:
                    writer.Write("(short) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_I2:
                    writer.Write("(int) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_I4:
                    writer.Write("(long) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_I8:
                    writer.Write("(long long) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_R4:
                    writer.Write("(float) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_R8:
                    writer.Write("(double) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_Ref:
                    writer.Write("(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_U1:
                    writer.Write("(unsigned short) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_U2:
                    writer.Write("(unsigned int) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ldind_U4:
                    writer.Write("(unsigned long) *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Stind_I:
                case Code.Stind_I1:
                case Code.Stind_I2:
                case Code.Stind_I4:
                case Code.Stind_I8:
                case Code.Stind_R4:
                case Code.Stind_R8:
                    writer.Write("*(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    writer.Write(" = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Stind_Ref:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Ldobj:
                    opCodeTypePart = opCode as OpCodeTypePart;
                    writer.Write("(");
                    this.WriteTypePrefix(writer, opCodeTypePart.Operand, true);
                    this.WriteTypeSuffix(writer, opCodeTypePart.Operand);
                    writer.Write(") *(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
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
                    writer.Write("(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(" & ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    writer.Write(")");
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
                    writer.Write("box(");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Unbox:
                case Code.Unbox_Any:
                    opCodeTypePart = opCode as OpCodeTypePart;
                    writer.Write("unbox<");
                    WriteTypeNoModifiers(writer, opCodeTypePart.Operand);
                    writer.Write(">(");
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.Write(")");
                    break;
                case Code.Ret:
                    writer.Write("return");
                    if (opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0)
                    {
                        writer.Write(' ');

                        var requiredType = this.RequiredType(opCode);

                        var oper1 = opCode.OpCodeOperands[0];
                        var oper1ReturnResult = this.ResultOf(oper1);

                        if (!requiredType.IsPointerAccessRequired && oper1ReturnResult != null && oper1ReturnResult.IsPointerAccessRequired)
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
                case Code.Ldarga:
                case Code.Ldarga_S:
                    asString = code.ToString();
                    var index = 0;
                    if (opCode.Any(Code.Ldarg_S, Code.Ldarg, Code.Ldarga, Code.Ldarga_S))
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
                        this.WriteParameterName(writer, index);
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
                        this.WriteParameterName(writer, index);
                    }

                    writer.Write(" = ");

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);

                    break;

                case Code.Ldftn:

                    writer.Write('&');
                    var opCodeMethodPart = opCode as OpCodeMethodInfoPart;
                    this.WriteMethodName(writer, opCodeMethodPart.Operand, this.writingMode == WritingMode.CodeOnly);

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

                    if (!opCode.UseAsConditionalExpression)
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

                    if (!opCode.UseAsConditionalExpression)
                    {
                        writer.Write(string.Concat(") goto L_" + opCode.JumpAddress()));
                        endings = Ending.SeparatorWithNewLine;
                    }

                    break;
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:

                    if (!opCode.UseAsConditionalExpression)
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

                    if (!opCode.UseAsConditionalExpression)
                    {
                        writer.Write(string.Concat(") goto L_", opCode.JumpAddress()));
                        endings = Ending.SeparatorWithNewLine;
                    }

                    break;
                case Code.Br:
                case Code.Br_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;

                    writer.Write(string.Concat("goto L_", opCode.JumpAddress()));
                    endings = Ending.SeparatorWithNewLine;

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

                    writer.Write("reinterpret_cast<");
                    this.WriteTypePrefix(writer, opCodeTypePart.Operand, true);
                    this.WriteTypeSuffix(writer, opCodeTypePart.Operand);
                    writer.Write(">(");
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.Write(")");

                    break;

                case Code.Isinst:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    writer.Write("dynamic_cast<");
                    this.WriteTypePrefix(writer, opCodeTypePart.Operand, true);
                    this.WriteTypeSuffix(writer, opCodeTypePart.Operand);
                    writer.Write(">(");
                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);
                    writer.Write(")");

                    break;

                case Code.Newobj:

                    var opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;

                    var declaringType = opCodeConstructorInfoPart.Operand.DeclaringType;

                    opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;

                    writer.Write(string.Concat("New", this.GetTypeId(declaringType)));
                    foreach (var paramCtor in opCodeConstructorInfoPart.Operand.GetParameters())
                    {
                        writer.Write(this.GetTypeId(paramCtor.ParameterType));
                    }

                    this.WriteCall(writer, opCode as OpCodeConstructorInfoPart, code == Code.Callvirt, false);

                    break;

                case Code.Newarr:

                    opCodeTypePart = opCode as OpCodeTypePart;

                    ////writer.Write("new ");
                    ////WriteTypePrefix(writer, opCodeTypePart.Operand, true, true);
                    ////writer.Write("[");
                    ////this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    ////writer.Write("]");

                    writer.Write("CreateInstance_100663687(");
                    // add type token here
                    writer.Write("GetTypeFromHandle_100667344(*New_33554749_33554734(New_33554734()))");
                    writer.Write(", ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");

                    break;

                case Code.Initobj:

                    opCodeTypePart = opCode as OpCodeTypePart;
                    if (opCodeTypePart.Operand.IsStructureType())
                    {
                        writer.Write("// ");
                    }

                    this.ActualWrite(writer, opCodeTypePart.OpCodeOperands[0]);

                    writer.Write(" = ");

                    writer.Write(string.Concat("New", this.GetTypeId(opCodeTypePart.Operand)));

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

                    opCodeInt32 = opCode as OpCodeInt32Part;

                    var jump = opCode.JumpOpCodeGroup(this);
                    if (jump != null && jump.JumpDestination != null && jump.JumpDestination.Contains(opCode))
                    {
                        writer.Write(string.Concat("goto L_", opCode.JumpAddress()));
                    }
                    else
                    {
                        endings = Ending.NoEndings;
                    }

                    break;

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

                case Code.Switch:

                    var opCodeLabels = opCode as OpCodeLabelsPart;

                    var varName = string.Concat("jump", opCode.AddressStart.ToString());

                    writer.Write("int ");
                    writer.Write(varName);
                    writer.Write(" = (int) (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    WriteEndings(Ending.SeparatorWithNewLine);

                    var indexLbl = 0;
                    foreach (var lbl in opCodeLabels.Operand)
                    {
                        writer.Write("if (");
                        writer.Write(varName);
                        writer.Write(" = " + indexLbl.ToString());
                        writer.Write(") goto L_" + opCodeLabels.JumpAddress(indexLbl));
                        WriteEndings(Ending.SeparatorWithNewLine);

                        indexLbl++;
                    }

                    endings = Ending.NoEndings;
                    break;
            }

            return endings;
        }

        private void WriteParameterName(IndentedTextWriter writer, int index)
        {
            this.WriteFieldOrMethodName(writer, this.ParameterInfo[index - (this.HasMethodThis ? 1 : 0)].Name);
            writer.Write("_");
        }

        /// <summary>
        /// </summary>
        /// <param name="genType">
        /// </param>
        /// <returns>
        /// </returns>
        private string ConvertTypesToCTypesInGenericArguments(string genType)
        {
            var sb = new StringBuilder();

            var parts = genType.Split(',');

            var count = 0;
            foreach (var part in parts)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                var typeName = part.Replace("::", ".");
                var type = ParsedType.GetTypeFromTypeName(typeName, this);
                if (type != null)
                {
                    var typeAsString = this.TypeToString(type);
                    sb.Append(typeAsString);
                }
                else
                {
                    sb.Append(part);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private IEnumerable<Type> GetAllInterfaces(Type type)
        {
            foreach (var one in type.GetInterfaces())
            {
                yield return one;
                foreach (var subOne in this.GetAllInterfaces(one))
                {
                    yield return subOne;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private int GetTemplateLevel(Type type)
        {
            var level = 0;
            if (type == null)
            {
                return level;
            }

            var index = type.Name.IndexOf('`');
            if (index == -1)
            {
                ////if (!this.nestTypes)
                ////{
                ////    if (!string.IsNullOrWhiteSpace(type.FullName))
                ////    {
                ////        index = type.FullName.LastIndexOf('`');
                ////        if (index != -1)
                ////        {
                ////            // find end of the index
                ////            var start = ++index;
                ////            while (index < type.FullName.Length && Char.IsDigit(type.FullName[index]))
                ////            {
                ////                index++;
                ////            }

                ////            level = int.Parse(type.FullName.Substring(start, index - start));
                ////        }
                ////    }
                ////    else
                ////    {
                ////        return GetTemplateLevel(type.DeclaringType);
                ////    }
                ////}
            }
            else
            {
                // find end of the index
                var start = ++index;
                while (index < type.Name.Length && char.IsDigit(type.Name[index]))
                {
                    index++;
                }

                level = int.Parse(type.Name.Substring(start, index - start));
            }

            return level;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private int GetTemplateLevelFromDeclaringTypes(Type type)
        {
            if (type == null)
            {
                return 0;
            }

            return this.GetTemplateLevel(type) + this.GetTemplateLevelFromDeclaringTypes(type.DeclaringType);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        private void SaveLocal(IndentedTextWriter writer, OpCodePart opCode)
        {
            var localIndex = GetLocalIndex(opCode);

            if (!LocalInfoUsed[localIndex])
            {
                this.WriteTypePrefix(this.Output, LocalInfo[localIndex].LocalType, false);
                writer.Write(" ");
                LocalInfoUsed[localIndex] = true;
            }

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
            this.ActualWrite(writer, opCodeConstructorInfo, opCodeConstructorInfo.OpCodeOperands, ctor.GetParameters(), @isVirtual, hasThis);
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

            if (isVirtual || hasThis)
            {
                this.WriteMemberAccess(writer, opCodeMethodInfo);
            }

            var startsWithThis = hasThis && opCodeMethodInfo.OpCodeOperands != null && opCodeMethodInfo.OpCodeOperands.Length > 0
                                 && opCodeMethodInfo.OpCodeOperands[0].Any(Code.Ldarg_0);

            this.WriteMethodName(writer, methodBase, this.writingMode == WritingMode.CodeOnly);

            this.ActualWrite(writer, opCodeMethodInfo, opCodeMethodInfo.OpCodeOperands, methodBase.GetParameters(), isVirtual, hasThis);
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

            if (opCode.JumpDestination != null && opCode.JumpDestination.Count > 0)
            {
                writer.Indent--;
                writer.WriteLine(string.Concat("L_", opCode.GroupAddressStart, ":;"));
                writer.Indent++;
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
                        this.WriteTypePrefix(writer, exceptionHandler.CatchType, true);
                        this.WriteTypeSuffix(writer, exceptionHandler.CatchType);
                        writer.Write(" e)");
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
        /// <param name="ctor">
        /// </param>
        private void WriteFactoryMethodNew(ConstructorInfo ctor)
        {
            if (ctor.IsStatic)
            {
                return;
            }

            //////////////////////////////////////////
            // write static new
            if (this.writingMode == WritingMode.CodeOnly)
            {
                if (ctor.DeclaringType.IsGenericType)
                {
                    this.WriteTemplateDefintion(ctor.DeclaringType);
                }
            }

            this.WriteTypePrefix(this.Output, ctor.DeclaringType, true);
            this.WriteTypeSuffix(this.Output, ctor.DeclaringType);

            this.Output.Write(" ");

            this.Output.Write("New");
            this.Output.Write(this.GetTypeId(ctor.DeclaringType));

            foreach (var paramCtor in ctor.GetParameters())
            {
                this.Output.Write(this.GetTypeId(paramCtor.ParameterType));
            }

            // add parameters suffixes
            this.WriteParametersSuffix(this.Output, ctor);
            this.WriteMethodParamsDef(this.Output, ctor.GetParameters(), false);

            if (this.writingMode == WritingMode.CodeOnly)
            {
                this.Output.WriteLine(" {");
                this.Output.Indent++;

                this.Output.Write("auto __this = new ");
                this.WriteTypeNoModifiers(this.Output, ctor.DeclaringType, false, true);
                this.Output.WriteLine("();");
                this.Output.Write("__this->");
                this.WriteMethodName(this.Output, ctor, this.writingMode == WritingMode.CodeOnly);
                this.WriteParamsToCall(this.Output, ctor.GetParameters());
                this.Output.WriteLine(";");
                this.Output.WriteLine("return __this;");

                this.Output.Indent--;
                this.Output.WriteLine("}");
                this.Output.WriteLine(string.Empty);
            }
            else
            {
                this.Output.WriteLine(";");
            }
        }
  
        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="fieldOrMethodName">
        /// </param>
        /// <param name="methodDeclaration">
        /// </param>
        /// <param name="isGenericMethod">
        /// </param>
        private void WriteFieldOrMethodName(IndentedTextWriter writer, string fieldOrMethodName, bool appendTypeNamespace = false, Type declaringType = null)
        {
            if (appendTypeNamespace)
            {
                this.WriteTypeNoModifiers(this.Output, declaringType, false, true);
                writer.Write("::");
            }

            fieldOrMethodName = fieldOrMethodName.Replace('.', '_').Replace('<', '_').Replace('>', '_').Replace(',', '_');
            var finalResult = fieldOrMethodName.Replace('$', '_').Replace('-', '_');
            writer.Write(finalResult);
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
        /// <param name="isGenericDefinition">
        /// </param>
        private void WriteGenericParameters(IndentedTextWriter writer, Type type, bool isGenericDefinition)
        {
            var level = this.GetTemplateLevelFromDeclaringTypes(type);

            for (int i = 0; i < level; i++)
            {
                if (i > 0)
                {
                    writer.Write(", ");
                }

                Type generic = null;

                if (this.GenericMethodArguments != null)
                {
                    generic = this.GenericMethodArguments.Where(a => a.GenericParameterPosition == i).FirstOrDefault();
                }

                if (generic == null && this.TypeGenericArguments != null)
                {
                    generic = this.TypeGenericArguments.Where(a => a.IsGenericParameter && a.GenericParameterPosition == i).FirstOrDefault();
                }

                if (isGenericDefinition)
                {
                    writer.Write("typename ");
                }

                if (generic != null)
                {
                    writer.Write(generic.Name);
                }
                else if (this.TypeGenericArguments != null)
                {
                    this.WriteTypePrefix(writer, this.TypeGenericArguments[i], false);
                    this.WriteTypeSuffix(writer, this.TypeGenericArguments[i]);
                }
                else if (type.GenericTypeArguments != null)
                {
                    this.WriteTypePrefix(writer, type.GenericTypeArguments[i], false);
                    this.WriteTypeSuffix(writer, type.GenericTypeArguments[i]);
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
                if (local.LocalType.IsByRef)
                {
                    continue;
                }

                this.WriteLocalVariableDeclaration(local);
                this.LocalInfoUsed[local.LocalIndex] = true;
            }
        }

        private void WriteLocalVariableDeclaration(LocalVariableInfo local)
        {
            this.WriteTypePrefix(this.Output, local.LocalType, false);
            this.Output.Write(" local{0}", local.LocalIndex);
            this.WriteTypeSuffix(this.Output, local.LocalType);
            this.Output.WriteLine(';');
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
            var oper1ReturnResult = this.ResultOf(oper1);

            var wrapInPrimitiveClass = oper1ReturnResult != null ? oper1ReturnResult.Type.IsPrimitiveType() : false;

            if (wrapInPrimitiveClass)
            {
                WriteTypeNoModifiers(writer, oper1ReturnResult.Type, false, true);
                writer.Write("(");
            }

            this.ActualWrite(writer, oper1);

            if (wrapInPrimitiveClass)
            {
                writer.Write(")");
            }

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
        /// <param name="useGenericArguments">
        /// </param>
        /// <param name="isGenericMethod">
        /// </param>
        private void WriteMethodName(
            IndentedTextWriter writer,
            MemberInfo memberInfo,
            bool appendTypeNamespace)
        {
            var appendId = string.Empty;
            appendId = AppendId(memberInfo.DeclaringType, appendId);
            this.WriteFieldOrMethodName(writer, string.Concat(memberInfo.Name, "_", memberInfo.MetadataToken, appendId), appendTypeNamespace, memberInfo.DeclaringType);
        }

        private void WriteFieldName(
            IndentedTextWriter writer,
            MemberInfo memberInfo,
            bool isStatic = false)
        {
            this.WriteFieldOrMethodName(writer, memberInfo.Name, isStatic, memberInfo.DeclaringType);
        }

        private string GetTypeNameWithId(Type type)
        {
            var appendId = "";
            appendId = AppendId(type, appendId);

            return string.Concat(type.Name, "_", type.MetadataToken, appendId);
        }

        private string GetTypeId(Type type)
        {
            var appendId = "";
            appendId = AppendId(type, appendId);

            return string.Concat("_", type.MetadataToken.ToString(), appendId);
        }

        private static string AppendId(Type type, string appendId)
        {
            // generic append
            if (type.IsGenericType)
            {
                foreach (var genType in type.GenericTypeArguments)
                {
                    appendId += "_" + genType.MetadataToken;

                    if (genType.IsGenericType)
                    {
                        appendId = AppendId(genType, appendId);
                    }
                }
            }

            return appendId;
        }

        /// <summary>
        /// </summary>
        /// <param name="endPart">
        /// </param>
        private void WriteMethodBody(string endPart = null)
        {
            if (this.writingMode == WritingMode.HeaderOnly)
            {
                return;
            }

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
        /// <param name="useGenericArguments">
        /// </param>
        private void WriteMethodName(
            IndentedTextWriter writer, MethodBase methodBase, bool appendTypeNamespace)
        {
            this.WriteMethodName(writer, (MemberInfo)methodBase, appendTypeNamespace);

            // add parameters suffixes
            this.WriteParametersSuffix(writer, methodBase);
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
                this.WriteFieldOrMethodName(writer, parameter.Name);
                writer.Write("_");
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
        /// <param name="cleanName">
        /// </param>
        /// <param name="methodPrefixName">
        /// </param>
        /// <param name="doNotConvertName">
        /// </param>
        private void WriteName(IndentedTextWriter writer, Type type, bool cleanName = false, bool methodPrefixName = false, bool doNotConvertName = false)
        {
            if (type.IsGenericParameter)
            {
                writer.Write(type.Name);
                return;
            }

            bool converted;
            var typeBaseName = TypeToCType(type, out converted, cleanName, doNotConvertName);

            if (type.IsPrimitiveType())
            {
                converted = true;
            }

            WriteName(writer, typeBaseName, !converted ? type.MetadataToken.ToString() : string.Empty, cleanName);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="typeBaseName">
        /// </param>
        /// <param name="cleanName">
        /// </param>
        private void WriteName(IndentedTextWriter writer, string typeBaseName, string suffix, bool cleanName = false)
        {
            // clean name
            if (typeBaseName.EndsWith("&"))
            {
                typeBaseName = typeBaseName.Substring(0, typeBaseName.Length - 1) + (cleanName ? "Ref" : string.Empty);
            }

            if (typeBaseName.EndsWith("*"))
            {
                typeBaseName = typeBaseName.Substring(0, typeBaseName.Length - 1) + (cleanName ? "Ptr" : string.Empty);
            }

            var index = typeBaseName.IndexOf('`');
            if (index >= 0)
            {
                typeBaseName = typeBaseName.Replace("`", "_");
            }

            index = typeBaseName.IndexOf('=');
            if (index >= 0)
            {
                var size_s = "_size" + typeBaseName.Substring(index + 1);
                typeBaseName = typeBaseName.Substring(0, index) + size_s;
            }

            index = 0;
            while ((index = typeBaseName.IndexOfAny(cleanName ? this.specCharsClean : this.specChars, index)) >= 0)
            {
                typeBaseName = typeBaseName.Remove(index, 1);
                typeBaseName = typeBaseName.Insert(index++, "_");
            }

            writer.Write(typeBaseName);
            if (!string.IsNullOrWhiteSpace(suffix))
            {
                writer.Write('_');
                writer.Write(suffix);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        private void WriteNamespaceEnd(IndentedTextWriter writer, Type type)
        {
            if (string.IsNullOrWhiteSpace(type.Namespace))
            {
                return;
            }

            WriteNamespaceEnd(writer, type.Namespace);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="typeNamespace">
        /// </param>
        private void WriteNamespaceEnd(IndentedTextWriter writer, string typeNamespace)
        {
            if (string.IsNullOrWhiteSpace(typeNamespace) || this.namespaceClosed)
            {
                return;
            }

            foreach (var part in typeNamespace.Split('.'))
            {
                writer.Indent--;
                writer.WriteLine("}");
            }

            this.namespaceClosed = true;
            this.lastNamespace = null;

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="openIfDifferent">
        /// </param>
        /// <param name="isFirst">
        /// </param>
        private void WriteNamespaceStart(IndentedTextWriter writer, Type type, bool openIfDifferent, bool isFirst)
        {
            if (this.lastNamespace == type.Namespace && openIfDifferent)
            {
                return;
            }

            if (openIfDifferent)
            {
                this.WriteNamespaceEnd(writer, this.lastNamespace);
            }

            if (string.IsNullOrWhiteSpace(type.Namespace))
            {
                this.lastNamespace = string.Empty;
                return;
            }

            foreach (var part in type.Namespace.Split('.'))
            {
                writer.Write("namespace ");
                writer.Write(part);
                writer.WriteLine(" {");
                writer.Indent++;
            }

            this.lastNamespace = type.Namespace;
            this.namespaceClosed = false;
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        private void WriteParametersSuffix(IndentedTextWriter writer, MethodBase methodBase)
        {
            ////var methodInfo = methodBase as MethodInfo;
            ////if (methodInfo != null)
            ////{
            ////    var type = methodInfo.ReturnType;
            ////    if (!type.IsVoid())
            ////    {
            ////        writer.Write('_');

            ////        this.WriteName(writer, type, true, true);

            ////        if (methodBase.GetParameters().Length > 0)
            ////        {
            ////            writer.Write('_');
            ////        }
            ////    }
            ////}

            ////WriteParametersSuffix(writer, methodBase.GetParameters());
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="parameters">
        /// </param>
        private void WriteParametersSuffix(IndentedTextWriter writer, IEnumerable<ParameterInfo> parameters)
        {
            foreach (var parameter in parameters)
            {
                writer.Write('_');
                WriteParameterSuffix(writer, parameter.ParameterType);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="parameterInfos">
        /// </param>
        private void WriteParamsToCall(IndentedTextWriter writer, IEnumerable<ParameterInfo> parameterInfos)
        {
            writer.Write("(");

            var index = 0;
            foreach (var parameter in parameterInfos)
            {
                if (index > 0)
                {
                    writer.Write(", ");
                }

                writer.Write(parameter.Name);
                writer.Write('_');

                index++;
            }

            writer.Write(")");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteTemplate(Type type)
        {
            if (!type.IsGenericTypeDefinition && type.IsGenericType)
            {
                this.Output.Write("<");
                this.WriteGenericParameters(this.Output, type, false);
                this.Output.Write(">");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        private void WriteTemplate(MethodInfo method)
        {
            if (!method.IsGenericMethodDefinition && method.IsGenericMethod)
            {
                this.Output.Write("<");
                this.WriteGenericParameters(this.Output, method);
                this.Output.Write(">");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        private void WriteTemplateDefinition(MethodInfo method)
        {
            this.Output.Write("template <");
            if (method.IsGenericMethodDefinition)
            {
                this.WriteGenericParameters(this.Output, method);
            }

            this.Output.Write("> ");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        private void WriteTemplateDefintion(Type type)
        {
            var level = this.GetTemplateLevelFromDeclaringTypes(type);
            if (level > 0)
            {
                this.Output.Write("template <");
                if (type.IsGenericTypeDefinition)
                {
                    this.WriteGenericParameters(this.Output, type, true);
                }

                this.Output.Write("> ");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="useGenericArguments">
        /// </param>
        private void WriteTemplateParameters(IndentedTextWriter writer, Type type, bool useGenericArguments)
        {
            var level = this.GetTemplateLevelFromDeclaringTypes(type);

            // write generic parameters
            if (type.IsGenericType && level > 0)
            {
                writer.Write('<');

                int count = 0;
                var args = useGenericArguments ? type.GetGenericArguments() : type.GenericTypeArguments;
                if (args == null || !args.Any())
                {
                    var genArgs = type.GetGenericArguments();
                    if (genArgs != null && genArgs.Any())
                    {
                        args = type.GetGenericArguments();
                    }
                }

                foreach (var genericParam in args)
                {
                    if (count > 0)
                    {
                        writer.Write(", ");
                    }

                    this.WriteTypePrefix(writer, genericParam, false);
                    this.WriteTypeSuffix(writer, genericParam);

                    count++;
                }

                writer.Write('>');
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
        private void WriteTypePrefix(IndentedTextWriter writer, Type type, bool structAsReference = true, bool doNotConvertName = false)
        {
            var effectiveType = this.WriteTypeNoModifiers(writer, type, false, doNotConvertName);
            WriteTypeModifiers(writer, type, structAsReference);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="isZeroArraySize">
        /// </param>
        private void WriteTypeSuffix(IndentedTextWriter writer, Type type)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="useGenericArguments">
        /// </param>
        /// <param name="doNotConvertName">
        /// </param>
        /// <returns>
        /// </returns>
        private Type WriteTypeNoModifiers(IndentedTextWriter writer, Type type, bool useGenericArguments = false, bool doNotConvertName = false)
        {
            var effectiveType = type;

            while (effectiveType.HasElementType)
            {
                effectiveType = effectiveType.GetElementType();
            }

            // write base name

            // if (effectiveType.IsGenericType && effectiveType.IsNested && effectiveType.DeclaringType.IsGenericType)
            if (effectiveType.IsGenericType && effectiveType.DeclaringType != null && effectiveType.DeclaringType.IsGenericType
                && effectiveType.GenericTypeArguments.Any(g => g.IsGenericParameter))
            {
                writer.Write("typename ");
            }

            WriteTypeNoModifiersInternal(writer, type, useGenericArguments, doNotConvertName);

            return effectiveType;
        }

        private void WriteTypeNoModifiersInternal(IndentedTextWriter writer, Type type, bool useGenericArguments, bool doNotConvertName)
        {
            if (type.IsArray)
            {
                this.WriteName(writer, typeof(Array), false, false, doNotConvertName);
                this.WriteTemplateParameters(writer, typeof(Array), useGenericArguments);
            }
            else if (type.HasElementType)
            {
                WriteTypePrefix(writer, type.GetElementType(), useGenericArguments, doNotConvertName);
                WriteTypeSuffix(writer, type.GetElementType());
            }
            else
            {
                this.WriteName(writer, type, false, false, doNotConvertName);
                this.WriteTemplateParameters(writer, type, useGenericArguments);
            }
        }

        #endregion

        /// <summary>
        /// </summary>
        [DebuggerDisplay("{Name}")]
        public class ParsedType
        {
            #region Constructors and Destructors

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            public ParsedType(string type)
            {
                if (type != null)
                {
                    this.Parse(type, 0);
                }
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// </summary>
            public Type NestedResolvedType
            {
                get
                {
                    if (this.Next != null)
                    {
                        return this.Next.NestedResolvedType;
                    }

                    return this.ResolvedType;
                }
            }

            #endregion

            #region Properties

            /// <summary>
            /// </summary>
            private List<ParsedType> GenericArguments { get; set; }

            /// <summary>
            /// </summary>
            private string Name { get; set; }

            /// <summary>
            /// </summary>
            private ParsedType Next { get; set; }

            /// <summary>
            /// </summary>
            private Type ResolvedType { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            /// <param name="typeName">
            /// </param>
            /// <param name="writer">
            /// </param>
            /// <returns>
            /// </returns>
            public static Type GetTypeFromTypeName(string typeName, CAsmWriter writer)
            {
                if (string.IsNullOrWhiteSpace(typeName))
                {
                    return null;
                }

                ObjectHandle handle = null;
                try
                {
                    handle = Activator.CreateInstance(writer.ThisType.Module.Assembly.FullName, typeName);
                }
                catch (Exception)
                {
                    try
                    {
                        handle = Activator.CreateInstance(typeof(string).Assembly.FullName, typeName);
                    }
                    catch (Exception)
                    {
                        return Type.GetType(typeName);
                    }
                }

                if (handle != null)
                {
                    var p = handle.Unwrap();
                    return p.GetType();
                }

                return null;
            }

            /// <summary>
            /// </summary>
            /// <param name="typeName">
            /// </param>
            /// <param name="writer">
            /// </param>
            /// <returns>
            /// </returns>
            public string AsCString(string typeName, CAsmWriter writer)
            {
                var type = GetTypeFromTypeName(typeName, writer);
                if (type == null || type.IsGenericType)
                {
                    return null;
                }

                if (type != null)
                {
                    return writer.TypeToString(type);
                }

                return null;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="writer">
            /// </param>
            /// <returns>
            /// </returns>
            public string AsCString(Type type, CAsmWriter writer)
            {
                if (type != null)
                {
                    return writer.TypeToString(type);
                }

                return null;
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IEnumerable<ParsedType> Following()
            {
                yield return this;
                if (this.Next == null)
                {
                    yield break;
                }

                foreach (var next in this.Next.Following())
                {
                    yield return next;
                }
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="start">
            /// </param>
            /// <returns>
            /// </returns>
            public int Parse(string type, int start)
            {
                this.Name = string.Empty;
                var i = start;

                for (; i < type.Length; i++)
                {
                    var ch = type[i];
                    if (char.IsLetterOrDigit(ch) || ch == '_')
                    {
                        this.Name += ch;
                    }

                    if (ch == '.')
                    {
                        this.Next = new ParsedType(null);
                        i = this.Next.Parse(type, ++i);
                    }

                    if (ch == '<')
                    {
                        this.GenericArguments = new List<ParsedType>();
                        i = this.ParseGenerics(type, ++i, this.GenericArguments);
                    }

                    if (ch == ',' || ch == '>')
                    {
                        i--;
                        break;
                    }
                }

                return i;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="start">
            /// </param>
            /// <param name="genericArguments">
            /// </param>
            /// <returns>
            /// </returns>
            public int ParseGenerics(string type, int start, List<ParsedType> genericArguments)
            {
                var i = start;
                for (; i < type.Length; i++)
                {
                    var ch = type[i];
                    if (ch == ',')
                    {
                        continue;
                    }

                    if (ch == '>')
                    {
                        break;
                    }

                    var gen = new ParsedType(null);
                    i = gen.Parse(type, i);
                    genericArguments.Add(gen);
                }

                return i;
            }

            /// <summary>
            /// </summary>
            /// <param name="typeGenericArguments">
            /// </param>
            public void ResolveGenerics(Type[] typeGenericArguments)
            {
                foreach (var next in this.Following())
                {
                    if (next.GenericArguments != null)
                    {
                        for (var i = 0; i < typeGenericArguments.Length && i < next.GenericArguments.Count; i++)
                        {
                            next.GenericArguments[i].ResolvedType = typeGenericArguments[i];
                        }
                    }
                }
            }

            /// <summary>
            /// </summary>
            /// <param name="types">
            /// </param>
            public void ResolveTypes(Type[] types)
            {
                foreach (var next in this.Following())
                {
                    var effectiveName = next.Name;
                    if (next.GenericArguments != null)
                    {
                        effectiveName += "`" + next.GenericArguments.Count.ToString();
                    }

                    next.ResolvedType = types.FirstOrDefault(i => i.Name == effectiveName);
                    if (next.ResolvedType != null && next.ResolvedType.IsGenericType && next.ResolvedType.GenericTypeArguments != null
                        && next.ResolvedType.GenericTypeArguments.Length > 0)
                    {
                        for (var i = 0; i < next.ResolvedType.GenericTypeArguments.Length; i++)
                        {
                            next.GenericArguments[i].ResolveTypes(new[] { next.ResolvedType.GenericTypeArguments[i] });
                            if (next.GenericArguments[i].ResolvedType == null && next.GenericArguments[i].Next == null)
                            {
                                next.GenericArguments[i].ResolvedType = next.ResolvedType.GenericTypeArguments[i];
                            }
                        }
                    }
                }
            }

            /// <summary>
            /// </summary>
            /// <param name="writer">
            /// </param>
            /// <param name="clean">
            /// </param>
            /// <param name="skip">
            /// </param>
            /// <returns>
            /// </returns>
            public string ToCString(CAsmWriter writer, bool clean = true, int skip = 0)
            {
                var sb = new StringBuilder();
                var count = 0;

                var following = this.Following().ToList();

                if (skip < 0)
                {
                    skip = following.Count + skip;
                }

                foreach (var next in following)
                {
                    if (skip > count++ && skip > 0)
                    {
                        continue;
                    }

                    if (count > 1)
                    {
                        if (count == 2 && sb.Length == 0 && skip == 0)
                        {
                            sb.Append("_");
                        }
                        else if (sb.Length > 0)
                        {
                            sb.Append("_"); //::
                        }
                    }

                    if (next.ResolvedType != null)
                    {
                        WriteName(sb, next.ResolvedType);
                    }
                    else
                    {
                        sb.Append(next.Name ?? string.Empty);
                    }

                    if (next.GenericArguments != null)
                    {
                        sb.Append("_T");
                        sb.Append(next.GenericArguments.Count);
                        sb.Append('<');

                        var count2 = 0;
                        foreach (var gen in next.GenericArguments)
                        {
                            if (count2++ > 0)
                            {
                                sb.Append(", ");
                            }

                            string cgenTypeName = null;
                            if (gen.ResolvedType != null)
                            {
                                cgenTypeName = AsCString(gen.ResolvedType, writer);
                            }
                            else
                            {
                                var genTypeName = gen.ToString(true);
                                cgenTypeName = AsCString(genTypeName, writer);
                            }

                            if (cgenTypeName != null)
                            {
                                sb.Append(cgenTypeName);
                            }
                            else
                            {
                                sb.Append(gen.ToCString(writer));
                                if (gen.NestedResolvedType != null && gen.NestedResolvedType.IsClass)
                                {
                                    sb.Append('*');
                                }
                            }
                        }

                        sb.Append('>');
                    }
                }

                var ret = sb.ToString();
                return clean ? ret.Replace('$', '_').Replace('-', '_') : ret;
            }

            /// <summary>
            /// </summary>
            /// <param name="genericLevel">
            /// </param>
            /// <returns>
            /// </returns>
            public string ToString(bool genericLevel = false)
            {
                var sb = new StringBuilder();
                var count = 0;
                foreach (var next in this.Following())
                {
                    if (count++ > 0)
                    {
                        sb.Append('.');
                    }

                    sb.Append(next.Name ?? string.Empty);

                    if (next.GenericArguments != null)
                    {
                        if (genericLevel)
                        {
                            sb.Append('`');
                            sb.Append(next.GenericArguments.Count);
                        }
                        else
                        {
                            sb.Append('<');

                            var count2 = 0;
                            foreach (var gen in next.GenericArguments)
                            {
                                if (count2++ > 0)
                                {
                                    sb.Append(", ");
                                }

                                sb.Append(gen.ToString());
                            }

                            sb.Append('>');
                        }
                    }
                }

                return sb.ToString();
            }

            #endregion

            #region Methods

            /// <summary>
            /// </summary>
            /// <param name="sb">
            /// </param>
            /// <param name="type">
            /// </param>
            private static void WriteName(StringBuilder sb, Type type)
            {
                if (type.IsNested)
                {
                    WriteName(sb, type.DeclaringType);
                    sb.Append('_');
                }

                if (type.Name == null)
                {
                    return;
                }

                var ind = type.Name.IndexOf('`');
                sb.Append(ind != -1 ? type.Name.Substring(0, ind) : type.Name);
            }

            #endregion
        }
    }
}
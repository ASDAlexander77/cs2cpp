namespace Il2Native.Logic
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Text;

    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using Il2Native.Logic.CodeParts;

    public class GoWriter : BaseWriter, ICodeWriter
    {
        private static IDictionary<string, string> systemTypesToGoTypes = new SortedDictionary<string, string>();

        public GoWriter(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var outputFile = extension != null && extension.Equals(string.Empty) ? fileName + ".go" : fileName;
            this.Output = new IndentedTextWriter(new StreamWriter(outputFile));
        }

        static GoWriter()
        {
            systemTypesToGoTypes["SByte"] = "int8";
            systemTypesToGoTypes["Int32"] = "int";
            systemTypesToGoTypes["Float"] = "float32";
            systemTypesToGoTypes["Double"] = "float64";
            systemTypesToGoTypes["Boolean"] = "bool";
            systemTypesToGoTypes["String"] = "string";
        }

        protected IndentedTextWriter Output { get; private set; }

        public void WriteStart(string moduleName)
        {
            this.Output.WriteLine("package main");
            this.Output.WriteLine();

            this.Output.WriteLine("import \"fmt\"");
            this.Output.WriteLine();
        }

        public void WriteTypeStart(Type type)
        {
            this.Output.Write("type ");

            if (!string.IsNullOrWhiteSpace(type.Namespace))
            {
                this.Output.Write(type.Namespace);
                this.Output.Write('_');
            }

            this.Output.Write(type.Name);

            this.Output.WriteLine(" struct {");

            this.Output.Indent++;
        }

        public void WriteBeforeFields()
        {
        }

        public void WriteFieldStart(FieldInfo field)
        {
            this.Output.Write(field.Name);
            this.Output.Write(' ');
            this.ActualWrite(this.Output, field.FieldType);
            this.Output.WriteLine(string.Empty);
        }

        public void WriteFieldEnd(FieldInfo field)
        {
        }

        public void WriteAfterFields()
        {
            this.Output.Indent--;
            this.Output.WriteLine("}");
            this.Output.WriteLine(string.Empty);
        }

        public void WriteBeforeConstructors()
        {
        }

        public void WriteConstructorStart(ConstructorInfo ctor)
        {
            this.Output.Write("func ");

            if (!ctor.IsStatic)
            {
                var declType = ctor.DeclaringType;
                this.Output.Write("(this");
                this.Output.Write(" *");
                this.Output.Write(TypeToGoType(declType));
                this.Output.Write(") ");
            }

            this.WriteMethodName(this.Output, ctor);

            ReadMethodInfo(ctor);

            this.WriteMethodParamsDef(this.Output, ctor.GetParameters(), this.HasMethodThis);

            this.Output.Write(" ");
            this.ActualWrite(this.Output, ctor.DeclaringType);
            this.Output.Write(" ");
            this.Output.WriteLine("{");

            this.Output.Indent++;

            // write local declarations
            foreach (var local in ctor.GetMethodBody().LocalVariables)
            {
                this.Output.Write("var local{0} ", local.LocalIndex);
                this.ActualWrite(this.Output, local.LocalType);
                this.Output.WriteLine(string.Empty);
            }
        }

        public void WriteConstructorEnd(ConstructorInfo ctor)
        {
            this.WriteMethodBody(" this");

            this.Output.Indent--;

            this.Output.WriteLine("}");
            this.Output.WriteLine(string.Empty);
        }

        public void WriteAfterConstructors()
        {
        }

        public void WriteBeforeMethods()
        {
        }

        public void WriteMethodStart(MethodInfo method)
        {
            StartProcess();

            // check if main
            if (method.IsStatic && method.Name.Equals("Main"))
            {
                this.MainMethod = method;
            }

            this.Output.Write("func ");

            if (!method.IsStatic)
            {
                var declType = method.DeclaringType;
                this.Output.Write("(this");
                this.Output.Write(" *");
                this.Output.Write(TypeToGoType(declType));
                this.Output.Write(") ");
            }

            this.WriteMethodName(this.Output, method);

            ReadMethodInfo(method);

            this.WriteMethodParamsDef(this.Output, method.GetParameters(), this.HasMethodThis);

            this.Output.Write(" ");

            if (method.ReturnType.Name != "Void")
            {
                this.ActualWrite(this.Output, method.ReturnType);
                this.Output.Write(" ");
            }

            this.Output.WriteLine("{");

            this.Output.Indent++;

            // write local declarations
            foreach (var local in method.GetMethodBody().LocalVariables)
            {
                this.Output.Write("var local{0} ", local.LocalIndex);
                this.ActualWrite(this.Output, local.LocalType);
                this.Output.WriteLine(string.Empty);
            }
        }

        public void Write(string rawText)
        {
            this.Output.Write(rawText);
        }

        public void Write(OpCodePart opCode)
        {
            this.Output.WriteLine("// {0}", opCode.OpCode.Name);
            Process(opCode);
        }

        public void WriteMethodEnd(MethodInfo method)
        {
            this.WriteMethodBody();

            this.Output.Indent--;

            this.Output.WriteLine("}");
            this.Output.WriteLine(string.Empty);
        }

        private void WriteMethodBody(string endPart = null)
        {
            var rest = PrepareWritingMethodBody();

            var i = 0;
            foreach (var opCodePart in rest)
            {
                i++;

                // we need to remove unsed ldarg.* etc
                if (opCodePart.OpCode.StackBehaviourPush != StackBehaviour.Push0
                    && opCodePart.OpCodeOperands == null)
                {
                    continue;
                }

                if (opCodePart.Skip)
                {
                    continue;
                }

                this.ActualWrite(this.Output, opCodePart);

                if (endPart != null && i == rest.Length)
                {
                    this.Output.Write(endPart);
                }

                this.Output.WriteLine(string.Empty);
            }
        }

        public void WriteAfterMethods()
        {
        }

        public void WriteTypeEnd(Type type)
        {
        }

        public void WriteEnd()
        {
            if (this.MainMethod != null)
            {
                this.Output.WriteLine("func main() {");
                this.Output.Indent++;

                this.WriteMethodName(this.Output, this.MainMethod);

                this.Output.Write("(");

                var index = 0;
                foreach (var parameter in this.MainMethod.GetParameters())
                {
                    if (index > 0)
                    {
                        this.Output.Write(", ");
                    }

                    this.Output.Write("nil");

                    index++;
                }

                this.Output.WriteLine(")");                

                // to use fmt
                this.Output.WriteLine("fmt.Println(\"done.\")");                

                this.Output.Indent--;
                this.Output.WriteLine("}");
            }
        }

        public void Close()
        {
            this.Output.Close();
        }

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

                writer.Write(parameter.Name);
                writer.Write(" ");
                this.ActualWrite(writer, parameter.ParameterType);

                index++;
            }

            writer.Write(")");
        }

        private void WriteCall(IndentedTextWriter writer, OpCodeConstructorInfoPart opCodeConstructorInfo, bool @isVirtual)
        {
            if (@isVirtual)
            {
                this.ActualWrite(writer, opCodeConstructorInfo.OpCodeOperands[0]);
                writer.Write('.');
            }

            // temp hack
            var ctor = opCodeConstructorInfo.Operand;
            this.WriteMethodName(writer, ctor);
            this.ActualWrite(writer, opCodeConstructorInfo.OpCodeOperands, ctor.GetParameters(), false);
        }

        private void WriteCall(IndentedTextWriter writer, OpCodeMethodInfoPart opCodeMethodInfo, bool isVirtual)
        {
            // temp hack
            var methodBase = opCodeMethodInfo.Operand;
            var declaringType = methodBase.DeclaringType;

            if (declaringType.Namespace == "System" &&
                declaringType.Name == "Object" &&
                methodBase.Name == ".ctor")
            {
                writer.Write("// ");
            }

            if (isVirtual)
            {
                this.ActualWrite(writer, opCodeMethodInfo.OpCodeOperands[0]);
                writer.Write('.');
            }

            if (declaringType.Namespace == "System" &&
                declaringType.Name == "Console" &&
                methodBase.Name == "WriteLine")
            {
                writer.Write("fmt.Println");
            }
            else
            {
                WriteMethodName(writer, methodBase);
            }

            this.ActualWrite(writer, opCodeMethodInfo.OpCodeOperands, methodBase.GetParameters(), isVirtual);
        }

        private static void WriteParametersSuffix(IndentedTextWriter writer, MethodBase methodBase)
        {
            var parameters = methodBase.GetParameters();

            if (parameters.Length == 0)
            {
                return;
            }

            writer.Write('_');

            foreach (var parameter in parameters)
            {
                var typeBaseName = TypeToGoType(parameter.ParameterType);
                writer.Write(typeBaseName[0]);
                foreach (var c in typeBaseName)
                {
                    if (Char.IsDigit(c))
                    {
                        writer.Write(c);
                    }
                }
            }
        }

        private void ActualWrite(IndentedTextWriter writer, OpCodePart[] used, IEnumerable<ParameterInfo> parameterInfos, bool @isVirtual)
        {
            writer.Write("(");

            var index = 0;
            foreach (var parameter in parameterInfos)
            {
                if (index > 0)
                {
                    writer.Write(", ");
                }

                this.ActualWrite(writer, used[index + (@isVirtual ? 1 : 0)]);

                index++;
            }

            writer.Write(")");
        }

        private void ActualWrite(IndentedTextWriter writer, OpCodePart opCode, bool noIf = false)
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
                case Code.Ldc_I4_S:
                    var opCodeInt32 = opCode as OpCodeInt32Part;
                    writer.Write(opCodeInt32.Operand);
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
                case Code.Ldfld:
                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    this.ActualWrite(writer, opCodeFieldInfoPart.OpCodeOperands[0]);
                    writer.Write('.');
                    writer.Write(opCodeFieldInfoPart.Operand.Name);
                    break;
                case Code.Stfld:
                    opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    var declType = opCodeFieldInfoPart.Operand.DeclaringType;
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(".");
                    writer.Write(opCodeFieldInfoPart.Operand.Name);
                    writer.Write(" = ");
                    this.ActualWrite(writer, opCode.OpCodeOperands[1]);
                    break;
                case Code.Call:
                case Code.Callvirt:
                    var opCodeMethodInfoPart = opCode as OpCodeMethodInfoPart;
                    var methodBase = opCodeMethodInfoPart.Operand;
                    this.WriteCall(writer, opCodeMethodInfoPart, code == Code.Callvirt);
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
                case Code.Not:
                    writer.Write("^");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Neg:
                    writer.Write("-");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Dup:
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    break;
                case Code.Ret:
                    writer.Write("return");
                    if (opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > 0)
                    {
                        writer.Write(' ');
                        this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    }

                    break;
                case Code.Stloc:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                case Code.Stloc_S:

                    bool increment;
                    bool before;

                    if (this.IsIncrementOrDecrement(opCode, out increment, out before))
                    {
                        SaveIncDec(writer, opCode, increment, before);
                    }
                    else
                    {
                        SaveLocal(writer, opCode);
                    }

                    break;
                case Code.Ldloc:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldloc_S:
                    writer.Write("local");
                    var asString = code.ToString();

                    if (code == Code.Ldloc_S || code == Code.Ldloc)
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
                    if (code == Code.Ldarg_S || code == Code.Ldarg)
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
                        writer.Write("this");
                    }
                    else
                    {
                        writer.Write(ParameterInfo[index - (HasMethodThis ? 1 : 0)].Name);
                    }

                    break;
                case Code.Beq:
                case Code.Beq_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Bge:
                case Code.Bge_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;

                    if (!noIf)
                    {
                        writer.Write("if ");
                    }

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);

                    if (noIf)
                    {
                        // we need to invert all comare command
                        switch (code)
                        {
                            case Code.Beq:
                            case Code.Beq_S:
                                writer.Write(" == ");
                                break;
                            case Code.Blt:
                            case Code.Blt_S:
                                writer.Write(" < ");
                                break;
                            case Code.Ble:
                            case Code.Ble_S:
                                writer.Write(" <= ");
                                break;
                            case Code.Bgt:
                            case Code.Bgt_S:
                                writer.Write(" > ");
                                break;
                            case Code.Bge:
                            case Code.Bge_S:
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

                    if (!noIf)
                    {
                        writer.Write(" {");
                        writer.Indent++;
                    }

                    break;
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:

                    opCodeInt32 = opCode as OpCodeInt32Part;

                    writer.Write("if ");

                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);

                    var returnType = this.ReturnType(opCode.OpCodeOperands[0]);

                    writer.Write(" != ");
                    if (code == Code.Brtrue || code == Code.Brtrue_S)
                    {
                        if (returnType != typeof(bool))
                        {
                            writer.Write("1");
                        }
                        else
                        {
                            writer.Write("true");
                        }
                    }

                    if (code == Code.Brfalse || code == Code.Brfalse_S)
                    {
                        if (returnType != typeof(bool))
                        {
                            writer.Write("0");
                        }
                        else
                        {
                            writer.Write("false");
                        }
                    }

                    writer.Write(" {");
                    writer.Indent++;

                    break;
                case Code.Br:
                case Code.Br_S:

                    if (opCode.UseAsElse)
                    {
                        writer.Write("else");
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
                    writer.Write("(uint) (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;

                case Code.Conv_R_Un:
                    writer.Write("(float32) (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");
                    break;

                case Code.Conv_R4:
                case Code.Conv_R8:
                    writer.Write("(");
                    writer.Write(code == Code.Conv_R4 ? "float32" : "float64");
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
                        isUInt = asString["Conv_Ovf_".Length] == 'I';
                        size = asString["Conv_Ovf_".Length + 1];
                    }
                    else
                    {
                        isInt = asString["Conv_".Length] == 'I';
                        isUInt = asString["Conv_".Length] == 'I';
                        size = asString["Conv_".Length + 1];
                    }

                    writer.Write("(");

                    if (isInt)
                    {
                        writer.Write("int");
                    }
                    else if (isUInt)
                    {
                        writer.Write("uint");
                    }

                    switch (size)
                    {
                        case '1': writer.Write("8"); break;
                        case '2': writer.Write("16"); break;
                        case '4': writer.Write(""); break;
                        case '8': writer.Write("64"); break;
                    }

                    writer.Write(") (");
                    this.ActualWrite(writer, opCode.OpCodeOperands[0]);
                    writer.Write(")");

                    break;
                case Code.Newobj:

                    var opCodeConstructorInfoPart = opCode as OpCodeConstructorInfoPart;

                    var declaringType = opCodeConstructorInfoPart.Operand.DeclaringType;

                    writer.Write("new(");
                    writer.Write(TypeToGoType(declaringType));
                    writer.Write(").");
                   
                    this.WriteCall(writer, opCode as OpCodeConstructorInfoPart, code == Code.Callvirt);

                    break;
            }
        }

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

        private void SaveLocal(IndentedTextWriter writer, OpCodePart opCode)
        {
            SaveLocalIndex(writer, opCode);
            writer.Write(" = ");
            this.ActualWrite(writer, opCode.OpCodeOperands[0]);
        }

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
                index = Int32.Parse(asString.Substring(asString.Length - 1));
            }

            writer.Write(index);
        }

        private void AdjustTypes(OpCodePart opCode)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            var usedOpCode1 = opCode.OpCodeOperands[0];
            if (usedOpCode1 == null)
            {
                // todo: should not be here
                return;
            }

            var usedCode1 = usedOpCode1.ToCode();

            // fix types
            var requiredType = this.IsRequiredType(opCode);
            if (requiredType != null)
            {
                var receivingType = this.ReturnType(usedOpCode1);
                if (requiredType != receivingType)
                {
                    if (requiredType == typeof(bool) && usedCode1 == Code.Ldc_I4_0)
                    {
                        usedOpCode1.UseAsBoolean = true;
                        return;
                    }

                    if (requiredType == typeof(bool) && usedCode1 == Code.Ldc_I4_1)
                    {
                        usedOpCode1.UseAsBoolean = true;
                        return;
                    }
                }
            }

            if (opCode.OpCodeOperands.Length == 2 
                && opCode.OpCode.StackBehaviourPop == StackBehaviour.Pop1_pop1 
                && (opCode.OpCode.StackBehaviourPush == StackBehaviour.Push1
                    || opCode.OpCode.StackBehaviourPush == StackBehaviour.Pushi))
            {
                // types should be equal
                var usedOpCode2 = opCode.OpCodeOperands[1];
                var usedCode2 = usedOpCode2.ToCode();

                var type1 = this.ReturnType(usedOpCode1);
                var type2 = this.ReturnType(usedOpCode2);

                if (type1 != type2)
                {
                    if (type1 == typeof(bool) && usedCode2 == Code.Ldc_I4_0)
                    {
                        usedOpCode2.UseAsBoolean = true;
                        return;
                    }

                    if (type1 == typeof(bool) && usedCode2 == Code.Ldc_I4_1)
                    {
                        usedOpCode2.UseAsBoolean = true;
                        return;
                    }

                    if (type2 == typeof(bool) && usedCode1 == Code.Ldc_I4_0)
                    {
                        usedOpCode1.UseAsBoolean = true;
                        return;
                    }

                    if (type2 == typeof(bool) && usedCode1 == Code.Ldc_I4_1)
                    {
                        usedOpCode1.UseAsBoolean = true;
                        return;
                    }
                }
            }
        }

        private Type ReturnType(OpCodePart opCode)
        {
            var code = opCode.ToCode();
            switch (code)
            {
                case Code.Call:
                case Code.Callvirt:
                    var methodBase = (opCode as OpCodeMethodInfoPart).Operand as MethodInfo;
                    return methodBase.ReturnType;
                case Code.Newobj:
                    var ctorInfo = (opCode as OpCodeConstructorInfoPart).Operand;
                    return ctorInfo.DeclaringType;
                case Code.Ldfld:
                    var fieldInfo = (opCode as OpCodeFieldInfoPart).Operand;
                    return fieldInfo.FieldType;
                case Code.Add:
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                case Code.Mul:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                case Code.Sub:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                case Code.Div:
                case Code.Div_Un:
                case Code.Beq:
                case Code.Beq_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Rem:
                case Code.Rem_Un:
                    return ReturnType(opCode.OpCodeOperands[0]);
                case Code.Ceq:
                case Code.Cgt:
                case Code.Cgt_Un:
                case Code.Clt:
                case Code.Clt_Un:
                    return typeof(bool);
                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                case Code.Conv_R_Un:
                case Code.Conv_R4:
                case Code.Conv_R8:
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
                case Code.Ret:
                case Code.Neg:
                case Code.Not:
                    return ReturnType(opCode.OpCodeOperands[0]);
                case Code.Ldloc:
                case Code.Ldloc_S:
                    return this.LocalInfo[(opCode as OpCodeInt32Part).Operand].LocalType;
                case Code.Ldloc_0:
                    return this.LocalInfo[0].LocalType;
                case Code.Ldloc_1:
                    return this.LocalInfo[1].LocalType;
                case Code.Ldloc_2:
                    return this.LocalInfo[2].LocalType;
                case Code.Ldloc_3:
                    return this.LocalInfo[3].LocalType;
                case Code.Ldarg:
                case Code.Ldarg_S:
                    return this.ParameterInfo[(opCode as OpCodeInt32Part).Operand].ParameterType;
                case Code.Ldarg_0:
                    return this.ParameterInfo[0].ParameterType;
                case Code.Ldarg_1:
                    return this.ParameterInfo[1].ParameterType;
                case Code.Ldarg_2:
                    return this.ParameterInfo[2].ParameterType;
                case Code.Ldarg_3:
                    return this.ParameterInfo[3].ParameterType;
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                case Code.Ldc_I4_M1:
                case Code.Ldc_I4:
                case Code.Ldc_I4_S:
                    return typeof(int);
                case Code.Ldc_I8:
                    return typeof(long);
                case Code.Ldc_R4:
                    return typeof(float);
                case Code.Ldc_R8:
                    return typeof(double);
                case Code.Ldstr:
                    return typeof(string);
            }

            return null;
        }

        private static string TypeToGoType(Type type)
        {
            var effectiveType = type;

            if (type.IsArray)
            {
                effectiveType = type.GetElementType();
            }

            if (effectiveType.Namespace == "System")
            {
                string goType;
                if (systemTypesToGoTypes.TryGetValue(effectiveType.Name, out goType))
                {
                    return goType;
                }
            }

            if (type.IsValueType)
            {
                return type.Name.ToLowerInvariant();
            }

            return type.FullName.Replace('.', '_');
        }

        private void WriteMethodName(IndentedTextWriter writer, MethodBase methodBase)
        {
            writer.Write(methodBase.DeclaringType.FullName.Replace('.', '_'));
            writer.Write("_");
            writer.Write(methodBase.Name.Replace('.', '_'));

            // add parameters suffixes
            WriteParametersSuffix(writer, methodBase);
        }

        private void ActualWrite(IndentedTextWriter writer, Type type)
        {
            var effectiveType = type;

            if (type.IsArray)
            {
                writer.Write("[]");
                effectiveType = type.GetElementType();
            }

            var typeBaseName = TypeToGoType(effectiveType);

            if (!effectiveType.IsValueType)
            {
                if (effectiveType.Namespace != "System" 
                    && effectiveType.Name != "String")
                {
                    writer.Write("*");
                }
            }

            writer.Write(typeBaseName);
        }
    }
}

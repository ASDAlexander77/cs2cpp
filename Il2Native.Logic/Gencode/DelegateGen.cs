// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic.Gencode
{
    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class DelegateGen
    {
        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsDelegateFunctionBody(this IMethod method)
        {
            if (!method.IsExternal || !method.DeclaringType.IsDelegate)
            {
                return false;
            }

            return method.Name == ".ctor" || method.Name == "Invoke" || method.Name == "BeginInvoke" || method.Name == "EndInvoke";
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="method">
        /// </param>
        public static void WriteDelegateFunctionBody(this LlvmWriter llvmWriter, IMethod method)
        {
            if (method.Name == ".ctor")
            {
                llvmWriter.WriteDelegateConstructor(method);
            }
            else if (method.Name == "Invoke")
            {
                llvmWriter.WriteDelegateInvoke(method);
            }
            else
            {
                llvmWriter.DefaultStub(method);
            }
        }

        private static void WriteDelegateConstructor(this LlvmWriter llvmWriter, IMethod method)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(" {");
            writer.Indent++;

            var opCode = OpCodePart.CreateNop;

            // create this variable
            llvmWriter.WriteArgumentCopyDeclaration("this", method.DeclaringType, true);
            for (var i = 1; i <= llvmWriter.GetArgCount() + 1; i++)
            {
                llvmWriter.WriteArgumentCopyDeclaration(llvmWriter.GetArgName(i), llvmWriter.GetArgType(i));
            }

            // load 'this' variable
            llvmWriter.WriteLlvmLoad(opCode, method.DeclaringType, new FullyDefinedReference("%.this", method.DeclaringType));
            writer.WriteLine(string.Empty);

            var thisResult = opCode.Result;

            // write access to a field 1
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 1, thisResult.ToFullyDefinedReference());
            writer.WriteLine(string.Empty);

            // load value 1
            opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_1, 0, 0) };
            llvmWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
            writer.WriteLine(string.Empty);
            
            // save value 1
            llvmWriter.WriteSaveToField(opCode, opCode.Result.Type, 0);
            writer.WriteLine(string.Empty);

            // write access to a field 2
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 2, thisResult.ToFullyDefinedReference());
            writer.WriteLine(string.Empty);

            // load value 2
            opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_2, 0, 0) };
            llvmWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
            writer.WriteLine(string.Empty);

            // save value 2
            llvmWriter.WriteSaveToField(opCode, opCode.Result.Type, 0);
            writer.WriteLine(string.Empty);

            writer.WriteLine("ret void");

            writer.Indent--;
            writer.WriteLine("}");
        }

        private static void WriteDelegateInvoke(this LlvmWriter llvmWriter, IMethod method)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(" {");
            writer.Indent++;

            var opCode = OpCodePart.CreateNop;

            // create this variable
            llvmWriter.WriteArgumentCopyDeclaration("this", method.DeclaringType, true);
            for (var i = 1; i <= llvmWriter.GetArgCount() + 1; i++)
            {
                llvmWriter.WriteArgumentCopyDeclaration(llvmWriter.GetArgName(i), llvmWriter.GetArgType(i));
            }

            // load 'this' variable
            llvmWriter.WriteLlvmLoad(opCode, method.DeclaringType, new FullyDefinedReference("%.this", method.DeclaringType));
            writer.WriteLine(string.Empty);

            var thisResult = opCode.Result;

            // write access to a field 1
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 1, thisResult.ToFullyDefinedReference());
            writer.WriteLine(string.Empty);

            var objectMemberAccessResultNumber = opCode.Result;

            // load value 1
            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, objectMemberAccessResultNumber.Type, objectMemberAccessResultNumber);
            writer.WriteLine(string.Empty);

            var objectResultNumber = opCode.Result;

            // write access to a field 2
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 2, thisResult.ToFullyDefinedReference());
            writer.WriteLine(string.Empty);

            // load value 2
            var methodMemberAccessResultNumber = opCode.Result;

            // load value 1
            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, methodMemberAccessResultNumber.Type, methodMemberAccessResultNumber);
            writer.WriteLine(string.Empty);

            var methodResultNumber = opCode.Result;

            writer.WriteLine("ret i32 undef");

            writer.Indent--;
            writer.WriteLine("}");
        }

        private static void DefaultStub(this LlvmWriter llvmWriter, IMethod method)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(" {");
            writer.Indent++;

            writer.Write("ret ");

            if (method.ReturnType.IsVoid())
            {
                writer.WriteLine("void");
            }
            else
            {
                method.ReturnType.WriteTypePrefix(writer);
                writer.WriteLine(" undef");
            }

            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
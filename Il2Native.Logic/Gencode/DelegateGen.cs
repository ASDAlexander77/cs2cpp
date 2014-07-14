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
            if (!method.IsExternal && method.DeclaringType.IsDelegate)
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

            llvmWriter.WriteLlvmLoad(opCode, method.DeclaringType, new FullyDefinedReference("%.this", method.DeclaringType), true, true);
            writer.WriteLine(string.Empty);

            // write access to a field
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 1, new FullyDefinedReference("%.this", method.DeclaringType));
            writer.WriteLine(string.Empty);

            writer.Write("ret void");

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
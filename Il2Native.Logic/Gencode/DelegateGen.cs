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
            return method.Name == "Invoke" || method.Name == "BeginInvoke" || method.Name == "EndInvoke";
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="method">
        /// </param>
        public static void WriteDelegateFunctionBody(this LlvmWriter llvmWriter, IMethod method)
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
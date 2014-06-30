// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArraySingleDimensionGen.cs" company="">
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
        public static bool IsDelegateFunctionBody(this IMethod method)
        {
            return method.Name == "Invoke" || method.Name == "BeginInvoke" || method.Name == "EndInvoke";
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteDelegateFunctionBody(this LlvmWriter llvmWriter, IMethod method)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(" {");
            writer.Indent++;

            if (method.Name == "BeginInvoke")
            {
                writer.WriteLine("ret %\"System.IAsyncResult\"* null");
            }
            else
            {
                writer.WriteLine("ret void");
            }

            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
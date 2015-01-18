// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RttiClassWithBaseAndNoInterfaces.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System.CodeDom.Compiler;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class RttiClassWithBaseAndNoInterfaces
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiClassInfoDeclaration(IType type, IndentedTextWriter writer)
        {
            writer.Write("{ i8*, i8*, i8* }");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteRttiClassInfoDefinition(IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine(
                "i8* bitcast (i8** getelementptr inbounds (i8** @_ZTVN10__cxxabiv120__si_class_type_infoE, i32 2) to i8*),");
            writer.WriteLine(
                "i8* getelementptr inbounds ([{1} x i8]* @\"{0}\", i32 0, i32 0),",
                type.GetRttiStringName(),
                type.StringLength());
            writer.Write("i8* bitcast (");
            type.BaseType.WriteRttiClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*)", type.BaseType.GetRttiInfoName());
            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
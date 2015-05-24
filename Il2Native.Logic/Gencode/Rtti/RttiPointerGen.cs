// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RttiPointerGen.cs" company="">
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
    public static class RttiPointerGen
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRttiPointerInfoName(this IType type, CWriter cWriter)
        {
            return string.Concat("_RTTI_", type.FullName, " Pointer Info").CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassDefinition(this IType type, CWriter cWriter)
        {
            type.WriteRttiPointerClassInfo(cWriter);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassInfo(this IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.Write(cWriter.declarationPrefix);
            writer.Write("const struct ");
            type.WriteRttiPointerClassInfoDeclaration(writer);
            writer.Write(" ");
            writer.Write(type.GetRttiPointerInfoName(cWriter));
            writer.Write(" = ");
            type.WriteRttiPointerClassInfoDefinition(cWriter);
            writer.WriteLine(";");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassInfoDeclaration(this IType type, IndentedTextWriter writer)
        {
            writer.Write("{ Byte* f1; Byte* f2; Int32 f3; Byte* f4; }");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassInfoDefinition(this IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine(
                "(Byte*) (((Byte**) &_ZTVN10__cxxabiv119__pointer_type_infoE) + 2),");
            writer.Write("(Byte*)");
            type.WriteRttiPointerNameString(writer);
            writer.WriteLine(",0,");
            writer.WriteLine("(Byte*)&{0}", type.GetRttiInfoName(cWriter));
            writer.Indent--;
            writer.Write("}");
        }

        public static void WriteRttiPointerNameString(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine(
                "\"P{1}{0}\"",
                type.FullName,
                type.FullName.Length);
        }
    }
}
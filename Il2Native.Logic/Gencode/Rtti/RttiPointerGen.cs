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
        public static string GetRttiPointerInfoName(this IType type)
        {
            return string.Concat("_RTTI_", type.FullName, " Pointer Info").CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRttiPointerStringName(this IType type)
        {
            return string.Concat("_RTTI_", type.FullName, " Pointer String Name").CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiClassInfoExternalDeclaration(this IType type, IndentedTextWriter writer)
        {
            writer.Write("@\"{0}\" = external global ", type.GetRttiInfoName());
            type.WriteRttiClassInfoDeclaration(writer);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassDefinition(this IType type, IndentedTextWriter writer)
        {
            type.WriteRttiPointerClassName(writer);
            type.WriteRttiPointerClassInfo(writer);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassInfo(this IType type, IndentedTextWriter writer)
        {
            type.WriteRttiPointerClassInfoDeclaration(writer);
            writer.Write(" ");
            writer.Write(type.GetRttiPointerInfoName());
            writer.Write(" = ");
            type.WriteRttiPointerClassInfoDefinition(writer);
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
            writer.Write("struct { Byte* f1; Byte* f2; Int32 f3; Byte* f4 }");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassInfoDefinition(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine(
                "(Byte*)_ZTVN10__cxxabiv119__pointer_type_infoE[2],");
            writer.WriteLine("(Byte*){0},", type.GetRttiPointerStringName());
            writer.WriteLine("0,");
            writer.WriteLine("(Byte*){0}", type.GetRttiInfoName());
            writer.Indent--;
            writer.Write("}");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassInfoExternalDeclaration(this IType type, IndentedTextWriter writer)
        {
            writer.Write("@\"{0}\" = external global ", type.GetRttiPointerInfoName());
            type.WriteRttiPointerClassInfoDeclaration(writer);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassName(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine(
                "const char* {0} = \"P{2}{1}\\00\";",
                type.GetRttiPointerStringName(),
                type.FullName,
                type.FullName.Length);
        }
    }
}
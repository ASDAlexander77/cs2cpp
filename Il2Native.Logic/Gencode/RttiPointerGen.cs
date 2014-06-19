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
        public static void WriteRttiPointerClassName(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine(
                "@\"{0}\" = linkonce_odr constant [{3} x i8] c\"P{2}{1}\\00\"", 
                type.GetRttiPointerStringName(), 
                type.FullName, 
                type.FullName.Length, 
                type.StringLength(1));
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRttiPointerStringName(this IType type)
        {
            return string.Concat(type.FullName, " Pointer String Name");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRttiPointerInfoName(this IType type)
        {
            return string.Concat(type.FullName, " Pointer Info");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiPointerClassInfo(this IType type, IndentedTextWriter writer)
        {
            writer.Write("@\"{0}\" = linkonce_odr unnamed_addr constant ", type.GetRttiPointerInfoName());
            type.WriteRttiPointerClassInfoDeclaration(writer);
            writer.Write(' ');
            type.WriteRttiPointerClassInfoDefinition(writer);
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
        public static void WriteRttiPointerClassInfoDeclaration(this IType type, IndentedTextWriter writer)
        {
            writer.Write("{ i8*, i8*, i32, i8* }");
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
            writer.WriteLine("i8* bitcast (i8** getelementptr inbounds (i8** @_ZTVN10__cxxabiv119__pointer_type_infoE, i32 2) to i8*),");
            writer.WriteLine("i8* getelementptr inbounds ([{1} x i8]* @\"{0}\", i32 0, i32 0),", type.GetRttiPointerStringName(), type.StringLength(1));
            writer.WriteLine("i32 0,");
            writer.Write("i8* bitcast (");
            type.WriteRttiClassInfoDeclaration(writer);
            writer.WriteLine("* @\"{0}\" to i8*)", type.GetRttiInfoName());
            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
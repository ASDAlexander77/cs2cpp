// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RttiClassGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.CodeDom.Compiler;
    using System.Linq;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class RttiClassGen
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRttiInfoName(this IType type, CWriter cWriter)
        {
            return string.Concat("_RTTI_", type.FullName, " Info").CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteRttiDefinition(this IType type, CWriter cWriter)
        {
            type.WriteRttiClassDefinition(cWriter);
            type.WriteRttiPointerClassDefinition(cWriter);
        }

        public static void WriteRttiDeclaration(this IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.Write(cWriter.declarationPrefix);
            writer.Write("const struct ");
            type.WriteRttiClassInfoDeclaration(writer);
            writer.Write(" ");
            writer.Write(type.GetRttiInfoName(cWriter));
            writer.WriteLine(";");

            writer.Write(cWriter.declarationPrefix);
            writer.Write("const struct ");
            type.WriteRttiPointerClassInfoDeclaration(writer);
            writer.Write(" ");
            writer.Write(type.GetRttiPointerInfoName(cWriter));
            writer.WriteLine(";");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteRttiClassDefinition(this IType type, CWriter cWriter)
        {
            type.WriteRttiClassInfo(cWriter);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteRttiClassInfo(this IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.Write(cWriter.declarationPrefix);
            cWriter.WriteRawText("const struct ");
            type.WriteRttiClassInfoDeclaration(writer);
            writer.Write(" ");
            writer.Write(type.GetRttiInfoName(cWriter));
            writer.Write(" = ");
            type.WriteRttiClassInfoDefinition(cWriter);
            writer.WriteLine(";");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiClassInfoDeclaration(this IType type, IndentedTextWriter writer)
        {
            var interfaces = type.GetInterfaces();
            var anyInterface = interfaces.Any();
            var onlyInterface = interfaces.Count() == 1;
            if (type.BaseType == null && !anyInterface)
            {
                RttiClassWithNoBaseAndNoInterfaces.WriteRttiClassInfoDeclaration(type, writer);
                return;
            }

            if (type.BaseType == null && onlyInterface)
            {
                RttiClassWithNoBaseAndSingleInterface.WriteRttiClassInfoDeclaration(type, writer);
                return;
            }

            if (anyInterface)
            {
                RttiClassWithBaseAndInterfaces.WriteRttiClassInfoDeclaration(type, writer);
                return;
            }

            RttiClassWithBaseAndNoInterfaces.WriteRttiClassInfoDeclaration(type, writer);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteRttiClassInfoDefinition(this IType type, CWriter cWriter)
        {
            var interfaces = type.GetInterfaces();
            var anyInterface = interfaces.Any();
            var onlyInterface = interfaces.Count() == 1;
            if (type.BaseType == null && !anyInterface)
            {
                RttiClassWithNoBaseAndNoInterfaces.WriteRttiClassInfoDefinition(type, cWriter);
                return;
            }

            if (anyInterface)
            {
                if (type.BaseType == null && onlyInterface)
                {
                    RttiClassWithNoBaseAndSingleInterface.WriteRttiClassInfoDefinition(type, cWriter);
                    return;
                }

                RttiClassWithBaseAndInterfaces.WriteRttiClassInfoDefinition(type, cWriter);
                return;
            }

            RttiClassWithBaseAndNoInterfaces.WriteRttiClassInfoDefinition(type, cWriter);
        }

        public static void WriteRttiClassNameString(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine(
                "\"{1}{0}\"",
                type.FullName,
                type.FullName.Length);
        }
    }
}
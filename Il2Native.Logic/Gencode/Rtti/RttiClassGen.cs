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
        public static string GetRttiInfoName(this IType type)
        {
            return string.Concat("_RTTI_", type.FullName, " Info").CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRttiStringName(this IType type)
        {
            return string.Concat("_RTTI_", type.FullName, " String Name").CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="additional">
        /// </param>
        /// <returns>
        /// </returns>
        public static int StringLength(this IType type, int additional = 0)
        {
            return type.FullName.Length + 1 + type.FullName.Length.ToString().Length + additional;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteRtti(this IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            cWriter.forwardTypeRttiDeclarationWritten.Add(type);

            if (type.BaseType != null)
            {
                cWriter.WriteRttiDeclarationIfNotWrittenYet(type.BaseType);
            }

            foreach (var @interface in type.GetInterfaces())
            {
                cWriter.WriteRttiDeclarationIfNotWrittenYet(@interface);
            }

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
            writer.Write(type.GetRttiInfoName());
            writer.WriteLine(";");

            writer.Write(cWriter.declarationPrefix);
            writer.Write("const struct ");
            type.WriteRttiPointerClassInfoDeclaration(writer);
            writer.Write(" ");
            writer.Write(type.GetRttiPointerInfoName());
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
            cWriter.Write("const struct ");
            type.WriteRttiClassInfoDeclaration(writer);
            writer.Write(" ");
            writer.Write(type.GetRttiInfoName());
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
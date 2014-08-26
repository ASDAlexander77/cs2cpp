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
            return string.Concat(type.FullName, " Info");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetRttiStringName(this IType type)
        {
            return string.Concat(type.FullName, " String Name");
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
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteRtti(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; RTTI class");
            type.WriteRttiClassDefinition(llvmWriter);
            writer.WriteLine("; RTTI pointer");
            type.WriteRttiPointerClassDefinition(writer);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteRttiClassDefinition(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            type.WriteRttiClassName(writer);
            type.WriteRttiClassInfo(llvmWriter);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteRttiClassInfo(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.Write("@\"{0}\" = linkonce_odr unnamed_addr constant ", type.GetRttiInfoName());
            type.WriteRttiClassInfoDeclaration(writer);
            writer.Write(' ');
            type.WriteRttiClassInfoDefinition(llvmWriter);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiClassInfoDeclaration(this IType type, IndentedTextWriter writer)
        {
            if (type.BaseType == null && !type.GetInterfaces().Any())
            {
                RttiClassWithNoBaseAndNotInterfacesGen.WriteRttiClassInfoDeclaration(type, writer);
                return;
            }

            if (type.GetInterfaces().Any())
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
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteRttiClassInfoDefinition(this IType type, LlvmWriter llvmWriter)
        {
            if (type.BaseType == null && !type.GetInterfaces().Any())
            {
                RttiClassWithNoBaseAndNotInterfacesGen.WriteRttiClassInfoDefinition(type, llvmWriter);
                return;
            }

            if (type.BaseType != null)
            {
                llvmWriter.typeRttiDeclRequired.Add(type.BaseType);
            }

            if (type.GetInterfaces().Any())
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    llvmWriter.typeRttiDeclRequired.Add(@interface);
                }

                RttiClassWithBaseAndInterfaces.WriteRttiClassInfoDefinition(type, llvmWriter);
                return;
            }

            RttiClassWithBaseAndNoInterfaces.WriteRttiClassInfoDefinition(type, llvmWriter);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiClassName(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine(
                "@\"{0}\" = linkonce_odr constant [{3} x i8] c\"{2}{1}\\00\"",
                type.GetRttiStringName(),
                type.FullName,
                type.FullName.Length,
                type.StringLength());
        }
    }
}
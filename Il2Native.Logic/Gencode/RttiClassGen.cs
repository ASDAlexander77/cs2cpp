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

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class RttiClassGen
    {
        /// <summary>
        /// </summary>
        private static bool generateClassNonVirtual = false;

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
            if (type.BaseType != null)
            {
                // class without virtual members
                if (generateClassNonVirtual)
                {
                    writer.Write("{ i8*, i8*, i32, i32, i8*, i32 }");
                }
                else
                {
                    writer.Write("{ i8*, i8*, i8* }");
                }
            }
            else
            {
                writer.Write("{ i8*, i8* }");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteRttiClassInfoDefinition(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            if (type.BaseType != null)
            {
                if (generateClassNonVirtual)
                {
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.WriteLine("i8* bitcast (i8** getelementptr inbounds (i8** @_ZTVN10__cxxabiv121__vmi_class_type_infoE, i32 2) to i8*),");
                    writer.WriteLine("i8* getelementptr inbounds ([{1} x i8]* @\"{0}\", i32 0, i32 0),", type.GetRttiStringName(), type.StringLength());
                    writer.WriteLine("i32 0,");
                    writer.WriteLine("i32 1,");
                    writer.Write("i8* bitcast (");
                    type.BaseType.WriteRttiClassInfoDeclaration(writer);
                    writer.WriteLine("* @\"{0}\" to i8*),", type.BaseType.GetRttiInfoName());
                    writer.WriteLine("i32 0");
                    writer.Indent--;
                    writer.WriteLine("}");
                }
                else
                {
                    writer.WriteLine("{");
                    writer.Indent++;
                    writer.WriteLine("i8* bitcast (i8** getelementptr inbounds (i8** @_ZTVN10__cxxabiv120__si_class_type_infoE, i32 2) to i8*),");
                    writer.WriteLine("i8* getelementptr inbounds ([{1} x i8]* @\"{0}\", i32 0, i32 0),", type.GetRttiStringName(), type.StringLength());
                    writer.Write("i8* bitcast (");
                    type.BaseType.WriteRttiClassInfoDeclaration(writer);
                    writer.WriteLine("* @\"{0}\" to i8*)", type.BaseType.GetRttiInfoName());
                    writer.Indent--;
                    writer.WriteLine("}");
                }

                llvmWriter.typeRttiDeclRequired.Add(type.BaseType);
            }
            else
            {
                writer.WriteLine("{");
                writer.Indent++;
                writer.WriteLine("i8* bitcast (i8** getelementptr inbounds (i8** @_ZTVN10__cxxabiv117__class_type_infoE, i32 2) to i8*),");
                writer.WriteLine("i8* getelementptr inbounds ([{1} x i8]* @\"{0}\", i32 0, i32 0)", type.GetRttiStringName(), type.StringLength());
                writer.Indent--;
                writer.WriteLine("}");
            }
        }
    }
}
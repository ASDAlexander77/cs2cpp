namespace Il2Native.Logic.Gencode
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PEAssemblyReader;

    public static class RttiClassGen
    {
        private static bool generateClassNonVirtual = false;

        public static void WriteRtti(this IType type, LlvmWriter llvmWriter, IndentedTextWriter writer)
        {
            writer.WriteLine("; RTTI class");
            type.WriteRttiClassDefinition(llvmWriter, writer);
            writer.WriteLine("; RTTI pointer");
            type.WriteRttiPointerClassDefinition(writer);
        }

        public static int StringLength(this IType type, int additional = 0)
        {
            return type.FullName.Length + 1 + type.FullName.Length.ToString().Length + additional;
        }

        public static void WriteRttiClassDefinition(this IType type, LlvmWriter llvmWriter, IndentedTextWriter writer)
        {
            type.WriteRttiClassName(writer);
            type.WriteRttiClassInfo(llvmWriter, writer);
        }

        public static void WriteRttiClassName(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine("@\"{0}\" = linkonce_odr constant [{3} x i8] c\"{2}{1}\\00\"", type.GetRttiStringName(), type.FullName, type.FullName.Length, type.StringLength());
        }

        public static string GetRttiStringName(this IType type)
        {
            return string.Concat(type.FullName, " String Name");
        }

        public static string GetRttiInfoName(this IType type)
        {
            return string.Concat(type.FullName, " Info");
        }

        public static void WriteRttiClassInfo(this IType type, LlvmWriter llvmWriter, IndentedTextWriter writer)
        {
            writer.Write("@\"{0}\" = linkonce_odr unnamed_addr constant ", type.GetRttiInfoName());
            type.WriteRttiClassInfoDeclaration(writer);
            writer.Write(' ');
            type.WriteRttiClassInfoDefinition(llvmWriter, writer);
        }

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

        public static void WriteRttiClassInfoDefinition(this IType type, LlvmWriter llvmWriter, IndentedTextWriter writer)
        {
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

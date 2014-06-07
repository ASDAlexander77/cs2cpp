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
        public static void WriteRttiClassDefinition(this IType type, IndentedTextWriter writer)
        {
            type.WriteRttiClassName(writer);
            type.WriteRttiClassInfo(writer);
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

        public static void WriteRttiClassInfo(this IType type, IndentedTextWriter writer)
        {
            writer.Write("@\"{0}\" = linkonce_odr unnamed_addr constant ", type.GetRttiInfoName());
            type.WriteRttiClassInfoDeclaration(writer);
            writer.Write(' ');
            type.WriteRttiClassInfoDefinition(writer);
        }

        public static void WriteRttiClassInfoDeclaration(this IType type, IndentedTextWriter writer)
        {
            writer.Write("{ i8*, i8* }");
        }

        public static void WriteRttiClassInfoDefinition(this IType type, IndentedTextWriter writer)
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

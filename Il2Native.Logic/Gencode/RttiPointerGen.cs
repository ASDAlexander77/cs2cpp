namespace Il2Native.Logic.Gencode
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PEAssemblyReader;

    public static class RttiPointerGen
    {
        public static void WriteRttiPointerClassDefinition(this IType type, IndentedTextWriter writer)
        {
            type.WriteRttiPointerClassName(writer);
            type.WriteRttiPointerClassInfo(writer);
        }

        public static void WriteRttiPointerClassName(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine("@\"{0}\" = linkonce_odr constant [{3} x i8] c\"P{2}{1}\\00\"", type.GetRttiPointerStringName(), type.FullName, type.FullName.Length, type.StringLength(1));
        }

        public static string GetRttiPointerStringName(this IType type)
        {
            return string.Concat(type.FullName, " Pointer String Name");
        }

        public static string GetRttiPointerInfoName(this IType type)
        {
            return string.Concat(type.FullName, " Pointer Info");
        }

        public static void WriteRttiPointerClassInfo(this IType type, IndentedTextWriter writer)
        {
            writer.Write("@\"{0}\" = linkonce_odr unnamed_addr constant ", type.GetRttiPointerInfoName());
            type.WriteRttiPointerClassInfoDeclaration(writer);
            writer.Write(' ');
            type.WriteRttiPointerClassInfoDefinition(writer);
        }

        public static void WriteRttiPointerClassInfoExternalDeclaration(this IType type, IndentedTextWriter writer)
        {
            writer.Write("@\"{0}\" = external global ", type.GetRttiPointerInfoName());
            type.WriteRttiPointerClassInfoDeclaration(writer);
        }

        public static void WriteRttiPointerClassInfoDeclaration(this IType type, IndentedTextWriter writer)
        {
            writer.Write("{ i8*, i8*, i32, i8* }");
        }

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

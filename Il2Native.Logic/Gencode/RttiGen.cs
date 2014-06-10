namespace Il2Native.Logic.Gencode
{
    using PEAssemblyReader;
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class RttiGen
    {
        public static void WriteRtti(this IType type, IndentedTextWriter writer)
        {
            writer.WriteLine("; RTTI class");
            type.WriteRttiClassDefinition(writer);
            writer.WriteLine("; RTTI pointer");
            type.WriteRttiPointerClassDefinition(writer);
        }

        public static int StringLength(this IType type, int additional = 0)
        {
            return type.FullName.Length + 1 + type.FullName.Length.ToString().Length + additional;
        }
    }
}

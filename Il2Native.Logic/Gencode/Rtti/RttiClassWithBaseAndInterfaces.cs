// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RttiClassWithBaseAndInterfaces.cs" company="">
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
    public class RttiClassWithBaseAndInterfaces
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public static void WriteRttiClassInfoDeclaration(IType type, IndentedTextWriter writer)
        {
            writer.Write("struct { Byte* f1; Byte* f2; Int32 f3; Int32 f4");

            if (type.BaseType != null)
            {
                writer.Write("; Byte* f5; Int32 f6");
            }

            var index = 7;
            foreach (var @interface in type.GetInterfaces())
            {
                writer.Write("; Byte* f{0}; Int32 f{1};", index++, index++);
            }

            writer.Write(" }");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteRttiClassInfoDefinition(IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            var @interfaces = type.GetInterfaces();

            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine(
                "(Byte*)_ZTVN10__cxxabiv121__vmi_class_type_infoE[2],");
            writer.WriteLine("(Byte*){0},", type.GetRttiStringName());
            writer.WriteLine("0,");
            writer.WriteLine("{0}", @interfaces.Count() + (type.BaseType != null ? 1 : 0));

            var nextFlag = 2;

            if (type.BaseType != null)
            {
                writer.Write(",");
                writer.WriteLine("(Byte*){0},", type.BaseType.GetRttiInfoName());

                // if class does not have any virtual method then next value should be 0, else 2 (and next class should be +1024)
                writer.WriteLine("{0}", nextFlag);

                // apply fields shift + base item
                // nextFlag += 1024 * (type.BaseType.GetFieldsShift() + 1);
                nextFlag += 1024 * (type.BaseType.GetTypeSize(cWriter) / CWriter.PointerSize);
            }

            foreach (var @interface in type.GetInterfaces())
            {
                writer.Write(",");
                writer.WriteLine("(Byte*){0},", @interface.GetRttiInfoName());
                writer.WriteLine("{0}", nextFlag);
                nextFlag += 1024;
            }

            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
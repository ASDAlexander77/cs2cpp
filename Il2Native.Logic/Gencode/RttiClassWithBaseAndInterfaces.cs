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
            writer.Write("{ i8*, i8*, i32, i32");

            if (type.BaseType != null)
            {
                writer.Write(", i8*, i32");
            }

            foreach (var @interface in type.GetInterfaces())
            {
                writer.Write(", i8*, i32");
            }

            writer.Write(" }");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteRttiClassInfoDefinition(IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var @interfaces = type.GetAllInterfaces();

            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("i8* bitcast (i8** getelementptr inbounds (i8** @_ZTVN10__cxxabiv121__vmi_class_type_infoE, i32 2) to i8*),");
            writer.WriteLine("i8* getelementptr inbounds ([{1} x i8]* @\"{0}\", i32 0, i32 0),", type.GetRttiStringName(), type.StringLength());
            writer.WriteLine("i32 0,");
            writer.WriteLine("i32 {0},", @interfaces.Count() + (type.BaseType != null ? 1 : 0));

            var nextFlag = 2;

            if (type.BaseType != null)
            {
                writer.Write("i8* bitcast (");
                type.BaseType.WriteRttiClassInfoDeclaration(writer);
                writer.WriteLine("* @\"{0}\" to i8*),", type.BaseType.GetRttiInfoName());
                // if class does not have any virtual method then next value should be 0, else 2 (and next class should be +1024)
                writer.WriteLine("i32 {0},", nextFlag);

                // apply fields shift + base item
                nextFlag += 1024 * (type.BaseType.GetFieldsShift() + 1);
            }

            foreach (var @interface in type.GetInterfaces())
            {
                writer.Write("i8* bitcast (");
                @interface.WriteRttiClassInfoDeclaration(writer);
                writer.WriteLine("* @\"{0}\" to i8*),", @interface.GetRttiInfoName());
                writer.WriteLine("i32 {0}", nextFlag);
                nextFlag += 1024;
            }

            writer.Indent--;
            writer.WriteLine("}");
        }
    }
}
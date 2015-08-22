// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatucClassGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class StatucClassGen
    {
        public static void WriteDefualtStructInitialization(this CIndentedTextWriter writer)
        {
            writer.Write("{ 0 }");
        }

        public static void WriteVirtualTableImplementationReference(this CWriter cWriter, IType tokenType)
        {
            var vtName = tokenType.InterfaceOwner != null
                ? tokenType.InterfaceOwner.GetVirtualInterfaceTableNameReference(tokenType, cWriter)
                : tokenType.GetVirtualTableNameReference(cWriter);
            cWriter.Output.Write(vtName);
        }

        public static void WriteClassInitialization(this CWriter cWriter, IType fieldType)
        {
            WriteClassInitializationInternal(cWriter, fieldType, fieldType);
        }

        private static void WriteClassInitializationInternal(CWriter cWriter, IType type, IType instanceType)
        {
            var comma = false;

            var writer = cWriter.Output;

            writer.Write("{ ");

            if (type.BaseType != null)
            {
                WriteClassInitializationInternal(cWriter, type.BaseType, instanceType);
                comma = true;
            }

            foreach (var field in IlReader.Fields(type, cWriter).Where(f => !f.IsStatic))
            {
                if (comma)
                {
                    writer.Write(", ");
                }

                if (!field.FieldType.IsStructureType())
                {
                    if (field.IsVirtualTable)
                    {
                        cWriter.WriteVirtualTableImplementationReference(instanceType);
                    }
                    else
                    {
                        writer.Write(" 0 ");
                    }
                }
                else
                {
                    cWriter.WriteClassInitialization(field.FieldType);
                }

                comma = true;
            }

            writer.Write(" }");
        }
    }
}
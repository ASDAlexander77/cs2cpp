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

        public static void WriteClassInitialization(this CWriter cWriter, IType fieldType, IType typeOfRuntimeTypeInfo)
        {
            cWriter.WriteClassInitializationInternal(fieldType, fieldType, typeOfRuntimeTypeInfo);
        }

        private static void WriteClassInitializationInternal(this CWriter cWriter, IType type, IType instanceType, IType typeOfRuntimeTypeInfo, bool withBase = true)
        {
            var isRuntimeTypeInfo = instanceType.TypeEquals(cWriter.System.System_RuntimeType);

            var comma = false;

            var writer = cWriter.Output;

            writer.Write("{ ");

            if (withBase && type.BaseType != null)
            {
                WriteClassInitializationInternal(cWriter, type.BaseType, instanceType, typeOfRuntimeTypeInfo);
                comma = true;
            }

            foreach (var field in IlReader.Fields(type, cWriter).Where(f => !f.IsStatic))
            {
                if (comma)
                {
                    writer.Write(", ");
                }

                // to support runtimeType info
                if (isRuntimeTypeInfo)
                {
                    var value = RuntimeTypeInfoGen.GetRuntimeTypeInfo(field, typeOfRuntimeTypeInfo, cWriter);
                    if (value != null)
                    {
                        writer.Write(value);
                        continue;
                    }
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
                    cWriter.WriteClassInitializationInternal(field.FieldType, field.FieldType, typeOfRuntimeTypeInfo, false);
                }

                comma = true;
            }

            writer.Write(" }");
        }
    }
}
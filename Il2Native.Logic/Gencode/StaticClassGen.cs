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
        public static void WriteDefualtStructInitialization(this CIndentedTextWriter writer, bool empty = false)
        {
            writer.Write(empty ? "{}" : "{ 0 }");
        }

        public static void WriteVirtualTableImplementationReference(this CWriter cWriter, IType tokenType)
        {
            var vtName = tokenType.InterfaceOwner != null
                ? tokenType.InterfaceOwner.GetVirtualInterfaceTableNameReference(tokenType, cWriter)
                : tokenType.GetVirtualTableNameReference(cWriter);
            cWriter.Output.Write(vtName);
        }

        public static void WriteClassInitialization(this CWriter cWriter, IType fieldType, object valueInstance)
        {
            cWriter.WriteClassInitializationInternal(fieldType, fieldType, valueInstance);
        }

        public static void WriteNestedClassInitializations(this CWriter cWriter, IType fieldType, object valueInstance)
        {
            cWriter.WriteNestedClassInitializationsInternal(fieldType, fieldType, valueInstance);
        }

        private static void WriteClassInitializationInternal(this CWriter cWriter, IType type, IType instanceType, object valueInstance, bool withBase = true)
        {
            var isRuntimeTypeInfo = instanceType.TypeEquals(cWriter.System.System_RuntimeType);

            var comma = false;

            var writer = cWriter.Output;

            writer.Write("{ ");

            if (withBase && type.BaseType != null)
            {
                WriteClassInitializationInternal(cWriter, type.BaseType, instanceType, valueInstance);
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
                    var value = RuntimeTypeInfoGen.GetRuntimeTypeInfo(field, valueInstance as IType, cWriter);
                    if (value != null)
                    {
                        if (value is string)
                        {
                            cWriter.WriteUnicodeStringReference(Math.Abs(field.GetHashCode()), (uint)value.GetHashCode());
                        }
                        else
                        {
                            writer.Write(value);
                        }

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
                    cWriter.WriteClassInitializationInternal(field.FieldType, field.FieldType, valueInstance, false);
                }

                comma = true;
            }

            writer.Write(" }");
        }

        private static void WriteNestedClassInitializationsInternal(this CWriter cWriter, IType type, IType instanceType, object valueInstance, bool withBase = true)
        {
            var isRuntimeTypeInfo = instanceType.TypeEquals(cWriter.System.System_RuntimeType);

            if (withBase && type.BaseType != null)
            {
                WriteNestedClassInitializationsInternal(cWriter, type.BaseType, instanceType, valueInstance);
            }

            foreach (var field in IlReader.Fields(type, cWriter).Where(f => !f.IsStatic))
            {
                // to support runtimeType info
                if (isRuntimeTypeInfo)
                {
                    var value = RuntimeTypeInfoGen.GetRuntimeTypeInfo(field, valueInstance as IType, cWriter);
                    if (value is string)
                    {
                        cWriter.WriteUnicodeString(new KeyValuePair<int, string>(Math.Abs(field.GetHashCode()), value as string));
                        continue;
                    }
                }

                if (field.FieldType.IsStructureType())
                {
                    cWriter.WriteNestedClassInitializationsInternal(field.FieldType, field.FieldType, valueInstance, false);
                }
            }
        }
    }
}
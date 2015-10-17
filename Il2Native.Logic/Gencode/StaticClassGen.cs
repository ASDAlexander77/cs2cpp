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
    public static class StaticClassGen
    {
        public static void WriteDefaultStructInitialization(this CIndentedTextWriter writer, bool isEmpty = false)
        {
            if (isEmpty)
            {
                writer.Write("{}");
            }
            else
            {
                writer.Write("{ 0 }");
            }
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
            var writer = cWriter.Output;

            writer.Write("{");
            if (cWriter.MultiThreadingSupport)
            {
                writer.Write(" (Byte*) -1, (Byte*) -1, ");
            }

            cWriter.WriteClassInitializationInternal(fieldType, fieldType, valueInstance);

            writer.Write("}");
        }

        public static void WriteNestedClassInitializations(this CWriter cWriter, IType fieldType, object valueInstance)
        {
            cWriter.WriteNestedClassInitializationsInternal(fieldType, fieldType, valueInstance);
        }

        public static void WriteUnicodeStringReference(this CWriter cWriter, int stringToken, uint stringHashCode)
        {
            var stringType = cWriter.System.System_String;
            var strType = cWriter.WriteToString(() => stringType.WriteTypePrefix(cWriter));

            var writer = cWriter.Output;

            if (cWriter.MultiThreadingSupport)
            {
                // shift for Mutex & Cond
                writer.Write(
                    "(({1}) ((Byte**) &_s{0}{2}_ + 2))",
                    stringToken,
                    strType,
                    stringHashCode);
            }
            else
            {
                writer.Write("(({1}) &_s{0}{2}_)", stringToken, strType, stringHashCode);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="pair">
        /// </param>
        public static void WriteUnicodeString(this CWriter cWriter, KeyValuePair<int, string> pair)
        {
            if (!cWriter.stringTokenDefinitionWritten.Add(pair.Key * pair.Value.GetHashCode()))
            {
                return;
            }

            var align = pair.Value.Length % 2 == 0;

            var writer = cWriter.Output;

            writer.Write(
                "__static_str<{2}> _s{0}{1}_ = {4} {3}",
                pair.Key,
                (uint)pair.Value.GetHashCode(),
                pair.Value.Length + (align ? 2 : 1),
                cWriter.GetStringValuesHeader(pair.Value.Length + (align ? 3 : 2), pair.Value.Length),
                "{");

            writer.Write("L\"");
            foreach (var c in pair.Value.ToCharArray())
            {
                if (Char.IsLetterOrDigit(c) || c == ' ' || (Char.IsPunctuation(c) && c != '\\' && c != '"'))
                {
                    writer.Write(c);
                }
                else
                {
                    writer.Write("\\x{0:X}", (uint)c);
                }
            }

            if (align)
            {
                writer.Write("\\0");
            }

            writer.Write("\"");

            writer.WriteLine(" {0};", '}');
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
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class TypeGen
    {
        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> SystemTypeSizes = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, int> sizeByType = new SortedDictionary<string, int>();

        /// <summary>
        /// </summary>
        static TypeGen()
        {
            SystemTypeSizes["Void"] = 0;
            SystemTypeSizes["Byte"] = 1;
            SystemTypeSizes["SByte"] = 1;
            SystemTypeSizes["Char"] = 2;
            SystemTypeSizes["Int16"] = 2;
            SystemTypeSizes["Int32"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Int64"] = 8;
            SystemTypeSizes["UInt16"] = 2;
            SystemTypeSizes["UInt32"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UInt64"] = 8;
            SystemTypeSizes["Float"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Single"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Double"] = 8;
            SystemTypeSizes["Boolean"] = 1;
            SystemTypeSizes["Byte&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["SByte&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Char&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Int16&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Int32&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Int64&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["IntPtr"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UIntPtr"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UInt16&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UInt32&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["UInt64&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Float&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Single&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Double&"] = LlvmWriter.pointerSize;
            SystemTypeSizes["Boolean&"] = LlvmWriter.pointerSize;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetTypeSize(this IType type)
        {
            // find index
            int size;
            if (!sizeByType.TryGetValue(type.FullName, out size))
            {
                size = type.CalculateSize();
            }

            return size;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static int CalculateSize(this IType type)
        {
            if (type.IsInterface)
            {
                // i8** (...)
                return LlvmWriter.pointerSize;
            }

            if (type.IsEnum)
            {
                return type.GetEnumUnderlyingType().GetTypeSize();
            }

            var size = 0;

            // add shift for virtual table
            if (type.IsRootOfVirtualTable())
            {
                size += LlvmWriter.pointerSize;
            }

            if (type.BaseType != null)
            {
                size += type.BaseType.GetTypeSize();
            }

            // add shift for interfaces
            if (type.BaseType == null)
            {
                size += type.GetInterfaces().Count() * LlvmWriter.pointerSize;
            }
            else
            {
                var baseInterfaces = type.BaseType.GetInterfaces();
                size += type.GetInterfaces().Count(i => !baseInterfaces.Contains(i)) * LlvmWriter.pointerSize;
            }

            foreach (var field in IlReader.Fields(type).Where(t => !t.IsStatic).ToList())
            {
                if (field.FieldType.IsStructureType())
                {
                    size += field.FieldType.GetTypeSize();
                }

                var fieldSize = 0;
                if (field.FieldType.IsClass)
                {
                    // pointer size
                    size += LlvmWriter.pointerSize;
                }
                else if (field.FieldType.Namespace == "System" && SystemTypeSizes.TryGetValue(field.FieldType.Name, out fieldSize))
                {
                    size += fieldSize;
                }
                else
                {
                    size += field.FieldType.GetTypeSize();
                }
            }

            sizeByType[type.FullName] = size;

            return size;
        }
    }
}
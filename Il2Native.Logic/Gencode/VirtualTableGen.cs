// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VirtualTableGen.cs" company="">
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
    using System.Reflection;
    using PEAssemblyReader;
    using SynthesizedMethods;

    /// <summary>
    /// </summary>
    public static class VirtualTableGen
    {
        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, List<CWriter.Pair<IMethod, IMethod>>> VirtualInterfaceTableByType = 
            new SortedDictionary<string, List<CWriter.Pair<IMethod, IMethod>>>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, List<IMethod>> VirtualInterfaceTableLayoutByType =
            new SortedDictionary<string, List<IMethod>>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, List<CWriter.Pair<IMethod, IMethod>>> VirtualTableByType =
            new SortedDictionary<string, List<CWriter.Pair<IMethod, IMethod>>>();

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="interface">
        /// </param>
        public static void BuildVirtualInterfaceTable(
            this List<CWriter.Pair<IMethod, IMethod>> virtualTable,
            IType thisType,
            IType @interface,
            ITypeResolver typeResolver)
        {
            var allPublic = IlReader.Methods(
                thisType,
                BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance,
                typeResolver);
            virtualTable.AddMethodsToVirtualInterfaceTable(@interface, allPublic, typeResolver);
        }

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void BuildVirtualTable(
            this List<CWriter.Pair<IMethod, IMethod>> virtualTable,
            IType thisType,
            ITypeResolver typeResolver)
        {
            if (thisType.BaseType != null)
            {
                virtualTable.BuildVirtualTable(thisType.BaseType, typeResolver);
            }

            // get all virtual methods in current type and replace or append
            foreach (
                var virtualOrAbstractMethod in
                    IlReader.Methods(thisType, typeResolver).Where(m => m.IsVirtual || m.IsAbstract || m.IsOverride))
            {
                Debug.Assert(!virtualOrAbstractMethod.IsGenericMethodDefinition);
                Debug.Assert(!virtualOrAbstractMethod.DeclaringType.IsGenericTypeDefinition);

                if (virtualOrAbstractMethod.IsAbstract && virtualOrAbstractMethod.DeclaringType.Equals(thisType))
                {
                    virtualTable.Add(
                        new CWriter.Pair<IMethod, IMethod>
                        {
                            Key = virtualOrAbstractMethod,
                            Value = virtualOrAbstractMethod
                        });
                    continue;
                }

                // find method in virtual table
                var baseMethod = virtualOrAbstractMethod.IsOverride
                    ? virtualTable.First(m => m.Key.IsMatchingOverride(virtualOrAbstractMethod))
                    : virtualTable.FirstOrDefault(m => m.Key.IsMatchingOverride(virtualOrAbstractMethod));

                if (baseMethod == null)
                {
                    virtualTable.Add(
                        new CWriter.Pair<IMethod, IMethod>
                        {
                            Key = virtualOrAbstractMethod,
                            Value = virtualOrAbstractMethod
                        });
                    continue;
                }

                baseMethod.Value = virtualOrAbstractMethod;
            }
        }

        /// <summary>
        /// </summary>
        public static void Clear()
        {
            VirtualTableByType.Clear();
            VirtualInterfaceTableByType.Clear();
            VirtualInterfaceTableLayoutByType.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<CWriter.Pair<IMethod, IMethod>> GetVirtualInterfaceTable(
            this IType thisType,
            IType @interface,
            ITypeResolver typeResolver)
        {
            List<CWriter.Pair<IMethod, IMethod>> virtualInterfaceTable;

            var key = string.Concat(thisType.FullName, '+', @interface.FullName);
            if (VirtualInterfaceTableByType.TryGetValue(key, out virtualInterfaceTable))
            {
                return virtualInterfaceTable;
            }

            virtualInterfaceTable = new List<CWriter.Pair<IMethod, IMethod>>();
            virtualInterfaceTable.BuildVirtualInterfaceTable(thisType, @interface, typeResolver);

            VirtualInterfaceTableByType[key] = virtualInterfaceTable;

            return virtualInterfaceTable;
        }

        /// <summary>
        /// </summary>
        /// <param name="interface">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<IMethod> GetVirtualInterfaceTableLayout(this IType @interface, ITypeResolver typeResolver)
        {
            List<IMethod> virtualInterfaceTableLayout;

            var key = string.Concat(@interface.FullName, '+', @interface.FullName);
            if (VirtualInterfaceTableLayoutByType.TryGetValue(key, out virtualInterfaceTableLayout))
            {
                return virtualInterfaceTableLayout;
            }

            virtualInterfaceTableLayout = new List<IMethod>();
            virtualInterfaceTableLayout.AddMethodsToVirtualInterfaceTableLayout(@interface, typeResolver);

            VirtualInterfaceTableLayoutByType[key] = virtualInterfaceTableLayout;

            return virtualInterfaceTableLayout;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetVirtualInterfaceTableName(this IType type, IType @interface)
        {
            return string.Concat(type.FullName, " vtable ", @interface.FullName, " interface").CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <param name="requiredInterface">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="KeyNotFoundException">
        /// </exception>
        public static int GetVirtualMethodIndex(
            this IType thisType,
            IMethod methodInfo,
            CWriter cWriter,
            out IType requiredInterface)
        {
            requiredInterface = null;

            if (!thisType.IsInterface)
            {
                var virtualTable = thisType.GetVirtualTable(cWriter);

                var index = 0;
                foreach (var virtualMethod in virtualTable.Select(v => v.Value))
                {
                    if (virtualMethod.IsMatchingOverride(methodInfo))
                    {
                        // + RTTI info shift
                        return index;
                    }

                    index++;
                }

                throw new KeyNotFoundException("virtual method could not be found");
            }
            else
            {
                var virtualTable = thisType.GetVirtualInterfaceTableLayout(cWriter);

                var index = 0;
                foreach (var virtualMethod in virtualTable)
                {
                    if (virtualMethod.IsMatchingOverride(methodInfo))
                    {
                        // + RTTI info shift
                        return index;
                    }

                    index++;
                }

                // try to find a method in all interfaces
                foreach (var @interface in thisType.SelectAllTopAndAllNotFirstChildrenInterfaces().Skip(1))
                {
                    var virtualTableOfSecondaryInterface = @interface.GetVirtualInterfaceTableLayout(cWriter);

                    index = 0;
                    foreach (var virtualMethod in virtualTableOfSecondaryInterface)
                    {
                        if (virtualMethod.IsMatchingOverride(methodInfo))
                        {
                            requiredInterface = @interface;

                            // + RTTI info shift
                            return index;
                        }

                        index++;
                    }
                }

                throw new KeyNotFoundException("virtual method could not be found");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<CWriter.Pair<IMethod, IMethod>> GetVirtualTable(
            this IType thisType,
            ITypeResolver typeResolver)
        {
            List<CWriter.Pair<IMethod, IMethod>> virtualTable;

            if (VirtualTableByType.TryGetValue(thisType.FullName, out virtualTable))
            {
                return virtualTable;
            }

            virtualTable = new List<CWriter.Pair<IMethod, IMethod>>();
            virtualTable.BuildVirtualTable(thisType, typeResolver);

            VirtualTableByType[thisType.FullName] = virtualTable;

            return virtualTable;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetVirtualTableName(this IType type)
        {
            return string.Concat(type.FullName, " vtable").CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetVirtualTableSize(this List<CWriter.Pair<IMethod, IMethod>> virtualTable)
        {
            return virtualTable.Count + 2;
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasExplicitInterfaceMethodOverride(this IType thisType, IMethod methodInfo)
        {
            return thisType.GetMethods(BindingFlags.Instance).Any(methodInfo.IsMatchingExplicitInterfaceOverride);
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasVirtualMethod(this IType thisType, IMethod methodInfo, CWriter cWriter)
        {
            return
                thisType.GetVirtualTable(cWriter)
                    .Select(v => v.Value)
                    .Any(virtualMethod => virtualMethod.IsMatchingOverride(methodInfo));
        }

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="interfaceIndex">
        /// </param>
        /// <param name="baseTypeFieldsOffset">
        /// </param>
        public static void WriteTableOfMethods(
            this List<CWriter.Pair<IMethod, IMethod>> virtualTable,
            CWriter cWriter,
            IType type,
            int interfaceIndex,
            int baseTypeFieldsOffset)
        {
            var writer = cWriter.Output;

            VirtualTableCName(cWriter, type);
            VirtualTableDeclaration(virtualTable, cWriter);
            cWriter.Output.Write(" ");
            writer.Write(type.GetVirtualTableName());
            cWriter.Output.Write(" = ");
            VirtualTableDefinition(virtualTable, cWriter, interfaceIndex, baseTypeFieldsOffset);
            cWriter.Output.Write(";");
        }

        private static void VirtualTableCName(CWriter cWriter, IType type)
        {
            var writer = cWriter.Output;

            writer.Write("static struct ");
        }

        private static void VirtualTableDeclaration(List<CWriter.Pair<IMethod, IMethod>> virtualTable, CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.WriteLine("{");
            writer.Indent++;
            writer.WriteLine("i8* thisOffset;");

            // RTTI info class
            writer.WriteLine("i8* rttiInfo;");

            // define virtual table
            foreach (var virtualMethod in virtualTable)
            {
                var method = virtualMethod.Key;

                // write pointer to method
                cWriter.WriteMethodReturnType(writer, method);
                writer.Write("(*");
                cWriter.WriteMethodDefinitionName(writer, method, shortName: true);
                writer.Write(")");
                cWriter.WriteMethodParamsDef(writer, method, true, method.DeclaringType, method.ReturnType, true);

                // write method pointer
                writer.WriteLine(";");
            }

            writer.Indent--;

            writer.Write("}");
        }

        private static void VirtualTableDefinition(
            List<CWriter.Pair<IMethod, IMethod>> virtualTable, CWriter cWriter, int interfaceIndex, int baseTypeFieldsOffset)
        {
            var writer = cWriter.Output;

            writer.WriteLine("{");

            writer.Indent++;
            writer.WriteLine(
                "(i8*) {0},",
                interfaceIndex == 0
                    ? "0"
                    : string.Format("-{0}", baseTypeFieldsOffset + ((interfaceIndex - 1) * CWriter.PointerSize)));

            // RTTI info class
            //writer.Write("(i8*) &{0}", type.GetRttiInfoName().CleanUpName());
            writer.Write("(i8*) 0");

            // define virtual table
            foreach (var virtualMethod in virtualTable)
            {
                writer.WriteLine(",");

                var methodKey = virtualMethod.Key;
                var method = virtualMethod.Value;

                writer.Write("(");
                cWriter.WriteMethodReturnType(writer, methodKey);
                writer.Write("(*)");
                cWriter.WriteMethodParamsDef(writer, methodKey, true, methodKey.DeclaringType, methodKey.ReturnType, true);
                writer.Write(")");
                writer.Write(" ");

                if (method == null || virtualMethod.Value.IsAbstract)
                {
                    writer.Write("&__cxa_pure_virtual()");
                }
                else
                {
                    // write pointer to method
                    writer.Write("&");
                    cWriter.WriteMethodDefinitionName(writer, method);
                }
            }

            writer.WriteLine(string.Empty);
            writer.Indent--;
            writer.Write("}");
        }

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <param name="allPublic">
        /// </param>
        private static void AddMethodsToVirtualInterfaceTable(
            this List<CWriter.Pair<IMethod, IMethod>> virtualTable,
            IType @interface,
            IEnumerable<IMethod> allPublic,
            ITypeResolver typeResolver)
        {
            var allInterfaces = @interface.GetInterfaces();
            var firstChildInterface = allInterfaces != null ? allInterfaces.FirstOrDefault() : null;
            if (firstChildInterface != null)
            {
                // get all virtual methods in current type and replace or append
                virtualTable.AddMethodsToVirtualInterfaceTable(firstChildInterface, allPublic, typeResolver);
            }

            // get all virtual methods in current type and replace or append
#if DEBUG
            var interfaceMethods = IlReader.Methods(@interface, typeResolver).ToList();
#else
            var interfaceMethods = IlReader.Methods(@interface, typeResolver);
#endif
            var list =
                interfaceMethods.Select(
                    interfaceMember =>
                    new CWriter.Pair<IMethod, IMethod>
                        {
                            Key = interfaceMember,
                            Value = allPublic.Where(interfaceMember.IsMatchingInterfaceOverride)
                                         .OrderByDescending(x => x.IsExplicitInterfaceImplementation)
                                         .FirstOrDefault()
                        }).ToList();

            ////Debug.Assert(list.All(i => i.Value != null), "Not all method could be resolved");

            virtualTable.AddRange(list);
        }

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <param name="interface">
        /// </param>
        private static void AddMethodsToVirtualInterfaceTableLayout(this List<IMethod> virtualTable, IType @interface, ITypeResolver typeResolver)
        {
            var allInterfaces = @interface.GetInterfaces();
            var firstChildInterface = allInterfaces != null ? allInterfaces.FirstOrDefault() : null;
            if (firstChildInterface != null)
            {
                // get all virtual methods in current type and replace or append
                virtualTable.AddMethodsToVirtualInterfaceTableLayout(firstChildInterface, typeResolver);
            }

            // get all virtual methods in current type and replace or append
            virtualTable.AddRange(IlReader.Methods(@interface, typeResolver));
        }
    }
}
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
        private static readonly IDictionary<string, List<LlvmWriter.Pair<IMethod, IMethod>>> virtualInterfaceTableByType
            =
            new SortedDictionary<string, List<LlvmWriter.Pair<IMethod, IMethod>>>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, List<IMethod>> virtualInterfaceTableLayoutByType =
            new SortedDictionary<string, List<IMethod>>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, List<LlvmWriter.Pair<IMethod, IMethod>>> virtualTableByType =
            new SortedDictionary<string, List<LlvmWriter.Pair<IMethod, IMethod>>>();

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <param name="thisType">
        /// </param>
        /// <param name="interface">
        /// </param>
        public static void BuildVirtualInterfaceTable(
            this List<LlvmWriter.Pair<IMethod, IMethod>> virtualTable,
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
        /// <param name="interface">
        /// </param>
        public static void BuildVirtualInterfaceTableLayout(this List<IMethod> virtualTable, IType @interface, ITypeResolver typeResolver)
        {
            virtualTable.AddMethodsToVirtualInterfaceTableLayout(@interface, typeResolver);
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
            this List<LlvmWriter.Pair<IMethod, IMethod>> virtualTable,
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
                        new LlvmWriter.Pair<IMethod, IMethod>
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
                        new LlvmWriter.Pair<IMethod, IMethod>
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
            virtualTableByType.Clear();
            virtualInterfaceTableByType.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<LlvmWriter.Pair<IMethod, IMethod>> GetVirtualInterfaceTable(
            this IType thisType,
            IType @interface,
            ITypeResolver typeResolver)
        {
            List<LlvmWriter.Pair<IMethod, IMethod>> virtualInterfaceTable;

            if (virtualInterfaceTableByType.TryGetValue(
                string.Concat(thisType.FullName, '+', @interface.FullName),
                out virtualInterfaceTable))
            {
                return virtualInterfaceTable;
            }

            virtualInterfaceTable = new List<LlvmWriter.Pair<IMethod, IMethod>>();
            virtualInterfaceTable.BuildVirtualInterfaceTable(thisType, @interface, typeResolver);

            virtualInterfaceTableByType[thisType.FullName] = virtualInterfaceTable;

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

            if (
                virtualInterfaceTableLayoutByType.TryGetValue(
                    string.Concat(@interface.FullName, '+', @interface.FullName),
                    out virtualInterfaceTableLayout))
            {
                return virtualInterfaceTableLayout;
            }

            virtualInterfaceTableLayout = new List<IMethod>();
            virtualInterfaceTableLayout.BuildVirtualInterfaceTableLayout(@interface, typeResolver);

            virtualInterfaceTableLayoutByType[@interface.FullName] = virtualInterfaceTableLayout;

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
            return string.Concat("@\"", type.FullName, " Virtual Table ", @interface.FullName, " Interface\"");
        }

        /// <summary>
        /// </summary>
        /// <param name="thisType">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="llvmWriter">
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
            LlvmWriter llvmWriter,
            out IType requiredInterface)
        {
            requiredInterface = null;

            if (!thisType.IsInterface)
            {
                var virtualTable = thisType.GetVirtualTable(llvmWriter);

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
                var virtualTable = thisType.GetVirtualInterfaceTableLayout(llvmWriter);

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
                    var virtualTableOfSecondaryInterface = @interface.GetVirtualInterfaceTableLayout(llvmWriter);

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
        /// <param name="llvmWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static List<LlvmWriter.Pair<IMethod, IMethod>> GetVirtualTable(
            this IType thisType,
            LlvmWriter llvmWriter)
        {
            List<LlvmWriter.Pair<IMethod, IMethod>> virtualTable;

            if (virtualTableByType.TryGetValue(thisType.FullName, out virtualTable))
            {
                return virtualTable;
            }

            virtualTable = new List<LlvmWriter.Pair<IMethod, IMethod>>();
            virtualTable.BuildVirtualTable(thisType, llvmWriter);

            virtualTableByType[thisType.FullName] = virtualTable;

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
            return string.Concat("@\"", type.FullName, " Virtual Table\"");
        }

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <returns>
        /// </returns>
        public static int GetVirtualTableSize(this List<LlvmWriter.Pair<IMethod, IMethod>> virtualTable)
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
        /// <param name="llvmWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool HasVirtualMethod(this IType thisType, IMethod methodInfo, LlvmWriter llvmWriter)
        {
            return
                thisType.GetVirtualTable(llvmWriter)
                    .Select(v => v.Value)
                    .Any(virtualMethod => virtualMethod.IsMatchingOverride(methodInfo));
        }

        /// <summary>
        /// </summary>
        /// <param name="virtualTable">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="interfaceIndex">
        /// </param>
        /// <param name="baseTypeFieldsOffset">
        /// </param>
        public static void WriteTableOfMethods(
            this List<LlvmWriter.Pair<IMethod, IMethod>> virtualTable,
            LlvmWriter llvmWriter,
            IType type,
            int interfaceIndex,
            int baseTypeFieldsOffset)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(" = linkonce_odr constant [{0} x i8*] [", virtualTable.GetVirtualTableSize());

            writer.Indent++;
            writer.WriteLine(
                "i8* {0},",
                interfaceIndex == 0
                    ? "null"
                    : string.Format(
                        "inttoptr (i32 -{0} to i8*)",
                        baseTypeFieldsOffset + ((interfaceIndex - 1) * LlvmWriter.PointerSize)));

            // RTTI info class
            writer.Write("i8* bitcast (");
            type.WriteRttiClassInfoDeclaration(writer);
            writer.Write("* @\"{0}\" to i8*)", type.GetRttiInfoName());

            // define virtual table
            foreach (var virtualMethod in virtualTable)
            {
                var method = virtualMethod.Value;

                // write method pointer
                writer.WriteLine(",");

                writer.Write("i8* bitcast (");
                if (virtualMethod.Value == null || virtualMethod.Value.IsAbstract)
                {
                    writer.Write("void ()* @__cxa_pure_virtual");
                }
                else
                {
                    // write pointer to method
                    llvmWriter.WriteMethodReturnType(writer, method);
                    llvmWriter.WriteMethodParamsDef(writer, method, true, method.DeclaringType, method.ReturnType, true);
                    writer.Write("* ");
                    llvmWriter.WriteMethodDefinitionName(writer, method);

                    llvmWriter.CheckIfMethodExternalDeclarationIsRequired(method);
                }

                writer.Write(" to i8*)");
            }

            writer.WriteLine(string.Empty);
            writer.Indent--;
            writer.WriteLine("]");
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
            this List<LlvmWriter.Pair<IMethod, IMethod>> virtualTable,
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
                    allPublic.Where(interfaceMember.IsMatchingInterfaceOverride).OrderByDescending(x => x.IsExplicitInterfaceImplementation).FirstOrDefault())
                                .Select(foundMethod => new LlvmWriter.Pair<IMethod, IMethod> { Key = foundMethod, Value = foundMethod })
                                .ToList();

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
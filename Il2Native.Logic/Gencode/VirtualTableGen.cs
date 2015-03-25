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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using PEAssemblyReader;

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
            else if (thisType.IsInterface)
            {
                virtualTable.BuildVirtualTable(typeResolver.System.System_Object, typeResolver);
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
        public static string GetVirtualInterfaceTableName(this IType type, IType @interface, CWriter cWriter, bool implementation = false)
        {
            return string.Concat(cWriter.GetAssemblyPrefix(type), type.FullName, " vtable ", @interface.FullName, implementation ? " interface_impl"  : " interface").CleanUpName();
        }

        public static string GetVirtualInterfaceTableNameReference(this IType type, IType @interface, CWriter cWriter)
        {
            return string.Concat("(Byte*) (((Byte**) &", GetVirtualInterfaceTableName(type, @interface, cWriter, true), ") + 2)");
        }

        public static void WriteVirtualInterfaceTableNameReferenceDeclaration(this IType type, IType @interface, CWriter cWriter)
        {
            cWriter.Output.Write(cWriter.declarationPrefix);
            cWriter.Output.Write("struct {} ");
            cWriter.Output.Write(GetVirtualInterfaceTableName(type, @interface, cWriter, true));
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
        public static int GetVirtualMethodIndexAndRequiredInterface(
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
        public static string GetVirtualTableName(this IType type, CWriter cWriter, bool implementation = false)
        {
            return string.Concat(cWriter.GetAssemblyPrefix(type), type.FullName, implementation ? " vtable_impl" : " vtable").CleanUpName();
        }

        public static string GetVirtualTableNameReference(this IType type, CWriter cWriter)
        {
            return string.Concat("(Byte*) (((Byte**) &", GetVirtualTableName(type, cWriter, true), ") + 2)");
        }

        public static void WriteVirtualTableEmptyImplementationDeclarations(this IType type, CWriter cWriter)
        {
            var write = cWriter.Output;

            type.GetVirtualTable(cWriter).WriteVirtualTableEmptyImplementationDeclaration(cWriter, type);
            write.WriteLine(";");

            foreach (var @interface in type.SelectAllTopAndAllNotFirstChildrenInterfaces().Distinct())
            {
                type.GetVirtualInterfaceTable(@interface, cWriter).WriteVirtualTableEmptyImplementationDeclaration(cWriter, type, @interface);
                write.WriteLine(";");
            }           
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

        public static void WriteTableOfMethodsAsDeclaration(
            this List<CWriter.Pair<IMethod, IMethod>> virtualTable,
            CWriter cWriter,
            IType type)
        {
            var writer = cWriter.Output;

            writer.Write(cWriter.declarationPrefix);
            writer.Write("struct ");
            
            writer.Write(type.GetVirtualTableName(cWriter));
            writer.Write(" ");
            VirtualTableDeclaration(virtualTable, cWriter, true, type);
            cWriter.Output.Write(";");
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
        public static void WriteTableOfMethodsWithImplementation(
            this List<CWriter.Pair<IMethod, IMethod>> virtualTable,
            CWriter cWriter,
            IType type,
            int interfaceIndex,
            int baseTypeFieldsOffset,
            IType interfaceType = null)
        {
            WriteVirtualTableImplementationDeclaration(virtualTable, cWriter, type, interfaceType);
            cWriter.Output.Write(" = ");
            VirtualTableDefinition(virtualTable, type, cWriter, interfaceIndex, baseTypeFieldsOffset);
            cWriter.Output.Write(";");
        }

        public static void WriteVirtualTableImplementationDeclaration(
            this List<CWriter.Pair<IMethod, IMethod>> virtualTable,
            CWriter cWriter,
            IType type,
            IType interfaceType = null)
        {
            var writer = cWriter.Output;

            writer.Write(cWriter.declarationPrefix);
            writer.Write("const struct ");
            VirtualTableDeclaration(virtualTable, cWriter);
            cWriter.Output.Write(" ");
            if (interfaceType == null)
            {
                writer.Write(type.GetVirtualTableName(cWriter, true));
            }
            else
            {
                writer.Write(type.GetVirtualInterfaceTableName(interfaceType, cWriter, true));
            }
        }

        public static void WriteVirtualTableEmptyImplementationDeclaration(
            this List<CWriter.Pair<IMethod, IMethod>> virtualTable,
            CWriter cWriter,
            IType type,
            IType interfaceType = null)
        {
            var writer = cWriter.Output;

            writer.Write(cWriter.declarationPrefix);
            writer.Write("const struct {}");
            cWriter.Output.Write(" ");
            if (interfaceType == null)
            {
                writer.Write(type.GetVirtualTableName(cWriter, true));
            }
            else
            {
                writer.Write(type.GetVirtualInterfaceTableName(interfaceType, cWriter, true));
            }
        }

        private static void VirtualTableDeclaration(List<CWriter.Pair<IMethod, IMethod>> virtualTable, CWriter cWriter, bool methodsOnly = false, IType declarationType = null)
        {
            var writer = cWriter.Output;

            writer.WriteLine("{");
            writer.Indent++;

            if (!methodsOnly)
            {
                writer.WriteLine("Byte* thisOffset;");

                // RTTI info class
                writer.WriteLine("Byte* rttiInfo;");
            }

            // define virtual table
            foreach (var virtualMethod in virtualTable)
            {
                var method = virtualMethod.Key;

                // write pointer to method
                cWriter.WriteMethodReturnType(writer, method);
                writer.Write("(*");
                cWriter.WriteMethodDefinitionName(writer, method, shortName: methodsOnly);
                writer.Write(")");
                cWriter.WriteMethodParamsDef(writer, method, true, declarationType ?? method.DeclaringType, method.ReturnType, true);

                // write method pointer
                writer.WriteLine(";");
            }

            writer.Indent--;

            writer.Write("}");
        }

        private static void VirtualTableDefinition(
            List<CWriter.Pair<IMethod, IMethod>> virtualTable, IType type, CWriter cWriter, int interfaceIndex, int baseTypeFieldsOffset)
        {
            var writer = cWriter.Output;

            writer.WriteLine("{");

            writer.Indent++;
            writer.WriteLine(
                "(Byte*) {0},",
                interfaceIndex == 0
                    ? "0"
                    : string.Format("-{0}", baseTypeFieldsOffset + ((interfaceIndex - 1) * CWriter.PointerSize)));

            // RTTI info class
            writer.Write("(Byte*) &{0}", type.GetRttiInfoName(cWriter).CleanUpName());

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
                    writer.Write("&__cxa_pure_virtual");
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
            var baseInterfaces = @interface.GetInterfaces();
            if (!baseInterfaces.Any() && @interface.BaseType == null)
            {
                // add method from Object to simulate inheritence from Object
#if DEBUG
                var objectInterfaceMethods = IlReader.Methods(typeResolver.System.System_Object, typeResolver).Where(m => m.IsVirtual || m.IsAbstract || m.IsOverride).ToList();
#else
                var objectInterfaceMethods = IlReader.Methods(typeResolver.System.System_Object, typeResolver).Where(m => m.IsVirtual || m.IsAbstract || m.IsOverride);
#endif

                ResolveAndAppendInterfaceMethods(virtualTable, objectInterfaceMethods, objectInterfaceMethods);
            }

            var firstChildInterface = baseInterfaces != null ? baseInterfaces.FirstOrDefault() : null;
            if (firstChildInterface != null)
            {
                // get all virtual methods in current type and replace or append
                virtualTable.AddMethodsToVirtualInterfaceTable(firstChildInterface, allPublic, typeResolver);
            }

            // get all virtual methods in current type and replace or append
#if DEBUG
            var interfaceMethods = IlReader.Methods(@interface, typeResolver).Where(m => !m.IsStatic).ToList();
#else
            var interfaceMethods = IlReader.Methods(@interface, typeResolver).Where(m => !m.IsStatic);
#endif
            ResolveAndAppendInterfaceMethods(virtualTable, allPublic, interfaceMethods);
        }

        private static void ResolveAndAppendInterfaceMethods(List<CWriter.Pair<IMethod, IMethod>> virtualTable, IEnumerable<IMethod> allPublic, List<IMethod> interfaceMethods)
        {
            var list =
                interfaceMethods.Select(
                    interfaceMember =>
                    new CWriter.Pair<IMethod, IMethod>
                        {
                            Key = interfaceMember,
                            Value =
                                allPublic.Where(interfaceMember.IsMatchingInterfaceOverride)
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
            var baseInterfaces = @interface.GetInterfaces();
            var firstChildInterface = baseInterfaces != null ? baseInterfaces.FirstOrDefault() : null;
            if (firstChildInterface == null)
            {
                // add Object virtual methods to simulate inheritance from Object type
                virtualTable.AddRange(
                    IlReader.Methods(typeResolver.System.System_Object, typeResolver)
                        .Where(m => m.IsVirtual || m.IsAbstract || m.IsOverride));
            }
            else
            {
                // get all virtual methods in current type and replace or append
                virtualTable.AddMethodsToVirtualInterfaceTableLayout(firstChildInterface, typeResolver);
            }

            // get all virtual methods in current type and replace or append
            virtualTable.AddRange(IlReader.Methods(@interface, typeResolver));
        }
    }
}
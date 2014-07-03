// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Il2Converter.cs" company="">
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class Il2Converter
    {
        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        /// <param name="args">
        /// </param>
        public static void Convert(string source, string outputFolder, string[] args = null)
        {
            var ilReader = new IlReader(source, args);
            ilReader.Load();
            GenerateLlvm(ilReader, Path.GetFileNameWithoutExtension(source), outputFolder, args);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        /// <param name="args">
        /// </param>
        public static void Convert(Type type, string outputFolder, string[] args = null)
        {
            var ilReader = new IlReader();
            ilReader.Load(type);
            var name = type.Module.Name.Replace(".dll", string.Empty);
            GenerateLlvm(ilReader, Path.GetFileNameWithoutExtension(name), outputFolder, args, new[] { type.FullName });
        }

        /// <summary>
        /// </summary>
        /// <param name="codeWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="genericDefinition">
        /// </param>
        /// <param name="disablePostDeclarations">
        /// </param>
        public static void WriteTypeDefinition(ICodeWriter codeWriter, IType type, IType genericDefinition, bool disablePostDeclarations = false)
        {
            codeWriter.WriteTypeStart(type, genericDefinition);

            var fields = IlReader.Fields(type);
            var count = fields.Count();
            var number = 1;

            codeWriter.WriteBeforeFields(count);

            foreach (var field in fields)
            {
                codeWriter.WriteFieldStart(field, number, count);
                codeWriter.WriteFieldEnd(field, number, count);

                number++;
            }

            codeWriter.WriteAfterFields(count, disablePostDeclarations);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="requiredITypesToAdd">
        /// </param>
        /// <param name="typesAdded">
        /// </param>
        private static void AddRequiredIType(IType type, List<IType> requiredITypesToAdd, HashSet<IType> typesAdded)
        {
            var effectiveIType = type;
            while (effectiveIType.HasElementType)
            {
                effectiveIType = effectiveIType.GetElementType();
            }

            if (typesAdded.Contains(effectiveIType))
            {
                return;
            }

            typesAdded.Add(effectiveIType);
            requiredITypesToAdd.Add(effectiveIType);
        }

        private static void ConvertAllTypes(
            IlReader ilReader,
            string[] filter,
            ICodeWriter codeWriter,
            List<IType> newListOfITypes,
            SortedDictionary<string, IType> genDefinitionsByGuid,
            ConvertingMode mode)
        {
            foreach (var type in newListOfITypes)
            {
                if (filter != null && !filter.Contains(type.FullName))
                {
                    continue;
                }

                if (type.Name == "<Module>")
                {
                    continue;
                }

                if (!type.IsInterface && type.IsGenericTypeDefinition)
                {
                    ////foreach (var genericSpecialization in genericSpecializations.Where(g => g.GUID == type.GUID))
                    ////{
                    ////    ConvertIType(ilReader, codeWriter, genericSpecialization);
                    ////}
                    continue;
                }

                IType genDef = null;
                if (type.IsGenericType)
                {
                    genDefinitionsByGuid.TryGetValue(type.Name, out genDef);
                }

                type.UseAsClass = true;
                ConvertIType(ilReader, codeWriter, type, genDef, mode);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ilReader">
        /// </param>
        /// <param name="codeWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="genericDefinition">
        /// </param>
        private static void ConvertIType(IlReader ilReader, ICodeWriter codeWriter, IType type, IType genericDefinition, ConvertingMode mode)
        {
            if (mode == ConvertingMode.Declaration)
            {
                WriteTypeDefinition(codeWriter, type, genericDefinition);

                codeWriter.WriteBeforeConstructors();
            }

            if (mode == ConvertingMode.Definition)
            {
                foreach (var ctor in IlReader.Constructors(type))
                {
                    codeWriter.WriteConstructorStart(ctor);
                    foreach (var ilCode in ilReader.OpCodes(ctor, type.GetGenericArguments().ToArray(), null /*ctor.GetGenericArguments()*/))
                    {
                        codeWriter.Write(ilCode);
                    }

                    codeWriter.WriteConstructorEnd(ctor);
                }
            }

            if (mode == ConvertingMode.Declaration)
            {
                codeWriter.WriteAfterConstructors();
                codeWriter.WriteBeforeMethods();
            }

            if (mode == ConvertingMode.Definition)
            {
                foreach (var method in IlReader.Methods(type))
                {
                    codeWriter.WriteMethodStart(method);
                    foreach (var ilCode in ilReader.OpCodes(method, type.GetGenericArguments().ToArray(), method.GetGenericArguments().ToArray()))
                    {
                        codeWriter.Write(ilCode);
                    }

                    codeWriter.WriteMethodEnd(method);
                }
            }

            if (mode == ConvertingMode.Declaration)
            {
                codeWriter.WriteAfterMethods();
                codeWriter.WriteTypeEnd(type);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        private static void DicoverGenericSpecializedIType(IType type, HashSet<IType> genericSpecializations)
        {
            if (type == null || genericSpecializations == null)
            {
                return;
            }

            if (type.HasElementType)
            {
                DicoverGenericSpecializedIType(type.GetElementType(), genericSpecializations);
                return;
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition && !genericSpecializations.Contains(type) && !TypeHasGenericParameter(type)
                && !TypeHasGenericParameterInGenericArguments(type))
            {
                genericSpecializations.Add(type);

                // todo the same for base class and interfaces
                GetAllRequiredITypesForIType(type, genericSpecializations).ToArray();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="ilReader">
        /// </param>
        /// <param name="fileName">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        /// <param name="args">
        /// </param>
        /// <param name="filter">
        /// </param>
        private static void GenerateLlvm(IlReader ilReader, string fileName, string outputFolder, string[] args, string[] filter = null)
        {
            var codeWriter = GetLlvmWriter(fileName, outputFolder, args);
            GenerateSource(ilReader, filter, codeWriter);
        }

        /// <summary>
        /// </summary>
        /// <param name="ilReader">
        /// </param>
        /// <param name="filter">
        /// </param>
        /// <param name="codeWriter">
        /// </param>
        private static void GenerateSource(IlReader ilReader, string[] filter, ICodeWriter codeWriter)
        {
            codeWriter.WriteStart(ilReader.ModuleName, ilReader.AssemblyQualifiedName);

            var genericSpecializations = new HashSet<IType>();
            var newListOfITypes = ResortITypes(ilReader.Types().ToList(), genericSpecializations);

            // build quick access array for Generic Definitions
            var genDefinitionsByGuid = new SortedDictionary<string, IType>();
            foreach (var genDef in newListOfITypes.Where(t => t.IsGenericTypeDefinition))
            {
                genDefinitionsByGuid[genDef.Name] = genDef;
            }

            for (var index = 0; index < newListOfITypes.Count; index++)
            {
                var type = newListOfITypes[index];
                codeWriter.WriteForwardDeclaration(type, index, newListOfITypes.Count);
            }

            ConvertAllTypes(ilReader, filter, codeWriter, newListOfITypes, genDefinitionsByGuid, ConvertingMode.Declaration);
            ConvertAllTypes(ilReader, filter, codeWriter, newListOfITypes, genDefinitionsByGuid, ConvertingMode.Definition);

            codeWriter.WriteEnd();

            codeWriter.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="allITypes">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        /// <returns>
        /// </returns>
        private static IEnumerable<IType> GetAllRequiredITypesForIType(IType type, HashSet<IType> genericSpecializations)
        {
            // if (type.FullName == "System.Object")
            // {
            // yield break;
            // }
            if (type.BaseType != null && type.BaseType.FullName == "System.Enum")
            {
                yield break;
            }

            if (type.BaseType != null)
            {
                DicoverGenericSpecializedIType(type.BaseType, genericSpecializations);
                yield return type.BaseType;
            }

            var interfaces = type.GetInterfaces();
            if (interfaces != null)
            {
                foreach (var @interface in interfaces)
                {
                    DicoverGenericSpecializedIType(@interface, genericSpecializations);
                    yield return @interface;
                }
            }

            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (fields != null)
            {
                foreach (var field in fields)
                {
                    if (field.FieldType.IsStructureType() && !field.FieldType.IsPointer)
                    {
                        DicoverGenericSpecializedIType(field.FieldType, genericSpecializations);
                        yield return field.FieldType;
                    }
                }
            }

            var methods = type.GetMethods(
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (methods != null)
            {
                foreach (var method in methods)
                {
                    DicoverGenericSpecializedIType(method.ReturnType, genericSpecializations);

                    foreach (var param in method.GetParameters())
                    {
                        DicoverGenericSpecializedIType(param.ParameterType, genericSpecializations);
                    }

                    var methodBody = method.GetMethodBody();
                    if (methodBody != null)
                    {
                        foreach (var localVar in methodBody.LocalVariables)
                        {
                            if (localVar.LocalType.IsStructureType() && !localVar.LocalType.IsPointer)
                            {
                                DicoverGenericSpecializedIType(localVar.LocalType, genericSpecializations);
                                yield return localVar.LocalType;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        /// <param name="args">
        /// </param>
        /// <returns>
        /// </returns>
        private static ICodeWriter GetLlvmWriter(string fileName, string outputFolder, string[] args)
        {
            return new LlvmWriter(Path.Combine(outputFolder, fileName), args);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="allITypes">
        /// </param>
        /// <param name="typesAdded">
        /// </param>
        /// <param name="requiredITypesToAdd">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        private static void ProcessNextRequiredITypes(
            IType type, HashSet<IType> typesAdded, List<IType> requiredITypesToAdd, HashSet<IType> genericSpecializations)
        {
            var requiredITypes = GetAllRequiredITypesForIType(type, genericSpecializations).ToList();
            foreach (var requiredIType in requiredITypes)
            {
                if (type.TypeNotEquals(requiredIType))
                {
                    AddRequiredIType(requiredIType, requiredITypesToAdd, typesAdded);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="types">
        /// </param>
        /// <param name="allITypes">
        /// </param>
        /// <param name="typesAdded">
        /// </param>
        /// <param name="newListOfITypes">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        public static void ProcessRequiredITypesForITypes(
            IEnumerable<IType> types, HashSet<IType> typesAdded, List<IType> newListOfITypes, HashSet<IType> genericSpecializations)
        {
            foreach (var type in types)
            {
                var requiredITypesToAdd = new List<IType>();
                ProcessNextRequiredITypes(type, typesAdded, requiredITypesToAdd, genericSpecializations);
                newListOfITypes.AddRange(requiredITypesToAdd);

                if (!typesAdded.Contains(type))
                {
                    typesAdded.Add(type);
                    newListOfITypes.Add(type);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="types">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        /// <returns>
        /// </returns>
        private static List<IType> ResortITypes(List<IType> types, HashSet<IType> genericSpecializations)
        {
            var newOrder = new List<IType>();

            var typesWithRequired = new List<Tuple<IType, List<IType>>>();
            foreach (var type in types)
            {
                var requiredITypesToAdd = new List<IType>();
                ProcessNextRequiredITypes(type, new HashSet<IType>(), requiredITypesToAdd, genericSpecializations);
                typesWithRequired.Add(new Tuple<IType, List<IType>>(type, requiredITypesToAdd));
            }

            // the same for generic specialized types
            foreach (var type in genericSpecializations)
            {
                var requiredITypesToAdd = new List<IType>();
                ProcessNextRequiredITypes(type, new HashSet<IType>(), requiredITypesToAdd, null);
                typesWithRequired.Add(new Tuple<IType, List<IType>>(type, requiredITypesToAdd));
            }

            var strictMode = true;
            while (typesWithRequired.Count > 0)
            {
                var before = typesWithRequired.Count;
                var toRemove = new List<Tuple<IType, List<IType>>>();

                // step 1 find Root;
                foreach (var type in typesWithRequired)
                {
                    var requiredITypes = type.Item2;
                    requiredITypes.RemoveAll(r => newOrder.Any(n => n.TypeEquals(r)));
                    // remove not used types, for example System.Object which maybe not in current assembly
                    requiredITypes.RemoveAll(r => !types.Any(n => n.TypeEquals(r)));
                    if (requiredITypes.Count == 0)
                    {
                        toRemove.Add(type);
                        newOrder.Add(type.Item1);
                    }
                }

                foreach (var type in toRemove)
                {
                    typesWithRequired.Remove(type);
                }

                var after = typesWithRequired.Count;
                if (before == after)
                {
                    if (strictMode)
                    {
                        strictMode = false;
                        continue;
                    }

                    // throw new Exception("Can't resolve any types anymore");
                    foreach (var typeItems in typesWithRequired)
                    {
                        newOrder.Add(typeItems.Item1);
                    }

                    break;
                }
            }

            return newOrder;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool TypeHasGenericParameter(IType type)
        {
            return type.IsGenericParameter
                   || type.GenericTypeArguments.Any(
                       t =>
                       t.IsGenericParameter || t.IsGenericType && (t.IsGenericTypeDefinition || TypeHasGenericParameter(t))
                       || t.HasElementType && TypeHasGenericParameter(t.GetElementType()));
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool TypeHasGenericParameterInGenericArguments(IType type)
        {
            return type.IsGenericParameter
                   || type.GetGenericArguments()
                          .Any(
                              t =>
                              t.IsGenericParameter || t.ContainsGenericParameters && TypeHasGenericParameterInGenericArguments(t)
                              || t.HasElementType && TypeHasGenericParameterInGenericArguments(t.GetElementType()));
        }

        public enum ConvertingMode
        {
            Declaration,

            Definition
        }

        /// <summary>
        /// </summary>
        private class InheritanceITypeComparer : IComparer<IType>
        {
            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="baseIType">
            /// </param>
            /// <returns>
            /// </returns>
            public static bool DependsOn(IType type, IType baseIType)
            {
                if (type.BaseType != null)
                {
                    if (type.BaseType.TypeEquals(baseIType) || DependsOn(type.BaseType, baseIType))
                    {
                        return true;
                    }
                }

                return false;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="valueIType">
            /// </param>
            /// <returns>
            /// </returns>
            public static bool HasAsValueType(IType type, IType valueIType)
            {
                if (!valueIType.IsValueType)
                {
                    return false;
                }

                var fields =
                    type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                if (fields != null)
                {
                    foreach (var field in fields)
                    {
                        if (field.FieldType.TypeEquals(valueIType))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="baseInterface">
            /// </param>
            /// <returns>
            /// </returns>
            public static bool HasInterface(IType type, IType baseInterface)
            {
                var interfaces = type.GetInterfaces();
                if (interfaces != null)
                {
                    foreach (var @interface in interfaces)
                    {
                        if (@interface.TypeEquals(baseInterface) || HasInterface(@interface, baseInterface))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            /// <summary>
            /// </summary>
            /// <param name="t">
            /// </param>
            /// <returns>
            /// </returns>
            public static int InheritanceLevel(IType t)
            {
                if (t == null || t.BaseType == null)
                {
                    return 0;
                }

                return 1 + InheritanceLevel(t.BaseType);
            }

            /// <summary>
            /// </summary>
            /// <param name="x">
            /// </param>
            /// <param name="y">
            /// </param>
            /// <returns>
            /// </returns>
            public int Compare(IType x, IType y)
            {
                var lvlX = InheritanceLevel(x);
                var lvlY = InheritanceLevel(y);

                var cmp = lvlX.CompareTo(lvlY);
                if (cmp != 0)
                {
                    return cmp;
                }

                if (DependsOn(x, y) || HasInterface(x, y) || HasAsValueType(x, y))
                {
                    return 1;
                }

                if (DependsOn(y, x) || HasInterface(y, x) || HasAsValueType(y, x))
                {
                    return -1;
                }

                return x.FullName.CompareTo(y.FullName);
            }
        }
    }
}
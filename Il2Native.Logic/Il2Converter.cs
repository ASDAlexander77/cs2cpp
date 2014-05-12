
#define SPLIT
//#define NEST_TYPES
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// </summary>
    public class Il2Converter
    {
        #region Static Fields

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        public static void Convert(string source, string outputFolder)
        {
            var ilReader = new IlReader(source);
            ilReader.Load();
            GenerateLlvm(ilReader, Path.GetFileNameWithoutExtension(source), outputFolder);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        public static void Convert(Type type, string outputFolder)
        {
            var ilReader = new IlReader();
            ilReader.Load(type);
            var name = type.Module.Name.Replace(".dll", string.Empty);
            GenerateLlvm(ilReader, Path.GetFileNameWithoutExtension(name), outputFolder);
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="requiredTypesToAdd">
        /// </param>
        /// <param name="typesAdded">
        /// </param>
        private static void AddRequiredType(Type type, List<Type> requiredTypesToAdd, HashSet<Type> typesAdded)
        {
            var effectiveType = type;
            while (effectiveType.HasElementType)
            {
                effectiveType = effectiveType.GetElementType();
            }

            if (typesAdded.Contains(effectiveType))
            {
                return;
            }

            typesAdded.Add(effectiveType);
            requiredTypesToAdd.Add(effectiveType);
        }

        /// <summary>
        /// </summary>
        /// <param name="ilReader">
        /// </param>
        /// <param name="codeWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        private static void ConvertType(IlReader ilReader, ICodeWriter codeWriter, Type type, Type genericDefinition)
        {
            codeWriter.WriteTypeStart(type, genericDefinition);

#if NEST_TYPES
    
    // process nested types
            var nestedTypes = type.GetNestedTypes(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var nestedType in nestedTypes)
            {
                ConvertType(ilReader, codeWriter, nestedType);
            }
#endif

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

            codeWriter.WriteAfterFields(count);

            codeWriter.WriteBeforeConstructors();

            foreach (var ctor in IlReader.Constructors(type))
            {
                codeWriter.WriteConstructorStart(ctor);
                foreach (var ilCode in ilReader.OpCodes(ctor, type.GetGenericArguments(), null /*ctor.GetGenericArguments()*/))
                {
                    codeWriter.Write(ilCode);
                }

                codeWriter.WriteConstructorEnd(ctor);
            }

            codeWriter.WriteAfterConstructors();
            codeWriter.WriteBeforeMethods();

            foreach (var method in IlReader.Methods(type))
            {
                codeWriter.WriteMethodStart(method);
                foreach (var ilCode in ilReader.OpCodes(method, type.GetGenericArguments(), method.GetGenericArguments()))
                {
                    codeWriter.Write(ilCode);
                }

                codeWriter.WriteMethodEnd(method);
            }

            codeWriter.WriteAfterMethods();
            codeWriter.WriteTypeEnd(type);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        private static void DicoverGenericSpecializedType(Type type, HashSet<Type> genericSpecializations)
        {
            if (type == null || genericSpecializations == null)
            {
                return;
            }

            if (type.HasElementType)
            {
                DicoverGenericSpecializedType(type.GetElementType(), genericSpecializations);
                return;
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition && !genericSpecializations.Contains(type) && !TypeHasGenericParameter(type)
                && !TypeHasGenericParameterInGenericArguments(type))
            {
                genericSpecializations.Add(type);

                // todo the same for base class and interfaces
                GetAllRequiredTypesForType(type, null, genericSpecializations).ToArray();
            }
        }

        private static void GenerateLlvm(IlReader ilReader, string fileName, string outputFolder, Type[] filter = null)
        {
            var codeWriter = GetLlvmWriter(fileName, outputFolder);
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
        private static void GenerateSource(IlReader ilReader, Type[] filter, ICodeWriter codeWriter)
        {
            codeWriter.WriteStart(ilReader.ModuleName);

            var genericSpecializations = new HashSet<Type>();
            var newListOfTypes = ResortTypes(ilReader.Types().ToList(), genericSpecializations);

            // build quick access array for Generic Definitions
            var genDefinitionsByGuid = new SortedDictionary<string, Type>();
            foreach (var genDef in newListOfTypes.Where(t => t.IsGenericTypeDefinition))
            {
                genDefinitionsByGuid[genDef.Name] = genDef;
            }

            for (var index = 0; index < newListOfTypes.Count; index++)
            {
                var type = newListOfTypes[index];
                codeWriter.WriteForwardDeclaration(type, index, newListOfTypes.Count);
            }

            // enumarte all types
            foreach (var type in newListOfTypes)
            {
#if NEST_TYPES
                if (type.IsNested)
                {
                    continue;
                }
#endif

                if (filter != null && !filter.Contains(type))
                {
                    continue;
                }

                if (!type.IsInterface && type.IsGenericTypeDefinition)
                {
                    ////foreach (var genericSpecialization in genericSpecializations.Where(g => g.GUID == type.GUID))
                    ////{
                    ////    ConvertType(ilReader, codeWriter, genericSpecialization);
                    ////}
                    continue;
                }

                Type genDef = null;
                if (type.IsGenericType)
                {
                    genDefinitionsByGuid.TryGetValue(type.Name, out genDef);
                }

                ConvertType(ilReader, codeWriter, type, genDef);
            }

            codeWriter.WriteEnd();

            codeWriter.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="allTypes">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        /// <returns>
        /// </returns>
        private static IEnumerable<Type> GetAllRequiredTypesForType(Type type, List<Type> allTypes, HashSet<Type> genericSpecializations)
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
                DicoverGenericSpecializedType(type.BaseType, genericSpecializations);
                yield return type.BaseType;
            }

            var interfaces = type.GetInterfaces();
            if (interfaces != null)
            {
                foreach (var @interface in interfaces)
                {
                    DicoverGenericSpecializedType(@interface, genericSpecializations);
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
                        DicoverGenericSpecializedType(field.FieldType, genericSpecializations);
                        yield return field.FieldType;
                    }

#if NEST_TYPES
                    if (field.FieldType.IsNested)
                    {
                        yield return GetDeclType(field.FieldType);
                    }
#endif
                }
            }

            var methods = type.GetMethods(
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (methods != null)
            {
                foreach (var method in methods)
                {
                    DicoverGenericSpecializedType(method.ReturnType, genericSpecializations);
#if NEST_TYPES
                    if (method.ReturnType.IsNested)
                    {
                        yield return GetDeclType(method.ReturnType);
                    }
#endif

                    foreach (var param in method.GetParameters())
                    {
                        DicoverGenericSpecializedType(param.ParameterType, genericSpecializations);
#if NEST_TYPES
                        if (param.ParameterType.IsNested)
                        {
                            yield return GetDeclType(param.ParameterType);
                        }
#endif
                    }

                    var methodBody = method.GetMethodBody();
                    if (methodBody != null)
                    {
                        foreach (var localVar in methodBody.LocalVariables)
                        {
                            DicoverGenericSpecializedType(localVar.LocalType, genericSpecializations);
                        }
                    }
                }
            }

            ////if (type.IsGenericType && !type.IsGenericTypeDefinition)
            ////{
            ////    DicoverGenericSpecializedType(type, genericSpecializations);

            ////    // find definition and return as required type
            ////    foreach (var typeOne in allTypes)
            ////    {
            ////        if (typeOne.GUID == type.GUID && typeOne.IsGenericTypeDefinition)
            ////        {
            ////            yield return typeOne;
            ////            break;
            ////        }
            ////    }
            ////}
#if NEST_TYPES
            var nestedTypes = type.GetNestedTypes(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            foreach (var nestedType in nestedTypes)
            {
                foreach (var reqNestedType in GetAllRequiredTypesForType(nestedType, allTypes))
                {
                    var hasBase = reqNestedType.BaseType != null && reqNestedType.BaseType.GUID == type.GUID;
                    var hasDeclatingType = reqNestedType.DeclaringType != null && reqNestedType.DeclaringType.GUID == type.GUID;
                    if (!hasBase && !hasDeclatingType)
                    {
                        yield return reqNestedType;
                    }
                }
            }
#endif
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        private static Type GetDeclType(Type type)
        {
            return !type.DeclaringType.IsNested ? type.DeclaringType : GetDeclType(type.DeclaringType);
        }

        private static ICodeWriter GetLlvmWriter(string fileName, string outputFolder)
        {
            return new LlvmWriter(Path.Combine(outputFolder, fileName)) as ICodeWriter;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <returns>
        /// </returns>
        private static bool IsBelongToDeclaringType(Type type, Type declaringType)
        {
            return type != null
                   && (type.IsNested && type.DeclaringType.GUID == declaringType.GUID || IsBelongToDeclaringType(type.DeclaringType, declaringType));
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="allTypes">
        /// </param>
        /// <param name="typesAdded">
        /// </param>
        /// <param name="requiredTypesToAdd">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        private static void ProcessNextRequiredTypes(
            Type type, List<Type> allTypes, HashSet<Type> typesAdded, List<Type> requiredTypesToAdd, HashSet<Type> genericSpecializations)
        {
            var requiredTypes = GetAllRequiredTypesForType(type, allTypes, genericSpecializations).ToList();
            foreach (var requiredType in requiredTypes)
            {
                if (type != requiredType)
                {
                    AddRequiredType(requiredType, requiredTypesToAdd, typesAdded);
                }

#if NEST_TYPES
                if (requiredType.IsNested && type != requiredType.DeclaringType)
                {
                    AddRequiredType(requiredType.DeclaringType, requiredTypesToAdd, typesAdded);
                }
#endif
            }

            ////if (requiredTypesToAdd.Count > 0)
            ////{
            ////    var requiredTypesToAddNextList = new List<Type>();
            ////    ProcessRequiredTypesForTypes(requiredTypesToAdd, allTypes, typesAdded, requiredTypesToAddNextList);
            ////    requiredTypesToAddNextList.AddRange(requiredTypesToAdd);
            ////    requiredTypesToAdd.Clear();
            ////    requiredTypesToAdd.AddRange(requiredTypesToAddNextList);
            ////}
        }

        /// <summary>
        /// </summary>
        /// <param name="types">
        /// </param>
        /// <param name="allTypes">
        /// </param>
        /// <param name="typesAdded">
        /// </param>
        /// <param name="newListOfTypes">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        private static void ProcessRequiredTypesForTypes(
            List<Type> types, List<Type> allTypes, HashSet<Type> typesAdded, List<Type> newListOfTypes, HashSet<Type> genericSpecializations)
        {
            foreach (var type in types)
            {
                var requiredTypesToAdd = new List<Type>();
                ProcessNextRequiredTypes(type, allTypes, typesAdded, requiredTypesToAdd, genericSpecializations);
                newListOfTypes.AddRange(requiredTypesToAdd);

                if (!typesAdded.Contains(type))
                {
                    typesAdded.Add(type);
                    newListOfTypes.Add(type);
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
        private static List<Type> ResortTypes(List<Type> types, HashSet<Type> genericSpecializations)
        {
            var newOrder = new List<Type>();

            var typesWithRequired = new List<Tuple<Type, List<Type>>>();
            foreach (var type in types)
            {
                var requiredTypesToAdd = new List<Type>();
                ProcessNextRequiredTypes(type, types, new HashSet<Type>(), requiredTypesToAdd, genericSpecializations);
                typesWithRequired.Add(new Tuple<Type, List<Type>>(type, requiredTypesToAdd));
            }

            // the same for generic specialized types
            foreach (var type in genericSpecializations)
            {
                var requiredTypesToAdd = new List<Type>();
                ProcessNextRequiredTypes(type, types, new HashSet<Type>(), requiredTypesToAdd, null);
                typesWithRequired.Add(new Tuple<Type, List<Type>>(type, requiredTypesToAdd));
            }

            var strictMode = true;
            while (typesWithRequired.Count > 0)
            {
                var before = typesWithRequired.Count;
                var toRemove = new List<Tuple<Type, List<Type>>>();

                // step 1 find Root;
                foreach (var type in typesWithRequired)
                {
                    var requiredTypes = type.Item2;
#if NEST_TYPES
                    requiredTypes.RemoveAll(r => newOrder.Any(n => n.GUID == r.GUID || IsBelongToDeclaringType(r, n));
#else
                    if (strictMode)
                    {
                        requiredTypes.RemoveAll(r => newOrder.Any(n => n == r));
                    }
                    else
                    {
                        requiredTypes.RemoveAll(r => newOrder.Any(n => n.GUID == r.GUID));
                    }

#endif
                    if (requiredTypes.Count == 0)
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
        private static bool TypeHasGenericParameter(Type type)
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
        private static bool TypeHasGenericParameterInGenericArguments(Type type)
        {
            return type.IsGenericParameter
                   || type.GetGenericArguments()
                          .Any(
                              t =>
                              t.IsGenericParameter || t.ContainsGenericParameters && TypeHasGenericParameterInGenericArguments(t)
                              || t.HasElementType && TypeHasGenericParameterInGenericArguments(t.GetElementType()));
        }

        #endregion

        /// <summary>
        /// </summary>
        private class InheritanceTypeComparer : IComparer<Type>
        {
            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="baseType">
            /// </param>
            /// <returns>
            /// </returns>
            public static bool DependsOn(Type type, Type baseType)
            {
                if (type.BaseType != null)
                {
                    if (type.BaseType.GUID == baseType.GUID || DependsOn(type.BaseType, baseType))
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
            /// <param name="valueType">
            /// </param>
            /// <returns>
            /// </returns>
            public static bool HasAsValueType(Type type, Type valueType)
            {
                if (!valueType.IsValueType)
                {
                    return false;
                }

                var fields =
                    type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                if (fields != null)
                {
                    foreach (var field in fields)
                    {
                        if (field.FieldType.GUID == valueType.GUID)
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
            public static bool HasInterface(Type type, Type baseInterface)
            {
                var interfaces = type.GetInterfaces();
                if (interfaces != null)
                {
                    foreach (var @interface in interfaces)
                    {
                        if (@interface.GUID == baseInterface.GUID || HasInterface(@interface, baseInterface))
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
            public static int InheritanceLevel(Type t)
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
            public int Compare(Type x, Type y)
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

            #endregion
        }
    }
}
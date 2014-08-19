// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Il2Converter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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
        /// <param name="types">
        /// </param>
        /// <param name="typesAdded">
        /// </param>
        /// <param name="newListOfITypes">
        /// </param>
        /// <param name="genericSpecializations">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        public static void ProcessRequiredITypesForITypes(
            IEnumerable<IType> types,
            HashSet<IType> typesAdded,
            List<IType> newListOfITypes,
            HashSet<IType> genericSpecializations,
            HashSet<IMethod> genericMethodSpecializations)
        {
            foreach (var type in types)
            {
                var requiredITypesToAdd = new List<IType>();
                ProcessNextRequiredITypes(type, typesAdded, requiredITypesToAdd, genericSpecializations, genericMethodSpecializations);
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
        /// <param name="codeWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        /// <param name="disablePostDeclarations">
        /// </param>
        public static void WriteTypeDefinition(ICodeWriter codeWriter, IType type, IGenericContext genericContext)
        {
            var fields = IlReader.Fields(type);
            var count = fields.Count();
            var number = 1;

            codeWriter.WriteTypeStart(type, genericContext);
            codeWriter.WriteBeforeFields(count);

            if (!type.ToNormal().IsEnum)
            {
                foreach (var field in fields)
                {
                    codeWriter.WriteFieldStart(field, number, count);
                    codeWriter.WriteFieldEnd(field, number, count);

                    number++;
                }
            }
            else
            {
                codeWriter.WriteFieldType(type.GetEnumUnderlyingType());
            }

            codeWriter.WriteAfterFields(count);
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

        /// <summary>
        /// </summary>
        /// <param name="ilReader">
        /// </param>
        /// <param name="filter">
        /// </param>
        /// <param name="codeWriter">
        /// </param>
        /// <param name="newListOfITypes">
        /// </param>
        /// <param name="genDefinitionsByMetadataName">
        /// </param>
        /// <param name="genMethodSpec">
        /// </param>
        /// <param name="mode">
        /// </param>
        private static void ConvertAllTypes(
            IlReader ilReader,
            string[] filter,
            ICodeWriter codeWriter,
            List<IType> newListOfITypes,
            SortedDictionary<string, IType> genDefinitionsByMetadataName,
            HashSet<IMethod> genMethodSpec,
            ConvertingMode mode)
        {
            var i = 0;

            foreach (var type in newListOfITypes)
            {
                Debug.WriteLine("Processing({1}): {0}", type.FullName, i);

                i++;

                if (filter != null && !filter.Contains(type.FullName))
                {
                    continue;
                }

                if (type.IsGenericDefinition() || type.Name == "<Module>")
                {
                    continue;
                }

                IType genDef = null;
                if (type.IsGenericType)
                {
                    genDefinitionsByMetadataName.TryGetValue(type.MetadataFullName, out genDef);
                }

                type.UseAsClass = true;
                ConvertIType(ilReader, codeWriter, type, genDef, genMethodSpec, mode);
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
        /// <param name="genericMethodSpecializatons">
        /// </param>
        /// <param name="mode">
        /// </param>
        private static void ConvertIType(
            IlReader ilReader, ICodeWriter codeWriter, IType type, IType genericDefinition, HashSet<IMethod> genericMethodSpecializatons, ConvertingMode mode)
        {
            var genericContext = new MetadataGenericContext();
            genericContext.TypeDefinition = genericDefinition;
            genericContext.TypeSpecialization = null;
            genericContext.MethodDefinition = null;
            genericContext.MethodSpecialization = null;

            if (mode == ConvertingMode.Declaration)
            {
                if (!codeWriter.IsProcessed(type))
                {
                    WriteTypeDefinition(codeWriter, type, genericContext);
                }

                codeWriter.WriteBeforeConstructors();
            }

            if (mode == ConvertingMode.Definition)
            {
                codeWriter.DisableWrite(true);

                // pre process step to get all used undefined structures
                foreach (var ctor in IlReader.Constructors(type))
                {
                    IConstructor genericCtor = null;
                    if (type.IsGenericType && !type.IsInterface && !type.IsDelegate)
                    {
                        // find the same constructor in generic class
                        Debug.Assert(genericDefinition != null);
                        genericCtor = IlReader.Constructors(genericDefinition).First(gm => ctor.IsMatchingGeneric(gm));
                    }

                    genericContext.TypeSpecialization = type.IsGenericType && !type.IsGenericDefinition() ? type : null;
                    genericContext.MethodDefinition = genericCtor;
                    genericContext.MethodSpecialization = null;

                    codeWriter.WriteConstructorStart(ctor, genericContext);

                    foreach (var ilCode in ilReader.OpCodes(genericCtor ?? ctor, genericContext))
                    {
                        codeWriter.Write(ilCode);
                    }

                    codeWriter.WriteConstructorEnd(ctor, genericContext);
                }

                codeWriter.DisableWrite(false);

                // Actual Write
                codeWriter.WriteRequiredTypesForBody();
                codeWriter.WriteStoredText();
            }

            if (mode == ConvertingMode.Declaration)
            {
                codeWriter.WriteAfterConstructors();
                codeWriter.WriteBeforeMethods();
            }

            if (mode == ConvertingMode.Definition)
            {
                codeWriter.DisableWrite(true);

                // pre process step to get all used undefined structures
                foreach (var method in IlReader.Methods(type))
                {
                    IMethod genericMethod = null;
                    if (type.IsGenericType && !type.IsInterface && !type.IsDelegate)
                    {
                        // find the same method in generic class
                        Debug.Assert(genericDefinition != null);
                        genericMethod = IlReader.Methods(genericDefinition).First(gm => method.IsMatchingGeneric(gm));
                    }

                    if (!method.IsGenericMethod)
                    {
                        genericContext.TypeSpecialization = type.IsGenericType && !type.IsGenericDefinition() ? type : null;
                        genericContext.MethodDefinition = genericMethod;
                        genericContext.MethodSpecialization = null;

                        codeWriter.WriteMethodStart(method, genericContext);

                        foreach (var ilCode in ilReader.OpCodes(genericMethod ?? method, genericContext))
                        {
                            codeWriter.Write(ilCode);
                        }

                        codeWriter.WriteMethodEnd(method, genericContext);
                    }
                    else
                    {
                        // write all specializations of a method
                        foreach (var methodSpec in genericMethodSpecializatons)
                        {
                            if (methodSpec.DeclaringType.FullName == method.DeclaringType.FullName 
                                && methodSpec.IsMatchingGeneric(method))
                            {
                                genericContext.TypeSpecialization = type.IsGenericType ? type : null;
                                genericContext.MethodDefinition = method;
                                genericContext.MethodSpecialization = methodSpec;

                                codeWriter.WriteMethodStart(methodSpec, genericContext);

                                foreach (var ilCode in ilReader.OpCodes(genericMethod ?? method, genericContext))
                                {
                                    codeWriter.Write(ilCode);
                                }

                                codeWriter.WriteMethodEnd(methodSpec, genericContext);
                            }
                        }
                    }
                }

                codeWriter.DisableWrite(false);

                // Actual Write
                codeWriter.WriteRequiredTypesForBody();
                codeWriter.WriteStoredText();
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
        /// <param name="genericMethodSpecializations">
        /// </param>
        private static void DicoverGenericSpecializedIType(IType type, HashSet<IType> genericSpecializations, HashSet<IMethod> genericMethodSpecializations)
        {
            if (type == null || genericSpecializations == null || genericMethodSpecializations == null)
            {
                return;
            }

            if (type.HasElementType)
            {
                DicoverGenericSpecializedIType(type.GetElementType(), genericSpecializations, genericMethodSpecializations);
                return;
            }

            if (type.IsGenericType && !type.IsGenericTypeDefinition && !genericSpecializations.Contains(type) && !TypeHasGenericParameter(type)
                && !TypeHasGenericParameterInGenericArguments(type))
            {
                genericSpecializations.Add(type);

                // todo the same for base class and interfaces
                GetAllRequiredITypesForIType(type, genericSpecializations, genericMethodSpecializations).ToArray();
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

            var genericTypeSpecializations = new HashSet<IType>();
            var genericMethodSpecializations = new HashSet<IMethod>();
            var types = ilReader.Types().ToList();
            var allTypes = ilReader.AllTypes().ToList();
            var newListOfITypes = ResortITypes(types.Where(t => !t.IsGenericTypeDefinition).ToList(), genericTypeSpecializations, genericMethodSpecializations);

            // build quick access array for Generic Definitions
            var genDefinitionsByMetadataName = new SortedDictionary<string, IType>();
            foreach (var genDef in allTypes.Where(t => t.IsGenericDefinition()))
            {
                genDefinitionsByMetadataName[genDef.MetadataFullName] = genDef;
            }

            for (var index = 0; index < newListOfITypes.Count; index++)
            {
                var type = newListOfITypes[index];
                if (type.IsGenericTypeDefinition)
                {
                    continue;
                }

                codeWriter.WriteForwardDeclaration(type, index, newListOfITypes.Count);
            }

            ConvertAllTypes(
                ilReader, filter, codeWriter, newListOfITypes, genDefinitionsByMetadataName, genericMethodSpecializations, ConvertingMode.Declaration);
            ConvertAllTypes(
                ilReader, filter, codeWriter, newListOfITypes, genDefinitionsByMetadataName, genericMethodSpecializations, ConvertingMode.Definition);

            codeWriter.WriteEnd();

            codeWriter.Close();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genericTypeSpecializations">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        /// <returns>
        /// </returns>
        private static IEnumerable<IType> GetAllRequiredITypesForIType(
            IType type, HashSet<IType> genericTypeSpecializations, HashSet<IMethod> genericMethodSpecializations)
        {
            if (type.BaseType != null)
            {
                DicoverGenericSpecializedIType(type.BaseType, genericTypeSpecializations, genericMethodSpecializations);
                yield return type.BaseType;
            }

            var interfaces = type.GetInterfaces();
            if (interfaces != null)
            {
                foreach (var @interface in interfaces)
                {
                    DicoverGenericSpecializedIType(@interface, genericTypeSpecializations, genericMethodSpecializations);
                    yield return @interface;
                }
            }

            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (fields != null)
            {
                foreach (var field in fields)
                {
                    DicoverGenericSpecializedIType(field.FieldType, genericTypeSpecializations, genericMethodSpecializations);
                    if (field.FieldType.IsStructureType() && !field.FieldType.IsPointer)
                    {
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
                    DicoverGenericSpecializedIType(method.ReturnType, genericTypeSpecializations, genericMethodSpecializations);

                    foreach (var param in method.GetParameters())
                    {
                        DicoverGenericSpecializedIType(param.ParameterType, genericTypeSpecializations, genericMethodSpecializations);
                    }

                    var methodBody = method.GetMethodBody(MetadataGenericContext.DiscoverFrom(method));
                    if (methodBody != null)
                    {
                        foreach (var localVar in methodBody.LocalVariables)
                        {
                            DicoverGenericSpecializedIType(localVar.LocalType, genericTypeSpecializations, genericMethodSpecializations);
                            if (localVar.LocalType.IsStructureType() && !localVar.LocalType.IsPointer)
                            {
                                yield return localVar.LocalType;
                            }
                        }

                        var usedStructTypes = new HashSet<IType>();
                        method.DiscoverRequiredTypesAndMethods(genericTypeSpecializations, genericMethodSpecializations, usedStructTypes);
                        foreach (var usedStructType in usedStructTypes)
                        {
                            yield return usedStructType;
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
        /// <param name="typesAdded">
        /// </param>
        /// <param name="requiredITypesToAdd">
        /// </param>
        /// <param name="genericTypeSpecializations">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        private static void ProcessNextRequiredITypes(
            IType type,
            HashSet<IType> typesAdded,
            List<IType> requiredITypesToAdd,
            HashSet<IType> genericTypeSpecializations,
            HashSet<IMethod> genericMethodSpecializations)
        {
            var requiredITypes = GetAllRequiredITypesForIType(type, genericTypeSpecializations, genericMethodSpecializations).ToList();
            foreach (var requiredIType in requiredITypes.Where(type.TypeNotEquals))
            {
                AddRequiredIType(requiredIType, requiredITypesToAdd, typesAdded);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="types">
        /// </param>
        /// <param name="genericTypeSpecializations">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        /// <returns>
        /// </returns>
        private static List<IType> ResortITypes(List<IType> types, HashSet<IType> genericTypeSpecializations, HashSet<IMethod> genericMethodSpecializations)
        {
            var newOrder = new List<IType>();

            var typesWithRequired = new List<Tuple<IType, List<IType>>>();
            foreach (var type in types)
            {
                var requiredITypesToAdd = new List<IType>();
                ProcessNextRequiredITypes(type, new HashSet<IType>(), requiredITypesToAdd, genericTypeSpecializations, genericMethodSpecializations);
                typesWithRequired.Add(new Tuple<IType, List<IType>>(type, requiredITypesToAdd));
            }

            ProcessGenericTypeToFindRequired(genericTypeSpecializations, typesWithRequired);

            var allTypes = new List<IType>();
            allTypes.AddRange(types);
            allTypes.AddRange(genericTypeSpecializations);

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
                    requiredITypes.RemoveAll(r => !allTypes.Any(n => n.TypeEquals(r)));

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

        private static void ProcessGenericTypeToFindRequired(HashSet<IType> genericTypeSpecializations, List<Tuple<IType, List<IType>>> typesWithRequired)
        {
            HashSet<IType> subSetGenericTypeSpecializations = new HashSet<IType>();
            HashSet<IMethod> subSetGenericMethodSpecializations = null; // new HashSet<IMethod>();

            // the same for generic specialized types
            foreach (var type in genericTypeSpecializations)
            {
                var requiredITypesToAdd = new List<IType>();
                ProcessNextRequiredITypes(type, new HashSet<IType>(), requiredITypesToAdd, subSetGenericTypeSpecializations, subSetGenericMethodSpecializations);
                typesWithRequired.Add(new Tuple<IType, List<IType>>(type, requiredITypesToAdd));
            }

            if (subSetGenericTypeSpecializations.Count > 0)
            {
                foreach (var disoveredType in typesWithRequired.Select(t => t.Item1))
                {
                    subSetGenericTypeSpecializations.Remove(disoveredType);
                }

                ProcessGenericTypeToFindRequired(subSetGenericTypeSpecializations, typesWithRequired);

                // join types
                foreach (var disoveredType in subSetGenericTypeSpecializations)
                {
                    genericTypeSpecializations.Add(disoveredType);
                }
            }
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

        /// <summary>
        /// </summary>
        public enum ConvertingMode
        {
            /// <summary>
            /// </summary>
            Declaration,

            /// <summary>
            /// </summary>
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
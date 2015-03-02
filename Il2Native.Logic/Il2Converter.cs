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
    using System.Threading.Tasks;
    using Gencode;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class Il2Converter
    {
        private static readonly IDictionary<IType, IEnumerable<IType>> cachedRequiredTypes = new SortedDictionary<IType, IEnumerable<IType>>();

        private static bool concurrent;

        private static ICodeWriter _codeWriter;

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

        public static bool VerboseOutput { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        /// <param name="args">
        /// </param>
        public static void Convert(string source, string outputFolder, string[] args = null, string[] filter = null)
        {
            Convert(new[] { source }, outputFolder, args, filter);
        }

        /// <summary>
        /// </summary>
        /// <param name="sources">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        /// <param name="args">
        /// </param>
        public static void Convert(string[] sources, string outputFolder, string[] args = null, string[] filter = null)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sources.First());

            var ilReader = new IlReader(sources, args);
            ilReader.Load();

            GenerateLlvm(
                ilReader,
                fileNameWithoutExtension,
                ilReader.SourceFilePath,
                ilReader.PdbFilePath,
                outputFolder,
                args,
                filter);
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
            INamespaceContainer<IType> newListOfITypes,
            ReadingTypesContext readingTypesContext)
        {
            if (concurrent)
            {
                Parallel.ForEach(types, type => AppendRequiredTypesForType(type, newListOfITypes, readingTypesContext));
            }
            else
            {
                foreach (var type in types)
                {
                    AppendRequiredTypesForType(type, newListOfITypes, readingTypesContext);
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
        public static void WriteTypeDefinition(ICodeWriter codeWriter, IType type, IGenericContext genericContext)
        {
            codeWriter.WriteTypeStart(type, genericContext);

            var fields = IlReader.Fields(type, codeWriter);
            var count = fields.Count();
            var number = 1;

            Debug.Assert(!type.IsGenericTypeDefinition);

            codeWriter.WriteBeforeFields(count);

            foreach (var field in fields)
            {
                codeWriter.WriteFieldStart(field, number, count);
                codeWriter.WriteFieldEnd(field, number, count);

                number++;
            }

            codeWriter.WriteAfterFields(count);
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
        /// <param name="processGenericMethodsOnly">
        /// </param>
        private static void ConvertAllTypes(
            IlReader ilReader,
            ICodeWriter codeWriter,
            IList<IType> newListOfITypes,
            IDictionary<IType, IEnumerable<IMethod>> genMethodSpec,
            ConvertingMode mode,
            bool processGenericMethodsOnly = false)
        {
            foreach (var type in newListOfITypes)
            {
                Debug.Assert(type != null);
                if (type == null)
                {
                    continue;
                }

                if (type.IsGenericTypeDefinition)
                {
                    continue;
                }

                if (type.FullName == "<Module>")
                {
                    continue;
                }

                IEnumerable<IMethod> genericMethodSpecializatonsForType = null;
                genMethodSpec.TryGetValue(type, out genericMethodSpecializatonsForType);

                ConvertIType(
                    ilReader,
                    codeWriter,
                    type.ToClass(),
                    genericMethodSpecializatonsForType,
                    mode,
                    processGenericMethodsOnly);
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
        /// <param name="typeDefinition">
        /// </param>
        /// <param name="genericMethodSpecializatons">
        /// </param>
        /// <param name="mode">
        /// </param>
        /// <param name="processGenericMethodsOnly">
        /// </param>
        private static void ConvertIType(
            IlReader ilReader,
            ICodeWriter codeWriter,
            IType type,
            IEnumerable<IMethod> genericMethodSpecializatons,
            ConvertingMode mode,
            bool processGenericMethodsOnly = false)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Converting {0}, Mode: {1}", type, mode));
            }

            var typeDefinition = type.IsGenericType ? type.GetTypeDefinition() : null;
            var typeSpecialization = type.IsGenericType && !type.IsGenericTypeDefinition ? type : null;
            var genericTypeContext = typeDefinition != null || typeSpecialization != null
                                         ? MetadataGenericContext.Create(typeDefinition, typeSpecialization)
                                         : null;

            if (mode == ConvertingMode.Declaration)
            {
                if (!codeWriter.IsProcessed(type))
                {
                    WriteTypeDefinition(codeWriter, type, genericTypeContext);
                }

                codeWriter.WritePostDeclarationsAndInternalDefinitions(type);

                codeWriter.WriteBeforeConstructors();
            }

            if (mode == ConvertingMode.Definition)
            {
                codeWriter.DisableWrite(true);

                if (!processGenericMethodsOnly)
                {
                    // pre process step to get all used undefined structures
                    foreach (var ctor in IlReader.Constructors(type, codeWriter))
                    {
                        codeWriter.WriteConstructorStart(ctor, genericTypeContext);

                        foreach (var ilCode in ilReader.OpCodes(type.IsGenericType ? ctor.GetMethodDefinition() : ctor, genericTypeContext))
                        {
                            codeWriter.Write(ilCode);
                        }

                        codeWriter.WriteConstructorEnd(ctor, genericTypeContext);
                    }
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
                foreach (
                    var method in
                        IlReader.Methods(type, codeWriter, true).Select(m => MethodBodyBank.GetMethodWithCustomBodyOrDefault(m, codeWriter)))
                {
                    if (!method.IsGenericMethodDefinition && !processGenericMethodsOnly)
                    {
                        var genericMethodContext = method.IsGenericMethod
                                                       ? MetadataGenericContext.Create(typeDefinition, typeSpecialization, method.GetMethodDefinition(), method)
                                                       : genericTypeContext;

                        codeWriter.WriteMethodStart(method, genericMethodContext);

                        foreach (var ilCode in ilReader.OpCodes(type.IsGenericType ? method.GetMethodDefinition() : method, genericMethodContext))
                        {
                            codeWriter.Write(ilCode);
                        }

                        codeWriter.WriteMethodEnd(method, genericMethodContext);
                    }

                    if (method.IsGenericMethodDefinition || method.IsGenericMethod)
                    {
                        // write all specializations of a method
                        if (genericMethodSpecializatons != null)
                        {
                            var methodDefinition = method.GetMethodDefinition();
                            foreach (
                                var methodSpec in
                                    genericMethodSpecializatons.Where(
                                        methodSpec => methodSpec.IsMatchingGeneric(methodDefinition) && (!methodSpec.Equals(method) || processGenericMethodsOnly)))
                            {
                                var genericMethodContext = MetadataGenericContext.Create(typeDefinition, typeSpecialization, method, methodSpec);

                                codeWriter.WriteMethodStart(methodSpec, genericMethodContext);

                                foreach (var ilCode in ilReader.OpCodes(methodDefinition ?? method, genericMethodContext))
                                {
                                    codeWriter.Write(ilCode);
                                }

                                codeWriter.WriteMethodEnd(methodSpec, genericMethodContext);
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
        private static void DicoverGenericSpecializedTypesAndAdditionalTypes(
            IType typeSource,
            ReadingTypesContext readingTypesContext)
        {
            Debug.Assert(typeSource != null, "Type is null");
            if (typeSource == null)
            {
                return;
            }

            var type = typeSource.NormalizeType();
            if (!readingTypesContext.ProcessedTypes.Add(type))
            {
                return;
            }

            var additionalTypesToProcess = readingTypesContext.AdditionalTypesToProcess;
            var genericSpecializations = readingTypesContext.GenericTypeSpecializations;
            var genericMethodSpecializations = readingTypesContext.GenericMethodSpecializations;

            if (additionalTypesToProcess != null && !type.IsGenericTypeDefinition && type.IsArray)
            {
                additionalTypesToProcess.Add(type);
            }

            if (genericSpecializations == null && genericMethodSpecializations == null)
            {
                return;
            }

            if (type.HasElementType)
            {
                DicoverGenericSpecializedTypesAndAdditionalTypes(type.GetElementType(), readingTypesContext);
            }

            var isGenericToDiscover = type.IsGenericType && !type.IsGenericTypeDefinition && !TypeHasGenericParameter(type) && !TypeHasGenericParameterInGenericArguments(type);
            if (isGenericToDiscover)
            {
                var bareType = type.ToBareType().ToNormal();
                if (genericSpecializations == null || genericSpecializations.Add(bareType))
                {
                    // todo the same for base class and interfaces
                    foreach (var item in GetAllRequiredTypesForType(type, readingTypesContext))
                    {
                    }
                }
            }

            if (type.IsArray)
            {
                foreach (var item in GetAllRequiredTypesForType(type, readingTypesContext))
                {
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="allTypes">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        private static void DiscoverAllGenericMethodsOfInterfaces(
            IEnumerable<IType> allTypes,
            ReadingTypesContext readingTypesContext)
        {
            var genericMethodSpecializations = readingTypesContext.GenericMethodSpecializations;

            var allSpecializedMethodsOfInterfaces = genericMethodSpecializations.Where(m => m.DeclaringType.IsInterface && m.IsGenericMethod).Distinct().ToList();
            var allSpecializedMethodsOfInterfacesGroupedByType = allSpecializedMethodsOfInterfaces.GroupBy(m => m.DeclaringType);

            if (concurrent)
            {
                Parallel.ForEach(
                    allSpecializedMethodsOfInterfacesGroupedByType,
                    specializedTypeMethods =>
                        DiscoverAllGenericMethodsOfInterfacesForMethod(
                            allTypes,
                            specializedTypeMethods,
                            readingTypesContext));
            }
            else
            {
                foreach (var specializedTypeMethods in allSpecializedMethodsOfInterfacesGroupedByType)
                {
                    DiscoverAllGenericMethodsOfInterfacesForMethod(
                        allTypes,
                        specializedTypeMethods,
                        readingTypesContext);
                }
            }
        }

        private static void DiscoverAllGenericMethodsOfInterfacesForMethod(
            IEnumerable<IType> allTypes,
            IGrouping<IType, IMethod> specializedTypeMethods,
            ReadingTypesContext readingTypesContext)
        {
            var types = allTypes.Where(t => t.GetAllInterfaces().Contains(specializedTypeMethods.Key)).ToList();
            foreach (var interfaceMethodSpecialization in specializedTypeMethods)
            {
                var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
                foreach (var classMethodDefinition in
                    types.SelectMany(
                        t =>
                            t.GetMethods(flags)
                                .Where(m => m.IsGenericMethodDefinition && m.IsMatchingOverride(interfaceMethodSpecialization))))
                {
                    // find interface 
                    var @interfaceDefinition =
                        classMethodDefinition.DeclaringType.GetInterfaces()
                            .First(i => i.TypeEquals(interfaceMethodSpecialization.DeclaringType));

                    var @interfaceMethodDefinition = @interfaceDefinition.GetMethods(flags)
                        .First(m => m.IsGenericMethodDefinition && m.IsMatchingOverride(interfaceMethodSpecialization));

                    var classMethodSpecialization =
                        classMethodDefinition.ToSpecialization(
                            MetadataGenericContext.CreateCustomMap(@interfaceMethodDefinition, interfaceMethodSpecialization, classMethodDefinition));

                    readingTypesContext.GenericMethodSpecializations.Add(classMethodSpecialization);

                    // rediscover generic methods again
                    classMethodSpecialization.DiscoverRequiredTypesAndMethodsInMethodBody(
                        readingTypesContext.GenericTypeSpecializations,
                        readingTypesContext.GenericMethodSpecializations,
                        null,
                        readingTypesContext.AdditionalTypesToProcess,
                        new Queue<IMethod>());

                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="allTypes">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        private static void DiscoverAllGenericVirtualMethods(
            IEnumerable<IType> allTypes,
            ReadingTypesContext readingTypesContext)
        {
            var genericMethodSpecializations = readingTypesContext.GenericMethodSpecializations;

            // find all override of generic methods 
            var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                        BindingFlags.Instance;
            var overrideSpecializedMethods = new List<IMethod>();
            foreach (
                var overrideGenericMethod in
                    allTypes.SelectMany(
                        t => t.GetMethods(flags).Where(m => m.IsOverride && m.IsGenericMethodDefinition)))
            {
                var method = overrideGenericMethod;
                var methodDefinition = overrideGenericMethod;
                overrideSpecializedMethods.AddRange(
                    from methodSpecialization in
                        genericMethodSpecializations.Where(m => m.IsVirtual || m.IsOverride || m.IsAbstract)
                    where
                        method.DeclaringType.IsDerivedFrom(methodSpecialization.DeclaringType) &&
                        method.IsMatchingOverride(methodSpecialization)
                    select
                        methodDefinition.ToSpecialization(
                            MetadataGenericContext.CreateCustomMap(null, methodSpecialization, methodDefinition)));
            }

            // append to discovered
            foreach (var overrideSpecializedMethod in overrideSpecializedMethods)
            {
                genericMethodSpecializations.Add(overrideSpecializedMethod);

                // rediscover generic methods again
                overrideSpecializedMethod.DiscoverRequiredTypesAndMethodsInMethodBody(
                    readingTypesContext.GenericTypeSpecializations,
                    readingTypesContext.GenericMethodSpecializations,
                    null,
                    readingTypesContext.AdditionalTypesToProcess,
                    new Queue<IMethod>());
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
        private static void GenerateLlvm(
            IlReader ilReader,
            string fileName,
            string sourceFilePath,
            string pdbFilePath,
            string outputFolder,
            string[] args,
            string[] filter = null)
        {
            concurrent = args != null && args.Any(a => a == "multi");
            VerboseOutput = args != null && args.Any(a => a == "verbose");
            var codeWriter = GetLlvmWriter(fileName, sourceFilePath, pdbFilePath, outputFolder, args);
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
            _codeWriter = codeWriter;

            cachedRequiredTypes.Clear();

            IList<IType> sortedListOfTypes;
            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted;
            ReadingTypes(
                ilReader,
                filter,
                out sortedListOfTypes,
                out genericMethodSpecializationsSorted);

            Writing(
                ilReader,
                codeWriter,
                sortedListOfTypes,
                genericMethodSpecializationsSorted);
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
        private static IEnumerable<IType> GetAllRequiredTypesForType(
            IType typeSource,
            ReadingTypesContext readingTypesContext)
        {
            Debug.Assert(typeSource != null, "Type is null");

            if (typeSource.RequiredTypes != null)
            {
                return typeSource.RequiredTypes;
            }

            lock (cachedRequiredTypes)
            {
                IEnumerable<IType> cachedQuery;
                if (cachedRequiredTypes.TryGetValue(typeSource, out cachedQuery))
                {
                    return cachedQuery;
                }
            }

            var query = IterateAllRequiredTypes(typeSource, readingTypesContext).ToList();
            typeSource.RequiredTypes = query;

            lock (cachedRequiredTypes)
            {
                if (!cachedRequiredTypes.ContainsKey(typeSource))
                {
                    cachedRequiredTypes.Add(typeSource, query);
                }
            }

            return query;
        }

        private static IEnumerable<IType> IterateAllRequiredTypes(IType type, ReadingTypesContext readingTypesContext)
        {
            Debug.Assert(type != null, "Type is null");

            if (type.BaseType != null)
            {
                DicoverGenericSpecializedTypesAndAdditionalTypes(type.BaseType, readingTypesContext);
                yield return type.BaseType;
            }

            if (type.HasElementType)
            {
                DicoverGenericSpecializedTypesAndAdditionalTypes(type.GetElementType(), readingTypesContext);
                yield return type.GetElementType();
            }

            var interfaces = type.GetInterfaces();
            if (interfaces != null)
            {
                foreach (var @interface in interfaces)
                {
                    DicoverGenericSpecializedTypesAndAdditionalTypes(@interface, readingTypesContext);
                    yield return @interface;
                }
            }

            if (!type.IsInterface)
            {
                var fields = IlReader.Fields(
                    type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, _codeWriter);
                foreach (var field in fields)
                {
                    ////#if DEBUG
                    ////    Debug.WriteLine("Processing field: {0}, Type: {1}", field.FullName, field.FieldType);
                    ////#endif

                    DicoverGenericSpecializedTypesAndAdditionalTypes(field.FieldType, readingTypesContext);
                    if (field.FieldType.IsStructureType() && !field.FieldType.IsPointer)
                    {
                        yield return field.FieldType;
                    }
                }

                var ctors = IlReader.Constructors(
                    type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, _codeWriter);
                foreach (var requiredType in ctors.SelectMany(ctor => GetAllRequiredTypesForMethod(ctor, readingTypesContext)))
                {
                    yield return requiredType;
                }
            }

            var methods = IlReader.Methods(
                type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, _codeWriter);
            foreach (var requiredType in methods.SelectMany(method => GetAllRequiredTypesForMethod(method, readingTypesContext)))
            {
                yield return requiredType;
            }
        }

        private static IEnumerable<IType> GetAllRequiredTypesForMethod(
            IMethod method,
            ReadingTypesContext readingTypesContext)
        {
            DicoverGenericSpecializedTypesAndAdditionalTypes(
                method.ReturnType,
                readingTypesContext);

            foreach (var param in method.GetParameters())
            {
                DicoverGenericSpecializedTypesAndAdditionalTypes(
                    param.ParameterType,
                    readingTypesContext);
            }

            if (method.DeclaringType.IsInterface)
            {
                yield break;
            }

            var methodWithCustomBodyOrDefault = MethodBodyBank.GetMethodWithCustomBodyOrDefault(method, _codeWriter);
            var methodBody = methodWithCustomBodyOrDefault.GetMethodBody(MetadataGenericContext.DiscoverFrom(method));
            if (methodBody != null)
            {
                foreach (var localVar in methodBody.LocalVariables)
                {
                    DicoverGenericSpecializedTypesAndAdditionalTypes(
                        localVar.LocalType,
                        readingTypesContext);
                    if (localVar.LocalType.IsStructureType() && !localVar.LocalType.IsPointer && !localVar.LocalType.IsByRef)
                    {
                        yield return localVar.LocalType;
                    }
                }

                var usedStructTypes = new NamespaceContainer<IType>();
                methodWithCustomBodyOrDefault.DiscoverRequiredTypesAndMethodsInMethodBody(
                    readingTypesContext.GenericTypeSpecializations,
                    readingTypesContext.GenericMethodSpecializations,
                    usedStructTypes,
                    readingTypesContext.AdditionalTypesToProcess,
                    new Queue<IMethod>());
                foreach (var usedStructType in usedStructTypes)
                {
                    yield return usedStructType;
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
        private static ICodeWriter GetLlvmWriter(
            string fileName,
            string sourceFilePath,
            string pdbFilePath,
            string outputFolder,
            string[] args)
        {
            return new LlvmWriter(Path.Combine(outputFolder, fileName), sourceFilePath, pdbFilePath, args);
        }

        /// <summary>
        /// </summary>
        /// <param name="genericMethodSpecializations">
        /// </param>
        /// <returns>
        /// </returns>
        private static SortedDictionary<IType, IEnumerable<IMethod>> GroupGenericMethodsByType(
            ISet<IMethod> genericMethodSpecializations)
        {
            // group generic methods by Type
            var genericMethodSpecializationsGroupedByType = genericMethodSpecializations.GroupBy(g => g.DeclaringType);
            var genericMethodSpecializationsSorted = new SortedDictionary<IType, IEnumerable<IMethod>>();
            foreach (var group in genericMethodSpecializationsGroupedByType)
            {
                genericMethodSpecializationsSorted[@group.Key] = @group;
            }

            IlReader.GenericMethodSpecializations = genericMethodSpecializationsSorted;

            return genericMethodSpecializationsSorted;
        }

        /// <summary>
        /// </summary>
        /// <param name="genericTypeSpecializations">
        /// </param>
        /// <param name="requiredTypes">
        /// </param>
        [Obsolete]
        private static void ProcessGenericTypesAndAdditionalTypesToFindRequiredTypes(
            IList<IAssoc<IType, INamespaceContainer<IType>>> requiredTypes,
            ReadingTypesContext readingTypesContext,
            bool applyConccurent = false)
        {
            var subSetGenericTypeSpecializations = new NamespaceContainer<IType>();
            var subSetAdditionalTypesToProcess = new NamespaceContainer<IType>();

            var subSetReadingContext = new ReadingTypesContext();
            subSetReadingContext.GenericTypeSpecializations = subSetGenericTypeSpecializations;
            subSetReadingContext.GenericMethodSpecializations = readingTypesContext.GenericMethodSpecializations;
            subSetReadingContext.AdditionalTypesToProcess = subSetAdditionalTypesToProcess;
            subSetReadingContext.ProcessedTypes = readingTypesContext.ProcessedTypes;

            // the same for generic specialized types
            if (concurrent && applyConccurent)
            {
                Parallel.ForEach(
                    readingTypesContext.GenericTypeSpecializations.ToList(),
                    type => AppendTypeWithRequiredTypePair(type, requiredTypes, subSetReadingContext));

                Parallel.ForEach(
                    readingTypesContext.AdditionalTypesToProcess.ToList(),
                    type => AppendTypeWithRequiredTypePair(type, requiredTypes, subSetReadingContext));
            }
            else
            {
                foreach (var type in readingTypesContext.GenericTypeSpecializations.ToList())
                {
                    Debug.Assert(type != null);
                    if (type == null)
                    {
                        continue;
                    }

                    AppendTypeWithRequiredTypePair(type, requiredTypes, subSetReadingContext);
                }

                foreach (var type in readingTypesContext.AdditionalTypesToProcess.ToList())
                {
                    Debug.Assert(type != null);
                    if (type == null)
                    {
                        continue;
                    }

                    AppendTypeWithRequiredTypePair(type, requiredTypes, subSetReadingContext);
                }
            }

            if (subSetGenericTypeSpecializations.Count > 0 || subSetAdditionalTypesToProcess.Count > 0)
            {
                foreach (var discoveredType in requiredTypes.Select(t => t.Key))
                {
                    subSetGenericTypeSpecializations.Remove(discoveredType);
                }

                foreach (var discoveredType in requiredTypes.Select(t => t.Key))
                {
                    Debug.Assert(discoveredType != null);
                    subSetAdditionalTypesToProcess.Remove(discoveredType);
                }

                ProcessGenericTypesAndAdditionalTypesToFindRequiredTypes(requiredTypes, subSetReadingContext);

                // join types
                foreach (var discoveredType in subSetGenericTypeSpecializations)
                {
                    Debug.Assert(discoveredType != null);
                    readingTypesContext.GenericTypeSpecializations.Add(discoveredType);
                }

                // join types
                foreach (var discoveredType in subSetAdditionalTypesToProcess)
                {
                    Debug.Assert(discoveredType != null);
                    readingTypesContext.AdditionalTypesToProcess.Add(discoveredType);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typesAdded">
        /// </param>
        /// <param name="addedRequiredTypes">
        /// </param>
        /// <param name="genericTypeSpecializations">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        private static void AppendRequiredTypesForType(
            IType type,
            IList<IType> addedRequiredTypes,
            ReadingTypesContext readingTypesContext)
        {
            Debug.Assert(type != null, "Type is null");

            var requiredTypes = GetAllRequiredTypesForType(type, readingTypesContext);
            foreach (var requiredType in requiredTypes.Where(requiredType => !requiredType.IsValueType() || requiredType.IsStructureType()))
            {
                addedRequiredTypes.Add(requiredType);
            }

            addedRequiredTypes.Remove(type);
        }

        private static void AppendTypeWithRequiredTypePair(IType type, IList<IAssoc<IType, INamespaceContainer<IType>>> requiredTypesByType, ReadingTypesContext readingTypesContext)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Reading info about type: {0}", type));
            }

            var requiredITypesToAdd = new NamespaceContainer<IType>();
            AppendRequiredTypesForType(type, requiredITypesToAdd, readingTypesContext);

            requiredTypesByType.Add(new NamespaceContainerAssoc<IType, INamespaceContainer<IType>>(type, requiredITypesToAdd));
        }

        private static void ReadingTypes(
            IlReader ilReader,
            string[] filter,
            out IList<IType> sortedListOfTypes,
            out IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted)
        {
            // clean it as you are using IlReader
            IlReader.GenericMethodSpecializations = null;

            // types in current assembly
            var readingTypesContext = ReadingTypesContext.New();
            var types = ilReader.Types().Where(t => !t.IsGenericTypeDefinition);
            if (filter != null)
            {
                types = types.Where(t => CheckFilter(filter, t));
            }

            var allTypes = ilReader.AllTypes().ToList();

            // TODO: temp hack to initialize ThisType for TypeResolver
            _codeWriter.Initialize(allTypes.First());

            sortedListOfTypes = SortTypesByUsage(types.ToList(), allTypes, readingTypesContext);

            genericMethodSpecializationsSorted = GroupGenericMethodsByType(readingTypesContext.GenericMethodSpecializations);

            Debug.Assert(sortedListOfTypes.All(t => !t.IsByRef), "Type is used with flag IsByRef");
        }

        private static bool CheckFilter(IEnumerable<string> filters, IType type)
        {
            foreach (var filter in filters)
            {
                if (filter.EndsWith("*"))
                {
                    if (filter.Length > 1 && filter[filter.Length - 2] == '*')
                    {
                        if (type.Namespace.StartsWith(filter.Substring(0, filter.Length - 2)))
                        {
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if (type.Namespace == filter.Substring(0, filter.Length - 1))
                    {
                        return true;
                    }
                }

                if (string.CompareOrdinal(type.MetadataFullName, 0, filter, 0, filter.Length) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static void RemoveAllResolvedTypesForType(
            IAssoc<IType, INamespaceContainer<IType>> type,
            IList<IType> newOrder,
            IList<IAssoc<IType, INamespaceContainer<IType>>> toRemove,
            object syncToRemove)
        {
            var requiredITypes = type.Value;

            requiredITypes.RemoveAll(newOrder.Contains);

            if (requiredITypes.Count != 0)
            {
                return;
            }

            lock (syncToRemove)
            {
                toRemove.Add(type);
            }

            newOrder.Add(type.Key);
        }

        private static void ReorderTypeByUsage(
            IList<IType> types,
            ISet<IType> genericTypeSpecializations,
            ISet<IType> additionalTypes,
            NamespaceContainer<IType, INamespaceContainer<IType>> typesWithRequired,
            IList<IType> newOrder)
        {
            var allTypes = new NamespaceContainer<IType>();
            foreach (var type in types)
            {
                allTypes.Add(type);
            }

            foreach (var type in genericTypeSpecializations)
            {
                allTypes.Add(type);
            }

            foreach (var type in additionalTypes)
            {
                allTypes.Add(type);
            }

            var strictMode = true;
            while (typesWithRequired.Count > 0)
            {
                var before = typesWithRequired.Count;
                var toRemove = new NamespaceContainer<IType, INamespaceContainer<IType>>();
                var syncToRemove = new object();

                // remove not used types, for example System.Object which maybe not in current assembly
                foreach (var requiredITypes in typesWithRequired)
                {
                    requiredITypes.Value.RemoveAll(r => !allTypes.Contains(r));
                }

                // step 1 find Root;
                if (concurrent)
                {
                    Parallel.ForEach(
                        typesWithRequired,
                        type =>
                            RemoveAllResolvedTypesForType(type, newOrder, toRemove, syncToRemove));
                }
                else
                {
                    foreach (var type in typesWithRequired)
                    {
                        RemoveAllResolvedTypesForType(type, newOrder, toRemove, syncToRemove);
                    }
                }

                lock (syncToRemove)
                {
                    foreach (var type in toRemove)
                    {
                        typesWithRequired.Remove(type);
                    }
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
                        newOrder.Add(typeItems.Key);
                    }

                    break;
                }
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
        private static IList<IType> SortTypesByUsage(IList<IType> types, IList<IType> allTypes, ReadingTypesContext readingTypesContext)
        {
            var newOrder = new NamespaceContainer<IType>();
            var typesWithRequired = new NamespaceContainer<IType, INamespaceContainer<IType>>();

            if (concurrent)
            {
                Parallel.ForEach(
                    types,
                    type =>
                        AppendTypeWithRequiredTypePair(
                            type,
                            typesWithRequired,
                            readingTypesContext));
            }
            else
            {
                foreach (var type in types)
                {
                    AppendTypeWithRequiredTypePair(
                        type,
                        typesWithRequired,
                        readingTypesContext);
                }
            }

            ProcessGenericTypesAndAdditionalTypesToFindRequiredTypes(
                typesWithRequired,
                readingTypesContext,
                true);

            DiscoverAllGenericVirtualMethods(allTypes, readingTypesContext);

            DiscoverAllGenericMethodsOfInterfaces(allTypes, readingTypesContext);

            ReorderTypeByUsage(types, readingTypesContext.GenericTypeSpecializations, readingTypesContext.AdditionalTypesToProcess, typesWithRequired, newOrder);

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
                           t.IsGenericParameter ||
                           t.IsGenericType && (t.IsGenericTypeDefinition || TypeHasGenericParameter(t))
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
                   || type.GenericTypeArguments.Any(
                       t =>
                       t.IsGenericParameter || t.ContainsGenericParameters && TypeHasGenericParameterInGenericArguments(t)
                       || t.HasElementType && TypeHasGenericParameterInGenericArguments(t.GetElementType()));
        }

        /// <summary>
        /// </summary>
        /// <param name="codeWriter">
        /// </param>
        /// <param name="newListOfITypes">
        /// </param>
        private static void WriteForwardDeclarations(ICodeWriter codeWriter, IList<IType> newListOfITypes)
        {
            // write forward declaration
            for (var index = 0; index < newListOfITypes.Count; index++)
            {
                var type = newListOfITypes[index];
                Debug.Assert(type != null);
                if (type == null || type.IsGenericTypeDefinition)
                {
                    continue;
                }

                codeWriter.WriteForwardDeclaration(type, index, newListOfITypes.Count);
            }
        }

        private static void Writing(
            IlReader ilReader,
            ICodeWriter codeWriter,
            IList<IType> newListOfITypes,
            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted)
        {
            // writing
            codeWriter.WriteStart(ilReader);

            WriteForwardDeclarations(codeWriter, newListOfITypes);

            ConvertAllTypes(
                ilReader,
                codeWriter,
                newListOfITypes,
                genericMethodSpecializationsSorted,
                ConvertingMode.Declaration);

            ConvertAllTypes(
                ilReader,
                codeWriter,
                newListOfITypes,
                genericMethodSpecializationsSorted,
                ConvertingMode.Definition);

            // Append definition of Generic Methods of not used non-generic types
            ConvertAllTypes(
                ilReader,
                codeWriter,
                genericMethodSpecializationsSorted.Keys.Where(k => !newListOfITypes.Contains(k)).ToList(),
                genericMethodSpecializationsSorted,
                ConvertingMode.Definition,
                true);

            codeWriter.WriteEnd();

            codeWriter.Close();
        }
    }

    public class ReadingTypesContext
    {
        public ReadingTypesContext()
        {
            this.GenericTypeSpecializations = new NamespaceContainer<IType>();
            this.GenericMethodSpecializations = new NamespaceContainer<IMethod>();
            this.AdditionalTypesToProcess = new NamespaceContainer<IType>();
            this.ProcessedTypes = new NamespaceContainer<IType>();
        }

        public ISet<IType> GenericTypeSpecializations { get; set; }

        public ISet<IMethod> GenericMethodSpecializations { get; set; }

        public ISet<IType> AdditionalTypesToProcess { get; set; }

        public ISet<IType> ProcessedTypes { get; set; }

        public static ReadingTypesContext New()
        {
            var context = new ReadingTypesContext();
            context.GenericTypeSpecializations = new NamespaceContainer<IType>();
            context.GenericMethodSpecializations = new NamespaceContainer<IMethod>();
            context.AdditionalTypesToProcess = new NamespaceContainer<IType>();
            context.ProcessedTypes = new NamespaceContainer<IType>();
            return context;
        }
    }
}
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
        private static readonly IDictionary<IType, IEnumerable<IType>> cachedRequiredDefinitionTypes = new SortedDictionary<IType, IEnumerable<IType>>();

        private static readonly IDictionary<IType, IEnumerable<IType>> cachedRequiredDeclarationTypes = new SortedDictionary<IType, IEnumerable<IType>>();

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
        /// <param name="types">
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
            IEnumerable<IType> types,
            IDictionary<IType, IEnumerable<IMethod>> genMethodSpec,
            ConvertingMode mode,
            bool processGenericMethodsOnly = false)
        {
            foreach (var type in types)
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

                ConvertType(
                    ilReader,
                    codeWriter,
                    type,
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
        private static void ConvertType(
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
                if (!codeWriter.IsTypeDefinitionWritten(type))
                {
                    WriteTypeDefinition(codeWriter, type, genericTypeContext);
                }

                codeWriter.WritePostDeclarationsAndInternalDefinitions(type);

                codeWriter.WriteBeforeConstructors();
            }

            if (mode == ConvertingMode.Definition)
            {
                if (!processGenericMethodsOnly)
                {
                    // pre process step to get all used undefined structures
                    foreach (var ctor in IlReader.Constructors(type, codeWriter).Select(m => MethodBodyBank.GetMethodWithCustomBodyOrDefault(m, codeWriter)))
                    {
                        codeWriter.WriteConstructorStart(ctor, genericTypeContext);

                        foreach (var ilCode in ilReader.OpCodes(type.IsGenericType ? ctor.GetMethodDefinition() : ctor, genericTypeContext))
                        {
                            codeWriter.Write(ilCode);
                        }

                        codeWriter.WriteConstructorEnd(ctor, genericTypeContext);
                    }
                }
            }

            if (mode == ConvertingMode.Declaration)
            {
                codeWriter.WriteAfterConstructors();
                codeWriter.WriteBeforeMethods();
            }

            if (mode == ConvertingMode.Definition)
            {
                // pre process step to get all used undefined structures
                foreach (
                    var method in
                        IlReader.Methods(type, codeWriter, true).Select(m => MethodBodyBank.GetMethodWithCustomBodyOrDefault(m, codeWriter)))
                {
                    if (VerboseOutput)
                    {
                        Trace.WriteLine(string.Format("writing method {0}", method));
                    }

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
            }

            if (mode == ConvertingMode.Declaration)
            {
                codeWriter.WriteAfterMethods();
                codeWriter.WriteTypeEnd(type);
            }
        }

        private static void AddTypeIfSpecializedTypeOrAdditionalType(IType type, ReadingTypesContext readingTypesContext)
        {
            var effectiveType = type;
            while (effectiveType.IsPointer || effectiveType.IsByRef)
            {
                effectiveType = effectiveType.GetElementType();
            }

            if (effectiveType.IsClass)
            {
                effectiveType = effectiveType.ToNormal();
            }

            if (effectiveType.IsArray)
            {
                readingTypesContext.AdditionalTypesToProcess.Add(effectiveType);
            }

            if (effectiveType.IsGenericType)
            {
                readingTypesContext.GenericTypeSpecializations.Add(effectiveType);
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
        private static void DiscoverGenericSpecializedTypesAndAdditionalTypes(
            IType typeSource,
            ReadingTypesContext readingTypesContext)
        {
            // TODO: you need to discover only actual usage of arrays and generics(?)
            // for example function Main(string[] args) needs only forward declararion, so gather only arrays and generics in method bodies only
            // I even belive that you need to take only array which used in Code.Newarr code. and the same is for Generics

            Debug.Assert(typeSource != null, "Type is null");
            if (typeSource == null)
            {
                return;
            }

            var type = typeSource.NormalizeType();
            if (!readingTypesContext.DiscoveredTypes.Add(type))
            {
                return;
            }

            ////var additionalTypesToProcess = readingTypesContext.AdditionalTypesToProcess;
            ////var genericSpecializations = readingTypesContext.GenericTypeSpecializations;
            ////var genericMethodSpecializations = readingTypesContext.GenericMethodSpecializations;

            ////if (additionalTypesToProcess != null && !type.IsGenericTypeDefinition && type.IsArray)
            ////{
            ////    additionalTypesToProcess.Add(type);
            ////}

            ////if (genericSpecializations == null && genericMethodSpecializations == null)
            ////{
            ////    return;
            ////}

            ////var isGenericToDiscover = type.IsGenericType && !type.IsGenericTypeDefinition && !TypeHasGenericParameter(type)
            ////                          && !TypeHasGenericParameterInGenericArguments(type);
            ////if (isGenericToDiscover)
            ////{
            ////    var bareType = type.ToBareType().ToNormal();
            ////    if (genericSpecializations == null)
            ////    {
            ////        genericSpecializations.Add(bareType);
            ////    }
            ////}

            ////if (type.BaseType != null)
            ////{
            ////    DiscoverGenericSpecializedTypesAndAdditionalTypes(type.BaseType, readingTypesContext);
            ////}

            ////if (type.HasElementType)
            ////{
            ////    DiscoverGenericSpecializedTypesAndAdditionalTypes(type.GetElementType(), readingTypesContext);
            ////}

            ////var interfaces = type.GetInterfaces();
            ////if (interfaces != null)
            ////{
            ////    foreach (var @interface in interfaces)
            ////    {
            ////        DiscoverGenericSpecializedTypesAndAdditionalTypes(@interface, readingTypesContext);
            ////    }
            ////}

            if (!type.IsInterface)
            {
                ////var fields = IlReader.Fields(type, IlReader.DefaultFlags, _codeWriter);
                ////foreach (var field in fields)
                ////{
                ////    DiscoverGenericSpecializedTypesAndAdditionalTypes(field.FieldType, readingTypesContext);
                ////}

                var ctors = IlReader.Constructors(type, IlReader.DefaultFlags, _codeWriter);
                foreach (var ctor in ctors)
                {
                    DiscoverGenericSpecializedTypesAndAdditionalTypes(ctor, readingTypesContext);
                }
            }

            var methods = IlReader.Methods(type, IlReader.DefaultFlags, _codeWriter);
            foreach (var method in methods)
            {
                DiscoverGenericSpecializedTypesAndAdditionalTypes(method, readingTypesContext);
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
                    classMethodSpecialization.DiscoverStructsArraysSpecializedTypesAndMethodsInMethodBody(
                        readingTypesContext.GenericTypeSpecializations,
                        readingTypesContext.GenericMethodSpecializations,
                        null,
                        readingTypesContext.AdditionalTypesToProcess,
                        new Queue<IMethod>(),
                        _codeWriter);

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
                overrideSpecializedMethod.DiscoverStructsArraysSpecializedTypesAndMethodsInMethodBody(
                    readingTypesContext.GenericTypeSpecializations,
                    readingTypesContext.GenericMethodSpecializations,
                    null,
                    readingTypesContext.AdditionalTypesToProcess,
                    new Queue<IMethod>(),
                    _codeWriter);
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
            ilReader.TypeResolver = codeWriter;
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

            cachedRequiredDefinitionTypes.Clear();
            cachedRequiredDeclarationTypes.Clear();

            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted;
            IEnumerable<IType> sortedListOfTypes = ReadingTypes(
                ilReader,
                filter,
                out genericMethodSpecializationsSorted);

            Writing(
                ilReader,
                codeWriter,
                sortedListOfTypes,
                genericMethodSpecializationsSorted);
        }

        public static IEnumerable<IType> GetRequiredDeclarationTypes(IType typeSource)
        {
            Debug.Assert(typeSource != null, "Type is null");

            lock (cachedRequiredDeclarationTypes)
            {
                IEnumerable<IType> cachedQuery;
                if (cachedRequiredDeclarationTypes.TryGetValue(typeSource, out cachedQuery))
                {
                    return cachedQuery;
                }
            }

            var query = IterateRequiredDeclarationTypes(typeSource).ToList();

            lock (cachedRequiredDeclarationTypes)
            {
                if (!cachedRequiredDeclarationTypes.ContainsKey(typeSource))
                {
                    cachedRequiredDeclarationTypes.Add(typeSource, query);
                }
            }

            return query;
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
        public static IEnumerable<IType> GetRequiredDefinitionTypes(IType typeSource)
        {
            Debug.Assert(typeSource != null, "Type is null");

            lock (cachedRequiredDefinitionTypes)
            {
                IEnumerable<IType> cachedQuery;
                if (cachedRequiredDefinitionTypes.TryGetValue(typeSource, out cachedQuery))
                {
                    return cachedQuery;
                }
            }

            var query = IterateRequiredDefinitionTypes(typeSource).ToList();

            lock (cachedRequiredDefinitionTypes)
            {
                if (!cachedRequiredDefinitionTypes.ContainsKey(typeSource))
                {
                    cachedRequiredDefinitionTypes.Add(typeSource, query);
                }
            }

            return query;
        }

        private static IEnumerable<IType> IterateRequiredDefinitionTypes(IType type)
        {
            Debug.Assert(type != null, "Type is null");

            if (type.BaseType != null)
            {
                yield return type.BaseType;
            }

            if (type.HasElementType)
            {
                yield return type.GetElementType();
            }

            var interfaces = type.GetInterfaces();
            if (interfaces != null)
            {
                foreach (var @interface in interfaces)
                {
                    yield return @interface;
                }
            }

            var fields = IlReader.Fields(
                type, IlReader.DefaultFlags, _codeWriter);
            foreach (var field in fields.Where(field => (field.FieldType.IsStructureType() && !field.FieldType.IsPointer)))
            {
                yield return field.FieldType;
            }

            if (type.IsInterface)
            {
                yield break;
            }

            // excluding interface
            var ctors = IlReader.Constructors(type, IlReader.DefaultFlags, _codeWriter);
            foreach (var requiredType in ctors.SelectMany(IterateRequiredDefinitionTypesInMethodBody))
            {
                yield return requiredType;
            }

            var methods = IlReader.Methods(type, IlReader.DefaultFlags, _codeWriter);
            foreach (var requiredType in methods.SelectMany(IterateRequiredDefinitionTypesInMethodBody))
            {
                yield return requiredType;
            }
        }

        private static IEnumerable<IType> IterateRequiredDeclarationTypes(IType type)
        {
            Debug.Assert(type != null, "Type is null");

            var fields = IlReader.Fields(type, IlReader.DefaultFlags, _codeWriter);
            foreach (var effectiveType in
                fields.Select(field => field.FieldType).Where(fieldType => !fieldType.IsVoid() && !fieldType.IsValueType && type.TypeNotEquals(fieldType)))
            {
                yield return effectiveType.NormalizeType();
            }
        }

        private static void DiscoverGenericSpecializedTypesAndAdditionalTypes(
            IMethod method,
            ReadingTypesContext readingTypesContext)
        {
            if (!method.ReturnType.IsVoid())
            {
                AddTypeIfSpecializedTypeOrAdditionalType(method.ReturnType, readingTypesContext);
                ////DiscoverGenericSpecializedTypesAndAdditionalTypes(method.ReturnType, readingTypesContext);
            }

            var parameters = method.GetParameters();
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    AddTypeIfSpecializedTypeOrAdditionalType(param.ParameterType, readingTypesContext);
                    ////DiscoverGenericSpecializedTypesAndAdditionalTypes(param.ParameterType, readingTypesContext);
                }
            }

            if (method.DeclaringType.IsInterface)
            {
                return;
            }

            var methodWithCustomBodyOrDefault = MethodBodyBank.GetMethodWithCustomBodyOrDefault(method, _codeWriter);
            var methodBody = methodWithCustomBodyOrDefault.GetMethodBody(MetadataGenericContext.DiscoverFrom(method));
            if (methodBody != null)
            {
                foreach (var localVar in methodBody.LocalVariables)
                {
                    AddTypeIfSpecializedTypeOrAdditionalType(localVar.LocalType, readingTypesContext);
                    ////DiscoverGenericSpecializedTypesAndAdditionalTypes(localVar.LocalType, readingTypesContext);
                }

                methodWithCustomBodyOrDefault.DiscoverStructsArraysSpecializedTypesAndMethodsInMethodBody(
                    readingTypesContext.GenericTypeSpecializations,
                    readingTypesContext.GenericMethodSpecializations,
                    null,
                    readingTypesContext.AdditionalTypesToProcess,
                    new Queue<IMethod>(),
                    _codeWriter);
            }
        }

        private static IEnumerable<IType> IterateRequiredDefinitionTypesInMethodBody(IMethod method)
        {
            if (method.DeclaringType.IsInterface)
            {
                yield break;
            }

            var methodWithCustomBodyOrDefault = MethodBodyBank.GetMethodWithCustomBodyOrDefault(method, _codeWriter);
            var methodBody = methodWithCustomBodyOrDefault.GetMethodBody(MetadataGenericContext.DiscoverFrom(method));
            if (methodBody != null)
            {
                foreach (
                    var localVar in
                        methodBody.LocalVariables.Where(
                            localVar => localVar.LocalType.IsStructureType() && !localVar.LocalType.IsPointer && !localVar.LocalType.IsByRef))
                {
                    yield return localVar.LocalType;
                }

                var usedStructTypes = new NamespaceContainer<IType>();
                methodWithCustomBodyOrDefault
                    .DiscoverStructsArraysSpecializedTypesAndMethodsInMethodBody(null, null, usedStructTypes, null, new Queue<IMethod>(), _codeWriter);
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
            return new CWriter(Path.Combine(outputFolder, fileName), sourceFilePath, pdbFilePath, args);
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
        private static void ProcessGenericTypesAndAdditionalTypesToDiscoverGenericSpecializedTypesAndAdditionalTypes(
            IList<IType> requiredTypes,
            ReadingTypesContext readingTypesContext,
            bool applyConccurent = false)
        {
            var subSetGenericTypeSpecializations = new NamespaceContainer<IType>();
            var subSetAdditionalTypesToProcess = new NamespaceContainer<IType>();

            var subSetReadingContext = new ReadingTypesContext();
            subSetReadingContext.GenericTypeSpecializations = subSetGenericTypeSpecializations;
            subSetReadingContext.GenericMethodSpecializations = readingTypesContext.GenericMethodSpecializations;
            subSetReadingContext.AdditionalTypesToProcess = subSetAdditionalTypesToProcess;
            subSetReadingContext.DiscoveredTypes = readingTypesContext.DiscoveredTypes;

            // the same for generic specialized types
            if (concurrent && applyConccurent)
            {
                Parallel.ForEach(
                    readingTypesContext.GenericTypeSpecializations.ToList(),
                    type => AppendTypeAndDiscoverGenericSpecializedTypesAndAdditionalTypes(type, requiredTypes, subSetReadingContext));

                Parallel.ForEach(
                    readingTypesContext.AdditionalTypesToProcess.ToList(),
                    type => AppendTypeAndDiscoverGenericSpecializedTypesAndAdditionalTypes(type, requiredTypes, subSetReadingContext));
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

                    AppendTypeAndDiscoverGenericSpecializedTypesAndAdditionalTypes(type, requiredTypes, subSetReadingContext);
                }

                foreach (var type in readingTypesContext.AdditionalTypesToProcess.ToList())
                {
                    Debug.Assert(type != null);
                    if (type == null)
                    {
                        continue;
                    }

                    AppendTypeAndDiscoverGenericSpecializedTypesAndAdditionalTypes(type, requiredTypes, subSetReadingContext);
                }
            }

            if (subSetGenericTypeSpecializations.Count > 0 || subSetAdditionalTypesToProcess.Count > 0)
            {
                foreach (var discoveredType in requiredTypes)
                {
                    subSetGenericTypeSpecializations.Remove(discoveredType);
                }

                foreach (var discoveredType in requiredTypes)
                {
                    Debug.Assert(discoveredType != null);
                    subSetAdditionalTypesToProcess.Remove(discoveredType);
                }

                ProcessGenericTypesAndAdditionalTypesToDiscoverGenericSpecializedTypesAndAdditionalTypes(requiredTypes, subSetReadingContext);

                // join types
                foreach (var discoveredType in subSetGenericTypeSpecializations)
                {
                    Debug.Assert(discoveredType != null);
                    if (discoveredType.NotSpecialUsage())
                    {
                        readingTypesContext.GenericTypeSpecializations.Add(discoveredType);
                    }
                }

                // join types
                foreach (var discoveredType in subSetAdditionalTypesToProcess)
                {
                    Debug.Assert(discoveredType != null);
                    if (discoveredType.NotSpecialUsage())
                    {
                        readingTypesContext.AdditionalTypesToProcess.Add(discoveredType);
                    }
                }
            }
        }

        private static void AppendTypeAndDiscoverGenericSpecializedTypesAndAdditionalTypes(IType type, IList<IType> requiredTypesByType, ReadingTypesContext readingTypesContext)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Reading info about type: {0}", type));
            }

            DiscoverGenericSpecializedTypesAndAdditionalTypes(type, readingTypesContext);

            if (type.NotSpecialUsage())
            {
                requiredTypesByType.Add(type);
            }
        }

        private static IEnumerable<IType> ReadingTypes(
            IlReader ilReader,
            string[] filter,
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

            var usedTypes = FindUsedTypes(types.ToList(), allTypes, readingTypesContext);

            genericMethodSpecializationsSorted = GroupGenericMethodsByType(readingTypesContext.GenericMethodSpecializations);

            Debug.Assert(usedTypes.All(t => !t.IsByRef), "Type is used with flag IsByRef");
            Debug.Assert(usedTypes.All(t => !t.IsPointer), "Type is used with flag IsPointer");

            var typesArray = usedTypes.ToArray();
            ////Array.Sort(typesArray, IsDerived);
            return typesArray;
        }

        private static int IsDerived(IType type, IType other)
        {
            if (type.IsDerivedFrom(other))
            {
                return 1;
            }

            if (other.IsDerivedFrom(type))
            {
                return -1;
            }

            if (type.IsInterface && other.GetAllInterfaces().Contains(type))
            {
                return 1;
            }

            if (other.IsInterface && type.GetAllInterfaces().Contains(other))
            {
                return -1;
            }

            return 0;
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
        private static IList<IType> FindUsedTypes(IEnumerable<IType> types, IList<IType> allTypes, ReadingTypesContext readingTypesContext)
        {
            var usedTypes = new NamespaceContainer<IType>();

            if (concurrent)
            {
                Parallel.ForEach(types, type => AppendTypeAndDiscoverGenericSpecializedTypesAndAdditionalTypes(type, usedTypes, readingTypesContext));
            }
            else
            {
                foreach (var type in types)
                {
                    AppendTypeAndDiscoverGenericSpecializedTypesAndAdditionalTypes(type, usedTypes, readingTypesContext);
                }
            }

            ProcessGenericTypesAndAdditionalTypesToDiscoverGenericSpecializedTypesAndAdditionalTypes(usedTypes, readingTypesContext, true);

            DiscoverAllGenericVirtualMethods(allTypes, readingTypesContext);

            DiscoverAllGenericMethodsOfInterfaces(allTypes, readingTypesContext);

            return usedTypes;
        }

        private static void Writing(
            IlReader ilReader,
            ICodeWriter codeWriter,
            IEnumerable<IType> types,
            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted)
        {
            // writing
            codeWriter.WriteStart(ilReader);

            ConvertAllTypes(
                ilReader,
                codeWriter,
                types,
                genericMethodSpecializationsSorted,
                ConvertingMode.Declaration);

            ConvertAllTypes(
                ilReader,
                codeWriter,
                types,
                genericMethodSpecializationsSorted,
                ConvertingMode.Definition);

            // Append definition of Generic Methods of not used non-generic types
            ConvertAllTypes(
                ilReader,
                codeWriter,
                genericMethodSpecializationsSorted.Keys.Where(k => !types.Contains(k)).ToList(),
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
            this.DiscoveredTypes = new NamespaceContainer<IType>();
        }

        public ISet<IType> GenericTypeSpecializations { get; set; }

        public ISet<IMethod> GenericMethodSpecializations { get; set; }

        public ISet<IType> AdditionalTypesToProcess { get; set; }

        public ISet<IType> DiscoveredTypes { get; set; }

        public static ReadingTypesContext New()
        {
            var context = new ReadingTypesContext();
            context.GenericTypeSpecializations = new NamespaceContainer<IType>();
            context.GenericMethodSpecializations = new NamespaceContainer<IMethod>();
            context.AdditionalTypesToProcess = new NamespaceContainer<IType>();
            context.DiscoveredTypes = new NamespaceContainer<IType>();
            return context;
        }
    }
}
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

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class Il2Converter
    {
        private static readonly IDictionary<IType, IEnumerable<IType>> cachedRequiredFullDeclarationTypes = new SortedDictionary<IType, IEnumerable<IType>>();

        private static readonly IDictionary<IType, IEnumerable<IType>> cachedRequiredForwardDeclarationTypes = new SortedDictionary<IType, IEnumerable<IType>>();

        private static bool concurrent;

        private static ICodeWriter _codeWriter;

        private static bool split;

        /// <summary>
        /// </summary>
        public enum ConvertingMode
        {
            /// <summary>
            /// </summary>
            ForwardDeclaration,

            /// <summary>
            /// </summary>
            Declaration,

            /// <summary>
            /// </summary>
            PostDeclaration,

            /// <summary>
            /// </summary>
            ForwardMethodDeclaration,

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

            GenerateC(
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
        public static void WriteTypeFullDeclaration(ICodeWriter codeWriter, IType type, IGenericContext genericContext)
        {
            codeWriter.WriteTypeStart(type, genericContext);

            var fields = IlReader.Fields(type, codeWriter);

            Debug.Assert(!type.IsGenericTypeDefinition);

            codeWriter.WriteBeforeFields();

            // fields
            foreach (var field in fields)
            {
                codeWriter.WriteField(field);
            }

            codeWriter.WriteAfterFields();
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

                if (!processGenericMethodsOnly && type.IsGenericTypeDefinition)
                {
                    continue;
                }

                IEnumerable<IMethod> genericMethodSpecializatonsForType = null;
                genMethodSpec.TryGetValue(type, out genericMethodSpecializatonsForType);

                if (mode == ConvertingMode.ForwardDeclaration)
                {
                    ConvertForwardTypeDeclaration(codeWriter, type);
                }
                else if (mode == ConvertingMode.Declaration)
                {
                    ConvertTypeDeclaration(codeWriter, type);
                }
                if (mode == ConvertingMode.PostDeclaration)
                {
                    codeWriter.WritePostDeclarations(type);
                }
                else if (mode == ConvertingMode.ForwardMethodDeclaration)
                {
                    ConvertTypeDefinition(
                        codeWriter,
                        type,
                        genericMethodSpecializatonsForType,
                        processGenericMethodsOnly,
                        true);
                }
                else if (mode == ConvertingMode.Definition)
                {
                    ConvertTypeDefinition(
                        codeWriter,
                        type,
                        genericMethodSpecializatonsForType,
                        processGenericMethodsOnly);
                }
            }
        }

        private static void ConvertAllRuntimeTypes(
            ICodeWriter codeWriter,
            IEnumerable<IType> types)
        {
            foreach (var type in types)
            {
                Debug.Assert(type != null);
                if (type == null)
                {
                    continue;
                }

                ConvertRuntimeTypeInfo(
                    codeWriter,
                    type);
            }
        }

        private static void ConvertForwardTypeDeclaration(ICodeWriter codeWriter, IType type)
        {
            IType typeDefinition;
            IType typeSpecialization;
            var genericTypeContext = GetGenericTypeContext(type, out typeDefinition, out typeSpecialization);

            codeWriter.WriteForwardTypeDeclaration(type, genericTypeContext);
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
        private static void ConvertTypeDeclaration(ICodeWriter codeWriter, IType type)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Converting {0} (declarations)", type));
            }

            IType typeDefinition;
            IType typeSpecialization;
            var genericTypeContext = GetGenericTypeContext(type, out typeDefinition, out typeSpecialization);

            WriteTypeFullDeclaration(codeWriter, type, genericTypeContext);
        }

        private static void ConvertTypeDefinition(
            ICodeWriter codeWriter,
            IType type,
            IEnumerable<IMethod> genericMethodSpecializatons,
            bool processGenericMethodsOnly = false,
            bool forwardDeclarations = false)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Converting {0} (definition)"));
            }

            if (!forwardDeclarations)
            {
                codeWriter.WritePostDefinitions(type);
            }

            IType typeDefinition;
            IType typeSpecialization;
            var genericTypeContext = GetGenericTypeContext(type, out typeDefinition, out typeSpecialization);

            if (!processGenericMethodsOnly)
            {
                foreach (var ctor in IlReader.Constructors(type, codeWriter).Select(m => MethodBodyBank.GetMethodWithCustomBodyOrDefault(m, codeWriter)))
                {
                    if (!forwardDeclarations)
                    {
                        codeWriter.WriteMethod(
                            ctor,
                            type.IsGenericType ? ctor.GetMethodDefinition() : null,
                            genericTypeContext);
                    }
                    else
                    {
                        codeWriter.WriteMethodForwardDeclaration(ctor, null);
                    }
                }
            }

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

                    if (!forwardDeclarations)
                    {
                        codeWriter.WriteMethod(
                            method,
                            type.IsGenericType ? method.GetMethodDefinition() : null,
                            genericMethodContext);
                    }
                    else
                    {
                        codeWriter.WriteMethodForwardDeclaration(method, null);
                    }
                }

                if (genericMethodSpecializatons != null && (method.IsGenericMethodDefinition || method.IsGenericMethod))
                {
                    // write all specializations of a method
                    var methodDefinition = method.GetMethodDefinition();
                    foreach (var methodSpecialization in
                        genericMethodSpecializatons.Where(
                            methodSpec => methodSpec.IsMatchingGeneric(methodDefinition) && (!methodSpec.Equals(method) || processGenericMethodsOnly)))
                    {
                        var genericMethodContext = MetadataGenericContext.Create(typeDefinition, typeSpecialization, method, methodSpecialization);

                        if (!forwardDeclarations)
                        {
                            codeWriter.WriteMethod(methodSpecialization, methodDefinition, genericMethodContext);
                        }
                        else
                        {
                            codeWriter.WriteMethodForwardDeclaration(methodSpecialization, null);
                        }
                    }
                }
            }

        }

        private static IGenericContext GetGenericTypeContext(
            IType type,
            out IType typeDefinition,
            out IType typeSpecialization)
        {
            typeDefinition = type.IsGenericType ? type.GetTypeDefinition() : null;
            typeSpecialization = type.IsGenericType && !type.IsGenericTypeDefinition ? type : null;
            return typeDefinition != null || typeSpecialization != null
                ? MetadataGenericContext.Create(typeDefinition, typeSpecialization)
                : null;
        }

        /// <summary>
        /// to support using RuntimeType info to Generic Definitions and Pointers
        /// </summary>
        private static void ConvertRuntimeTypeInfo(ICodeWriter codeWriter, IType type)
        {
            Debug.Assert(type.IsGenericTypeDefinition || type.IsPointer, "This method is for Generic Definitions or pointers only as it should not be processed in notmal way using ConvertType");

            var method = MethodBodyBank.GetMethodWithCustomBodyOrDefault(new SynthesizedGetTypeStaticMethod(type, codeWriter), codeWriter);
            codeWriter.WritePostDefinitions(type, true);
            ConvertMethod(codeWriter, type, method);
        }

        private static void ConvertMethod(ICodeWriter codeWriter, IType type, IMethod method, IMethod methodOpCodeHolder = null)
        {
            IType typeDefinition;
            IType typeSpecialization;
            var genericTypeContext = GetGenericTypeContext(type, out typeDefinition, out typeSpecialization);

            codeWriter.WriteMethod(method, methodOpCodeHolder, genericTypeContext);
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
                        classMethodDefinition.DeclaringType.GetAllInterfaces()
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
        private static void GenerateC(
            IlReader ilReader,
            string fileName,
            string sourceFilePath,
            string pdbFilePath,
            string outputFolder,
            string[] args,
            string[] filter = null)
        {
            concurrent = args != null && args.Any(a => a == "multi");
            split = args != null && args.Any(a => a == "split");
            VerboseOutput = args != null && args.Any(a => a == "verbose");

            var settings = new Settings()
                               {
                                   FileName = fileName,
                                   SourceFilePath = sourceFilePath,
                                   PdbFilePath = pdbFilePath,
                                   OutputFolder = outputFolder,
                                   Args = args,
                                   Filter = filter
                               };
            if (!split)
            {
                GenerateSource(ilReader, settings);
            }
            else
            {
                // generate file for each namespace
                var namespaces = ilReader.Types().Select(t => t.Namespace).Distinct().ToList();
                if (!namespaces.Contains(string.Empty))
                {
                    namespaces.Add(string.Empty);
                }

                GenerateMultiSources(ilReader, namespaces, settings);
            }
        }

        private static ICodeWriter GetCodeWriter(IlReader ilReader, Settings settings, bool isHeader = false)
        {
            var codeWriter = GetCWriter(settings.FileName, settings.FileExt, settings.SourceFilePath, settings.PdbFilePath, settings.OutputFolder, settings.Args);
            codeWriter.IsHeader = isHeader;
            ilReader.TypeResolver = codeWriter;
            codeWriter.IlReader = ilReader;
            _codeWriter = codeWriter;
            _codeWriter.Initialize(ilReader.Types().First());
            return codeWriter;
        }

        /// <summary>
        /// </summary>
        /// <param name="ilReader">
        /// </param>
        /// <param name="filter">
        /// </param>
        /// <param name="codeWriter">
        /// </param>
        private static void GenerateSource(IlReader ilReader, Settings settings)
        {
            settings.FileExt = ".h";
            var codeHeaderWriter = GetCodeWriter(ilReader, settings, true);

            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted;
            var sortedListOfTypes = ReadingTypes(
                ilReader,
                settings.Filter,
                out genericMethodSpecializationsSorted);

            WritingDeclarations(ilReader, codeHeaderWriter, sortedListOfTypes, genericMethodSpecializationsSorted);

            settings.FileExt = ".cpp";
            var codeWriter = GetCodeWriter(ilReader, settings);
            WritingDefinitions(ilReader, codeWriter, sortedListOfTypes, genericMethodSpecializationsSorted);
        }

        private static void GenerateMultiSources(IlReader ilReader, IEnumerable<string> namespaces, Settings settings)
        {
            settings.FileExt = ".h";
            var codeHeaderWriter = GetCodeWriter(ilReader, settings);

            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted;
            var sortedListOfTypes = ReadingTypes(
                ilReader,
                settings.Filter,
                out genericMethodSpecializationsSorted);

            WritingDeclarations(
                ilReader,
                codeHeaderWriter,
                sortedListOfTypes,
                genericMethodSpecializationsSorted);

            var fileName = settings.FileName;

            foreach (var ns in namespaces)
            {
                settings.FileName = string.Concat(fileName, "_", string.IsNullOrEmpty(ns) ? "no_namespace" : ns.CleanUpName());
                settings.FileExt = ".cpp";

                var codeWriterForNameSpace = GetCodeWriter(ilReader, settings);
                WritingDefinitions(ilReader, codeWriterForNameSpace, sortedListOfTypes.Where(t => t.Namespace == ns).ToList(), genericMethodSpecializationsSorted);
            }
        }

        public static IEnumerable<IType> GetRequiredForwardDeclarationTypes(IType typeSource)
        {
            Debug.Assert(typeSource != null, "Type is null");

            lock (cachedRequiredForwardDeclarationTypes)
            {
                IEnumerable<IType> cachedQuery;
                if (cachedRequiredForwardDeclarationTypes.TryGetValue(typeSource, out cachedQuery))
                {
                    return cachedQuery;
                }
            }

            var query = IterateRequiredForwardDeclarationTypes(typeSource).ToList();

            lock (cachedRequiredForwardDeclarationTypes)
            {
                if (!cachedRequiredForwardDeclarationTypes.ContainsKey(typeSource))
                {
                    cachedRequiredForwardDeclarationTypes.Add(typeSource, query);
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
        public static IEnumerable<IType> GetRequiredFullDeclarationTypes(IType typeSource)
        {
            Debug.Assert(typeSource != null, "Type is null");

            lock (cachedRequiredFullDeclarationTypes)
            {
                IEnumerable<IType> cachedQuery;
                if (cachedRequiredFullDeclarationTypes.TryGetValue(typeSource, out cachedQuery))
                {
                    return cachedQuery;
                }
            }

            var query = IterateRequiredFullDeclarationTypes(typeSource).ToList();

            lock (cachedRequiredFullDeclarationTypes)
            {
                if (!cachedRequiredFullDeclarationTypes.ContainsKey(typeSource))
                {
                    cachedRequiredFullDeclarationTypes.Add(typeSource, query);
                }
            }

            return query;
        }

        private static IEnumerable<IType> IterateRequiredFullDeclarationTypes(IType type)
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

            var fields = IlReader.Fields(type, IlReader.DefaultFlags, _codeWriter);
            foreach (var field in fields.Where(field => !field.IsStatic && field.FieldType.IsStructureType() && !field.FieldType.IsPointer))
            {
                yield return field.FieldType;
            }
        }

        private static IEnumerable<IType> IterateRequiredForwardDeclarationTypes(IType type)
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
        private static ICodeWriter GetCWriter(
            string fileName,
            string fileExt,
            string sourceFilePath,
            string pdbFilePath,
            string outputFolder,
            string[] args)
        {
            return new CWriter(Path.Combine(outputFolder, fileName), fileExt, sourceFilePath, pdbFilePath, args);
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
            cachedRequiredFullDeclarationTypes.Clear();
            cachedRequiredForwardDeclarationTypes.Clear();

            // clean it as you are using IlReader
            IlReader.GenericMethodSpecializations = null;

            // types in current assembly
            var readingTypesContext = ReadingTypesContext.New();
            var types = ilReader.Types().Where(t => !t.IsGenericTypeDefinition && t.Name != "<Module>");
            if (filter != null)
            {
                types = types.Where(t => CheckFilter(filter, t));
            }

            var allTypes = ilReader.AllTypes().ToList();

            var usedTypes = FindUsedTypes(types.ToList(), allTypes, readingTypesContext);

            genericMethodSpecializationsSorted = GroupGenericMethodsByType(readingTypesContext.GenericMethodSpecializations);

            Debug.Assert(usedTypes.All(t => !t.IsByRef), "Type is used with flag IsByRef");
            Debug.Assert(usedTypes.All(t => !t.IsPointer), "Type is used with flag IsPointer");

            return usedTypes;
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

            var assemblyQualifiedName = types.First().AssemblyQualifiedName;

            var list = new List<IType>();
            var usedTypesForOrder = new NamespaceContainer<IType>();
            foreach (var usedType in usedTypes)
            {
                AddType(list, usedTypesForOrder, usedType, assemblyQualifiedName);
            }

            return list;
        }

        private static void AddType(IList<IType> order, ISet<IType> usedTypes, IType type, string assemblyQualifiedName)
        {
            if (type == null || (type.AssemblyQualifiedName != assemblyQualifiedName && !(type.IsArray || type.IsGenericType)) || usedTypes.Contains(type))
            {
                return;
            }

            // add base type first
            AddType(order, usedTypes, type.BaseType, assemblyQualifiedName);

            // then all interfaces
            foreach (var @interface in type.GetInterfaces())
            {
                AddType(order, usedTypes, @interface, assemblyQualifiedName);
            }

            // then all structures in fields
            foreach (var field in type.GetFields(IlReader.DefaultFlags).Where(f => !f.IsStatic && !f.IsConst && f.FieldType.IsStructureType()))
            {
                AddType(order, usedTypes, field.FieldType, assemblyQualifiedName);
            }

            // add type
            if (usedTypes.Add(type))
            {
                order.Add(type);
            }
        }

        private static void WritingDeclarations(
            IlReader ilReader,
            ICodeWriter codeHeaderWriter,
            IEnumerable<IType> sortedListOfTypes,
            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted)
        {
            // we need to generate sgettype for Generic definitions
            ilReader.UsedTypeTokens = ilReader.UsedTypeTokens ?? new NamespaceContainer<IType>();

            // writing
            codeHeaderWriter.WriteStart();

            ConvertAllTypes(
                codeHeaderWriter,
                sortedListOfTypes,
                genericMethodSpecializationsSorted,
                ConvertingMode.ForwardDeclaration);

            ConvertAllTypes(
                codeHeaderWriter,
                sortedListOfTypes,
                genericMethodSpecializationsSorted,
                ConvertingMode.Declaration);

            ConvertAllTypes(
                codeHeaderWriter,
                sortedListOfTypes,
                genericMethodSpecializationsSorted,
                ConvertingMode.PostDeclaration);

            ConvertAllTypes(
                codeHeaderWriter,
                sortedListOfTypes,
                genericMethodSpecializationsSorted,
                ConvertingMode.ForwardMethodDeclaration);

            codeHeaderWriter.WriteEnd();

            codeHeaderWriter.Close();
        }

        private static void WritingDefinitions(
            IlReader ilReader,
            ICodeWriter codeWriter,
            IEnumerable<IType> types,
            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted)
        {
            // we need to generate sgettype for Generic definitions
            ilReader.UsedTypeTokens = ilReader.UsedTypeTokens ?? new NamespaceContainer<IType>();

            // writing
            codeWriter.WriteStart();

            ConvertAllTypes(
                codeWriter,
                types,
                genericMethodSpecializationsSorted,
                ConvertingMode.Definition);

            // Append definition of Generic Methods of not used non-generic types
            ConvertAllTypes(
                codeWriter,
                genericMethodSpecializationsSorted.Keys.Where(k => !types.Contains(k)).ToList(),
                genericMethodSpecializationsSorted,
                ConvertingMode.Definition,
                true);

            // Append not generated sgettypes
            ConvertAllRuntimeTypes(
                codeWriter,
                ilReader.UsedTypeTokens.Where(k => (k.IsGenericTypeDefinition || k.IsPointer) && !types.Contains(k)).ToList());

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

    public class Settings
    {
        public string FileName { get; set; }

        public string FileExt { get; set; }

        public string SourceFilePath { get; set; }

        public string PdbFilePath { get; set; }

        public string OutputFolder { get; set; }

        public string[] Args { get; set; }

        public string[] Filter { get; set; }
    }
}
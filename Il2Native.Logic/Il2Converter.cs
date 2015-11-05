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
    using System.ComponentModel.Design;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Gencode;

    using Il2Native.Logic.Gencode.SynthesizedMethods;
    using Il2Native.Logic.Properties;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class Il2Converter
    {
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
            PreDeclaration,

            /// <summary>
            /// </summary>
            Declaration,

            /// <summary>
            /// </summary>
            PostDeclaration,

            /// <summary>
            /// </summary>
            PreDefinition,

            /// <summary>
            /// </summary>
            Definition,

            /// <summary>
            /// </summary>
            PostDefinition
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
        public static void WriteTypeFullDeclaration(
            ICodeWriter codeWriter,
            IType type,
            IGenericContext genericContext,
            IEnumerable<IMethod> genericMethodSpecializatons,
            MergeTypeContext mergeType,
            bool processGenericMethodsOnly = false)
        {
            if (!processGenericMethodsOnly)
            {
                WriteTypeDefinition(codeWriter, type, genericContext);

                // if it is Struct we need to generate struct Data
                if (type.IsStructureType())
                {
                    WriteTypeDefinition(codeWriter, type.ToClass(), genericContext);
                }
            }

            codeWriter.WriteBeforeMethods(type);

            ConvertTypeDefinition(codeWriter, type, genericMethodSpecializatons, mergeType, processGenericMethodsOnly, true);

            codeWriter.WriteAfterMethods(type);

            if (!processGenericMethodsOnly)
            {
                codeWriter.WriteTypeEnd(type);
            }
        }

        private static void WriteTypeDefinition(ICodeWriter codeWriter, IType type, IGenericContext genericContext)
        {
            codeWriter.WriteTypeStart(type, genericContext);

            var fields = IlReader.Fields(type, codeWriter);

            Debug.Assert(!type.IsGenericTypeDefinition);

            codeWriter.WriteBeforeFields();

            if (!type.IsStructureType())
            {
                codeWriter.WriteInheritance();
            }

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
            ReadTypesContext readTypes,
            ConvertingMode mode,
            bool processGenericMethodsOnly = false)
        {
            foreach (var type in readTypes.UsedTypes.Where(t => t != null))
            {
                if (!processGenericMethodsOnly && type.IsGenericTypeDefinition)
                {
                    continue;
                }

                // get method specializations
                IEnumerable<IMethod> genericMethodSpecializatonsForType = null;
                readTypes.GenericMethodSpecializations.TryGetValue(type, out genericMethodSpecializatonsForType);

                // get megre parts
                MergeTypeContext mergeType = null;
                if (readTypes.MergeTypes != null)
                {
                    readTypes.MergeTypes.TryGetValue(type, out mergeType);
                }

                if (mode == ConvertingMode.ForwardDeclaration)
                {
                    ConvertForwardTypeDeclaration(codeWriter, type);
                }
                else if (mode == ConvertingMode.PreDeclaration)
                {
                    codeWriter.WritePreDeclarations(type);
                }
                else if (mode == ConvertingMode.Declaration)
                {
                    ConvertTypeDeclaration(
                        codeWriter,
                        type,
                        genericMethodSpecializatonsForType,
                        mergeType,
                        processGenericMethodsOnly);
                }
                else if (mode == ConvertingMode.PostDeclaration)
                {
                    codeWriter.WritePostDeclarations(type);
                }
                else if (mode == ConvertingMode.PreDefinition)
                {
                    codeWriter.WritePreDefinitions(type);
                }
                else if (mode == ConvertingMode.Definition)
                {
                    ConvertTypeDefinition(
                        codeWriter,
                        type,
                        genericMethodSpecializatonsForType,
                        mergeType,
                        processGenericMethodsOnly);
                }
                else if (mode == ConvertingMode.PostDefinition)
                {
                    codeWriter.WritePostDefinitions(type);
                }
            }
        }

        private static void ConvertAllRuntimeTypesForwardDeclaration(
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

                ConvertRuntimeTypeInfoForwardDeclaration(
                    codeWriter,
                    type);
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

            // if it is Struct we need to generate struct Data
            if (type.IsStructureType())
            {
                codeWriter.WriteForwardTypeDeclaration(type.ToClass(), genericTypeContext);
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
        private static void ConvertTypeDeclaration(
            ICodeWriter codeWriter,
            IType type,
            IEnumerable<IMethod> genericMethodSpecializatons,
            MergeTypeContext mergeType,
            bool processGenericMethodsOnly = false)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Converting {0} (declarations)", type));
            }

            IType typeDefinition;
            IType typeSpecialization;
            var genericTypeContext = GetGenericTypeContext(type, out typeDefinition, out typeSpecialization);

            WriteTypeFullDeclaration(codeWriter, type, genericTypeContext, genericMethodSpecializatons, mergeType, processGenericMethodsOnly);
        }

        private static void ConvertTypeDefinition(
            ICodeWriter codeWriter,
            IType type,
            IEnumerable<IMethod> genericMethodSpecializatons,
            MergeTypeContext mergeType,
            bool processGenericMethodsOnly = false,
            bool forwardDeclarations = false)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Converting {0} (definition)", type));
            }

            ////codeWriter.WriteBeforeMethods(type);

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
                        codeWriter.WriteMethodForwardDeclaration(ctor, null, genericTypeContext);
                    }
                }
            }

            foreach (
                var method in
                    IlReader.Methods(type, codeWriter, true).Select(m => MethodBodyBank.GetMethodWithCustomBodyOrDefault(m, codeWriter)))
            {
                if (forwardDeclarations && type.IsInterface && !method.IsStatic)
                {
                    continue;
                }

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
                        codeWriter.WriteMethodForwardDeclaration(method, null, genericMethodContext);
                    }
                }

                if (genericMethodSpecializatons != null && (method.IsGenericMethodDefinition || method.IsGenericMethod))
                {
                    // write all specializations of a method
                    var methodDefinition = method.GetMethodDefinition();
                    foreach (var methodSpecialization in
                        genericMethodSpecializatons.Where(
                            methodSpec => methodSpec.GetMethodDefinition().IsMatchingGeneric(methodDefinition, true) && (!methodSpec.Equals(method) || processGenericMethodsOnly)))
                    {
                        var genericMethodContext = MetadataGenericContext.Create(
                            typeDefinition, typeSpecialization, methodSpecialization.GetMethodDefinition(), methodSpecialization);

                        if (!forwardDeclarations)
                        {
                            codeWriter.WriteMethod(
                                methodSpecialization,
                                MethodBodyBank.GetMethodWithCustomBodyOrDefault(methodSpecialization.GetMethodDefinition(), codeWriter),
                                genericMethodContext);
                        }
                        else
                        {
                            codeWriter.WriteMethodForwardDeclaration(methodSpecialization, null, genericMethodContext);
                        }

                        // write struct object adapters for struct types with generic methods
                        if (type.IsValueType() && methodSpecialization.ShouldHaveStructToObjectAdapter())
                        {
                            var structObjectAdapter = ObjectInfrastructure.GetInvokeWrapperForStructUsedInObject(methodSpecialization, codeWriter);
                            if (!forwardDeclarations)
                            {
                                codeWriter.WriteMethod(
                                    structObjectAdapter,
                                    MethodBodyBank.GetMethodWithCustomBodyOrDefault(structObjectAdapter.GetMethodDefinition(), codeWriter),
                                    genericMethodContext);
                            }
                            else
                            {
                                codeWriter.WriteMethodForwardDeclaration(structObjectAdapter, null, genericMethodContext);
                            }
                        }
                    }
                }
            }

            // merge methods
            if (mergeType != null)
            {
                if (!forwardDeclarations)
                {
                    foreach (var mergedMethod in mergeType.MissingMethods)
                    {
                        var genericMethodContext = mergedMethod.IsGenericMethod
                                                       ? MetadataGenericContext.Create(
                                                           typeDefinition, typeSpecialization, mergedMethod.GetMethodDefinition(), mergedMethod)
                                                       : genericTypeContext;

                        codeWriter.WriteMethod(mergedMethod, null, genericMethodContext);
                    }
                }
                else
                {
                    foreach (var missingMethod in mergeType.MissingMethods)
                    {
                        var genericMethodContext = missingMethod.IsGenericMethod
                                                       ? MetadataGenericContext.Create(
                                                           typeDefinition, typeSpecialization, missingMethod.GetMethodDefinition(), missingMethod)
                                                       : genericTypeContext;

                        codeWriter.WriteMethodForwardDeclaration(missingMethod, null, genericMethodContext);
                    }
                }
            }

            ////codeWriter.WriteAfterMethods(type);
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

        private static void ConvertRuntimeTypeInfoForwardDeclaration(ICodeWriter codeWriter, IType type)
        {
            var fields = IlReader.Fields(type, codeWriter);
            // fields
            foreach (var field in fields.Where(f => f.Name == RuntimeTypeInfoGen.RuntimeTypeHolderFieldName))
            {
                codeWriter.WriteStaticField(field, false);
            }
        }

        /// <summary>
        /// to support using RuntimeType info to Generic Definitions and Pointers
        /// </summary>
        private static void ConvertRuntimeTypeInfo(ICodeWriter codeWriter, IType type)
        {
            Debug.Assert(type.IsGenericTypeDefinition || type.IsPointer, "This method is for Generic Definitions or pointers only as it should not be processed in notmal way using ConvertType");

            var fields = IlReader.Fields(type, codeWriter);

            foreach (var field in fields.Where(f => f.Name == RuntimeTypeInfoGen.RuntimeTypeHolderFieldName))
            {
                codeWriter.WriteStaticField(field, typeForRuntimeTypeInfo: type);
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

        private static void AddTypeIfTypeOrAdditionalType(IType type, ReadingTypesContext readingTypesContext)
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

            readingTypesContext.UsedTypes.Add(effectiveType);
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

            AddUsedTokenType(readingTypesContext, type);

            foreach (var @interface in type.GetInterfaces())
            {
                DiscoverGenericSpecializedTypesAndAdditionalTypes(@interface, readingTypesContext);
            }

            if (!type.IsInterface)
            {
                var fields = IlReader.Fields(type, IlReader.DefaultFlags, _codeWriter);
                foreach (var field in fields)
                {
                    AddTypeIfSpecializedTypeOrAdditionalType(field.FieldType, readingTypesContext);
                    //DiscoverGenericSpecializedTypesAndAdditionalTypes(field.FieldType, readingTypesContext);
                }

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

        private static void AddUsedTokenType(ReadingTypesContext readingTypesContext, IType type)
        {
            if (readingTypesContext == null)
            {
                return;
            }

            if (readingTypesContext.UsedTypeTokens == null)
            {
                return;
            }

            readingTypesContext.UsedTypeTokens.Add(type);

            if (type.BaseType != null)
            {
                AddUsedTokenType(readingTypesContext, type.BaseType);
            }

            if (type.HasElementType)
            {
                AddUsedTokenType(readingTypesContext, type.GetElementType());
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
            var types = allTypes.Where(t => !t.IsGenericTypeDefinition && t.GetAllInterfaces().Contains(specializedTypeMethods.Key)).ToList();
            foreach (var interfaceMethodSpecialization in specializedTypeMethods)
            {
                var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
                foreach (var classMethodDefinition in
                    types.SelectMany(
                        t =>
                            t.GetMethods(flags)
                                .Where(m => m.IsGenericMethodDefinition && m.IsMatchingOverrideOrExplicitInterface(interfaceMethodSpecialization))))
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
                        readingTypesContext.UsedTypeTokens,
                        null,
                        null,
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
                    allTypes.Where(t => !t.IsGenericTypeDefinition).SelectMany(
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
                    readingTypesContext.UsedTypeTokens,
                    null,
                    null,
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
                var namespaces = ilReader.Types().Where(t => !string.IsNullOrEmpty(t.Namespace)).Select(t => t.Namespace).Distinct().ToList();
                GenerateMultiSources(ilReader, namespaces, settings);
            }
        }

        private static ICodeWriter GetCodeWriter(IlReader ilReader, Settings settings, bool isHeader = false)
        {
            var codeWriter = GetCWriter(settings.FileName, settings.FileExt, settings.SourceFilePath, settings.PdbFilePath, settings.OutputFolder, settings.Args);
            MethodBodyBank.Reset();
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

            var readTypes = ReadingTypes(
                ilReader,
                settings.Filter);

            WritingDeclarations(ilReader, codeHeaderWriter, readTypes);

            settings.FileExt = ".cpp";
            var codeWriter = GetCodeWriter(ilReader, settings);
            codeWriter.FileHeader = settings.FileName;
            WritingDefinitions(ilReader, codeWriter, readTypes);
        }

        private static void GenerateMultiSources(IlReader ilReader, IEnumerable<string> namespaces, Settings settings)
        {
            settings.FileExt = ".h";
            var codeHeaderWriter = GetCodeWriter(ilReader, settings, true);

            var readTypes = ReadingTypes(
                ilReader,
                settings.Filter);

            WritingDeclarations(
                ilReader,
                codeHeaderWriter,
                readTypes);

            var fileName = settings.FileName;

            foreach (var ns in namespaces)
            {
                GenerateSourceForNamespace(ilReader, settings, fileName, ns, readTypes);
            }

            // very important step to generate code for empty space
            GenerateSourceForNamespace(ilReader, settings, fileName, string.Empty, readTypes);
        }

        private static void GenerateSourceForNamespace(
            IlReader ilReader, Settings settings, string fileName, string ns, ReadTypesContext readTypes)
        {
            settings.FileName = string.Concat(fileName, "_", string.IsNullOrEmpty(ns) ? "no_namespace" : ns.CleanUpName());
            settings.FileExt = ".cpp";

            var codeWriterForNameSpace = GetCodeWriter(ilReader, settings);
            codeWriterForNameSpace.IsSplit = true;
            codeWriterForNameSpace.SplitNamespace = ns;
            codeWriterForNameSpace.FileHeader = fileName;

            var readTypesByNamespace = readTypes.Clone();
            readTypesByNamespace.UsedTypes = readTypes.UsedTypes.Where(t => t.Namespace == ns).ToList();

            WritingDefinitions(ilReader, codeWriterForNameSpace, readTypesByNamespace);
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
                    readingTypesContext.UsedTypeTokens,
                    null,
                    null,
                    new Queue<IMethod>(),
                    _codeWriter);
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
            subSetReadingContext.UsedTypeTokens = readingTypesContext.UsedTypeTokens;
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
                Debug.Assert(!type.IsGenericTypeDefinition);
                requiredTypesByType.Add(type);
            }
        }

        private static ReadTypesContext ReadingTypes(
            IlReader ilReader,
            string[] filter)
        {
            // clean it as you are using IlReader
            IlReader.GenericMethodSpecializations = null;

            // types in current assembly
            var readTypesContext = ReadTypesContext.New();
            var readingTypesContext = ReadingTypesContext.New();

            var allTypes = ilReader.AllTypes().ToList();

            var typeDict = new SortedDictionary<string, IType>();
            foreach (var type in allTypes.Where(t => !t.IsInternal))
            {
                var key = type.ToString();
                if (!typeDict.ContainsKey(key))
                {
                    typeDict.Add(key, type);
                }
            }

            var typesToGet = ilReader.Types().Where(t => !t.IsGenericTypeDefinition);
            if (!ilReader.IsCoreLib)
            {
                typesToGet = typesToGet.Where(t => CheckFilter(filter, t, typeDict));
            }

            var types = typesToGet.ToList();

            readTypesContext.UsedTypes = FindUsedTypes(types, allTypes, readingTypesContext, ilReader.TypeResolver);
            readTypesContext.GenericMethodSpecializations = GroupGenericMethodsByType(readingTypesContext.GenericMethodSpecializations);

            var genericMethodSpecializations = readTypesContext.GenericMethodSpecializations;
            IlReader.GenericMethodSpecializations = genericMethodSpecializations;
            ilReader.UsedTypeTokens = readingTypesContext.UsedTypeTokens;

            Debug.Assert(readTypesContext.UsedTypes.All(t => !t.IsByRef), "Type is used with flag IsByRef");
            Debug.Assert(readTypesContext.UsedTypes.All(t => !t.IsPointer), "Type is used with flag IsPointer");
            Debug.Assert(readTypesContext.UsedTypes.All(t => !t.IsGenericTypeDefinition), "Generic DefinitionType is used");

            // to support compact mode
            DiscoverAllCalledMethod(types, readingTypesContext, ilReader.TypeResolver);

            return readTypesContext;
        }

        private static void DiscoverAllCalledMethod(
            List<IType> types,
            ReadingTypesContext readingTypesContext,
            ITypeResolver typeResolver)
        {
            var queue = new Queue<IMethod>();
            var used = new object();
            foreach (var method in types.SelectMany(t => IlReader.Methods(t, typeResolver)))
            {
                readingTypesContext.CalledMethods.Add(new MethodKey(method, null));
            }

            // check all methods
            var countBefore = 0;
            do
            {
                countBefore = readingTypesContext.CalledMethods.Count;
                foreach (var methodKey in readingTypesContext.CalledMethods.Where(m => m.Tag == null))
                {
                    methodKey.Tag = used;
                    methodKey.Method.DiscoverMethodsInMethodBody(readingTypesContext.CalledMethods, queue, typeResolver);
                }
            } 
            while (readingTypesContext.CalledMethods.Count != countBefore);

            Debug.Assert(false);
        }

        ////private static IType LoadNativeTypeFromSource(IIlReader ilReader, string assemblyName = null)
        ////{
        ////    return ilReader.CompileSourceWithRoslyn(assemblyName, Resources.NativeType).First(t => !t.IsModule);
        ////}

        private static bool CheckFilter(string[] filters, IType type, IDictionary<string, IType> allTypes)
        {
            if (allTypes != null && !type.IsModule && !type.IsPrivateImplementationDetails)
            {
                IType existringType;
                if (allTypes.TryGetValue(type.ToString(), out existringType) && existringType != null &&
                    existringType.AssemblyQualifiedName != type.AssemblyQualifiedName)
                {
                    return false;
                }
            }

            if (filters == null || filters.Length == 0)
            {
                return true;
            }

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
        private static IList<IType> FindUsedTypes(IEnumerable<IType> types, IList<IType> allTypes, ReadingTypesContext readingTypesContext, ITypeResolver typeResolver)
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

            var genericMethodSpecializations = 0;
            var genericTypeSpecializations = 0;
            var additionalTypesToProcess = 0;

            // repeat it until all types processed
            while (genericMethodSpecializations != readingTypesContext.GenericMethodSpecializations.Count
                   || genericTypeSpecializations != readingTypesContext.GenericTypeSpecializations.Count
                   || additionalTypesToProcess != readingTypesContext.AdditionalTypesToProcess.Count)
            {
                genericMethodSpecializations = readingTypesContext.GenericMethodSpecializations.Count;
                genericTypeSpecializations = readingTypesContext.GenericTypeSpecializations.Count;
                additionalTypesToProcess = readingTypesContext.AdditionalTypesToProcess.Count;

                DiscoverAllGenericVirtualMethods(allTypes, readingTypesContext);
                DiscoverAllGenericVirtualMethods(readingTypesContext.GenericTypeSpecializations, readingTypesContext);
                DiscoverAllGenericVirtualMethods(readingTypesContext.AdditionalTypesToProcess, readingTypesContext);

                DiscoverAllGenericMethodsOfInterfaces(allTypes, readingTypesContext);
                DiscoverAllGenericMethodsOfInterfaces(readingTypesContext.GenericTypeSpecializations, readingTypesContext);
                DiscoverAllGenericMethodsOfInterfaces(readingTypesContext.AdditionalTypesToProcess, readingTypesContext);

                ProcessGenericTypesAndAdditionalTypesToDiscoverGenericSpecializedTypesAndAdditionalTypes(usedTypes, readingTypesContext, true);
            }

            var assemblyQualifiedName = types.First().AssemblyQualifiedName;

            var list = new List<IType>();
            var usedTypesForOrder = new NamespaceContainer<IType>();
            foreach (var usedType in usedTypes)
            {
                AddTypeInOrderOfUsage(list, usedTypesForOrder, usedType, assemblyQualifiedName, typeResolver);
            }

            return list;
        }

        private static void AddTypeInOrderOfUsage(IList<IType> order, ISet<IType> usedTypes, IType type, string assemblyQualifiedName, ITypeResolver typeResolver)
        {
            if (type == null || type.IsPointer || type.IsByRef || ((assemblyQualifiedName != null && type.AssemblyQualifiedName != assemblyQualifiedName) && !(type.IsArray || type.IsGenericType)) || usedTypes.Contains(type))
            {
                return;
            }

            // add base type first
            AddTypeInOrderOfUsage(order, usedTypes, type.BaseType, assemblyQualifiedName, typeResolver);

            // then all interfaces
            foreach (var @interface in type.GetInterfaces())
            {
                AddTypeInOrderOfUsage(order, usedTypes, @interface, assemblyQualifiedName, typeResolver);
            }

            // then all structures in fields
            foreach (var field in IlReader.Fields(type, typeResolver).Where(f => !f.IsStatic && !f.IsConst && (f.FieldType.IsStructureType() || f.IsFixed)))
            {
                if (field.IsFixed && field.FieldType.GetElementType().IsStructureType())
                {
                    AddTypeInOrderOfUsage(order, usedTypes, field.FieldType.GetElementType(), assemblyQualifiedName, typeResolver);
                }
                else
                {
                    AddTypeInOrderOfUsage(order, usedTypes, field.FieldType, assemblyQualifiedName, typeResolver);
                }
            }

            // add type
            if (usedTypes.Add(type))
            {
                Debug.Assert(!type.IsPointer && !type.IsByRef && !type.IsGenericTypeDefinition, "Not allowble type here");
                order.Add(type);
            }
        }

        private static void WritingDeclarations(
            IlReader ilReader,
            ICodeWriter codeHeaderWriter,
            ReadTypesContext readTypes)
        {
            var fullNames = readTypes.UsedTypes.Select(t => t.FullName).ToList();
            var runtimeTypes = ilReader.UsedTypeTokens.Where(k => (k.IsGenericTypeDefinition || k.IsPointer) && !fullNames.Contains(k.FullName)).ToList();

            // writing
            codeHeaderWriter.WriteStart();

            ConvertAllTypes(
                codeHeaderWriter,
                readTypes,
                ConvertingMode.ForwardDeclaration);

            // Append not generated sgettypes
            ConvertAllRuntimeTypesForwardDeclaration(
                codeHeaderWriter,
                runtimeTypes);

            ConvertAllTypes(
                codeHeaderWriter,
                readTypes,
                ConvertingMode.PreDeclaration);

            WriteTypesWithGenericsStep(
                codeHeaderWriter,
                readTypes,
                ConvertingMode.Declaration);

            ConvertAllTypes(
                codeHeaderWriter,
                readTypes,
                ConvertingMode.PostDeclaration);

            codeHeaderWriter.WriteEnd();

            codeHeaderWriter.Close();
        }

        private static void WritingDefinitions(
            IlReader ilReader,
            ICodeWriter codeWriter,
            ReadTypesContext readTypes)
        {
            // writing
            codeWriter.WriteStart();

            WriteTypesWithGenericsStep(codeWriter, readTypes, ConvertingMode.PreDefinition);

            WriteTypesWithGenericsStep(codeWriter, readTypes, ConvertingMode.Definition);

            WriteTypesWithGenericsStep(codeWriter, readTypes, ConvertingMode.PostDefinition);

            if (!codeWriter.IsSplit || codeWriter.IsSplit && string.IsNullOrWhiteSpace(codeWriter.SplitNamespace))
            {
                // Append not generated sgettypes
                var fullNames = readTypes.UsedTypes.Select(t => t.FullName).ToList();
                var runtimeTypes = ilReader.UsedTypeTokens.Where(k => (k.IsGenericTypeDefinition || k.IsPointer) && !fullNames.Contains(k.FullName)).ToList();

                ConvertAllRuntimeTypes(codeWriter, runtimeTypes);
            }

            codeWriter.WriteEnd();

            codeWriter.Close();
        }

        private static void WriteTypesWithGenericsStep(
            ICodeWriter codeWriter,
            ReadTypesContext readTypes,
            ConvertingMode step)
        {
            ConvertAllTypes(
                codeWriter,
                readTypes,
                step);

            WriteLeftGenericMethodsStep(
                codeWriter,
                readTypes,
                step);
        }

        private static void WriteLeftGenericMethodsStep(
            ICodeWriter codeWriter,
            ReadTypesContext readTypes,
            ConvertingMode step)
        {
            var nonGenericReadTypes = readTypes.Clone();

            if (codeWriter.IsHeader || !codeWriter.IsSplit)
            {
                nonGenericReadTypes.UsedTypes =
                    readTypes.GenericMethodSpecializations.Keys.Where(k => !readTypes.UsedTypes.Contains(k)).ToList();

                // Append definition of Generic Methods of not used non-generic types
                ConvertAllTypes(
                    codeWriter,
                    nonGenericReadTypes,
                    step,
                    true);
            }
            else if (codeWriter.IsSplit)
            {
                nonGenericReadTypes.UsedTypes =
                    readTypes.GenericMethodSpecializations.Keys.Where(
                        k => k.Namespace == codeWriter.SplitNamespace && !readTypes.UsedTypes.Contains(k)).ToList();

                // Append definition of Generic Methods of not used non-generic types
                ConvertAllTypes(
                    codeWriter,
                    nonGenericReadTypes,
                    step,
                    true);
            }
        }
    }

    public class ReadingTypesContext
    {
        public ReadingTypesContext()
        {
            this.GenericTypeSpecializations = new NamespaceContainer<IType>();
            this.GenericMethodSpecializations = new NamespaceContainer<IMethod>();
            this.AdditionalTypesToProcess = new NamespaceContainer<IType>();
            this.UsedTypeTokens = new NamespaceContainer<IType>();
            this.DiscoveredTypes = new NamespaceContainer<IType>();
            this.UsedTypes = new NamespaceContainer<IType>();
            this.CalledMethods = new NamespaceContainer<MethodKey>();
        }

        public ISet<IType> GenericTypeSpecializations { get; set; }

        public ISet<IMethod> GenericMethodSpecializations { get; set; }

        public ISet<IType> AdditionalTypesToProcess { get; set; }

        public ISet<IType> UsedTypeTokens { get; set; }

        public ISet<IType> DiscoveredTypes { get; set; }

        public ISet<IType> UsedTypes { get; set; }

        public ISet<MethodKey> CalledMethods { get; set; }

        public static ReadingTypesContext New()
        {
            return new ReadingTypesContext();
        }
    }

    public class ReadTypesContext
    {
        public IList<IType> UsedTypes { get; set; }

        public IDictionary<IType, IEnumerable<IMethod>> GenericMethodSpecializations { get; set; }

        public IDictionary<IType, MergeTypeContext> MergeTypes { get; set; }

        public static ReadTypesContext New()
        {
            return new ReadTypesContext();
        }

        public ReadTypesContext Clone()
        {
            return (ReadTypesContext)this.MemberwiseClone();
        }
    }

    public class MergeTypeContext
    {
        public IEnumerable<IMethod> MethodsWithBody { get; set; }

        public IEnumerable<IMethod> MissingMethods { get; set; }

        public IEnumerable<IField> MissingFields { get; set; }

        public static MergeTypeContext New()
        {
            return new MergeTypeContext();
        }

        public MergeTypeContext Clone()
        {
            return (MergeTypeContext)this.MemberwiseClone();
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
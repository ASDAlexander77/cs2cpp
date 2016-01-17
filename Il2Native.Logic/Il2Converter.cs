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
    using Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class Il2Converter
    {
        private ICodeWriterEx _codeWriter;

        private bool concurrent;

        private bool split;

        private bool compact;

        private bool headers;

        private bool isCoreLib;

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

        public static void Convert(string source, string outputFolder, string[] args = null, string[] filter = null)
        {
            new Il2Converter().ConvertInternal(new[] { source }, outputFolder, args, filter);
        }

        public static void Convert(string[] sources, string outputFolder, string[] args = null, string[] filter = null)
        {
            new Il2Converter().ConvertInternal(sources, outputFolder, args, filter);
        }

        /// <summary>
        /// </summary>
        /// <param name="sources">
        /// </param>
        /// <param name="outputFolder">
        /// </param>
        /// <param name="args">
        /// </param>
        protected void ConvertInternal(string[] sources, string outputFolder, string[] args = null, string[] filter = null)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(sources.First());

            var ilReader = new IlReader(sources, args);
            ilReader.Load();
            isCoreLib = ilReader.IsCoreLib;

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
        public void WriteTypeFullDeclaration(
            ICodeWriterEx codeWriter,
            IType type,
            IGenericContext genericContext,
            IEnumerable<IMethod> genericMethodSpecializatons,
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

            ConvertTypeDefinition(codeWriter, type, genericMethodSpecializatons, processGenericMethodsOnly, true);

            codeWriter.WriteAfterMethods(type);

            if (!processGenericMethodsOnly)
            {
                codeWriter.WriteTypeEnd(type);
            }
        }

        private void WriteTypeDefinition(ICodeWriterEx codeWriter, IType type, IGenericContext genericContext)
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
        private void ConvertAllTypes(
            ICodeWriterEx codeWriter,
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
                        processGenericMethodsOnly);
                }
                else if (mode == ConvertingMode.PostDefinition)
                {
                    codeWriter.WritePostDefinitions(type);
                }
            }
        }

        private void ConvertAllRuntimeTypesForwardDeclaration(
            ICodeWriterEx codeWriter,
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


        private void ConvertAllRuntimeTypes(
            ICodeWriterEx codeWriter,
            IEnumerable<IType> types)
        {
            foreach (var type in types)
            {
                Debug.Assert(type != null);
                if (type == null)
                {
                    continue;
                }

                ConvertRuntimeTypeInfo(codeWriter, type);
            }
        }

        private void ConvertForwardTypeDeclaration(ICodeWriterEx codeWriter, IType type)
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
        private void ConvertTypeDeclaration(
            ICodeWriterEx codeWriter,
            IType type,
            IEnumerable<IMethod> genericMethodSpecializatons,
            bool processGenericMethodsOnly = false)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Converting {0} (declarations)", type));
            }

            IType typeDefinition;
            IType typeSpecialization;
            var genericTypeContext = GetGenericTypeContext(type, out typeDefinition, out typeSpecialization);

            WriteTypeFullDeclaration(codeWriter, type, genericTypeContext, genericMethodSpecializatons, processGenericMethodsOnly);
        }

        private void ConvertTypeDefinition(
            ICodeWriterEx codeWriter,
            IType type,
            IEnumerable<IMethod> genericMethodSpecializatons,
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

            ////codeWriter.WriteAfterMethods(type);
        }

        private IGenericContext GetGenericTypeContext(
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

        private void ConvertRuntimeTypeInfoForwardDeclaration(ICodeWriterEx codeWriter, IType type)
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
        private void ConvertRuntimeTypeInfo(ICodeWriterEx codeWriter, IType type)
        {
            Debug.Assert(type.IsGenericTypeDefinition || type.IsPointer || compact, "This method is for Generic Definitions or pointers only as it should not be processed in normal way using ConvertType");

            var fields = IlReader.Fields(type, codeWriter);

            foreach (var field in fields.Where(f => f.Name == RuntimeTypeInfoGen.RuntimeTypeHolderFieldName))
            {
                codeWriter.WriteStaticField(field, typeForRuntimeTypeInfo: type);
            }
        }

        private void AddTypeIfSpecializedTypeOrAdditionalType(IType type, ReadingTypesContext readingTypesContext)
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

            if (effectiveType.IsArray && !(!compact && !isCoreLib && effectiveType.IsArrayInternal()))
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
        private void GenerateC(
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
            compact = args != null && args.Any(a => a == "compact");
            VerboseOutput = args != null && args.Any(a => a == "verbose");
            headers = args != null && args.Any(a => a == "headers");

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
                var namespaces = (!compact ? ilReader.Types() : ilReader.AllTypes()).Where(t => !string.IsNullOrEmpty(t.Namespace)).Select(t => t.Namespace).Distinct().ToList();
                GenerateMultiSources(ilReader, namespaces, settings);
            }
        }

        private ICodeWriterEx GetCodeWriter(IlReader ilReader, Settings settings, bool isHeader = false)
        {
            var codeWriter = GetCWriter(
                settings.FileName, settings.FileExt, settings.SourceFilePath, settings.PdbFilePath, settings.OutputFolder, settings.Args);
            codeWriter.IsHeader = isHeader;
            ilReader.CodeWriter = codeWriter;
            codeWriter.IlReader = ilReader;
            _codeWriter = codeWriter;
            _codeWriter.Initialize();
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
        private void GenerateSource(IlReader ilReader, Settings settings)
        {
            settings.FileExt = ".h";
            var codeHeaderWriter = GetCodeWriter(ilReader, settings, true);
            var readTypes = ReadingTypes(ilReader, settings.Filter);
            WritingDeclarations(ilReader, codeHeaderWriter, readTypes);
            if (headers)
            {
                return;
            }

            settings.FileExt = ".cpp";
            var codeWriter = GetCodeWriter(ilReader, settings);
            codeWriter.FileHeader = settings.FileName;
            WritingDefinitions(ilReader, codeWriter, readTypes);
        }

        private void GenerateMultiSources(IlReader ilReader, IEnumerable<string> namespaces, Settings settings)
        {
            settings.FileExt = ".h";
            var codeHeaderWriter = GetCodeWriter(ilReader, settings, true);
            var readTypes = ReadingTypes(ilReader, settings.Filter);
            WritingDeclarations(ilReader, codeHeaderWriter, readTypes);
            if (headers)
            {
                return;
            }

            var fileName = settings.FileName;
            foreach (var ns in namespaces)
            {
                this.GenerateSourceForNamespace(
                    ilReader,
                    settings,
                    fileName,
                    ns,
                    namespaces.Any(n => n != ns && string.Compare(n, ns, StringComparison.OrdinalIgnoreCase) == 0),
                    readTypes);
            }

            // very important step to generate code for empty space
            this.GenerateSourceForNamespace(ilReader, settings, fileName, string.Empty, false, readTypes);
        }

        private void GenerateSourceForNamespace(
            IlReader ilReader, Settings settings, string fileName, string ns, bool notUnique, ReadTypesContext readTypes)
        {
            settings.FileName = string.Concat(
                fileName,
                "_",
                string.IsNullOrEmpty(ns) ? "no_namespace" : ns.CleanUpName(),
                notUnique ? string.Concat("_", Math.Abs(ns.GetHashCode())) : string.Empty);
            settings.FileExt = ".cpp";

            var codeWriterForNameSpace = GetCodeWriter(ilReader, settings);
            codeWriterForNameSpace.IsSplit = true;
            codeWriterForNameSpace.SplitNamespace = ns;
            codeWriterForNameSpace.FileHeader = fileName;

            var readTypesByNamespace = readTypes.Clone();
            readTypesByNamespace.UsedTypes = readTypes.UsedTypes.Where(t => t.Namespace == ns).ToList();

            WritingDefinitions(ilReader, codeWriterForNameSpace, readTypesByNamespace);
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
        private ICodeWriterEx GetCWriter(
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
        private SortedDictionary<IType, IEnumerable<IMethod>> GroupGenericMethodsByType(
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

        private ReadTypesContext ReadingTypes(
            IlReader ilReader,
            string[] filter)
        {
            // clean it as you are using IlReader
            ilReader.GenericMethodSpecializations = null;

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

            readTypesContext.UsedTypes = types;
            readTypesContext.GenericMethodSpecializations = GroupGenericMethodsByType(readingTypesContext.GenericMethodSpecializations);

            var genericMethodSpecializations = readTypesContext.GenericMethodSpecializations;
            ilReader.GenericMethodSpecializations = genericMethodSpecializations;

            Debug.Assert(readTypesContext.UsedTypes.All(t => !t.IsByRef), "Type is used with flag IsByRef");
            Debug.Assert(readTypesContext.UsedTypes.All(t => !t.IsPointer), "Type is used with flag IsPointer");
            Debug.Assert(readTypesContext.UsedTypes.All(t => !t.IsGenericTypeDefinition), "Generic DefinitionType is used");

            if (compact)
            {
                readTypesContext.AssemblyQualifiedName = readingTypesContext.AssemblyQualifiedName;
                readTypesContext.CalledMethods = readingTypesContext.CalledMethods;
                readTypesContext.UsedStaticFields = readingTypesContext.UsedStaticFields;
                readTypesContext.UsedVirtualTableImplementationTypes = readingTypesContext.UsedVirtualTableImplementationTypes;
            }

            return readTypesContext;
        }

        ////private IType LoadNativeTypeFromSource(IIlReader ilReader, string assemblyName = null)
        ////{
        ////    return ilReader.CompileSourceWithRoslyn(assemblyName, Resources.NativeType).First(t => !t.IsModule);
        ////}

        private bool CheckFilter(string[] filters, IType type, IDictionary<string, IType> allTypes)
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

        private void WritingDeclarations(
            IlReader ilReader,
            ICodeWriterEx codeHeaderWriter,
            ReadTypesContext readTypes)
        {
            var runtimeTypes = readTypes.UsedTypes;

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

        private void WritingDefinitions(
            IlReader ilReader,
            ICodeWriterEx codeWriter,
            ReadTypesContext readTypes)
        {
            // writing
            codeWriter.WriteStart();

            WriteTypesWithGenericsStep(codeWriter, readTypes, ConvertingMode.PreDefinition);

            // Append not generated sgettypes
            var fullNames = readTypes.UsedTypes.Select(t => t.FullName).ToList();
            if (!compact)
            {
                WriteTypesWithGenericsStep(codeWriter, readTypes, ConvertingMode.Definition);
                WriteTypesWithGenericsStep(codeWriter, readTypes, ConvertingMode.PostDefinition);
            }
            else if (!split)
            {
                // we just need to write all called methods
                WriteBulkOfStaticFields(codeWriter, readTypes.UsedStaticFields.Where(f => f.AssemblyQualifiedName != readTypes.AssemblyQualifiedName && !f.DeclaringType.IsGenericOrArray()));
                WriteBulkOfMethod(codeWriter, readTypes.CalledMethods.Select(m => m.Method));
                WriteBulkOfVirtualTableImplementation(codeWriter, readTypes.UsedVirtualTableImplementationTypes);
            }
            else
            {
                // split & compact
                // we just need to write all called methods
                WriteBulkOfStaticFields(codeWriter, readTypes.UsedStaticFields.Where(f => f.DeclaringType.Namespace == codeWriter.SplitNamespace).Where(f => !fullNames.Contains(f.DeclaringType.FullName)));
                WriteBulkOfMethod(codeWriter, readTypes.CalledMethods.Where(m => m.Method.Namespace == codeWriter.SplitNamespace).Select(m => m.Method));
                WriteBulkOfVirtualTableImplementation(codeWriter, readTypes.UsedVirtualTableImplementationTypes.Where(t => t.Namespace == codeWriter.SplitNamespace));
            }

            if (!codeWriter.IsSplit || codeWriter.IsSplit && (string.IsNullOrWhiteSpace(codeWriter.SplitNamespace) || this.compact))
            {
                ////ConvertAllRuntimeTypes(codeWriter, runtimeTypes.ToList());
            }

            codeWriter.WriteEnd();

            codeWriter.Close();
        }

        private void WriteBulkOfMethod(ICodeWriterEx codeWriter, IEnumerable<IMethod> methods)
        {
            foreach (var calledMethod in methods)
            {
                WriteSinleMethodDefinition(codeWriter, calledMethod);
            }
        }

        private void WriteBulkOfVirtualTableImplementation(ICodeWriterEx codeWriter, IEnumerable<IType> virtualTableImplementations)
        {
            foreach (var virtualTableImplementation in virtualTableImplementations)
            {
                Debug.Assert(!virtualTableImplementation.IsGenericTypeDefinition, "Generic Definition not allowed here");

                codeWriter.WriteVirtualTableImplementations(virtualTableImplementation);
            }
        }

        private void WriteBulkOfStaticFields(ICodeWriterEx codeWriter, IEnumerable<IField> staticFields)
        {
            foreach (var staticField in staticFields)
            {
                codeWriter.WriteStaticField(staticField);
            }
        }

        private void WriteSinleMethodDefinition(ICodeWriterEx codeWriter, IMethod calledMethod)
        {
            Debug.Assert(!calledMethod.IsGenericMethodDefinition, "Method Definition is not allowed here");

            var type = calledMethod.DeclaringType;

            IType typeDefinition;
            IType typeSpecialization;
            var genericTypeContext = GetGenericTypeContext(type, out typeDefinition, out typeSpecialization);

            var method = MethodBodyBank.GetMethodWithCustomBodyOrDefault(calledMethod, codeWriter);

            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("writing method {0}", method));
            }

            if (method.IsGenericMethodDefinition)
            {
                return;
            }

            var genericMethodContext = method.IsGenericMethod
                ? MetadataGenericContext.Create(typeDefinition, typeSpecialization, method.GetMethodDefinition(), method)
                : genericTypeContext;

            codeWriter.WriteMethod(
                method,
                MethodBodyBank.GetMethodWithCustomBodyOrDefault(type.IsGenericType || method.IsGenericMethod ? method.GetMethodDefinition() : method, codeWriter),
                genericMethodContext);
        }

        private void WriteTypesWithGenericsStep(
            ICodeWriterEx codeWriter,
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

        private void WriteLeftGenericMethodsStep(
            ICodeWriterEx codeWriter,
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
            this.UsedStaticFields = new NamespaceContainer<IField>();
            this.UsedVirtualTableImplementationTypes = new NamespaceContainer<IType>();
        }

        public string AssemblyQualifiedName { get; set; }

        public ISet<IType> GenericTypeSpecializations { get; set; }

        public ISet<IMethod> GenericMethodSpecializations { get; set; }

        public ISet<IType> AdditionalTypesToProcess { get; set; }

        public ISet<IType> UsedTypeTokens { get; set; }

        public ISet<IType> DiscoveredTypes { get; set; }

        public ISet<IType> UsedTypes { get; set; }

        public ISet<MethodKey> CalledMethods { get; set; }

        public ISet<IField> UsedStaticFields { get; set; }

        public ISet<IType> UsedVirtualTableImplementationTypes { get; set; }

        public static ReadingTypesContext New()
        {
            return new ReadingTypesContext();
        }
    }

    public class ReadTypesContext
    {
        public string AssemblyQualifiedName { get; set; }

        public IList<IType> UsedTypes { get; set; }

        public IDictionary<IType, IEnumerable<IMethod>> GenericMethodSpecializations { get; set; }

        // to support compact mode
        public ISet<MethodKey> CalledMethods { get; set; }

        // to support compact mode
        public ISet<IField> UsedStaticFields { get; set; }

        // to support compact mode
        public ISet<IType> UsedVirtualTableImplementationTypes { get; set; }

        public static ReadTypesContext New()
        {
            return new ReadTypesContext();
        }

        public ReadTypesContext Clone()
        {
            return (ReadTypesContext)this.MemberwiseClone();
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
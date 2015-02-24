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
            string[] filter,
            ICodeWriter codeWriter,
            IList<IType> newListOfITypes,
            IDictionary<string, IType> genDefinitionsByMetadataName,
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

                if (filter != null && !filter.Contains(type.FullName))
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

                IType genDef = null;
                if (type.IsGenericType)
                {
                    genDefinitionsByMetadataName.TryGetValue(type.MetadataFullName, out genDef);
                }

                IEnumerable<IMethod> genericMethodSpecializatonsForType = null;
                genMethodSpec.TryGetValue(type, out genericMethodSpecializatonsForType);

                ConvertIType(
                    ilReader,
                    codeWriter,
                    type.ToClass(),
                    genDef,
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
        /// <param name="genericDefinition">
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
            IType genericDefinition,
            IEnumerable<IMethod> genericMethodSpecializatons,
            ConvertingMode mode,
            bool processGenericMethodsOnly = false)
        {
            if (VerboseOutput)
            {
                Trace.WriteLine(string.Format("Converting {0}, Mode: {1}", type, mode));
            }

            var typeSpecialization = type.IsGenericType && !type.IsGenericTypeDefinition ? type : null;

            var genericTypeContext = genericDefinition != null || typeSpecialization != null
                                         ? MetadataGenericContext.Create(genericDefinition, typeSpecialization)
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
                        IConstructor genericCtor = null;
                        if (ctor.IsGenericMethodDefinition && !type.IsInterface && !type.IsDelegate && !type.IsArray)
                        {
                            // find the same constructor in generic class
                            Debug.Assert(genericDefinition != null);
                            genericCtor =
                                IlReader.Constructors(genericDefinition, codeWriter).First(gm => ctor.IsMatchingGeneric(gm));
                        }

                        var genericCtorContext = genericCtor != null
                                                     ? MetadataGenericContext.Create(genericDefinition, typeSpecialization, genericCtor, null)
                                                     : genericTypeContext;

                        codeWriter.WriteConstructorStart(ctor, genericCtorContext);

                        foreach (var ilCode in ilReader.OpCodes(genericCtor ?? ctor, genericCtorContext))
                        {
                            codeWriter.Write(ilCode);
                        }

                        codeWriter.WriteConstructorEnd(ctor, genericCtorContext);
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
                    IMethod genericMethod = null;
                    if (method.IsGenericMethodDefinition && !type.IsInterface && !type.IsDelegate && !type.IsArray)
                    {
                        // find the same method in generic class
                        genericMethod =
                            IlReader.Methods(genericDefinition, codeWriter, true)
                                    .FirstOrDefault(gm => method.IsMatchingGeneric(gm.ToSpecialization(genericTypeContext)))
                            ?? IlReader.Methods(genericDefinition, codeWriter, true).FirstOrDefault(gm => method.IsMatchingGeneric(gm));

                        Debug.Assert(genericMethod != null, "generic method is null");
                    }

                    if (!method.IsGenericMethodDefinition && !processGenericMethodsOnly)
                    {
                        var genericMethodContext = genericMethod != null || method.IsGenericMethod
                                                       ? MetadataGenericContext.Create(
                                                           genericDefinition, typeSpecialization, genericMethod, genericMethod != null ? method : null)
                                                       : genericTypeContext;

                        codeWriter.WriteMethodStart(method, genericMethodContext);

                        foreach (var ilCode in ilReader.OpCodes(genericMethod ?? method, genericMethodContext))
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
                                var genericMethodContext = MetadataGenericContext.Create(genericDefinition, typeSpecialization, method, methodSpec);

                                codeWriter.WriteMethodStart(methodSpec, genericMethodContext);

                                foreach (var ilCode in ilReader.OpCodes(genericMethod ?? method, genericMethodContext))
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

            var additionalTypesToProcess = readingTypesContext.AdditionalTypesToProcess;
            var genericSpecializations = readingTypesContext.GenericTypeSpecializations;
            var genericMethodSpecializations = readingTypesContext.GenericMethodSpecializations;

            var type = typeSource.NormalizeType();

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
            ISet<IMethod> genericMethodSpecializations)
        {
            var allSpecializedMethodsOfInterfaces =
                genericMethodSpecializations.Where(m => m.DeclaringType.IsInterface && m.IsGenericMethod)
                    .Distinct()
                    .ToList();
            var allSpecializedMethodsOfInterfacesGroupedByType =
                allSpecializedMethodsOfInterfaces.GroupBy(m => m.DeclaringType);

            if (concurrent)
            {
                Parallel.ForEach(
                    allSpecializedMethodsOfInterfacesGroupedByType,
                    specializedTypeMethods =>
                        DiscoverAllGenericMethodsOfInterfacesForMethod(
                            allTypes,
                            genericMethodSpecializations,
                            specializedTypeMethods));
            }
            else
            {
                foreach (var specializedTypeMethods in allSpecializedMethodsOfInterfacesGroupedByType)
                {
                    DiscoverAllGenericMethodsOfInterfacesForMethod(
                        allTypes,
                        genericMethodSpecializations,
                        specializedTypeMethods);
                }
            }
        }

        private static void DiscoverAllGenericMethodsOfInterfacesForMethod(
            IEnumerable<IType> allTypes,
            ISet<IMethod> genericMethodSpecializations,
            IGrouping<IType, IMethod> specializedTypeMethods)
        {
            var types = allTypes.Where(t => t.GetAllInterfaces().Contains(specializedTypeMethods.Key)).ToList();
            foreach (var specializedTypeMethod in specializedTypeMethods)
            {
                var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
                foreach (var genericMethodOfInterface in
                    types.SelectMany(
                        t =>
                            t.GetMethods(flags)
                                .Where(m => m.IsGenericMethodDefinition && m.IsMatchingOverride(specializedTypeMethod))))
                {
                    genericMethodSpecializations.Add(
                        genericMethodOfInterface.ToSpecialization(
                            MetadataGenericContext.Create(genericMethodOfInterface, specializedTypeMethod)));
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
            ISet<IMethod> genericMethodSpecializations)
        {
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
                var genericMethod = overrideGenericMethod;
                overrideSpecializedMethods.AddRange(
                    from specializationMethod in
                        genericMethodSpecializations.Where(m => m.IsVirtual || m.IsOverride || m.IsAbstract)
                    where
                        method.DeclaringType.IsDerivedFrom(specializationMethod.DeclaringType) &&
                        method.IsMatchingOverride(specializationMethod)
                    select
                        genericMethod.ToSpecialization(
                            MetadataGenericContext.Create(genericMethod, specializationMethod)));
            }

            // append to discovered
            foreach (var overrideSpecializedMethod in overrideSpecializedMethods)
            {
                genericMethodSpecializations.Add(overrideSpecializedMethod);
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

            IList<IType> sortedListOfTypes;
            IDictionary<string, IType> genDefinitionsByMetadataName;
            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted;
            ReadingTypes(
                ilReader,
                filter,
                out sortedListOfTypes,
                out genDefinitionsByMetadataName,
                out genericMethodSpecializationsSorted);

            Writing(
                ilReader,
                filter,
                codeWriter,
                sortedListOfTypes,
                genDefinitionsByMetadataName,
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

            var type = typeSource.NormalizeType();
            if (!readingTypesContext.ProcessedTypes.Add(type))
            {
                yield break;
            }

            if (type.BaseType != null)
            {
                DicoverGenericSpecializedTypesAndAdditionalTypes(type.BaseType, readingTypesContext);
                yield return type.BaseType;
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
                foreach (var requiredType in ctors.SelectMany(ctor => GetAllRequiredTypesForMethod(
                                ctor, readingTypesContext)))
                {
                    yield return requiredType;
                }
            }

            var methods = IlReader.Methods(
                type, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, _codeWriter);
            foreach (var requiredType in methods.SelectMany(method => GetAllRequiredTypesForMethod(
                            method, readingTypesContext)))
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
                    readingTypesContext.GenericTypeSpecializations,
                    type => AppendTypeWithRequiredTypePair(type, requiredTypes, subSetReadingContext));

                Parallel.ForEach(
                    readingTypesContext.AdditionalTypesToProcess,
                    type => AppendTypeWithRequiredTypePair(type, requiredTypes, subSetReadingContext));
            }
            else
            {
                foreach (var type in readingTypesContext.GenericTypeSpecializations)
                {
                    Debug.Assert(type != null);
                    if (type == null)
                    {
                        continue;
                    }

                    AppendTypeWithRequiredTypePair(type, requiredTypes, subSetReadingContext);
                }

                foreach (var type in readingTypesContext.AdditionalTypesToProcess)
                {
                    Debug.Assert(type != null);
                    if (type == null)
                    {
                        continue;
                    }

                    AppendTypeWithRequiredTypePair(type, requiredTypes, subSetReadingContext);
                }
            }

            if (subSetGenericTypeSpecializations.Count > 0)
            {
                foreach (var discoveredType in requiredTypes.Select(t => t.Key))
                {
                    subSetGenericTypeSpecializations.Remove(discoveredType);
                }

                ProcessGenericTypesAndAdditionalTypesToFindRequiredTypes(requiredTypes, subSetReadingContext);

                // join types
                foreach (var discoveredType in subSetGenericTypeSpecializations)
                {
                    Debug.Assert(discoveredType != null);
                    readingTypesContext.GenericTypeSpecializations.Add(discoveredType);
                }
            }

            if (subSetAdditionalTypesToProcess.Count > 0)
            {
                foreach (var discoveredType in requiredTypes.Select(t => t.Key))
                {
                    Debug.Assert(discoveredType != null);
                    subSetAdditionalTypesToProcess.Remove(discoveredType);
                }

                ProcessGenericTypesAndAdditionalTypesToFindRequiredTypes(requiredTypes, subSetReadingContext);

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
            out IDictionary<string, IType> genDefinitionsByMetadataName,
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

            sortedListOfTypes = SortTypesByUsage(types.ToList(), readingTypesContext);

            // build quick access array for Generic Definitions
            genDefinitionsByMetadataName = new SortedDictionary<string, IType>();
            foreach (var genDef in allTypes.Where(t => t.IsGenericTypeDefinition))
            {
                genDefinitionsByMetadataName[genDef.MetadataFullName] = genDef;
            }

            DiscoverAllGenericVirtualMethods(allTypes, readingTypesContext.GenericMethodSpecializations);

            DiscoverAllGenericMethodsOfInterfaces(allTypes, readingTypesContext.GenericMethodSpecializations);

            genericMethodSpecializationsSorted = GroupGenericMethodsByType(readingTypesContext.GenericMethodSpecializations);

            Debug.Assert(sortedListOfTypes.All(t => !t.IsByRef), "Type is used with flag IsByRef");
        }

        private static bool CheckFilter(IEnumerable<string> filters, IType type)
        {
            foreach (var filter in filters)
            {
                if (filter.EndsWith("*") && type.Namespace == filter.Substring(0, filter.Length - 1))
                {
                    return true;
                }

                if (string.CompareOrdinal(type.FullName, 0, filter, 0, filter.Length) == 0)
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
        private static IList<IType> SortTypesByUsage(IList<IType> types, ReadingTypesContext readingTypesContext)
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
            string[] filter,
            ICodeWriter codeWriter,
            IList<IType> newListOfITypes,
            IDictionary<string, IType> genDefinitionsByMetadataName,
            IDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted)
        {
            // writing
            codeWriter.WriteStart(ilReader);

            WriteForwardDeclarations(codeWriter, newListOfITypes);

            ConvertAllTypes(
                ilReader,
                filter,
                codeWriter,
                newListOfITypes,
                genDefinitionsByMetadataName,
                genericMethodSpecializationsSorted,
                ConvertingMode.Declaration);

            ConvertAllTypes(
                ilReader,
                filter,
                codeWriter,
                newListOfITypes,
                genDefinitionsByMetadataName,
                genericMethodSpecializationsSorted,
                ConvertingMode.Definition);

            // Append definition of Generic Methods of not used non-generic types
            ConvertAllTypes(
                ilReader,
                filter,
                codeWriter,
                genericMethodSpecializationsSorted.Keys.Where(k => !newListOfITypes.Contains(k)).ToList(),
                genDefinitionsByMetadataName,
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
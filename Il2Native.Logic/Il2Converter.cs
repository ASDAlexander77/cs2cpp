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

    using Il2Native.Logic.Gencode;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class Il2Converter
    {
        private static bool concurrent = false;

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
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(source);

            var ilReader = new IlReader(source, args);
            ilReader.Load();

            GenerateLlvm(ilReader, fileNameWithoutExtension, ilReader.SourceFilePath, ilReader.PdbFilePath, outputFolder, args);
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
            var name = type.Module.Name.Replace(".dll", string.Empty);
            var filePath = Path.GetDirectoryName(name);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
            var pdbFileName = Path.Combine(filePath, string.Concat(fileNameWithoutExtension, ".pdb"));

            var ilReader = new IlReader();
            ilReader.Load(type);
            GenerateLlvm(ilReader, fileNameWithoutExtension, null, pdbFileName, outputFolder, args, new[] { type.FullName });
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
            ISet<IType> typesAdded,
            List<IType> newListOfITypes,
            ISet<IType> genericSpecializations,
            ISet<IMethod> genericMethodSpecializations,
            ISet<IType> processedAlready)
        {
            if (concurrent)
            {
                Parallel.ForEach(
                    types, type => ProcessRequiredITypesForType(typesAdded, newListOfITypes, genericSpecializations, genericMethodSpecializations, type, processedAlready));
            }
            else
            {
                foreach (var type in types)
                {
                    ProcessRequiredITypesForType(typesAdded, newListOfITypes, genericSpecializations, genericMethodSpecializations, type, processedAlready);
                }
            }
        }

        private static void ProcessRequiredITypesForType(
            ISet<IType> typesAdded, List<IType> newListOfITypes, ISet<IType> genericSpecializations, ISet<IMethod> genericMethodSpecializations, IType type, ISet<IType> processedAlready)
        {
            var requiredITypesToAdd = new List<IType>();
            ProcessNextRequiredITypes(type, typesAdded, requiredITypesToAdd, genericSpecializations, genericMethodSpecializations, processedAlready);
            newListOfITypes.AddRange(requiredITypesToAdd);

            if (typesAdded.Contains(type))
            {
                return;
            }

            typesAdded.Add(type);
            newListOfITypes.Add(type);
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
            var fields = IlReader.Fields(type);
            var count = fields.Count();
            var number = 1;

            Debug.Assert(!type.IsGenericTypeDefinition);
            Debug.Assert(!type.IsArray);

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
        private static void AddRequiredIType(IType type, List<IType> requiredITypesToAdd, ISet<IType> typesAdded)
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
            var i = 0;
            foreach (var type in newListOfITypes)
            {
                i++;

                if (filter != null && !filter.Contains(type.FullName))
                {
                    continue;
                }

                if (type.IsGenericTypeDefinition || type.Name == "<Module>")
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

                ConvertIType(ilReader, codeWriter, type.ToClass(), genDef, genericMethodSpecializatonsForType, mode, processGenericMethodsOnly);
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
            Debug.WriteLine("Converting {0}, Mode: {1}", type, mode);

            var typeSpecialization = type.IsGenericType && !type.IsGenericTypeDefinition ? type : null;

            var genericContext = new MetadataGenericContext();
            genericContext.TypeDefinition = genericDefinition;
            genericContext.TypeSpecialization = typeSpecialization;
            genericContext.MethodDefinition = null;
            genericContext.MethodSpecialization = null;

            if (mode == ConvertingMode.Declaration)
            {
                if (!codeWriter.IsProcessed(type))
                {
                    WriteTypeDefinition(codeWriter, type, genericContext);
                }

                codeWriter.WritePostDeclarations(type);

                codeWriter.WriteBeforeConstructors();
            }

            if (mode == ConvertingMode.Definition)
            {
                codeWriter.DisableWrite(true);

                if (!processGenericMethodsOnly)
                {
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

                        genericContext.MethodDefinition = genericCtor;
                        genericContext.MethodSpecialization = null;

                        codeWriter.WriteConstructorStart(ctor, genericContext);

                        foreach (var ilCode in ilReader.OpCodes(genericCtor ?? ctor, genericContext))
                        {
                            codeWriter.Write(ilCode);
                        }

                        codeWriter.WriteConstructorEnd(ctor, genericContext);
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
                foreach (var method in IlReader.MethodsOriginal(type).Select(m => MethodBodyBank.GetMethodBodyOrDefault(m, codeWriter)))
                {
                    IMethod genericMethod = null;
                    if (type.IsGenericType && !type.IsInterface && !type.IsDelegate)
                    {
                        // find the same method in generic class
                        genericMethod =
                            IlReader.MethodsOriginal(genericDefinition).FirstOrDefault(gm => method.IsMatchingGeneric(gm.ToSpecialization(genericContext)))
                            ?? IlReader.MethodsOriginal(genericDefinition).FirstOrDefault(gm => method.IsMatchingGeneric(gm));
                        
                        Debug.Assert(genericMethod != null);
                    }

                    if (!method.IsGenericMethodDefinition && !processGenericMethodsOnly)
                    {
                        genericContext.MethodDefinition = genericMethod;
                        genericContext.MethodSpecialization = genericMethod != null ? method : null;

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
                        if (genericMethodSpecializatons != null)
                        {
                            foreach (var methodSpec in genericMethodSpecializatons)
                            {
                                if (!methodSpec.IsMatchingGeneric(method.GetMethodDefinition()))
                                {
                                    continue;
                                }

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
        private static void DicoverGenericSpecializedIType(IType type, ISet<IType> genericSpecializations, ISet<IMethod> genericMethodSpecializations, ISet<IType> processedAlready)
        {
            if (type == null || (genericSpecializations == null && genericMethodSpecializations == null))
            {
                return;
            }

            if (type.HasElementType)
            {
                DicoverGenericSpecializedIType(type.GetElementType(), genericSpecializations, genericMethodSpecializations, processedAlready);
                return;
            }

            var bareType = type.ToBareType().ToNormal();
            if (type.IsGenericType && !type.IsGenericTypeDefinition && !genericSpecializations.Contains(bareType) && !TypeHasGenericParameter(type)
                && !TypeHasGenericParameterInGenericArguments(type))
            {
                genericSpecializations.Add(bareType);

                // todo the same for base class and interfaces
                foreach (var item in GetAllRequiredITypesForIType(type, genericSpecializations, genericMethodSpecializations, processedAlready))
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
        private static void DiscoverAllGenericMethodsOfInterfaces(IEnumerable<IType> allTypes, ISet<IMethod> genericMethodSpecializations)
        {
            var allSpecializedMethodsOfInterfaces =
                genericMethodSpecializations.Where(m => m.DeclaringType.IsInterface && m.IsGenericMethod).Distinct().ToList();
            var allSpecializedMethodsOfInterfacesGroupedByType = allSpecializedMethodsOfInterfaces.GroupBy(m => m.DeclaringType);

            if (concurrent)
            {
                Parallel.ForEach(
                    allSpecializedMethodsOfInterfacesGroupedByType,
                    specializedTypeMethods => DiscoverAllGenericMethodsOfInterfacesForMethod(allTypes, genericMethodSpecializations, specializedTypeMethods));
            }
            else
            {
                foreach (var specializedTypeMethods in allSpecializedMethodsOfInterfacesGroupedByType)
                {
                    DiscoverAllGenericMethodsOfInterfacesForMethod(allTypes, genericMethodSpecializations, specializedTypeMethods);
                }
            }
        }

        private static void DiscoverAllGenericMethodsOfInterfacesForMethod(IEnumerable<IType> allTypes, ISet<IMethod> genericMethodSpecializations, IGrouping<IType, IMethod> specializedTypeMethods)
        {
            var types = allTypes.Where(t => t.GetAllInterfaces().Contains(specializedTypeMethods.Key)).ToList();
            foreach (var specializedTypeMethod in specializedTypeMethods)
            {
                var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance;
                foreach (var genericMethodOfInterface in
                    types.SelectMany(t => t.GetMethods(flags).Where(m => m.IsGenericMethodDefinition && m.IsMatchingOverride(specializedTypeMethod))))
                {
                    genericMethodSpecializations.Add(
                        genericMethodOfInterface.ToSpecialization(MetadataGenericContext.CreateMap(genericMethodOfInterface, specializedTypeMethod)));
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="allTypes">
        /// </param>
        /// <param name="genericMethodSpecializations">
        /// </param>
        private static void DiscoverAllGenericVirtualMethods(IEnumerable<IType> allTypes, ISet<IMethod> genericMethodSpecializations)
        {
            // find all override of generic methods 
            var flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            var overrideSpecializedMethods = new List<IMethod>();
            foreach (var overrideGenericMethod in allTypes.SelectMany(t => t.GetMethods(flags).Where(m => m.IsOverride && m.IsGenericMethodDefinition)))
            {
                var method = overrideGenericMethod;
                var genericMethod = overrideGenericMethod;
                overrideSpecializedMethods.AddRange(
                    from specializationMethod in genericMethodSpecializations.Where(m => m.IsVirtual || m.IsOverride || m.IsAbstract)
                    where method.DeclaringType.IsDerivedFrom(specializationMethod.DeclaringType) && method.IsMatchingOverride(specializationMethod)
                    select genericMethod.ToSpecialization(MetadataGenericContext.CreateMap(genericMethod, specializationMethod)));
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
        private static void GenerateLlvm(IlReader ilReader, string fileName, string sourceFilePath, string pdbFilePath, string outputFolder, string[] args, string[] filter = null)
        {
            concurrent = args != null && args.Any(a => a == "multi");
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
            List<IType> newListOfITypes;
            SortedDictionary<string, IType> genDefinitionsByMetadataName;
            SortedDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted;
            ReadingTypes(ilReader, out newListOfITypes, out genDefinitionsByMetadataName, out genericMethodSpecializationsSorted);

            Writing(ilReader, filter, codeWriter, newListOfITypes, genDefinitionsByMetadataName, genericMethodSpecializationsSorted);
        }

        private static void ReadingTypes(IlReader ilReader, out List<IType> newListOfITypes, out SortedDictionary<string, IType> genDefinitionsByMetadataName, out SortedDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted)
        {
            // types in current assembly
            var genericTypeSpecializations = new HashSet<IType>();
            var genericMethodSpecializations = new HashSet<IMethod>();
            var types = ilReader.Types().Where(t => !t.IsGenericTypeDefinition).ToList();
            var allTypes = ilReader.AllTypes().ToList();
#if !FOR_MSCORLIBTEST_DISABLE_RESORT
            newListOfITypes = ResortITypes(types, genericTypeSpecializations, genericMethodSpecializations);
#else
            var newListOfITypes = allTypes;
#endif
            // build quick access array for Generic Definitions
            genDefinitionsByMetadataName = new SortedDictionary<string, IType>();
            foreach (var genDef in allTypes.Where(t => t.IsGenericTypeDefinition))
            {
                genDefinitionsByMetadataName[genDef.MetadataFullName] = genDef;
            }

            DiscoverAllGenericVirtualMethods(allTypes, genericMethodSpecializations);

            DiscoverAllGenericMethodsOfInterfaces(allTypes, genericMethodSpecializations);

            genericMethodSpecializationsSorted = GroupGenericMethodsByType(genericMethodSpecializations);
        }

        private static void Writing(IlReader ilReader, string[] filter, ICodeWriter codeWriter, IList<IType> newListOfITypes, IDictionary<string, IType> genDefinitionsByMetadataName, SortedDictionary<IType, IEnumerable<IMethod>> genericMethodSpecializationsSorted)
        {
            // writing
            codeWriter.WriteStart(ilReader.ModuleName, ilReader.AssemblyQualifiedName, ilReader.IsCoreLib, ilReader.AllReferences());

            WriteForwardDeclarations(codeWriter, newListOfITypes);

            ConvertAllTypes(
                ilReader, filter, codeWriter, newListOfITypes, genDefinitionsByMetadataName, genericMethodSpecializationsSorted, ConvertingMode.Declaration);

            ConvertAllTypes(
                ilReader, filter, codeWriter, newListOfITypes, genDefinitionsByMetadataName, genericMethodSpecializationsSorted, ConvertingMode.Definition);

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
            IType typeSource, ISet<IType> genericTypeSpecializations, ISet<IMethod> genericMethodSpecializations, ISet<IType> processedAlready)
        {
            Debug.Assert(typeSource != null);

            var type = typeSource.ToBareType().ToNormal();

            if (processedAlready.Contains(type))
            {
                yield break;
            }

            processedAlready.Add(type);

            if (type.BaseType != null)
            {
                DicoverGenericSpecializedIType(type.BaseType, genericTypeSpecializations, genericMethodSpecializations, processedAlready);
                yield return type.BaseType;
            }

            var interfaces = type.GetInterfaces();
            if (interfaces != null)
            {
                foreach (var @interface in interfaces)
                {
                    DicoverGenericSpecializedIType(@interface, genericTypeSpecializations, genericMethodSpecializations, processedAlready);
                    yield return @interface;
                }
            }

            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (fields != null)
            {
                foreach (var field in fields)
                {
                    DicoverGenericSpecializedIType(field.FieldType, genericTypeSpecializations, genericMethodSpecializations, processedAlready);
                    if (field.FieldType.IsStructureType() && !field.FieldType.IsPointer)
                    {
                        yield return field.FieldType;
                    }
                }
            }

            var ctors = type.GetConstructors(
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (ctors != null)
            {
                foreach (var @ctor in ctors)
                {
                    foreach (var requiredType in GetAllRequiredITypesForMethod(@ctor, genericTypeSpecializations, genericMethodSpecializations, processedAlready))
                    {
                        yield return requiredType;
                    }
                }
            }

            var methods = type.GetMethods(
                BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
            if (methods != null)
            {
                foreach (var method in methods)
                {
                    foreach (var requiredType in GetAllRequiredITypesForMethod(method, genericTypeSpecializations, genericMethodSpecializations, processedAlready))
                    {
                        yield return requiredType;
                    }
                }
            }
        }

        private static IEnumerable<IType> GetAllRequiredITypesForMethod(
            IMethod method, ISet<IType> genericTypeSpecializations, ISet<IMethod> genericMethodSpecializations, ISet<IType> processedAlready)
        {
            DicoverGenericSpecializedIType(method.ReturnType, genericTypeSpecializations, genericMethodSpecializations, processedAlready);

            foreach (var param in method.GetParameters())
            {
                DicoverGenericSpecializedIType(param.ParameterType, genericTypeSpecializations, genericMethodSpecializations, processedAlready);
            }

            var methodBody = method.GetMethodBody(MetadataGenericContext.DiscoverFrom(method));
            if (methodBody != null)
            {
                foreach (var localVar in methodBody.LocalVariables)
                {
                    DicoverGenericSpecializedIType(localVar.LocalType, genericTypeSpecializations, genericMethodSpecializations, processedAlready);
                    if (localVar.LocalType.IsStructureType() && !localVar.LocalType.IsPointer)
                    {
                        yield return localVar.LocalType;
                    }
                }

                // TODO: too many calls, fix it
                ////Debug.Assert(method.ExplicitName != "List<B>.BinarySearch");

                var usedStructTypes = new HashSet<IType>();
                method.DiscoverRequiredTypesAndMethodsInMethodBody(
                    genericTypeSpecializations, genericMethodSpecializations, usedStructTypes, new Queue<IMethod>());
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
        private static ICodeWriter GetLlvmWriter(string fileName, string sourceFilePath, string pdbFilePath, string outputFolder, string[] args)
        {
            return new LlvmWriter(Path.Combine(outputFolder, fileName), sourceFilePath, pdbFilePath, args);
        }

        /// <summary>
        /// </summary>
        /// <param name="genericMethodSpecializations">
        /// </param>
        /// <returns>
        /// </returns>
        private static SortedDictionary<IType, IEnumerable<IMethod>> GroupGenericMethodsByType(ISet<IMethod> genericMethodSpecializations)
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
        private static void ProcessGenericTypeToFindRequiredTypes(ISet<IType> genericTypeSpecializations, ISet<IMethod> genericMethodSpecializations, ICollection<Tuple<IType, List<IType>>> requiredTypes, object requiredTypesSyncRoot, ISet<IType> processedAlready, bool applyConccurent = false)
        {
            var subSetGenericTypeSpecializations = new HashSet<IType>();

            // the same for generic specialized types
            if (concurrent && applyConccurent)
            {
                Parallel.ForEach(
                    genericTypeSpecializations,
                    type => ProcessGenericTypeToFindRequiredTypesForType(
                        requiredTypes, requiredTypesSyncRoot, type, subSetGenericTypeSpecializations, genericMethodSpecializations, processedAlready));
            }
            else
            {
                foreach (var type in genericTypeSpecializations)
                {
                    Debug.Assert(type != null);
                    if (type == null)
                    {
                        continue;
                    }

                    ProcessGenericTypeToFindRequiredTypesForType(requiredTypes, requiredTypesSyncRoot, type, subSetGenericTypeSpecializations, genericMethodSpecializations, processedAlready);
                }
            }

            if (subSetGenericTypeSpecializations.Count > 0)
            {
                foreach (var discoveredType in requiredTypes.Select(t => t.Item1))
                {
                    subSetGenericTypeSpecializations.Remove(discoveredType);
                }

                ProcessGenericTypeToFindRequiredTypes(subSetGenericTypeSpecializations, genericMethodSpecializations, requiredTypes, requiredTypesSyncRoot, processedAlready);

                // join types
                foreach (var discoveredType in subSetGenericTypeSpecializations)
                {
                    Debug.Assert(discoveredType != null);
                    genericTypeSpecializations.Add(discoveredType);
                }
            }
        }

        private static void ProcessGenericTypeToFindRequiredTypesForType(
            ICollection<Tuple<IType, List<IType>>> requiredTypes, object requiredTypesSyncRoot, IType type, ISet<IType> subSetGenericTypeSpecializations, ISet<IMethod> subSetGenericMethodSpecializations, ISet<IType> processedAlready)
        {
            Debug.Assert(type != null);
            Debug.WriteLine("Analyzing generic type: {0}", type);

            var requiredITypesToAdd = new List<IType>();
            ProcessNextRequiredITypes(type, new HashSet<IType>(), requiredITypesToAdd, subSetGenericTypeSpecializations, subSetGenericMethodSpecializations, processedAlready);
            lock (requiredTypesSyncRoot)
            {
                requiredTypes.Add(new Tuple<IType, List<IType>>(type, requiredITypesToAdd));
            }
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
            ISet<IType> typesAdded,
            List<IType> requiredITypesToAdd,
            ISet<IType> genericTypeSpecializations,
            ISet<IMethod> genericMethodSpecializations,
            ISet<IType> processedAlready)
        {
            Debug.Assert(type != null);

            var requiredITypes = GetAllRequiredITypesForIType(type, genericTypeSpecializations, genericMethodSpecializations, processedAlready)
                .Where(type.TypeNotEquals);
            foreach (var requiredIType in requiredITypes)
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
        private static List<IType> ResortITypes(List<IType> types, ISet<IType> genericTypeSpecializations, ISet<IMethod> genericMethodSpecializations)
        {
            var newOrderSyncRoot = new object();
            var newOrder = new List<IType>();

            var typesWithRequiredSyncRoot = new object();
            var typesWithRequired = new List<Tuple<IType, List<IType>>>();

            var processedAlready = new HashSet<IType>();

            if (concurrent)
            {
                Parallel.ForEach(
                    types,
                    type =>
                    ProcessNextRequiredTypesForType(
                        genericTypeSpecializations, genericMethodSpecializations, type, typesWithRequired, typesWithRequiredSyncRoot, processedAlready));
            }
            else
            {
                foreach (var type in types)
                {
                    ProcessNextRequiredTypesForType(genericTypeSpecializations, genericMethodSpecializations, type, typesWithRequired, typesWithRequiredSyncRoot, processedAlready);
                }
            }

            ProcessGenericTypeToFindRequiredTypes(genericTypeSpecializations, genericMethodSpecializations, typesWithRequired, typesWithRequiredSyncRoot, processedAlready);

            ReorderTypeByUsage(types, genericTypeSpecializations, typesWithRequired, newOrder, newOrderSyncRoot);

            return newOrder;
        }

        private static void ProcessNextRequiredTypesForType(ISet<IType> genericTypeSpecializations, ISet<IMethod> genericMethodSpecializations, IType type, List<Tuple<IType, List<IType>>> typesWithRequired, object typesWithRequiredSyncRoot, ISet<IType> processedAlready)
        {
            Debug.WriteLine("Reading info about type: {0}", type);

            var requiredITypesToAdd = new List<IType>();
            ProcessNextRequiredITypes(type, new HashSet<IType>(), requiredITypesToAdd, genericTypeSpecializations, genericMethodSpecializations, processedAlready);
            lock (typesWithRequiredSyncRoot)
            {
                typesWithRequired.Add(new Tuple<IType, List<IType>>(type, requiredITypesToAdd));
            }
        }

        private static void ReorderTypeByUsage(List<IType> types, ISet<IType> genericTypeSpecializations, List<Tuple<IType, List<IType>>> typesWithRequired, List<IType> newOrder, object syncObject)
        {
            var allTypes = new HashSet<IType>();
            foreach (var type in types)
            {
                allTypes.Add(type);
            }
            foreach (var type in genericTypeSpecializations)
            {
                allTypes.Add(type);
            }

            var strictMode = true;
            while (typesWithRequired.Count > 0)
            {
                var before = typesWithRequired.Count;
                var toRemove = new List<Tuple<IType, List<IType>>>();
                var syncToRemove = new object();

                // step 1 find Root;
                if (concurrent)
                {
                    Parallel.ForEach(typesWithRequired, type => RemoveAllResolvedTypesForType(type, newOrder, allTypes, toRemove, syncObject, syncToRemove));
                }
                else
                {
                    foreach (var type in typesWithRequired)
                    {
                        RemoveAllResolvedTypesForType(type, newOrder, allTypes, toRemove, syncObject, syncToRemove);
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
                        newOrder.Add(typeItems.Item1);
                    }

                    break;
                }
            }
        }

        private static void RemoveAllResolvedTypesForType(Tuple<IType, List<IType>> type, List<IType> newOrder, ISet<IType> allTypes, ICollection<Tuple<IType, List<IType>>> toRemove, object syncObject, object syncToRemove)
        {
            var requiredITypes = type.Item2;

            lock (syncObject)
            {
                requiredITypes.RemoveAll(r => newOrder.Any(n => n.TypeEquals(r)));
            }

            // remove not used types, for example System.Object which maybe not in current assembly
            requiredITypes.RemoveAll(r => !allTypes.Contains(r));

            if (requiredITypes.Count != 0)
            {
                return;
            }

            lock (syncToRemove)
            {
                toRemove.Add(type);
            }

            lock (syncObject)
            {
                newOrder.Add(type.Item1);
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
                   || type.GenericTypeArguments
                          .Any(
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
                if (type.IsGenericTypeDefinition)
                {
                    continue;
                }

                codeWriter.WriteForwardDeclaration(type, index, newListOfITypes.Count);
            }
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
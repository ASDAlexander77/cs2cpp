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
            GenerateSource(ilReader, settings);
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
            var readTypes = ReadingTypes(ilReader, settings.Filter);
        }

        private ReadTypesContext ReadingTypes(
            IlReader ilReader,
            string[] filter)
        {
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
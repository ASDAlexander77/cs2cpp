namespace Il2Native.Logic.DebugInfo
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter;
    using Il2Native.Logic.Gencode;
    using Il2Native.Logic.Metadata.Model;

    using PEAssemblyReader;

    using PdbReader;

    public class DebugInfoGenerator
    {
        private const string IdentityString = "C# Native compiler";

        private readonly IList<NamedMetadata> namedMetadata = new List<NamedMetadata>(3);

        private readonly IList<CollectionMetadata> indexedMetadata = new List<CollectionMetadata>();

        private readonly string pdbFileName;

        private readonly string defaultSourceFilePath;

        private LlvmWriter writer;

        private readonly IDictionary<int, int> indexByOffset = new SortedDictionary<int, int>();

        private readonly IDictionary<string, CollectionMetadata> typesMetadataCache = new SortedDictionary<string, CollectionMetadata>();  

        private CollectionMetadata file;

        private CollectionMetadata fileType;

        private CollectionMetadata globalVariables;

        private CollectionMetadata currentFunction;

        private CollectionMetadata tagExpression;

        public DebugInfoGenerator(string pdbFileName, string defaultSourceFilePath)
        {
            this.pdbFileName = pdbFileName;
            this.defaultSourceFilePath = defaultSourceFilePath;

            this.CompileUnit = new CollectionMetadata();
            this.Flags = new CollectionMetadata();
            this.Identity = new CollectionMetadata();

            namedMetadata.Add(new NamedMetadata("llvm.dbg.cu", this.CompileUnit));
            namedMetadata.Add(new NamedMetadata("llvm.module.flags", this.Flags));
            namedMetadata.Add(new NamedMetadata("llvm.ident", this.Identity));

            // add default flags and identity
            this.Identity.Add(new CollectionMetadata(indexedMetadata).Add(IdentityString));
            this.Flags.Add(new CollectionMetadata(indexedMetadata).Add(2, "Dwarf Version", 4));
            this.Flags.Add(new CollectionMetadata(indexedMetadata).Add(2, "Debug Info Version", 2));
        }

        public IConverter PdbConverter { get; set; }

        public string DefaultSourceFilePath
        {
            get
            {
                return this.defaultSourceFilePath;
            }
        }

        public CollectionMetadata CompileUnit { get; private set; }

        public CollectionMetadata Flags { get; private set; }

        public CollectionMetadata Identity { get; private set; }

        public int? CurrentDebugLine
        {
            get;
            private set;
        }

        public void WriteTo(TextWriter output)
        {
            output.WriteLine(string.Empty);

            output.WriteLine("; Metadata Entries");
            foreach (var namedMetadataItem in this.namedMetadata)
            {
                namedMetadataItem.WriteTo(output, true);
            }

            output.WriteLine(string.Empty);

            output.WriteLine("; Metadata Items");
            foreach (var indexedMetadataItem in this.indexedMetadata)
            {
                if (indexedMetadataItem.NullIfEmpty && indexedMetadataItem.IsEmpty)
                {
                    continue;
                }

                output.Write("!{0} = ", indexedMetadataItem.Index);
                indexedMetadataItem.WriteValueTo(output);
                output.WriteLine(string.Empty);
            }
        }

        public void StartGenerating(LlvmWriter writer)
        {
            if (File.Exists(this.pdbFileName))
            {
                this.writer = writer;
                this.PdbConverter = Converter.GetConverter(this.pdbFileName, new DebugInfoSymbolWriter.DebugInfoSymbolWriter(this));
                // to force generating CompileUnit info
                this.PdbConverter.ConvertFunction(-1);
            }
        }

        public void GenerateFunction(int token)
        {
            this.indexByOffset.Clear();
            this.CurrentDebugLine = null;

            this.PdbConverter.ConvertFunction(token);
        }

        public CollectionMetadata DefineFile(ISourceFileEntry entry)
        {
            return new CollectionMetadata(indexedMetadata).Add(entry.FileName, entry.Directory.Replace("\\", "\\5C"));
        }

        public void DefineCompilationUnit(CollectionMetadata file, out CollectionMetadata enumTypes, out CollectionMetadata retainedTypes, out CollectionMetadata subprograms, out CollectionMetadata globalVariables, out CollectionMetadata importedEntities)
        {
            // add compile unit template
            var compilationUnit = new CollectionMetadata(indexedMetadata).Add(
                string.Format(@"0x11\0012\00{0}\000\00\000\00\001", IdentityString),
                // file
                file,
                // Enum Types
                enumTypes = new CollectionMetadata(indexedMetadata),
                // Retained Types
                retainedTypes = new CollectionMetadata(indexedMetadata),
                // Subprograms
                subprograms = new CollectionMetadata(indexedMetadata),
                // Global Variables
                new CollectionMetadata(indexedMetadata).Add(globalVariables = new CollectionMetadata(indexedMetadata) { NullIfEmpty = true }),
                // Imported entities
                importedEntities = new CollectionMetadata(indexedMetadata));

            this.globalVariables = globalVariables;
            this.file = file;
            this.fileType = new CollectionMetadata(indexedMetadata).Add("0x29", file);

            this.CompileUnit.Add(compilationUnit);
        }

        public CollectionMetadata DefineMethod(ISourceMethod method, CollectionMetadata file, out CollectionMetadata functionVariables)
        {
            // Flags 256 - definition (as main()), 259 - public (member of a class)
            var flag = 256;

            // Line number of the opening '{' of the function
            var scopeLine = method.LineNumber;

            // find method definition
            var methodDefinition = this.writer.MethodsByToken[method.Token];

            var methodReferenceType = this.writer.WriteToString(() => this.writer.WriteMethodPointerType(this.writer.Output, methodDefinition));
            var methodDefinitionName = this.writer.WriteToString(() => this.writer.WriteMethodDefinitionName(this.writer.Output, methodDefinition));

            CollectionMetadata subroutineTypes;
            CollectionMetadata parametersTypes;

            // add compile unit template
            var methodMetadataDefinition =
                new CollectionMetadata(indexedMetadata).Add(
                    string.Format(
                        @"0x2e\00{0}\00{1}\00{2}\00{3}\000\001\000\000\00{4}\000\00{5}",
                        method.Name,
                        method.DisplayName,
                        method.LinkageName,
                        method.LineNumber,
                        flag,
                        scopeLine),
                // Source directory (including trailing slash) & file pair
                file,
                // Reference to context descriptor
                this.fileType,
                // Subroutine types
                subroutineTypes = new CollectionMetadata(indexedMetadata),
                // indicates which base type contains the vtable pointer for the derived class
                null,
                // function method reference ex. "i32 ()* @main"                
                new PlainTextMetadata(string.Concat(methodReferenceType, " ", methodDefinitionName)),
                // Lists function template parameters
                null,
                // Function declaration descriptor
                null,
                // List of function variables
                functionVariables = new CollectionMetadata(indexedMetadata));

            // add subrouting type
            subroutineTypes.Add(
                @"0x15\00\000\000\000\000\000\000",
                null,
                null,
                null,
                parametersTypes = new CollectionMetadata(indexedMetadata),
                null,
                null,
                null);

            this.currentFunction = methodMetadataDefinition;

            // add return type
            parametersTypes.Add(
                !methodDefinition.ReturnType.IsVoid() && methodDefinition.ReturnType.IsStructureType() ? this.DefineType(methodDefinition.ReturnType) : null);
            foreach (var parameter in methodDefinition.GetParameters())
            {
                parametersTypes.Add(this.DefineType(parameter.ParameterType));
            }

            return methodMetadataDefinition;
        }

        public void SequencePoint(int offset, int lineBegin, int colBegin, CollectionMetadata function)
        {
            var dbgLine = new CollectionMetadata(indexedMetadata).Add(lineBegin, colBegin, function, null);
            if (dbgLine.Index.HasValue)
            {
                indexByOffset[offset] = dbgLine.Index.Value;
            }
        }

        public void DefineGlobal(PEAssemblyReader.IField field)
        {
            if (globalVariables == null)
            {
                throw new NullReferenceException("globalVariables");
            }

            var globalType = this.writer.WriteToString(() => field.FieldType.WriteTypePrefix(this.writer.Output, true));
            var globalName = string.Format("@\"{0}\"", field.GetFullName());

            var line = 0;

            this.globalVariables.Add(
                string.Format(@"0x34\00{0}\00{1}\00{2}\00{3}\000\001", field.Name, field.Name, field.Name, line),
                null,
                this.fileType,
                DefineType(field.FieldType),
                new PlainTextMetadata(string.Concat(globalType, " ", globalName)));
        }

        public CollectionMetadata DefineVariable(string name, IType vriableType, DebugVariableType debugVariableType)
        {
            var line = 0;

            var tag = string.Empty;
            switch (debugVariableType)
            {
                case DebugVariableType.Argument:
                    tag = "0x101";
                    line = 16777220;
                    break;
                case DebugVariableType.Auto:
                    tag = "0x100";
                    break;
                default:
                    break;
            }

            var type = this.DefineType(vriableType);

            return new CollectionMetadata(indexedMetadata).Add(
                string.Format(@"{2}\00{0}\00{1}\000", name, line, tag),
                this.currentFunction,
                this.fileType,
                type);
        }

        public CollectionMetadata DefineType(IType type)
        {
            var line = 0;
            var offset = 0;
            var flags = 0;

            CollectionMetadata typeMetadata;
            if (typesMetadataCache.TryGetValue(type.FullName, out typeMetadata))
            {
                return typeMetadata;
            }

            typeMetadata =
                new CollectionMetadata(indexedMetadata).Add(
                    string.Format(
                        @"0x24\00{0}\00{1}\00{2}\00{3}\00{4}\00{5}\005",
                        type.FullName,
                        line,
                        type.GetTypeSize(true) * 8,
                        LlvmWriter.PointerSize * 8,
                        offset,
                        flags),
                    null,
                    null);

            typesMetadataCache[type.FullName] = typeMetadata;

            return typeMetadata;
        }

        public CollectionMetadata DefineTagExpression()
        {
            if (this.tagExpression == null)
            {
                this.tagExpression = new CollectionMetadata(indexedMetadata).Add("0x102");
            }

            return this.tagExpression;
        }

        public void ReadAndSetCurrentDebugLine(int offset)
        {
            var newLine = this.GetLineByOffiset(offset);
            if (newLine.HasValue)
            {
                this.CurrentDebugLine = newLine;
            }
        }

        protected int? GetLineByOffiset(int offset)
        {
            int index;
            if (indexByOffset.TryGetValue(offset, out index))
            {
                return index;
            }

            return null;
        }
    }
}

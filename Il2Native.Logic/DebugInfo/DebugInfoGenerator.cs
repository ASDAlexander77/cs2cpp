namespace Il2Native.Logic.DebugInfo
{
    using System.Collections.Generic;
    using System.IO;

    using Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter;
    using Il2Native.Logic.Metadata.Model;

    using PdbReader;

    public class DebugInfoGenerator
    {
        private readonly IList<NamedMetadata> namedMetadata = new List<NamedMetadata>(3);

        private readonly IList<CollectionMetadata> indexedMetadata = new List<CollectionMetadata>();

        private readonly string pdbFileName;

        private readonly string defaultSourceFilePath;

        private string identity = "C# Native compiler";

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
            this.Identity.Add(new CollectionMetadata(indexedMetadata).Add(this.identity));
            this.Flags.Add(new CollectionMetadata(indexedMetadata).Add(2, "Dwarf Version", 4));
            this.Flags.Add(new CollectionMetadata(indexedMetadata).Add(2, "Debug Info Version", 1));
        }

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

        public void WriteTo(TextWriter output)
        {
            output.WriteLine(string.Empty);

            // build references
            var index = 0;
            foreach (var indexedMetadataItem in this.indexedMetadata)
            {
                indexedMetadataItem.Index = index++;
            }

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

        public void Generate()
        {
            if (File.Exists(this.pdbFileName))
            {
                Converter.Convert(this.pdbFileName, new DebugInfoSymbolWriter.DebugInfoSymbolWriter(this));
            }
        }

        public CollectionMetadata DefineFile(ISourceFileEntry entry)
        {
            return new CollectionMetadata(indexedMetadata).Add(entry.FileName, entry.Directory.Replace("\\", "\\5C"));
        }

        public void DefineCompilationUnit(CollectionMetadata file, out CollectionMetadata enumTypes, out CollectionMetadata retainedTypes, out CollectionMetadata subprograms, out CollectionMetadata globalVariables, out CollectionMetadata importedEntities)
        {
            // add compile unit template
            var compilationUnit = new CollectionMetadata(indexedMetadata).Add(
                786449,
                // file
                file,
                12,
                this.identity,
                // isOptimized?
                false,
                // Flags
                string.Empty,
                // Runtime Version
                0,
                // Enum Types
                enumTypes = new CollectionMetadata(indexedMetadata),
                // Retained Types
                retainedTypes = new CollectionMetadata(indexedMetadata),
                // Subprograms
                subprograms = new CollectionMetadata(indexedMetadata),
                // Global Variables
                globalVariables = new CollectionMetadata(indexedMetadata),
                // Imported entities
                importedEntities = new CollectionMetadata(indexedMetadata),
                // Split debug filename
                string.Empty,
                // Full debug info
                0);

            this.CompileUnit.Add(compilationUnit);
        }

        public CollectionMetadata DefineMethod(ISourceMethod method, CollectionMetadata file, out CollectionMetadata subroutineTypes, out CollectionMetadata functionVariables)
        {
            // member of a class   
            // !6 = metadata !{i32 786478, metadata !1, metadata !"_ZTS5Hello", metadata !"Test", metadata !"Test", metadata !"_ZN5Hello4TestEv", i32 4, metadata !7,  i1 false, i1 false, i32 0, i32 0, null, i32 259, i1 false, null,          null, i32 0, null,        i32 4 } ; [ DW_TAG_subprogram ] [line 4] [public] [Test]
            // definition
            // !7 = metadata !{i32 786478, metadata !1, metadata !12,           metadata !"main", metadata !"main", metadata !"",                 i32 9, metadata !13, i1 false, i1 true,  i32 0, i32 0, null, i32 256, i1 false, i32 ()* @main, null, null,  metadata !2, i32 10} ; [ DW_TAG_subprogram ] [line 9] [def] [scope 10] [main]

            // add compile unit template
            var methodDefinition = new CollectionMetadata(indexedMetadata).Add(
                786478,
                file,
                12,
                method.Name,
                method.DisplayName,
                method.LinkageName,
                method.LineNumber,
                // Subroutine types
                subroutineTypes = new CollectionMetadata(indexedMetadata),
                // Is local
                false,
                // Is definition
                true,
                // Virtuality attribute, e.g. pure virtual function
                0,
                // Index into virtual table for C++ methods
                0,
                // Flags 256 - definition (as main()), 259 - public (member of a class)
                256,
                // True if this function is optimized
                false,
                // function method reference ex. "i32 ()* @main"
                method.MethodReference,
                // Function template parameters
                null,
                // Function declaration
                null,
                // List of function variables
                functionVariables = new CollectionMetadata(indexedMetadata),
                // Line number of the opening '{' of the function
                method.LineNumber);

            return methodDefinition;
        }
    }
}

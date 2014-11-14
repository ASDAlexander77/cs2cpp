namespace Il2Native.Logic.DebugInfo
{
    using System.Collections.Generic;
    using System.IO;

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

        public void DefineCompilationUnit(ISourceFileEntry entry, out CollectionMetadata enumTypes, out CollectionMetadata retainedTypes, out CollectionMetadata subprograms, out CollectionMetadata globalVariables, out CollectionMetadata importedEntities)
        {
            // add compile unit template
            var compilationUnit = new CollectionMetadata(indexedMetadata).Add(
                786449,
                // file
                new CollectionMetadata(indexedMetadata).Add(entry.FileName, entry.Directory.Replace("\\", "\\5C")),
                12,
                this.identity,
                // isOptimized?
                false,
                // Flags
                string.Empty,
                // Runtime Version
                0,
                // Enum Types
                new CollectionMetadata(indexedMetadata).Add(enumTypes = new CollectionMetadata(indexedMetadata) { NullIfEmpty = true }),
                // Retained Types
                new CollectionMetadata(indexedMetadata).Add(retainedTypes = new CollectionMetadata(indexedMetadata) { NullIfEmpty = true }),
                // Subprograms
                new CollectionMetadata(indexedMetadata).Add(subprograms = new CollectionMetadata(indexedMetadata) { NullIfEmpty = true }),
                // Global Variables
                new CollectionMetadata(indexedMetadata).Add(globalVariables = new CollectionMetadata(indexedMetadata) { NullIfEmpty = true }),
                // Imported entities
                new CollectionMetadata(indexedMetadata).Add(importedEntities = new CollectionMetadata(indexedMetadata) { NullIfEmpty = true }),
                // Split debug filename
                string.Empty,
                // Full debug info
                0);

            this.CompileUnit.Add(compilationUnit);
        }

        public void DefineMethod(ISourceMethod method)
        {

        }
    }
}

namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using Metadata.Model;
    using PdbReader;

    public class DebugInfoCompileUnitEntry : ICompileUnitEntry
    {
        private readonly DebugInfoGenerator debugInfoGenerator;
        private readonly CollectionMetadata file;
        private readonly CollectionMetadata subprograms;
        private CollectionMetadata enumTypes;
        private CollectionMetadata globalVariables;
        private CollectionMetadata importedEntities;
        private CollectionMetadata retainedTypes;

        public DebugInfoCompileUnitEntry(
            DebugInfoGenerator debugInfoGenerator,
            ISourceFileEntry entry,
            CollectionMetadata file)
        {
            this.debugInfoGenerator = debugInfoGenerator;

            this.file = file;

            CollectionMetadata enumTypes;
            CollectionMetadata retainedTypes;
            CollectionMetadata subprograms;
            CollectionMetadata globalVariables;
            CollectionMetadata importedEntities;
            this.debugInfoGenerator.DefineCompilationUnit(
                this.file,
                out enumTypes,
                out retainedTypes,
                out subprograms,
                out globalVariables,
                out importedEntities);

            this.enumTypes = enumTypes;
            this.retainedTypes = retainedTypes;
            this.subprograms = subprograms;
            this.globalVariables = globalVariables;
            this.importedEntities = importedEntities;
        }

        public ISourceMethodBuilder DefineMethod(ISourceMethod method)
        {
            return new DebugInfoSourceMethodBuilder(this.debugInfoGenerator, method, this.file, this.subprograms);
        }
    }
}
namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using Il2Native.Logic.Metadata.Model;

    using PdbReader;

    public class DebugInfoCompileUnitEntry : ICompileUnitEntry
    {
        private DebugInfoGenerator debugInfoGenerator;

        private CollectionMetadata file;

        private CollectionMetadata enumTypes;
        private CollectionMetadata retainedTypes;
        private CollectionMetadata subprograms;
        private CollectionMetadata globalVariables;
        private CollectionMetadata importedEntities;

        public DebugInfoCompileUnitEntry(DebugInfoGenerator debugInfoGenerator, ISourceFileEntry entry, CollectionMetadata file)
        {
            this.debugInfoGenerator = debugInfoGenerator;

            this.file = file;

            CollectionMetadata enumTypes;
            CollectionMetadata retainedTypes; 
            CollectionMetadata subprograms;
            CollectionMetadata globalVariables;
            CollectionMetadata importedEntities;
            this.debugInfoGenerator.DefineCompilationUnit(
                this.file, out enumTypes, out retainedTypes, out subprograms, out globalVariables, out importedEntities);

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

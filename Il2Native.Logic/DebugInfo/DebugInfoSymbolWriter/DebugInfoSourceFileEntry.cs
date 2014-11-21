namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using PdbReader;
    using Il2Native.Logic.Metadata.Model;

    public class DebugInfoSourceFileEntry : ISourceFileEntry
    {
        private DebugInfoGenerator debugInfoGenerator;

        private CollectionMetadata file;

        public DebugInfoSourceFileEntry(DebugInfoGenerator debugInfoGenerator, string directory, string fileName)
        {
            this.debugInfoGenerator = debugInfoGenerator;

            this.Directory = directory;
            this.FileName = fileName;

            this.file = this.debugInfoGenerator.DefineFile(this);
        }

        public string Directory { get; private set; }

        public string FileName { get; private set; }

        public ICompileUnitEntry DefineCompilationUnit()
        {
            return new DebugInfoCompileUnitEntry(debugInfoGenerator, this, this.file);
        }
    }
}

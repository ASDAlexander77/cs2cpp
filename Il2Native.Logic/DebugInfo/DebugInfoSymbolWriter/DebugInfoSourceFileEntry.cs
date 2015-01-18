namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using Metadata.Model;
    using PdbReader;

    public class DebugInfoSourceFileEntry : ISourceFileEntry
    {
        private readonly DebugInfoGenerator debugInfoGenerator;
        private readonly CollectionMetadata file;

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
            return new DebugInfoCompileUnitEntry(this.debugInfoGenerator, this, this.file);
        }
    }
}
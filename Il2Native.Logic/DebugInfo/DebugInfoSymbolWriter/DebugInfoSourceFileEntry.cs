namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using PdbReader;

    public class DebugInfoSourceFileEntry : ISourceFileEntry
    {
        private DebugInfoGenerator debugInfoGenerator;

        public DebugInfoSourceFileEntry(DebugInfoGenerator debugInfoGenerator, string directory, string fileName)
        {
            this.debugInfoGenerator = debugInfoGenerator;

            this.Directory = directory;
            this.FileName = fileName;
        }

        public string Directory { get; private set; }

        public string FileName { get; private set; }

        public ICompileUnitEntry DefineCompilationUnit()
        {
            return new DebugInfoCompileUnitEntry(debugInfoGenerator);
        }
    }
}

namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using PdbReader;
    using Il2Native.Logic.Metadata.Model;

    public class DebugInfoSourceFileEntry : ISourceFileEntry
    {
        private DebugInfoGenerator debugInfoGenerator;

        public DebugInfoSourceFileEntry(DebugInfoGenerator debugInfoGenerator)
        {
            this.debugInfoGenerator = debugInfoGenerator;
        }

        public string Directory { get; set; }

        public string FileName { get; set; }

        public ICompileUnitEntry DefineCompilationUnit()
        {
            return new DebugInfoCompileUnitEntry(debugInfoGenerator, this);
        }
    }
}

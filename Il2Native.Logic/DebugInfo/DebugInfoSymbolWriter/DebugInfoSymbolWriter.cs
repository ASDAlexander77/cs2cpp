namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using System.IO;

    using PdbReader;

    public class DebugInfoSymbolWriter : ISymbolWriter
    {
        private readonly DebugInfoGenerator debugInfoGenerator;

        public DebugInfoSymbolWriter(DebugInfoGenerator debugInfoGenerator)
        {
            this.debugInfoGenerator = debugInfoGenerator;
        }

        public ISourceFileEntry DefineDocument(string name)
        {
            var effectiveName = string.IsNullOrWhiteSpace(name) ? this.debugInfoGenerator.SourceFilePath : name;
            var path = Path.GetDirectoryName(effectiveName);
            var filename = Path.GetFileName(effectiveName);

            return new DebugInfoSourceFileEntry(this.debugInfoGenerator, path, filename);
        }

        public ICompileUnitEntry DefineCompilationUnit(ISourceFileEntry entry)
        {
            return entry.DefineCompilationUnit();
        }

        public ISourceMethodBuilder OpenMethod(ICompileUnitEntry compilationUnit, ISourceMethod method)
        {
            return compilationUnit.DefineMethod(method);
        }

        public void DefineLocalVariable(int slot, string name)
        {
            this.debugInfoGenerator.DefineLocalVariable(name, slot);
        }

        public void CloseMethod()
        {
        }
    }
}

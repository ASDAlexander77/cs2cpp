namespace Ll2NativeTests.DummySymbolWriter
{
    using PdbReader;

    public class DummySourceFileEntry : ISourceFileEntry
    {
        public string Directory { get; private set; }

        public string FileName { get; private set; }

        public ICompileUnitEntry DefineCompilationUnit()
        {
            return new DummyCompileUnitEntry();
        }
    }
}

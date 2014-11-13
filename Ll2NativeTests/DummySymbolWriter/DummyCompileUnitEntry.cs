namespace Ll2NativeTests.DummySymbolWriter
{
    using PdbReader;

    public class DummyCompileUnitEntry : ICompileUnitEntry
    {
        public ISourceFile SourceFile { get; private set; }
    }
}

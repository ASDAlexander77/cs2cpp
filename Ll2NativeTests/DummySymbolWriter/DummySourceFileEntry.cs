namespace Ll2NativeTests.DummySymbolWriter
{
    using PdbReader;

    public class DummySourceFileEntry : ISourceFileEntry
    {
        public ISourceFile SourceFile { get; set; }
    }
}

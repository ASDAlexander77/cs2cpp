namespace Ll2NativeTests.DummySymbolWriter
{
    using PdbReader;

    public class DummySourceMethodBuilder : ISourceMethodBuilder
    {
        public void MarkSequencePoint(int offset, ISourceFile sourceFile, int i, int colBegin, bool isHidden)
        {
        }
    }
}
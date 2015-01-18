namespace Ll2NativeTests.DummySymbolWriter
{
    using PdbReader;

    public class DummyCompileUnitEntry : ICompileUnitEntry
    {
        public ISourceMethodBuilder DefineMethod(ISourceMethod method)
        {
            return new DummySourceMethodBuilder();
        }
    }
}
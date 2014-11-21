namespace Ll2NativeTests.DummySymbolWriter
{
    using PdbReader;

    public class DummySymbolWriter : ISymbolWriter
    {
        public ISourceFileEntry DefineDocument(string name)
        {
            return new DummySourceFileEntry();
        }

        public ICompileUnitEntry DefineCompilationUnit(ISourceFileEntry entry)
        {
            return new DummyCompileUnitEntry();
        }

        public ISourceMethodBuilder OpenMethod(ICompileUnitEntry compilationUnit, ISourceMethod method)
        {
            return new DummySourceMethodBuilder();
        }

        public void DefineLocalVariable(int slot, string name)
        {
        }

        public void CloseMethod()
        {
        }
    }
}

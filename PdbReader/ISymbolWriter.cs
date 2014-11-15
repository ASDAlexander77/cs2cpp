namespace PdbReader
{
    public interface ISymbolWriter
    {
        ISourceFileEntry DefineDocument(string name);

        ICompileUnitEntry DefineCompilationUnit(ISourceFileEntry entry);

        ISourceMethodBuilder OpenMethod(ICompileUnitEntry compilationUnit, ISourceMethod method);

        void DefineLocalVariable(int slot, string name);

        void CloseMethod();
    }

    public interface ISourceMethod
    {
        string Name { get; set; }

        string DisplayName { get; set; }

        string LinkageName { get; set; }

        int LineNumber { get; set; }

        string MethodReference { get; set; }
    }

    public interface ISourceMethodBuilder
    {
        void MarkSequencePoint(int offset, ISourceFile sourceFile, int lineBegin, int colBegin, bool isHidden);
    }

    public interface ICompileUnitEntry
    {
        ISourceMethodBuilder DefineMethod(ISourceMethod method);
    }

    public interface ISourceFile
    {
    }

    public interface ISourceFileEntry
    {
        string Directory { get; }

        string FileName { get; }

        ICompileUnitEntry DefineCompilationUnit();
    }
}

namespace PdbReader
{
    using System;

    public interface ISymbolWriter
    {
        ISourceFileEntry DefineDocument(string name);

        ICompileUnitEntry DefineCompilationUnit(ISourceFileEntry entry);

        ISourceMethodBuilder OpenMethod(ICompileUnitEntry compilationUnit, int methodNumber, ISourceMethod method);

        void DefineLocalVariable(int slot, string name);

        void CloseMethod();
    }

    public interface ISourceMethod
    {
    }

    public interface ISourceMethodBuilder
    {
        void MarkSequencePoint(int offset, ISourceFile sourceFile, int i, int colBegin, bool isHidden);
    }

    public interface ICompileUnitEntry
    {
        ISourceFile SourceFile { get; }
    }

    public interface ISourceFile
    {        
    }

    public interface ISourceFileEntry
    {
    }
}

namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using PdbReader;

    public class DebugInfoSourceMethodBuilder : ISourceMethodBuilder
    {
        public DebugInfoSourceMethodBuilder(ISourceMethod method)
        {
        }

        public void MarkSequencePoint(int offset, ISourceFile sourceFile, int lineBegin, int colBegin, bool isHidden)
        {

        }
    }
}

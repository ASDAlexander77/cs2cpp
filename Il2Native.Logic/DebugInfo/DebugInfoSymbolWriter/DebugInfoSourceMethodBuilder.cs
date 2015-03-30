namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using PdbReader;

    public class DebugInfoSourceMethodBuilder : ISourceMethodBuilder
    {
        private DebugInfoGenerator debugInfoGenerator;

        public DebugInfoSourceMethodBuilder(DebugInfoGenerator debugInfoGenerator)
        {
            this.debugInfoGenerator = debugInfoGenerator;
        }

        public void MarkSequencePoint(int offset, ISourceFile sourceFile, int lineBegin, int colBegin, bool isHidden)
        {
            this.debugInfoGenerator.SequencePoint(offset, lineBegin, colBegin);
        }
    }
}

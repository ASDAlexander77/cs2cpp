namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using Metadata.Model;
    using PdbReader;

    public class DebugInfoSourceMethodBuilder : ISourceMethodBuilder
    {
        private readonly DebugInfoGenerator debugInfoGenerator;
        private readonly CollectionMetadata function;

        public DebugInfoSourceMethodBuilder(
            DebugInfoGenerator debugInfoGenerator,
            ISourceMethod method,
            CollectionMetadata file,
            CollectionMetadata subprograms)
        {
            this.debugInfoGenerator = debugInfoGenerator;

            CollectionMetadata functionVariables;
            subprograms.Add(this.function = debugInfoGenerator.DefineMethod(method, file, out functionVariables));
        }

        public void MarkSequencePoint(int offset, ISourceFile sourceFile, int lineBegin, int colBegin, bool isHidden)
        {
            this.debugInfoGenerator.SequencePoint(offset, lineBegin, colBegin, this.function);
        }
    }
}
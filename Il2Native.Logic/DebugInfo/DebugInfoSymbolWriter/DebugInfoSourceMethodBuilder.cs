namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using Il2Native.Logic.Metadata.Model;

    using PdbReader;

    public class DebugInfoSourceMethodBuilder : ISourceMethodBuilder
    {
        private DebugInfoGenerator debugInfoGenerator;

        private CollectionMetadata function;

        private CollectionMetadata functionVariables;

        public DebugInfoSourceMethodBuilder(DebugInfoGenerator debugInfoGenerator, ISourceMethod method, CollectionMetadata file, CollectionMetadata subprograms)
        {
            this.debugInfoGenerator = debugInfoGenerator;

            CollectionMetadata subroutineTypes;
            CollectionMetadata functionVariables;
            subprograms.Add(this.function = debugInfoGenerator.DefineMethod(method, file, out functionVariables));

            this.functionVariables = functionVariables;
        }

        public void MarkSequencePoint(int offset, ISourceFile sourceFile, int lineBegin, int colBegin, bool isHidden)
        {
            this.debugInfoGenerator.SequencePoint(offset, lineBegin, colBegin, this.function);
        }
    }
}

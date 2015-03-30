namespace Il2Native.Logic.DebugInfo.DebugInfoSymbolWriter
{
    using PdbReader;

    public class DebugInfoCompileUnitEntry : ICompileUnitEntry
    {
        private DebugInfoGenerator debugInfoGenerator;

        public DebugInfoCompileUnitEntry(DebugInfoGenerator debugInfoGenerator)
        {
            this.debugInfoGenerator = debugInfoGenerator;
        }

        public ISourceMethodBuilder DefineMethod(ISourceMethod method)
        {
            return new DebugInfoSourceMethodBuilder(this.debugInfoGenerator);
        }
    }
}

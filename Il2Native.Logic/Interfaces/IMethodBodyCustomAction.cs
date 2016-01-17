namespace Il2Native.Logic.Gencode.InternalMethods
{
    using CodeParts;

    public interface IMethodBodyCustomAction
    {
        void Execute(ICodeWriterEx writer, OpCodePart opCode);
    }
}
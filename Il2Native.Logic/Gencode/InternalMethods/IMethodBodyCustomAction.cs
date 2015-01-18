namespace Il2Native.Logic.Gencode.InternalMethods
{
    using CodeParts;

    public interface IMethodBodyCustomAction
    {
        void Execute(LlvmWriter writer, OpCodePart opCode);
    }
}
namespace Il2Native.Logic.Gencode.InternalMethods
{
    using Il2Native.Logic.CodeParts;

    public interface IMethodBodyCustomAction
    {
        void Execute(LlvmWriter writer, OpCodePart opCode);
    }
}

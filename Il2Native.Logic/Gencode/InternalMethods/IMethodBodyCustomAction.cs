namespace Il2Native.Logic.Gencode.InternalMethods
{
    using CodeParts;

    public interface IMethodBodyCustomAction
    {
        void Execute(CWriter writer, OpCodePart opCode);
    }
}
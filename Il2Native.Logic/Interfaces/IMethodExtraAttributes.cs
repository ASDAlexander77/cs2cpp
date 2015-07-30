namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public interface IMethodExtraAttributes
    {
        bool IsStructObjectAdapter { get; }

        IMethod Original { get; } 
    }
}

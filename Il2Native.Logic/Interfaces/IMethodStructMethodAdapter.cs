namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public interface IMethodStructMethodAdapter
    {
        bool IsStructObjectAdapter { get; }

        IMethod Original { get; } 
    }
}

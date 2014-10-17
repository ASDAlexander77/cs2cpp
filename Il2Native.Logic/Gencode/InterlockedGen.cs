namespace Il2Native.Logic.Gencode
{
    using PEAssemblyReader;

    public static class InterlockedGen
    {
        public static bool IsInterlockedFunction(this IMethod method)
        {
            if (!method.IsExternal)
            {
                return false;
            }

            return method.Name == "CompareExchange";
        }
    }
}

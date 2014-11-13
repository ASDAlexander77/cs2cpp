namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Linq;
    using PEAssemblyReader;
    using Il2Native.Logic.CodeParts;

    public static class ArrayGen
    {
        public static bool IsArrayFunction(this IMethod method)
        {
            if (method.DeclaringType.FullName != "System.Array")
            {
                return false;
            }

            switch (method.Name)
            {
                case "get_Length":
                    return true;
            }

            return false;
        }

        public static void WriteArrayFunction(this IMethod method, OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            switch (method.Name)
            {
                case "get_Length":
                    llvmWriter.WriteArrayGetLength(opCodeMethodInfo);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

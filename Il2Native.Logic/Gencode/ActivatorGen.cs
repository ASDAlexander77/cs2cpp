namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Linq;
    using PEAssemblyReader;
    using Il2Native.Logic.CodeParts;

    public static class ActivatorGen
    {
        public static bool IsCreateInstanceFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            if (method.DeclaringType.FullName != "System.Activator")
            {
                return false;
            }

            switch (method.MetadataName)
            {
                case "CreateInstance`1":
                    return true;
            }

            return false;
        }

        public static void WriteCreateInstanceFunction(this IMethod method, OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            switch (method.MetadataName)
            {
                case "CreateInstance`1":

                    // this is dummy function which is not used now as using Boxing before calling CreateInstance is enough for us

                    var type = method.GetGenericArguments().First();
                    ////if (!type.IsStructureType())
                    ////{
                    ////    opCodeMethodInfo.Result = llvmWriter.WriteNewCallingDefaultConstructor(llvmWriter.Output, type);
                    ////}
                    ////else
                    ////{
                    ////    llvmWriter.WriteInit(opCodeMethodInfo, type, opCodeMethodInfo.Destination);
                    ////}

                    opCodeMethodInfo.Result = new ConstValue(null, type);

                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}

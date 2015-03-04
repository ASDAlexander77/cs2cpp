namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class FastAllocateStringGen
    {
        public static readonly string Name = "System.String System.String.FastAllocateString(Int32)";

        public static void Register(ITypeResolver typeResolver)
        {
            var codeBuilder = new IlCodeBuilder();
            codeBuilder.LoadArgument(0);
            codeBuilder.Add(Code.Call, 1);
            codeBuilder.Add(Code.Dup);
            codeBuilder.LoadArgument(0);
            codeBuilder.Add(Code.Stfld, 2);
            codeBuilder.Add(Code.Ret);

            var parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_Int32.ToParameter());

            var tokenResolutions = new List<object>();
            tokenResolutions.Add(
                new SynthesizedStaticMethod(
                    string.Empty,
                    typeResolver.System.System_String,
                    typeResolver.System.System_String,
                    parameters,
                    (llvmWriter, opCode) => llvmWriter.WriteNewMethodBody(opCode, typeResolver.System.System_String, enableStringFastAllocation: true)));
            tokenResolutions.Add(typeResolver.System.System_String.GetFieldByName("m_stringLength", typeResolver));

            var locals = new List<IType>();

            MethodBodyBank.Register(Name, codeBuilder.GetCode(), tokenResolutions, locals, parameters);
        }
    }
}
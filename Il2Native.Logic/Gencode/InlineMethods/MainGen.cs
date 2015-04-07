namespace Il2Native.Logic.Gencode.InlineMethods
{
    using System.Linq;

    using PEAssemblyReader;

    public class MainGen
    {
        public static IlCodeBuilder GetMainMethodBody(IlCodeBuilder ilCodeBuilder, IMethod main, ITypeResolver typeResolver)
        {
            var isVoid = main.ReturnType.IsVoid();
            var hasParameters = main.GetParameters().Any();

            var stringType = typeResolver.System.System_String;
            var bytePointerType = typeResolver.System.System_SByte.ToPointerType();

            // parameters
            ilCodeBuilder.Parameters.Add(typeResolver.System.System_Int32.ToParameter("value_0"));
            ilCodeBuilder.Parameters.Add(bytePointerType.ToPointerType().ToParameter("value_1"));

            // code to call gctors

            if (hasParameters)
            {
                // locals
                ilCodeBuilder.Locals.Add(stringType.ToArrayType(1));
                ilCodeBuilder.Locals.Add(typeResolver.System.System_Int32);

                // code
                ilCodeBuilder.LoadArgument(0);
                ilCodeBuilder.NewArray(stringType);
                ilCodeBuilder.SaveLocal(0);
                ilCodeBuilder.LoadConstant(0);
                ilCodeBuilder.SaveLocal(1);

                var jump = ilCodeBuilder.Branch(Code.Br, Code.Br_S);

                var loop = ilCodeBuilder.CreateLabel();

                ilCodeBuilder.LoadLocal(0);
                ilCodeBuilder.LoadLocal(1);
                ilCodeBuilder.LoadArgument(1);
                ilCodeBuilder.Add(Code.Dup);
                ilCodeBuilder.SizeOf(bytePointerType);
                ilCodeBuilder.Add(Code.Add);
                ilCodeBuilder.SaveArgument(1);
                ilCodeBuilder.Add(Code.Ldind_I);
                ilCodeBuilder.New(
                    IlReader.Constructors(stringType, typeResolver)
                            .First(c => c.GetParameters().Count() == 1 && c.GetParameters().First().ParameterType.TypeEquals(bytePointerType)));
                ilCodeBuilder.Add(Code.Stelem_Ref);
                ilCodeBuilder.LoadLocal(1);
                ilCodeBuilder.LoadConstant(1);
                ilCodeBuilder.Add(Code.Add);
                ilCodeBuilder.SaveLocal(1);

                ilCodeBuilder.Add(jump);

                ilCodeBuilder.LoadLocal(1);
                ilCodeBuilder.LoadArgument(0);

                ilCodeBuilder.Branch(Code.Blt, Code.Blt_S, loop);
            }

            // array created
            var environmentType = typeResolver.ResolveType("System.Environment");
            var setExitCode = environmentType.GetFirstMethodByName("set_ExitCode", typeResolver);
            var getExitCode = environmentType.GetFirstMethodByName("get_ExitCode", typeResolver);

            if (isVoid)
            {
                ilCodeBuilder.LoadConstant(0);
                ilCodeBuilder.Call(setExitCode);
            }
            else
            {
                ilCodeBuilder.LoadLocal(1);
            }
            
            ilCodeBuilder.Call(main);

            if (isVoid)
            {
                ilCodeBuilder.Call(getExitCode);
            }

            ilCodeBuilder.Add(Code.Ret);
            return ilCodeBuilder;
        }
    }
}
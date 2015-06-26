namespace Il2Native.Logic.Gencode.InlineMethods
{
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;

    public class MainGen
    {
        public static IlCodeBuilder GetMainMethodBody(IlCodeBuilder ilCodeBuilder, IMethod main, IEnumerable<IMethod> gctors, ITypeResolver typeResolver)
        {
            var isVoid = main.ReturnType.IsVoid();
            var hasParameters = main.GetParameters().Any();

            var stringType = typeResolver.System.System_String;
            var bytePointerType = typeResolver.System.System_SByte.ToPointerType();

            // parameters
            ilCodeBuilder.Parameters.Add(typeResolver.System.System_Int32.ToParameter("value_0"));
            ilCodeBuilder.Parameters.Add(bytePointerType.ToPointerType().ToParameter("value_1"));

            // code to call gctors
            ilCodeBuilder.Locals.Add(typeResolver.System.System_Int32);
            ilCodeBuilder.Locals.Add(typeResolver.System.System_Exception);

            var tryMain = ilCodeBuilder.Try();

            // call global ctors
            if (gctors != null)
            {
                foreach (var method in gctors)
                {
                    ilCodeBuilder.Call(method);
                }
            }

            if (hasParameters)
            {
                // locals
                ilCodeBuilder.Locals.Add(stringType.ToArrayType(1));

                // code
                ilCodeBuilder.LoadArgument(0);
                ilCodeBuilder.NewArray(stringType);
                ilCodeBuilder.SaveLocal(2);
                ilCodeBuilder.LoadConstant(0);
                ilCodeBuilder.SaveLocal(0);

                var jump = ilCodeBuilder.Branch(Code.Br, Code.Br_S);

                var loop = ilCodeBuilder.CreateLabel();

                ilCodeBuilder.LoadLocal(2);
                ilCodeBuilder.LoadLocal(0);
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
                ilCodeBuilder.LoadLocal(0);
                ilCodeBuilder.LoadConstant(1);
                ilCodeBuilder.Add(Code.Add);
                ilCodeBuilder.SaveLocal(0);

                ilCodeBuilder.Add(jump);

                ilCodeBuilder.LoadLocal(0);
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

            if (hasParameters)
            {
                ilCodeBuilder.LoadLocal(1);
            }

            ilCodeBuilder.Call(main);
            if (!isVoid)
            {
                ilCodeBuilder.Call(setExitCode);
            }

            var leaveBlock = ilCodeBuilder.Branch(Code.Leave, Code.Leave_S);

            ilCodeBuilder.TryEnd(tryMain);

            var catchMain = ilCodeBuilder.Catch(typeResolver.System.System_Exception, tryMain);

            // catch handler
            ilCodeBuilder.LoadConstant(-1);
            ilCodeBuilder.Call(setExitCode);

            // print message
            ilCodeBuilder.SaveLocal(1);
            ilCodeBuilder.LoadString("Unhandled exception: {0}: {1}");
            ilCodeBuilder.LoadLocal(1);
            ilCodeBuilder.Call(typeResolver.System.System_Object.GetFirstMethodByName("GetType", typeResolver));
            ilCodeBuilder.Call(typeResolver.System.System_Type.GetFirstMethodByName("get_FullName", typeResolver));
            ilCodeBuilder.LoadLocal(1);
            ilCodeBuilder.Call(typeResolver.System.System_Exception.GetFirstMethodByName("get_Message", typeResolver));
            ilCodeBuilder.Add(Code.Dup);
            var jumpCond = ilCodeBuilder.Branch(Code.Brtrue, Code.Brtrue_S);
            ilCodeBuilder.Add(Code.Pop);
            ilCodeBuilder.LoadString("<none>");
            ilCodeBuilder.Add(jumpCond);

            ilCodeBuilder.Call(
                typeResolver.ResolveType("System.Console")
                            .GetMethodsByName("WriteLine", typeResolver)
                            .First(m => m.GetParameters().Count() == 3 && m.GetParameters().First().ParameterType.TypeEquals(typeResolver.System.System_String)));
            
            ilCodeBuilder.Branch(Code.Leave, Code.Leave_S, leaveBlock);

            ilCodeBuilder.CatchEnd(catchMain);

            // leave block
            ilCodeBuilder.Add(leaveBlock);

            ilCodeBuilder.Call(getExitCode);
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }
    }
}
namespace Il2Native.Logic.Gencode.InlineMethods
{
    using System.Collections.Generic;
    using System.Linq;

    using PEAssemblyReader;

    public class MainGen
    {
        public static IlCodeBuilder GetMainMethodBody(IlCodeBuilder ilCodeBuilder, IMethod main, IEnumerable<IMethod> gctors, ICodeWriter codeWriter)
        {
            var isVoid = main.ReturnType.IsVoid();
            var hasParameters = main.GetParameters().Any();

            var stringType = codeWriter.System.System_String;
            var bytePointerType = codeWriter.System.System_SByte.ToPointerType();

            // parameters
            ilCodeBuilder.Parameters.Add(codeWriter.System.System_Int32.ToParameter("value_0"));
            ilCodeBuilder.Parameters.Add(bytePointerType.ToPointerType().ToParameter("value_1"));

            // code to call gctors
            ilCodeBuilder.Locals.Add(codeWriter.System.System_Int32);
            ilCodeBuilder.Locals.Add(codeWriter.System.System_Exception);

            const int localReturnCode = 0;
            const int localException = 1;
            const int localStringArray = 2;

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
                ilCodeBuilder.SaveLocal(localStringArray);
                ilCodeBuilder.LoadConstant(0);
                ilCodeBuilder.SaveLocal(localReturnCode);

                var jump = ilCodeBuilder.Branch(Code.Br, Code.Br_S);

                var loop = ilCodeBuilder.CreateLabel();

                ilCodeBuilder.LoadLocal(localStringArray);
                ilCodeBuilder.LoadLocal(localReturnCode);
                ilCodeBuilder.LoadArgument(1);
                ilCodeBuilder.Add(Code.Dup);
                ilCodeBuilder.SizeOf(bytePointerType);
                ilCodeBuilder.Add(Code.Add);
                ilCodeBuilder.SaveArgument(1);
                ilCodeBuilder.Add(Code.Ldind_I);
                ilCodeBuilder.New(
                    IlReader.Constructors(stringType, codeWriter)
                            .First(c => c.GetParameters().Count() == 1 && c.GetParameters().First().ParameterType.TypeEquals(bytePointerType)));
                ilCodeBuilder.Add(Code.Stelem_Ref);
                ilCodeBuilder.LoadLocal(localReturnCode);
                ilCodeBuilder.LoadConstant(1);
                ilCodeBuilder.Add(Code.Add);
                ilCodeBuilder.SaveLocal(localReturnCode);

                ilCodeBuilder.Add(jump);

                ilCodeBuilder.LoadLocal(localReturnCode);
                ilCodeBuilder.LoadArgument(0);

                ilCodeBuilder.Branch(Code.Blt, Code.Blt_S, loop);
            }

            // array created
            var environmentType = codeWriter.ResolveType("System.Environment");
            var setExitCode = OpCodeExtensions.GetFirstMethodByName(environmentType, "set_ExitCode", codeWriter);
            var getExitCode = OpCodeExtensions.GetFirstMethodByName(environmentType, "get_ExitCode", codeWriter);

            if (isVoid)
            {
                ilCodeBuilder.LoadConstant(0);
                ilCodeBuilder.Call(setExitCode);
            }

            if (hasParameters)
            {
                ilCodeBuilder.LoadLocal(localStringArray);
            }

            ilCodeBuilder.Call(main);
            if (!isVoid)
            {
                ilCodeBuilder.Call(setExitCode);
            }

            var leaveBlock = ilCodeBuilder.Branch(Code.Leave, Code.Leave_S);

            ilCodeBuilder.TryEnd(tryMain);

            var catchMain = ilCodeBuilder.Catch(codeWriter.System.System_Exception, tryMain);

            // catch handler
            ilCodeBuilder.LoadConstant(-1);
            ilCodeBuilder.Call(setExitCode);

            // print message
            ilCodeBuilder.SaveLocal(localException);
            ilCodeBuilder.LoadString("Unhandled exception: {0}: {1}");
            ilCodeBuilder.LoadLocal(localException);
            ilCodeBuilder.Call(OpCodeExtensions.GetFirstMethodByName(codeWriter.System.System_Object, "GetType", codeWriter));
            ilCodeBuilder.Call(OpCodeExtensions.GetFirstMethodByName(codeWriter.System.System_Type, "get_FullName", codeWriter));
            ilCodeBuilder.LoadLocal(localException);
            ilCodeBuilder.Call(OpCodeExtensions.GetFirstMethodByName(codeWriter.System.System_Exception, "get_Message", codeWriter));
            ilCodeBuilder.Add(Code.Dup);
            var jumpCond = ilCodeBuilder.Branch(Code.Brtrue, Code.Brtrue_S);
            ilCodeBuilder.Add(Code.Pop);
            ilCodeBuilder.LoadString("<none>");
            ilCodeBuilder.Add(jumpCond);

            ilCodeBuilder.Call(
                OpCodeExtensions.GetMethodsByName(codeWriter.ResolveType("System.Console"), "WriteLine", codeWriter)
                            .First(m => m.GetParameters().Count() == 3 && m.GetParameters().First().ParameterType.TypeEquals(codeWriter.System.System_String)));
            
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
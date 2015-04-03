namespace Il2Native.Logic.Gencode.InlineMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using PEAssemblyReader;

    public class MainGen
    {
        public static void GetLoadingArgumentsMethodBody(
            bool isVoid,
            ITypeResolver typeResolver,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            var stringType = typeResolver.System.System_String;
            var bytePointerType = typeResolver.System.System_SByte.ToPointerType();

            parameters = new List<IParameter>();
            parameters.Add(typeResolver.System.System_Int32.ToParameter("value"));
            parameters.Add(bytePointerType.ToPointerType().ToParameter("value"));

            var codeList = new List<object>();
            codeList.AddRange(
                new object[]
                {
                    Code.Ldarg_0,
                    Code.Newarr,
                    1,
                    0,
                    0,
                    0,
                    Code.Stloc_0,
                    Code.Ldc_I4_0,
                    Code.Stloc_1,
                    Code.Br_S,
                    24,
                    Code.Ldloc_0,
                    Code.Ldloc_1,
                    Code.Ldarg_1,
                    Code.Dup,
                    Code.Sizeof,
                    2,
                    0,
                    0,
                    0,
                    Code.Add,
                    Code.Starg_S,
                    1,
                    Code.Ldind_I,
                    Code.Newobj,
                    3,
                    0,
                    0,
                    0,
                    Code.Stelem_Ref,
                    Code.Ldloc_1,
                    Code.Ldc_I4_1,
                    Code.Add,
                    Code.Stloc_1,
                    Code.Ldloc_1,
                    Code.Ldarg_0,
                    Code.Blt_S,
                    -28
                });

            code = codeList.ToArray();

            locals = new List<IType>();
            locals.Add(stringType.ToArrayType(1));
            locals.Add(typeResolver.System.System_Int32);

            tokenResolutions = new List<object>();
            tokenResolutions.Add(stringType);
            tokenResolutions.Add(bytePointerType);
            tokenResolutions.Add(
                IlReader.Constructors(stringType, typeResolver)
                    .First(
                        c =>
                            c.GetParameters().Count() == 1 &&
                            c.GetParameters().First().ParameterType.TypeEquals(bytePointerType)));
        }
    }
}
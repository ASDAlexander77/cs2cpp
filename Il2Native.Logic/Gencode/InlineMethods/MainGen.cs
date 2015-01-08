namespace Il2Native.Logic.Gencode.InlineMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    public class MainGen
    {
        public static void GetLoadingArgumentsMethodBody(bool isVoid, ICodeWriter codeWriter, out object[] code, out IList<object> tokenResolutions, out IList<IType> locals, out IList<IParameter> parameters)
        {
            parameters = new List<IParameter>();
            parameters.Add(codeWriter.ResolveType("System.Int32").ToParameter());
            parameters.Add(codeWriter.ResolveType("System.Byte").ToPointerType().ToPointerType().ToParameter());

            var codeList = new List<object>();
            codeList.AddRange(new object[]
                    {
                        Code.Ldc_I4_0,
                        Code.Newarr,
                        1,
                        0,
                        0,
                        0,
                    });

            code = codeList.ToArray();

            locals = new List<IType>();
            locals.Add(codeWriter.ResolveType("System.String").ToArrayType(1));
            locals.Add(codeWriter.ResolveType("System.Int32"));

            tokenResolutions = new List<object>();
            tokenResolutions.Add(codeWriter.ResolveType("System.String"));
        }
    }
}

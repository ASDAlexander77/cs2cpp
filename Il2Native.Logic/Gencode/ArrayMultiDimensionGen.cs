namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using CodeParts;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    public class ArrayMultiDimensionGen
    {
        /*
        public ArrayMultiDimensionGen_Ctor()
        {
            this.rank = 2;
            this.typeCode = type.GetElementType().TypeCode;
            this.elementSize = type.GetSize();
            this.length = dim1 * dim2;
            this.lowerBounds = new int[2] { 0, 0 };
            this.bounds = new int[2] { dim1, dim2 };
        }
         */

        public static void GetMultiDimensionArrayCtor(
            IType type,
            ITypeResolver typeResolver,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            var codeList = new List<object>();

            codeList.AddRange(
                new object[]
                    {
                        Code.Ret
                    });

            // locals
            locals = new List<IType>();

            // tokens
            tokenResolutions = new List<object>();

            // code
            code = codeList.ToArray();

            // parameters
            var intType = typeResolver.ResolveType("System.Int32");
            parameters = Enumerable.Range(0, type.ArrayRank).Select(n => intType.ToParameter()).ToList();
        }
    }
}

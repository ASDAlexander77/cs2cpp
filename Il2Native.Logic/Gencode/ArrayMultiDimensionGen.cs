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

            var rank = BitConverter.GetBytes((int)type.ArrayRank);
            var typeCode = BitConverter.GetBytes((int)type.GetTypeCode());
            var elementSize = BitConverter.GetBytes((int)type.GetTypeSize(typeResolver, true));

            codeList.AddRange(
                new object[]
                    {
                        Code.Ldarg_0,
                        Code.Dup,
                        Code.Dup,
                        Code.Ldc_I4,
                        (byte)rank[0],
                        (byte)rank[1],
                        (byte)rank[2],
                        (byte)rank[3],
                        Code.Stfld,
                        1,
                        0,
                        0,
                        0,
                        Code.Ldc_I4,
                        (byte)typeCode[0],
                        (byte)typeCode[1],
                        (byte)typeCode[2],
                        (byte)typeCode[3],
                        Code.Stfld,
                        2,
                        0,
                        0,
                        0,
                        Code.Ldc_I4,
                        (byte)elementSize[0],
                        (byte)elementSize[1],
                        (byte)elementSize[2],
                        (byte)elementSize[3],
                        Code.Stfld,
                        3,
                        0,
                        0,
                        0,
                        Code.Ret
                    });

            // locals
            locals = new List<IType>();

            // tokens
            tokenResolutions = new List<object>();
            tokenResolutions.Add(type.GetFieldByName("rank", typeResolver));
            tokenResolutions.Add(type.GetFieldByName("typeCode", typeResolver));
            tokenResolutions.Add(type.GetFieldByName("elementSize", typeResolver));

            // code
            code = codeList.ToArray();

            // parameters
            var intType = typeResolver.ResolveType("System.Int32");
            parameters = Enumerable.Range(0, type.ArrayRank).Select(n => intType.ToParameter()).ToList();
        }
    }
}

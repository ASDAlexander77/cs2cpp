namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using CodeParts;

    using PEAssemblyReader;

    public static class ArrayMultiDimensionGen
    {
        public static IEnumerable<IField> GetFields(IType arrayType, ITypeResolver typeResolver)
        {
            Debug.Assert(arrayType.IsMultiArray, "This is for multi arrays only");

            var shortType = typeResolver.System.System_Int16;
            var intType = typeResolver.System.System_Int32;
            var pointerType = typeResolver.System.System_Byte.ToPointerType();

            // return dummy fields to compensate interfaces for SingleDim array
            foreach (var dummyField in arrayType.GetElementType().ToArrayType(1).GetInterfaces())
            {
                yield return pointerType.ToField(arrayType, dummyField.Name);
            }

            yield return shortType.ToField(arrayType, "rank");
            yield return shortType.ToField(arrayType, "typeCode");
            yield return intType.ToField(arrayType, "elementSize");
            yield return intType.ToField(arrayType, "length");
            yield return intType.ToArrayType(1).ToField(arrayType, "lowerBounds");
            yield return intType.ToArrayType(1).ToField(arrayType, "lengths");
            yield return arrayType.GetElementType().ToField(arrayType, "data", isFixed: true);
        }

        public static void GetMultiDimensionArrayCtor(
            IType arrayType,
            ITypeResolver typeResolver,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            Debug.Assert(arrayType.IsMultiArray, "This is for multi arrays only");

            var codeList = new List<object>();

            var arrayRank = arrayType.ArrayRank;
            var elementType = arrayType.GetElementType();
            var typeCode = elementType.GetTypeCode();
            var elementSize = elementType.GetTypeSize(typeResolver, true);

            codeList.AddRange(
                new object[]
                    {
                        Code.Ldarg_0,
                        Code.Dup,
                        Code.Dup,
                        Code.Dup
                    });

            codeList.AppendLoadInt(arrayRank);
            codeList.AppendInt(Code.Stfld, 1);
            codeList.AppendLoadInt(typeCode);
            codeList.AppendInt(Code.Stfld, 2);
            codeList.AppendLoadInt(elementSize);
            codeList.AppendInt(Code.Stfld, 3);

            // init length
            // init multiplier
            codeList.Add(Code.Ldc_I4_1);

            foreach (var i in Enumerable.Range(0, arrayType.ArrayRank))
            {
                codeList.AppendLoadArgument(arrayType.ArrayRank - i);
                codeList.Add(Code.Mul);
            }

            codeList.AppendInt(Code.Stfld, 8);

            // init lowerBounds
            // set all 0
            codeList.AppendLoadInt(arrayRank);
            codeList.AppendInt(Code.Newarr, 4);
            codeList.Add(Code.Stloc_0);

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, arrayRank))
            {
                codeList.Add(Code.Ldloc_0);
                codeList.AppendLoadInt(i);
                codeList.AddRange(
                    new object[]
                    {
                        Code.Ldc_I4_0,
                        Code.Stelem_I4
                });
            }

            // save new array into field lowerBounds
            codeList.AddRange(
                new object[]
                {
                        Code.Ldarg_0,
                        Code.Ldloc_0,
                });
            codeList.AppendInt(Code.Stfld, 5);

            // init Bounds
            codeList.AppendLoadInt(arrayRank);
            codeList.AppendInt(Code.Newarr, 6);
            codeList.Add(Code.Stloc_1);

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, arrayRank))
            {
                codeList.Add(Code.Ldloc_1);
                codeList.AppendLoadInt(i);
                //codeList.AppendLoadArgument(i + 1);
                codeList.AppendLoadArgument(arrayRank - i);
                codeList.AddRange(
                    new object[]
                    {
                        Code.Stelem_I4
                    });
            }

            // save new array into field lowerBounds
            codeList.AddRange(
                new object[]
                {
                        Code.Ldarg_0,
                        Code.Ldloc_1,
                });
            codeList.AppendInt(Code.Stfld, 7);

            // return
            codeList.AddRange(
                new object[]
                {
                        Code.Ret
                });

            // locals
            locals = new List<IType>();
            locals.Add(typeResolver.System.System_Int32.ToArrayType(1));
            locals.Add(typeResolver.System.System_Int32.ToArrayType(1));

            // tokens
            tokenResolutions = new List<object>();
            tokenResolutions.Add(arrayType.GetFieldByName("rank", typeResolver));
            tokenResolutions.Add(arrayType.GetFieldByName("typeCode", typeResolver));
            tokenResolutions.Add(arrayType.GetFieldByName("elementSize", typeResolver));
            // lowerBounds
            tokenResolutions.Add(typeResolver.System.System_Int32);
            tokenResolutions.Add(arrayType.GetFieldByName("lowerBounds", typeResolver));
            // bounds
            tokenResolutions.Add(typeResolver.System.System_Int32);
            tokenResolutions.Add(arrayType.GetFieldByName("lengths", typeResolver));
            tokenResolutions.Add(arrayType.GetFieldByName("length", typeResolver));

            // code
            code = codeList.ToArray();

            // parameters
            parameters = GetParameters(arrayType, typeResolver);
        }

        public static void GetMultiDimensionArrayGet(
            IType arrayType,
            ITypeResolver typeResolver,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            Debug.Assert(arrayType.IsMultiArray, "This is for multi arrays only");

            var codeList = new List<object>();

            codeList.Add(Code.Ldarg_0);

            // element index 
            codeList.AddRange(GetIndexPartMethodBody(arrayType, typeResolver, out tokenResolutions, out locals));

            codeList.AppendInt(Code.Ldelem, 4);

            // return
            codeList.Add(Code.Ret);

            // code
            code = codeList.ToArray();

            // parameters
            parameters = GetParameters(arrayType, typeResolver);
        }

        public static void GetMultiDimensionArraySet(
            IType arrayType,
            ITypeResolver typeResolver,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            Debug.Assert(arrayType.IsMultiArray, "This is for multi arrays only");

            var codeList = new List<object>();

            codeList.Add(Code.Ldarg_0);

            // element index
            codeList.AddRange(GetIndexPartMethodBody(arrayType, typeResolver, out tokenResolutions, out locals));

            // put value on stack (+ 'this' as first)
            codeList.AppendLoadArgument(arrayType.ArrayRank + 1);

            codeList.AppendInt(Code.Stelem, 4);

            // return
            codeList.Add(Code.Ret);

            // code
            code = codeList.ToArray();

            // parameters
            var list = GetParameters(arrayType, typeResolver);
            list.Add(arrayType.GetElementType().ToParameter("arrayElement"));
            parameters = list;
        }

        public static void GetMultiDimensionArrayAddress(
            IType arrayType,
            ITypeResolver typeResolver,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            Debug.Assert(arrayType.IsMultiArray, "This is for multi arrays only");

            var codeList = new List<object>();

            codeList.Add(Code.Ldarg_0);

            // element index 
            codeList.AddRange(GetIndexPartMethodBody(arrayType, typeResolver, out tokenResolutions, out locals));

            codeList.AppendInt(Code.Ldelema, 4);

            // return
            codeList.Add(Code.Ret);

            // code
            code = codeList.ToArray();

            // parameters
            parameters = GetParameters(arrayType, typeResolver);
        }

        public static List<IParameter> GetParameters(IType type, ITypeResolver typeResolver)
        {
            var intType = typeResolver.System.System_Int32;
            var index = 0;
            return Enumerable.Range(0, type.ArrayRank).Select(n => intType.ToParameter("param" + index++)).ToList();
        }

        public static void MultiDimArrayAllocationSizeMethodBody(
            IlCodeBuilder codeBuilder,
            ITypeResolver typeResolver,
            IType arrayType)
        {
            // add element size
            var elementType = arrayType.GetElementType();
            var elementSize = elementType.GetTypeSize(typeResolver, true);
            codeBuilder.SizeOf(elementType);

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, arrayType.ArrayRank))
            {
                codeBuilder.LoadArgument(i);
                codeBuilder.Add(Code.Mul);
            }

            // add element size
            codeBuilder.SizeOf(arrayType);
            codeBuilder.Add(Code.Add);

            // calculate alignment
            codeBuilder.Add(Code.Dup);

            var alignForType = Math.Max(CWriter.PointerSize, !elementType.IsStructureType() ? elementSize : CWriter.PointerSize);
            codeBuilder.LoadConstant(alignForType - 1);
            codeBuilder.Add(Code.Add);

            codeBuilder.LoadConstant(~(alignForType - 1));
            codeBuilder.Add(Code.And);

            // parameters
            codeBuilder.Parameters.AddRange(GetParameters(arrayType, typeResolver));
        }

        private static List<object> GetIndexPartMethodBody(
            IType arrayType,
            ITypeResolver typeResolver,
            out IList<object> tokenResolutions,
            out IList<IType> locals)
        {
            var codeList = new List<object>();

            // init multiplier
            codeList.Add(Code.Ldc_I4_1);
            codeList.Add(Code.Stloc_0);

            // load index, load lowerBound value, calculate shift
            foreach (var i in Enumerable.Range(0, arrayType.ArrayRank))
            {
                // load index (first is 'this')
                //codeList.AppendLoadArgument(i + 1);
                codeList.AppendLoadArgument(arrayType.ArrayRank - i);

                // - lowerBounds[index]
                // load lowerBound value by index
                codeList.Add(Code.Ldarg_0);
                // load field 1 = lowerBounds
                codeList.AppendInt(Code.Ldfld, 2);
                // lower bound index
                codeList.AppendLoadInt(i);
                // load element
                codeList.Add(Code.Ldelem_I4);

                // calculate diff.
                codeList.Add(Code.Sub);

                if (i > 0)
                {
                    // * bounds[index - 1]
                    // load bound value by index
                    codeList.Add(Code.Ldarg_0);
                    // load field 2 = bounds
                    codeList.AppendInt(Code.Ldfld, 3);
                    // lower bound index
                    codeList.AppendLoadInt(i - 1);
                    // load element
                    codeList.Add(Code.Ldelem_I4);

                    // calculate multiplier
                    codeList.Add(Code.Ldloc_0);
                    codeList.Add(Code.Mul);
                    codeList.Add(Code.Dup);
                    codeList.Add(Code.Stloc_0);

                    // multiply index.
                    codeList.Add(Code.Mul);

                    // + sum
                    codeList.Add(Code.Add);
                }
            }

            // End of Code
            // tokens
            tokenResolutions = new List<object>();
            // data
            tokenResolutions.Add(arrayType.GetFieldByName("data", typeResolver));
            // lowerBounds
            tokenResolutions.Add(arrayType.GetFieldByName("lowerBounds", typeResolver));
            // bounds
            tokenResolutions.Add(arrayType.GetFieldByName("lengths", typeResolver));
            // element type
            tokenResolutions.Add(arrayType.GetElementType());
            // element type as pointer
            tokenResolutions.Add(arrayType.GetElementType().ToPointerType());

            locals = new List<IType>();
            locals.Add(typeResolver.System.System_Int32);

            return codeList;
        }
    }
}

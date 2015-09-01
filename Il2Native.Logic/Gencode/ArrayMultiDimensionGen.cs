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
            out IlCodeBuilder ilCodeBuilder)
        {
            Debug.Assert(arrayType.IsMultiArray, "This is for multi arrays only");

            ilCodeBuilder = new IlCodeBuilder();

            var arrayRank = arrayType.ArrayRank;
            var elementType = arrayType.GetElementType();
            var typeCode = elementType.GetTypeCode();

            var token1 = arrayType.GetFieldByName("rank", typeResolver);
            var token2 = arrayType.GetFieldByName("typeCode", typeResolver);
            var token3 = arrayType.GetFieldByName("elementSize", typeResolver);
            // lowerBounds
            var token4 = typeResolver.System.System_Int32;
            var token5 = arrayType.GetFieldByName("lowerBounds", typeResolver);
            // bounds
            var token6 = typeResolver.System.System_Int32;
            var token7 = arrayType.GetFieldByName("lengths", typeResolver);
            var token8 = arrayType.GetFieldByName("length", typeResolver);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Duplicate();
            ilCodeBuilder.Duplicate();
            ilCodeBuilder.Duplicate();

            ilCodeBuilder.LoadConstant(arrayRank);
            ilCodeBuilder.SaveField(token1);
            ilCodeBuilder.LoadConstant(typeCode);
            ilCodeBuilder.SaveField(token2);
            ilCodeBuilder.SizeOf(elementType);
            ilCodeBuilder.SaveField(token3);

            // init length
            // init multiplier
            ilCodeBuilder.Add(Code.Ldc_I4_1);

            foreach (var i in Enumerable.Range(0, arrayType.ArrayRank))
            {
                ilCodeBuilder.LoadArgument(arrayType.ArrayRank - i);
                ilCodeBuilder.Add(Code.Mul);
            }

            ilCodeBuilder.SaveField(token8);

            // init lowerBounds
            // set all 0
            ilCodeBuilder.LoadConstant(arrayRank);
            ilCodeBuilder.NewArray(token4);
            ilCodeBuilder.Add(Code.Stloc_0);

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, arrayRank))
            {
                ilCodeBuilder.LoadLocal(0);
                ilCodeBuilder.LoadConstant(i);
                ilCodeBuilder.LoadConstant(0);
                ilCodeBuilder.Add(Code.Stelem_I4);
            }

            // save new array into field lowerBounds
            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadLocal(0);
            ilCodeBuilder.SaveField(token5);

            // init Bounds
            ilCodeBuilder.LoadConstant(arrayRank);
            ilCodeBuilder.NewArray(token6);
            ilCodeBuilder.Add(Code.Stloc_1);

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, arrayRank))
            {
                ilCodeBuilder.Add(Code.Ldloc_1);
                ilCodeBuilder.LoadConstant(i);
                //codeList.AppendLoadArgument(i + 1);
                ilCodeBuilder.LoadArgument(arrayRank - i);
                ilCodeBuilder.Add(Code.Stelem_I4);
            }

            // save new array into field lowerBounds
            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadLocal(1);
            ilCodeBuilder.SaveField(token7);

            // return
            ilCodeBuilder.Return();

            // locals
            ilCodeBuilder.Locals.Add(typeResolver.System.System_Int32.ToArrayType(1));
            ilCodeBuilder.Locals.Add(typeResolver.System.System_Int32.ToArrayType(1));

            // parameters
            ilCodeBuilder.Parameters.AddRange(GetParameters(arrayType, typeResolver));
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

            if (!elementType.IsStructureType())
            {
                var alignForType = Math.Max(CWriter.PointerSize, elementType.GetKnownTypeSize());
                codeBuilder.LoadConstant(alignForType - 1);
                codeBuilder.Add(Code.Add);

                codeBuilder.LoadConstant(~(alignForType - 1));
                codeBuilder.Add(Code.And);
            }
            else
            {
                codeBuilder.SizeOf(elementType);
                codeBuilder.LoadConstant(1);
                codeBuilder.Add(Code.Sub);
                codeBuilder.Add(Code.Add);

                codeBuilder.SizeOf(elementType);
                codeBuilder.LoadConstant(1);
                codeBuilder.Add(Code.Sub);
                codeBuilder.Add(Code.Not);
                codeBuilder.Add(Code.And);
            }

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

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using CodeParts;

    using Il2Native.Logic.Gencode.InlineMethods;
    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    public static class ArrayMultiDimensionGen
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
            var typeCode = arrayType.GetTypeCode();
            var elementSize = arrayType.GetTypeSize(typeResolver, true);

            codeList.AddRange(
                new object[]
                    {
                        Code.Ldarg_0,
                        Code.Dup,
                        Code.Dup,
                    });

            codeList.AppendLoadInt(arrayRank);
            codeList.AppendInt(Code.Stfld, 1);
            codeList.AppendLoadInt(typeCode);
            codeList.AppendInt(Code.Stfld, 2);
            codeList.AppendLoadInt(elementSize);
            codeList.AppendInt(Code.Stfld, 3);

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
                codeList.AppendLoadArg(i + 1);
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
            locals.Add(typeResolver.ResolveType("System.Int32").ToArrayType(1));
            locals.Add(typeResolver.ResolveType("System.Int32").ToArrayType(1));

            // tokens
            tokenResolutions = new List<object>();
            tokenResolutions.Add(arrayType.GetFieldByName("rank", typeResolver));
            tokenResolutions.Add(arrayType.GetFieldByName("typeCode", typeResolver));
            tokenResolutions.Add(arrayType.GetFieldByName("elementSize", typeResolver));
            // lowerBounds
            tokenResolutions.Add(typeResolver.ResolveType("System.Int32"));
            tokenResolutions.Add(arrayType.GetFieldByName("lowerBounds", typeResolver));
            // bounds
            tokenResolutions.Add(typeResolver.ResolveType("System.Int32"));
            tokenResolutions.Add(arrayType.GetFieldByName("lengths", typeResolver));

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

            // index for expr: *(data + index)
            // element index 
            codeList.AddRange(GetIndexPartMethodBody(arrayType, typeResolver, out tokenResolutions));

            // load element by type
            var loadIndirectCode = arrayType.GetElementType().GetLoadIndirectCode();
            codeList.Add(loadIndirectCode);

            if (loadIndirectCode == Code.Ldind_Ref)
            {
                // cast to type
                codeList.Add(Code.Conv_I);
                codeList.AppendInt(Code.Castclass, 4);
            }

            // return
            codeList.Add(Code.Ret);

            // locals
            locals = new List<IType>();

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

            var saveIndirectCode = arrayType.GetElementType().GetSaveIndirectCode();

            // index for expr: *(data + index)
            // element index 
            codeList.AddRange(GetIndexPartMethodBody(arrayType, typeResolver, out tokenResolutions));
            if (saveIndirectCode == Code.Stind_Ref)
            {
                codeList.AppendInt(Code.Castclass, 5);
            }

            // put value on stack (+ 'this' as first)
            codeList.AppendLoadArg(arrayType.ArrayRank + 1);

            // save element by type
            codeList.Add(saveIndirectCode);

            // return
            codeList.Add(Code.Ret);

            // locals
            locals = new List<IType>();

            // code
            code = codeList.ToArray();

            // parameters
            var list = GetParameters(arrayType, typeResolver);
            list.Add(arrayType.GetElementType().ToParameter());
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

            // index for expr: *(data + index)
            // element index 
            codeList.AddRange(GetIndexPartMethodBody(arrayType, typeResolver, out tokenResolutions));
            codeList.AppendInt(Code.Castclass, 5);

            // return
            codeList.Add(Code.Ret);

            // locals
            locals = new List<IType>();

            // code
            code = codeList.ToArray();

            // parameters
            parameters = GetParameters(arrayType, typeResolver);
        }

        public static List<IParameter> GetParameters(IType type, ITypeResolver typeResolver)
        {
            var intType = typeResolver.ResolveType("System.Int32");
            return Enumerable.Range(0, type.ArrayRank).Select(n => intType.ToParameter()).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        public static FullyDefinedReference WriteMultiDimArrayAllocationSize(
            this LlvmWriter llvmWriter,
            OpCodePart opCode,
            IType arrayType)
        {
            Debug.Assert(arrayType.IsMultiArray, "This is for multi arrays only");

            var writer = llvmWriter.Output;

            writer.WriteLine("; Calculate MultiDim allocation size");
            return WriteCalculationPartOfMultiDimArrayAllocationSize(llvmWriter, arrayType, null);
        }

        public static FullyDefinedReference WriteCalculationPartOfMultiDimArrayAllocationSize(
            this LlvmWriter llvmWriter,
            IType type,
            IGenericContext currentGenericContext)
        {
            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            GetCalculationPartOfMultiDimArrayAllocationSizeMethodBody(
                llvmWriter,
                type,
                out code,
                out tokenResolutions,
                out locals,
                out parameters);

            var constructedMethod = MethodBodyBank.GetMethodDecorator(null, code, tokenResolutions, locals, parameters);

            // actual write
            var opCodes = llvmWriter.WriteCustomMethodPart(constructedMethod, currentGenericContext);
            return opCodes.Last().Result;
        }

        private static void GetCalculationPartOfMultiDimArrayAllocationSizeMethodBody(
            LlvmWriter llvmWriter,
            IType arrayType,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            var codeList = new List<object>();

            // add element size
            var elementType = arrayType.GetElementType();
            var elementSize = elementType.GetTypeSize(llvmWriter, true);
            codeList.AppendLoadInt(elementSize);

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, arrayType.ArrayRank))
            {
                codeList.AppendLoadArg(i);
                codeList.Add(Code.Mul);
            }

            // add element size
            var multiArrayTypeSizeWithoutArrayData = arrayType.GetTypeSize(llvmWriter);
            codeList.AppendLoadInt(multiArrayTypeSizeWithoutArrayData);
            codeList.Add(Code.Add);

            // calculate alignment
            codeList.Add(Code.Dup);

            var alignForType = Math.Max(LlvmWriter.PointerSize, !elementType.IsStructureType() ? elementSize : LlvmWriter.PointerSize);
            codeList.AppendLoadInt(alignForType);
            codeList.Add(Code.Rem);

            codeList.Add(Code.Add);

            codeList.AppendLoadInt(alignForType);
            codeList.Add(Code.Sub);

            // locals
            locals = new List<IType>();

            // tokens
            tokenResolutions = new List<object>();

            // parameters
            parameters = GetParameters(arrayType, llvmWriter);

            code = codeList.ToArray();
        }

        private static List<object> GetIndexPartMethodBody(
            IType arrayType,
            ITypeResolver typeResolver,
            out IList<object> tokenResolutions)
        {
            var codeList = new List<object>();

            // load index, load lowerBound value, calculate shift
            foreach (var i in Enumerable.Range(0, arrayType.ArrayRank))
            {
                // load index (first is 'this')
                codeList.AppendLoadArg(i + 1);

                // - lowerBounds[index]
                // load lowerBound value by index
                codeList.Add(Code.Ldarg_0);
                // load field 1 = lowerBounds
                codeList.AppendInt(Code.Ldfld, 2);
                // lower bound index
                codeList.AppendLoadInt(i);
                // load element
                if (LlvmWriter.PointerSize == 8)
                {
                    codeList.Add(Code.Ldelem_I8);
                }
                else
                {
                    codeList.Add(Code.Ldelem_I4);
                }

                // calculate diff.
                codeList.Add(Code.Sub);

                if (i > 0)
                {
                    // * bounds[index - 1]
                    // load lowerBound value by index
                    codeList.Add(Code.Ldarg_0);
                    // load field 2 = bounds
                    codeList.AppendInt(Code.Ldfld, 3);
                    // lower bound index
                    codeList.AppendLoadInt(i - 1);
                    // load element
                    if (LlvmWriter.PointerSize == 8)
                    {
                        codeList.Add(Code.Ldelem_I8);
                    }
                    else
                    {
                        codeList.Add(Code.Ldelem_I4);
                    }

                    // calculate diff.
                    codeList.Add(Code.Mul);

                    // + sum
                    codeList.Add(Code.Add);
                }
            }

            // add address of 'data' field and multiply index by element size

            // load element size
            codeList.AppendLoadInt(arrayType.GetElementType().GetTypeSize(typeResolver, true));

            codeList.Add(Code.Mul);

            // data for expr: *(data + index)
            // this
            codeList.Add(Code.Ldarg_0);

            // field 'data'
            codeList.AppendInt(Code.Ldflda, 1);

            codeList.Add(Code.Add);

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

            return codeList;
        }

        public static void AppendLoadArg(this List<object> codeList, int argIndex)
        {
            switch (argIndex)
            {
                case 0:
                    codeList.Add(Code.Ldarg_0);
                    break;
                case 1:
                    codeList.Add(Code.Ldarg_1);
                    break;
                case 2:
                    codeList.Add(Code.Ldarg_2);
                    break;
                case 3:
                    codeList.Add(Code.Ldarg_3);
                    break;
                default:
                    codeList.AppendInt(Code.Ldarg, argIndex);
                    break;
            }
        }

        public static void AppendLoadInt(this List<object> codeList, int value)
        {
            codeList.AppendInt(Code.Ldc_I4, value);
        }

        public static void AppendInt(this List<object> codeList, Code op, int valueInt)
        {
            var value = BitConverter.GetBytes(valueInt);
            codeList.AddRange(
                new object[]
                    {
                        op, (byte)value[0], (byte)value[1], (byte)value[2], (byte)value[3],
                    });
        }
    }
}

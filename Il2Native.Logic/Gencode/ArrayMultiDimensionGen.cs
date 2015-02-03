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
                    });

            // init lowerBounds
            // set all 0
            codeList.AddRange(
                new object[]
                {
                        Code.Ldc_I4,
                        (byte)rank[0],
                        (byte)rank[1],
                        (byte)rank[2],
                        (byte)rank[3],
                        Code.Newarr,
                        4,
                        0,
                        0,
                        0,
                        Code.Stloc_0,
                });

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, type.ArrayRank))
            {
                var index = BitConverter.GetBytes((int)i);
                codeList.AddRange(
                    new object[]
                {
                        Code.Ldloc_0,
                        Code.Ldc_I4,
                        (byte)index[0],
                        (byte)index[1],
                        (byte)index[2],
                        (byte)index[3],
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
                        Code.Stfld,
                        5,
                        0,
                        0,
                        0,
                });

            // init Bounds
            codeList.AddRange(
                new object[]
                {
                        Code.Ldc_I4,
                        (byte)rank[0],
                        (byte)rank[1],
                        (byte)rank[2],
                        (byte)rank[3],
                        Code.Newarr,
                        6,
                        0,
                        0,
                        0,
                        Code.Stloc_1,
                });

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, type.ArrayRank))
            {
                var index = BitConverter.GetBytes((int)i);
                codeList.AddRange(
                    new object[]
                    {
                        Code.Ldloc_1,
                        Code.Ldc_I4,
                        (byte)index[0],
                        (byte)index[1],
                        (byte)index[2],
                        (byte)index[3],
                    });

                switch (i)
                {
                    case 0:
                        codeList.Add(Code.Ldarg_1);
                        break;
                    case 1:
                        codeList.Add(Code.Ldarg_2);
                        break;
                    case 2:
                        codeList.Add(Code.Ldarg_3);
                        break;
                    default:
                        var argIndex = i + 1;
                        codeList.AppendInt(Code.Ldarg, argIndex);
                        break;
                }

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
                        Code.Stfld,
                        7,
                        0,
                        0,
                        0,
                });

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
            tokenResolutions.Add(type.GetFieldByName("rank", typeResolver));
            tokenResolutions.Add(type.GetFieldByName("typeCode", typeResolver));
            tokenResolutions.Add(type.GetFieldByName("elementSize", typeResolver));
            // lowerBounds
            tokenResolutions.Add(typeResolver.ResolveType("System.Int32"));
            tokenResolutions.Add(type.GetFieldByName("lowerBounds", typeResolver));
            // bounds
            tokenResolutions.Add(typeResolver.ResolveType("System.Int32"));
            tokenResolutions.Add(type.GetFieldByName("lengths", typeResolver));

            // code
            code = codeList.ToArray();

            // parameters
            parameters = GetParameters(type, typeResolver);
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
            codeList.AppendInt(elementSize);

            // init each item in lowerBounds
            foreach (var i in Enumerable.Range(0, arrayType.ArrayRank))
            {
                switch (i)
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
                    default:
                        var argIndex = i;
                        codeList.AppendInt(Code.Ldarg, argIndex);
                        break;
                }

                codeList.Add(Code.Mul);
            }

            // add element size
            var multiArrayTypeSizeWithoutArrayData = arrayType.GetTypeSize(llvmWriter);
            codeList.AppendInt(multiArrayTypeSizeWithoutArrayData);
            codeList.Add(Code.Add);

            // calculate alignment
            codeList.Add(Code.Dup);

            var alignForType = Math.Max(LlvmWriter.PointerSize, !elementType.IsStructureType() ? elementSize : LlvmWriter.PointerSize);
            codeList.AppendInt(alignForType);
            codeList.Add(Code.Rem);

            codeList.Add(Code.Add);

            codeList.AppendInt(alignForType);
            codeList.Add(Code.Sub);

            // locals
            locals = new List<IType>();

            // tokens
            tokenResolutions = new List<object>();

            // parameters
            parameters = GetParameters(arrayType, llvmWriter);

            code = codeList.ToArray();
        }

        public static void AppendInt(this List<object> codeList, int value)
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

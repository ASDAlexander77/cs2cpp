// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArraySingleDimensionGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class ArraySingleDimensionGen
    {
        /// <summary>
        /// </summary>
        private static string _singleDimArrayPrefixDataType;

        /// <summary>
        /// </summary>
        private static string _singleDimArrayPrefixNullConstData;

        public static IEnumerable<IField> GetFields(IType arrayType, ITypeResolver typeResolver)
        {
            Debug.Assert(arrayType.IsArray && !arrayType.IsMultiArray, "This is for multi arrays only");

            var shortType = typeResolver.System.System_Int16;
            var intType = typeResolver.System.System_Int32;

            yield return shortType.ToField(arrayType, "rank");
            yield return shortType.ToField(arrayType, "typeCode");
            yield return intType.ToField(arrayType, "elementSize");
            yield return intType.ToField(arrayType, "length");
            yield return arrayType.GetElementType().ToField(arrayType, "data", isFixed: true);
        }

        public static IField GetDataField(IType arrayType, CWriter cWriter)
        {
            return arrayType.GetFieldByName("data", cWriter);
        }

        public static void SingleDimArrayAllocationSizeMethodBody(
            IlCodeBuilder codeList,
            ITypeResolver typeResolver,
            IType arrayType)
        {
            // add element size
            var elementType = arrayType.GetElementType();
            codeList.SizeOf(elementType);

            // load length
            codeList.LoadArgument(0);
            codeList.Add(Code.Mul);

            codeList.SizeOf(arrayType);
            codeList.Add(Code.Add);

            // calculate alignment
            codeList.Add(Code.Dup);

            if (!elementType.IsStructureType())
            {
                var alignForType = Math.Max(CWriter.PointerSize, elementType.GetKnownTypeSize());
                codeList.LoadConstant(alignForType - 1);
                codeList.Add(Code.Add);

                codeList.LoadConstant(~(alignForType - 1));
                codeList.Add(Code.And);
            }
            else
            {
                codeList.SizeOf(elementType);
                codeList.LoadConstant(1);
                codeList.Add(Code.Sub);
                codeList.Add(Code.Add);

                codeList.SizeOf(elementType);
                codeList.LoadConstant(1);
                codeList.Add(Code.Sub);
                codeList.Add(Code.Not);
                codeList.Add(Code.And);
            }

            // parameters
            codeList.Parameters.AddRange(ArrayMultiDimensionGen.GetParameters(arrayType, typeResolver));
        }

        public static void GetSingleDimensionArrayCtor(
            IType arrayType,
            ITypeResolver typeResolver,
            out IlCodeBuilder codeBuilder)
        {
            Debug.Assert(arrayType.IsArray && !arrayType.IsMultiArray, "This is for single dim arrays only");

            codeBuilder = new IlCodeBuilder();

            var arrayRank = arrayType.ArrayRank;
            var elementType = arrayType.GetElementType();
            var typeCode = elementType.GetTypeCode();

            var token1 = arrayType.GetFieldByName("rank", typeResolver);
            var token2 = arrayType.GetFieldByName("typeCode", typeResolver);
            var token3 = arrayType.GetFieldByName("elementSize", typeResolver);
            var token4 = arrayType.GetFieldByName("length", typeResolver);

            codeBuilder.LoadArgument(0);
            codeBuilder.Duplicate();
            codeBuilder.Duplicate();
            codeBuilder.Duplicate();

            codeBuilder.LoadConstant(arrayRank);
            codeBuilder.SaveField(token1);
            codeBuilder.LoadConstant(typeCode);
            codeBuilder.SaveField(token2);
            codeBuilder.SizeOf(elementType);
            codeBuilder.SaveField(token3);
            codeBuilder.LoadArgument(1);
            codeBuilder.SaveField(token4);

            // return
            codeBuilder.Return();

            // locals
            codeBuilder.Locals.Add(typeResolver.System.System_Int32.ToArrayType(1));
            codeBuilder.Locals.Add(typeResolver.System.System_Int32.ToArrayType(1));

            // parameters
            codeBuilder.Parameters.AddRange(ArrayMultiDimensionGen.GetParameters(arrayType, typeResolver));
        }

        /// <summary>
        /// resetting cahced values to force calling AddRequiredVirtualTablesDeclaration(stringType) and AddRequiredRttiDeclaration(stringType)
        /// </summary>
        public static void ResetClass()
        {
            _singleDimArrayPrefixDataType = null;
            _singleDimArrayPrefixNullConstData = null;
        }

        public static string GetSingleDimArrayPrefixDataType(ITypeResolver typeResolver)
        {
            if (_singleDimArrayPrefixDataType != null)
            {
                return _singleDimArrayPrefixDataType;
            }

            var sb = new StringBuilder();

            if (typeResolver.MultiThreadingSupport)
            {
                sb.Append("Byte* cond; ");
                sb.Append("Byte* lock; ");
            }

            sb.Append("Byte* vtable;");
            sb.Append("Int16 rank;");
            sb.Append("Int16 typeCode;");
            sb.Append("Int32 elementSize;");
            sb.Append("Int32 length");

            _singleDimArrayPrefixDataType = sb.ToString();
            return _singleDimArrayPrefixDataType;
        }

        public static string GetSingleDimArrayPrefixNullConstData(CWriter cWriter)
        {
            if (_singleDimArrayPrefixNullConstData != null)
            {
                return _singleDimArrayPrefixNullConstData;
            }

            ITypeResolver typeResolver = cWriter;

            var bytesArrayType = typeResolver.System.System_Byte.ToArrayType(1);

            var sb = new StringBuilder();

            if (typeResolver.MultiThreadingSupport)
            {
                sb.Append("(Byte*) -1, ");
                sb.Append("(Byte*) -1, ");
            }

            sb.AppendLine(string.Empty);
            sb.Append("(Byte*) ");
            sb.Append(bytesArrayType.GetVirtualTableNameReference(cWriter));
            sb.AppendLine(string.Empty);

            _singleDimArrayPrefixNullConstData = sb.ToString();
            return _singleDimArrayPrefixNullConstData;
        }

        public static string GetArrayTypeHeader(this CWriter cWriter, IType elementType, int length)
        {
            var typeString = cWriter.WriteToString(() => elementType.WriteTypePrefix(cWriter));

            return "{ " + GetSingleDimArrayPrefixDataType(cWriter) + "; " + typeString + " data[" + length + "]; }";
        }

        public static string GetArrayValuesHeader(
            this CWriter cWriter,
            IType elementType,
            int length,
            int storeLength)
        {
            var typeName = cWriter.WriteToString(() => elementType.WriteTypePrefix(cWriter));
            return GetSingleDimArrayPrefixNullConstData(cWriter) + ", 0, " + elementType.GetTypeCode() + ", sizeof(" + typeName + "), " + storeLength + ", ";
        }
    }
}
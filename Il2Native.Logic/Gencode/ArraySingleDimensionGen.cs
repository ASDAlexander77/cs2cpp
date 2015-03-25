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

        public static IlCodeBuilder SingleDimArrayAllocationSizeMethodBody(
            ITypeResolver typeResolver,
            IType arrayType)
        {
            var codeList = new IlCodeBuilder();

            // add element size
            var elementType = arrayType.GetElementType();
            var elementSize = elementType.GetTypeSize(typeResolver, true);
            codeList.SizeOf(elementType);

            // load length
            codeList.LoadArgument(0);
            codeList.Add(Code.Mul);

            codeList.SizeOf(arrayType);
            codeList.Add(Code.Add);

            // calculate alignment
            codeList.Add(Code.Dup);

            var alignForType = Math.Max(CWriter.PointerSize, !elementType.IsStructureType() ? elementSize : CWriter.PointerSize);
            codeList.LoadConstant(alignForType - 1);
            codeList.Add(Code.Add);

            codeList.LoadConstant(~(alignForType - 1));
            codeList.Add(Code.And);

            // parameters
            codeList.Parameters.AddRange(ArrayMultiDimensionGen.GetParameters(arrayType, typeResolver));

            return codeList;
        }

        public static void GetSingleDimensionArrayCtor(
            IType arrayType,
            ITypeResolver typeResolver,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            Debug.Assert(arrayType.IsArray && !arrayType.IsMultiArray, "This is for single dim arrays only");

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
                        Code.Dup,
                    });

            codeList.AppendLoadInt(arrayRank);
            codeList.AppendInt(Code.Stfld, 1);
            codeList.AppendLoadInt(typeCode);
            codeList.AppendInt(Code.Stfld, 2);
            codeList.AppendLoadInt(elementSize);
            codeList.AppendInt(Code.Stfld, 3);
            codeList.AppendLoadArgument(1);
            codeList.AppendInt(Code.Stfld, 4);

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
            tokenResolutions.Add(arrayType.GetFieldByName("length", typeResolver));

            // code
            code = codeList.ToArray();

            // parameters
            parameters = ArrayMultiDimensionGen.GetParameters(arrayType, typeResolver);
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

            var bytesArrayType = typeResolver.System.System_Byte.ToArrayType(1);

            var sb = new StringBuilder();

            sb.Append("Byte* vtable");

            var index = 0;
            foreach (var @interface in bytesArrayType.SelectAllTopAndAllNotFirstChildrenInterfaces().Distinct())
            {
                if (sb.Length > 0)
                {
                    sb.Append("; ");
                }

                sb.Append("Byte* ifce" + index++);
            }

            sb.Append("; Int16 rank");
            sb.Append("; Int16 typeCode");
            sb.Append("; Int32 elementSize");
            sb.Append("; Int32 length");

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

            sb.AppendLine(string.Empty);
            sb.Append("(Byte*) ");
            sb.Append(bytesArrayType.GetVirtualTableNameReference(cWriter));

            foreach (var @interface in bytesArrayType.SelectAllTopAndAllNotFirstChildrenInterfaces().Distinct())
            {
                if (sb.Length > 0)
                {
                    sb.AppendLine(", ");
                }

                sb.Append("(Byte*) ");
                sb.Append(bytesArrayType.GetVirtualInterfaceTableNameReference(@interface, cWriter));
            }

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
            return GetSingleDimArrayPrefixNullConstData(cWriter) + ", 0, " + elementType.GetTypeCode() + ", " + elementType.GetTypeSize(cWriter, true) + ", " + storeLength + ", ";
        }
    }
}
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

        /// <summary>
        /// </summary>
        /// <param name="methodBase">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsItArrayInitialization(this IMethod methodBase)
        {
            if (methodBase.Name == "InitializeArray" && methodBase.Namespace == "System.Runtime.CompilerServices")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteArrayGetLength(this CWriter cWriter, OpCodePart opCode)
        {
            var writer = cWriter.Output;
            cWriter.LoadElement(writer, opCode, "length");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteArrayInit(this CWriter cWriter, OpCodePart opCode)
        {
            var writer = cWriter.Output;

            // TODO: finish it

            //writer.WriteLine("// Init array with values");

            //var opCodeFieldInfoPart = opCode.OpCodeOperands[1] as OpCodeFieldInfoPart;
            //Debug.Assert(opCodeFieldInfoPart != null, "opCode is not OpCodeFieldInfoPart");
            //if (opCodeFieldInfoPart == null)
            //{
            //    return;
            //}

            //var staticArrayInitTypeSizeLabel = "__StaticArrayInitTypeSize=";
            //var hasSize = opCodeFieldInfoPart.Operand.FieldType.MetadataName.Contains(staticArrayInitTypeSizeLabel);

            //var data = opCodeFieldInfoPart.Operand.GetFieldRVAData();
            //var arrayLength = hasSize
            //    ? int.Parse(
            //        opCodeFieldInfoPart.Operand.FieldType.MetadataName.Substring(staticArrayInitTypeSizeLabel.Length))
            //    : opCodeFieldInfoPart.Operand.FieldType.GetTypeSize(cWriter, true);

            //arrayLength = arrayLength.Align(CWriter.PointerSize);

            //var subData = new byte[arrayLength];
            //Array.Copy(data, subData, Math.Min(data.Length, arrayLength));

            //var bytesIndex = cWriter.GetBytesIndex(subData);
            //var byteType = cWriter.System.System_Byte;
            //var arrayData = cWriter.GetArrayTypeReference(
            //    string.Concat("@.bytes", bytesIndex),
            //    byteType,
            //    arrayLength);

            //var storedResult = opCode.OpCodeOperands[0].Result;

            //var opCodeConvert = OpCodePart.CreateNop;

            //cWriter.WriteFieldAccess(writer, opCode, GetDataField(storedResult.Type, cWriter));
            //writer.WriteLine(string.Empty);

            //var firstElementResult = opCode.Result;

            //cWriter.WriteBitcast(opCodeConvert, firstElementResult);
            //var firstBytes = opCodeConvert.Result;
            //writer.WriteLine(string.Empty);

            //// second array to i8*
            //var byteArrayType = byteType.ToArrayType(1);

            //var opCodeDataHolder = OpCodePart.CreateNop;
            //opCodeDataHolder.OpCodeOperands = new[] { OpCodePart.CreateNop };
            //opCodeDataHolder.OpCodeOperands[0].Result = new FullyDefinedReference(
            //    arrayData,
            //    byteArrayType);
            //var secondFirstElementResult = GetArrayDataAddressHelper(
            //    cWriter,
            //    opCodeDataHolder,
            //    byteType,
            //    GetDataField(byteArrayType, cWriter) + cWriter.CalculateFirstFieldPositionInType(byteArrayType),
            //    0);

            //cWriter.WriteBitcast(opCodeConvert, secondFirstElementResult);
            //var secondBytes = opCodeConvert.Result;
            //writer.WriteLine(string.Empty);

            //writer.WriteLine(
            //    "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)",
            //    firstBytes,
            //    secondBytes,
            //    arrayLength,
            //    CWriter.PointerSize /*Align*/);

            //opCode.OpCodeOperands[0].Result = storedResult;

            writer.WriteLine(string.Empty);
        }
    }
}
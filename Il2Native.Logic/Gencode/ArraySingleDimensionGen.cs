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
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using CodeParts;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class ArraySingleDimensionGen
    {
        /// <summary>
        /// </summary>
        public const int ArrayDataElementRank = 0;

        /// <summary>
        /// </summary>
        public const int ArrayDataElementTypeCode = 1;

        /// <summary>
        /// </summary>
        public const int ArrayDataElementSize = 2;

        /// <summary>
        /// </summary>
        public const int ArrayDataLength = 3;

        /// <summary>
        /// count of array header data fields
        /// </summary>
        public static int ArraySupportFields = 4;

        /// <summary>
        /// shift for first field after System.Array
        /// </summary>
        public static int _arrayHeaderDataStartsWith = -1;

        /// <summary>
        /// shift for first element of an array
        /// </summary>
        public static int _arrayDataStartsWith = -1;

        /// <summary>
        /// </summary>
        private static string _singleDimArrayPrefixDataType;

        /// <summary>
        /// </summary>
        private static string _singleDimArrayPrefixNullConstData;

        public static int GetArrayDataStartsWith(ITypeResolver typeResolver)
        {
            if (_arrayDataStartsWith != -1)
            {
                return _arrayDataStartsWith;
            }

            var count = GetArrayHeaderDataStartsWith(typeResolver);
            _arrayDataStartsWith = count + ArraySupportFields;
            return _arrayDataStartsWith;
        }

        public static int GetArrayHeaderDataStartsWith(ITypeResolver typeResolver)
        {
            if (_arrayHeaderDataStartsWith != -1)
            {
                return _arrayHeaderDataStartsWith;
            }

            var arraySystemType = typeResolver.ResolveType("System.Array");
            _arrayHeaderDataStartsWith = arraySystemType.GetTypeSizes(typeResolver).Count();
            return _arrayHeaderDataStartsWith;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static string GetSingleDimArrayPrefixDataType(ITypeResolver typeResolver)
        {
            //return "i8*, i8*, i8*, i32, i32";
            if (_singleDimArrayPrefixDataType != null)
            {
                return _singleDimArrayPrefixDataType;
            }

            var arraySystemType = typeResolver.ResolveType("System.Array");

            var sb = new StringBuilder();
            foreach (var memberLocationInfo in arraySystemType.GetTypeSizes(typeResolver))
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                if (memberLocationInfo.MemberType == MemberTypes.Root || memberLocationInfo.MemberType == MemberTypes.Interface)
                {
                    sb.Append("i8*");
                }
            }

            sb.Append(", i16, i16, i32, i32");

            _singleDimArrayPrefixDataType = sb.ToString();
            return _singleDimArrayPrefixDataType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public static string GetSingleDimArrayPrefixNullConstData(ITypeResolver typeResolver)
        {
            if (_singleDimArrayPrefixNullConstData != null)
            {
                return _singleDimArrayPrefixNullConstData;
            }

            var arraySystemType = typeResolver.ResolveType("System.Array");

            var sb = new StringBuilder();
            foreach (var memberLocationInfo in arraySystemType.GetTypeSizes(typeResolver))
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                if (memberLocationInfo.MemberType == MemberTypes.Root || memberLocationInfo.MemberType == MemberTypes.Interface)
                {
                    sb.Append("i8* null");
                }
            }

            _singleDimArrayPrefixNullConstData = sb.ToString();
            return _singleDimArrayPrefixNullConstData;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetArrayTypeHeader(this LlvmWriter llvmWriter, IType elementType, int length)
        {
            var typeString = llvmWriter.WriteToString(
                () =>
                {
                    var writer = llvmWriter.Output;
                    elementType.WriteTypePrefix(llvmWriter);
                });

            return "{ " + GetSingleDimArrayPrefixDataType(llvmWriter) + ", [" + length + " x " + typeString + "] }";
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="name">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetArrayTypeReference(
            this LlvmWriter llvmWriter,
            string name,
            IType elementType,
            int length)
        {
            var convertString = llvmWriter.WriteToString(
                () =>
                {
                    var writer = llvmWriter.Output;

                    var array = elementType.ToArrayType(1);
                    writer.Write("bitcast (");
                    writer.Write("{1}* {0} to ", name, llvmWriter.GetArrayTypeHeader(elementType, length));
                    array.WriteTypePrefix(llvmWriter);
                    writer.Write(")");
                });

            return convertString;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <param name="storeLength">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetArrayValuesHeader(
            this LlvmWriter llvmWriter,
            IType elementType,
            int length,
            int storeLength)
        {
            var typeString = llvmWriter.WriteToString(
                () =>
                {
                    var writer = llvmWriter.Output;
                    elementType.WriteTypePrefix(llvmWriter);
                });

            return GetSingleDimArrayPrefixNullConstData(llvmWriter) + ", i16 0, i16 " + elementType.GetTypeCode() +
                   ", i32 " + elementType.GetTypeSize(llvmWriter, true) + ", i32 " + storeLength + ", [" +
                   length + " x " + typeString + "]";
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

        public static void WriteArrayGetElementSize(this LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var intType = llvmWriter.ResolveType("System.Int32");

            var arrayDataHeaderShift = GetArrayHeaderDataStartsWith(llvmWriter);
            var elementSizeResult = GetArrayDataAddressHelper(llvmWriter, opCode, intType, arrayDataHeaderShift + ArrayDataElementSize);

            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, intType, elementSizeResult);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteArrayGetLength(this LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var intType = llvmWriter.ResolveType("System.Int32");

            var arrayDataHeaderShift = GetArrayHeaderDataStartsWith(llvmWriter);
            var lengthResult = GetArrayDataAddressHelper(llvmWriter, opCode, intType, arrayDataHeaderShift + ArrayDataLength);

            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, intType, lengthResult);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteArrayInit(this LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; Init array with values");

            var opCodeFieldInfoPart = opCode.OpCodeOperands[1] as OpCodeFieldInfoPart;
            Debug.Assert(opCodeFieldInfoPart != null, "opCode is not OpCodeFieldInfoPart");
            if (opCodeFieldInfoPart == null)
            {
                return;
            }

            var staticArrayInitTypeSizeLabel = "__StaticArrayInitTypeSize=";
            var hasSize = opCodeFieldInfoPart.Operand.FieldType.MetadataName.Contains(staticArrayInitTypeSizeLabel);

            var data = opCodeFieldInfoPart.Operand.GetFieldRVAData();
            var arrayLength = hasSize
                ? int.Parse(
                    opCodeFieldInfoPart.Operand.FieldType.MetadataName.Substring(staticArrayInitTypeSizeLabel.Length))
                : opCodeFieldInfoPart.Operand.FieldType.GetTypeSize(llvmWriter, true);

            arrayLength = arrayLength.Align(LlvmWriter.PointerSize);

            var subData = new byte[arrayLength];
            Array.Copy(data, subData, Math.Min(data.Length, arrayLength));

            var bytesIndex = llvmWriter.GetBytesIndex(subData);
            var byteType = llvmWriter.ResolveType("System.Byte");
            var arrayData = llvmWriter.GetArrayTypeReference(
                string.Concat("@.bytes", bytesIndex),
                byteType,
                arrayLength);

            var storedResult = opCode.OpCodeOperands[0].Result;

            var opCodeConvert = OpCodePart.CreateNop;

            if (!storedResult.Type.IsMultiArray)
            {
                // first array to i8*
                var firstElementResult = GetArrayDataAddressHelper(
                    llvmWriter,
                    opCode,
                    storedResult.Type.GetElementType(),
                    GetArrayDataStartsWith(llvmWriter),
                    0);
                llvmWriter.WriteBitcast(opCodeConvert, firstElementResult);
                var firstBytes = opCodeConvert.Result;
                writer.WriteLine(string.Empty);

                // second array to i8*
                var opCodeDataHolder = OpCodePart.CreateNop;
                opCodeDataHolder.OpCodeOperands = new[] { OpCodePart.CreateNop };
                opCodeDataHolder.OpCodeOperands[0].Result = new FullyDefinedReference(
                    arrayData,
                    byteType.ToArrayType(1));
                var secondFirstElementResult = GetArrayDataAddressHelper(
                    llvmWriter,
                    opCodeDataHolder,
                    byteType,
                    GetArrayDataStartsWith(llvmWriter),
                    0);
                llvmWriter.WriteBitcast(opCodeConvert, secondFirstElementResult);
                var secondBytes = opCodeConvert.Result;
                writer.WriteLine(string.Empty);

                writer.WriteLine(
                    "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)",
                    firstBytes,
                    secondBytes,
                    arrayLength,
                    LlvmWriter.PointerSize /*Align*/);
            }
            else
            {
                // TODO: multiarray
                writer.WriteLine(
                    "; MultiArray init.  Call <ARRAY>::Address(int, int) to get an address of the first element");

                Debug.Assert(false, "to be finished");
            }

            opCode.OpCodeOperands[0].Result = storedResult;

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="elementType">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteNewArrayMethod(this LlvmWriter llvmWriter, IType elementType)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedNewArrayMethod(elementType, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; New Array method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null, true);

            // load first parameter
            var arraySizeType = llvmWriter.ResolveType("System.Int32");
            var destinationName = llvmWriter.GetArgVarName("value", 0);
            var fullyDefinedReference = new FullyDefinedReference(destinationName, arraySizeType);
            llvmWriter.WriteLlvmLoad(opCode, arraySizeType, fullyDefinedReference);

            var opCodeOperand1 = OpCodePart.CreateNop;
            opCodeOperand1.Result = opCode.Result;
            opCode.OpCodeOperands = new [] { opCodeOperand1 };

            llvmWriter.WriteNewArrayMethodBody(opCode, elementType, opCode.Result);
            writer.WriteLine(string.Empty);
            writer.Write("ret ");
            elementType.ToArrayType(1).WriteTypePrefix(llvmWriter);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);
            writer.WriteLine(string.Empty);
            llvmWriter.WriteMethodEnd(method, null);
        }

        public static void WriteCallNewArrayMethod(this LlvmWriter llvmWriter, OpCodePart opCode, IType elementType, OpCodePart length)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedNewArrayMethod(elementType, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call New Array method");
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            opCodeNope.OpCodeOperands = new [] { length };
            llvmWriter.WriteCall(
                opCodeNope,
                method,
                false,
                false,
                false,
                opCode.Result,
                llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCode.Result = opCodeNope.Result;
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
        public static void WriteNewArrayMethodBody(
            this LlvmWriter llvmWriter,
            OpCodePart opCode,
            IType elementType,
            FullyDefinedReference lengthResult)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; New array");

            var arraySystemType = llvmWriter.ResolveType("System.Array");
            var intType = llvmWriter.ResolveType("System.Int32");
            var shortType = llvmWriter.ResolveType("System.Int16");

            var sizeOfElement = elementType.GetTypeSize(llvmWriter, true);
            llvmWriter.UnaryOper(writer, opCode, "mul", intType, options: LlvmWriter.OperandOptions.AdjustIntTypes | LlvmWriter.OperandOptions.GenerateResult);
            writer.WriteLine(", {0}", sizeOfElement);

            var resMul = opCode.Result;

            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write(
                "add i32 {1}, {0}",
                resMul,
                arraySystemType.GetTypeSize(llvmWriter) + GetSingleDimArraySupportedFieldsSize(llvmWriter, intType, shortType)); // add header size
            writer.WriteLine(string.Empty);

            var resAdd = opCode.Result;

            var alignForType = Math.Max(
                LlvmWriter.PointerSize,
                !elementType.IsStructureType() ? sizeOfElement : LlvmWriter.PointerSize);

            // align size
            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("srem i32 {0}, {1}", resAdd, alignForType); // add header size
            writer.WriteLine(string.Empty);

            var resSRem = opCode.Result;

            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("sub i32 {1}, {0}", resSRem, alignForType); // add header size
            writer.WriteLine(string.Empty);

            var resSub = opCode.Result;

            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("add i32 {1}, {0}", resAdd, resSub); // add header size
            writer.WriteLine(string.Empty);

            var resSize = opCode.Result;

            // new alloc
            var bytePointerType = llvmWriter.ResolveType("System.Byte").ToPointerType();
            var resAlloc = llvmWriter.WriteSetResultNumber(
                opCode,
                bytePointerType);
            writer.Write("call i8* @{1}(i32 {0})", resSize, llvmWriter.GetAllocator());

            writer.WriteLine(string.Empty);
            llvmWriter.WriteTestNullValueAndThrowException(
                writer,
                opCode,
                resAlloc,
                "System.OutOfMemoryException",
                "new_arr");

            writer.WriteLine(string.Empty);

            if (!llvmWriter.Gc)
            {
                llvmWriter.WriteMemSet(resAlloc, resSize, LlvmWriter.PointerSize /*Align*/);
            }

            var opCodeTemp = OpCodePart.CreateNop;
            opCodeTemp.OpCodeOperands = opCode.OpCodeOperands;

            // init System.Array
            llvmWriter.WriteBitcast(opCode, resAlloc, arraySystemType);
            arraySystemType.WriteCallInitObjectMethod(llvmWriter, opCode);
            writer.WriteLine(string.Empty);

            var arrayType = elementType.ToArrayType(1);
            llvmWriter.WriteBitcast(opCode, resAlloc, arrayType);
            writer.WriteLine(string.Empty);

            var arrayInstanceResult = opCode.Result;

            var elementTypeCode = elementType.GetTypeCode();

            var arrayDataHeaderShift = GetArrayHeaderDataStartsWith(llvmWriter);
            // save element typecode
            llvmWriter.WriteSetResultNumber(opCode, shortType);
            writer.Write("getelementptr inbounds ");
            arrayInstanceResult.Type.WriteTypePrefix(llvmWriter);
            writer.Write(" ");
            llvmWriter.WriteResult(arrayInstanceResult);
            writer.Write(", i32 0, i32 {0}", arrayDataHeaderShift + ArrayDataElementTypeCode);
            writer.WriteLine(string.Empty);

            writer.Write("store ");
            opCode.Result.Type.WriteTypePrefix(llvmWriter);
            writer.Write(" {0}, ", elementTypeCode);
            opCode.Result.Type.WriteTypePrefix(llvmWriter, true);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);
            writer.WriteLine(string.Empty);

            // save element size
            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("getelementptr inbounds ");
            arrayInstanceResult.Type.WriteTypePrefix(llvmWriter);
            writer.Write(" ");
            llvmWriter.WriteResult(arrayInstanceResult);
            writer.Write(", i32 0, i32 {0}", arrayDataHeaderShift + ArrayDataElementSize);
            writer.WriteLine(string.Empty);

            writer.Write("store ");
            opCode.Result.Type.WriteTypePrefix(llvmWriter);
            writer.Write(" {0}, ", sizeOfElement);
            opCode.Result.Type.WriteTypePrefix(llvmWriter, true);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);
            writer.WriteLine(string.Empty);

            // save array size
            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("getelementptr inbounds ");
            arrayInstanceResult.Type.WriteTypePrefix(llvmWriter);
            writer.Write(" ");
            llvmWriter.WriteResult(arrayInstanceResult);
            writer.Write(", i32 0, i32 {0}", arrayDataHeaderShift + ArrayDataLength);
            writer.WriteLine(string.Empty);

            writer.Write("store ");
            lengthResult.Type.WriteTypePrefix(llvmWriter);
            writer.Write(" ");
            llvmWriter.WriteResult(lengthResult);
            writer.Write(", ");
            opCode.Result.Type.WriteTypePrefix(llvmWriter, true);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);
            writer.WriteLine(string.Empty);

            writer.WriteLine("; end of new array");

            opCode.Result = arrayInstanceResult;
        }

        private static int GetSingleDimArraySupportedFieldsSize(LlvmWriter llvmWriter, IType intType, IType shortType)
        {
            return (ArraySupportFields - 2) * intType.GetTypeSize(llvmWriter, true) + 2 * shortType.GetTypeSize(llvmWriter, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="dataType">
        /// </param>
        /// <param name="dataIndex">
        /// </param>
        /// <param name="secondIndex">
        /// </param>
        /// <returns>
        /// </returns>
        private static IncrementalResult GetArrayDataAddressHelper(
            LlvmWriter llvmWriter,
            OpCodePart opCode,
            IType dataType,
            int dataIndex,
            int secondIndex = -1)
        {
            var writer = llvmWriter.Output;

            var arrayInstanceResult = opCode.OpCodeOperands[0].Result;
            if (!arrayInstanceResult.Type.IsArray)
            {
                // this is Array instance
                var opCodeNope = OpCodePart.CreateNop;
                llvmWriter.WriteBitcast(
                    opCodeNope,
                    arrayInstanceResult,
                    llvmWriter.ResolveType("System.Byte").ToArrayType(1));
                arrayInstanceResult = opCodeNope.Result;

                writer.WriteLine(string.Empty);
            }

            var result = llvmWriter.WriteSetResultNumber(opCode, dataType);
            writer.Write("getelementptr ");
            arrayInstanceResult.Type.WriteTypePrefix(llvmWriter, true);

            writer.Write(" ");
            llvmWriter.WriteResult(arrayInstanceResult);
            writer.Write(", i32 0, i32 {0}", dataIndex);
            if (secondIndex != -1)
            {
                writer.Write(", i32 {0}", secondIndex);
            }

            writer.WriteLine(string.Empty);
            return result;
        }
    }
}
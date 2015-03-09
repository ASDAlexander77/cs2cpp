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

    using Il2Native.Logic.Gencode.SynthesizedMethods;

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

        /*
        public ArraySingleDimensionGen_Ctor()
        {
            this.rank = 0; //?
            this.typeCode = type.GetElementType().TypeCode;
            this.elementSize = type.GetSize();
            this.length = dim1;
        }
         */

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

        public static int GetDataFieldIndex(IType arrayType, CWriter cWriter)
        {
            return cWriter.GetFieldIndex(arrayType, "data");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        public static FullyDefinedReference WriteSingleDimArrayAllocationSize(
            this CWriter cWriter,
            OpCodePart opCode,
            IType arrayType)
        {
            Debug.Assert(arrayType.IsArray && !arrayType.IsMultiArray, "This is for single dim arrays only");

            var writer = cWriter.Output;

            writer.WriteLine("; Calculate SingleDim allocation size");

            object[] code;
            IList<object> tokenResolutions;
            IList<IType> locals;
            IList<IParameter> parameters;
            GetCalculationPartOfSingleDimArrayAllocationSizeMethodBody(
                cWriter,
                arrayType,
                out code,
                out tokenResolutions,
                out locals,
                out parameters);

            var constructedMethod = MethodBodyBank.GetMethodDecorator(null, code, tokenResolutions, locals, parameters);

            // actual write
            var opCodes = cWriter.WriteCustomMethodPart(constructedMethod, null);
            return opCodes.Last().Result;
        }

        private static void GetCalculationPartOfSingleDimArrayAllocationSizeMethodBody(
            ITypeResolver typeResolver,
            IType arrayType,
            out object[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            var codeList = new List<object>();

            // add element size
            var elementType = arrayType.GetElementType();
            var elementSize = elementType.GetTypeSize(typeResolver, true);
            codeList.AppendLoadInt(elementSize);

            // load length
            codeList.AppendLoadArgument(0);
            codeList.Add(Code.Mul);

            var arrayTypeSizeWithoutArrayData = arrayType.GetTypeSize(typeResolver);
            codeList.AppendLoadInt(arrayTypeSizeWithoutArrayData);
            codeList.Add(Code.Add);

            // calculate alignment
            codeList.Add(Code.Dup);

            var alignForType = Math.Max(CWriter.PointerSize, !elementType.IsStructureType() ? elementSize : CWriter.PointerSize);
            codeList.AppendLoadInt(alignForType - 1);
            codeList.Add(Code.Add);

            codeList.AppendLoadInt(~(alignForType - 1));
            codeList.Add(Code.And);

            // locals
            locals = new List<IType>();

            // tokens
            tokenResolutions = new List<object>();

            // parameters
            parameters = ArrayMultiDimensionGen.GetParameters(arrayType, typeResolver);

            code = codeList.ToArray();
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
        /// </summary>
        /// <returns>
        /// </returns>
        [Obsolete]
        public static string GetSingleDimArrayPrefixDataType(ITypeResolver typeResolver)
        {
            // TODO: fix it as you did in StringGen with using tringSystemType.SelectAllTopAndAllNotFirstChildrenInterfaces().Distinct()
            //return "i8*, i8*, i8*, i32, i32";
            if (_singleDimArrayPrefixDataType != null)
            {
                return _singleDimArrayPrefixDataType;
            }

            var arraySystemType = typeResolver.System.System_Byte.ToArrayType(1);

            var sb = new StringBuilder();
            foreach (var memberLocationInfo in arraySystemType.GetTypeSizes(typeResolver))
            {
                if (memberLocationInfo.Size == 0)
                {
                    break;
                }

                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                if (memberLocationInfo.MemberType == MemberTypes.Root || memberLocationInfo.MemberType == MemberTypes.Interface)
                {
                    sb.Append("i8*");
                }
                else
                {
                    if (memberLocationInfo.Size == 0)
                    {
                        break;
                    }

                    sb.Append("i" + (memberLocationInfo.Size * 8));
                }
            }

            _singleDimArrayPrefixDataType = sb.ToString();
            return _singleDimArrayPrefixDataType;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        [Obsolete]
        public static string GetSingleDimArrayPrefixNullConstData(ITypeResolver typeResolver)
        {
            // TODO: fix it as you did in StringGen with using tringSystemType.SelectAllTopAndAllNotFirstChildrenInterfaces().Distinct()

            if (_singleDimArrayPrefixNullConstData != null)
            {
                return _singleDimArrayPrefixNullConstData;
            }

            var arraySystemType = typeResolver.System.System_Byte.ToArrayType(1);

            var sb = new StringBuilder();
            foreach (var memberLocationInfo in arraySystemType.GetTypeSizes(typeResolver))
            {
                if (memberLocationInfo.MemberType == MemberTypes.Root || memberLocationInfo.MemberType == MemberTypes.Interface)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }

                    sb.Append("i8* null");
                }
                else
                {
                    break;
                }
            }

            _singleDimArrayPrefixNullConstData = sb.ToString();
            return _singleDimArrayPrefixNullConstData;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="elementType">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetArrayTypeHeader(this CWriter cWriter, IType elementType, int length)
        {
            var typeString = cWriter.WriteToString(
                () =>
                {
                    elementType.WriteTypePrefix(cWriter);
                });

            return "{ " + GetSingleDimArrayPrefixDataType(cWriter) + ", [" + length + " x " + typeString + "] }";
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
            this CWriter cWriter,
            string name,
            IType elementType,
            int length)
        {
            var convertString = cWriter.WriteToString(
                () =>
                {
                    var writer = cWriter.Output;

                    var array = elementType.ToArrayType(1);
                    writer.Write("bitcast (");
                    writer.Write("{1}* {0} to ", name, cWriter.GetArrayTypeHeader(elementType, length));
                    array.WriteTypePrefix(cWriter);
                    writer.Write(")");
                });

            return convertString;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
            this CWriter cWriter,
            IType elementType,
            int length,
            int storeLength)
        {
            var typeString = cWriter.WriteToString(
                () =>
                {
                    var writer = cWriter.Output;
                    elementType.WriteTypePrefix(cWriter);
                });

            return GetSingleDimArrayPrefixNullConstData(cWriter) + ", i16 0, i16 " + elementType.GetTypeCode() +
                   ", i32 " + elementType.GetTypeSize(cWriter, true) + ", i32 " + storeLength + ", [" +
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

            writer.WriteLine("// Init array with values");

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
                : opCodeFieldInfoPart.Operand.FieldType.GetTypeSize(cWriter, true);

            arrayLength = arrayLength.Align(CWriter.PointerSize);

            var subData = new byte[arrayLength];
            Array.Copy(data, subData, Math.Min(data.Length, arrayLength));

            var bytesIndex = cWriter.GetBytesIndex(subData);
            var byteType = cWriter.System.System_Byte;
            var arrayData = cWriter.GetArrayTypeReference(
                string.Concat("@.bytes", bytesIndex),
                byteType,
                arrayLength);

            var storedResult = opCode.OpCodeOperands[0].Result;

            var opCodeConvert = OpCodePart.CreateNop;

            cWriter.WriteFieldAccess(writer, opCode, GetDataFieldIndex(storedResult.Type, cWriter));
            writer.WriteLine(string.Empty);

            var firstElementResult = opCode.Result;

            cWriter.WriteBitcast(opCodeConvert, firstElementResult);
            var firstBytes = opCodeConvert.Result;
            writer.WriteLine(string.Empty);

            // second array to i8*
            var byteArrayType = byteType.ToArrayType(1);

            var opCodeDataHolder = OpCodePart.CreateNop;
            opCodeDataHolder.OpCodeOperands = new[] { OpCodePart.CreateNop };
            opCodeDataHolder.OpCodeOperands[0].Result = new FullyDefinedReference(
                arrayData,
                byteArrayType);
            var secondFirstElementResult = GetArrayDataAddressHelper(
                cWriter,
                opCodeDataHolder,
                byteType,
                GetDataFieldIndex(byteArrayType, cWriter) + cWriter.CalculateFirstFieldPositionInType(byteArrayType),
                0);

            cWriter.WriteBitcast(opCodeConvert, secondFirstElementResult);
            var secondBytes = opCodeConvert.Result;
            writer.WriteLine(string.Empty);

            writer.WriteLine(
                "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)",
                firstBytes,
                secondBytes,
                arrayLength,
                CWriter.PointerSize /*Align*/);

            opCode.OpCodeOperands[0].Result = storedResult;

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
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
            CWriter cWriter,
            OpCodePart opCode,
            IType dataType,
            int dataIndex,
            int secondIndex = -1)
        {
            // TODO: is obsolete
            var writer = cWriter.Output;

            var arrayInstanceResult = opCode.OpCodeOperands[0].Result;
            if (!arrayInstanceResult.Type.IsArray)
            {
                // this is Array instance
                var opCodeNope = OpCodePart.CreateNop;
                cWriter.WriteCCast(
                    opCodeNope,
                    arrayInstanceResult,
                    cWriter.System.System_Byte.ToArrayType(1));
                arrayInstanceResult = opCodeNope.Result;

                writer.WriteLine(string.Empty);
            }

            var result = cWriter.SetResultNumber(opCode, dataType);
            writer.Write("getelementptr ");
            arrayInstanceResult.Type.WriteTypePrefix(cWriter, true);
            writer.Write(" ");

            cWriter.WriteResult(arrayInstanceResult);
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
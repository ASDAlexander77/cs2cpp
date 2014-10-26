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
    using System.Diagnostics;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class ArraySingleDimensionGen
    {
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
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteArrayGetLength(this LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            // TODO: should be removed in the future when Skip field is not used
            if (opCode.OpCodeOperands[0].Result == null)
            {
                llvmWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
            }

            var typeToLoad = llvmWriter.ResolveType("System.Int32");
            llvmWriter.WriteBitcast(opCode, opCode.OpCodeOperands[0].Result, typeToLoad);
            writer.WriteLine(string.Empty);

            var res = opCode.Result;
            var resLen = llvmWriter.WriteSetResultNumber(opCode, typeToLoad);
            writer.Write("getelementptr ");
            typeToLoad.WriteTypePrefix(writer);
            writer.Write("* ");
            llvmWriter.WriteResult(res);
            writer.WriteLine(", i32 -1");

            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, typeToLoad, resLen);
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

            var data = opCodeFieldInfoPart.Operand.GetFieldRVAData();

            var storedResult = opCode.OpCodeOperands[0].Result;
            if (storedResult.Type.HasElementType && storedResult.Type.GetElementType().TypeNotEquals(llvmWriter.ResolveType("System.Byte")))
            {
                llvmWriter.WriteBitcast(opCode.OpCodeOperands[0], opCode.OpCodeOperands[0].Result);
                writer.WriteLine(string.Empty);
            }

            var staticArrayInitTypeSizeLabel = "__StaticArrayInitTypeSize=";
            if (!opCodeFieldInfoPart.Operand.FieldType.MetadataName.Contains(staticArrayInitTypeSizeLabel))
            {
                return;
            }

            var bytesIndex = llvmWriter.GetBytesIndex(data);
            var arrayLength = int.Parse(opCodeFieldInfoPart.Operand.FieldType.MetadataName.Substring(staticArrayInitTypeSizeLabel.Length));
            var arrayData = string.Format(
                "bitcast ([{1} x i8]* getelementptr inbounds ({2} i32, [{1} x i8] {3}* @.bytes{0}, i32 0, i32 1) to i8*)", bytesIndex, data.Length, '{', '}');

            writer.WriteLine(
                "call void @llvm.memcpy.p0i8.p0i8.i32(i8* {0}, i8* {1}, i32 {2}, i32 {3}, i1 false)",
                opCode.OpCodeOperands[0].Result,
                arrayData,
                arrayLength,
                LlvmWriter.PointerSize /*Align*/);

            opCode.OpCodeOperands[0].Result = storedResult;

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <param name="length">
        /// </param>
        public static void WriteNewArray(this LlvmWriter llvmWriter, OpCodePart opCode, IType declaringType, OpCodePart length)
        {
            if (opCode.HasResult)
            {
                return;
            }

            var writer = llvmWriter.Output;

            writer.WriteLine("; New array");

            var sizeOfElement = declaringType.GetTypeSize(true);
            llvmWriter.UnaryOper(writer, opCode, "mul");
            writer.WriteLine(", {0}", sizeOfElement);

            var resMul = opCode.Result;

            var intType = llvmWriter.ResolveType("System.Int32");
            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("add i32 {1}, {0}", resMul, llvmWriter.ResolveType("System.Array").GetTypeSize() + 2 * 4); // add header size
            writer.WriteLine(string.Empty);

            var resAdd = opCode.Result;

            var resAlloc = llvmWriter.WriteSetResultNumber(opCode, llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.Write("call i8* @{1}(i32 {0})", resAdd, llvmWriter.GetAllocator());

            writer.WriteLine(string.Empty);
            llvmWriter.WriteTestNullValueAndThrowException(writer, opCode, resAlloc, "System.OutOfMemoryException", "new_arr");

            writer.WriteLine(string.Empty);

            if (!llvmWriter.Gc)
            {
                writer.WriteLine(
                   "call void @llvm.memset.p0i8.i32(i8* {0}, i8 0, i32 {1}, i32 {2}, i1 false)",
                   resAlloc,
                   resAdd,
                   LlvmWriter.PointerSize /*Align*/);
            }

            var opCodeTemp = OpCodePart.CreateNop;
            opCodeTemp.OpCodeOperands = opCode.OpCodeOperands;

            // init System.Array
            var arraySystemType = llvmWriter.ResolveType("System.Array");
            llvmWriter.WriteBitcast(opCode, resAlloc, arraySystemType);
            llvmWriter.ResolveType("System.Array").WriteCallInitObjectMethod(llvmWriter, opCode);
            writer.WriteLine(string.Empty);

            var arrayType = declaringType.ToArrayType(1);
            llvmWriter.WriteBitcast(opCode, resAlloc, declaringType.ToArrayType(1));
            writer.WriteLine(string.Empty);

            var arrayInstanceResult = opCode.Result;

            // save element size
            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("getelementptr inbounds ");
            arrayInstanceResult.Type.WriteTypePrefix(writer);
            writer.Write(" ");
            llvmWriter.WriteResult(arrayInstanceResult);
            writer.Write(", i32 0, i32 3");
            writer.WriteLine(string.Empty);

            writer.Write("store ");
            opCode.Result.Type.WriteTypePrefix(writer);
            writer.Write(" {0}, ", sizeOfElement);
            opCode.Result.Type.WriteTypePrefix(writer, true);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);
            writer.WriteLine(string.Empty);

            // save array size
            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("getelementptr inbounds ");
            arrayInstanceResult.Type.WriteTypePrefix(writer);
            writer.Write(" ");
            llvmWriter.WriteResult(arrayInstanceResult);
            writer.Write(", i32 0, i32 4");
            writer.WriteLine(string.Empty);

            writer.Write("store ");
            length.Result.Type.WriteTypePrefix(writer);
            writer.Write(" ");
            llvmWriter.WriteResult(length.Result);
            writer.Write(", ");
            opCode.Result.Type.WriteTypePrefix(writer, true);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);
            writer.WriteLine(string.Empty);

            writer.WriteLine("; end of new array");

            opCode.Result = arrayInstanceResult;
        }

        public static string GetArrayTypeReference(this LlvmWriter llvmWriter, string name, IType elementType, int length)
        {
            var convertString = llvmWriter.WriteToString(
            () =>
            {
                var writer = llvmWriter.Output;

                var array = elementType.ToArrayType(1);
                writer.Write("bitcast (");
                writer.Write("{1}* {0} to ", name, llvmWriter.GetArrayTypeHeader(elementType, length));
                array.WriteTypePrefix(writer);
                writer.Write(")");
            });

            return convertString;
        }

        public static string GetArrayPrefixDataType()
        {
            return "i32 (...)**, i32 (...)**, i32 (...)**, i32, i32";
        }

        public static string GetArrayTypeHeader(this LlvmWriter llvmWriter, IType elementType, int length)
        {
            var typeString = llvmWriter.WriteToString(
            () =>
            {
                var writer = llvmWriter.Output;
                elementType.WriteTypePrefix(writer);
            });

            return "{ " + GetArrayPrefixDataType() + ", [" + length + " x " + typeString + "] }";
        }

        // TODO: you need to init first 3 value with VTable of Object and interfaces, make it universal
        public static string GetArrayValuesHeader(this LlvmWriter llvmWriter, IType elementType, int length, int storeLength)
        {
            var typeString = llvmWriter.WriteToString(
            () =>
            {
                var writer = llvmWriter.Output;
                elementType.WriteTypePrefix(writer);
            });

            return "i32 (...)** null, i32 (...)** null, i32 (...)** null, i32 " + elementType.GetTypeSize(true) + ", i32 " + storeLength + ", [" + length + " x " + typeString + "]";
        }
    }
}
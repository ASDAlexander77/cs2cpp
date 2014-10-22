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

            var arrayIndex = llvmWriter.GetBytesIndex(data);
            var arrayLength = int.Parse(opCodeFieldInfoPart.Operand.FieldType.MetadataName.Substring(staticArrayInitTypeSizeLabel.Length));
            var arrayData = string.Format(
                "bitcast ([{1} x i8]* getelementptr inbounds ({2} i32, [{1} x i8] {3}* @.array{0}, i32 0, i32 1) to i8*)", arrayIndex, data.Length, '{', '}');

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

            var size = declaringType.GetTypeSize();
            llvmWriter.UnaryOper(writer, opCode, "mul");
            writer.WriteLine(", {0}", size);

            var resMul = opCode.Result;

            var intType = llvmWriter.ResolveType("System.Int32");
            llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("add i32 4, {0}", resMul);
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

            llvmWriter.WriteBitcast(opCode, resAlloc, intType);
            writer.WriteLine(string.Empty);

            var opCodeTemp = OpCodePart.CreateNop;
            opCodeTemp.OpCodeOperands = opCode.OpCodeOperands;

            // save array size
            llvmWriter.ProcessOperator(writer, opCodeTemp, "store");
            llvmWriter.PostProcessOperand(writer, opCode, 0, !opCode.OpCodeOperands[0].HasResult);
            writer.Write(", ");
            intType.WriteTypePrefix(writer);
            writer.Write("* ");
            llvmWriter.WriteResult(opCode.Result);
            writer.WriteLine(string.Empty);

            var tempRes = opCode.Result;
            var newArrayResult = llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("getelementptr ");

            // WriteTypePrefix(writer, declaringType);
            writer.Write("i32* ");
            llvmWriter.WriteResult(tempRes);
            writer.WriteLine(", i32 1");

            if (declaringType.TypeNotEquals(intType))
            {
                llvmWriter.WriteBitcast(opCode, newArrayResult, declaringType.ToArrayType(1));
                writer.WriteLine(string.Empty);
            }
            else
            {
                opCode.Result = opCode.Result.ToType(declaringType.ToArrayType(1));
            }

            writer.WriteLine("; end of new array");
        }
    }
}
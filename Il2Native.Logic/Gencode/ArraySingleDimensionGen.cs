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
    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class ArraySingleDimensionGen
    {
        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteArrayGetLength(this LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var typeToLoad = llvmWriter.ResolveType("System.Int32");
            llvmWriter.WriteBitcast(opCode, opCode.OpCodeOperands[0].Result, typeToLoad);
            writer.WriteLine(string.Empty);

            var res = opCode.Result;
            var resLen = llvmWriter.WriteSetResultNumber(opCode, typeToLoad);
            writer.Write("getelementptr ");
            typeToLoad.WriteTypePrefix(writer);
            writer.Write("* ");
            llvmWriter.WriteResultNumber(res);
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

            var resAlloc = llvmWriter.WriteSetResultNumber(opCode, llvmWriter.ResolveType("System.Byte").CreatePointer());
            writer.Write("call i8* @malloc(i32 {0})", resAdd);
            writer.WriteLine(string.Empty);

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
            llvmWriter.WriteResultNumber(opCode.Result);
            writer.WriteLine(string.Empty);

            var tempRes = opCode.Result;
            var resGetArr = llvmWriter.WriteSetResultNumber(opCode, intType);
            writer.Write("getelementptr ");

            // WriteTypePrefix(writer, declaringType);
            writer.Write("i32* ");
            llvmWriter.WriteResultNumber(tempRes);
            writer.WriteLine(", i32 1");

            if (declaringType.TypeNotEquals(intType))
            {
                llvmWriter.WriteCast(opCode, resGetArr, declaringType, !declaringType.IsValueType);
                writer.WriteLine(string.Empty);
            }

            opCode.Result = new LlvmResult(opCode.Result.Number, declaringType.CreateArray(1));

            writer.WriteLine("; end of new array");
        }
    }
}
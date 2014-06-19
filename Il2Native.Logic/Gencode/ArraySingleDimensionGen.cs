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
        /// <param name="writer">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <param name="length">
        /// </param>
        public static void WriteNewArray(this LlvmWriter llvmWriter, LlvmIndentedTextWriter writer, OpCodePart opCode, IType declaringType, OpCodePart length)
        {
            if (opCode.ResultNumber.HasValue)
            {
                return;
            }

            writer.WriteLine("; New array");

            var size = declaringType.GetTypeSize();
            llvmWriter.UnaryOper(writer, opCode, "mul");
            writer.WriteLine(", {0}", size);

            var resMul = opCode.ResultNumber;

            llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.Write("add i32 4, {0}", llvmWriter.GetResultNumber(resMul ?? -1));
            writer.WriteLine(string.Empty);

            var resAdd = opCode.ResultNumber;

            var resAlloc = llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.Write("call i8* @malloc(i32 {0})", llvmWriter.GetResultNumber(resAdd ?? -1));
            writer.WriteLine(string.Empty);

            llvmWriter.WriteBitcast(writer, opCode, resAlloc, TypeAdapter.FromType(typeof(int)));
            writer.WriteLine(string.Empty);

            var opCodeTemp = OpCodePart.CreateNop;
            opCodeTemp.OpCodeOperands = opCode.OpCodeOperands;

            // save array size
            llvmWriter.ProcessOperator(writer, opCodeTemp, "store");
            llvmWriter.PostProcessOperand(writer, opCode, 0, !opCode.OpCodeOperands[0].ResultNumber.HasValue);
            writer.Write(", ");
            llvmWriter.WriteTypePrefix(writer, TypeAdapter.FromType(typeof(int)));
            writer.Write("* ");
            llvmWriter.WriteResultNumber(opCode.ResultNumber ?? -1);
            writer.WriteLine(string.Empty);

            var tempRes = opCode.ResultNumber.Value;
            var resGetArr = llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.Write("getelementptr ");

            // WriteTypePrefix(writer, declaringType);
            writer.Write("i32* ");
            llvmWriter.WriteResultNumber(tempRes);
            writer.Write(", i32 1");

            if (declaringType != TypeAdapter.FromType(typeof(int)))
            {
                writer.WriteLine(string.Empty);
                llvmWriter.WriteCast(writer, opCode, TypeAdapter.FromType(typeof(int)), resGetArr, declaringType, true);
            }

            writer.WriteLine("; end of new array");
        }
    }
}
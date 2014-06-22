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

            llvmWriter.WriteBitcast(opCode.OpCodeOperands[0], TypeAdapter.FromType(typeof(int)));
            writer.WriteLine(string.Empty);

            var res = opCode.Result;
            var resLen = llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(int)));
            writer.Write("getelementptr ");
            TypeAdapter.FromType(typeof(int)).WriteTypePrefix(writer);
            writer.Write("* ");
            llvmWriter.WriteResultNumber(res);
            writer.WriteLine(", i32 -1");

            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, TypeAdapter.FromType(typeof(int)), llvmWriter.GetResultNumber(resLen));
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

            llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(int)));
            writer.Write("add i32 4, {0}", llvmWriter.GetResultNumber(resMul));
            writer.WriteLine(string.Empty);

            var resAdd = opCode.Result;

            var resAlloc = llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(byte*)));
            writer.Write("call i8* @malloc(i32 {0})", llvmWriter.GetResultNumber(resAdd));
            writer.WriteLine(string.Empty);

            llvmWriter.WriteBitcast(opCode, resAlloc, TypeAdapter.FromType(typeof(int)));
            writer.WriteLine(string.Empty);

            var opCodeTemp = OpCodePart.CreateNop;
            opCodeTemp.OpCodeOperands = opCode.OpCodeOperands;

            // save array size
            llvmWriter.ProcessOperator(writer, opCodeTemp, "store");
            llvmWriter.PostProcessOperand(writer, opCode, 0, !opCode.OpCodeOperands[0].HasResult);
            writer.Write(", ");
            TypeAdapter.FromType(typeof(int)).WriteTypePrefix(writer);
            writer.Write("* ");
            llvmWriter.WriteResultNumber(opCode.Result);
            writer.WriteLine(string.Empty);

            var tempRes = opCode.Result;
            var resGetArr = llvmWriter.WriteSetResultNumber(opCode, TypeAdapter.FromType(typeof(int)));
            writer.Write("getelementptr ");

            // WriteTypePrefix(writer, declaringType);
            writer.Write("i32* ");
            llvmWriter.WriteResultNumber(tempRes);
            writer.WriteLine(", i32 1");

            if (declaringType.TypeNotEquals(TypeAdapter.FromType(typeof(int))))
            {
                llvmWriter.WriteCast(opCode, resGetArr, declaringType, !declaringType.IsValueType);
                writer.WriteLine(string.Empty);
            }

            opCode.Result = new LlvmResult(opCode.Result.Number, declaringType.CreateArray(1));

            writer.WriteLine("; end of new array");
        }
    }
}
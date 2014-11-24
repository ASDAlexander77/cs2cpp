// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InterlockedGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System.Linq;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class InterlockedGen
    {
        /// <summary>
        /// </summary>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="oper">
        /// </param>
        /// <param name="attribs">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void IncDecInterlockBase(this OpCodePart opCodeMethodInfo, string oper, string attribs, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var first = opCodeMethodInfo.OpCodeOperands.First();
            var resultType = first.Result.Type.ToDereferencedType();

            llvmWriter.WriteSetResultNumber(opCodeMethodInfo, resultType);

            writer.Write(oper);

            // i32* %ptr, i32 %cmp, i32 %squared 
            var index = 0;
            foreach (var operand in opCodeMethodInfo.OpCodeOperands)
            {
                if (index++ > 0)
                {
                    writer.Write(", ");
                }

                operand.Result.Type.WriteTypePrefix(writer);
                writer.Write(' ');
                llvmWriter.WriteResult(operand.Result);
            }

            writer.Write(", ");
            resultType.WriteTypePrefix(writer);
            writer.Write(" 1");

            writer.WriteLine(attribs);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="oper">
        /// </param>
        /// <param name="attribs">
        /// </param>
        /// <param name="extractValue">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void InterlockBase(this OpCodePart opCodeMethodInfo, string oper, string attribs, bool extractValue, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; {0} start", oper);

            IType pointerIntSize = null;
            IType originalType = null;
            var first = opCodeMethodInfo.OpCodeOperands.First();
            var operType = first.Result.Type.ToDereferencedType();

            var pointerExchange = operType.IsClass || operType.IsDelegate || operType.IsPointer;
            if (pointerExchange)
            {
                pointerIntSize = llvmWriter.GetIntTypeByByteSize(LlvmWriter.PointerSize);

                llvmWriter.WriteBitcast(first, first.Result, pointerIntSize.ToPointerType());
                writer.WriteLine(string.Empty);

                foreach (var operand in opCodeMethodInfo.OpCodeOperands.Skip(1))
                {
                    if (originalType == null)
                    {
                        originalType = operand.Result.Type;
                    }

                    llvmWriter.WritePtrToInt(operand, operand.Result, pointerIntSize);
                    writer.WriteLine(string.Empty);
                }
            }

            var opResult = llvmWriter.WriteSetResultNumber(opCodeMethodInfo, pointerIntSize ?? opCodeMethodInfo.OpCodeOperands.Skip(1).First().Result.Type);

            writer.Write(oper);

            var index = 0;
            foreach (var operand in opCodeMethodInfo.OpCodeOperands)
            {
                if (index++ > 0)
                {
                    writer.Write(", ");
                }

                operand.Result.Type.WriteTypePrefix(writer);
                writer.Write(' ');
                llvmWriter.WriteResult(operand.Result);
            }

            writer.WriteLine(attribs);

            if (extractValue)
            {
                llvmWriter.WriteSetResultNumber(opCodeMethodInfo, pointerIntSize ?? opCodeMethodInfo.OpCodeOperands.Skip(1).First().Result.Type);
                writer.Write("extractvalue { ");
                opResult.Type.WriteTypePrefix(writer);
                writer.Write(", i1 } ");
                llvmWriter.WriteResult(opResult);
                writer.WriteLine(", 0");
            }

            if (pointerExchange)
            {
                // cast back
                llvmWriter.WriteIntToPtr(opCodeMethodInfo, opCodeMethodInfo.Result, originalType);
            }

            writer.WriteLine(string.Empty);
            writer.WriteLine("; {0} end", oper);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsInterlockedFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            if (method.DeclaringType.FullName != "System.Threading.Interlocked")
            {
                return false;
            }

            switch (method.MetadataName)
            {
                case "Increment":
                case "Decrement":
                case "Exchange`1":
                case "Exchange":
                case "CompareExchange`1":
                case "CompareExchange":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteInterlockedFunction(this IMethod method, OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            switch (method.MetadataName)
            {
                case "Increment":
                    opCodeMethodInfo.IncDecInterlockBase("atomicrmw add ", " acquire", llvmWriter);
                    break;

                case "Decrement":
                    opCodeMethodInfo.IncDecInterlockBase("atomicrmw sub ", " acquire", llvmWriter);
                    break;

                case "Exchange`1":
                case "Exchange":
                    opCodeMethodInfo.InterlockBase("atomicrmw xchg ", " acquire", false, llvmWriter);
                    break;

                case "CompareExchange`1":
                case "CompareExchange":
                    opCodeMethodInfo.InterlockBase("cmpxchg ", llvmWriter.IsLlvm34OrLower ? " acq_rel" : " acq_rel monotonic", !llvmWriter.IsLlvm35 && !llvmWriter.IsLlvm34OrLower, llvmWriter);
                    break;
            }
        }
    }
}
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
    using System;
    using System.Globalization;
    using System.Linq;
    using CodeParts;
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
        public static void IncDecInterlockBase(
            this OpCodePart opCodeMethodInfo,
            string oper,
            string attribs,
            LlvmWriter llvmWriter)
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

                operand.Result.Type.WriteTypePrefix(llvmWriter);
                writer.Write(' ');
                llvmWriter.WriteResult(operand.Result);
            }

            writer.Write(", ");
            resultType.WriteTypePrefix(llvmWriter);
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
        public static void InterlockBase(
            this OpCodePart opCodeMethodInfo,
            string oper,
            string attribs,
            bool extractValue,
            LlvmWriter llvmWriter,
            int[] operands)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine("; {0} start", oper);

            IType intType = null;
            IType originalType = null;
            var first = opCodeMethodInfo.OpCodeOperands.First();
            var operType = first.Result.Type.ToDereferencedType();

            // TODO: fix issue with to change value for IntPtr/UIntPtr

            bool realExchange = false;
            var pointerExchange = operType.IsClass || operType.IsDelegate || operType.IsPointer || operType.IsArray || operType.TypeEquals(llvmWriter.System.System_IntPtr) || operType.TypeEquals(llvmWriter.System.System_UIntPtr);
            if (pointerExchange)
            {
                intType = llvmWriter.GetIntTypeByByteSize(LlvmWriter.PointerSize);

                llvmWriter.WriteBitcast(first, first.Result, intType.ToPointerType());
                writer.WriteLine(string.Empty);

                foreach (var operand in opCodeMethodInfo.OpCodeOperands.Skip(1))
                {
                    if (originalType == null)
                    {
                        originalType = operand.Result.Type;
                    }

                    llvmWriter.WritePtrToInt(operand, operand.Result, intType);
                    writer.WriteLine(string.Empty);
                }
            }
            else if (operType.IsReal())
            {
                realExchange = true;
                intType = llvmWriter.GetIntTypeByByteSize(operType.Name == "Double" ? 8 : operType.Name == "Single" ? 4 : LlvmWriter.PointerSize);

                // bitcast float to i32 and double to i64
                llvmWriter.WriteBitcast(first, first.Result, intType.ToPointerType());
                writer.WriteLine(string.Empty);

                foreach (var operand in opCodeMethodInfo.OpCodeOperands.Skip(1))
                {
                    if (originalType == null)
                    {
                        originalType = operand.Result.Type;
                    }

                    if (!(operand.Result is ConstValue))
                    {
                        llvmWriter.WriteBitcast(operand, operand.Result, intType, false);
                        writer.WriteLine(string.Empty);
                    }
                    else
                    {
                        operand.Result = new ConstValue(Convert.ToInt64(operand.Result.Name, 16), intType);
                    }
                }
            } 
            
            var opResult = llvmWriter.WriteSetResultNumber(
                opCodeMethodInfo,
                intType ?? opCodeMethodInfo.OpCodeOperands.Skip(1).First().Result.Type);

            writer.Write(oper);

            var index = 0;
            foreach (var operandNumber in operands)
            {
                llvmWriter.WriteParameter(index++, opCodeMethodInfo.OpCodeOperands[operandNumber]);
            }

            writer.WriteLine(attribs);

            if (extractValue)
            {
                llvmWriter.WriteSetResultNumber(
                    opCodeMethodInfo,
                    intType ?? opCodeMethodInfo.OpCodeOperands.Skip(1).First().Result.Type);
                writer.Write("extractvalue { ");
                opResult.Type.WriteTypePrefix(llvmWriter);
                writer.Write(", i1 } ");
                llvmWriter.WriteResult(opResult);
                writer.WriteLine(", 0");
            }

            if (pointerExchange)
            {
                // cast back
                llvmWriter.WriteIntToPtr(opCodeMethodInfo, opCodeMethodInfo.Result, originalType);
            }
            else if (realExchange)
            {
                // cast back to float/double
                llvmWriter.WriteBitcast(opCodeMethodInfo, opCodeMethodInfo.Result, originalType, false);
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
        public static void WriteInterlockedFunction(
            this IMethod method,
            OpCodePart opCodeMethodInfo,
            LlvmWriter llvmWriter)
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
                    opCodeMethodInfo.InterlockBase("atomicrmw xchg ", " acquire", false, llvmWriter, new[] { 0, 1 });
                    break;

                case "CompareExchange`1":
                case "CompareExchange":
                    opCodeMethodInfo.InterlockBase(
                        "cmpxchg ",
                        llvmWriter.IsLlvm34OrLower ? " acq_rel" : " acq_rel monotonic",
                        !llvmWriter.IsLlvm35 && !llvmWriter.IsLlvm34OrLower,
                        llvmWriter,
                        new[] { 0, 2, 1 });
                    break;
            }
        }

        private static int WriteParameter(this LlvmWriter llvmWriter, int index, OpCodePart operand)
        {
            var writer = llvmWriter.Output;

            if (index++ > 0)
            {
                writer.Write(", ");
            }

            operand.Result.Type.WriteTypePrefix(llvmWriter);
            writer.Write(' ');
            llvmWriter.WriteResult(operand.Result);
            return index;
        }
    }
}
namespace Il2Native.Logic.Gencode
{
    using System;
    using System.Linq;
    using Il2Native.Logic.CodeParts;
    using PEAssemblyReader;

    public static class InterlockedGen
    {
        public static bool IsInterlockedFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            if (!method.IsExternal && !method.IsGenericMethod)
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

        public static void WriteInterlockedFunction(this IMethod method, OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            switch (method.MetadataName)
            {
                case "Increment":
                    method.IncDecInterlockBase(opCodeMethodInfo, "atomicrmw add ", " acquire", llvmWriter);
                    break;

                case "Decrement":
                    method.IncDecInterlockBase(opCodeMethodInfo, "atomicrmw sub ", " acquire", llvmWriter);
                    break;

                case "Exchange`1":
                case "Exchange":
                    method.InterlockBase(opCodeMethodInfo, "atomicrmw xchg ", " acquire", llvmWriter);
                    break;

                case "CompareExchange`1":
                case "CompareExchange":
                    method.InterlockBase(opCodeMethodInfo, "cmpxchg ", " acq_rel monotonic", llvmWriter);
                    break;
            }
        }

        private static void IncDecInterlockBase(this IMethod method, OpCodePart opCodeMethodInfo, string oper, string attribs, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var first = opCodeMethodInfo.OpCodeOperands.First();
            var resultType = first.Result.Type.ToDereferencedType();

            llvmWriter.WriteSetResultNumber(opCodeMethodInfo, resultType);

            writer.Write(oper);
            //i32* %ptr, i32 %cmp, i32 %squared 

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

        private static void InterlockBase(this IMethod method, OpCodePart opCodeMethodInfo, string oper, string attribs, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

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

            llvmWriter.WriteSetResultNumber(opCodeMethodInfo, pointerIntSize ?? opCodeMethodInfo.OpCodeOperands.Skip(1).First().Result.Type);

            writer.Write(oper);
            //i32* %ptr, i32 %cmp, i32 %squared 

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

            if (pointerExchange)
            {
                // cast back
                llvmWriter.WriteIntToPtr(opCodeMethodInfo, opCodeMethodInfo.Result, originalType);
            }
        }
    }
}

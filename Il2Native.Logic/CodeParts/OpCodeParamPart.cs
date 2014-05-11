namespace Il2Native.Logic.CodeParts
{
    using System.Reflection.Emit;

    public class OpCodeParamPart<T> : OpCodePart
    {
        public OpCodeParamPart(OpCode opcode, int addressStart, int addressEnd, T param)
            : base(opcode, addressStart, addressEnd)
        {
            this.Operand = param;
        }

        public T Operand { get; private set; }
    }
}

namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeBytePart : OpCodeParamPart<Byte>
    {
        public OpCodeBytePart(OpCode opcode, int addressStart, int addressEnd, Byte param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

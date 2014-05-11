namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeSBytePart : OpCodeParamPart<SByte>
    {
        public OpCodeSBytePart(OpCode opcode, int addressStart, int addressEnd, SByte param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

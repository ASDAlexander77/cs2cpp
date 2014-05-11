namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeStringPart : OpCodeParamPart<String>
    {
        public OpCodeStringPart(OpCode opcode, int addressStart, int addressEnd, String param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

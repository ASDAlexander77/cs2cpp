namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeDoublePart : OpCodeParamPart<Double>
    {
        public OpCodeDoublePart(OpCode opcode, int addressStart, int addressEnd, Double param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

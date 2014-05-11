namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeLabelsPart : OpCodeParamPart<int[]>
    {
        public OpCodeLabelsPart(OpCode opcode, int addressStart, int addressEnd, int[] param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

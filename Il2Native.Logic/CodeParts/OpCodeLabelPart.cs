namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeLabelPart : OpCodeParamPart<Label>
    {
        public OpCodeLabelPart(OpCode opcode, int addressStart, int addressEnd, Label param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

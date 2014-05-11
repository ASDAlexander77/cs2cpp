namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeSinglePart : OpCodeParamPart<Single>
    {
        public OpCodeSinglePart(OpCode opcode, int addressStart, int addressEnd, Single param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

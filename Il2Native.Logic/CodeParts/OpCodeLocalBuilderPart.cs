namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeLocalBuilderPart : OpCodeParamPart<LocalBuilder>
    {
        public OpCodeLocalBuilderPart(OpCode opcode, int addressStart, int addressEnd, LocalBuilder param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

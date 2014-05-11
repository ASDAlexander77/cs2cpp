namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeInt64Part : OpCodeParamPart<Int64>
    {
        public OpCodeInt64Part(OpCode opcode, int addressStart, int addressEnd, Int64 param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

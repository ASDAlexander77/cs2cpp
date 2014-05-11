namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeInt32Part : OpCodeParamPart<Int32>
    {
        public OpCodeInt32Part(OpCode opcode, int addressStart, int addressEnd, Int32 param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

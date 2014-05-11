namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeInt16Part : OpCodeParamPart<Int16>
    {
        public OpCodeInt16Part(OpCode opcode, int addressStart, int addressEnd, Int16 param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

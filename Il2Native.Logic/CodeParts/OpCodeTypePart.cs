namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeTypePart : OpCodeParamPart<Type>
    {
        public OpCodeTypePart(OpCode opcode, int addressStart, int addressEnd, Type param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public class OpCodeFieldInfoPart : OpCodeParamPart<FieldInfo>
    {
        public OpCodeFieldInfoPart(OpCode opcode, int addressStart, int addressEnd, FieldInfo param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

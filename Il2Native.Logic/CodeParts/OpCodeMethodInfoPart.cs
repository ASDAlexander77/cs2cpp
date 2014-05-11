namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    public class OpCodeMethodInfoPart : OpCodeParamPart<MethodBase>
    {
        public OpCodeMethodInfoPart(OpCode opcode, int addressStart, int addressEnd, MethodBase param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

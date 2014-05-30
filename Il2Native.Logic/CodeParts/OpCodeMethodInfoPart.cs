namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using PEAssemblyReader;

    public class OpCodeMethodInfoPart : OpCodeParamPart<IMethod>
    {
        public OpCodeMethodInfoPart(OpCode opcode, int addressStart, int addressEnd, IMethod param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    using PEAssemblyReader;

    public class OpCodeTypePart : OpCodeParamPart<IType>
    {
        public OpCodeTypePart(OpCode opcode, int addressStart, int addressEnd, IType param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

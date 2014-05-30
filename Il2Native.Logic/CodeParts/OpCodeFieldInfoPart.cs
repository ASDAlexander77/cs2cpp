namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    using PEAssemblyReader;

    public class OpCodeFieldInfoPart : OpCodeParamPart<IField>
    {
        public OpCodeFieldInfoPart(OpCode opcode, int addressStart, int addressEnd, IField param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

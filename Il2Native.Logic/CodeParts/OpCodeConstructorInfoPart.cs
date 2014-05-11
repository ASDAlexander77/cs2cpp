namespace Il2Native.Logic.CodeParts
{
    using System.Reflection;
    using System.Reflection.Emit;

    public class OpCodeConstructorInfoPart : OpCodeParamPart<ConstructorInfo>
    {
        public OpCodeConstructorInfoPart(OpCode opcode, int addressStart, int addressEnd, ConstructorInfo param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

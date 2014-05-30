namespace Il2Native.Logic.CodeParts
{
    using System.Reflection;
    using System.Reflection.Emit;

    using PEAssemblyReader;

    public class OpCodeConstructorInfoPart : OpCodeParamPart<IConstructor>
    {
        public OpCodeConstructorInfoPart(OpCode opcode, int addressStart, int addressEnd, IConstructor param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    public class OpCodeSignatureHelperPart : OpCodeParamPart<SignatureHelper>
    {
        public OpCodeSignatureHelperPart(OpCode opcode, int addressStart, int addressEnd, SignatureHelper param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}

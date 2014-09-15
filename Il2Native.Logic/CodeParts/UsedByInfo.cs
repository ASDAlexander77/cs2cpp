namespace Il2Native.Logic.CodeParts
{
    using System.Linq;

    public class UsedByInfo
    {
        public UsedByInfo(OpCodePart opCode)
        {
            this.OpCode = opCode;
        }

        public UsedByInfo(OpCodePart opCode, int operandPosition)
        {
            this.OpCode = opCode;
            this.OperandPosition = operandPosition;
        }

        public OpCodePart OpCode { get; private set; }

        public int OperandPosition { get; private set; }

        public bool Any(params Code[] codes)
        {
            var code = OpCode.ToCode();
            return codes.Any(item => item == code);
        }
    }
}

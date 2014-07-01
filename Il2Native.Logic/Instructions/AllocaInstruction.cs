namespace Il2Native.Logic.Instructions
{
    using System;
    using Il2Native.Logic.Instructions.Base;

    public class AllocaInstruction : BaseInstructionWithResult
    {
        public AllocaInstruction()
        {
        }

        public override void WriteTo(LlvmIndentedTextWriter writer)
        {
            base.WriteTo(writer);
        }
    }
}

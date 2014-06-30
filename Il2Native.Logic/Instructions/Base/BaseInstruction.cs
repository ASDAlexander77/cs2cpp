namespace Il2Native.Logic.Instructions.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class BaseInstruction
    {
        public abstract void WriteTo(LlvmIndentedTextWriter writer);
    }
}

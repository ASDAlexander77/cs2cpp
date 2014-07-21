// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllocaInstruction.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Instructions
{
    using Il2Native.Logic.Instructions.Base;

    /// <summary>
    /// </summary>
    public class AllocaInstruction : BaseInstructionWithResult
    {
        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        public override void WriteTo(LlvmIndentedTextWriter writer)
        {
            base.WriteTo(writer);
        }
    }
}
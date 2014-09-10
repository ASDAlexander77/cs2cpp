// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseInstruction.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Instructions.Base
{
    /// <summary>
    /// </summary>
    public abstract class BaseInstruction
    {
        /// <summary>
        /// </summary>
        public string Command { get; protected set; }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        public abstract void WriteTo(LlvmIndentedTextWriter writer);
    }
}
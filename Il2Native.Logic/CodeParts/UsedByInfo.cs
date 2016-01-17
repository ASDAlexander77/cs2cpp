// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsedByInfo.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.CodeParts
{
    using System.Linq;

    /// <summary>
    /// </summary>
    public class UsedByInfo
    {
        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        public UsedByInfo(OpCodePart opCode)
        {
            this.OpCode = opCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="operandPosition">
        /// </param>
        public UsedByInfo(OpCodePart opCode, int operandPosition)
        {
            this.OpCode = opCode;
            this.OperandPosition = operandPosition;
        }

        /// <summary>
        /// </summary>
        public OpCodePart OpCode { get; private set; }

        /// <summary>
        /// </summary>
        public int OperandPosition { get; private set; }
    }
}
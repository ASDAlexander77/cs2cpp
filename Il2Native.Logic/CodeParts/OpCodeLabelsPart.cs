// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeLabelsPart.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.CodeParts
{
    using System.Reflection.Emit;

    /// <summary>
    /// </summary>
    public class OpCodeLabelsPart : OpCodeParamPart<int[]>
    {
        /// <summary>
        /// </summary>
        /// <param name="opcode">
        /// </param>
        /// <param name="addressStart">
        /// </param>
        /// <param name="addressEnd">
        /// </param>
        /// <param name="param">
        /// </param>
        public OpCodeLabelsPart(OpCode opcode, int addressStart, int addressEnd, int[] param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
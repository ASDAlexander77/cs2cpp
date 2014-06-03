// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeLabelPart.cs" company="">
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
    public class OpCodeLabelPart : OpCodeParamPart<Label>
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
        public OpCodeLabelPart(OpCode opcode, int addressStart, int addressEnd, Label param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
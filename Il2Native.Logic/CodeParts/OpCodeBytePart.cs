// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeBytePart.cs" company="">
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
    public class OpCodeBytePart : OpCodeParamPart<byte>
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
        public OpCodeBytePart(OpCode opcode, int addressStart, int addressEnd, byte param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
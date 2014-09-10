// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeSBytePart.cs" company="">
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
    public class OpCodeSBytePart : OpCodeParamPart<sbyte>
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
        public OpCodeSBytePart(OpCode opcode, int addressStart, int addressEnd, sbyte param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
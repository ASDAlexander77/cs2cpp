// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeSinglePart.cs" company="">
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
    public class OpCodeSinglePart : OpCodeParamPart<float>
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
        public OpCodeSinglePart(OpCode opcode, int addressStart, int addressEnd, float param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
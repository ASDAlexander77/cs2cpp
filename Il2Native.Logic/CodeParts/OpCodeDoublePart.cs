// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeDoublePart.cs" company="">
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
    public class OpCodeDoublePart : OpCodeParamPart<double>
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
        public OpCodeDoublePart(OpCode opcode, int addressStart, int addressEnd, double param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
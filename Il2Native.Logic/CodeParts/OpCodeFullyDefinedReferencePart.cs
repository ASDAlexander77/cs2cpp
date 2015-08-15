// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeFullyDefinedReferencePart.cs" company="">
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
    public class OpCodeFullyDefinedReferencePart : OpCodeParamPart<FullyDefinedReference>
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
        public OpCodeFullyDefinedReferencePart(OpCode opcode, int addressStart, int addressEnd, FullyDefinedReference param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
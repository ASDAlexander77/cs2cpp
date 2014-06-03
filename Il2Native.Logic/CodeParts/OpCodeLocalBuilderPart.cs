// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeLocalBuilderPart.cs" company="">
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
    public class OpCodeLocalBuilderPart : OpCodeParamPart<LocalBuilder>
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
        public OpCodeLocalBuilderPart(OpCode opcode, int addressStart, int addressEnd, LocalBuilder param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeTypePart.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.CodeParts
{
    using System.Reflection.Emit;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class OpCodeTypePart : OpCodeParamPart<IType>
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
        public OpCodeTypePart(OpCode opcode, int addressStart, int addressEnd, IType param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
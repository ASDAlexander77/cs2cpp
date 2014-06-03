// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeStringPart.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Reflection.Emit;

    /// <summary>
    /// </summary>
    public class OpCodeStringPart : OpCodeParamPart<string>
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
        public OpCodeStringPart(OpCode opcode, int addressStart, int addressEnd, string param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
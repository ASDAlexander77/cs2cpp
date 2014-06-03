// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeInt64Part.cs" company="">
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
    public class OpCodeInt64Part : OpCodeParamPart<long>
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
        public OpCodeInt64Part(OpCode opcode, int addressStart, int addressEnd, long param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
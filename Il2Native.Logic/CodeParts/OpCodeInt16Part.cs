// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeInt16Part.cs" company="">
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
    public class OpCodeInt16Part : OpCodeParamPart<short>
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
        public OpCodeInt16Part(OpCode opcode, int addressStart, int addressEnd, short param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
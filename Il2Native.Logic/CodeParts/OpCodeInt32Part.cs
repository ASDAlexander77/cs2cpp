// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeInt32Part.cs" company="">
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
    public class OpCodeInt32Part : OpCodeParamPart<int>
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
        public OpCodeInt32Part(OpCode opcode, int addressStart, int addressEnd, int param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
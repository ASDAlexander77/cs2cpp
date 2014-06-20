// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeParamPart.cs" company="">
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
    /// <typeparam name="T">
    /// </typeparam>
    public class OpCodeParamPart<T> : OpCodePart
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
        public OpCodeParamPart(OpCode opcode, int addressStart, int addressEnd, T param)
            : base(opcode, addressStart, addressEnd)
        {
            this.Operand = param;
        }

        /// <summary>
        /// </summary>
        public T Operand { get; private set; }
    }
}
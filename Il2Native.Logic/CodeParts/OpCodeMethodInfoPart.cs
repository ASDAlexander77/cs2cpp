// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeMethodInfoPart.cs" company="">
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
    public class OpCodeMethodInfoPart : OpCodeParamPart<IMethod>
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
        public OpCodeMethodInfoPart(OpCode opcode, int addressStart, int addressEnd, IMethod param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
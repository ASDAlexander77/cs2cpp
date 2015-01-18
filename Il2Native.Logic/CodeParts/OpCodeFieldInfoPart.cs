// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeFieldInfoPart.cs" company="">
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
    public class OpCodeFieldInfoPart : OpCodeParamPart<IField>
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
        public OpCodeFieldInfoPart(OpCode opcode, int addressStart, int addressEnd, IField param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
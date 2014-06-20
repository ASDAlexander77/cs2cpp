// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeConstructorInfoPart.cs" company="">
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
    public class OpCodeConstructorInfoPart : OpCodeParamPart<IConstructor>
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
        public OpCodeConstructorInfoPart(OpCode opcode, int addressStart, int addressEnd, IConstructor param)
            : base(opcode, addressStart, addressEnd, param)
        {
        }
    }
}
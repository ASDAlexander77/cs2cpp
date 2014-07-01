// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LlvmResult.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    using System;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class LlvmResult
    {
        /// <summary>
        /// </summary>
        /// <param name="result">
        /// </param>
        public LlvmResult(LlvmResult result)
            : this(result.Number, result.Type)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="number">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public LlvmResult(int number, IType type)
        {
            if (number <= 0)
            {
                throw new ArgumentException("number");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            this.Number = number;
            this.Type = type;
        }

        /// <summary>
        /// </summary>
        protected LlvmResult()
        {
        }

        public override string ToString()
        {
            return string.Concat("%.r", this.Number);
        }

        /// <summary>
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// </summary>
        public IType Type { get; private set; }
    }
}
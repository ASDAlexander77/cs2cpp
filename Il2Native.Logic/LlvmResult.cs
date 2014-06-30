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

        public LlvmResult(OpCodePart directValue, IType type)
        {
            if (directValue == null)
            {
                throw new ArgumentException("directValue");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            this.DirectValue = directValue;
            this.Type = type;
        }

        /// <summary>
        /// </summary>
        protected LlvmResult()
        {
        }

        /// <summary>
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// </summary>
        public IType Type { get; private set; }

        /// <summary>
        /// temp solution to fix issue with converting data where conversion is not required and operand is direct value
        /// </summary>
        public OpCodePart DirectValue { get; private set; }
    }
}
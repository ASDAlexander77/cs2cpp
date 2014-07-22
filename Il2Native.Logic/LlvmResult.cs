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

        /// <summary>
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// </summary>
        public IType Type { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public FullyDefinedReference ToFullyDefinedReference()
        {
            return new FullyDefinedReference(this.ToString(), this.Type);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public FullyDefinedReference ToFullyDefinedReferenceAsNormalType()
        {
            return new FullyDefinedReference(this.ToString(), this.Type.ToNormal());
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return string.Concat("%.r", this.Number);
        }
    }
}
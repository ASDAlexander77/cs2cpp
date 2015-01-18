// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncrementalResult.cs" company="">
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
    public class IncrementalResult : FullyDefinedReference
    {
        /// <summary>
        /// </summary>
        /// <param name="result">
        /// </param>
        public IncrementalResult(IncrementalResult result)
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
        public IncrementalResult(int number, IType type)
            : base(type)
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
        }

        /// <summary>
        /// </summary>
        protected int Number { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return string.Concat("%.r", this.Number);
        }

        /// <summary>
        /// </summary>
        /// <param name="newType">
        /// </param>
        /// <returns>
        /// </returns>
        public override FullyDefinedReference ToType(IType newType)
        {
            return new IncrementalResult(this.Number, newType);
        }
    }
}
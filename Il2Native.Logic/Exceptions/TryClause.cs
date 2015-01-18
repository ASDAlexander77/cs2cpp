// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TryClause.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Exceptions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public class TryClause : IComparable
    {
        /// <summary>
        /// </summary>
        private readonly Lazy<List<CatchOfFinallyClause>> lazyCatches =
            new Lazy<List<CatchOfFinallyClause>>(() => new List<CatchOfFinallyClause>());

        /// <summary>
        /// </summary>
        public List<CatchOfFinallyClause> Catches
        {
            get { return this.lazyCatches.Value; }
        }

        /// <summary>
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        public int CompareTo(object obj)
        {
            var other = obj as TryClause;
            if (other != null)
            {
                return this.CompareTo(other);
            }

            return -1;
        }

        /// <summary>
        /// </summary>
        /// <param name="other">
        /// </param>
        /// <returns>
        /// </returns>
        public int CompareTo(TryClause other)
        {
            var cmp = this.Offset.CompareTo(other.Offset);
            if (cmp != 0)
            {
                return cmp;
            }

            return this.Length.CompareTo(other.Length);
        }
    }
}
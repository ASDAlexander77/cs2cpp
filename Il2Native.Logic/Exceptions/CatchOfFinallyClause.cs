// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CatchOfFinallyClause.cs" company="">
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
    using System.Reflection;
    using PEAssemblyReader;

    // if Exception is null means Finally Clause
    /// <summary>
    /// </summary>
    public class CatchOfFinallyClause
    {
        /// <summary>
        /// </summary>
        private readonly Lazy<List<string>> lazyFinallyJumps = new Lazy<List<string>>(() => new List<string>());

        /// <summary>
        /// </summary>
        public IType Catch { get; set; }

        /// <summary>
        /// </summary>
        public bool EmptyFinallyRethrowRequired { get; set; }

        /// <summary>
        /// </summary>
        public FullyDefinedReference ExceptionResult { get; set; }

        /// <summary>
        /// </summary>
        public IList<string> FinallyJumps
        {
            get { return this.lazyFinallyJumps.Value; }
        }

        /// <summary>
        /// </summary>
        public bool FinallyVariablesAreWritten { get; set; }

        /// <summary>
        /// </summary>
        public ExceptionHandlingClauseOptions Flags { get; set; }

        /// <summary>
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// </summary>
        public CatchOfFinallyClause Next { get; set; }

        /// <summary>
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// </summary>
        public TryClause OwnerTry { get; set; }

        /// <summary>
        /// </summary>
        public bool RethrowCatchWithCleanUpRequired { get; set; }
    }
}
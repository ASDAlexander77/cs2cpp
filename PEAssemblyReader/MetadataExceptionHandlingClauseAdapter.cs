// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MetadataExceptionHandlingClauseAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("Name = {Name}, Type = {CatchType.FullName}")]
    public class MetadataExceptionHandlingClauseAdapter : IExceptionHandlingClause
    {
        /// <summary>
        /// </summary>
        private readonly TypeSymbol catchType;

        /// <summary>
        /// </summary>
        private ExceptionRegion exceptionRegion;

        /// <summary>
        /// </summary>
        private readonly Lazy<IList<string>> lazyFinallyJumps;

        /// <summary>
        /// </summary>
        /// <param name="exceptionRegion">
        /// </param>
        /// <param name="catchType">
        /// </param>
        internal MetadataExceptionHandlingClauseAdapter(ExceptionRegion exceptionRegion, TypeSymbol catchType)
        {
            this.exceptionRegion = exceptionRegion;
            this.catchType = catchType;
            this.lazyFinallyJumps = new Lazy<IList<string>>(() => new List<string>());
        }

        /// <summary>
        /// </summary>
        /// <param name="exceptionRegion">
        /// </param>
        /// <param name="catchType">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        internal MetadataExceptionHandlingClauseAdapter(ExceptionRegion exceptionRegion, TypeSymbol catchType, IGenericContext genericContext)
            : this(exceptionRegion, catchType)
        {
            this.GenericContext = genericContext;
        }

        /// <summary>
        /// </summary>
        public IType CatchType
        {
            get
            {
                return this.catchType != null
                           ? new MetadataTypeAdapter(MetadataModuleAdapter.SubstituteTypeSymbolIfNeeded(this.catchType, this.GenericContext))
                           : null;
            }
        }

        /// <summary>
        /// </summary>
        public IList<string> FinallyJumps
        {
            get
            {
                return this.lazyFinallyJumps.Value;
            }
        }

        /// <summary>
        /// </summary>
        public bool FinallyVariablesAreWritten { get; set; }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public ExceptionHandlingClauseOptions Flags
        {
            get
            {
                switch (this.exceptionRegion.Kind)
                {
                    case ExceptionRegionKind.Catch:
                        return ExceptionHandlingClauseOptions.Clause;
                    case ExceptionRegionKind.Filter:
                        return ExceptionHandlingClauseOptions.Filter;
                    case ExceptionRegionKind.Finally:
                        return ExceptionHandlingClauseOptions.Finally;
                    case ExceptionRegionKind.Fault:
                        return ExceptionHandlingClauseOptions.Fault;
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        public IGenericContext GenericContext { get; set; }

        /// <summary>
        /// </summary>
        public int HandlerLength
        {
            get
            {
                return this.exceptionRegion.HandlerLength;
            }
        }

        /// <summary>
        /// </summary>
        public int HandlerOffset
        {
            get
            {
                return this.exceptionRegion.HandlerOffset;
            }
        }

        /// <summary>
        /// </summary>
        public bool RethrowCatchWithCleanUpRequired { get; set; }

        /// <summary>
        /// </summary>
        public int TryLength
        {
            get
            {
                return this.exceptionRegion.TryLength;
            }
        }

        /// <summary>
        /// </summary>
        public int TryOffset
        {
            get
            {
                return this.exceptionRegion.TryOffset;
            }
        }
    }
}
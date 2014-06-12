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
    using System.Reflection;
    using System.Reflection.Metadata;

    using Microsoft.CodeAnalysis.CSharp.Symbols;

    /// <summary>
    /// </summary>
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
        /// <param name="exceptionRegion">
        /// </param>
        /// <param name="catchType">
        /// </param>
        internal MetadataExceptionHandlingClauseAdapter(ExceptionRegion exceptionRegion, TypeSymbol catchType)
        {
            this.exceptionRegion = exceptionRegion;
            this.catchType = catchType;
        }

        /// <summary>
        /// </summary>
        public IType CatchType
        {
            get
            {
                return catchType != null ? new MetadataTypeAdapter(this.catchType) : null;
            }
        }

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
                    case ExceptionRegionKind.Catch: return ExceptionHandlingClauseOptions.Clause;
                    case ExceptionRegionKind.Filter: return ExceptionHandlingClauseOptions.Filter;
                    case ExceptionRegionKind.Finally: return ExceptionHandlingClauseOptions.Finally;
                    case ExceptionRegionKind.Fault: return ExceptionHandlingClauseOptions.Fault;
                }

                throw new NotImplementedException();
            }
        }

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

        public int CompareTo(object obj)
        {
            var eh = obj as IExceptionHandlingClause;
            if (eh != null)
            {
                return this.HandlerOffset.CompareTo(eh.HandlerOffset);
            }

            return -1;
        }

        public override bool Equals(object obj)
        {
            return this.CompareTo(obj) == 0;
        }
    }
}
namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.CSharp.Symbols;

    public class MetadataExceptionHandlingClauseAdapter : IExceptionHandlingClause
    {
        private ExceptionRegion exceptionRegion;
        private TypeSymbol catchType;

        internal MetadataExceptionHandlingClauseAdapter(ExceptionRegion exceptionRegion, TypeSymbol catchType)
        {
            this.exceptionRegion = exceptionRegion;
            this.catchType = catchType;
        }

        public IType CatchType
        {
            get { return new MetadataTypeAdapter(this.catchType); }
        }

        public System.Reflection.ExceptionHandlingClauseOptions Flags
        {
            get { throw new NotImplementedException(); }
        }

        public int TryOffset
        {
            get { return this.exceptionRegion.TryOffset; }
        }

        public int TryLength
        {
            get { return this.exceptionRegion.TryLength; }
        }

        public int HandlerOffset
        {
            get { return this.exceptionRegion.HandlerOffset; }
        }

        public int HandlerLength
        {
            get { return this.exceptionRegion.HandlerLength; }
        }
    }
}

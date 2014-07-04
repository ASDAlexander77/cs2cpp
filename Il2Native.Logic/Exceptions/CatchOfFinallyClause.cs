namespace Il2Native.Logic.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using PEAssemblyReader;

    // if Exception is null means Finally Clause
    public class CatchOfFinallyClause
    {
        private Lazy<List<string>> lazyFinallyJumps = new Lazy<List<string>>(() => new List<string>()); 

        public ExceptionHandlingClauseOptions Flags { get; set; }

        public int Offset { get; set; }

        public int Length { get; set; }

        public IType Catch { get; set; }

        public CatchOfFinallyClause Next { get; set; }

        public TryClause OwnerTry { get; set; }

        public IList<string> FinallyJumps 
        {
            get
            {
                return lazyFinallyJumps.Value;
            }
        }

        public bool FinallyVariablesAreWritten { get; set; }

        public bool RethrowCatchWithCleanUpRequired { get; set; }
    }
}

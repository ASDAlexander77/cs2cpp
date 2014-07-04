namespace Il2Native.Logic.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class TryClause : IComparable
    {
        private Lazy<List<CatchOfFinallyClause>> lazyCatches = new Lazy<List<CatchOfFinallyClause>>(() => new List<CatchOfFinallyClause>()); 

        public int Offset { get; set; }

        public int Length { get; set; }

        public List<CatchOfFinallyClause> Catches 
        {
            get
            {
                return lazyCatches.Value;
            }
        }

        public int CompareTo(TryClause other)
        {
            var cmp = this.Offset.CompareTo(other.Offset);
            if (cmp != 0)
            {
                return cmp;
            }

            return this.Length.CompareTo(other.Length);
        }

        public int CompareTo(object obj)
        {
            var other = obj as TryClause;
            if (other != null)
            {
                return this.CompareTo(other);
            }

            return -1;
        }
    }
}

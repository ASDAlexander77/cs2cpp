namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Il2Native.Logic.CodeParts;

    public class StackBranch
    {
        public StackBranch(int branchStopAddress)
        {
            this.Stack = new Stack<OpCodePart>();
            this.BranchStopAddress = branchStopAddress;
        }

        public StackBranch(int branchStopAddress, StackBranch rootBranch)
            : this(branchStopAddress)
        {
            this.RootBranch = rootBranch.Clone();
        }

        public StackBranch RootBranch
        {
            get;
            private set;
        }

        /// <summary>
        /// </summary>
        public int BranchStopAddress { get; private set; }

        /// <summary>
        /// </summary>
        protected Stack<OpCodePart> Stack { get; private set; }

        public void Push(OpCodePart opCodePart)
        {
            this.Stack.Push(opCodePart);
        }

        public OpCodePart Pop()
        {
            if (this.Stack.Count > 0)
            {
                return this.Stack.Pop();
            }

            if (this.RootBranch != null && this.RootBranch.Any())
            {
                return this.RootBranch.Pop();
            }

            throw new IndexOutOfRangeException();
        }

        public OpCodePart Peek()
        {
            return this.Stack.Peek();
        }

        public OpCodePart First()
        {
            return this.Stack.First();
        }

        public bool Any()
        {
            return this.Stack.Count > 0 || (this.RootBranch != null && this.RootBranch.Any());
        }

        public bool Empty()
        {
            return this.Stack.Count == 0;
        }

        private StackBranch Clone()
        {
            var newBranch = new StackBranch(this.BranchStopAddress);
            newBranch.Stack = new Stack<OpCodePart>(this.Stack);
            newBranch.RootBranch = this.RootBranch != null ? this.RootBranch.Clone() : null;
            return newBranch;
        }
    }
}

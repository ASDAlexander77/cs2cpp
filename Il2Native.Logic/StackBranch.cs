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

        public StackBranch(int branchStopAddress, StackBranch toClone)
            : this(branchStopAddress)
        {
            foreach (var opCodePart in toClone.Stack)
            {
                this.Stack.Push(opCodePart);
            }
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
            return this.Stack.Pop();
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
            return this.Stack.Count > 0;
        }
    }
}

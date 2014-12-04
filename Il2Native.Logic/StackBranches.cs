namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Il2Native.Logic.CodeParts;

    public class StackBranches
    {
        private readonly List<StackBranch> branches = new List<StackBranch>();
        
        private int currentAddress = 0;

        private StackBranch main;

        private StackBranch current;

        public StackBranches()
        {
            this.CreateMainBranch();
        }

        public void CreateNewBranch(int branchEndAddress)
        {
            var newBranch = new StackBranch(branchEndAddress, current);
            this.branches.Add(newBranch);
            this.current = newBranch;
        }

        public void UpdateCurrentAddress(int address)
        {
            this.currentAddress = address;
            this.SwitchStackIfNeeded();
        }

        public void Push(OpCodePart opCodePart)
        {
            this.current.Push(opCodePart);
        }

        public OpCodePart Pop()
        {
            var value = this.current.Pop();

            // read all alternative values from other branches
            if (this.branches.Count > 1 && this.HasAnyNonEmptyClosedBranch())
            {
                var phiNodes = new PhiNodes();
                var any = false;
                foreach (var alternateValue in this.branches.Where(b => b.BranchStopAddress <= this.currentAddress).Select(branch => branch.Pop()))
                {
                    var label = alternateValue.FindBeginOfBasicBlock();
                    Debug.Assert(label.HasValue);
                    if (label.HasValue)
                    {
                        phiNodes.Values.Add(alternateValue);
                        phiNodes.Labels.Add(label.Value);
                        any = true;
                    }
                }

                // current value
                var currentLabel = value.FindBeginOfBasicBlock();
                Debug.Assert(currentLabel.HasValue);
                if (currentLabel.HasValue)
                {
                    phiNodes.Values.Add(value);
                    phiNodes.Labels.Add(currentLabel.Value);
                }

                this.CleanUpBranches();

                if (any)
                {
                    value.AlternativeValues = phiNodes;
                }
            }

            return value;
        }

        public OpCodePart Peek()
        {
            return this.current.Peek();
        }

        public OpCodePart First()
        {
            return this.current.First();
        }

        public bool Any()
        {
            return this.current.Any();
        }

        public void Clear()
        {
            this.branches.Clear();
            this.CreateMainBranch();
        }

        private void SwitchStackIfNeeded()
        {
            if (this.current.BranchStopAddress > this.currentAddress)
            {
                // no need to change stack thread
                return;
            }

            this.CleanUpBranches();

            // set current
            this.current = this.branches.Where(stack => stack.BranchStopAddress > this.currentAddress).OrderBy(stack => stack.BranchStopAddress).First();
        }

        private void CleanUpBranches()
        {
            // remove empty branch
            this.branches.RemoveAll(stack => stack.BranchStopAddress <= this.currentAddress && !stack.Any());
        }

        private void CreateMainBranch()
        {
            var newBranch = new StackBranch(int.MaxValue);
            this.branches.Add(newBranch);
            this.current = this.main = newBranch;
        }

        private bool HasAnyNonEmptyClosedBranch()
        {
            return this.branches.Any(stack => stack.BranchStopAddress <= this.currentAddress && stack.Any());
        }
    }
}

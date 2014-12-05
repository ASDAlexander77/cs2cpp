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
            // read all alternative values from other branches
            if (this.current == this.main && this.HasAnyNonEmptyClosedBranch())
            {
                opCodePart.AlternativeValues = this.GetPhiValues(null);
            }

            this.current.Push(opCodePart);
        }

        public OpCodePart Pop(out PhiNodes alternativeValues)
        {
            alternativeValues = null;
            var value = this.current.Pop();

            // read all alternative values from other branches
            if (this.branches.Count > 1 && this.HasAnyNonEmptyClosedBranch())
            {
                alternativeValues = this.GetPhiValues(value);
            }

            return value;
        }

        private PhiNodes GetPhiValues(OpCodePart value)
        {
            var phiNodes = new PhiNodes();
            foreach (var alternateValue in
                this.branches.Where(b => b.BranchStopAddress <= this.currentAddress).Select(branch => branch.Pop()).Where(alternateValue => alternateValue != null))
            {
                AddPhiValue(phiNodes, alternateValue);
            }

            var hasAnyValue = phiNodes.Values.Any();
            if (hasAnyValue && value != null)
            {
                // current value
                AddPhiValue(phiNodes, value);
            }

            this.CleanUpBranches();

            return hasAnyValue ? phiNodes : null;
        }

        private static void AddPhiValue(PhiNodes phiNodes, OpCodePart value)
        {
            var currentLabel = value.FindBeginOfBasicBlock();
            phiNodes.Values.Add(value);
            if (currentLabel.HasValue)
            {
                phiNodes.Labels.Add(currentLabel.Value);
            }
            else
            {
                phiNodes.Labels.Add(value.GroupAddressStart);

                // we need to create a label
                var opCode = value.OpCodeOperands != null && value.OpCodeOperands.Length > 0 ? value.OpCodeOperands[0] : value;
                if (opCode.JumpDestination == null)
                {
                    opCode.JumpDestination = new List<OpCodePart>();
                }

                opCode.JumpDestination.Add(value);
            }
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

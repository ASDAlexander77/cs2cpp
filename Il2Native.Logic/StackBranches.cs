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

        public void CheckIfNewBranchToCreate(OpCodePart opCode, BaseWriter baseWriter)
        {
            switch (opCode.ToCode())
            {
                case Code.Br:
                case Code.Br_S:
                    if (opCode.IsJumpForward())
                    {
                        this.CreateNewBranch(opCode.JumpAddress());
                    }
                    break;
                case Code.Beq:
                case Code.Beq_S:
                case Code.Blt:
                case Code.Blt_S:
                case Code.Bgt:
                case Code.Bgt_S:
                case Code.Ble:
                case Code.Ble_S:
                case Code.Bge:
                case Code.Bge_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:
                    if (opCode.IsJumpForward()
                        && !opCode.JumpOpCode(baseWriter).Previous.Any(Code.Br, Code.Br_S, Code.Ret, Code.Leave, Code.Leave_S, Code.Endfilter, Code.Endfinally))
                    {
                        this.CreateNewBranch(opCode.JumpAddress());
                    }

                    break;
            }
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
            foreach (var branch in
                this.branches.Where(b => b.BranchStopAddress <= this.currentAddress))
            {
                // to align stack
                branch.Push(null);
            }

            this.current.Push(opCodePart);
        }

        public OpCodePart Pop()
        {
            var value = this.current.Pop();

            // read all alternative values from other branches
            if (this.branches.Count > 1 && this.HasAnyNonEmptyClosedBranch())
            {
                var alternativeValues = this.GetPhiValues(value);
                if (alternativeValues != null)
                {
                    alternativeValues.Values.First().Next.AlternativeValues = alternativeValues;
                }
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
            return this.branches.Any(stack => stack.BranchStopAddress <= this.currentAddress && !stack.Empty());
        }
    }
}

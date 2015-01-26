namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection.Emit;
    using CodeParts;

    public class StackBranches
    {
        private readonly Stack<OpCodePart> main = new Stack<OpCodePart>();

        public bool Any()
        {
            return this.main.Any();
        }

        public void Clear()
        {
            this.main.Clear();
        }

        public OpCodePart First()
        {
            return this.main.First();
        }

        public OpCodePart Peek()
        {
            return this.main.Peek();
        }

        public OpCodePart Pop()
        {
            return this.main.Pop();
        }

        public void ProcessAlternativeValues(OpCodePart opCodePart)
        {
            // read all alternative values from other branches
            if (opCodePart.JumpDestination == null)
            {
                return;
            }

            var previousFlow = opCodePart.Previous != null ? opCodePart.Previous.OpCode.FlowControl : FlowControl.Break;
            var noMainEntry = previousFlow == FlowControl.Branch || previousFlow == FlowControl.Return ||
                              previousFlow == FlowControl.Throw || previousFlow == FlowControl.Break;
            var entires = opCodePart.JumpDestination.Where(opCode => opCode.IsJumpForward());
            var entriesList = entires as IList<OpCodePart> ?? entires.ToList();
            ////if (entriesList.Count() <= (noMainEntry ? 1 : 0))
            if (!entriesList.Any())
            {
                return;
            }

            // TODO: check if alternative stack has the same values then ignore alternative stack
            var valueNumber = 0;
            while (true)
            {
                var values =
                    entriesList.Where(opCode => opCode.BranchStackValue != null && opCode.BranchStackValue.Count > 0)
                        .Select(opCode => opCode.BranchStackValue.Peek());
                if (!values.Any())
                {
                    return;
                }

                var currentStackValue = this.main.Skip(valueNumber).First();
                var alternativeValues = GetPhiValues(values, !noMainEntry ? currentStackValue : null);
                if (alternativeValues == null)
                {
                    return;
                }

                var firstValue = alternativeValues.Values.OrderByDescending(v => v.AddressStart).First();
                if (alternativeValues.Values.Count() == 1)
                {
                    // it just one value, we can push it back to stack
                    if (!this.main.Any() || this.main.All(op => op.AddressStart != firstValue.AddressStart))
                    {
                        this.main.Push(firstValue);
                    }

                    return;
                }

                if (opCodePart.AlternativeValues == null)
                {
                    opCodePart.AlternativeValues = new Queue<PhiNodes>();
                }
                else
                {
                    // if labels are different it means that values belong to other PHI Nodes
                    var firstAlternativeValues = opCodePart.AlternativeValues.First();
                    if (firstAlternativeValues.Labels.Count != alternativeValues.Labels.Count ||
                        alternativeValues.Labels.Where((t, i) => t != firstAlternativeValues.Labels[i]).Any())
                    {
                        return;
                    }
                }

                opCodePart.AlternativeValues.Enqueue(alternativeValues);

                // remove values from branchstack
                foreach (
                    var branchStack in
                        entriesList.Where(
                            opCode => opCode.BranchStackValue != null && opCode.BranchStackValue.Count > 0)
                            .Select(opCode => opCode.BranchStackValue))
                {
                    branchStack.Pop();
                }

                if (noMainEntry)
                {
                    return;
                }

                // we remove all previously added values at line this.main.Push(firstValue); to reinsert it after removing alternative values from cache which is not at top level
                var top = new Stack<OpCodePart>();
                for (var i = 0; i < valueNumber; i++)
                {
                    top.Push(this.main.Pop());
                }

                // we remove all alternative values from cache, they not needed anymore
                while (this.main.Any() && alternativeValues.Values.Contains(this.main.Peek()))
                {
                    this.main.Pop();
                }

                // this is new value
                this.main.Push(firstValue);

                // we insert top back
                for (var i = 0; i < valueNumber; i++)
                {
                    this.main.Push(top.Pop());
                }

                valueNumber++;
            }
        }

        public void Push(OpCodePart opCodePart)
        {
            this.main.Push(opCodePart);
        }

        public void SaveBranchStackValue(OpCodePart opCode, BaseWriter baseWriter)
        {
            switch (opCode.ToCode())
            {
                case Code.Br:
                case Code.Br_S:
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
                    if (opCode.IsJumpForward() && this.main.Any())
                    {
                        // to clone whole stack
                        var clonedStack = this.main.ToList();
                        clonedStack.Reverse();
                        opCode.BranchStackValue = new Stack<OpCodePart>(clonedStack);
                    }

                    break;
            }
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
                // we need to create a label
                var opCode = value.OpCodeOperands != null && value.OpCodeOperands.Length > 0
                    ? value.OpCodeOperands[0]
                    : value;
                if (opCode.JumpDestination == null)
                {
                    opCode.JumpDestination = new List<OpCodePart>();
                }

                opCode.JumpDestination.Add(value);

                phiNodes.Labels.Add(opCode.AddressStart);
            }
        }

        private static PhiNodes GetPhiValues(IEnumerable<OpCodePart> values, OpCodePart currentValue)
        {
            var phiNodes = new PhiNodes();
            var any = false;
            foreach (
                var alternateValue in
                    values.Where(alternateValue => currentValue == null || !alternateValue.Equals(currentValue)))
            {
                AddPhiValue(phiNodes, alternateValue);
                any = true;
            }

            if (any)
            {
                if (currentValue != null)
                {
                    AddPhiValue(phiNodes, currentValue);
                }

                // all values the same - return null
                var firstValueAddressStart = phiNodes.Values.First().AddressStart;
                if (phiNodes.Values.Count() > 1 && phiNodes.Values.All(v => v.AddressStart == firstValueAddressStart))
                {
                    return null;
                }

                return phiNodes;
            }

            return null;
        }
    }
}
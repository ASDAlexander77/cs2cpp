namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Emit;

    using Il2Native.Logic.CodeParts;

    public class StackBranches
    {
        private Stack<OpCodePart> main = new Stack<OpCodePart>();

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
                        opCode.BranchStackValue = this.main.Peek();
                    }

                    break;
            }
        }

        public void Push(OpCodePart opCodePart)
        {
            this.main.Push(opCodePart);
        }

        public void ProcessAlternativeValues(OpCodePart opCodePart)
        {
            // read all alternative values from other branches
            if (opCodePart.JumpDestination == null)
            {
                return;
            }

            var previousFlow = opCodePart.Previous != null ? opCodePart.Previous.OpCode.FlowControl : FlowControl.Break;
            var noMainEntry = previousFlow == FlowControl.Branch || previousFlow == FlowControl.Return || previousFlow == FlowControl.Throw || previousFlow == FlowControl.Break;
            var entires = opCodePart.JumpDestination.Where(opCode => opCode.IsJumpForward());
            var entriesList = entires as IList<OpCodePart> ?? entires.ToList();
            //if (entriesList.Count() <= (noMainEntry ? 1 : 0))
            if (!entriesList.Any())
            {
                return;
            }

            var values = entriesList.Where(opCode => opCode.BranchStackValue != null).Select(opCode => opCode.BranchStackValue);
            if (!values.Any())
            {
                return;
            }

            var alternativeValues = GetPhiValues(values, !noMainEntry ? this.main.Peek() : null);
            if (alternativeValues == null)
            {
                return;
            }

            var firstValue = alternativeValues.Values.OrderByDescending(v => v.AddressStart).First();
            if (alternativeValues.Values.Count() == 1)
            {
                // it just one value, we can push it back to stack
                if (!this.main.Any() || !this.main.Any(op => op.AddressStart == firstValue.AddressStart))
                {
                    this.main.Push(firstValue);
                }

                return;
            }

            opCodePart.AlternativeValues = alternativeValues;

            if (noMainEntry)
            {
                return;
            }

            while (this.main.Any() && alternativeValues.Values.Contains(this.main.Peek()))
            {
                this.main.Pop();
            }

            this.main.Push(firstValue);
        }

        public OpCodePart Pop()
        {
            return this.main.Pop();
        }

        private static PhiNodes GetPhiValues(IEnumerable<OpCodePart> values, OpCodePart currentValue)
        {
            var phiNodes = new PhiNodes();
            var any = false;
            foreach (var alternateValue in values.Where(alternateValue => currentValue == null || !alternateValue.Equals(currentValue)))
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
            return this.main.Peek();
        }

        public OpCodePart First()
        {
            return this.main.First();
        }

        public bool Any()
        {
            return this.main.Any();
        }

        public void Clear()
        {
            this.main.Clear();
        }
    }
}

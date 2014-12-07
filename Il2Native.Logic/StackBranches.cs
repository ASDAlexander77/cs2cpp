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

        public StackBranches()
        {
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
                        opCode.BranchStackValue = this.main.Peek();
                    }

                    break;
            }
        }

        public void Push(OpCodePart opCodePart)
        {
            // read all alternative values from other branches
            if (opCodePart.JumpDestination != null && this.main.Any())
            {
                var previousFlow = opCodePart.Previous.OpCode.FlowControl;
                var noMainEntry = previousFlow == FlowControl.Branch || previousFlow == FlowControl.Return || previousFlow == FlowControl.Throw || previousFlow == FlowControl.Break;
                var entires = opCodePart.JumpDestination.Where(opCode => opCode.IsJumpForward());
                if (entires.Count() > (noMainEntry ? 1 : 0))
                {
                    var values = entires.Where(opCode => opCode.BranchStackValue != null).Select(opCode => opCode.BranchStackValue);
                    var alternativeValues = this.GetPhiValues(values, this.main.Peek());
                    if (alternativeValues != null)
                    {
                        alternativeValues.Values.OrderByDescending(v => v.AddressStart).First().Next.AlternativeValues = alternativeValues;
                    }
                }
            }

            this.main.Push(opCodePart);
        }

        public OpCodePart Pop()
        {
            return this.main.Pop();
        }

        private PhiNodes GetPhiValues(IEnumerable<OpCodePart> values, OpCodePart currentValue)
        {
            var phiNodes = new PhiNodes();
            var any = false;
            foreach (var alternateValue in values)
            {
                if (alternateValue.Equals(currentValue))
                {
                    continue;
                }

                AddPhiValue(phiNodes, alternateValue);
                any = true;
            }

            if (any)
            {
                AddPhiValue(phiNodes, currentValue);
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

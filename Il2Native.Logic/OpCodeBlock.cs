namespace Il2Native.Logic
{
    using System.Linq;
    using Il2Native.Logic.CodeParts;
    
    using OpCodesEmit = System.Reflection.Emit.OpCodes;
    using System.Collections.Generic;

    public class OpCodeBlock : OpCodePart
    {
        public OpCodeBlock(OpCodePart[] opCodes, bool runDetect = true, bool detectOnly = false) : base(OpCodesEmit.Nop, 0, 0)
        {
            this.OpCodes = opCodes;

            if (runDetect)
            {
                if (this.DetectBlock() && !detectOnly)
                {
                    // HACK: improve me by using Try/Catch blocks instead
                    var first = OpCodes.First();
                    if (first.Try != null)
                    {
                        this.Try = first.Try;
                        first.Try = null;
                    }

                    if (first.Cases != null)
                    {
                        this.Cases = first.Cases;
                        first.Cases = null;
                    }

                    if (first.DefaultCase)
                    {
                        this.DefaultCase = true;
                        first.DefaultCase = false;
                    }
                }
            }
        }

        public OpCodePart[] OpCodes { get; private set; }

        public override int GroupAddressStart
        {
            get
            {
                return this.OpCodes[0].GroupAddressStart;
            }
        }

        public override int GroupAddressEnd
        {
            get
            {
                return this.OpCodes[this.OpCodes.Length - 1].GroupAddressEnd;
            }
        }

        private bool DetectBlock()
        {
            if (this.OpCodes == null || this.OpCodes.Length == 0)
            {
                UseAsEmpty = true;              
                return false;
            }

            var first = this.OpCodes.First();

            if (first.UseAsCaseCondition)
            {
                UseAsSwitch = true;
                UseAsIfElseSwitch = first.UseAsIfElseSwitch;
                return false;
            }

            var last = this.OpCodes.Last();

            if (first.IsCondBranch() && !first.UseAsConditionalBreak && !first.UseAsConditionalContinue)
            {
                // to inver condition
                this.OpCodes[0].UseAsIf = true; 

                // to write block as If
                UseAsIf = true;

                // build if conditions
                var list = new List<OpCodePart>();
                var count = 0;
                while (count < this.OpCodes.Length && this.OpCodes[count].UseAsIfWhileForSubCondition)
                {
                    list.Add(this.OpCodes[count]);
                    count++;
                }

                if (count > 0)
                {
                    BaseWriter.ConditionsParseForIf(list.ToArray(), list[list.Count - 1].GroupAddressEnd);
                }

                return true;
            }

            var beforeLastCondition = this.OpCodes.Where(op => !op.UseAsIfWhileForSubCondition).Last();
            var hasContinue = beforeLastCondition != null
                && beforeLastCondition.JumpDestination != null
                && beforeLastCondition.JumpDestination.Any(j => j.UseAsConditionalContinue || j.UseAsContinue);

            if (first.IsBranch() && last.IsCondBranch() && !last.IsJumpForward() && last.JumpAddress() == first.GroupAddressEnd)
            {               
                // to write block as While or For
                if (hasContinue)
                {
                    UseAsFor = true;
                }
                else
                {
                    UseAsWhile = true;
                }

                return false;
            }

            if (first.IsBranch() && first.IsJumpForward())
            {
                // to write block as If
                UseAsElse = true;
                return false;
            }

            if (last.IsBranch() && !last.IsJumpForward())
            {
                UseAsFor = true;
                return true;
            }

            if (last.IsCondBranch() && !last.IsJumpForward())
            {
                if (hasContinue)
                {
                    UseAsFor = true;
                }
                else
                {
                    UseAsDoWhile = true;
                }

                return true;
            }

            return false;
        }
    }
}

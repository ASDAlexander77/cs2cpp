// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodeBlock.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using Il2Native.Logic.CodeParts;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class OpCodeBlock : OpCodePart
    {
        /// <summary>
        /// </summary>
        /// <param name="opCodes">
        /// </param>
        /// <param name="runDetect">
        /// </param>
        /// <param name="detectOnly">
        /// </param>
        public OpCodeBlock(OpCodePart[] opCodes, bool runDetect = true, bool detectOnly = false)
            : base(OpCodesEmit.Nop, 0, 0)
        {
            this.OpCodes = opCodes;

            if (runDetect)
            {
                if (this.DetectBlock() && !detectOnly)
                {
                    // HACK: improve me by using Try/Catch blocks instead
                    OpCodePart first = this.OpCodes.First();
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

        /// <summary>
        /// </summary>
        public OpCodePart[] OpCodes { get; private set; }

        /// <summary>
        /// </summary>
        public override int GroupAddressStart
        {
            get
            {
                return this.OpCodes[0].GroupAddressStart;
            }
        }

        /// <summary>
        /// </summary>
        public override int GroupAddressEnd
        {
            get
            {
                return this.OpCodes[this.OpCodes.Length - 1].GroupAddressEnd;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        private bool DetectBlock()
        {
            if (this.OpCodes == null || this.OpCodes.Length == 0)
            {
                this.UseAsEmpty = true;
                return false;
            }

            OpCodePart first = this.OpCodes.First();

            if (first.UseAsCaseCondition)
            {
                this.UseAsSwitch = true;
                this.UseAsIfElseSwitch = first.UseAsIfElseSwitch;
                return false;
            }

            OpCodePart last = this.OpCodes.Last();

            if (first.IsCondBranch() && !first.UseAsConditionalBreak && !first.UseAsConditionalContinue)
            {
                // to inver condition
                this.OpCodes[0].UseAsIf = true;

                // to write block as If
                this.UseAsIf = true;

                // build if conditions
                var list = new List<OpCodePart>();
                int count = 0;
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

            OpCodePart beforeLastCondition = this.OpCodes.Where(op => !op.UseAsIfWhileForSubCondition).Last();
            bool hasContinue = beforeLastCondition != null && beforeLastCondition.JumpDestination != null
                               && beforeLastCondition.JumpDestination.Any(j => j.UseAsConditionalContinue || j.UseAsContinue);

            if (first.IsBranch() && last.IsCondBranch() && !last.IsJumpForward() && last.JumpAddress() == first.GroupAddressEnd)
            {
                // to write block as While or For
                if (hasContinue)
                {
                    this.UseAsFor = true;
                }
                else
                {
                    this.UseAsWhile = true;
                }

                return false;
            }

            if (first.IsBranch() && first.IsJumpForward())
            {
                // to write block as If
                this.UseAsElse = true;
                return false;
            }

            if (last.IsBranch() && !last.IsJumpForward())
            {
                this.UseAsFor = true;
                return true;
            }

            if (last.IsCondBranch() && !last.IsJumpForward())
            {
                if (hasContinue)
                {
                    this.UseAsFor = true;
                }
                else
                {
                    this.UseAsDoWhile = true;
                }

                return true;
            }

            return false;
        }
    }
}
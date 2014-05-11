namespace Il2Native.Logic.CodeParts
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    [DebuggerDisplay("{OpCode.Name}, {OpCode.FlowControl}, {OpCode.StackBehaviourPop}, {OpCode.StackBehaviourPush}")]
    public class OpCodePart
    {
        private int? resultNumber;
        private System.Type resultType;

        public OpCodePart(OpCode opcode, int addressStart, int addressEnd)
        {
            this.OpCode = opcode;
            this.AddressStart = addressStart;
            this.AddressEnd = addressEnd;
        }

        protected OpCodePart()
        {
        }

        public OpCode OpCode { get; private set; }

        public int AddressStart { get; private set; }

        public int AddressEnd { get; private set; }

        public virtual int GroupAddressStart
        {
            get
            {
                return OpCodeOperands != null && OpCodeOperands.Length > 0 ? OpCodeOperands[0].GroupAddressStart : this.AddressStart;
            }
        }

        public virtual int GroupAddressEnd
        {
            get
            {
                return this.AddressEnd;
            }
        }

        public OpCodePart[] OpCodeOperands { get; set; }

        // to mark Op as used to skip it in write operation
        public bool Skip { get; set; }

        // used to mark that jump for this op is process when blocks builds
        public bool JumpProcessed { get; set; }

        public bool UseAsBoolean { get; set; }

        public bool HasDup
        {
            get
            {
                // todo: fix for 2 bytes command
                if (this.ToCode() == Code.Dup)
                {
                    return true;
                }

                if (this.OpCodeOperands != null)
                {
                    return this.OpCodeOperands.Any(u => u.HasDup);
                }

                return false;
            }
        }

        public HashSet<int> Try { get; set; }

        public HashSet<int> EndOfTry { get; set; }

        public List<ExceptionHandlingClause> ExceptionHandlers { get; set; }

        public HashSet<int> EndOfClausesOrFinal { get; set; }

        public List<OpCodePart> Cases { get; set; }

        public bool DefaultCase { get; set; }

        public List<OpCodePart> JumpDestination { get; set; }

        public bool InvertCondition { get; set; }

        public bool ConjunctionAndCondition { get; set; }

        public bool ConjunctionOrCondition { get; set; }

        public int OpenRoundBrackets { get; set; }

        public int CloseRoundBrackets { get; set; }

        public bool UseAsEmpty { get; set; }

        public bool UseAsIf { get; set; }

        public bool UseAsIfWhileForSubCondition { get; set; }

        public bool UseAsElse { get; set; }

        public bool UseAsFor { get; set; }

        public bool UseAsDoWhile { get; set; }

        public bool UseAsWhile { get; set; }

        public bool UseAsBreak { get; set; }

        public bool UseAsConditionalBreak { get; set; }

        public bool UseAsContinue { get; set; }

        public bool UseAsConditionalContinue { get; set; }

        public bool UseAsConditionalExpression { get; set; }

        public bool UseAsNullCoalescingExpression { get; set; }

        public bool UseAsLeadingIncDecExpression { get; set; }

        public bool UseAsIncDecExpression { get; set; }

        public bool UseAsSwitch { get; set; }

        public bool UseAsIfElseSwitch { get; set; }

        public bool UseAsCaseCondition { get; set; }

        public bool UseAsCaseBreak { get; set; }

        public bool DupProcessedOnce { get; set; }

        public bool ReadExceptionFromStack { get; set; }

        public int? ResultNumber
        {
            get
            {
                if (this.Any(Code.Dup))
                {
                    return this.OpCodeOperands[0].ResultNumber;
                }

                return this.resultNumber;
            }
            set
            {
                this.resultNumber = value;
            }
        }

        public System.Type ResultType
        {
            get
            {
                if (this.Any(Code.Dup))
                {
                    return this.OpCodeOperands[0].ResultType;
                }

                return this.resultType;
            }
            set
            {
                this.resultType = value;
            }
        }

        public string DestinationName { get; set; }

        public System.Type DestinationType { get; set; }

        public OpCodeBlock BeforeBlock { get; set; }
    }
}

namespace Il2Native.Logic.CodeParts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("{OpCode.Name}, {OpCode.FlowControl}, {OpCode.StackBehaviourPop}, {OpCode.StackBehaviourPush}")]
    public class OpCodePart
    {
        #region Fields

        /// <summary>
        /// </summary>
        private int? resultNumber;

        /// <summary>
        /// </summary>
        private IType resultType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="opcode">
        /// </param>
        /// <param name="addressStart">
        /// </param>
        /// <param name="addressEnd">
        /// </param>
        public OpCodePart(OpCode opcode, int addressStart, int addressEnd)
        {
            this.OpCode = opcode;
            this.AddressStart = addressStart;
            this.AddressEnd = addressEnd;
        }

        /// <summary>
        /// </summary>
        protected OpCodePart()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public int AddressEnd { get; private set; }

        /// <summary>
        /// </summary>
        public int AddressStart { get; private set; }

        /// <summary>
        /// </summary>
        public List<OpCodePart> Cases { get; set; }

        /// <summary>
        /// </summary>
        public int CloseRoundBrackets { get; set; }

        /// <summary>
        /// </summary>
        public bool ConjunctionAndCondition { get; set; }

        /// <summary>
        /// </summary>
        public bool ConjunctionOrCondition { get; set; }

        /// <summary>
        /// </summary>
        public bool DefaultCase { get; set; }

        /// <summary>
        /// </summary>
        public string DestinationName { get; set; }

        /// <summary>
        /// </summary>
        public IType DestinationType { get; set; }

        /// <summary>
        /// </summary>
        public bool DupProcessedOnce { get; set; }

        /// <summary>
        /// </summary>
        public HashSet<int> EndOfClausesOrFinal { get; set; }

        /// <summary>
        /// </summary>
        public HashSet<int> EndOfTry { get; set; }

        /// <summary>
        /// </summary>
        public IList<IExceptionHandlingClause> ExceptionHandlers { get; set; }

        /// <summary>
        /// </summary>
        public virtual int GroupAddressEnd
        {
            get
            {
                return this.AddressEnd;
            }
        }

        /// <summary>
        /// </summary>
        public virtual int GroupAddressStart
        {
            get
            {
                return this.OpCodeOperands != null && this.OpCodeOperands.Length > 0 ? this.OpCodeOperands[0].GroupAddressStart : this.AddressStart;
            }
        }

        /// <summary>
        /// </summary>
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

        /// <summary>
        /// </summary>
        public bool InvertCondition { get; set; }

        /// <summary>
        /// </summary>
        public List<OpCodePart> JumpDestination { get; set; }

        /// <summary>
        /// </summary>
        public bool JumpProcessed { get; set; }

        /// <summary>
        /// </summary>
        public OpCode OpCode { get; private set; }

        /// <summary>
        /// </summary>
        public OpCodePart[] OpCodeOperands { get; set; }

        /// <summary>
        /// </summary>
        public int OpenRoundBrackets { get; set; }

        /// <summary>
        /// </summary>
        public bool ReadExceptionFromStack { get; set; }

        /// <summary>
        /// </summary>
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

        /// <summary>
        /// </summary>
        public IType ResultType
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

        /// <summary>
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// </summary>
        public HashSet<int> Try { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsBoolean { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsBreak { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsCaseBreak { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsCaseCondition { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsConditionalBreak { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsConditionalContinue { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsConditionalExpression { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsContinue { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsDoWhile { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsElse { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsEmpty { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsFor { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsIf { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsIfElseSwitch { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsIfWhileForSubCondition { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsIncDecExpression { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsLeadingIncDecExpression { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsNullCoalescingExpression { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsSwitch { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsWhile { get; set; }

        #endregion
    }
}
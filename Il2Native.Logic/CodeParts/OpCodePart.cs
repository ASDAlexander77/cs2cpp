// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpCodePart.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic.CodeParts
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Emit;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("{OpCode.Name}, {OpCode.FlowControl}, {OpCode.StackBehaviourPop}, {OpCode.StackBehaviourPush}")]
    public class OpCodePart
    {
        /// <summary>
        /// </summary>
        private LlvmResult result;

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

        /// <summary>
        /// </summary>
        public static OpCodePart CreateNop
        {
            get
            {
                return new OpCodePart(OpCodesEmit.Nop, 0, 0);
            }
        }

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
        public HashSet<IExceptionHandlingClause> CatchOrFinallyEnd { get; set; }

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
        public bool HasResult
        {
            get
            {
                return this.Result != null;
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
        public LlvmResult Result
        {
            get
            {
                if (this.result != null)
                {
                    return this.result;
                }

                if (this.Any(Code.Dup))
                {
                    return this.OpCodeOperands[0].Result;
                }

                return this.result;
            }

            set
            {
                this.result = value;
            }
        }

        /// <summary>
        /// </summary>
        public bool Skip { get; set; }

        /// <summary>
        /// </summary>
        public HashSet<IExceptionHandlingClause> TryBegin { get; set; }

        /// <summary>
        /// </summary>
        public HashSet<IExceptionHandlingClause> TryEnd { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsBoolean { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsConditionalExpression { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsEmpty { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsNullCoalescingExpression { get; set; }
    }
}
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Emit;

    using Il2Native.Logic.Exceptions;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("{OpCode.Name}, {OpCode.FlowControl}, {OpCode.StackBehaviourPop}, {OpCode.StackBehaviourPush}")]
    public class OpCodePart
    {
        /// <summary>
        /// </summary>
        private FullyDefinedReference result;

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
        public CatchOfFinallyClause CatchOrFinallyBegin { get; set; }

        /// <summary>
        /// </summary>
        public CatchOfFinallyClause CatchOrFinallyEnd { get; set; }

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
        public int? CustomJumpAddress { get; set; }

        /// <summary>
        /// </summary>
        public string CreatedLabel { get; set; }

        /// <summary>
        /// </summary>
        public bool DefaultCase { get; set; }

        /// <summary>
        /// </summary>
        public FullyDefinedReference Destination { get; set; }

        /// <summary>
        /// </summary>
        public bool DupProcessedOnce { get; set; }

        /// <summary>
        /// </summary>
        public IList<CatchOfFinallyClause> ExceptionHandlers { get; set; }

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
                return this.OpCodeOperands != null && this.OpCodeOperands.Length > 0 && this.OpCodeOperands[0] != this
                           ? this.OpCodeOperands[0].GroupAddressStart
                           : this.AddressStart;
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
        public IType ReadExceptionFromStackType { get; set; }

        /// <summary>
        /// </summary>
        public FullyDefinedReference Result
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

        public bool Skip { get; set; }

        /// <summary>
        /// </summary>
        public bool SkipRecursive
        {
            get
            {
                if (this.Skip)
                {
                    return true;
                }

                if (this.UsedBy != null)
                {
                    return this.UsedBy.SkipRecursive;
                }

                return false;
            }
        }

        /// <summary>
        /// </summary>
        public List<TryClause> TryBegin { get; set; }

        /// <summary>
        /// </summary>
        public TryClause TryEnd { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsBoolean { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsConditionalExpression { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsNull { get; set; }

        /// <summary>
        /// </summary>
        public bool UseAsNullCoalescingExpression { get; set; }

        /// <summary>
        /// </summary>
        public OpCodePart UsedBy { get; set; }
    }
}
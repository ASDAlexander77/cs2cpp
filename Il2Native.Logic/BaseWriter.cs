// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseWriter.cs" company="">
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class BaseWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        public BaseWriter()
        {
            this.StaticConstructors = new List<IConstructor>();
            this.Ops = new List<OpCodePart>();
            this.Stack = new Stack<OpCodePart>();
            this.OpsByGroupAddressStart = new SortedDictionary<int, OpCodePart>();
            this.OpsByGroupAddressEnd = new SortedDictionary<int, OpCodePart>();
            this.OpsByAddressStart = new SortedDictionary<int, OpCodePart>();
            this.OpsByAddressEnd = new SortedDictionary<int, OpCodePart>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// </summary>
        public IDictionary<int, OpCodePart> OpsByAddressEnd { get; private set; }

        /// <summary>
        /// </summary>
        public IDictionary<int, OpCodePart> OpsByAddressStart { get; private set; }

        /// <summary>
        /// </summary>
        public IDictionary<int, OpCodePart> OpsByGroupAddressEnd { get; private set; }

        /// <summary>
        /// </summary>
        public IDictionary<int, OpCodePart> OpsByGroupAddressStart { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// </summary>
        protected IExceptionHandlingClause[] ExceptionHandlingClauses { get; private set; }

        /// <summary>
        /// </summary>
        protected IType[] GenericMethodArguments { get; private set; }

        /// <summary>
        /// </summary>
        protected bool HasMethodThis { get; private set; }

        /// <summary>
        /// </summary>
        protected bool IsInterface { get; set; }

        /// <summary>
        /// </summary>
        protected ILocalVariable[] LocalInfo { get; private set; }

        /// <summary>
        /// </summary>
        protected bool[] LocalInfoUsed { get; private set; }

        /// <summary>
        /// </summary>
        protected IMethod MainMethod { get; set; }

        /// <summary>
        /// </summary>
        protected IType MethodReturnType { get; private set; }

        /// <summary>
        /// </summary>
        protected bool NoBody { get; private set; }

        /// <summary>
        /// </summary>
        protected List<OpCodePart> Ops { get; private set; }

        /// <summary>
        /// </summary>
        protected IParam[] Parameters { get; private set; }

        /// <summary>
        /// </summary>
        protected Stack<OpCodePart> Stack { get; private set; }

        /// <summary>
        /// </summary>
        protected List<IConstructor> StaticConstructors { get; set; }

        /// <summary>
        /// </summary>
        protected IType ThisType { get; private set; }

        /// <summary>
        /// </summary>
        protected IType[] TypeGenericArguments { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="conditions">
        /// </param>
        /// <param name="startOfTrueExpression">
        /// </param>
        public static void ConditionsParseForConditionalExpression(OpCodePart[] conditions, int startOfTrueExpression)
        {
            bool nextJoinAnd = true;
            OpCodePart[][] groups = BuildConditionGroups(conditions);
            foreach (var group in groups)
            {
                // all Or
                if (group.Last().JumpAddress() == startOfTrueExpression && group.First().JumpAddress() == startOfTrueExpression)
                {
                    foreach (OpCodePart element in group)
                    {
                        element.ConjunctionOrCondition = true;
                    }

                    nextJoinAnd = true;
                    continue;
                }

                // TODO: var r1 = ok == 1 && ok == 2 || error == 3 && error == 4 && (ok == 10 || ok == 11 || ok ==12) ? 1 : 0;
                // in this expression last  OR chain is not detected
                bool internalAndJoin = group.Last().JumpAddress() == startOfTrueExpression;
                if (internalAndJoin)
                {
                    foreach (OpCodePart element in group)
                    {
                        element.InvertCondition = true;
                        element.ConjunctionAndCondition = true;
                    }

                    group.Last().InvertCondition = false;
                }
                else
                {
                    group[0].OpenRoundBrackets++;

                    foreach (OpCodePart element in group)
                    {
                        element.ConjunctionOrCondition = true;
                    }

                    group.Last().InvertCondition = true;
                    group.Last().CloseRoundBrackets++;
                }

                if (nextJoinAnd)
                {
                    group[0].ConjunctionAndCondition = true;
                    group[0].ConjunctionOrCondition = false;
                }
                else
                {
                    group[0].ConjunctionAndCondition = false;
                    group[0].ConjunctionOrCondition = true;
                }

                nextJoinAnd = !internalAndJoin;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="conditions">
        /// </param>
        /// <param name="startOfTrueExpression">
        /// </param>
        public static void ConditionsParseForIf(OpCodePart[] conditions, int startOfTrueExpression)
        {
            bool nextJoinAnd = true;
            OpCodePart[][] groups = BuildConditionGroups(conditions);
            foreach (var group in groups)
            {
                // all Or
                if (group.Last().JumpAddress() != startOfTrueExpression && group.First().JumpAddress() != startOfTrueExpression)
                {
                    foreach (OpCodePart element in group)
                    {
                        element.InvertCondition = true;
                        element.ConjunctionAndCondition = true;
                    }

                    nextJoinAnd = false;
                    continue;
                }

                // TODO: var r1 = ok == 1 && ok == 2 || error == 3 && error == 4 && (ok == 10 || ok == 11 || ok ==12) ? 1 : 0;
                // in this expression last  OR chain is not detected
                bool internalAndJoin = group.Last().JumpAddress() == startOfTrueExpression;
                if (internalAndJoin)
                {
                    foreach (OpCodePart element in group)
                    {
                        element.ConjunctionAndCondition = true;
                    }

                    group.Last().InvertCondition = true;
                }
                else
                {
                    foreach (OpCodePart element in group)
                    {
                        element.InvertCondition = false;
                        element.ConjunctionOrCondition = true;
                    }

                    group.Last().InvertCondition = true;
                }

                if (nextJoinAnd)
                {
                    group[0].ConjunctionAndCondition = true;
                    group[0].ConjunctionOrCondition = false;
                }
                else
                {
                    group[0].ConjunctionAndCondition = false;
                    group[0].ConjunctionOrCondition = true;
                }

                nextJoinAnd = !internalAndJoin;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="oper1">
        /// </param>
        /// <returns>
        /// </returns>
        public bool IsThis(OpCodePart oper1)
        {
            bool isThis = oper1.Any(Code.Ldarg_0) && this.HasMethodThis;
            return isThis;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        public ReturnResult ResultOf(OpCodePart opCode)
        {
            Code code = opCode.ToCode();
            switch (code)
            {
                case Code.Call:
                case Code.Callvirt:
                    var methodBase = (opCode as OpCodeMethodInfoPart).Operand;
                    return new ReturnResult(methodBase.ReturnType);
                case Code.Newobj:
                    var ctorInfo = (opCode as OpCodeConstructorInfoPart).Operand;
                    return new ReturnResult(ctorInfo.DeclaringType);
                case Code.Ldfld:
                case Code.Ldsfld:
                    var fieldInfo = (opCode as OpCodeFieldInfoPart).Operand;
                    return new ReturnResult(fieldInfo.FieldType);
                case Code.Add:
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                case Code.Mul:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                case Code.Sub:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                case Code.Div:
                case Code.Div_Un:
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
                case Code.Rem:
                case Code.Rem_Un:
                    return this.ResultOf(opCode.OpCodeOperands[0]);
                case Code.Ceq:
                case Code.Cgt:
                case Code.Cgt_Un:
                case Code.Clt:
                case Code.Clt_Un:
                    return new ReturnResult(typeof(bool));
                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                    return new ReturnResult(typeof(int));
                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                    return new ReturnResult(typeof(uint));
                case Code.Conv_R_Un:
                case Code.Conv_R4:
                    return new ReturnResult(typeof(float));
                case Code.Conv_R8:
                    return new ReturnResult(typeof(double));
                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                    return new ReturnResult(typeof(sbyte));
                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                    return new ReturnResult(typeof(short));
                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                    return new ReturnResult(typeof(int));
                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                    return new ReturnResult(typeof(long));
                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                    return new ReturnResult(typeof(byte));
                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                    return new ReturnResult(typeof(ushort));
                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                    return new ReturnResult(typeof(uint));
                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    return new ReturnResult(typeof(ulong));
                case Code.Castclass:
                    return new ReturnResult((opCode as OpCodeTypePart).Operand);
                case Code.Ret:
                case Code.Neg:
                case Code.Not:
                case Code.Dup:
                    return this.ResultOf(opCode.OpCodeOperands[0]);
                case Code.Ldlen:
                    return new ReturnResult(typeof(int));
                case Code.Ldloca:
                case Code.Ldloca_S:
                    IType localVarType = this.LocalInfo[(opCode as OpCodeInt32Part).Operand].LocalType;
                    return new ReturnResult(localVarType) { IsAddress = true };
                case Code.Ldloc:
                case Code.Ldloc_S:
                    localVarType = this.LocalInfo[(opCode as OpCodeInt32Part).Operand].LocalType;
                    return new ReturnResult(localVarType);
                case Code.Ldloc_0:
                    localVarType = this.LocalInfo[0].LocalType;
                    return new ReturnResult(localVarType);
                case Code.Ldloc_1:
                    localVarType = this.LocalInfo[1].LocalType;
                    return new ReturnResult(localVarType);
                case Code.Ldloc_2:
                    localVarType = this.LocalInfo[2].LocalType;
                    return new ReturnResult(localVarType);
                case Code.Ldloc_3:
                    localVarType = this.LocalInfo[3].LocalType;
                    return new ReturnResult(localVarType);
                case Code.Ldarg:
                case Code.Ldarg_S:
                    return new ReturnResult(this.Parameters[(opCode as OpCodeInt32Part).Operand - (this.HasMethodThis ? 1 : 0)].ParameterType);
                case Code.Ldarg_0:
                    return new ReturnResult(this.HasMethodThis ? this.ThisType : this.Parameters[0].ParameterType);
                case Code.Ldarg_1:
                    return new ReturnResult(this.Parameters[this.HasMethodThis ? 0 : 1].ParameterType);
                case Code.Ldarg_2:
                    return new ReturnResult(this.Parameters[this.HasMethodThis ? 1 : 2].ParameterType);
                case Code.Ldarg_3:
                    return new ReturnResult(this.Parameters[this.HasMethodThis ? 2 : 3].ParameterType);
                case Code.Ldarga:
                case Code.Ldarga_S:
                    var result = new ReturnResult(this.Parameters[(opCode as OpCodeInt32Part).Operand - (this.HasMethodThis ? 1 : 0)].ParameterType);
                    result.IsAddress = true;
                    return result;
                case Code.Ldelem:
                case Code.Ldelem_I:
                case Code.Ldelem_I1:
                case Code.Ldelem_I2:
                case Code.Ldelem_I4:
                case Code.Ldelem_I8:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_U1:
                case Code.Ldelem_U2:
                case Code.Ldelem_U4:

                    result = this.ResultOf(opCode.OpCodeOperands[0]);

                    // we are loading address of item of the array so we need to return type of element not the type of the array
                    return new ReturnResult(result.IType.GetElementType());
                case Code.Ldelem_Ref:
                    result = this.ResultOf(opCode.OpCodeOperands[0]) ?? new ReturnResult((IType)null);
                    result.IsReference = true;
                    return result;
                case Code.Ldelema:
                    result = this.ResultOf(opCode.OpCodeOperands[0]);

                    // we are loading address of item of the array so we need to return type of element not the type of the array
                    return new ReturnResult(result.IType.GetElementType()) { IsAddress = true };
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                case Code.Ldc_I4_M1:
                case Code.Ldc_I4:
                case Code.Ldc_I4_S:
                    return new ReturnResult(opCode.UseAsBoolean ? typeof(bool) : typeof(int)) { IsConst = true };
                case Code.Ldc_I8:
                    return new ReturnResult(typeof(long)) { IsConst = true };
                case Code.Ldc_R4:
                    return new ReturnResult(typeof(float)) { IsConst = true };
                case Code.Ldc_R8:
                    return new ReturnResult(typeof(double)) { IsConst = true };
                case Code.Ldstr:
                    return new ReturnResult(typeof(string));
                case Code.Ldind_I:
                    return new ReturnResult(typeof(int)) { IsIndirect = true };
                case Code.Ldind_I1:
                    return new ReturnResult(typeof(byte)) { IsIndirect = true };
                case Code.Ldind_I2:
                    return new ReturnResult(typeof(short)) { IsIndirect = true };
                case Code.Ldind_I4:
                    return new ReturnResult(typeof(int)) { IsIndirect = true };
                case Code.Ldind_I8:
                    return new ReturnResult(typeof(long)) { IsIndirect = true };
                case Code.Ldind_U1:
                    return new ReturnResult(typeof(byte)) { IsIndirect = true };
                case Code.Ldind_U2:
                    return new ReturnResult(typeof(ushort)) { IsIndirect = true };
                case Code.Ldind_U4:
                    return new ReturnResult(typeof(uint)) { IsIndirect = true };
                case Code.Ldind_R4:
                    return new ReturnResult(typeof(float)) { IsIndirect = true };
                case Code.Ldind_R8:
                    return new ReturnResult(typeof(double)) { IsIndirect = true };
                case Code.Ldind_Ref:
                    return new ReturnResult(this.ResultOf(opCode.OpCodeOperands[0]).IType.GetElementType()) { IsIndirect = true, IsReference = true };
                case Code.Ldflda:
                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    return new ReturnResult(opCodeFieldInfoPart.Operand.FieldType) { IsField = true, IsAddress = true };
                case Code.Ldobj:
                    var opCodeTypePart = opCode as OpCodeTypePart;
                    return new ReturnResult(opCodeTypePart.Operand);
                case Code.Box:

                    // TODO: call .KeyedCollection`2, Method ContainsItem have a problem with Box and Stloc.1
                    ReturnResult res = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (res != null)
                    {
                        return new ReturnResult(res.IType) { Boxed = true };
                    }
                    else
                    {
                        return null;
                    }
            }

            var opCodeBlock = opCode as OpCodeBlock;
            if (opCodeBlock != null)
            {
                if (opCodeBlock.UseAsIncDecExpression)
                {
                    return this.ResultOf(opCodeBlock.OpCodes[0]);
                }

                if (opCodeBlock.UseAsLeadingIncDecExpression)
                {
                    return this.ResultOf(opCodeBlock.OpCodes[0]);
                }
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        protected void AdjustTypes(OpCodePart opCode)
        {
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            OpCodePart usedOpCode1 = opCode.OpCodeOperands[0];
            if (usedOpCode1 == null)
            {
                // todo: should not be here
                return;
            }

            // fix types
            ReturnResult requiredType = this.RequiredType(opCode);
            if (requiredType != null)
            {
                ReturnResult receivingType = this.ResultOf(usedOpCode1);
                if (requiredType != receivingType)
                {
                    if (requiredType.IsTypeOf(typeof(bool)) && usedOpCode1.Any(Code.Ldc_I4_0, Code.Ldc_I4_1))
                    {
                        usedOpCode1.UseAsBoolean = true;
                        return;
                    }
                }
            }

            if (opCode.OpCodeOperands.Length == 2 && opCode.OpCode.StackBehaviourPop == StackBehaviour.Pop1_pop1
                && (opCode.OpCode.StackBehaviourPush == StackBehaviour.Push1 || opCode.OpCode.StackBehaviourPush == StackBehaviour.Pushi))
            {
                // types should be equal
                OpCodePart usedOpCode2 = opCode.OpCodeOperands[1];
                Code usedCode2 = usedOpCode2.ToCode();

                ReturnResult type1 = this.ResultOf(usedOpCode1);
                ReturnResult type2 = this.ResultOf(usedOpCode2);

                if (type1 != null && type2 != null && type1 != type2)
                {
                    if (type1.IsTypeOf(typeof(bool)) && usedOpCode2.Any(Code.Ldc_I4_0, Code.Ldc_I4_1))
                    {
                        usedOpCode2.UseAsBoolean = true;
                        return;
                    }

                    if (type2.IsTypeOf(typeof(bool)) && usedOpCode1.Any(Code.Ldc_I4_0, Code.Ldc_I4_1))
                    {
                        usedOpCode1.UseAsBoolean = true;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        protected void AssignExceptionsToOpCodes()
        {
            if (this.ExceptionHandlingClauses == null)
            {
                return;
            }

            foreach (var exceptionHandlingClause in this.ExceptionHandlingClauses)
            {
                OpCodePart opCodePart;
                if (this.OpsByGroupAddressStart.TryGetValue(exceptionHandlingClause.TryOffset, out opCodePart))
                {
                    if (opCodePart.Try == null)
                    {
                        opCodePart.Try = new HashSet<int>();
                    }

                    opCodePart.Try.Add(exceptionHandlingClause.TryOffset + exceptionHandlingClause.TryLength);
                }

                if (this.OpsByGroupAddressEnd.TryGetValue(exceptionHandlingClause.TryOffset + exceptionHandlingClause.TryLength, out opCodePart))
                {
                    if (opCodePart.EndOfTry == null)
                    {
                        opCodePart.EndOfTry = new HashSet<int>();
                    }

                    opCodePart.EndOfTry.Add(exceptionHandlingClause.TryOffset + exceptionHandlingClause.TryLength);
                }

                if (this.OpsByGroupAddressEnd.TryGetValue(exceptionHandlingClause.HandlerOffset + exceptionHandlingClause.HandlerLength, out opCodePart))
                {
                    if (opCodePart.EndOfClausesOrFinal == null)
                    {
                        opCodePart.EndOfClausesOrFinal = new HashSet<int>();
                    }

                    opCodePart.EndOfClausesOrFinal.Add(
                        exceptionHandlingClause.TryOffset + exceptionHandlingClause.TryLength + exceptionHandlingClause.HandlerOffset
                        + exceptionHandlingClause.HandlerLength);
                }

                if (this.OpsByGroupAddressEnd.TryGetValue(exceptionHandlingClause.HandlerOffset, out opCodePart))
                {
                    if (opCodePart.ExceptionHandlers == null)
                    {
                        opCodePart.ExceptionHandlers = new List<IExceptionHandlingClause>();
                    }

                    opCodePart.ExceptionHandlers.Add(exceptionHandlingClause);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodes">
        /// </param>
        protected void AssignJumpBlocks(OpCodePart[] opCodes)
        {
            foreach (var opCodePart in opCodes)
            {
                if (opCodePart.IsAnyBranch())
                {
                    var jumpOp = opCodePart as OpCodeInt32Part;
                    if (jumpOp != null)
                    {
                        var nextAddress = opCodePart.JumpAddress();
                        OpCodePart target = this.OpsByAddressStart[nextAddress];
                        if (target.JumpDestination == null)
                        {
                            target.JumpDestination = new List<OpCodePart>();
                        }

                        target.JumpDestination.Add(jumpOp);

                        continue;
                    }

                    var switchOp = opCodePart as OpCodeLabelsPart;
                    if (switchOp != null)
                    {
                        var index = 0;
                        foreach (var jumpAddress in switchOp.Operand)
                        {
                            var nextAddress = switchOp.JumpAddress(index);
                            var target = this.OpsByAddressStart[nextAddress];
                            if (target.JumpDestination == null)
                            {
                                target.JumpDestination = new List<OpCodePart>();
                            }

                            target.JumpDestination.Add(switchOp);

                            index++;
                        }

                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodes">
        /// </param>
        protected void BuildGroupAddressIndexes(OpCodePart[] opCodes)
        {
            this.OpsByGroupAddressStart.Clear();
            this.OpsByGroupAddressEnd.Clear();

            foreach (var opCodePart in opCodes)
            {
                if (!this.OpsByGroupAddressStart.ContainsKey(opCodePart.GroupAddressStart))
                {
                    this.OpsByGroupAddressStart[opCodePart.GroupAddressStart] = opCodePart;
                }

                if (!this.OpsByGroupAddressEnd.ContainsKey(opCodePart.GroupAddressEnd))
                {
                    this.OpsByGroupAddressEnd[opCodePart.GroupAddressEnd] = opCodePart;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="size">
        /// </param>
        protected void FoldNestedOpCodes(OpCodePart opCodePart, int size)
        {
            if (this.Stack.Count == 0)
            {
                return;
            }

            List<OpCodePart> insertBack = null;

            var opCodeParts = new OpCodePart[size];

            for (var i = 1; i <= size; i++)
            {
                OpCodePart opCodePartUsed = this.Stack.Pop();

                if (opCodePartUsed.ToCode() == Code.Nop)
                {
                    if (insertBack == null)
                    {
                        insertBack = new List<OpCodePart>();
                    }

                    insertBack.Add(opCodePartUsed);
                    i--;
                    continue;
                }

                if (opCodePart.ToCode() == Code.Ret && this.MethodReturnType == null)
                {
                    opCodeParts = this.RemoveUnusedOps(size, opCodeParts, i, opCodePartUsed);
                    break;
                }

                if (opCodePartUsed.ToCode() == Code.Dup && !opCodePartUsed.DupProcessedOnce)
                {
                    opCodePartUsed.DupProcessedOnce = true;

                    if (insertBack == null)
                    {
                        insertBack = new List<OpCodePart>();
                    }

                    insertBack.Add(opCodePartUsed);
                }

                if (opCodePartUsed.Any(Code.Leave, Code.Leave_S))
                {
                    opCodeParts = this.RemoveUnusedOps(size, opCodeParts, i + 1, opCodePartUsed);

                    if (insertBack == null)
                    {
                        insertBack = new List<OpCodePart>();
                    }

                    var opCodeNope = new OpCodePart(OpCodesEmit.Nop, opCodePart.AddressEnd + 1, opCodePart.AddressEnd + 1);
                    opCodeNope.ReadExceptionFromStack = true;
                    opCodePartUsed = opCodeNope;
                }
                else if (opCodePartUsed.OpCode.StackBehaviourPush == StackBehaviour.Push0
                          || opCodePartUsed.OpCode.StackBehaviourPush == StackBehaviour.Varpush && opCodePartUsed is OpCodeMethodInfoPart
                          && (((OpCodeMethodInfoPart)opCodePartUsed).Operand as IMethod).ReturnType.IsVoid())
                {
                    if (insertBack == null)
                    {
                        insertBack = new List<OpCodePart>();
                    }

                    insertBack.Add(opCodePartUsed);
                    i--;
                    continue;

                    // opCodeParts = RemoveUnusedOps(size, opCodeParts, i, opCodePartUsed);
                    // break;
                }
                else if (opCodePartUsed.OpCode.StackBehaviourPush == StackBehaviour.Varpush)
                {
                    var opCodeMethodPartUsed = opCodePartUsed as OpCodeMethodInfoPart;
                    if (opCodeMethodPartUsed != null && opCodeMethodPartUsed.Operand.IsConstructor)
                    {
                        opCodeParts = this.RemoveUnusedOps(size, opCodeParts, i, opCodePartUsed);
                        break;
                    }
                }

                // check here if you have conditional argument (cond) ? a1 : b1;
                int sizeOfCondition = 0;
                OpCodePart firstCondition = null;
                OpCodePart branchJumpCondition = null;
                while (this.IsConditionalExpression(opCodePart, opCodePartUsed, this.Stack, out sizeOfCondition, out firstCondition, out branchJumpCondition))
                {
                    var newBlockOps = new List<OpCodePart>();
                    newBlockOps.Add(opCodePartUsed);
                    for (int k = 0; k < sizeOfCondition; k++)
                    {
                        newBlockOps.Add(this.Stack.Pop());
                    }

                    // because it is used you do not need to process it twice
                    foreach (var opCode in newBlockOps)
                    {
                        opCode.Skip = true;
                    }

                    newBlockOps.Reverse();

                    var opCodeBlock = new OpCodeBlock(
                        this.ConditionalExpressionConditionsParse(newBlockOps.ToArray(), firstCondition, branchJumpCondition), false);
                    opCodeBlock.UseAsConditionalExpression = true;
                    opCodePartUsed = opCodeBlock;
                }

                // ?? - test condition default expression
                if (this.IsNullCoalescingExpression(opCodePart, opCodePartUsed, this.Stack))
                {
                    var newBlockOps = new List<OpCodePart>();
                    newBlockOps.Add(opCodePartUsed);
                    for (int k = 0; k < 3; k++)
                    {
                        newBlockOps.Add(this.Stack.Pop());
                    }

                    newBlockOps.Reverse();

                    var opCodeBlock = new OpCodeBlock(newBlockOps.ToArray(), false);
                    opCodeBlock.UseAsNullCoalescingExpression = true;
                    opCodePartUsed = opCodeBlock;
                }

                opCodeParts[size - i] = opCodePartUsed;
            }

            opCodePart.OpCodeOperands = opCodeParts;

            // respore stack for not used OpCodes
            if (insertBack != null)
            {
                insertBack.Reverse();
                foreach (var pushBack in insertBack)
                {
                    this.Stack.Push(pushBack);
                }
            }

            this.AdjustTypes(opCodePart);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        protected OpCodePart[] PrepareWritingMethodBody()
        {
            OpCodePart[] rest = this.Stack.Reverse().ToArray();

            this.BuildGroupAddressIndexes(rest);
            this.AssignExceptionsToOpCodes();
            this.AssignJumpBlocks(rest);

            return this.Ops.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        protected void Process(OpCodePart opCode)
        {
            this.Ops.Add(opCode);

            this.AddAddressIndex(opCode);

            var code = opCode.ToCode();
            switch (code)
            {
                case Code.Call:
                    var methodBase = (opCode as OpCodeMethodInfoPart).Operand;
                    this.FoldNestedOpCodes(
                        opCode, (methodBase.CallingConvention.HasFlag(CallingConventions.HasThis) ? 1 : 0) + methodBase.GetParameters().Count());
                    break;
                case Code.Callvirt:
                    methodBase = (opCode as OpCodeMethodInfoPart).Operand;
                    this.FoldNestedOpCodes(opCode, (code == Code.Callvirt ? 1 : 0) + methodBase.GetParameters().Count());
                    break;
                case Code.Newobj:
                    IConstructor ctorInfo = (opCode as OpCodeConstructorInfoPart).Operand;
                    this.FoldNestedOpCodes(opCode, (code == Code.Callvirt ? 1 : 0) + ctorInfo.GetParameters().Count());
                    break;
                case Code.Stelem:
                case Code.Stelem_I:
                case Code.Stelem_I1:
                case Code.Stelem_I2:
                case Code.Stelem_I4:
                case Code.Stelem_I8:
                case Code.Stelem_R4:
                case Code.Stelem_R8:
                case Code.Stelem_Ref:
                    this.FoldNestedOpCodes(opCode, 3);
                    break;
                case Code.Add:
                case Code.Add_Ovf:
                case Code.Add_Ovf_Un:
                case Code.Mul:
                case Code.Mul_Ovf:
                case Code.Mul_Ovf_Un:
                case Code.Sub:
                case Code.Sub_Ovf:
                case Code.Sub_Ovf_Un:
                case Code.Div:
                case Code.Div_Un:
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
                case Code.Rem:
                case Code.Rem_Un:
                case Code.Stfld:
                case Code.Ceq:
                case Code.Cgt:
                case Code.Cgt_Un:
                case Code.Clt:
                case Code.Clt_Un:
                case Code.Or:
                case Code.Xor:
                case Code.And:
                case Code.Shl:
                case Code.Shr:
                case Code.Shr_Un:
                case Code.Ldelem:
                case Code.Ldelem_I:
                case Code.Ldelem_I1:
                case Code.Ldelem_I2:
                case Code.Ldelem_I4:
                case Code.Ldelem_I8:
                case Code.Ldelem_U1:
                case Code.Ldelem_U2:
                case Code.Ldelem_U4:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Ldelema:
                case Code.Stobj:
                case Code.Stind_I:
                case Code.Stind_I1:
                case Code.Stind_I2:
                case Code.Stind_I4:
                case Code.Stind_I8:
                case Code.Stind_R4:
                case Code.Stind_R8:
                case Code.Stind_Ref:
                    this.FoldNestedOpCodes(opCode, 2);
                    break;
                case Code.Stloc:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                case Code.Stloc_S:
                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                case Code.Conv_R_Un:
                case Code.Conv_R4:
                case Code.Conv_R8:
                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                case Code.Ret:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Ldlen:
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Neg:
                case Code.Not:
                case Code.Dup:
                case Code.Box:
                case Code.Unbox:
                case Code.Unbox_Any:
                case Code.Newarr:
                case Code.Castclass:
                case Code.Isinst:
                case Code.Initobj:
                case Code.Throw:
                case Code.Stsfld:
                case Code.Switch:
                case Code.Ldind_I:
                case Code.Ldind_I1:
                case Code.Ldind_I2:
                case Code.Ldind_I4:
                case Code.Ldind_I8:
                case Code.Ldind_R4:
                case Code.Ldind_R8:
                case Code.Ldind_Ref:
                case Code.Ldind_U1:
                case Code.Ldind_U2:
                case Code.Ldind_U4:
                case Code.Ldobj:
                case Code.Starg:
                case Code.Starg_S:
                case Code.Localloc:
                    this.FoldNestedOpCodes(opCode, 1);
                    break;
                case Code.Ldloc:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldloc_S:
                case Code.Ldarg:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldarg_S:
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                case Code.Ldc_I4_M1:
                case Code.Ldc_I4:
                case Code.Ldc_I4_S:
                case Code.Ldc_I8:
                case Code.Ldc_R4:
                case Code.Ldc_R8:
                case Code.Ldstr:
                case Code.Rethrow:
                    break;
            }

            this.Stack.Push(opCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        protected void ReadMethodInfo(IMethod methodInfo)
        {
            this.Parameters = methodInfo.GetParameters().ToArray();
            this.HasMethodThis = methodInfo.CallingConvention.HasFlag(CallingConventions.HasThis);

            this.MethodReturnType = null;
            this.ThisType = methodInfo.DeclaringType;

            ////this.GenericMethodArguments = methodBase.GetGenericArguments();
            var methodBody = methodInfo.GetMethodBody();
            this.NoBody = methodBody == null;
            if (methodBody != null)
            {
                this.LocalInfo = methodBody.LocalVariables.ToArray();
                this.LocalInfoUsed = new bool[this.LocalInfo.Length];
                this.ExceptionHandlingClauses = methodBody.ExceptionHandlingClauses.ToArray();
            }

            this.MethodReturnType = !methodInfo.ReturnType.IsVoid() ? methodInfo.ReturnType : null;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="genericType">
        /// </param>
        protected void ReadTypeInfo(IType type, IType genericType)
        {
            this.IsInterface = type.IsInterface;
            this.TypeGenericArguments = type.GetGenericArguments().ToArray();
            this.ThisType = type;
        }

        /// <summary>
        /// </summary>
        /// <param name="size">
        /// </param>
        /// <param name="opCodeParts">
        /// </param>
        /// <param name="i">
        /// </param>
        /// <param name="opCodePartUsed">
        /// </param>
        /// <returns>
        /// </returns>
        protected OpCodePart[] RemoveUnusedOps(int size, OpCodePart[] opCodeParts, int i, OpCodePart opCodePartUsed)
        {
            var newOpCodeParts = new List<OpCodePart>(opCodeParts);
            for (int j = 0; j <= size - i; j++)
            {
                newOpCodeParts.RemoveAt(0);
            }

            opCodeParts = newOpCodeParts.ToArray();

            this.Stack.Push(opCodePartUsed);
            return opCodeParts;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        protected ReturnResult RequiredType(OpCodePart opCodePart)
        {
            Code code = opCodePart.ToCode();
            if (code == Code.Ret)
            {
                return new ReturnResult(this.MethodReturnType);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        protected void StartProcess()
        {
            this.Ops.Clear();
            this.Stack.Clear();
            this.OpsByAddressStart.Clear();
            this.OpsByAddressEnd.Clear();
            this.OpsByGroupAddressStart.Clear();
            this.OpsByGroupAddressEnd.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="conditions">
        /// </param>
        /// <returns>
        /// </returns>
        private static OpCodePart[][] BuildConditionGroups(OpCodePart[] conditions)
        {
            var groups = new List<OpCodePart[]>();

            for (int i = 0; i < conditions.Length;)
            {
                var group = new List<OpCodePart>();

                OpCodePart firstOfGroup = conditions[i];
                int stopAddress = firstOfGroup.JumpAddress();

                i++;
                group.Add(firstOfGroup);

                while (i < conditions.Length)
                {
                    OpCodePart element = conditions[i];
                    if (element.GroupAddressStart < stopAddress)
                    {
                        group.Add(element);
                    }
                    else
                    {
                        break;
                    }

                    i++;
                }

                groups.Add(group.ToArray());
            }

            return groups.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        private void AddAddressIndex(OpCodePart opCode)
        {
            this.OpsByAddressStart[opCode.AddressStart] = opCode;
            this.OpsByAddressEnd[opCode.AddressEnd] = opCode;
        }

        /// <summary>
        /// </summary>
        /// <param name="condExpBlock">
        /// </param>
        /// <param name="firstCondition">
        /// </param>
        /// <param name="branchJump">
        /// </param>
        /// <returns>
        /// </returns>
        private OpCodePart[] ConditionalExpressionConditionsParse(OpCodePart[] condExpBlock, OpCodePart firstCondition, OpCodePart branchJump)
        {
            // calculate all addresses
            int startOfTrueExpression = branchJump.GroupAddressEnd;

            OpCodePart lastCondition = null;
            foreach (OpCodePart opCodePart in condExpBlock)
            {
                if (opCodePart.IsCondBranch())
                {
                    lastCondition = opCodePart;
                    continue;
                }

                break;
            }

            var conditionsList = new List<OpCodePart>();

            // adjust all condition conjunctions
            foreach (OpCodePart opCodePart in condExpBlock)
            {
                conditionsList.Add(opCodePart);

                if (opCodePart == lastCondition)
                {
                    break;
                }
            }

            ConditionsParseForConditionalExpression(conditionsList.ToArray(), startOfTrueExpression);

            return condExpBlock;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="currentArgument">
        /// </param>
        /// <param name="stack">
        /// </param>
        /// <param name="sizeOfCondition">
        /// </param>
        /// <param name="firstCondition">
        /// </param>
        /// <param name="lastCondition">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsConditionalExpression(
            OpCodePart opCodePart, 
            OpCodePart currentArgument, 
            Stack<OpCodePart> stack, 
            out int sizeOfCondition, 
            out OpCodePart firstCondition, 
            out OpCodePart lastCondition)
        {
            sizeOfCondition = 3;
            firstCondition = null;
            lastCondition = null;

            for (;;)
            {
                IEnumerable<OpCodePart> subOpCodes = stack.Take(sizeOfCondition);
                if (subOpCodes.Count() != sizeOfCondition)
                {
                    break;
                }

                OpCodePart first = subOpCodes.First();
                OpCodePart last = subOpCodes.Last();

                bool isFirstElementBranch = first.IsBranch() && first.IsJumpForward()
                                            && first.JumpAddress().Equals(currentArgument.GroupAddressEnd /*opCodePart.GroupAddressStart*/);

                // more checks need here. there should be second return
                OpCodePart beforeFirst = first.PreviousOpCodeGroup(this);
                if (first.IsReturn() && first.OpCodeOperands != null && first.OpCodeOperands.Length == 1 && beforeFirst != null && beforeFirst.IsReturn()
                    && beforeFirst.OpCodeOperands != null && beforeFirst.OpCodeOperands.Length == 1)
                {
                    isFirstElementBranch = true;
                }

                bool isLastElementCondition = last.IsCondBranch() && last.IsJumpForward() && last.JumpAddress() <= first.GroupAddressEnd;

                if (isFirstElementBranch && isLastElementCondition)
                {
                    // in reverse order
                    firstCondition = last;
                    lastCondition = first;

                    sizeOfCondition++;
                    continue;
                }

                break;
            }

            if (sizeOfCondition == 3)
            {
                return false;
            }

            --sizeOfCondition;

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="currentArgument">
        /// </param>
        /// <param name="stack">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsNullCoalescingExpression(OpCodePart opCodePart, OpCodePart currentArgument, Stack<OpCodePart> stack)
        {
            if (stack.Count < 3)
            {
                return false;
            }

            if (!stack.First().Any(Code.Pop))
            {
                return false;
            }

            OpCodePart second = stack.Skip(1).First();
            if (!second.Any(Code.Brtrue, Code.Brtrue_S))
            {
                return false;
            }

            if (second.JumpAddress() != opCodePart.GroupAddressStart)
            {
                // we do not have full expression
                return false;
            }

            if (!stack.Skip(2).First().Any(Code.Dup))
            {
                return false;
            }

            return true;
        }

        #endregion

        /// <summary>
        /// </summary>
        public class ReturnResult : IEquatable<ReturnResult>
        {
            #region Constructors and Destructors

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            public ReturnResult(IType type)
            {
                this.IType = type;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="asReference">
            /// </param>
            public ReturnResult(IType type, bool asReference)
                : this(type)
            {
                this.IsReference = asReference;
            }

            public ReturnResult(Type type)
            {
                this.IType = TypeAdapter.FromType(type);
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="asReference">
            /// </param>
            public ReturnResult(Type type, bool asReference)
                : this(type)
            {
                this.IsReference = asReference;
            }

            #endregion

            #region Public Properties

            /// <summary>
            /// </summary>
            public bool? Boxed { get; set; }

            /// <summary>
            /// </summary>
            public bool? IsAddress { get; set; }

            /// <summary>
            /// </summary>
            public bool? IsConst { get; set; }

            /// <summary>
            /// </summary>
            public bool IsDotAccessRequired
            {
                get
                {
                    return this.IType != null && this.IType.IsValueType || ((this.IsAddress ?? false) && (this.IsField ?? false))
                           || this.IType != null && this.IType.IsByRef && this.IType.GetElementType().IsValueType;
                }
            }

            /// <summary>
            /// </summary>
            public bool? IsField { get; set; }

            /// <summary>
            /// </summary>
            public bool? IsIndirect { get; set; }

            /// <summary>
            /// </summary>
            public bool IsPointerAccessRequired
            {
                get
                {
                    if ((this.IsReference ?? false) || (this.IsAddress ?? false) || (this.Boxed ?? false))
                    {
                        return true;
                    }

                    if (this.IType.IsValueType())
                    {
                        return false;
                    }

                    return true;
                }
            }

            /// <summary>
            /// </summary>
            public bool? IsReference { get; set; }

            /// <summary>
            /// </summary>
            public IType IType { get; set; }

            #endregion

            #region Public Methods and Operators

            /// <summary>
            /// </summary>
            /// <param name="other">
            /// </param>
            /// <returns>
            /// </returns>
            public bool Equals(ReturnResult other)
            {
                return this.IType == other.IType && this.IsReference == other.IsReference && this.IsField == other.IsField && this.Boxed == other.Boxed;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <returns>
            /// </returns>
            public bool IsTypeOf(IType type)
            {
                if (this.IType == null || type == null)
                {
                    return false;
                }

                return this.IType == type;
            }

            public bool IsTypeOf(Type type)
            {
                if (this.IType == null || type == null)
                {
                    return false;
                }

                return this.IType == TypeAdapter.FromType(type);
            }

            #endregion
        }
    }
}
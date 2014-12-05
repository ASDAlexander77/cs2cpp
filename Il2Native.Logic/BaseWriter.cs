// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseWriter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Exceptions;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class BaseWriter
    {
        /// <summary>
        /// </summary>
        protected readonly ISet<IType> RequiredTypesForBody = new HashSet<IType>();

        /// <summary>
        /// </summary>
        protected readonly IDictionary<string, IType> ResolvedTypes = new SortedDictionary<string, IType>();

        /// <summary>
        /// </summary>
        protected readonly StackBranches Stacks = new StackBranches();

        /// <summary>
        /// </summary>
        public BaseWriter()
        {
            this.StaticConstructors = new List<IConstructor>();
            this.Ops = new List<OpCodePart>();
            this.OpsByGroupAddressStart = new SortedDictionary<int, OpCodePart>();
            this.OpsByGroupAddressEnd = new SortedDictionary<int, OpCodePart>();
            this.OpsByAddressStart = new SortedDictionary<int, OpCodePart>();
            this.OpsByAddressEnd = new SortedDictionary<int, OpCodePart>();
        }

        /// <summary>
        /// </summary>
        public string AssemblyQualifiedName { get; protected set; }

        /// <summary>
        /// </summary>
        public bool HasMethodThis { get; private set; }

        /// <summary>
        /// </summary>
        public ILocalVariable[] LocalInfo { get; private set; }

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

        /// <summary>
        /// </summary>
        public IParameter[] Parameters { get; private set; }

        /// <summary>
        /// </summary>
        public IType ThisType { get; private set; }

        /// <summary>
        /// </summary>
        protected IExceptionHandlingClause[] ExceptionHandlingClauses { get; private set; }

        /// <summary>
        /// </summary>
        protected IType[] GenericMethodArguments { get; private set; }

        /// <summary>
        /// </summary>
        protected bool IsInterface { get; set; }

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
        protected List<IConstructor> StaticConstructors { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="parameters">
        /// </param>
        public void CheckIfParameterTypeIsRequired(IEnumerable<IParameter> parameters)
        {
            if (parameters == null)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                if (parameter.ParameterType.IsStructureType())
                {
                    this.CheckIfTypeIsRequiredForBody(parameter.ParameterType);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public void CheckIfTypeIsRequiredForBody(IType type)
        {
            var item = type.HasElementType ? type.GetElementType() : type;
            if (!item.IsPrimitiveType())
            {
                this.RequiredTypesForBody.Add(item);
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
            var isThis = oper1.Any(Code.Ldarg_0) && this.HasMethodThis;
            return isThis;
        }

        /// <summary>
        /// </summary>
        /// <param name="fullTypeName">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ResolveType(string fullTypeName)
        {
            IType result;
            if (this.ResolvedTypes.TryGetValue(fullTypeName, out result))
            {
                return result;
            }

            result = this.ThisType.Module.ResolveType(fullTypeName, null);
            this.ResolvedTypes[result.FullName] = result;
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="doNotUseCachedResult">
        /// </param>
        /// <returns>
        /// </returns>
        public ReturnResult ResultOf(OpCodePart opCode, bool doNotUseCachedResult = false)
        {
            if (!doNotUseCachedResult && opCode.HasResult)
            {
                return new ReturnResult(opCode.Result);
            }

            var code = opCode.ToCode();
            switch (code)
            {
                case Code.Call:
                case Code.Callvirt:
                    var methodBase = (opCode as OpCodeMethodInfoPart).Operand;
                    return new ReturnResult(methodBase.ReturnType);
                case Code.Newobj:
                    if (opCode.ReadExceptionFromStack)
                    {
                        return new ReturnResult(opCode.ReadExceptionFromStackType);
                    }

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
                case Code.Rem:
                case Code.Rem_Un:
                case Code.Shl:
                case Code.Shr:
                case Code.Shr_Un:
                case Code.And:
                case Code.Or:
                case Code.Xor:
                    var op1 = this.ResultOf(opCode.OpCodeOperands[0]);
                    return !op1.IsConst ? op1 : this.ResultOf(opCode.OpCodeOperands[1]);
                case Code.Isinst:
                    return new ReturnResult((opCode as OpCodeTypePart).Operand);
                case Code.Ceq:
                case Code.Cgt:
                case Code.Cgt_Un:
                case Code.Clt:
                case Code.Clt_Un:
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
                case Code.Brfalse:
                case Code.Brfalse_S:
                case Code.Brtrue:
                case Code.Brtrue_S:
                case Code.Bne_Un:
                case Code.Bne_Un_S:
                case Code.Bge_Un:
                case Code.Bge_Un_S:
                case Code.Ble_Un:
                case Code.Ble_Un_S:
                case Code.Bgt_Un:
                case Code.Bgt_Un_S:
                case Code.Blt_Un:
                case Code.Blt_Un_S:
                    return new ReturnResult(this.ResolveType("System.Boolean"));
                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                    return new ReturnResult(this.ResolveType("System.Int32"));
                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:
                    return new ReturnResult(this.ResolveType("System.UInt32"));
                case Code.Conv_R_Un:
                case Code.Conv_R4:
                    return new ReturnResult(this.ResolveType("System.Single"));
                case Code.Conv_R8:
                    return new ReturnResult(this.ResolveType("System.Double"));
                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                    return new ReturnResult(this.ResolveType("System.SByte"));
                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                    return new ReturnResult(this.ResolveType("System.Int16"));
                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                    return new ReturnResult(this.ResolveType("System.Int32"));
                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                    return new ReturnResult(this.ResolveType("System.Int64"));
                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                    return new ReturnResult(this.ResolveType("System.Byte"));
                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                    return new ReturnResult(this.ResolveType("System.UInt16"));
                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                    return new ReturnResult(this.ResolveType("System.UInt32"));
                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    return new ReturnResult(this.ResolveType("System.UInt64"));
                case Code.Castclass:
                    return new ReturnResult((opCode as OpCodeTypePart).Operand);
                case Code.Newarr:
                    return new ReturnResult((opCode as OpCodeTypePart).Operand.ToArrayType(1));
                case Code.Ret:
                case Code.Neg:
                case Code.Not:
                case Code.Dup:
                    return this.ResultOf(opCode.OpCodeOperands[0]);
                case Code.Ldlen:
                    return new ReturnResult(this.ResolveType("System.Int32"));
                case Code.Ldloca:
                case Code.Ldloca_S:
                    var localVarType = this.LocalInfo[(opCode as OpCodeInt32Part).Operand].LocalType;
                    return new ReturnResult(localVarType.IsValueType ? localVarType.ToPointerType() : localVarType);
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
                    var opCodeInt32Part = opCode as OpCodeInt32Part;
                    var parameterType = this.Parameters[opCodeInt32Part.Operand - (this.HasMethodThis ? 1 : 0)].ParameterType;
                    return new ReturnResult(parameterType.ToPointerType());
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
                    var result = this.ResultOf(opCode.OpCodeOperands[0]);

                    // we are loading address of item of the array so we need to return type of element not the type of the array
                    return new ReturnResult(result.Type.GetElementType());
                case Code.Ldelem_Ref:
                    result = this.ResultOf(opCode.OpCodeOperands[0]) ?? new ReturnResult((IType)null);
                    return result;
                case Code.Ldelema:
                    result = this.ResultOf(opCode.OpCodeOperands[0]);

                    // we are loading address of item of the array so we need to return type of element not the type of the array
                    // var typeOfElement = result.IType.HasElementType ? result.IType.GetElementType() : result.IType;
                    var typeOfElement = result.Type.GetElementType();
                    return new ReturnResult(typeOfElement.ToPointerType());
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
                    return new ReturnResult(opCode.UseAsNull ? this.ResolveType("System.Void").ToPointerType() : this.ResolveType("System.Int32"))
                               {
                                   IsConst = true
                               };
                case Code.Ldc_I8:
                    return new ReturnResult(this.ResolveType("System.Int64")) { IsConst = true };
                case Code.Ldc_R4:
                    return new ReturnResult(this.ResolveType("System.Single")) { IsConst = true };
                case Code.Ldc_R8:
                    return new ReturnResult(this.ResolveType("System.Double")) { IsConst = true };
                case Code.Ldstr:
                    return new ReturnResult(this.ResolveType("System.String"));
                case Code.Ldind_I:
                    return new ReturnResult(this.ResolveType("System.Int32"));
                case Code.Ldind_I1:
                    return new ReturnResult(this.ResolveType("System.Byte"));
                case Code.Ldind_I2:
                    return new ReturnResult(this.ResolveType("System.Int16"));
                case Code.Ldind_I4:
                    return new ReturnResult(this.ResolveType("System.Int32"));
                case Code.Ldind_I8:
                    return new ReturnResult(this.ResolveType("System.Int64"));
                case Code.Ldind_U1:
                    return new ReturnResult(this.ResolveType("System.Byte"));
                case Code.Ldind_U2:
                    return new ReturnResult(this.ResolveType("System.UInt16"));
                case Code.Ldind_U4:
                    return new ReturnResult(this.ResolveType("System.UInt32"));
                case Code.Ldind_R4:
                    return new ReturnResult(this.ResolveType("System.Single"));
                case Code.Ldind_R8:
                    return new ReturnResult(this.ResolveType("System.Double"));
                case Code.Ldind_Ref:
                    var resultType = this.ResultOf(opCode.OpCodeOperands[0]).Type;
                    return new ReturnResult(resultType.GetElementType());
                case Code.Ldflda:
                case Code.Ldsflda:
                    var opCodeFieldInfoPart = opCode as OpCodeFieldInfoPart;
                    var fieldType = opCodeFieldInfoPart.Operand.FieldType;
                    return new ReturnResult(fieldType.ToPointerType());
                case Code.Nop:
                    return null;
                case Code.Ldobj:
                    var opCodeTypePart = opCode as OpCodeTypePart;
                    return new ReturnResult(opCodeTypePart.Operand);
                case Code.Box:

                    // TODO: call .KeyedCollection`2, Method ContainsItem have a problem with Box and Stloc.1
                    var res = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (res != null)
                    {
                        result = new ReturnResult(res.Type.ToClass());
                        return result;
                    }

                    return null;

                case Code.Unbox:
                case Code.Unbox_Any:

                    // TODO: call .KeyedCollection`2, Method ContainsItem have a problem with Box and Stloc.1
                    res = this.ResultOf(opCode.OpCodeOperands[0]);
                    if (res != null)
                    {
                        return new ReturnResult(res.Type);
                    }

                    return null;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        public virtual void StartProcess()
        {
            this.Ops.Clear();
            this.Stacks.Clear();
            this.OpsByAddressStart.Clear();
            this.OpsByAddressEnd.Clear();
            this.OpsByGroupAddressStart.Clear();
            this.OpsByGroupAddressEnd.Clear();
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        protected void AddOpCode(OpCodePart opCode)
        {
            this.Ops.Add(opCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        protected void AdjustTypes(OpCodePart opCode)
        {
            // TODO: review this function, I think I need to get rid of it
            if (opCode.OpCodeOperands == null || opCode.OpCodeOperands.Length == 0)
            {
                return;
            }

            var usedOpCode1 = opCode.OpCodeOperands[0];
            if (usedOpCode1 == null)
            {
                // todo: should not be here
                return;
            }

            // fix types
            var requiredType = this.RequiredIncomingType(opCode);
            if (requiredType != null)
            {
                if ((requiredType.IsPointer || requiredType.IsByRef) && usedOpCode1.Any(Code.Conv_U) && usedOpCode1.OpCodeOperands[0].Any(Code.Ldc_I4_0))
                {
                    usedOpCode1.OpCodeOperands[0].UseAsNull = true;
                }
            }
        }

        /// <summary>
        /// </summary>
        protected void AssignExceptionsToOpCodes()
        {
            if (this.ExceptionHandlingClauses == null || !this.ExceptionHandlingClauses.Any())
            {
                return;
            }

            var tries = new List<TryClause>();
            foreach (var groupedEh in this.ExceptionHandlingClauses.GroupBy(eh => eh.TryOffset + eh.TryLength))
            {
                TryClause tryItem = null;
                CatchOfFinallyClause previousClause = null;
                foreach (var exceptionHandlingClause in groupedEh)
                {
                    if (tryItem == null)
                    {
                        tryItem = new TryClause();
                        tryItem.Offset = exceptionHandlingClause.TryOffset;
                        tryItem.Length = exceptionHandlingClause.TryLength;
                    }

                    var catchOfFinallyClause = new CatchOfFinallyClause
                                                   {
                                                       Flags = exceptionHandlingClause.Flags,
                                                       Offset = exceptionHandlingClause.HandlerOffset,
                                                       Length = exceptionHandlingClause.HandlerLength,
                                                       Catch = exceptionHandlingClause.CatchType,
                                                       OwnerTry = tryItem
                                                   };

                    tryItem.Catches.Add(catchOfFinallyClause);

                    if (previousClause != null)
                    {
                        previousClause.Next = catchOfFinallyClause;
                    }

                    previousClause = catchOfFinallyClause;
                }

                tries.Add(tryItem);
            }

            foreach (var tryItem in tries)
            {
                OpCodePart opCodePart;
                if (this.OpsByAddressStart.TryGetValue(tryItem.Offset, out opCodePart))
                {
                    if (opCodePart.TryBegin == null)
                    {
                        opCodePart.TryBegin = new List<TryClause>();
                    }

                    opCodePart.TryBegin.Add(tryItem);
                }

                if (this.OpsByAddressEnd.TryGetValue(tryItem.Offset + tryItem.Length, out opCodePart))
                {
                    Debug.Assert(opCodePart.TryEnd == null);
                    opCodePart.TryEnd = tryItem;
                }

                if (this.OpsByAddressEnd.TryGetValue(tryItem.Catches.First().Offset, out opCodePart))
                {
                    opCodePart.ExceptionHandlers = tryItem.Catches;
                }

                foreach (var catchOrFinally in tryItem.Catches)
                {
                    if (this.OpsByAddressStart.TryGetValue(catchOrFinally.Offset, out opCodePart))
                    {
                        Debug.Assert(opCodePart.CatchOrFinallyBegin == null);
                        opCodePart.CatchOrFinallyBegin = catchOrFinally;
                    }

                    if (this.OpsByAddressEnd.TryGetValue(catchOrFinally.Offset + catchOrFinally.Length, out opCodePart))
                    {
                        // Debug.Assert(opCodePart.CatchOrFinallyEnds == null);
                        if (opCodePart.CatchOrFinallyEnds == null)
                        {
                            opCodePart.CatchOrFinallyEnds = new List<CatchOfFinallyClause>();
                        }

                        opCodePart.CatchOrFinallyEnds.Add(catchOrFinally);
                    }
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodes">
        /// </param>
        protected void AssignJumpBlocks(IEnumerable<OpCodePart> opCodes)
        {
            foreach (var opCodePart in opCodes)
            {
                if (opCodePart.IsAnyBranch())
                {
                    var jumpOp = opCodePart as OpCodeInt32Part;
                    if (jumpOp != null)
                    {
                        var nextAddress = opCodePart.JumpAddress();
                        var target = this.OpsByAddressStart[nextAddress];
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
        protected void CalculateRequiredTypesForAlternativeValues(IEnumerable<OpCodePart> opCodes)
        {
            foreach (var opCodePart in opCodes)
            {
                if (opCodePart.AlternativeValues == null)
                {
                    continue;
                }

                // detect required types in alternative values
                var firstOpCode = opCodePart.AlternativeValues.Values.FirstOrDefault(v => v.UsedBy != null && !v.UsedBy.Any(Code.Pop));
                if (firstOpCode == null)
                {
                    // TODO: find out why it happens here (test-154.cs)
                    continue;
                }

                var usedBy = firstOpCode.UsedBy;
                var requiredType = this.RequiredIncomingType(usedBy.OpCode);
                foreach (var val in opCodePart.AlternativeValues.Values)
                {
                    val.RequiredOutgoingType = requiredType;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="size">
        /// </param>
        protected void FoldNestedOpCodes(OpCodePart opCodePart, int size, bool varArg = false)
        {
            if (opCodePart.OpCode.StackBehaviourPop == StackBehaviour.Pop0)
            {
                return;
            }

            if (opCodePart.ToCode() == Code.Ret && this.MethodReturnType == null)
            {
                return;
            }

            var opCodeParts = new List<OpCodePart>(size);

            for (var i = 1; i <= size || varArg; i++)
            {
                var isVarArg = i > size && varArg;
                // take value from Stack
                var opCodePartUsed = PopValue(isVarArg);
                if (isVarArg && opCodePartUsed == null)
                {
                    break;
                }

                if (opCodePartUsed.ToCode() == Code.Dup)
                {
                    this.Stacks.Push(opCodePartUsed.OpCodeOperands[0]);
                }

                opCodeParts.Insert(0, opCodePartUsed);
            }

            opCodePart.OpCodeOperands = opCodeParts.ToArray();
            var operandPosition = 0;
            foreach (var childCodePart in opCodeParts)
            {
                childCodePart.UsedBy = new UsedByInfo(opCodePart, operandPosition++);
            }

            this.AdjustTypes(opCodePart);
        }

        private OpCodePart PopValue(bool varArg = false)
        {
            if (varArg && !this.Stacks.Any())
            {
                return null;
            }

            var opCodePartUsed = this.Stacks.Pop();
            return opCodePartUsed;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual IEnumerable<OpCodePart> InsertAfterOpCode(OpCodePart opCode)
        {
            yield break;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        protected virtual IEnumerable<OpCodePart> InsertBeforeOpCode(OpCodePart opCode)
        {
            // insert result of exception
            var exceptionHandling = this.ExceptionHandlingClauses.FirstOrDefault(eh => eh.HandlerOffset == opCode.AddressStart);
            if (exceptionHandling == null || exceptionHandling.CatchType == null)
            {
                yield break;
            }

            var opCodeNope = new OpCodePart(OpCodesEmit.Newobj, opCode.AddressStart, opCode.AddressStart);
            opCodeNope.ReadExceptionFromStack = true;
            opCodeNope.ReadExceptionFromStackType = exceptionHandling.CatchType;
            yield return opCodeNope;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodes">
        /// </param>
        /// <returns>
        /// </returns>
        protected IEnumerable<OpCodePart> PreProcessOpCodes(IEnumerable<OpCodePart> opCodes)
        {
            OpCodePart last = null;
            foreach (var opCodePart in opCodes)
            {
                foreach (var opCodePartBefore in this.InsertBeforeOpCode(opCodePart))
                {
                    last = BuildChain(last, opCodePartBefore);
                    yield return opCodePartBefore;
                }

                last = BuildChain(last, opCodePart);
                yield return opCodePart;

                foreach (var opCodePartAfter in this.InsertAfterOpCode(opCodePart))
                {
                    last = BuildChain(last, opCodePartAfter);
                    yield return opCodePartAfter;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        protected IEnumerable<OpCodePart> PrepareWritingMethodBody()
        {
            var ops = this.PreProcessOpCodes(this.Ops).ToList();
            this.ProcessAll(ops);
            this.CalculateRequiredTypesForAlternativeValues(ops);
            this.AssignJumpBlocks(ops);
            this.AssignExceptionsToOpCodes();
            return ops;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        protected void Process(OpCodePart opCode)
        {
            var code = opCode.ToCode();

            this.Stacks.UpdateCurrentAddress(opCode.AddressStart);

            switch (code)
            {
                case Code.Call:
                    var methodBase = (opCode as OpCodeMethodInfoPart).Operand;
                    this.FoldNestedOpCodes(
                        opCode, (methodBase.CallingConvention.HasFlag(CallingConventions.HasThis) ? 1 : 0) + methodBase.GetParameters().Count(), methodBase.CallingConvention.HasFlag(CallingConventions.VarArgs));
                    this.CheckIfParameterTypeIsRequired(methodBase.GetParameters());
                    break;
                case Code.Callvirt:
                    methodBase = (opCode as OpCodeMethodInfoPart).Operand;
                    this.FoldNestedOpCodes(opCode, (code == Code.Callvirt ? 1 : 0) + methodBase.GetParameters().Count(), methodBase.CallingConvention.HasFlag(CallingConventions.VarArgs));
                    this.CheckIfParameterTypeIsRequired(methodBase.GetParameters());
                    break;
                case Code.Newobj:
                    if (opCode.ReadExceptionFromStack)
                    {
                        break;
                    }

                    var ctorInfo = (opCode as OpCodeConstructorInfoPart).Operand;
                    this.FoldNestedOpCodes(opCode, (code == Code.Callvirt ? 1 : 0) + ctorInfo.GetParameters().Count(), ctorInfo.CallingConvention.HasFlag(CallingConventions.VarArgs));
                    this.CheckIfParameterTypeIsRequired(ctorInfo.GetParameters());
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
                case Code.Pop:
                case Code.Ldvirtftn:
                case Code.Mkrefany:
                case Code.Refanytype:
                case Code.Refanyval:
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

            this.BuildGroupIndex(opCode);

            this.CheckIfNewBranchToCreate(opCode, code);

            // add to stack
            if (opCode.OpCode.StackBehaviourPush == StackBehaviour.Push0)
            {
                return;
            }

            var isItMethodWithVoid = opCode.OpCode.StackBehaviourPush == StackBehaviour.Varpush && opCode is OpCodeMethodInfoPart
                                     && ((OpCodeMethodInfoPart)opCode).Operand.ReturnType.IsVoid();
            if (!isItMethodWithVoid)
            {
                this.Stacks.Push(opCode);
            }
        }

        private void CheckIfNewBranchToCreate(OpCodePart opCode, Code code)
        {
            switch (code)
            {
                case Code.Br:
                case Code.Br_S:
                    if (opCode.IsJumpForward())
                    {
                        this.Stacks.CreateNewBranch(opCode.JumpAddress());
                    }
                    break;
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
                    if (opCode.IsJumpForward() && !opCode.JumpOpCode(this).Next.Any(Code.Br, Code.Br_S))
                    {
                        this.Stacks.CreateNewBranch(opCode.JumpAddress());
                    }

                    break;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodes">
        /// </param>
        protected void ProcessAll(IEnumerable<OpCodePart> opCodes)
        {
            this.OpsByGroupAddressStart.Clear();
            this.OpsByGroupAddressEnd.Clear();

            foreach (var opCodePart in opCodes)
            {
                this.AddAddressIndex(opCodePart);
            }

            foreach (var opCodePart in opCodes)
            {
                this.Process(opCodePart);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        protected void ReadMethodInfo(IMethod methodInfo, IGenericContext genericContext)
        {
            this.Parameters = methodInfo.GetParameters().ToArray();
            this.HasMethodThis = methodInfo.CallingConvention.HasFlag(CallingConventions.HasThis);

            this.MethodReturnType = null;
            this.ThisType = methodInfo.DeclaringType;

            ////this.GenericMethodArguments = methodBase.GetGenericArguments();
            var methodBody = methodInfo.ResolveMethodBody(genericContext);
            this.NoBody = !methodBody.HasBody;
            if (!this.NoBody)
            {
                this.LocalInfo = methodBody.LocalVariables.ToArray();

                this.AdjustLocalVariableTypes();

#if DEBUG
                Debug.Assert(genericContext == null || !this.LocalInfo.Any(li => li.LocalType.IsGenericParameter));
#endif

                this.LocalInfoUsed = new bool[this.LocalInfo.Length];
                this.ExceptionHandlingClauses = methodBody.ExceptionHandlingClauses.ToArray();
            }

            this.MethodReturnType = !methodInfo.ReturnType.IsVoid() ? methodInfo.ReturnType : null;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        protected void ReadTypeInfo(IType type)
        {
            this.IsInterface = type.IsInterface;
            this.ThisType = type;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="operandPosition">
        /// </param>
        /// <returns>
        /// </returns>
        protected IType RequiredIncomingType(OpCodePart opCodePart, int operandPosition = -1)
        {
            // TODO: need a good review of required types etc
            IType retType = null;
            if (opCodePart.Any(Code.Ret))
            {
                retType = this.MethodReturnType;
                return retType;
            }

            if (opCodePart.Any(Code.Stloc, Code.Stloc_0, Code.Stloc_1, Code.Stloc_2, Code.Stloc_3, Code.Stloc_S))
            {
                retType = opCodePart.GetLocalType(this);
                return retType;
            }

            if (opCodePart.Any(Code.Starg, Code.Starg_S))
            {
                var index = opCodePart.GetArgIndex();
                if (this.HasMethodThis && index == 0)
                {
                    retType = this.ThisType;
                    return retType;
                }

                retType = this.GetArgType(index);
                return retType;
            }

            if (opCodePart.Any(Code.Stfld, Code.Stsfld))
            {
                retType = ((OpCodeFieldInfoPart)opCodePart).Operand.FieldType;
                return retType;
            }

            if (opCodePart.Any(Code.Stobj))
            {
                retType = ((OpCodeTypePart)opCodePart).Operand;
                return retType;
            }

            if (opCodePart.Any(Code.Stind_Ref))
            {
                retType = this.RequiredOutgoingType(opCodePart.OpCodeOperands[0]);
                if (retType.IsByRef)
                {
                    retType = retType.ToNormal();
                }

                return retType;
            }

            if (opCodePart.Any(Code.Unbox, Code.Unbox_Any))
            {
                retType = ((OpCodeTypePart)opCodePart).Operand;
                return retType.IsPrimitiveType() || retType.IsStructureType() ? retType.ToClass() : retType;
            }

            if (opCodePart.Any(Code.Box))
            {
                retType = ((OpCodeTypePart)opCodePart).Operand;
                return retType.UseAsClass ? retType.ToNormal() : retType;
            }

            if (opCodePart.Any(Code.Call, Code.Callvirt))
            {
                var effectiveoperandPosition = operandPosition;
                var opCodePartMethod = opCodePart as OpCodeMethodInfoPart;
                if (opCodePart.Any(Code.Callvirt) || opCodePartMethod.Operand.CallingConvention.HasFlag(CallingConventions.HasThis))
                {
                    if (operandPosition == 0)
                    {
                        return opCodePartMethod.Operand.DeclaringType;
                    }
                    else
                    {
                        effectiveoperandPosition--;
                    }
                }

                var parameters = opCodePartMethod.Operand.GetParameters();
                var index = 0;
                foreach (var parameter in parameters)
                {
                    if (index == effectiveoperandPosition)
                    {
                        retType = parameter.ParameterType;
                        break;
                    }

                    index++;
                }
            }

            return retType;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        protected IType RequiredOutgoingType(OpCodePart opCodePart)
        {
            // TODO: need a good review of required types etc
            IType retType = null;
            if (opCodePart.Any(Code.Ret))
            {
                retType = this.MethodReturnType;
                return retType;
            }

            if (opCodePart.Any(Code.Ldloc, Code.Ldloc_0, Code.Ldloc_1, Code.Ldloc_2, Code.Ldloc_3, Code.Ldloc_S))
            {
                retType = opCodePart.GetLocalType(this);
                return retType;
            }

            if (opCodePart.Any(Code.Ldarg, Code.Ldarg_0, Code.Ldarg_1, Code.Ldarg_2, Code.Ldarg_3, Code.Ldarg_S))
            {
                var index = opCodePart.GetArgIndex();
                if (this.HasMethodThis && index == 0)
                {
                    retType = this.ThisType;
                    return retType;
                }

                retType = this.GetArgType(index);
                return retType;
            }

            if (opCodePart.Any(Code.Ldfld, Code.Ldsfld))
            {
                retType = ((OpCodeFieldInfoPart)opCodePart).Operand.FieldType;
                return retType;
            }

            if (opCodePart.Any(Code.Ldobj))
            {
                retType = ((OpCodeTypePart)opCodePart).Operand;
                return retType;
            }

            if (opCodePart.Any(Code.Ldind_Ref))
            {
                retType = this.RequiredIncomingType(opCodePart.OpCodeOperands[0]);
                return retType;
            }

            if (opCodePart.Any(Code.Unbox, Code.Unbox_Any))
            {
                retType = ((OpCodeTypePart)opCodePart).Operand;
                return retType.UseAsClass ? retType.ToNormal() : retType;
            }

            if (opCodePart.Any(Code.Box))
            {
                retType = ((OpCodeTypePart)opCodePart).Operand;
                return retType.IsPrimitiveType() || retType.IsStructureType() ? retType.ToClass() : retType;
            }

            if (opCodePart.Any(Code.Call, Code.Callvirt))
            {
                var opCodePartMethod = opCodePart as OpCodeMethodInfoPart;
                return opCodePartMethod.Operand.ReturnType;
            }

            return retType;
        }

        /// <summary>
        /// </summary>
        /// <param name="last">
        /// </param>
        /// <param name="opCodePartBefore">
        /// </param>
        /// <returns>
        /// </returns>
        private static OpCodePart BuildChain(OpCodePart last, OpCodePart opCodePartBefore)
        {
            if (last != null)
            {
                last.Next = opCodePartBefore;
                opCodePartBefore.Previous = last;
            }

            last = opCodePartBefore;
            return last;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        private void AddAddressIndex(OpCodePart opCode)
        {
            if (!this.OpsByAddressStart.ContainsKey(opCode.AddressStart))
            {
                this.OpsByAddressStart[opCode.AddressStart] = opCode;
            }

            if (!this.OpsByAddressEnd.ContainsKey(opCode.AddressEnd))
            {
                this.OpsByAddressEnd[opCode.AddressEnd] = opCode;
            }
        }

        /// <summary>
        /// </summary>
        private void AdjustLocalVariableTypes()
        {
            // replace pinned IntPtr& with Int
            foreach (var localInfo in this.LocalInfo.Where(li => li.LocalType.IsPinned))
            {
                localInfo.LocalType = localInfo.LocalType.FullName == "System.IntPtr"
                                          ? this.ResolveType("System.Void").ToPointerType()
                                          : localInfo.LocalType.ToPointerType();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        private void BuildGroupIndex(OpCodePart opCodePart)
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

        /// <summary>
        /// </summary>
        /// <param name="currentArgument">
        /// </param>
        /// <param name="stackBranches">
        /// </param>
        /// <returns>
        /// </returns>
        private bool IsConditionalExpression(OpCodePart currentArgument, StackBranches stackBranches, out bool whenSecondValueSeparatedByExpression)
        {
            whenSecondValueSeparatedByExpression = false;

            if (!stackBranches.Any())
            {
                return false;
            }

            var firstValue = currentArgument;

            var middleJump = firstValue.PreviousOpCodeGroup(this);
            var isJumpForward = middleJump != null && middleJump.IsBranch() && middleJump.IsJumpForward();

            var secondValue = stackBranches.First();

            if (!isJumpForward)
            {
                // check value in stack if it is followed by jump
                middleJump = secondValue.NextOpCodeGroup(this);
                isJumpForward = middleJump != null && middleJump.IsBranch() && middleJump.IsJumpForward();
                if (!isJumpForward)
                {
                    return false;
                }

                whenSecondValueSeparatedByExpression = true;
            }

            while (true)
            {
                var firstCondJump = secondValue.PreviousOpCodeGroup(this);
                if (firstCondJump == null)
                {
                    return false;
                }

                var isCondJumpForward = firstCondJump != null && firstCondJump.IsCondBranch() && firstCondJump.IsJumpForward()
                                        && firstCondJump.JumpAddress() == middleJump.AddressEnd;
                if (!isCondJumpForward)
                {
                    secondValue = firstCondJump;
                    continue;
                }

                break;
            }

            // expression is not full yet
            if (firstValue.GroupAddressEnd != middleJump.JumpAddress())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// </summary>
        // TODO: you need to get rid of using it
        public class ReturnResult : IEquatable<ReturnResult>
        {
            /// <summary>
            /// </summary>
            /// <param name="result">
            /// </param>
            public ReturnResult(FullyDefinedReference result)
            {
                this.Type = result.Type;
                this.IsConst = result is ConstValue;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            public ReturnResult(IType type)
            {
                this.Type = type;
            }

            /// <summary>
            /// </summary>
            public bool IsConst { get; set; }

            /// <summary>
            /// </summary>
            public bool IsPointerAccessRequired
            {
                get
                {
                    if (this.Type.IsPointer || this.Type.UseAsClass)
                    {
                        return true;
                    }

                    if (this.Type.IsValueType())
                    {
                        return false;
                    }

                    return true;
                }
            }

            /// <summary>
            /// </summary>
            public IType Type { get; set; }

            /// <summary>
            /// </summary>
            /// <param name="other">
            /// </param>
            /// <returns>
            /// </returns>
            public bool Equals(ReturnResult other)
            {
                return this.Type.TypeEquals(other.Type) && this.IsConst == other.IsConst;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <returns>
            /// </returns>
            public bool IsTypeOf(IType type)
            {
                if (this.Type == null || type == null)
                {
                    return false;
                }

                return this.Type.TypeEquals(type);
            }
        }
    }
}
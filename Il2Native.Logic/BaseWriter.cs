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
    using CodeParts;
    using DebugInfo.DebugInfoSymbolWriter;
    using Exceptions;
    using Gencode;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public class BaseWriter : ITypeResolver
    {
        /// <summary>
        /// </summary>
        protected readonly IDictionary<string, IType> ResolvedTypes = new SortedDictionary<string, IType>();

        /// <summary>
        /// </summary>
        private static readonly IDictionary<string, IType> GlobalResolvedTypes = new SortedDictionary<string, IType>();

        /// <summary>
        /// </summary>
        protected readonly StackBranches Stacks = new StackBranches();

        /// <summary>
        /// </summary>
        public BaseWriter()
        {
            this.Ops = new List<OpCodePart>();
            this.OpsByGroupAddressStart = new SortedDictionary<int, OpCodePart>();
            this.OpsByGroupAddressEnd = new SortedDictionary<int, OpCodePart>();
            this.OpsByAddressStart = new SortedDictionary<int, OpCodePart>();
            this.OpsByAddressEnd = new SortedDictionary<int, OpCodePart>();
        }

        /// <summary>
        /// </summary>
        public string AssemblyQualifiedName
        {
            get
            {
                return this.IlReader.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// </summary>
        public bool GcSupport { get; protected set; }

        /// <summary>
        /// </summary>
        public bool MultiThreadingSupport { get; protected set; }

        /// <summary>
        /// </summary>
        public bool Unsafe { get; protected set; }

        /// <summary>
        /// </summary>
        public bool GcDebug { get; protected set; }

        /// <summary>
        /// </summary>
        public bool Gctors { get; protected set; }

        /// <summary>
        /// </summary>
        public bool HasMethodThis { get; set; }

        /// <summary>
        /// </summary>
        public ILocalVariable[] LocalInfo { get; set; }

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
        public IParameter[] Parameters { get; set; }

        /// <summary>
        /// </summary>
        public IModule Module { get; private set; }

        /// <summary>
        /// </summary>
        public IType ThisType { get; set; }

        /// <summary>
        /// </summary>
        public SystemTypes System { get; protected set; }

        /// <summary>
        /// </summary>
        public IIlReader IlReader { get; set; }

        /// <summary>
        /// </summary>
        protected IExceptionHandlingClause[] ExceptionHandlingClauses { get; private set; }

        /// <summary>
        /// </summary>
        protected IType[] GenericMethodArguments { get; private set; }

        /// <summary>
        /// </summary>
        protected bool[] LocalInfoUsed { get; private set; }

        /// <summary>
        /// </summary>
        protected IMethod MainMethod { get; set; }

        /// <summary>
        /// </summary>
        protected int MainDebugInfoStartLine { get; set; }

        /// <summary>
        /// </summary>
        public IType MethodReturnType { get; set; }

        /// <summary>
        /// </summary>
        protected bool NoBody { get; private set; }

        /// <summary>
        /// </summary>
        protected List<OpCodePart> Ops { get; private set; }

        public static bool IsAssemblyNamespaceRequired(IType type, IMethod method = null, IType ownerOfExplicitInterface = null)
        {
            if (type.IsGenericType || type.IsGenericTypeDefinition || type.IsArray || type.IsModule)
            {
                return true;
            }

            if (method != null && (method.IsGenericMethod || method.IsGenericMethodDefinition))
            {
                return true;
            }

            if (ownerOfExplicitInterface != null && (ownerOfExplicitInterface.IsGenericType || ownerOfExplicitInterface.IsGenericTypeDefinition || ownerOfExplicitInterface.IsArray))
            {
                return true;
            }

            return false;
        }
        
        public void Initialize(IType type)
        {
            Debug.Assert(type != null, "You should provide type here");

            this.Module = type.Module;
            this.System = new SystemTypes(this.Module);
            MethodBodyBank.Clear();
            StringGen.ResetClass();
            ArraySingleDimensionGen.ResetClass();
        }

        public virtual string GetAllocator(bool isAtomic, bool isBigObj, bool debugOrigignalRequired)
        {
            string allocator;
            if (isBigObj)
            {
                allocator = isAtomic ? "GC_MALLOC_ATOMIC_IGNORE_OFF_PAGE" : "GC_MALLOC_IGNORE_OFF_PAGE";
                if (debugOrigignalRequired)
                {
                    return string.Concat(allocator, "_ORIGINAL");
                }

                return allocator;
            }

            allocator = isAtomic ? "GC_MALLOC_ATOMIC" : "GC_MALLOC";
            if (debugOrigignalRequired)
            {
                return string.Concat(allocator, "_ORIGINAL");
            }

            return allocator;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        public void AddOpCode(OpCodePart opCode)
        {
            this.Ops.Add(opCode);
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
        /// <returns>
        /// </returns>
        public IEnumerable<OpCodePart> PrepareWritingMethodBody(IMethod method)
        {
            var ops = this.PreProcessOpCodes(this.Ops, method).ToList();
            this.BuildAddressIndexes(ops);
            this.AssignJumpBlocks(ops);
            this.ProcessAll(ops);
            this.AssignExceptionsToOpCodes();
            this.SanitizePointerOperations(ops);
            this.InsertMissingTypeCasts(ops);
            return ops;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="genericContext">
        /// </param>
        public void ReadMethodInfo(IMethod methodInfo, IGenericContext genericContext)
        {
            var parameters = methodInfo.GetParameters();
            this.Parameters = parameters != null ? parameters.ToArray() : new IParameter[0];
            this.HasMethodThis = methodInfo.CallingConvention.HasFlag(CallingConventions.HasThis);

            this.MethodReturnType = null;
            this.ReadThisTypeInfo(methodInfo);

            var methodBody = methodInfo.ResolveMethodBody(genericContext);
            this.NoBody = !methodBody.HasBody;
            if (!this.NoBody)
            {
                this.LocalInfo = methodBody.LocalVariables.ToArray();

#if DEBUG
                Debug.Assert(
                    genericContext == null || !this.LocalInfo.Any(li => li.LocalType.IsGenericParameter),
                    "Has Generic Parameter");
#endif

                this.LocalInfoUsed = new bool[this.LocalInfo.Length];
                this.ExceptionHandlingClauses = methodBody.ExceptionHandlingClauses.ToArray();
            }

            this.MethodReturnType = !methodInfo.ReturnType.IsVoid() ? methodInfo.ReturnType : null;
        }

        /// <summary>
        /// </summary>
        /// <param name="fullTypeName">
        /// </param>
        /// <returns>
        /// </returns>
        public IType ResolveType(string fullTypeName, IGenericContext genericContext = null)
        {
            if (genericContext != null && !genericContext.IsEmpty)
            {
                return this.Module.ResolveType(fullTypeName, genericContext);
            }

            IType result;
            if (this.ResolvedTypes.TryGetValue(fullTypeName, out result))
            {
                return result;
            }

            if (GlobalResolvedTypes.Count > 0 && GlobalResolvedTypes.TryGetValue(fullTypeName, out result))
            {
                return result;
            }

            result = this.Module.ResolveType(fullTypeName, null);
            this.ResolvedTypes[result.FullName] = result;
            return result;
        }

        public void RegisterType(IType type)
        {
            GlobalResolvedTypes[type.FullName] = type;
        }

        public string GetStaticFieldName(IField field)
        {
            if (IsAssemblyNamespaceRequired(field.DeclaringType))
            {
                return string.Concat(field.FullName.CleanUpName(), "_", this.AssemblyQualifiedName.CleanUpName());
            }

            return field.FullName.CleanUpName();
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="doNotUseCachedResult">
        /// </param>
        /// <returns>
        /// </returns>
        public ReturnResult EstimatedResultOf(OpCodePart opCode, bool doNotUseCachedResult = false, bool ignoreAlternativeValues = false, bool exactType = false)
        {
            if (!doNotUseCachedResult && opCode.HasResult && opCode.Result.Type != null)
            {
                return new ReturnResult(opCode.Result);
            }

            if (!ignoreAlternativeValues && opCode.UsedByAlternativeValues != null && opCode.UsedByAlternativeValues.RequiredOutgoingType != null)
            {
                return new ReturnResult(opCode.UsedByAlternativeValues.RequiredOutgoingType);
            }

            if (!doNotUseCachedResult && opCode.RequiredOutgoingType != null)
            {
                return new ReturnResult(opCode.RequiredOutgoingType);
            }

            return new ReturnResult(this.RequiredOutgoingType(opCode, ignoreAlternativeValues, exactType));
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
                    Debug.Assert(opCodePart.TryEnd == null, "Try is null");
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
                        Debug.Assert(opCodePart.CatchOrFinallyBegin == null, "CatchOrFinallyBegin is null");
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
                        Debug.Assert(this.OpsByAddressStart.ContainsKey(nextAddress));
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
                    }
                }
            }
        }

        public enum ConversionType
        {
            None,
            BaseToDerived,
            DerivedToBase,
            ObjectToInterface,
            InterfaceToObject,
            PointerToValue,
            ValueToPointer,
            PointerToBoxedValue,
            IntPtrToInt,
            PointerToInt,
            IntToPointer,
            ArrayOfDerivedTypeToArrayOfBaseType,
            ArrayOfInterfaceToArrayOfObject,
            ArrayOfObjectToArrayOfInterface,
            ValueToStructureType,
            CCast
        }

        protected void InsertMissingTypeCasts(IEnumerable<OpCodePart> opCodes)
        {
            foreach (var opCodePart in opCodes)
            {
                if (opCodePart.OpCodeOperands != null && opCodePart.OpCodeOperands.Length > 0)
                {
                    var index = -1;
                    foreach (var opCodeOperand in opCodePart.OpCodeOperands)
                    {
                        index++;
                        if (opCodeOperand.UsedByAlternativeValues != null)
                        {
                            // it will be process as value for alternatives
                            continue;
                        }

                        this.InsertCastFixOperation(opCodeOperand, new UsedByInfo(opCodePart, index));
                    }
                }

                if (opCodePart.AlternativeValues != null)
                {
                    // adjust when you set phi nodes
                    foreach (var alternativeValue in opCodePart.AlternativeValues)
                    {
                        var index = 0;
                        foreach (var opCodeOperand in alternativeValue.Values.ToArray())
                        {
                            var insertCastFixOperation = this.InsertCastFixOperation(opCodeOperand);
                            opCodeOperand.UsedByAlternativeValues = null;
                            alternativeValue.Values[index++] = insertCastFixOperation;
                            insertCastFixOperation.UsedByAlternativeValues = alternativeValue;
                        }
                    }

                    // adjust when you read phi noes
                    foreach (var alternativeValue in opCodePart.AlternativeValues)
                    {
                        foreach (var opCodeOperand in alternativeValue.Values.ToArray())
                        {
                            if (opCodeOperand.UsedBy == null)
                            {
                                continue;
                            }

                            this.InsertCastFixOperation(opCodeOperand, opCodeOperand.UsedBy);
                        }
                    }
                }
            }
        }

        private OpCodePart InsertCastFixOperation(OpCodePart opCodeOperand, UsedByInfo actualUsedByInfo = null)
        {
            IType destinationType;
            var conversionType = this.GetConversionType(opCodeOperand, actualUsedByInfo, out destinationType);
            if (conversionType == ConversionType.None)
            {
                return opCodeOperand;
            }

            if (IsCastConversion(conversionType))
            {
                var castOpCode = new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, destinationType);
                castOpCode.RequiredOutgoingType = destinationType;
                this.InsertOperand(opCodeOperand, castOpCode);
                return castOpCode;
            }

            if (IsLoadObjectConversion(conversionType))
            {
                var castOpCode = new OpCodeTypePart(OpCodesEmit.Ldobj, 0, 0, destinationType);
                this.InsertOperand(opCodeOperand, castOpCode);
                return castOpCode;
            }

            if (conversionType == ConversionType.PointerToBoxedValue)
            {
                var type = destinationType.ToNormal();
                var ldobjOpCode = new OpCodeTypePart(OpCodesEmit.Ldobj, 0, 0, type);
                this.InsertOperand(opCodeOperand, ldobjOpCode);
                var boxOpCode = new OpCodeTypePart(OpCodesEmit.Box, 0, 0, type);
                boxOpCode.RequiredOutgoingType = type.ToClass();
                this.InsertOperand(ldobjOpCode, boxOpCode);
                return boxOpCode;
            }

            if (conversionType == ConversionType.IntPtrToInt)
            {
                var field = this.System.System_IntPtr.GetFieldByFieldNumber(0, this);
                var ldfldOpCode = new OpCodeFieldInfoPart(OpCodesEmit.Ldfld, 0, 0, field);
                this.InsertOperand(opCodeOperand, ldfldOpCode);
                var castOpCode = new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, destinationType);
                castOpCode.RequiredOutgoingType = destinationType;
                this.InsertOperand(ldfldOpCode, castOpCode);
                return castOpCode;
            }

            if (conversionType == ConversionType.ValueToStructureType)
            {
                // cast to type of first field
                var fieldType = destinationType.GetFieldByFieldNumber(0, this).FieldType;
                var castOpCode = new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, fieldType);
                castOpCode.RequiredOutgoingType = fieldType;
                this.InsertOperand(opCodeOperand, castOpCode);
                return castOpCode;
            }

            return opCodeOperand;
        }

        private static bool IsCastConversion(ConversionType conversionType)
        {
            switch (conversionType)
            {
                case ConversionType.CCast:
                case ConversionType.BaseToDerived:
                case ConversionType.DerivedToBase:
                case ConversionType.ObjectToInterface:
                case ConversionType.InterfaceToObject:
                case ConversionType.PointerToInt:
                case ConversionType.IntToPointer:
                case ConversionType.ArrayOfDerivedTypeToArrayOfBaseType:
                case ConversionType.ArrayOfInterfaceToArrayOfObject:
                case ConversionType.ArrayOfObjectToArrayOfInterface:
                    return true;
            }

            return false;
        }

        private static bool IsLoadObjectConversion(ConversionType conversionType)
        {
            switch (conversionType)
            {
                case ConversionType.PointerToValue:
                    return true;
            }

            return false;
        }

        /// <summary>
        /// actualUsedByInfo not equals to opCodeOperand.UsedBy in case of Code.Dup
        /// </summary>
        private ConversionType GetConversionType(OpCodePart opCodeOperand, UsedByInfo actualUsedByInfo, out IType destinationType)
        {
            destinationType = null;

            var usedByInfo = actualUsedByInfo ?? opCodeOperand.UsedBy;
            var usedByAlternativeValues = opCodeOperand.UsedByAlternativeValues;
            if (usedByInfo == null && usedByAlternativeValues == null)
            {
                Debug.Assert(false, "OpCode is not used at all");
                return ConversionType.None;
            }

            var sourceType = opCodeOperand.RequiredOutgoingType ?? this.RequiredOutgoingType(opCodeOperand);
            if (actualUsedByInfo == null && usedByAlternativeValues != null)
            {
                // in case of AlternativeValues
                if (usedByAlternativeValues.RequiredOutgoingType != null)
                {
                    usedByInfo = null;
                    destinationType = usedByAlternativeValues.RequiredOutgoingType;
                }
            }

            if (usedByInfo != null)
            {
                var requiredIncomingTypes = usedByInfo.OpCode.RequiredIncomingTypes;
                destinationType = requiredIncomingTypes != null ? requiredIncomingTypes[usedByInfo.OperandPosition] : null;
            }

            if (sourceType == null || destinationType == null)
            {
                return ConversionType.None;
            }

            if (sourceType.TypeEquals(destinationType))
            {
                return ConversionType.None;
            }

            if (sourceType.GetAllInterfaces().Contains(destinationType))
            {
                return ConversionType.ObjectToInterface;
            }

            if (sourceType.UseAsClass && !destinationType.UseAsClass && sourceType.ToNormal().TypeEquals(destinationType))
            {
                return ConversionType.PointerToValue;           
            }

            if (sourceType.IsPointer && !destinationType.UseAsClass && sourceType.GetElementType().TypeEquals(destinationType))
            {
                return ConversionType.PointerToValue;
            }

            if (sourceType.IsPointer && destinationType.UseAsClass && destinationType.ToNormal().TypeEquals(sourceType.GetElementType())
                && !sourceType.GetElementType().IsStructureType())
            {
                return ConversionType.PointerToBoxedValue;
            }

            if (sourceType.IsDerivedFrom(destinationType))
            {
                return ConversionType.DerivedToBase;
            }

            if (sourceType.IsInterface && destinationType.IsObject)
            {
                return ConversionType.InterfaceToObject;
            }

            if ((sourceType.IsPointer || sourceType.IsByRef) && (destinationType.IsPointer || destinationType.IsByRef) &&
                sourceType.GetElementType().TypeNotEquals(destinationType.GetElementType()))
            {
                return ConversionType.CCast;
            }

            if (sourceType.IsVoidPointer() && destinationType.IntTypeBitSize() >= 8 * CWriter.PointerSize)
            {
                return ConversionType.CCast;
            }

            // in case we send pointer to call method
            if (sourceType.IsPointer && destinationType.UseAsClass && sourceType.GetElementType().TypeNotEquals(destinationType.ToNormal()))
            {
                return ConversionType.CCast;
            }

            if (destinationType.IsDerivedFrom(sourceType))
            {
                return ConversionType.BaseToDerived;
            }

            if (sourceType.IsArray && destinationType.IsArray && sourceType.GetElementType().IsDerivedFrom(destinationType.GetElementType()))
            {
                return ConversionType.ArrayOfDerivedTypeToArrayOfBaseType;
            }

            // TODO: warning: casting Array of Interfaces to Array of Objects (may not work)
            if (sourceType.IsArray && destinationType.IsArray && sourceType.GetElementType().IsInterface && destinationType.GetElementType().IsObject)
            {
                return ConversionType.ArrayOfInterfaceToArrayOfObject;
            }

            // TODO: warning: casting Array of Objects to Array of Generic Interfaces (may not work)
            if (sourceType.IsArray && destinationType.IsInterface)
            {
                return ConversionType.ArrayOfObjectToArrayOfInterface;
            }

            if (sourceType.IsIntPtrOrUIntPtr() && destinationType.IntTypeBitSize() >= 8 * CWriter.PointerSize)
            {
                return ConversionType.IntPtrToInt;
            }

            if (sourceType.IsPointer && destinationType.IntTypeBitSize() >= 8 * CWriter.PointerSize)
            {
                return ConversionType.PointerToInt;
            }

            if (sourceType.IntTypeBitSize() >= 8 * CWriter.PointerSize && destinationType.IsPointer)
            {
                return ConversionType.IntToPointer;
            }

            if (sourceType.IsPrimitiveTypeOrEnum() && destinationType.IsStructureType())
            {
                return ConversionType.ValueToStructureType;
            }

            // in case we need to cast object to other object using Object inheritance
            if (sourceType.IsReference() && destinationType.IsReference() && sourceType.NormalizeType().TypeNotEquals(destinationType.NormalizeType()))
            {
                return ConversionType.CCast;
            }

            // cast Int(S) to UInt(S) and vice versa
            if ((sourceType.IsSignedType() && destinationType.IsUnsignedType() || sourceType.IsUnsignedType() && destinationType.IsSignedType())
                && sourceType.IntTypeBitSize() == destinationType.IntTypeBitSize())
            {
                return ConversionType.CCast;
            }

            return ConversionType.None;
        }

        protected void SanitizePointerOperations(IEnumerable<OpCodePart> opCodes)
        {
            foreach (var opCodePart in opCodes)
            {
                switch (opCodePart.ToCode())
                {
                    case Code.Add:
                    case Code.Add_Ovf:
                    case Code.Add_Ovf_Un:
                    case Code.Sub:
                    case Code.Sub_Ovf:
                    case Code.Sub_Ovf_Un:

                        // TO FIX pointer operations;

                        var opCodeOperand0 = opCodePart.OpCodeOperands[0];
                        var opCodeOperand1 = opCodePart.OpCodeOperands[1];
                        var op0 = EstimatedResultOf(opCodeOperand0);
                        var op1 = EstimatedResultOf(opCodeOperand1);
                        if (op0.Type.IsPointer && (!op1.Type.IsPointer || IsPointerConvert(opCodeOperand1)))
                        {
                            this.FixAddSubPointerOperation(opCodePart, opCodeOperand0, opCodeOperand1, op0, op1);
                        }
                        else if (op1.Type.IsPointer && (!op0.Type.IsPointer || IsPointerConvert(opCodeOperand0)))
                        {
                            this.FixAddSubPointerOperation(opCodePart, opCodeOperand1, opCodeOperand0, op1, op0);
                        }
                        else if (op0.Type.IsPointer && op1.Type.IsPointer
                                 && (op0.Type.GetElementType().TypeNotEquals(op1.Type.GetElementType()) || op0.Type.IsVoidPointer() || op1.Type.IsVoidPointer()))
                        {
                            if (op0.Type.GetElementType().TypeNotEquals(System.System_Byte))
                            {
                                var pointerType = this.System.System_Byte.ToPointerType();
                                this.InsertOperand(opCodeOperand0, new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, pointerType));
                                // to force recalculation
                                opCodePart.RequiredOutgoingType = pointerType;
                            }

                            if (op1.Type.GetElementType().TypeNotEquals(System.System_Byte))
                            {
                                var pointerType = this.System.System_Byte.ToPointerType();
                                this.InsertOperand(opCodeOperand1, new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, pointerType));
                                // to force recalculation
                                opCodePart.RequiredOutgoingType = pointerType;
                            }
                        }

                        break;

                    case Code.Div:
                    case Code.Div_Un:

                        opCodeOperand0 = opCodePart.OpCodeOperands[0];
                        opCodeOperand1 = opCodePart.OpCodeOperands[1];
                        op0 = EstimatedResultOf(opCodeOperand0);
                        op1 = EstimatedResultOf(opCodeOperand1);

                        if (op0.Type.IsPointer && (!op1.Type.IsPointer || IsPointerConvert(opCodeOperand1)))
                        {
                            this.FixDivPointerOperation(opCodePart, opCodeOperand0, op0, opCodeOperand1, 0);
                        }
                        else if (op1.Type.IsPointer && (!op0.Type.IsPointer || IsPointerConvert(opCodeOperand0)))
                        {
                            this.FixDivPointerOperation(opCodePart, opCodeOperand1, op1, opCodeOperand0, 1);
                        }

                        break;
                }
            }
        }

        private void FixAddSubPointerOperation(OpCodePart opCodePart, OpCodePart opCodeOperand0, OpCodePart opCodeOperand1, ReturnResult op0, ReturnResult op1)
        {
            var pointerOpFixed = this.FixPointerOperation(opCodeOperand0, opCodeOperand1, op0.Type.GetElementType());
            if (!pointerOpFixed && this.GetIntegerValueFromOpCode(opCodeOperand0) <= 0 && !op1.Type.IsPointer)
            {
                var pointerType = this.System.System_Byte.ToPointerType();
                this.InsertOperand(opCodeOperand0, new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, pointerType));
                // to force recalculation
                opCodePart.RequiredOutgoingType = pointerType;
            }
        }

        private void FixDivPointerOperation(OpCodePart opCodePart, OpCodePart opCodeOperand, ReturnResult op, OpCodePart opCodeOperandOther, int useOperand)
        {
            var elementType = op.Type.GetElementType();
            var typePart = opCodeOperandOther as OpCodeTypePart;
            var integerValueFromOpCode = this.GetIntegerValueFromOpCode(opCodeOperandOther);
            if ((opCodeOperandOther.Any(Code.Sizeof) && typePart != null && elementType.TypeEquals(typePart.Operand))
                || elementType.GetKnownTypeSize() == integerValueFromOpCode)
            {
                this.ReplaceOperand(opCodePart, opCodeOperand);
            }
            else if (1 == integerValueFromOpCode)
            {
                // in case pointers casted to Byte*
                switch (opCodeOperand.ToCode())
                {
                    case Code.Add:
                    case Code.Add_Ovf:
                    case Code.Add_Ovf_Un:
                    case Code.Sub:
                    case Code.Sub_Ovf:
                    case Code.Sub_Ovf_Un:
                        this.InsertOperand(opCodeOperand.OpCodeOperands[0], new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, this.System.System_Byte.ToPointerType()));
                        this.InsertOperand(opCodeOperand.OpCodeOperands[1], new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, this.System.System_Byte.ToPointerType()));
                        // to force recalculation
                        opCodePart.RequiredOutgoingType = null;
                        break;
                    default:
                        this.InsertOperand(opCodeOperand, new OpCodeTypePart(OpCodesEmit.Castclass, 0, 0, this.System.System_Byte.ToPointerType()));
                        // to force recalculation
                        opCodePart.RequiredOutgoingType = null;
                        break;
                }

                this.ReplaceOperand(opCodePart, opCodeOperand);
            }
        }

        private static bool IsPointerConvert(OpCodePart opCodeOperand1)
        {
            return opCodeOperand1.Any(Code.Conv_I, Code.Conv_Ovf_I, Code.Conv_Ovf_I, Code.Conv_U, Code.Conv_Ovf_U, Code.Conv_Ovf_U_Un);
        }

        private bool FixPointerOperation(OpCodePart pointer, OpCodePart index, IType type)
        {
            var typeSize = type.GetKnownTypeSize();
            var opCodePart = index;
            switch (opCodePart.ToCode())
            {
                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:

                    if (opCodePart.OpCodeOperands[0].Any(Code.Mul))
                    {
                        opCodePart = opCodePart.OpCodeOperands[0];
                        goto case Code.Mul;
                    }

                    var value = GetIntegerValueFromOpCode(opCodePart.OpCodeOperands[0]);
                    if (value > 0 && typeSize > 0 && value % typeSize == 0)
                    {
                        this.ReplaceOperand(opCodePart.OpCodeOperands[0], new OpCodeInt32Part(OpCodesEmit.Ldc_I4, 0, 0, (int)value / typeSize));
                        return true;
                    }

                    break;

                case Code.Mul:
                    var opCodeOperand0 = opCodePart.OpCodeOperands[0];
                    var opCodeOperand1 = opCodePart.OpCodeOperands[1];

                    var op0Size = GetIntegerValueFromOpCode(opCodeOperand0);
                    var op1Size = GetIntegerValueFromOpCode(opCodeOperand1);

                    // case '* sizepf'
                    if (opCodeOperand0.Any(Code.Sizeof) && (opCodeOperand0 as OpCodeTypePart).Operand.Equals(type))
                    {
                        // disable which mul
                        this.ReplaceOperand(opCodePart, opCodeOperand1);
                        return true;
                    }

                    // case '* sizepf'
                    if (opCodeOperand1.Any(Code.Sizeof) && (opCodeOperand1 as OpCodeTypePart).Operand.Equals(type))
                    {
                        // disable which mul
                        this.ReplaceOperand(opCodePart, opCodeOperand0);
                        return true;
                    }

                    // case '* sizepf'
                    if (op0Size > 0 && typeSize == op0Size)
                    {
                        // disable which mul
                        this.ReplaceOperand(opCodePart, opCodeOperand1);
                        return true;
                    }

                    // case '* sizepf'
                    if (op1Size > 0 && typeSize == op1Size)
                    {
                        // disable which mul
                        this.ReplaceOperand(opCodePart, opCodeOperand0);
                        return true;
                    }

                    break;
            }

            return false;
        }

        protected long GetIntegerValueFromOpCode(OpCodePart opCodePart)
        {
            switch (opCodePart.ToCode())
            {
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                    var asString = opCodePart.ToCode().ToString();
                    return Int32.Parse(asString.Substring(asString.Length - 1, 1));
                case Code.Ldc_I4_M1:
                    return -1;
                case Code.Ldc_I4:
                    var opCodeInt32 = opCodePart as OpCodeInt32Part;
                    return opCodeInt32.Operand;
                case Code.Ldc_I4_S:
                    opCodeInt32 = opCodePart as OpCodeInt32Part;
                    return opCodeInt32.Operand > 127 ? -(256 - opCodeInt32.Operand) : opCodeInt32.Operand;
                case Code.Ldc_I8:
                    var opCodeInt64 = opCodePart as OpCodeInt64Part;
                    return opCodeInt64.Operand;
                case Code.Castclass:
                    return this.GetIntegerValueFromOpCode(opCodePart.OpCodeOperands[0]);
                case Code.Conv_I:
                case Code.Conv_I1:
                case Code.Conv_I2:
                case Code.Conv_I4:
                case Code.Conv_I8:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                case Code.Conv_Ovf_I_Un:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                case Code.Conv_Ovf_U_Un:
                    return this.GetIntegerValueFromOpCode(opCodePart.OpCodeOperands[0]);
            }

            return -2;
        }

        private void ReplaceOperand(OpCodePart oldOperand, OpCodePart newOperand, bool doNotChangeFlowOrder = false)
        {
            var usedByInfo = oldOperand.UsedBy;
            OpCodePart usedByOpCodePart = usedByInfo.OpCode;
            usedByOpCodePart.OpCodeOperands[usedByInfo.OperandPosition] = newOperand;
            newOperand.UsedBy = new UsedByInfo(usedByOpCodePart, usedByInfo.OperandPosition);
            // do not remove usedby to prevent it to be written at first level
            ////usedByOpCodePart.UsedBy = null;

            if (!doNotChangeFlowOrder)
            {
                var isNew = newOperand.Next == null && newOperand.Previous == null;

                newOperand.Next = oldOperand.Next;
                if (isNew)
                {
                    oldOperand.Previous.Next = newOperand;
                }
                else if (oldOperand.Previous != null)
                {
                    oldOperand.Previous.Next = oldOperand.Next;
                }

                newOperand.Previous = oldOperand.Previous;
            }
        }

        private void InsertOperand(OpCodePart oldOperand, OpCodePart newOperand)
        {
            var usedByInfo = oldOperand.UsedBy;
            if (usedByInfo != null)
            {
                OpCodePart usedByOpCodePart = usedByInfo.OpCode;
                usedByOpCodePart.OpCodeOperands[usedByInfo.OperandPosition] = newOperand;
                newOperand.UsedBy = new UsedByInfo(usedByOpCodePart, usedByInfo.OperandPosition);
                newOperand.OpCodeOperands = new[] { oldOperand };
                oldOperand.UsedBy = new UsedByInfo(newOperand, 0);
            }
            else if (oldOperand.UsedByAlternativeValues != null)
            {
                // in case it is used in AlternativeValues
                var usedByAlternativeValues = oldOperand.UsedByAlternativeValues;
                for (var index = 0; index < usedByAlternativeValues.Values.Count; index++)
                {
                    var opCodePart = usedByAlternativeValues.Values[index];
                    if (opCodePart == oldOperand)
                    {
                        usedByAlternativeValues.Values[index] = newOperand;
                        newOperand.OpCodeOperands = new[] { oldOperand };
                        oldOperand.UsedBy = new UsedByInfo(newOperand, 0);
                        break;
                    }
                }
            }
            else
            {
                Debug.Assert(false, "Could not insert operand as it is not used");
            }

            newOperand.Next = oldOperand.Next;
            newOperand.Previous = oldOperand;
            oldOperand.Next = newOperand;
        }

        private void CalculateRequiredTypesForAlternativeValues(OpCodePart opCodePart)
        {
            if (opCodePart.AlternativeValues == null)
            {
                return;
            }

            foreach (var alternativeValues in opCodePart.AlternativeValues)
            {
                this.CalculateRequiredTypesForAlternativeValues(alternativeValues);
            }
        }

        private void CalculateRequiredTypesForAlternativeValues(PhiNodes alternativeValues)
        {
            if (alternativeValues.RequiredOutgoingType != null)
            {
                return;
            }

            if (alternativeValues.UsedByAlternativeValues != null && alternativeValues != alternativeValues.UsedByAlternativeValues)
            {
                CalculateRequiredTypesForAlternativeValues(alternativeValues.UsedByAlternativeValues);
            }

            // before we need to ensure all requiredOutgoingTypes are calculated
            foreach (var opCodeValue in alternativeValues.Values.Where(v => v.RequiredOutgoingType == null))
            {
                opCodeValue.RequiredOutgoingType = RequiredOutgoingType(opCodeValue, true);
            }

            OpCodePart opCodeUsedFromAlternativeValues = null;
            IType requiredType = null;

            // detect required types in alternative values
            opCodeUsedFromAlternativeValues = GetUsedByFromAlternativeValues(alternativeValues);
            if (opCodeUsedFromAlternativeValues != null)
            {
                var opCodeUsingAlternativeValues = opCodeUsedFromAlternativeValues.UsedBy;

                requiredType = opCodeUsingAlternativeValues.OpCode.RequiredIncomingTypes != null
                                   ? alternativeValues.Values.Where(v => v != null && v.UsedBy != null && !v.UsedBy.Any(Code.Pop))
                                                      .Select(v => opCodeUsingAlternativeValues.OpCode.RequiredIncomingTypes[v.UsedBy.OperandPosition])
                                                      .LastOrDefault(v => v != null)
                                   : null;

                requiredType = requiredType
                               ?? this.RequiredOutgoingType(opCodeUsedFromAlternativeValues, true)
                               ?? alternativeValues.Values.Select(v => v.RequiredOutgoingType).FirstOrDefault(v => v != null)
                               ?? this.EstimatedResultOf(opCodeUsedFromAlternativeValues, ignoreAlternativeValues: true).Type;
            }
            else
            {
                requiredType = alternativeValues.Values.Select(v => v.RequiredOutgoingType).FirstOrDefault(v => v != null);
            }

            if (requiredType != null && alternativeValues.Values.Any(v => requiredType.TypeNotEquals(v.RequiredOutgoingType)))
            {
                // find base type, for example if first value is IDictionary and second is Object then required type should be Object
                foreach (var alternateValueOutgoingType in
                    alternativeValues.Values.Select(v => v.RequiredOutgoingType)
                                     .Where(t => t != null)
                                     .Where(alternateValueOutgoingType => requiredType.TypeNotEquals(alternateValueOutgoingType)))
                {
                    if ((requiredType.IsDerivedFrom(alternateValueOutgoingType) || requiredType.GetAllInterfaces().Contains(alternateValueOutgoingType))
                        || alternateValueOutgoingType.IsObject)
                    {
                        requiredType = alternateValueOutgoingType;
                        continue;
                    }

                    // special case when ValueType used in mixture with ValueTypePointer
                    if (alternateValueOutgoingType.IsValueType && requiredType.NormalizeType().TypeEquals(alternateValueOutgoingType))
                    {
                        requiredType = alternateValueOutgoingType;
                    }
                }
            }

            Debug.Assert(requiredType != null, "Required Type for Phi nodes can't be calculated");
            alternativeValues.RequiredOutgoingType = requiredType;
        }

        private static OpCodePart GetUsedByFromAlternativeValues(PhiNodes phiNodes)
        {
            OpCodePart opCodeUsedFromAlternativeValues = null;
            while (opCodeUsedFromAlternativeValues == null)
            {
                opCodeUsedFromAlternativeValues = phiNodes.Values.LastOrDefault(v => v != null && v.UsedBy != null && !v.UsedBy.Any(Code.Pop));
                if (opCodeUsedFromAlternativeValues == null)
                {
                    var usedByAlternativeValues = phiNodes.Values.LastOrDefault(v => v.UsedByAlternativeValues != null);
                    if (usedByAlternativeValues != null && phiNodes != usedByAlternativeValues.UsedByAlternativeValues)
                    {
                        phiNodes = usedByAlternativeValues.UsedByAlternativeValues;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return opCodeUsedFromAlternativeValues;
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
                var opCodePartUsed = this.PopValue(isVarArg);
                if (isVarArg && opCodePartUsed == null)
                {
                    break;
                }

                opCodeParts.Insert(0, opCodePartUsed);
            }

            opCodePart.OpCodeOperands = opCodeParts.ToArray();
            var operandPosition = 0;
            foreach (var childCodePart in opCodeParts)
            {
                childCodePart.UsedBy = new UsedByInfo(opCodePart, operandPosition++);
            }

            // to support Code.Constrained
            if (opCodePart.ToCode() == Code.Callvirt && opCodePart.Previous.ToCode() == Code.Constrained)
            {
                var constrained = opCodePart.Previous;
                var firstOperand = opCodePart.OpCodeOperands[0];
                this.ReplaceOperand(firstOperand, constrained, doNotChangeFlowOrder: true);
                constrained.OpCodeOperands = new[] { firstOperand };
                firstOperand.UsedBy = new UsedByInfo(constrained, 0);

                this.RequiredIncomingOutgoingTypes(constrained);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="operandIndex">
        /// </param>
        /// <returns>
        /// </returns>
        protected IType GetTypeOfReference(OpCodePart opCode, int operandIndex = 0)
        {
            IType type = null;
            if (opCode.HasResult)
            {
                type = opCode.Result.Type;
            }
            else if (opCode.RequiredOutgoingType != null)
            {
                type = opCode.RequiredOutgoingType;
                return type;
            }
            else if (opCode.OpCodeOperands != null && opCode.OpCodeOperands.Length > operandIndex)
            {
                var resultOf = this.EstimatedResultOf(opCode.OpCodeOperands[operandIndex]);
                type = resultOf.Type;
            }

            if (type.IsArray || type.IsByRef || type.IsPointer)
            {
                return type.GetElementType();
            }

            return type;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        protected OpCodePart InsertBeforeOpCode(OpCodePart opCode)
        {
            if (this.ExceptionHandlingClauses == null)
            {
                return null;
            }

            // insert result of exception
            var exceptionHandling =
                this.ExceptionHandlingClauses.FirstOrDefault(eh => eh.HandlerOffset == opCode.AddressStart);
            if (exceptionHandling == null || exceptionHandling.CatchType == null)
            {
                return null;
            }

            var opCodeNope = new OpCodePart(OpCodes.Newobj, opCode.AddressStart, opCode.AddressStart);
            opCodeNope.ReadExceptionFromStack = true;
            opCodeNope.ReadExceptionFromStackType = exceptionHandling.CatchType;
            return opCodeNope;
        }

        protected int IntOpBitSize(OpCodePart opCode)
        {
            return Math.Max(
                opCode.OpCodeOperands.Length > 0 ? this.IntOpOperandBitSize(opCode.OpCodeOperands[0]) : 0,
                opCode.OpCodeOperands.Length > 1 ? this.IntOpOperandBitSize(opCode.OpCodeOperands[1]) : 0);
        }

        protected int IntOpOperandBitSize(OpCodePart opCode)
        {
            var op1ReturnResult = this.EstimatedResultOf(opCode);

            if (op1ReturnResult == null || op1ReturnResult.Type == null)
            {
                return 0;
            }

            return op1ReturnResult.Type.IntTypeBitSize();
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        protected bool IsFloatingPointOp(OpCodePart opCode)
        {
            return (opCode.OpCodeOperands.Length > 0 && this.IsFloatingPointOpOperand(opCode.OpCodeOperands[0]))
                   || (opCode.OpCodeOperands.Length > 1 && this.IsFloatingPointOpOperand(opCode.OpCodeOperands[1]));
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        protected bool IsFloatingPointOpOperand(OpCodePart opCode)
        {
            var op1ReturnResult = this.EstimatedResultOf(opCode);

            // TODO: result of unbox is null, fix it
            if (op1ReturnResult == null || op1ReturnResult.Type == null)
            {
                return false;
            }

            var op1IsReal = !op1ReturnResult.Type.UseAsClass && op1ReturnResult.Type.IsReal();
            return op1IsReal;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        protected bool IsPointerArithmetic(OpCodePart opCode)
        {
            if (opCode == null || opCode.OpCodeOperands == null)
            {
                return false;
            }

            if (opCode.OpCodeOperands.Any(o => this.EstimatedResultOf(o).Type.IsPointer))
            {
                return true;
            }

            ////if (opCode.Any(Code.Add, Code.Add_Ovf, Code.Add_Ovf_Un, Code.Sub, Code.Sub_Ovf, Code.Sub_Ovf_Un))
            ////{
            ////    return true;
            ////}

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodes">
        /// </param>
        /// <returns>
        /// </returns>
        protected IEnumerable<OpCodePart> PreProcessOpCodes(IEnumerable<OpCodePart> opCodes, IMethod method)
        {
            OpCodePart last = null;
            if (method is IConstructor && method.IsStatic)
            {
                var zeroOpCode = new OpCodePart(OpCodesEmit.Ldc_I4_0, 0, 0);
                last = BuildChain(last, zeroOpCode);
                yield return zeroOpCode;
                var initializedField = method.DeclaringType.GetFieldByName(ObjectInfrastructure.CalledCctorFieldName, this);
                var setStaticFieldValueOpCode = new OpCodeFieldInfoPart(OpCodesEmit.Stsfld, 0, 0, initializedField);
                last = BuildChain(last, setStaticFieldValueOpCode);
                yield return setStaticFieldValueOpCode;
            }

            foreach (var opCodePart in opCodes)
            {
                var opCodePartBefore = this.InsertBeforeOpCode(opCodePart);
                if (opCodePartBefore != null)
                {
                    last = BuildChain(last, opCodePartBefore);
                    yield return opCodePartBefore;
                }

                last = BuildChain(last, opCodePart);
                yield return opCodePart;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        protected void Process(OpCodePart opCode)
        {
            this.Stacks.ProcessAlternativeValues(opCode);

            var code = opCode.ToCode();
            switch (code)
            {
                case Code.Call:
                    var methodBase = (opCode as OpCodeMethodInfoPart).Operand;
                    IEnumerable<IParameter> parameters = methodBase.GetParameters();
                    var size = (methodBase.CallingConvention.HasFlag(CallingConventions.HasThis) ? 1 : 0) +
                               (parameters != null
                                   ? parameters.Count()
                                   : 0);
                    this.FoldNestedOpCodes(
                        opCode,
                        size,
                        methodBase.CallingConvention.HasFlag(CallingConventions.VarArgs));
                    break;
                case Code.Callvirt:
                    methodBase = (opCode as OpCodeMethodInfoPart).Operand;
                    parameters = methodBase.GetParameters();
                    this.FoldNestedOpCodes(
                        opCode,
                        (code == Code.Callvirt ? 1 : 0) + parameters.Count(),
                        methodBase.CallingConvention.HasFlag(CallingConventions.VarArgs));
                    break;
                case Code.Newobj:
                    if (opCode.ReadExceptionFromStack)
                    {
                        break;
                    }

                    var ctorInfo = (opCode as OpCodeConstructorInfoPart).Operand;
                    this.FoldNestedOpCodes(
                        opCode,
                        (code == Code.Callvirt ? 1 : 0) + ctorInfo.GetParameters().Count(),
                        ctorInfo.CallingConvention.HasFlag(CallingConventions.VarArgs));
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
                case Code.Initblk:
                case Code.Cpblk:
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
                case Code.Cpobj:
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

            this.Stacks.SaveBranchStackValue(opCode, this);

            this.RequiredIncomingOutgoingTypes(opCode);

            // add to stack
            if (opCode.OpCode.StackBehaviourPush == StackBehaviour.Push0)
            {
                return;
            }

            var isItMethodWithVoid = opCode.OpCode.StackBehaviourPush == StackBehaviour.Varpush &&
                                     opCode is OpCodeMethodInfoPart
                                     && ((OpCodeMethodInfoPart)opCode).Operand.ReturnType.IsVoid();
            if (!isItMethodWithVoid)
            {
                if (opCode.Any(Code.Dup))
                {
                    var newDup = new OpCodePart(opCode.OpCode, opCode.AddressStart, 0);
                    newDup.OpCodeOperands = new[] { opCode };
                    this.Stacks.Push(newDup);

                    // make proper chain
                    newDup.Previous = opCode.Previous;
                    newDup.Next = opCode.Next;
                    opCode.Next = newDup;
                }

                this.Stacks.Push(opCode);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodes">
        /// </param>
        protected void ProcessAll(IEnumerable<OpCodePart> opCodes)
        {
            foreach (var opCodePart in opCodes)
            {
                this.Process(opCodePart);
            }
        }

        protected void ReadThisTypeInfo(IType type)
        {
            this.ThisType = type;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        protected void ReadThisTypeInfo(IMethod method)
        {
            this.ThisType = method.GetThisTypeForMethod();
        }

        protected bool IsSafeForMultilineCode(OpCodePart opCode)
        {
            var code = opCode.ToCode();
            switch (code)
            {
                case Code.Conv_R_Un:
                case Code.Conv_R4:
                case Code.Conv_R8:
                case Code.Conv_I1:
                case Code.Conv_I2:
                case Code.Conv_I4:
                case Code.Conv_I8:
                case Code.Conv_U1:
                case Code.Conv_U2:
                case Code.Conv_U4:
                case Code.Conv_U8:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Ldlen:
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
                    return true;
            }

            return false;
        }

        protected void RequiredIncomingOutgoingTypes(OpCodePart opCodePart)
        {
            if (opCodePart.OpCodeOperands == null)
            {
                return;
            }

            var index = 0;
            opCodePart.RequiredIncomingTypes = new RequiredIncomingTypes(opCodePart.OpCodeOperands.Length);
            foreach (var opCodeOperand in opCodePart.OpCodeOperands)
            {
                if (opCodeOperand.UsedByAlternativeValues != null)
                {
                    CalculateRequiredTypesForAlternativeValues(opCodeOperand.UsedByAlternativeValues);
                }

                opCodePart.RequiredIncomingTypes[index] = this.RequiredIncomingType(opCodePart, index++);
                opCodeOperand.RequiredOutgoingType = this.RequiredOutgoingType(opCodeOperand, true);
            }

            // TODO: double check if you process Code.Constrained
            // special case for constrained
            ////if (opCode.OpCode.StackBehaviourPush != StackBehaviour.Push0 || opCode.ToCode() == Code.Constrained)
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="operandPosition">
        /// </param>
        /// <returns>
        /// </returns>
        protected IType RequiredIncomingType(
            OpCodePart opCodePart,
            int operandPosition,
            bool forArithmeticOperations = false,
            bool exactType = false)
        {
            if (opCodePart.Discovering)
            {
                // to prevent cercular calls
                return null;
            }

            opCodePart.Discovering = true;

            try
            {

                IType retType = null;
                ReturnResult result = null;
                IType type = null;
                switch (opCodePart.ToCode())
                {
                    case Code.Ret:
                        retType = this.MethodReturnType;
                        return retType;

                    case Code.Stloc:
                    case Code.Stloc_0:
                    case Code.Stloc_1:
                    case Code.Stloc_2:
                    case Code.Stloc_3:
                    case Code.Stloc_S:
                        retType = opCodePart.GetLocalType(this);
                        return retType;

                    case Code.Starg:
                    case Code.Starg_S:
                        var index = opCodePart.GetArgIndex();
                        if (this.HasMethodThis && index == 0)
                        {
                            retType = this.ThisType;
                            return retType;
                        }

                        retType = this.GetArgType(index);
                        return retType;

                    case Code.Ldfld:
                        var operand = ((OpCodeFieldInfoPart)opCodePart).Operand;
                        retType = operandPosition == 0 ? operand.DeclaringType.ToClass() : null;
                        return retType;

                    case Code.Stsfld:
                        operand = ((OpCodeFieldInfoPart)opCodePart).Operand;
                        return this.MultiThreadingSupport && !exactType && operand.IsThreadStatic ? System.System_Object : operand.FieldType;
                    case Code.Stfld:
                        operand = ((OpCodeFieldInfoPart)opCodePart).Operand;
                        retType = operandPosition == 0 ? operand.DeclaringType.ToClass() : operand.FieldType;
                        return retType;

                    case Code.Stobj:
                        type = ((OpCodeTypePart)opCodePart).Operand;
                        retType = operandPosition == 1 ? type : null;
                        return retType;

                    case Code.Stind_Ref:
                        retType = null;
                        if (operandPosition == 1)
                        {
                            retType = this.RequiredOutgoingType(opCodePart.OpCodeOperands[0]);
                            if (retType == null)
                            {
                                retType = this.System.System_Void.ToPointerType();
                            }
                            else if (retType.IsByRef)
                            {
                                retType = retType.GetElementType();
                            }
                        }

                        return retType;

                    case Code.Stind_I:
                        return operandPosition == 1 ? this.GetTypeOfReference(opCodePart) : null;

                    case Code.Stind_I1:
                        if (operandPosition == 1)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            type = result.Type.HasElementType ? result.Type.GetElementType() : result.Type;
                            if (type.IsVoid() || type.IntTypeBitSize() > 8)
                            {
                                type = this.System.System_SByte;
                            }

                            return type;
                        }
                        else
                        {
                            return null;
                        }

                    case Code.Stind_I2:
                        return operandPosition == 1 ? this.System.System_Int16 : null;

                    case Code.Stind_I4:
                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Stind_I8:
                        return operandPosition == 1 ? this.System.System_Int64 : null;

                    case Code.Stind_R4:
                        return operandPosition == 1 ? this.System.System_Single : null;

                    case Code.Stind_R8:
                        return operandPosition == 1 ? this.System.System_Double : null;

                    case Code.Stelem_Ref:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0], true, exactType: true);
                            return result.Type;
                        }

                        return operandPosition == 1
                            ? this.System.System_Int32
                            : operandPosition == 2 ? this.GetTypeOfReference(opCodePart) : null;

                    case Code.Stelem_I:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return result.Type;
                        }

                        return operandPosition == 1
                                   ? this.System.System_Int32
                                   : operandPosition == 2 ? this.GetTypeOfReference(opCodePart) : null;

                    case Code.Stelem:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return ((OpCodeTypePart)opCodePart).Operand.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1
                            ? this.System.System_Int32
                            : operandPosition == 2 ? ((OpCodeTypePart)opCodePart).Operand : null;

                    case Code.Stelem_I1:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            type = result.Type.GetElementType();
                            if (type.IsVoid() || type.IntTypeBitSize() > 8)
                            {
                                type = this.System.System_SByte;
                            }

                            return type.ToArrayType(result.Type.ArrayRank);
                        }
                        else if (operandPosition == 2)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            type = result.Type.GetElementType();
                            if (type.IsVoid() || type.IntTypeBitSize() > 8)
                            {
                                type = this.System.System_SByte;
                            }

                            return type;
                        }
                        else
                        {
                            return operandPosition == 1 ? this.System.System_Int32 : null;
                        }

                    case Code.Stelem_I2:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Int16.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1
                                   ? this.System.System_Int32
                                   : operandPosition == 2 ? this.System.System_Int16 : null;

                    case Code.Stelem_I4:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Int32.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1
                                   ? this.System.System_Int32
                                   : operandPosition == 2 ? this.System.System_Int32 : null;

                    case Code.Stelem_I8:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Int64.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1
                                   ? this.System.System_Int32
                                   : operandPosition == 2 ? this.System.System_Int64 : null;

                    case Code.Stelem_R4:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Single.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1
                                   ? this.System.System_Int32
                                   : operandPosition == 2 ? this.System.System_Single : null;

                    case Code.Stelem_R8:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Double.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1
                                   ? this.System.System_Int32
                                   : operandPosition == 2 ? this.System.System_Double : null;

                    case Code.Ldlen:

                        result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0], true, exactType: true);
                        if (result.Type.IsArray)
                        {
                            return result.Type;
                        }

                        return this.System.System_Byte.ToArrayType(1);

                    case Code.Ldelem_I:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return result.Type;
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_I1:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_SByte.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_I2:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Int16.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_I4:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Int32.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_U1:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Byte.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_U2:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_UInt16.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_U4:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_UInt32.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_R4:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Single.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_R8:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return System.System_Double.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelem_Ref:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0], true, exactType: true);
                            return result.Type;
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Ldelema:

                        if (operandPosition == 0)
                        {
                            result = this.EstimatedResultOf(opCodePart.OpCodeOperands[0]);
                            return ((OpCodeTypePart)opCodePart).Operand.ToArrayType(result.Type.ArrayRank);
                        }

                        return operandPosition == 1 ? this.System.System_Int32 : null;

                    case Code.Unbox:
                    case Code.Unbox_Any:
                        if (operandPosition == 0)
                        {
                            retType = ((OpCodeTypePart)opCodePart).Operand;
                            return retType.ToClass();
                        }

                        return null;

                    case Code.Box:

                        if (operandPosition == 0)
                        {
                            retType = ((OpCodeTypePart)opCodePart).Operand;
                            if (retType.IsPointer)
                            {
                                return this.System.System_Int32;
                            }

                            return retType.UseAsClass ? retType.ToNormal() : retType;
                        }

                        return null;

                    case Code.Call:
                    case Code.Callvirt:
                        var effectiveOperandPosition = operandPosition;
                        var opCodePartMethod = opCodePart as OpCodeMethodInfoPart;
                        if (opCodePartMethod.Operand.CallingConvention.HasFlag(CallingConventions.HasThis))
                        {
                            if (operandPosition == 0)
                            {
                                var requiredIncomingType = opCodePartMethod.Operand.DeclaringType;
                                if (!requiredIncomingType.IsInterface)
                                {
                                    return opCodePartMethod.Operand.GetThisTypeForMethod();
                                }
                            }

                            effectiveOperandPosition--;
                        }

                        var parameters = opCodePartMethod.Operand.GetParameters();
                        index = 0;
                        if (parameters != null)
                        {
                            foreach (var parameter in parameters)
                            {
                                if (index == effectiveOperandPosition)
                                {
                                    retType = parameter.ParameterType;
                                    break;
                                }

                                index++;
                            }
                        }

                        break;

                    case Code.Newobj:

                        if (opCodePart.ReadExceptionFromStack)
                        {
                            retType = opCodePart.ReadExceptionFromStackType;
                            break;
                        }

                        effectiveOperandPosition = operandPosition;
                        var opCodeConstructorInfoPart = opCodePart as OpCodeConstructorInfoPart;
                        parameters = opCodeConstructorInfoPart.Operand.GetParameters();
                        index = 0;
                        foreach (var parameter in parameters)
                        {
                            if (index == effectiveOperandPosition)
                            {
                                retType = parameter.ParameterType;
                                break;
                            }

                            index++;
                        }

                        break;

                    case Code.Newarr:
                    case Code.Localloc:
                        return System.System_Int32;

                    case Code.Initblk:
                        if (operandPosition == 0)
                        {
                            return System.System_Byte.ToPointerType();
                        }
                        else if (operandPosition >= 1)
                        {
                            return System.System_Int32;
                        }

                        return null;

                    case Code.Cpblk:

                        if (operandPosition <= 1)
                        {
                            return System.System_Byte.ToPointerType();
                        }
                        else if (operandPosition == 2)
                        {
                            return System.System_Int32;
                        }

                        return null;

                    case Code.Dup:
                        // TODO: TEMP HACK
                        // to fix issue which changing type in RequiredOutgoing type when fixing Pointers in SanitizePointers func.
                        if (opCodePart.UsedBy == null)
                        {
                            if (!opCodePart.OpCodeOperands[0].Any(Code.Ldnull))
                            {
                                return this.RequiredOutgoingType(opCodePart.OpCodeOperands[0]);
                            }

                            return null;
                        }

                        return this.RequiredIncomingType(opCodePart.UsedBy.OpCode, opCodePart.UsedBy.OperandPosition);

                    case Code.Add_Ovf:
                    case Code.Sub_Ovf:
                    case Code.Mul_Ovf:
                        result = this.EstimatedResultOf(opCodePart.OpCodeOperands[operandPosition]);
                        if (result.Type.TypeEquals(System.System_Byte))
                        {
                            return System.System_SByte;
                        }

                        if (result.Type.TypeEquals(System.System_UInt16))
                        {
                            return System.System_Int16;
                        }

                        if (result.Type.TypeEquals(System.System_UInt32))
                        {
                            return System.System_Int32;
                        }

                        if (result.Type.TypeEquals(System.System_UInt64))
                        {
                            return System.System_Int64;
                        }

                        break;

                    case Code.Add_Ovf_Un:
                    case Code.Sub_Ovf_Un:
                    case Code.Mul_Ovf_Un:
                        result = this.EstimatedResultOf(opCodePart.OpCodeOperands[operandPosition]);
                        if (result.Type.TypeEquals(System.System_SByte))
                        {
                            return System.System_Byte;
                        }

                        if (result.Type.TypeEquals(System.System_Int16))
                        {
                            return System.System_UInt16;
                        }

                        if (result.Type.TypeEquals(System.System_Int32))
                        {
                            return System.System_UInt32;
                        }

                        if (result.Type.TypeEquals(System.System_Int64))
                        {
                            return System.System_UInt64;
                        }

                        break;
                }

                if (forArithmeticOperations)
                {
                    return this.RequiredArithmeticIncomingType(opCodePart) ?? retType;
                }

                return retType;
            }
            finally
            {
                opCodePart.Discovering = false;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        protected IType RequiredOutgoingType(OpCodePart opCodePart, bool ignoreAlternativeValues = false, bool exactType = false)
        {
            if (!ignoreAlternativeValues && opCodePart.UsedByAlternativeValues != null)
            {
                return opCodePart.UsedByAlternativeValues.RequiredOutgoingType;
            }

            IType retType = null;
            var code = opCodePart.ToCode();
            switch (code)
            {
                case Code.Ret:
                    retType = this.MethodReturnType;
                    return retType;

                case Code.Ldstr:
                    return this.System.System_String;

                case Code.Ldnull:
                    return System.System_Void.ToPointerType();

                case Code.Ldloc:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldloc_S:
                    retType = opCodePart.GetLocalType(this);
                    return retType;

                case Code.Ldloca:
                case Code.Ldloca_S:
                    retType = opCodePart.GetLocalType(this);
                    return retType.ToPointerType();

                case Code.Ldarg:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldarg_S:
                case Code.Ldarga_S:
                    var index = opCodePart.GetArgIndex();
                    if (this.HasMethodThis && index == 0)
                    {
                        retType = this.ThisType.ToClass();
                    }
                    else
                    {
                        retType = this.GetArgType(index);
                    }

                    return code == Code.Ldarga_S ? retType.ToPointerType() : retType;

                case Code.Ldfld:
                case Code.Ldsfld:
                    var operand = ((OpCodeFieldInfoPart)opCodePart).Operand;
                    return (this.MultiThreadingSupport && !exactType && operand.IsThreadStatic) ? System.System_Object : operand.FieldType;

                case Code.Ldflda:
                case Code.Ldsflda:
                    var opCodeFieldInfoPart = opCodePart as OpCodeFieldInfoPart;
                    var fieldType = opCodeFieldInfoPart.Operand.FieldType;
                    return fieldType.ToPointerType();

                case Code.Ldobj:
                    retType = ((OpCodeTypePart)opCodePart).Operand;
                    return retType;

                case Code.Ldelema:
                    retType = ((OpCodeTypePart)opCodePart).Operand;
                    return retType.ToPointerType();

                case Code.Ldelem:
                    retType = ((OpCodeTypePart)opCodePart).Operand;
                    return retType;

                case Code.Ldelem_I:
                    return this.System.System_IntPtr;

                case Code.Ldelem_I1:
                    return this.System.System_SByte;

                case Code.Ldelem_I2:
                    return this.System.System_Int16;

                case Code.Ldelem_I4:
                    return this.System.System_Int32;

                case Code.Ldelem_I8:
                    return this.System.System_Int64;

                case Code.Ldelem_U1:
                    return this.System.System_Byte;

                case Code.Ldelem_U2:
                    return this.System.System_UInt16;

                case Code.Ldelem_U4:
                    return this.System.System_UInt32;

                case Code.Ldelem_R4:
                    return this.System.System_Single;

                case Code.Ldelem_R8:
                    return this.System.System_Double;

                case Code.Ldelem_Ref:
                    retType = this.RequiredIncomingType(opCodePart, 0, exactType: true);
                    if (retType != null)
                    {
                        Debug.Assert(retType.HasElementType);
                        return retType.GetElementType();
                    }

                    retType = this.RequiredOutgoingType(opCodePart.OpCodeOperands[0], ignoreAlternativeValues);
                    Debug.Assert(retType.HasElementType);
                    return retType.GetElementType();

                case Code.Ldind_I:

                    retType = this.RequiredOutgoingType(opCodePart.OpCodeOperands[0]);
                    if ((retType.IsByRef || retType.IsPointer) && !retType.IsVoidPointer())
                    {
                        return retType.GetElementType();
                    }

                    return this.System.System_Int32;

                case Code.Ldind_I1:
                    return this.System.System_SByte;

                case Code.Ldind_I2:
                    return this.System.System_Int16;

                case Code.Ldind_I4:
                    return this.System.System_Int32;

                case Code.Ldind_I8:
                    return this.System.System_Int64;

                case Code.Ldind_R4:
                    return this.System.System_Single;

                case Code.Ldind_R8:
                    return this.System.System_Double;

                case Code.Ldind_U1:
                    return this.System.System_Byte;

                case Code.Ldind_U2:
                    return this.System.System_UInt16;

                case Code.Ldind_U4:
                    return this.System.System_UInt32;

                case Code.Ldind_Ref:
                    retType = opCodePart.RequiredIncomingTypes[0] ??
                              this.RequiredIncomingType(opCodePart, 0) ??
                              this.RequiredOutgoingType(opCodePart.OpCodeOperands[0]);
                    Debug.Assert(retType != null && retType.GetElementType() != null);
                    return retType.GetElementType();

                case Code.Box:
                    retType = ((OpCodeTypePart)opCodePart).Operand;
                    if (retType.IsPointer)
                    {
                        return System.System_Int32.ToClass();
                    }

                    var isNullable = retType.TypeEquals(this.System.System_Nullable_T);
                    return isNullable ? retType.GenericTypeArguments.First().ToClass() : retType.ToClass();

                case Code.Unbox:
                case Code.Unbox_Any:
                    retType = ((OpCodeTypePart)opCodePart).Operand;
                    return retType.ToNormal();

                case Code.Call:
                case Code.Callvirt:
                    var opCodePartMethod = opCodePart as OpCodeMethodInfoPart;
                    return opCodePartMethod.Operand.ReturnType;

                case Code.Newobj:
                    var opCodeConstructorInfoPart = opCodePart as OpCodeConstructorInfoPart;
                    return opCodeConstructorInfoPart == null
                        ? opCodePart.ReadExceptionFromStackType.ToClass()
                        : opCodeConstructorInfoPart.Operand.DeclaringType.IsValueType()
                            ? opCodeConstructorInfoPart.Operand.DeclaringType.ToPointerType()
                            : opCodeConstructorInfoPart.Operand.DeclaringType.ToClass();

                case Code.Newarr:
                    var opCodeTypePart = opCodePart as OpCodeTypePart;
                    return opCodeTypePart.Operand.ToArrayType(1);

                case Code.Castclass:
                    return ((OpCodeTypePart)opCodePart).Operand;

                case Code.Isinst:
                    return ((OpCodeTypePart)opCodePart).Operand.ToClass();

                case Code.Conv_I8:
                case Code.Conv_Ovf_I8:
                case Code.Conv_Ovf_I8_Un:
                    return this.System.System_Int64;

                case Code.Conv_I4:
                case Code.Conv_Ovf_I4:
                case Code.Conv_Ovf_I4_Un:
                    return this.System.System_Int32;

                case Code.Conv_I2:
                case Code.Conv_Ovf_I2:
                case Code.Conv_Ovf_I2_Un:
                    return this.System.System_Int16;

                case Code.Conv_I1:
                case Code.Conv_Ovf_I1:
                case Code.Conv_Ovf_I1_Un:
                    return this.System.System_SByte;

                case Code.Conv_I:
                case Code.Conv_Ovf_I:
                case Code.Conv_Ovf_I_Un:

                    return this.RequiredOutgoingTypeOfConv_I_U(opCodePart, System.System_IntPtr, true);

                case Code.Conv_U:
                case Code.Conv_Ovf_U:
                case Code.Conv_Ovf_U_Un:

                    return this.RequiredOutgoingTypeOfConv_I_U(opCodePart, System.System_UIntPtr, false);

                case Code.Conv_U8:
                case Code.Conv_Ovf_U8:
                case Code.Conv_Ovf_U8_Un:
                    return this.System.System_UInt64;

                case Code.Conv_U4:
                case Code.Conv_Ovf_U4:
                case Code.Conv_Ovf_U4_Un:
                    return this.System.System_UInt32;

                case Code.Conv_U2:
                case Code.Conv_Ovf_U2:
                case Code.Conv_Ovf_U2_Un:
                    return this.System.System_UInt16;

                case Code.Conv_U1:
                case Code.Conv_Ovf_U1:
                case Code.Conv_Ovf_U1_Un:
                    return this.System.System_Byte;

                case Code.Conv_R4:
                    return this.System.System_Single;

                case Code.Conv_R8:
                    return this.System.System_Double;

                case Code.Ldc_I4:
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
                case Code.Ldc_I4_S:
                    return this.System.System_Int32;

                case Code.Ldc_I8:
                    return this.System.System_Int64;

                case Code.Ldc_R4:
                    return this.System.System_Single;

                case Code.Ldc_R8:
                    return this.System.System_Double;

                case Code.Ceq:
                case Code.Cgt:
                case Code.Cgt_Un:
                case Code.Clt:
                case Code.Clt_Un:
                    return this.System.System_Boolean;

                case Code.Neg:
                case Code.Not:
                    return this.RequiredOutgoingType(opCodePart.OpCodeOperands[0]);

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

                    var opArithmetic = this.RequiredArithmeticIncomingType(opCodePart);
                    if (opArithmetic != null)
                    {
                        return opArithmetic;
                    }

                    var op1 = this.RequiredOutgoingType(opCodePart.OpCodeOperands[0], ignoreAlternativeValues);
                    var op2 = this.RequiredOutgoingType(opCodePart.OpCodeOperands[1], ignoreAlternativeValues);
                    var returnOp = op1.TypeEquals(op2) || op1.IsPointer || op1.IsByRef || op1.IntTypeBitSize() > op2.IntTypeBitSize() ? op1 : op2;

                    // in case of Pointer operations
                    if (returnOp.IsPointer && returnOp.GetElementType().IsVoid()
                        && (IntTypeRequired(opCodePart.OpCodeOperands[0]) || IntTypeRequired(opCodePart.OpCodeOperands[1])))
                    {
                        return System.System_Int32;
                    }

                    return returnOp;

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
                    return this.System.System_Boolean;

                case Code.Ldlen:
                case Code.Sizeof:
                    return this.System.System_UInt32;

                case Code.Localloc:
                    return this.System.System_Byte.ToPointerType();

                case Code.Dup:
                    return this.RequiredOutgoingType(opCodePart.OpCodeOperands[0]);

                case Code.Ldtoken:

                    opCodeTypePart = opCodePart as OpCodeTypePart;
                    if (opCodeTypePart != null)
                    {
                        return this.System.System_RuntimeTypeHandle;
                    }

                    var opCodeFieldInfoPartToken = opCodePart as OpCodeFieldInfoPart;
                    if (opCodeFieldInfoPartToken != null)
                    {
                        var constBytes = opCodeFieldInfoPartToken.Operand.ConstantValue as IConstBytes;
                        if (constBytes != null)
                        {
                            return System.System_Byte.ToArrayType(1);
                        }

                        return this.System.System_RuntimeFieldHandle;
                    }

                    var opCodeMethodInfoPartToken = opCodePart as OpCodeMethodInfoPart;
                    if (opCodeMethodInfoPartToken != null)
                    {
                        // hack to be able to initialize finalizer
                        if (opCodeMethodInfoPartToken.Operand is SynthesizedFinalizerWrapperMethod)
                        {
                            return System.System_Void.ToPointerType();
                        }

                        return this.System.System_RuntimeMethodHandle;
                    }

                    return this.System.System_Void.ToPointerType();

                case Code.Arglist:
                    return this.System.System_RuntimeArgumentHandle;

                case Code.Ldftn:
                case Code.Ldvirtftn:
                    return this.System.System_IntPtr;

                case Code.Mkrefany:
                    return this.System.System_TypedReference;

                case Code.Refanytype:
                    if (opCodePart.UsedBy == null)
                    {
                        // could not detect it yet
                        return null;
                    }

                    return this.RequiredIncomingType(opCodePart.UsedBy.OpCode, opCodePart.UsedBy.OperandPosition);

                case Code.Refanyval:
                    return this.System.System_Void.ToPointerType();

                case Code.Constrained:
                    opCodeTypePart = opCodePart as OpCodeTypePart;
                    if (opCodeTypePart != null)
                    {
                        return opCodeTypePart.Operand.IsValueType() ? opCodeTypePart.Operand.ToClass() : opCodeTypePart.Operand;
                    }

                    return null;
            }

            return retType;
        }

        private IType RequiredOutgoingTypeOfConv_I_U(OpCodePart opCodePart, IType convType, bool sign)
        {
            IType retType;
            retType = this.RequiredOutgoingType(opCodePart.OpCodeOperands[0]);
            if (retType.IsPointer)
            {
                return retType;
            }

            if (retType.IsByRef && retType.GetElementType().TypeNotEquals(convType))
            {
                return retType.GetElementType().ToPointerType();
            }

            if (retType.IsPinned)
            {
                return this.System.System_Void.ToPointerType();
            }

            var intPtrOper = IntTypeRequired(opCodePart);
            var nativeIntType = intPtrOper
                ? (sign ? this.System.System_Int32 : this.System.System_UInt32)
                : this.System.System_Void.ToPointerType();
            return nativeIntType;
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
                Debug.Assert(last.Next != opCodePartBefore, "circular reference detected");
                last.Next = opCodePartBefore;
                Debug.Assert(opCodePartBefore.Previous != last, "circular reference detected");
                opCodePartBefore.Previous = last;
            }

            last = opCodePartBefore;

            return last;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        protected static bool IntTypeRequired(OpCodePart opCode)
        {
            if (opCode == null || opCode.UsedBy == null)
            {
                return false;
            }

            if (opCode.UsedBy.Any(
                Code.Add, Code.Add_Ovf, Code.Add_Ovf_Un, Code.Sub, Code.Sub_Ovf, Code.Sub_Ovf_Un, Code.Mul, Code.Mul_Ovf, Code.Mul_Ovf_Un, Code.Div, Code.Div_Un))
            {
                return true;
            }

            if (opCode.UsedBy.Any(Code.And, Code.Xor, Code.Or))
            {
                return true;
            }

            if (opCode.UsedBy.OperandPosition == 1
                && opCode.UsedBy.Any(
                    Code.Ldelem,
                    Code.Ldelem_I,
                    Code.Ldelem_I1,
                    Code.Ldelem_I2,
                    Code.Ldelem_I4,
                    Code.Ldelem_I8,
                    Code.Ldelem_R4,
                    Code.Ldelem_R8,
                    Code.Ldelem_Ref,
                    Code.Ldelem_U1,
                    Code.Ldelem_U2,
                    Code.Ldelem_U4,
                    Code.Ldelema))
            {
                return true;
            }

            if (opCode.UsedBy.OperandPosition == 1
                && opCode.UsedBy.Any(
                    Code.Stelem, Code.Stelem_I, Code.Stelem_I1, Code.Stelem_I2, Code.Stelem_I4, Code.Stelem_I8, Code.Stelem_R4, Code.Stelem_R8, Code.Stelem_Ref))
            {
                return true;
            }

            return false;
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

        private void BuildAddressIndexes(IEnumerable<OpCodePart> opCodes)
        {
            this.OpsByGroupAddressStart.Clear();
            this.OpsByGroupAddressEnd.Clear();

            foreach (var opCodePart in opCodes)
            {
                this.AddAddressIndex(opCodePart);
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

        private OpCodePart PopValue(bool varArg = false)
        {
            if (varArg && !this.Stacks.Any())
            {
                return null;
            }

            return this.Stacks.Pop();
        }

        private IType RequiredArithmeticIncomingType(OpCodePart opCodePart)
        {
            if (!opCodePart.Any(
                Code.Add,
                Code.Sub,
                Code.Mul,
                Code.Div,
                Code.Div_Un,
                Code.Rem,
                Code.Rem_Un,
                Code.Shl,
                Code.Shr,
                Code.Shr_Un,
                Code.Not))
            {
                return null;
            }

            var uintRequired = opCodePart.Any(Code.Shl, Code.Shr, Code.Shr_Un);

            if (this.IsFloatingPointOp(opCodePart) || this.IsPointerArithmetic(opCodePart))
            {
                return null;
            }

            var intOpBitSize = this.IntOpBitSize(opCodePart);
            var typeResolver = (ITypeResolver)this;
            if (intOpBitSize == 1 || intOpBitSize >= (CWriter.PointerSize * 8))
            {
                return uintRequired ? typeResolver.GetUIntTypeByBitSize(intOpBitSize) : this.GetIntTypeByBitSize(intOpBitSize);
            }

            return uintRequired
                ? typeResolver.GetUIntTypeByByteSize(CWriter.PointerSize)
                : typeResolver.GetIntTypeByByteSize(CWriter.PointerSize);
        }

        /// <summary>
        /// </summary>
        public class ReturnResult
        {
            /// <summary>
            /// </summary>
            /// <param name="result">
            /// </param>
            public ReturnResult(FullyDefinedReference result)
            {
                Debug.Assert(result != null, "result can't be null");

                this.Type = result.Type;
            }

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            public ReturnResult(IType type)
            {
                Debug.Assert(type != null, "type can't be null");

                this.Type = type;
            }

            /// <summary>
            /// </summary>
            public bool IsReference
            {
                get
                {
                    if (this.Type.IsPointer || this.Type.IsByRef || this.Type.UseAsClass)
                    {
                        return true;
                    }

                    if (this.Type.IsValueType)
                    {
                        return false;
                    }

                    return true;
                }
            }

            /// <summary>
            /// </summary>
            public IType Type { get; set; }
        }
    }
}
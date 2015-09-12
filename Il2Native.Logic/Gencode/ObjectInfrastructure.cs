// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectInfrastructure.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using System.Threading;
    using CodeParts;
    using DebugInfo.DebugInfoSymbolWriter;
    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;

    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class ObjectInfrastructure
    {
        /// <summary>
        /// </summary>
        public const int FunctionsOffsetInVirtualTable = 2;

        public const string CalledCctorFieldName = "_cctor_called";

        public static void WriteAllocateMemory(
            this CWriter cWriter,
            OpCodePart opCodePart,
            FullyDefinedReference size)
        {
            var writer = cWriter.Output;

            writer.Write("(Byte*) ");
            writer.Write(cWriter.GetAllocator(false, false, cWriter.GcDebug));
            writer.Write("(");
            cWriter.WriteResult(size);
            writer.Write(")");
        }

        public static void GetAllocateMemoryCodeForObject(
            this ITypeResolver typeResolver, IlCodeBuilder newAlloc, IType declaringClassType, bool doNotTestNullValue, bool enableStringFastAllocation = false)
        {
            if (declaringClassType.IsMultiArray)
            {
                ArrayMultiDimensionGen.MultiDimArrayAllocationSizeMethodBody(newAlloc, typeResolver, declaringClassType);
            }
            else if (declaringClassType.IsArray)
            {
                ArraySingleDimensionGen.SingleDimArrayAllocationSizeMethodBody(newAlloc, typeResolver, declaringClassType);
            }
            else if (enableStringFastAllocation && declaringClassType.IsString)
            {
                StringGen.StringAllocationSizeMethodBody(newAlloc, typeResolver, declaringClassType, typeResolver.System.System_Char, true);
            }
            else
            {
                newAlloc.SizeOf(declaringClassType);
            }

            // TODO: to reduce usage of locks you can limit it to object types only
            if (typeResolver.MultiThreadingSupport)
            {
                // Add area to save 'cond'
                newAlloc.SizeOf(typeResolver.System.System_Object.ToPointerType());
                newAlloc.Add(Code.Add);

                // Add area to save 'mutex'
                newAlloc.SizeOf(typeResolver.System.System_Object.ToPointerType());
                newAlloc.Add(Code.Add);
            }

            // static is not part of class
            var isAtomicAllocation = typeResolver.CanBeAllocatedAtomically(declaringClassType);

            var localNumber = -1;
            if (isAtomicAllocation)
            {
                localNumber = newAlloc.Locals.Count;
                newAlloc.Locals.Add(typeResolver.System.System_Int32);

                newAlloc.Add(Code.Dup);
                newAlloc.SaveLocal(localNumber);
            }

#if NO_GC_MALLOC_IGNORE_OFF_PAGE
            newAlloc.Call(
                new SynthesizedMethod(
                    typeResolver.GetAllocator(isAtomicAllocation),
                    typeResolver.System.System_Void.ToPointerType(),
                    new[] { typeResolver.System.System_Int32.ToParameter("size") }));
#else
            newAlloc.Add(Code.Dup);
            // 100K
            newAlloc.LoadConstant(100 * 1024);
            var ifBigger100k = newAlloc.Branch(Code.Bge_Un, Code.Bge_Un_S);

            var debugOriginalRequired = typeResolver.GcDebug && enableStringFastAllocation;
            var allocator = typeResolver.GetAllocator(isAtomicAllocation, false, debugOriginalRequired);
            newAlloc.Call(
                new SynthesizedMethod(
                    allocator,
                    typeResolver.System.System_Void.ToPointerType(),
                    new[] { typeResolver.System.System_Int32.ToParameter("size") }));

            var leave = newAlloc.Branch(Code.Br, Code.Br_S);

            newAlloc.Add(ifBigger100k);

            var allocatorBigObj = typeResolver.GetAllocator(isAtomicAllocation, true, debugOriginalRequired);
            newAlloc.Call(
                new SynthesizedMethod(
                    allocatorBigObj,
                    typeResolver.System.System_Void.ToPointerType(),
                    new[] { typeResolver.System.System_Int32.ToParameter("size") }));

            newAlloc.Add(leave);
#endif

            if (!doNotTestNullValue)
            {
                newAlloc.Add(Code.Dup);
                var jump = newAlloc.Branch(Code.Brtrue, Code.Brtrue_S);

                var throwType = typeResolver.ResolveType("System.OutOfMemoryException");
                var defaultConstructor = throwType.FindConstructor(typeResolver);

                Debug.Assert(defaultConstructor != null, "default constructor is null");

                newAlloc.New(defaultConstructor);
                newAlloc.Throw();

                newAlloc.Add(jump);
            }

            if (isAtomicAllocation)
            {
                // if this is atomic, you need to init memory
                newAlloc.Add(Code.Dup);
                newAlloc.LoadConstant(0);
                newAlloc.LoadLocal(localNumber);
                newAlloc.Add(Code.Initblk);
            }

            if (typeResolver.GcSupport)
            {
                var finalizer = declaringClassType.FindFinalizer(typeResolver);
                if (finalizer != null)
                {
                    // obj
                    newAlloc.Add(Code.Dup);
                    // finalizer function address
                    newAlloc.LoadToken(declaringClassType.GetMethodsByName(SynthesizedFinalizerWrapperMethod.Name, typeResolver).First());
                    // Dummy nulls
                    newAlloc.LoadNull();
                    newAlloc.LoadNull();
                    newAlloc.LoadNull();

                    var voidPointer = typeResolver.System.System_Void.ToPointerType();
                    var parameters = new[]
                                         {
                                             voidPointer.ToParameter("obj"), voidPointer.ToParameter("fn"), voidPointer.ToParameter("cd"),
                                             voidPointer.ToParameter("ofn"), voidPointer.ToPointerType().ToParameter("ocd")
                                         };
                    newAlloc.Call(new SynthesizedMethodStringAdapter("GC_REGISTER_FINALIZER", null, typeResolver.System.System_Void, parameters));

                }
            }

            if (typeResolver.MultiThreadingSupport)
            {
                // init cond area with -1 value and shift address
                newAlloc.Add(Code.Dup);
                newAlloc.Castclass(typeResolver.System.System_Int32.ToPointerType());

                newAlloc.LoadConstant(-1);
                newAlloc.SaveObject(typeResolver.System.System_Int32);

                newAlloc.SizeOf(declaringClassType.ToPointerType());
                newAlloc.Add(Code.Add);

                // init mutex area with -1 value and shift address
                newAlloc.Add(Code.Dup);
                newAlloc.Castclass(typeResolver.System.System_Int32.ToPointerType());

                newAlloc.LoadConstant(-1);
                newAlloc.SaveObject(typeResolver.System.System_Int32);

                newAlloc.SizeOf(declaringClassType.ToPointerType());
                newAlloc.Add(Code.Add);

                newAlloc.Castclass(typeResolver.System.System_Void.ToPointerType());
            }

            newAlloc.Castclass(declaringClassType);
        }

        private static bool CanBeAllocatedAtomically(this ITypeResolver typeResolver, IType declaringClassType)
        {
            if (declaringClassType.IsInterface)
            {
                return true;
            }

            return Logic.IlReader.Fields(declaringClassType, IlReader.DefaultFlags | BindingFlags.FlattenHierarchy, typeResolver)
                        .All(typeResolver.IsAtomicValue);
        }

        private static bool IsAtomicValue(this ITypeResolver typeResolver, IField field)
        {
            if (field.IsConst || field.IsVirtualTable || field.IsStatic || typeResolver.IsAtomicValue(field.FieldType))
            {
                return true;
            }

            if (field.IsFixed)
            {
                var elementType = field.FieldType.GetElementType();
                return typeResolver.IsAtomicValue(elementType);
            }

            return false;
        }

        private static bool IsAtomicValue(this ITypeResolver typeResolver, IType type)
        {
            if (!type.IsValueType || type.IsPointer)
            {
                return false;
            }

            if (type.IsStructureType())
            {
                return typeResolver.CanBeAllocatedAtomically(type);
            }

            return true;
        }

        public static IlCodeBuilder GetBoxMethod(
            this ITypeResolver typeResolver,
            IType type,
            bool doNotTestNullValue)
        {
            var isNullable = type.TypeEquals(typeResolver.System.System_Nullable_T);
            var declaringClassType = isNullable ? type.GenericTypeArguments.First().ToClass() : type.ToClass();
            var normal = declaringClassType.ToNormal();
            var isValueType = normal.IsValueType();

            var ilCodeBuilder = new IlCodeBuilder();

            // in case nullable does not have value just return null
            if (isNullable)
            {
                ilCodeBuilder.LoadArgument(0);
                ilCodeBuilder.LoadField(type.GetFieldByFieldNumber(0, typeResolver));
                var jump = ilCodeBuilder.Branch(Code.Brtrue, Code.Brtrue_S);
                ilCodeBuilder.LoadNull();
                ilCodeBuilder.Add(Code.Ret);
                ilCodeBuilder.Add(jump);
            }

            typeResolver.GetNewMethod(ilCodeBuilder, declaringClassType, doNotCallInit: true, doNotTestNullValue: doNotTestNullValue);

            ilCodeBuilder.Parameters.Add(type.ToParameter("_value"));

            // we need to remove last code which is Code.Ret
            ilCodeBuilder.RemoveLast();

            if (!isValueType)
            {
                ilCodeBuilder.Add(Code.Dup);
                ilCodeBuilder.LoadArgument(0);
                if (isNullable)
                {
                    ilCodeBuilder.LoadField(type.GetFieldByFieldNumber(1, typeResolver));
                }

                ilCodeBuilder.SaveField(declaringClassType.GetFieldByFieldNumber(0, typeResolver));
            }
            else
            {
                // copy structure
                ilCodeBuilder.Add(Code.Dup);
                var firstField = declaringClassType.GetFieldByFieldNumber(0, typeResolver);
                if (firstField != null)
                {
                    ilCodeBuilder.LoadFieldAddress(firstField);
                    ilCodeBuilder.LoadArgument(0);
                    if (isNullable)
                    {
                        ilCodeBuilder.LoadField(type.GetFieldByFieldNumber(1, typeResolver));
                    }

                    ilCodeBuilder.CopyObject(normal);
                }
            }

            ilCodeBuilder.Add(Code.Dup);
            ilCodeBuilder.Call(declaringClassType.GetFirstMethodByName(SynthesizedInitMethod.Name, typeResolver));

            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetUnboxMethod(this ITypeResolver typeResolver, IType type)
        {
            var isNullable = type.TypeEquals(typeResolver.System.System_Nullable_T);
            var isValueType = type.IsValueType();

            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgument(0);
            var jumpIfNotNull = ilCodeBuilder.Branch(Code.Brtrue, Code.Brtrue_S);

            if (typeResolver.Unsafe || isNullable)
            {
                ilCodeBuilder.Locals.Add(type);
                ilCodeBuilder.LoadLocalAddress(0);
                ilCodeBuilder.InitializeObject(type);
                ilCodeBuilder.LoadLocal(0);
                ilCodeBuilder.Return();
            }
            else
            {
                ilCodeBuilder.New(typeResolver.System.System_NullReferenceException.FindConstructor(typeResolver));
                ilCodeBuilder.Throw();
            }

            ilCodeBuilder.Add(jumpIfNotNull);

            if (!isValueType && !isNullable)
            {
                var firstField = type.GetFieldByFieldNumber(0, typeResolver);
                if (firstField != null)
                {
                    ilCodeBuilder.LoadArgument(0);
                    ilCodeBuilder.LoadField(firstField);
                }
                else
                {
                    ilCodeBuilder.LoadConstant(0);
                }
            }
            else
            {
                // copy structure
                var firstField = type.GetFieldByFieldNumber(0, typeResolver);
                if (firstField != null)
                {
                    if (!isNullable)
                    {
                        ilCodeBuilder.LoadArgument(0);
                        ilCodeBuilder.LoadFieldAddress(firstField);
                        ilCodeBuilder.LoadObject(type);
                    }
                    else
                    {
                        ilCodeBuilder.Locals.Add(type);
                        ilCodeBuilder.LoadLocalAddress(ilCodeBuilder.Locals.Count() - 1);
                        var nullableValue = type.GetFieldByFieldNumber(1, typeResolver);
                        ilCodeBuilder.LoadFieldAddress(nullableValue);

                        ilCodeBuilder.LoadArgument(0);
                        
                        ilCodeBuilder.SaveObject(type);
                        ilCodeBuilder.LoadLocal(ilCodeBuilder.Locals.Count() - 1);
                    }
                }
                else
                {
                    ilCodeBuilder.Locals.Add(type);
                    ilCodeBuilder.LoadLocalAddress(0);
                    ilCodeBuilder.InitializeObject(type);
                    ilCodeBuilder.LoadLocal(ilCodeBuilder.Locals.Count() - 1);
                }
            }

            ilCodeBuilder.Return();

            return ilCodeBuilder;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallBoxObjectMethod(this IType type, CWriter cWriter, OpCodePart opCode)
        {
            var method = new SynthesizedBoxMethod(type, cWriter);
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            opCodeNope.OpCodeOperands = opCode.OpCodeOperands;

            AppendDebugParemeters(cWriter, opCodeNope);

            cWriter.WriteCall(
                opCodeNope,
                method,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodeConstructorInfoPart">
        /// </param>
        public static void WriteCallConstructor(
            this CWriter cWriter,
            OpCodeConstructorInfoPart opCodeConstructorInfoPart)
        {
            cWriter.WriteCallConstructor(opCodeConstructorInfoPart, opCodeConstructorInfoPart.Operand);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        public static void WriteCallConstructor(
            this CWriter cWriter,
            OpCodePart opCodePart,
            IConstructor methodBase)
        {
            cWriter.WriteCall(
                opCodePart,
                methodBase,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallInitObjectMethod(this IType type, CWriter cWriter, OpCodePart opCode)
        {
            var method = new SynthesizedInitMethod(type, cWriter);
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.OpCodeOperands = opCode.OpCodeOperands;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            cWriter.WriteCall(
                opCodeNope,
                method,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallNewObjectMethod(this IType type, CWriter cWriter, OpCodePart opCode)
        {
            var method = new SynthesizedNewMethod(type.ToNormal(), cWriter);
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            if (type.IsArray)
            {
                opCodeNope.OpCodeOperands = opCode.OpCodeOperands;
            }

            AppendDebugParemeters(cWriter, opCodeNope);

            cWriter.WriteCall(
                opCodeNope,
                method,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);

            cWriter.Output.WriteLine(";");
        }

        private static void AppendDebugParemeters(CWriter cWriter, OpCodePart opCodeNope)
        {
            if (cWriter.GcDebug)
            {
                var ops = opCodeNope.OpCodeOperands != null
                    ? new List<OpCodePart>(opCodeNope.OpCodeOperands)
                    : new List<OpCodePart>();

                var opFile = OpCodePart.CreateNop;
                opFile.Result = new FullyDefinedReference("(SByte*)__FILE__", cWriter.System.System_Void.ToPointerType());
                ops.Add(opFile);

                var opLine = OpCodePart.CreateNop;
                opLine.Result = new FullyDefinedReference("__LINE__", cWriter.System.System_Int32);
                ops.Add(opLine);

                opCodeNope.OpCodeOperands = ops.ToArray();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallUnboxObjectMethod(this IType type, CWriter cWriter, OpCodePart opCode)
        {
            var method = new SynthesizedUnboxMethod(type, cWriter);
            cWriter.WriteCall(
                opCode,
                method,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        public static void WriteGetHashCodeObjectForEnum(
            this CWriter cWriter,
            OpCodePart opCode,
            IType declaringType)
        {
            var writer = cWriter.Output;

            var isValueType = declaringType.IsValueType();

            writer.WriteLine("; Returning Hash Code");
            writer.WriteLine(string.Empty);

            writer.WriteLine("; Get data");

            if (!isValueType)
            {
                // write access to a field
                if (cWriter.WriteFieldAccess(
                        opCode,
                        declaringType.ToClass(),
                        declaringType.ToClass(),
                        0,
                        opCode.Result) == null)
                {
                    writer.WriteLine("// No data");
                    return;
                }
            }
            else
            {
                Debug.Fail("Not implemented yet");
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="declaringTypeIn">
        /// </param>
        public static void WriteInit(
            this CWriter cWriter,
            OpCodePart opCodePart,
            IType declaringTypeIn)
        {
            var writer = cWriter.Output;

            var declaringTypeNormal = declaringTypeIn.ToNormal();
            if (declaringTypeNormal.IsValueType)
            {
                cWriter.WriteMemSet(declaringTypeNormal, opCodePart.OpCodeOperands[0]);
            }
            else
            {
                // this is type reference, initialize it with null
                cWriter.LoadIndirect(writer, opCodePart, declaringTypeNormal);
                writer.Write(" = 0");
            }
        }

        public static IlCodeBuilder GetInitMethod(this ITypeResolver typeResolver, IType declaringType)
        {
            var codeBuilder = new IlCodeBuilder();

            if (declaringType.HasAnyVirtualMethod(typeResolver))
            {
                // set virtual table
                codeBuilder.LoadArgument(0);
                codeBuilder.LoadToken(declaringType.ToVirtualTableImplementation());
                codeBuilder.SaveField(typeResolver.System.System_Object.GetFieldByName(CWriter.VTable, typeResolver));
            }

            codeBuilder.Add(Code.Ret);

            return codeBuilder;
        }

        public static IlCodeBuilder GetGetTypeMethod(this ITypeResolver typeResolver, IType declaringType)
        {
            var codeBuilder = new IlCodeBuilder();

            codeBuilder.LoadToken(declaringType.GetFullyDefinedRefereneForRuntimeType((CWriter)typeResolver));
            codeBuilder.Add(Code.Ret);

            return codeBuilder;
        }

        public static void GetGetStaticMethod(this ITypeResolver typeResolver, IlCodeBuilder codeBuilder, IType declaringType, IField field)
        {
            var cctor = declaringType.FindStaticConstructor(typeResolver);
            if (cctor != null)
            {
                codeBuilder.LoadField(declaringType.GetFieldByName(ObjectInfrastructure.CalledCctorFieldName, typeResolver));
                var initializedJump = codeBuilder.Branch(Code.Brfalse, Code.Brfalse_S);
                codeBuilder.Call(cctor);
                codeBuilder.Add(initializedJump);
            }

            codeBuilder.LoadField(field);

            if (field.IsThreadStatic && field.FieldType.IsStructureType())
            {
                codeBuilder.Castclass(typeResolver.System.System_Void.ToPointerType());
                codeBuilder.Unbox(field.FieldType);
            }

            codeBuilder.Add(Code.Ret);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodeConstructorInfoPart">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        public static void WriteNew(
            this CWriter cWriter,
            OpCodeConstructorInfoPart opCodeConstructorInfoPart,
            IType declaringType)
        {
            var @class = declaringType.ToClass();
            var objectReference = cWriter.WriteVariableForNew(opCodeConstructorInfoPart, @class);
            WriteNew(cWriter, opCodeConstructorInfoPart, declaringType, objectReference);
        }

        public static void WriteNew(
            this CWriter cWriter, OpCodeConstructorInfoPart opCodeConstructorInfoPart, IType declaringType, FullyDefinedReference objectReference)
        {
            var @class = declaringType.ToClass();
            if (!declaringType.IsString)
            {
                if (!declaringType.IsValueType())
                {
                    @class.WriteCallNewObjectMethod(cWriter, opCodeConstructorInfoPart);
                }
                else
                {
                    // if this is atomic, you need to init memory
                    cWriter.WriteMemSet(objectReference, 0, declaringType);
                    cWriter.Output.WriteLine(";");
                }

                // for '__this'
                var opCodeNope = OpCodePart.CreateNop;
                opCodeNope.Result = objectReference;

                var callOps = new List<OpCodePart>(opCodeConstructorInfoPart.OpCodeOperands != null ? opCodeConstructorInfoPart.OpCodeOperands.Length : 0 + 1);
                callOps.Add(opCodeNope);
                if (opCodeConstructorInfoPart.OpCodeOperands != null)
                {
                    callOps.AddRange(opCodeConstructorInfoPart.OpCodeOperands);
                }

                opCodeConstructorInfoPart.OpCodeOperands = callOps.ToArray();

                // call
                cWriter.WriteCallConstructor(opCodeConstructorInfoPart);

                opCodeConstructorInfoPart.Result = objectReference;
            }
            else
            {
                // special string case
                var stringCtorMethodBase = StringGen.GetCtorMethodByParameters(declaringType, opCodeConstructorInfoPart.Operand.GetParameters(), cWriter);
                var hasThis = stringCtorMethodBase.CallingConvention.HasFlag(CallingConventions.HasThis);

                OpCodePart opCodeNope = opCodeConstructorInfoPart;
                if (hasThis)
                {
                    // insert 'This' as null
                    opCodeNope = OpCodePart.CreateNop;
                    var operands = new List<OpCodePart>(opCodeConstructorInfoPart.OpCodeOperands.Length + 1);

                    // this
                    var opCodeThis = OpCodePart.CreateNop;
                    opCodeThis.Result = new ConstValue("(System_String*)1/*dummay value for __this*/", declaringType);
                    operands.Add(opCodeThis);

                    // params
                    operands.AddRange(opCodeConstructorInfoPart.OpCodeOperands);

                    opCodeNope.OpCodeOperands = operands.ToArray();
                }

                cWriter.WriteCall(opCodeNope, stringCtorMethodBase, cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);

                opCodeConstructorInfoPart.Result = objectReference;
            }
        }

        public static void WriteNew(
            this CWriter cWriter,
            OpCodePart opCodePart,
            IType declaringType)
        {
            var @class = declaringType.ToClass();
            var objectReference = cWriter.WriteVariableForNew(opCodePart, @class);
            @class.WriteCallNewObjectMethod(cWriter, opCodePart);

            opCodePart.Result = objectReference;
        }

        /// <summary>
        /// </summary>
        /// <param name="typeResolver">
        /// </param>
        /// <param name="ilCodeBuilder"></param>
        /// <param name="type">
        /// </param>
        /// <param name="doNotCallInit">
        /// </param>
        /// <param name="doNotTestNullValue">
        /// </param>
        /// <param name="enableStringFastAllocation"></param>
        /// <param name="opCodePart">
        /// </param>
        public static void GetNewMethod(this ITypeResolver typeResolver, IlCodeBuilder ilCodeBuilder, IType type, bool doNotCallInit = false, bool doNotTestNullValue = false, bool enableStringFastAllocation = false)
        {
            var classType = type.ToClass();
            typeResolver.GetAllocateMemoryCodeForObject(ilCodeBuilder, classType, doNotTestNullValue, enableStringFastAllocation);
            if (!doNotCallInit)
            {
                ilCodeBuilder.Add(Code.Dup);
                ilCodeBuilder.Call(new SynthesizedInitMethod(type, typeResolver));
            }

            if (!enableStringFastAllocation)
            {
                ilCodeBuilder.Add(Code.Ret);
            }
        }

        public static IlCodeBuilder GetSizeMethod(
            this ITypeResolver typeResolver,
            IType type)
        {
            var ilCodeBuilder = new IlCodeBuilder();
            ilCodeBuilder.SizeOf(type.ToClass());
            ilCodeBuilder.Add(Code.Ret);
            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetFinalizerWrapperMethod(
            this ITypeResolver typeResolver,
            IType type)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Parameters.Add(typeResolver.System.System_Void.ToPointerType().ToParameter("obj"));
            ilCodeBuilder.Parameters.Add(typeResolver.System.System_Void.ToPointerType().ToParameter("cd"));

            ilCodeBuilder.LoadArgument(0);

            if (typeResolver.MultiThreadingSupport)
            {
                // to adjust pointer to point VTable
                // for mutex area
                ilCodeBuilder.SizeOf(type.ToPointerType());
                ilCodeBuilder.Add(Code.Add);
                // for 'cond' area
                ilCodeBuilder.SizeOf(type.ToPointerType());
                ilCodeBuilder.Add(Code.Add);
                ilCodeBuilder.Castclass(typeResolver.System.System_Void.ToPointerType());
            }

            // TODO: can be removed when InsertMissingTypes is finished
            ilCodeBuilder.Castclass(type);
            ilCodeBuilder.Call(type.FindFinalizer(typeResolver));

            ilCodeBuilder.Add(Code.Ret);
            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetDynamicCastMethod(
            this ITypeResolver typeResolver,
            IType type,
            bool throwInvalidCast = false)
        {
            var code = new IlCodeBuilder();

            code.Parameters.Add(typeResolver.System.System_Object.ToParameter("_obj"));
            code.Parameters.Add(typeResolver.System.System_Type.ToParameter("_type"));

            code.Locals.Add(typeResolver.System.System_Type);

            code.LoadArgument(0);
            var jumpNull = code.Branch(Code.Brtrue, Code.Brtrue_S);

            code.LoadNull();
            code.Add(Code.Ret);

            code.Add(jumpNull);

            // test if it is an interface
            code.LoadArgument(1);
            code.Castclass(typeResolver.System.System_Void.ToPointerType());
            code.Call(typeResolver.System.System_RuntimeTypeHandle.GetMethodsByName("IsInterface", typeResolver).First(p => p.GetParameters().Count() == 1));
            var jumpInterace = code.Branch(Code.Brtrue, Code.Brtrue_S);

            code.LoadArgument(0);
            code.Call(typeResolver.System.System_Object.GetMethodsByName("GetType", typeResolver).First(p => !p.GetParameters().Any()));
            code.SaveLocal(0);
            var jump = code.Branch(Code.Br, Code.Br_S);

            var cond_back_jump = code.CreateLabel();

            code.LoadLocal(0);
            code.LoadArgument(1);

            var jump_not_equal = code.Branch(Code.Bne_Un, Code.Bne_Un_S);

            code.LoadArgument(0);
            code.Add(Code.Ret);

            code.Add(jump_not_equal);

            code.LoadLocal(0);
            code.Call(typeResolver.System.System_Type.GetMethodsByName("get_BaseType", typeResolver).First(p => !p.GetParameters().Any()));
            code.SaveLocal(0);

            code.Add(jump);

            code.LoadLocal(0);
            code.Branch(Code.Brtrue, Code.Brtrue_S, cond_back_jump);

            if (!throwInvalidCast)
            {
                code.LoadNull();
                code.Add(Code.Ret);
            }
            else
            {
                code.Throw(typeResolver.ResolveType("System.InvalidCastException").FindConstructor(typeResolver));
            }

            // end of object branch
            code.Add(jumpInterace);

            code.LoadArgument(0);
            code.LoadArgument(1);
            code.Call(typeResolver.System.System_Object.GetMethodsByName(SynthesizedResolveInterfaceMethod.Name, typeResolver).First());

            if (throwInvalidCast)
            {
                // if result is null, throw exception
                code.Add(Code.Dup);
                var jumpOverThrow = code.Branch(Code.Brtrue, Code.Brtrue_S);
                code.Throw(typeResolver.ResolveType("System.InvalidCastException").FindConstructor(typeResolver));
                code.Add(jumpOverThrow);
            }

            code.Add(Code.Ret);

            return code;
        }

        public static IlCodeBuilder GetResolveInterfaceMethod(
            this ITypeResolver typeResolver,
            IType type,
            bool throwInvalidCast = false)
        {
            var code = new IlCodeBuilder();

            code.Parameters.Add(typeResolver.System.System_Type.ToParameter("_type"));

            foreach (var @interface in type.GetAllInterfaces())
            {
                code.LoadToken(@interface.GetFullyDefinedRefereneForRuntimeType((CWriter)typeResolver));
                code.LoadArgument(1);

                var jump_not_equal = code.Branch(Code.Bne_Un, Code.Bne_Un_S);

                code.LoadArgument(0);
                code.Castclass(@interface);
                code.Castclass(typeResolver.System.System_Void.ToPointerType());
                code.Add(Code.Ret);

                code.Add(jump_not_equal);                
            }

            code.LoadNull();
            code.Add(Code.Ret);

            return code;
        }

        public static IEnumerable<IType> HaveStaticVirtualTablesForInterfaces(this IType type, ITypeResolver typeResolver)
        {
            foreach (var @interface in type.GetInterfaces())
            {
                yield return @interface;
            }

            foreach (
                var @interface in
                    type.GetVirtualTable(typeResolver)
                        .Where(p => p.Kind != CWriter.PairKind.Method)
                        .OfType<CWriter.Pair<IType, IType>>()
                        .Select(p => p.Value)
                        .Where(t => !type.GetInterfaces().Contains(t))
                        .Where(@interface => VirtualTableGen.HasVirtualMethodOrExplicitMethod(type, type.FindInterfaceOwner(@interface), @interface, typeResolver))
                )
            {
                yield return @interface;
            }
        }

        public static IMethod GetInvokeWrapperForStructUsedInObject(IMethod method, ITypeResolver typeResolver)
        {
            var codeBuilder = new IlCodeBuilder();

            var count = method.GetParameters().Count() + (method.IsStatic ? 0 : 1);
            foreach (var argNum in Enumerable.Range(0, count))
            {
                codeBuilder.LoadArgument(argNum);

                // in case of 'this'
                if (argNum == 0 && !method.IsStatic)
                {
                    codeBuilder.Castclass(typeResolver.System.System_Byte.ToPointerType());
                    codeBuilder.SizeOf(typeResolver.System.System_Void.ToPointerType());
                    codeBuilder.Add(Code.Add);
                    codeBuilder.Castclass(method.DeclaringType.ToNormal().ToPointerType());
                }
            }

            codeBuilder.CallDirect(method);
            codeBuilder.Add(Code.Ret);

            var methodGenerated = codeBuilder.GetMethodDecorator(method);
            methodGenerated.Suffix = "__entry_for_object";
            methodGenerated.IsStructObjectAdapter = true;
            return methodGenerated;
        }
    }
}
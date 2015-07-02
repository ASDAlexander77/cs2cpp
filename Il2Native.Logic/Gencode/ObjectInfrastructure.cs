// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectInfrastructure.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#define CHECK_NULL_IN_UNBOX_AND_RETURN_DEFAULT

namespace Il2Native.Logic.Gencode
{
    using System;
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;
    using CodeParts;

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

        public const string TypeHolderFieldName = ".type";

        public static void WriteAllocateMemory(
            this CWriter cWriter,
            OpCodePart opCodePart,
            FullyDefinedReference size)
        {
            var writer = cWriter.Output;

            writer.Write("(::Byte*) ");
            writer.Write(cWriter.GetAllocator(false, false));
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
            if (typeResolver.GetMultiThreadingSupport())
            {
                // Add area to save locks
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

            newAlloc.Call(
                new SynthesizedMethod(
                    typeResolver.GetAllocator(isAtomicAllocation, false),
                    typeResolver.System.System_Void.ToPointerType(),
                    new[] { typeResolver.System.System_Int32.ToParameter("size") }));

            var leave = newAlloc.Branch(Code.Br, Code.Br_S);

            newAlloc.Add(ifBigger100k);

            newAlloc.Call(
                new SynthesizedMethod(
                    typeResolver.GetAllocator(isAtomicAllocation, true),
                    typeResolver.System.System_Void.ToPointerType(),
                    new[] { typeResolver.System.System_Int32.ToParameter("size") }));

            newAlloc.Add(leave);
#endif

            if (!doNotTestNullValue)
            {
                newAlloc.Add(Code.Dup);
                var jump = newAlloc.Branch(Code.Brtrue, Code.Brtrue_S);

                var throwType = typeResolver.ResolveType("System.OutOfMemoryException");
                var defaultConstructor = IlReader.FindConstructor(throwType, typeResolver);
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

            if (typeResolver.GetGcSupport())
            {
                var finalizer = IlReader.FindFinalizer(declaringClassType, typeResolver);
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

            if (typeResolver.GetMultiThreadingSupport())
            {
                // init lock area with -1 value and shift address
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
            var isStruct = normal.IsStructureType();

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

            if (!isStruct)
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
                ilCodeBuilder.LoadArgument(0);
                if (isNullable)
                {
                    ilCodeBuilder.LoadField(type.GetFieldByFieldNumber(1, typeResolver));
                }

                ilCodeBuilder.CopyObject(normal);
            }

            ilCodeBuilder.Add(Code.Dup);
            ilCodeBuilder.Call(declaringClassType.GetFirstMethodByName(SynthesizedInitMethod.Name, typeResolver));

            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetUnboxMethod(this ITypeResolver typeResolver, IType type)
        {
            var declaringClassType = type.ToClass();

            var ilCodeBuilder = new IlCodeBuilder();

#if CHECK_NULL_IN_UNBOX_AND_RETURN_DEFAULT
            ilCodeBuilder.LoadArgument(0);
            var jumpIfNotNull = ilCodeBuilder.Branch(Code.Brtrue, Code.Brtrue_S);

            ilCodeBuilder.Locals.Add(type);
            ilCodeBuilder.LoadLocalAddress(0);
            ilCodeBuilder.InitializeObject(type);
            ilCodeBuilder.LoadLocal(0);
            ilCodeBuilder.Return();

            ilCodeBuilder.Add(jumpIfNotNull);
#endif

            if (!type.IsStructureType())
            {
                var firstField = declaringClassType.GetFieldByFieldNumber(0, typeResolver);
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
                ilCodeBuilder.LoadArgument(0);
                ilCodeBuilder.LoadObject(type);
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
            cWriter.WriteCall(
                opCode,
                method,
                false,
                false,
                false,
                null,
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
            var resAlloc = opCodePart.Result;
            cWriter.WriteCall(
                opCodePart,
                methodBase,
                opCodePart.ToCode() == Code.Callvirt,
                true,
                true,
                resAlloc,
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
        public static void WriteCallGetTypeObjectMethod(this IType type, CWriter cWriter, OpCodePart opCode)
        {
            var method = new SynthesizedGetTypeStaticMethod(type, cWriter);
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.OpCodeOperands = opCode.OpCodeOperands;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            cWriter.WriteCall(
                opCodeNope,
                method,
                false,
                false,
                false,
                opCode.Result,
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
                false,
                true,
                false,
                opCode.Result,
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
            var method = new SynthesizedNewMethod(type, cWriter);
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            opCodeNope.OpCodeOperands = opCode.OpCodeOperands;
            cWriter.WriteCall(
                opCodeNope,
                method,
                false,
                false,
                false,
                opCode.Result,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);

            cWriter.Output.WriteLine(";");
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
                false,
                true,
                false,
                null,
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

            var isStruct = declaringType.IsStructureType();

            writer.WriteLine("; Returning Hash Code");
            writer.WriteLine(string.Empty);

            writer.WriteLine("; Get data");

            if (!isStruct)
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
                if (declaringTypeNormal.IsStructureType())
                {
                    // and you need to call Init to allow to use Struct address in virtual calls for Code.Constrained
                    cWriter.Output.WriteLine(";");
                    declaringTypeNormal.WriteCallInitObjectMethod(cWriter, opCodePart);
                }
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

            // init all interfaces
            var nesting = new Stack<IType>();
            foreach (var @interface in declaringType.SelectAllTopAndAllNotFirstChildrenInterfaces(nesting))
            {
                // set virtual table
                codeBuilder.LoadArgument(0);

                // you need to have next line to select correct interface in case the same interface used many times
                foreach (var ownerInterface in nesting)
                {
                    codeBuilder.Castclass(ownerInterface);
                }

                codeBuilder.Castclass(@interface);
                codeBuilder.LoadToken(@interface.ToVirtualTableImplementation(declaringType));
                codeBuilder.SaveField(@interface.GetInterfaceVTable(typeResolver));
            }

            codeBuilder.Add(Code.Ret);

            return codeBuilder;
        }

        public static IlCodeBuilder GetGetTypeMethod(this ITypeResolver typeResolver, IType declaringType)
        {
            var codeBuilder = new IlCodeBuilder();

            codeBuilder.Call(declaringType.GetFirstMethodByName(SynthesizedGetTypeStaticMethod.Name, typeResolver));
            codeBuilder.Add(Code.Ret);

            return codeBuilder;
        }

        public static IlCodeBuilder GetGetTypeStaticMethod(this ITypeResolver typeResolver, IType declaringType)
        {
            var codeBuilder = new IlCodeBuilder();

            var typeStorageType = declaringType.GetFieldByName(ObjectInfrastructure.TypeHolderFieldName, typeResolver);
            codeBuilder.LoadField(typeStorageType);
            var jumpIfNotNull = codeBuilder.Branch(Code.Brtrue, Code.Brtrue_S);

            codeBuilder.LoadFieldAddress(typeStorageType);
            ////codeBuilder.New(Logic.IlReader.FindConstructor(typeResolver.System.System_RuntimeType, typeResolver));
            var nativeRuntimeType = typeResolver.ResolveType("System.NativeType");
            codeBuilder.Call(nativeRuntimeType.GetFirstMethodByName(SynthesizedNewMethod.Name, typeResolver));
            codeBuilder.LoadNull();

            codeBuilder.Call(
                typeResolver.ResolveType("System.Threading.Interlocked")
                            .GetMethodsByName("CompareExchange", typeResolver)
                            .First(m => m.GetParameters().First().ParameterType.TypeEquals(typeResolver.System.System_Object.ToByRefType())));

            codeBuilder.Add(Code.Pop);

            // start initializing RuntimeCache
            codeBuilder.Locals.Add(nativeRuntimeType);
            codeBuilder.LoadField(typeStorageType);
            codeBuilder.SaveLocal(0);
            codeBuilder.LoadLocal(0);
            codeBuilder.LoadString(declaringType.Name);
            codeBuilder.SaveField(nativeRuntimeType.GetFieldByName("name", typeResolver));
            codeBuilder.LoadLocal(0);
            codeBuilder.LoadString(declaringType.FullName);
            codeBuilder.SaveField(nativeRuntimeType.GetFieldByName("fullname", typeResolver));
            if (declaringType.BaseType != null)
            {
                codeBuilder.LoadLocal(0);
                codeBuilder.LoadToken(declaringType.BaseType);
                codeBuilder.Call(
                    typeResolver.System.System_Type.GetMethodsByName("GetTypeFromHandle", typeResolver).First(m => m.IsStatic && m.GetParameters().Count() == 1));
                codeBuilder.SaveField(nativeRuntimeType.GetFieldByName("baseType", typeResolver));
            }

            codeBuilder.Add(jumpIfNotNull);

            codeBuilder.LoadField(typeStorageType);
            codeBuilder.Add(Code.Ret);

            return codeBuilder;
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

            if (!declaringType.IsString)
            {
                @class.WriteCallNewObjectMethod(cWriter, opCodeConstructorInfoPart);

                opCodeConstructorInfoPart.Result = objectReference;

                cWriter.WriteCallConstructor(opCodeConstructorInfoPart);
            }
            else
            {
                // special string case
                var stringCtorMethodBase = StringGen.GetCtorMethodByParameters(
                    declaringType, opCodeConstructorInfoPart.Operand.GetParameters(), cWriter);
                var hasThis = stringCtorMethodBase.CallingConvention.HasFlag(CallingConventions.HasThis);

                OpCodePart opCodeNope = opCodeConstructorInfoPart;
                if (hasThis)
                {
                    // insert 'This' as null
                    opCodeNope = OpCodePart.CreateNop;
                    var operands = new List<OpCodePart>(opCodeConstructorInfoPart.OpCodeOperands.Length);
                    operands.AddRange(opCodeConstructorInfoPart.OpCodeOperands);

                    var opCodeThis = OpCodePart.CreateNop;
                    opCodeThis.Result = new ConstValue("0/*null*/", declaringType);
                    operands.Insert(0, opCodeThis);

                    opCodeNope.OpCodeOperands = operands.ToArray();
                }

                cWriter.WriteCall(opCodeNope, stringCtorMethodBase, false, hasThis, false, null, cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);

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
            var declaringClassType = type.ToClass();

            typeResolver.GetAllocateMemoryCodeForObject(ilCodeBuilder, declaringClassType, doNotTestNullValue, enableStringFastAllocation);

            if (!doNotCallInit)
            {
                ilCodeBuilder.Add(Code.Dup);
                ilCodeBuilder.Call(new SynthesizedInitMethod(declaringClassType, typeResolver));
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

            if (typeResolver.GetMultiThreadingSupport())
            {
                // to adjust pointer to point VTable
                ilCodeBuilder.SizeOf(type.ToPointerType());
                ilCodeBuilder.Add(Code.Add);
            }

            // TODO: can be removed when InsertMissingTypes is finished
            ilCodeBuilder.Castclass(type);
            ilCodeBuilder.Call(IlReader.FindFinalizer(type, typeResolver));

            ilCodeBuilder.Add(Code.Ret);
            return ilCodeBuilder;
        }
    }
}
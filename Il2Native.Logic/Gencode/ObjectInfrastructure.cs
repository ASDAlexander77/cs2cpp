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
    using System.Diagnostics;
    using System.Reflection.Emit;
    using CodeParts;
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

        /// <summary>
        /// </summary>
        /// <param name="declaringType">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetVirtualTableReference(this IType declaringType, ITypeResolver typeResolver)
        {
            var virtualTable = declaringType.GetVirtualTable(typeResolver);

            return string.Format(
                "getelementptr inbounds ([{0} x i8*]* {1}, i32 0, i32 {2})",
                virtualTable.GetVirtualTableSize(),
                declaringType.GetVirtualTableName(),
                FunctionsOffsetInVirtualTable);
        }

        /// <summary>
        /// </summary>
        /// <param name="declaringType">
        /// </param>
        /// <param name="interface">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetVirtualTableReference(this IType declaringType, IType @interface, ITypeResolver typeResolver)
        {
            var virtualInterfaceTable = declaringType.GetVirtualInterfaceTable(@interface, typeResolver);
            return string.Format(
                "getelementptr inbounds ([{0} x i8*]* {1}, i32 0, i32 {2})",
                virtualInterfaceTable.GetVirtualTableSize(),
                declaringType.GetVirtualInterfaceTableName(@interface),
                FunctionsOffsetInVirtualTable);
        }

        public static void WriteAllocateMemory(
            this CWriter cWriter,
            OpCodePart opCodePart,
            FullyDefinedReference size,
            bool doNotTestNullValue)
        {
            var writer = cWriter.Output;

            var mallocResult = cWriter.SetResultNumber(
                opCodePart,
                cWriter.System.System_Byte.ToPointerType());
            writer.Write("call i8* @{0}(", cWriter.GetAllocator());
            size.Type.WriteTypePrefix(cWriter);
            writer.Write(" ");
            cWriter.WriteResult(size);
            writer.WriteLine(")");

            if (!doNotTestNullValue)
            {
                cWriter.WriteTestNullValueAndThrowException(
                    writer,
                    opCodePart,
                    mallocResult,
                    "System.OutOfMemoryException",
                    "new_obj");
            }
        }

        public static IlCodeBuilder GetAllocateMemoryCodeForObject(
            this ITypeResolver typeResolver, IType declaringClassType, bool doNotTestNullValue, bool enableStringFastAllocation = false)
        {
            IlCodeBuilder newAlloc;
            if (declaringClassType.IsMultiArray)
            {
                newAlloc = ArrayMultiDimensionGen.MultiDimArrayAllocationSizeMethodBody(typeResolver, declaringClassType);
            }
            else if (declaringClassType.IsArray)
            {
                newAlloc = ArraySingleDimensionGen.SingleDimArrayAllocationSizeMethodBody(typeResolver, declaringClassType);
            }
            else if (enableStringFastAllocation && declaringClassType.IsString)
            {
                newAlloc = StringGen.StringAllocationSizeMethodBody(typeResolver, declaringClassType, typeResolver.System.System_Char);
            }
            else
            {
                newAlloc = new IlCodeBuilder();
                newAlloc.SizeOf(declaringClassType);
            }

            newAlloc.Call(
                new SynthesizedMethod(
                    typeResolver.GetAllocator(),
                    typeResolver.System.System_Void.ToPointerType(),
                    new[] { typeResolver.System.System_Int32.ToParameter() }));

            if (!doNotTestNullValue)
            {
                //cWriter.WriteTestNullValueAndThrowException(
                //    writer,
                //    opCodePart,
                //    mallocResult,
                //    "System.OutOfMemoryException",
                //    "new_obj");
            }

            newAlloc.Castclass(declaringClassType);

            return newAlloc;
        }

        public static IlCodeBuilder GetBoxMethod(
            this ITypeResolver typeResolver,
            IType type,
            bool doNotTestNullValue)
        {
            var declaringClassType = type.ToClass();
            var normal = type.ToNormal();
            var isStruct = normal.IsStructureType();

            var ilCodeBuilder = typeResolver.GetNewMethod(declaringClassType, true, doNotTestNullValue);

            ilCodeBuilder.Parameters.Add(normal.ToParameter());

            // we need to remove last code which is Code.Ret
            ilCodeBuilder.RemoveLast();

            if (!isStruct)
            {
                ilCodeBuilder.Add(Code.Dup);
                ilCodeBuilder.LoadArgument(0);
                ilCodeBuilder.SaveField(declaringClassType.GetFieldByFieldNumber(0, typeResolver));
            }
            else
            {
                // copy structure
                ilCodeBuilder.Add(Code.Dup);
                ilCodeBuilder.LoadArgumentAddress(0);
                ilCodeBuilder.CopyObject(declaringClassType);
            }

            ilCodeBuilder.Add(Code.Dup);
            ilCodeBuilder.Call(declaringClassType.GetMethodByName(SynthesizedInitMethod.Name, typeResolver));

            ilCodeBuilder.Add(Code.Ret);

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
            opCode.Result = opCode.Result.ToClassType();
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
            opCodePart.Result = null;
            cWriter.WriteCall(
                opCodePart,
                methodBase,
                opCodePart.ToCode() == Code.Callvirt,
                true,
                true,
                resAlloc,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);
            opCodePart.Result = resAlloc;
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
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            cWriter.WriteCall(
                opCodeNope,
                method,
                false,
                false,
                false,
                opCode.Result,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);
            opCode.Result = opCodeNope.Result;
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
            opCode.Result = opCodeNope.Result;

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
            var writer = cWriter.Output;

            var method = new SynthesizedUnboxMethod(type);
            writer.WriteLine(string.Empty);
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
                if (
                    !cWriter.WriteFieldAccess(
                        opCode,
                        declaringType.ToClass(),
                        declaringType.ToClass(),
                        0,
                        opCode.Result))
                {
                    writer.WriteLine("; No data");
                    return;
                }

                writer.WriteLine(string.Empty);
            }
            else
            {
                Debug.Fail("Not implemented yet");
                throw new NotImplementedException();
            }

            // load value from field
            var memberAccessResultNumber = opCode.Result;

            opCode.Result = null;
            cWriter.WriteLlvmLoad(opCode, memberAccessResultNumber.Type.ToNormal(), memberAccessResultNumber);
            writer.WriteLine(string.Empty);

            if (opCode.Result.Type.IntTypeBitSize() != cWriter.System.System_Int32.IntTypeBitSize())
            {
                cWriter.AdjustIntConvertableTypes(writer, opCode, cWriter.System.System_Int32);
                writer.WriteLine(string.Empty);
            }

            writer.WriteLine("; End of Getting data");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteInternalGetTypeMethod(this IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            var method = new SynthesizedGetTypeMethod(type, cWriter);

            var systemType = cWriter.System.System_Type;

            var opCode = OpCodePart.CreateNop;
            cWriter.WriteMethodStart(method, null);
            cWriter.WriteLlvmLoad(
                opCode,
                type.ToClass(),
                new FullyDefinedReference(cWriter.GetThisName(), cWriter.ThisType),
                true,
                true);
            writer.WriteLine(string.Empty);

            var normalType = type.ToNormal();

            cWriter.TypeTokenRequired.Add(normalType);

            cWriter.WriteGetTypeObject(opCode, normalType);

            writer.Write("ret ");
            systemType.WriteTypePrefix(cWriter);

            writer.Write(" ");
            cWriter.WriteResult(opCode.Result);

            cWriter.WriteMethodEnd(method, null);

            cWriter.methodsHaveDefinition.Add(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        public static void WriteGetTypeObject(this CWriter cWriter, OpCodePart opCode, IType declaringType)
        {
            declaringType.WriteCallGetTypeObjectMethod(cWriter, opCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="declaringTypeIn">
        /// </param>
        public static void WriteInit(this CWriter cWriter, OpCodePart opCodePart, IType declaringTypeIn)
        {
            cWriter.WriteInit(opCodePart, declaringTypeIn, opCodePart.OpCodeOperands[0].Result);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="declaringTypeIn">
        /// </param>
        /// <param name="objectSource">
        /// </param>
        public static void WriteInit(
            this CWriter cWriter,
            OpCodePart opCodePart,
            IType declaringTypeIn,
            FullyDefinedReference objectSource)
        {
            var writer = cWriter.Output;

            var declaringTypeNormal = declaringTypeIn.ToNormal();
            var declaringTypeClass = declaringTypeIn.IsValueType ? declaringTypeIn.ToClass() : declaringTypeIn;

            writer.WriteLine("// Init obj");

            cWriter.ActualWrite(writer, opCodePart.OpCodeOperands[0]);
            if (declaringTypeNormal.IsValueType)
            {
                cWriter.WriteMemSet(declaringTypeNormal, opCodePart.OpCodeOperands[0].Result);
                writer.WriteLine(string.Empty);

                if (declaringTypeNormal.IsStructureType())
                {
                    // init now
                    opCodePart.Result = objectSource;

                    declaringTypeClass.WriteCallInitObjectMethod(cWriter, opCodePart);
                    writer.WriteLine(";");
                }
            }
            else
            {
                // this is type reference, initialize it with null
                writer.WriteLine("*((i8*) ({0})) = 0;", opCodePart.OpCodeOperands[0].Result);
            }

            writer.Write("// end");
        }

        public static IlCodeBuilder GetInitMethod(this ITypeResolver typeResolver, IType declaringType)
        {
            var codeBuilder = new IlCodeBuilder();

            if (declaringType.HasAnyVirtualMethod(typeResolver))
            {
                // set virtual table
                codeBuilder.LoadArgument(0);
                codeBuilder.LoadToken(declaringType.ToVirtualTable());
                codeBuilder.SaveField(typeResolver.System.System_Object.GetFieldByName("vtable", typeResolver));
            }

            // init all interfaces
            foreach (var @interface in declaringType.SelectAllTopAndAllNotFirstChildrenInterfaces())
            {
                //cWriter.WriteInterfaceAccess(writer, opCode, thisType, @interface);

                // set virtual table
                codeBuilder.LoadArgument(0);
                codeBuilder.LoadToken(@interface.ToVirtualTable(declaringType));
                codeBuilder.SaveField(@interface.GetFieldByName("vtable", typeResolver));
            }

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
            // temp var
            declaringType.WriteTypePrefix(cWriter);
            var newVar = string.Format("_new{0}", opCodeConstructorInfoPart.AddressStart);
            cWriter.Output.WriteLine(" {0};", newVar);

            cWriter.Output.Write("{0} = ", newVar);

            declaringType.WriteCallNewObjectMethod(cWriter, opCodeConstructorInfoPart);

            opCodeConstructorInfoPart.Result = new FullyDefinedReference(newVar, declaringType);
            cWriter.WriteCallConstructor(opCodeConstructorInfoPart);
        }

        /// <summary>
        /// </summary>
        /// <param name="typeResolver">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="doNotCallInit">
        /// </param>
        /// <param name="doNotTestNullValue">
        /// </param>
        public static IlCodeBuilder GetNewMethod(
            this ITypeResolver typeResolver,
            IType type,
            bool doNotCallInit = false,
            bool doNotTestNullValue = false,
            bool enableStringFastAllocation = false)
        {
            var declaringClassType = type.ToClass();

            var ilCodeBuilder = typeResolver.GetAllocateMemoryCodeForObject(declaringClassType, doNotTestNullValue, enableStringFastAllocation);

            if (!doNotCallInit)
            {
                ilCodeBuilder.Add(Code.Dup);
                ilCodeBuilder.Call(new SynthesizedInitMethod(declaringClassType, typeResolver));
            }

            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
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

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteUnboxMethod(this IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            var method = new SynthesizedUnboxMethod(type);
            writer.WriteLine("; Unbox method");

            var opCode = OpCodePart.CreateNop;

            var normalType = type.ToNormal();
            var isStruct = normalType.IsStructureType();
            if (isStruct)
            {
                opCode.Result = new FullyDefinedReference("%agg.result", normalType);
            }

            cWriter.WriteMethodStart(method, null);
            cWriter.WriteLlvmLoad(
                opCode,
                type.ToClass(),
                new FullyDefinedReference(cWriter.GetThisName(), cWriter.ThisType),
                true,
                true);
            writer.WriteLine(string.Empty);

            var resultPresents = cWriter.WriteUnboxObject(opCode, normalType);

            writer.Write("ret ");
            if (!isStruct)
            {
                if (normalType.IsEnum)
                {
                    normalType.GetEnumUnderlyingType().WriteTypePrefix(cWriter);
                }
                else
                {
                    normalType.WriteTypePrefix(cWriter);
                }

                writer.Write(" ");

                if (resultPresents)
                {
                    cWriter.WriteResult(opCode.Result);
                }
                else
                {
                    writer.WriteLine(" undef");
                }
            }
            else
            {
                writer.WriteLine(" void");
            }

            cWriter.WriteMethodEnd(method, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool WriteUnboxObject(this CWriter cWriter, OpCodePart opCode, IType declaringType)
        {
            var writer = cWriter.Output;

            var isStruct = declaringType.IsStructureType();

            writer.WriteLine("; Unboxing");
            writer.WriteLine(string.Empty);

            writer.WriteLine("; Copy data");

            if (!isStruct)
            {
                // write access to a field
                if (!cWriter.WriteFieldAccess(
                    opCode,
                    declaringType.ToClass(),
                    declaringType.ToClass(),
                    0,
                    opCode.Result))
                {
                    writer.WriteLine("; No data");
                    return false;
                }

                writer.WriteLine(string.Empty);
            }

            // load value from field
            var memberAccessResultNumber = opCode.Result;

            if (!isStruct)
            {
                opCode.Result = null;
            }

            cWriter.WriteLlvmLoad(opCode, memberAccessResultNumber.Type.ToNormal(), memberAccessResultNumber);

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");

            return true;
        }
    }
}
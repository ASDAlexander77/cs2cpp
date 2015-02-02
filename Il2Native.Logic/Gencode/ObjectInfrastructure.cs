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
        /// <param name="llvmWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetVirtualTableReference(this IType declaringType, LlvmWriter llvmWriter)
        {
            var virtualTable = declaringType.GetVirtualTable(llvmWriter);

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
            this LlvmWriter llvmWriter,
            OpCodePart opCodePart,
            FullyDefinedReference size,
            bool doNotTestNullValue)
        {
            var writer = llvmWriter.Output;

            var mallocResult = llvmWriter.WriteSetResultNumber(
                opCodePart,
                llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.Write("call i8* @{0}(", llvmWriter.GetAllocator());
            size.Type.WriteTypePrefix(llvmWriter);
            writer.Write(" ");
            llvmWriter.WriteResult(size);
            writer.WriteLine(")");

            if (!doNotTestNullValue)
            {
                llvmWriter.WriteTestNullValueAndThrowException(
                    writer,
                    opCodePart,
                    mallocResult,
                    "System.OutOfMemoryException",
                    "new_obj");
            }

            if (!llvmWriter.Gc)
            {
                llvmWriter.WriteMemSet(mallocResult, size);
                writer.WriteLine(string.Empty);
            }
        }

        public static void WriteAllocateMemoryForObject(
            this LlvmWriter llvmWriter,
            OpCodePart opCodePart,
            IType declaringClassType,
            bool doNotTestNullValue)
        {
            var writer = llvmWriter.Output;

            var size = declaringClassType.GetTypeSize(llvmWriter);

            if (declaringClassType.IsMultiArray)
            {
                llvmWriter.WriteMultiDimArrayAllocationSize(opCodePart, declaringClassType);
            }

            var mallocResult = llvmWriter.WriteSetResultNumber(
                opCodePart,
                llvmWriter.ResolveType("System.Byte").ToPointerType());
            writer.WriteLine("call i8* @{1}(i32 {0})", size, llvmWriter.GetAllocator());

            if (!doNotTestNullValue)
            {
                llvmWriter.WriteTestNullValueAndThrowException(
                    writer,
                    opCodePart,
                    mallocResult,
                    "System.OutOfMemoryException",
                    "new_obj");
            }

            if (!llvmWriter.Gc)
            {
                llvmWriter.WriteMemSet(declaringClassType, mallocResult);
                writer.WriteLine(string.Empty);
            }

            llvmWriter.WriteBitcast(opCodePart, mallocResult, declaringClassType);
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="typeIn">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteBoxMethod(this IType typeIn, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var classType = typeIn.ToClass();
            var normalType = typeIn.ToNormal();

            var method = new SynthesizedBoxMethod(classType);
            writer.WriteLine("; Box method");

            var isStruct = classType.ToNormal().IsStructureType();

            var opCode = OpCodePart.CreateNop;

            llvmWriter.WriteMethodStart(method, null);

            llvmWriter.WriteNewMethodBody(opCode, normalType, true);
            var newObjectResult = opCode.Result;
            opCode.Result = null;

            if (!isStruct)
            {
                llvmWriter.WriteLlvmLoad(
                    opCode,
                    normalType,
                    new FullyDefinedReference(llvmWriter.GetArgVarName("value", 0), normalType));
                writer.WriteLine(string.Empty);
            }

            llvmWriter.WriteBoxObject(opCode, classType, newObjectResult, true);

            writer.Write("ret ");
            classType.WriteTypePrefix(llvmWriter);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);

            llvmWriter.WriteMethodEnd(method, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <param name="newObjectResult">
        /// </param>
        /// <param name="callInit">
        /// </param>
        public static void WriteBoxObject(
            this LlvmWriter llvmWriter,
            OpCodePart opCode,
            IType declaringType,
            FullyDefinedReference newObjectResult = null,
            bool callInit = false)
        {
            var writer = llvmWriter.Output;

            var valueLoadResult = opCode.Result;

            var isStruct = declaringType.ToNormal().IsStructureType();

            opCode.Result = null;

            writer.WriteLine("; Boxing");

            writer.WriteLine(string.Empty);
            llvmWriter.CheckIfExternalDeclarationIsRequired(declaringType);

            // call new if null
            if (newObjectResult == null)
            {
                declaringType.WriteCallNewObjectMethod(llvmWriter, opCode);
                newObjectResult = opCode.Result;
            }
            else
            {
                opCode.Result = newObjectResult;
            }

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Copy data");

            if (!isStruct)
            {
                // write access to a field
                if (!llvmWriter.WriteFieldAccess(
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

            var fieldType = declaringType.ToNormal();

            opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_0, 0, 0) };
            opCode.OpCodeOperands[0].Result = valueLoadResult;
            if (valueLoadResult == null)
            {
                llvmWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
            }

            llvmWriter.SaveToField(opCode, fieldType, 0);

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");

            if (callInit)
            {
                opCode.Result = newObjectResult;
                declaringType.WriteCallInitObjectMethod(llvmWriter, opCode);
                writer.WriteLine(string.Empty);
            }

            opCode.Result = newObjectResult.ToClassType();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallBoxObjectMethod(this IType type, LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedBoxMethod(type);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call Box Object method");
            llvmWriter.WriteCall(
                opCode,
                method,
                false,
                false,
                false,
                null,
                llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCode.Result = opCode.Result.ToClassType();
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodeConstructorInfoPart">
        /// </param>
        public static void WriteCallConstructor(
            this LlvmWriter llvmWriter,
            OpCodeConstructorInfoPart opCodeConstructorInfoPart)
        {
            llvmWriter.WriteCallConstructor(opCodeConstructorInfoPart, opCodeConstructorInfoPart.Operand);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="methodBase">
        /// </param>
        public static void WriteCallConstructor(
            this LlvmWriter llvmWriter,
            OpCodePart opCodePart,
            IConstructor methodBase)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Call Constructor");
            var resAlloc = opCodePart.Result;
            opCodePart.Result = null;
            llvmWriter.WriteCall(
                opCodePart,
                methodBase,
                opCodePart.ToCode() == Code.Callvirt,
                true,
                true,
                resAlloc,
                llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCodePart.Result = resAlloc;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallGetTypeObjectMethod(this IType type, LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedGetTypeStaticMethod(type, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call .getType Object method");
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            llvmWriter.WriteCall(
                opCodeNope,
                method,
                false,
                false,
                false,
                opCode.Result,
                llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCode.Result = opCodeNope.Result;
            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Getting data");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallInitObjectMethod(this IType type, LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedInitMethod(type, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call Init Object method");
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            llvmWriter.WriteCall(
                opCodeNope,
                method,
                false,
                true,
                false,
                opCode.Result,
                llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallNewObjectMethod(this IType type, LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedNewMethod(type, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call New Object method");
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            opCodeNope.OpCodeOperands = opCode.OpCodeOperands;
            llvmWriter.WriteCall(
                opCodeNope,
                method,
                false,
                false,
                false,
                opCode.Result,
                llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCode.Result = opCodeNope.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteCallUnboxObjectMethod(this IType type, LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedUnboxMethod(type);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call Unbox Object method");
            llvmWriter.WriteCall(
                opCode,
                method,
                false,
                true,
                false,
                null,
                llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteGetHashCodeMethodForEnum(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedGetHashCodeMethod(type, llvmWriter);
            writer.WriteLine("; default GetHashCode method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(
                opCode,
                type.ToClass(),
                new FullyDefinedReference(llvmWriter.GetThisName(), llvmWriter.ThisType),
                true,
                true);
            writer.WriteLine(string.Empty);

            var normalType = type.ToNormal();

            llvmWriter.WriteGetHashCodeObjectForEnum(opCode, normalType);

            writer.Write("ret i32");

            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);

            llvmWriter.WriteMethodEnd(method, null);

            llvmWriter.methodsHaveDefinition.Add(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        public static void WriteGetHashCodeObjectForEnum(
            this LlvmWriter llvmWriter,
            OpCodePart opCode,
            IType declaringType)
        {
            var writer = llvmWriter.Output;

            var isStruct = declaringType.IsStructureType();

            writer.WriteLine("; Returning Hash Code");
            writer.WriteLine(string.Empty);

            llvmWriter.CheckIfExternalDeclarationIsRequired(declaringType);

            writer.WriteLine("; Get data");

            if (!isStruct)
            {
                // write access to a field
                if (
                    !llvmWriter.WriteFieldAccess(
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
            llvmWriter.WriteLlvmLoad(opCode, memberAccessResultNumber.Type.ToNormal(), memberAccessResultNumber);
            writer.WriteLine(string.Empty);

            if (opCode.Result.Type.IntTypeBitSize() != llvmWriter.ResolveType("System.Int32").IntTypeBitSize())
            {
                llvmWriter.AdjustIntConvertableTypes(writer, opCode, llvmWriter.ResolveType("System.Int32"));
                writer.WriteLine(string.Empty);
            }

            writer.WriteLine("; End of Getting data");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteInternalGetTypeMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedGetTypeMethod(type, llvmWriter);

            var systemType = llvmWriter.ResolveType("System.Type");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(
                opCode,
                type.ToClass(),
                new FullyDefinedReference(llvmWriter.GetThisName(), llvmWriter.ThisType),
                true,
                true);
            writer.WriteLine(string.Empty);

            var normalType = type.ToNormal();

            llvmWriter.TypeTokenRequired.Add(normalType);

            llvmWriter.WriteGetTypeObject(opCode, normalType);

            writer.Write("ret ");
            systemType.WriteTypePrefix(llvmWriter);

            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);

            llvmWriter.WriteMethodEnd(method, null);

            llvmWriter.methodsHaveDefinition.Add(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteInternalGetSizeMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedGetSizeMethod(type, llvmWriter);

            llvmWriter.WriteMethodStart(method, null, noLocalVars:true);

            writer.Write("ret ");
            llvmWriter.GetIntTypeByByteSize(LlvmWriter.PointerSize).WriteTypePrefix(llvmWriter);
            writer.WriteLine(" {0}", type.GetTypeSize(llvmWriter));

            llvmWriter.WriteMethodEnd(method, null);

            llvmWriter.methodsHaveDefinition.Add(method);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        public static void WriteGetTypeObject(this LlvmWriter llvmWriter, OpCodePart opCode, IType declaringType)
        {
            declaringType.WriteCallGetTypeObjectMethod(llvmWriter, opCode);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="declaringTypeIn">
        /// </param>
        public static void WriteInit(this LlvmWriter llvmWriter, OpCodePart opCodePart, IType declaringTypeIn)
        {
            llvmWriter.WriteInit(opCodePart, declaringTypeIn, opCodePart.OpCodeOperands[0].Result);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="declaringTypeIn">
        /// </param>
        /// <param name="objectSource">
        /// </param>
        public static void WriteInit(
            this LlvmWriter llvmWriter,
            OpCodePart opCodePart,
            IType declaringTypeIn,
            FullyDefinedReference objectSource)
        {
            var writer = llvmWriter.Output;

            var declaringTypeNormal = declaringTypeIn.ToNormal();
            var declaringTypeClass = declaringTypeIn.IsValueType ? declaringTypeIn.ToClass() : declaringTypeIn;

            writer.WriteLine("; Init obj");

            if (declaringTypeNormal.IsValueType)
            {
                llvmWriter.WriteBitcast(opCodePart, objectSource, llvmWriter.ResolveType("System.Byte").ToPointerType());
                writer.WriteLine(string.Empty);

                llvmWriter.WriteMemSet(declaringTypeNormal, opCodePart.Result);
                writer.WriteLine(string.Empty);

                if (declaringTypeNormal.IsStructureType())
                {
                    // init now
                    opCodePart.Result = objectSource;

                    declaringTypeClass.WriteCallInitObjectMethod(llvmWriter, opCodePart);
                    writer.WriteLine(string.Empty);
                }
            }
            else
            {
                // this is type reference, initialize it with null
                llvmWriter.WriteBitcast(
                    opCodePart,
                    objectSource,
                    llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType());
                writer.WriteLine(string.Empty);

                writer.WriteLine("store i8* null, i8** {0}", opCodePart.Result);
            }

            writer.Write("; end of init obj");
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteInitObject(this LlvmWriter llvmWriter, IType declaringType, OpCodePart opCode)
        {
            if (declaringType.IsInterface)
            {
                return;
            }

            var writer = llvmWriter.Output;

            var thisType = opCode.Result.Type;

            // Init Object From Here
            if (declaringType.HasAnyVirtualMethod(llvmWriter))
            {
                var opCodeResult = opCode.Result;

                writer.WriteLine("; set virtual table");

                // initialize virtual table
                if (opCode.HasResult)
                {
                    llvmWriter.WriteBitcast(
                        opCode,
                        opCode.Result.ToClassType(),
                        llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType().ToPointerType());
                }
                else
                {
                    llvmWriter.WriteCast(
                        opCode,
                        thisType.ToClass(),
                        llvmWriter.GetThisName(),
                        llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType(),
                        true);
                }

                writer.WriteLine(string.Empty);

                writer.Write("store i8** {0}, i8*** ", declaringType.GetVirtualTableReference(llvmWriter));

                llvmWriter.WriteResult(opCode.Result);

                writer.WriteLine(string.Empty);

                // restore
                opCode.Result = opCodeResult;
            }

            // init all interfaces
            foreach (var @interface in declaringType.SelectAllTopAndAllNotFirstChildrenInterfaces())
            {
                var opCodeResult = opCode.Result;

                writer.WriteLine("; set virtual interface table");

                llvmWriter.WriteInterfaceAccess(writer, opCode, thisType, @interface);

                if (opCode.HasResult)
                {
                    llvmWriter.WriteBitcast(
                        opCode,
                        opCode.Result.ToClassType(),
                        llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType().ToPointerType());
                }
                else
                {
                    llvmWriter.WriteCast(
                        opCode,
                        @interface,
                        llvmWriter.GetThisName(),
                        llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType(),
                        true);
                }

                writer.WriteLine(string.Empty);

                writer.Write("store i8** {0}, i8*** ", declaringType.GetVirtualTableReference(@interface, llvmWriter));
                llvmWriter.WriteResult(opCode.Result);
                writer.WriteLine(string.Empty);

                // restore
                opCode.Result = opCodeResult;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="typeIn">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteInitObjectMethod(this IType typeIn, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var classType = typeIn.ToClass();
            var mainArrayType = classType;
            if (classType.IsArray && !classType.IsMultiArray)
            {
                classType = classType.BaseType;
            }

            var method = new SynthesizedInitMethod(mainArrayType, llvmWriter);
            writer.WriteLine("; Init Object method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(
                opCode,
                classType,
                new FullyDefinedReference(llvmWriter.GetThisName(), classType),
                true,
                true);
            writer.WriteLine(string.Empty);
            llvmWriter.WriteInitObject(mainArrayType, opCode);
            writer.WriteLine("ret void");
            llvmWriter.WriteMethodEnd(method, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodeConstructorInfoPart">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        public static void WriteNew(
            this LlvmWriter llvmWriter,
            OpCodeConstructorInfoPart opCodeConstructorInfoPart,
            IType declaringType)
        {
            declaringType.WriteCallNewObjectMethod(llvmWriter, opCodeConstructorInfoPart);
            llvmWriter.WriteCallConstructor(opCodeConstructorInfoPart);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="declaringTypeIn">
        /// </param>
        /// <param name="doNotCallInit">
        /// </param>
        /// <param name="doNotTestNullValue">
        /// </param>
        public static void WriteNewMethodBody(
            this LlvmWriter llvmWriter,
            OpCodePart opCodePart,
            IType declaringTypeIn,
            bool doNotCallInit = false,
            bool doNotTestNullValue = false)
        {
            var declaringClassType = declaringTypeIn.ToClass();

            var writer = llvmWriter.Output;

            writer.WriteLine("; New obj");

            llvmWriter.WriteAllocateMemoryForObject(opCodePart, declaringClassType, doNotTestNullValue);

            var castResult = opCodePart.Result;

            if (!doNotCallInit)
            {
                // llvmWriter.WriteInitObject(writer, opCode, declaringType);
                declaringClassType.WriteCallInitObjectMethod(llvmWriter, opCodePart);
            }

            // restore result and type
            opCodePart.Result = castResult;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; end of new obj");
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteNewObjectMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedNewMethod(type, llvmWriter);
            writer.WriteLine("; New Object method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteNewMethodBody(opCode, type);
            writer.WriteLine(string.Empty);
            writer.Write("ret ");
            type.WriteTypePrefix(llvmWriter);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);
            writer.WriteLine(string.Empty);
            llvmWriter.WriteMethodEnd(method, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteUnboxMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedUnboxMethod(type);
            writer.WriteLine("; Unbox method");

            var opCode = OpCodePart.CreateNop;

            var normalType = type.ToNormal();
            var isStruct = normalType.IsStructureType();
            if (isStruct)
            {
                opCode.Result = new FullyDefinedReference("%agg.result", normalType);
            }

            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(
                opCode,
                type.ToClass(),
                new FullyDefinedReference(llvmWriter.GetThisName(), llvmWriter.ThisType),
                true,
                true);
            writer.WriteLine(string.Empty);

            var resultPresents = llvmWriter.WriteUnboxObject(opCode, normalType);

            writer.Write("ret ");
            if (!isStruct)
            {
                if (normalType.IsEnum)
                {
                    normalType.GetEnumUnderlyingType().WriteTypePrefix(llvmWriter);
                }
                else
                {
                    normalType.WriteTypePrefix(llvmWriter);
                }

                writer.Write(" ");

                if (resultPresents)
                {
                    llvmWriter.WriteResult(opCode.Result);
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

            llvmWriter.WriteMethodEnd(method, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool WriteUnboxObject(this LlvmWriter llvmWriter, OpCodePart opCode, IType declaringType)
        {
            var writer = llvmWriter.Output;

            var isStruct = declaringType.IsStructureType();

            writer.WriteLine("; Unboxing");
            writer.WriteLine(string.Empty);

            llvmWriter.CheckIfExternalDeclarationIsRequired(declaringType);

            writer.WriteLine("; Copy data");

            if (!isStruct)
            {
                // write access to a field
                if (!llvmWriter.WriteFieldAccess(
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

            llvmWriter.WriteLlvmLoad(opCode, memberAccessResultNumber.Type.ToNormal(), memberAccessResultNumber);

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");

            return true;
        }
    }
}
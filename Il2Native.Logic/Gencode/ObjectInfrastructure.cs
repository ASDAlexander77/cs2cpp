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

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

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
        /// <param name="type">
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

            if (!isStruct)
            {
                llvmWriter.WriteLlvmLoad(opCode, normalType, new FullyDefinedReference(llvmWriter.GetArgVarName("value", 0), normalType));
                writer.WriteLine(string.Empty);
            }

            llvmWriter.WriteBoxObject(opCode, classType);

            writer.Write("ret ");
            classType.WriteTypePrefix(writer);
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
        public static void WriteBoxObject(this LlvmWriter llvmWriter, OpCodePart opCode, IType declaringType)
        {
            var writer = llvmWriter.Output;

            var valueLoadResult = opCode.Result;

            var isStruct = declaringType.ToNormal().IsStructureType();

            opCode.Result = null;

            writer.WriteLine("; Boxing");

            writer.WriteLine(string.Empty);
            llvmWriter.CheckIfExternalDeclarationIsRequired(declaringType);
            declaringType.WriteCallNewObjectMethod(llvmWriter, opCode);

            var newObjectResult = opCode.Result;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Copy data");

            if (!isStruct)
            {
                // write access to a field
                if (!llvmWriter.WriteFieldAccess(writer, opCode, declaringType.ToClass(), declaringType.ToClass(), 1, opCode.Result))
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
                llvmWriter.ActualWriteOpCode(writer, opCode.OpCodeOperands[0]);
            }

            llvmWriter.SaveToField(opCode, fieldType, 0);

            if (isStruct)
            {
                // init now
                declaringType.WriteCallInitObjectMethod(llvmWriter, opCode);
            }

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");

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
            llvmWriter.WriteCall(opCode, method, false, false, false, null, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCode.Result = opCode.Result.ToClassType();
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodeConstructorInfoPart">
        /// </param>
        public static void WriteCallConstructor(this LlvmWriter llvmWriter, OpCodeConstructorInfoPart opCodeConstructorInfoPart)
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
        public static void WriteCallConstructor(this LlvmWriter llvmWriter, OpCodePart opCodePart, IConstructor methodBase)
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

        public static void WriteCallNewObjectMethod(this IType type, LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedNewMethod(type, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call New Object method");
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            llvmWriter.WriteCall(opCodeNope, method, false, false, false, opCode.Result, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
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
        public static void WriteCallInitObjectMethod(this IType type, LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedInitMethod(type, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call Init Object method");
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            llvmWriter.WriteCall(opCodeNope, method, false, true, false, opCode.Result, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
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
            llvmWriter.WriteCall(opCode, method, false, true, false, null, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            if (opCode.Result != null)
            {
                opCode.Result = opCode.Result.ToClassType();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteInitObject(this LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var declaringType = opCode.Result.Type;
            if (declaringType.IsInterface)
            {
                return;
            }

            var writer = llvmWriter.Output;

            // Init Object From Here
            if (declaringType.HasAnyVirtualMethod())
            {
                var opCodeResult = opCode.Result;

                writer.WriteLine("; set virtual table");

                // initialize virtual table
                if (opCode.HasResult)
                {
                    llvmWriter.WriteBitcast(opCode, opCode.Result.ToClassType(), llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType().ToPointerType());
                }
                else
                {
                    llvmWriter.WriteCast(opCode, declaringType.ToClass(), llvmWriter.GetThisName(), llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType(), true);
                }

                writer.WriteLine(string.Empty);

                writer.Write("store i8** {0}, i8*** ", declaringType.GetVirtualTableReference(llvmWriter));
                if (opCode.HasResult)
                {
                    llvmWriter.WriteResult(opCode.Result);
                }

                writer.WriteLine(string.Empty);

                // restore
                opCode.Result = opCodeResult;
            }

            // init all interfaces
            foreach (var @interface in declaringType.SelectAllTopAndAllNotFirstChildrenInterfaces())
            {
                var opCodeResult = opCode.Result;

                writer.WriteLine("; set virtual interface table");

                llvmWriter.WriteInterfaceAccess(writer, opCode, declaringType, @interface);

                if (opCode.HasResult)
                {
                    llvmWriter.WriteBitcast(opCode, opCode.Result.ToClassType(), llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType().ToPointerType());
                }
                else
                {
                    llvmWriter.WriteCast(opCode, @interface, llvmWriter.GetThisName(), llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType(), true);
                }

                writer.WriteLine(string.Empty);

                writer.Write("store i8** {0}, i8*** ", declaringType.GetVirtualTableReference(@interface));
                llvmWriter.WriteResult(opCode.Result);
                writer.WriteLine(string.Empty);

                // restore
                opCode.Result = opCodeResult;
            }
        }

        public static string GetVirtualTableReference(this IType declaringType, LlvmWriter llvmWriter)
        {
            var virtualTable = declaringType.GetVirtualTable(llvmWriter);

            return string.Format(
                "getelementptr inbounds ([{0} x i8*]* {1}, i32 0, i32 {2})",
                virtualTable.GetVirtualTableSize(),
                declaringType.GetVirtualTableName(),
                FunctionsOffsetInVirtualTable);
        }

        public static string GetVirtualTableReference(this IType declaringType, IType @interface)
        {
            var virtualInterfaceTable = declaringType.GetVirtualInterfaceTable(@interface);
            return string.Format(
                "getelementptr inbounds ([{0} x i8*]* {1}, i32 0, i32 {2})",
                virtualInterfaceTable.GetVirtualTableSize(),
                declaringType.GetVirtualInterfaceTableName(@interface),
                FunctionsOffsetInVirtualTable);
        }

        public static void WriteNewObjectMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedNewMethod(type, llvmWriter);
            writer.WriteLine("; New Object method");

            var isStruct = type.ToNormal().IsStructureType();

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteNewMethodBody(opCode, type, isStruct);
            writer.WriteLine(string.Empty);
            writer.Write("ret ");
            type.WriteTypePrefix(writer);
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
        public static void WriteInitObjectMethod(this IType typeIn, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var classType = typeIn.ToClass();

            var method = new SynthesizedInitMethod(classType, llvmWriter);
            writer.WriteLine("; Init Object method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(opCode, classType, new FullyDefinedReference(llvmWriter.GetThisName(), llvmWriter.ThisType), true, true);
            writer.WriteLine(string.Empty);
            llvmWriter.WriteInitObject(opCode);
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
        public static void WriteNew(this LlvmWriter llvmWriter, OpCodeConstructorInfoPart opCodeConstructorInfoPart, IType declaringType)
        {
            if (opCodeConstructorInfoPart.HasResult)
            {
                return;
            }

            declaringType.WriteCallNewObjectMethod(llvmWriter, opCodeConstructorInfoPart);
            llvmWriter.WriteCallConstructor(opCodeConstructorInfoPart);
        }

        public static void WriteInit(this LlvmWriter llvmWriter, OpCodePart opCodePart, IType declaringTypeIn)
        {
            if (opCodePart.HasResult)
            {
                return;
            }

            var writer = llvmWriter.Output;

            var declaringTypeNormal = declaringTypeIn.ToNormal();
            var declaringTypeClass = (declaringTypeIn.IsValueType) ? declaringTypeIn.ToClass() : declaringTypeIn;

            writer.WriteLine("; Init obj");

            llvmWriter.PreProcessOperand(writer, opCodePart, 0);

            var fullyDefinedReference = opCodePart.OpCodeOperands[0].Result;

            if (declaringTypeNormal.IsValueType)
            {
                llvmWriter.WriteBitcast(opCodePart, fullyDefinedReference, llvmWriter.ResolveType("System.Byte").ToPointerType());
                writer.WriteLine(string.Empty);

                llvmWriter.WriteMemSet(declaringTypeClass, opCodePart.Result);
                writer.WriteLine(string.Empty);

                if (declaringTypeNormal.IsStructureType())
                {
                    // init now
                    opCodePart.Result = fullyDefinedReference;

                    declaringTypeClass.WriteCallInitObjectMethod(llvmWriter, opCodePart);
                    writer.WriteLine(string.Empty);
                }
            }
            else
            {
                // this is type reference, initialize it with null
                llvmWriter.WriteBitcast(opCodePart, fullyDefinedReference, llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType());
                writer.WriteLine(string.Empty);

                writer.WriteLine("store i8* null, i8** {0}", opCodePart.Result);
            }

            writer.Write("; end of init obj");
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodePart">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        /// <param name="doNotCallInit">
        /// </param>
        public static void WriteNewMethodBody(this LlvmWriter llvmWriter, OpCodePart opCodePart, IType declaringTypeIn, bool doNotCallInit = false, bool doNotTestNullValue = false)
        {
            if (opCodePart.HasResult)
            {
                return;
            }

            var declaringClassType = declaringTypeIn.ToClass();

            var writer = llvmWriter.Output;

            writer.WriteLine("; New obj");

            var mallocResult = llvmWriter.WriteSetResultNumber(opCodePart, llvmWriter.ResolveType("System.Byte").ToPointerType());
            var size = declaringClassType.GetTypeSize();
            writer.WriteLine("call i8* @{1}(i32 {0})", size, llvmWriter.GetAllocator());

            if (!doNotTestNullValue)
            {
                llvmWriter.WriteTestNullValueAndThrowException(writer, opCodePart, mallocResult, "System.OutOfMemoryException", "new_obj");
            }

            if (!llvmWriter.Gc)
            {
                llvmWriter.WriteMemSet(declaringClassType, mallocResult);
                writer.WriteLine(string.Empty);
            }

            llvmWriter.WriteBitcast(opCodePart, mallocResult, declaringClassType);
            writer.WriteLine(string.Empty);

            var castResult = opCodePart.Result;

            if (!doNotCallInit)
            {
                // llvmWriter.WriteInitObject(writer, opCode, declaringType);
                declaringClassType.WriteCallInitObjectMethod(llvmWriter, opCodePart);
            }

            // restore result and type
            opCodePart.Result = castResult;

            writer.WriteLine(string.Empty);
            writer.Write("; end of new obj");
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
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(opCode, type.ToClass(), new FullyDefinedReference(llvmWriter.GetThisName(), llvmWriter.ThisType), true, true);
            writer.WriteLine(string.Empty);

            var normalType = type.ToNormal();
            var isStruct = normalType.IsStructureType();
            if (isStruct)
            {
                opCode.Destination = new FullyDefinedReference("%agg.result", normalType);
            }

            var resultPresents = llvmWriter.WriteUnboxObject(opCode, normalType);

            writer.Write("ret ");
            if (!isStruct)
            {
                if (normalType.IsEnum)
                {
                    normalType.GetEnumUnderlyingType().WriteTypePrefix(writer);
                }
                else
                {
                    normalType.WriteTypePrefix(writer);
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
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteGetHashCodeMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedGetHashCodeMethod(type, llvmWriter);
            writer.WriteLine("; default GetHashCode method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(opCode, type.ToClass(), new FullyDefinedReference(llvmWriter.GetThisName(), llvmWriter.ThisType), true, true);
            writer.WriteLine(string.Empty);

            var normalType = type.ToNormal();

            llvmWriter.WriteGetHashCodeObject(opCode, normalType);

            writer.Write("ret i32");

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
        public static void WriteGetTypeMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedGetTypeMethod(type, llvmWriter);
            writer.WriteLine("; default GetType method");

            var systemType = llvmWriter.ResolveType("System.Type");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(opCode, type.ToClass(), new FullyDefinedReference(llvmWriter.GetThisName(), llvmWriter.ThisType), true, true);
            writer.WriteLine(string.Empty);

            var normalType = type.ToNormal();

            llvmWriter.WriteGetTypeObject(opCode, normalType);

            writer.Write("ret ");
            systemType.WriteTypePrefix(writer);

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
                if (!llvmWriter.WriteFieldAccess(writer, opCode, declaringType.ToClass(), declaringType.ToClass(), 1, opCode.Result))
                {
                    writer.WriteLine("; No data");
                    return false;
                }

                writer.WriteLine(string.Empty);
            }

            // load value from field
            var memberAccessResultNumber = opCode.Result;

            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, memberAccessResultNumber.Type.ToNormal(), memberAccessResultNumber);

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");

            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="declaringType">
        /// </param>
        public static void WriteGetHashCodeObject(this LlvmWriter llvmWriter, OpCodePart opCode, IType declaringType)
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
                if (!llvmWriter.WriteFieldAccess(writer, opCode, declaringType.ToClass(), declaringType.ToClass(), 1, opCode.Result))
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
                var storeResult = opCode.Result;
                var retResult = llvmWriter.WriteSetResultNumber(opCode, llvmWriter.ResolveType("System.Int32"));
                opCode.Result = storeResult;
                llvmWriter.AdjustIntConvertableTypes(writer, opCode, llvmWriter.ResolveType("System.Int32"));
                opCode.Result = retResult;
                writer.WriteLine(string.Empty);
            }

            writer.WriteLine("; End of Getting data");
        }

        public static void WriteCallGetTypeObjectMethod(this IType type, LlvmWriter llvmWriter, OpCodePart opCode)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedGetTypeStaticMethod(type, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call .getType Object method");
            var opCodeNope = OpCodePart.CreateNop;
            opCodeNope.UsedBy = new UsedByInfo(opCode);
            llvmWriter.WriteCall(opCodeNope, method, false, false, false, opCode.Result, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCode.Result = opCodeNope.Result;
            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Getting data");
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
    }
}
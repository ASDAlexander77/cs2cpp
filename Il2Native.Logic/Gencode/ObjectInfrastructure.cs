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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class ObjectInfrastructure
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteBoxMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedBoxMethod(type, llvmWriter);
            writer.WriteLine("; Box method");

            type.UseAsClass = false;
            var isStruct = type.IsStructureType();
            type.UseAsClass = true;

            var opCode = OpCodePart.CreateNop;

            llvmWriter.WriteMethodStart(method, null);

            if (!isStruct)
            {
                var normalType = type.ToNormal();
                llvmWriter.WriteLlvmLoad(opCode, normalType, new FullyDefinedReference("%.value", normalType));
                writer.WriteLine(string.Empty);
            }

            llvmWriter.WriteBoxObject(opCode, type);

            writer.Write("ret ");
            type.UseAsClass = true;
            type.WriteTypePrefix(writer);
            writer.Write(" ");
            llvmWriter.WriteResultNumber(opCode.Result);

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

            declaringType.UseAsClass = false;
            var isStruct = declaringType.IsStructureType();
            declaringType.UseAsClass = true;

            opCode.Result = null;

            writer.WriteLine("; Boxing");

            writer.WriteLine(string.Empty);
            llvmWriter.CheckIfExternalDeclarationIsRequired(declaringType);
            llvmWriter.WriteNewWithoutCallingConstructor(opCode, declaringType, isStruct);

            var newObjectResult = opCode.Result;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Copy data");

            if (!isStruct)
            {
                // write access to a field
                llvmWriter.WriteFieldAccess(writer, opCode, declaringType.ToClass(), declaringType.ToClass(), 1, opCode.Result.ToFullyDefinedReference());
                writer.WriteLine(string.Empty);
            }

            var fieldType = declaringType;

            fieldType.UseAsClass = false;

            opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_0, 0, 0) };
            opCode.OpCodeOperands[0].Result = valueLoadResult;
            llvmWriter.WriteSaveToField(opCode, fieldType, 0);

            if (isStruct)
            {
                // init now
                declaringType.WriteCallInitObjectMethod(llvmWriter, opCode);
            }

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");

            opCode.Result = newObjectResult;
            opCode.Result.Type.UseAsClass = true;
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

            var method = new SynthesizedBoxMethod(type, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call Box Object method");
            llvmWriter.WriteCall(opCode, method, false, false, false, null, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCode.Result.Type.UseAsClass = true;
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

            var method = new SynthesizedUnboxMethod(type, llvmWriter);
            writer.WriteLine(string.Empty);
            writer.WriteLine("; call Unbox Object method");
            llvmWriter.WriteCall(opCode, method, false, true, false, null, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            if (opCode.Result != null)
            {
                opCode.Result.Type.UseAsClass = false;
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

                // initializw virtual table
                if (opCode.HasResult)
                {
                    opCode.Result.Type.UseAsClass = true;
                    llvmWriter.WriteCast(opCode, opCode.Result, llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType(), true);
                }
                else
                {
                    declaringType.UseAsClass = true;
                    llvmWriter.WriteCast(opCode, declaringType, "%this", llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType(), true);
                }

                writer.WriteLine(string.Empty);

                var virtualTable = declaringType.GetVirtualTable();

                writer.Write(
                    "store i8** getelementptr inbounds ([{0} x i8*]* {1}, i64 0, i64 {2}), i8*** ", 
                    virtualTable.GetVirtualTableSize(), 
                    declaringType.GetVirtualTableName(), 
                    FunctionsOffsetInVirtualTable);
                if (opCode.HasResult)
                {
                    llvmWriter.WriteResultNumber(opCode.Result);
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
                    opCode.Result.Type.UseAsClass = true;
                    llvmWriter.WriteCast(opCode, opCode.Result, llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType(), true);
                }
                else
                {
                    llvmWriter.WriteCast(opCode, @interface, "%this", llvmWriter.ResolveType("System.Byte").ToPointerType().ToPointerType(), true);
                }

                writer.WriteLine(string.Empty);

                var virtualInterfaceTable = declaringType.GetVirtualInterfaceTable(@interface);

                writer.Write(
                    "store i8** getelementptr inbounds ([{0} x i8*]* {1}, i64 0, i64 {2}), i8*** ", 
                    virtualInterfaceTable.GetVirtualTableSize(), 
                    declaringType.GetVirtualInterfaceTableName(@interface), 
                    FunctionsOffsetInVirtualTable);
                llvmWriter.WriteResultNumber(opCode.Result);
                writer.WriteLine(string.Empty);

                // restore
                opCode.Result = opCodeResult;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteInitObjectMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedInitMethod(type, llvmWriter);
            writer.WriteLine("; Init Object method");

            type.UseAsClass = true;

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(opCode, type, new FullyDefinedReference("%.this", llvmWriter.ThisType), true, true);
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

            var writer = llvmWriter.Output;

            llvmWriter.WriteNewWithoutCallingConstructor(opCodeConstructorInfoPart, declaringType);
            llvmWriter.WriteCallConstructor(opCodeConstructorInfoPart);
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
        public static void WriteNewWithoutCallingConstructor(this LlvmWriter llvmWriter, OpCodePart opCodePart, IType declaringType, bool doNotCallInit = false)
        {
            if (opCodePart.HasResult)
            {
                return;
            }

            declaringType.UseAsClass = true;

            var writer = llvmWriter.Output;

            writer.WriteLine("; New obj");

            var mallocResult = llvmWriter.WriteSetResultNumber(opCodePart, llvmWriter.ResolveType("System.Byte").ToPointerType());
            var size = declaringType.GetTypeSize();
            writer.WriteLine("call i8* @_Znwj(i32 {0})", size);
            llvmWriter.WriteMemSet(declaringType, mallocResult);
            writer.WriteLine(string.Empty);

            llvmWriter.WriteBitcast(opCodePart, mallocResult, declaringType);
            writer.WriteLine(string.Empty);

            var castResult = opCodePart.Result;

            if (!doNotCallInit)
            {
                // llvmWriter.WriteInitObject(writer, opCode, declaringType);
                declaringType.WriteCallInitObjectMethod(llvmWriter, opCodePart);
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

            var method = new SynthesizedUnboxMethod(type, llvmWriter);
            writer.WriteLine("; Unbox method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteLlvmLoad(opCode, type.ToClass(), new FullyDefinedReference("%.this", llvmWriter.ThisType), true, true);
            writer.WriteLine(string.Empty);

            var normalType = type.ToNormal();
            var isStruct = normalType.IsStructureType();
            if (isStruct)
            {
                opCode.Destination = new FullyDefinedReference("%agg.result", normalType);
            }

            llvmWriter.WriteUnboxObject(opCode, type.ToNormal());

            writer.Write("ret ");
            if (!isStruct)
            {
                type.WriteTypePrefix(writer);
                writer.Write(" ");
                llvmWriter.WriteResultNumber(opCode.Result);
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
        public static void WriteUnboxObject(this LlvmWriter llvmWriter, OpCodePart opCode, IType declaringType)
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
                if (!llvmWriter.WriteFieldAccess(writer, opCode, declaringType.ToClass(), declaringType.ToClass(), 1, opCode.Result.ToFullyDefinedReference()))
                {
                    writer.WriteLine("; No data");
                    return;
                }

                writer.WriteLine(string.Empty);
            }

            // load value from field
            var memberAccessResultNumber = opCode.Result;

            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, memberAccessResultNumber.Type.ToNormal(), memberAccessResultNumber);

            writer.WriteLine(string.Empty);
            writer.WriteLine("; End of Copy data");
        }

        /// <summary>
        /// </summary>
        public const int FunctionsOffsetInVirtualTable = 2;

        /// <summary>
        /// </summary>
        private class SynthesizedBoxMethod : IMethod
        {
            /// <summary>
            /// </summary>
            private LlvmWriter writer;

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="writer">
            /// </param>
            public SynthesizedBoxMethod(IType type, LlvmWriter writer)
            {
                this.Type = type;
                this.writer = writer;
            }

            /// <summary>
            /// </summary>
            public string AssemblyQualifiedName { get; private set; }

            /// <summary>
            /// </summary>
            public CallingConventions CallingConvention
            {
                get
                {
                    return CallingConventions.Standard;
                }
            }

            /// <summary>
            /// </summary>
            public IType DeclaringType
            {
                get
                {
                    return this.Type.Clone();
                }
            }

            /// <summary>
            /// </summary>
            public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
            {
                get
                {
                    return new IExceptionHandlingClause[0];
                }
            }

            /// <summary>
            /// </summary>
            public string ExplicitName
            {
                get
                {
                    return string.Concat(this.Type.Name, "..box");
                }
            }

            /// <summary>
            /// </summary>
            public string FullName
            {
                get
                {
                    return string.Concat(this.Type.FullName, "..box");
                }
            }

            /// <summary>
            /// </summary>
            public string MetadataName
            {
                get
                {
                    return this.ExplicitName;
                }
            }

            /// <summary>
            /// </summary>
            public string MetadataFullName
            {
                get
                {
                    return this.FullName;
                }
            }

            /// <summary>
            /// </summary>
            public bool IsAbstract { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsConstructor { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsGenericMethod { get; private set; }

            /// <summary>
            /// custom field
            /// </summary>
            public bool IsInternalCall
            {
                get
                {
                    return false;
                }
            }

            public bool IsExternal
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// </summary>
            public bool IsOverride { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsStatic
            {
                get
                {
                    return true;
                }
            }

            /// <summary>
            /// </summary>
            public bool IsVirtual { get; private set; }

            /// <summary>
            /// </summary>
            public IEnumerable<ILocalVariable> LocalVariables
            {
                get
                {
                    return new ILocalVariable[0];
                }
            }

            /// <summary>
            /// </summary>
            public IModule Module { get; private set; }

            /// <summary>
            /// </summary>
            public string Name
            {
                get
                {
                    return string.Concat(this.Type.Name, "..box");
                }
            }

            /// <summary>
            /// </summary>
            public string Namespace { get; private set; }

            /// <summary>
            /// </summary>
            public IType ReturnType
            {
                get
                {
                    return this.Type.ToClass();
                }
            }

            /// <summary>
            /// </summary>
            public IType Type { get; private set; }

            /// <summary>
            /// </summary>
            /// <param name="obj">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <param name="obj">
            /// </param>
            /// <returns>
            /// </returns>
            public override bool Equals(object obj)
            {
                return this.ToString().Equals(obj.ToString());
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IEnumerable<IType> GetGenericArguments()
            {
                return null;
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public byte[] GetILAsByteArray()
            {
                return new byte[0];
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IMethodBody GetMethodBody(IMethod genericMethodSpecialization = null)
            {
                return new SynthesizedDummyMethodBody();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IEnumerable<IParameter> GetParameters()
            {
                return new[] { new SynthesizedValueParameter(this.Type.ToNormal()) };
            }

            /// <summary>
            /// </summary>
            /// <param name="ownerOfExplicitInterface">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public string ToString(IType ownerOfExplicitInterface)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public override string ToString()
            {
                var result = new StringBuilder();

                // write return type
                result.Append(this.ReturnType);
                result.Append(' ');

                // write Full Name
                result.Append(this.FullName);

                // write Parameter Types
                result.Append('(');
                var index = 0;
                foreach (var parameterType in this.GetParameters())
                {
                    if (index != 0)
                    {
                        result.Append(", ");
                    }

                    result.Append(parameterType);
                    index++;
                }

                result.Append(')');

                return result.ToString();
            }
        }

        /// <summary>
        /// </summary>
        public class SynthesizedDummyMethodBody : IMethodBody
        {
            /// <summary>
            /// </summary>
            public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
            {
                get
                {
                    return new IExceptionHandlingClause[0];
                }
            }

            /// <summary>
            /// </summary>
            public IEnumerable<ILocalVariable> LocalVariables
            {
                get
                {
                    return new ILocalVariable[0];
                }
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public byte[] GetILAsByteArray()
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// </summary>
        private class SynthesizedInitMethod : IMethod
        {
            /// <summary>
            /// </summary>
            private readonly LlvmWriter writer;

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="writer">
            /// </param>
            public SynthesizedInitMethod(IType type, LlvmWriter writer)
            {
                this.Type = type;
                this.writer = writer;
            }

            /// <summary>
            /// </summary>
            public string AssemblyQualifiedName { get; private set; }

            /// <summary>
            /// </summary>
            public CallingConventions CallingConvention
            {
                get
                {
                    return CallingConventions.HasThis;
                }
            }

            /// <summary>
            /// </summary>
            public IType DeclaringType
            {
                get
                {
                    return this.Type.Clone();
                }
            }

            /// <summary>
            /// </summary>
            public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
            {
                get
                {
                    return new IExceptionHandlingClause[0];
                }
            }

            /// <summary>
            /// </summary>
            public string ExplicitName
            {
                get
                {
                    return string.Concat(this.Type.Name, "..init");
                }
            }

            /// <summary>
            /// </summary>
            public string FullName
            {
                get
                {
                    return string.Concat(this.Type.FullName, "..init");
                }
            }

            /// <summary>
            /// </summary>
            public string MetadataName
            {
                get
                {
                    return this.ExplicitName;
                }
            }

            /// <summary>
            /// </summary>
            public string MetadataFullName
            {
                get
                {
                    return this.FullName;
                }
            }

            /// <summary>
            /// </summary>
            public bool IsAbstract { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsConstructor { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsGenericMethod { get; private set; }

            /// <summary>
            /// custom field
            /// </summary>
            public bool IsInternalCall
            {
                get
                {
                    return false;
                }
            }

            public bool IsExternal
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// </summary>
            public bool IsOverride { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsStatic { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsVirtual { get; private set; }

            /// <summary>
            /// </summary>
            public IEnumerable<ILocalVariable> LocalVariables
            {
                get
                {
                    return new ILocalVariable[0];
                }
            }

            /// <summary>
            /// </summary>
            public IModule Module { get; private set; }

            /// <summary>
            /// </summary>
            public string Name
            {
                get
                {
                    return string.Concat(this.Type.Name, "..init");
                }
            }

            /// <summary>
            /// </summary>
            public string Namespace { get; private set; }

            /// <summary>
            /// </summary>
            public IType ReturnType
            {
                get
                {
                    return this.writer.ResolveType("System.Void");
                }
            }

            /// <summary>
            /// </summary>
            public IType Type { get; private set; }

            /// <summary>
            /// </summary>
            /// <param name="obj">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <param name="obj">
            /// </param>
            /// <returns>
            /// </returns>
            public override bool Equals(object obj)
            {
                return this.ToString().Equals(obj.ToString());
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IEnumerable<IType> GetGenericArguments()
            {
                return null;
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public byte[] GetILAsByteArray()
            {
                return new byte[0];
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IMethodBody GetMethodBody(IMethod genericMethodSpecialization = null)
            {
                return new SynthesizedDummyMethodBody();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IEnumerable<IParameter> GetParameters()
            {
                return new IParameter[0];
            }

            /// <summary>
            /// </summary>
            /// <param name="ownerOfExplicitInterface">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public string ToString(IType ownerOfExplicitInterface)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public override string ToString()
            {
                var result = new StringBuilder();

                // write return type
                result.Append(this.ReturnType);
                result.Append(' ');

                // write Full Name
                result.Append(this.FullName);

                // write Parameter Types
                result.Append('(');
                var index = 0;
                foreach (var parameterType in this.GetParameters())
                {
                    if (index != 0)
                    {
                        result.Append(", ");
                    }

                    result.Append(parameterType);
                    index++;
                }

                result.Append(')');

                return result.ToString();
            }
        }

        /// <summary>
        /// </summary>
        private class SynthesizedUnboxMethod : IMethod
        {
            /// <summary>
            /// </summary>
            private LlvmWriter writer;

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            /// <param name="writer">
            /// </param>
            public SynthesizedUnboxMethod(IType type, LlvmWriter writer)
            {
                this.Type = type.ToNormal();
                this.writer = writer;
            }

            /// <summary>
            /// </summary>
            public string AssemblyQualifiedName { get; private set; }

            /// <summary>
            /// </summary>
            public CallingConventions CallingConvention
            {
                get
                {
                    return CallingConventions.HasThis;
                }
            }

            /// <summary>
            /// </summary>
            public IType DeclaringType
            {
                get
                {
                    return this.Type.Clone();
                }
            }

            /// <summary>
            /// </summary>
            public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
            {
                get
                {
                    return new IExceptionHandlingClause[0];
                }
            }

            /// <summary>
            /// </summary>
            public string ExplicitName
            {
                get
                {
                    return string.Concat(this.Type.Name, "..unbox");
                }
            }

            /// <summary>
            /// </summary>
            public string FullName
            {
                get
                {
                    return string.Concat(this.Type.FullName, "..unbox");
                }
            }

            /// <summary>
            /// </summary>
            public string MetadataName
            {
                get
                {
                    return this.ExplicitName;
                }
            }

            /// <summary>
            /// </summary>
            public string MetadataFullName
            {
                get
                {
                    return this.FullName;
                }
            }

            /// <summary>
            /// </summary>
            public bool IsAbstract { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsConstructor { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsGenericMethod { get; private set; }

            /// <summary>
            /// custom field
            /// </summary>
            public bool IsInternalCall
            {
                get
                {
                    return false;
                }
            }

            public bool IsExternal
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// </summary>
            public bool IsOverride { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsStatic { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsVirtual { get; private set; }

            /// <summary>
            /// </summary>
            public IEnumerable<ILocalVariable> LocalVariables
            {
                get
                {
                    return new ILocalVariable[0];
                }
            }

            /// <summary>
            /// </summary>
            public IModule Module { get; private set; }

            /// <summary>
            /// </summary>
            public string Name
            {
                get
                {
                    return string.Concat(this.Type.Name, "..unbox");
                }
            }

            /// <summary>
            /// </summary>
            public string Namespace { get; private set; }

            /// <summary>
            /// </summary>
            public IType ReturnType
            {
                get
                {
                    return this.Type;
                }
            }

            /// <summary>
            /// </summary>
            public IType Type { get; private set; }

            /// <summary>
            /// </summary>
            /// <param name="obj">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public int CompareTo(object obj)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <param name="obj">
            /// </param>
            /// <returns>
            /// </returns>
            public override bool Equals(object obj)
            {
                return this.ToString().Equals(obj.ToString());
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IEnumerable<IType> GetGenericArguments()
            {
                return null;
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public override int GetHashCode()
            {
                return this.ToString().GetHashCode();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public byte[] GetILAsByteArray()
            {
                return new byte[0];
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IMethodBody GetMethodBody(IMethod genericMethodSpecialization = null)
            {
                return new SynthesizedDummyMethodBody();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IEnumerable<IParameter> GetParameters()
            {
                return new IParameter[0];
            }

            /// <summary>
            /// </summary>
            /// <param name="ownerOfExplicitInterface">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public string ToString(IType ownerOfExplicitInterface)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public override string ToString()
            {
                var result = new StringBuilder();

                // write return type
                result.Append(this.ReturnType);
                result.Append(' ');

                // write Full Name
                result.Append(this.FullName);

                // write Parameter Types
                result.Append('(');
                var index = 0;
                foreach (var parameterType in this.GetParameters())
                {
                    if (index != 0)
                    {
                        result.Append(", ");
                    }

                    result.Append(parameterType);
                    index++;
                }

                result.Append(')');

                return result.ToString();
            }
        }

        /// <summary>
        /// </summary>
        private class SynthesizedValueParameter : IParameter
        {
            /// <summary>
            /// </summary>
            private readonly IType type;

            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            public SynthesizedValueParameter(IType type)
            {
                this.type = type.Clone();
                this.type.UseAsClass = false;
            }

            /// <summary>
            /// </summary>
            public bool IsOut
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// </summary>
            public bool IsRef
            {
                get
                {
                    return false;
                }
            }

            /// <summary>
            /// </summary>
            public string Name
            {
                get
                {
                    return "value";
                }
            }

            /// <summary>
            /// </summary>
            public IType ParameterType
            {
                get
                {
                    return this.type;
                }
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public override string ToString()
            {
                var result = new StringBuilder();

                if (this.IsRef)
                {
                    result.Append("Ref ");
                }

                if (this.IsOut)
                {
                    result.Append("Out ");
                }

                result.Append(this.ParameterType);
                return result.ToString();
            }
        }
    }
}
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
    using System.Reflection;
    using System.Text;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class ObjectInfrastructure
    {
        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCodeConstructorInfoPart">
        /// </param>
        public static void WriteCallConstructor(this LlvmWriter llvmWriter, OpCodeConstructorInfoPart opCodeConstructorInfoPart)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(string.Empty);
            writer.WriteLine("; Call Constructor");
            var methodBase = opCodeConstructorInfoPart.Operand;
            var resAlloc = opCodeConstructorInfoPart.Result;
            opCodeConstructorInfoPart.Result = null;
            llvmWriter.WriteCall(
                opCodeConstructorInfoPart,
                methodBase,
                opCodeConstructorInfoPart.ToCode() == Code.Callvirt,
                true,
                true,
                resAlloc,
                llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            opCodeConstructorInfoPart.Result = resAlloc;
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

            var method = new SynthesizedInitMethod(type);
            writer.WriteLine("; call Init Object method");
            var opCodeNope = OpCodePart.CreateNop;
            llvmWriter.WriteCall(opCodeNope, method, false, true, false, opCode.Result, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
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
                    llvmWriter.WriteCast(opCode, opCode.Result, TypeAdapter.FromType(typeof(byte**)), doNotConvert: true);
                }
                else
                {
                    llvmWriter.WriteCast(opCode, declaringType, "%this", TypeAdapter.FromType(typeof(byte**)), doNotConvert: true);
                }

                writer.WriteLine(string.Empty);

                var virtualTable = declaringType.GetVirtualTable();

                writer.Write(
                    "store i8** getelementptr inbounds ([{0} x i8*]* {1}, i64 0, i64 2), i8*** ",
                    virtualTable.GetVirtualTableSize(),
                    declaringType.GetVirtualTableName());
                if (opCode.HasResult)
                {
                    llvmWriter.WriteResultNumber(opCode.Result);
                }
                else
                {
                }

                writer.WriteLine(string.Empty);

                // restore
                opCode.Result = opCodeResult;
            }

            // init all interfaces
            foreach (var @interface in declaringType.GetInterfaces())
            {
                var opCodeResult = opCode.Result;

                writer.WriteLine("; set virtual interface table");

                llvmWriter.WriteInterfaceAccess(writer, opCode, declaringType, @interface);

                if (opCode.HasResult)
                {
                    llvmWriter.WriteCast(opCode, opCode.Result, TypeAdapter.FromType(typeof(byte**)), doNotConvert:true);
                }
                else
                {
                    llvmWriter.WriteCast(opCode, @interface, "%this", TypeAdapter.FromType(typeof(byte**)), doNotConvert: true);
                }

                writer.WriteLine(string.Empty);

                var virtualInterfaceTable = declaringType.GetVirtualInterfaceTable(@interface);

                writer.Write(
                    "store i8** getelementptr inbounds ([{0} x i8*]* {1}, i64 0, i64 1), i8*** ",
                    virtualInterfaceTable.GetVirtualTableSize(),
                    declaringType.GetVirtualInterfaceTableName(@interface));
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

            var method = new SynthesizedInitMethod(type);
            writer.WriteLine("; Init Object method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method);
            llvmWriter.WriteLlvmLoad(opCode, type, "%.this", true, true, true);
            writer.WriteLine(string.Empty);
            llvmWriter.WriteInitObject(opCode);
            writer.WriteLine("ret void");
            llvmWriter.WriteMethodEnd(method);
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

            writer.WriteLine("; New obj");

            var mallocResult = llvmWriter.WriteSetResultNumber(opCodeConstructorInfoPart, TypeAdapter.FromType(typeof(byte*)));
            var size = declaringType.GetTypeSize();
            writer.WriteLine("call i8* @_Znwj(i32 {0})", size);
            llvmWriter.WriteMemSet(declaringType, mallocResult);
            writer.WriteLine(string.Empty);

            llvmWriter.WriteBitcast(opCodeConstructorInfoPart, mallocResult, declaringType);
            writer.WriteLine(string.Empty);

            var castResult = opCodeConstructorInfoPart.Result;

            // this.WriteInitObject(writer, opCode, declaringType);
            declaringType.WriteCallInitObjectMethod(llvmWriter, opCodeConstructorInfoPart);

            // restore result and type
            opCodeConstructorInfoPart.Result = castResult;

            writer.WriteLine(string.Empty);
            writer.Write("; end of new obj");

            llvmWriter.WriteCallConstructor(opCodeConstructorInfoPart);
        }

        /// <summary>
        /// </summary>
        private class SynthesizedInitMethod : IMethod
        {
            /// <summary>
            /// </summary>
            /// <param name="type">
            /// </param>
            public SynthesizedInitMethod(IType type)
            {
                this.Type = type;
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
                    return this.Type;
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
            public string FullName
            {
                get
                {
                    return string.Concat(this.Type.FullName, "..init");
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
                    return TypeAdapter.FromType(typeof(void));
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
            public IMethodBody GetMethodBody()
            {
                return new SynthesizedInitMethodBody();
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
        private class SynthesizedInitMethodBody : IMethodBody
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
    }
}
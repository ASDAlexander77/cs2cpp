// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateGen.cs" company="">
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
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Il2Native.Logic.CodeParts;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class DelegateGen
    {
        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsDelegateFunctionBody(this IMethod method)
        {
            if (!method.IsExternal || !method.DeclaringType.IsDelegate)
            {
                return false;
            }

            return method.Name == ".ctor" || method.Name == "Invoke" || method.Name == "BeginInvoke" || method.Name == "EndInvoke";
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="method">
        /// </param>
        public static void WriteDelegateFunctionBody(this LlvmWriter llvmWriter, IMethod method)
        {
            if (method.Name == ".ctor")
            {
                llvmWriter.WriteDelegateConstructor(method);
            }
            else if (method.Name == "Invoke")
            {
                llvmWriter.WriteDelegateInvoke(method);
            }
            else
            {
                llvmWriter.DefaultStub(method);
            }
        }

        private static void WriteDelegateConstructor(this LlvmWriter llvmWriter, IMethod method)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(" {");
            writer.Indent++;

            var opCode = OpCodePart.CreateNop;

            // create this variable
            llvmWriter.WriteArgumentCopyDeclaration("this", method.DeclaringType, true);
            for (var i = 1; i <= llvmWriter.GetArgCount() + 1; i++)
            {
                llvmWriter.WriteArgumentCopyDeclaration(llvmWriter.GetArgName(i), llvmWriter.GetArgType(i));
            }

            // load 'this' variable
            llvmWriter.WriteLlvmLoad(opCode, method.DeclaringType, new FullyDefinedReference("%.this", method.DeclaringType));
            writer.WriteLine(string.Empty);

            var thisResult = opCode.Result;

            // write access to a field 1
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 1, thisResult.ToFullyDefinedReference());
            writer.WriteLine(string.Empty);

            // load value 1
            opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_1, 0, 0) };
            llvmWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
            writer.WriteLine(string.Empty);
            
            // save value 1
            llvmWriter.WriteSaveToField(opCode, opCode.Result.Type, 0);
            writer.WriteLine(string.Empty);

            // write access to a field 2
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 2, thisResult.ToFullyDefinedReference());
            writer.WriteLine(string.Empty);

            // load value 2
            opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_2, 0, 0) };
            llvmWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
            writer.WriteLine(string.Empty);

            // save value 2
            llvmWriter.WriteSaveToField(opCode, opCode.Result.Type, 0);
            writer.WriteLine(string.Empty);

            writer.WriteLine("ret void");

            writer.Indent--;
            writer.WriteLine("}");
        }

        private static void WriteDelegateInvoke(this LlvmWriter llvmWriter, IMethod method)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(" {");
            writer.Indent++;

            var opCode = OpCodePart.CreateNop;

            // create this variable
            llvmWriter.WriteArgumentCopyDeclaration("this", method.DeclaringType, true);
            for (var i = 1; i <= llvmWriter.GetArgCount() + 1; i++)
            {
                llvmWriter.WriteArgumentCopyDeclaration(llvmWriter.GetArgName(i), llvmWriter.GetArgType(i));
            }

            // load 'this' variable
            llvmWriter.WriteLlvmLoad(opCode, method.DeclaringType, new FullyDefinedReference("%.this", method.DeclaringType));
            writer.WriteLine(string.Empty);

            var thisResult = opCode.Result;

            // write access to a field 1
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 1, thisResult.ToFullyDefinedReference());
            writer.WriteLine(string.Empty);

            var objectMemberAccessResultNumber = opCode.Result;

            // load value 1
            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, objectMemberAccessResultNumber.Type, objectMemberAccessResultNumber);
            writer.WriteLine(string.Empty);

            var objectResultNumber = opCode.Result;

            // write access to a field 2
            llvmWriter.WriteFieldAccess(
                writer, opCode, method.DeclaringType, method.DeclaringType.BaseType.BaseType, 2, thisResult.ToFullyDefinedReference());
            writer.WriteLine(string.Empty);

            // load value 2
            var methodMemberAccessResultNumber = opCode.Result;

            // load value 1
            opCode.Result = null;
            llvmWriter.WriteLlvmLoad(opCode, methodMemberAccessResultNumber.Type, methodMemberAccessResultNumber);
            writer.WriteLine(string.Empty);

            var methodResultNumber = opCode.Result;

            var callResult = llvmWriter.WriteCallInvokeMethod(objectResultNumber, methodResultNumber, method);

            var returnOper = new OpCodePart(OpCodesEmit.Ret, 0, 0);
            returnOper.OpCodeOperands = new[] { OpCodePart.CreateNop };
            returnOper.OpCodeOperands[0].Result = callResult;
            llvmWriter.WriteReturn(writer, returnOper, method.ReturnType);
            writer.WriteLine(string.Empty);

            writer.Indent--;
            writer.WriteLine("}");
        }

        public static LlvmResult WriteCallInvokeMethod(this LlvmWriter llvmWriter, LlvmResult objectResult, LlvmResult methodResult, IMethod invokeMethod)
        {
            var writer = llvmWriter.Output;

            var method = new SynthesizedInvokeMethod(llvmWriter, objectResult, methodResult, invokeMethod);
            var opCodeNope = OpCodePart.CreateNop;

            opCodeNope.OpCodeOperands =
                Enumerable.Range(0, invokeMethod.GetParameters().Count())
                          .Select(p => new OpCodeInt32Part(OpCodesEmit.Ldarg, 0, 0, p + 1))
                          .ToArray();

            // bitcast object to method
            var opCodeNopeForBitCast = OpCodePart.CreateNop;
            opCodeNopeForBitCast.OpCodeOperands = new[] { OpCodePart.CreateNop };
            opCodeNopeForBitCast.OpCodeOperands[0].Result = methodResult;
            llvmWriter.UnaryOper(writer, opCodeNopeForBitCast, "bitcast", methodResult.Type, options: LlvmWriter.OperandOptions.GenerateResult);
            writer.Write(" to ");
            llvmWriter.WriteMethodPointerType(writer, method);
            writer.WriteLine(string.Empty);

            method.MethodResult = opCodeNopeForBitCast.Result;

            // write actual call
            llvmWriter.WriteCall(opCodeNope, method, false, true, false, objectResult, llvmWriter.tryScopes.Count > 0 ? llvmWriter.tryScopes.Peek() : null);
            writer.WriteLine(string.Empty);

            return opCodeNope.Result;
        }

        private static void DefaultStub(this LlvmWriter llvmWriter, IMethod method)
        {
            var writer = llvmWriter.Output;

            writer.WriteLine(" {");
            writer.Indent++;

            writer.Write("ret ");

            if (method.ReturnType.IsVoid())
            {
                writer.WriteLine("void");
            }
            else
            {
                method.ReturnType.WriteTypePrefix(writer);
                writer.WriteLine(" undef");
            }

            writer.Indent--;
            writer.WriteLine("}");
        }

        /// <summary>
        /// </summary>
        private class SynthesizedInvokeMethod : IMethod
        {
            /// <summary>
            /// </summary>
            private readonly LlvmWriter writer;

            /// <summary>
            /// </summary>
            private readonly LlvmResult objectResult;

            /// <summary>
            /// </summary>
            private readonly IMethod invokeMethod;

            /// <summary>
            /// </summary>
            /// <param name="writer">
            /// </param>
            /// <param name="methodResult">
            /// </param>
            public SynthesizedInvokeMethod(LlvmWriter writer, LlvmResult objectResult, LlvmResult methodResult, IMethod invokeMethod)
            {
                this.writer = writer;
                this.objectResult = objectResult;
                this.MethodResult = methodResult;
                this.invokeMethod = invokeMethod;
            }

            /// <summary>
            /// </summary>
            public LlvmResult MethodResult { get; set; }

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
                    return this.objectResult.Type;
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
                    return this.MethodResult.ToString();
                }
            }

            /// <summary>
            /// </summary>
            public string FullName
            {
                get
                {
                    return this.MethodResult.ToString();
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
                    return this.MethodResult.ToString();
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
                    return this.invokeMethod.ReturnType;
                }
            }

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
                return new ObjectInfrastructure.SynthesizedDummyMethodBody();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IEnumerable<IParameter> GetParameters()
            {
                return this.invokeMethod.GetParameters();
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
                return this.Name;
            }
        }
    }
}
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
    using CodeParts;
    using Microsoft.CodeAnalysis;
    using PEAssemblyReader;
    using SynthesizedMethods;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class DelegateGen
    {
        public static void GetMulticastDelegateInvoke(
            IMethod method,
            ITypeResolver typeResolver,
            out byte[] code,
            out IList<object> tokenResolutions,
            out IList<IType> locals,
            out IList<IParameter> parameters)
        {
            parameters = method.GetParameters().ToList();

            var codeList = new IlCodeBuilder();

            codeList.Add(Code.Ldarg_0);
            codeList.Add(Code.Ldfld, 1);

#if MSCORLIB
            // to load value from IntPtr
            codeList.Add(Code.Ldind_I);
#endif

            var jumpForBrtrue_S = codeList.Branch(Code.Brtrue, Code.Brtrue_S);
            codeList.Add(Code.Call, 2);
            codeList.Add(jumpForBrtrue_S);

            codeList.Add(Code.Ldc_I4_0);
            codeList.Add(Code.Stloc_0);

            var labelForFirstJump = codeList.Branch(Code.Br, Code.Br_S);

            // label
            var labelForConditionLoop = codeList.CreateLabel();

            codeList.Add(Code.Ldarg_0);
            codeList.Add(Code.Ldfld, 3);

#if MSCORLIB
            codeList.Add(Code.Castclass, 5);
#endif

            codeList.Add(Code.Ldloc_0);
            codeList.Add(Code.Ldelem_Ref);

            var index = 1;
            foreach (var parameter in parameters)
            {   
                codeList.LoadArgument(index);
                index++;
            }

            codeList.Add(Code.Callvirt, 4);

            if (!method.ReturnType.IsVoid())
            {
                codeList.Add(Code.Stloc_1);
            }

            codeList.LoadLocal(0);
            codeList.LoadConstant(1);
            codeList.Add(Code.Add);
            codeList.SaveLocal(0);

            // label
            codeList.Add(labelForFirstJump);

            // for test
            codeList.LoadLocal(0);
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldfld, 1);

#if MSCORLIB
            // to load value from IntPtr
            codeList.Add(Code.Ldind_I);
#endif

            codeList.Branch(Code.Blt, Code.Blt_S, labelForConditionLoop);

            if (!method.ReturnType.IsVoid())
            {
                codeList.Add(Code.Ldloc_1);
            }

            codeList.Add(Code.Ret);

            code = codeList.GetCode();

            locals = new List<IType>();
            locals.Add(typeResolver.System.System_Int32);
            if (!method.ReturnType.IsVoid())
            {
                locals.Add(method.ReturnType);
            }

            tokenResolutions = new List<object>();

            // 1
            tokenResolutions.Add(method.DeclaringType.BaseType.GetFieldByName("_invocationCount", typeResolver));

            // call Delegate.Invoke
            // 2
            tokenResolutions.Add(
                new SynthesizedStaticMethod(
                    string.Empty,
                    method.DeclaringType,
                    typeResolver.System.System_Void,
                    new List<IParameter>(),
                    (llvmWriter, opCode) =>
                    {
                        // get element size
                        llvmWriter.WriteDelegateInvoke(method, true, true);
                    }));

            // 3
            tokenResolutions.Add(method.DeclaringType.BaseType.GetFieldByName("_invocationList", typeResolver));

            // call Default stub for now - "ret undef";
            // 4
            tokenResolutions.Add(IlReader.Methods(method.DeclaringType, typeResolver).First(m => m.Name == "Invoke"));

#if MSCORLIB
            // 5, to case Object to Object[]
            tokenResolutions.Add(typeResolver.System.System_Object.ToArrayType(1));
#endif
        }

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

            return method.Name == ".ctor" || method.Name == "Invoke" || method.Name == "BeginInvoke" ||
                   method.Name == "EndInvoke";
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="objectResult">
        /// </param>
        /// <param name="methodResult">
        /// </param>
        /// <param name="invokeMethod">
        /// </param>
        /// <param name="isStatic">
        /// </param>
        /// <returns>
        /// </returns>
        public static FullyDefinedReference WriteCallInvokeMethod(
            this CWriter cWriter,
            FullyDefinedReference objectResult,
            FullyDefinedReference methodResult,
            IMethod invokeMethod,
            bool isStatic)
        {
            var writer = cWriter.Output;

            var method = new SynthesizedInvokeMethod(cWriter, objectResult, methodResult, invokeMethod, isStatic);
            var opCodeNope = OpCodePart.CreateNop;

            opCodeNope.OpCodeOperands =
                Enumerable.Range(0, invokeMethod.GetParameters().Count())
                    .Select(p => new OpCodeInt32Part(OpCodesEmit.Ldarg, 0, 0, p + 1))
                    .ToArray();

            foreach (var generatedOperand in opCodeNope.OpCodeOperands)
            {
                cWriter.ActualWrite(writer, generatedOperand);
            }

            writer.WriteLine(string.Empty);

            // bitcast object to method
            var opCodeNopeForBitCast = OpCodePart.CreateNop;
            opCodeNopeForBitCast.OpCodeOperands = new[] { OpCodePart.CreateNop };
            opCodeNopeForBitCast.OpCodeOperands[0].Result = methodResult;

            cWriter.UnaryOper(
                writer,
                opCodeNopeForBitCast,
                "bitcast",
                methodResult.Type);
            writer.Write(" to ");
            cWriter.WriteMethodPointerType(writer, method);
            writer.WriteLine(string.Empty);

            method.MethodResult = opCodeNopeForBitCast.Result;

            // actual call
            cWriter.WriteCall(
                opCodeNope,
                method,
                false,
                !isStatic,
                false,
                objectResult,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);
            writer.WriteLine(string.Empty);

            return opCodeNope.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="method">
        /// </param>
        public static void WriteDelegateFunctionBody(this CWriter cWriter, IMethod method)
        {
            if (method.Name == ".ctor")
            {
                cWriter.WriteDelegateConstructor(method);
            }
            else if (method.Name == "Invoke")
            {
                cWriter.WriteDelegateInvoke(method);
            }
            else
            {
                cWriter.DefaultStub(method);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="method">
        /// </param>
        public static void DefaultStub(this CWriter cWriter, IMethod method, bool disableCurlyBrakets = false)
        {
            var writer = cWriter.Output;

            if (!disableCurlyBrakets)
            {
                writer.WriteLine(" {");
                writer.Indent++;
            }

            writer.Write("ret ");

            if (method.ReturnType.IsVoid() || method.ReturnType.IsStructureType())
            {
                writer.WriteLine("void");
            }
            else
            {
                method.ReturnType.WriteTypePrefix(cWriter);
                writer.WriteLine(" undef");
            }

            if (!disableCurlyBrakets)
            {
                writer.Indent--;
                writer.WriteLine("}");
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="method">
        /// </param>
        private static void WriteDelegateConstructor(this CWriter cWriter, IMethod method)
        {
            var writer = cWriter.Output;

            writer.WriteLine(" {");
            writer.Indent++;

            var opCode = OpCodePart.CreateNop;

            // load 'this' variable
            cWriter.WriteLlvmLoad(
                opCode,
                method.DeclaringType,
                new FullyDefinedReference(cWriter.GetThisName(), method.DeclaringType));
            writer.WriteLine(string.Empty);

            var thisResult = opCode.Result;

            var delegateType = cWriter.System.System_Delegate;

            // write access to a field 1
            try
            {
                var _targetFieldIndex = cWriter.GetFieldIndex(delegateType, "_target");
                cWriter.WriteFieldAccess(
                    opCode,
                    method.DeclaringType,
                    delegateType,
                    _targetFieldIndex,
                    thisResult);
                writer.WriteLine(string.Empty);

                // load value 1
                opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_1, 0, 0) };
                cWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
                writer.WriteLine(string.Empty);

                // save value 1
                cWriter.SaveToField(opCode, opCode.Result.Type, 0);
                writer.WriteLine(string.Empty);

                // write access to a field 2
                var _methodPtrFieldIndex = cWriter.GetFieldIndex(delegateType, "_methodPtr");
                cWriter.WriteFieldAccess(
                    opCode,
                    method.DeclaringType,
                    delegateType,
                    _methodPtrFieldIndex,
                    thisResult);
                writer.WriteLine(string.Empty);

                // load value 2
                opCode.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_2, 0, 0) };
                cWriter.ActualWrite(writer, opCode.OpCodeOperands[0]);
                writer.WriteLine(string.Empty);

                // save value 2
                cWriter.SaveToField(opCode, opCode.Result.Type, 0);
                writer.WriteLine(string.Empty);
            }
            catch (KeyNotFoundException)
            {
            }

            writer.WriteLine("ret void");

            writer.Indent--;
            writer.WriteLine("}");
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="method">
        /// </param>
        private static void WriteDelegateInvoke(
            this CWriter cWriter,
            IMethod method,
            bool disableCurlyBrakets = false,
            bool disableLoadingParams = false)
        {
            var writer = cWriter.Output;

            if (!disableCurlyBrakets)
            {
                writer.WriteLine(" {");
                writer.Indent++;
            }

            var opCode = OpCodePart.CreateNop;

            // load 'this' variable
            cWriter.WriteLlvmLoad(
                opCode,
                method.DeclaringType,
                new FullyDefinedReference(cWriter.GetThisName(), method.DeclaringType));
            writer.WriteLine(string.Empty);

            var thisResult = opCode.Result;

            var delegateType = cWriter.System.System_Delegate;

            // write access to a field 1
            try
            {
                var _targetFieldIndex = cWriter.GetFieldIndex(delegateType, "_target");
                cWriter.WriteFieldAccess(
                    opCode,
                    method.DeclaringType,
                    delegateType,
                    _targetFieldIndex,
                    thisResult);
                writer.WriteLine(string.Empty);

                var objectMemberAccessResultNumber = opCode.Result;

                // load value 1
                opCode.Result = null;
                cWriter.WriteLlvmLoad(opCode, objectMemberAccessResultNumber.Type, objectMemberAccessResultNumber);
                writer.WriteLine(string.Empty);

                var objectResultNumber = opCode.Result;

                // write access to a field 2
                var _methodPtrFieldIndex = cWriter.GetFieldIndex(delegateType, "_methodPtr");
                cWriter.WriteFieldAccess(
                    opCode,
                    method.DeclaringType,
                    delegateType,
                    _methodPtrFieldIndex,
                    thisResult);
                writer.WriteLine(string.Empty);

                // additional step to extract value from IntPtr structure
                cWriter.WriteFieldAccess(opCode, opCode.Result.Type, opCode.Result.Type, 0, opCode.Result);
                writer.WriteLine(string.Empty);

                // load value 2
                var methodMemberAccessResultNumber = opCode.Result;

                // load value 1
                opCode.Result = null;
                cWriter.WriteLlvmLoad(opCode, methodMemberAccessResultNumber.Type, methodMemberAccessResultNumber);
                writer.WriteLine(string.Empty);

                var methodResultNumber = opCode.Result;

                // switch code if method is static
                var compareResult = cWriter.SetResultNumber(opCode, cWriter.System.System_Boolean);
                writer.Write("icmp ne ");
                objectResultNumber.Type.WriteTypePrefix(cWriter);
                writer.Write(" ");
                writer.Write(objectResultNumber);
                writer.WriteLine(", null");
                cWriter.WriteCondBranch(writer, compareResult, "normal", "static");

                // normal brunch
                var callResult = cWriter.WriteCallInvokeMethod(objectResultNumber, methodResultNumber, method, false);

                var returnNormal = new OpCodePart(OpCodesEmit.Ret, 0, 0);
                returnNormal.OpCodeOperands = new[] { OpCodePart.CreateNop };
                returnNormal.OpCodeOperands[0].Result = callResult;
                cWriter.WriteReturn(writer, returnNormal, method.ReturnType);
                writer.WriteLine(string.Empty);

                // static brunch
                cWriter.WriteLabel(writer, "static");

                var callStaticResult = cWriter.WriteCallInvokeMethod(
                    objectResultNumber,
                    methodResultNumber,
                    method,
                    true);

                var returnStatic = new OpCodePart(OpCodesEmit.Ret, 0, 0);
                returnStatic.OpCodeOperands = new[] { OpCodePart.CreateNop };
                returnStatic.OpCodeOperands[0].Result = callStaticResult;
                cWriter.WriteReturn(writer, returnStatic, method.ReturnType);
                writer.WriteLine(string.Empty);
            }
            catch (KeyNotFoundException)
            {
                writer.WriteLine("unreachable");
            }

            if (!disableCurlyBrakets)
            {
                writer.Indent--;
                writer.WriteLine("}");
            }
        }

        /// <summary>
        /// </summary>
        private class SynthesizedInvokeMethod : IMethod
        {
            /// <summary>
            /// </summary>
            private readonly IMethod invokeMethod;

            /// <summary>
            /// </summary>
            private readonly bool isStatic;

            /// <summary>
            /// </summary>
            private readonly FullyDefinedReference objectResult;

            /// <summary>
            /// </summary>
            private readonly CWriter writer;

            /// <summary>
            /// </summary>
            /// <param name="writer">
            /// </param>
            /// <param name="objectResult">
            /// </param>
            /// <param name="methodResult">
            /// </param>
            /// <param name="invokeMethod">
            /// </param>
            /// <param name="isStatic">
            /// </param>
            public SynthesizedInvokeMethod(
                CWriter writer,
                FullyDefinedReference objectResult,
                FullyDefinedReference methodResult,
                IMethod invokeMethod,
                bool isStatic)
            {
                this.writer = writer;
                this.objectResult = objectResult;
                this.MethodResult = methodResult;
                this.invokeMethod = invokeMethod;
                this.isStatic = isStatic;
            }

            /// <summary>
            /// </summary>
            public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
            {
                get { return new IExceptionHandlingClause[0]; }
            }

            /// <summary>
            /// </summary>
            public IEnumerable<ILocalVariable> LocalVariables
            {
                get { return new ILocalVariable[0]; }
            }

            /// <summary>
            /// </summary>
            public FullyDefinedReference MethodResult { get; set; }

            /// <summary>
            /// </summary>
            public int? Token { get; private set; }

            /// <summary>
            /// </summary>
            public string AssemblyQualifiedName { get; private set; }

            /// <summary>
            /// </summary>
            public CallingConventions CallingConvention
            {
                get { return this.isStatic ? CallingConventions.Standard : CallingConventions.HasThis; }
            }

            /// <summary>
            /// </summary>
            public IType DeclaringType
            {
                get { return this.objectResult.Type; }
            }

            /// <summary>
            /// </summary>
            public DllImportData DllImportData
            {
                get { return null; }
            }

            /// <summary>
            /// </summary>
            public string ExplicitName
            {
                get { return this.MethodResult.ToString(); }
            }

            /// <summary>
            /// </summary>
            public string FullName
            {
                get { return this.MethodResult.ToString(); }
            }

            /// <summary>
            /// </summary>
            public bool IsAbstract { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsConstructor { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsUnmanagedDllImport
            {
                get { return false; }
            }

            /// <summary>
            /// </summary>
            public bool IsExplicitInterfaceImplementation
            {
                get { return false; }
            }

            /// <summary>
            /// </summary>
            public bool IsExternal
            {
                get { return false; }
            }

            /// <summary>
            /// </summary>
            public bool IsGenericMethod { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsGenericMethodDefinition { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsOverride { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsStatic
            {
                get { return this.isStatic; }
            }

            /// <summary>
            ///     custom field
            /// </summary>
            public bool IsUnmanaged
            {
                get { return false; }
            }

            /// <summary>
            ///     custom field
            /// </summary>
            public bool IsUnmanagedMethodReference
            {
                get { return false; }
            }

            /// <summary>
            /// </summary>
            public bool IsVirtual { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsAnonymousDelegate { get; private set; }

            /// <summary>
            /// </summary>
            public string MetadataFullName
            {
                get { return this.FullName; }
            }

            /// <summary>
            /// </summary>
            public string MetadataName
            {
                get { return this.ExplicitName; }
            }

            /// <summary>
            /// </summary>
            public IModule Module { get; private set; }

            /// <summary>
            /// </summary>
            public string Name
            {
                get { return this.MethodResult.ToString(); }
            }

            /// <summary>
            /// </summary>
            public string Namespace { get; private set; }

            /// <summary>
            /// </summary>
            public IType ReturnType
            {
                get { return this.invokeMethod.ReturnType; }
            }

            /// <summary>
            /// </summary>
            public bool IsInline { get; protected set; }

            /// <summary>
            /// </summary>
            public bool HasProceduralBody { get; protected set; }

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
            /// <exception cref="NotImplementedException">
            /// </exception>
            public IEnumerable<IType> GetGenericParameters()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <param name="genericContext">
            /// </param>
            /// <returns>
            /// </returns>
            public IMethodBody GetMethodBody(IGenericContext genericContext = null)
            {
                return new SynthesizedDummyMethodBody();
            }

            /// <summary>
            /// </summary>
            /// <returns>
            /// </returns>
            public IMethod GetMethodDefinition()
            {
                return this;
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
            /// <param name="genericContext">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public IMethod ToSpecialization(IGenericContext genericContext)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// </summary>
            /// <param name="ownerOfExplicitInterface">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public string ToString(IType ownerOfExplicitInterface, bool shortName = false)
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
            /// <param name="type">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public IType ResolveTypeParameter(IType type)
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
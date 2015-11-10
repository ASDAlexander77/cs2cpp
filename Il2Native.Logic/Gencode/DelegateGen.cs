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
        public static IlCodeBuilder GetMulticastDelegateInvoke(
            this ICodeWriter codeWriter,
            IMethod method)
        {
            var codeList = new IlCodeBuilder();

            codeList.Parameters.AddRange(method.GetParameters());

            codeList.LoadArgument(0);
            var invocationCountField = method.DeclaringType.BaseType.GetFieldByName("_invocationCount", codeWriter);

            codeList.LoadFieldAddress(invocationCountField);
            // to load value from IntPtr
            codeList.LoadField(OpCodeExtensions.GetFieldByFieldNumber(codeWriter.System.System_IntPtr, 0, codeWriter));
            codeList.Add(Code.Conv_I4);

            var jumpForBrtrue_S = codeList.Branch(Code.Brtrue, Code.Brtrue_S);

            AddDelegateInvokeBody(codeList, method, codeWriter);

            codeList.Add(Code.Ret);

            codeList.Add(jumpForBrtrue_S);

            codeList.LoadConstant(0);
            codeList.SaveLocal(0);

            var labelForFirstJump = codeList.Branch(Code.Br, Code.Br_S);

            // label
            var labelForConditionLoop = codeList.CreateLabel();

            codeList.LoadArgument(0);
            codeList.LoadField(method.DeclaringType.BaseType.GetFieldByName("_invocationList", codeWriter));

            codeList.Castclass(codeWriter.System.System_Object.ToArrayType(1));

            codeList.LoadLocal(0);
            codeList.Add(Code.Ldelem_Ref);

            codeList.Castclass(method.DeclaringType);

            var index = 1;
            foreach (var parameter in method.GetParameters())
            {
                codeList.LoadArgument(index);
                index++;
            }

            codeList.Call(IlReader.Methods(method.DeclaringType, codeWriter).First(m => m.Name == "Invoke"));

            if (!method.ReturnType.IsVoid())
            {
                codeList.SaveLocal(1);
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

            codeList.LoadFieldAddress(invocationCountField);
            // to load value from IntPtr
            codeList.LoadField(OpCodeExtensions.GetFieldByFieldNumber(codeWriter.System.System_IntPtr, 0, codeWriter));
            codeList.Add(Code.Conv_I4);

            codeList.Branch(Code.Blt, Code.Blt_S, labelForConditionLoop);

            if (!method.ReturnType.IsVoid())
            {
                codeList.LoadLocal(1);
            }

            codeList.Add(Code.Ret);

            // Locals
            codeList.Locals.Add(codeWriter.System.System_Int32);
            if (!method.ReturnType.IsVoid())
            {
                codeList.Locals.Add(method.ReturnType);
            }

            return codeList;
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

            return method.Name == ".ctor" || method.Name == "Invoke" || method.Name == "BeginInvoke" || method.Name == "EndInvoke";
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
            var method = new SynthesizedInvokeMethod(methodResult, invokeMethod, isStatic);
            var opCodeMethodInfo = new OpCodeMethodInfoPart(OpCodesEmit.Call, 0, 0, method);

            var argAdditionalCount = isStatic ? 0 : 1;
            var argShift = isStatic ? 1 : 0;

            opCodeMethodInfo.OpCodeOperands =
                Enumerable.Range(0, invokeMethod.GetParameters().Count() + argAdditionalCount)
                    .Select(
                        p =>
                            !isStatic && p == 0
                                ? new OpCodePart(OpCodesEmit.Nop, 0, 0) { Result = objectResult }
                                : new OpCodeInt32Part(OpCodesEmit.Ldarg, 0, 0, p + argShift))
                    .ToArray();

            cWriter.WriteCall(
                opCodeMethodInfo,
                method,
                cWriter.tryScopes.Count > 0 ? cWriter.tryScopes.Peek() : null);

            return opCodeMethodInfo.Result;
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="method">
        /// </param>
        public static void WriteDelegateStubFunctionBody(this CWriter cWriter, IMethod method)
        {
            cWriter.DefaultStub(method);
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

#if RETURN_EMPTY
            writer.Write("return ");

            if (!method.ReturnType.IsVoid())
            {
                if (method.ReturnType.IsStructureType())
                {
                    method.ReturnType.WriteTypePrefix(cWriter);
                    writer.Write("()");
                }
                else
                {
                    writer.Write("(");
                    method.ReturnType.WriteTypePrefix(cWriter);
                    writer.Write(")0");
                }
            }
#else
            cWriter.WriteThrowException(cWriter.System.System_NotImplementedException.FullName);
#endif
            
            if (!disableCurlyBrakets)
            {
                writer.WriteLine(";");
                writer.Indent--;
                writer.WriteLine("}");
            }
        }

        public static IlCodeBuilder GetDelegateConstructorMethod(this ICodeWriter codeWriter, IType declaringType)
        {
            var codeBuilder = new IlCodeBuilder();

            codeBuilder.Parameters.Add(codeWriter.System.System_Object.ToParameter(name: "object"));
            codeBuilder.Parameters.Add(codeWriter.System.System_IntPtr.ToParameter(name: "method"));

            codeBuilder.LoadArgument(0);
            codeBuilder.LoadArgument(1);
            codeBuilder.SaveField(declaringType.GetFieldByName("_target", codeWriter, true));
            codeBuilder.LoadArgument(0);
            codeBuilder.LoadArgument(2);
            codeBuilder.SaveField(declaringType.GetFieldByName("_methodPtr", codeWriter, true));

            codeBuilder.Add(Code.Ret);

            return codeBuilder;
        }

        public static IlCodeBuilder GetDelegateInvokeMethod(this ICodeWriter codeWriter, IMethod method)
        {
            var codeBuilder = new IlCodeBuilder();
            
            codeBuilder.Parameters.AddRange(method.GetParameters());
            
            AddDelegateInvokeBody(codeBuilder, method, codeWriter);
           
            codeBuilder.Add(Code.Ret);

            return codeBuilder;
        }

        public static IlCodeBuilder GetDelegateBeginInvokeMethod(this ICodeWriter codeWriter, IMethod method)
        {
            var codeBuilder = new IlCodeBuilder();

            codeBuilder.Parameters.AddRange(method.GetParameters());

            codeBuilder.LoadNull();
            codeBuilder.Add(Code.Ret);

            return codeBuilder;
        }

        public static IlCodeBuilder GetDelegateEndInvokeMethod(this ICodeWriter codeWriter, IMethod method)
        {
            var codeBuilder = new IlCodeBuilder();

            codeBuilder.Parameters.AddRange(method.GetParameters());

            if (!method.ReturnType.IsValueType)
            {
                codeBuilder.LoadNull();
            }
            else if (!method.ReturnType.IsVoid())
            {
                codeBuilder.Locals.Add(method.ReturnType);
                codeBuilder.LoadLocalAddress(0);
                codeBuilder.InitializeObject(method.ReturnType);
                codeBuilder.LoadLocal(0);
            }

            codeBuilder.Add(Code.Ret);

            return codeBuilder;
        }

        private static void AddDelegateInvokeBody(IlCodeBuilder codeBuilder, IMethod method, ICodeWriter codeWriter)
        {
            var delegateType = codeWriter.System.System_Delegate;

            codeBuilder.LoadArgument(0);
            var targetField = OpCodeExtensions.GetFieldByName(delegateType, "_target", codeWriter, true);
            codeBuilder.LoadField(targetField);

            var jumpToThisCall = codeBuilder.Branch(Code.Brtrue, Code.Brtrue_S);

            codeBuilder.Call(GetInvokeCallMethod(method, codeWriter, true));

            var jumpToSkipThisCall = codeBuilder.Branch(Code.Br, Code.Br_S);

            codeBuilder.Add(jumpToThisCall);

            codeBuilder.Call(GetInvokeCallMethod(method, codeWriter, false));

            codeBuilder.Add(jumpToSkipThisCall);
        }

        private static SynthesizedInlinedTextMethod GetInvokeCallMethod(IMethod method, ICodeWriter codeWriter, bool isStatic)
        {
            var delegateType = codeWriter.System.System_Delegate;
            var intPtrType = codeWriter.System.System_IntPtr;

            return new SynthesizedInlinedTextMethod(
                string.Empty,
                delegateType,
                method.ReturnType,
                new IParameter[0],
                (cWriter, opCodePart) =>
                {
                    var opCodeTarget = OpCodePart.CreateNop;
                    opCodeTarget.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_0, 0, 0) };

                    var field = OpCodeExtensions.GetFieldByName(delegateType, "_methodPtr", codeWriter, true);
                    var opCodeMethodPtr = new OpCodeFieldInfoPart(OpCodesEmit.Ldfld, 0, 0, field);
                    opCodeMethodPtr.OpCodeOperands = new[] { new OpCodePart(OpCodesEmit.Ldarg_0, 0, 0) };

                    var fieldIntPtrValue = OpCodeExtensions.GetFieldByFieldNumber(intPtrType, 0, codeWriter);
                    var opCodeFieldIntPtrValue = new OpCodeFieldInfoPart(OpCodesEmit.Ldfld, 0, 0, fieldIntPtrValue);
                    opCodeFieldIntPtrValue.OpCodeOperands = new[] { opCodeMethodPtr };

                    var objRef =
                        cWriter.WriteToString(
                            () =>
                            {
                                cWriter.WriteCCastOnly(method.DeclaringType);
                                cWriter.WriteFieldAccess(opCodeTarget, OpCodeExtensions.GetFieldByName(delegateType, "_target", codeWriter, true));
                            });

                    var methodRef = cWriter.WriteToString(
                        () =>
                        {
                            cWriter.Output.Write("(*((");
                            cWriter.WriteMethodPointerType(cWriter.Output, method, asStatic: isStatic);
                            cWriter.Output.Write(")");
                            cWriter.ActualWriteOpCode(cWriter.Output, opCodeFieldIntPtrValue);
                            cWriter.Output.Write("))");
                        });

                    WriteCallInvokeMethod(
                        cWriter,
                        new FullyDefinedReference(objRef, codeWriter.System.System_Object),
                        new FullyDefinedReference(methodRef, null),
                        method,
                        isStatic);
                });
        }

        /// <summary>
        /// </summary>
        public class SynthesizedInvokeMethod : IMethod
        {
            /// <summary>
            /// </summary>
            private readonly IMethod invokeMethod;

            /// <summary>
            /// </summary>
            private readonly bool isStatic;

            /// <summary>
            /// </summary>
            private readonly string suffix;

            /// <summary>
            /// </summary>
            public SynthesizedInvokeMethod(
                FullyDefinedReference methodResult,
                IMethod invokeMethod,
                bool isStatic)
                : this(invokeMethod, isStatic, null)
            {
                this.MethodResult = methodResult;
            }

            /// <summary>
            /// </summary>
            public SynthesizedInvokeMethod(
                IMethod invokeMethod,
                bool isStatic,
                string suffix)
            {
                this.invokeMethod = invokeMethod;
                this.suffix = suffix;
                this.isStatic = isStatic;
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
            public string AssemblyFullyQualifiedName { get; private set; }

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
                get
                {
                    // it should be NULL to write name without using MethodName
                    return null;
                }
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
                get { return this.MethodResult != null ? this.MethodResult.ToString() : string.Concat(this.invokeMethod.ExplicitName, this.suffix); }
            }

            /// <summary>
            /// </summary>
            public string FullName
            {
                get { return this.MethodResult != null ? this.MethodResult.ToString() : string.Concat(this.invokeMethod.FullName, this.suffix); }
            }

            /// <summary>
            /// </summary>
            public bool IsAbstract { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsConstructor { get; private set; }

            /// <summary>
            /// </summary>
            public bool IsDestructor { get; private set; }

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
            public IType ExplicitInterface
            {
                get { return null; }
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
            public bool IsMerge
            {
                get
                {
                    return false;
                }
            }

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
                get { return this.MethodResult != null ? this.MethodResult.ToString() : string.Concat(this.invokeMethod.Name, this.suffix); }
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
            public bool IsPublic
            {
                get { return false; }
            }

            /// <summary>
            /// </summary>
            public bool IsInternal
            {
                get { return true; }
            }

            /// <summary>
            /// </summary>
            public bool IsProtected
            {
                get { return false; }
            }

            /// <summary>
            /// </summary>
            public bool IsPrivate
            {
                get { return false; }
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
                return this.ToString();
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
            public override string ToString()
            {
                return this.Name;
            }
        }
    }
}
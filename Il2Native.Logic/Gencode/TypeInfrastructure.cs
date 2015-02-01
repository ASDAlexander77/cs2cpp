// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeInfrastructure.cs" company="">
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
    using System.Text;
    using CodeParts;
    using PEAssemblyReader;
    using SynthesizedMethods;

    /// <summary>
    /// </summary>
    public static class TypeInfrastructure
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static byte[] GenerateTypeInfoBytes(this IType type, ITypeResolver typeResolver)
        {
            var bytes = new List<byte>();
            bytes.AddRange(Encoding.ASCII.GetBytes(type.FullName));
            return bytes.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static string GetTypeStaticFieldName(this IType type)
        {
            return string.Format("@\"{0}..type\"", type.FullName);
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsTypeOfCallFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            return method.FullName == "System.Type.GetTypeFromHandle";
        }

        /// <summary>
        /// </summary>
        /// <param name="typeIn">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteGetTypeStaticMethod(this IType typeIn, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var classType = typeIn.ToClass();

            var systemType = llvmWriter.ResolveType("System.Type");
            var method = new SynthesizedGetTypeStaticMethod(classType, llvmWriter);
            writer.WriteLine("; Get Type Object method");

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null, true);
            llvmWriter.WriteGetOrCreateRuntimeTypeStaticObject(opCode, classType, systemType);

            writer.Write("ret ");
            systemType.WriteTypePrefix(llvmWriter);
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
        /// <param name="type">
        /// </param>
        /// <param name="operandType">
        /// </param>
        public static void WriteGetOrCreateRuntimeTypeStaticObject(
            this LlvmWriter llvmWriter,
            OpCodePart opCode,
            IType type,
            IType operandType)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteLlvmLoad(
                opCode,
                operandType,
                new FullyDefinedReference(type.GetTypeStaticFieldName(), operandType));
            writer.WriteLine(string.Empty);

            var result = opCode.Result;

            var testNullResultNumber = llvmWriter.WriteTestNull(writer, opCode, opCode.Result);

            llvmWriter.WriteBranchSwitchToExecute(
                writer,
                opCode,
                testNullResultNumber,
                operandType.FullName,
                "gettype",
                "new",
                () =>
                {
                    // TODO: here send predifined byte array data with info for Type
                    IType runtimeType = null;
                    try
                    {
                        runtimeType = llvmWriter.ResolveType("System.RuntimeType");
                    }
                    catch (KeyNotFoundException)
                    {
                        opCode.Result = new ConstValue(null, llvmWriter.ResolveType("System.Type"));
                        writer.Write("ret ");
                        llvmWriter.ResolveType("System.Type").WriteTypePrefix(llvmWriter);
                        writer.WriteLine(" null");
                        return;
                    }

                    var byteType = llvmWriter.ResolveType("System.Byte");
                    var byteArrayType = byteType.ToArrayType(1);
                    var bytes = type.GenerateTypeInfoBytes(llvmWriter);
                    var bytesIndex = llvmWriter.GetBytesIndex(bytes);
                    var firstParameterValue =
                        new FullyDefinedReference(
                            llvmWriter.GetArrayTypeReference(
                                string.Format("@.bytes{0}", bytesIndex),
                                byteType,
                                bytes.Length),
                            byteArrayType);

                    opCode.Result = null;
                    var newObjectResult = llvmWriter.WriteNewWithCallingConstructor(
                        opCode,
                        runtimeType,
                        byteArrayType,
                        firstParameterValue,
                        llvmWriter);
                    writer.WriteLine(string.Empty);

                    // call cmp exchnage
                    var noOpCmpXchg = OpCodePart.CreateNop;
                    noOpCmpXchg.OpCodeOperands = new[]
                    {
                        OpCodePart.CreateNop,
                        OpCodePart.CreateNop,
                        OpCodePart.CreateNop
                    };
                    noOpCmpXchg.OpCodeOperands[0].Result = new FullyDefinedReference(
                        type.GetTypeStaticFieldName(),
                        operandType.ToPointerType());
                    noOpCmpXchg.OpCodeOperands[1].Result = newObjectResult;
                    noOpCmpXchg.OpCodeOperands[2].Result = new ConstValue(null, operandType);
                    noOpCmpXchg.InterlockBase(
                        "cmpxchg ",
                        llvmWriter.IsLlvm34OrLower ? " acq_rel" : " acq_rel monotonic",
                        !llvmWriter.IsLlvm35 && !llvmWriter.IsLlvm34OrLower,
                        llvmWriter,
                        new[] { 0, 2, 1 });
                    writer.WriteLine(string.Empty);

                    // load again
                    opCode.Result = null;
                    llvmWriter.WriteLlvmLoad(
                        opCode,
                        operandType,
                        new FullyDefinedReference(type.GetTypeStaticFieldName(), operandType));
                    writer.WriteLine(string.Empty);

                    writer.Write("ret ");
                    opCode.Result.Type.WriteTypePrefix(llvmWriter);
                    writer.Write(" ");
                    llvmWriter.WriteResult(opCode.Result);
                    writer.WriteLine(string.Empty);
                });

            opCode.Result = result;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static bool WriteTypeOfFunction(this OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            // call .getType
            var typeInfo = opCodeMethodInfo.OpCodeOperands[0] as OpCodeTypePart;
            if (typeInfo == null)
            {
                return false;
            }

            typeInfo.Operand.WriteCallGetTypeObjectMethod(llvmWriter, opCodeMethodInfo);
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteTypeStorageStaticField(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.Write(
                "{0} = {1}global ",
                type.GetTypeStaticFieldName(),
                "linkonce_odr ");
            llvmWriter.ResolveType("System.Type").WriteTypePrefix(llvmWriter);
            writer.WriteLine(" null");
        }
    }
}
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
        /// <param name="cWriter">
        /// </param>
        public static void WriteGetTypeStaticMethod(this IType typeIn, CWriter cWriter)
        {
            var writer = cWriter.Output;

            var classType = typeIn.ToClass();

            var systemType = cWriter.System.System_Type;
            var method = new SynthesizedGetTypeStaticMethod(classType, cWriter);
            writer.WriteLine("; Get Type Object method");

            var opCode = OpCodePart.CreateNop;
            cWriter.WriteMethodStart(method, null, true);
            cWriter.WriteGetOrCreateRuntimeTypeStaticObject(opCode, classType, systemType);

            writer.Write("ret ");
            systemType.WriteTypePrefix(cWriter);
            writer.Write(" ");
            cWriter.WriteResult(opCode.Result);

            cWriter.WriteMethodEnd(method, null);
        }

        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="operandType">
        /// </param>
        public static void WriteGetOrCreateRuntimeTypeStaticObject(
            this CWriter cWriter,
            OpCodePart opCode,
            IType type,
            IType operandType)
        {
            var writer = cWriter.Output;

            cWriter.WriteLlvmLoad(
                opCode,
                operandType,
                new FullyDefinedReference(type.GetTypeStaticFieldName(), operandType));
            writer.WriteLine(string.Empty);

            var result = opCode.Result;

            var testNullResultNumber = cWriter.WriteTestNull(writer, opCode, opCode.Result);

            cWriter.WriteBranchSwitchToExecute(
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
                        runtimeType = cWriter.System.System_RuntimeType;
                    }
                    catch (KeyNotFoundException)
                    {
                        opCode.Result = new ConstValue(null, cWriter.System.System_Type);
                        writer.Write("ret ");
                        cWriter.System.System_Type.WriteTypePrefix(cWriter);
                        writer.WriteLine(" null");
                        return;
                    }

                    var byteType = cWriter.System.System_Byte;
                    var byteArrayType = byteType.ToArrayType(1);
                    var bytes = type.GenerateTypeInfoBytes(cWriter);
                    var bytesIndex = cWriter.GetBytesIndex(bytes);
                    var firstParameterValue =
                        new FullyDefinedReference(
                            cWriter.GetArrayTypeReference(
                                string.Format("@.bytes{0}", bytesIndex),
                                byteType,
                                bytes.Length),
                            byteArrayType);

                    opCode.Result = null;
                    var newObjectResult = cWriter.WriteNewWithCallingConstructor(
                        opCode,
                        runtimeType,
                        byteArrayType,
                        firstParameterValue);
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
                        cWriter.IsLlvm34OrLower ? " acq_rel" : " acq_rel monotonic",
                        !cWriter.IsLlvm35 && !cWriter.IsLlvm34OrLower,
                        cWriter,
                        new[] { 0, 2, 1 });
                    writer.WriteLine(string.Empty);

                    // load again
                    opCode.Result = null;
                    cWriter.WriteLlvmLoad(
                        opCode,
                        operandType,
                        new FullyDefinedReference(type.GetTypeStaticFieldName(), operandType));
                    writer.WriteLine(string.Empty);

                    writer.Write("ret ");
                    opCode.Result.Type.WriteTypePrefix(cWriter);
                    writer.Write(" ");
                    cWriter.WriteResult(opCode.Result);
                    writer.WriteLine(string.Empty);
                });

            opCode.Result = result;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static bool WriteTypeOfFunction(this OpCodePart opCodeMethodInfo, CWriter cWriter)
        {
            // call .getType
            var typeInfo = opCodeMethodInfo.OpCodeOperands[0] as OpCodeTypePart;
            if (typeInfo == null)
            {
                return false;
            }

            typeInfo.Operand.WriteCallGetTypeObjectMethod(cWriter, opCodeMethodInfo);
            return true;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteTypeStorageStaticField(this IType type, CWriter cWriter)
        {
            var writer = cWriter.Output;

            writer.Write(
                "{0} = {1}global ",
                type.GetTypeStaticFieldName(),
                "linkonce_odr ");
            cWriter.System.System_Type.WriteTypePrefix(cWriter);
            writer.WriteLine(" null");
        }
    }
}
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
    using System.Diagnostics;
    using System.Text;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    public static class TypeInfrastructure
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteTypeStorageStaticField(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            writer.Write("{0} = {1}global ", type.GetTypeStaticFieldName(), type.IsGenericType ? "linkonce_odr " : string.Empty);
            llvmWriter.ResolveType("System.Type").WriteTypePrefix(writer);
            writer.WriteLine(" null");
        }

        public static string GetTypeStaticFieldName(this IType type)
        {
            return string.Format("@\"{0}..type\"", type.FullName);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        public static void WriteGetTypeStaticObject(this LlvmWriter llvmWriter, OpCodePart opCode, IType type, IType operandType)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteLlvmLoad(opCode, operandType, new FullyDefinedReference(type.GetTypeStaticFieldName(), operandType));
            writer.WriteLine(string.Empty);

            var result = opCode.Result;

            var testNullResultNumber = llvmWriter.WriteTestNull(writer, opCode, opCode.Result);

            llvmWriter.WriteBranchSwitchToExecute(writer,
                opCode,
                testNullResultNumber,
                operandType.FullName,
                "gettype",
                "new",
                () =>
                {
                    // TODO: here send predifined byte array data with info for Type
                    var runtimeType = llvmWriter.ResolveType("System.RuntimeType");
                    var byteType = llvmWriter.ResolveType("System.Byte");
                    var byteArrayType = byteType.ToArrayType(1);
                    var bytes = type.GenerateTypeInfoBytes();
                    var bytesIndex = llvmWriter.GetBytesIndex(bytes);
                    var firstParameterValue = new FullyDefinedReference(
                            llvmWriter.GetArrayTypeReference(string.Format("@.bytes{0}", bytesIndex), byteType, bytes.Length),
                            byteArrayType);

                    opCode.Result = null;
                    var newObjectResult = llvmWriter.WriteNewWithCallingConstructor(opCode, runtimeType, byteArrayType, firstParameterValue);
                    writer.WriteLine(string.Empty);

                    // call cmp exchnage
                    var noOpCmpXchg = OpCodePart.CreateNop;
                    noOpCmpXchg.OpCodeOperands = new[] { OpCodePart.CreateNop, OpCodePart.CreateNop, OpCodePart.CreateNop };
                    noOpCmpXchg.OpCodeOperands[0].Result = new FullyDefinedReference(type.GetTypeStaticFieldName(), operandType.ToPointerType());
                    noOpCmpXchg.OpCodeOperands[1].Result = new ConstValue(null, operandType);
                    noOpCmpXchg.OpCodeOperands[2].Result = newObjectResult;
                    noOpCmpXchg.InterlockBase("cmpxchg ", " acq_rel monotonic", llvmWriter.IsLlvm35OrLess, llvmWriter);
                    writer.WriteLine(string.Empty);

                    // load again
                    opCode.Result = null;
                    llvmWriter.WriteLlvmLoad(opCode, operandType, new FullyDefinedReference(type.GetTypeStaticFieldName(), operandType));
                    writer.WriteLine(string.Empty);

                    writer.Write("ret ");
                    opCode.Result.Type.WriteTypePrefix(writer);
                    writer.Write(" ");
                    llvmWriter.WriteResult(opCode.Result);
                    writer.WriteLine(string.Empty);
                });

            opCode.Result = result;
        }

        public static byte[] GenerateTypeInfoBytes(this IType type)
        {
            return Encoding.ASCII.GetBytes(type.FullName);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="llvmWriter">
        /// </param>
        public static void WriteGetTypeStaticMethod(this IType type, LlvmWriter llvmWriter)
        {
            var writer = llvmWriter.Output;

            var systemType = llvmWriter.ResolveType("System.Type");
            var method = new SynthesizedGetTypeStaticMethod(type, llvmWriter);
            writer.WriteLine("; Get Type Object method");

            type.UseAsClass = true;

            var opCode = OpCodePart.CreateNop;
            llvmWriter.WriteMethodStart(method, null);
            llvmWriter.WriteGetTypeStaticObject(opCode, type, systemType);

            writer.Write("ret ");
            systemType.WriteTypePrefix(writer);
            writer.Write(" ");
            llvmWriter.WriteResult(opCode.Result);

            llvmWriter.WriteMethodEnd(method, null);
        }

        public static bool IsTypeOfCallFunction(this IMethod method)
        {
            if (!method.IsStatic)
            {
                return false;
            }

            return method.FullName == "System.Type.GetTypeFromHandle";
        }

        public static void WriteTypeOfFunction(this OpCodePart opCodeMethodInfo, LlvmWriter llvmWriter)
        {
            // call .getType
            var typeInfo = opCodeMethodInfo.OpCodeOperands[0] as OpCodeTypePart;
            typeInfo.Operand.WriteCallGetTypeObjectMethod(llvmWriter, opCodeMethodInfo);
        }
    }
}
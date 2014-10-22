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

            writer.Write("{0} = global ", type.GetTypeStaticFieldName());
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
                    var newObjectResult = llvmWriter.WriteNew(writer, operandType.FullName);

                    // store new value
                    writer.Write("store ");
                    newObjectResult.Type.WriteTypePrefix(writer);
                    writer.Write(" ");
                    llvmWriter.WriteResult(newObjectResult);
                    writer.Write(", ");
                    newObjectResult.Type.WriteTypePrefix(writer);
                    writer.WriteLine("* {0}", type.GetTypeStaticFieldName());
                    opCode.Result = null;
                    llvmWriter.WriteLlvmLoad(opCode, operandType, new FullyDefinedReference(type.GetTypeStaticFieldName(), operandType));
                    writer.WriteLine(string.Empty);



                    writer.Write("ret ");
                    opCode.Result.Type.WriteTypePrefix(writer);
                    writer.Write(" ");
                    llvmWriter.WriteResult(opCode.Result);
                });

            opCode.Result = result;
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
            var method = new SynthesizedGetTypeMethod(type, systemType);
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

    }
}
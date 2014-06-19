// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LlvmHelpersGen.cs" company="">
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

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public static class LlvmHelpersGen
    {
        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <param name="localVarName">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <param name="structAsRef">
        /// </param>
        public static void WriteLlvmLoad(
            this LlvmWriter llvmWriter, OpCodePart opCode, IType type, string localVarName, bool appendReference = true, bool structAsRef = false)
        {
            if (opCode.ResultNumber.HasValue)
            {
                return;
            }

            var writer = llvmWriter.Output;

            if (!type.IsStructureType() || structAsRef)
            {
                llvmWriter.WriteSetResultNumber(writer, opCode, type);

                // last part
                writer.Write("load ");
                type.WriteTypePrefix(writer, structAsRef);
                if (appendReference)
                {
                    // add reference to type
                    writer.Write('*');
                }

                writer.Write(' ');
                writer.Write(localVarName);

                // TODO: optional do we need to calculate it propertly?
                writer.Write(", align " + LlvmWriter.pointerSize);
            }
            else
            {
                Debug.Assert(opCode.DestinationName != null);
                llvmWriter.WriteCopyStruct(writer, opCode, type, localVarName, opCode.DestinationName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <param name="asReference">
        /// </param>
        public static void WriteLlvmLocalVarAccess(this LlvmWriter llvmWriter, int index, bool asReference = false)
        {
            var writer = llvmWriter.Output;

            llvmWriter.LocalInfo[index].LocalType.WriteTypePrefix(writer, false);
            if (asReference)
            {
                writer.Write('*');
            }

            writer.Write(' ');
            writer.Write(llvmWriter.GetLocalVarName(index));

            // TODO: optional do we need to calculate it propertly?
            writer.Write(", align " + LlvmWriter.pointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="type">
        /// </param>
        public static void WriteAlloca(this LlvmWriter llvmWriter, IType type)
        {
            var writer = llvmWriter.Output;

            // for value types
            writer.Write("alloca ");
            type.WriteTypePrefix(writer);
            writer.Write(", align " + LlvmWriter.pointerSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="res">
        /// </param>
        /// <param name="toType">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, int res, IType toType)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.Write("bitcast i8* ");
            llvmWriter.WriteResultNumber(res);
            writer.Write(" to ");
            toType.WriteTypePrefix(writer, true);
            opCode.ResultType = toType;
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="options">
        /// </param>
        public static void WriteBitcast(
            this LlvmWriter llvmWriter, OpCodePart opCode, IType toType, LlvmWriter.OperandOptions options = LlvmWriter.OperandOptions.None)
        {
            var writer = llvmWriter.Output;
            llvmWriter.UnaryOper(writer, opCode, "bitcast", opCode.ResultType, options: options);
            writer.Write(" to ");
            toType.WriteTypePrefix(writer, true);
            opCode.ResultType = TypeAdapter.FromType(typeof(byte*));
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="name">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, IType fromType, string name)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.Write("bitcast ");
            fromType.WriteTypePrefix(writer, true);
            writer.Write(" ");
            writer.Write(name);
            writer.Write(" to i8*");
            opCode.ResultType = TypeAdapter.FromType(typeof(byte*));
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="res">
        /// </param>
        /// <param name="custom">
        /// </param>
        public static void WriteBitcast(this LlvmWriter llvmWriter, OpCodePart opCode, IType fromType, int res, string custom)
        {
            var writer = llvmWriter.Output;

            llvmWriter.WriteSetResultNumber(writer, opCode);
            writer.Write("bitcast ");
            fromType.WriteTypePrefix(writer, true);
            writer.Write(' ');
            llvmWriter.WriteResultNumber(res);
            writer.Write(" to ");
            writer.Write(custom);

            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="res">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        public static void WriteCast(this LlvmWriter llvmWriter, OpCodePart opCode, IType fromType, int res, IType toType, bool appendReference = false)
        {
            var writer = llvmWriter.Output;

            if (!fromType.IsInterface && toType.IsInterface)
            {
                opCode.ResultNumber = res;
                llvmWriter.WriteInterfaceAccess(writer, opCode, fromType, toType);
            }
            else
            {
                llvmWriter.WriteSetResultNumber(writer, opCode);
                writer.Write("bitcast ");
                fromType.WriteTypePrefix(writer, true);
                writer.Write(' ');
                llvmWriter.WriteResultNumber(res);
                writer.Write(" to ");
                toType.WriteTypePrefix(writer, true);
                if (appendReference)
                {
                    // result should be array
                    writer.Write('*');
                }
            }

            opCode.ResultType = toType;
            writer.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        /// <param name="llvmWriter">
        /// </param>
        /// <param name="opCode">
        /// </param>
        /// <param name="fromType">
        /// </param>
        /// <param name="custromName">
        /// </param>
        /// <param name="toType">
        /// </param>
        /// <param name="appendReference">
        /// </param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public static void WriteCast(
            this LlvmWriter llvmWriter, OpCodePart opCode, IType fromType, string custromName, IType toType, bool appendReference = false)
        {
            var writer = llvmWriter.Output;

            if (!fromType.IsInterface && toType.IsInterface)
            {
                throw new NotImplementedException();

                ////opCode.ResultNumber = res;
                ////this.WriteInterfaceAccess(writer, opCode, fromType, toType);
            }
            else
            {
                llvmWriter.WriteSetResultNumber(writer, opCode);
                writer.Write("bitcast ");
                fromType.WriteTypePrefix(writer, true);
                writer.Write(' ');
                writer.Write(custromName);
                writer.Write(" to ");
                toType.WriteTypePrefix(writer, true);
                if (appendReference)
                {
                    // result should be array
                    writer.Write('*');
                }
            }

            opCode.ResultType = toType;
            writer.WriteLine(string.Empty);
        }
    }
}
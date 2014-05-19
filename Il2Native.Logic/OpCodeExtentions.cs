namespace Il2Native.Logic
{
    using System;
    using System.Linq;
    using System.Reflection.Emit;

    using Il2Native.Logic.CodeParts;

    /// <summary>
    /// </summary>
    public static class OpCodeExtentions
    {
        #region Public Methods and Operators

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="codes">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool Any(this OpCodePart opCode, params Code[] codes)
        {
            var code = opCode.ToCode();
            foreach (var item in codes)
            {
                if (item == code)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsAnyBranch(this OpCodePart opCodePart)
        {
            return (opCodePart.OpCode.FlowControl == FlowControl.Cond_Branch || opCodePart.OpCode.FlowControl == FlowControl.Branch)
                   && opCodePart.ToCode() != Code.Leave && opCodePart.ToCode() != Code.Leave_S;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsBranch(this OpCodePart opCodePart)
        {
            return opCodePart.OpCode.FlowControl == FlowControl.Branch && opCodePart.ToCode() != Code.Switch && opCodePart.ToCode() != Code.Leave
                   && opCodePart.ToCode() != Code.Leave_S;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsCondBranch(this OpCodePart opCodePart)
        {
            return opCodePart.OpCode.FlowControl == FlowControl.Cond_Branch && opCodePart.ToCode() != Code.Switch && opCodePart.ToCode() != Code.Leave
                   && opCodePart.ToCode() != Code.Leave_S;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsDouble(this Type type)
        {
            return type != null && type.Name == "Double" && type.Namespace == "System";
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsFloat(this Type type)
        {
            return type != null && type.Name == "Float" && type.Namespace == "System";
        }

        public static bool IsSingle(this Type type)
        {
            return type != null && type.Name == "Single" && type.Namespace == "System";
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsJumpForward(this OpCodePart opCodePart)
        {
            var opCode = opCodePart as OpCodeInt32Part;

            if (opCode.OpCode.OperandType == OperandType.ShortInlineBrTarget && (opCode.Operand & 0x80) == 0x80)
            {
                return false;
            }

            return opCode.Operand > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsPrimitiveType(this Type type)
        {
            return type != null && type.IsValueType && type.IsPrimitive;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsReal(this Type type)
        {
            return type.IsFloat() || type.IsDouble() || type.IsSingle();
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsReturn(this OpCodePart opCodePart)
        {
            return opCodePart.OpCode.FlowControl == FlowControl.Return && opCodePart.ToCode() != Code.Endfinally;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsStructureType(this Type type, bool recurse = false)
        {
            return type != null && (type.IsValueType && !type.IsEnum && !type.IsPrimitive && !type.IsVoid() || recurse && type.HasElementType && type.GetElementType().IsStructureType(recurse));
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsValueType(this Type type)
        {
            return type != null && type.IsValueType;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static bool IsVoid(this Type type)
        {
            return type != null && type.Name == "Void" && type.Namespace == "System";
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodePart">
        /// </param>
        /// <returns>
        /// </returns>
        public static int JumpAddress(this OpCodePart opCodePart)
        {
            var opCode = opCodePart as OpCodeInt32Part;

            if (opCode.OpCode.OperandType == OperandType.ShortInlineBrTarget && (opCode.Operand & 0x80) == 0x80)
            {
                return opCode.AddressEnd - (255 - (short)opCode.Operand + 1);
            }

            return opCode.Operand + opCode.AddressEnd;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCodeLabelsPart">
        /// </param>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        public static int JumpAddress(this OpCodeLabelsPart opCodeLabelsPart, int index)
        {
            return opCodeLabelsPart.Operand[index] + opCodeLabelsPart.AddressEnd;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="baseWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static OpCodePart NextOpCodeGroup(this OpCodePart opCode, BaseWriter baseWriter)
        {
            OpCodePart ret = null;
            baseWriter.OpsByGroupAddressStart.TryGetValue(opCode.GroupAddressEnd, out ret);
            return ret;
        }

        public static OpCodePart NextOpCode(this OpCodePart opCode, BaseWriter baseWriter)
        {
            OpCodePart ret = null;
            baseWriter.OpsByAddressStart.TryGetValue(opCode.AddressEnd, out ret);
            return ret;
        }

        public static OpCodePart JumpOpCodeGroup(this OpCodePart opCode, BaseWriter baseWriter)
        {
            var jumpAddress = opCode.JumpAddress();
            OpCodePart stopForBranch;
            if (baseWriter.OpsByGroupAddressStart.TryGetValue(jumpAddress, out stopForBranch))
            {
                return stopForBranch;
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <param name="baseWriter">
        /// </param>
        /// <returns>
        /// </returns>
        public static OpCodePart PreviousOpCodeGroup(this OpCodePart opCode, BaseWriter baseWriter)
        {
            OpCodePart ret = null;
            baseWriter.OpsByGroupAddressEnd.TryGetValue(opCode.GroupAddressStart, out ret);
            return ret;
        }

        public static OpCodePart PreviousOpCode(this OpCodePart opCode, BaseWriter baseWriter)
        {
            OpCodePart ret = null;
            baseWriter.OpsByAddressEnd.TryGetValue(opCode.AddressStart, out ret);
            return ret;
        }

        /// <summary>
        /// </summary>
        /// <param name="opCode">
        /// </param>
        /// <returns>
        /// </returns>
        public static Code ToCode(this OpCodePart opCode)
        {
            var val = opCode.OpCode.Value;
            if (val < 0xE1 && val >= 0)
            {
                return (Code)val;
            }

            return (Code)(val - (val >> 8 << 8) + 0xE1);
        }

        public static bool HasAnyVirtualMethod(this Type thisType)
        {
            if (IlReader.Methods(thisType).Any(m => m.IsVirtual || m.IsAbstract))
            {
                return true;
            }

            return thisType.BaseType != null && thisType.BaseType.HasAnyVirtualMethod();
        }

        #endregion
    }
}
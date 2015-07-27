// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CallGen.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using CodeParts;

    using Il2Native.Logic.Exceptions;

    using PEAssemblyReader;
    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    /// <summary>
    /// </summary>
    public static class CallGen
    {
        /// <summary>
        /// </summary>
        /// <param name="cWriter">
        /// </param>
        /// <param name="opCodeMethodInfo">
        /// </param>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="tryClause">
        /// </param>
        public static void WriteCall(this CWriter cWriter, OpCodePart opCodeMethodInfo, IMethod methodInfo, TryClause tryClause)
        {
            if (cWriter.ProcessPluggableMethodCall(opCodeMethodInfo, methodInfo))
            {
                return;
            }

            cWriter.WriteFunctionNameExpression(methodInfo);
            cWriter.WriteFunctionCallArguments(opCodeMethodInfo);
        }

        public static void WriteCallVirtual(this CWriter cWriter, OpCodePart opCodeMethodInfo, IMethod methodInfo, TryClause tryClause)
        {
            if (cWriter.ProcessPluggableMethodCall(opCodeMethodInfo, methodInfo))
            {
                return;
            }

            // split in 2 (vtable call and vtable-interface call
            if (methodInfo.DeclaringType.IsInterface)
            {
                cWriter.WriteCallInterface(opCodeMethodInfo, methodInfo, tryClause);
            }

            var writer = cWriter.Output;

            // virtual call
            writer.Write("(");
            cWriter.WriteCCastOnly(methodInfo.DeclaringType.ToVirtualTable());
            cWriter.WriteFieldAccess(opCodeMethodInfo, cWriter.System.System_Object.GetFieldByName(CWriter.VTable, cWriter));
            writer.Write(")->");
            cWriter.WriteFunctionNameExpression(methodInfo);
            cWriter.WriteFunctionCallArguments(opCodeMethodInfo);
        }

        private static void WriteCallInterface(this CWriter cWriter, OpCodePart opCodeMethodInfo, IMethod methodInfo, TryClause tryClause)
        {
        }

        /// <summary>
        /// </summary>
        public static void WriteFunctionCallArguments(
            this CWriter cWriter,
            OpCodePart opCodeMethodInfo)
        {
            var writer = cWriter.Output;

            writer.Write("(");

            var opCodeOperands = opCodeMethodInfo.OpCodeOperands;
            if (opCodeOperands != null)
            {
                // add parameters
                var comma = false;
                foreach (var usedItem in opCodeOperands)
                {
                    if (comma)
                    {
                        writer.Write(", ");
                    }
                    else
                    {
                        comma = true;
                    }

                    cWriter.WriteResultOrActualWrite(usedItem);
                }
            }

            writer.Write(")");
        }

        public static IType GetOwnerOfExplicitInterface(IType methodDeclaringType, IType argument0)
        {
            return methodDeclaringType.IsInterface && methodDeclaringType.TypeNotEquals(argument0) ? argument0 : null;
        }

        /// <summary>
        /// </summary>
        /// <param name="methodInfo">
        /// </param>
        /// <param name="methodAddressResultNumber">
        /// </param>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <param name="cWriter">
        /// </param>
        public static void WriteFunctionNameExpression(
            this CWriter cWriter,
            IMethod methodInfo)
        {
            var writer = cWriter.Output;

            if (methodInfo.IsUnmanaged)
            {
                // just name (for example calloc)
                cWriter.Output.Write(methodInfo.Name);
            }
            else
            {
                // default method name
                cWriter.WriteMethodDefinitionName(writer, methodInfo);
            }
        }
    }
}
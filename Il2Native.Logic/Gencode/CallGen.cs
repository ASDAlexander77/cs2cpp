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
        public static void WriteCall(this CWriter cWriter, OpCodePart opCodeMethodInfo, IMethod methodInfo, TryClause tryClause, bool callVirtual = false, bool excludeArguments = false)
        {
            if (cWriter.ProcessPluggableMethodCall(opCodeMethodInfo, methodInfo))
            {
                return;
            }

            //  split in 2 - direct call and virtual call
            if (methodInfo.IsMethodVirtual() && callVirtual)
            {
                cWriter.WriteCallVirtual(opCodeMethodInfo, methodInfo, tryClause, excludeArguments);
                return;
            }

            cWriter.WriteFunctionNameExpression(methodInfo);

            if (excludeArguments)
            {
                return;
            }

            cWriter.WriteFunctionCallArguments(opCodeMethodInfo);
        }

        private static void WriteCallVirtual(this CWriter cWriter, OpCodePart opCodeMethodInfo, IMethod methodInfo, TryClause tryClause, bool excludeArguments = false)
        {
            // split in 2 - vtable call and vtable-interface call
            if (methodInfo.DeclaringType.IsInterface)
            {
                cWriter.WriteCallInterface(opCodeMethodInfo, methodInfo, tryClause, excludeArguments);
                return;
            }

            var writer = cWriter.Output;

            // virtual call
            writer.Write("(");
            cWriter.WriteCCastOnly(methodInfo.DeclaringType.ToClass().ToVirtualTable());
            cWriter.WriteFieldAccess(opCodeMethodInfo, cWriter.System.System_Object.GetFieldByName(CWriter.VTable, cWriter));
            writer.Write(")");
            writer.Write("->");
            cWriter.WriteFunctionNameExpression(methodInfo, true);

            if (excludeArguments)
            {
                return;
            }

            cWriter.WriteFunctionCallArguments(opCodeMethodInfo);
        }

        private static void WriteCallInterface(this CWriter cWriter, OpCodePart opCodeMethodInfo, IMethod methodInfo, TryClause tryClause, bool excludeArguments = false)
        {
            Debug.Assert(methodInfo.DeclaringType.IsInterface, "Method should belong to an interface");

            // split in 2 (interface call when 'this' is object and when 'this' is interface
            var thisOperand = opCodeMethodInfo.OpCodeOperands[0];
            var estimatedResultOf = cWriter.EstimatedResultOf(thisOperand);
            if (estimatedResultOf.Type.IsInterface || estimatedResultOf.Type.IsVoidPointer())
            {
                cWriter.WriteCallInterfaceForInterface(opCodeMethodInfo, methodInfo, tryClause, excludeArguments);
                return;
            }

            if (estimatedResultOf.Type.IsPointer && estimatedResultOf.Type.GetElementType().IsValueType())
            {
                cWriter.WriteCallInterfaceForValueType(opCodeMethodInfo, methodInfo, tryClause, excludeArguments);
                return;
            }


            var writer = cWriter.Output;

            writer.Write("(");
            cWriter.WriteCCastOnly(estimatedResultOf.Type.ToClass().ToVirtualTable());
            cWriter.WriteFieldAccess(opCodeMethodInfo, cWriter.System.System_Object.GetFieldByName(CWriter.VTable, cWriter));
            writer.Write(")");
            cWriter.WriteInterfaceAccessRightSide(methodInfo.DeclaringType, estimatedResultOf.Type, true);
            cWriter.WriteFunctionNameExpression(methodInfo, true);

            if (excludeArguments)
            {
                return;
            }

            cWriter.WriteFunctionCallArguments(opCodeMethodInfo, methodInfo.DeclaringType);
        }

        private static void WriteCallInterfaceForInterface(this CWriter cWriter, OpCodePart opCodeMethodInfo, IMethod methodInfo, TryClause tryClause, bool excludeArguments = false)
        {
            Debug.Assert(methodInfo.DeclaringType.IsInterface, "Method should belong to an interface");

            // split in 2 (interface call when 'this' is object and when 'this' is interface
            var thisOperand = opCodeMethodInfo.OpCodeOperands[0];
            var estimatedResultOf = cWriter.EstimatedResultOf(thisOperand);
            var isVoidPointer = estimatedResultOf.Type.IsVoidPointer();
            Debug.Assert(estimatedResultOf.Type.IsInterface || isVoidPointer, "Interface needed");

            cWriter.WriteInterfaceAccess(thisOperand, !isVoidPointer ? estimatedResultOf.Type : methodInfo.DeclaringType, methodInfo.DeclaringType, allowLastAccess: true);
            cWriter.WriteFunctionNameExpression(methodInfo, true);

            if (excludeArguments)
            {
                return;
            }

            cWriter.WriteFunctionCallArguments(opCodeMethodInfo, methodInfo.DeclaringType, interfaceThisAccess: true);
        }

        private static void WriteCallInterfaceForValueType(this CWriter cWriter, OpCodePart opCodeMethodInfo, IMethod methodInfo, TryClause tryClause, bool excludeArguments = false)
        {
            Debug.Assert(methodInfo.DeclaringType.IsInterface, "Method should belong to an interface");

            // split in 2 (interface call when 'this' is object and when 'this' is interface
            var thisOperand = opCodeMethodInfo.OpCodeOperands[0];

            var estimatedResultOf = cWriter.EstimatedResultOf(thisOperand);

            Debug.Assert(estimatedResultOf.Type.IsPointer, "Pointer type is needed");

            var bareType = estimatedResultOf.Type.GetElementType();

            var actualMethod = bareType.GetCorrespondingMethodForInterface(methodInfo);
            // function name
            cWriter.WriteFunctionNameExpression(actualMethod);
            if (excludeArguments)
            {
                return;
            }

            cWriter.WriteFunctionCallArguments(opCodeMethodInfo);
        }

        /// <summary>
        /// </summary>
        public static void WriteFunctionCallArguments(
            this CWriter cWriter,
            OpCodePart opCodeMethodInfo,
            IType castThisTo = null,
            bool interfaceThisAccess = false)
        {
            var writer = cWriter.Output;

            writer.Write("(");

            var opCodeOperands = opCodeMethodInfo.OpCodeOperands;
            if (opCodeOperands != null)
            {
                // add parameters
                var first = true;
                foreach (var usedItem in opCodeOperands)
                {
                    if (!first)
                    {
                        writer.Write(", ");
                    }
                    else if (castThisTo != null)
                    {
                        cWriter.WriteCCastOnly(castThisTo);
                    }

                    if (first && interfaceThisAccess)
                    {
                        writer.Write("__this_from_interface(");
                    }

                    // operand write
                    cWriter.WriteResultOrActualWrite(usedItem);

                    if (first && interfaceThisAccess)
                    {
                        writer.Write(")");
                    }

                    first = false;
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
            IMethod methodInfo,
            bool noGenericSuffix = false)
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
                if (noGenericSuffix)
                {
                    cWriter.WriteMethodDefinitionNameNoGenericSuffix(writer, methodInfo);
                }
                else
                {
                    cWriter.WriteMethodDefinitionName(writer, methodInfo);
                }
            }
        }
    }
}
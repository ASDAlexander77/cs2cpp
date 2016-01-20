namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeGen;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.CodeGen;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.Text;
    using Roslyn.Utilities;

    public class CCodeMethodSerializer
    {
        private IndentedTextWriter itw;

        private enum CallKind
        {
            Call,
            CallVirt,
            ConstrainedCallVirt,
        }

        public CCodeMethodSerializer(IndentedTextWriter itw)
        {
            this.itw = itw;
        }

        internal void Serialize(BoundStatement boundBody)
        {
            this.EmitStatement(boundBody);
        }

        private void EmitStatement(BoundStatement statement)
        {
            switch (statement.Kind)
            {
                case BoundKind.Block:
                    EmitBlock((BoundBlock)statement);
                    break;

                case BoundKind.SequencePoint:
                    this.EmitSequencePointStatement((BoundSequencePoint)statement);
                    break;

                case BoundKind.SequencePointWithSpan:
                    this.EmitSequencePointStatement((BoundSequencePointWithSpan)statement);
                    break;

                case BoundKind.ExpressionStatement:
                    EmitExpression(((BoundExpressionStatement)statement).Expression, false);
                    break;

                case BoundKind.StatementList:
                    EmitStatementList((BoundStatementList)statement);
                    break;

                case BoundKind.ReturnStatement:
                    EmitReturnStatement((BoundReturnStatement)statement);
                    break;

                case BoundKind.GotoStatement:
                    ////EmitGotoStatement((BoundGotoStatement)statement);
                    break;

                case BoundKind.LabelStatement:
                    ////EmitLabelStatement((BoundLabelStatement)statement);
                    break;

                case BoundKind.ConditionalGoto:
                    ////EmitConditionalGoto((BoundConditionalGoto)statement);
                    break;

                case BoundKind.ThrowStatement:
                    ////EmitThrowStatement((BoundThrowStatement)statement);
                    break;

                case BoundKind.TryStatement:
                    ////EmitTryStatement((BoundTryStatement)statement);
                    break;

                case BoundKind.SwitchStatement:
                    ////EmitSwitchStatement((BoundSwitchStatement)statement);
                    break;

                case BoundKind.IteratorScope:
                    ////EmitIteratorScope((BoundIteratorScope)statement);
                    break;

                case BoundKind.NoOpStatement:
                    ////EmitNoOpStatement((BoundNoOpStatement)statement);
                    break;

                default:
                    // Code gen should not be invoked if there are errors.
                    throw ExceptionUtilities.UnexpectedValue(statement.Kind);
            }
        }

        private void EmitReturnStatement(BoundReturnStatement boundReturnStatement)
        {
            this.itw.Write("return");
            if (boundReturnStatement.ExpressionOpt != null)
            {
                this.itw.Write(" ");
            }

            // TODO: investigate about indirect return
            this.EmitExpression(boundReturnStatement.ExpressionOpt, true);

            this.itw.WriteLine(";");
        }

        private void EmitStatementList(BoundStatementList list)
        {
            for (int i = 0, n = list.Statements.Length; i < n; i++)
            {
                EmitStatement(list.Statements[i]);
            }
        }

        private void EmitBlock(BoundBlock block)
        {
            var hasLocals = !block.Locals.IsEmpty;

            if (hasLocals)
            {
                foreach (var local in block.Locals)
                {
                    var declaringReferences = local.DeclaringSyntaxReferences;
                    ////DefineLocal(local, !declaringReferences.IsEmpty ? (CSharpSyntaxNode)declaringReferences[0].GetSyntax() : block.Syntax);
                }
            }

            foreach (var statement in block.Statements)
            {
                EmitStatement(statement);
            }
        }

        private void EmitSequencePointStatement(BoundSequencePoint node)
        {
            BoundStatement statement = node.StatementOpt;
            int instructionsEmitted = 0;

            if (statement != null)
            {
                this.EmitStatement(statement);
            }
        }

        private void EmitSequencePointStatement(BoundSequencePointWithSpan node)
        {
            BoundStatement statement = node.StatementOpt;
            int instructionsEmitted = 0;

            if (statement != null)
            {
                this.EmitStatement(statement);
            }
        }

        private void EmitConstantExpression(TypeSymbol type, ConstantValue constantValue, bool used, CSharpSyntaxNode syntaxNode)
        {
            if (used)  // unused constant has no sideeffects
            {
                // Null type parameter values must be emitted as 'initobj' rather than 'ldnull'.
                if (((object)type != null) && (type.TypeKind == TypeKind.TypeParameter) && constantValue.IsNull)
                {
                    ////EmitInitObj(type, used, syntaxNode);
                }
                else
                {
                    EmitConstantValue(constantValue);
                }
            }
        }

        internal void EmitConstantValue(ConstantValue value)
        {
            ConstantValueTypeDiscriminator discriminator = value.Discriminator;

            switch (discriminator)
            {
                case ConstantValueTypeDiscriminator.Null:
                    itw.Write("nullptr");
                    break;
                case ConstantValueTypeDiscriminator.SByte:
                    itw.Write(value.SByteValue);
                    break;
                case ConstantValueTypeDiscriminator.Byte:
                    itw.Write(value.ByteValue);
                    break;
                case ConstantValueTypeDiscriminator.UInt16:
                    itw.Write(value.UInt16Value);
                    break;
                case ConstantValueTypeDiscriminator.Char:
                    itw.Write("L'{0}'", value.CharValue);
                    break;
                case ConstantValueTypeDiscriminator.Int16:
                    itw.Write(value.Int16Value);
                    break;
                case ConstantValueTypeDiscriminator.Int32:
                case ConstantValueTypeDiscriminator.UInt32:
                    itw.Write(value.Int32Value);
                    break;
                case ConstantValueTypeDiscriminator.Int64:
                    itw.Write(value.Int64Value);
                    itw.Write("L");
                    break;
                case ConstantValueTypeDiscriminator.UInt64:
                    itw.Write(value.Int64Value);
                    itw.Write("UL");
                    break;
                case ConstantValueTypeDiscriminator.Single:
                    itw.Write(value.SingleValue);
                    break;
                case ConstantValueTypeDiscriminator.Double:
                    itw.Write(value.DoubleValue);
                    break;
                case ConstantValueTypeDiscriminator.String:
                    itw.Write("L\"{0}\"", value.StringValue);
                    break;
                case ConstantValueTypeDiscriminator.Boolean:
                    itw.Write(value.BooleanValue.ToString().ToLowerInvariant());
                    break;
                default:
                    throw ExceptionUtilities.UnexpectedValue(discriminator);
            }
        }

        private void EmitExpression(BoundExpression expression, bool used)
        {
            if (expression == null)
            {
                return;
            }

            var constantValue = expression.ConstantValue;
            if (constantValue != null)
            {
                if (!used)
                {
                    // unused constants have no sideeffects.
                    return;
                }

                if ((object)expression.Type == null || expression.Type.SpecialType != SpecialType.System_Decimal)
                {
                    EmitConstantExpression(expression.Type, constantValue, used, expression.Syntax);
                    return;
                }
            }

            switch (expression.Kind)
            {
                case BoundKind.AssignmentOperator:
                    ////EmitAssignmentExpression((BoundAssignmentOperator)expression, used);
                    break;

                case BoundKind.Call:
                    EmitCallExpression((BoundCall)expression, used);
                    break;

                case BoundKind.ObjectCreationExpression:
                    ////EmitObjectCreationExpression((BoundObjectCreationExpression)expression, used);
                    break;

                case BoundKind.DelegateCreationExpression:
                    ////EmitDelegateCreationExpression((BoundDelegateCreationExpression)expression, used);
                    break;

                case BoundKind.ArrayCreation:
                    ////EmitArrayCreationExpression((BoundArrayCreation)expression, used);
                    break;

                case BoundKind.StackAllocArrayCreation:
                    ////EmitStackAllocArrayCreationExpression((BoundStackAllocArrayCreation)expression, used);
                    break;

                case BoundKind.Conversion:
                    ////EmitConversionExpression((BoundConversion)expression, used);
                    break;

                case BoundKind.Local:
                    ////EmitLocalLoad((BoundLocal)expression, used);
                    break;

                case BoundKind.Dup:
                    ////EmitDupExpression((BoundDup)expression, used);
                    break;

                case BoundKind.Parameter:
                    if (used)  // unused parameter has no sideeffects
                    {
                        ////EmitParameterLoad((BoundParameter)expression);
                    }
                    break;

                case BoundKind.FieldAccess:
                    ////EmitFieldLoad((BoundFieldAccess)expression, used);
                    break;

                case BoundKind.ArrayAccess:
                    ////EmitArrayElementLoad((BoundArrayAccess)expression, used);
                    break;

                case BoundKind.ArrayLength:
                    ////EmitArrayLength((BoundArrayLength)expression, used);
                    break;

                case BoundKind.ThisReference:
                    if (used) // unused this has no sideeffects
                    {
                        ////EmitThisReferenceExpression((BoundThisReference)expression);
                    }

                    break;

                case BoundKind.PreviousSubmissionReference:
                    // Script references are lowered to a this reference and a field access.
                    throw ExceptionUtilities.UnexpectedValue(expression.Kind);

                case BoundKind.BaseReference:
                    if (used) // unused base has no sideeffects
                    {
                        ////var thisType = this.method.ContainingType;
                        ////builder.EmitOpCode(ILOpCode.Ldarg_0);
                        ////if (thisType.IsValueType)
                        ////{
                        ////    EmitLoadIndirect(thisType, expression.Syntax);
                        ////    EmitBox(thisType, expression.Syntax);
                        ////}
                    }
                    break;

                case BoundKind.Sequence:
                    /////EmitSequenceExpression((BoundSequence)expression, used);
                    break;

                case BoundKind.SequencePointExpression:
                    ////EmitSequencePointExpression((BoundSequencePointExpression)expression, used);
                    break;

                case BoundKind.UnaryOperator:
                    ////EmitUnaryOperatorExpression((BoundUnaryOperator)expression, used);
                    break;

                case BoundKind.BinaryOperator:
                    ////EmitBinaryOperatorExpression((BoundBinaryOperator)expression, used);
                    break;

                case BoundKind.NullCoalescingOperator:
                    ////EmitNullCoalescingOperator((BoundNullCoalescingOperator)expression, used);
                    break;

                case BoundKind.IsOperator:
                    ////EmitIsExpression((BoundIsOperator)expression, used);
                    break;

                case BoundKind.AsOperator:
                    ////EmitAsExpression((BoundAsOperator)expression, used);
                    break;

                case BoundKind.DefaultOperator:
                    ////EmitDefaultExpression((BoundDefaultOperator)expression, used);
                    break;

                case BoundKind.TypeOfOperator:
                    if (used) // unused typeof has no sideeffects
                    {
                        ////EmitTypeOfExpression((BoundTypeOfOperator)expression);
                    }
                    break;

                case BoundKind.SizeOfOperator:
                    if (used) // unused sizeof has no sideeffects
                    {
                        ////EmitSizeOfExpression((BoundSizeOfOperator)expression);
                    }
                    break;

                case BoundKind.MethodInfo:
                    if (used)
                    {
                        ////EmitMethodInfoExpression((BoundMethodInfo)expression);
                    }
                    break;

                case BoundKind.FieldInfo:
                    if (used)
                    {
                        ////EmitFieldInfoExpression((BoundFieldInfo)expression);
                    }
                    break;

                case BoundKind.ConditionalOperator:
                    ////EmitConditionalOperator((BoundConditionalOperator)expression, used);
                    break;

                case BoundKind.AddressOfOperator:
                    ////EmitAddressOfExpression((BoundAddressOfOperator)expression, used);
                    break;

                case BoundKind.PointerIndirectionOperator:
                    ////EmitPointerIndirectionOperator((BoundPointerIndirectionOperator)expression, used);
                    break;

                case BoundKind.ArgList:
                    ////EmitArgList(used);
                    break;

                case BoundKind.ArgListOperator:
                    Debug.Assert(used);
                    ////EmitArgListOperator((BoundArgListOperator)expression);
                    break;

                case BoundKind.RefTypeOperator:
                    ////EmitRefTypeOperator((BoundRefTypeOperator)expression, used);
                    break;

                case BoundKind.MakeRefOperator:
                    ////EmitMakeRefOperator((BoundMakeRefOperator)expression, used);
                    break;

                case BoundKind.RefValueOperator:
                    ////EmitRefValueOperator((BoundRefValueOperator)expression, used);
                    break;

                case BoundKind.ConditionalAccess:
                    ////EmitConditionalAccessExpression((BoundConditionalAccess)expression, used);
                    break;

                case BoundKind.ConditionalReceiver:
                    ////EmitConditionalReceiver((BoundConditionalReceiver)expression, used);
                    break;

                default:
                    // Code gen should not be invoked if there are errors.
                    Debug.Assert(expression.Kind != BoundKind.BadExpression);

                    // node should have been lowered:
                    throw ExceptionUtilities.UnexpectedValue(expression.Kind);
            }
        }

        private void EmitCallExpression(BoundCall call, bool used)
        {
            var method = call.Method;
            var receiver = call.ReceiverOpt;
            LocalDefinition tempOpt = null;

            // Calls to the default struct constructor are emitted as initobj, rather than call.
            // NOTE: constructor invocations are usually represented as BoundObjectCreationExpressions,
            // rather than BoundCalls.  This is why we can be confident that if we see a call to a
            // constructor, it has this very specific form.
            if (method.IsParameterlessValueTypeConstructor())
            {
                Debug.Assert(method.IsImplicitlyDeclared);
                Debug.Assert(method.ContainingType == receiver.Type);
                Debug.Assert(receiver.Kind == BoundKind.ThisReference);

                //tempOpt = EmitReceiverRef(receiver);
                //builder.EmitOpCode(ILOpCode.Initobj);    //  initobj  <MyStruct>
                //EmitSymbolToken(method.ContainingType, call.Syntax);

                return;
            }

            var arguments = call.Arguments;

            CallKind callKind = CallKind.Call;

            if (method.IsStatic)
            {
                callKind = CallKind.Call;
            }
            else
            {
                var receiverType = receiver.Type;

                if (receiverType.IsVerifierReference())
                {
                    ////tempOpt = EmitReceiverRef(receiver, isAccessConstrained: false);

                    ////// In some cases CanUseCallOnRefTypeReceiver returns true which means that 
                    ////// null check is unnecessary and we can use "call"
                    ////if (receiver.SuppressVirtualCalls ||
                    ////    (!method.IsMetadataVirtual() && CanUseCallOnRefTypeReceiver(receiver)))
                    ////{
                    ////    callKind = CallKind.Call;
                    ////}
                    ////else
                    ////{
                    ////    callKind = CallKind.CallVirt;
                    ////}
                }
                else if (receiverType.IsVerifierValue())
                {
                    NamedTypeSymbol methodContainingType = method.ContainingType;
                    ////if (methodContainingType.IsVerifierValue() && MayUseCallForStructMethod(method))
                    ////{
                    ////    // NOTE: this should be either a method which overrides some abstract method or 
                    ////    //       does not override anything (with few exceptions, see MayUseCallForStructMethod); 
                    ////    //       otherwise we should not use direct 'call' and must use constrained call;

                    ////    // calling a method defined in a value type
                    ////    Debug.Assert(receiverType == methodContainingType);
                    ////    tempOpt = EmitReceiverRef(receiver);
                    ////    callKind = CallKind.Call;
                    ////}
                    ////else
                    ////{
                    ////    if (method.IsMetadataVirtual())
                    ////    {
                    ////        // When calling a method that is virtual in metadata on a struct receiver, 
                    ////        // we use a constrained virtual call. If possible, it will skip boxing.
                    ////        tempOpt = EmitReceiverRef(receiver, isAccessConstrained: true);
                    ////        callKind = CallKind.ConstrainedCallVirt;
                    ////    }
                    ////    else
                    ////    {
                    ////        // calling a method defined in a base class.
                    ////        EmitExpression(receiver, used: true);
                    ////        EmitBox(receiverType, receiver.Syntax);
                    ////        callKind = CallKind.Call;
                    ////    }
                    ////}
                }
                else
                {
                    // receiver is generic and method must come from the base or an interface or a generic constraint
                    // if the receiver is actually a value type it would need to be boxed.
                    // let .constrained sort this out. 
                    callKind = receiverType.IsReferenceType ?
                                CallKind.CallVirt :
                                CallKind.ConstrainedCallVirt;

                    ////tempOpt = EmitReceiverRef(receiver, isAccessConstrained: callKind == CallKind.ConstrainedCallVirt);
                }
            }

            // When emitting a callvirt to a virtual method we always emit the method info of the
            // method that first declared the virtual method, not the method info of an
            // overriding method. It would be a subtle breaking change to change that rule;
            // see bug 6156 for details.

            MethodSymbol actualMethodTargetedByTheCall = method;
            if (method.IsOverride && callKind != CallKind.Call)
            {
                ////actualMethodTargetedByTheCall = method.GetConstructedLeastOverriddenMethod(this.method.ContainingType);
            }

            if (callKind == CallKind.ConstrainedCallVirt && actualMethodTargetedByTheCall.ContainingType.IsValueType)
            {
                // special case for overriden methods like ToString(...) called on
                // value types: if the original method used in emit cannot use callvirt in this
                // case, change it to Call.
                callKind = CallKind.Call;
            }

            /*
            // Devirtualizing of calls to effectively sealed methods.
            if (callKind == CallKind.CallVirt)
            {
                // NOTE: we check that we call method in same module just to be sure
                // that it cannot be recompiled as not final and make our call not verfiable. 
                // such change by adversarial user would arguably be a compat break, but better be safe...
                // In reality we would typically have one method calling another method in the same class (one GetEnumerator calling another).
                // Other scenarios are uncommon since base class cannot be sealed and 
                // referring to a derived type in a different module is not an easy thing to do.
                if (IsThisReceiver(receiver) && actualMethodTargetedByTheCall.ContainingType.IsSealed &&
                        (object)actualMethodTargetedByTheCall.ContainingModule == (object)this.method.ContainingModule)
                {
                    // special case for target is in a sealed class and "this" receiver.
                    Debug.Assert(receiver.Type.IsVerifierReference());
                    callKind = CallKind.Call;
                }

                // NOTE: we do not check that we call method in same module.
                // Because of the "GetOriginalConstructedOverriddenMethod" above, the actual target
                // can only be final when it is "newslot virtual final".
                // In such case Dev11 emits "call" and we will just replicate the behavior. (see DevDiv: 546853 )
                else if (actualMethodTargetedByTheCall.IsMetadataFinal() && CanUseCallOnRefTypeReceiver(receiver))
                {
                    // special case for calling 'final' virtual method on reference receiver
                    Debug.Assert(receiver.Type.IsVerifierReference());
                    callKind = CallKind.Call;
                }
            }
            */

            ////EmitArguments(arguments, method.Parameters);
            ////int stackBehavior = GetCallStackBehavior(call);
            ////switch (callKind)
            ////{
            ////    case CallKind.Call:
            ////        builder.EmitOpCode(ILOpCode.Call, stackBehavior);
            ////        break;

            ////    case CallKind.CallVirt:
            ////        builder.EmitOpCode(ILOpCode.Callvirt, stackBehavior);
            ////        break;

            ////    case CallKind.ConstrainedCallVirt:
            ////        builder.EmitOpCode(ILOpCode.Constrained);
            ////        EmitSymbolToken(receiver.Type, receiver.Syntax);
            ////        builder.EmitOpCode(ILOpCode.Callvirt, stackBehavior);
            ////        break;
            ////}

            ////EmitSymbolToken(actualMethodTargetedByTheCall, call.Syntax,
            ////                actualMethodTargetedByTheCall.IsVararg ? (BoundArgListOperator)call.Arguments[call.Arguments.Length - 1] : null);
        }
    }
}

namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CodeGen;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.CodeGen;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.Text;
    using Roslyn.Utilities;

    public class CCodeMethodSerializer
    {
        private readonly CCodeWriter c;

        public CCodeMethodSerializer(CCodeWriter c)
        {
            this.c = c;
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
            this.c.Write("return");
            if (boundReturnStatement.ExpressionOpt != null)
            {
                this.c.Write(" ");
            }

            // TODO: investigate about indirect return
            this.EmitExpression(boundReturnStatement.ExpressionOpt, true);
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
            this.c.OpenBlock();

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
                this.c.WriteLine(";");
            }

            this.c.EndBlock();
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
                    c.Write("nullptr");
                    break;
                case ConstantValueTypeDiscriminator.SByte:
                    c.Write(value.SByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Byte:
                    c.Write(value.ByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.UInt16:
                    c.Write(value.UInt16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Char:
                    c.Write(string.Format("L'{0}'", value.CharValue));
                    break;
                case ConstantValueTypeDiscriminator.Int16:
                    c.Write(value.Int16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Int32:
                case ConstantValueTypeDiscriminator.UInt32:
                    c.Write(value.Int32Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Int64:
                    c.Write(value.Int64Value.ToString());
                    c.Write("L");
                    break;
                case ConstantValueTypeDiscriminator.UInt64:
                    c.Write(value.Int64Value.ToString());
                    c.Write("UL");
                    break;
                case ConstantValueTypeDiscriminator.Single:
                    c.Write(value.SingleValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Double:
                    c.Write(value.DoubleValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.String:
                    c.Write(string.Format("L\"{0}\"", value.StringValue));
                    break;
                case ConstantValueTypeDiscriminator.Boolean:
                    c.Write(value.BooleanValue.ToString().ToLowerInvariant());
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

            c.WriteMethodFullName(method);
            this.c.Write("(");
            var anyArgs = false;
            foreach (var boundExpression in call.Arguments)
            {
                if (anyArgs)
                {
                    this.c.Write(", ");
                }

                EmitExpression(boundExpression, false);

                anyArgs = true;
            }

            this.c.Write(")");
        }

        /// <summary>
        /// checks if receiver is effectively ldarg.0
        /// </summary>
        private bool IsThisReceiver(BoundExpression receiver)
        {
            switch (receiver.Kind)
            {
                case BoundKind.ThisReference:
                    return true;

                case BoundKind.Sequence:
                    var seqValue = ((BoundSequence)(receiver)).Value;
                    return IsThisReceiver(seqValue);
            }

            return false;
        }
    }
}

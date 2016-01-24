namespace Il2Native.Logic
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Roslyn.Utilities;

    public class CCodeMethodSerializer
    {
        private readonly CCodeWriterDOM c;

        public CCodeMethodSerializer(CCodeWriterDOM c)
        {
            this.c = c;
        }

        public IMethodSymbol Method { get; set; }

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
                    EmitExpression(((BoundExpressionStatement)statement).Expression);
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
                    EmitConditionalGoto((BoundConditionalGoto)statement);
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

        private void EmitConditionalGoto(BoundConditionalGoto boundConditionalGoto)
        {
            //object label = boundConditionalGoto.Label;
            //jumpIfTrue = boundConditionalGoto.JumpIfTrue;
            //Debug.Assert(label != null);

            c.TextSpan("if");
            c.WhiteSpace();
            c.TextSpan("(");
            EmitExpression(boundConditionalGoto.Condition);
            c.TextSpan(")");
            c.WhiteSpace();
        }


        private void EmitReturnStatement(BoundReturnStatement boundReturnStatement)
        {
            this.c.TextSpan("return");
            if (boundReturnStatement.ExpressionOpt != null)
            {
                this.c.WhiteSpace();

                // TODO: investigate about indirect return
                this.EmitExpression(boundReturnStatement.ExpressionOpt);
            }
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
                this.c.OpenStatement();
                EmitStatement(statement);
                this.c.EndStatement();
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

        private void EmitConstantExpression(TypeSymbol type, ConstantValue constantValue, CSharpSyntaxNode syntaxNode)
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

        internal void EmitConstantValue(ConstantValue value)
        {
            ConstantValueTypeDiscriminator discriminator = value.Discriminator;

            switch (discriminator)
            {
                case ConstantValueTypeDiscriminator.Null:
                    c.TextSpan("nullptr");
                    break;
                case ConstantValueTypeDiscriminator.SByte:
                    c.TextSpan(value.SByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Byte:
                    c.TextSpan(value.ByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.UInt16:
                    c.TextSpan(value.UInt16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Char:
                    c.TextSpan(string.Format("L'{0}'", value.CharValue));
                    break;
                case ConstantValueTypeDiscriminator.Int16:
                    c.TextSpan(value.Int16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Int32:
                case ConstantValueTypeDiscriminator.UInt32:
                    c.TextSpan(value.Int32Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Int64:
                    c.TextSpan(value.Int64Value.ToString());
                    c.TextSpan("L");
                    break;
                case ConstantValueTypeDiscriminator.UInt64:
                    c.TextSpan(value.Int64Value.ToString());
                    c.TextSpan("UL");
                    break;
                case ConstantValueTypeDiscriminator.Single:
                    c.TextSpan(value.SingleValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Double:
                    c.TextSpan(value.DoubleValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.String:
                    c.TextSpan(string.Format("L\"{0}\"_s", value.StringValue));
                    break;
                case ConstantValueTypeDiscriminator.Boolean:
                    c.TextSpan(value.BooleanValue.ToString().ToLowerInvariant());
                    break;
                default:
                    throw ExceptionUtilities.UnexpectedValue(discriminator);
            }
        }

        private void EmitExpression(BoundExpression expression)
        {
            if (expression == null)
            {
                return;
            }

            var constantValue = expression.ConstantValue;
            if (constantValue != null)
            {
                if ((object)expression.Type == null || expression.Type.SpecialType != SpecialType.System_Decimal)
                {
                    EmitConstantExpression(expression.Type, constantValue, expression.Syntax);
                    return;
                }
            }

            switch (expression.Kind)
            {
                case BoundKind.AssignmentOperator:
                    EmitAssignmentExpression((BoundAssignmentOperator)expression);
                    break;

                case BoundKind.Call:
                    EmitCallExpression((BoundCall)expression);
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
                    EmitConversionExpression((BoundConversion)expression);
                    break;

                case BoundKind.Local:
                    EmitLocalLoad((BoundLocal)expression);
                    break;

                case BoundKind.Dup:
                    ////EmitDupExpression((BoundDup)expression, used);
                    break;

                case BoundKind.Parameter:
                    EmitParameterLoad((BoundParameter)expression);
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
                    ////EmitThisReferenceExpression((BoundThisReference)expression);
                    break;

                case BoundKind.PreviousSubmissionReference:
                    // Script references are lowered to a this reference and a field access.
                    throw ExceptionUtilities.UnexpectedValue(expression.Kind);

                case BoundKind.BaseReference:
                    ////var thisType = this.method.ContainingType;
                    ////builder.EmitOpCode(ILOpCode.Ldarg_0);
                    ////if (thisType.IsValueType)
                    ////{
                    ////    EmitLoadIndirect(thisType, expression.Syntax);
                    ////    EmitBox(thisType, expression.Syntax);
                    ////}
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
                    EmitBinaryOperatorExpression((BoundBinaryOperator)expression);
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
                    ////EmitTypeOfExpression((BoundTypeOfOperator)expression);
                    break;

                case BoundKind.SizeOfOperator:
                    ////EmitSizeOfExpression((BoundSizeOfOperator)expression);
                    break;

                case BoundKind.MethodInfo:
                    ////EmitMethodInfoExpression((BoundMethodInfo)expression);
                    break;

                case BoundKind.FieldInfo:
                    ////EmitFieldInfoExpression((BoundFieldInfo)expression);
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

        private void EmitAssignmentExpression(BoundAssignmentOperator assignmentOperator)
        {
            EmitExpression(assignmentOperator.Left);
            this.c.WhiteSpace();
            this.c.TextSpan("=");
            this.c.WhiteSpace();
            EmitExpression(assignmentOperator.Right);
        }

        private void EmitLocalLoad(BoundLocal local)
        {
            local is LocalWithInitializer

            this.c.WriteName(local.LocalSymbol);
        }

        private void EmitParameterLoad(BoundParameter expression)
        {
            c.WriteName(expression.ParameterSymbol);
        }

        private void EmitConversionExpression(BoundConversion conversion)
        {
            switch (conversion.ConversionKind)
            {
                case ConversionKind.MethodGroup:
                    throw new NotImplementedException();
                    ////EmitMethodGroupConversion(conversion, used);
                    return;
                case ConversionKind.NullToPointer:
                    // The null pointer is represented as 0u.
                    c.TextSpan("(uintptr_t)0");
                    return;
            }

            c.TextSpan("(");
            this.c.WriteType(conversion.Type);
            c.TextSpan(")");
            c.TextSpan("(");
            EmitExpression(conversion.Operand);
            c.TextSpan(")");
        }

        private void EmitBinaryOperatorExpression(BoundBinaryOperator expression)
        {
            EmitExpression(expression.Left);

            this.c.WhiteSpace();

            switch (expression.OperatorKind & BinaryOperatorKind.OpMask)
            {
                case BinaryOperatorKind.Multiplication:
                    this.c.TextSpan("*");
                    break;
                case BinaryOperatorKind.Addition:
                    this.c.TextSpan("+");
                    break;
                case BinaryOperatorKind.Subtraction:
                    this.c.TextSpan("-");
                    break;
                case BinaryOperatorKind.Division:
                    this.c.TextSpan("/");
                    break;
                case BinaryOperatorKind.Remainder:
                    this.c.TextSpan("%");
                    break;
                case BinaryOperatorKind.LeftShift:
                    this.c.TextSpan("<<");
                    break;
                case BinaryOperatorKind.RightShift:
                    this.c.TextSpan(">>");
                    break;
                case BinaryOperatorKind.Equal:
                    this.c.TextSpan("==");
                    break;
                case BinaryOperatorKind.NotEqual:
                    this.c.TextSpan("!=");
                    break;
                case BinaryOperatorKind.GreaterThan:
                    this.c.TextSpan(">");
                    break;
                case BinaryOperatorKind.LessThan:
                    this.c.TextSpan("<");
                    break;
                case BinaryOperatorKind.GreaterThanOrEqual:
                    this.c.TextSpan(">=");
                    break;
                case BinaryOperatorKind.LessThanOrEqual:
                    this.c.TextSpan("<=");
                    break;
                case BinaryOperatorKind.And:
                    this.c.TextSpan("&&");
                    break;
                case BinaryOperatorKind.Xor:
                    this.c.TextSpan("^");
                    break;
                case BinaryOperatorKind.Or:
                    this.c.TextSpan("||");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.c.WhiteSpace();

            EmitExpression(expression.Right);
        }

        private void EmitCallExpression(BoundCall call)
        {
            var method = call.Method;
            var receiver = call.ReceiverOpt;

            if (Method.MethodKind == MethodKind.Constructor && method.MethodKind == MethodKind.Constructor && receiver.Type.ToKeyString().Equals(((TypeSymbol)Method.ContainingType).ToKeyString()))
            {
                c.MarkHeader();
            }

            c.WriteMethodFullName(method);
            this.c.TextSpan("(");
            var anyArgs = false;
            foreach (var boundExpression in call.Arguments)
            {
                if (anyArgs)
                {
                    c.TextSpan(",");
                    c.WhiteSpace();
                }

                EmitExpression(boundExpression);

                anyArgs = true;
            }

            this.c.TextSpan(")");
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

// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
 namespace Il2Native.Logic.DOM2
{
    using System;

    using Il2Native.Logic.DOM.Implementations;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Conversion : Expression
    {
        public bool Checked { get; set; }

        public override Kinds Kind
        {
            get { return Kinds.Conversion; }
        }

        public Expression Operand { get; set; }

        // TODO: get rid of TypeSoure and use Operand.Type for it
        public ITypeSymbol TypeSource { get; set; }

        internal ConversionKind ConversionKind { get; set; }

        internal void Parse(BoundConversion boundConversion)
        {
            base.Parse(boundConversion);
            this.TypeSource = boundConversion.Operand.Type;
            this.Operand = Deserialize(boundConversion.Operand) as Expression;
            this.ConversionKind = boundConversion.ConversionKind;
            this.Checked = boundConversion.Checked;

            var methodGroupOperand = this.Operand as MethodGroup;
            if (methodGroupOperand != null && methodGroupOperand.Method == null)
            {
                var boundMethodGroup = boundConversion.Operand as BoundMethodGroup;
                if (boundMethodGroup != null)
                {
                    var methodGroup = new MethodGroup
                                          {
                                              ReceiverOpt = Deserialize(boundMethodGroup.ReceiverOpt) as Expression,
                                              Method = boundConversion.ExpressionSymbol as IMethodSymbol
                                          };

                    this.Operand = methodGroup;
                }
            }
        }

        internal override void Visit(Action<Base> visitor)
        {
            this.Operand.Visit(visitor);
            base.Visit(visitor);
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            var interfaceCastRequired = this.ConversionKind == ConversionKind.Boxing && Type.TypeKind == TypeKind.Interface;
            if (interfaceCastRequired)
            {
                c.TextSpan(this.TypeSource.AllInterfaces.Contains((INamedTypeSymbol)Type) ? "interface_cast" : "dynamic_interface_cast_or_throw");
                c.TextSpan("<");
                c.WriteType(Type);
                c.TextSpan(">");
                c.TextSpan("(");
            }

            var effectiveExpression = this.Operand;
            if ((this.ConversionKind == ConversionKind.Boxing || this.ConversionKind == ConversionKind.ImplicitReference) 
                && this.Operand.IsStaticOrSupportedVolatileWrapperCall())
            {
                effectiveExpression = new Cast
                {
                    Type = this.TypeSource,
                    Operand = effectiveExpression,
                    CCast = true,
                    UseEnumUnderlyingType = true,
                };
            }

            if (this.ConversionKind == ConversionKind.Unboxing && this.Operand.Type.TypeKind == TypeKind.Interface)
            {
                effectiveExpression = new Conversion
                {
                    Type = new NamedTypeImpl { SpecialType = SpecialType.System_Object },
                    TypeSource = this.TypeSource,
                    Operand = effectiveExpression,
                    ConversionKind = ConversionKind.ImplicitReference
                };
            }

            var parenthesis = false;
            if (this.WriteCast(c, out parenthesis))
            {
                if (parenthesis)
                {
                    c.WriteExpressionInParenthesesIfNeeded(effectiveExpression);
                }
                else
                {
                    c.TextSpan("(");
                    if (this.ConversionKind == ConversionKind.Boxing && Cs2CGenerator.DebugOutput)
                    {
                        c.TextSpan("__FILE__, __LINE__, ");
                    }

                    effectiveExpression.WriteTo(c);
                    c.TextSpan(")");
                }
            }

            if (interfaceCastRequired)
            {
                c.TextSpan(")");
            }
        }

        private bool WriteCast(CCodeWriterBase c, out bool parenthesis)
        {
            parenthesis = false;

            switch (this.ConversionKind)
            {
                case ConversionKind.MethodGroup:
                    var newDelegate = new DelegateCreationExpression { Type = Type };
                    newDelegate.Arguments.Add(this.Operand);
                    newDelegate.WriteTo(c);
                    return false;
                case ConversionKind.NullToPointer:
                    // The null pointer is represented as 0u.
                    c.TextSpan("(");
                    c.WriteType(Type);
                    c.TextSpan(")");
                    c.TextSpan("nullptr");
                    return false;
                case ConversionKind.Boxing:
                    c.TextSpan("__box");
                    if (Cs2CGenerator.DebugOutput)
                    {
                        c.TextSpan("_debug");
                    }

                    break;
                case ConversionKind.Unboxing:
                    c.TextSpan("__unbox<");
                    c.WriteType(Type, true, false, true);
                    c.TextSpan(">");
                    break;
                case ConversionKind.ExplicitReference:
                case ConversionKind.ImplicitReference:

                    if (Type.TypeKind == TypeKind.TypeParameter)
                    {
                        c.TextSpan("cast<");
                        c.WriteType(Type);
                        c.TextSpan(">");                        
                    }
                    else if (Type.TypeKind == TypeKind.Interface && this.TypeSource.AllInterfaces.Contains((INamedTypeSymbol)Type))
                    {
                        c.TextSpan("interface_cast<");
                        c.WriteType(Type);
                        c.TextSpan(">");
                    }
                    else if (Type.TypeKind == TypeKind.Interface)
                    {
                        c.TextSpan("dynamic_interface_cast_or_throw<");
                        c.WriteType(Type);
                        c.TextSpan(">");
                    }
                    else if (this.TypeSource.IsDerivedFrom(Type))
                    {
                        c.TextSpan("static_cast<");
                        c.WriteType(Type);
                        c.TextSpan(">");
                    }
                    else if (this.TypeSource.TypeKind == TypeKind.Interface && Type.SpecialType == SpecialType.System_Object)
                    {
                        c.TextSpan("object_cast");
                    }
                    else if (this.TypeSource.TypeKind == TypeKind.Array && Type.TypeKind == TypeKind.Array)
                    {
                        c.TextSpan("reinterpret_cast<");
                        c.WriteType(Type);
                        c.TextSpan(">");
                    }
                    else
                    {
                        c.TextSpan("cast<");
                        c.WriteType(Type);
                        c.TextSpan(">");
                    }

                    break;
                case ConversionKind.PointerToInteger:
                case ConversionKind.IntegerToPointer:
                case ConversionKind.PointerToPointer:

                    if (!Type.IsIntPtrType())
                    {
                        c.TextSpan("(");
                        c.WriteType(Type);
                        c.TextSpan(")");

                        parenthesis = true;
                    }

                    break;
                case ConversionKind.Identity:
                    // for string
                    if (this.TypeSource.SpecialType == SpecialType.System_String && Type.TypeKind == TypeKind.Pointer)
                    {
                        c.TextSpan("&");
                        this.Operand.WriteTo(c);
                        c.TextSpan("->m_firstChar");
                        return false;
                    }

                    return true;
                default:
                    if (this.Checked)
                    {
                        c.TextSpan("checked_");
                    }

                    c.TextSpan("static_cast<");
                    c.WriteType(Type);
                    c.TextSpan(">");
                    break;
            }

            return true;
        }
    }
}

namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Literal : Expression
    {
        private ConstantValue constantValue;

        internal void Parse(BoundLiteral boundLiteral)
        {
            if (boundLiteral == null)
            {
                throw new ArgumentNullException();
            }

            this.constantValue = boundLiteral.ConstantValue;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            ConstantValueTypeDiscriminator discriminator = this.constantValue.Discriminator;

            switch (discriminator)
            {
                case ConstantValueTypeDiscriminator.Null:
                    c.TextSpan("nullptr");
                    break;
                case ConstantValueTypeDiscriminator.SByte:
                    c.TextSpan(this.constantValue.SByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Byte:
                    c.TextSpan(this.constantValue.ByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.UInt16:
                    c.TextSpan(this.constantValue.UInt16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Char:
                    c.TextSpan(string.Format("L'{0}'", this.constantValue.CharValue));
                    break;
                case ConstantValueTypeDiscriminator.Int16:
                    c.TextSpan(this.constantValue.Int16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Int32:
                case ConstantValueTypeDiscriminator.UInt32:
                    c.TextSpan(this.constantValue.Int32Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Int64:
                    c.TextSpan(this.constantValue.Int64Value.ToString());
                    c.TextSpan("L");
                    break;
                case ConstantValueTypeDiscriminator.UInt64:
                    c.TextSpan(this.constantValue.Int64Value.ToString());
                    c.TextSpan("UL");
                    break;
                case ConstantValueTypeDiscriminator.Single:
                    c.TextSpan(this.constantValue.SingleValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Double:
                    c.TextSpan(this.constantValue.DoubleValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.String:
                    c.TextSpan(string.Format("L\"{0}\"_s", this.constantValue.StringValue));
                    break;
                case ConstantValueTypeDiscriminator.Boolean:
                    c.TextSpan(this.constantValue.BooleanValue.ToString().ToLowerInvariant());
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}

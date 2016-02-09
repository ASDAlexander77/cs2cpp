namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using System.Text;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Literal : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Literal; }
        }

        internal ConstantValue Value { get; set; }

        internal void Parse(BoundLiteral boundLiteral)
        {
            base.Parse(boundLiteral);
            this.Value = boundLiteral.ConstantValue;
        }

        public override string ToString()
        {
            ConstantValueTypeDiscriminator discriminator = this.Value.Discriminator;

            switch (discriminator)
            {
                case ConstantValueTypeDiscriminator.Null:
                    return "nullptr";
                case ConstantValueTypeDiscriminator.SByte:
                    return this.Value.SByteValue.ToString();
                case ConstantValueTypeDiscriminator.Byte:
                    return this.Value.ByteValue.ToString();
                case ConstantValueTypeDiscriminator.Int16:
                    return this.Value.Int16Value.ToString();
                case ConstantValueTypeDiscriminator.UInt16:
                    return this.Value.UInt16Value.ToString();
                case ConstantValueTypeDiscriminator.Char:
                    return string.Format("L'{0}'", UnicodeChar(this.Value.CharValue));
                case ConstantValueTypeDiscriminator.Int32:
                    return this.Value.Int32Value.ToString();
                case ConstantValueTypeDiscriminator.UInt32:
                    return  string.Format("{0}U", this.Value.UInt32Value);
                case ConstantValueTypeDiscriminator.Int64:
                    return  string.Format("{0}LL", this.Value.Int64Value);
                case ConstantValueTypeDiscriminator.UInt64:
                    return  string.Format("{0}ULL", this.Value.UInt64Value);
                case ConstantValueTypeDiscriminator.Single:
                    var line = this.Value.DoubleValue.ToString();
                    return string.Format("{0}f", line.IndexOf('.') != -1 ? line : string.Concat(line, ".0"));
                case ConstantValueTypeDiscriminator.Double:
                    line = this.Value.DoubleValue.ToString();
                    return line.IndexOf('.') != -1 ? line : string.Concat(line, ".0");
                case ConstantValueTypeDiscriminator.String:
                    return string.Format("L\"{0}\"", UnicodeString(this.Value.StringValue));
                case ConstantValueTypeDiscriminator.Boolean:
                    return this.Value.BooleanValue.ToString().ToLowerInvariant();
                default:
                    throw new NotSupportedException();
            }
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            ConstantValueTypeDiscriminator discriminator = this.Value.Discriminator;
            switch (discriminator)
            {
                case ConstantValueTypeDiscriminator.Null:
                    c.TextSpan("nullptr");
                    break;
                case ConstantValueTypeDiscriminator.SByte:
                    c.TextSpan("(int8_t)");
                    c.TextSpan(this.Value.SByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Byte:
                    c.TextSpan("(uint8_t)");
                    c.TextSpan(this.Value.ByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Int16:
                    c.TextSpan("(int16_t)");
                    c.TextSpan(this.Value.Int16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.UInt16:
                    c.TextSpan("(uint16_t)");
                    c.TextSpan(this.Value.UInt16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Char:
                    c.TextSpan(string.Format("L'{0}'", UnicodeChar(this.Value.CharValue)));
                    break;
                case ConstantValueTypeDiscriminator.Int32:
                    c.TextSpan(this.Value.Int32Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.UInt32:
                    c.TextSpan(this.Value.UInt32Value.ToString());
                    c.TextSpan("U");
                    break;
                case ConstantValueTypeDiscriminator.Int64:
                    c.TextSpan(this.Value.Int64Value.ToString());
                    c.TextSpan("LL");
                    break;
                case ConstantValueTypeDiscriminator.UInt64:
                    c.TextSpan(this.Value.UInt64Value.ToString());
                    c.TextSpan("ULL");
                    break;
                case ConstantValueTypeDiscriminator.Single:
                    var line = this.Value.SingleValue.ToString();
                    c.TextSpan(line.IndexOf('.') != -1 ? line : string.Concat(line, ".0"));
                    c.TextSpan("f");
                    break;
                case ConstantValueTypeDiscriminator.Double:
                    line = this.Value.DoubleValue.ToString();
                    c.TextSpan(line.IndexOf('.') != -1 ? line : string.Concat(line, ".0"));
                    break;
                case ConstantValueTypeDiscriminator.String:
                    c.TextSpan(string.Format("L\"{0}\"_s", UnicodeString(this.Value.StringValue)));
                    break;
                case ConstantValueTypeDiscriminator.Boolean:
                    c.TextSpan(this.Value.BooleanValue.ToString().ToLowerInvariant());
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public static string UnicodeString(string value)
        {
            var sb = new StringBuilder();
            foreach (var c in value.ToCharArray())
            {
                sb.Append(UnicodeChar(c));
            }

            return sb.ToString();
        }

        public static string UnicodeChar(char c)
        {
            var code = (uint)c;
            switch (code)
            {
                case 0x07:
                    return (@"\a");
                case 0x08:
                    return (@"\b");
                case 0x0C:
                    return (@"\f");
                case 0x0A:
                    return (@"\n");
                case 0x0D:
                    return (@"\r");
                case 0x09:
                    return (@"\t");
                case 0x0B:
                    return (@"\v");
                case 0x5C:
                    return (@"\\");
                case 0x27:
                    return (@"\'");
                case 0x22:
                    return (@"\""");
                case 0x3F:
                    return (@"\?");
                default:
                    if (code >= 0x20 && c <= '~')
                    {
                        return c.ToString();
                    }

                    if (char.IsHighSurrogate(c) || char.IsLowSurrogate(c))
                    {
                        return string.Format("\\x{0:X4}", (uint)c);
                    }

                    return string.Format("\\u{0:X4}", (uint)c);
            }
        }
    }
}

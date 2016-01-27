namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Diagnostics;
    using System.Text;

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
                    c.TextSpan("(int8_t)");
                    c.TextSpan(this.constantValue.SByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Byte:
                    c.TextSpan("(uint8_t)");
                    c.TextSpan(this.constantValue.ByteValue.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Int16:
                    c.TextSpan("(int16_t)");
                    c.TextSpan(this.constantValue.Int16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.UInt16:
                    c.TextSpan("(uint16_t)");
                    c.TextSpan(this.constantValue.UInt16Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.Char:
                    c.TextSpan(string.Format("L'{0}'", UnicodeChar(this.constantValue.CharValue)));
                    break;
                case ConstantValueTypeDiscriminator.Int32:
                    c.TextSpan(this.constantValue.Int32Value.ToString());
                    break;
                case ConstantValueTypeDiscriminator.UInt32:
                    c.TextSpan("(uint32_t)");
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
                    var line = this.constantValue.DoubleValue.ToString();
                    c.TextSpan(line.IndexOf('.') != -1 ? line : string.Concat(line, ".0"));
                    c.TextSpan("f");
                    break;
                case ConstantValueTypeDiscriminator.Double:
                    line = this.constantValue.DoubleValue.ToString();
                    c.TextSpan(line.IndexOf('.') != -1 ? line : string.Concat(line, ".0"));
                    break;
                case ConstantValueTypeDiscriminator.String:
                    c.TextSpan(string.Format("L\"{0}\"_s", UnicodeString(this.constantValue.StringValue)));
                    break;
                case ConstantValueTypeDiscriminator.Boolean:
                    c.TextSpan(this.constantValue.BooleanValue.ToString().ToLowerInvariant());
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

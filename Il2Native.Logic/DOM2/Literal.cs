// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    using System;
    using System.Text;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class Literal : Expression
    {
        public override Kinds Kind
        {
            get { return Kinds.Literal; }
        }

        public bool CppConstString { get; set; }

        internal ConstantValue Value { get; set; }

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

                    if (c == 0 || char.IsHighSurrogate(c) || char.IsLowSurrogate(c))
                    {
                        return string.Format("\\x{0:X4}", (uint)c);
                    }

                    return string.Format("\\u{0:X4}", (uint)c);
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

        public override string ToString()
        {
            var discriminator = this.Value.Discriminator;

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
                    return string.Format("u'{0}'", UnicodeChar(this.Value.CharValue));
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
                    if (float.IsPositiveInfinity(this.Value.SingleValue))
                    {
                        return "std::numeric_limits<float>::infinity()";
                    }
                    else if (float.IsNegativeInfinity(this.Value.SingleValue))
                    {
                        return "-std::numeric_limits<float>::infinity()";
                    }
                    else if (float.IsNaN(this.Value.SingleValue))
                    {
                        return "std::numeric_limits<float>::quiet_NaN()";
                    }

                    return string.Format("{0}f", line.IndexOf('.') != -1 || line.IndexOf('E') != -1 ? line : string.Concat(line, ".0"));
                case ConstantValueTypeDiscriminator.Double:
                    if (double.IsPositiveInfinity(this.Value.DoubleValue))
                    {
                        return "std::numeric_limits<double>::infinity()";
                    }
                    else if (double.IsNegativeInfinity(this.Value.DoubleValue))
                    {
                        return "-std::numeric_limits<double>::infinity()";
                    }
                    else if (double.IsNaN(this.Value.DoubleValue))
                    {
                        return "std::numeric_limits<double>::quiet_NaN()";
                    }

                    line = this.Value.DoubleValue.ToString();
                    return line.IndexOf('.') != -1 || line.IndexOf('E') != -1 ? line : string.Concat(line, ".0");
                case ConstantValueTypeDiscriminator.String:
                    return string.Format("u\"{0}\"", UnicodeString(this.Value.StringValue));
                case ConstantValueTypeDiscriminator.Boolean:
                    return this.Value.BooleanValue.ToString().ToLowerInvariant();
                default:
                    throw new NotSupportedException();
            }
        }

        internal void Parse(BoundLiteral boundLiteral)
        {
            base.Parse(boundLiteral);
            this.Value = boundLiteral.ConstantValue;
        }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (Type != null && Type.TypeKind == TypeKind.Enum)
            {
                c.TextSpan("(");
                c.WriteType(Type);
                c.TextSpan(")");
            }

            var discriminator = this.Value.Discriminator;
            switch (discriminator)
            {
                case ConstantValueTypeDiscriminator.Null:
                    c.TextSpan("nullptr");
                    break;
                case ConstantValueTypeDiscriminator.SByte:
                    if (sbyte.MaxValue == this.Value.SByteValue)
                    {
                        c.TextSpan("std::numeric_limits<int8_t>::max()");
                    }
                    else if (sbyte.MinValue == this.Value.SByteValue)
                    {
                        c.TextSpan("std::numeric_limits<int8_t>::min()");
                    }
                    else
                    {
                        c.TextSpan("(int8_t)");
                        c.TextSpan(this.Value.SByteValue.ToString());
                    }
                    
                    break;
                case ConstantValueTypeDiscriminator.Byte:
                    if (byte.MaxValue == this.Value.ByteValue)
                    {
                        c.TextSpan("std::numeric_limits<uint8_t>::max()");
                    }
                    else if (byte.MinValue == this.Value.ByteValue)
                    {
                        c.TextSpan("std::numeric_limits<uint8_t>::min()");
                    }
                    else
                    {
                        c.TextSpan("(uint8_t)");
                        c.TextSpan(this.Value.ByteValue.ToString());
                    }

                    break;
                case ConstantValueTypeDiscriminator.Int16:
                    if (short.MaxValue == this.Value.Int16Value)
                    {
                        c.TextSpan("std::numeric_limits<int16_t>::max()");
                    }
                    else if (short.MinValue == this.Value.Int16Value)
                    {
                        c.TextSpan("std::numeric_limits<int16_t>::min()");
                    }
                    else
                    {
                        c.TextSpan("(int16_t)");
                        c.TextSpan(this.Value.Int16Value.ToString());
                    }

                    break;
                case ConstantValueTypeDiscriminator.UInt16:
                    if (ushort.MaxValue == this.Value.UInt16Value)
                    {
                        c.TextSpan("std::numeric_limits<uint16_t>::max()");
                    }
                    else if (ushort.MinValue == this.Value.UInt16Value)
                    {
                        c.TextSpan("std::numeric_limits<uint16_t>::min()");
                    }
                    else
                    {
                        c.TextSpan("(uint16_t)");
                        c.TextSpan(this.Value.UInt16Value.ToString());
                    }
                    break;
                case ConstantValueTypeDiscriminator.Char:
                    c.TextSpan(string.Format("u'{0}'", UnicodeChar(this.Value.CharValue)));
                    break;
                case ConstantValueTypeDiscriminator.Int32:
                    if (int.MaxValue == this.Value.Int32Value)
                    {
                        c.TextSpan("std::numeric_limits<int32_t>::max()");
                    }
                    else if (int.MinValue == this.Value.Int32Value)
                    {
                        c.TextSpan("std::numeric_limits<int32_t>::min()");
                    }
                    else
                    {
                        c.TextSpan(this.Value.Int32Value.ToString());
                    }

                    break;
                case ConstantValueTypeDiscriminator.UInt32:
                    if (uint.MaxValue == this.Value.UInt32Value)
                    {
                        c.TextSpan("std::numeric_limits<uint32_t>::max()");
                    }
                    else if (uint.MinValue == this.Value.UInt32Value)
                    {
                        c.TextSpan("std::numeric_limits<uint32_t>::min()");
                    }
                    else
                    {
                        c.TextSpan(this.Value.UInt32Value.ToString());
                        c.TextSpan("U");
                    }

                    break;
                case ConstantValueTypeDiscriminator.Int64:
                    if (long.MaxValue == this.Value.Int64Value)
                    {
                        c.TextSpan("std::numeric_limits<int64_t>::max()");
                    }
                    else if (long.MinValue == this.Value.Int64Value)
                    {
                        c.TextSpan("std::numeric_limits<int64_t>::min()");
                    }
                    else
                    {
                        c.TextSpan("(int64_t)");
                        c.TextSpan(this.Value.Int64Value.ToString());
                        c.TextSpan("LL");
                    }

                    break;
                case ConstantValueTypeDiscriminator.UInt64:

                    if (ulong.MaxValue == this.Value.UInt64Value)
                    {
                        c.TextSpan("std::numeric_limits<uint64_t>::max()");
                    }
                    else if (ulong.MinValue == this.Value.UInt64Value)
                    {
                        c.TextSpan("std::numeric_limits<uint64_t>::min()");
                    }
                    else
                    {
                        c.TextSpan("(uint64_t)");
                        c.TextSpan(this.Value.UInt64Value.ToString());
                        c.TextSpan("ULL");
                    }

                    break;
                case ConstantValueTypeDiscriminator.Single:
                    if (float.IsPositiveInfinity(this.Value.SingleValue))
                    {
                        c.TextSpan("std::numeric_limits<float>::infinity()");
                    }
                    else if (float.IsNegativeInfinity(this.Value.SingleValue))
                    {
                        c.TextSpan("-std::numeric_limits<float>::infinity()");
                    }
                    else if (float.IsNaN(this.Value.SingleValue))
                    {
                        c.TextSpan("std::numeric_limits<float>::quiet_NaN()");
                    }
                    else if (float.MaxValue == this.Value.SingleValue)
                    {
                        c.TextSpan("std::numeric_limits<float>::max()");
                    }
                    else if (float.MinValue == this.Value.SingleValue)
                    {
                        c.TextSpan("std::numeric_limits<float>::min()");
                    }
                    else
                    {
                        var line = this.Value.SingleValue.ToString();
                        c.TextSpan(line.IndexOf('.') != -1 || line.IndexOf('E') != -1 ? line : string.Concat(line, ".0"));
                        c.TextSpan("f");
                    }

                    break;
                case ConstantValueTypeDiscriminator.Double:
                    if (double.IsPositiveInfinity(this.Value.DoubleValue))
                    {
                        c.TextSpan("std::numeric_limits<double>::infinity()");
                    }
                    else if (double.IsNegativeInfinity(this.Value.DoubleValue))
                    {
                        c.TextSpan("-std::numeric_limits<double>::infinity()");
                    }
                    else if (double.IsNaN(this.Value.DoubleValue))
                    {
                        c.TextSpan("std::numeric_limits<double>::quiet_NaN()");
                    }
                    else if (double.MaxValue == this.Value.DoubleValue)
                    {
                        c.TextSpan("std::numeric_limits<double>::max()");
                    }
                    else if (double.MinValue == this.Value.DoubleValue)
                    {
                        c.TextSpan("std::numeric_limits<double>::min()");
                    }
                    else
                    {
                        var line = this.Value.DoubleValue.ToString();
                        c.TextSpan(line.IndexOf('.') != -1 || line.IndexOf('E') != -1 ? line : string.Concat(line, ".0"));
                    }

                    break;
                case ConstantValueTypeDiscriminator.String:
                    c.TextSpan(string.Format("u\"{0}\"", UnicodeString(this.Value.StringValue)));
                    if (!CppConstString)
                    {
                        c.TextSpan("_s");
                    }

                    break;
                case ConstantValueTypeDiscriminator.Boolean:
                    c.TextSpan(this.Value.BooleanValue.ToString().ToLowerInvariant());
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}

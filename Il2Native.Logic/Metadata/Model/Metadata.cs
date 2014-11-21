namespace Il2Native.Logic.Metadata.Model
{
    using System.IO;

    public class Metadata
    {
        public Metadata(object value)
        {
            this.Value = value;
        }

        public object Value { get; private set; }

        public virtual void WriteTo(TextWriter output, bool suppressMetadataKeyword = false)
        {
            WriteValueTo(output, this.Value, suppressMetadataKeyword);
        }

        public static void WriteValueTo(TextWriter output, object value, bool suppressMetadataKeyword = false)
        {
            var metadata = value as Metadata;
            if (metadata != null)
            {
                metadata.WriteTo(output, suppressMetadataKeyword);
            }
            else if (value == null)
            {
                output.Write("null");
            }
            else if (value is string)
            {
                if (!suppressMetadataKeyword)
                {
                    output.Write("metadata ");
                }

                output.Write("!\"");
                output.Write(value.ToString());
                output.Write("\"");
            }
            else if (value is int)
            {
                output.Write("i32 ");
                output.Write(value.ToString());
            }
            else if (value is bool)
            {
                output.Write("i1 ");
                output.Write(value.ToString().ToLowerInvariant());
            }
        }
    }
}

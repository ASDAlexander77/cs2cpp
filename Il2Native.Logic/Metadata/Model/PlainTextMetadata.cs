namespace Il2Native.Logic.Metadata.Model
{
    using System.IO;

    public class PlainTextMetadata : Metadata
    {
        public PlainTextMetadata(object value)
            : base(value)
        {
        }

        public override void WriteTo(TextWriter output, bool suppressMetadataKeyword = false)
        {
            output.Write(this.Value);
        }
    }
}

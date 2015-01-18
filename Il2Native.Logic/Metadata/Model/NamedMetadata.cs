namespace Il2Native.Logic.Metadata.Model
{
    using System.IO;

    public class NamedMetadata : Metadata
    {
        public NamedMetadata(string name, object value)
            : base(value)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public override void WriteTo(TextWriter output, bool suppressMetadataKeyword = false)
        {
            output.Write("!{0} = ", this.Name);
            base.WriteTo(output, suppressMetadataKeyword);
            output.WriteLine(string.Empty);
        }
    }
}
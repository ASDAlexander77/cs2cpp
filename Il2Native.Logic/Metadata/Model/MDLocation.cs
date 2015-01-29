namespace Il2Native.Logic.Metadata.Model
{
    using System.Collections.Generic;
    using System.IO;

    public class MDLocation : CollectionMetadata
    {
        private int _line;
        private int _offset;
        private object _scope;

        public MDLocation(int line, int offset, object scope, IList<CollectionMetadata> model)
            : base(model)
        {
            _line = line;
            _offset = offset;
            _scope = scope;
        }

        public override void WriteValueTo(TextWriter output, bool suppressMetadataKeyword = false)
        {
            output.Write("!MDLocation(line: {0}, column: {1}, scope:", _line, _offset);
            WriteValueTo(output, _scope, suppressMetadataKeyword);
            output.Write(")");
        }
    }
}

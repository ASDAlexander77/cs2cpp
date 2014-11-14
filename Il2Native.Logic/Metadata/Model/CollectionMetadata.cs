namespace Il2Native.Logic.Metadata.Model
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class CollectionMetadata : Metadata
    {
        public CollectionMetadata()
            : base(new List<object>())
        {
        }

        public CollectionMetadata(IList<CollectionMetadata> model)
            : this()
        {
            model.Add(this);
        }

        public int? Index { get; set; }

        public bool NullIfEmpty { get; set; }

        public bool IsEmpty
        {
            get
            {
                var objects = this.Value as IList<object>;
                if (objects == null)
                {
                    throw new NotSupportedException();
                }

                return objects.Count == 0;
            }
        }

        public CollectionMetadata Add(object value)
        {
            var objects = this.Value as IList<object>;
            if (objects != null)
            {
                objects.Add(value);
                return this;
            }

            throw new NotSupportedException();
        }

        public CollectionMetadata Add(params object[] values)
        {
            var objects = this.Value as IList<object>;
            if (objects != null)
            {
                foreach (var value in values)
                {
                    objects.Add(value);
                }

                return this;
            }

            throw new NotSupportedException();
        }

        public override void WriteTo(System.IO.TextWriter output, bool suppressMetadataKeyword = false)
        {
            if (this.NullIfEmpty && this.IsEmpty)
            {
                return;
            }

            if (Index.HasValue)
            {
                if (!suppressMetadataKeyword)
                {
                    output.Write("metadata ");
                }

                output.Write("!");
                output.Write(Index.Value);
                return;
            }

            this.WriteValueTo(output, suppressMetadataKeyword);
        }

        public void WriteValueTo(TextWriter output, bool suppressMetadataKeyword = false)
        {
            var objects = this.Value as IList<object>;
            if (objects == null)
            {
                throw new NotSupportedException();
            }

            if (!suppressMetadataKeyword)
            {
                output.Write("metadata ");
            }

            output.Write("!{");

            var index = 0;
            foreach (var @object in objects)
            {
                if (index > 0)
                {
                    output.Write(", ");
                }

                Metadata.WriteValueTo(output, @object, suppressMetadataKeyword);

                index++;
            }

            output.Write("}");
        }
    }
}

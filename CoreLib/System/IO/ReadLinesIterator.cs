using System.Text;

namespace System.IO
{
    internal class ReadLinesIterator : Iterator<string>
    {
        private readonly string _path;
        private readonly Encoding _encoding;
        private StreamReader _reader;

        private ReadLinesIterator(string path, Encoding encoding, StreamReader reader)
        {
            _path = path;
            _encoding = encoding;
            _reader = reader;
        }

        public override bool MoveNext()
        {
            if (this._reader != null)
            {
                this.current = _reader.ReadLine();
                if (this.current != null)
                    return true;

                Dispose();
            }

            return false;
        }

        protected override Iterator<string> Clone()
        {
            return CreateIterator(_path, _encoding, _reader);
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_reader != null)
                    {
                        _reader.Dispose();
                    }
                }
            }
            finally
            {
                _reader = null;
                base.Dispose(disposing);
            }
        }

        internal static ReadLinesIterator CreateIterator(string path, Encoding encoding)
        {
            return CreateIterator(path, encoding, (StreamReader)null);
        }

        private static ReadLinesIterator CreateIterator(string path, Encoding encoding, StreamReader reader)
        {
            return new ReadLinesIterator(path, encoding, reader ?? new StreamReader(path, encoding));
        }
    }
}

namespace System.Collections.Generic
{
    using System;
    using System.Text;

    [Serializable]
    public struct KeyValuePair<TKey, TValue>
    {
        private TKey key;
        private TValue value;

        public KeyValuePair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }

        public TKey Key
        {
            get { return key; }
        }

        public TValue Value
        {
            get { return value; }
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append('[');
            if (Key != null)
            {
                s.Append(Key);
            }

            s.Append(", ");
            if (Value != null)
            {
                s.Append(Value);
            }

            s.Append(']');

            return s.ToString();
        }
    }
}

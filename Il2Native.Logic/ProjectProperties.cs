namespace Il2Native.Logic
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Reflection.Metadata;

    public class ProjectProperties : IDictionary<string, string>
    {
        IDictionary<string, string> dict = new Dictionary<string, string>();

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            dict.Add(item);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return dict.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            dict.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return dict.Remove(item);
        }

        public int Count
        {
            get { return dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return dict.IsReadOnly; }
        }

        public bool ContainsKey(string key)
        {
            return dict.ContainsKey(key);
        }

        public void Add(string key, string value)
        {
            dict.Add(key, value);
        }

        public bool Remove(string key)
        {
            return dict.Remove(key);
        }

        public bool TryGetValue(string key, out string value)
        {
            return dict.TryGetValue(key, out value);
        }

        public string this[string key]
        {
            get
            {
                if (!this.ContainsKey(key))
                {
                    return string.Empty;
                }

                return this.dict[key];
            }

            set { this.dict[key] = value; }
        }

        public ICollection<string> Keys 
        {
            get { return this.dict.Keys; }
        }

        public ICollection<string> Values
        {
            get { return this.dict.Values; }
        }
    }
}

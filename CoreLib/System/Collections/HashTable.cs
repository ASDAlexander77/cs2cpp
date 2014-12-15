////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;


namespace System.Collections
{
    /// <summary>
    /// HashTable is an Associative Container.
    /// Created in March 2010.
    /// by Eric Harlow.
    /// </summary>
    public class Hashtable : ICloneable, IDictionary
    {
        internal const Int32 HashPrime = 101;

        Entry[] _buckets;
        int _numberOfBuckets;
        int _count;
        int _loadFactor;
        int _maxLoadFactor;
        double _growthFactor;
        const int _defaultCapacity = 4;
        const int _defaultLoadFactor = 2;

        /// <summary>
        /// Initializes a new, empty instance of the Hashtable class using the default initial capacity and load factor.
        /// </summary>
        public Hashtable()
        {
            InitializeHashTable(_defaultCapacity, _defaultLoadFactor);
        }

        /// <summary>
        /// Initializes a new, empty instance of the Hashtable class using the specified initial capacity, 
        /// and the default load factor.
        /// </summary>
        /// <param name="capacity">The initial capacity of the HashTable</param>
        public Hashtable(int capacity)
        {
            InitializeHashTable(capacity, _defaultLoadFactor);
        }

        /// <summary>
        /// Initializes a new, empty instance of the Hashtable class using the specified initial capacity, 
        /// load factor.
        /// </summary>
        /// <param name="capacity">The initial capacity of the HashTable</param>
        /// <param name="maxLoadFactor">The load factor to cause a rehash</param>
        public Hashtable(int capacity, int maxLoadFactor)
        {
            InitializeHashTable(capacity, maxLoadFactor);
        }

        //initialize attributes
        private void InitializeHashTable(int capacity, int maxLoadFactor)
        {
            _buckets = new Entry[capacity];
            _numberOfBuckets = capacity;
            _maxLoadFactor = maxLoadFactor;
            _growthFactor = 2;
        }

        /// <summary>
        /// MaxLoadFactor Property is the value used to trigger a rehash.
        /// Default value is 2.
        /// A higher number can decrease lookup performance for large collections.
        /// While a value of 1 maintains a constant time complexity at the cost of increased memory requirements.   
        /// </summary>
        public int MaxLoadFactor
        {
            get { return _maxLoadFactor; }
            set { _maxLoadFactor = value; }
        }

        /// <summary>
        /// GrowthFactor Property is a multiplier to increase the HashTable size by during a rehash.
        /// Default value is 2.
        /// </summary>
        public double GrowthFactor
        {
            get { return _growthFactor; }
            set { _growthFactor = value; }
        }

        //adding for internal purposes
        private void Add(ref Entry[] buckets, object key, object value, bool overwrite)
        {
            int whichBucket = Hash(key, _numberOfBuckets);
            Entry match = EntryForKey(key, buckets[whichBucket]);

            if (match != null && overwrite)
            { //i.e. already exists in table
                match.value = value;
                return;
            }
            else if ((match != null && !overwrite))
            {
                throw new ArgumentException("key exists");
            }
            else
            {
                // insert at front
                Entry newOne = new Entry(key, value, ref buckets[whichBucket]);
                buckets[whichBucket] = newOne;
                _count++;
            }

            _loadFactor = _count / _numberOfBuckets;
        }

        // Hash function.
        private int Hash(object key, int numOfBuckets)
        {
            int hashcode = key.GetHashCode();

            if (hashcode < 0) // don't know how to mod with a negative number
                hashcode = hashcode * -1;

            return hashcode % numOfBuckets;
        }

        //looks up value in bucket
        private Entry EntryForKey(object key, Entry head)
        {
            for (Entry cur = head; cur != null; cur = cur.next)
                if (cur.key.Equals(key)) return cur;

            return null;
        }

        //Rehashes the table to reduce the load factor
        private void Rehash(int newSize)
        {
            Entry[] newTable = new Entry[newSize];
            _numberOfBuckets = newSize;
            _count = 0;
            for (int i = 0; i < _buckets.Length; i++)
            {
                if (_buckets[i] != null)
                {
                    for (Entry cur = _buckets[i]; cur != null; cur = cur.next)
                        Add(ref newTable, cur.key, cur.value, false);
                }
            }
            _buckets = newTable;
        }

        //implementation for KeyCollection and ValueCollection copyTo method
        private void CopyToCollection(Array array, int index, EnumeratorType type)
        {
            if (index < 0 && index > _numberOfBuckets)
                throw new IndexOutOfRangeException("index");

            int j = 0;
            int len = array.Length;

            for (int i = index; i < _numberOfBuckets; i++)
            {
                for (Entry cur = _buckets[i]; cur != null && j < len; cur = cur.next)
                {
                    if (type == EnumeratorType.KEY)
                        ((IList)array)[j] = cur.key;
                    else
                        ((IList)array)[j] = cur.value;

                    j++;
                }
            }
        }

        #region ICloneable Members
        //shallow copy
        public object Clone()
        {
            Hashtable ht = new Hashtable();
            ht.InitializeHashTable(_numberOfBuckets, _maxLoadFactor);
            ht._count = _count;
            ht._loadFactor = _loadFactor;
            Array.Copy(_buckets, ht._buckets, _numberOfBuckets);
            return ht;
        }

        #endregion ICloneable Members

        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return new HashtableEnumerator(this, EnumeratorType.DE);
        }

        #endregion IEnumerable Members

        #region ICollection Members

        public int Count
        {
            get { return _count; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        public void CopyTo(Array array, int index)
        {
            if (index < 0 && index > _buckets.Length)
                throw new IndexOutOfRangeException("index");

            int j = 0;
            int len = array.Length;
            for (int i = index; i < _buckets.Length; i++)
            {
                for (Entry cur = _buckets[i]; cur != null && j < len; cur = cur.next)
                {
                    ((IList)array)[j] = new DictionaryEntry(cur.key, cur.value);
                    j++;
                }
            }
        }

        #endregion ICollection Members

        #region IDictionary Members

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public ICollection Keys
        {
            get
            {
                return new KeyCollection(this);
            }
        }

        public ICollection Values
        {
            get
            {
                return new ValueCollection(this);
            }
        }

        public object this[object key]
        {
            get
            {
                if (key == null) throw new ArgumentNullException("key is null");
                int whichBucket = Hash(key, _numberOfBuckets);
                Entry match = EntryForKey(key, _buckets[whichBucket]);
                if (match != null)
                    return match.value;
                return null;
            }
            set
            {
                if (key == null) throw new ArgumentNullException("key is null");

                Add(ref _buckets, key, value, true);
                if (_loadFactor >= _maxLoadFactor)
                    Rehash((int)(_numberOfBuckets * _growthFactor));
            }
        }

        public void Add(object key, object value)
        {
            if (key == null) throw new ArgumentNullException("key is null");

            Add(ref _buckets, key, value, false);
            if (_loadFactor >= _maxLoadFactor)
                Rehash((int)(_numberOfBuckets * _growthFactor));
        }

        public void Clear()
        {
            _buckets = new Entry[_defaultCapacity];
            _numberOfBuckets = _defaultCapacity;
            _loadFactor = 0;
            _count = 0;
        }

        public bool Contains(object key)
        {
            if (key == null) throw new ArgumentNullException("key is null");
            int whichBucket = Hash(key, _numberOfBuckets);
            Entry match = EntryForKey(key, _buckets[whichBucket]);

            if (match != null)
                return true;
            return false;
        }

        public void Remove(object key)
        {
            if (key == null) throw new ArgumentNullException("key is null");
            int whichBucket = Hash(key, _numberOfBuckets);
            Entry match = EntryForKey(key, _buckets[whichBucket]);

            //does entry exist?
            if (match == null)
                return;

            //is entry at front?
            if (_buckets[whichBucket] == match)
            {
                _buckets[whichBucket] = match.next;
                _count--;
                return;
            }

            //handle entry in middle and at the end
            for (Entry cur = _buckets[whichBucket]; cur != null; cur = cur.next)
            {
                if (cur.next == match)
                {
                    cur.next = match.next;
                    _count--;
                    return;
                }
            }
        }

        #endregion IDictionary Members

        private class Entry
        {
            public Object key;
            public Object value;
            public Entry next;

            public Entry(object key, object value, ref Entry n)
            {
                this.key = key;
                this.value = value;
                this.next = n;
            }
        }

        private class HashtableEnumerator : IEnumerator
        {
            Hashtable ht;
            Entry temp;
            Int32 index = -1;
            EnumeratorType returnType;

            public HashtableEnumerator(Hashtable hashtable, EnumeratorType type)
            {
                ht = hashtable;
                returnType = type;
            }

            // Return the current item.
            public Object Current
            {
                get
                {
                    switch (returnType)
                    {
                        case EnumeratorType.DE:
                            return new DictionaryEntry(temp.key, temp.value);

                        case EnumeratorType.KEY:
                            return temp.key;

                        case EnumeratorType.VALUE:
                            return temp.value;

                        default:
                            break;
                    }
                    return new DictionaryEntry(temp.key, temp.value);
                }
            }

            // Advance to the next item.
            public Boolean MoveNext()
            {
            startLoop:
                //iterate index or list
                if (temp == null)
                {
                    index++;
                    if (index < ht._numberOfBuckets)
                        temp = ht._buckets[index];
                    else
                        return false;
                }
                else
                    temp = temp.next;

                //null check
                if (temp == null)
                    goto startLoop;

                return true;
            }

            // Reset the index to restart the enumeration.
            public void Reset()
            {
                index = -1;
            }
        }

        // EnumeratorType - Enum that describes which object the Enumerator's Current property will return.
        private enum EnumeratorType
        {
            // DictionaryEntry object. 
            DE,

            // Key object.
            KEY,

            // Value object.
            VALUE
        }

        private class KeyCollection : ICollection
        {
            Hashtable ht;

            public KeyCollection(Hashtable hashtable)
            {
                ht = hashtable;
            }

            #region ICollection Members

            public int Count
            {
                get
                {
                    return ht._count;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return ht.IsSynchronized;
                }
            }

            public object SyncRoot
            {
                get
                {
                    return ht.SyncRoot;
                }
            }

            public void CopyTo(Array array, int index)
            {
                ht.CopyToCollection(array, index, EnumeratorType.KEY);
            }

            #endregion

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return new HashtableEnumerator(ht, EnumeratorType.KEY);
            }

            #endregion
        }

        private class ValueCollection : ICollection
        {
            Hashtable ht;

            public ValueCollection(Hashtable hashtable)
            {
                ht = hashtable;
            }

            #region ICollection Members

            public int Count
            {
                get
                {
                    return ht._count;
                }
            }

            public bool IsSynchronized
            {
                get
                {
                    return ht.IsSynchronized;
                }
            }

            public object SyncRoot
            {
                get
                {
                    return ht.SyncRoot;
                }
            }

            public void CopyTo(Array array, int index)
            {
                ht.CopyToCollection(array, index, EnumeratorType.VALUE);
            }

            #endregion

            #region IEnumerable Members

            public IEnumerator GetEnumerator()
            {
                return new HashtableEnumerator(ht, EnumeratorType.VALUE);
            }

            #endregion
        }
    }

    internal static class HashHelpers
    {
        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369};

        public static bool IsPrime(int candidate)
        {
            if ((candidate & 1) != 0)
            {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2)
                {
                    if ((candidate % divisor) == 0)
                        return false;
                }
                return true;
            }
            return (candidate == 2);
        }

        public static int GetPrime(int min)
        {
            if (min < 0)
                throw new ArgumentException("HTCapacityOverflow");

            for (int i = 0; i < primes.Length; i++)
            {
                int prime = primes[i];
                if (prime >= min) return prime;
            }

            for (int i = (min | 1); i < Int32.MaxValue; i += 2)
            {
                if (IsPrime(i) && ((i - 1) % Hashtable.HashPrime != 0))
                    return i;
            }
            return min;
        }

        public static int GetMinPrime()
        {
            return primes[0];
        }

        // Returns size of hashtable to grow to.
        public static int ExpandPrime(int oldSize)
        {
            int newSize = 2 * oldSize;

            // Allow the hashtables to grow to maximum possible size (~2G elements) before encoutering capacity overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
            if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            {
                return MaxPrimeArrayLength;
            }

            return GetPrime(newSize);
        }

        // This is the maximum prime smaller than Array.MaxArrayLength
        public const int MaxPrimeArrayLength = 0x7FEFFFFD;
    }
}

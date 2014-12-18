////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace System.Collections
{
    using System;
    using System.Threading;
    [Serializable]
    public class Hashtable : IDictionary, ICloneable
    {
        internal const Int32 HashPrime = 101;
        private const Int32 InitialSize = 3;

        private struct bucket
        {
            public Object key;
            public Object val;
            public int hash_coll;
        }

        private bucket[] buckets;

        private int count;

        private int occupancy;

        private int loadsize;
        private float loadFactor;

        private volatile int version;
        private volatile bool isWriterInProgress;

        private ICollection keys;
        private ICollection values;

        private IEqualityComparer _keycomparer;
        private Object _syncRoot;

        protected IEqualityComparer EqualityComparer
        {
            get
            {
                return _keycomparer;
            }
        }

        internal Hashtable(bool trash)
        {
        }

        public Hashtable()
            : this(0, 1.0f)
        {
        }

        public Hashtable(int capacity)
            : this(capacity, 1.0f)
        {
        }

        public Hashtable(int capacity, float loadFactor)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException("capacity", "NeedNonNegNum");
            if (!(loadFactor >= 0.1f && loadFactor <= 1.0f))
                throw new ArgumentOutOfRangeException("loadFactor", "HashtableLoadFactor");

            
            this.loadFactor = 0.72f * loadFactor;

            double rawsize = capacity / this.loadFactor;
            if (rawsize > Int32.MaxValue)
                throw new ArgumentException("HTCapacityOverflow");

            
            int hashsize = (rawsize > InitialSize) ? HashHelpers.GetPrime((int)rawsize) : InitialSize;
            buckets = new bucket[hashsize];

            loadsize = (int)(this.loadFactor * hashsize);
            isWriterInProgress = false;
        }

        public Hashtable(int capacity, float loadFactor, IEqualityComparer equalityComparer)
            : this(capacity, loadFactor)
        {
            this._keycomparer = equalityComparer;
        }

        public Hashtable(IEqualityComparer equalityComparer)
            : this(0, 1.0f, equalityComparer)
        {
        }

        public Hashtable(int capacity, IEqualityComparer equalityComparer)
            : this(capacity, 1.0f, equalityComparer)
        {
        }

        private uint InitHash(Object key, int hashsize, out uint seed, out uint incr)
        {
            uint hashcode = (uint)GetHash(key) & 0x7FFFFFFF;
            seed = (uint)hashcode;
            incr = (uint)(1 + ((seed * HashPrime) % ((uint)hashsize - 1)));
            return hashcode;
        }

        public virtual void Add(Object key, Object value)
        {
            Insert(key, value, true);
        }

        public virtual void Clear()
        {
            if (count == 0 && occupancy == 0)
                return;

            isWriterInProgress = true;
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i].hash_coll = 0;
                buckets[i].key = null;
                buckets[i].val = null;
            }

            count = 0;
            occupancy = 0;
            UpdateVersion();
            isWriterInProgress = false;
        }

        public virtual Object Clone()
        {
            bucket[] lbuckets = buckets;
            Hashtable ht = new Hashtable(count, _keycomparer);
            ht.version = version;
            ht.loadFactor = loadFactor;
            ht.count = 0;

            int bucket = lbuckets.Length;
            while (bucket > 0)
            {
                bucket--;
                Object keyv = lbuckets[bucket].key;
                if ((keyv != null) && (keyv != lbuckets))
                {
                    ht[keyv] = lbuckets[bucket].val;
                }
            }

            return ht;
        }

        public virtual bool Contains(Object key)
        {
            return ContainsKey(key);
        }

        public virtual bool ContainsKey(Object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "Key");
            }

            uint seed;
            uint incr;
            
            bucket[] lbuckets = buckets;
            uint hashcode = InitHash(key, lbuckets.Length, out seed, out incr);
            int ntry = 0;

            bucket b;
            int bucketNumber = (int)(seed % (uint)lbuckets.Length);
            do
            {
                b = lbuckets[bucketNumber];
                if (b.key == null)
                {
                    return false;
                }
                if (((b.hash_coll & 0x7FFFFFFF) == hashcode) &&
                    KeyEquals(b.key, key))
                    return true;
                bucketNumber = (int)(((long)bucketNumber + incr) % (uint)lbuckets.Length);
            } while (b.hash_coll < 0 && ++ntry < lbuckets.Length);
            return false;
        }

        public virtual bool ContainsValue(Object value)
        {

            if (value == null)
            {
                for (int i = buckets.Length; --i >= 0; )
                {
                    if (buckets[i].key != null && buckets[i].key != buckets && buckets[i].val == null)
                        return true;
                }
            }
            else
            {
                for (int i = buckets.Length; --i >= 0; )
                {
                    Object val = buckets[i].val;
                    if (val != null && val.Equals(value)) return true;
                }
            }
            return false;
        }

        private void CopyKeys(Array array, int arrayIndex)
        {
            bucket[] lbuckets = buckets;
            for (int i = lbuckets.Length; --i >= 0; )
            {
                Object keyv = lbuckets[i].key;
                if ((keyv != null) && (keyv != buckets))
                {
                    array.SetValue(keyv, arrayIndex++);
                }
            }
        }

        private void CopyEntries(Array array, int arrayIndex)
        {
            bucket[] lbuckets = buckets;
            for (int i = lbuckets.Length; --i >= 0; )
            {
                Object keyv = lbuckets[i].key;
                if ((keyv != null) && (keyv != buckets))
                {
                    DictionaryEntry entry = new DictionaryEntry(keyv, lbuckets[i].val);
                    array.SetValue(entry, arrayIndex++);
                }
            }
        }

        public virtual void CopyTo(Array array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array", "Array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", "NeedNonNegNum");
            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("ArrayPlusOffTooSmall");
            CopyEntries(array, arrayIndex);
        }

        internal virtual KeyValuePairs[] ToKeyValuePairsArray()
        {
            KeyValuePairs[] array = new KeyValuePairs[count];
            int index = 0;
            bucket[] lbuckets = buckets;
            for (int i = lbuckets.Length; --i >= 0; )
            {
                Object keyv = lbuckets[i].key;
                if ((keyv != null) && (keyv != buckets))
                {
                    array[index++] = new KeyValuePairs(keyv, lbuckets[i].val);
                }
            }

            return array;
        }

        private void CopyValues(Array array, int arrayIndex)
        {
            bucket[] lbuckets = buckets;
            for (int i = lbuckets.Length; --i >= 0; )
            {
                Object keyv = lbuckets[i].key;
                if ((keyv != null) && (keyv != buckets))
                {
                    array.SetValue(lbuckets[i].val, arrayIndex++);
                }
            }
        }

        public virtual Object this[Object key]
        {
            get
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key", "Key");
                }

                uint seed;
                uint incr;
                
                bucket[] lbuckets = buckets;
                uint hashcode = InitHash(key, lbuckets.Length, out seed, out incr);
                int ntry = 0;

                bucket b;
                int bucketNumber = (int)(seed % (uint)lbuckets.Length);
                do
                {
                    int currentversion;

                    int spinCount = 0;
                    do
                    {
                        currentversion = version;
                        b = lbuckets[bucketNumber];

                        if ((++spinCount) % 8 == 0)
                        {
                            Thread.Sleep(1);   
                        }
                    } while (isWriterInProgress || (currentversion != version));

                    if (b.key == null)
                    {
                        return null;
                    }
                    if (((b.hash_coll & 0x7FFFFFFF) == hashcode) &&
                        KeyEquals(b.key, key))
                        return b.val;
                    bucketNumber = (int)(((long)bucketNumber + incr) % (uint)lbuckets.Length);
                } while (b.hash_coll < 0 && ++ntry < lbuckets.Length);
                return null;
            }

            set
            {
                Insert(key, value, false);
            }
        }

        private void expand()
        {
            int rawsize = HashHelpers.ExpandPrime(buckets.Length);
            rehash(rawsize, false);
        }

        
        private void rehash()
        {
            rehash(buckets.Length, false);
        }

        private void UpdateVersion()
        {
            
            
            version++;
        }

        private void rehash(int newsize, bool forceNewHashCode)
        {

            
            occupancy = 0;

            
            
            
            
            
            
            bucket[] newBuckets = new bucket[newsize];

            
            int nb;
            for (nb = 0; nb < buckets.Length; nb++)
            {
                bucket oldb = buckets[nb];
                if ((oldb.key != null) && (oldb.key != buckets))
                {
                    int hashcode = ((forceNewHashCode ? GetHash(oldb.key) : oldb.hash_coll) & 0x7FFFFFFF);
                    putEntry(newBuckets, oldb.key, oldb.val, hashcode);
                }
            }

            
            isWriterInProgress = true;
            buckets = newBuckets;
            loadsize = (int)(loadFactor * newsize);
            UpdateVersion();
            isWriterInProgress = false;
        }

        
        
        
        
        //
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new HashtableEnumerator(this, HashtableEnumerator.DictEntry);
        }

        
        
        
        
        //
        public virtual IDictionaryEnumerator GetEnumerator()
        {
            return new HashtableEnumerator(this, HashtableEnumerator.DictEntry);
        }

        
        
        
        protected virtual int GetHash(Object key)
        {
            if (_keycomparer != null)
                return _keycomparer.GetHashCode(key);
            return key.GetHashCode();
        }

        
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        public virtual bool IsFixedSize
        {
            get { return false; }
        }

        
        public virtual bool IsSynchronized
        {
            get { return false; }
        }

        
        
        
        
        protected virtual bool KeyEquals(Object item, Object key)
        {
            if (Object.ReferenceEquals(buckets, item))
            {
                return false;
            }

            if (Object.ReferenceEquals(item, key))
                return true;

            if (_keycomparer != null)
                return _keycomparer.Equals(item, key);
            return item == null ? false : item.Equals(key);
        }

        
        
        
        
        
        
        
        
        
        public virtual ICollection Keys
        {
            get
            {
                if (keys == null) keys = new KeyCollection(this);
                return keys;
            }
        }

        
        
        
        
        
        
        
        
        
        
        public virtual ICollection Values
        {
            get
            {
                if (values == null) values = new ValueCollection(this);
                return values;
            }
        }

        
        
        
        private void Insert(Object key, Object nvalue, bool add)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "Key");
            }
            if (count >= loadsize)
            {
                expand();
            }
            else if (occupancy > loadsize && count > 100)
            {
                rehash();
            }

            uint seed;
            uint incr;
            
            
            uint hashcode = InitHash(key, buckets.Length, out seed, out incr);
            int ntry = 0;
            int emptySlotNumber = -1; 
            
            int bucketNumber = (int)(seed % (uint)buckets.Length);
            do
            {

                
                
                
                
                if (emptySlotNumber == -1 && (buckets[bucketNumber].key == buckets) && (buckets[bucketNumber].hash_coll < 0))//(((buckets[bucketNumber].hash_coll & unchecked(0x80000000))!=0)))
                    emptySlotNumber = bucketNumber;

                
                
                
                if ((buckets[bucketNumber].key == null) ||
                    (buckets[bucketNumber].key == buckets && ((buckets[bucketNumber].hash_coll & unchecked(0x80000000)) == 0)))
                {

                    
                    
                    if (emptySlotNumber != -1) 
                        bucketNumber = emptySlotNumber;

                    
                    
                    isWriterInProgress = true;
                    buckets[bucketNumber].val = nvalue;
                    buckets[bucketNumber].key = key;
                    buckets[bucketNumber].hash_coll |= (int)hashcode;
                    count++;
                    UpdateVersion();
                    isWriterInProgress = false;

                    return;
                }

                
                
                
                if (((buckets[bucketNumber].hash_coll & 0x7FFFFFFF) == hashcode) &&
                    KeyEquals(buckets[bucketNumber].key, key))
                {
                    if (add)
                    {
                        throw new ArgumentException("AddingDuplicate");
                    }
                    isWriterInProgress = true;
                    buckets[bucketNumber].val = nvalue;
                    UpdateVersion();
                    isWriterInProgress = false;

                    return;
                }

                
                
                
                if (emptySlotNumber == -1)
                {
                    if (buckets[bucketNumber].hash_coll >= 0)
                    {
                        buckets[bucketNumber].hash_coll |= unchecked((int)0x80000000);
                        occupancy++;
                    }
                }

                bucketNumber = (int)(((long)bucketNumber + incr) % (uint)buckets.Length);
            } while (++ntry < buckets.Length);

            
            if (emptySlotNumber != -1)
            {
                
                
                isWriterInProgress = true;
                buckets[emptySlotNumber].val = nvalue;
                buckets[emptySlotNumber].key = key;
                buckets[emptySlotNumber].hash_coll |= (int)hashcode;
                count++;
                UpdateVersion();
                isWriterInProgress = false;

                return;
            }

            throw new InvalidOperationException("HashInsertFailed");
        }

        private void putEntry(bucket[] newBuckets, Object key, Object nvalue, int hashcode)
        {
            uint seed = (uint)hashcode;
            uint incr = (uint)(1 + ((seed * HashPrime) % ((uint)newBuckets.Length - 1)));
            int bucketNumber = (int)(seed % (uint)newBuckets.Length);
            do
            {

                if ((newBuckets[bucketNumber].key == null) || (newBuckets[bucketNumber].key == buckets))
                {
                    newBuckets[bucketNumber].val = nvalue;
                    newBuckets[bucketNumber].key = key;
                    newBuckets[bucketNumber].hash_coll |= hashcode;
                    return;
                }

                if (newBuckets[bucketNumber].hash_coll >= 0)
                {
                    newBuckets[bucketNumber].hash_coll |= unchecked((int)0x80000000);
                    occupancy++;
                }
                bucketNumber = (int)(((long)bucketNumber + incr) % (uint)newBuckets.Length);
            } while (true);
        }

        
        
        
        
        public virtual void Remove(Object key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key", "Key");
            }

            uint seed;
            uint incr;
            
            uint hashcode = InitHash(key, buckets.Length, out seed, out incr);
            int ntry = 0;

            bucket b;
            int bn = (int)(seed % (uint)buckets.Length);  
            do
            {
                b = buckets[bn];
                if (((b.hash_coll & 0x7FFFFFFF) == hashcode) &&
                    KeyEquals(b.key, key))
                {
                    isWriterInProgress = true;
                    
                    buckets[bn].hash_coll &= unchecked((int)0x80000000);
                    if (buckets[bn].hash_coll != 0)
                    {
                        buckets[bn].key = buckets;
                    }
                    else
                    {
                        buckets[bn].key = null;
                    }
                    buckets[bn].val = null;  
                    count--;
                    UpdateVersion();
                    isWriterInProgress = false;
                    return;
                }
                bn = (int)(((long)bn + incr) % (uint)buckets.Length);
            } while (b.hash_coll < 0 && ++ntry < buckets.Length);

            //throw new ArgumentException("RemoveArgNotFound");
        }

        
        public virtual Object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
                }
                return _syncRoot;
            }
        }

        
        
        public virtual int Count
        {
            get { return count; }
        }

        
        //
        public static Hashtable Synchronized(Hashtable table)
        {
            if (table == null)
                throw new ArgumentNullException("table");
            return new SyncHashtable(table);
        }

        //
        
        //

        
        
        [Serializable]
        private class KeyCollection : ICollection
        {
            private Hashtable _hashtable;

            internal KeyCollection(Hashtable hashtable)
            {
                _hashtable = hashtable;
            }

            public virtual void CopyTo(Array array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException("array");
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException("arrayIndex", "NeedNonNegNum");
                if (array.Length - arrayIndex < _hashtable.count)
                    throw new ArgumentException("ArrayPlusOffTooSmall");
                _hashtable.CopyKeys(array, arrayIndex);
            }

            public virtual IEnumerator GetEnumerator()
            {
                return new HashtableEnumerator(_hashtable, HashtableEnumerator.Keys);
            }

            public virtual bool IsSynchronized
            {
                get { return _hashtable.IsSynchronized; }
            }

            public virtual Object SyncRoot
            {
                get { return _hashtable.SyncRoot; }
            }

            public virtual int Count
            {
                get { return _hashtable.count; }
            }
        }

        
        
        [Serializable]
        private class ValueCollection : ICollection
        {
            private Hashtable _hashtable;

            internal ValueCollection(Hashtable hashtable)
            {
                _hashtable = hashtable;
            }

            public virtual void CopyTo(Array array, int arrayIndex)
            {
                if (array == null)
                    throw new ArgumentNullException("array");
                if (arrayIndex < 0)
                    throw new ArgumentOutOfRangeException("arrayIndex", "NeedNonNegNum");
                if (array.Length - arrayIndex < _hashtable.count)
                    throw new ArgumentException("ArrayPlusOffTooSmall");
                _hashtable.CopyValues(array, arrayIndex);
            }

            public virtual IEnumerator GetEnumerator()
            {
                return new HashtableEnumerator(_hashtable, HashtableEnumerator.Values);
            }

            public virtual bool IsSynchronized
            {
                get { return _hashtable.IsSynchronized; }
            }

            public virtual Object SyncRoot
            {
                get { return _hashtable.SyncRoot; }
            }

            public virtual int Count
            {
                get { return _hashtable.count; }
            }
        }

        
        [Serializable]
        private class SyncHashtable : Hashtable, IEnumerable
        {
            protected Hashtable _table;

            internal SyncHashtable(Hashtable table)
                : base(false)
            {
                _table = table;
            }

            public override int Count
            {
                get { return _table.Count; }
            }

            public override bool IsReadOnly
            {
                get { return _table.IsReadOnly; }
            }

            public override bool IsFixedSize
            {
                get { return _table.IsFixedSize; }
            }

            public override bool IsSynchronized
            {
                get { return true; }
            }

            public override Object this[Object key]
            {
                get
                {
                    return _table[key];
                }
                set
                {
                    lock (_table.SyncRoot)
                    {
                        _table[key] = value;
                    }
                }
            }

            public override Object SyncRoot
            {
                get { return _table.SyncRoot; }
            }

            public override void Add(Object key, Object value)
            {
                lock (_table.SyncRoot)
                {
                    _table.Add(key, value);
                }
            }

            public override void Clear()
            {
                lock (_table.SyncRoot)
                {
                    _table.Clear();
                }
            }

            public override bool Contains(Object key)
            {
                return _table.Contains(key);
            }

            public override bool ContainsKey(Object key)
            {
                if (key == null)
                {
                    throw new ArgumentNullException("key", "Key");
                }
                return _table.ContainsKey(key);
            }

            public override bool ContainsValue(Object key)
            {
                lock (_table.SyncRoot)
                {
                    return _table.ContainsValue(key);
                }
            }

            public override void CopyTo(Array array, int arrayIndex)
            {
                lock (_table.SyncRoot)
                {
                    _table.CopyTo(array, arrayIndex);
                }
            }

            public override Object Clone()
            {
                lock (_table.SyncRoot)
                {
                    return Hashtable.Synchronized((Hashtable)_table.Clone());
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return _table.GetEnumerator();
            }

            public override IDictionaryEnumerator GetEnumerator()
            {
                return _table.GetEnumerator();
            }

            public override ICollection Keys
            {
                get
                {
                    lock (_table.SyncRoot)
                    {
                        return _table.Keys;
                    }
                }
            }

            public override ICollection Values
            {
                get
                {
                    lock (_table.SyncRoot)
                    {
                        return _table.Values;
                    }
                }
            }

            public override void Remove(Object key)
            {
                lock (_table.SyncRoot)
                {
                    _table.Remove(key);
                }
            }

            internal override KeyValuePairs[] ToKeyValuePairsArray()
            {
                return _table.ToKeyValuePairsArray();
            }

        }

        [Serializable]
        private class HashtableEnumerator : IDictionaryEnumerator, ICloneable
        {
            private Hashtable hashtable;
            private int bucket;
            private int version;
            private bool current;
            private int getObjectRetType;   
            private Object currentKey;
            private Object currentValue;

            internal const int Keys = 1;
            internal const int Values = 2;
            internal const int DictEntry = 3;

            internal HashtableEnumerator(Hashtable hashtable, int getObjRetType)
            {
                this.hashtable = hashtable;
                bucket = hashtable.buckets.Length;
                version = hashtable.version;
                current = false;
                getObjectRetType = getObjRetType;
            }

            public Object Clone()
            {
                return MemberwiseClone();
            }

            public virtual Object Key
            {
                get
                {
                    if (current == false) throw new InvalidOperationException("EnumNotStarted");
                    return currentKey;
                }
            }

            public virtual bool MoveNext()
            {
                if (version != hashtable.version) throw new InvalidOperationException("EnumFailedVersion");
                while (bucket > 0)
                {
                    bucket--;
                    Object keyv = hashtable.buckets[bucket].key;
                    if ((keyv != null) && (keyv != hashtable.buckets))
                    {
                        currentKey = keyv;
                        currentValue = hashtable.buckets[bucket].val;
                        current = true;
                        return true;
                    }
                }
                current = false;
                return false;
            }

            public virtual DictionaryEntry Entry
            {
                get
                {
                    if (current == false) throw new InvalidOperationException("EnumOpCantHappen");
                    return new DictionaryEntry(currentKey, currentValue);
                }
            }


            public virtual Object Current
            {
                get
                {
                    if (current == false) throw new InvalidOperationException("EnumOpCantHappen");

                    if (getObjectRetType == Keys)
                        return currentKey;
                    else if (getObjectRetType == Values)
                        return currentValue;
                    else
                        return new DictionaryEntry(currentKey, currentValue);
                }
            }

            public virtual Object Value
            {
                get
                {
                    if (current == false) throw new InvalidOperationException("EnumOpCantHappen");
                    return currentValue;
                }
            }

            public virtual void Reset()
            {
                if (version != hashtable.version) throw new InvalidOperationException("EnumFailedVersion");
                current = false;
                bucket = hashtable.buckets.Length;
                currentKey = null;
                currentValue = null;
            }
        }

        
        internal class HashtableDebugView
        {
            private Hashtable hashtable;

            public HashtableDebugView(Hashtable hashtable)
            {
                if (hashtable == null)
                {
                    throw new ArgumentNullException("hashtable");
                }

                this.hashtable = hashtable;
            }

            public KeyValuePairs[] Items
            {
                get
                {
                    return hashtable.ToKeyValuePairsArray();
                }
            }
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

        
        public static int ExpandPrime(int oldSize)
        {
            int newSize = 2 * oldSize;

            if ((uint)newSize > MaxPrimeArrayLength && MaxPrimeArrayLength > oldSize)
            {
                return MaxPrimeArrayLength;
            }

            return GetPrime(newSize);
        }

        
        public const int MaxPrimeArrayLength = 0x7FEFFFFD;
    }
}

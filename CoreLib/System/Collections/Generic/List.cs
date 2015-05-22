////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System.Collections.Generic
{
    using System;

    [Serializable]
    public class List<T> : IList<T>, System.Collections.IList
    {
        private const int _defaultCapacity = 4;

        private T[] _items;
        private int _size;
        private int _version;
        [NonSerialized]
        private Object _syncRoot;

        static readonly T[] _emptyArray = new T[0];

        public List()
        {
            _items = _emptyArray;
        }

        public List(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException("NeedNonNegNum");

            if (capacity == 0)
                _items = _emptyArray;
            else
                _items = new T[capacity];
        }

        public List(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            ICollection<T> c = collection as ICollection<T>;
            if (c != null)
            {
                int count = c.Count;
                if (count == 0)
                {
                    _items = _emptyArray;
                }
                else
                {
                    _items = new T[count];
                    c.CopyTo(_items, 0);
                    _size = count;
                }
            }
            else
            {
                _size = 0;
                _items = _emptyArray;
                // This enumerable could be empty.  Let Add allocate a new array, if needed.
                // Note it will also go to _defaultCapacity first, not 1, then 2, etc.

                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Add(en.Current);
                    }
                }
            }
        }

        // Gets and sets the capacity of this list.  The capacity is the size of
        // the internal array used to hold items.  When set, the internal 
        // array of the list is reallocated to the given capacity.
        // 
        public int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value < _size)
                {
                    throw new ArgumentOutOfRangeException("_size");
                }

                if (value != _items.Length)
                {
                    if (value > 0)
                    {
                        T[] newItems = new T[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = _emptyArray;
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                return _size;
            }
        }

        bool System.Collections.IList.IsFixedSize
        {
            get { return false; }
        }


        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        bool System.Collections.IList.IsReadOnly
        {
            get { return false; }
        }

        bool System.Collections.ICollection.IsSynchronized
        {
            get { return false; }
        }

        Object System.Collections.ICollection.SyncRoot
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
        
        public T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)_size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                return _items[index];
            }

            set
            {
                if ((uint)index >= (uint)_size)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _items[index] = value;
                _version++;
            }
        }

        private static bool IsCompatibleObject(object value)
        {
            return ((value is T) || (value == null && default(T) == null));
        }

        Object System.Collections.IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        public void Add(T item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size++] = item;
            _version++;
        }

        int System.Collections.IList.Add(Object item)
        {
            Add((T)item);
            return Count - 1;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            InsertRange(_size, collection);
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("NeedNonNegNum");
            if (count < 0)
                throw new ArgumentOutOfRangeException("NeedNonNegNum");
            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");

            return Array.BinarySearch<T>(_items, index, count, item, comparer);
        }

        public int BinarySearch(T item)
        {
            return BinarySearch(0, Count, item, null);
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return BinarySearch(0, Count, item, comparer);
        }

        public void Clear()
        {
            if (_size > 0)
            {
                Array.Clear(_items, 0, _size); 
                _size = 0;
            }
            _version++;
        }

        public bool Contains(T item)
        {
            if ((Object)item == null)
            {
                for (int i = 0; i < _size; i++)
                    if ((Object)_items[i] == null)
                        return true;
                return false;
            }
            else
            {
                EqualityComparer<T> c = EqualityComparer<T>.Default;
                for (int i = 0; i < _size; i++)
                {
                    if (c.Equals(_items[i], item)) return true;
                }
                return false;
            }
        }

        bool System.Collections.IList.Contains(Object item)
        {
            if (IsCompatibleObject(item))
            {
                return Contains((T)item);
            }
            return false;
        }

        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException();
            }

            List<TOutput> list = new List<TOutput>(_size);
            for (int i = 0; i < _size; i++)
            {
                list._items[i] = converter(_items[i]);
            }
            list._size = _size;
            return list;
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        void System.Collections.ICollection.CopyTo(Array array, int arrayIndex)
        {
            if (array != null)
            {
                throw new ArgumentException("RankMultiDimNotSupported");
            }

            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (_size - index < count)
            {
                throw new ArgumentException("InvalidOffLen");
            }

            Array.Copy(_items, index, array, arrayIndex, count);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                // Allow the list to grow to maximum possible capacity (~2G elements) before encountering overflow.
                // Note that this check works even when _items.Length overflowed thanks to the (uint) cast
                if ((uint)newCapacity > int.MaxValue) newCapacity = int.MaxValue;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }

        public void ForEach(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException();
            }

            int version = _version;

            for (int i = 0; i < _size; i++)
            {
                if (version != _version)
                {
                    break;
                }

                action(_items[i]);
            }

            if (version != _version)
                throw new InvalidOperationException("EnumFailedVersion");
        }

        public List<T> FindAll(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            List<T> list = new List<T>();
            for (int i = 0; i < _size; i++)
            {
                if (match(_items[i]))
                {
                    list.Add(_items[i]);
                }
            }
            return list;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public List<T> GetRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("NeedNonNegNum");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("NeedNonNegNum");
            }

            if (_size - index < count)
            {
                throw new ArgumentException("InvalidOffLen");
            }

            List<T> list = new List<T>(count);
            Array.Copy(_items, index, list._items, 0, count);
            list._size = count;
            return list;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf(_items, item, 0, _size);
        }

        int System.Collections.IList.IndexOf(Object item)
        {
            if (IsCompatibleObject(item))
            {
                return IndexOf((T)item);
            }
            return -1;
        }

        public int IndexOf(T item, int index)
        {
            if (index > _size)
                throw new ArgumentOutOfRangeException("Index");
            return Array.IndexOf(_items, item, index, _size - index);
        }

        // Returns the index of the first occurrence of a given value in a range of
        // this list. The list is searched forwards, starting at index
        // index and upto count number of elements. The
        // elements of the list are compared to the given value using the
        // Object.Equals method.
        // 
        // This method uses the Array.IndexOf method to perform the
        // search.
        // 
        public int IndexOf(T item, int index, int count)
        {
            if (index > _size)
                throw new ArgumentOutOfRangeException("Index");

            if (count < 0 || index > _size - count) throw new ArgumentOutOfRangeException("Count");

            return Array.IndexOf(_items, item, index, count);
        }

        // Inserts an element into this list at a given index. The size of the list
        // is increased by one. If required, the capacity of the list is doubled
        // before inserting the new element.
        // 
        public void Insert(int index, T item)
        {
            // Note that insertions at the end are legal.
            if ((uint)index > (uint)_size)
            {
                throw new ArgumentOutOfRangeException("ListInsert");
            }

            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = item;
            _size++;
            _version++;
        }

        void System.Collections.IList.Insert(int index, Object item)
        {
            Insert(index, (T)item);
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            if ((uint)index > (uint)_size)
            {
                throw new ArgumentOutOfRangeException("Index");
            }

            ICollection<T> c = collection as ICollection<T>;
            if (c != null)
            {    // if collection is ICollection<T>
                int count = c.Count;
                if (count > 0)
                {
                    EnsureCapacity(_size + count);
                    if (index < _size)
                    {
                        Array.Copy(_items, index, _items, index + count, _size - index);
                    }

                    if (this == c)
                    {
                        Array.Copy(_items, 0, _items, index, index);
                        Array.Copy(_items, index + count, _items, index * 2, _size - index);
                    }
                    else
                    {
                        T[] itemsToInsert = new T[count];
                        c.CopyTo(itemsToInsert, 0);
                        itemsToInsert.CopyTo(_items, index);
                    }
                    _size += count;
                }
            }
            else
            {
                using (IEnumerator<T> en = collection.GetEnumerator())
                {
                    while (en.MoveNext())
                    {
                        Insert(index++, en.Current);
                    }
                }
            }
            _version++;
        }

        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        void System.Collections.IList.Remove(Object item)
        {
            if (IsCompatibleObject(item))
            {
                Remove((T)item);
            }
        }

        // This method removes all items which matches the predicate.
        // The complexity is O(n).   
        public int RemoveAll(Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException();
            }

            int freeIndex = 0;   // the first free slot in items array

            // Find the first item which needs to be removed.
            while (freeIndex < _size && !match(_items[freeIndex])) freeIndex++;
            if (freeIndex >= _size) return 0;

            int current = freeIndex + 1;
            while (current < _size)
            {
                // Find the first item which needs to be kept.
                while (current < _size && match(_items[current])) current++;

                if (current < _size)
                {
                    // copy item to the free slot.
                    _items[freeIndex++] = _items[current++];
                }
            }

            Array.Clear(_items, freeIndex, _size - freeIndex);
            int result = _size - freeIndex;
            _size = freeIndex;
            _version++;
            return result;
        }

        // Removes the element at the given index. The size of the list is
        // decreased by one.
        // 
        public void RemoveAt(int index)
        {
            if ((uint)index >= (uint)_size)
            {
                throw new ArgumentOutOfRangeException();
            }

            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = default(T);
            _version++;
        }

        // Removes a range of elements from this list.
        // 
        public void RemoveRange(int index, int count)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("NeedNonNegNum");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("NeedNonNegNum");
            }

            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");

            if (count > 0)
            {
                int i = _size;
                _size -= count;
                if (index < _size)
                {
                    Array.Copy(_items, index + count, _items, index, _size - index);
                }
                Array.Clear(_items, _size, count);
                _version++;
            }
        }

        public void Sort()
        {
            Sort(0, Count, null);
        }

        public void Sort(IComparer<T> comparer)
        {
            Sort(0, Count, comparer);
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("NeedNonNegNum");
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException("NeedNonNegNum");
            }

            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");

            Array.Sort<T>(_items, index, count, comparer);
            _version++;
        }

        public void Sort(Comparison<T> comparison)
        {
            if (comparison == null)
            {
                throw new ArgumentNullException();
            }

            if (_size > 0)
            {
                IComparer<T> comparer = new Array.FunctorComparer<T>(comparison);
                Array.Sort(_items, 0, _size, comparer);
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[_size];
            Array.Copy(_items, 0, array, 0, _size);
            return array;
        }

        public void TrimExcess()
        {
            int threshold = (int)(((double)_items.Length) * 0.9);
            if (_size < threshold)
            {
                Capacity = _size;
            }
        }

        internal static IList<T> Synchronized(List<T> list)
        {
            return new SynchronizedList(list);
        }

        [Serializable()]
        internal class SynchronizedList : IList<T>
        {
            private List<T> _list;
            private Object _root;

            internal SynchronizedList(List<T> list)
            {
                _list = list;
                _root = ((System.Collections.ICollection)list).SyncRoot;
            }

            public int Count
            {
                get
                {
                    lock (_root)
                    {
                        return _list.Count;
                    }
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return ((ICollection<T>)_list).IsReadOnly;
                }
            }

            public void Add(T item)
            {
                lock (_root)
                {
                    _list.Add(item);
                }
            }

            public void Clear()
            {
                lock (_root)
                {
                    _list.Clear();
                }
            }

            public bool Contains(T item)
            {
                lock (_root)
                {
                    return _list.Contains(item);
                }
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                lock (_root)
                {
                    _list.CopyTo(array, arrayIndex);
                }
            }

            public bool Remove(T item)
            {
                lock (_root)
                {
                    return _list.Remove(item);
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                lock (_root)
                {
                    return _list.GetEnumerator();
                }
            }

            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                lock (_root)
                {
                    return ((IEnumerable<T>)_list).GetEnumerator();
                }
            }

            public T this[int index]
            {
                get
                {
                    lock (_root)
                    {
                        return _list[index];
                    }
                }
                set
                {
                    lock (_root)
                    {
                        _list[index] = value;
                    }
                }
            }

            public int IndexOf(T item)
            {
                lock (_root)
                {
                    return _list.IndexOf(item);
                }
            }

            public void Insert(int index, T item)
            {
                lock (_root)
                {
                    _list.Insert(index, item);
                }
            }

            public void RemoveAt(int index)
            {
                lock (_root)
                {
                    _list.RemoveAt(index);
                }
            }
        }

        [Serializable]
        public struct Enumerator : IEnumerator<T>, System.Collections.IEnumerator
        {
            private List<T> list;
            private int index;
            private int version;
            private T current;

            internal Enumerator(List<T> list)
            {
                this.list = list;
                index = 0;
                version = list._version;
                current = default(T);
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {

                List<T> localList = list;

                if (version == localList._version && ((uint)index < (uint)localList._size))
                {
                    current = localList._items[index];
                    index++;
                    return true;
                }
                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                if (version != list._version)
                {
                    throw new InvalidOperationException("EnumFailedVersion");
                }

                index = list._size + 1;
                current = default(T);
                return false;
            }

            public T Current
            {
                get
                {
                    return current;
                }
            }

            Object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (index == 0 || index == list._size + 1)
                    {
                        throw new InvalidOperationException("EnumOpCantHappen");
                    }
                    return Current;
                }
            }

            void System.Collections.IEnumerator.Reset()
            {
                if (version != list._version)
                {
                    throw new InvalidOperationException("EnumFailedVersion");
                }

                index = 0;
                current = default(T);
            }

        }
    }
}


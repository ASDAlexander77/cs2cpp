////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System.Collections
{
    using System;

    [Serializable()]
    public class ArrayList : IList, ICloneable
    {
        private Object[] _items;
        private int _size;
        private int _version;
        [NonSerialized]
        private Object _syncRoot;

        private const int _defaultCapacity = 4;
        private static readonly Object[] emptyArray = new Object[0];

        
        
        internal ArrayList(bool trash)
        {
        }

        
        
        
        public ArrayList()
        {
            _items = emptyArray;
        }

        
        
        
        
        public ArrayList(int capacity)
        {
            if (capacity < 0) throw new ArgumentOutOfRangeException("capacity");
            _items = new Object[capacity];
        }

        
        
        
        
        public ArrayList(ICollection c)
        {
            if (c == null)
                throw new ArgumentNullException("c", "Collection");
            _items = new Object[c.Count];
            AddRange(c);
        }

        
        
        
        
        public virtual int Capacity
        {
            get { return _items.Length; }
            set
            {
                
                
                if (value != _items.Length)
                {
                    if (value < _size)
                    {
                        throw new ArgumentOutOfRangeException("value", "SmallCapacity");
                    }

                    if (value > 0)
                    {
                        Object[] newItems = new Object[value];
                        if (_size > 0)
                        {
                            Array.Copy(_items, 0, newItems, 0, _size);
                        }
                        _items = newItems;
                    }
                    else
                    {
                        _items = new Object[_defaultCapacity];
                    }
                }
            }
        }

        
        public virtual int Count
        {
            get { return _size; }
        }

        public virtual bool IsFixedSize
        {
            get { return false; }
        }


        
        public virtual bool IsReadOnly
        {
            get { return false; }
        }

        
        public virtual bool IsSynchronized
        {
            get { return false; }
        }

        
        public virtual Object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new Object(), null);
                }
                return _syncRoot;
            }
        }

        
        
        public virtual Object this[int index]
        {
            get
            {
                if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException("index", "Index");
                return _items[index];
            }
            set
            {
                if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException("index", "Index");
                _items[index] = value;
                _version++;
            }
        }

        
        
        
        
        
        
        
        //
        public static ArrayList Adapter(IList list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new IListWrapper(list);
        }

        
        
        
        //
        public virtual int Add(Object value)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size] = value;
            _version++;
            return _size++;
        }

        public virtual void AddRange(ICollection c)
        {
            InsertRange(_size, c);
        }
        
        public virtual int BinarySearch(int index, int count, Object value, IComparer comparer)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");

            return Array.BinarySearch((Array)_items, index, count, value, comparer);
        }

        public virtual int BinarySearch(Object value)
        {
            return BinarySearch(0, Count, value, null);
        }

        public virtual int BinarySearch(Object value, IComparer comparer)
        {
            return BinarySearch(0, Count, value, comparer);
        }


        
        public virtual void Clear()
        {
            Array.Clear(_items, 0, _size); 
            _size = 0;
            _version++;
        }

        
        
        
        public virtual Object Clone()
        {
            ArrayList la = new ArrayList(_size);
            la._size = _size;
            la._version = _version;
            Array.Copy(_items, 0, la._items, 0, _size);
            return la;
        }


        
        
        
        //
        public virtual bool Contains(Object item)
        {
            if (item == null)
            {
                for (int i = 0; i < _size; i++)
                    if (_items[i] == null)
                        return true;
                return false;
            }
            else
            {
                for (int i = 0; i < _size; i++)
                    if ((_items[i] != null) && (_items[i].Equals(item)))
                        return true;
                return false;
            }
        }

        
        
        //
        public virtual void CopyTo(Array array)
        {
            CopyTo(array, 0);
        }

        
        
        //
        public virtual void CopyTo(Array array, int arrayIndex)
        {
            if ((array != null))
                throw new ArgumentException("RankMultiDimNotSupported");
            
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        
        
        
        
        public virtual void CopyTo(int index, Array array, int arrayIndex, int count)
        {
            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");
            if ((array != null))
                throw new ArgumentException("RankMultiDimNotSupported");
            
            Array.Copy(_items, index, array, arrayIndex, count);
        }

        
        
        
        
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }

        
        
        //
        public static IList FixedSize(IList list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new FixedSizeList(list);
        }

        
        
        //
        public static ArrayList FixedSize(ArrayList list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new FixedSizeArrayList(list);
        }

        
        
        
        
        //
        public virtual IEnumerator GetEnumerator()
        {
            return new ArrayListEnumeratorSimple(this);
        }

        
        
        
        
        //
        public virtual IEnumerator GetEnumerator(int index, int count)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");

            return new ArrayListEnumerator(this, index, count);
        }

        
        
        
        
        
        
        
        
        public virtual int IndexOf(Object value)
        {
            return Array.IndexOf((Array)_items, value, 0, _size);
        }

        
        
        
        
        
        
        
        
        
        public virtual int IndexOf(Object value, int startIndex)
        {
            if (startIndex > _size)
                throw new ArgumentOutOfRangeException("startIndex", "Index");
            return Array.IndexOf((Array)_items, value, startIndex, _size - startIndex);
        }

        
        
        
        
        
        
        
        
        
        public virtual int IndexOf(Object value, int startIndex, int count)
        {
            if (startIndex > _size)
                throw new ArgumentOutOfRangeException("startIndex", "Index");
            if (count < 0 || startIndex > _size - count) throw new ArgumentOutOfRangeException("count", "Count");
            return Array.IndexOf((Array)_items, value, startIndex, count);
        }

        
        
        
        
        public virtual void Insert(int index, Object value)
        {
            
            if (index < 0 || index > _size) throw new ArgumentOutOfRangeException("index", "ArrayListInsert");
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = value;
            _size++;
            _version++;
        }

        
        
        
        
        //
        public virtual void InsertRange(int index, ICollection c)
        {
            if (c == null)
                throw new ArgumentNullException("c", "Collection");
            if (index < 0 || index > _size) throw new ArgumentOutOfRangeException("index", "Index");
            int count = c.Count;
            if (count > 0)
            {
                EnsureCapacity(_size + count);
                
                if (index < _size)
                {
                    Array.Copy(_items, index, _items, index + count, _size - index);
                }

                Object[] itemsToInsert = new Object[count];
                c.CopyTo(itemsToInsert, 0);
                itemsToInsert.CopyTo(_items, index);
                _size += count;
                _version++;
            }
        }

        public static IList ReadOnly(IList list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new ReadOnlyList(list);
        }

        public static ArrayList ReadOnly(ArrayList list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new ReadOnlyArrayList(list);
        }
        
        public virtual void Remove(Object obj)
        {
            int index = IndexOf(obj);
            if (index >= 0)
                RemoveAt(index);
        }

        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException("index", "Index");
            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = null;
            _version++;
        }

        
        
        public virtual void RemoveRange(int index, int count)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
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
                while (i > _size) _items[--i] = null;
                _version++;
            }
        }

        
        //
        public static ArrayList Repeat(Object value, int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");

            ArrayList list = new ArrayList((count > _defaultCapacity) ? count : _defaultCapacity);
            for (int i = 0; i < count; i++)
                list.Add(value);
            return list;
        }


        public virtual void SetRange(int index, ICollection c)
        {
            if (c == null) throw new ArgumentNullException("c", "Collection");
            int count = c.Count;
            if (index < 0 || index > _size - count) throw new ArgumentOutOfRangeException("index", "Index");

            if (count > 0)
            {
                c.CopyTo(_items, index);
                _version++;
            }
        }

        public virtual ArrayList GetRange(int index, int count)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");
            return new Range(this, index, count);
        }

        
        
        public virtual void Sort()
        {
            Sort(0, Count, Comparer.Default);
        }

        
        
        public virtual void Sort(IComparer comparer)
        {
            Sort(0, Count, comparer);
        }

        
        
        
        
        
        
        
        
        public virtual void Sort(int index, int count, IComparer comparer)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
            if (_size - index < count)
                throw new ArgumentException("InvalidOffLen");

            Array.Sort(_items, index, count, comparer);
            _version++;
        }

        
        public static IList Synchronized(IList list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new SyncIList(list);
        }

        
        public static ArrayList Synchronized(ArrayList list)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            return new SyncArrayList(list);
        }

        
        
        public virtual Object[] ToArray()
        {
            Object[] array = new Object[_size];
            Array.Copy(_items, 0, array, 0, _size);
            return array;
        }

        
        
        
        
        //
        public virtual Array ToArray(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            Array array = Array.CreateInstance(type, _size);
            Array.Copy(_items, 0, array, 0, _size);
            return array;
        }

        
        
        
        
        
        
        
        
        
        public virtual void TrimToSize()
        {
            Capacity = _size;
        }


        
        
        [Serializable()]
        private class IListWrapper : ArrayList
        {
            private IList _list;

            internal IListWrapper(IList list)
            {
                _list = list;
                _version = 0; 
            }

            public override int Capacity
            {
                get { return _list.Count; }
                set
                {
                    if (value < _list.Count) throw new ArgumentOutOfRangeException("value", "SmallCapacity");
                }
            }

            public override int Count
            {
                get { return _list.Count; }
            }

            public override bool IsReadOnly
            {
                get { return _list.IsReadOnly; }
            }

            public override bool IsFixedSize
            {
                get { return _list.IsFixedSize; }
            }


            public override bool IsSynchronized
            {
                get { return _list.IsSynchronized; }
            }

            public override Object this[int index]
            {
                get
                {
                    return _list[index];
                }
                set
                {
                    _list[index] = value;
                    _version++;
                }
            }

            public override Object SyncRoot
            {
                get { return _list.SyncRoot; }
            }

            public override int Add(Object obj)
            {
                int i = _list.Add(obj);
                _version++;
                return i;
            }

            public override void AddRange(ICollection c)
            {
                InsertRange(Count, c);
            }

            
            public override int BinarySearch(int index, int count, Object value, IComparer comparer)
            {
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_list.Count - index < count)
                    throw new ArgumentException("InvalidOffLen");
                if (comparer == null)
                    comparer = Comparer.Default;

                int lo = index;
                int hi = index + count - 1;
                int mid;
                while (lo <= hi)
                {
                    mid = (lo + hi) / 2;
                    int r = comparer.Compare(value, _list[mid]);
                    if (r == 0)
                        return mid;
                    if (r < 0)
                        hi = mid - 1;
                    else
                        lo = mid + 1;
                }
                
                
                return ~lo;
            }

            public override void Clear()
            {
                
                
                if (_list.IsFixedSize)
                {
                    throw new NotSupportedException("FixedSizeCollection");
                }

                _list.Clear();
                _version++;
            }

            public override Object Clone()
            {
                
                
                return new IListWrapper(_list);
            }

            public override bool Contains(Object obj)
            {
                return _list.Contains(obj);
            }

            public override void CopyTo(Array array, int index)
            {
                _list.CopyTo(array, index);
            }

            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                if (array == null)
                    throw new ArgumentNullException("array");
                if (index < 0 || arrayIndex < 0)
                    throw new ArgumentOutOfRangeException((index < 0) ? "index" : "arrayIndex", "NeedNonNegNum");
                if (count < 0)
                    throw new ArgumentOutOfRangeException("count", "NeedNonNegNum");
                if (array.Length - arrayIndex < count)
                    throw new ArgumentException("InvalidOffLen");
                if (_list.Count - index < count)
                    throw new ArgumentException("InvalidOffLen");

                for (int i = index; i < index + count; i++)
                    array.SetValue(_list[i], arrayIndex++);
            }

            public override IEnumerator GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            public override IEnumerator GetEnumerator(int index, int count)
            {
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_list.Count - index < count)
                    throw new ArgumentException("InvalidOffLen");

                return new IListWrapperEnumWrapper(this, index, count);
            }

            public override int IndexOf(Object value)
            {
                return _list.IndexOf(value);
            }

            public override int IndexOf(Object value, int startIndex)
            {
                return IndexOf(value, startIndex, _list.Count - startIndex);
            }

            public override int IndexOf(Object value, int startIndex, int count)
            {
                if (startIndex < 0 || startIndex > _list.Count) throw new ArgumentOutOfRangeException("startIndex", "Index");
                if (count < 0 || startIndex > _list.Count - count) throw new ArgumentOutOfRangeException("count", "Count");

                int endIndex = startIndex + count;
                if (value == null)
                {
                    for (int i = startIndex; i < endIndex; i++)
                        if (_list[i] == null)
                            return i;
                    return -1;
                }
                else
                {
                    for (int i = startIndex; i < endIndex; i++)
                        if (_list[i] != null && _list[i].Equals(value))
                            return i;
                    return -1;
                }
            }

            public override void Insert(int index, Object obj)
            {
                _list.Insert(index, obj);
                _version++;
            }

            public override void InsertRange(int index, ICollection c)
            {
                if (c == null)
                    throw new ArgumentNullException("c", "Collection");
                if (index < 0 || index > _list.Count) throw new ArgumentOutOfRangeException("index", "Index");

                if (c.Count > 0)
                {
                    ArrayList al = _list as ArrayList;
                    if (al != null)
                    {
                        
                        
                        
                        al.InsertRange(index, c);
                    }
                    else
                    {
                        IEnumerator en = c.GetEnumerator();
                        while (en.MoveNext())
                        {
                            _list.Insert(index++, en.Current);
                        }
                    }
                    _version++;
                }
            }

            public override void Remove(Object value)
            {
                int index = IndexOf(value);
                if (index >= 0)
                    RemoveAt(index);
            }

            public override void RemoveAt(int index)
            {
                _list.RemoveAt(index);
                _version++;
            }

            public override void RemoveRange(int index, int count)
            {
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_list.Count - index < count)
                    throw new ArgumentException("InvalidOffLen");

                if (count > 0)    
                    _version++;

                while (count > 0)
                {
                    _list.RemoveAt(index);
                    count--;
                }
            }

            public override void SetRange(int index, ICollection c)
            {
                if (c == null)
                {
                    throw new ArgumentNullException("c", "Collection");
                }

                if (index < 0 || index > _list.Count - c.Count)
                {
                    throw new ArgumentOutOfRangeException("index", "Index");
                }

                if (c.Count > 0)
                {
                    IEnumerator en = c.GetEnumerator();
                    while (en.MoveNext())
                    {
                        _list[index++] = en.Current;
                    }
                    _version++;
                }
            }

            public override ArrayList GetRange(int index, int count)
            {
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_list.Count - index < count)
                    throw new ArgumentException("InvalidOffLen");
                return new Range(this, index, count);
            }

            public override void Sort(int index, int count, IComparer comparer)
            {
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_list.Count - index < count)
                    throw new ArgumentException("InvalidOffLen");

                Object[] array = new Object[count];
                CopyTo(index, array, 0, count);
                Array.Sort(array, 0, count, comparer);
                for (int i = 0; i < count; i++)
                    _list[i + index] = array[i];

                _version++;
            }


            public override Object[] ToArray()
            {
                Object[] array = new Object[Count];
                _list.CopyTo(array, 0);
                return array;
            }

            public override Array ToArray(Type type)
            {
                if (type == null)
                    throw new ArgumentNullException("type");
                Array array = Array.CreateInstance(type, _list.Count);
                _list.CopyTo(array, 0);
                return array;
            }

            public override void TrimToSize()
            {
                
            }

            
            
            [Serializable()]
            private sealed class IListWrapperEnumWrapper : IEnumerator, ICloneable
            {
                private IEnumerator _en;
                private int _remaining;
                private int _initialStartIndex;   
                private int _initialCount;        
                private bool _firstCall;       

                private IListWrapperEnumWrapper()
                {
                }

                internal IListWrapperEnumWrapper(IListWrapper listWrapper, int startIndex, int count)
                {
                    _en = listWrapper.GetEnumerator();
                    _initialStartIndex = startIndex;
                    _initialCount = count;
                    while (startIndex-- > 0 && _en.MoveNext()) ;
                    _remaining = count;
                    _firstCall = true;
                }

                public Object Clone()
                {
                    
                    IListWrapperEnumWrapper clone = new IListWrapperEnumWrapper();
                    clone._en = (IEnumerator)((ICloneable)_en).Clone();
                    clone._initialStartIndex = _initialStartIndex;
                    clone._initialCount = _initialCount;
                    clone._remaining = _remaining;
                    clone._firstCall = _firstCall;
                    return clone;
                }

                public bool MoveNext()
                {
                    if (_firstCall)
                    {
                        _firstCall = false;
                        return _remaining-- > 0 && _en.MoveNext();
                    }
                    if (_remaining < 0)
                        return false;
                    bool r = _en.MoveNext();
                    return r && _remaining-- > 0;
                }

                public Object Current
                {
                    get
                    {
                        if (_firstCall)
                            throw new InvalidOperationException("EnumNotStarted");
                        if (_remaining < 0)
                            throw new InvalidOperationException("EnumEnded");
                        return _en.Current;
                    }
                }

                public void Reset()
                {
                    _en.Reset();
                    int startIndex = _initialStartIndex;
                    while (startIndex-- > 0 && _en.MoveNext()) ;
                    _remaining = _initialCount;
                    _firstCall = true;
                }
            }
        }


        [Serializable()]
        private class SyncArrayList : ArrayList
        {
            private ArrayList _list;
            private Object _root;

            internal SyncArrayList(ArrayList list)
                : base(false)
            {
                _list = list;
                _root = list.SyncRoot;
            }

            public override int Capacity
            {
                get
                {
                    lock (_root)
                    {
                        return _list.Capacity;
                    }
                }
                set
                {
                    lock (_root)
                    {
                        _list.Capacity = value;
                    }
                }
            }

            public override int Count
            {
                get { lock (_root) { return _list.Count; } }
            }

            public override bool IsReadOnly
            {
                get { return _list.IsReadOnly; }
            }

            public override bool IsFixedSize
            {
                get { return _list.IsFixedSize; }
            }


            public override bool IsSynchronized
            {
                get { return true; }
            }

            public override Object this[int index]
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

            public override Object SyncRoot
            {
                get { return _root; }
            }

            public override int Add(Object value)
            {
                lock (_root)
                {
                    return _list.Add(value);
                }
            }

            public override void AddRange(ICollection c)
            {
                lock (_root)
                {
                    _list.AddRange(c);
                }
            }

            public override int BinarySearch(Object value)
            {
                lock (_root)
                {
                    return _list.BinarySearch(value);
                }
            }

            public override int BinarySearch(Object value, IComparer comparer)
            {
                lock (_root)
                {
                    return _list.BinarySearch(value, comparer);
                }
            }

            public override int BinarySearch(int index, int count, Object value, IComparer comparer)
            {
                lock (_root)
                {
                    return _list.BinarySearch(index, count, value, comparer);
                }
            }

            public override void Clear()
            {
                lock (_root)
                {
                    _list.Clear();
                }
            }

            public override Object Clone()
            {
                lock (_root)
                {
                    return new SyncArrayList((ArrayList)_list.Clone());
                }
            }

            public override bool Contains(Object item)
            {
                lock (_root)
                {
                    return _list.Contains(item);
                }
            }

            public override void CopyTo(Array array)
            {
                lock (_root)
                {
                    _list.CopyTo(array);
                }
            }

            public override void CopyTo(Array array, int index)
            {
                lock (_root)
                {
                    _list.CopyTo(array, index);
                }
            }

            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                lock (_root)
                {
                    _list.CopyTo(index, array, arrayIndex, count);
                }
            }

            public override IEnumerator GetEnumerator()
            {
                lock (_root)
                {
                    return _list.GetEnumerator();
                }
            }

            public override IEnumerator GetEnumerator(int index, int count)
            {
                lock (_root)
                {
                    return _list.GetEnumerator(index, count);
                }
            }

            public override int IndexOf(Object value)
            {
                lock (_root)
                {
                    return _list.IndexOf(value);
                }
            }

            public override int IndexOf(Object value, int startIndex)
            {
                lock (_root)
                {
                    return _list.IndexOf(value, startIndex);
                }
            }

            public override int IndexOf(Object value, int startIndex, int count)
            {
                lock (_root)
                {
                    return _list.IndexOf(value, startIndex, count);
                }
            }

            public override void Insert(int index, Object value)
            {
                lock (_root)
                {
                    _list.Insert(index, value);
                }
            }

            public override void InsertRange(int index, ICollection c)
            {
                lock (_root)
                {
                    _list.InsertRange(index, c);
                }
            }

            public override void Remove(Object value)
            {
                lock (_root)
                {
                    _list.Remove(value);
                }
            }

            public override void RemoveAt(int index)
            {
                lock (_root)
                {
                    _list.RemoveAt(index);
                }
            }

            public override void RemoveRange(int index, int count)
            {
                lock (_root)
                {
                    _list.RemoveRange(index, count);
                }
            }

            public override void SetRange(int index, ICollection c)
            {
                lock (_root)
                {
                    _list.SetRange(index, c);
                }
            }

            public override ArrayList GetRange(int index, int count)
            {
                lock (_root)
                {
                    return _list.GetRange(index, count);
                }
            }

            public override void Sort()
            {
                lock (_root)
                {
                    _list.Sort();
                }
            }

            public override void Sort(IComparer comparer)
            {
                lock (_root)
                {
                    _list.Sort(comparer);
                }
            }

            public override void Sort(int index, int count, IComparer comparer)
            {
                lock (_root)
                {
                    _list.Sort(index, count, comparer);
                }
            }

            public override Object[] ToArray()
            {
                lock (_root)
                {
                    return _list.ToArray();
                }
            }

            public override Array ToArray(Type type)
            {
                lock (_root)
                {
                    return _list.ToArray(type);
                }
            }

            public override void TrimToSize()
            {
                lock (_root)
                {
                    _list.TrimToSize();
                }
            }
        }


        [Serializable()]
        private class SyncIList : IList
        {
            private IList _list;
            private Object _root;

            internal SyncIList(IList list)
            {
                _list = list;
                _root = list.SyncRoot;
            }

            public virtual int Count
            {
                get { lock (_root) { return _list.Count; } }
            }

            public virtual bool IsReadOnly
            {
                get { return _list.IsReadOnly; }
            }

            public virtual bool IsFixedSize
            {
                get { return _list.IsFixedSize; }
            }


            public virtual bool IsSynchronized
            {
                get { return true; }
            }

            public virtual Object this[int index]
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

            public virtual Object SyncRoot
            {
                get { return _root; }
            }

            public virtual int Add(Object value)
            {
                lock (_root)
                {
                    return _list.Add(value);
                }
            }


            public virtual void Clear()
            {
                lock (_root)
                {
                    _list.Clear();
                }
            }

            public virtual bool Contains(Object item)
            {
                lock (_root)
                {
                    return _list.Contains(item);
                }
            }

            public virtual void CopyTo(Array array, int index)
            {
                lock (_root)
                {
                    _list.CopyTo(array, index);
                }
            }

            public virtual IEnumerator GetEnumerator()
            {
                lock (_root)
                {
                    return _list.GetEnumerator();
                }
            }

            public virtual int IndexOf(Object value)
            {
                lock (_root)
                {
                    return _list.IndexOf(value);
                }
            }

            public virtual void Insert(int index, Object value)
            {
                lock (_root)
                {
                    _list.Insert(index, value);
                }
            }

            public virtual void Remove(Object value)
            {
                lock (_root)
                {
                    _list.Remove(value);
                }
            }

            public virtual void RemoveAt(int index)
            {
                lock (_root)
                {
                    _list.RemoveAt(index);
                }
            }
        }

        [Serializable()]
        private class FixedSizeList : IList
        {
            private IList _list;

            internal FixedSizeList(IList l)
            {
                _list = l;
            }

            public virtual int Count
            {
                get { return _list.Count; }
            }

            public virtual bool IsReadOnly
            {
                get { return _list.IsReadOnly; }
            }

            public virtual bool IsFixedSize
            {
                get { return true; }
            }

            public virtual bool IsSynchronized
            {
                get { return _list.IsSynchronized; }
            }

            public virtual Object this[int index]
            {
                get
                {
                    return _list[index];
                }
                set
                {
                    _list[index] = value;
                }
            }

            public virtual Object SyncRoot
            {
                get { return _list.SyncRoot; }
            }

            public virtual int Add(Object obj)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public virtual void Clear()
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public virtual bool Contains(Object obj)
            {
                return _list.Contains(obj);
            }

            public virtual void CopyTo(Array array, int index)
            {
                _list.CopyTo(array, index);
            }

            public virtual IEnumerator GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            public virtual int IndexOf(Object value)
            {
                return _list.IndexOf(value);
            }

            public virtual void Insert(int index, Object obj)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public virtual void Remove(Object value)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public virtual void RemoveAt(int index)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }
        }

        [Serializable()]
        private class FixedSizeArrayList : ArrayList
        {
            private ArrayList _list;

            internal FixedSizeArrayList(ArrayList l)
            {
                _list = l;
                _version = _list._version;
            }

            public override int Count
            {
                get { return _list.Count; }
            }

            public override bool IsReadOnly
            {
                get { return _list.IsReadOnly; }
            }

            public override bool IsFixedSize
            {
                get { return true; }
            }

            public override bool IsSynchronized
            {
                get { return _list.IsSynchronized; }
            }

            public override Object this[int index]
            {
                get
                {
                    return _list[index];
                }
                set
                {
                    _list[index] = value;
                    _version = _list._version;
                }
            }

            public override Object SyncRoot
            {
                get { return _list.SyncRoot; }
            }

            public override int Add(Object obj)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public override void AddRange(ICollection c)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public override int BinarySearch(int index, int count, Object value, IComparer comparer)
            {
                return _list.BinarySearch(index, count, value, comparer);
            }

            public override int Capacity
            {
                get { return _list.Capacity; }
                set { throw new NotSupportedException("FixedSizeCollection"); }
            }

            public override void Clear()
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public override Object Clone()
            {
                FixedSizeArrayList arrayList = new FixedSizeArrayList(_list);
                arrayList._list = (ArrayList)_list.Clone();
                return arrayList;
            }

            public override bool Contains(Object obj)
            {
                return _list.Contains(obj);
            }

            public override void CopyTo(Array array, int index)
            {
                _list.CopyTo(array, index);
            }

            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                _list.CopyTo(index, array, arrayIndex, count);
            }

            public override IEnumerator GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            public override IEnumerator GetEnumerator(int index, int count)
            {
                return _list.GetEnumerator(index, count);
            }

            public override int IndexOf(Object value)
            {
                return _list.IndexOf(value);
            }

            public override int IndexOf(Object value, int startIndex)
            {
                return _list.IndexOf(value, startIndex);
            }

            public override int IndexOf(Object value, int startIndex, int count)
            {
                return _list.IndexOf(value, startIndex, count);
            }

            public override void Insert(int index, Object obj)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public override void InsertRange(int index, ICollection c)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public override void Remove(Object value)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public override void RemoveAt(int index)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public override void RemoveRange(int index, int count)
            {
                throw new NotSupportedException("FixedSizeCollection");
            }

            public override void SetRange(int index, ICollection c)
            {
                _list.SetRange(index, c);
                _version = _list._version;
            }

            public override ArrayList GetRange(int index, int count)
            {
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (Count - index < count)
                    throw new ArgumentException("InvalidOffLen");
                return new Range(this, index, count);
            }

            public override void Sort(int index, int count, IComparer comparer)
            {
                _list.Sort(index, count, comparer);
                _version = _list._version;
            }

            public override Object[] ToArray()
            {
                return _list.ToArray();
            }

            public override Array ToArray(Type type)
            {
                return _list.ToArray(type);
            }

            public override void TrimToSize()
            {
                throw new NotSupportedException("FixedSizeCollection");
            }
        }

        [Serializable()]
        private class ReadOnlyList : IList
        {
            private IList _list;

            internal ReadOnlyList(IList l)
            {
                _list = l;
            }

            public virtual int Count
            {
                get { return _list.Count; }
            }

            public virtual bool IsReadOnly
            {
                get { return true; }
            }

            public virtual bool IsFixedSize
            {
                get { return true; }
            }

            public virtual bool IsSynchronized
            {
                get { return _list.IsSynchronized; }
            }

            public virtual Object this[int index]
            {
                get
                {
                    return _list[index];
                }
                set
                {
                    throw new NotSupportedException("ReadOnlyCollection");
                }
            }

            public virtual Object SyncRoot
            {
                get { return _list.SyncRoot; }
            }

            public virtual int Add(Object obj)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public virtual void Clear()
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public virtual bool Contains(Object obj)
            {
                return _list.Contains(obj);
            }

            public virtual void CopyTo(Array array, int index)
            {
                _list.CopyTo(array, index);
            }

            public virtual IEnumerator GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            public virtual int IndexOf(Object value)
            {
                return _list.IndexOf(value);
            }

            public virtual void Insert(int index, Object obj)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public virtual void Remove(Object value)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public virtual void RemoveAt(int index)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }
        }

        [Serializable()]
        private class ReadOnlyArrayList : ArrayList
        {
            private ArrayList _list;

            internal ReadOnlyArrayList(ArrayList l)
            {
                _list = l;
            }

            public override int Count
            {
                get { return _list.Count; }
            }

            public override bool IsReadOnly
            {
                get { return true; }
            }

            public override bool IsFixedSize
            {
                get { return true; }
            }

            public override bool IsSynchronized
            {
                get { return _list.IsSynchronized; }
            }

            public override Object this[int index]
            {
                get
                {
                    return _list[index];
                }
                set
                {
                    throw new NotSupportedException("ReadOnlyCollection");
                }
            }

            public override Object SyncRoot
            {
                get { return _list.SyncRoot; }
            }

            public override int Add(Object obj)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override void AddRange(ICollection c)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override int BinarySearch(int index, int count, Object value, IComparer comparer)
            {
                return _list.BinarySearch(index, count, value, comparer);
            }


            public override int Capacity
            {
                get { return _list.Capacity; }
                set { throw new NotSupportedException("ReadOnlyCollection"); }
            }

            public override void Clear()
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override Object Clone()
            {
                ReadOnlyArrayList arrayList = new ReadOnlyArrayList(_list);
                arrayList._list = (ArrayList)_list.Clone();
                return arrayList;
            }

            public override bool Contains(Object obj)
            {
                return _list.Contains(obj);
            }

            public override void CopyTo(Array array, int index)
            {
                _list.CopyTo(array, index);
            }

            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                _list.CopyTo(index, array, arrayIndex, count);
            }

            public override IEnumerator GetEnumerator()
            {
                return _list.GetEnumerator();
            }

            public override IEnumerator GetEnumerator(int index, int count)
            {
                return _list.GetEnumerator(index, count);
            }

            public override int IndexOf(Object value)
            {
                return _list.IndexOf(value);
            }

            public override int IndexOf(Object value, int startIndex)
            {
                return _list.IndexOf(value, startIndex);
            }

            public override int IndexOf(Object value, int startIndex, int count)
            {
                return _list.IndexOf(value, startIndex, count);
            }

            public override void Insert(int index, Object obj)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override void InsertRange(int index, ICollection c)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override void Remove(Object value)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override void RemoveAt(int index)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override void RemoveRange(int index, int count)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override void SetRange(int index, ICollection c)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override ArrayList GetRange(int index, int count)
            {
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (Count - index < count)
                    throw new ArgumentException("InvalidOffLen");
                return new Range(this, index, count);
            }

            public override void Sort(int index, int count, IComparer comparer)
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }

            public override Object[] ToArray()
            {
                return _list.ToArray();
            }

            public override Array ToArray(Type type)
            {
                return _list.ToArray(type);
            }

            public override void TrimToSize()
            {
                throw new NotSupportedException("ReadOnlyCollection");
            }
        }


        
        
        
        [Serializable()]
        private sealed class ArrayListEnumerator : IEnumerator, ICloneable
        {
            private ArrayList list;
            private int index;
            private int endIndex;       
            private int version;
            private Object currentElement;
            private int startIndex;     

            internal ArrayListEnumerator(ArrayList list, int index, int count)
            {
                this.list = list;
                startIndex = index;
                this.index = index - 1;
                endIndex = this.index + count;  
                version = list._version;
                currentElement = null;
            }

            public Object Clone()
            {
                return MemberwiseClone();
            }

            public bool MoveNext()
            {
                if (version != list._version) throw new InvalidOperationException("EnumFailedVersion");
                if (index < endIndex)
                {
                    currentElement = list[++index];
                    return true;
                }
                else
                {
                    index = endIndex + 1;
                }

                return false;
            }

            public Object Current
            {
                get
                {
                    if (index < startIndex)
                        throw new InvalidOperationException("EnumNotStarted");
                    else if (index > endIndex)
                    {
                        throw new InvalidOperationException("EnumEnded");
                    }
                    return currentElement;
                }
            }

            public void Reset()
            {
                if (version != list._version) throw new InvalidOperationException("EnumFailedVersion");
                index = startIndex - 1;
            }
        }

        
        
        [Serializable()]
        private class Range : ArrayList
        {
            private ArrayList _baseList;
            private int _baseIndex;
            private int _baseSize;
            private int _baseVersion;

            internal Range(ArrayList list, int index, int count)
                : base(false)
            {
                _baseList = list;
                _baseIndex = index;
                _baseSize = count;
                _baseVersion = list._version;
                
                _version = list._version;
            }

            private void InternalUpdateRange()
            {
                if (_baseVersion != _baseList._version)
                    throw new InvalidOperationException("UnderlyingArrayListChanged");
            }

            private void InternalUpdateVersion()
            {
                _baseVersion++;
                _version++;
            }

            public override int Add(Object value)
            {
                InternalUpdateRange();
                _baseList.Insert(_baseIndex + _baseSize, value);
                InternalUpdateVersion();
                return _baseSize++;
            }

            public override void AddRange(ICollection c)
            {
                InternalUpdateRange();
                if (c == null)
                {
                    throw new ArgumentNullException("c");
                }

                int count = c.Count;
                if (count > 0)
                {
                    _baseList.InsertRange(_baseIndex + _baseSize, c);
                    InternalUpdateVersion();
                    _baseSize += count;
                }
            }

            
            public override int BinarySearch(int index, int count, Object value, IComparer comparer)
            {
                InternalUpdateRange();
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_baseSize - index < count)
                    throw new ArgumentException("InvalidOffLen");
                int i = _baseList.BinarySearch(_baseIndex + index, count, value, comparer);
                if (i >= 0) return i - _baseIndex;
                return i + _baseIndex;
            }

            public override int Capacity
            {
                get
                {
                    return _baseList.Capacity;
                }

                set
                {
                    if (value < Count) throw new ArgumentOutOfRangeException("value", "SmallCapacity");
                }
            }


            public override void Clear()
            {
                InternalUpdateRange();
                if (_baseSize != 0)
                {
                    _baseList.RemoveRange(_baseIndex, _baseSize);
                    InternalUpdateVersion();
                    _baseSize = 0;
                }
            }

            public override Object Clone()
            {
                InternalUpdateRange();
                Range arrayList = new Range(_baseList, _baseIndex, _baseSize);
                arrayList._baseList = (ArrayList)_baseList.Clone();
                return arrayList;
            }

            public override bool Contains(Object item)
            {
                InternalUpdateRange();
                if (item == null)
                {
                    for (int i = 0; i < _baseSize; i++)
                        if (_baseList[_baseIndex + i] == null)
                            return true;
                    return false;
                }
                else
                {
                    for (int i = 0; i < _baseSize; i++)
                        if (_baseList[_baseIndex + i] != null && _baseList[_baseIndex + i].Equals(item))
                            return true;
                    return false;
                }
            }

            public override void CopyTo(Array array, int index)
            {
                InternalUpdateRange();
                if (array == null)
                    throw new ArgumentNullException("array");
                if (index < 0)
                    throw new ArgumentOutOfRangeException("index", "NeedNonNegNum");
                if (array.Length - index < _baseSize)
                    throw new ArgumentException("InvalidOffLen");
                _baseList.CopyTo(_baseIndex, array, index, _baseSize);
            }

            public override void CopyTo(int index, Array array, int arrayIndex, int count)
            {
                InternalUpdateRange();
                if (array == null)
                    throw new ArgumentNullException("array");
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (array.Length - arrayIndex < count)
                    throw new ArgumentException("InvalidOffLen");
                if (_baseSize - index < count)
                    throw new ArgumentException("InvalidOffLen");
                _baseList.CopyTo(_baseIndex + index, array, arrayIndex, count);
            }

            public override int Count
            {
                get
                {
                    InternalUpdateRange();
                    return _baseSize;
                }
            }

            public override bool IsReadOnly
            {
                get { return _baseList.IsReadOnly; }
            }

            public override bool IsFixedSize
            {
                get { return _baseList.IsFixedSize; }
            }

            public override bool IsSynchronized
            {
                get { return _baseList.IsSynchronized; }
            }

            public override IEnumerator GetEnumerator()
            {
                return GetEnumerator(0, _baseSize);
            }

            public override IEnumerator GetEnumerator(int index, int count)
            {
                InternalUpdateRange();
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_baseSize - index < count)
                    throw new ArgumentException("InvalidOffLen");
                return _baseList.GetEnumerator(_baseIndex + index, count);
            }

            public override ArrayList GetRange(int index, int count)
            {
                InternalUpdateRange();
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_baseSize - index < count)
                    throw new ArgumentException("InvalidOffLen");
                return new Range(this, index, count);
            }

            public override Object SyncRoot
            {
                get
                {
                    return _baseList.SyncRoot;
                }
            }


            public override int IndexOf(Object value)
            {
                InternalUpdateRange();
                int i = _baseList.IndexOf(value, _baseIndex, _baseSize);
                if (i >= 0) return i - _baseIndex;
                return -1;
            }

            public override int IndexOf(Object value, int startIndex)
            {
                InternalUpdateRange();
                if (startIndex < 0)
                    throw new ArgumentOutOfRangeException("startIndex", "NeedNonNegNum");
                if (startIndex > _baseSize)
                    throw new ArgumentOutOfRangeException("startIndex", "Index");

                int i = _baseList.IndexOf(value, _baseIndex + startIndex, _baseSize - startIndex);
                if (i >= 0) return i - _baseIndex;
                return -1;
            }

            public override int IndexOf(Object value, int startIndex, int count)
            {
                InternalUpdateRange();
                if (startIndex < 0 || startIndex > _baseSize)
                    throw new ArgumentOutOfRangeException("startIndex", "Index");

                if (count < 0 || (startIndex > _baseSize - count))
                    throw new ArgumentOutOfRangeException("count", "Count");

                int i = _baseList.IndexOf(value, _baseIndex + startIndex, count);
                if (i >= 0) return i - _baseIndex;
                return -1;
            }

            public override void Insert(int index, Object value)
            {
                InternalUpdateRange();
                if (index < 0 || index > _baseSize) throw new ArgumentOutOfRangeException("index", "Index");
                _baseList.Insert(_baseIndex + index, value);
                InternalUpdateVersion();
                _baseSize++;
            }

            public override void InsertRange(int index, ICollection c)
            {
                InternalUpdateRange();
                if (index < 0 || index > _baseSize) throw new ArgumentOutOfRangeException("index", "Index");

                if (c == null)
                {
                    throw new ArgumentNullException("c");

                }
                int count = c.Count;
                if (count > 0)
                {
                    _baseList.InsertRange(_baseIndex + index, c);
                    _baseSize += count;
                    InternalUpdateVersion();
                }
            }

            public override void RemoveAt(int index)
            {
                InternalUpdateRange();
                if (index < 0 || index >= _baseSize) throw new ArgumentOutOfRangeException("index", "Index");
                _baseList.RemoveAt(_baseIndex + index);
                InternalUpdateVersion();
                _baseSize--;
            }

            public override void RemoveRange(int index, int count)
            {
                InternalUpdateRange();
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_baseSize - index < count)
                    throw new ArgumentException("InvalidOffLen");
                
                
                if (count > 0)
                {
                    _baseList.RemoveRange(_baseIndex + index, count);
                    InternalUpdateVersion();
                    _baseSize -= count;
                }
            }

            public override void SetRange(int index, ICollection c)
            {
                InternalUpdateRange();
                if (index < 0 || index >= _baseSize) throw new ArgumentOutOfRangeException("index", "Index");
                _baseList.SetRange(_baseIndex + index, c);
                if (c.Count > 0)
                {
                    InternalUpdateVersion();
                }
            }

            public override void Sort(int index, int count, IComparer comparer)
            {
                InternalUpdateRange();
                if (index < 0 || count < 0)
                    throw new ArgumentOutOfRangeException((index < 0 ? "index" : "count"), "NeedNonNegNum");
                if (_baseSize - index < count)
                    throw new ArgumentException("InvalidOffLen");
                _baseList.Sort(_baseIndex + index, count, comparer);
                InternalUpdateVersion();
            }

            public override Object this[int index]
            {
                get
                {
                    InternalUpdateRange();
                    if (index < 0 || index >= _baseSize) throw new ArgumentOutOfRangeException("index", "Index");
                    return _baseList[_baseIndex + index];
                }
                set
                {
                    InternalUpdateRange();
                    if (index < 0 || index >= _baseSize) throw new ArgumentOutOfRangeException("index", "Index");
                    _baseList[_baseIndex + index] = value;
                    InternalUpdateVersion();
                }
            }

            public override Object[] ToArray()
            {
                InternalUpdateRange();
                Object[] array = new Object[_baseSize];
                Array.Copy(_baseList._items, _baseIndex, array, 0, _baseSize);
                return array;
            }

            public override Array ToArray(Type type)
            {
                InternalUpdateRange();
                if (type == null)
                    throw new ArgumentNullException("type");
                Array array = Array.CreateInstance(type, _baseSize);
                _baseList.CopyTo(_baseIndex, array, 0, _baseSize);
                return array;
            }

            public override void TrimToSize()
            {
                throw new NotSupportedException("RangeCollection");
            }
        }

        [Serializable()]
        private sealed class ArrayListEnumeratorSimple : IEnumerator, ICloneable
        {
            private ArrayList list;
            private int index;
            private int version;
            private Object currentElement;
            [NonSerializedAttribute]
            private bool isArrayList;
            
            static Object dummyObject = new Object();

            internal ArrayListEnumeratorSimple(ArrayList list)
            {
                this.list = list;
                this.index = -1;
                version = list._version;
                isArrayList = (list.GetType() == typeof(ArrayList));
                currentElement = dummyObject;
            }

            public Object Clone()
            {
                return MemberwiseClone();
            }

            public bool MoveNext()
            {
                if (version != list._version)
                {
                    throw new InvalidOperationException("EnumFailedVersion");
                }

                if (isArrayList)
                {  
                    if (index < list._size - 1)
                    {
                        currentElement = list._items[++index];
                        return true;
                    }
                    else
                    {
                        currentElement = dummyObject;
                        index = list._size;
                        return false;
                    }
                }
                else
                {
                    if (index < list.Count - 1)
                    {
                        currentElement = list[++index];
                        return true;
                    }
                    else
                    {
                        index = list.Count;
                        currentElement = dummyObject;
                        return false;
                    }
                }
            }

            public Object Current
            {
                get
                {
                    object temp = currentElement;
                    if (dummyObject == temp)
                    { 
                        if (index == -1)
                        {
                            throw new InvalidOperationException("EnumNotStarted");
                        }
                        else
                        {
                            throw new InvalidOperationException("EnumEnded");
                        }
                    }

                    return temp;
                }
            }

            public void Reset()
            {
                if (version != list._version)
                {
                    throw new InvalidOperationException("EnumFailedVersion");
                }

                currentElement = dummyObject;
                index = -1;
            }
        }
    }
}

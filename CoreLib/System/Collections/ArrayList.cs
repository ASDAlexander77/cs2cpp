////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace System.Collections
{
    [Serializable()]
    public class ArrayList : IList, ICloneable
    {
        private Object[] _items;
        private int _size;
        private int _version;

        // Keep in-sync with c_DefaultCapacity in CLR_RT_HeapBlock_ArrayList in TinyCLR_Runtime__HeapBlock.h
        private const int _defaultCapacity = 4;

        public ArrayList()
        {
            _items = new Object[_defaultCapacity];
        }

        public virtual int Capacity
        {
            get { return _items.Length; }
            set
            {
                SetCapacity(value);
            }
        }

        private void SetCapacity(int capacity)
        {
            if (capacity != _items.Length)
            {
                if (capacity < _size)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                if (capacity > 0)
                {
                    Object[] newItems = new Object[capacity];
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
            get { return this; }
        }

        public virtual Object this[int index]
        {

            get
            {
                if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException("index");
                return _items[index];

            }

            set
            {
                if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException("index");
                _items[index] = value;
                _version++;
            }
        }


        public virtual int Add(Object value)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size] = value;
            _version++;
            return _size++;
        }

        public virtual int BinarySearch(Object value, IComparer comparer)
        {
            return Array.BinarySearch(_items, 0, _size, value, comparer);
        }


        public virtual void Clear()
        {
            Array.Clear(_items, 0, _size); // Don't need to doc this but we clear the elements so that the gc can reclaim the references.
            _size = 0;
            _version++;
        }

        public virtual Object Clone()
        {
            ArrayList la = new ArrayList();

            if (_size > _defaultCapacity)
            {
                // only re-allocate a new array if the size isn't what we need.
                // otherwise, the one allocated in the constructor will be just fine
                la._items = new Object[_size];
            }

            la._size = _size;
            Array.Copy(_items, 0, la._items, 0, _size);
            return la;
        }

        public virtual bool Contains(Object item)
        {
            //return Array.IndexOf(_items, item, 0, _size) >= 0;
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

        public virtual void CopyTo(Array array)
        {
            CopyTo(array, 0);
        }

        public virtual void CopyTo(Array array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, _size);
        }

        public virtual IEnumerator GetEnumerator()
        {
            return new Array.SZArrayEnumerator(_items, 0, _size);
        }

        public virtual int IndexOf(Object value)
        {
            return Array.IndexOf(_items, value, 0, _size);
        }

        public virtual int IndexOf(Object value, int startIndex)
        {
            return Array.IndexOf(_items, value, startIndex, _size - startIndex);
        }

        public virtual int IndexOf(Object value, int startIndex, int count)
        {
            return Array.IndexOf(_items, value, startIndex, count);
        }


        public virtual void Insert(int index, Object value)
        {
            // Note that insertions at the end are legal.
            if (index < 0 || index > _size) throw new ArgumentOutOfRangeException("index");
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            if (index < _size)
            {
                Array.Copy(_items, index, _items, index + 1, _size - index);
            }
            _items[index] = value;
            _size++;
            _version++;
        }

        public virtual void Remove(Object obj)
        {
            int index = Array.IndexOf(_items, obj, 0, _size);
            if (index >= 0)
            {
                RemoveAt(index);
            }
        }


        public virtual void RemoveAt(int index)
        {
            if (index < 0 || index >= _size) throw new ArgumentOutOfRangeException("index");
            _size--;
            if (index < _size)
            {
                Array.Copy(_items, index + 1, _items, index, _size - index);
            }
            _items[_size] = null;
            _version++;
        }

        public virtual Object[] ToArray()
        {
            return (Object[])ToArray(typeof(object));
        }

        public virtual Array ToArray(Type type)
        {
            Array array = Array.CreateInstance(type, _size);

            Array.Copy(_items, 0, array, 0, _size);

            return array;
        }

        // Ensures that the capacity of this list is at least the given minimum
        // value. If the currect capacity of the list is less than min, the
        // capacity is increased to twice the current capacity or to min,
        // whichever is larger.
        private void EnsureCapacity(int min)
        {
            if (_items.Length < min)
            {
                int newCapacity = _items.Length == 0 ? _defaultCapacity : _items.Length * 2;
                if (newCapacity < min) newCapacity = min;
                Capacity = newCapacity;
            }
        }
    }
}



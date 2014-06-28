////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace System.Collections
{
    [Serializable()]
    public class ArrayList : IList, ICloneable
    {
        private Object[] _items;
        private int _size;

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
            throw new NotImplementedException();
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
                throw new NotImplementedException();
            }
            
            set
            {
                throw new NotImplementedException();
            }
        }

        
        public virtual int Add(Object value)
        {
            throw new NotImplementedException();
        }

        public virtual int BinarySearch(Object value, IComparer comparer)
        {
            return Array.BinarySearch(_items, 0, _size, value, comparer);
        }

        
        public virtual void Clear()
        {
            throw new NotImplementedException();
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
            return Array.IndexOf(_items, item, 0, _size) >= 0;
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}



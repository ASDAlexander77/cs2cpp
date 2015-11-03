// Licensed under the MIT license. 

namespace System.Runtime.InteropServices
{
    using System;
    using Collections.Generic;

    partial struct GCHandle
    {
        private static object syncObject = new object();

        private static readonly Array<KeyValuePair<object, GCHandleType>> handlers = new Array<KeyValuePair<object, GCHandleType>>();

        private static readonly Array<int> removedAt = new Array<int>();
        
        // Internal native calls that this implementation uses.
        internal static IntPtr InternalAlloc(Object value, GCHandleType type)
        {
            var index = 0;
            lock (syncObject)
            {
                if (removedAt.Count > 0)
                {
                    index = removedAt[0];
                    handlers[index] = new KeyValuePair<object, GCHandleType>(value, type);
                    removedAt.RemoveAt(0);
                    return new IntPtr(index);
                }

                index = handlers.Count;
                handlers.Add(new KeyValuePair<object, GCHandleType>(value, type));
                return new IntPtr(index);
            }
        }

        internal static void InternalFree(IntPtr handle)
        {
            lock (syncObject)
            {
                removedAt.Add(handle.ToInt32());
            }
        }

        internal static Object InternalGet(IntPtr handle)
        {
            lock (syncObject)
            {
                return handlers[handle.ToInt32()].Key;
            }
        }

        internal static void InternalSet(IntPtr handle, Object value, bool isPinned)
        {
            lock (syncObject)
            {
                handlers[handle.ToInt32()] = new KeyValuePair<object, GCHandleType>(value, isPinned ? GCHandleType.Pinned : default(GCHandleType));
            }
        }

        internal static Object InternalCompareExchange(IntPtr handle, Object value, Object oldValue, bool isPinned)
        {
            lock (syncObject)
            {
                var original = handlers[handle.ToInt32()].Value;
                if (Object.ReferenceEquals(original, value) || (oldValue != null && original.Equals(oldValue)))
                {
                    handlers[handle.ToInt32()] = new KeyValuePair<object, GCHandleType>(value, isPinned ? GCHandleType.Pinned : default(GCHandleType));
                }

                return original;
            }
        }

        internal static IntPtr InternalAddrOfPinnedObject(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        internal static void InternalCheckDomain(IntPtr handle)
        {
        }

        internal static GCHandleType InternalGetHandleType(IntPtr handle)
        {
            lock (syncObject)
            {
                return handlers[handle.ToInt32()].Value;
            }
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">
        /// </typeparam>
        public class Array<T>
        {
            private const int _defaultCapacity = 4;

            private T[] _items;
            private int _size;

            static T[] _emptyArray = new T[0];

            /// <summary>
            /// </summary>
            public Array()
            {
                _items = _emptyArray;
            }

            /// <summary>
            /// </summary>
            /// <param name="i">
            /// </param>
            /// <returns>
            /// </returns>
            public T this[int i]
            {
                get
                {
                    if (i < 0 || i >= this.Count)
                    {
                        throw new ArgumentOutOfRangeException("i");
                    }

                    return _items[i];
                }

                set
                {
                    while (i >= this.Count)
                    {
                        this.Add(default(T));
                    }

                    _items[i] = value;
                }
            }

            /// <summary>
            /// </summary>
            public int Length
            {
                get
                {
                    return this.Count;
                }

                set
                {
                    this.Capacity = value;
                }
            }

            #region implementation

            /// <summary>
            /// </summary>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public int Capacity
            {
                get { return _items.Length; }
                set
                {
                    if (value != _items.Length)
                    {
                        if (value > 0)
                        {
                            T[] newItems = new T[value];
                            if (_size > 0)
                            {
                                Copy(_items, newItems);
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

            /// <summary>
            /// </summary>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public int Count
            {
                get { return _size; }
            }

            /// <summary>
            /// </summary>
            /// <param name="item">
            /// </param>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public void Add(T item)
            {
                if (_size == _items.Length)
                {
                    EnsureCapacity(_size + 1);
                }

                _items[_size++] = item;
            }

            /// <summary>
            /// </summary>
            /// <param name="items">
            /// </param>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public void AddRange(IEnumerable<T> items)
            {
                foreach (var item in items)
                {
                    this.Add(item);
                }
            }

            public void AddRange(T[] items)
            {
                foreach (var item in items)
                {
                    this.Add(item);
                }
            }

            /// <summary>
            /// </summary>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public void Clear()
            {
                for (var i = 0; i < _size; i++)
                {
                    _items[i] = default(T);
                }

                _size = 0;
            }

            /// <summary>
            /// </summary>
            /// <param name="t">
            /// </param>
            /// <returns>
            /// </returns>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public int IndexOf(T item)
            {
                return IndexOf(_items, item);
            }

            /// <summary>
            /// </summary>
            /// <param name="i">
            /// </param>
            /// <exception cref="NotImplementedException">
            /// </exception>
            public void RemoveAt(int index)
            {
                if ((uint)index >= (uint)_size)
                {
                    throw new IndexOutOfRangeException();
                }

                _size--;
                if (index < _size)
                {
                    Copy(_items, index + 1, _items, index, _size - index);
                }
                _items[_size] = default(T);
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

            private void Copy(T[] items, T[] newItems)
            {
                for (var i = 0; i < _size; i++)
                {
                    newItems[i] = items[i];
                }
            }

            private void Copy(T[] items, int startItemsIndex, T[] newItems, int startNewItensIndex, int count)
            {
                for (var i = 0; i < count; i++)
                {
                    newItems[i + startNewItensIndex] = items[i + startItemsIndex];
                }
            }

            private int IndexOf(T[] items, T item)
            {
                for (var i = 0; i < _size; i++)
                {
                    if (item.Equals(items[i]))
                    {
                        return i;
                    }
                }

                return -1;
            }

            #endregion
        }
    }
}

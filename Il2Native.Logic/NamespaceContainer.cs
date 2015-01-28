namespace Il2Native.Logic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;

    public interface INamespaceContainer<T> : ISet<T>, IList<T>
    {
        void AddRange(IEnumerable<T> range);
        void RemoveAll(Func<T, bool> criteria);
    }

    [DebuggerDisplay("Count: {Count}")]
    public class NamespaceContainer<T> : INamespaceContainer<T> where T : PEAssemblyReader.IName
    {
        private SubContainer _root = new SubContainer();

        private IList<T> _list = new List<T>();

        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public bool IsReadOnly { get; private set; }

        public T this[int index]
        {
            get
            {
                return _list[index];
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(T item)
        {
            if (_root.Add(item))
            {
                _list.Add(item);
            }
        }

        bool ISet<T>.Add(T item)
        {
            if (_root.Add(item))
            {
                _list.Add(item);
                return true;
            }

            return false;
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Add(item);
            }
        }

        public void RemoveAll(Func<T, bool> criteria)
        {
            foreach (var item in _list)
            {
                if (criteria == null || criteria(item))
                {
                    Remove(item);
                }
            }
        }

        public void Clear()
        {
            _root.Clear();
            _list.Clear();
        }

        public bool Contains(T item)
        {
            return _root.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            return _root.Remove(item);
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        [DebuggerDisplay("Count: {Containers.Count}, Objects: {Basket.Count}")]
        private class SubContainer
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private object _containerLocker = new object();

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private IDictionary<string, SubContainer> _containers;

            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private ISet<T> _basket;

            public IDictionary<string, SubContainer> Containers
            {
                get
                {
                    lock (_containerLocker)
                    {
                        if (_containers == null)
                        {
                            _containers = new SortedDictionary<string, SubContainer>();
                        }

                        return _containers;
                    }
                }
            }

            public ISet<T> Basket
            {
                get
                {
                    lock (_containerLocker)
                    {
                        if (_basket == null)
                        {
                            _basket = new HashSet<T>();
                        }

                        return _basket;
                    }
                }
            }

            public bool Add(T obj)
            {
                return this.Add(obj.Namespace, obj);
            }

            public bool Contains(T obj)
            {
                return this.Contains(obj.Namespace, obj);
            }

            public bool Remove(T obj)
            {
                return this.Remove(obj.Namespace, obj);
            }

            public void Clear()
            {
                if (_containers != null)
                {
                    foreach (var subContainer in _containers)
                    {
                        subContainer.Value.Clear();
                    }

                    _containers.Clear();
                }

                if (_basket != null)
                {
                    _basket.Clear();
                }
            }

            private bool Add(string subNamespace, T obj)
            {
                if (string.IsNullOrEmpty(subNamespace))
                {
                    lock (_containerLocker)
                    {
                        return this.Basket.Add(obj);
                    }
                }

                string tail;
                var name = this.GetNamechain(subNamespace, out tail);
                var container = this.GetOrCreateContainer(name);
                return container.Add(tail, obj);
            }

            private bool Contains(string subNamespace, T obj)
            {
                if (string.IsNullOrEmpty(subNamespace))
                {
                    if (_basket == null)
                    {
                        return false;
                    }

                    lock (_containerLocker)
                    {
                        return this.Basket.Contains(obj);
                    }
                }

                if (_containers == null)
                {
                    return false;
                }

                string tail;
                var name = this.GetNamechain(subNamespace, out tail);
                var container = this.GetContainer(name);
                if (container == null)
                {
                    return false;
                }

                return container.Contains(tail, obj);
            }

            private bool Remove(string subNamespace, T obj)
            {
                if (string.IsNullOrEmpty(subNamespace))
                {
                    lock (_containerLocker)
                    {
                        return this.Basket.Remove(obj);
                    }
                }

                string tail;
                var name = this.GetNamechain(subNamespace, out tail);
                var container = this.GetContainer(name);
                if (container == null)
                {
                    return false;
                }

                return container.Remove(tail, obj);
            }

            private SubContainer GetOrCreateContainer(string name)
            {
                lock (_containerLocker)
                {
                    SubContainer container;
                    if (!this.Containers.TryGetValue(name, out container))
                    {
                        container = new SubContainer();
                        this.Containers.Add(name, container);
                    }

                    return container;
                }
            }

            private SubContainer GetContainer(string name)
            {
                if (_containers == null)
                {
                    return null;
                }

                SubContainer container;
                if (!this.Containers.TryGetValue(name, out container))
                {
                    return null;
                }

                return container;
            }

            private string GetNamechain(string subNamespace, out string tail)
            {
                tail = null;

                if (subNamespace == null)
                {
                    return string.Empty;
                }

                var pos = subNamespace.IndexOf('.');
                if (pos >= 0)
                {
                    tail = subNamespace.Substring(pos + 1);
                    return subNamespace.Substring(0, pos);
                }

                return subNamespace;
            }
        }
    }

    public class NamespaceContainer<K, V> : NamespaceContainer<IAssoc<K, V>> where K : PEAssemblyReader.IName
    {
    }
}
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class Array : ICloneable, IList
    {       
        public static Array CreateInstance(Type elementType, int length)
        {
            throw new NotImplementedException();
        }

        public static void Copy(Array sourceArray, Array destinationArray, int length)
        {
            Copy(sourceArray, 0, destinationArray, 0, length);
        }
        
        public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
        {
            throw new NotImplementedException();
        }
       
        public static void Clear(Array array, int index, int length)
        {
            throw new NotImplementedException();
        }

        public unsafe Object GetValue(params int[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");
            if (Rank != indices.Length)
                throw new ArgumentException(Environment.GetResourceString("Arg_RankIndices"));

            TypedReference elemref = new TypedReference();
            fixed (int* pIndices = indices)
                InternalGetReference(&elemref, indices.Length, pIndices);
            return TypedReference.InternalToObject(&elemref);
        }

        public unsafe Object GetValue(int index)
        {
            if (Rank != 1)
                throw new ArgumentException(Environment.GetResourceString("Arg_Need1DArray"));

            TypedReference elemref = new TypedReference();
            InternalGetReference(&elemref, 1, &index);
            return TypedReference.InternalToObject(&elemref);
        }

        public unsafe Object GetValue(int index1, int index2)
        {
            if (Rank != 2)
                throw new ArgumentException(Environment.GetResourceString("Arg_Need2DArray"));

            int* pIndices = stackalloc int[2];
            pIndices[0] = index1;
            pIndices[1] = index2;

            TypedReference elemref = new TypedReference();
            InternalGetReference(&elemref, 2, pIndices);
            return TypedReference.InternalToObject(&elemref);
        }

        public unsafe Object GetValue(int index1, int index2, int index3)
        {
            if (Rank != 3)
                throw new ArgumentException(Environment.GetResourceString("Arg_Need3DArray"));

            int* pIndices = stackalloc int[3];
            pIndices[0] = index1;
            pIndices[1] = index2;
            pIndices[2] = index3;

            TypedReference elemref = new TypedReference();
            InternalGetReference(&elemref, 3, pIndices);
            return TypedReference.InternalToObject(&elemref);
        }

        public Object GetValue(long index)
        {
            if (index > Int32.MaxValue || index < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));

            return this.GetValue((int)index);
        }

        public Object GetValue(long index1, long index2)
        {
            if (index1 > Int32.MaxValue || index1 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index1", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));
            if (index2 > Int32.MaxValue || index2 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index2", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));

            return this.GetValue((int)index1, (int)index2);
        }

        public Object GetValue(long index1, long index2, long index3)
        {
            if (index1 > Int32.MaxValue || index1 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index1", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));
            if (index2 > Int32.MaxValue || index2 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index2", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));
            if (index3 > Int32.MaxValue || index3 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index3", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));

            return this.GetValue((int)index1, (int)index2, (int)index3);
        }

        public Object GetValue(params long[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");
            if (Rank != indices.Length)
                throw new ArgumentException(Environment.GetResourceString("Arg_RankIndices"));

            int[] intIndices = new int[indices.Length];

            for (int i = 0; i < indices.Length; ++i)
            {
                long index = indices[i];
                if (index > Int32.MaxValue || index < Int32.MinValue)
                    throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));
                intIndices[i] = (int)index;
            }

            return this.GetValue(intIndices);
        }

        public unsafe void SetValue(Object value, int index)
        {
            if (Rank != 1)
                throw new ArgumentException(Environment.GetResourceString("Arg_Need1DArray"));

            TypedReference elemref = new TypedReference();
            InternalGetReference(&elemref, 1, &index);
            InternalSetValue(&elemref, value);
        }

        public unsafe void SetValue(Object value, int index1, int index2)
        {
            if (Rank != 2)
                throw new ArgumentException(Environment.GetResourceString("Arg_Need2DArray"));

            int* pIndices = stackalloc int[2];
            pIndices[0] = index1;
            pIndices[1] = index2;

            TypedReference elemref = new TypedReference();
            InternalGetReference(&elemref, 2, pIndices);
            InternalSetValue(&elemref, value);
        }

        public unsafe void SetValue(Object value, int index1, int index2, int index3)
        {
            if (Rank != 3)
                throw new ArgumentException(Environment.GetResourceString("Arg_Need3DArray"));

            int* pIndices = stackalloc int[3];
            pIndices[0] = index1;
            pIndices[1] = index2;
            pIndices[2] = index3;

            TypedReference elemref = new TypedReference();
            InternalGetReference(&elemref, 3, pIndices);
            InternalSetValue(&elemref, value);
        }

        public unsafe void SetValue(Object value, params int[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");
            if (Rank != indices.Length)
                throw new ArgumentException(Environment.GetResourceString("Arg_RankIndices"));

            TypedReference elemref = new TypedReference();
            fixed (int* pIndices = indices)
                InternalGetReference(&elemref, indices.Length, pIndices);
            InternalSetValue(&elemref, value);
        }

        public void SetValue(Object value, long index)
        {
            if (index > Int32.MaxValue || index < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));

            this.SetValue(value, (int)index);
        }

        public void SetValue(Object value, long index1, long index2)
        {
            if (index1 > Int32.MaxValue || index1 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index1", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));
            if (index2 > Int32.MaxValue || index2 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index2", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));

            this.SetValue(value, (int)index1, (int)index2);
        }

        public void SetValue(Object value, long index1, long index2, long index3)
        {
            if (index1 > Int32.MaxValue || index1 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index1", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));
            if (index2 > Int32.MaxValue || index2 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index2", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));
            if (index3 > Int32.MaxValue || index3 < Int32.MinValue)
                throw new ArgumentOutOfRangeException("index3", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));

            this.SetValue(value, (int)index1, (int)index2, (int)index3);
        }

        public void SetValue(Object value, params long[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");
            if (Rank != indices.Length)
                throw new ArgumentException(Environment.GetResourceString("Arg_RankIndices"));

            int[] intIndices = new int[indices.Length];

            for (int i = 0; i < indices.Length; ++i)
            {
                long index = indices[i];
                if (index > Int32.MaxValue || index < Int32.MinValue)
                    throw new ArgumentOutOfRangeException("index", Environment.GetResourceString("ArgumentOutOfRange_HugeArrayNotSupported"));
                intIndices[i] = (int)index;
            }

            this.SetValue(value, intIndices);
        }

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        // reference to TypedReference is banned, so have to pass result as pointer
        private unsafe extern void InternalGetReference(void* elemRef, int rank, int* pIndices);

        // Ideally, we would like to use TypedReference.SetValue instead. Unfortunately, TypedReference.SetValue
        // always throws not-supported exception
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private unsafe extern static void InternalSetValue(void* target, Object value);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern int GetUpperBound(int dimension);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern int GetLowerBound(int dimension);

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public extern int GetLength(int dimension);

        public extern int Rank
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern int Length
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        int ICollection.Count
        {
            get
            {
                return this.Length;
            }
        }

        public Object SyncRoot
        {
            get { return this; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return true; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        Object IList.this[int index]
        {
            
            get
            {
                return this.GetValue(index);
            }
            
            set
            {
                this.SetValue(value, index);

            }
        }

        int IList.Add(Object value)
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(Object value)
        {
            return Array.IndexOf(this, value) >= 0;
        }

        void IList.Clear()
        {
            Array.Clear(this, 0, this.Length);
        }   

        int IList.IndexOf(Object value)
        {
            return Array.IndexOf(this, value);
        }

        void IList.Insert(int index, Object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(Object value)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        public Object Clone()
        {
            int length = this.Length;
            Array destArray = Array.CreateInstance(this.GetType().GetElementType(), length);
            Array.Copy(this, destArray, length);

            return destArray;
        }

        private static int GetMedian(int low, int hi)
        {
            return low + ((hi - low) >> 1);
        }

        public static int BinarySearch(Array array, Object value, IComparer comparer)
        {
            return BinarySearch(array, 0, array.Length, value, comparer);
        }

        public static int BinarySearch(Array array, int index, int length, Object value, IComparer comparer)
        {
            int lo = index;
            int hi = index + length - 1;
            while (lo <= hi)
            {
                int i = (lo + hi) >> 1;
                int c = comparer.Compare(array.GetValue(i), value);

                if (c == 0) return i;
                if (c < 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }

            return ~lo;
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(this, 0, array, index, this.Length);
        }

        public IEnumerator GetEnumerator()
        {
            return new SZArrayEnumerator(this);
        }

        public static int IndexOf(Array array, Object value)
        {
            return IndexOf(array, value, 0, array.Length);
        }

        public static int IndexOf(Array array, Object value, int startIndex)
        {
            return IndexOf(array, value, startIndex, array.Length - startIndex);
        }

        public static int IndexOf(Array array, Object value, int startIndex, int count)
        {
            int endIndex = startIndex + count;

            for (int i = startIndex; i < endIndex; i++)
            {
                Object obj = array.GetValue(i);

                if (Object.Equals(obj, value)) return i;
            }

            return -1;
        }

        public static void Resize<T>(ref T[] array, int newSize)
        {
            if (newSize < 0)
                throw new ArgumentOutOfRangeException("newSize", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));

            T[] larray = array;
            if (larray == null)
            {
                array = new T[newSize];
                return;
            }

            if (larray.Length != newSize)
            {
                T[] newArray = new T[newSize];
                Array.Copy(larray, 0, newArray, 0, larray.Length > newSize ? newSize : larray.Length);
                array = newArray;
            }
        }

        public static int BinarySearch<T>(T[] array, T value) {
            if (array==null)
                throw new ArgumentNullException("array");
            return BinarySearch<T>(array, 0, array.Length, value, null);
        }

        public static int BinarySearch<T>(T[] array, T value, System.Collections.Generic.IComparer<T> comparer) {
            if (array==null)
                throw new ArgumentNullException("array");
            return BinarySearch<T>(array, 0, array.Length, value, comparer);
        }

        public static int BinarySearch<T>(T[] array, int index, int length, T value) {
            return BinarySearch<T>(array, index, length, value, null);
        }

        public static int BinarySearch<T>(T[] array, int index, int length, T value, System.Collections.Generic.IComparer<T> comparer) {
            if (array==null) 
                throw new ArgumentNullException("array");
            if (index < 0 || length < 0)
                throw new ArgumentOutOfRangeException((index<0 ? "index" : "length"), "NeedNonNegNum");
            if (array.Length - index < length)
                throw new ArgumentException("Argument_InvalidOffLen");

            return ArraySortHelper<T>.Default.BinarySearch(array, index, length, value, comparer);
        }

        public static void Sort(Array array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            Sort(array, null, 0, array.Length, null);
        }

        public static void Sort(Array keys, Array items)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");
            Sort(keys, items, 0, keys.Length, null);
        }

        public static void Sort(Array array, int index, int length)
        {
            Sort(array, null, index, length, null);
        }

        public static void Sort(Array keys, Array items, int index, int length)
        {
            Sort(keys, items, index, length, null);
        }

        public static void Sort(Array array, IComparer comparer)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            Sort(array, null, 0, array.Length, comparer);
        }

        public static void Sort(Array keys, Array items, IComparer comparer)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");
            Sort(keys, items, 0, keys.Length, comparer);
        }

        public static void Sort(Array array, int index, int length, IComparer comparer)
        {
            Sort(array, null, index, length, comparer);
        }

        public static void Sort(Array keys, Array items, int index, int length, IComparer comparer)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");
            if (length > 1)
            {
                Object[] objKeys = keys as Object[];
                Object[] objItems = null;
                if (objKeys != null)
                    objItems = items as Object[];
                SorterObjectArray sorter = new SorterObjectArray(objKeys, objItems, comparer);
                sorter.Sort(index, length);
            }
        }

        public static void Sort<T>(T[] array) {
            if (array==null)
                throw new ArgumentNullException("array");
            Sort<T>(array, 0, array.Length, null);
        }

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items) {
            if (keys==null)
                throw new ArgumentNullException("keys");
            Sort<TKey, TValue>(keys, items, 0, keys.Length, null);
        }

        public static void Sort<T>(T[] array, int index, int length) {
            Sort<T>(array, index, length, null);
        }

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length) {
            Sort<TKey, TValue>(keys, items, index, length, null);
        }

        public static void Sort<T>(T[] array, System.Collections.Generic.IComparer<T> comparer) {
            if (array==null)
                throw new ArgumentNullException("array");
            Sort<T>(array, 0, array.Length, comparer);
        }

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, System.Collections.Generic.IComparer<TKey> comparer) {
            if (keys==null)
                throw new ArgumentNullException("keys");
            Sort<TKey, TValue>(keys, items, 0, keys.Length, comparer);
        }

        public static void Sort<T>(T[] array, int index, int length, System.Collections.Generic.IComparer<T> comparer) {
            if (array==null)
                throw new ArgumentNullException("array");

            if (length > 1) {
                ArraySortHelper<T>.Default.Sort(array, index, length, comparer);                
            }
        }

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length, System.Collections.Generic.IComparer<TKey> comparer) {
            if (keys==null)
                throw new ArgumentNullException("keys");
            if (index < 0 || length < 0)

            if (length > 1) {
                if (items == null)
                {
                    Sort<TKey>(keys, index, length, comparer);
                    return;
                }

                ArraySortHelper<TKey, TValue>.Default.Sort(keys, items, index, length, comparer);
            }
        }

        public static void Sort<T>(T[] array, Comparison<T> comparison)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (comparison == null)
            {
                throw new ArgumentNullException("comparison");
            }

            IComparer<T> comparer = new FunctorComparer<T>(comparison);
            Array.Sort(array, comparer);
        }

        public static bool TrueForAll<T>(T[] array, Predicate<T> match)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (match == null)
            {
                throw new ArgumentNullException("match");
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (!match(array[i]))
                {
                    return false;
                }
            }
            return true;
        }

        // Private value type used by the Sort methods.
        private struct SorterObjectArray
        {
            private Object[] keys;
            private Object[] items;
            private IComparer comparer;

            internal SorterObjectArray(Object[] keys, Object[] items, IComparer comparer)
            {
                if (comparer == null) comparer = Comparer.Default;
                this.keys = keys;
                this.items = items;
                this.comparer = comparer;
            }

            internal void SwapIfGreaterWithItems(int a, int b)
            {
                if (a != b)
                {
                    if (comparer.Compare(keys[a], keys[b]) > 0)
                    {
                        Object temp = keys[a];
                        keys[a] = keys[b];
                        keys[b] = temp;
                        if (items != null)
                        {
                            Object item = items[a];
                            items[a] = items[b];
                            items[b] = item;
                        }
                    }
                }
            }

            private void Swap(int i, int j)
            {
                Object t = keys[i];
                keys[i] = keys[j];
                keys[j] = t;

                if (items != null)
                {
                    Object item = items[i];
                    items[i] = items[j];
                    items[j] = item;
                }
            }

            internal void Sort(int left, int length)
            {
                DepthLimitedQuickSort(left, length + left - 1, IntrospectiveSortUtilities.QuickSortDepthThreshold);
            }

            private void DepthLimitedQuickSort(int left, int right, int depthLimit)
            {
                // Can use the much faster jit helpers for array access.
                do
                {
                    if (depthLimit == 0)
                    {
                        // Add a try block here to detect IComparers (or their
                        // underlying IComparables, etc) that are bogus.
                        try
                        {
                            Heapsort(left, right);
                            return;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new ArgumentException("BogusIComparer");
                        }
                        catch (Exception e)
                        {
                            throw new InvalidOperationException("IComparerFailed");
                        }
                    }

                    int i = left;
                    int j = right;

                    // pre-sort the low, middle (pivot), and high values in place.
                    // this improves performance in the face of already sorted data, or 
                    // data that is made up of multiple sorted runs appended together.
                    int middle = GetMedian(i, j);

                    // Add a try block here to detect IComparers (or their
                    // underlying IComparables, etc) that are bogus.
                    try
                    {
                        SwapIfGreaterWithItems(i, middle); // swap the low with the mid point
                        SwapIfGreaterWithItems(i, j);      // swap the low with the high
                        SwapIfGreaterWithItems(middle, j); // swap the middle with the high
                    }
                    catch (Exception e)
                    {
                        throw new InvalidOperationException("IComparerFailed");
                    }
                    Object x = keys[middle];
                    do
                    {
                        // Add a try block here to detect IComparers (or their
                        // underlying IComparables, etc) that are bogus.
                        try
                        {
                            while (comparer.Compare(keys[i], x) < 0) i++;
                            while (comparer.Compare(x, keys[j]) < 0) j--;
                        }
                        catch (IndexOutOfRangeException)
                        {
                            throw new ArgumentException("BogusIComparer");
                        }
                        catch (Exception e)
                        {
                            throw new InvalidOperationException("IComparerFailed");
                        }

                        if (i > j) break;
                        if (i < j)
                        {
                            Object key = keys[i];
                            keys[i] = keys[j];
                            keys[j] = key;
                            if (items != null)
                            {
                                Object item = items[i];
                                items[i] = items[j];
                                items[j] = item;
                            }
                        }
                        i++;
                        j--;
                    } while (i <= j);

                    // The next iteration of the while loop is to "recursively" sort the larger half of the array and the
                    // following calls recrusively sort the smaller half.  So we subtrack one from depthLimit here so
                    // both sorts see the new value.
                    depthLimit--;

                    if (j - left <= right - i)
                    {
                        if (left < j) DepthLimitedQuickSort(left, j, depthLimit);
                        left = i;
                    }
                    else
                    {
                        if (i < right) DepthLimitedQuickSort(i, right, depthLimit);
                        right = j;
                    }
                } while (left < right);
            }

            private void IntrospectiveSort(int left, int length)
            {
                if (length < 2)
                    return;

                try
                {
                    IntroSort(left, length + left - 1, 2 * IntrospectiveSortUtilities.FloorLog2(keys.Length));
                }
                catch (IndexOutOfRangeException)
                {
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("IComparerFailed");
                }
            }

            private void IntroSort(int lo, int hi, int depthLimit)
            {
                while (hi > lo)
                {
                    int partitionSize = hi - lo + 1;
                    if (partitionSize <= IntrospectiveSortUtilities.IntrosortSizeThreshold)
                    {
                        if (partitionSize == 1)
                        {
                            return;
                        }
                        if (partitionSize == 2)
                        {
                            SwapIfGreaterWithItems(lo, hi);
                            return;
                        }
                        if (partitionSize == 3)
                        {
                            SwapIfGreaterWithItems(lo, hi - 1);
                            SwapIfGreaterWithItems(lo, hi);
                            SwapIfGreaterWithItems(hi - 1, hi);
                            return;
                        }

                        InsertionSort(lo, hi);
                        return;
                    }

                    if (depthLimit == 0)
                    {
                        Heapsort(lo, hi);
                        return;
                    }
                    depthLimit--;

                    int p = PickPivotAndPartition(lo, hi);
                    IntroSort(p + 1, hi, depthLimit);
                    hi = p - 1;
                }
            }

            private int PickPivotAndPartition(int lo, int hi)
            {
                // Compute median-of-three.  But also partition them, since we've done the comparison.
                int mid = lo + (hi - lo) / 2;
                // Sort lo, mid and hi appropriately, then pick mid as the pivot.
                SwapIfGreaterWithItems(lo, mid);
                SwapIfGreaterWithItems(lo, hi);
                SwapIfGreaterWithItems(mid, hi);

                Object pivot = keys[mid];
                Swap(mid, hi - 1);
                int left = lo, right = hi - 1;  // We already partitioned lo and hi and put the pivot in hi - 1.  And we pre-increment & decrement below.

                while (left < right)
                {
                    while (comparer.Compare(keys[++left], pivot) < 0) ;
                    while (comparer.Compare(pivot, keys[--right]) < 0) ;

                    if (left >= right)
                        break;

                    Swap(left, right);
                }

                // Put pivot in the right location.
                Swap(left, (hi - 1));
                return left;
            }

            private void Heapsort(int lo, int hi)
            {
                int n = hi - lo + 1;
                for (int i = n / 2; i >= 1; i = i - 1)
                {
                    DownHeap(i, n, lo);
                }
                for (int i = n; i > 1; i = i - 1)
                {
                    Swap(lo, lo + i - 1);

                    DownHeap(1, i - 1, lo);
                }
            }

            private void DownHeap(int i, int n, int lo)
            {
                Object d = keys[lo + i - 1];
                Object dt = (items != null) ? items[lo + i - 1] : null;
                int child;
                while (i <= n / 2)
                {
                    child = 2 * i;
                    if (child < n && comparer.Compare(keys[lo + child - 1], keys[lo + child]) < 0)
                    {
                        child++;
                    }
                    if (!(comparer.Compare(d, keys[lo + child - 1]) < 0))
                        break;
                    keys[lo + i - 1] = keys[lo + child - 1];
                    if (items != null)
                        items[lo + i - 1] = items[lo + child - 1];
                    i = child;
                }
                keys[lo + i - 1] = d;
                if (items != null)
                    items[lo + i - 1] = dt;
            }

            private void InsertionSort(int lo, int hi)
            {
                int i, j;
                Object t, ti;
                for (i = lo; i < hi; i++)
                {
                    j = i;
                    t = keys[i + 1];
                    ti = (items != null) ? items[i + 1] : null;
                    while (j >= lo && comparer.Compare(t, keys[j]) < 0)
                    {
                        keys[j + 1] = keys[j];
                        if (items != null)
                            items[j + 1] = items[j];
                        j--;
                    }
                    keys[j + 1] = t;
                    if (items != null)
                        items[j + 1] = ti;
                }
            }
        }

        // This is the underlying Enumerator for all of our array-based data structures (Array, ArrayList, Stack, and Queue)
        // It supports enumerating over an array, a part of an array, and also will wrap around when the endIndex
        // specified is larger than the size of the array (to support Queue's internal circular array)
        internal class SZArrayEnumerator : IEnumerator
        {
            private Array _array;
            private int _index;
            private int _endIndex;
            private int _startIndex;
            private int _arrayLength;

            internal SZArrayEnumerator(Array array)
            {
                _array = array;
                _arrayLength = _array.Length;
                _endIndex = _arrayLength;
                _startIndex = 0;
                _index = -1;
            }

            // By specifying the startIndex and endIndex, the enumerator will enumerate
            // only a subset of the array. Note that startIndex is inclusive, while
            // endIndex is NOT inclusive.
            // For example, if array is of size 5,
            // new SZArrayEnumerator(array, 0, 3) will enumerate through
            // array[0], array[1], array[2]
            //
            // This also supports an array acting as a circular data structure.
            // For example, if array is of size 5,
            // new SZArrayEnumerator(array, 4, 7) will enumerate through
            // array[4], array[0], array[1]
            internal SZArrayEnumerator(Array array, int startIndex, int endIndex)
            {
                _array = array;
                _arrayLength = _array.Length;
                _endIndex = endIndex;
                _startIndex = startIndex;
                _index = _startIndex - 1;
            }

            public bool MoveNext()
            {
                if (_index < _endIndex)
                {
                    _index++;
                    return (_index < _endIndex);
                }

                return false;
            }

            public Object Current
            {
                get
                {
                    return _array.GetValue(_index % _arrayLength);
                }
            }

            public void Reset()
            {
                _index = _startIndex - 1;
            }
        }

        internal sealed class FunctorComparer<T> : IComparer<T>
        {
            Comparison<T> comparison;

            public FunctorComparer(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }

            public int Compare(T x, T y)
            {
                return comparison(x, y);
            }
        }
    }

    internal static class EmptyArray<T>
    {
        public static readonly T[] Value = new T[0];
    }
}



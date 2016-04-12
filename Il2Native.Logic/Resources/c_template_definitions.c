// Array
template <typename T>
int32_t  __array<T>::__array_element_size()
{
	return sizeof(T);
}

template <typename T>
void __array<T>::InternalGetReference(void* elemRef, int32_t rank, int32_t* pIndices)
{
	if (rank != 1)
	{
		throw __new<CoreLib::System::InvalidOperationException>(L"rank"_s);
	}

	if (elemRef == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>(L"elemRef"_s);
	}	

	if (pIndices == nullptr)
	{
		throw __new<CoreLib::System::ArgumentNullException>(L"pIndices"_s);
	}	

	auto index = pIndices[0];
	if (index < 0 || index >= this->_length)
	{
		throw __new<CoreLib::System::IndexOutOfRangeException>();
	}	

	auto typedRef = reinterpret_cast<CoreLib::System::TypedReference*>(elemRef);
	typedRef->Value = __init<CoreLib::System::IntPtr>((void*)&this->_data[index]);
	typedRef->Type = __init<CoreLib::System::IntPtr>((void*)_typeof<T>());
}

template <typename T>
int32_t __array<T>::get_Length()
{
	return this->_length;
}

template <typename T>
int32_t __array<T>::get_Rank()
{
	return 1;
}

// IListT1
template <typename T>
T __array<T>::System_Collections_Generic_IListT1_get_Item(int32_t) 
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_Generic_IListT1_set_Item(int32_t, T)
{
	throw 0xC000C000;
}

template <typename T>
int32_t __array<T>::System_Collections_Generic_IListT1_IndexOf(T)
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_Generic_IListT1_Insert(int32_t, T)
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_Generic_IListT1_RemoveAt(int32_t)
{
	throw 0xC000C000;
}

// ICollectionT1
template <typename T>
int32_t __array<T>::System_Collections_Generic_ICollectionT1_get_Count() 
{
	throw 0xC000C000;
}

template <typename T>
bool __array<T>::System_Collections_Generic_ICollectionT1_get_IsReadOnly() 
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_Generic_ICollectionT1_Add(T) 
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_Generic_ICollectionT1_Clear() 
{
	throw 0xC000C000;
}

template <typename T>
bool __array<T>::System_Collections_Generic_ICollectionT1_Contains(T) 
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_Generic_ICollectionT1_CopyTo(__array<T>*, int32_t) 
{
	throw 0xC000C000;
}

template <typename T>
bool __array<T>::System_Collections_Generic_ICollectionT1_Remove(T) 
{
	throw 0xC000C000;
}

// IEnumerableT1
template <typename T>
CoreLib::System::Collections::Generic::IEnumeratorT1<T>* __array<T>::System_Collections_Generic_IEnumerableT1_GetEnumerator()
{
	throw 0xC000C000;
}

// IList
template <typename T>
CoreLib::System::Object* __array<T>::System_Collections_IList_get_Item(int32_t)
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_IList_set_Item(int32_t, CoreLib::System::Object*)
{
	throw 0xC000C000;
}

template <typename T>
int32_t __array<T>::System_Collections_IList_Add(CoreLib::System::Object*) 
{
	throw 0xC000C000;
}

template <typename T>
bool __array<T>::System_Collections_IList_Contains(CoreLib::System::Object*) 
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_IList_Clear()
{
	throw 0xC000C000;
}

template <typename T>
bool __array<T>::System_Collections_IList_get_IsReadOnly()
{
	throw 0xC000C000;
}

template <typename T>
bool __array<T>::System_Collections_IList_get_IsFixedSize() 
{
	throw 0xC000C000;
}

template <typename T>
int32_t __array<T>::System_Collections_IList_IndexOf(CoreLib::System::Object*) 
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_IList_Insert(int32_t, CoreLib::System::Object*)
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_IList_Remove(CoreLib::System::Object*)
{
	throw 0xC000C000;
}

template <typename T>
void __array<T>::System_Collections_IList_RemoveAt(int32_t) 
{
	throw 0xC000C000;
}

// ICollection
template <typename T>
void __array<T>::System_Collections_ICollection_CopyTo(CoreLib::System::Array*, int32_t) 	
{
	throw 0xC000C000;
}

template <typename T>
int32_t __array<T>::System_Collections_ICollection_get_Count() 	
{
	throw 0xC000C000;
}

template <typename T>
CoreLib::System::Object* __array<T>::System_Collections_ICollection_get_SyncRoot() 	
{
	throw 0xC000C000;
}

template <typename T>
bool __array<T>::System_Collections_ICollection_get_IsSynchronized() 	
{
	throw 0xC000C000;
}

// IEnumerable
template <typename T>
CoreLib::System::Collections::IEnumerator* __array<T>::System_Collections_IEnumerable_GetEnumerator()	
{
	throw 0xC000C000;
}

// Method : 
template <typename T> 
T __array<T>::__array_IListT1::System_Collections_Generic_IListT1_get_Item(int32_t index)
{
    return this->_class->System_Collections_Generic_IListT1_get_Item(index);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::System_Collections_Generic_IListT1_set_Item(int32_t index, T value)
{
    return this->_class->System_Collections_Generic_IListT1_set_Item(index, value);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IListT1::System_Collections_Generic_IListT1_IndexOf(T item)
{
    return this->_class->System_Collections_Generic_IListT1_IndexOf(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::System_Collections_Generic_IListT1_Insert(int32_t index, T item)
{
    return this->_class->System_Collections_Generic_IListT1_Insert(index, item);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::System_Collections_Generic_IListT1_RemoveAt(int32_t index)
{
    return this->_class->System_Collections_Generic_IListT1_RemoveAt(index);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IListT1::System_Collections_Generic_ICollectionT1_get_Count()
{
    return this->_class->System_Collections_Generic_ICollectionT1_get_Count();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IListT1::System_Collections_Generic_ICollectionT1_get_IsReadOnly()
{
    return this->_class->System_Collections_Generic_ICollectionT1_get_IsReadOnly();
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::System_Collections_Generic_ICollectionT1_Add(T item)
{
    return this->_class->System_Collections_Generic_ICollectionT1_Add(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::System_Collections_Generic_ICollectionT1_Clear()
{
    return this->_class->System_Collections_Generic_ICollectionT1_Clear();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IListT1::System_Collections_Generic_ICollectionT1_Contains(T item)
{
    return this->_class->System_Collections_Generic_ICollectionT1_Contains(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_IListT1::System_Collections_Generic_ICollectionT1_CopyTo(__array<T>* array, int32_t arrayIndex)
{
    return this->_class->System_Collections_Generic_ICollectionT1_CopyTo(array, arrayIndex);
}

// Method : 
template <typename T> 
bool __array<T>::__array_IListT1::System_Collections_Generic_ICollectionT1_Remove(T item)
{
    return this->_class->System_Collections_Generic_ICollectionT1_Remove(item);
}

// Method : 
template <typename T> 
CoreLib::System::Collections::Generic::IEnumeratorT1<T>* __array<T>::__array_IListT1::System_Collections_Generic_IEnumerableT1_GetEnumerator()
{
    return this->_class->System_Collections_Generic_IEnumerableT1_GetEnumerator();
}

// Method : 
template <typename T> 
CoreLib::System::Collections::IEnumerator* __array<T>::__array_IListT1::System_Collections_IEnumerable_GetEnumerator()
{
    return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_ICollectionT1::System_Collections_Generic_ICollectionT1_get_Count()
{
    return this->_class->System_Collections_Generic_ICollectionT1_get_Count();
}

// Method : 
template <typename T> 
bool __array<T>::__array_ICollectionT1::System_Collections_Generic_ICollectionT1_get_IsReadOnly()
{
    return this->_class->System_Collections_Generic_ICollectionT1_get_IsReadOnly();
}

// Method : 
template <typename T> 
void __array<T>::__array_ICollectionT1::System_Collections_Generic_ICollectionT1_Add(T item)
{
    return this->_class->System_Collections_Generic_ICollectionT1_Add(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_ICollectionT1::System_Collections_Generic_ICollectionT1_Clear()
{
    return this->_class->System_Collections_Generic_ICollectionT1_Clear();
}

// Method : 
template <typename T> 
bool __array<T>::__array_ICollectionT1::System_Collections_Generic_ICollectionT1_Contains(T item)
{
    return this->_class->System_Collections_Generic_ICollectionT1_Contains(item);
}

// Method : 
template <typename T> 
void __array<T>::__array_ICollectionT1::System_Collections_Generic_ICollectionT1_CopyTo(__array<T>* array, int32_t arrayIndex)
{
    return this->_class->System_Collections_Generic_ICollectionT1_CopyTo(array, arrayIndex);
}

// Method : 
template <typename T> 
bool __array<T>::__array_ICollectionT1::System_Collections_Generic_ICollectionT1_Remove(T item)
{
    return this->_class->System_Collections_Generic_ICollectionT1_Remove(item);
}

// Method : 
template <typename T> 
CoreLib::System::Collections::Generic::IEnumeratorT1<T>* __array<T>::__array_ICollectionT1::System_Collections_Generic_IEnumerableT1_GetEnumerator()
{
    return this->_class->System_Collections_Generic_IEnumerableT1_GetEnumerator();
}

// Method : 
template <typename T> 
CoreLib::System::Collections::IEnumerator* __array<T>::__array_ICollectionT1::System_Collections_IEnumerable_GetEnumerator()
{
    return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
CoreLib::System::Collections::Generic::IEnumeratorT1<T>* __array<T>::__array_IEnumerableT1::System_Collections_Generic_IEnumerableT1_GetEnumerator()
{
    return this->_class->System_Collections_Generic_IEnumerableT1_GetEnumerator();
}

// Method : 
template <typename T> 
CoreLib::System::Collections::IEnumerator* __array<T>::__array_IEnumerableT1::System_Collections_IEnumerable_GetEnumerator()
{
    return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
CoreLib::System::Collections::IEnumerator* __array<T>::__array_IEnumerable::System_Collections_IEnumerable_GetEnumerator()
{
    return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
object* __array<T>::__array_IList::System_Collections_IList_get_Item(int32_t index)
{
    return this->_class->System_Collections_IList_get_Item(index);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::System_Collections_IList_set_Item(int32_t index, object* value)
{
    return this->_class->System_Collections_IList_set_Item(index, value);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IList::System_Collections_IList_Add(object* value)
{
    return this->_class->System_Collections_IList_Add(value);
}

// Method : 
template <typename T> 
bool __array<T>::__array_IList::System_Collections_IList_Contains(object* value)
{
    return this->_class->System_Collections_IList_Contains(value);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::System_Collections_IList_Clear()
{
    return this->_class->System_Collections_IList_Clear();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IList::System_Collections_IList_get_IsReadOnly()
{
    return this->_class->System_Collections_IList_get_IsReadOnly();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IList::System_Collections_IList_get_IsFixedSize()
{
    return this->_class->System_Collections_IList_get_IsFixedSize();
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IList::System_Collections_IList_IndexOf(object* value)
{
    return this->_class->System_Collections_IList_IndexOf(value);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::System_Collections_IList_Insert(int32_t index, object* value)
{
    return this->_class->System_Collections_IList_Insert(index, value);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::System_Collections_IList_Remove(object* value)
{
    return this->_class->System_Collections_IList_Remove(value);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::System_Collections_IList_RemoveAt(int32_t index)
{
    return this->_class->System_Collections_IList_RemoveAt(index);
}

// Method : 
template <typename T> 
void __array<T>::__array_IList::System_Collections_ICollection_CopyTo(CoreLib::System::Array* array, int32_t index)
{
    return this->_class->System_Collections_ICollection_CopyTo(array, index);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_IList::System_Collections_ICollection_get_Count()
{
    return this->_class->System_Collections_ICollection_get_Count();
}

// Method : 
template <typename T> 
object* __array<T>::__array_IList::System_Collections_ICollection_get_SyncRoot()
{
    return this->_class->System_Collections_ICollection_get_SyncRoot();
}

// Method : 
template <typename T> 
bool __array<T>::__array_IList::System_Collections_ICollection_get_IsSynchronized()
{
    return this->_class->System_Collections_ICollection_get_IsSynchronized();
}

// Method : 
template <typename T> 
CoreLib::System::Collections::IEnumerator* __array<T>::__array_IList::System_Collections_IEnumerable_GetEnumerator()
{
    return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// Method : 
template <typename T> 
void __array<T>::__array_ICollection::System_Collections_ICollection_CopyTo(CoreLib::System::Array* array, int32_t index)
{
    return this->_class->System_Collections_ICollection_CopyTo(array, index);
}

// Method : 
template <typename T> 
int32_t __array<T>::__array_ICollection::System_Collections_ICollection_get_Count()
{
    return this->_class->System_Collections_ICollection_get_Count();
}

// Method : 
template <typename T> 
object* __array<T>::__array_ICollection::System_Collections_ICollection_get_SyncRoot()
{
    return this->_class->System_Collections_ICollection_get_SyncRoot();
}

// Method : 
template <typename T> 
bool __array<T>::__array_ICollection::System_Collections_ICollection_get_IsSynchronized()
{
    return this->_class->System_Collections_ICollection_get_IsSynchronized();
}

// Method : 
template <typename T> 
CoreLib::System::Collections::IEnumerator* __array<T>::__array_ICollection::System_Collections_IEnumerable_GetEnumerator()
{
    return this->_class->System_Collections_IEnumerable_GetEnumerator();
}

// multi array
// Array
template <typename T, int32_t RANK>
int32_t  __multi_array<T, RANK>::__array_element_size()
{
	return sizeof(T);
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::GetUpperBound(int32_t dimension)
{
	if (dimension >= RANK)
	{
		throw __new<CoreLib::System::IndexOutOfRangeException>();
	}	

    return this->_upperBoundries[dimension] - 1;
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::GetLowerBound(int32_t dimension)
{
	if (dimension >= RANK)
	{
		throw __new<CoreLib::System::IndexOutOfRangeException>();
	}	

	return this->_lowerBoundries[dimension];
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::GetLength(int32_t dimension)
{
	if (dimension >= RANK)
	{
		throw __new<CoreLib::System::IndexOutOfRangeException>();
	}	

    return this->_upperBoundries[dimension] - this->_lowerBoundries[dimension];
}

template <typename T, int32_t RANK>
int32_t __multi_array<T, RANK>::get_Rank()
{
    return RANK;
}
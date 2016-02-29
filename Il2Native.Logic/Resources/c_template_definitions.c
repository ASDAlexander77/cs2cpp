
// Array
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

	auto typedRef = reinterpret_cast<CoreLib::System::TypedReference&>(elemRef);
	typedRef.Value = &this->_data[index];
	// TODO: finish it
	////typedRef.Type = &T::__type;
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

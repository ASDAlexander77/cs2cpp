template <typename T> class __array;

template <typename T> struct __val
{
public: 
	T _value;
	__val() = default;
	inline __val(T value) : _value(value) {}
#if _MSC_VER 
	__val(const <<%assemblyName%>>::System::IntPtr& value);
#else
	template <typename F> __val(const F& value);
#endif
	inline operator T() { return _value; }
};

template <typename T, typename TUnderlying> struct __enum
{
public: 
	TUnderlying _value;
	__enum() = default;
	inline __enum(TUnderlying value) : _value(value) {}
	inline operator TUnderlying() { return _value; }
};

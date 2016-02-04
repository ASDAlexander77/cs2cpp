template <typename T> class __array;

template <typename T> struct __val
{
public: 
	T _value;
	inline __val(T value) : _value(value) {}
	template <typename F> __val(const F& value);
	inline operator T() { return _value; }
};

template <typename T, typename TUnderlying> struct __enum
{
public: 
	TUnderlying _value;
	inline __enum(TUnderlying value) : _value(value) {}
	inline operator TUnderlying() { return _value; }
};

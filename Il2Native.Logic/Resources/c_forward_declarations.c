template <typename T> class __array;

template <typename T, typename TUnderlying> struct __enum
{
public: 
	TUnderlying _value;
	__enum() = default;
	inline __enum(TUnderlying value) : _value(value) {}
	inline operator TUnderlying() { return _value; }
	inline __enum& operator++()
	{
		// actual increment takes place here
		_value++;
		return *this;
	}
	inline __enum operator++(int)
	{
		__enum tmp(*this);
		operator++();
		return tmp;
	}
};

template <typename T> struct __unbound_generic_type
{
};

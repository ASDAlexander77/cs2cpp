#include <cstdio>
#include <cinttypes>
#include <iostream>
#include <functional>

using namespace std;

struct Int32
{
public:
	int32_t m_value;
};

class Object;
class String;

class Object
{
public:
	virtual String* ToString();
};

class String : public Object
{
public:
	const wchar_t* m_str;	
	String(const wchar_t* str);
};

String* Object::ToString()
{
	return new String(L"test");
}

String::String(const wchar_t* str)
{
	m_str = str;
}

class CInt32C : public Object, public Int32
{
public:
	CInt32C() {}
	CInt32C(Int32& i) : Int32(i) {}

	virtual void Print()
	{
		cout << m_value << endl;	
	}

	virtual String* ToString()
	{
		wchar_t buffer[65];
		return new String(_itow(m_value, buffer, 10));	
	}

	int32_t Test()
	{
		cout << "Test cool!" << endl;
		return 10 + Int32::m_value;
	}

	virtual int32_t Test2()
	{
		cout << "Test cool! Virtual" << endl;
		return 20 + Int32::m_value;
	}
};

struct Decimal
{
private:
    double d;
public:
    Decimal() {};
    Decimal(double v) { d = v; };
    Decimal(int32_t v) { d = (double)v; };
    operator int32_t() { return (int32_t)d; };
    explicit operator float() { return (int32_t)d; };
    Decimal* operator ++() { d+=1.0; return this; };
    Decimal* operator ++(int) { return new Decimal(d + 1.0); };
};

template <typename T> struct _val
{
public: 
	T _value;
	_val(T value);
	operator T();
};

template <typename T> _val<T>::_val(T value) : _value(value) {}

template <typename T> _val<T>::operator T() { return _value; }

struct foo { 
    int _len;
    int x[2]; 
    template <typename... T> foo(int len, T... ts) : _len(len), x{ts...} { // note the use of brace-init-list
    } 
};

class Finally
{
public:
	function<void()> _dtor;
	Finally(function<void()> dtor) : _dtor(dtor) {};
	~Finally() { _dtor(); }
};

class DelegatePrototype
{
public:
	function<int32_t ()> method;
	int32_t invoke()
	{
		return method();
	}
};

int main(void)
{
	cout << sizeof(Int32) << endl;
	cout << sizeof(CInt32C) << endl;

	foo* f = new foo(2, 1, 2);

	Finally finally([&]() {
		cout << f->x[0] << endl;
		cout << f->x[1] << endl;
		cout << "Exit";
	});

	CInt32C i;
	i.Int32::m_value = 3;
	CInt32C* pi = &i;

	DelegatePrototype dp;
	dp.method = [&] () {
		return pi->Test();
	};

	cout << "result : " << dp.invoke() << endl;

	dp.method = [&] () {
		return pi->Test2();
	};

	cout << "result virt : " << dp.invoke() << endl;

	return 0;
}
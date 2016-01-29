#include <cstdio>
#include <cinttypes>
#include <iostream>

using namespace std;

class CInt32C
{
	int32_t i;
public:
	CInt32C() : i(3) {}

	int32_t Test()
	{
		cout << "Test cool!" << endl;
		return 10 + i;
	}

	virtual int32_t TestVirt()
	{
		cout << "Test cool! Virtual" << endl;
		return 20 - i;
	}
};

template <class T> struct __union
{
public:
    typedef void ( T::*member_type) ();
    union U 
    {
        void *addr;
	member_type member;
    };

    static void* __addr(member_type member) 
    {
        U u;
        u.member = member;
        return u.addr;
    }

    static void* __addr_of_virtual(T* instance, member_type member) 
    {
        U u;
        u.member = member;
	return reinterpret_cast<void***>(instance)[0][((intptr_t)u.addr - 1) / sizeof(void*)];
    }
};

struct __invoker
{
public:
    typedef int32_t (*this_type_member) (void* __this);

    void* __this;
    void* __member;
 
    __invoker(void* _this, void* _addr) : __this(_this), __member(_addr) {};

    // TO invoke virtual you need to get address from virtual table and cast it to invoker member
    int32_t invoke()
    {
	return (*((this_type_member)__member))(__this);
    }            
};


int main(void)
{
	CInt32C i;

	void* addr = __union<CInt32C>::__addr(reinterpret_cast<__union<CInt32C>::member_type>(&CInt32C::Test));

	__invoker inv(&i, addr);
	auto r = inv.invoke();

	cout << "result of Test = " << r << endl;

	// virtual method
	void* addr_of_virtual = __union<CInt32C>::__addr_of_virtual(&i, reinterpret_cast<__union<CInt32C>::member_type>(&CInt32C::TestVirt));
	
	__invoker invVirt(&i, addr_of_virtual);
	auto rVirt = invVirt.invoke();

	cout << "result of Virtual Test = " << rVirt << endl;

	return 0;
}
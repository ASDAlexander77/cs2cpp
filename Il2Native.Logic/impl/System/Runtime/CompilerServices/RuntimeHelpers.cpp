#include "CoreLib.h"
namespace CoreLib { namespace System { namespace Runtime { namespace CompilerServices { 
    
    // Method : System.Runtime.CompilerServices.RuntimeHelpers.GetObjectValue(object)
    object* RuntimeHelpers::GetObjectValue(object* obj)
    {
        throw 0xC000C000;
    }
    
    // Method : System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(System.RuntimeTypeHandle)
    void RuntimeHelpers::RunClassConstructor(CoreLib::System::RuntimeTypeHandle type)
    {
        throw 0xC000C000;
    }
    
    // Method : System.Runtime.CompilerServices.RuntimeHelpers.OffsetToStringData.get
    int32_t RuntimeHelpers::get_OffsetToStringData()
    {
        return 0;
    }
    
    // Method : System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(object)
    int32_t RuntimeHelpers::GetHashCode(object* o)
    {
        return o->__hash();
    }
    
    // Method : System.Runtime.CompilerServices.RuntimeHelpers.Equals(object, object)
    bool RuntimeHelpers::Equals(object* o1, object* o2)
    {
        return o1->__equals(o2);
    }
}}}}

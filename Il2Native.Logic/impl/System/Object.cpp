#include "CoreLib.h"

// Method : object.GetType()
CoreLib::System::Type* CoreLib::System::Object::GetType()
{
    return this->__get_type();
}

// Method : object.MemberwiseClone()
object* CoreLib::System::Object::MemberwiseClone()
{
    throw 0xC000C000;
}

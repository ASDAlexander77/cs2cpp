#include "CoreLib.h"

namespace CoreLib {
	namespace System {

		namespace _ = ::CoreLib::System;

		// Method : object.GetType()
		_::Type* Object::GetType()
		{
			return this->__get_type();
		}

		// Method : object.MemberwiseClone()
		object* Object::MemberwiseClone()
		{
			return this->__clone();
		}
	}
}
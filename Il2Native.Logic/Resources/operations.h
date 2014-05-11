template<typename T> System::Object * box(T t);
template<> System::Object * box(bool b) { return new System::Boolean(b); }
template<> System::Object * box(int i) { return new System::Int32(i); }

template<typename T> T unbox(::System::Object * o);
template<> bool unbox(System::Object * o) { return *(dynamic_cast<System::Boolean*>(o)); }
template<> int unbox(System::Object * o) { return *(dynamic_cast<System::Int32*>(o)); }

template<typename T> inline T nullCoalescing(T val, T def) { return (val != nullptr) ? val : def; }

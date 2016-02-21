
// Method : System.Activator.CreateInstance<T>()
template <typename T> 
T CoreLib::System::Activator::CreateInstance()
{
	return __create_instance<T>();
}

namespace System
{
    using Globalization;
    using Reflection;

    public sealed class Activator
    {
        public static extern T CreateInstance<T>();

        public static object CreateInstance(Type runtimeType, BindingFlags bindingFlags, Binder binder, object[] providedArgs, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static object CreateInstance(Type runtimeType, bool _dummy)
        {
            throw new NotImplementedException();
        }
    }
}


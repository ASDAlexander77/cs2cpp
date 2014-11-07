namespace System
{
    public sealed class Activator
    {
        public static T CreateInstance<T>() where T : new()
        {
            return new T();
        }
    }
}


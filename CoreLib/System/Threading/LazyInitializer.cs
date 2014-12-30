////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System.Threading
{
    public enum LazyThreadSafetyMode
    {
        None,

        PublicationOnly,

        ExecutionAndPublication
    }

    public static class LazyInitializer
    {
        public static T EnsureInitialized<T>(ref T target) where T : class, new()
        {
            // Fast path.
            if (Volatile.Read<T>(ref target) != null)
            {
                return target;
            }

            return EnsureInitializedCore<T>(ref target, LazyHelpers<T>.s_activatorFactorySelector);
        }

        public static T EnsureInitialized<T>(ref T target, Func<T> valueFactory) where T : class
        {
            // Fast path.
            if (Volatile.Read<T>(ref target) != null)
            {
                return target;
            }

            return EnsureInitializedCore<T>(ref target, valueFactory);
        }

        private static T EnsureInitializedCore<T>(ref T target, Func<T> valueFactory) where T : class
        {
            T value = valueFactory();
            if (value == null)
            {
                throw new InvalidOperationException("InvalidOperation");
            }

            Interlocked.CompareExchange(ref target, value, null);
            return target;
        }

        public static T EnsureInitialized<T>(ref T target, ref int initialized, ref object syncLock) where T : class, new()
        {
            // Fast path.
            if (Volatile.Read(ref initialized) == 1)
            {
                return target;
            }

            return EnsureInitializedCore<T>(ref target, ref initialized, ref syncLock, LazyHelpers<T>.s_activatorFactorySelector);
        }

        public static T EnsureInitialized<T>(ref T target, ref int initialized, ref object syncLock, Func<T> valueFactory)
        {
            // Fast path.
            if (Volatile.Read(ref initialized) == 1)
            {
                return target;
            }


            return EnsureInitializedCore<T>(ref target, ref initialized, ref syncLock, valueFactory);
        }

        private static T EnsureInitializedCore<T>(ref T target, ref int initialized, ref object syncLock, Func<T> valueFactory)
        {
            object slock = syncLock;
            if (slock == null)
            {
                object newLock = new object();
                slock = Interlocked.CompareExchange(ref syncLock, newLock, null);
                if (slock == null)
                {
                    slock = newLock;
                }
            }

            lock (slock)
            {
                if (Volatile.Read(ref initialized) != 1)
                {
                    target = valueFactory();
                    Volatile.Write(ref initialized, 1);
                }
            }

            return target;
        }

    }

    static class LazyHelpers<T> where T : class, new()
    {
        internal static Func<T> s_activatorFactorySelector = new Func<T>(ActivatorFactorySelector);

        private static T ActivatorFactorySelector()
        {
            return new T();
        }
    }
}

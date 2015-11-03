namespace System
{
    partial class Type
    {
        // Given a class handle, this will return the class for that handle.
        internal static RuntimeType GetTypeFromHandleUnsafe(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
        {
            throw new NotImplementedException();
        }
    }
}

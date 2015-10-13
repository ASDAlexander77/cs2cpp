
namespace System
{
    [MergeCode]
    partial class Type
    {
        // Given a class handle, this will return the class for that handle.
        [MergeCode]
        internal static RuntimeType GetTypeFromHandleUnsafe(IntPtr handle)
        {
            throw new NotImplementedException();
        }

        [MergeCode]
        public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
        {
            throw new NotImplementedException();
        }
    }
}

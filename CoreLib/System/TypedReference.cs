////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{
    using System.Runtime.CompilerServices;

    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
    public struct TypedReference 
    {
        private IntPtr Type;

        private IntPtr Value;

        internal bool IsNull
        {
            get
            {
                return Value.IsNull() && Type.IsNull();
            }
        }

        public unsafe static Object ToObject(TypedReference value)
        {
            return InternalToObject(&value);
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        internal unsafe extern static Object InternalToObject(void* value);
    }
}



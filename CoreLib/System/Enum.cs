////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    using System.Reflection;
    using System.Collections;
    using System.Runtime.CompilerServices;

    [Serializable]
    public abstract class Enum : ValueType
    {
        public override String ToString()
        {
            throw new NotImplementedException();
        }

        public static ulong ToUInt64(object getValue)
        {
            throw new NotImplementedException();
        }

        internal static ulong[] InternalGetValues(RuntimeType runtimeType)
        {
            throw new NotImplementedException();
        }

        internal static RuntimeType GetUnderlyingType(RuntimeType parameterType)
        {
            throw new NotImplementedException();
        }

        public object GetValue()
        {
            throw new NotImplementedException();
        }

        internal static object ToObject(RuntimeType runtimeType, ulong @ulong)
        {
            throw new NotImplementedException();
        }

        internal static string[] InternalGetNames(RuntimeType runtimeType)
        {
            throw new NotImplementedException();
        }

        internal static RuntimeType InternalGetUnderlyingType(RuntimeType runtimeType)
        {
            throw new NotImplementedException();
        }
    }
}



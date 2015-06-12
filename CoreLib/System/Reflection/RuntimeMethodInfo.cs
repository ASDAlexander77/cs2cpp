////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System.Reflection
namespace System.Reflection
{

    using System;
    using System.Runtime.CompilerServices;

    // This is defined to support VarArgs
    //typedef ArgIterator  va_list;
    [Serializable()]
    internal sealed class RuntimeMethodInfo : MethodInfo
    {
        public override Type ReturnType
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return this.GetType().FullName;
        }

        public override Type ReflectedType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}   // Namespace



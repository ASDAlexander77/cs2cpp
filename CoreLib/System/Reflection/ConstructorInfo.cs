////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System.Reflection
namespace System.Reflection
{

    using System;
    using System.Runtime.CompilerServices;

    //This class is marked serializable, but it's really the subclasses that
    //are responsible for handling the actual work of serialization if they need it.
    [Serializable()]
    abstract public class ConstructorInfo : MethodBase
    {
        public override MemberTypes MemberType
        {
            get { return System.Reflection.MemberTypes.Constructor; }
        }

        
        public Object Invoke(Object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}



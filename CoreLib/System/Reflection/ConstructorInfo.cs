////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
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

        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public Object Invoke(Object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}



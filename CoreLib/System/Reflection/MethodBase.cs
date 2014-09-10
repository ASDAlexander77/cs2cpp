////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System.Reflection
namespace System.Reflection
{
    using System.Runtime.CompilerServices;
    ////////////////////////////////////////////////////////////////////////////////
    //   Method is the class which represents a Method. These are accessed from
    //   Class through getMethods() or getMethod(). This class contains information
    //   about each method and also allows the method to be dynamically invoked
    //   on an instance.
    ////////////////////////////////////////////////////////////////////////////////
    using System;
    [Serializable()]
    public abstract class MethodBase : MemberInfo
    {
        public bool IsPublic
        {

            get
            {
                throw new NotImplementedException();
            }

        }

        public bool IsStatic
        {

            get
            {
                throw new NotImplementedException();
            }

        }

        public bool IsFinal
        {
            
            get
            {
                throw new NotImplementedException();
            }

        }

        public bool IsVirtual
        {

            get
            {
                throw new NotImplementedException();
            }

        }

        public bool IsAbstract
        {
            
            get
            {
                throw new NotImplementedException();
            }

        }

        
        public Object Invoke(Object obj, Object[] parameters)
        {
            throw new NotImplementedException();
        }

        public override String Name
        {

            get
            {
                throw new NotImplementedException();
            }
        }

        public override Type DeclaringType
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}



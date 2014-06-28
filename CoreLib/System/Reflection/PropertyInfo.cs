////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System.Reflection
namespace System.Reflection
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable()]
    abstract public class PropertyInfo : MemberInfo
    {
        public abstract Type PropertyType
        {
            get;
        }

        
        public virtual Object GetValue(Object obj, Object[] index)
        {
            throw new NotImplementedException();
        }
        
        public virtual void SetValue(Object obj, Object value, Object[] index)
        {
            throw new NotImplementedException();
        }
    }
}



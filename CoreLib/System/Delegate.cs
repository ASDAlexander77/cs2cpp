////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{

    using System;
    using System.Reflection;
    using System.Threading;
    using System.Runtime.CompilerServices;
    [Serializable()]
    public abstract class Delegate
    {


        public override bool Equals(Object obj)
        {
            throw new NotImplementedException();
        }

        
        public static extern Delegate Combine(Delegate a, Delegate b);

        extern public MethodInfo Method
        {
            
            get;
        }

        extern public Object Target
        {
            
            get;
        }

        
        public static extern Delegate Remove(Delegate source, Delegate value);

        
        public static extern bool operator ==(Delegate d1, Delegate d2);

        
        public static extern bool operator !=(Delegate d1, Delegate d2);

    }
}



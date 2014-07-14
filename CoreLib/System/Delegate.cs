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
        private Object obj;
        private IntPtr methodPtr;

        public override bool Equals(Object obj)
        {
            throw new NotImplementedException();
        }


        public static Delegate Combine(Delegate a, Delegate b)
        {
            throw new NotImplementedException();
        }

        public MethodInfo Method
        {

            get
            {
                throw new NotImplementedException();
            }
        }

        public Object Target
        {

            get
            {
                throw new NotImplementedException();
            }
        }

        public static Delegate Remove(Delegate source, Delegate value)
        {
            throw new NotImplementedException();
        }


        public static bool operator ==(Delegate d1, Delegate d2)
        {
            throw new NotImplementedException();
        }


        public static bool operator !=(Delegate d1, Delegate d2)
        {
            throw new NotImplementedException();
        }

    }
}



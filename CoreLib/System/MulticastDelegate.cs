////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Microsoft Corporation.  All rights reserved.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    [Serializable()]
    public abstract class MulticastDelegate : Delegate
    {

        
        public static bool operator ==(MulticastDelegate d1, MulticastDelegate d2)
        {
            throw new NotImplementedException();
        }
        
        public static bool operator !=(MulticastDelegate d1, MulticastDelegate d2)
        {
            throw new NotImplementedException();
        }

    }
}



// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Security.Permissions
{    
    [Serializable]
[System.Runtime.InteropServices.ComVisible(true)]
#if !FEATURE_CAS_POLICY
    // The csharp compiler requires these types to be public, but they are not used elsewhere.
    [Obsolete("SecurityAction is no longer accessible to application code.")]
#endif
    public enum SecurityAction
    {
        // Demand permission of all caller
        Demand = 2,

        // Assert permission so callers don't need
        Assert = 3,

        // Deny permissions so checks will fail
        [Obsolete("Deny is obsolete and will be removed in a future release of the .NET Framework. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
        Deny = 4,

        // Reduce permissions so check will fail
        PermitOnly = 5,

        // Demand permission of caller
        LinkDemand = 6,
    
        // Demand permission of a subclass
        InheritanceDemand = 7,

        // Request minimum permissions to run
        [Obsolete("Assembly level declarative security is obsolete and is no longer enforced by the CLR by default. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
        RequestMinimum = 8,

        // Request optional additional permissions
        [Obsolete("Assembly level declarative security is obsolete and is no longer enforced by the CLR by default. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
        RequestOptional = 9,

        // Refuse to be granted these permissions
        [Obsolete("Assembly level declarative security is obsolete and is no longer enforced by the CLR by default. See http://go.microsoft.com/fwlink/?LinkID=155570 for more information.")]
        RequestRefuse = 10,
    }


[Serializable]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Assembly, AllowMultiple = true, Inherited = false )] 
[System.Runtime.InteropServices.ComVisible(true)]
#if !FEATURE_CAS_POLICY
    // The csharp compiler requires these types to be public, but they are not used elsewhere.
    [Obsolete("SecurityAttribute is no longer accessible to application code.")]
#endif
    public abstract class SecurityAttribute : System.Attribute
    {
        /// <internalonly/>
        internal SecurityAction m_action;
        /// <internalonly/>
        internal bool m_unrestricted;
#if FEATURE_LEGACYNETCF
        public
#else
        protected
#endif
            SecurityAttribute( SecurityAction action ) 
        {
            m_action = action;
        }

        public SecurityAction Action
        {
            get { return m_action; }
            set { m_action = value; }
        }

        public bool Unrestricted
        {
            get { return m_unrestricted; }
            set { m_unrestricted = value; }
        }
    }
}

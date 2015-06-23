// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace System.Security.Permissions
{
    using System;
    using System.Diagnostics.Contracts;

    [Serializable]
    [Flags]
    [System.Runtime.InteropServices.ComVisible(true)]
#if !FEATURE_CAS_POLICY
    // The csharp compiler requires these types to be public, but they are not used elsewhere.
    [Obsolete("SecurityPermissionFlag is no longer accessible to application code.")]
#endif
    public enum SecurityPermissionFlag
    {
        NoFlags = 0x00,
        /* The following enum value is used in the EE (ASSERT_PERMISSION in security.cpp)
         * Should this value change, make corresponding changes there
         */
        Assertion = 0x01,
        UnmanagedCode = 0x02,       // Update vm\Security.h if you change this !
        SkipVerification = 0x04,    // Update vm\Security.h if you change this !
        Execution = 0x08,
        ControlThread = 0x10,
        ControlEvidence = 0x20,
        ControlPolicy = 0x40,
        SerializationFormatter = 0x80,
        ControlDomainPolicy = 0x100,
        ControlPrincipal = 0x200,
        ControlAppDomain = 0x400,
        RemotingConfiguration = 0x800,
        Infrastructure = 0x1000,
        BindingRedirects = 0x2000,
        AllFlags = 0x3fff,
    }

    [System.Runtime.InteropServices.ComVisible(true)]
    [Serializable]
    sealed public class SecurityPermission : SecurityAttribute
    {
#pragma warning disable 618
        private SecurityPermissionFlag m_flags;

        private bool m_controlThread;
#pragma warning restore 618

        //
        // Public Constructors
        //

        public SecurityPermission(SecurityAction action)
            : base(action)
        {
        }

        public SecurityPermission(PermissionState state) : base(SecurityAction.Demand)
        {
            if (state == PermissionState.Unrestricted)
            {
                SetUnrestricted(true);
            }
            else if (state == PermissionState.None)
            {
                SetUnrestricted(false);
                Reset();
            }
            else
            {
                throw new ArgumentException(Environment.GetResourceString("Argument_InvalidPermissionState"));
            }
        }


        // SecurityPermission
        //
#pragma warning disable 618
        public SecurityPermission(SecurityPermissionFlag flag) : base(SecurityAction.Demand)
#pragma warning restore 618
        {
            SetUnrestricted(false);
            m_flags = flag;
        }


        //------------------------------------------------------
        //
        // PRIVATE AND PROTECTED MODIFIERS 
        //
        //------------------------------------------------------


        private void SetUnrestricted(bool unrestricted)
        {
            if (unrestricted)
            {
#pragma warning disable 618
                m_flags = SecurityPermissionFlag.AllFlags;
#pragma warning restore 618
            }
        }

        private void Reset()
        {
#pragma warning disable 618
            m_flags = SecurityPermissionFlag.NoFlags;
#pragma warning restore 618
        }


#pragma warning disable 618
        public SecurityPermissionFlag Flags
#pragma warning restore 618
        {
            set
            {
                m_flags = value;
            }

            get
            {
                return m_flags;
            }
        }

        public bool ControlThread
        {
            set
            {
                m_controlThread = value;
            }

            get
            {
                return m_controlThread;
            }
        }
    }
}

// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// 

namespace System.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Runtime.InteropServices;
    using System.Runtime.CompilerServices;
#if FEATURE_REMOTING
    using System.Runtime.Remoting.Metadata;
#endif //FEATURE_REMOTING
    using System.Security.Permissions;
    using System.Threading;

    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [System.Runtime.InteropServices.ComVisible(true)]
    public class ParameterInfo
    {
        #region Legacy Protected Members
        protected String NameImpl; 
        protected Type ClassImpl; 
        protected int PositionImpl; 
        protected Object DefaultValueImpl; // cannot cache this as it may be non agile user defined enum
        protected MemberInfo MemberImpl;
        #endregion

        #region Legacy Private Members
        // These are here only for backwards compatibility -- they are not set
        // until this instance is serialized, so don't rely on their values from
        // arbitrary code.
#pragma warning disable 169
        private IntPtr _importer;
        private int _token;
        private bool bExtraConstChecked;
#pragma warning restore 169
        #endregion

        #region Constructor
        protected ParameterInfo() 
        { 
        }         
        #endregion

        #region Internal Members
        // this is an internal api for DynamicMethod. A better solution is to change the relationship
        // between ParameterInfo and ParameterBuilder so that a ParameterBuilder can be seen as a writer
        // api over a ParameterInfo. However that is a possible breaking change so it needs to go through some process first
        internal void SetName(String name) 
        {
            NameImpl = name;
        }
        #endregion

        #region Public Methods
        public virtual Type ParameterType 
        { 
            get 
            {
                return ClassImpl;
            } 
        }            
        
        public virtual String Name 
        { 
            get 
            {
                return NameImpl;
            } 
        }

        public virtual bool HasDefaultValue { get { throw new NotImplementedException(); }  }

        public virtual Object DefaultValue { get { throw new NotImplementedException(); } }
        public virtual Object RawDefaultValue  { get { throw new NotImplementedException(); } } 

        public virtual int Position { get { return PositionImpl; } }                                    

        public virtual MemberInfo Member {
            get {
                Contract.Ensures(Contract.Result<MemberInfo>() != null);
                return MemberImpl;
            }
        }

        public virtual Type[] GetRequiredCustomModifiers() 
        {
            return EmptyArray<Type>.Value;
        }

        public virtual Type[] GetOptionalCustomModifiers() 
        {
            return EmptyArray<Type>.Value;
        }
        #endregion

        #region Object Overrides
        public override String ToString()
        {
            return ParameterType.FormatTypeName() + " " + Name;
        }
        #endregion

        public virtual IEnumerable<CustomAttributeData> CustomAttributes
        {
            get
            {
                return GetCustomAttributesData();
            }
        }
        #region ICustomAttributeProvider
        public virtual Object[] GetCustomAttributes(bool inherit)
        {
            return EmptyArray<Object>.Value;
        }

        public virtual Object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            if (attributeType == null)
                throw new ArgumentNullException("attributeType");
            Contract.EndContractBlock();

            return EmptyArray<Object>.Value;
        }

        public virtual bool IsDefined(Type attributeType, bool inherit)
        {
            if (attributeType == null)
                throw new ArgumentNullException("attributeType");
            Contract.EndContractBlock();

            return false;
        }

        public virtual IList<CustomAttributeData> GetCustomAttributesData()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region _ParameterInfo implementation

#if !FEATURE_CORECLR
        void _ParameterInfo.GetTypeInfoCount(out uint pcTInfo)
        {
            throw new NotImplementedException();
        }

        void _ParameterInfo.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
        {
            throw new NotImplementedException();
        }

        void _ParameterInfo.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
        {
            throw new NotImplementedException();
        }

        void _ParameterInfo.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
        {
            throw new NotImplementedException();
        }
#endif

        #endregion
    }
}

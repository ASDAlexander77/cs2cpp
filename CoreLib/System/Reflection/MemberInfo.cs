////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System.Reflection
namespace System.Reflection
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public abstract class MemberInfo
    {
        internal virtual bool CacheEquals(object o) { throw new NotImplementedException(); }

        public abstract MemberTypes MemberType { get; }

        public abstract String Name { get; }

        public abstract Type DeclaringType { get; }

        public abstract Type ReflectedType { get; }

        public virtual IEnumerable<CustomAttributeData> CustomAttributes
        {
            get
            {
                return GetCustomAttributesData();
            }
        }
        public abstract Object[] GetCustomAttributes(bool inherit);

        public abstract Object[] GetCustomAttributes(Type attributeType, bool inherit);

        public abstract bool IsDefined(Type attributeType, bool inherit);

        public virtual IList<CustomAttributeData> GetCustomAttributesData()
        {
            throw new NotImplementedException();
        }

        public virtual int MetadataToken { get { throw new InvalidOperationException(); } }

        public virtual Module Module
        {
            get
            {
                if (this is Type)
                    return ((Type)this).Module;

                throw new NotImplementedException();
            }
        }
    }
}



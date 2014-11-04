////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System.Reflection
namespace System.Reflection
{

    using System;
    using System.Runtime.CompilerServices;

    [Serializable()]
    internal sealed class RuntimeFieldInfo : FieldInfo
    {
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

        public override Type FieldType
        {

            get
            {
                throw new NotImplementedException();
            }
        }

        
        public override Object GetValue(Object obj)
        {
            throw new NotImplementedException();
        }
    }
}



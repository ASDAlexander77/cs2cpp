namespace System
{

    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;

    [Serializable()]
    internal sealed class RuntimeType : Type
    {
        private readonly string fullName;

        public RuntimeType(byte[] rttiData)
        {
            this.fullName = new string(Encoding.ASCII.GetChars(rttiData, 0, rttiData.Length));
        }

        public override MemberTypes MemberType
        {
            get
            {
                return (this.DeclaringType != null) ? MemberTypes.NestedType : MemberTypes.TypeInfo;
            }
        }

        public override Assembly Assembly
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }

        public override String Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override String FullName
        {
            
            get
            {
                return this.fullName;
            }
        }

        public override String AssemblyQualifiedName
        {
            get
            {
                return FullName + ", " + this.Assembly.FullName;
            }
        }

        public override Type BaseType
        {
            
            get
            {
                throw new NotImplementedException();
            }
        }

        
        public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        
        public override FieldInfo GetField(String name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        
        public override FieldInfo[] GetFields(BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        // GetInterfaces
        // This method will return all of the interfaces implemented by a
        //  class
        
        public override Type[] GetInterfaces()
        {
            throw new NotImplementedException();
        }
        ////////////////////////////////////////////////////////////////////////////////////
        //////
        ////// Attributes
        //////
        //////   The attributes are all treated as read-only properties on a class.  Most of
        //////  these boolean properties have flag values defined in this class and act like
        //////  a bit mask of attributes.  There are also a set of boolean properties that
        //////  relate to the classes relationship to other classes and to the state of the
        //////  class inside the runtime.
        //////
        ////////////////////////////////////////////////////////////////////////////////////
        
        public override Type GetElementType()
        {
            throw new NotImplementedException();
        }
    }
}



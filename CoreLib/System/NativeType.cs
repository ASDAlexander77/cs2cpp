namespace System
{
    [Serializable]
    internal class NativeType : Type
    {
        private string name;
        private string fullname;

        internal NativeType()
        {
        }

        public override string Name
        {
            get
            {
                return this.name;
            }
        }

        public override string FullName
        {
            get
            {
                return this.fullname;
            }
        }

        public override string ToString()
        {
            return this.fullname;
        }

        public override Reflection.Assembly Assembly
        {
            get { throw new NotImplementedException(); }
        }

        public override string AssemblyQualifiedName
        {
            get { throw new NotImplementedException(); }
        }

        public override Type BaseType
        {
            get { throw new NotImplementedException(); }
        }

        public override Reflection.MethodInfo[] GetMethods(Reflection.BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Reflection.FieldInfo GetField(string name, Reflection.BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Reflection.FieldInfo[] GetFields(Reflection.BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        public override Type[] GetInterfaces()
        {
            throw new NotImplementedException();
        }

        public override Type GetElementType()
        {
            throw new NotImplementedException();
        }

        protected override bool HasElementTypeImpl()
        {
            throw new NotImplementedException();
        }

        public override Reflection.MemberTypes MemberType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
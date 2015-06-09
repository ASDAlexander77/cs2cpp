namespace System
{

    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;

    internal enum TypeNameFormatFlags
    {
        FormatBasic = 0x00000000, // Not a bitmask, simply the tersest flag settings possible
        FormatNamespace = 0x00000001, // Include namespace and/or enclosing class names in type names
        FormatFullInst = 0x00000002, // Include namespace and assembly in generic types (regardless of other flag settings)
        FormatAssembly = 0x00000004, // Include assembly display name in type names
        FormatSignature = 0x00000008, // Include signature in method names
        FormatNoVersion = 0x00000010, // Suppress version and culture information in all assembly names
#if _DEBUG
        FormatDebug         = 0x00000020, // For debug printing of types only
#endif
        FormatAngleBrackets = 0x00000040, // Whether generic types are C<T> or C[T]
        FormatStubInfo = 0x00000080, // Include stub info like {unbox-stub}
        FormatGenericParam = 0x00000100, // Use !name and !!name for generic type and method parameters

        // If we want to be able to distinguish between overloads whose parameter types have the same name but come from different assemblies,
        // we can add FormatAssembly | FormatNoVersion to FormatSerialization. But we are omitting it because it is not a useful scenario
        // and including the assembly name will normally increase the size of the serialized data and also decrease the performance.
        FormatSerialization = FormatNamespace |
                              FormatGenericParam |
                              FormatFullInst
    }

    internal enum TypeNameKind
    {
        Name,
        ToString,
        SerializationName,
        FullName,
    }

    [Serializable]
    internal sealed class RuntimeType : Type
    {
        private RuntimeTypeCache cache;

        internal RuntimeType()
        {
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

        private RuntimeTypeCache Cache
        {
            get
            {
                if (this.cache == null)
                {
                    Interlocked.CompareExchange<RuntimeTypeCache>(ref this.cache, new RuntimeTypeCache(this), null);
                }

                return this.cache;
            }
        }

        public override String Name
        {
            get
            {
                return GetCachedName(TypeNameKind.Name);
            }
        }

        public override String FullName
        {
            
            get
            {
                throw new NotImplementedException();
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

        protected override bool HasElementTypeImpl()
        {
            return RuntimeTypeHandle.HasElementType(this);
        }

        private string GetCachedName(TypeNameKind kind)
        {
            return Cache.GetName(kind);
        }

        internal class RuntimeTypeCache
        {
            private RuntimeType m_runtimeType;

            private string m_name;

            private string m_fullname;

            private string m_toString;

            private string m_serializationname;

            public RuntimeTypeCache(RuntimeType runtimeType)
            {
                m_runtimeType = runtimeType;
            }

            internal string GetName(TypeNameKind kind)
            {
                switch (kind)
                {
                    case TypeNameKind.Name:
                        // No namespace, full instantiation, and assembly.
                        return ConstructName(ref m_name, TypeNameFormatFlags.FormatBasic);

                    case TypeNameKind.FullName:
                        // We exclude the types that contain generic parameters because their names cannot be roundtripped.
                        // We allow generic type definitions (and their refs, ptrs, and arrays) because their names can be roundtriped.
                        // Theoretically generic types instantiated with generic type definitions can be roundtripped, e.g. List`1<Dictionary`2>.
                        // But these kind of types are useless, rare, and hard to identity. We would need to recursively examine all the 
                        // generic arguments with the same criteria. We will exclude them unless we see a real user scenario.
                        if (!m_runtimeType.GetRootElementType().IsGenericTypeDefinition && m_runtimeType.ContainsGenericParameters)
                            return null;

                        // No assembly.
                        return ConstructName(ref m_fullname, TypeNameFormatFlags.FormatNamespace | TypeNameFormatFlags.FormatFullInst);

                    case TypeNameKind.ToString:
                        // No full instantiation and assembly.
                        return ConstructName(ref m_toString, TypeNameFormatFlags.FormatNamespace);

                    case TypeNameKind.SerializationName:
                        // Use FormatGenericParam in serialization. Otherwise we won't be able 
                        // to distinguish between a generic parameter and a normal type with the same name.
                        // e.g. Foo<T>.Bar(T t), the parameter type T could be !1 or a real type named "T".
                        // Excluding the version number in the assembly name for VTS.
                        return ConstructName(ref m_serializationname, TypeNameFormatFlags.FormatSerialization);

                    default:
                        throw new InvalidOperationException();
                }
            }

            private string ConstructName(ref string name, TypeNameFormatFlags formatFlags)
            {
                if (name == null)
                {
                    name = new RuntimeTypeHandle(m_runtimeType).ConstructName(formatFlags);
                }

                return name;
            }
        }
    }
}



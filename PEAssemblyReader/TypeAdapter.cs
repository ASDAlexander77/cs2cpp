// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeAdapter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// </summary>
    public class TypeAdapter : IType
    {
        /// <summary>
        /// </summary>
        private readonly Type type;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        protected TypeAdapter(Type type)
        {
            Debug.Assert(type != null);
            this.type = type;
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string AssemblyQualifiedName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IType BaseType
        {
            get
            {
                return this.type.BaseType != null ? new TypeAdapter(this.type.BaseType) : null;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool ContainsGenericParameters
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IType DeclaringType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string FullName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int GenericParameterPosition
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IType> GenericTypeArguments
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool HasElementType
        {
            get
            {
                return this.type.HasElementType;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsArray
        {
            get
            {
                return this.type.IsArray;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsByRef
        {
            get
            {
                return this.type.IsByRef;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsClass
        {
            get
            {
                return this.type.IsClass;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsEnum
        {
            get
            {
                return this.type.IsEnum;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsGenericParameter
        {
            get
            {
                return this.type.IsGenericParameter;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsGenericType
        {
            get
            {
                return this.type.IsGenericType;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsGenericTypeDefinition
        {
            get
            {
                return this.type.IsGenericTypeDefinition;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsInterface
        {
            get
            {
                return this.type.IsInterface;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsPointer
        {
            get
            {
                return this.type.IsPointer;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsPrimitive
        {
            get
            {
                return this.type.IsPrimitive;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsValueType
        {
            get
            {
                return this.type.IsValueType;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string Name
        {
            get
            {
                return this.type.Name;
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string Namespace
        {
            get
            {
                return this.type.Namespace;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int CompareTo(object obj)
        {
            var type = obj as IType;
            if (type == null)
            {
                return 1;
            }

            var val = type.Name.CompareTo(this.Name);
            if (val != 0)
            {
                return val;
            }

            val = type.Namespace.CompareTo(this.Namespace);
            if (val != 0)
            {
                return val;
            }

            return 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IConstructor> GetConstructors(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IType GetElementType()
        {
            return new TypeAdapter(this.type.GetElementType());
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IType GetEnumUnderlyingType()
        {
            if (this.type.IsEnum)
            {
                var firstEnumField = this.type.GetFields(BindingFlags.Default).First();
                return new TypeAdapter(firstEnumField.FieldType);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IField> GetFields(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IType> GetGenericArguments()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IType> GetInterfaces()
        {
            return this.type.GetInterfaces().Select(i => new TypeAdapter(i));
        }

        /// <summary>
        /// </summary>
        /// <param name="bindingFlags">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IEnumerable<IMethod> GetMethods(BindingFlags bindingFlags)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public bool IsAssignableFrom(IType type)
        {
            return type.IsDerivedFrom(this);
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        public static IType FromType(Type type)
        {
            return new TypeAdapter(type);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            if (type.IsValueType && type.Namespace == "System")
            {
                return type.Name;
            }

            return this.type.ToString();
        }
    }
}
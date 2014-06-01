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
    using System.Reflection;

    /// <summary>
    /// </summary>
    public class TypeAdapter : IType
    {
        #region Fields

        /// <summary>
        /// </summary>
        private Type type;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        protected TypeAdapter(Type type)
        {
            this.type = type;
        }

        #endregion

        #region Public Properties

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
                throw new NotImplementedException();
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

        #endregion

        #region Public Methods and Operators

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
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
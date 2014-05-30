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
        public Guid GUID
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
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
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FullyDefinedReference.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    using System;
    using System.Diagnostics;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    [DebuggerDisplay("{ToString(),nq} ({Type.ToString(),nq})")]
    public class FullyDefinedReference
    {
        /// <summary>
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <param name="type">
        /// </param>
        /// <exception cref="ArgumentException">
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public FullyDefinedReference(string name, IType type)
            : this(type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("name");
            }

            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            this.Name = name;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        protected FullyDefinedReference(IType type)
        {
            this.Type = type;
        }

        /// <summary>
        /// </summary>
        public IType Type { get; private set; }

        /// <summary>
        /// </summary>
        protected string Name { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public FullyDefinedReference ToByRefType()
        {
            return this.ToType(this.Type.ToByRefType());
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public FullyDefinedReference ToClassType()
        {
            return this.ToType(this.Type.ToClass());
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public FullyDefinedReference ToDereferencedType()
        {
            return this.ToType(this.Type.ToDereferencedType());
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public FullyDefinedReference ToElementType()
        {
            return this.ToType(this.Type.GetElementType());
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public FullyDefinedReference ToNormalType()
        {
            return this.ToType(this.Type.ToNormal());
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public FullyDefinedReference ToPointerType()
        {
            return this.ToType(this.Type.ToPointerType());
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// </summary>
        /// <param name="newType">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual FullyDefinedReference ToType(IType newType)
        {
            return new FullyDefinedReference(this.ToString(), newType);
        }
    }
}
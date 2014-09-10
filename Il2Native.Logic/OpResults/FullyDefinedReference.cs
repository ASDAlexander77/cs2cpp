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

    using PEAssemblyReader;
    using System.Diagnostics;

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
        public override string ToString()
        {
            return this.Name;
        }

        public virtual FullyDefinedReference ToType(IType newType)
        {
            return new FullyDefinedReference(this.ToString(), newType);
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
        public FullyDefinedReference ToElementType()
        {
            return this.ToType(this.Type.GetElementType());
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
        public FullyDefinedReference ToDereferencedType()
        {
            return this.ToType(this.Type.ToDereferencedType());
        }
    }
}
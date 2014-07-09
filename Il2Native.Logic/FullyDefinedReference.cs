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

    /// <summary>
    /// </summary>
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
            this.Type = type;
        }

        /// <summary>
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// </summary>
        public IType Type { get; private set; }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
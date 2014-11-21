// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstValue.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class ConstValue : FullyDefinedReference
    {
        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="type">
        /// </param>
        public ConstValue(bool value, IType type)
            : base(value.ToString().ToLowerInvariant(), type)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <param name="type">
        /// </param>
        public ConstValue(object value, IType type)
            : base(value == null ? "null" : value.ToString(), type)
        {
            this.IsNull = value == null;
        }

        /// <summary>
        /// </summary>
        public bool IsNull { get; private set; }
    }
}
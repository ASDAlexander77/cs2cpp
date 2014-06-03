// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IParameter.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace PEAssemblyReader
{
    /// <summary>
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// </summary>
        string Name { get; }

        /// <summary>
        /// </summary>
        IType ParameterType { get; }
    }
}
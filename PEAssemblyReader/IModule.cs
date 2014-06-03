// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IModule.cs" company="">
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
    public interface IModule
    {
        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        IField ResolveField(int token, IType[] typeGenerics, IType[] methodGenerics);

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        IMember ResolveMember(int token, IType[] typeGenerics, IType[] methodGenerics);

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        IMethod ResolveMethod(int token, IType[] typeGenerics, IType[] methodGenerics);

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <returns>
        /// </returns>
        string ResolveString(int token);

        /// <summary>
        /// </summary>
        /// <param name="token">
        /// </param>
        /// <param name="typeGenerics">
        /// </param>
        /// <param name="methodGenerics">
        /// </param>
        /// <returns>
        /// </returns>
        IType ResolveType(int token, IType[] typeGenerics, IType[] methodGenerics);
    }
}
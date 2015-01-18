// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodsWalker.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class MethodsWalker
    {
        /// <summary>
        /// </summary>
        private readonly IMethod startMethod;

        /// <summary>
        /// </summary>
        /// <param name="startMethod">
        /// </param>
        public MethodsWalker(IMethod startMethod)
        {
            this.startMethod = startMethod;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public HashSet<IType> DiscoverAllStaticFieldsDependencies()
        {
            var calledMethods = new HashSet<IMethod>();
            var readStaticFields = new HashSet<IField>();

            this.WalkMethod(this.startMethod, calledMethods, readStaticFields);

            var typesWithStaticFields = new HashSet<IType>();
            foreach (var staticField in readStaticFields)
            {
                if (staticField.DeclaringType.TypeEquals(this.startMethod.DeclaringType))
                {
                    continue;
                }

                typesWithStaticFields.Add(staticField.DeclaringType);
            }

            return typesWithStaticFields;
        }

        /// <summary>
        /// </summary>
        /// <param name="method">
        /// </param>
        /// <param name="allCalledMethods">
        /// </param>
        /// <param name="allReadStaticFields">
        /// </param>
        private void WalkMethod(IMethod method, HashSet<IMethod> allCalledMethods, HashSet<IField> allReadStaticFields)
        {
            var calledMethods = new HashSet<IMethod>();
            method.DiscoverMethod(null, calledMethods, allReadStaticFields);

            foreach (var nextMethod in calledMethods)
            {
                if (allCalledMethods.Contains(nextMethod))
                {
                    continue;
                }

                allCalledMethods.Add(nextMethod);

                this.WalkMethod(nextMethod, allCalledMethods, allReadStaticFields);
            }
        }
    }
}
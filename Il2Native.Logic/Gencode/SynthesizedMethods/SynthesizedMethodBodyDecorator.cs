// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedMethodBodyDecorator.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Collections.Generic;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedMethodBodyDecorator : IMethodBody
    {
        private readonly IMethodBody methodBody;

        private readonly byte[] customBody;

        public SynthesizedMethodBodyDecorator(IMethodBody methodBody)
        {
            this.methodBody = methodBody;
        }

        public SynthesizedMethodBodyDecorator(IMethodBody methodBody, byte[] customBody)
            : this(methodBody)
        {
            this.customBody = customBody;
        }

        /// <summary>
        /// </summary>
        public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
        {
            get
            {
                return this.methodBody.ExceptionHandlingClauses;
            }
        }

        /// <summary>
        /// </summary>
        public bool HasBody
        {
            get
            {
                return this.customBody != null || this.methodBody.HasBody;
            }
        }

        /// <summary>
        /// </summary>
        public IEnumerable<ILocalVariable> LocalVariables
        {
            get
            {
                return this.methodBody.LocalVariables;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual byte[] GetILAsByteArray()
        {
            return this.customBody ?? this.methodBody.GetILAsByteArray();
        }
    }
}
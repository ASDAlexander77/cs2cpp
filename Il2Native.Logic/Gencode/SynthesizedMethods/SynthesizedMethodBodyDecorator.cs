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
    using System.Linq;
    using System.Collections.Generic;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedMethodBodyDecorator : IMethodBody
    {
        private readonly IMethodBody methodBody;

        private readonly byte[] customBody;

        private readonly IEnumerable<ILocalVariable> locals;

        public SynthesizedMethodBodyDecorator(IMethodBody methodBody)
        {
            this.methodBody = methodBody;
        }

        public SynthesizedMethodBodyDecorator(IMethodBody methodBody, IList<IType> locals, byte[] customBody)
            : this(methodBody)
        {
            this.customBody = customBody;
            var index = 0;
            this.locals = locals.Select(t => new SynthesizedLocalVariable(index++, t)).ToList();
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
                if (this.locals != null)
                {
                    return this.locals;
                }

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
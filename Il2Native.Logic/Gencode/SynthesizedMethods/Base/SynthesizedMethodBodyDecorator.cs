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
    using System.Linq;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedMethodBodyDecorator : IMethodBody
    {
        private readonly byte[] customBody;

        private readonly IEnumerable<ILocalVariable> locals;

        private readonly IEnumerable<IExceptionHandlingClause> exceptionHandlingClauses;

        private readonly IMethodBody methodBody;

        public SynthesizedMethodBodyDecorator(IMethodBody methodBody)
        {
            this.methodBody = methodBody;
        }

        public SynthesizedMethodBodyDecorator(IMethodBody methodBody, IList<IType> locals, IEnumerable<IExceptionHandlingClause> exceptionHandlingClauses, byte[] customBody)
            : this(methodBody)
        {
            this.customBody = customBody;
            var index = 0;
            if (locals != null)
            {
                this.locals = locals.Select(t => new SynthesizedLocalVariable(index++, t)).ToList();
            }
            else
            {
                this.locals = new ILocalVariable[0];
            }

            this.exceptionHandlingClauses = exceptionHandlingClauses;
        }

        /// <summary>
        /// </summary>
        public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
        {
            get
            {
                if (this.exceptionHandlingClauses != null)
                {
                    return exceptionHandlingClauses;
                }

                if (this.methodBody == null)
                {
                    return new IExceptionHandlingClause[0];
                }

                return this.methodBody.ExceptionHandlingClauses;
            }
        }

        /// <summary>
        /// </summary>
        public bool HasBody
        {
            get { return this.customBody != null || this.methodBody.HasBody; }
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
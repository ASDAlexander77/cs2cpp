// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedMethod.cs" company="">
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
    public class SynthesizedMethod : SynthesizedMethodBase
    {
        private IEnumerable<IParameter> parameters;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="methodName">
        /// </param>
        public SynthesizedMethod(string methodName, IType returningType, IEnumerable<IParameter> parameters)
        {
            this.MethodName = methodName;
            this.ReturningType = returningType;
            this.parameters = parameters;
        }

        /// <summary>
        /// </summary>
        public override IType DeclaringType
        {
            get { return null; }
        }

        /// <summary>
        /// </summary>
        public override string ExplicitName
        {
            get { return string.Concat(this.MethodName); }
        }

        /// <summary>
        /// </summary>
        public override string FullName
        {
            get { return string.Concat(this.MethodName); }
        }

        /// <summary>
        /// </summary>
        public override string Name
        {
            get { return this.MethodName; }
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get { return this.ReturningType; }
        }

        /// <summary>
        /// </summary>
        protected string MethodName { get; set; }

        /// <summary>
        /// </summary>
        protected IType ReturningType { get; set; }

        public override System.Collections.Generic.IEnumerable<IParameter> GetParameters()
        {
            return parameters;
        }
    }
}
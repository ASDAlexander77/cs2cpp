// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedMethodTypeBase.cs" company="">
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
    public class SynthesizedMethodStringAdapter : SynthesizedMethodBase
    {
        private readonly IParameter[] parameters;
        private readonly string typeFullName;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="methodName">
        /// </param>
        public SynthesizedMethodStringAdapter(
            string methodName,
            string typeFullName,
            IType returnType,
            IParameter[] parameters = null)
        {
            this.typeFullName = typeFullName;
            this.Type = returnType.ToNormal();
            this.MethodName = methodName;
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
            get { return string.Concat(this.typeFullName, ".", this.MethodName); }
        }

        /// <summary>
        /// </summary>
        public override string FullName
        {
            get { return string.Concat(this.typeFullName, ".", this.MethodName); }
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
            get { return this.Type; }
        }

        /// <summary>
        /// </summary>
        protected string MethodName { get; set; }

        /// <summary>
        /// </summary>
        protected IType Type { get; set; }

        public override IEnumerable<IParameter> GetParameters()
        {
            if (this.parameters == null)
            {
                return new IParameter[0];
            }

            return this.parameters;
        }
    }
}
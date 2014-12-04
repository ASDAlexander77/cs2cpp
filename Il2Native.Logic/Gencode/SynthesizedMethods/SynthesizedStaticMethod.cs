// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedGetHashCodeMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System;
    using System.Linq;
    using System.Reflection;

    using PEAssemblyReader;
    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode.InternalMethods;
    using System.Collections.Generic;

    /// <summary>
    /// </summary>
    public class SynthesizedStaticMethod : SynthesizedMethodTypeBase, IMethodBodyCustomAction
    {
        private IType returnType;
        private IEnumerable<IParameter> parameters;
        private Action<LlvmWriter, OpCodePart> action;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedStaticMethod(string name, IType declaringType, IType returnType, IEnumerable<IType> parameters)
            : base(declaringType, name)
        {
            this.returnType = returnType;
            this.parameters = parameters.Select(p => new SynthesizedValueParameter(p)).ToList();
        }

        public SynthesizedStaticMethod(string name, IType declaringType, IType returnType, IEnumerable<IType> parameters, Action<LlvmWriter, OpCodePart> action)
            : this(name, declaringType, returnType, parameters)
        {
            this.action = action;
            this.IsInline = true;
            this.HasProceduralBody = true;
        }

        /// <summary>
        /// </summary>
        public override CallingConventions CallingConvention
        {
            get
            {
                return CallingConventions.Standard;
            }
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get
            {
                return this.returnType;
            }
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            return this.parameters;
        }

        public void Execute(LlvmWriter writer, OpCodePart opCode)
        {
            if (this.action != null)
            {
                this.action.Invoke(writer, opCode);
            }
        }
    }
}
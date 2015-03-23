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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using CodeParts;
    using InternalMethods;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    // TODO: review usage, prefer using accoring OpCode instead of Inline func.
    public class SynthesizedInlinedTextMethod : SynthesizedMethodTypeBase, IMethodBodyCustomAction
    {
        private readonly Action<CWriter, OpCodePart> action;
        private readonly IEnumerable<IParameter> parameters;
        private readonly IType returnType;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedInlinedTextMethod(
            string name,
            IType declaringType,
            IType returnType)
            : base(declaringType, name)
        {
            this.returnType = returnType;
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedInlinedTextMethod(
            string name,
            IType declaringType,
            IType returnType,
            IEnumerable<IParameter> parameters)
            : this(name, declaringType, returnType)
        {
            this.parameters = parameters.ToList();
        }

        public SynthesizedInlinedTextMethod(
            string name,
            IType declaringType,
            IType returnType,
            IEnumerable<IParameter> parameters,
            Action<CWriter, OpCodePart> action)
            : this(name, declaringType, returnType, parameters)
        {
            this.action = action;
            HasProceduralBody = true;
        }

        /// <summary>
        /// </summary>
        public override CallingConventions CallingConvention
        {
            get { return CallingConventions.Standard; }
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get { return this.returnType; }
        }

        public void Execute(CWriter writer, OpCodePart opCode)
        {
            if (this.action != null)
            {
                this.action.Invoke(writer, opCode);
            }
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            return this.parameters;
        }
    }
}
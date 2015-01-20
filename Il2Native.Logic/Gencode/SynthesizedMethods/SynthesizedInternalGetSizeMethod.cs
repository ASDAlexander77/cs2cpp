// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedInternalGetSizeMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System;
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedInternalGetSizeMethod : SynthesizedMethodTypeBase
    {
        public const string Name = ".internalGetSize";

        /// <summary>
        /// </summary>
        private readonly ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedInternalGetSizeMethod(IType type, ITypeResolver typeResolver)
            : base(type, Name)
        {
            this.typeResolver = typeResolver;
        }

        public override bool IsVirtual
        {
            get
            {
                return true;
            }

            protected set
            {
                throw new NotSupportedException();
            }
        }

        /// <summary>
        /// </summary>
        public override CallingConventions CallingConvention
        {
            get { return CallingConventions.HasThis; }
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get { return this.typeResolver.GetIntTypeByByteSize(LlvmWriter.PointerSize); }
        }
    }
}
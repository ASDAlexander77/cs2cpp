namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Collections.Generic;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedGetTypeMethod : SynthesizedMethodTypeBase
    {
        private IType systemType;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedGetTypeMethod(IType type, IType systemType)
            : base(type, ".getType")
        {
            this.systemType = systemType;
        }

        /// <summary>
        /// </summary>
        public override IType ReturnType
        {
            get
            {
                return this.systemType;
            }
        }
    }
}

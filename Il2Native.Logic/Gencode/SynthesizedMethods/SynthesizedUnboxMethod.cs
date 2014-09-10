namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Reflection;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedUnboxMethod : SynthesizedMethodTypeBase
    {
        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public SynthesizedUnboxMethod(IType type)
            : base(type, ".unbox")
        {
        }

        /// <summary>
        /// </summary>
        public override CallingConventions CallingConvention
        {
            get
            {
                return CallingConventions.HasThis;
            }
        }
    }
}

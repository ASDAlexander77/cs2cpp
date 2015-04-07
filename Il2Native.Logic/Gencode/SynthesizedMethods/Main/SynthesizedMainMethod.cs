// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedMainMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Linq;

    using Il2Native.Logic.Gencode.InlineMethods;
    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedMainMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        private ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedMainMethod(IlCodeBuilder mainCodeBuilder, IMethod main, ITypeResolver typeResolver)
            : base(mainCodeBuilder, "main", main.DeclaringType, null)
        {
            this.typeResolver = typeResolver;
        }

        public override IType DeclaringType
        {
            get
            {
                return null;
            }
        }

        public override IType ReturnType
        {
            get
            {
                return this.typeResolver.System.System_Int32;
            }
        }

        public override System.Collections.Generic.IEnumerable<IParameter> GetParameters()
        {
            if (!this.GetIlCodeBuilder().Parameters.Any())
            {
                yield break;
            }

            yield return this.typeResolver.System.System_Int32.ToParameter("count");
            yield return this.typeResolver.System.System_SByte.ToPointerType().ToPointerType().ToParameter("parameters");
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedBoxMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Linq;

    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedBoxMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        private ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public SynthesizedBoxMethod(IType type, ITypeResolver typeResolver)
            : base(null, ".box", type, ReturningType(type, typeResolver))
        {
            this.typeResolver = typeResolver;
        }

        private static IType ReturningType(IType type, ITypeResolver typeResolver)
        {
            var isNullable = type.TypeEquals(typeResolver.System.System_Nullable_T);
            var returningType = isNullable ? type.GenericTypeArguments.First().ToClass() : type.ToClass();
            return returningType;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return typeResolver.GetBoxMethod(Type, false);
        }
    }
}
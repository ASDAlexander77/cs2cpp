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
    using System.Collections.Generic;
    using System.Diagnostics;
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
            Debug.Assert(!type.UseAsClass, "Normal type should be used");
            this.typeResolver = typeResolver;
        }

        private static IType ReturningType(IType type, ITypeResolver typeResolver)
        {
            var isNullable = type.TypeEquals(typeResolver.System.System_Nullable_T);
            var returningType = isNullable ? type.GenericTypeArguments.First().ToClass() : type.ToClass();
            return returningType;
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            var parameters = base.GetParameters();

            if (typeResolver.GcSupport)
            {
                // add file name and file
                var list = parameters != null ? parameters.ToList() : new List<IParameter>();
                list.Add(typeResolver.System.System_SByte.ToPointerType().ToParameter("__file"));
                list.Add(typeResolver.System.System_Int32.ToParameter("__line"));
                return list;
            }

            return parameters;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return typeResolver.GetBoxMethod(Type, false);
        }
    }
}
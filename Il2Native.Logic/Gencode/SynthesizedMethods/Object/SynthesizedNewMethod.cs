// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedNewMethod.cs" company="">
//   
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Collections.Generic;
    using System.Linq;
    using Il2Native.Logic.Gencode.SynthesizedMethods.Base;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedNewMethod : SynthesizedIlCodeBuilderStaticMethod
    {
        public const string Name = ".new";

        private readonly ITypeResolver typeResolver;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedNewMethod(IType type, ITypeResolver typeResolver)
            : base(null, Name, type, type.ToClass())
        {
            Type = type.ToClass();
            this.typeResolver = typeResolver;
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            var parameters = this.Type.IsArray ? ArrayMultiDimensionGen.GetParameters(this.Type, this.typeResolver) : base.GetParameters();

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
            var codeBuilder = new IlCodeBuilder();
            typeResolver.GetNewMethod(codeBuilder, this.Type);
            return codeBuilder;
        }
    }
}
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
        private ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public SynthesizedBoxMethod(IType type, ICodeWriter codeWriter)
            : base(null, ".box", type, ReturningType(type, codeWriter))
        {
            Debug.Assert(!type.UseAsClass, "Normal type should be used");
            this.codeWriter = codeWriter;
        }

        private static IType ReturningType(IType type, ICodeWriter codeWriter)
        {
            var isNullable = type.TypeEquals(codeWriter.System.System_Nullable_T);
            var returningType = isNullable ? type.GenericTypeArguments.First().ToClass() : type.ToClass();
            return returningType;
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            var parameters = base.GetParameters();

            if (this.codeWriter.GcDebug)
            {
                // add file name and file
                var list = parameters != null ? parameters.ToList() : new List<IParameter>();
                list.Add(this.codeWriter.System.System_SByte.ToPointerType().ToParameter("__file"));
                list.Add(this.codeWriter.System.System_Int32.ToParameter("__line"));
                return list;
            }

            return parameters;
        }

        protected override IlCodeBuilder GetIlCodeBuilder()
        {
            return ObjectInfrastructure.GetBoxMethod(this.codeWriter, Type, false);
        }
    }
}
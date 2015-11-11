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

        private readonly ICodeWriter codeWriter;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="writer">
        /// </param>
        public SynthesizedNewMethod(IType type, ICodeWriter codeWriter)
            : base(null, Name, type, type.ToClass())
        {
            Type = type.ToClass();
            this.codeWriter = codeWriter;
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            var parameters = this.Type.IsArray ? ArrayMultiDimensionGen.GetParameters(this.Type, this.codeWriter) : base.GetParameters();

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
            var codeBuilder = new IlCodeBuilder();
            ObjectInfrastructure.GetNewMethod(this.codeWriter, codeBuilder, this.Type);
            return codeBuilder;
        }
    }
}
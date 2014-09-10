namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Text;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedValueParameter : IParameter
    {
        /// <summary>
        /// </summary>
        private readonly IType type;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        public SynthesizedValueParameter(IType type)
        {
            this.type = type.Clone();
            this.type.UseAsClass = false;
        }

        /// <summary>
        /// </summary>
        public bool IsOut
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsRef
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public string Name
        {
            get
            {
                return "value";
            }
        }

        /// <summary>
        /// </summary>
        public IType ParameterType
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();

            if (this.IsRef)
            {
                result.Append("Ref ");
            }

            if (this.IsOut)
            {
                result.Append("Out ");
            }

            result.Append(this.ParameterType);
            return result.ToString();
        }
    }
}

namespace Il2Native.Logic.Gencode.SynthesizedMethods.SingleDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedSingleDimArrayIListGetEnumeratorMethod : SynthesizedThisMethod
    {
        private readonly IMethodBody _methodBody;

        private readonly IList<IParameter> _parameters;

        private readonly IList<object> _tokenResolutions;

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <param name="typeResolver">
        /// </param>
        public SynthesizedSingleDimArrayIListGetEnumeratorMethod(IType arrayType, ITypeResolver typeResolver)
            : base("GetEnumerator", arrayType, typeResolver.ResolveType("System.Collections.Generic.IEnumerator`1", MetadataGenericContext.Create("T", arrayType.GetElementType())))
        {
            var codeList = new IlCodeBuilder();
            codeList.LoadArgument(0);
            codeList.Add(Code.Newobj, 1);
            codeList.Add(Code.Newobj, 2);
            codeList.Add(Code.Ret);

            var locals = new List<IType>();

            this._methodBody =
                new SynthesizedMethodBodyDecorator(
                    null,
                    locals,
                    codeList.GetCode());

            this._parameters = new List<IParameter>();

            this._tokenResolutions = new List<object>();

            var customGenericContext = MetadataGenericContext.Create("T", arrayType.GetElementType());
            var arraySegmentType = typeResolver.ResolveType("System.ArraySegment`1", customGenericContext);
            this._tokenResolutions.Add(
                IlReader.Constructors(arraySegmentType, typeResolver).First(c => c.GetParameters().Count() == 1));
            this._tokenResolutions.Add(
                IlReader.Constructors(arraySegmentType.GetNestedTypes().First(), typeResolver).First(c => c.GetParameters().Count() == 1));
        }

        public override IEnumerable<IParameter> GetParameters()
        {
            return this._parameters;
        }

        public override IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            return this._methodBody;
        }

        public override IModule Module
        {
            get { return new SynthesizedModuleResolver(null, this._tokenResolutions); }
        }
    }
}

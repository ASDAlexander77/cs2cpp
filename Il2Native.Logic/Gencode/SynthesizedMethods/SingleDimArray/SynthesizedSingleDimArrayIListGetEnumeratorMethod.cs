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
        /// <param name="codeWriterer">
        /// </param>
        public SynthesizedSingleDimArrayIListGetEnumeratorMethod(IType arrayType, ICodeWriter codeWriter)
            : base("GetEnumerator", arrayType, codeWriter.System.System_Collections_Generic_IEnumerator_T.Construct(arrayType.GetElementType()))
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
                    codeList.GetExceptions(),
                    codeList.GetCode());

            this._parameters = new List<IParameter>();

            this._tokenResolutions = new List<object>();

            var arraySegmentType = codeWriter.System.System_ArraySegment_T1.Construct(arrayType.GetElementType());
            this._tokenResolutions.Add(
                IlReader.Constructors(arraySegmentType, codeWriter).First(c => c.GetParameters().Count() == 1));
            this._tokenResolutions.Add(
                IlReader.Constructors(arraySegmentType.GetNestedTypes().First(), codeWriter).First(c => c.GetParameters().Count() == 1));
        }

        /// <summary>
        /// </summary>
        public override bool IsExplicitInterfaceImplementation
        {
            get { return true; }
        }

        /// <summary>
        /// </summary>
        public override bool IsPublic
        {
            get { return false; }
        }

        /// <summary>
        /// </summary>
        public override bool IsInternal
        {
            get { return false; }
        }

        /// <summary>
        /// </summary>
        public override bool IsProtected
        {
            get { return false; }
        }

        /// <summary>
        /// </summary>
        public override bool IsPrivate
        {
            get { return true; }
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

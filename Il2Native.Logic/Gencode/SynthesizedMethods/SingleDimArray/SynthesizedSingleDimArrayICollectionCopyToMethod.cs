namespace Il2Native.Logic.Gencode.SynthesizedMethods.SingleDimArray
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedSingleDimArrayICollectionCopyToMethod : SynthesizedThisMethod
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
        public SynthesizedSingleDimArrayICollectionCopyToMethod(IType arrayType, ITypeResolver typeResolver)
            : base("CopyTo", arrayType, typeResolver.System.System_Void)
        {
            var codeList = new IlCodeBuilder();
            codeList.LoadArgument(0);
            codeList.LoadConstant(0);
            codeList.LoadArgument(1);
            codeList.LoadArgument(2);
            codeList.LoadArgument(0);
            codeList.Add(Code.Ldlen);
            codeList.Call(arrayType.BaseType.GetMethodsByName("Copy", typeResolver).First(m => m.GetParameters().Count() == 5));
            codeList.Add(Code.Ret);

            codeList.Parameters.Add(arrayType.ToParameter("_array"));
            codeList.Parameters.Add(typeResolver.System.System_Int32.ToParameter("arrayIndex"));

            this._methodBody = codeList.GetMethodBody(null);
            this._parameters = codeList.GetParameters();
            this._tokenResolutions = codeList.GetTokenResolutions();
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

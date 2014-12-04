namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using PEAssemblyReader;
    using System.Collections.Generic;

    public class SynthesizedModuleResolver : IModule
    {
        private IMethod method;
        private IList<object> tokenResolutions;

        public SynthesizedModuleResolver(IMethod method, IList<object> tokenResolutions)
        {
            this.method = method;
            this.tokenResolutions = tokenResolutions;
        }

        public IField ResolveField(int token, IGenericContext genericContext)
        {
            return this.method.Module.ResolveField(token, genericContext);
        }

        public IMember ResolveMember(int token, IGenericContext genericContext)
        {
            return this.method.Module.ResolveMember(token, genericContext);
        }

        public IMethod ResolveMethod(int token, IGenericContext genericContext)
        {
            if (tokenResolutions != null && tokenResolutions.Count >= token)
            {
                return tokenResolutions[token - 1] as IMethod;
            }

            return this.method.Module.ResolveMethod(token, genericContext);
        }

        public string ResolveString(int token)
        {
            return this.method.Module.ResolveString(token);
        }

        public object ResolveToken(int token, IGenericContext genericContext)
        {
            return this.method.Module.ResolveToken(token, genericContext);
        }

        public IType ResolveType(int token, IGenericContext genericContext)
        {
            if (tokenResolutions != null && tokenResolutions.Count >= token)
            {
                return tokenResolutions[token - 1] as IType;
            }

            return this.method.Module.ResolveType(token, genericContext);
        }

        public IType ResolveType(string s, IGenericContext genericContext)
        {
            return this.method.Module.ResolveType(s, genericContext);
        }
    }
}

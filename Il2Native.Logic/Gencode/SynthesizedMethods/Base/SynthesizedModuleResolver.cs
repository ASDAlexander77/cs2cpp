namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using PEAssemblyReader;

    public class SynthesizedModuleResolver : IModule
    {
        private readonly IMethod method;
        private readonly IList<object> tokenResolutions;

        public SynthesizedModuleResolver(IMethod method, IList<object> tokenResolutions)
        {
            this.method = method;
            this.tokenResolutions = tokenResolutions;
        }

        public IField ResolveField(int token, IGenericContext genericContext)
        {
            if (this.tokenResolutions != null && this.tokenResolutions.Count >= token)
            {
                return this.tokenResolutions[token - 1] as IField;
            }

            return this.method.Module.ResolveField(token, genericContext);
        }

        public IMember ResolveMember(int token, IGenericContext genericContext)
        {
            if (this.tokenResolutions != null && this.tokenResolutions.Count >= token)
            {
                return this.tokenResolutions[token - 1] as IMember;
            }

            return this.method.Module.ResolveMember(token, genericContext);
        }

        public IMethod ResolveMethod(int token, IGenericContext genericContext)
        {
            if (this.tokenResolutions != null && this.tokenResolutions.Count >= token)
            {
                var method = this.tokenResolutions[token - 1] as IMethod;
                if (genericContext != null)
                {
                    return method.ToSpecialization(genericContext);
                }

                return method;
            }

            return this.method.Module.ResolveMethod(token, genericContext);
        }

        public string ResolveString(int token)
        {
            if (this.tokenResolutions != null && this.tokenResolutions.Count >= token)
            {
                return this.tokenResolutions[token - 1] as string;
            }

            return this.method.Module.ResolveString(token);
        }

        public object ResolveToken(int token, IGenericContext genericContext)
        {
            if (this.tokenResolutions != null && this.tokenResolutions.Count >= token)
            {
                return this.tokenResolutions[token - 1];
            }

            return this.method.Module.ResolveToken(token, genericContext);
        }

        public IType ResolveType(int token, IGenericContext genericContext)
        {
            if (this.tokenResolutions != null && this.tokenResolutions.Count >= token)
            {
                var type = this.tokenResolutions[token - 1] as IType;
                if (type != null && type.IsGenericParameter)
                {
                    Debug.Assert(genericContext != null, "You are using generic without context");
                    return genericContext.ResolveTypeParameter(type);
                }

                return type;
            }

            return this.method.Module.ResolveType(token, genericContext);
        }

        public IType ResolveType(string s, IGenericContext genericContext)
        {
            return this.method.Module.ResolveType(s, genericContext);
        }
    }
}
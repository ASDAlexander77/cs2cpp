// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SynthesizedMethodBase.cs" company="">
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
    using System.Reflection;
    using System.Text;

    using Il2Native.Logic.CodeParts;
    using Il2Native.Logic.Gencode.InternalMethods;

    using Microsoft.CodeAnalysis;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedMethodDecorator : IMethod, IMethodBodyCustomAction, IMethodExtraAttributes
    {
        private readonly IMethodBody methodBody;
        private readonly IModule module;
        private readonly IEnumerable<IParameter> parameters;
        private IMethod method;
        private IGenericContext genericContext;

        public SynthesizedMethodDecorator(
            IMethod method,
            IMethodBody methodBody,
            IEnumerable<IParameter> parameters,
            IModule module)
            : this(method)
        {
            this.methodBody = methodBody;
            this.module = module;
            this.parameters = parameters;
        }

        public SynthesizedMethodDecorator(IMethod method, IGenericContext genericContext)
            : this(method)
        {
            this.genericContext = genericContext;
        }

        public SynthesizedMethodDecorator(IMethod method, IMethodBody methodBody, IGenericContext genericContext)
            : this(method)
        {
            this.methodBody = methodBody;
            this.genericContext = genericContext;
        }

        public string Suffix
        {
            get; set;
        }

        protected SynthesizedMethodDecorator(IMethod method)
        {
            this.method = method;
        }

        public string AssemblyQualifiedName
        {
            get { return this.method.AssemblyQualifiedName; }
        }

        public string AssemblyFullyQualifiedName
        {
            get { return this.method.AssemblyFullyQualifiedName; }
        }

        public IType DeclaringType
        {
            get { return this.method.DeclaringType; }
        }

        public string FullName
        {
            get { return string.Concat(this.method.FullName, this.Suffix); }
        }

        public string MetadataFullName
        {
            get { return string.Concat(this.method.MetadataFullName, this.Suffix); }
        }

        public string MetadataName
        {
            get { return string.Concat(this.method.MetadataName, this.Suffix); }
        }

        public string Name
        {
            get { return string.Concat(this.method.Name, this.Suffix); }
        }

        public string Namespace
        {
            get { return this.method.Namespace; }
        }

        public bool IsAbstract
        {
            get { return this.method.IsAbstract; }
        }

        public bool IsOverride
        {
            get { return this.method.IsOverride; }
        }

        public bool IsStatic
        {
            get { return this.method.IsStatic; }
        }

        public bool IsVirtual
        {
            get { return this.method.IsVirtual; }
        }

        public bool IsAnonymousDelegate
        {
            get { return this.method.IsAnonymousDelegate; }
        }

        public bool IsMerge
        {
            get { return this.method.IsMerge; }
        }

        public IModule Module
        {
            get { return this.module ?? this.method.Module; }
        }

        public int? Token
        {
            get { return this.method.Token; }
        }

        public CallingConventions CallingConvention
        {
            get { return this.method != null ? this.method.CallingConvention : CallingConventions.Standard; }
        }

        public DllImportData DllImportData
        {
            get { return this.method.DllImportData; }
        }

        public string ExplicitName
        {
            get { return string.Concat(this.method.ExplicitName, this.Suffix); }
        }

        public bool IsConstructor
        {
            get { return this.method.IsConstructor; }
        }

        public bool IsDestructor
        {
            get { return this.method.IsDestructor; }
        }

        public bool IsUnmanagedDllImport
        {
            get { return this.method.IsUnmanagedDllImport; }
        }

        public bool IsExplicitInterfaceImplementation
        {
            get { return this.method.IsExplicitInterfaceImplementation; }
        }

        public IType ExplicitInterface
        {
            get { return this.method.ExplicitInterface; }
        }

        public bool IsExternal
        {
            get { return this.method.IsExternal; }
        }

        public bool IsGenericMethod
        {
            get { return this.method.IsGenericMethod; }
        }

        public bool IsGenericMethodDefinition
        {
            get { return this.method.IsGenericMethodDefinition; }
        }

        public bool IsUnmanaged
        {
            get { return this.method.IsUnmanaged; }
        }

        public bool IsUnmanagedMethodReference
        {
            get { return this.method.IsUnmanagedMethodReference; }
        }

        public IType ReturnType
        {
            get
            {
                if (this.genericContext != null)
                {
                    return this.genericContext.ResolveTypeParameter(this.method.ReturnType);
                }

                return this.method.ReturnType;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsInline
        {
            get { return this.method.IsInline; }
        }

        /// <summary>
        /// </summary>
        public bool HasProceduralBody
        {
            get { return this.method.HasProceduralBody; }
        }

        /// <summary>
        /// </summary>
        public bool IsPublic
        {
            get { return this.method.IsPublic; }
        }

        /// <summary>
        /// </summary>
        public bool IsInternal
        {
            get { return this.method.IsInternal; }
        }

        /// <summary>
        /// </summary>
        public bool IsProtected
        {
            get { return method.IsProtected; }
        }

        /// <summary>
        /// </summary>
        public bool IsPrivate
        {
            get { return this.method.IsPrivate; }
        }

        public bool IsStructObjectAdapter { get; set; }

        public IMethod Original
        {
            get { return this.method; }
        }

        public int CompareTo(object obj)
        {
            return this.method.CompareTo(obj);
        }

        public IEnumerable<IType> GetGenericArguments()
        {
            return this.method.GetGenericArguments();
        }

        public IEnumerable<IType> GetGenericParameters()
        {
            return this.method.GetGenericParameters();
        }

        public IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            return this.methodBody ?? this.method.GetMethodBody(genericContext);
        }

        public IMethod GetMethodDefinition()
        {
            var methodClone = (SynthesizedMethodDecorator)this.MemberwiseClone();
            methodClone.method = this.method.GetMethodDefinition();
            return methodClone;
        }

        public IEnumerable<IParameter> GetParameters()
        {
            IEnumerable<IParameter> enumerator;
            if (this.parameters != null)
            {
                enumerator = this.parameters;
            }
            else
            {
                enumerator = this.method.GetParameters();
            }

            if (enumerator == null)
            {
                yield break;
            }

            if (this.genericContext != null)
            {
                foreach (var parameter in enumerator)
                {
                    yield return this.genericContext.ResolveTypeParameter(parameter.ParameterType).ToParameter(parameter.Name, parameter.IsOut, parameter.IsRef);
                }
            }
            else
            {
                foreach (var parameter in enumerator)
                {
                    yield return parameter;
                }                
            }
        }

        public IMethod ToSpecialization(IGenericContext genericContext)
        {
            return this.method.ToSpecialization(genericContext);
        }

        public string ToString(IType ownerOfExplicitInterface, bool shortName = false)
        {
            var result = new StringBuilder();

            // write return type
            result.Append(this.ReturnType);
            result.Append(' ');

            // write Full Name
            if (ownerOfExplicitInterface != null)
            {
                result.Append(ownerOfExplicitInterface.FullName);
                result.Append('.');
            }

            if (shortName)
            {
                result.Append(this.Name);
            }
            else
            {
                result.Append(this.FullName);
            }

            // write Parameter Types
            result.Append('(');
            var index = 0;
            foreach (var parameterType in this.GetParameters())
            {
                if (index != 0)
                {
                    result.Append(", ");
                }

                result.Append(parameterType);
                index++;
            }

            if (CallingConvention.HasFlag(CallingConventions.VarArgs))
            {
                if (index != 0)
                {
                    result.Append(", ");
                }

                result.Append("__arglist");
            }

            result.Append(')');

            return result.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.method.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.method.GetHashCode();
        }

        public override string ToString()
        {
            var result = new StringBuilder();

            // write return type
            result.Append(this.ReturnType);
            result.Append(' ');

            // write Full Name
            result.Append(this.FullName);

            // write Parameter Types
            result.Append('(');
            var index = 0;
            foreach (var parameterType in this.GetParameters())
            {
                if (index != 0)
                {
                    result.Append(", ");
                }

                result.Append(parameterType);
                index++;
            }

            result.Append(')');

            return result.ToString();
        }

        public void Execute(CWriter writer, OpCodePart opCode)
        {
            if (this.method.HasProceduralBody)
            {
                var methodBody = this.method as IMethodBodyCustomAction;
                Debug.Assert(methodBody != null, "should have IMethodBodyCustomAction");
                if (methodBody != null)
                {
                    methodBody.Execute(writer, opCode);
                }
            }
        }
    }
}
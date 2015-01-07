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
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;

    using Microsoft.CodeAnalysis;

    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class SynthesizedMethodBase : IMethod
    {
        /// <summary>
        /// </summary>
        public int? Token { get; private set; }

        /// <summary>
        /// </summary>
        public virtual string AssemblyQualifiedName { get; private set; }

        /// <summary>
        /// </summary>
        public virtual CallingConventions CallingConvention
        {
            get
            {
                return CallingConventions.Standard;
            }
        }

        /// <summary>
        /// </summary>
        public virtual IType DeclaringType
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        public DllImportData DllImportData
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        public IEnumerable<IExceptionHandlingClause> ExceptionHandlingClauses
        {
            get
            {
                return new IExceptionHandlingClause[0];
            }
        }

        /// <summary>
        /// </summary>
        public virtual string ExplicitName
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        public virtual string FullName
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsAbstract { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsConstructor { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsUnmanagedDllImport
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsExplicitInterfaceImplementation
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsExternal
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsGenericMethod { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsGenericMethodDefinition { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsOverride { get; private set; }

        /// <summary>
        /// </summary>
        public virtual bool IsStatic { get; private set; }

        /// <summary>
        /// custom field
        /// </summary>
        public bool IsUnmanaged
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsUnmanagedMethodReference
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsVirtual { get; protected set; }

        /// <summary>
        /// </summary>
        public bool IsAnonymousDelegate { get; private set; }

        /// <summary>
        /// </summary>
        public virtual IEnumerable<ILocalVariable> LocalVariables
        {
            get
            {
                return new ILocalVariable[0];
            }
        }

        /// <summary>
        /// </summary>
        public string MetadataFullName
        {
            get
            {
                return this.FullName;
            }
        }

        /// <summary>
        /// </summary>
        public string MetadataName
        {
            get
            {
                return this.Name;
            }
        }

        /// <summary>
        /// </summary>
        public IModule Module { get; private set; }

        /// <summary>
        /// </summary>
        public virtual string Name
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        public string Namespace { get; private set; }

        /// <summary>
        /// </summary>
        public virtual IType ReturnType
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// </summary>
        public bool IsInline
        {
            get;
            protected set;
        }

        /// <summary>
        /// </summary>
        public bool HasProceduralBody
        {
            get;
            protected set;
        }

        /// <summary>
        /// </summary>
        /// <param name="obj">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public int CompareTo(object obj)
        {
            var name = obj as IName;
            if (name == null)
            {
                return 1;
            }

            return this.FullName.CompareTo(name.FullName);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual IEnumerable<IType> GetGenericArguments()
        {
            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public virtual IEnumerable<IType> GetGenericParameters()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.ToString().CompareTo(obj.ToString()) == 0;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public byte[] GetILAsByteArray()
        {
            return new byte[0];
        }

        /// <summary>
        /// </summary>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        public virtual IMethodBody GetMethodBody(IGenericContext genericContext = null)
        {
            return new SynthesizedDummyMethodBody();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public IMethod GetMethodDefinition()
        {
            return null;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        public virtual IEnumerable<IParameter> GetParameters()
        {
            return new IParameter[0];
        }

        /// <summary>
        /// </summary>
        /// <param name="type">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IType ResolveTypeParameter(IType type)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="genericContext">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public IMethod ToSpecialization(IGenericContext genericContext)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <param name="ownerOfExplicitInterface">
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public string ToString(IType ownerOfExplicitInterface)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
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
    }
}
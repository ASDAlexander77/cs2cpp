// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*=============================================================================
**
**
** 
** 
**
**
** Purpose: For Assembly-related stuff.
**
**
=============================================================================*/

namespace System.Reflection 
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using CultureInfo = System.Globalization.CultureInfo;
    using System.Security;
    using System.Security.Permissions;
    using System.IO;
    using StringBuilder = System.Text.StringBuilder;
    using StackCrawlMark = System.Threading.StackCrawlMark;
    using System.Runtime.InteropServices;
#if FEATURE_SERIALIZATION
    using BinaryFormatter = System.Runtime.Serialization.Formatters.Binary.BinaryFormatter;
#endif // FEATURE_SERIALIZATION
    using System.Runtime.CompilerServices;
    using Microsoft.Win32;
    using System.Threading;
    using __HResults = System.__HResults;
    using System.Runtime.Versioning;
    using System.Diagnostics.Contracts;


    [Serializable]
    [ClassInterface(ClassInterfaceType.None)]
    [System.Runtime.InteropServices.ComVisible(true)]
#pragma warning disable 618
    #if PROTECTION
#if PROTECTION
[PermissionSetAttribute(SecurityAction.InheritanceDemand, Unrestricted = true)]
#endif
#endif
#pragma warning restore 618
    public abstract class Assembly : ICustomAttributeProvider
    {
        #region constructors
        protected Assembly() {}
        #endregion

        #region public static methods

        public static String CreateQualifiedName(String assemblyName, String typeName)
        {
            return typeName + ", " + assemblyName;
        }

        public static Assembly GetAssembly(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            Contract.EndContractBlock();

            Module m = type.Module;
            if (m == null)
                return null;
            else
                return m.Assembly;
        }

#if !FEATURE_CORECLR
        public static bool operator ==(Assembly left, Assembly right)
        {
            if (ReferenceEquals(left, right))
                return true;

            if ((object)left == null || (object)right == null ||
                left is RuntimeAssembly || right is RuntimeAssembly)
            {
                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Assembly left, Assembly right)
        {
            return !(left == right);
        }
#endif // !FEATURE_CORECLR

        public override bool Equals(object o)
        {
            return base.Equals(o);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        /*
         * Get the assembly that the current code is running from.
         */
        [System.Security.SecuritySafeCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable 
        public static Assembly GetExecutingAssembly()
        {
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
            return RuntimeAssembly.GetExecutingAssembly(ref stackMark);
        }
       
        [System.Security.SecuritySafeCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable
        public static Assembly GetCallingAssembly()
        {
            // LookForMyCallersCaller is not guarantee to return the correct stack frame
            // because of inlining, tail calls, etc. As a result GetCallingAssembly is not 
            // ganranteed to return the correct result. We should document it as such.
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCallersCaller;
            return RuntimeAssembly.GetExecutingAssembly(ref stackMark);
        }
       
        [System.Security.SecuritySafeCritical]  // auto-generated
        public static Assembly GetEntryAssembly() {
            throw new NotImplementedException();
        }
    
        #endregion // public static methods

        public virtual String CodeBase
        {
#if FEATURE_CORECLR
            [System.Security.SecurityCritical] // auto-generated
#endif
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual String FullName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual MethodInfo EntryPoint
        {
            get
            {
                throw new NotImplementedException();
            }
        }

#if !FEATURE_CORECLR
        Type _Assembly.GetType()
        {
            return base.GetType();
        }
#endif

        public virtual Type GetType(String name)
        {
            return GetType(name, false, false);
        }

        public virtual Type GetType(String name, bool throwOnError)
        {
            return GetType(name, throwOnError, false);
        }

        public virtual Type GetType(String name, bool throwOnError, bool ignoreCase)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<Type> ExportedTypes
        {
            get
            {
                return GetExportedTypes();
            }
        }

        public virtual Type[] GetExportedTypes()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<TypeInfo> DefinedTypes
        {
            get
            {
                Type[] types = GetTypes();

                TypeInfo[] typeinfos = new TypeInfo[types.Length];

                for (int i = 0; i < types.Length; i++)
                {

                    TypeInfo typeinfo = types[i].GetTypeInfo();
                    if (typeinfo == null)
                        throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoTypeInfo", types[i].FullName));

                    typeinfos[i] = typeinfo;
                }

                return typeinfos;
            }
        }

        public virtual Type[] GetTypes()
        {
            Module[] m = GetModules(false);

            int iNumModules = m.Length;
            int iFinalLength = 0;
            Type[][] ModuleTypes = new Type[iNumModules][];

            for (int i = 0; i < iNumModules; i++)
            {
                ModuleTypes[i] = m[i].GetTypes();
                iFinalLength += ModuleTypes[i].Length;
            }

            int iCurrent = 0;
            Type[] ret = new Type[iFinalLength];
            for (int i = 0; i < iNumModules; i++)
            {
                int iLength = ModuleTypes[i].Length;
                Array.Copy(ModuleTypes[i], 0, ret, iCurrent, iLength);
                iCurrent += iLength;
            }

            return ret;
        }

        // Load a resource based on the NameSpace of the type.
        public virtual Stream GetManifestResourceStream(Type type, String name)
        {
            throw new NotImplementedException();
        }

        public virtual Stream GetManifestResourceStream(String name)
        {
            throw new NotImplementedException();
        }

        public virtual Assembly GetSatelliteAssembly(CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        // Useful for binding to a very specific version of a satellite assembly
        public virtual Assembly GetSatelliteAssembly(CultureInfo culture, Version version)
        {
            throw new NotImplementedException();
        }

#if FEATURE_CAS_POLICY
        public virtual Evidence Evidence
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual PermissionSet PermissionSet
        {
            // SecurityCritical because permissions can contain sensitive information such as paths
            [SecurityCritical]
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsFullyTrusted
        {
            [SecuritySafeCritical]
            get
            {
                return PermissionSet.IsUnrestricted();
            }
        }

        public virtual SecurityRuleSet SecurityRuleSet
        {
            get
            {
                throw new NotImplementedException();
            }
        }

#endif // FEATURE_CAS_POLICY

        [ComVisible(false)]
        public virtual Module ManifestModule
        {
            get
            {
                // This API was made virtual in V4. Code compiled against V2 might use
                // "call" rather than "callvirt" to call it.
                // This makes sure those code still works.
                RuntimeAssembly rtAssembly = this as RuntimeAssembly;
                if (rtAssembly != null)
                    return rtAssembly.ManifestModule;

                throw new NotImplementedException();
            }
        }

        public virtual IEnumerable<CustomAttributeData> CustomAttributes
        {
            get
            {
                return GetCustomAttributesData();
            }
        }
        public virtual Object[] GetCustomAttributes(bool inherit)
        {
            Contract.Ensures(Contract.Result<Object[]>() != null);
            throw new NotImplementedException();
        }

        public virtual Object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            Contract.Ensures(Contract.Result<Object[]>() != null);
            throw new NotImplementedException();
        }

        public virtual bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

#if FEATURE_LEGACYNETCF
        internal virtual bool IsProfileAssembly
        {
            [System.Security.SecurityCritical]
            get
            {
                throw new NotImplementedException();
            }
        }
#endif // FEATURE_LEGACYNETCF

        public virtual IList<CustomAttributeData> GetCustomAttributesData()
        {
            throw new NotImplementedException();
        }

        // To not break compatibility with the V1 _Assembly interface we need to make this
        // new member ComVisible(false).
        [ComVisible(false)]
        public virtual bool ReflectionOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

#if FEATURE_MULTIMODULE_ASSEMBLIES

        public Module LoadModule(String moduleName,
                                 byte[] rawModule)
        {
            return LoadModule(moduleName, rawModule, null);
        }

        public virtual Module LoadModule(String moduleName,
                                 byte[] rawModule,
                                 byte[] rawSymbolStore)
        {
            throw new NotImplementedException();
        }
#endif //FEATURE_MULTIMODULE_ASSEMBLIES

        //
        // Locates a type from this assembly and creates an instance of it using
        // the system activator. 
        //
        public Object CreateInstance(String typeName)
        {
            return CreateInstance(typeName,
                                  false, // ignore case
                                  BindingFlags.Public | BindingFlags.Instance,
                                  null, // binder
                                  null, // args
                                  null, // culture
                                  null); // activation attributes
        }

        public Object CreateInstance(String typeName,
                                     bool ignoreCase)
        {
            return CreateInstance(typeName,
                                  ignoreCase,
                                  BindingFlags.Public | BindingFlags.Instance,
                                  null, // binder
                                  null, // args
                                  null, // culture
                                  null); // activation attributes
        }

        public virtual Object CreateInstance(String typeName, 
                                     bool ignoreCase,
                                     BindingFlags bindingAttr, 
                                     Binder binder,
                                     Object[] args,
                                     CultureInfo culture,
                                     Object[] activationAttributes)
        {
            Type t = GetType(typeName, false, ignoreCase);
            if (t == null) return null;
            return Activator.CreateInstance(t,
                                            bindingAttr,
                                            binder,
                                            args,
                                            culture,
                                            activationAttributes);
        }

        public virtual IEnumerable<Module> Modules
        {
            get
            {
                return GetLoadedModules(true);
            }
        }
                                     
        public Module[] GetLoadedModules()
        {
            return GetLoadedModules(false);
        }

        public virtual Module[] GetLoadedModules(bool getResourceModules)
        {
            throw new NotImplementedException();
        }
                                     
        public Module[] GetModules()
        {
            return GetModules(false);
        }

        public virtual Module[] GetModules(bool getResourceModules)
        {
            throw new NotImplementedException();
        }

        public virtual Module GetModule(String name)
        {
            throw new NotImplementedException();
        }

        // Returns the file in the File table of the manifest that matches the
        // given name.  (Name should not include path.)
#if FEATURE_CORECLR
        [System.Security.SecurityCritical] // auto-generated
#endif
        public virtual FileStream GetFile(String name)
        {
            throw new NotImplementedException();
        }

        public virtual FileStream[] GetFiles()
        {
            return GetFiles(false);
        }

#if FEATURE_CORECLR
        [System.Security.SecurityCritical] // auto-generated
#endif
        public virtual FileStream[] GetFiles(bool getResourceModules)
        {
            throw new NotImplementedException();
        }

        // Returns the names of all the resources
        public virtual String[] GetManifestResourceNames()
        {
            throw new NotImplementedException();
        }

        public override String ToString()
        {
            String displayName = FullName; 
            if (displayName == null)
                return base.ToString();
            else
                return displayName;
        }

        public virtual String Location
        {
#if FEATURE_CORECLR
            [System.Security.SecurityCritical] // auto-generated
#endif
            get
            {
                throw new NotImplementedException();
            }
        }

        // To not break compatibility with the V1 _Assembly interface we need to make this
        // new member ComVisible(false).
        [ComVisible(false)]
        public virtual String ImageRuntimeVersion
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /*
          Returns true if the assembly was loaded from the global assembly cache.
        */        
        public virtual bool GlobalAssemblyCache
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        [ComVisible(false)]
        public virtual Int64 HostContext
        {
            get
            {
                // This API was made virtual in V4. Code compiled against V2 might use
                // "call" rather than "callvirt" to call it.
                // This makes sure those code still works.
                RuntimeAssembly rtAssembly = this as RuntimeAssembly;
                if (rtAssembly != null)
                    return rtAssembly.HostContext;

                throw new NotImplementedException();
            }
        }

        public virtual bool IsDynamic
        {
            get
            {
                return false;
            }
        }
    }

    // Keep this in sync with LOADCTX_TYPE defined in fusionpriv.idl
    internal enum LoadContext
    {
       DEFAULT,
       LOADFROM,
       UNKNOWN,
       HOSTED,
    }

    [Serializable]
    internal class RuntimeAssembly : Assembly
#if !FEATURE_CORECLR
        , ICustomQueryInterface
#endif
    {
#if !FEATURE_CORECLR
        #region ICustomQueryInterface
        [System.Security.SecurityCritical]
        CustomQueryInterfaceResult ICustomQueryInterface.GetInterface([In]ref Guid iid, out IntPtr ppv)
        {
            if (iid == typeof(NativeMethods.IDispatch).GUID)
            {
                ppv = Marshal.GetComInterfaceForObject(this, typeof(_Assembly));
                return CustomQueryInterfaceResult.Handled;
            }

            ppv = IntPtr.Zero;
            return CustomQueryInterfaceResult.NotHandled;
        }
        #endregion
#endif // !FEATURE_CORECLR

#if FEATURE_APPX
        // The highest byte is the flags and the lowest 3 bytes are 
        // the cached ctor token of [DynamicallyInvocableAttribute].
        private enum ASSEMBLY_FLAGS : uint
        {
            ASSEMBLY_FLAGS_UNKNOWN =            0x00000000,
            ASSEMBLY_FLAGS_INITIALIZED =        0x01000000,
            ASSEMBLY_FLAGS_FRAMEWORK =          0x02000000,
            ASSEMBLY_FLAGS_SAFE_REFLECTION =    0x04000000,
            ASSEMBLY_FLAGS_TOKEN_MASK =         0x00FFFFFF,
        }
#endif // FEATURE_APPX

        private const uint COR_E_LOADING_REFERENCE_ASSEMBLY = 0x80131058U;

        internal RuntimeAssembly() { throw new NotSupportedException(); }

        #region private data members
        [method: System.Security.SecurityCritical]
        private string m_fullname;
        private object m_syncRoot;   // Used to keep collectible types alive and as the syncroot for reflection.emit
        private IntPtr m_assembly;    // slack for ptr datum on unmanaged side

#if FEATURE_APPX
        private ASSEMBLY_FLAGS m_flags;
#endif
        #endregion

#if FEATURE_APPX
        internal int InvocableAttributeCtorToken
        {
            get
            {
                int token = (int)(Flags & ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_TOKEN_MASK);

                return token | (int)MetadataTokenType.MethodDef;
            }
        }

        private ASSEMBLY_FLAGS Flags
        {
            [SecuritySafeCritical]
            get
            {
                if ((m_flags & ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_INITIALIZED) == 0)
                {
                    ASSEMBLY_FLAGS flags = ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_UNKNOWN;

#if !FEATURE_CORECLR
                    if (RuntimeAssembly.IsFrameworkAssembly(GetName()))
#else
                    if (IsProfileAssembly)
#endif
                    {
                        flags |= ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_FRAMEWORK | ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_SAFE_REFLECTION;

                        foreach (string name in s_unsafeFrameworkAssemblyNames)
                        {
                            if (String.Compare(GetSimpleName(), name, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                flags &= ~ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_SAFE_REFLECTION;
                                break;
                            }
                        }

                        // Each blessed API will be annotated with a "__DynamicallyInvokableAttribute".
                        // This "__DynamicallyInvokableAttribute" is a type defined in its own assembly.
                        // So the ctor is always a MethodDef and the type a TypeDef.
                        // We cache this ctor MethodDef token for faster custom attribute lookup.
                        // If this attribute type doesn't exist in the assembly, it means the assembly
                        // doesn't contain any blessed APIs.
                        Type invocableAttribute = GetType("__DynamicallyInvokableAttribute", false);
                        if (invocableAttribute != null)
                        {
                            Contract.Assert(((MetadataToken)invocableAttribute.MetadataToken).IsTypeDef);

                            ConstructorInfo ctor = invocableAttribute.GetConstructor(Type.EmptyTypes);
                            Contract.Assert(ctor != null);

                            int token = ctor.MetadataToken;
                            Contract.Assert(((MetadataToken)token).IsMethodDef);

                            flags |= (ASSEMBLY_FLAGS)token & ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_TOKEN_MASK;
                        }
                    }
                    else if (IsDesignerBindingContext())
                    {
                        flags = ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_SAFE_REFLECTION;
                    }

                    m_flags = flags | ASSEMBLY_FLAGS.ASSEMBLY_FLAGS_INITIALIZED;
                }

                return m_flags;
            }
        }
#endif // FEATURE_APPX

        internal object SyncRoot
        {
            get
            {
                if (m_syncRoot == null)
                {
                    Interlocked.CompareExchange<object>(ref m_syncRoot, new object(), null);
                }
                return m_syncRoot;
            }
        }

        private const String s_localFilePrefix = "file:";

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetCodeBase(RuntimeAssembly assembly, 
                                               bool copiedName, 
                                               StringHandleOnStack retString);

        [System.Security.SecurityCritical]  // auto-generated
        internal String GetCodeBase(bool copiedName)
        {
            String codeBase = null;
            GetCodeBase(GetNativeHandle(), copiedName, JitHelpers.GetStringHandleOnStack(ref codeBase));
            return codeBase;
        }

        public override String CodeBase
        {
#if FEATURE_CORECLR
            [System.Security.SecurityCritical] // auto-generated
#else
            [System.Security.SecuritySafeCritical]
#endif
            get {
                String codeBase = GetCodeBase(false);
                VerifyCodeBaseDiscovery(codeBase);
                return codeBase;
            }
        }

        internal RuntimeAssembly GetNativeHandle()
        {
            return this;
        }

#if FEATURE_APTCA
        // This method is called from the VM when creating conditional APTCA exceptions, in order to include
        // the text which must be added to the partial trust visible assembly list
        [SecurityCritical]
        #if PROTECTION
[PermissionSet(SecurityAction.Assert, Unrestricted = true)]
#endif
        private string GetNameForConditionalAptca()
        {
            AssemblyName assemblyName = GetName();
            return assemblyName.GetNameWithPublicKey();

        }
#endif // FEATURE_APTCA

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static void GetFullName(RuntimeAssembly assembly, StringHandleOnStack retString);

        public override String FullName
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            get {
                // If called by Object.ToString(), return val may be NULL.
                if (m_fullname == null)
                {
                    string s = null;
                    GetFullName(GetNativeHandle(), JitHelpers.GetStringHandleOnStack(ref s));
                    Interlocked.CompareExchange<string>(ref m_fullname, s, null);
                }

                return m_fullname;
            }
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetEntryPoint(RuntimeAssembly assembly, ObjectHandleOnStack retMethod);
           
        public override MethodInfo EntryPoint
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            get {
                IRuntimeMethodInfo methodHandle = null;
                GetEntryPoint(GetNativeHandle(), JitHelpers.GetObjectHandleOnStack(ref methodHandle));

                if (methodHandle == null)
                    return null;

                    return (MethodInfo)RuntimeType.GetMethodBase(methodHandle);
            }
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetType(RuntimeAssembly assembly, 
                                                        String name, 
                                                        bool throwOnError, 
                                                        bool ignoreCase,
                                                        ObjectHandleOnStack type);
        
        [System.Security.SecuritySafeCritical]
        public override Type GetType(String name, bool throwOnError, bool ignoreCase) 
        {
            // throw on null strings regardless of the value of "throwOnError"
            if (name == null)
                throw new ArgumentNullException("name");

            RuntimeType type = null;
            GetType(GetNativeHandle(), name, throwOnError, ignoreCase, JitHelpers.GetObjectHandleOnStack(ref type));
            return type;
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        internal extern static void GetForwardedTypes(RuntimeAssembly assembly, ObjectHandleOnStack retTypes);

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static void GetExportedTypes(RuntimeAssembly assembly, ObjectHandleOnStack retTypes); 
        
        [System.Security.SecuritySafeCritical]  // auto-generated
        public override Type[] GetExportedTypes()
        {
            Type[] types = null;
            GetExportedTypes(GetNativeHandle(), JitHelpers.GetObjectHandleOnStack(ref types));
            return types;
        }

        public override IEnumerable<TypeInfo> DefinedTypes
        {
            [System.Security.SecuritySafeCritical]
            get
            {
                List<RuntimeType> rtTypes = new List<RuntimeType>();

                RuntimeModule[] modules = GetModulesInternal(true, false);

                for (int i = 0; i < modules.Length; i++)
                {
                    rtTypes.AddRange(modules[i].GetDefinedTypes());
                }

                return rtTypes.ToArray();
            }
        }

        // Load a resource based on the NameSpace of the type.
        [System.Security.SecuritySafeCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable
        public override Stream GetManifestResourceStream(Type type, String name)
        {
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
            return GetManifestResourceStream(type, name, false, ref stackMark);
        }
    
        [System.Security.SecuritySafeCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable
        public override Stream GetManifestResourceStream(String name)
        {
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
            return GetManifestResourceStream(name, ref stackMark, false);
        }

#if FEATURE_CAS_POLICY
        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static void GetEvidence(RuntimeAssembly assembly, ObjectHandleOnStack retEvidence);

        [SecurityCritical]
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static SecurityRuleSet GetSecurityRuleSet(RuntimeAssembly assembly);

        public override Evidence Evidence
        {
            [SecuritySafeCritical]
            #if PROTECTION
[SecurityPermissionAttribute( SecurityAction.Demand, ControlEvidence = true )]
#endif
            get
            {
                Evidence evidence = EvidenceNoDemand;
                return evidence.Clone();
            }           
        }

        internal Evidence EvidenceNoDemand
        {
            [SecurityCritical]
            get
            {
                Evidence evidence = null;
                GetEvidence(GetNativeHandle(), JitHelpers.GetObjectHandleOnStack(ref evidence));
                return evidence;
            }
        }

        public override PermissionSet PermissionSet
        {
            [SecurityCritical]
            get
            {
                PermissionSet grantSet = null;
                PermissionSet deniedSet = null;

                GetGrantSet(out grantSet, out deniedSet);

                if (grantSet != null)
                {
                    return grantSet.Copy();
                }
                else
                {
                    return new PermissionSet(PermissionState.Unrestricted);
                }
            }
        }

        public override SecurityRuleSet SecurityRuleSet
        {
            [SecuritySafeCritical]
            get
            {
                return GetSecurityRuleSet(GetNativeHandle());
            }
        }
#endif // FEATURE_CAS_POLICY

        public override Module ManifestModule
        {
            get
            {
                // We don't need to return the "external" ModuleBuilder because
                // it is meant to be read-only
                return RuntimeAssembly.GetManifestModule(GetNativeHandle());
            }
        }

        public override Object[] GetCustomAttributes(bool inherit)
        {
            return CustomAttribute.GetCustomAttributes(this, typeof(object) as RuntimeType);
        }
                    
        public override Object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            if (attributeType == null)
                throw new ArgumentNullException("attributeType");
            Contract.EndContractBlock();

            RuntimeType attributeRuntimeType = attributeType.UnderlyingSystemType as RuntimeType;

            if (attributeRuntimeType == null) 
                throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"),"attributeType");

            return CustomAttribute.GetCustomAttributes(this, attributeRuntimeType);
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            if (attributeType == null)
                throw new ArgumentNullException("attributeType");
            Contract.EndContractBlock();

            RuntimeType attributeRuntimeType = attributeType.UnderlyingSystemType as RuntimeType;

            if (attributeRuntimeType == null) 
                throw new ArgumentException(Environment.GetResourceString("Arg_MustBeType"),"caType");

            return CustomAttribute.IsDefined(this, attributeRuntimeType);
        }

        public override IList<CustomAttributeData> GetCustomAttributesData()
        {
            return CustomAttributeData.GetCustomAttributesInternal(this);
        }
        
#if FEATURE_HOSTED_BINDER
        // Wrapper function to wrap the typical use of InternalLoadAssemblyName. Matches exactly with the signature below if FEATURE_HOSTED_BINDER is not set
        [System.Security.SecurityCritical]  // auto-generated
        internal static RuntimeAssembly InternalLoadAssemblyName(
            AssemblyName assemblyRef,
            Evidence assemblySecurity,
            RuntimeAssembly reqAssembly,
            ref StackCrawlMark stackMark,
            bool throwOnFileNotFound,
            bool forIntrospection,
            bool suppressSecurityChecks)
        {
            return InternalLoadAssemblyName(assemblyRef, assemblySecurity, reqAssembly, ref stackMark, IntPtr.Zero, true /*throwOnError*/, forIntrospection, suppressSecurityChecks);
        }
#endif

        // These are the framework assemblies that does reflection invocation
        // on behalf of user code. We allow framework code to invoke non-W8P
        // framework APIs but don't want user code to gain that privilege 
        // through these assemblies. So we blaklist them.
        static string[] s_unsafeFrameworkAssemblyNames = new string[] {
            "System.Reflection.Context",
            "Microsoft.VisualBasic"
        };

#if FEATURE_FUSION
        // used by vm
        [System.Security.SecurityCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable
        private static unsafe RuntimeAssembly LoadWithPartialNameHack(String partialName, bool cropPublicKey)
        {
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
        
            AssemblyName an = new AssemblyName(partialName);
        
            if (!IsSimplyNamed(an))
            {
                if (cropPublicKey)
                {
                    an.SetPublicKey(null);
                    an.SetPublicKeyToken(null);
                }
                
                if(IsFrameworkAssembly(an) || !AppDomain.IsAppXModel())
                {
                    AssemblyName GACAssembly = EnumerateCache(an);
                    if(GACAssembly != null)
                        return InternalLoadAssemblyName(GACAssembly, null, null,ref stackMark, true /*thrownOnFileNotFound*/, false, false);
                    else
                        return null;
                }
            }

            if (AppDomain.IsAppXModel())
            {
                // also try versionless bind from the package
                an.Version = null;
                return nLoad(an, null, null, null, ref stackMark, 
#if FEATURE_HOSTED_BINDER
                       IntPtr.Zero,
#endif
                       false, false, false);
            }
            return null;
            
        }        

#if !FEATURE_CORECLR
        [System.Security.SecurityCritical]  // auto-generated
        internal static RuntimeAssembly LoadWithPartialNameInternal(String partialName, Evidence securityEvidence, ref StackCrawlMark stackMark)
        {
            AssemblyName an = new AssemblyName(partialName);
            return LoadWithPartialNameInternal(an, securityEvidence, ref stackMark);
        }

        [System.Security.SecurityCritical]  // auto-generated
        internal static RuntimeAssembly LoadWithPartialNameInternal(AssemblyName an, Evidence securityEvidence, ref StackCrawlMark stackMark)
        {
            if (securityEvidence != null)
            {
#if FEATURE_CAS_POLICY
                if (!AppDomain.CurrentDomain.IsLegacyCasPolicyEnabled)
                {
                    throw new NotSupportedException(Environment.GetResourceString("NotSupported_RequiresCasPolicyImplicit"));
                }
#endif // FEATURE_CAS_POLICY
                new SecurityPermission(SecurityPermissionFlag.ControlEvidence).Demand();
            }
            
            AppDomain.CheckLoadWithPartialNameSupported(stackMark);

            RuntimeAssembly result = null;
            try {
                result = nLoad(an, null, securityEvidence, null, ref stackMark, 
#if FEATURE_HOSTED_BINDER
                               IntPtr.Zero,
#endif
                               true, false, false);
            }
            catch(Exception e) {
                if (e.IsTransient)
                    throw e;

                if (IsUserError(e))
                    throw;


                if(IsFrameworkAssembly(an) || !AppDomain.IsAppXModel())
                {
                    if (IsSimplyNamed(an))
                        return null;

                    AssemblyName GACAssembly = EnumerateCache(an);
                    if(GACAssembly != null)
                        result = InternalLoadAssemblyName(GACAssembly, securityEvidence, null, ref stackMark, true /*thrownOnFileNotFound*/, false, false);
                }
                else
                {
                    an.Version = null;
                    result = nLoad(an, null, securityEvidence, null, ref stackMark, 
#if FEATURE_HOSTED_BINDER
                                   IntPtr.Zero,
#endif
                                   false, false, false);
                }   
           }


            return result;
        }
#endif // !FEATURE_CORECLR

        [SecuritySafeCritical]
        private static bool IsUserError(Exception e)
        {
            return (uint)e.HResult == COR_E_LOADING_REFERENCE_ASSEMBLY;
        }

        private static bool IsSimplyNamed(AssemblyName partialName)
        {
            byte[] pk = partialName.GetPublicKeyToken();
            if ((pk != null) &&
                (pk.Length == 0))
                return true;

            pk = partialName.GetPublicKey();
            if ((pk != null) &&
                (pk.Length == 0))
                return true;

            return false;
        }        

        [System.Security.SecurityCritical]  // auto-generated
        private static AssemblyName EnumerateCache(AssemblyName partialName)
        {
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();

            partialName.Version = null;

            ArrayList a = new ArrayList();
            Fusion.ReadCache(a, partialName.FullName, ASM_CACHE.GAC);
            
            IEnumerator myEnum = a.GetEnumerator();
            AssemblyName ainfoBest = null;
            CultureInfo refCI = partialName.CultureInfo;

            while (myEnum.MoveNext()) {
                AssemblyName ainfo = new AssemblyName((String)myEnum.Current);

                if (CulturesEqual(refCI, ainfo.CultureInfo)) {
                    if (ainfoBest == null)
                        ainfoBest = ainfo;
                    else {
                        // Choose highest version
                        if (ainfo.Version > ainfoBest.Version)
                            ainfoBest = ainfo;
                    }
                }
            }

            return ainfoBest;
        }

        private static bool CulturesEqual(CultureInfo refCI, CultureInfo defCI)
        {
            bool defNoCulture = defCI.Equals(CultureInfo.InvariantCulture);

            // cultured asms aren't allowed to be bound to if
            // the ref doesn't ask for them specifically
            if ((refCI == null) || refCI.Equals(CultureInfo.InvariantCulture))
                return defNoCulture;

            if (defNoCulture || 
                ( !defCI.Equals(refCI) ))
                return false;

            return true;
        }
#endif // FEATURE_FUSION

        [System.Security.SecurityCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern bool IsReflectionOnly(RuntimeAssembly assembly);

        // To not break compatibility with the V1 _Assembly interface we need to make this
        // new member ComVisible(false).
        [ComVisible(false)]
        public override bool ReflectionOnly
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            get
            {
                return IsReflectionOnly(GetNativeHandle());
            }
        }

#if FEATURE_MULTIMODULE_ASSEMBLIES
        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static void LoadModule(RuntimeAssembly assembly,
                                                      String moduleName,
                                                      byte[] rawModule, int cbModule,
                                                      byte[] rawSymbolStore, int cbSymbolStore,
                                                      ObjectHandleOnStack retModule);

        #if PROTECTION
[SecurityPermissionAttribute(SecurityAction.Demand, ControlEvidence = true)]
#endif
        [System.Security.SecuritySafeCritical]  // auto-generated
        public override Module LoadModule(String moduleName, byte[] rawModule, byte[] rawSymbolStore)
        {
            RuntimeModule retModule = null;
            LoadModule(
                GetNativeHandle(),
                moduleName,
                rawModule,
                (rawModule != null) ? rawModule.Length : 0,
                rawSymbolStore,
                (rawSymbolStore != null) ? rawSymbolStore.Length : 0,
                JitHelpers.GetObjectHandleOnStack(ref retModule));

            return retModule;
        }
#endif //FEATURE_MULTIMODULE_ASSEMBLIES

        // Returns the module in this assembly with name 'name'

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetModule(RuntimeAssembly assembly, String name, ObjectHandleOnStack retModule);

        [System.Security.SecuritySafeCritical]  // auto-generated
        public override Module GetModule(String name)
        {
            Module retModule = null;
            GetModule(GetNativeHandle(), name, JitHelpers.GetObjectHandleOnStack(ref retModule));
            return retModule;
        }

        // Returns the file in the File table of the manifest that matches the
        // given name.  (Name should not include path.)
#if FEATURE_CORECLR
        [System.Security.SecurityCritical] // auto-generated
#else
        [System.Security.SecuritySafeCritical]
#endif
        public override FileStream GetFile(String name)
        {
            RuntimeModule m = (RuntimeModule)GetModule(name);
            if (m == null)
                return null;

            return new FileStream(m.GetFullyQualifiedName(),
                                  FileMode.Open,
                                  FileAccess.Read, FileShare.Read, FileStream.DefaultBufferSize, false);
        }

#if FEATURE_CORECLR
        [System.Security.SecurityCritical] // auto-generated
#else
        [System.Security.SecuritySafeCritical]
#endif
        public override FileStream[] GetFiles(bool getResourceModules)
        {
            Module[] m = GetModules(getResourceModules);
            int iLength = m.Length;
            FileStream[] fs = new FileStream[iLength];

            for(int i = 0; i < iLength; i++)
                fs[i] = new FileStream(((RuntimeModule)m[i]).GetFullyQualifiedName(),
                                       FileMode.Open,
                                       FileAccess.Read, FileShare.Read, FileStream.DefaultBufferSize, false);

            return fs;
        }


        // Returns the names of all the resources
        [System.Security.SecurityCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern String[] GetManifestResourceNames(RuntimeAssembly assembly);

        // Returns the names of all the resources
        [System.Security.SecuritySafeCritical]  // auto-generated
        public override String[] GetManifestResourceNames()
        {
            return GetManifestResourceNames(GetNativeHandle());
        }
    
            
        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static void GetExecutingAssembly(StackCrawlMarkHandle stackMark, ObjectHandleOnStack retAssembly);

        [System.Security.SecurityCritical]  // auto-generated
        internal static RuntimeAssembly GetExecutingAssembly(ref StackCrawlMark stackMark)
        {
            RuntimeAssembly retAssembly = null;
            GetExecutingAssembly(JitHelpers.GetStackCrawlMarkHandle(ref stackMark), JitHelpers.GetObjectHandleOnStack(ref retAssembly));
            return retAssembly;
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetLocation(RuntimeAssembly assembly, StringHandleOnStack retString);

        public override String Location
        {
#if FEATURE_CORECLR
            [System.Security.SecurityCritical] // auto-generated
#else
            [System.Security.SecuritySafeCritical]
#endif
            get {
                String location = null;

                GetLocation(GetNativeHandle(), JitHelpers.GetStringHandleOnStack(ref location));

                return location;
            }
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static void GetImageRuntimeVersion(RuntimeAssembly assembly, StringHandleOnStack retString);

        // To not break compatibility with the V1 _Assembly interface we need to make this
        // new member ComVisible(false).
        [ComVisible(false)]
        public override String ImageRuntimeVersion
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            get{
                String s = null;
                GetImageRuntimeVersion(GetNativeHandle(), JitHelpers.GetStringHandleOnStack(ref s));
                return s;
            }
        }


        [System.Security.SecurityCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private extern static bool IsGlobalAssemblyCache(RuntimeAssembly assembly);

        public override bool GlobalAssemblyCache
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            get
            {
                return IsGlobalAssemblyCache(GetNativeHandle());
            }
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static Int64 GetHostContext(RuntimeAssembly assembly);

        public override Int64 HostContext
        {
            [System.Security.SecuritySafeCritical]  // auto-generated
            get
            {
                return GetHostContext(GetNativeHandle());
            }
        }

        private static String VerifyCodeBase(String codebase)
        {
            if(codebase == null)
                return null;

            int len = codebase.Length;
            if (len == 0)
                return null;


            int j = codebase.IndexOf(':');
            // Check to see if the url has a prefix
            if( (j != -1) &&
                (j+2 < len) &&
                ((codebase[j+1] == '/') || (codebase[j+1] == '\\')) &&
                ((codebase[j+2] == '/') || (codebase[j+2] == '\\')) )
                return codebase;
#if !PLATFORM_UNIX
            else if ((len > 2) && (codebase[0] == '\\') && (codebase[1] == '\\'))
                return "file://" + codebase;
            else
                return "file:///" + Path.GetFullPathInternal( codebase );
#else
            else
                return "file://" + Path.GetFullPathInternal( codebase );
#endif // !PLATFORM_UNIX
        }

        [System.Security.SecurityCritical]  // auto-generated
        internal Stream GetManifestResourceStream(
            Type type,
            String name,
            bool skipSecurityCheck,
            ref StackCrawlMark stackMark)
        {
            StringBuilder sb = new StringBuilder();
            if(type == null) {
                if (name == null)
                    throw new ArgumentNullException("type");
            }
            else {
                String nameSpace = type.Namespace;
                if(nameSpace != null) {
                    sb.Append(nameSpace);
                    if(name != null) 
                        sb.Append(Type.Delimiter);
                }
            }

            if(name != null)
                sb.Append(name);
    
            return GetManifestResourceStream(sb.ToString(), ref stackMark, skipSecurityCheck);
        }

#if FEATURE_CAS_POLICY
        internal bool IsStrongNameVerified
        {
            [System.Security.SecurityCritical]  // auto-generated
            get { return GetIsStrongNameVerified(GetNativeHandle()); }
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern bool GetIsStrongNameVerified(RuntimeAssembly assembly);
#endif // FEATURE_CAS_POLICY

        // GetResource will return a pointer to the resources in memory.
        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static unsafe extern byte* GetResource(RuntimeAssembly assembly,
                                                       String resourceName,
                                                       out ulong length,
                                                       StackCrawlMarkHandle stackMark,
                                                       bool skipSecurityCheck);

        [System.Security.SecurityCritical]  // auto-generated
        internal unsafe Stream GetManifestResourceStream(String name, ref StackCrawlMark stackMark, bool skipSecurityCheck)
        {
            ulong length = 0;
            byte* pbInMemoryResource = GetResource(GetNativeHandle(), name, out length, JitHelpers.GetStackCrawlMarkHandle(ref stackMark), skipSecurityCheck);

            if (pbInMemoryResource != null) {
                //Console.WriteLine("Creating an unmanaged memory stream of length "+length);
                if (length > Int64.MaxValue)
                    throw new NotImplementedException(Environment.GetResourceString("NotImplemented_ResourcesLongerThan2^63"));

                return new UnmanagedMemoryStream(pbInMemoryResource, (long)length, (long)length, FileAccess.Read, true);
            }

            //Console.WriteLine("GetManifestResourceStream: Blob "+name+" not found...");
            return null;
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetVersion(RuntimeAssembly assembly, 
                                              out int majVer, 
                                              out int minVer, 
                                              out int buildNum,
                                              out int revNum);

        [System.Security.SecurityCritical]  // auto-generated
        internal Version GetVersion()
        {
            int majorVer, minorVer, build, revision;
            GetVersion(GetNativeHandle(), out majorVer, out minorVer, out build, out revision);
            return new Version (majorVer, minorVer, build, revision);
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetLocale(RuntimeAssembly assembly, StringHandleOnStack retString);

        [System.Security.SecurityCritical]  // auto-generated
        internal CultureInfo GetLocale()
        {
            String locale = null;

            GetLocale(GetNativeHandle(), JitHelpers.GetStringHandleOnStack(ref locale));

            if (locale == null)
                return CultureInfo.InvariantCulture;

            return new CultureInfo(locale);
        }

        [System.Security.SecurityCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        private static extern bool FCallIsDynamic(RuntimeAssembly assembly);

        public override bool IsDynamic
        {
            [SecuritySafeCritical]
            get {
                return FCallIsDynamic(GetNativeHandle());
            }
        }

        [System.Security.SecurityCritical]  // auto-generated
        private void VerifyCodeBaseDiscovery(String codeBase)
        {
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetSimpleName(RuntimeAssembly assembly, StringHandleOnStack retSimpleName);

        [SecuritySafeCritical]
        internal String GetSimpleName()
        {
            string name = null;
            GetSimpleName(GetNativeHandle(), JitHelpers.GetStringHandleOnStack(ref name));
            return name;
        }
        
        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static AssemblyNameFlags GetFlags(RuntimeAssembly assembly);

        [System.Security.SecurityCritical]  // auto-generated
        private AssemblyNameFlags GetFlags()
        {
            return GetFlags(GetNativeHandle());
        }

        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetRawBytes(RuntimeAssembly assembly, ObjectHandleOnStack retRawBytes);

        // Get the raw bytes of the assembly
        [SecuritySafeCritical]
        internal byte[] GetRawBytes()
        {
            byte[] rawBytes = null;

            GetRawBytes(GetNativeHandle(), JitHelpers.GetObjectHandleOnStack(ref rawBytes));
            return rawBytes;
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private static extern void GetPublicKey(RuntimeAssembly assembly, ObjectHandleOnStack retPublicKey);

        [System.Security.SecurityCritical]  // auto-generated
        internal byte[] GetPublicKey()
        {
            byte[] publicKey = null;
            GetPublicKey(GetNativeHandle(), JitHelpers.GetObjectHandleOnStack(ref publicKey));
            return publicKey;
        }

        [SecurityCritical]
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static void GetGrantSet(RuntimeAssembly assembly, ObjectHandleOnStack granted, ObjectHandleOnStack denied);

#if FEATURE_LEGACYNETCF
        [System.Security.SecurityCritical]
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetIsProfileAssembly(RuntimeAssembly assembly);

        // True if the assembly is a trusted platform assembly
        internal override bool IsProfileAssembly
        {
            [System.Security.SecurityCritical]
            get
            {
                return GetIsProfileAssembly(GetNativeHandle());
            }
        }
#endif // FEATURE_LEGACYNETCF

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool IsAllSecurityCritical(RuntimeAssembly assembly);

        // Is everything introduced by this assembly critical
        [System.Security.SecuritySafeCritical]  // auto-generated
        internal bool IsAllSecurityCritical()
        {
            return IsAllSecurityCritical(GetNativeHandle());
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool IsAllSecuritySafeCritical(RuntimeAssembly assembly);

        // Is everything introduced by this assembly safe critical
        [System.Security.SecuritySafeCritical]  // auto-generated
        internal bool IsAllSecuritySafeCritical()
        {
            return IsAllSecuritySafeCritical(GetNativeHandle());
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool IsAllPublicAreaSecuritySafeCritical(RuntimeAssembly assembly);

        // Is everything introduced by this assembly safe critical
        [System.Security.SecuritySafeCritical]  // auto-generated
        internal bool IsAllPublicAreaSecuritySafeCritical()
        {
            return IsAllPublicAreaSecuritySafeCritical(GetNativeHandle());
        }

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool IsAllSecurityTransparent(RuntimeAssembly assembly);

        // Is everything introduced by this assembly transparent
        [System.Security.SecuritySafeCritical]  // auto-generated
        internal bool IsAllSecurityTransparent()
        {
            return IsAllSecurityTransparent(GetNativeHandle());
        }

        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable  
        public override Assembly GetSatelliteAssembly(CultureInfo culture)
        {
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
            return InternalGetSatelliteAssembly(culture, null, ref stackMark);
        }

        // Useful for binding to a very specific version of a satellite assembly
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable  
        public override Assembly GetSatelliteAssembly(CultureInfo culture, Version version)
        {
            StackCrawlMark stackMark = StackCrawlMark.LookForMyCaller;
            return InternalGetSatelliteAssembly(culture, version, ref stackMark);
        }

        [System.Security.SecuritySafeCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.NoInlining)] // Methods containing StackCrawlMark local var has to be marked non-inlineable  
        internal Assembly InternalGetSatelliteAssembly(CultureInfo culture,
                                                       Version version,
                                                       ref StackCrawlMark stackMark)
        {
            if (culture == null)
                throw new ArgumentNullException("culture");
            Contract.EndContractBlock();


            String name = GetSimpleName() + ".resources";
            return InternalGetSatelliteAssembly(name, culture, version, true, ref stackMark);
        }

#if FEATURE_CORECLR
        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        static internal extern unsafe void nLoadFromUnmanagedArray(bool fIntrospection, 
                                                                            byte* assemblyContent, 
                                                                            ulong assemblySize,
                                                                            byte* pdbContent, 
                                                                            ulong pdbSize,
                                                                            StackCrawlMarkHandle stackMark,
                                                                            ObjectHandleOnStack retAssembly);
#endif

        [System.Security.SecurityCritical]  // auto-generated
        [DllImport(JitHelpers.QCall, CharSet = CharSet.Unicode)]
        [SuppressUnmanagedCodeSecurity]
        private extern static void GetModules(RuntimeAssembly assembly, 
                                              bool loadIfNotFound, 
                                              bool getResourceModules, 
                                              ObjectHandleOnStack retModuleHandles);
        
        [System.Security.SecuritySafeCritical]  // auto-generated
        private RuntimeModule[] GetModulesInternal(bool loadIfNotFound,
                                     bool getResourceModules)
        {
            RuntimeModule[] modules = null;
            GetModules(GetNativeHandle(), loadIfNotFound, getResourceModules, JitHelpers.GetObjectHandleOnStack(ref modules));
            return modules;
        }

        public override Module[] GetModules(bool getResourceModules)
        {
            return GetModulesInternal(true, getResourceModules);
        }

        public override Module[] GetLoadedModules(bool getResourceModules)
        {
            return GetModulesInternal(false, getResourceModules);
        }

        [System.Security.SecuritySafeCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal static extern RuntimeModule GetManifestModule(RuntimeAssembly assembly);

#if FEATURE_APTCA
        [System.Security.SecuritySafeCritical]
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal static extern bool AptcaCheck(RuntimeAssembly targetAssembly, RuntimeAssembly sourceAssembly);
#endif // FEATURE_APTCA

        [System.Security.SecurityCritical]  // auto-generated
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        internal static extern int GetToken(RuntimeAssembly assembly);
    }
}

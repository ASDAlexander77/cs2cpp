//-----------------------------------------------------------------------------
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace System.Resources
{
    public class ResourceManager
    {
        internal const string s_fileExtension = ".tinyresources";
        internal const string s_resourcesExtension = ".resources";

        private int m_resourceFileId;
        private Assembly m_assembly;
        private Assembly m_baseAssembly;
        private string m_baseName;
        internal string m_cultureName;
        private ResourceManager m_rmFallback;

        
        private int FindResource(string baseName, Assembly assembly)
        {
            throw new NotImplementedException();
        }

        
        private object GetObjectInternal(short id)
        {
            throw new NotImplementedException();
        }

        
        private object GetObjectInternal(short id, int offset, int length)
        {
            throw new NotImplementedException();
        }

        public ResourceManager(string baseName, Assembly assembly)
            : this(baseName, assembly, CultureInfo.CurrentUICulture.Name, true)
        {
        }

        internal ResourceManager(string baseName, Assembly assembly, string cultureName, bool fThrowOnFailure)
        {
            if (fThrowOnFailure)
            {
                throw new ArgumentException();
            }
        }

        internal ResourceManager(string baseName, string cultureName, int iResourceFileId, Assembly assemblyBase, Assembly assemblyResource)
        {
            //found resource
            this.m_baseAssembly = assemblyBase;
            this.m_assembly = assemblyResource;
            this.m_baseName = baseName;
            this.m_cultureName = cultureName;
            this.m_resourceFileId = iResourceFileId;
        }

        private bool IsValid
        {
            get { return m_resourceFileId >= 0; }
        }

        private string GetParentCultureName(string cultureName)
        {
            int iDash = cultureName.LastIndexOf('-');
            if (iDash < 0)
                cultureName = "";
            else
                cultureName = cultureName.Substring(0, iDash);

            return cultureName;
        }

        internal bool Initialize(string baseName, Assembly assemblyBase, string cultureName, Assembly assemblyResource)
        {
            while (true)
            {
                string resourceName = baseName;
                bool fInvariantCulture = (cultureName == "");

                if (!fInvariantCulture)
                {
                    resourceName = baseName + "." + cultureName;
                }

                resourceName = resourceName + s_fileExtension;

                int iResourceFileId = FindResource(resourceName, assemblyResource);

                if (iResourceFileId >= 0)
                {
                    //found resource
                    this.m_baseAssembly = assemblyBase;
                    this.m_assembly = assemblyResource;
                    this.m_baseName = baseName;
                    this.m_cultureName = cultureName;
                    this.m_resourceFileId = iResourceFileId;

                    break;
                }
                else if (fInvariantCulture)
                {
                    break;
                }

                cultureName = GetParentCultureName(cultureName);
            }

            return this.IsValid;
        }

        private object GetObjectFromId(short id)
        {
            ResourceManager rm = this;

            while (rm != null)
            {
                object obj = rm.GetObjectInternal(id);

                if (obj != null)
                    return obj;

                if (rm.m_rmFallback == null)
                {
                    if (rm.m_cultureName != "")
                    {
                        string cultureNameParent = GetParentCultureName(rm.m_cultureName);
                        ResourceManager rmFallback = new ResourceManager(m_baseName, m_baseAssembly, cultureNameParent, false);

                        if (rmFallback.IsValid)
                            rm.m_rmFallback = rmFallback;
                    }
                }

                rm = rm.m_rmFallback;
            }

            throw new ArgumentException();
        }

        private object GetObjectChunkFromId(short id, int offset, int length)
        {
            ResourceManager rm = this;

            while (rm != null)
            {
                object obj = rm.GetObjectInternal(id, offset, length);

                if (obj != null)
                    return obj;

                if (rm.m_rmFallback == null)
                {
                    if (rm.m_cultureName != "")
                    {
                        string cultureNameParent = GetParentCultureName(rm.m_cultureName);
                        ResourceManager rmFallback = new ResourceManager(m_baseName, m_baseAssembly, cultureNameParent, false);

                        if (rmFallback.IsValid)
                            rm.m_rmFallback = rmFallback;
                    }
                }

                rm = rm.m_rmFallback;
            }

            throw new ArgumentException();
        }
        
        internal static object GetObject(System.Resources.ResourceManager rm, Enum id)
        {
            throw new NotImplementedException();
        }

        internal object GetObject(string name, CultureInfo info)
        {
            throw new NotImplementedException();
        }

        public string GetString(string name, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}



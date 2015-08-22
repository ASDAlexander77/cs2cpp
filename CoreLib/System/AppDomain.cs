////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Reflection;
using System.Threading;
using System.Runtime.CompilerServices;

namespace System
{
    public sealed class AppDomainSetup
    {
        public string TargetFrameworkName
        {
            get
            {
                return ".NETFramework,Version=v4.0";
            }
        }
    }

    public sealed class AppDomain : MarshalByRefObject
    {
        [System.Reflection.FieldNoReflection]
        private object m_appDomain;

        private string m_friendlyName;

        private AppDomainSetup appDomainSetup;

        private AppDomain()
        {
            throw new Exception();
        }

        public AppDomainSetup SetupInformation
        {
            get
            {
                if (appDomainSetup == null)
                {
                    appDomainSetup = new AppDomainSetup();
                }

                return appDomainSetup;
            }
        }
        
        public extern static AppDomain CreateDomain(String friendlyName);

        public static AppDomain CurrentDomain
        {
            get { return Thread.GetDomain(); }
        }

        public String FriendlyName
        {
            get
            {
                return m_friendlyName;
            }
        }

        public string BaseDirectory
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int Id { get; set; }

        public EventHandler DomainUnload { get; set; }

        
        public Assembly[] GetAssemblies()
        {
            throw new NotImplementedException();
        }
        
        public static extern void Unload(AppDomain domain);

        public int GetId()
        {
            return 1;
        }

        public bool IsFinalizingForUnload()
        {
            throw new NotImplementedException();
        }

        public string GetTargetFrameworkName()
        {
            return this.SetupInformation.TargetFrameworkName;
        }

        public bool? IsCompatibilitySwitchSet(string compatibilitySwitch)
        {
            return true;
        }
    }
}



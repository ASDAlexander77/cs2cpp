////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////#define ENABLE_CROSS_APPDOMAIN
#define ENABLE_CROSS_APPDOMAIN
namespace System.Globalization
{
    using System;
    using System.Threading;
    using System.Collections;
    using System.Runtime.CompilerServices;
    using System.Reflection;
    using System.Resources;
    public class CultureInfo /*: ICloneable , IFormatProvider*/ {
        internal NumberFormatInfo numInfo = null;
        internal DateTimeFormatInfo dateTimeInfo = null;
        internal string m_name = null;
        internal ResourceManager m_rm;
        [NonSerialized]
        private CultureInfo m_parent;
        [NonSerialized]
        internal bool m_isInherited;
        const string c_ResourceBase = "System.Globalization.Resources.CultureInfo";
        internal string EnsureStringResource(ref string str, System.Globalization.Resources.CultureInfo.StringResources id)
        {
            if (str == null)
            {
                str = (string)ResourceManager.GetObject(m_rm, id);
            }

            return str;
        }

        internal string[] EnsureStringArrayResource(ref string[] strArray, System.Globalization.Resources.CultureInfo.StringResources id)
        {
            if (strArray == null)
            {
                string str = (string)ResourceManager.GetObject(m_rm, id);
                strArray = str.Split('|');
            }

            return (string[])strArray.Clone();
        }

        public CultureInfo(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            //m_rm = new ResourceManager(c_ResourceBase, typeof(CultureInfo).Assembly, name, true);
            //m_name = m_rm.m_cultureName;
        }

        internal CultureInfo(ResourceManager resourceManager)
        {
            m_rm = resourceManager;
            m_name = resourceManager.m_cultureName;
        }

        public static CultureInfo CurrentUICulture
        {
            get
            {
                //only one system-wide culture.  We do not currently support per-thread cultures
                CultureInfo culture = CurrentUICultureInternal;
                if (culture == null)
                {
                    culture = new CultureInfo("");
                    CurrentUICultureInternal = culture;
                }

                return culture;
            }
        }

        private static CultureInfo CurrentUICultureInternal
        {
            get;

            set;
        }

        public static CultureInfo CurrentCulture
        {
            get
            {
                //only one system-wide culture.  We do not currently support per-thread cultures
                CultureInfo culture = CurrentCultureInternal;
                if (culture == null)
                {
                    culture = new CultureInfo("");
                    CurrentCultureInternal = culture;
                }

                return culture;
            }
        }

        private static CultureInfo CurrentCultureInternal
        {
            get;

            set;
        }

        public virtual CultureInfo Parent
        {
            get
            {
                if (m_parent == null)
                {
                    if (m_name == "") //Invariant culture
                    {
                        m_parent = this;
                    }
                    else
                    {
                        string parentName = m_name;
                        int iDash = m_name.LastIndexOf('-');
                        if (iDash >= 0)
                        {
                            parentName = parentName.Substring(0, iDash);
                        }
                        else
                        {
                            parentName = "";
                        }

                        m_parent = new CultureInfo(parentName);
                    }
                }

                return m_parent;
            }
        }

        public virtual String Name
        {
            get
            {
                return m_name;
            }
        }

        public override String ToString()
        {
            return m_name;
        }

        public virtual Object GetFormat(Type formatType)
        {
            if (formatType == typeof(NumberFormatInfo))
            {
                return (NumberFormat);
            }
            if (formatType == typeof(DateTimeFormatInfo))
            {
                return (DateTimeFormat);
            }
            return (null);
        }

        public virtual NumberFormatInfo NumberFormat
        {
            get
            {
                return NumberFormatInfo.InvariantInfo;
            }
        }

        public virtual DateTimeFormatInfo DateTimeFormat
        {
            get
            {
                if (dateTimeInfo == null)
                {
                    dateTimeInfo = new DateTimeFormatInfo(this);
                }

                return dateTimeInfo;
            }
        }
    }
}



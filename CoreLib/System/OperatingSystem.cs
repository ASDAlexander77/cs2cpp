// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*============================================================
**
**
**
** Purpose: 
**
**
===========================================================*/
namespace System {
    using System.Globalization;
    using System.Security.Permissions;
    using System.Runtime.InteropServices;
    using System.Diagnostics.Contracts;


    [ComVisible(true)]
    [Serializable]
    public sealed class OperatingSystem : ICloneable
    {
        private Version _version;
        private PlatformID _platform;
        private string _servicePack;
        private string _versionString;

        private OperatingSystem()
        {
        }

        public OperatingSystem(PlatformID platform, Version version) : this(platform, version, null) {
        }
    
        internal OperatingSystem(PlatformID platform, Version version, string servicePack) {
#if !FEATURE_LEGACYNETCF
            if( platform < PlatformID.Win32S || platform > PlatformID.MacOSX) {
#else // FEATURE_LEGACYNETCF
            if( platform < PlatformID.Win32S || platform > PlatformID.NokiaS60) {
#endif // FEATURE_LEGACYNETCF
                throw new ArgumentException(
                    Environment.GetResourceString("Arg_EnumIllegalVal", (int)platform),
                    "platform");
            }

            if ((Object) version == null)
                throw new ArgumentNullException("version");
            Contract.EndContractBlock();

            _platform = platform;
            _version = (Version) version.Clone();
            _servicePack = servicePack;
        }

        public PlatformID Platform {
            get { return _platform; }
        }
        
        public string ServicePack { 
            get { 
                if( _servicePack == null) {
                    return string.Empty;
                }

                return _servicePack;
            }
        }    

        public Version Version {
            get { return _version; }
        }
    
        public Object Clone() {
            return new OperatingSystem(_platform,
                                       _version, _servicePack );
        }
    
        public override String ToString() {
            return VersionString;
        }

        public String VersionString {
            get {
                if(_versionString != null) {
                    return _versionString;
                }

                String os;
                switch(_platform)
                {
                    case PlatformID.Win32NT:
                        os = "Microsoft Windows NT ";
                        break;
                    case PlatformID.Win32Windows:
                        if ((_version.Major > 4) ||
                            ((_version.Major == 4) && (_version.Minor > 0)))
                            os = "Microsoft Windows 98 ";
                        else
                            os = "Microsoft Windows 95 ";
                        break;
                    case PlatformID.Win32S:
                        os = "Microsoft Win32S ";
                        break;
                    case PlatformID.WinCE:
                        os = "Microsoft Windows CE ";
                        break;
#if !FEATURE_LEGACYNETCF
                    case PlatformID.MacOSX:
                        os = "Mac OS X ";
                        break;
#endif
                    default:
                        os = "<unknown> ";
                        break;
                }

                if( String.IsNullOrEmpty(_servicePack)) {
                    _versionString = os + _version.ToString();
                }
                else {
                    _versionString = os + _version.ToString(3) + " " + _servicePack;
                }

                return _versionString;            
            }
        }
    }
}

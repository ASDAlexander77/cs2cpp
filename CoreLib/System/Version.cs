////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    using System.Diagnostics.Contracts;
    using System.Globalization;

    // A Version object contains four hierarchical numeric components: major, minor,
    // revision and build.  Revision and build may be unspecified, which is represented
    // internally as a -1.  By definition, an unspecified component matches anything
    // (both unspecified and specified), and an unspecified component is "less than" any
    // specified component.

    public sealed class Version // : ICloneable, IComparable, IComparable<Version>, IEquatable<Version>
    {
        // AssemblyName depends on the order staying the same
        private int _Major;
        private int _Minor;
        private int _Build;// = -1;
        private int _Revision;// = -1;
        private static readonly char[] SeparatorsArray = new char[] { '.' };


        public Version(int major, int minor, int build, int revision)
        {
            if (major < 0 || minor < 0 || revision < 0 || build < 0)
                throw new ArgumentOutOfRangeException();

            _Major = major;
            _Minor = minor;
            _Revision = revision;
            _Build = build;
        }

        public Version(int major, int minor, int build)
        {
            if (major < 0 || minor < 0 || build < 0)
                throw new ArgumentOutOfRangeException();

            _Major = major;
            _Minor = minor;
            _Revision = -1;
            _Build = build;
        }

        public Version(int major, int minor)
        {
            if (major < 0)
                throw new ArgumentOutOfRangeException();

            if (minor < 0)
                throw new ArgumentOutOfRangeException();

            _Major = major;
            _Minor = minor;

            // Other 2 initialize to -1 as it done on desktop and CE
            _Build = -1;
            _Revision = -1;
        }

        public Version(String version)
        {
            Version v = Version.Parse(version);
            _Major = v.Major;
            _Minor = v.Minor;
            _Build = v.Build;
            _Revision = v.Revision;
        }

        // Properties for setting and getting version numbers
        public int Major
        {
            get { return _Major; }
        }

        public int Minor
        {
            get { return _Minor; }
        }

        public int Revision
        {
            get { return _Revision; }
        }

        public int Build
        {
            get { return _Build; }
        }

        public override bool Equals(Object obj)
        {
            if (((Object)obj == null) ||
                (!(obj is Version)))
                return false;

            Version v = (Version)obj;
            // check that major, minor, build & revision numbers match
            if ((this._Major != v._Major) ||
                (this._Minor != v._Minor) ||
                (this._Build != v._Build) ||
                (this._Revision != v._Revision))
                return false;

            return true;
        }

        public override String ToString()
        {
            string retStr = _Major + "." + _Minor;

            // Adds _Build and then _Revision if they are positive. They could be -1 in this case not added.
            if (_Build >= 0)
            {
                retStr += "." + _Build;
                if (_Revision >= 0)
                {
                    retStr += "." + _Revision;
                }
            }

            return retStr;
        }

        public static Version Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            Contract.EndContractBlock();

            VersionResult r = new VersionResult();
            r.Init("input", true);
            if (!TryParseVersion(input, ref r))
            {
                throw r.GetVersionParseException();
            }
            return r.m_parsedVersion;
        }

        public static bool TryParse(string input, out Version result)
        {
            VersionResult r = new VersionResult();
            r.Init("input", false);
            bool b = TryParseVersion(input, ref r);
            result = r.m_parsedVersion;
            return b;
        }

        private static bool TryParseVersion(string version, ref VersionResult result)
        {
            int major, minor, build, revision;

            if ((Object)version == null)
            {
                result.SetFailure(ParseFailureKind.ArgumentNullException);
                return false;
            }

            String[] parsedComponents = version.Split(SeparatorsArray);
            int parsedComponentsLength = parsedComponents.Length;
            if ((parsedComponentsLength < 2) || (parsedComponentsLength > 4))
            {
                result.SetFailure(ParseFailureKind.ArgumentException);
                return false;
            }

            if (!TryParseComponent(parsedComponents[0], "version", ref result, out major))
            {
                return false;
            }

            if (!TryParseComponent(parsedComponents[1], "version", ref result, out minor))
            {
                return false;
            }

            parsedComponentsLength -= 2;

            if (parsedComponentsLength > 0)
            {
                if (!TryParseComponent(parsedComponents[2], "build", ref result, out build))
                {
                    return false;
                }

                parsedComponentsLength--;

                if (parsedComponentsLength > 0)
                {
                    if (!TryParseComponent(parsedComponents[3], "revision", ref result, out revision))
                    {
                        return false;
                    }
                    else
                    {
                        result.m_parsedVersion = new Version(major, minor, build, revision);
                    }
                }
                else
                {
                    result.m_parsedVersion = new Version(major, minor, build);
                }
            }
            else
            {
                result.m_parsedVersion = new Version(major, minor);
            }

            return true;
        }

        private static bool TryParseComponent(string component, string componentName, ref VersionResult result, out int parsedComponent)
        {
            if (!Int32.TryParse(component, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedComponent))
            {
                result.SetFailure(ParseFailureKind.FormatException, component);
                return false;
            }

            if (parsedComponent < 0)
            {
                result.SetFailure(ParseFailureKind.ArgumentOutOfRangeException, componentName);
                return false;
            }

            return true;
        }


        internal enum ParseFailureKind
        {
            ArgumentNullException,
            ArgumentException,
            ArgumentOutOfRangeException,
            FormatException
        }

        internal struct VersionResult
        {
            internal Version m_parsedVersion;
            internal ParseFailureKind m_failure;
            internal string m_exceptionArgument;
            internal string m_argumentName;
            internal bool m_canThrow;

            internal void Init(string argumentName, bool canThrow)
            {
                m_canThrow = canThrow;
                m_argumentName = argumentName;
            }

            internal void SetFailure(ParseFailureKind failure)
            {
                SetFailure(failure, String.Empty);
            }

            internal void SetFailure(ParseFailureKind failure, string argument)
            {
                m_failure = failure;
                m_exceptionArgument = argument;
                if (m_canThrow)
                {
                    throw GetVersionParseException();
                }
            }

            internal Exception GetVersionParseException()
            {
                switch (m_failure)
                {
                    case ParseFailureKind.ArgumentNullException:
                        return new ArgumentNullException(m_argumentName);
                    case ParseFailureKind.ArgumentException:
                        return new ArgumentException(Environment.GetResourceString("Arg_VersionString"));
                    case ParseFailureKind.ArgumentOutOfRangeException:
                        return new ArgumentOutOfRangeException(m_exceptionArgument, Environment.GetResourceString("ArgumentOutOfRange_Version"));
                    case ParseFailureKind.FormatException:
                        // Regenerate the FormatException as would be thrown by Int32.Parse()
                        try
                        {
                            Int32.Parse(m_exceptionArgument, CultureInfo.InvariantCulture);
                        }
                        catch (FormatException e)
                        {
                            return e;
                        }
                        catch (OverflowException e)
                        {
                            return e;
                        }
                        Contract.Assert(false, "Int32.Parse() did not throw exception but TryParse failed: " + m_exceptionArgument);
                        return new FormatException(Environment.GetResourceString("Format_InvalidString"));
                    default:
                        Contract.Assert(false, "Unmatched case in Version.GetVersionParseException() for value: " + m_failure);
                        return new ArgumentException(Environment.GetResourceString("Arg_VersionString"));
                }
            }

        }
    }
}



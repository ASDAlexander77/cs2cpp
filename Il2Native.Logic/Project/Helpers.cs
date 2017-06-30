using System.IO;

namespace Il2Native.Logic.Project
{
    class Helpers
    {
        internal static string EnsureTrailingSlash(string fileSpec)
        {
            if (!EndsWithSlash(fileSpec))
            {
                fileSpec += Path.DirectorySeparatorChar;
            }

            return fileSpec;
        }

        internal static bool EndsWithSlash(string fileSpec)
        {
            return (fileSpec.Length > 0)
                ? IsSlash(fileSpec[fileSpec.Length - 1])
                : false;
        }

        internal static bool IsSlash(char c)
        {
            return ((c == Path.DirectorySeparatorChar) || (c == Path.AltDirectorySeparatorChar));
        }

        internal static string NormalizeDirectory(params string[] path)
        {
            return EnsureTrailingSlash(NormalizePath(path));
        }

        internal static string NormalizePath(params string[] path)
        {
            var localPath = Path.Combine(path);
            return string.IsNullOrEmpty(localPath) || Path.DirectorySeparatorChar == '\\' ? localPath : localPath.Replace('\\', '/');//.Replace("//", "/");
        }

        internal static string ValueOrDefault(string conditionValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(conditionValue))
            {
                return defaultValue;
            }
            else
            {
                return conditionValue;
            }
        }

        internal static string EnsureNoLeadingSlash(string path)
        {
            path = FixFilePath(path);
            if (path.Length > 0 && IsSlash(path[0]))
            {
                path = path.Substring(1);
            }

            return path;
        }

        /// <summary>
        /// Ensures the path does not have a trailing slash.
        /// </summary>
        internal static string EnsureNoTrailingSlash(string path)
        {
            path = FixFilePath(path);
            if (EndsWithSlash(path))
            {
                path = path.Substring(0, path.Length - 1);
            }

            return path;
        }

        internal static string FixFilePath(string path)
        {
            return string.IsNullOrEmpty(path) || Path.DirectorySeparatorChar == '\\' ? path : path.Replace('\\', '/');
        }
    }
}

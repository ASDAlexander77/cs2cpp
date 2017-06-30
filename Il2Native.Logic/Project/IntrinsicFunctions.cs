using System;
using System.IO;
using System.Linq;

namespace Il2Native.Logic.Project
{
    internal static class IntrinsicFunctions
    {
        internal static double Add(double a, double b)
        {
            return a + b;
        }

        internal static long Add(long a, long b)
        {
            return a + b;
        }

        internal static double Subtract(double a, double b)
        {
            return a - b;
        }

        internal static long Subtract(long a, long b)
        {
            return a - b;
        }

        internal static double Multiply(double a, double b)
        {
            return a * b;
        }

        internal static long Multiply(long a, long b)
        {
            return a * b;
        }

        internal static double Divide(double a, double b)
        {
            return a / b;
        }

        internal static long Divide(long a, long b)
        {
            return a / b;
        }

        internal static double Modulo(double a, double b)
        {
            return a % b;
        }

        internal static long Modulo(long a, long b)
        {
            return a % b;
        }

        internal static int BitwiseOr(int first, int second)
        {
            return first | second;
        }

        internal static int BitwiseAnd(int first, int second)
        {
            return first & second;
        }

        internal static int BitwiseXor(int first, int second)
        {
            return first ^ second;
        }

        internal static int BitwiseNot(int first)
        {
            return ~first;
        }

        internal static string MakeRelative(string basePath, string path)
        {
            if (basePath.Length == 0)
            {
                return path;
            }

            Uri baseUri = new Uri(Helpers.EnsureTrailingSlash(basePath), UriKind.Absolute); // May throw UriFormatException

            Uri pathUri = CreateUriFromPath(path);

            if (!pathUri.IsAbsoluteUri)
            {
                // the path is already a relative url, we will just normalize it...
                pathUri = new Uri(baseUri, pathUri);
            }

            Uri relativeUri = baseUri.MakeRelativeUri(pathUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.IsAbsoluteUri ? relativeUri.LocalPath : relativeUri.ToString());

            string result = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            return result;
        }

        private static Uri CreateUriFromPath(string path)
        {
            Uri pathUri = null;

            // Try absolute first, then fall back on relative, otherwise it
            // makes some absolute UNC paths like (\\foo\bar) relative ...
            if (!Uri.TryCreate(path, UriKind.Absolute, out pathUri))
            {
                pathUri = new Uri(path, UriKind.Relative);
            }

            return pathUri;
        }

        internal static string GetDirectoryNameOfFileAbove(string startingDirectory, string fileName)
        {
            // Canonicalize our starting location
            string lookInDirectory = Path.GetFullPath(startingDirectory);

            do
            {
                // Construct the path that we will use to test against
                string possibleFileDirectory = Path.Combine(lookInDirectory, fileName);

                // If we successfully locate the file in the directory that we're
                // looking in, simply return that location. Otherwise we'll
                // keep moving up the tree.
                if (File.Exists(possibleFileDirectory))
                {
                    // We've found the file, return the directory we found it in
                    return lookInDirectory;
                }
                else
                {
                    // GetDirectoryName will return null when we reach the root
                    // terminating our search
                    lookInDirectory = Path.GetDirectoryName(lookInDirectory);
                }
            }
            while (lookInDirectory != null);

            // When we didn't find the location, then return an empty string
            return String.Empty;
        }

        internal static string GetPathOfFileAbove(string file, string startingDirectory)
        {
            // This method does not accept a path, only a file name
            if (file.Any(i => i.Equals(Path.DirectorySeparatorChar) || i.Equals(Path.AltDirectorySeparatorChar)))
            {
                throw new ArgumentException("InvalidGetPathOfFileAboveParameter", file);
            }

            // Search for a directory that contains that file
            string directoryName = GetDirectoryNameOfFileAbove(startingDirectory, file);

            return String.IsNullOrWhiteSpace(directoryName) ? String.Empty : Helpers.NormalizePath(directoryName, file);
        }

    }
}

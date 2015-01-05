namespace System
{
    public static class Environment
    {
        public static string NewLine = "\r\n";

        public static string CurrentDirectory { get; set; }

        public static int ExitCode { get; set; }

        public static string GetResourceString(string name)
        {
            return name;
        }
    }
}

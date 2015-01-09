namespace System
{
    public static class Environment
    {
        public static string NewLine = "\r\n";

        public static string Space = " ";

        public static string CurrentDirectory { get; set; }

        public static int ExitCode { get; set; }

        public static string GetResourceString(string name)
        {
            return name;
        }

        public static string GetResourceString(string name, string value)
        {
            return name + Space + value;
        }

        public static string GetResourceString(string name, string value, string value2)
        {
            return name + Space + value + Space + value2;
        }
    }
}

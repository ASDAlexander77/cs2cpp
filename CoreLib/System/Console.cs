namespace System
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;

    public static class Console
    {
        private static string NewLine = "\r\n";
        private static string PrintString = "%.*s";
        private static string PrintDouble = "%f";
        private static string PrintInt = "%i";
        private static string PrintChar = "%c";

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern int wprintf(char[] chars);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern int wprintf(char[] format, int length, char[] chars);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern int wprintf(char[] format, double d);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern int wprintf(char[] format, float d);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public static extern int wprintf(char[] format, int t);

        public static int Read()
        {
            throw new NotImplementedException();
        }

        public static String ReadLine()
        {
            throw new NotImplementedException();
        }

        public static void WriteLine()
        { 
            wprintf(NewLine.ToCharArray());
        }

        public static void WriteLine(bool value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(char value)
        {
            wprintf(PrintChar.ToCharArray(), value);
            wprintf(NewLine.ToCharArray());
        }

        public static void WriteLine(char[] buffer)
        {
            wprintf(PrintString.ToCharArray(), buffer.Length, buffer);
            wprintf(NewLine.ToCharArray());
        }

        public static void WriteLine(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(decimal value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(double value)
        {
            wprintf(PrintDouble.ToCharArray(), value);
            wprintf(NewLine.ToCharArray());
        }

        public static void WriteLine(float value)
        {
            wprintf(PrintDouble.ToCharArray(), value);
            wprintf(NewLine.ToCharArray());
        }

        public static void WriteLine(int value)
        {
            wprintf(PrintInt.ToCharArray(), value);
            wprintf(NewLine.ToCharArray());
        }

        [CLSCompliant(false)]
        public static void WriteLine(uint value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(long value)
        {
            throw new NotImplementedException();
        }

        [CLSCompliant(false)]
        public static void WriteLine(ulong value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(object value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(string value)
        {
            var chars = value.ToCharArray();
            wprintf(PrintString.ToCharArray(), chars.Length, chars);
            wprintf(NewLine.ToCharArray());
        }

        public static void WriteLine(String format, Object arg0)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0);
            WriteLine(sb.ToString());
        }

        public static void WriteLine(String format, Object arg0, Object arg1)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1);
            WriteLine(sb.ToString());
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1, arg2);
            WriteLine(sb.ToString());
        }

        public static void WriteLine(String format, params Object[] arg)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg);
            WriteLine(sb.ToString());
        }

        public static void Write(String format, Object arg0)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0);
            Write(sb.ToString());
        }

        public static void Write(String format, Object arg0, Object arg1)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1);
            Write(sb.ToString());
        }

        public static void Write(String format, Object arg0, Object arg1, Object arg2)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1, arg2);
            Write(sb.ToString());
        }

        public static void Write(String format, params Object[] arg)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg);
            Write(sb.ToString());
        }

        public static void Write(bool value)
        {
            throw new NotImplementedException();
        }

        public static void Write(char value)
        {
            wprintf(PrintChar.ToCharArray(), value);
        }

        public static void Write(char[] buffer)
        {
            wprintf(buffer);
        }

        public static void Write(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public static void Write(double value)
        {
            wprintf(PrintDouble.ToCharArray(), value);
        }

        public static void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        public static void Write(float value)
        {
            wprintf(PrintDouble.ToCharArray(), value);
        }

        public static void Write(int value)
        {
            wprintf(PrintInt.ToCharArray(), value);
        }

        [CLSCompliant(false)]
        public static void Write(uint value)
        {
            throw new NotImplementedException();
        }

        public static void Write(long value)
        {
            throw new NotImplementedException();
        }

        [CLSCompliant(false)]
        public static void Write(ulong value)
        {
            throw new NotImplementedException();
        }

        public static void Write(Object value)
        {
            Write(value.ToString());
        }

        public static void Write(String value)
        {
            wprintf(value.ToCharArray());
        }
    }
}

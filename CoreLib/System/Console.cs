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
        private static string PrintLong = "%lld";
        private static string PrintInt = "%ld";
        private static string PrintChar = "%c";

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* chars);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, int length, char* chars);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, double d);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, int t);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, long t);

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
            unsafe
            {
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(nl);
                }
            }
        }

        public static void WriteLine(bool value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(char value)
        {
            unsafe
            {
                fixed (char* pc = &PrintChar.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(pc, value);
                    wprintf(nl);
                }
            }
        }

        public static void WriteLine(char[] buffer)
        {
            unsafe
            {
                fixed (char* ps = &PrintString.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                fixed (char* b = &buffer[0])
                {
                    wprintf(ps, buffer.Length, b);
                    wprintf(nl);
                }
            }
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
            unsafe
            {
                fixed (char* pd = &PrintDouble.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(pd, value);
                    wprintf(nl);
                }
            }
        }

        public static void WriteLine(float value)
        {
            unsafe
            {
                fixed (char* pd = &PrintDouble.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(pd, value);
                    wprintf(nl);
                }
            }
        }

        public static void WriteLine(int value)
        {
            unsafe
            {
                fixed (char* pi = &PrintInt.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(pi, value);
                    wprintf(nl);
                }
            }
        }

        [CLSCompliant(false)]
        public static void WriteLine(uint value)
        {
            unsafe
            {
                fixed (char* pi = &PrintInt.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(pi, value);
                    wprintf(nl);
                }
            }
        }

        public static void WriteLine(long value)
        {
            unsafe
            {
                fixed (char* pi = &PrintLong.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(pi, value);
                    wprintf(nl);
                }
            }
        }

        [CLSCompliant(false)]
        public static void WriteLine(ulong value)
        {
            unsafe
            {
                fixed (char* pi = &PrintLong.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(pi, value);
                    wprintf(nl);
                }
            }
        }

        public static void WriteLine(object value)
        {
            WriteLine(value.ToString());
        }

        public static void WriteLine(string value)
        {
            var chars = value.ToCharArray();
            unsafe
            {
                fixed (char* ps = &PrintString.ToCharArray()[0])
                fixed (char* nl = &NewLine.ToCharArray()[0])
                fixed (char* c = &chars[0])
                {
                    wprintf(ps, chars.Length, c);
                    wprintf(nl);
                }
            }
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
            unsafe
            {
                fixed (char* pc = &PrintChar.ToCharArray()[0])
                {
                    wprintf(pc, value);
                }
            }
        }

        public static void Write(char[] buffer)
        {
            unsafe
            {
                fixed (char* b = &buffer[0])
                {
                    wprintf(b);
                }
            }
        }

        public static void Write(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public static void Write(double value)
        {
            unsafe
            {
                fixed (char* pd = &PrintDouble.ToCharArray()[0])
                {
                    wprintf(pd, value);
                }
            }
        }

        public static void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        public static void Write(float value)
        {
            unsafe
            {
                fixed (char* pd = &PrintDouble.ToCharArray()[0])
                {
                    wprintf(pd, value);
                }
            }
        }

        public static void Write(int value)
        {
            unsafe
            {
                fixed (char* pi = &PrintInt.ToCharArray()[0])
                {
                    wprintf(pi, value);
                }
            }
        }

        [CLSCompliant(false)]
        public static void Write(uint value)
        {
            unsafe
            {
                fixed (char* pi = &PrintInt.ToCharArray()[0])
                {
                    wprintf(pi, value);
                }
            }
        }

        public static void Write(long value)
        {
            unsafe
            {
                fixed (char* pi = &PrintLong.ToCharArray()[0])
                {
                    wprintf(pi, value);
                }
            }
        }

        [CLSCompliant(false)]
        public static void Write(ulong value)
        {
            unsafe
            {
                fixed (char* pi = &PrintLong.ToCharArray()[0])
                {
                    wprintf(pi, value);
                }
            }
        }

        public static void Write(Object value)
        {
            Write(value.ToString());
        }

        public static void Write(String value)
        {
            unsafe
            {
                fixed (char* p = &value.ToCharArray()[0])
                {
                    wprintf(p);
                }
            }
        }

        public static class Out
        {
            public static void WriteLine(object value)
            {
                WriteLine(value.ToString());
            }

            public static void WriteLine(String format, params Object[] arg)
            {
                var sb = new StringBuilder();
                sb.AppendFormat(format, arg);
                WriteLine(sb.ToString());
            }

            public static void Write(object value)
            {
                Write(value.ToString());
            }

            public static void Write(String format, params Object[] arg)
            {
                var sb = new StringBuilder();
                sb.AppendFormat(format, arg);
                Write(sb.ToString());
            }
        }

        public static class Error
        {
            public static void WriteLine(object value)
            {
                WriteLine(value.ToString());
            }

            public static void WriteLine(String format, params Object[] arg)
            {
                var sb = new StringBuilder();
                sb.AppendFormat(format, arg);
                WriteLine(sb.ToString());
            }

            public static void Write(object value)
            {
                Write(value.ToString());
            }

            public static void Write(String format, params Object[] arg)
            {
                var sb = new StringBuilder();
                sb.AppendFormat(format, arg);
                Write(sb.ToString());
            }
        }
    }
}

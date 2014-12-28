using System.Text;

namespace System.IO
{
    using Runtime.CompilerServices;

    internal class __DebugOutputTextWriter : TextWriter
    {
        private static string NewLine = "\r\n";
        private static string PrintString = "%.*s";
        private static string PrintDouble = "%f";
        private static string PrintLong = "%lld";
        private static string PrintInt = "%ld";
        private static string PrintChar = "%c";

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* chars);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, int length, char* chars);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, double d);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, int t);

        [MethodImplAttribute(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, long t);

        internal __DebugOutputTextWriter()
            : base()
        {
        }

        public override Encoding Encoding
        {
            get
            {
                return new UnicodeEncoding(false, false);
            }
        }

        public override void WriteLine()
        {
            unsafe
            {
                fixed (char* nl = &NewLine.ToCharArray()[0])
                {
                    wprintf(nl);
                }
            }
        }

        public override void WriteLine(bool value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(char value)
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

        public override void WriteLine(char[] buffer)
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

        public override void WriteLine(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(decimal value)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(double value)
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

        public override void WriteLine(float value)
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

        public override void WriteLine(int value)
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
        public override void WriteLine(uint value)
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

        public override void WriteLine(long value)
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
        public override void WriteLine(ulong value)
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

        public override void WriteLine(object value)
        {
            WriteLine(value.ToString());
        }

        public override void WriteLine(string value)
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

        public override void WriteLine(String format, Object arg0)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0);
            WriteLine(sb.ToString());
        }

        public override void WriteLine(String format, Object arg0, Object arg1)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1);
            WriteLine(sb.ToString());
        }

        public override void WriteLine(String format, Object arg0, Object arg1, Object arg2)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1, arg2);
            WriteLine(sb.ToString());
        }

        public override void WriteLine(String format, params Object[] arg)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg);
            WriteLine(sb.ToString());
        }

        public override void Write(String format, Object arg0)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0);
            Write(sb.ToString());
        }

        public override void Write(String format, Object arg0, Object arg1)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1);
            Write(sb.ToString());
        }

        public override void Write(String format, Object arg0, Object arg1, Object arg2)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1, arg2);
            Write(sb.ToString());
        }

        public override void Write(String format, params Object[] arg)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg);
            Write(sb.ToString());
        }

        public override void Write(bool value)
        {
            throw new NotImplementedException();
        }

        public override void Write(char value)
        {
            unsafe
            {
                fixed (char* pc = &PrintChar.ToCharArray()[0])
                {
                    wprintf(pc, value);
                }
            }
        }

        public override void Write(char[] buffer)
        {
            unsafe
            {
                fixed (char* b = &buffer[0])
                {
                    wprintf(b);
                }
            }
        }

        public override void Write(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override void Write(double value)
        {
            unsafe
            {
                fixed (char* pd = &PrintDouble.ToCharArray()[0])
                {
                    wprintf(pd, value);
                }
            }
        }

        public override void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        public override void Write(float value)
        {
            unsafe
            {
                fixed (char* pd = &PrintDouble.ToCharArray()[0])
                {
                    wprintf(pd, value);
                }
            }
        }

        public override void Write(int value)
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
        public override void Write(uint value)
        {
            unsafe
            {
                fixed (char* pi = &PrintInt.ToCharArray()[0])
                {
                    wprintf(pi, value);
                }
            }
        }

        public override void Write(long value)
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
        public override void Write(ulong value)
        {
            unsafe
            {
                fixed (char* pi = &PrintLong.ToCharArray()[0])
                {
                    wprintf(pi, value);
                }
            }
        }

        public override void Write(Object value)
        {
            Write(value.ToString());
        }

        public override void Write(String value)
        {
            unsafe
            {
                fixed (char* p = &value.ToCharArray()[0])
                {
                    wprintf(p);
                }
            }
        }
    }
}

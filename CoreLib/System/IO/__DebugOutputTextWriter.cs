using System.Text;

namespace System.IO
{
    using Runtime.CompilerServices;

    internal class __DebugOutputTextWriter : TextWriter
    {
        private const string PrintString = "%.*s";
        private const string PrintDouble = "%f";
        private const string PrintLong = "%lld";
        private const string PrintInt = "%ld";
        private const string PrintChar = "%c";

        private const string NewLine = "\r\n";
        private const string PrintStringNewLine = "%.*s\r\n";
        private const string PrintDoubleNewLine = "%f\r\n";
        private const string PrintLongNewLine = "%lld\r\n";
        private const string PrintIntNewLine = "%ld\r\n";
        private const string PrintCharNewLine = "%c\r\n";

        [MethodImpl(MethodImplOptions.Unmanaged)]
        public unsafe static extern int wprintf(char* format, __arglist);

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
                fixed (char* nl = NewLine)
                {
                    wprintf(nl, __arglist());
                }
            }
        }

        public override void WriteLine(bool value)
        {
            Write(value ? bool.TrueLiteral : bool.FalseLiteral);
        }

        public override void WriteLine(char value)
        {
            unsafe
            {
                fixed (char* pc = PrintCharNewLine)
                {
                    wprintf(pc, __arglist(value));
                }
            }
        }

        public override void WriteLine(char[] buffer)
        {
            unsafe
            {
                fixed (char* ps = PrintStringNewLine)
                fixed (char* b = buffer)
                {
                    wprintf(ps, __arglist(buffer.Length, b));
                }
            }
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(decimal value)
        {
            WriteLine(value.ToString());
        }

        public override void WriteLine(double value)
        {
            unsafe
            {
                fixed (char* pd = PrintDoubleNewLine)
                {
                    wprintf(pd, __arglist(value));
                }
            }
        }

        public override void WriteLine(float value)
        {
            unsafe
            {
                fixed (char* pd = PrintDoubleNewLine)
                {
                    wprintf(pd, __arglist(value));
                }
            }
        }

        public override void WriteLine(int value)
        {
            unsafe
            {
                fixed (char* pi = PrintIntNewLine)
                {
                    wprintf(pi, __arglist(value));
                }
            }
        }

        [CLSCompliant(false)]
        public override void WriteLine(uint value)
        {
            unsafe
            {
                fixed (char* pi = PrintIntNewLine)
                {
                    wprintf(pi, __arglist(value));
                }
            }
        }

        public override void WriteLine(long value)
        {
            unsafe
            {
                fixed (char* pi = PrintLongNewLine)
                {
                    wprintf(pi, __arglist(value));
                }
            }
        }

        [CLSCompliant(false)]
        public override void WriteLine(ulong value)
        {
            unsafe
            {
                fixed (char* pi = PrintLongNewLine)
                {
                    wprintf(pi, __arglist(value));
                }
            }
        }

        public override void WriteLine(object value)
        {
            if (value == null)
            {
                this.WriteLine();
                return;
            }

            WriteLine(value.ToString());
        }

        public override void WriteLine(string value)
        {
            if (value == null)
            {
                this.WriteLine();
                return;
            }

            var chars = value.ToCharArray();
            unsafe
            {
                fixed (char* ps = PrintStringNewLine)
                fixed (char* c = chars)
                {
                    wprintf(ps, __arglist(chars.Length, c));
                }
            }
        }

        public override void WriteLine(string format, object arg0)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0);
            WriteLine(sb.ToString());
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1);
            WriteLine(sb.ToString());
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1, arg2);
            WriteLine(sb.ToString());
        }

        public override void WriteLine(string format, params object[] arg)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg);
            WriteLine(sb.ToString());
        }

        public override void Write(string format, object arg0)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0);
            Write(sb.ToString());
        }

        public override void Write(string format, object arg0, object arg1)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1);
            Write(sb.ToString());
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            var sb = new StringBuilder();
            sb.AppendFormat(format, arg0, arg1, arg2);
            Write(sb.ToString());
        }

        public override void Write(string format, params object[] arg)
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
                fixed (char* pc = PrintChar)
                {
                    wprintf(pc, __arglist(value));
                }
            }
        }

        public override void Write(char[] buffer)
        {
            unsafe
            {
                fixed (char* b = buffer)
                {
                    wprintf(b, __arglist());
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
                fixed (char* pd = PrintDouble)
                {
                    wprintf(pd, __arglist(value));
                }
            }
        }

        public override void Write(decimal value)
        {
            this.Write(value.ToString());
        }

        public override void Write(float value)
        {
            unsafe
            {
                fixed (char* pd = PrintDouble)
                {
                    wprintf(pd, __arglist(value));
                }
            }
        }

        public override void Write(int value)
        {
            unsafe
            {
                fixed (char* pi = PrintInt)
                {
                    wprintf(pi, __arglist(value));
                }
            }
        }

        [CLSCompliant(false)]
        public override void Write(uint value)
        {
            unsafe
            {
                fixed (char* pi = PrintInt)
                {
                    wprintf(pi, __arglist(value));
                }
            }
        }

        public override void Write(long value)
        {
            unsafe
            {
                fixed (char* pi = PrintLong)
                {
                    wprintf(pi, __arglist(value));
                }
            }
        }

        [CLSCompliant(false)]
        public override void Write(ulong value)
        {
            unsafe
            {
                fixed (char* pi = PrintLong)
                {
                    wprintf(pi, __arglist(value));
                }
            }
        }

        public override void Write(object value)
        {
            if (value != null)
            {
                Write(value.ToString());
            }
        }

        public override void Write(string value)
        {
            if (value != null)
            {
                unsafe
                {
                    fixed (char* p = value)
                    {
                        wprintf(p, __arglist());
                    }
                }
            }
        }
    }
}

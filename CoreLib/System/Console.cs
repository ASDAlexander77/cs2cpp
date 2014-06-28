namespace System
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class Console
    {
        [MethodImplAttribute(MethodImplOptions.InternalCall)]
        public static extern int puts(char[] chars);

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
            throw new NotImplementedException();
        }

        public static void WriteLine(bool value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(char value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(char[] buffer)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public static void WriteLine(float value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(int value)
        {
            throw new NotImplementedException();
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

        public static void WriteLine(Object value)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(String value)
        {
            puts(value.ToCharArray());
        }

        public static void WriteLine(String format, Object arg0)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(String format, Object arg0, Object arg1)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2)
        {
            throw new NotImplementedException();
        }

        public static void WriteLine(String format, params Object[] arg)
        {
            throw new NotImplementedException();
        }

        public static void Write(String format, Object arg0)
        {
            throw new NotImplementedException();
        }

        public static void Write(String format, Object arg0, Object arg1)
        {
            throw new NotImplementedException();
        }

        public static void Write(String format, Object arg0, Object arg1, Object arg2)
        {
            throw new NotImplementedException();
        }

        public static void Write(String format, params Object[] arg)
        {
            throw new NotImplementedException();
        }

        public static void Write(bool value)
        {
            throw new NotImplementedException();
        }

        public static void Write(char value)
        {
            throw new NotImplementedException();
        }

        public static void Write(char[] buffer)
        {
            throw new NotImplementedException();
        }

        public static void Write(char[] buffer, int index, int count)
        {
            throw new NotImplementedException();
        }

        public static void Write(double value)
        {
            throw new NotImplementedException();
        }

        public static void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        public static void Write(float value)
        {
            throw new NotImplementedException();
        }

        public static void Write(int value)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public static void Write(String value)
        {
            throw new NotImplementedException();
        }
    }
}

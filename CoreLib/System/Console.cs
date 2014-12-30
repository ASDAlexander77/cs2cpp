#define USE_DEBUG_CONSOLE

namespace System
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Runtime.InteropServices;

    // Provides static fields for console input & output.  Use 
    // Console.In for input from the standard input stream (stdin),
    // Console.Out for output to stdout, and Console.Error
    // for output to stderr.  If any of those console streams are 
    // redirected from the command line, these streams will be redirected.
    // A program can also redirect its own output or input with the 
    // SetIn, SetOut, and SetError methods.
    // 
    // The distinction between Console.Out & Console.Error is useful
    // for programs that redirect output to a file or a pipe.  Note that
    // stdout & stderr can be output to different files at the same
    // time from the DOS command line:
    // 
    // someProgram 1> out 2> err
    // 
    //Contains only static data.  Serializable attribute not required.
    public static class Console
    {
        internal const int STD_INPUT_HANDLE = -10;
        internal const int STD_OUTPUT_HANDLE = -11;
        internal const int STD_ERROR_HANDLE = -12;

        private const int DefaultConsoleBufferSize = 256;

        // Beep range - see MSDN.
        private const int MinBeepFrequency = 37;
        private const int MaxBeepFrequency = 32767;

        private static volatile TextReader _in;
        private static volatile TextWriter _out;
        private static volatile TextWriter _error;

        // For ResetColor
        private static volatile bool _haveReadDefaultColors;
        private static volatile byte _defaultColors;

        private static volatile Encoding _inputEncoding = null;
        private static volatile Encoding _outputEncoding = null;

        private static volatile bool _stdInRedirectQueried = false;
        private static volatile bool _stdOutRedirectQueried = false;
        private static volatile bool _stdErrRedirectQueried = false;

        private static bool _isStdInRedirected;
        private static bool _isStdOutRedirected;
        private static bool _isStdErrRedirected;

        // Private object for locking instead of locking on a public type for SQL reliability work.
        // Use this for internal synchronization during initialization, wiring up events, or for short, non-blocking OS calls.
        private static volatile Object s_InternalSyncObject;

        private static Object InternalSyncObject
        {
            get
            {
                if (s_InternalSyncObject == null)
                {
                    Object o = new Object();
                    Interlocked.CompareExchange<Object>(ref s_InternalSyncObject, o, null);
                }
                return s_InternalSyncObject;
            }
        }

        public static TextReader In
        {
            get
            {
                // Because most applications don't use stdin, we can delay 
                // initialize it slightly better startup performance.
                if (_in == null)
                {
                    lock (InternalSyncObject)
                    {
                        if (_in == null)
                        {
                            // Set up Console.In
                            Stream s = OpenStandardInput(DefaultConsoleBufferSize);
                            TextReader tr;
                            if (s == Stream.Null)
                            {
                                tr = StreamReader.Null;
                            }
                            else
                            {
                                // Hopefully Encoding.GetEncoding doesn't load as many classes now.
                                Encoding enc = InputEncoding;
                                tr =
                                    TextReader.Synchronized(
                                        new StreamReader(s, enc, false, DefaultConsoleBufferSize, true));
                            }

                            Thread.MemoryBarrier();
                            _in = tr;
                        }
                    }
                }
                return _in;
            }
        }

        public static TextWriter Out
        {
            get
            {
                // Hopefully this is inlineable.
                if (_out == null)
                {
                    InitializeStdOutError(true);
                }
                return _out;
            }
        }

        public static TextWriter Error
        {
            get
            {
                // Hopefully this is inlineable.
                if (_error == null)
                {
                    InitializeStdOutError(false);
                }
                return _error;
            }
        }

        // For console apps, the console handles are set to values like 3, 7, 
        // and 11 OR if you've been created via CreateProcess, possibly -1
        // or 0.  -1 is definitely invalid, while 0 is probably invalid.
        // Also note each handle can independently be invalid or good.
        // For Windows apps, the console handles are set to values like 3, 7, 
        // and 11 but are invalid handles - you may not write to them.  However,
        // you can still spawn a Windows app via CreateProcess and read stdout
        // and stderr.
        // So, we always need to check each handle independently for validity
        // by trying to write or read to it, unless it is -1.

        // We do not do a security check here, under the assumption that this
        // cannot create a security hole, but only waste a user's time or 
        // cause a possible denial of service attack.

        private static void InitializeStdOutError(bool stdout)
        {
            // Set up Console.Out or Console.Error.
            lock (InternalSyncObject)
            {
                if (stdout && _out != null)
                {
                    return;
                }
                else if (!stdout && _error != null)
                {
                    return;
                }

                TextWriter writer = null;
#if USE_DEBUG_CONSOLE
                writer = new __DebugOutputTextWriter();
#else
                Stream s;
                if (stdout)
                {
                    s = OpenStandardOutput(DefaultConsoleBufferSize);
                }
                else
                {
                    s = OpenStandardError(DefaultConsoleBufferSize);
                }

                if (s == Stream.Null)
                {
                    writer = TextWriter.Synchronized(StreamWriter.Null);
                }
                else
                {
                    Encoding encoding = OutputEncoding;
                    StreamWriter stdxxx = new StreamWriter(s, encoding, DefaultConsoleBufferSize, true);
                    stdxxx.HaveWrittenPreamble = true;
                    stdxxx.AutoFlush = true;
                    writer = TextWriter.Synchronized(stdxxx);
                }
#endif // USE_DEBUG_CONSOLE

                if (stdout)
                {
                    _out = writer;
                }
                else
                {
                    _error = writer;
                }
            }
        }

        private static Stream GetStandardFile(int stdHandleName, FileAccess access, int bufferSize)
        {
            Stream console = new __ConsoleStream(null, access);
            return console;
        }

        public static Encoding InputEncoding
        {
            get
            {
                if (null != _inputEncoding)
                {
                    return _inputEncoding;
                }

                lock (InternalSyncObject)
                {
                    if (null != _inputEncoding)
                    {
                        return _inputEncoding;
                    }

                    _inputEncoding = Encoding.UTF8;
                    return _inputEncoding;
                }
            }
        } // public static Encoding InputEncoding

        public static Encoding OutputEncoding
        {
            get
            {
                if (null != _outputEncoding)
                {
                    return _outputEncoding;
                }

                lock (InternalSyncObject)
                {
                    if (null != _outputEncoding)
                    {
                        return _outputEncoding;
                    }

                    _outputEncoding = Encoding.UTF8;
                    return _outputEncoding;
                }
            }
        } // public static Encoding OutputEncoding

        public static void Beep()
        {
            Beep(800, 200);
        }

        public static void Beep(int frequency, int duration)
        {
            if (frequency < MinBeepFrequency || frequency > MaxBeepFrequency)
            {
                throw new ArgumentOutOfRangeException("frequency");
            }
            if (duration <= 0)
            {
                throw new ArgumentOutOfRangeException("duration");
            }

            // Note that Beep over Remote Desktop connections does not currently

            // work.  Ignore any failures here.
            throw new NotImplementedException();
        }

        public static Stream OpenStandardError()
        {
            return OpenStandardError(DefaultConsoleBufferSize);
        }

        public static Stream OpenStandardError(int bufferSize)
        {
            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize", "NeedNonNegNum");
            }

            return GetStandardFile(STD_ERROR_HANDLE, FileAccess.Write, bufferSize);
        }

        public static Stream OpenStandardInput()
        {
            return OpenStandardInput(DefaultConsoleBufferSize);
        }

        public static Stream OpenStandardInput(int bufferSize)
        {
            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize", "NeedNonNegNum");
            }

            return GetStandardFile(STD_INPUT_HANDLE, FileAccess.Read, bufferSize);
        }

        public static Stream OpenStandardOutput()
        {
            return OpenStandardOutput(DefaultConsoleBufferSize);
        }

        public static Stream OpenStandardOutput(int bufferSize)
        {
            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize", "NeedNonNegNum");
            }

            return GetStandardFile(STD_OUTPUT_HANDLE, FileAccess.Write, bufferSize);
        }

        public static void SetIn(TextReader newIn)
        {
            if (newIn == null)
            {
                throw new ArgumentNullException("newIn");
            }

            newIn = TextReader.Synchronized(newIn);
            lock (InternalSyncObject)
            {
                _in = newIn;
            }
        }

        public static void SetOut(TextWriter newOut)
        {
            if (newOut == null)
            {
                throw new ArgumentNullException("newOut");
            }

            newOut = TextWriter.Synchronized(newOut);
            lock (InternalSyncObject)
            {
                _out = newOut;
            }
        }

        public static void SetError(TextWriter newError)
        {
            if (newError == null)
            {
                throw new ArgumentNullException("newError");
            }

            newError = TextWriter.Synchronized(newError);
            lock (InternalSyncObject)
            {
                _error = newError;
            }
        }

        public static int Read()
        {
            return In.Read();
        }

        public static String ReadLine()
        {
            return In.ReadLine();
        }

        public static void WriteLine()
        {
            Out.WriteLine();
        }

        public static void WriteLine(bool value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(char value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(char[] buffer)
        {
            Out.WriteLine(buffer);
        }

        public static void WriteLine(char[] buffer, int index, int count)
        {
            Out.WriteLine(buffer, index, count);
        }

        public static void WriteLine(decimal value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(double value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(float value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(int value)
        {
            Out.WriteLine(value);
        }

        [CLSCompliant(false)]
        public static void WriteLine(uint value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(long value)
        {
            Out.WriteLine(value);
        }

        [CLSCompliant(false)]
        public static void WriteLine(ulong value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(Object value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(String value)
        {
            Out.WriteLine(value);
        }

        public static void WriteLine(String format, Object arg0)
        {
            Out.WriteLine(format, arg0);
        }

        public static void WriteLine(String format, Object arg0, Object arg1)
        {
            Out.WriteLine(format, arg0, arg1);
        }

        public static void WriteLine(String format, Object arg0, Object arg1, Object arg2)
        {
            Out.WriteLine(format, arg0, arg1, arg2);
        }

        public static void WriteLine(String format, params Object[] arg)
        {
            if (arg == null) // avoid ArgumentNullException from String.Format
            {
                Out.WriteLine(format, null, null); // faster than Out.WriteLine(format, (Object)arg);
            }
            else
            {
                Out.WriteLine(format, arg);
            }
        }

        public static void Write(String format, Object arg0)
        {
            Out.Write(format, arg0);
        }

        public static void Write(String format, Object arg0, Object arg1)
        {
            Out.Write(format, arg0, arg1);
        }

        public static void Write(String format, Object arg0, Object arg1, Object arg2)
        {
            Out.Write(format, arg0, arg1, arg2);
        }

        public static void Write(String format, params Object[] arg)
        {
            if (arg == null) // avoid ArgumentNullException from String.Format
            {
                Out.Write(format, null, null); // faster than Out.Write(format, (Object)arg);
            }
            else
            {
                Out.Write(format, arg);
            }
        }

        public static void Write(bool value)
        {
            Out.Write(value);
        }

        public static void Write(char value)
        {
            Out.Write(value);
        }

        public static void Write(char[] buffer)
        {
            Out.Write(buffer);
        }

        public static void Write(char[] buffer, int index, int count)
        {
            Out.Write(buffer, index, count);
        }

        public static void Write(double value)
        {
            Out.Write(value);
        }

        public static void Write(decimal value)
        {
            Out.Write(value);
        }

        public static void Write(float value)
        {
            Out.Write(value);
        }

        public static void Write(int value)
        {
            Out.Write(value);
        }

        [CLSCompliant(false)]
        public static void Write(uint value)
        {
            Out.Write(value);
        }

        public static void Write(long value)
        {
            Out.Write(value);
        }

        [CLSCompliant(false)]
        public static void Write(ulong value)
        {
            Out.Write(value);
        }

        public static void Write(Object value)
        {
            Out.Write(value);
        }

        public static void Write(String value)
        {
            Out.Write(value);
        }
    } // public static class Console
} // namespace System
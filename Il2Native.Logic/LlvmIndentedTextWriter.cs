namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Text;

    public class LlvmIndentedTextWriter : IndentedTextWriter
    {
        private bool isMethod;
        private StringBuilder sb;
        private IndentedTextWriter sw;

        public LlvmIndentedTextWriter(TextWriter writer)
            : base(writer)
        {
        }

        public void StartMethodBody()
        {
            this.isMethod = true;
            this.sb = new StringBuilder();
            this.sw = new IndentedTextWriter(new StringWriter(this.sb));
            sw.Indent = Indent;
            sw.WriteLine(string.Empty);
        }

        public void EndMethodBody()
        {
            this.sw.Close();

            var savedIndent = Indent;
            Indent = 0;

            var lines = sb.ToString().Split(new [] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (!line.Contains("alloca "))
                {
                    continue;
                }

                base.WriteLine(line);
            }

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.Contains("alloca "))
                {
                    continue;
                }

                base.WriteLine(line);
            }

            this.isMethod = false;

            Indent = savedIndent;
        }

        public override void Write(char c)
        {
            if (!this.isMethod)
            {
                base.Write(c);
            }
            else
            {
                sw.Indent = Indent;
                sw.Write(c);
            }
        }

        public override void Write(int i)
        {
            if (!this.isMethod)
            {
                base.Write(i);
            }
            else
            {
                sw.Indent = Indent;
                sw.Write(i);
            }
        }

        public override void Write(string s)
        {
            if (!this.isMethod)
            {
                base.Write(s);
            }
            else
            {
                sw.Indent = Indent;
                sw.Write(s);
            }
        }

        public override void WriteLine(string s)
        {
            if (!this.isMethod)
            {
                base.WriteLine(s);
            }
            else
            {
                sw.Indent = Indent;
                sw.WriteLine(s);
            }
        }

        public override void Write(string format, object arg0)
        {
            if (!this.isMethod)
            {
                base.Write(format, arg0);
            }
            else
            {
                sw.Indent = Indent;
                sw.Write(format, arg0);
            }
        }

        public override void Write(string format, object arg0, object arg1)
        {
            if (!this.isMethod)
            {
                base.Write(format, arg0, arg1);
            }
            else
            {
                sw.Indent = Indent;
                sw.Write(format, arg0, arg1);
            }
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            if (!this.isMethod)
            {
                base.Write(format, arg0, arg1, arg2);
            }
            else
            {
                sw.Indent = Indent;
                sw.Write(format, arg0, arg1, arg2);
            }
        }

        public override void Write(string format, params object[] arg)
        {
            if (!this.isMethod)
            {
                base.Write(format, arg);
            }
            else
            {
                sw.Indent = Indent;
                sw.Write(format, arg);
            }
        }

        public override void WriteLine(string format, object arg0)
        {
            if (!this.isMethod)
            {
                base.WriteLine(format, arg0);
            }
            else
            {
                sw.Indent = Indent;
                sw.WriteLine(format, arg0);
            }
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            if (!this.isMethod)
            {
                base.WriteLine(format, arg0, arg1);
            }
            else
            {
                sw.Indent = Indent;
                sw.WriteLine(format, arg0, arg1);
            }
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            if (!this.isMethod)
            {
                base.WriteLine(format, arg0, arg1, arg2);
            }
            else
            {
                sw.Indent = Indent;
                sw.WriteLine(format, arg0, arg1, arg2);
            }
        }

        public override void WriteLine(string format, params object[] arg)
        {
            if (!this.isMethod)
            {
                base.WriteLine(format, arg);
            }
            else
            {
                sw.Indent = Indent;
                sw.Write(format, arg);
            }
        }
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LlvmIndentedTextWriter.cs" company="">
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Text;

    /// <summary>
    /// </summary>
    public class LlvmIndentedTextWriter : IndentedTextWriter
    {
        /// <summary>
        /// </summary>
        private bool isMethod;

        /// <summary>
        /// </summary>
        private StringBuilder sb;

        /// <summary>
        /// </summary>
        private IndentedTextWriter sw;

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        public LlvmIndentedTextWriter(TextWriter writer)
            : base(writer)
        {
        }

        /// <summary>
        /// </summary>
        public void StartMethodBody()
        {
            this.isMethod = true;
            this.sb = new StringBuilder();
            this.sw = new IndentedTextWriter(new StringWriter(this.sb));
            this.sw.Indent = this.Indent;
            this.sw.WriteLine(string.Empty);
        }

        /// <summary>
        /// </summary>
        public void EndMethodBody()
        {
            this.sw.Close();

            var savedIndent = this.Indent;
            this.Indent = 0;

            var lines = this.sb.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
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

            this.Indent = savedIndent;
        }

        /// <summary>
        /// </summary>
        /// <param name="c">
        /// </param>
        public override void Write(char c)
        {
            if (!this.isMethod)
            {
                base.Write(c);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.Write(c);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="i">
        /// </param>
        public override void Write(int i)
        {
            if (!this.isMethod)
            {
                base.Write(i);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.Write(i);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="s">
        /// </param>
        public override void Write(string s)
        {
            if (!this.isMethod)
            {
                base.Write(s);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.Write(s);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="s">
        /// </param>
        public override void WriteLine(string s)
        {
            if (!this.isMethod)
            {
                base.WriteLine(s);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.WriteLine(s);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="arg0">
        /// </param>
        public override void Write(string format, object arg0)
        {
            if (!this.isMethod)
            {
                base.Write(format, arg0);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.Write(format, arg0);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="arg0">
        /// </param>
        /// <param name="arg1">
        /// </param>
        public override void Write(string format, object arg0, object arg1)
        {
            if (!this.isMethod)
            {
                base.Write(format, arg0, arg1);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.Write(format, arg0, arg1);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="arg0">
        /// </param>
        /// <param name="arg1">
        /// </param>
        /// <param name="arg2">
        /// </param>
        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            if (!this.isMethod)
            {
                base.Write(format, arg0, arg1, arg2);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.Write(format, arg0, arg1, arg2);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="arg">
        /// </param>
        public override void Write(string format, params object[] arg)
        {
            if (!this.isMethod)
            {
                base.Write(format, arg);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.Write(format, arg);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="arg0">
        /// </param>
        public override void WriteLine(string format, object arg0)
        {
            if (!this.isMethod)
            {
                base.WriteLine(format, arg0);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.WriteLine(format, arg0);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="arg0">
        /// </param>
        /// <param name="arg1">
        /// </param>
        public override void WriteLine(string format, object arg0, object arg1)
        {
            if (!this.isMethod)
            {
                base.WriteLine(format, arg0, arg1);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.WriteLine(format, arg0, arg1);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="arg0">
        /// </param>
        /// <param name="arg1">
        /// </param>
        /// <param name="arg2">
        /// </param>
        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            if (!this.isMethod)
            {
                base.WriteLine(format, arg0, arg1, arg2);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.WriteLine(format, arg0, arg1, arg2);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="format">
        /// </param>
        /// <param name="arg">
        /// </param>
        public override void WriteLine(string format, params object[] arg)
        {
            if (!this.isMethod)
            {
                base.WriteLine(format, arg);
            }
            else
            {
                this.sw.Indent = this.Indent;
                this.sw.Write(format, arg);
            }
        }
    }
}
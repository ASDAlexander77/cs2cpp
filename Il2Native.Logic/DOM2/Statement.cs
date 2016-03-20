// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
namespace Il2Native.Logic.DOM2
{
    public abstract class Statement : Base
    {
        public override Kinds Kind
        {
            get { return Kinds.Statement; }
        }

        public bool SuppressEnding { get; set; }

        internal override void WriteTo(CCodeWriterBase c)
        {
            if (!this.SuppressEnding)
            {
                c.EndStatement();
            }
        }
    }
}

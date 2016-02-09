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

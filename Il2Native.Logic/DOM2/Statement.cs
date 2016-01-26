namespace Il2Native.Logic.DOM2
{
    public abstract class Statement : Base
    {
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

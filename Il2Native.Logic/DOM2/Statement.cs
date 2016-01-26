namespace Il2Native.Logic.DOM2
{
    public abstract class Statement : Base
    {
        internal override void WriteTo(CCodeWriterBase c)
        {
            c.EndStatement();
        }
    }
}

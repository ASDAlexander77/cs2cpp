namespace Il2Native.Logic
{
    using DOM;

    public abstract class CCodeDefinition : CCodeBase
    {
        public abstract bool IsGeneric
        {
            get;
        }

        public bool IsStub { get; set; }
    }
}

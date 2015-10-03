namespace System
{
    public abstract partial class Delegate
    {
        public IntPtr MethodPtr
        {
            get { return this._methodPtr; }
        }
    }
}



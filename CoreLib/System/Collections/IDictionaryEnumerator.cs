namespace System.Collections
{
    using System;
    [System.Runtime.InteropServices.ComVisible(true)]
    public interface IDictionaryEnumerator : IEnumerator
    {
        Object Key
        {
            get;
        }

        Object Value
        {
            get;
        }

        DictionaryEntry Entry
        {
            get;
        }
    }
}

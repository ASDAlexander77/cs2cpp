// Licensed under the MIT license.

namespace System.Reflection
{
    // Define the delegate
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(true)]
    public delegate bool TypeFilter(Type m, object filterCriteria);
}

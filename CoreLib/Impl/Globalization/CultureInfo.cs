namespace System.Globalization
{
    using Runtime.CompilerServices;

    public partial class CultureInfo
    {
        private static bool InternalGetDefaultLocaleName(int localetype, StringHandleOnStack localeString)
        {
            // to force loading Invariant
            return true;
        }

        private static bool InternalGetUserDefaultUILanguage(StringHandleOnStack userDefaultUiLanguage)
        {
            return true;
        }
    }
}

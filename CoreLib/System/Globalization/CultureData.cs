namespace System.Globalization
{
    using System;

    //
    // List of culture data
    // Note the we cache overrides.
    // Note that localized names (resource names) aren't available from here.
    //

    //
    // Our names are a tad confusing.
    //
    // sWindowsName -- The name that windows thinks this culture is, ie:
    //                            en-US if you pass in en-US
    //                            de-DE_phoneb if you pass in de-DE_phoneb
    //                            fj-FJ if you pass in fj (neutral, on a pre-Windows 7 machine)
    //                            fj if you pass in fj (neutral, post-Windows 7 machine)
    //
    // sRealName -- The name you used to construct the culture, in pretty form
    //                       en-US if you pass in EN-us
    //                       en if you pass in en
    //                       de-DE_phoneb if you pass in de-DE_phoneb
    //
    // sSpecificCulture -- The specific culture for this culture
    //                             en-US for en-US
    //                             en-US for en
    //                             de-DE_phoneb for alt sort
    //                             fj-FJ for fj (neutral)
    //
    // sName -- The IETF name of this culture (ie: no sort info, could be neutral)
    //                en-US if you pass in en-US
    //                en if you pass in en
    //                de-DE if you pass in de-DE_phoneb
    //

    // StructLayout is needed here otherwise compiler can re-arrange the fields.
    // We have to keep this in-[....] with the definition in comnlsinfo.h
    //
    // WARNING WARNING WARNING
    //
    // WARNING: Anything changed here also needs to be updated on the native side (object.h see type CultureDataBaseObject)
    // WARNING: The type loader will rearrange class member offsets so the mscorwks!CultureDataBaseObject
    // WARNING: must be manually structured to match the true loaded class layout
    //
    internal class CultureData
    {
        const int undef = -1;

        // Override flag
        private String sRealName; // Name you passed in (ie: en-US, en, or de-DE_phoneb)
        private String sWindowsName; // Name OS thinks the object is (ie: de-DE_phoneb, or en-US (even if en was passed in))

        // Identity
        private String sName; // locale name (ie: en-us, NO sort info, but could be neutral)
        private String sParent; // Parent name (which may be a custom locale/culture)
        private String sLocalizedDisplayName; // Localized pretty name for this locale
        private String sEnglishDisplayName; // English pretty name for this locale
        private String sNativeDisplayName; // Native pretty name for this locale
        private String sSpecificCulture; // The culture name to be used in CultureInfo.CreateSpecificCulture(), en-US form if neutral, sort name if sort

        // Language
        private String sISO639Language; // ISO 639 Language Name
        private String sLocalizedLanguage; // Localized name for this language
        private String sEnglishLanguage; // English name for this language
        private String sNativeLanguage; // Native name of this language

        // Region
        private String sRegionName; // (RegionInfo)
        //        private int    iCountry=undef           ; // (user can override) ---- code (RegionInfo)
        private int iGeoId = undef; // GeoId
        private String sLocalizedCountry; // localized country name
        private String sEnglishCountry; // english country name (RegionInfo)
        private String sNativeCountry; // native country name
        private String sISO3166CountryName; // ISO 3166 (RegionInfo), ie: US

        // Numbers
        private String sPositiveSign; // (user can override) positive sign
        private String sNegativeSign; // (user can override) negative sign
        private String[] saNativeDigits; // (user can override) native characters for digits 0-9
        // (nfi populates these 5, don't have to be = undef)
        private int iDigitSubstitution; // (user can override) Digit substitution 0=context, 1=none/arabic, 2=Native/national (2 seems to be unused)
        private int iLeadingZeros; // (user can override) leading zeros 0 = no leading zeros, 1 = leading zeros
        private int iDigits; // (user can override) number of fractional digits
        private int iNegativeNumber; // (user can override) negative number format
        private int[] waGrouping; // (user can override) grouping of digits
        private String sDecimalSeparator; // (user can override) decimal separator
        private String sThousandSeparator; // (user can override) thousands separator
        private String sNaN; // Not a Number
        private String sPositiveInfinity; // + Infinity
        private String sNegativeInfinity; // - Infinity

        // Percent
        private int iNegativePercent = undef; // Negative Percent (0-3)
        private int iPositivePercent = undef; // Positive Percent (0-11)
        private String sPercent; // Percent (%) symbol
        private String sPerMille; // PerMille (‰) symbol

        // Currency
        private String sCurrency; // (user can override) local monetary symbol
        private String sIntlMonetarySymbol; // international monetary symbol (RegionInfo)
        private String sEnglishCurrency; // English name for this currency
        private String sNativeCurrency; // Native name for this currency
        // (nfi populates these 4, don't have to be = undef)
        private int iCurrencyDigits; // (user can override) # local monetary fractional digits
        private int iCurrency; // (user can override) positive currency format
        private int iNegativeCurrency; // (user can override) negative currency format
        private int[] waMonetaryGrouping; // (user can override) monetary grouping of digits
        private String sMonetaryDecimal; // (user can override) monetary decimal separator
        private String sMonetaryThousand; // (user can override) monetary thousands separator

        // Misc
        private int iMeasure = undef; // (user can override) system of measurement 0=metric, 1=US (RegionInfo)
        private String sListSeparator; // (user can override) list separator
        //        private int    iPaperSize               ; // default paper size (RegionInfo)

        // Time
        private String sAM1159; // (user can override) AM designator
        private String sPM2359; // (user can override) PM designator
        private String sTimeSeparator;
        private volatile String[] saLongTimes; // (user can override) time format
        private volatile String[] saShortTimes; // short time format
        private volatile String[] saDurationFormats; // time duration format

        // Calendar specific data
        private int iFirstDayOfWeek = undef; // (user can override) first day of week (gregorian really)
        private int iFirstWeekOfYear = undef; // (user can override) first week of year (gregorian really)
        private volatile int[] waCalendars; // all available calendar type(s).  The first one is the default calendar


        // Text information
        private int iReadingLayout = undef; // Reading layout data
        // 0 - Left to right (eg en-US)
        // 1 - Right to left (eg arabic locales)
        // 2 - Vertical top to bottom with columns to the left and also left to right (ja-JP locales)
        // 3 - Vertical top to bottom with columns proceeding to the right
        private String sTextInfo; // Text info name to use for custom
        private String sCompareInfo; // Compare info name (including sorting key) to use if custom
        private String sScripts; // Typical Scripts for this locale (latn;cyrl; etc)

        // CoreCLR depends on this even though its not exposed publicly.
        private int iDefaultAnsiCodePage = undef; // default ansi code page ID (ACP)
        private int iDefaultOemCodePage = undef; // default oem code page ID (OCP or OEM)
        private int iDefaultMacCodePage = undef; // default macintosh code page
        private int iDefaultEbcdicCodePage = undef; // default EBCDIC code page

        // These are desktop only, not coreclr
        private int iLanguage; // locale ID (0409) - NO sort information
        private String sAbbrevLang; // abbreviated language name (Windows Language Name) ex: ENU
        private String sAbbrevCountry; // abbreviated country name (RegionInfo) (Windows Region Name) ex: USA
        private String sISO639Language2; // 3 char ISO 639 lang name 2 ex: eng
        private String sISO3166CountryName2; // 3 char ISO 3166 country name 2 2(RegionInfo) ex: USA (ISO)
        private int iInputLanguageHandle = undef;// input language handle
        private String sConsoleFallbackName; // The culture name for the console fallback UI culture
        private String sKeyboardsToInstall; // Keyboard installation string.
        private String fontSignature; // Font signature (16 WORDS)

        // The bools all need to be in one spot
        private bool bUseOverrides; // use user overrides?
        private bool bNeutral; // Flags for the culture (ie: neutral or not right now)
        private bool bWin32Installed; // Flags indicate if the culture is Win32 installed
        private bool bFramework; // Flags for indicate if the culture is one of Whidbey cultures

        /////////////////////////////////////////////////////////////////////////
        // Build our invariant information
        //
        // We need an invariant instance, which we build hard-coded
        /////////////////////////////////////////////////////////////////////////
        internal static CultureData Invariant
        {
            get
            {
                if (s_Invariant == null)
                {
                    // Make a new culturedata
                    CultureData invariant = new CultureData();

                    // Call the native code to get the value of bWin32Installed.
                    // For versions <= Vista, we set this to false for compatibility with v2.
                    // For Windows 7, the flag is true.
                    invariant.bUseOverrides = false;
                    invariant.sRealName = "";

                    // Basics
                    // Note that we override the resources since this IS NOT supposed to change (by definition)
                    invariant.bUseOverrides = false;
                    invariant.sRealName = "";                     // Name you passed in (ie: en-US, en, or de-DE_phoneb)
                    invariant.sWindowsName = "";                     // Name OS thinks the object is (ie: de-DE_phoneb, or en-US (even if en was passed in))

                    // Identity
                    invariant.sName = "";                     // locale name (ie: en-us)
                    invariant.sParent = "";                     // Parent name (which may be a custom locale/culture)
                    invariant.bNeutral = false;                   // Flags for the culture (ie: neutral or not right now)

                    // Don't set invariant.bWin32Installed, we used nativeInitCultureData for that.
                    invariant.bFramework = true;

                    invariant.sEnglishDisplayName = "Invariant Language (Invariant Country)"; // English pretty name for this locale
                    invariant.sNativeDisplayName = "Invariant Language (Invariant Country)";  // Native pretty name for this locale
                    invariant.sSpecificCulture = "";                     // The culture name to be used in CultureInfo.CreateSpecificCulture()

                    // Language
                    invariant.sISO639Language = "iv";                   // ISO 639 Language Name
                    invariant.sLocalizedLanguage = "Invariant Language";   // Display name for this Language
                    invariant.sEnglishLanguage = "Invariant Language";   // English name for this language
                    invariant.sNativeLanguage = "Invariant Language";   // Native name of this language

                    // Region
                    invariant.sRegionName = "IV";                   // (RegionInfo)
                    // Unused for now:
                    //            invariant.iCountry              =1;                      // ---- code (RegionInfo)
                    invariant.iGeoId = 244;                    // GeoId (Windows Only)
                    invariant.sEnglishCountry = "Invariant Country";    // english country name (RegionInfo)
                    invariant.sNativeCountry = "Invariant Country";    // native country name (Windows Only)
                    invariant.sISO3166CountryName = "IV";                   // (RegionInfo), ie: US

                    // Numbers
                    invariant.sPositiveSign = "+";                    // positive sign
                    invariant.sNegativeSign = "-";                    // negative sign
                    invariant.saNativeDigits = new String[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" }; // native characters for digits 0-9
                    invariant.iDigitSubstitution = 1;                      // Digit substitution 0=context, 1=none/arabic, 2=Native/national (2 seems to be unused) (Windows Only)
                    invariant.iLeadingZeros = 1;                      // leading zeros 0=no leading zeros, 1=leading zeros
                    invariant.iDigits = 2;                      // number of fractional digits
                    invariant.iNegativeNumber = 1;                      // negative number format
                    invariant.waGrouping = new int[] { 3 };          // grouping of digits
                    invariant.sDecimalSeparator = ".";                    // decimal separator
                    invariant.sThousandSeparator = ",";                    // thousands separator
                    invariant.sNaN = "NaN";                  // Not a Number
                    invariant.sPositiveInfinity = "Infinity";             // + Infinity
                    invariant.sNegativeInfinity = "-Infinity";            // - Infinity

                    // Percent
                    invariant.iNegativePercent = 0;                      // Negative Percent (0-3)
                    invariant.iPositivePercent = 0;                      // Positive Percent (0-11)
                    invariant.sPercent = "%";                    // Percent (%) symbol
                    invariant.sPerMille = "\x2030";               // PerMille(‰) symbol

                    // Currency
                    invariant.sCurrency = "\x00a4";                // local monetary symbol "¤: for international monetary symbol
                    invariant.sIntlMonetarySymbol = "XDR";                  // international monetary symbol (RegionInfo)
                    invariant.sEnglishCurrency = "International Monetary Fund"; // English name for this currency (Windows Only)
                    invariant.sNativeCurrency = "International Monetary Fund"; // Native name for this currency (Windows Only)
                    invariant.iCurrencyDigits = 2;                      // # local monetary fractional digits
                    invariant.iCurrency = 0;                      // positive currency format
                    invariant.iNegativeCurrency = 0;                      // negative currency format
                    invariant.waMonetaryGrouping = new int[] { 3 };          // monetary grouping of digits
                    invariant.sMonetaryDecimal = ".";                    // monetary decimal separator
                    invariant.sMonetaryThousand = ",";                    // monetary thousands separator

                    // Misc
                    invariant.iMeasure = 0;                      // system of measurement 0=metric, 1=US (RegionInfo)
                    invariant.sListSeparator = ",";                    // list separator
                    // Unused for now:
                    //            invariant.iPaperSize            =9;                      // default paper size (RegionInfo)
                    //            invariant.waFontSignature       ="\x0002\x0000\x0000\x0000\x0000\x0000\x0000\x8000\x0001\x0000\x0000\x8000\x0001\x0000\x0000\x8000"; // Font signature (16 WORDS) (Windows Only)

                    // Time
                    invariant.sAM1159 = "AM";                   // AM designator
                    invariant.sPM2359 = "PM";                   // PM designator
                    invariant.saLongTimes = new String[] { "HH:mm:ss" };                             // time format
                    invariant.saShortTimes = new String[] { "HH:mm", "hh:mm tt", "H:mm", "h:mm tt" }; // short time format
                    invariant.saDurationFormats = new String[] { "HH:mm:ss" };                             // time duration format

                    // Calendar specific data
                    invariant.iFirstDayOfWeek = 0;                      // first day of week
                    invariant.iFirstWeekOfYear = 0;                      // first week of year

                    // Text information
                    invariant.iReadingLayout = 0;                      // Reading Layout = RTL

                    invariant.sTextInfo = "";                     // Text info name to use for custom
                    invariant.sCompareInfo = "";                     // Compare info name (including sorting key) to use if custom
                    invariant.sScripts = "Latn;";                // Typical Scripts for this locale (latn,cyrl, etc)

                    // These are desktop only, not coreclr

                    invariant.iLanguage = 0x007f;                 // locale ID (0409) - NO sort information
                    invariant.iDefaultAnsiCodePage = 1252;                   // default ansi code page ID (ACP)
                    invariant.iDefaultOemCodePage = 437;                    // default oem code page ID (OCP or OEM)
                    invariant.iDefaultMacCodePage = 10000;                  // default macintosh code page
                    invariant.iDefaultEbcdicCodePage = 037;                    // default EBCDIC code page
                    invariant.sAbbrevLang = "IVL";                  // abbreviated language name (Windows Language Name)
                    invariant.sAbbrevCountry = "IVC";                  // abbreviated country name (RegionInfo) (Windows Region Name)
                    invariant.sISO639Language2 = "ivl";                  // 3 char ISO 639 lang name 2
                    invariant.sISO3166CountryName2 = "ivc";                  // 3 char ISO 3166 country name 2 2(RegionInfo)
                    invariant.iInputLanguageHandle = 0x007f;                 // input language handle
                    invariant.sConsoleFallbackName = "";                     // The culture name for the console fallback UI culture
                    invariant.sKeyboardsToInstall = "0409:00000409";        // Keyboard installation string.

                    // Remember it
                    s_Invariant = invariant;
                }
                return s_Invariant;
            }
        }
        private volatile static CultureData s_Invariant;
    }
}

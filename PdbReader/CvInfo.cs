// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="CvInfo.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Microsoft.Cci.Pdb
{
    using System;

    /// <summary>
    /// </summary>
    internal struct FLOAT10
    {
        /// <summary>
        /// </summary>
        internal byte Data_0;

        /// <summary>
        /// </summary>
        internal byte Data_1;

        /// <summary>
        /// </summary>
        internal byte Data_2;

        /// <summary>
        /// </summary>
        internal byte Data_3;

        /// <summary>
        /// </summary>
        internal byte Data_4;

        /// <summary>
        /// </summary>
        internal byte Data_5;

        /// <summary>
        /// </summary>
        internal byte Data_6;

        /// <summary>
        /// </summary>
        internal byte Data_7;

        /// <summary>
        /// </summary>
        internal byte Data_8;

        /// <summary>
        /// </summary>
        internal byte Data_9;
    };

    /// <summary>
    /// </summary>
    internal enum CV_SIGNATURE
    {
        /// <summary>
        /// </summary>
        C6 = 0, // Actual signature is >64K
        /// <summary>
        /// </summary>
        C7 = 1, // First explicit signature
        /// <summary>
        /// </summary>
        C11 = 2, // C11 (vc5.x) 32-bit types
        /// <summary>
        /// </summary>
        C13 = 4, // C13 (vc7.x) zero terminated names
        /// <summary>
        /// </summary>
        RESERVERD = 5, // All signatures from 5 to 64K are reserved
    };

    // CodeView Symbol and Type OMF type information is broken up into two
    // ranges.  Type indices less than 0x1000 describe type information
    // that is frequently used.  Type indices above 0x1000 are used to
    // describe more complex features such as functions, arrays and
    // structures.

    // Primitive types have predefined meaning that is encoded in the
    // values of the various bit fields in the value.
    // A CodeView primitive type is defined as:
    // 1 1
    // 1 089  7654  3  210
    // r mode type  r  sub
    // Where
    // mode is the pointer mode
    // type is a type indicator
    // sub  is a subtype enumeration
    // r    is a reserved field
    // See Microsoft Symbol and Type OMF (Version 4.0) for more
    // information.

    // pointer mode enumeration values

    /// <summary>
    /// </summary>
    internal enum CV_prmode
    {
        /// <summary>
        /// </summary>
        CV_TM_DIRECT = 0, // mode is not a pointer
        /// <summary>
        /// </summary>
        CV_TM_NPTR32 = 4, // mode is a 32 bit near pointer
        /// <summary>
        /// </summary>
        CV_TM_NPTR64 = 6, // mode is a 64 bit near pointer
        /// <summary>
        /// </summary>
        CV_TM_NPTR128 = 7, // mode is a 128 bit near pointer
    };

    // type enumeration values

    /// <summary>
    /// </summary>
    internal enum CV_type
    {
        /// <summary>
        /// </summary>
        CV_SPECIAL = 0x00, // special type size values
        /// <summary>
        /// </summary>
        CV_SIGNED = 0x01, // signed integral size values
        /// <summary>
        /// </summary>
        CV_UNSIGNED = 0x02, // unsigned integral size values
        /// <summary>
        /// </summary>
        CV_BOOLEAN = 0x03, // Boolean size values
        /// <summary>
        /// </summary>
        CV_REAL = 0x04, // real number size values
        /// <summary>
        /// </summary>
        CV_COMPLEX = 0x05, // complex number size values
        /// <summary>
        /// </summary>
        CV_SPECIAL2 = 0x06, // second set of special types
        /// <summary>
        /// </summary>
        CV_INT = 0x07, // integral (int) values
        /// <summary>
        /// </summary>
        CV_CVRESERVED = 0x0f, 
    };

    // subtype enumeration values for CV_SPECIAL

    /// <summary>
    /// </summary>
    internal enum CV_special
    {
        /// <summary>
        /// </summary>
        CV_SP_NOTYPE = 0x00, 

        /// <summary>
        /// </summary>
        CV_SP_ABS = 0x01, 

        /// <summary>
        /// </summary>
        CV_SP_SEGMENT = 0x02, 

        /// <summary>
        /// </summary>
        CV_SP_VOID = 0x03, 

        /// <summary>
        /// </summary>
        CV_SP_CURRENCY = 0x04, 

        /// <summary>
        /// </summary>
        CV_SP_NBASICSTR = 0x05, 

        /// <summary>
        /// </summary>
        CV_SP_FBASICSTR = 0x06, 

        /// <summary>
        /// </summary>
        CV_SP_NOTTRANS = 0x07, 

        /// <summary>
        /// </summary>
        CV_SP_HRESULT = 0x08, 
    };

    // subtype enumeration values for CV_SPECIAL2

    /// <summary>
    /// </summary>
    internal enum CV_special2
    {
        /// <summary>
        /// </summary>
        CV_S2_BIT = 0x00, 

        /// <summary>
        /// </summary>
        CV_S2_PASCHAR = 0x01, // Pascal CHAR
    };

    // subtype enumeration values for CV_SIGNED, CV_UNSIGNED and CV_BOOLEAN

    /// <summary>
    /// </summary>
    internal enum CV_integral
    {
        /// <summary>
        /// </summary>
        CV_IN_1BYTE = 0x00, 

        /// <summary>
        /// </summary>
        CV_IN_2BYTE = 0x01, 

        /// <summary>
        /// </summary>
        CV_IN_4BYTE = 0x02, 

        /// <summary>
        /// </summary>
        CV_IN_8BYTE = 0x03, 

        /// <summary>
        /// </summary>
        CV_IN_16BYTE = 0x04, 
    };

    // subtype enumeration values for CV_REAL and CV_COMPLEX

    /// <summary>
    /// </summary>
    internal enum CV_real
    {
        /// <summary>
        /// </summary>
        CV_RC_REAL32 = 0x00, 

        /// <summary>
        /// </summary>
        CV_RC_REAL64 = 0x01, 

        /// <summary>
        /// </summary>
        CV_RC_REAL80 = 0x02, 

        /// <summary>
        /// </summary>
        CV_RC_REAL128 = 0x03, 
    };

    // subtype enumeration values for CV_INT (really int)

    /// <summary>
    /// </summary>
    internal enum CV_int
    {
        /// <summary>
        /// </summary>
        CV_RI_CHAR = 0x00, 

        /// <summary>
        /// </summary>
        CV_RI_INT1 = 0x00, 

        /// <summary>
        /// </summary>
        CV_RI_WCHAR = 0x01, 

        /// <summary>
        /// </summary>
        CV_RI_UINT1 = 0x01, 

        /// <summary>
        /// </summary>
        CV_RI_INT2 = 0x02, 

        /// <summary>
        /// </summary>
        CV_RI_UINT2 = 0x03, 

        /// <summary>
        /// </summary>
        CV_RI_INT4 = 0x04, 

        /// <summary>
        /// </summary>
        CV_RI_UINT4 = 0x05, 

        /// <summary>
        /// </summary>
        CV_RI_INT8 = 0x06, 

        /// <summary>
        /// </summary>
        CV_RI_UINT8 = 0x07, 

        /// <summary>
        /// </summary>
        CV_RI_INT16 = 0x08, 

        /// <summary>
        /// </summary>
        CV_RI_UINT16 = 0x09, 
    };

    /// <summary>
    /// </summary>
    internal struct CV_PRIMITIVE_TYPE
    {
        // function to extract primitive mode, type and size

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool CV_IS_INTERNAL_PTR(TYPE_ENUM typ)
        {
            return CV_IS_PRIMITIVE(typ) && CV_TYPE(typ) == CV_type.CV_CVRESERVED && CV_TYP_IS_PTR(typ);
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool CV_IS_PRIMITIVE(TYPE_ENUM typ)
        {
            return (uint)(typ) < CV_FIRST_NONPRIM;
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static CV_prmode CV_MODE(TYPE_ENUM typ)
        {
            return (CV_prmode)((((uint)typ) & CV_MMASK) >> CV_MSHIFT);
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static uint CV_SUBT(TYPE_ENUM typ)
        {
            return (((uint)typ) & CV_SMASK) >> CV_SSHIFT;
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static CV_type CV_TYPE(TYPE_ENUM typ)
        {
            return (CV_type)((((uint)typ) & CV_TMASK) >> CV_TSHIFT);
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool CV_TYP_IS_COMPLEX(TYPE_ENUM typ)
        {
            return (CV_TYPE(typ) == CV_type.CV_COMPLEX) && CV_TYP_IS_DIRECT(typ);
        }

        // functions to check the type of a primitive

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool CV_TYP_IS_DIRECT(TYPE_ENUM typ)
        {
            return CV_MODE(typ) == CV_prmode.CV_TM_DIRECT;
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool CV_TYP_IS_PTR(TYPE_ENUM typ)
        {
            return CV_MODE(typ) != CV_prmode.CV_TM_DIRECT;
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool CV_TYP_IS_REAL(TYPE_ENUM typ)
        {
            return (CV_TYPE(typ) == CV_type.CV_REAL) && CV_TYP_IS_DIRECT(typ);
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool CV_TYP_IS_SIGNED(TYPE_ENUM typ)
        {
            return ((CV_TYPE(typ) == CV_type.CV_SIGNED) && CV_TYP_IS_DIRECT(typ)) || (typ == TYPE_ENUM.T_INT1) || (typ == TYPE_ENUM.T_INT2)
                    || (typ == TYPE_ENUM.T_INT4) || (typ == TYPE_ENUM.T_INT8) || (typ == TYPE_ENUM.T_INT16) || (typ == TYPE_ENUM.T_RCHAR);
        }

        /// <summary>
        /// </summary>
        /// <param name="typ">
        /// </param>
        /// <returns>
        /// </returns>
        internal static bool CV_TYP_IS_UNSIGNED(TYPE_ENUM typ)
        {
            return ((CV_TYPE(typ) == CV_type.CV_UNSIGNED) && CV_TYP_IS_DIRECT(typ)) || (typ == TYPE_ENUM.T_UINT1) || (typ == TYPE_ENUM.T_UINT2)
                    || (typ == TYPE_ENUM.T_UINT4) || (typ == TYPE_ENUM.T_UINT8) || (typ == TYPE_ENUM.T_UINT16);
        }

        /// <summary>
        /// </summary>
        private const uint CV_MMASK = 0x700; // mode mask

        /// <summary>
        /// </summary>
        private const uint CV_TMASK = 0x0f0; // type mask

        /// <summary>
        /// </summary>
        private const uint CV_SMASK = 0x00f; // subtype mask

        /// <summary>
        /// </summary>
        private const int CV_MSHIFT = 8; // primitive mode right shift count

        /// <summary>
        /// </summary>
        private const int CV_TSHIFT = 4; // primitive type right shift count

        /// <summary>
        /// </summary>
        private const int CV_SSHIFT = 0; // primitive subtype right shift count

        /// <summary>
        /// </summary>
        private const uint CV_FIRST_NONPRIM = 0x1000;
    }

    // selected values for type_index - for a more complete definition, see
    // Microsoft Symbol and Type OMF document

    // Special Types

    /// <summary>
    /// </summary>
    internal enum TYPE_ENUM
    {
        // Special Types

        /// <summary>
        /// </summary>
        T_NOTYPE = 0x0000, // uncharacterized type (no type)
        /// <summary>
        /// </summary>
        T_ABS = 0x0001, // absolute symbol
        /// <summary>
        /// </summary>
        T_SEGMENT = 0x0002, // segment type
        /// <summary>
        /// </summary>
        T_VOID = 0x0003, // void
        /// <summary>
        /// </summary>
        T_HRESULT = 0x0008, // OLE/COM HRESULT
        /// <summary>
        /// </summary>
        T_32PHRESULT = 0x0408, // OLE/COM HRESULT __ptr32//
        /// <summary>
        /// </summary>
        T_64PHRESULT = 0x0608, // OLE/COM HRESULT __ptr64//
        /// <summary>
        /// </summary>
        T_PVOID = 0x0103, // near pointer to void
        /// <summary>
        /// </summary>
        T_PFVOID = 0x0203, // far pointer to void
        /// <summary>
        /// </summary>
        T_PHVOID = 0x0303, // huge pointer to void
        /// <summary>
        /// </summary>
        T_32PVOID = 0x0403, // 32 bit pointer to void
        /// <summary>
        /// </summary>
        T_64PVOID = 0x0603, // 64 bit pointer to void
        /// <summary>
        /// </summary>
        T_CURRENCY = 0x0004, // BASIC 8 byte currency value
        /// <summary>
        /// </summary>
        T_NOTTRANS = 0x0007, // type not translated by cvpack
        /// <summary>
        /// </summary>
        T_BIT = 0x0060, // bit
        /// <summary>
        /// </summary>
        T_PASCHAR = 0x0061, // Pascal CHAR

        // Character types

        /// <summary>
        /// </summary>
        T_CHAR = 0x0010, // 8 bit signed
        /// <summary>
        /// </summary>
        T_32PCHAR = 0x0410, // 32 bit pointer to 8 bit signed
        /// <summary>
        /// </summary>
        T_64PCHAR = 0x0610, // 64 bit pointer to 8 bit signed

        /// <summary>
        /// </summary>
        T_UCHAR = 0x0020, // 8 bit unsigned
        /// <summary>
        /// </summary>
        T_32PUCHAR = 0x0420, // 32 bit pointer to 8 bit unsigned
        /// <summary>
        /// </summary>
        T_64PUCHAR = 0x0620, // 64 bit pointer to 8 bit unsigned

        // really a character types

        /// <summary>
        /// </summary>
        T_RCHAR = 0x0070, // really a char
        /// <summary>
        /// </summary>
        T_32PRCHAR = 0x0470, // 32 bit pointer to a real char
        /// <summary>
        /// </summary>
        T_64PRCHAR = 0x0670, // 64 bit pointer to a real char

        // really a wide character types

        /// <summary>
        /// </summary>
        T_WCHAR = 0x0071, // wide char
        /// <summary>
        /// </summary>
        T_32PWCHAR = 0x0471, // 32 bit pointer to a wide char
        /// <summary>
        /// </summary>
        T_64PWCHAR = 0x0671, // 64 bit pointer to a wide char

        // 8 bit int types

        /// <summary>
        /// </summary>
        T_INT1 = 0x0068, // 8 bit signed int
        /// <summary>
        /// </summary>
        T_32PINT1 = 0x0468, // 32 bit pointer to 8 bit signed int
        /// <summary>
        /// </summary>
        T_64PINT1 = 0x0668, // 64 bit pointer to 8 bit signed int

        /// <summary>
        /// </summary>
        T_UINT1 = 0x0069, // 8 bit unsigned int
        /// <summary>
        /// </summary>
        T_32PUINT1 = 0x0469, // 32 bit pointer to 8 bit unsigned int
        /// <summary>
        /// </summary>
        T_64PUINT1 = 0x0669, // 64 bit pointer to 8 bit unsigned int

        // 16 bit short types

        /// <summary>
        /// </summary>
        T_SHORT = 0x0011, // 16 bit signed
        /// <summary>
        /// </summary>
        T_32PSHORT = 0x0411, // 32 bit pointer to 16 bit signed
        /// <summary>
        /// </summary>
        T_64PSHORT = 0x0611, // 64 bit pointer to 16 bit signed

        /// <summary>
        /// </summary>
        T_USHORT = 0x0021, // 16 bit unsigned
        /// <summary>
        /// </summary>
        T_32PUSHORT = 0x0421, // 32 bit pointer to 16 bit unsigned
        /// <summary>
        /// </summary>
        T_64PUSHORT = 0x0621, // 64 bit pointer to 16 bit unsigned

        // 16 bit int types

        /// <summary>
        /// </summary>
        T_INT2 = 0x0072, // 16 bit signed int
        /// <summary>
        /// </summary>
        T_32PINT2 = 0x0472, // 32 bit pointer to 16 bit signed int
        /// <summary>
        /// </summary>
        T_64PINT2 = 0x0672, // 64 bit pointer to 16 bit signed int

        /// <summary>
        /// </summary>
        T_UINT2 = 0x0073, // 16 bit unsigned int
        /// <summary>
        /// </summary>
        T_32PUINT2 = 0x0473, // 32 bit pointer to 16 bit unsigned int
        /// <summary>
        /// </summary>
        T_64PUINT2 = 0x0673, // 64 bit pointer to 16 bit unsigned int

        // 32 bit long types

        /// <summary>
        /// </summary>
        T_LONG = 0x0012, // 32 bit signed
        /// <summary>
        /// </summary>
        T_ULONG = 0x0022, // 32 bit unsigned
        /// <summary>
        /// </summary>
        T_32PLONG = 0x0412, // 32 bit pointer to 32 bit signed
        /// <summary>
        /// </summary>
        T_32PULONG = 0x0422, // 32 bit pointer to 32 bit unsigned
        /// <summary>
        /// </summary>
        T_64PLONG = 0x0612, // 64 bit pointer to 32 bit signed
        /// <summary>
        /// </summary>
        T_64PULONG = 0x0622, // 64 bit pointer to 32 bit unsigned

        // 32 bit int types

        /// <summary>
        /// </summary>
        T_INT4 = 0x0074, // 32 bit signed int
        /// <summary>
        /// </summary>
        T_32PINT4 = 0x0474, // 32 bit pointer to 32 bit signed int
        /// <summary>
        /// </summary>
        T_64PINT4 = 0x0674, // 64 bit pointer to 32 bit signed int

        /// <summary>
        /// </summary>
        T_UINT4 = 0x0075, // 32 bit unsigned int
        /// <summary>
        /// </summary>
        T_32PUINT4 = 0x0475, // 32 bit pointer to 32 bit unsigned int
        /// <summary>
        /// </summary>
        T_64PUINT4 = 0x0675, // 64 bit pointer to 32 bit unsigned int

        // 64 bit quad types

        /// <summary>
        /// </summary>
        T_QUAD = 0x0013, // 64 bit signed
        /// <summary>
        /// </summary>
        T_32PQUAD = 0x0413, // 32 bit pointer to 64 bit signed
        /// <summary>
        /// </summary>
        T_64PQUAD = 0x0613, // 64 bit pointer to 64 bit signed

        /// <summary>
        /// </summary>
        T_UQUAD = 0x0023, // 64 bit unsigned
        /// <summary>
        /// </summary>
        T_32PUQUAD = 0x0423, // 32 bit pointer to 64 bit unsigned
        /// <summary>
        /// </summary>
        T_64PUQUAD = 0x0623, // 64 bit pointer to 64 bit unsigned

        // 64 bit int types

        /// <summary>
        /// </summary>
        T_INT8 = 0x0076, // 64 bit signed int
        /// <summary>
        /// </summary>
        T_32PINT8 = 0x0476, // 32 bit pointer to 64 bit signed int
        /// <summary>
        /// </summary>
        T_64PINT8 = 0x0676, // 64 bit pointer to 64 bit signed int

        /// <summary>
        /// </summary>
        T_UINT8 = 0x0077, // 64 bit unsigned int
        /// <summary>
        /// </summary>
        T_32PUINT8 = 0x0477, // 32 bit pointer to 64 bit unsigned int
        /// <summary>
        /// </summary>
        T_64PUINT8 = 0x0677, // 64 bit pointer to 64 bit unsigned int

        // 128 bit octet types

        /// <summary>
        /// </summary>
        T_OCT = 0x0014, // 128 bit signed
        /// <summary>
        /// </summary>
        T_32POCT = 0x0414, // 32 bit pointer to 128 bit signed
        /// <summary>
        /// </summary>
        T_64POCT = 0x0614, // 64 bit pointer to 128 bit signed

        /// <summary>
        /// </summary>
        T_UOCT = 0x0024, // 128 bit unsigned
        /// <summary>
        /// </summary>
        T_32PUOCT = 0x0424, // 32 bit pointer to 128 bit unsigned
        /// <summary>
        /// </summary>
        T_64PUOCT = 0x0624, // 64 bit pointer to 128 bit unsigned

        // 128 bit int types

        /// <summary>
        /// </summary>
        T_INT16 = 0x0078, // 128 bit signed int
        /// <summary>
        /// </summary>
        T_32PINT16 = 0x0478, // 32 bit pointer to 128 bit signed int
        /// <summary>
        /// </summary>
        T_64PINT16 = 0x0678, // 64 bit pointer to 128 bit signed int

        /// <summary>
        /// </summary>
        T_UINT16 = 0x0079, // 128 bit unsigned int
        /// <summary>
        /// </summary>
        T_32PUINT16 = 0x0479, // 32 bit pointer to 128 bit unsigned int
        /// <summary>
        /// </summary>
        T_64PUINT16 = 0x0679, // 64 bit pointer to 128 bit unsigned int

        // 32 bit real types

        /// <summary>
        /// </summary>
        T_REAL32 = 0x0040, // 32 bit real
        /// <summary>
        /// </summary>
        T_32PREAL32 = 0x0440, // 32 bit pointer to 32 bit real
        /// <summary>
        /// </summary>
        T_64PREAL32 = 0x0640, // 64 bit pointer to 32 bit real

        // 64 bit real types

        /// <summary>
        /// </summary>
        T_REAL64 = 0x0041, // 64 bit real
        /// <summary>
        /// </summary>
        T_32PREAL64 = 0x0441, // 32 bit pointer to 64 bit real
        /// <summary>
        /// </summary>
        T_64PREAL64 = 0x0641, // 64 bit pointer to 64 bit real

        // 80 bit real types

        /// <summary>
        /// </summary>
        T_REAL80 = 0x0042, // 80 bit real
        /// <summary>
        /// </summary>
        T_32PREAL80 = 0x0442, // 32 bit pointer to 80 bit real
        /// <summary>
        /// </summary>
        T_64PREAL80 = 0x0642, // 64 bit pointer to 80 bit real

        // 128 bit real types

        /// <summary>
        /// </summary>
        T_REAL128 = 0x0043, // 128 bit real
        /// <summary>
        /// </summary>
        T_32PREAL128 = 0x0443, // 32 bit pointer to 128 bit real
        /// <summary>
        /// </summary>
        T_64PREAL128 = 0x0643, // 64 bit pointer to 128 bit real

        // 32 bit complex types

        /// <summary>
        /// </summary>
        T_CPLX32 = 0x0050, // 32 bit complex
        /// <summary>
        /// </summary>
        T_32PCPLX32 = 0x0450, // 32 bit pointer to 32 bit complex
        /// <summary>
        /// </summary>
        T_64PCPLX32 = 0x0650, // 64 bit pointer to 32 bit complex

        // 64 bit complex types

        /// <summary>
        /// </summary>
        T_CPLX64 = 0x0051, // 64 bit complex
        /// <summary>
        /// </summary>
        T_32PCPLX64 = 0x0451, // 32 bit pointer to 64 bit complex
        /// <summary>
        /// </summary>
        T_64PCPLX64 = 0x0651, // 64 bit pointer to 64 bit complex

        // 80 bit complex types

        /// <summary>
        /// </summary>
        T_CPLX80 = 0x0052, // 80 bit complex
        /// <summary>
        /// </summary>
        T_32PCPLX80 = 0x0452, // 32 bit pointer to 80 bit complex
        /// <summary>
        /// </summary>
        T_64PCPLX80 = 0x0652, // 64 bit pointer to 80 bit complex

        // 128 bit complex types

        /// <summary>
        /// </summary>
        T_CPLX128 = 0x0053, // 128 bit complex
        /// <summary>
        /// </summary>
        T_32PCPLX128 = 0x0453, // 32 bit pointer to 128 bit complex
        /// <summary>
        /// </summary>
        T_64PCPLX128 = 0x0653, // 64 bit pointer to 128 bit complex

        // boolean types

        /// <summary>
        /// </summary>
        T_BOOL08 = 0x0030, // 8 bit boolean
        /// <summary>
        /// </summary>
        T_32PBOOL08 = 0x0430, // 32 bit pointer to 8 bit boolean
        /// <summary>
        /// </summary>
        T_64PBOOL08 = 0x0630, // 64 bit pointer to 8 bit boolean

        /// <summary>
        /// </summary>
        T_BOOL16 = 0x0031, // 16 bit boolean
        /// <summary>
        /// </summary>
        T_32PBOOL16 = 0x0431, // 32 bit pointer to 18 bit boolean
        /// <summary>
        /// </summary>
        T_64PBOOL16 = 0x0631, // 64 bit pointer to 18 bit boolean

        /// <summary>
        /// </summary>
        T_BOOL32 = 0x0032, // 32 bit boolean
        /// <summary>
        /// </summary>
        T_32PBOOL32 = 0x0432, // 32 bit pointer to 32 bit boolean
        /// <summary>
        /// </summary>
        T_64PBOOL32 = 0x0632, // 64 bit pointer to 32 bit boolean

        /// <summary>
        /// </summary>
        T_BOOL64 = 0x0033, // 64 bit boolean
        /// <summary>
        /// </summary>
        T_32PBOOL64 = 0x0433, // 32 bit pointer to 64 bit boolean
        /// <summary>
        /// </summary>
        T_64PBOOL64 = 0x0633, // 64 bit pointer to 64 bit boolean
    };

    // No leaf index can have a value of 0x0000.  The leaf indices are
    // separated into ranges depending upon the use of the type record.
    // The second range is for the type records that are directly referenced
    // in symbols. The first range is for type records that are not
    // referenced by symbols but instead are referenced by other type
    // records.  All type records must have a starting leaf index in these
    // first two ranges.  The third range of leaf indices are used to build
    // up complex lists such as the field list of a class type record.  No
    // type record can begin with one of the leaf indices. The fourth ranges
    // of type indices are used to represent numeric data in a symbol or
    // type record. These leaf indices are greater than 0x8000.  At the
    // point that type or symbol processor is expecting a numeric field, the
    // next two bytes in the type record are examined.  If the value is less
    // than 0x8000, then the two bytes contain the numeric value.  If the
    // value is greater than 0x8000, then the data follows the leaf index in
    // a format specified by the leaf index. The final range of leaf indices
    // are used to force alignment of subfields within a complex type record..

    /// <summary>
    /// </summary>
    internal enum LEAF
    {
        // leaf indices starting records but referenced from symbol records

        /// <summary>
        /// </summary>
        LF_VTSHAPE = 0x000a, 

        /// <summary>
        /// </summary>
        LF_COBOL1 = 0x000c, 

        /// <summary>
        /// </summary>
        LF_LABEL = 0x000e, 

        /// <summary>
        /// </summary>
        LF_NULL = 0x000f, 

        /// <summary>
        /// </summary>
        LF_NOTTRAN = 0x0010, 

        /// <summary>
        /// </summary>
        LF_ENDPRECOMP = 0x0014, // not referenced from symbol
        /// <summary>
        /// </summary>
        LF_TYPESERVER_ST = 0x0016, // not referenced from symbol

        // leaf indices starting records but referenced only from type records

        /// <summary>
        /// </summary>
        LF_LIST = 0x0203, 

        /// <summary>
        /// </summary>
        LF_REFSYM = 0x020c, 

        /// <summary>
        /// </summary>
        LF_ENUMERATE_ST = 0x0403, 

        // 32-bit type index versions of leaves, all have the 0x1000 bit set
        /// <summary>
        /// </summary>
        LF_TI16_MAX = 0x1000, 

        /// <summary>
        /// </summary>
        LF_MODIFIER = 0x1001, 

        /// <summary>
        /// </summary>
        LF_POINTER = 0x1002, 

        /// <summary>
        /// </summary>
        LF_ARRAY_ST = 0x1003, 

        /// <summary>
        /// </summary>
        LF_CLASS_ST = 0x1004, 

        /// <summary>
        /// </summary>
        LF_STRUCTURE_ST = 0x1005, 

        /// <summary>
        /// </summary>
        LF_UNION_ST = 0x1006, 

        /// <summary>
        /// </summary>
        LF_ENUM_ST = 0x1007, 

        /// <summary>
        /// </summary>
        LF_PROCEDURE = 0x1008, 

        /// <summary>
        /// </summary>
        LF_MFUNCTION = 0x1009, 

        /// <summary>
        /// </summary>
        LF_COBOL0 = 0x100a, 

        /// <summary>
        /// </summary>
        LF_BARRAY = 0x100b, 

        /// <summary>
        /// </summary>
        LF_DIMARRAY_ST = 0x100c, 

        /// <summary>
        /// </summary>
        LF_VFTPATH = 0x100d, 

        /// <summary>
        /// </summary>
        LF_PRECOMP_ST = 0x100e, // not referenced from symbol
        /// <summary>
        /// </summary>
        LF_OEM = 0x100f, // oem definable type string
        /// <summary>
        /// </summary>
        LF_ALIAS_ST = 0x1010, // alias (typedef) type
        /// <summary>
        /// </summary>
        LF_OEM2 = 0x1011, // oem definable type string

        // leaf indices starting records but referenced only from type records

        /// <summary>
        /// </summary>
        LF_SKIP = 0x1200, 

        /// <summary>
        /// </summary>
        LF_ARGLIST = 0x1201, 

        /// <summary>
        /// </summary>
        LF_DEFARG_ST = 0x1202, 

        /// <summary>
        /// </summary>
        LF_FIELDLIST = 0x1203, 

        /// <summary>
        /// </summary>
        LF_DERIVED = 0x1204, 

        /// <summary>
        /// </summary>
        LF_BITFIELD = 0x1205, 

        /// <summary>
        /// </summary>
        LF_METHODLIST = 0x1206, 

        /// <summary>
        /// </summary>
        LF_DIMCONU = 0x1207, 

        /// <summary>
        /// </summary>
        LF_DIMCONLU = 0x1208, 

        /// <summary>
        /// </summary>
        LF_DIMVARU = 0x1209, 

        /// <summary>
        /// </summary>
        LF_DIMVARLU = 0x120a, 

        /// <summary>
        /// </summary>
        LF_BCLASS = 0x1400, 

        /// <summary>
        /// </summary>
        LF_VBCLASS = 0x1401, 

        /// <summary>
        /// </summary>
        LF_IVBCLASS = 0x1402, 

        /// <summary>
        /// </summary>
        LF_FRIENDFCN_ST = 0x1403, 

        /// <summary>
        /// </summary>
        LF_INDEX = 0x1404, 

        /// <summary>
        /// </summary>
        LF_MEMBER_ST = 0x1405, 

        /// <summary>
        /// </summary>
        LF_STMEMBER_ST = 0x1406, 

        /// <summary>
        /// </summary>
        LF_METHOD_ST = 0x1407, 

        /// <summary>
        /// </summary>
        LF_NESTTYPE_ST = 0x1408, 

        /// <summary>
        /// </summary>
        LF_VFUNCTAB = 0x1409, 

        /// <summary>
        /// </summary>
        LF_FRIENDCLS = 0x140a, 

        /// <summary>
        /// </summary>
        LF_ONEMETHOD_ST = 0x140b, 

        /// <summary>
        /// </summary>
        LF_VFUNCOFF = 0x140c, 

        /// <summary>
        /// </summary>
        LF_NESTTYPEEX_ST = 0x140d, 

        /// <summary>
        /// </summary>
        LF_MEMBERMODIFY_ST = 0x140e, 

        /// <summary>
        /// </summary>
        LF_MANAGED_ST = 0x140f, 

        // Types w/ SZ names

        /// <summary>
        /// </summary>
        LF_ST_MAX = 0x1500, 

        /// <summary>
        /// </summary>
        LF_TYPESERVER = 0x1501, // not referenced from symbol
        /// <summary>
        /// </summary>
        LF_ENUMERATE = 0x1502, 

        /// <summary>
        /// </summary>
        LF_ARRAY = 0x1503, 

        /// <summary>
        /// </summary>
        LF_CLASS = 0x1504, 

        /// <summary>
        /// </summary>
        LF_STRUCTURE = 0x1505, 

        /// <summary>
        /// </summary>
        LF_UNION = 0x1506, 

        /// <summary>
        /// </summary>
        LF_ENUM = 0x1507, 

        /// <summary>
        /// </summary>
        LF_DIMARRAY = 0x1508, 

        /// <summary>
        /// </summary>
        LF_PRECOMP = 0x1509, // not referenced from symbol
        /// <summary>
        /// </summary>
        LF_ALIAS = 0x150a, // alias (typedef) type
        /// <summary>
        /// </summary>
        LF_DEFARG = 0x150b, 

        /// <summary>
        /// </summary>
        LF_FRIENDFCN = 0x150c, 

        /// <summary>
        /// </summary>
        LF_MEMBER = 0x150d, 

        /// <summary>
        /// </summary>
        LF_STMEMBER = 0x150e, 

        /// <summary>
        /// </summary>
        LF_METHOD = 0x150f, 

        /// <summary>
        /// </summary>
        LF_NESTTYPE = 0x1510, 

        /// <summary>
        /// </summary>
        LF_ONEMETHOD = 0x1511, 

        /// <summary>
        /// </summary>
        LF_NESTTYPEEX = 0x1512, 

        /// <summary>
        /// </summary>
        LF_MEMBERMODIFY = 0x1513, 

        /// <summary>
        /// </summary>
        LF_MANAGED = 0x1514, 

        /// <summary>
        /// </summary>
        LF_TYPESERVER2 = 0x1515, 

        /// <summary>
        /// </summary>
        LF_NUMERIC = 0x8000, 

        /// <summary>
        /// </summary>
        LF_CHAR = 0x8000, 

        /// <summary>
        /// </summary>
        LF_SHORT = 0x8001, 

        /// <summary>
        /// </summary>
        LF_USHORT = 0x8002, 

        /// <summary>
        /// </summary>
        LF_LONG = 0x8003, 

        /// <summary>
        /// </summary>
        LF_ULONG = 0x8004, 

        /// <summary>
        /// </summary>
        LF_REAL32 = 0x8005, 

        /// <summary>
        /// </summary>
        LF_REAL64 = 0x8006, 

        /// <summary>
        /// </summary>
        LF_REAL80 = 0x8007, 

        /// <summary>
        /// </summary>
        LF_REAL128 = 0x8008, 

        /// <summary>
        /// </summary>
        LF_QUADWORD = 0x8009, 

        /// <summary>
        /// </summary>
        LF_UQUADWORD = 0x800a, 

        /// <summary>
        /// </summary>
        LF_COMPLEX32 = 0x800c, 

        /// <summary>
        /// </summary>
        LF_COMPLEX64 = 0x800d, 

        /// <summary>
        /// </summary>
        LF_COMPLEX80 = 0x800e, 

        /// <summary>
        /// </summary>
        LF_COMPLEX128 = 0x800f, 

        /// <summary>
        /// </summary>
        LF_VARSTRING = 0x8010, 

        /// <summary>
        /// </summary>
        LF_OCTWORD = 0x8017, 

        /// <summary>
        /// </summary>
        LF_UOCTWORD = 0x8018, 

        /// <summary>
        /// </summary>
        LF_DECIMAL = 0x8019, 

        /// <summary>
        /// </summary>
        LF_DATE = 0x801a, 

        /// <summary>
        /// </summary>
        LF_UTF8STRING = 0x801b, 

        /// <summary>
        /// </summary>
        LF_PAD0 = 0xf0, 

        /// <summary>
        /// </summary>
        LF_PAD1 = 0xf1, 

        /// <summary>
        /// </summary>
        LF_PAD2 = 0xf2, 

        /// <summary>
        /// </summary>
        LF_PAD3 = 0xf3, 

        /// <summary>
        /// </summary>
        LF_PAD4 = 0xf4, 

        /// <summary>
        /// </summary>
        LF_PAD5 = 0xf5, 

        /// <summary>
        /// </summary>
        LF_PAD6 = 0xf6, 

        /// <summary>
        /// </summary>
        LF_PAD7 = 0xf7, 

        /// <summary>
        /// </summary>
        LF_PAD8 = 0xf8, 

        /// <summary>
        /// </summary>
        LF_PAD9 = 0xf9, 

        /// <summary>
        /// </summary>
        LF_PAD10 = 0xfa, 

        /// <summary>
        /// </summary>
        LF_PAD11 = 0xfb, 

        /// <summary>
        /// </summary>
        LF_PAD12 = 0xfc, 

        /// <summary>
        /// </summary>
        LF_PAD13 = 0xfd, 

        /// <summary>
        /// </summary>
        LF_PAD14 = 0xfe, 

        /// <summary>
        /// </summary>
        LF_PAD15 = 0xff, 
    };

    // end of leaf indices

    // Type enum for pointer records
    // Pointers can be one of the following types

    /// <summary>
    /// </summary>
    internal enum CV_ptrtype
    {
        /// <summary>
        /// </summary>
        CV_PTR_BASE_SEG = 0x03, // based on segment
        /// <summary>
        /// </summary>
        CV_PTR_BASE_VAL = 0x04, // based on value of base
        /// <summary>
        /// </summary>
        CV_PTR_BASE_SEGVAL = 0x05, // based on segment value of base
        /// <summary>
        /// </summary>
        CV_PTR_BASE_ADDR = 0x06, // based on address of base
        /// <summary>
        /// </summary>
        CV_PTR_BASE_SEGADDR = 0x07, // based on segment address of base
        /// <summary>
        /// </summary>
        CV_PTR_BASE_TYPE = 0x08, // based on type
        /// <summary>
        /// </summary>
        CV_PTR_BASE_SELF = 0x09, // based on self
        /// <summary>
        /// </summary>
        CV_PTR_NEAR32 = 0x0a, // 32 bit pointer
        /// <summary>
        /// </summary>
        CV_PTR_64 = 0x0c, // 64 bit pointer
        /// <summary>
        /// </summary>
        CV_PTR_UNUSEDPTR = 0x0d // first unused pointer type
    };

    // Mode enum for pointers
    // Pointers can have one of the following modes

    /// <summary>
    /// </summary>
    internal enum CV_ptrmode
    {
        /// <summary>
        /// </summary>
        CV_PTR_MODE_PTR = 0x00, // "normal" pointer
        /// <summary>
        /// </summary>
        CV_PTR_MODE_REF = 0x01, // reference
        /// <summary>
        /// </summary>
        CV_PTR_MODE_PMEM = 0x02, // pointer to data member
        /// <summary>
        /// </summary>
        CV_PTR_MODE_PMFUNC = 0x03, // pointer to member function
        /// <summary>
        /// </summary>
        CV_PTR_MODE_RESERVED = 0x04 // first unused pointer mode
    };

    // enumeration for pointer-to-member types

    /// <summary>
    /// </summary>
    internal enum CV_pmtype
    {
        /// <summary>
        /// </summary>
        CV_PMTYPE_Undef = 0x00, // not specified (pre VC8)
        /// <summary>
        /// </summary>
        CV_PMTYPE_D_Single = 0x01, // member data, single inheritance
        /// <summary>
        /// </summary>
        CV_PMTYPE_D_Multiple = 0x02, // member data, multiple inheritance
        /// <summary>
        /// </summary>
        CV_PMTYPE_D_Virtual = 0x03, // member data, virtual inheritance
        /// <summary>
        /// </summary>
        CV_PMTYPE_D_General = 0x04, // member data, most general
        /// <summary>
        /// </summary>
        CV_PMTYPE_F_Single = 0x05, // member function, single inheritance
        /// <summary>
        /// </summary>
        CV_PMTYPE_F_Multiple = 0x06, // member function, multiple inheritance
        /// <summary>
        /// </summary>
        CV_PMTYPE_F_Virtual = 0x07, // member function, virtual inheritance
        /// <summary>
        /// </summary>
        CV_PMTYPE_F_General = 0x08, // member function, most general
    };

    // enumeration for method properties

    /// <summary>
    /// </summary>
    internal enum CV_methodprop
    {
        /// <summary>
        /// </summary>
        CV_MTvanilla = 0x00, 

        /// <summary>
        /// </summary>
        CV_MTvirtual = 0x01, 

        /// <summary>
        /// </summary>
        CV_MTstatic = 0x02, 

        /// <summary>
        /// </summary>
        CV_MTfriend = 0x03, 

        /// <summary>
        /// </summary>
        CV_MTintro = 0x04, 

        /// <summary>
        /// </summary>
        CV_MTpurevirt = 0x05, 

        /// <summary>
        /// </summary>
        CV_MTpureintro = 0x06
    };

    // enumeration for virtual shape table entries

    /// <summary>
    /// </summary>
    internal enum CV_VTS_desc
    {
        /// <summary>
        /// </summary>
        CV_VTS_near = 0x00, 

        /// <summary>
        /// </summary>
        CV_VTS_far = 0x01, 

        /// <summary>
        /// </summary>
        CV_VTS_thin = 0x02, 

        /// <summary>
        /// </summary>
        CV_VTS_outer = 0x03, 

        /// <summary>
        /// </summary>
        CV_VTS_meta = 0x04, 

        /// <summary>
        /// </summary>
        CV_VTS_near32 = 0x05, 

        /// <summary>
        /// </summary>
        CV_VTS_far32 = 0x06, 

        /// <summary>
        /// </summary>
        CV_VTS_unused = 0x07
    };

    // enumeration for LF_LABEL address modes

    /// <summary>
    /// </summary>
    internal enum CV_LABEL_TYPE
    {
        /// <summary>
        /// </summary>
        CV_LABEL_NEAR = 0, // near return
        /// <summary>
        /// </summary>
        CV_LABEL_FAR = 4 // far return
    };

    // enumeration for LF_MODIFIER values

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_modifier : ushort
    {
        /// <summary>
        /// </summary>
        MOD_const = 0x0001, 

        /// <summary>
        /// </summary>
        MOD_volatile = 0x0002, 

        /// <summary>
        /// </summary>
        MOD_unaligned = 0x0004, 
    };

    // bit field structure describing class/struct/union/enum properties

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_prop : ushort
    {
        /// <summary>
        /// </summary>
        packed = 0x0001, // true if structure is packed
        /// <summary>
        /// </summary>
        ctor = 0x0002, // true if constructors or destructors present
        /// <summary>
        /// </summary>
        ovlops = 0x0004, // true if overloaded operators present
        /// <summary>
        /// </summary>
        isnested = 0x0008, // true if this is a nested class
        /// <summary>
        /// </summary>
        cnested = 0x0010, // true if this class contains nested types
        /// <summary>
        /// </summary>
        opassign = 0x0020, // true if overloaded assignment (=)
        /// <summary>
        /// </summary>
        opcast = 0x0040, // true if casting methods
        /// <summary>
        /// </summary>
        fwdref = 0x0080, // true if forward reference (incomplete defn)
        /// <summary>
        /// </summary>
        scoped = 0x0100, // scoped definition
    }

    // class field attribute

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_fldattr
    {
        /// <summary>
        /// </summary>
        access = 0x0003, // access protection CV_access_t
        /// <summary>
        /// </summary>
        mprop = 0x001c, // method properties CV_methodprop_t
        /// <summary>
        /// </summary>
        pseudo = 0x0020, // compiler generated fcn and does not exist
        /// <summary>
        /// </summary>
        noinherit = 0x0040, // true if class cannot be inherited
        /// <summary>
        /// </summary>
        noconstruct = 0x0080, // true if class cannot be constructed
        /// <summary>
        /// </summary>
        compgenx = 0x0100, // compiler generated fcn and does exist
    }

    // Structures to access to the type records

    /// <summary>
    /// </summary>
    internal struct TYPTYPE
    {
        /// <summary>
        /// </summary>
        internal ushort leaf;

        /// <summary>
        /// </summary>
        internal ushort len;

        // byte data[];

        // char *NextType (char * pType) {
        // return (pType + ((TYPTYPE *)pType)->len + sizeof(ushort));
        // }
    };

    // general types record

    // memory representation of pointer to member.  These representations are
    // indexed by the enumeration above in the LF_POINTER record

    // representation of a 32 bit pointer to data for a class with
    // or without virtual functions and no virtual bases

    /// <summary>
    /// </summary>
    internal struct CV_PDMR32_NVVFCN
    {
        /// <summary>
        /// </summary>
        internal int mdisp; // displacement to data (NULL = 0x80000000)
    };

    // representation of a 32 bit pointer to data for a class
    // with virtual bases

    /// <summary>
    /// </summary>
    internal struct CV_PDMR32_VBASE
    {
        /// <summary>
        /// </summary>
        internal int mdisp; // displacement to data

        /// <summary>
        /// </summary>
        internal int pdisp; // this pointer displacement

        /// <summary>
        /// </summary>
        internal int vdisp; // vbase table displacement

        // NULL = (,,0xffffffff)
    };

    // representation of a 32 bit pointer to member function for a
    // class with no virtual bases and a single address point

    /// <summary>
    /// </summary>
    internal struct CV_PMFR32_NVSA
    {
        /// <summary>
        /// </summary>
        internal uint off; // near address of function (NULL = 0L)
    };

    // representation of a 32 bit pointer to member function for a
    // class with no virtual bases and multiple address points

    /// <summary>
    /// </summary>
    internal struct CV_PMFR32_NVMA
    {
        /// <summary>
        /// </summary>
        internal int disp;

        /// <summary>
        /// </summary>
        internal uint off; // near address of function (NULL = 0L,x)
    };

    // representation of a 32 bit pointer to member function for a
    // class with virtual bases

    /// <summary>
    /// </summary>
    internal struct CV_PMFR32_VBASE
    {
        /// <summary>
        /// </summary>
        internal int mdisp; // displacement to data

        /// <summary>
        /// </summary>
        internal uint off; // near address of function (NULL = 0L,x,x,x)

        /// <summary>
        /// </summary>
        internal int pdisp; // this pointer displacement

        /// <summary>
        /// </summary>
        internal int vdisp; // vbase table displacement
    };

    //////////////////////////////////////////////////////////////////////////////
    // The following type records are basically variant records of the
    // above structure.  The "ushort leaf" of the above structure and
    // the "ushort leaf" of the following type definitions are the same
    // symbol.

    // Notes on alignment
    // Alignment of the fields in most of the type records is done on the
    // basis of the TYPTYPE record base.  That is why in most of the lf*
    // records that the type is located on what appears to
    // be a offset mod 4 == 2 boundary.  The exception to this rule are those
    // records that are in a list (lfFieldList, lfMethodList), which are
    // aligned to their own bases since they don't have the length field

    // Type record for LF_MODIFIER

    /// <summary>
    /// </summary>
    internal struct LeafModifier
    {
        // internal ushort leaf;      // LF_MODIFIER [TYPTYPE]

        /// <summary>
        /// </summary>
        internal CV_modifier attr; // modifier attribute modifier_t

        /// <summary>
        /// </summary>
        internal uint type; // (type index) modified type
    };

    // type record for LF_POINTER

    /// <summary>
    /// </summary>
    [Flags]
    internal enum LeafPointerAttr : uint
    {
        /// <summary>
        /// </summary>
        ptrtype = 0x0000001f, // ordinal specifying pointer type (CV_ptrtype)
        /// <summary>
        /// </summary>
        ptrmode = 0x000000e0, // ordinal specifying pointer mode (CV_ptrmode)
        /// <summary>
        /// </summary>
        isflat32 = 0x00000100, // true if 0:32 pointer
        /// <summary>
        /// </summary>
        isvolatile = 0x00000200, // TRUE if volatile pointer
        /// <summary>
        /// </summary>
        isconst = 0x00000400, // TRUE if const pointer
        /// <summary>
        /// </summary>
        isunaligned = 0x00000800, // TRUE if unaligned pointer
        /// <summary>
        /// </summary>
        isrestrict = 0x00001000, // TRUE if restricted pointer (allow agressive opts)
    };

    /// <summary>
    /// </summary>
    internal struct LeafPointer
    {
        /// <summary>
        /// </summary>
        internal struct LeafPointerBody
        {
            // internal ushort leaf;  // LF_POINTER [TYPTYPE]

            /// <summary>
            /// </summary>
            internal LeafPointerAttr attr;

            /// <summary>
            /// </summary>
            internal uint utype; // (type index) type index of the underlying type
        };

#if false
        union {
            internal struct {
                uint    pmclass;    // (type index) index of containing class for pointer to member
                ushort  pmenum;     // enumeration specifying pm format (CV_pmtype)
            };
            ushort  bseg;           // base segment if PTR_BASE_SEG
            byte[]  Sym;            // copy of base symbol record (including length)
            internal struct  {
                uint    index;      // (type index) type index if CV_PTR_BASE_TYPE
                string  name;       // name of base type
            } btype;
        } pbase;
#endif
    }

    // type record for LF_ARRAY

    /// <summary>
    /// </summary>
    internal struct LeafArray
    {
        // internal ushort leaf;      // LF_ARRAY [TYPTYPE]
        /// <summary>
        /// </summary>
        internal byte[] data; // variable length data specifying size in bytes

        /// <summary>
        /// </summary>
        internal uint elemtype; // (type index) type index of element type

        /// <summary>
        /// </summary>
        internal uint idxtype; // (type index) type index of indexing type

        /// <summary>
        /// </summary>
        internal string name;
    };

    // type record for LF_CLASS, LF_STRUCTURE

    /// <summary>
    /// </summary>
    internal struct LeafClass
    {
        // internal ushort leaf;      // LF_CLASS, LF_STRUCT [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort count; // count of number of elements in class

        /// <summary>
        /// </summary>
        internal byte[] data; // data describing length of structure in bytes

        /// <summary>
        /// </summary>
        internal uint derived; // (type index) type index of derived from list if not zero

        /// <summary>
        /// </summary>
        internal uint field; // (type index) type index of LF_FIELD descriptor list

        /// <summary>
        /// </summary>
        internal string name;

        /// <summary>
        /// </summary>
        internal ushort property; // (CV_prop_t) property attribute field (prop_t)

        /// <summary>
        /// </summary>
        internal uint vshape; // (type index) type index of vshape table for this class
    };

    // type record for LF_UNION

    /// <summary>
    /// </summary>
    internal struct LeafUnion
    {
        // internal ushort leaf;      // LF_UNION [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort count; // count of number of elements in class

        /// <summary>
        /// </summary>
        internal byte[] data; // variable length data describing length of

        /// <summary>
        /// </summary>
        internal uint field; // (type index) type index of LF_FIELD descriptor list

        /// <summary>
        /// </summary>
        internal string name;

        /// <summary>
        /// </summary>
        internal ushort property; // (CV_prop_t) property attribute field
    };

    // type record for LF_ALIAS

    /// <summary>
    /// </summary>
    internal struct LeafAlias
    {
        // internal ushort leaf;      // LF_ALIAS [TYPTYPE]

        /// <summary>
        /// </summary>
        internal string name; // alias name

        /// <summary>
        /// </summary>
        internal uint utype; // (type index) underlying type
    };

    // type record for LF_MANAGED

    /// <summary>
    /// </summary>
    internal struct LeafManaged
    {
        // internal ushort leaf;      // LF_MANAGED [TYPTYPE]
        /// <summary>
        /// </summary>
        internal string name; // utf8, zero terminated managed type name
    };

    // type record for LF_ENUM

    /// <summary>
    /// </summary>
    internal struct LeafEnum
    {
        // internal ushort leaf;      // LF_ENUM [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort count; // count of number of elements in class

        /// <summary>
        /// </summary>
        internal uint field; // (type index) type index of LF_FIELD descriptor list

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name of enum

        /// <summary>
        /// </summary>
        internal ushort property; // (CV_propt_t) property attribute field

        /// <summary>
        /// </summary>
        internal uint utype; // (type index) underlying type of the enum
    };

    // Type record for LF_PROCEDURE

    /// <summary>
    /// </summary>
    internal struct LeafProc
    {
        // internal ushort leaf;      // LF_PROCEDURE [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint arglist; // (type index) type index of argument list

        /// <summary>
        /// </summary>
        internal byte calltype; // calling convention (CV_call_t)

        /// <summary>
        /// </summary>
        internal ushort parmcount; // number of parameters

        /// <summary>
        /// </summary>
        internal byte reserved; // reserved for future use

        /// <summary>
        /// </summary>
        internal uint rvtype; // (type index) type index of return value
    };

    // Type record for member function

    /// <summary>
    /// </summary>
    internal struct LeafMFunc
    {
        // internal ushort leaf;      // LF_MFUNCTION [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint arglist; // (type index) type index of argument list

        /// <summary>
        /// </summary>
        internal byte calltype; // calling convention (call_t)

        /// <summary>
        /// </summary>
        internal uint classtype; // (type index) type index of containing class

        /// <summary>
        /// </summary>
        internal ushort parmcount; // number of parameters

        /// <summary>
        /// </summary>
        internal byte reserved; // reserved for future use

        /// <summary>
        /// </summary>
        internal uint rvtype; // (type index) type index of return value

        /// <summary>
        /// </summary>
        internal int thisadjust; // this adjuster (long because pad required anyway)

        /// <summary>
        /// </summary>
        internal uint thistype; // (type index) type index of this pointer (model specific)
    };

    // type record for virtual function table shape

    /// <summary>
    /// </summary>
    internal struct LeafVTShape
    {
        // internal ushort leaf;      // LF_VTSHAPE [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort count; // number of entries in vfunctable

        /// <summary>
        /// </summary>
        internal byte[] desc; // 4 bit (CV_VTS_desc) descriptors
    };

    // type record for cobol0

    /// <summary>
    /// </summary>
    internal struct LeafCobol0
    {
        // internal ushort leaf;      // LF_COBOL0 [TYPTYPE]

        /// <summary>
        /// </summary>
        internal byte[] data;

        /// <summary>
        /// </summary>
        internal uint type; // (type index) parent type record index
    };

    // type record for cobol1

    /// <summary>
    /// </summary>
    internal struct LeafCobol1
    {
        // internal ushort leaf;      // LF_COBOL1 [TYPTYPE]
        /// <summary>
        /// </summary>
        internal byte[] data;
    };

    // type record for basic array

    /// <summary>
    /// </summary>
    internal struct LeafBArray
    {
        // internal ushort leaf;      // LF_BARRAY [TYPTYPE]
        /// <summary>
        /// </summary>
        internal uint utype; // (type index) type index of underlying type
    };

    // type record for assembler labels

    /// <summary>
    /// </summary>
    internal struct LeafLabel
    {
        // internal ushort leaf;      // LF_LABEL [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort mode; // addressing mode of label
    };

    // type record for dimensioned arrays

    /// <summary>
    /// </summary>
    internal struct LeafDimArray
    {
        // internal ushort leaf;      // LF_DIMARRAY [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint diminfo; // (type index) dimension information

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name

        /// <summary>
        /// </summary>
        internal uint utype; // (type index) underlying type of the array
    };

    // type record describing path to virtual function table

    /// <summary>
    /// </summary>
    internal struct LeafVFTPath
    {
        // internal ushort leaf;      // LF_VFTPATH [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint[] bases; // (type index) bases from root to leaf

        /// <summary>
        /// </summary>
        internal uint count; // count of number of bases in path
    };

    // type record describing inclusion of precompiled types

    /// <summary>
    /// </summary>
    internal struct LeafPreComp
    {
        // internal ushort leaf;      // LF_PRECOMP [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint count; // number of types in inclusion

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name of included type file

        /// <summary>
        /// </summary>
        internal uint signature; // signature

        /// <summary>
        /// </summary>
        internal uint start; // starting type index included
    };

    // type record describing end of precompiled types that can be
    // included by another file

    /// <summary>
    /// </summary>
    internal struct LeafEndPreComp
    {
        // internal ushort leaf;      // LF_ENDPRECOMP [TYPTYPE]
        /// <summary>
        /// </summary>
        internal uint signature; // signature
    };

    // type record for OEM definable type strings

    /// <summary>
    /// </summary>
    internal struct LeafOEM
    {
        // internal ushort leaf;      // LF_OEM [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint count; // count of type indices to follow

        /// <summary>
        /// </summary>
        internal ushort cvOEM; // MS assigned OEM identified

        /// <summary>
        /// </summary>
        internal uint[] index; // (type index) array of type indices followed

        /// <summary>
        /// </summary>
        internal ushort recOEM; // OEM assigned type identifier

        // by OEM defined data
    };

    /// <summary>
    /// </summary>
    internal enum OEM_ID
    {
        /// <summary>
        /// </summary>
        OEM_MS_FORTRAN90 = 0xF090, 

        /// <summary>
        /// </summary>
        OEM_ODI = 0x0010, 

        /// <summary>
        /// </summary>
        OEM_THOMSON_SOFTWARE = 0x5453, 

        /// <summary>
        /// </summary>
        OEM_ODI_REC_BASELIST = 0x0000, 
    };

    /// <summary>
    /// </summary>
    internal struct LeafOEM2
    {
        // internal ushort leaf;      // LF_OEM2 [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint count; // count of type indices to follow

        /// <summary>
        /// </summary>
        internal Guid idOem; // an oem ID (Guid)

        /// <summary>
        /// </summary>
        internal uint[] index; // (type index) array of type indices followed

        // by OEM defined data
    };

    // type record describing using of a type server

    /// <summary>
    /// </summary>
    internal struct LeafTypeServer
    {
        // internal ushort leaf;      // LF_TYPESERVER [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint age; // age of database used by this module

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name of PDB

        /// <summary>
        /// </summary>
        internal uint signature; // signature
    };

    // type record describing using of a type server with v7 (GUID) signatures

    /// <summary>
    /// </summary>
    internal struct LeafTypeServer2
    {
        // internal ushort leaf;      // LF_TYPESERVER2 [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint age; // age of database used by this module

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name of PDB

        /// <summary>
        /// </summary>
        internal Guid sig70; // guid signature
    };

    // description of type records that can be referenced from
    // type records referenced by symbols

    // type record for skip record

    /// <summary>
    /// </summary>
    internal struct LeafSkip
    {
        // internal ushort leaf;      // LF_SKIP [TYPTYPE]

        /// <summary>
        /// </summary>
        internal byte[] data; // pad data

        /// <summary>
        /// </summary>
        internal uint type; // (type index) next valid index
    };

    // argument list leaf

    /// <summary>
    /// </summary>
    internal struct LeafArgList
    {
        // internal ushort leaf;      // LF_ARGLIST [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint[] arg; // (type index) number of arguments

        /// <summary>
        /// </summary>
        internal uint count; // number of arguments
    };

    // derived class list leaf

    /// <summary>
    /// </summary>
    internal struct LeafDerived
    {
        // internal ushort leaf;      // LF_DERIVED [TYPTYPE]
        /// <summary>
        /// </summary>
        internal uint count; // number of arguments

        /// <summary>
        /// </summary>
        internal uint[] drvdcls; // (type index) type indices of derived classes
    };

    // leaf for default arguments

    /// <summary>
    /// </summary>
    internal struct LeafDefArg
    {
        // internal ushort leaf;      // LF_DEFARG [TYPTYPE]

        /// <summary>
        /// </summary>
        internal byte[] expr; // length prefixed expression string

        /// <summary>
        /// </summary>
        internal uint type; // (type index) type of resulting expression
    };

    // list leaf
    // This list should no longer be used because the utilities cannot
    // verify the contents of the list without knowing what type of list
    // it is.  New specific leaf indices should be used instead.

    /// <summary>
    /// </summary>
    internal struct LeafList
    {
        // internal ushort leaf;      // LF_LIST [TYPTYPE]
        /// <summary>
        /// </summary>
        internal byte[] data; // data format specified by indexing type
    };

    // field list leaf
    // This is the header leaf for a complex list of class and structure
    // subfields.

    /// <summary>
    /// </summary>
    internal struct LeafFieldList
    {
        // internal ushort leaf;      // LF_FIELDLIST [TYPTYPE]
        /// <summary>
        /// </summary>
        internal char[] data; // field list sub lists
    };

    // type record for non-static methods and friends in overloaded method list

    /// <summary>
    /// </summary>
    internal struct mlMethod
    {
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t) method attribute

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index to type record for procedure

        /// <summary>
        /// </summary>
        internal ushort pad0; // internal padding, must be 0

        /// <summary>
        /// </summary>
        internal uint[] vbaseoff; // offset in vfunctable if intro virtual
    };

    /// <summary>
    /// </summary>
    internal struct LeafMethodList
    {
        // internal ushort leaf;      // LF_METHODLIST [TYPTYPE]
        /// <summary>
        /// </summary>
        internal byte[] mList; // really a mlMethod type
    };

    // type record for LF_BITFIELD

    /// <summary>
    /// </summary>
    internal struct LeafBitfield
    {
        // internal ushort leaf;      // LF_BITFIELD [TYPTYPE]

        /// <summary>
        /// </summary>
        internal byte length;

        /// <summary>
        /// </summary>
        internal byte position;

        /// <summary>
        /// </summary>
        internal uint type; // (type index) type of bitfield
    };

    // type record for dimensioned array with constant bounds

    /// <summary>
    /// </summary>
    internal struct LeafDimCon
    {
        // internal ushort leaf;      // LF_DIMCONU or LF_DIMCONLU [TYPTYPE]

        /// <summary>
        /// </summary>
        internal byte[] dim; // array of dimension information with

        /// <summary>
        /// </summary>
        internal ushort rank; // number of dimensions

        /// <summary>
        /// </summary>
        internal uint typ; // (type index) type of index

        // either upper bounds or lower/upper bound
    };

    // type record for dimensioned array with variable bounds

    /// <summary>
    /// </summary>
    internal struct LeafDimVar
    {
        // internal ushort leaf;      // LF_DIMVARU or LF_DIMVARLU [TYPTYPE]
        /// <summary>
        /// </summary>
        internal uint[] dim; // (type index) array of type indices for either

        /// <summary>
        /// </summary>
        internal uint rank; // number of dimensions

        /// <summary>
        /// </summary>
        internal uint typ; // (type index) type of index

        // variable upper bound or variable
        // lower/upper bound.  The count of type
        // indices is rank or rank*2 depending on
        // whether it is LFDIMVARU or LF_DIMVARLU.
        // The referenced types must be
        // LF_REFSYM or T_VOID
    };

    // type record for referenced symbol

    /// <summary>
    /// </summary>
    internal struct LeafRefSym
    {
        // internal ushort leaf;      // LF_REFSYM [TYPTYPE]
        /// <summary>
        /// </summary>
        internal byte[] Sym; // copy of referenced symbol record

        // (including length)
    };

    // the following are numeric leaves.  They are used to indicate the
    // size of the following variable length data.  When the numeric
    // data is a single byte less than 0x8000, then the data is output
    // directly.  If the data is more the 0x8000 or is a negative value,
    // then the data is preceeded by the proper index.

    // signed character leaf

    /// <summary>
    /// </summary>
    internal struct LeafChar
    {
        // internal ushort leaf;      // LF_CHAR [TYPTYPE]
        /// <summary>
        /// </summary>
        internal sbyte val; // signed 8-bit value
    };

    // signed short leaf

    /// <summary>
    /// </summary>
    internal struct LeafShort
    {
        // internal ushort leaf;      // LF_SHORT [TYPTYPE]
        /// <summary>
        /// </summary>
        internal short val; // signed 16-bit value
    };

    // ushort leaf

    /// <summary>
    /// </summary>
    internal struct LeafUShort
    {
        // internal ushort leaf;      // LF_ushort [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort val; // unsigned 16-bit value
    };

    // signed (32-bit) long leaf

    /// <summary>
    /// </summary>
    internal struct LeafLong
    {
        // internal ushort leaf;      // LF_LONG [TYPTYPE]
        /// <summary>
        /// </summary>
        internal int val; // signed 32-bit value
    };

    // uint    leaf

    /// <summary>
    /// </summary>
    internal struct LeafULong
    {
        // internal ushort leaf;      // LF_ULONG [TYPTYPE]
        /// <summary>
        /// </summary>
        internal uint val; // unsigned 32-bit value
    };

    // signed quad leaf

    /// <summary>
    /// </summary>
    internal struct LeafQuad
    {
        // internal ushort leaf;      // LF_QUAD [TYPTYPE]
        /// <summary>
        /// </summary>
        internal long val; // signed 64-bit value
    };

    // unsigned quad leaf

    /// <summary>
    /// </summary>
    internal struct LeafUQuad
    {
        // internal ushort leaf;      // LF_UQUAD [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ulong val; // unsigned 64-bit value
    };

    // signed int128 leaf

    /// <summary>
    /// </summary>
    internal struct LeafOct
    {
        // internal ushort leaf;      // LF_OCT [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ulong val0;

        /// <summary>
        /// </summary>
        internal ulong val1; // signed 128-bit value
    };

    // unsigned int128 leaf

    /// <summary>
    /// </summary>
    internal struct LeafUOct
    {
        // internal ushort leaf;      // LF_UOCT [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ulong val0;

        /// <summary>
        /// </summary>
        internal ulong val1; // unsigned 128-bit value
    };

    // real 32-bit leaf

    /// <summary>
    /// </summary>
    internal struct LeafReal32
    {
        // internal ushort leaf;      // LF_REAL32 [TYPTYPE]
        /// <summary>
        /// </summary>
        internal float val; // 32-bit real value
    };

    // real 64-bit leaf

    /// <summary>
    /// </summary>
    internal struct LeafReal64
    {
        // internal ushort leaf;      // LF_REAL64 [TYPTYPE]
        /// <summary>
        /// </summary>
        internal double val; // 64-bit real value
    };

    // real 80-bit leaf

    /// <summary>
    /// </summary>
    internal struct LeafReal80
    {
        // internal ushort leaf;      // LF_REAL80 [TYPTYPE]
        /// <summary>
        /// </summary>
        internal FLOAT10 val; // real 80-bit value
    };

    // real 128-bit leaf

    /// <summary>
    /// </summary>
    internal struct LeafReal128
    {
        // internal ushort leaf;      // LF_REAL128 [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ulong val0;

        /// <summary>
        /// </summary>
        internal ulong val1; // real 128-bit value
    };

    // complex 32-bit leaf

    /// <summary>
    /// </summary>
    internal struct LeafCmplx32
    {
        // internal ushort leaf;      // LF_COMPLEX32 [TYPTYPE]

        /// <summary>
        /// </summary>
        internal float val_imag; // imaginary component

        /// <summary>
        /// </summary>
        internal float val_real; // real component
    };

    // complex 64-bit leaf

    /// <summary>
    /// </summary>
    internal struct LeafCmplx64
    {
        // internal ushort leaf;      // LF_COMPLEX64 [TYPTYPE]

        /// <summary>
        /// </summary>
        internal double val_imag; // imaginary component

        /// <summary>
        /// </summary>
        internal double val_real; // real component
    };

    // complex 80-bit leaf

    /// <summary>
    /// </summary>
    internal struct LeafCmplx80
    {
        // internal ushort leaf;      // LF_COMPLEX80 [TYPTYPE]

        /// <summary>
        /// </summary>
        internal FLOAT10 val_imag; // imaginary component

        /// <summary>
        /// </summary>
        internal FLOAT10 val_real; // real component
    };

    // complex 128-bit leaf

    /// <summary>
    /// </summary>
    internal struct LeafCmplx128
    {
        // internal ushort leaf;      // LF_COMPLEX128 [TYPTYPE]

        /// <summary>
        /// </summary>
        internal ulong val0_imag;

        /// <summary>
        /// </summary>
        internal ulong val0_real;

        /// <summary>
        /// </summary>
        internal ulong val1_imag; // imaginary component

        /// <summary>
        /// </summary>
        internal ulong val1_real; // real component
    };

    // variable length numeric field

    /// <summary>
    /// </summary>
    internal struct LeafVarString
    {
        // internal ushort leaf;      // LF_VARSTRING [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort len; // length of value in bytes

        /// <summary>
        /// </summary>
        internal byte[] value; // value
    };

    // index leaf - contains type index of another leaf
    // a major use of this leaf is to allow the compilers to emit a
    // long complex list (LF_FIELD) in smaller pieces.

    /// <summary>
    /// </summary>
    internal struct LeafIndex
    {
        // internal ushort leaf;      // LF_INDEX [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint index; // (type index) type index of referenced leaf

        /// <summary>
        /// </summary>
        internal ushort pad0; // internal padding, must be 0
    };

    // subfield record for base class field

    /// <summary>
    /// </summary>
    internal struct LeafBClass
    {
        // internal ushort leaf;      // LF_BCLASS [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t) attribute

        /// <summary>
        /// </summary>
        internal uint index; // (type index) type index of base class

        /// <summary>
        /// </summary>
        internal byte[] offset; // variable length offset of base within class
    };

    // subfield record for direct and indirect virtual base class field

    /// <summary>
    /// </summary>
    internal struct LeafVBClass
    {
        // internal ushort leaf;      // LF_VBCLASS | LV_IVBCLASS [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t) attribute

        /// <summary>
        /// </summary>
        internal uint index; // (type index) type index of direct virtual base class

        /// <summary>
        /// </summary>
        internal byte[] vbpoff; // virtual base pointer offset from address point

        /// <summary>
        /// </summary>
        internal uint vbptr; // (type index) type index of virtual base pointer

        // followed by virtual base offset from vbtable
    };

    // subfield record for friend class

    /// <summary>
    /// </summary>
    internal struct LeafFriendCls
    {
        // internal ushort leaf;      // LF_FRIENDCLS [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index to type record of friend class

        /// <summary>
        /// </summary>
        internal ushort pad0; // internal padding, must be 0
    };

    // subfield record for friend function

    /// <summary>
    /// </summary>
    internal struct LeafFriendFcn
    {
        // internal ushort leaf;      // LF_FRIENDFCN [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index to type record of friend function

        /// <summary>
        /// </summary>
        internal string name; // name of friend function

        /// <summary>
        /// </summary>
        internal ushort pad0; // internal padding, must be 0
    };

    // subfield record for non-static data members

    /// <summary>
    /// </summary>
    internal struct LeafMember
    {
        // internal ushort leaf;      // LF_MEMBER [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t)attribute mask

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index of type record for field

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name of field

        /// <summary>
        /// </summary>
        internal byte[] offset; // variable length offset of field
    };

    // type record for static data members

    /// <summary>
    /// </summary>
    internal struct LeafSTMember
    {
        // internal ushort leaf;      // LF_STMEMBER [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t) attribute mask

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index of type record for field

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name of field
    };

    // subfield record for virtual function table pointer

    /// <summary>
    /// </summary>
    internal struct LeafVFuncTab
    {
        // internal ushort leaf;      // LF_VFUNCTAB [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort pad0; // internal padding, must be 0

        /// <summary>
        /// </summary>
        internal uint type; // (type index) type index of pointer
    };

    // subfield record for virtual function table pointer with offset

    /// <summary>
    /// </summary>
    internal struct LeafVFuncOff
    {
        // internal ushort leaf;      // LF_VFUNCOFF [TYPTYPE]
        /// <summary>
        /// </summary>
        internal int offset; // offset of virtual function table pointer

        /// <summary>
        /// </summary>
        internal ushort pad0; // internal padding, must be 0.

        /// <summary>
        /// </summary>
        internal uint type; // (type index) type index of pointer
    };

    // subfield record for overloaded method list

    /// <summary>
    /// </summary>
    internal struct LeafMethod
    {
        // internal ushort leaf;      // LF_METHOD [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort count; // number of occurrences of function

        /// <summary>
        /// </summary>
        internal uint mList; // (type index) index to LF_METHODLIST record

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name of method
    };

    // subfield record for nonoverloaded method

    /// <summary>
    /// </summary>
    internal struct LeafOneMethod
    {
        // internal ushort leaf;      // LF_ONEMETHOD [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t) method attribute

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index to type record for procedure

        /// <summary>
        /// </summary>
        internal string name;

        /// <summary>
        /// </summary>
        internal uint[] vbaseoff; // offset in vfunctable if intro virtual
    };

    // subfield record for enumerate

    /// <summary>
    /// </summary>
    internal struct LeafEnumerate
    {
        // internal ushort leaf;      // LF_ENUMERATE [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t) access

        /// <summary>
        /// </summary>
        internal string name;

        /// <summary>
        /// </summary>
        internal byte[] value; // variable length value field
    };

    // type record for nested (scoped) type definition

    /// <summary>
    /// </summary>
    internal struct LeafNestType
    {
        // internal ushort leaf;      // LF_NESTTYPE [TYPTYPE]

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index of nested type definition

        /// <summary>
        /// </summary>
        internal string name; // length prefixed type name

        /// <summary>
        /// </summary>
        internal ushort pad0; // internal padding, must be 0
    };

    // type record for nested (scoped) type definition, with attributes
    // new records for vC v5.0, no need to have 16-bit ti versions.

    /// <summary>
    /// </summary>
    internal struct LeafNestTypeEx
    {
        // internal ushort leaf;      // LF_NESTTYPEEX [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t) member access

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index of nested type definition

        /// <summary>
        /// </summary>
        internal string name; // length prefixed type name
    };

    // type record for modifications to members

    /// <summary>
    /// </summary>
    internal struct LeafMemberModify
    {
        // internal ushort leaf;      // LF_MEMBERMODIFY [TYPTYPE]
        /// <summary>
        /// </summary>
        internal ushort attr; // (CV_fldattr_t) the new attributes

        /// <summary>
        /// </summary>
        internal uint index; // (type index) index of base class type definition

        /// <summary>
        /// </summary>
        internal string name; // length prefixed member name
    };

    // type record for pad leaf

    /// <summary>
    /// </summary>
    internal struct LeafPad
    {
        /// <summary>
        /// </summary>
        internal byte leaf;
    };

    // Symbol definitions

    /// <summary>
    /// </summary>
    internal enum SYM
    {
        /// <summary>
        /// </summary>
        S_END = 0x0006, // Block, procedure, "with" or thunk end
        /// <summary>
        /// </summary>
        S_OEM = 0x0404, // OEM defined symbol

        /// <summary>
        /// </summary>
        S_REGISTER_ST = 0x1001, // Register variable
        /// <summary>
        /// </summary>
        S_CONSTANT_ST = 0x1002, // constant symbol
        /// <summary>
        /// </summary>
        S_UDT_ST = 0x1003, // User defined type
        /// <summary>
        /// </summary>
        S_COBOLUDT_ST = 0x1004, // special UDT for cobol that does not symbol pack
        /// <summary>
        /// </summary>
        S_MANYREG_ST = 0x1005, // multiple register variable
        /// <summary>
        /// </summary>
        S_BPREL32_ST = 0x1006, // BP-relative
        /// <summary>
        /// </summary>
        S_LDATA32_ST = 0x1007, // Module-local symbol
        /// <summary>
        /// </summary>
        S_GDATA32_ST = 0x1008, // Global data symbol
        /// <summary>
        /// </summary>
        S_PUB32_ST = 0x1009, // a internal symbol (CV internal reserved)
        /// <summary>
        /// </summary>
        S_LPROC32_ST = 0x100a, // Local procedure start
        /// <summary>
        /// </summary>
        S_GPROC32_ST = 0x100b, // Global procedure start
        /// <summary>
        /// </summary>
        S_VFTABLE32 = 0x100c, // address of virtual function table
        /// <summary>
        /// </summary>
        S_REGREL32_ST = 0x100d, // register relative address
        /// <summary>
        /// </summary>
        S_LTHREAD32_ST = 0x100e, // local thread storage
        /// <summary>
        /// </summary>
        S_GTHREAD32_ST = 0x100f, // global thread storage

        /// <summary>
        /// </summary>
        S_LPROCMIPS_ST = 0x1010, // Local procedure start
        /// <summary>
        /// </summary>
        S_GPROCMIPS_ST = 0x1011, // Global procedure start

        // new symbol records for edit and continue information

        /// <summary>
        /// </summary>
        S_FRAMEPROC = 0x1012, // extra frame and proc information
        /// <summary>
        /// </summary>
        S_COMPILE2_ST = 0x1013, // extended compile flags and info

        // new symbols necessary for 16-bit enumerates of IA64 registers
        // and IA64 specific symbols

        /// <summary>
        /// </summary>
        S_MANYREG2_ST = 0x1014, // multiple register variable
        /// <summary>
        /// </summary>
        S_LPROCIA64_ST = 0x1015, // Local procedure start (IA64)
        /// <summary>
        /// </summary>
        S_GPROCIA64_ST = 0x1016, // Global procedure start (IA64)

        // Local symbols for IL
        /// <summary>
        /// </summary>
        S_LOCALSLOT_ST = 0x1017, // local IL sym with field for local slot index
        /// <summary>
        /// </summary>
        S_PARAMSLOT_ST = 0x1018, // local IL sym with field for parameter slot index

        /// <summary>
        /// </summary>
        S_ANNOTATION = 0x1019, // Annotation string literals

        // symbols to support managed code debugging
        /// <summary>
        /// </summary>
        S_GMANPROC_ST = 0x101a, // Global proc
        /// <summary>
        /// </summary>
        S_LMANPROC_ST = 0x101b, // Local proc
        /// <summary>
        /// </summary>
        S_RESERVED1 = 0x101c, // reserved
        /// <summary>
        /// </summary>
        S_RESERVED2 = 0x101d, // reserved
        /// <summary>
        /// </summary>
        S_RESERVED3 = 0x101e, // reserved
        /// <summary>
        /// </summary>
        S_RESERVED4 = 0x101f, // reserved
        /// <summary>
        /// </summary>
        S_LMANDATA_ST = 0x1020, 

        /// <summary>
        /// </summary>
        S_GMANDATA_ST = 0x1021, 

        /// <summary>
        /// </summary>
        S_MANFRAMEREL_ST = 0x1022, 

        /// <summary>
        /// </summary>
        S_MANREGISTER_ST = 0x1023, 

        /// <summary>
        /// </summary>
        S_MANSLOT_ST = 0x1024, 

        /// <summary>
        /// </summary>
        S_MANMANYREG_ST = 0x1025, 

        /// <summary>
        /// </summary>
        S_MANREGREL_ST = 0x1026, 

        /// <summary>
        /// </summary>
        S_MANMANYREG2_ST = 0x1027, 

        /// <summary>
        /// </summary>
        S_MANTYPREF = 0x1028, // Index for type referenced by name from metadata
        /// <summary>
        /// </summary>
        S_UNAMESPACE_ST = 0x1029, // Using namespace

        // Symbols w/ SZ name fields. All name fields contain utf8 encoded strings.
        /// <summary>
        /// </summary>
        S_ST_MAX = 0x1100, // starting point for SZ name symbols

        /// <summary>
        /// </summary>
        S_OBJNAME = 0x1101, // path to object file name
        /// <summary>
        /// </summary>
        S_THUNK32 = 0x1102, // Thunk Start
        /// <summary>
        /// </summary>
        S_BLOCK32 = 0x1103, // block start
        /// <summary>
        /// </summary>
        S_WITH32 = 0x1104, // with start
        /// <summary>
        /// </summary>
        S_LABEL32 = 0x1105, // code label
        /// <summary>
        /// </summary>
        S_REGISTER = 0x1106, // Register variable
        /// <summary>
        /// </summary>
        S_CONSTANT = 0x1107, // constant symbol
        /// <summary>
        /// </summary>
        S_UDT = 0x1108, // User defined type
        /// <summary>
        /// </summary>
        S_COBOLUDT = 0x1109, // special UDT for cobol that does not symbol pack
        /// <summary>
        /// </summary>
        S_MANYREG = 0x110a, // multiple register variable
        /// <summary>
        /// </summary>
        S_BPREL32 = 0x110b, // BP-relative
        /// <summary>
        /// </summary>
        S_LDATA32 = 0x110c, // Module-local symbol
        /// <summary>
        /// </summary>
        S_GDATA32 = 0x110d, // Global data symbol
        /// <summary>
        /// </summary>
        S_PUB32 = 0x110e, // a internal symbol (CV internal reserved)
        /// <summary>
        /// </summary>
        S_LPROC32 = 0x110f, // Local procedure start
        /// <summary>
        /// </summary>
        S_GPROC32 = 0x1110, // Global procedure start
        /// <summary>
        /// </summary>
        S_REGREL32 = 0x1111, // register relative address
        /// <summary>
        /// </summary>
        S_LTHREAD32 = 0x1112, // local thread storage
        /// <summary>
        /// </summary>
        S_GTHREAD32 = 0x1113, // global thread storage

        /// <summary>
        /// </summary>
        S_LPROCMIPS = 0x1114, // Local procedure start
        /// <summary>
        /// </summary>
        S_GPROCMIPS = 0x1115, // Global procedure start
        /// <summary>
        /// </summary>
        S_COMPILE2 = 0x1116, // extended compile flags and info
        /// <summary>
        /// </summary>
        S_MANYREG2 = 0x1117, // multiple register variable
        /// <summary>
        /// </summary>
        S_LPROCIA64 = 0x1118, // Local procedure start (IA64)
        /// <summary>
        /// </summary>
        S_GPROCIA64 = 0x1119, // Global procedure start (IA64)
        /// <summary>
        /// </summary>
        S_LOCALSLOT = 0x111a, // local IL sym with field for local slot index
        /// <summary>
        /// </summary>
        S_SLOT = S_LOCALSLOT, // alias for LOCALSLOT
        /// <summary>
        /// </summary>
        S_PARAMSLOT = 0x111b, // local IL sym with field for parameter slot index

        // symbols to support managed code debugging
        /// <summary>
        /// </summary>
        S_LMANDATA = 0x111c, 

        /// <summary>
        /// </summary>
        S_GMANDATA = 0x111d, 

        /// <summary>
        /// </summary>
        S_MANFRAMEREL = 0x111e, 

        /// <summary>
        /// </summary>
        S_MANREGISTER = 0x111f, 

        /// <summary>
        /// </summary>
        S_MANSLOT = 0x1120, 

        /// <summary>
        /// </summary>
        S_MANMANYREG = 0x1121, 

        /// <summary>
        /// </summary>
        S_MANREGREL = 0x1122, 

        /// <summary>
        /// </summary>
        S_MANMANYREG2 = 0x1123, 

        /// <summary>
        /// </summary>
        S_UNAMESPACE = 0x1124, // Using namespace

        // ref symbols with name fields
        /// <summary>
        /// </summary>
        S_PROCREF = 0x1125, // Reference to a procedure
        /// <summary>
        /// </summary>
        S_DATAREF = 0x1126, // Reference to data
        /// <summary>
        /// </summary>
        S_LPROCREF = 0x1127, // Local Reference to a procedure
        /// <summary>
        /// </summary>
        S_ANNOTATIONREF = 0x1128, // Reference to an S_ANNOTATION symbol
        /// <summary>
        /// </summary>
        S_TOKENREF = 0x1129, // Reference to one of the many MANPROCSYM's

        // continuation of managed symbols
        /// <summary>
        /// </summary>
        S_GMANPROC = 0x112a, // Global proc
        /// <summary>
        /// </summary>
        S_LMANPROC = 0x112b, // Local proc

        // short, light-weight thunks
        /// <summary>
        /// </summary>
        S_TRAMPOLINE = 0x112c, // trampoline thunks
        /// <summary>
        /// </summary>
        S_MANCONSTANT = 0x112d, // constants with metadata type info

        // native attributed local/parms
        /// <summary>
        /// </summary>
        S_ATTR_FRAMEREL = 0x112e, // relative to virtual frame ptr
        /// <summary>
        /// </summary>
        S_ATTR_REGISTER = 0x112f, // stored in a register
        /// <summary>
        /// </summary>
        S_ATTR_REGREL = 0x1130, // relative to register (alternate frame ptr)
        /// <summary>
        /// </summary>
        S_ATTR_MANYREG = 0x1131, // stored in >1 register

        // Separated code (from the compiler) support
        /// <summary>
        /// </summary>
        S_SEPCODE = 0x1132, 

        /// <summary>
        /// </summary>
        S_LOCAL = 0x1133, // defines a local symbol in optimized code
        /// <summary>
        /// </summary>
        S_DEFRANGE = 0x1134, // defines a single range of addresses in which symbol can be evaluated
        /// <summary>
        /// </summary>
        S_DEFRANGE2 = 0x1135, // defines ranges of addresses in which symbol can be evaluated

        /// <summary>
        /// </summary>
        S_SECTION = 0x1136, // A COFF section in a PE executable
        /// <summary>
        /// </summary>
        S_COFFGROUP = 0x1137, // A COFF group
        /// <summary>
        /// </summary>
        S_EXPORT = 0x1138, // A export

        /// <summary>
        /// </summary>
        S_CALLSITEINFO = 0x1139, // Indirect call site information
        /// <summary>
        /// </summary>
        S_FRAMECOOKIE = 0x113a, // Security cookie information

        /// <summary>
        /// </summary>
        S_DISCARDED = 0x113b, // Discarded by LINK /OPT:REF (experimental, see richards)

        /// <summary>
        /// </summary>
        S_RECTYPE_MAX, // one greater than last
        /// <summary>
        /// </summary>
        S_RECTYPE_LAST = S_RECTYPE_MAX - 1, 
    };

    // enum describing compile flag ambient data model

    /// <summary>
    /// </summary>
    internal enum CV_CFL_DATA
    {
        /// <summary>
        /// </summary>
        CV_CFL_DNEAR = 0x00, 

        /// <summary>
        /// </summary>
        CV_CFL_DFAR = 0x01, 

        /// <summary>
        /// </summary>
        CV_CFL_DHUGE = 0x02
    };

    // enum describing compile flag ambiant code model

    /// <summary>
    /// </summary>
    internal enum CV_CFL_CODE
    {
        /// <summary>
        /// </summary>
        CV_CFL_CNEAR = 0x00, 

        /// <summary>
        /// </summary>
        CV_CFL_CFAR = 0x01, 

        /// <summary>
        /// </summary>
        CV_CFL_CHUGE = 0x02
    };

    // enum describing compile flag target floating point package

    /// <summary>
    /// </summary>
    internal enum CV_CFL_FPKG
    {
        /// <summary>
        /// </summary>
        CV_CFL_NDP = 0x00, 

        /// <summary>
        /// </summary>
        CV_CFL_EMU = 0x01, 

        /// <summary>
        /// </summary>
        CV_CFL_ALT = 0x02
    };

    // enum describing function return method

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_PROCFLAGS : byte
    {
        /// <summary>
        /// </summary>
        CV_PFLAG_NOFPO = 0x01, // frame pointer present
        /// <summary>
        /// </summary>
        CV_PFLAG_INT = 0x02, // interrupt return
        /// <summary>
        /// </summary>
        CV_PFLAG_FAR = 0x04, // far return
        /// <summary>
        /// </summary>
        CV_PFLAG_NEVER = 0x08, // function does not return
        /// <summary>
        /// </summary>
        CV_PFLAG_NOTREACHED = 0x10, // label isn't fallen into
        /// <summary>
        /// </summary>
        CV_PFLAG_CUST_CALL = 0x20, // custom calling convention
        /// <summary>
        /// </summary>
        CV_PFLAG_NOINLINE = 0x40, // function marked as noinline
        /// <summary>
        /// </summary>
        CV_PFLAG_OPTDBGINFO = 0x80, // function has debug information for optimized code
    };

    // Extended proc flags
    /// <summary>
    /// </summary>
    internal struct CV_EXPROCFLAGS
    {
        /// <summary>
        /// </summary>
        internal byte flags; // (CV_PROCFLAGS)

        /// <summary>
        /// </summary>
        internal byte reserved; // must be zero
    };

    // local variable flags
    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_LVARFLAGS : ushort
    {
        /// <summary>
        /// </summary>
        fIsParam = 0x0001, // variable is a parameter
        /// <summary>
        /// </summary>
        fAddrTaken = 0x0002, // address is taken
        /// <summary>
        /// </summary>
        fCompGenx = 0x0004, // variable is compiler generated
        /// <summary>
        /// </summary>
        fIsAggregate = 0x0008, // the symbol is splitted in temporaries,
        // which are treated by compiler as
        // independent entities
        /// <summary>
        /// </summary>
        fIsAggregated = 0x0010, // Counterpart of fIsAggregate - tells
        // that it is a part of a fIsAggregate symbol
        /// <summary>
        /// </summary>
        fIsAliased = 0x0020, // variable has multiple simultaneous lifetimes
        /// <summary>
        /// </summary>
        fIsAlias = 0x0040, // represents one of the multiple simultaneous lifetimes
    };

    // represents an address range, used for optimized code debug info
    /// <summary>
    /// </summary>
    internal struct CV_lvar_addr_range
    {
        // defines a range of addresses

        /// <summary>
        /// </summary>
        internal uint cbRange;

        /// <summary>
        /// </summary>
        internal ushort isectStart;

        /// <summary>
        /// </summary>
        internal uint offStart;
    };

    // enum describing function data return method

    /// <summary>
    /// </summary>
    internal enum CV_GENERIC_STYLE
    {
        /// <summary>
        /// </summary>
        CV_GENERIC_VOID = 0x00, // void return type
        /// <summary>
        /// </summary>
        CV_GENERIC_REG = 0x01, // return data is in registers
        /// <summary>
        /// </summary>
        CV_GENERIC_ICAN = 0x02, // indirect caller allocated near
        /// <summary>
        /// </summary>
        CV_GENERIC_ICAF = 0x03, // indirect caller allocated far
        /// <summary>
        /// </summary>
        CV_GENERIC_IRAN = 0x04, // indirect returnee allocated near
        /// <summary>
        /// </summary>
        CV_GENERIC_IRAF = 0x05, // indirect returnee allocated far
        /// <summary>
        /// </summary>
        CV_GENERIC_UNUSED = 0x06 // first unused
    };

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_GENERIC_FLAG : ushort
    {
        /// <summary>
        /// </summary>
        cstyle = 0x0001, // true push varargs right to left
        /// <summary>
        /// </summary>
        rsclean = 0x0002, // true if returnee stack cleanup
    };

    // flag bitfields for separated code attributes

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_SEPCODEFLAGS : uint
    {
        /// <summary>
        /// </summary>
        fIsLexicalScope = 0x00000001, // S_SEPCODE doubles as lexical scope
        /// <summary>
        /// </summary>
        fReturnsToParent = 0x00000002, // code frag returns to parent
    };

    // Generic layout for symbol records

    /// <summary>
    /// </summary>
    internal struct SYMTYPE
    {
        /// <summary>
        /// </summary>
        internal ushort reclen; // Record length

        /// <summary>
        /// </summary>
        internal ushort rectyp; // Record type

        // byte        data[CV_ZEROLEN];
        // SYMTYPE *NextSym (SYMTYPE * pSym) {
        // return (SYMTYPE *) ((char *)pSym + pSym->reclen + sizeof(ushort));
        // }
    };

    // non-model specific symbol types

    /// <summary>
    /// </summary>
    internal struct RegSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_REGISTER

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal ushort reg; // register enumerate

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or Metadata token
    };

    /// <summary>
    /// </summary>
    internal struct AttrRegSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANREGISTER | S_ATTR_REGISTER

        /// <summary>
        /// </summary>
        internal ushort flags; // (CV_LVARFLAGS)local var flags

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint offCod; // first code address where var is live

        /// <summary>
        /// </summary>
        internal ushort reg; // register enumerate

        /// <summary>
        /// </summary>
        internal ushort segCod;

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or Metadata token
    };

    /// <summary>
    /// </summary>
    internal struct ManyRegSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANYREG

        /// <summary>
        /// </summary>
        internal byte count; // count of number of registers

        /// <summary>
        /// </summary>
        internal string name; // length-prefixed name.

        /// <summary>
        /// </summary>
        internal byte[] reg; // count register enumerates, most-sig first

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or metadata token
    };

    /// <summary>
    /// </summary>
    internal struct ManyRegSym2
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANYREG2

        /// <summary>
        /// </summary>
        internal ushort count; // count of number of registers,

        /// <summary>
        /// </summary>
        internal string name; // length-prefixed name.

        /// <summary>
        /// </summary>
        internal ushort[] reg; // count register enumerates, most-sig first

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or metadata token
    };

    /// <summary>
    /// </summary>
    internal struct AttrManyRegSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANMANYREG

        /// <summary>
        /// </summary>
        internal byte count; // count of number of registers

        /// <summary>
        /// </summary>
        internal ushort flags; // (CV_LVARFLAGS)local var flags

        /// <summary>
        /// </summary>
        internal string name; // utf-8 encoded zero terminate name

        /// <summary>
        /// </summary>
        internal uint offCod; // first code address where var is live

        /// <summary>
        /// </summary>
        internal byte[] reg; // count register enumerates, most-sig first

        /// <summary>
        /// </summary>
        internal ushort segCod;

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or metadata token
    };

    /// <summary>
    /// </summary>
    internal struct AttrManyRegSym2
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANMANYREG2 | S_ATTR_MANYREG

        /// <summary>
        /// </summary>
        internal ushort count; // count of number of registers

        /// <summary>
        /// </summary>
        internal ushort flags; // (CV_LVARFLAGS)local var flags

        /// <summary>
        /// </summary>
        internal string name; // utf-8 encoded zero terminate name

        /// <summary>
        /// </summary>
        internal uint offCod; // first code address where var is live

        /// <summary>
        /// </summary>
        internal ushort[] reg; // count register enumerates, most-sig first

        /// <summary>
        /// </summary>
        internal ushort segCod;

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or metadata token
    };

    /// <summary>
    /// </summary>
    internal struct ConstSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_CONSTANT or S_MANCONSTANT
        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index (containing enum if enumerate) or metadata token

        /// <summary>
        /// </summary>
        internal ushort value; // numeric leaf containing value
    };

    /// <summary>
    /// </summary>
    internal struct UdtSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_UDT | S_COBOLUDT

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index
    };

    /// <summary>
    /// </summary>
    internal struct ManyTypRef
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANTYPREF
        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index
    };

    /// <summary>
    /// </summary>
    internal struct SearchSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_SSEARCH

        /// <summary>
        /// </summary>
        internal ushort seg; // segment of symbol

        /// <summary>
        /// </summary>
        internal uint startsym; // offset of the procedure
    };

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CFLAGSYM_FLAGS : ushort
    {
        /// <summary>
        /// </summary>
        pcode = 0x0001, // true if pcode present
        /// <summary>
        /// </summary>
        floatprec = 0x0006, // floating precision
        /// <summary>
        /// </summary>
        floatpkg = 0x0018, // float package
        /// <summary>
        /// </summary>
        ambdata = 0x00e0, // ambient data model
        /// <summary>
        /// </summary>
        ambcode = 0x0700, // ambient code model
        /// <summary>
        /// </summary>
        mode32 = 0x0800, // true if compiled 32 bit mode
    };

    /// <summary>
    /// </summary>
    internal struct CFlagSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_COMPILE

        /// <summary>
        /// </summary>
        internal ushort flags; // (CFLAGSYM_FLAGS)

        /// <summary>
        /// </summary>
        internal byte language; // language index

        /// <summary>
        /// </summary>
        internal byte machine; // target processor

        /// <summary>
        /// </summary>
        internal string ver; // Length-prefixed compiler version string
    };

    /// <summary>
    /// </summary>
    [Flags]
    internal enum COMPILESYM_FLAGS : uint
    {
        /// <summary>
        /// </summary>
        iLanguage = 0x000000ff, // language index
        /// <summary>
        /// </summary>
        fEC = 0x00000100, // compiled for E/C
        /// <summary>
        /// </summary>
        fNoDbgInfo = 0x00000200, // not compiled with debug info
        /// <summary>
        /// </summary>
        fLTCG = 0x00000400, // compiled with LTCG
        /// <summary>
        /// </summary>
        fNoDataAlign = 0x00000800, // compiled with -Bzalign
        /// <summary>
        /// </summary>
        fManagedPresent = 0x00001000, // managed code/data present
        /// <summary>
        /// </summary>
        fSecurityChecks = 0x00002000, // compiled with /GS
        /// <summary>
        /// </summary>
        fHotPatch = 0x00004000, // compiled with /hotpatch
        /// <summary>
        /// </summary>
        fCVTCIL = 0x00008000, // converted with CVTCIL
        /// <summary>
        /// </summary>
        fMSILModule = 0x00010000, // MSIL netmodule
    };

    /// <summary>
    /// </summary>
    internal struct CompileSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_COMPILE2
        /// <summary>
        /// </summary>
        internal uint flags; // (COMPILESYM_FLAGS)

        /// <summary>
        /// </summary>
        internal ushort machine; // target processor

        /// <summary>
        /// </summary>
        internal string[] verArgs; // block of zero terminated strings, ended by double-zero.

        /// <summary>
        /// </summary>
        internal ushort verBuild; // back end build version #

        /// <summary>
        /// </summary>
        internal ushort verFEBuild; // front end build version #

        /// <summary>
        /// </summary>
        internal ushort verFEMajor; // front end major version #

        /// <summary>
        /// </summary>
        internal ushort verFEMinor; // front end minor version #

        /// <summary>
        /// </summary>
        internal ushort verMajor; // back end major version #

        /// <summary>
        /// </summary>
        internal ushort verMinor; // back end minor version #

        /// <summary>
        /// </summary>
        internal string verSt; // Length-prefixed compiler version string, followed
    };

    /// <summary>
    /// </summary>
    internal struct ObjNameSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_OBJNAME

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint signature; // signature
    };

    /// <summary>
    /// </summary>
    internal struct EndArgSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_ENDARG
    };

    /// <summary>
    /// </summary>
    internal struct ReturnSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_RETURN
        /// <summary>
        /// </summary>
        internal CV_GENERIC_FLAG flags; // flags

        /// <summary>
        /// </summary>
        internal byte style; // CV_GENERIC_STYLE return style

        // followed by return method data
    };

    /// <summary>
    /// </summary>
    internal struct EntryThisSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_ENTRYTHIS
        /// <summary>
        /// </summary>
        internal byte thissym; // symbol describing this pointer on entry
    };

    /// <summary>
    /// </summary>
    internal struct BpRelSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_BPREL32
        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal int off; // BP-relative offset

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or Metadata token
    };

    /// <summary>
    /// </summary>
    internal struct FrameRelSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANFRAMEREL | S_ATTR_FRAMEREL
        /// <summary>
        /// </summary>
        internal ushort flags; // (CV_LVARFLAGS)local var flags

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal int off; // Frame relative offset

        /// <summary>
        /// </summary>
        internal uint offCod; // first code address where var is live

        /// <summary>
        /// </summary>
        internal ushort segCod;

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or Metadata token
    };

    /// <summary>
    /// </summary>
    internal struct SlotSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_LOCALSLOT or S_PARAMSLOT
        /// <summary>
        /// </summary>
        internal uint index; // slot index

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or Metadata token
    };

    /// <summary>
    /// </summary>
    internal struct AttrSlotSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANSLOT
        /// <summary>
        /// </summary>
        internal ushort flags; // (CV_LVARFLAGS)local var flags

        /// <summary>
        /// </summary>
        internal uint index; // slot index

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint offCod; // first code address where var is live

        /// <summary>
        /// </summary>
        internal ushort segCod;

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or Metadata token
    };

    /// <summary>
    /// </summary>
    internal struct AnnotationSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_ANNOTATION

        /// <summary>
        /// </summary>
        internal ushort csz; // Count of zero terminated annotation strings

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal string[] rgsz; // Sequence of zero terminated annotation strings

        /// <summary>
        /// </summary>
        internal ushort seg;
    };

    /// <summary>
    /// </summary>
    internal struct DatasSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_LDATA32, S_GDATA32 or S_PUB32, S_LMANDATA, S_GMANDATA

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal ushort seg;

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index, or Metadata token if a managed symbol
    };

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_PUBSYMFLAGS : uint
    {
        /// <summary>
        /// </summary>
        fNone = 0, 

        /// <summary>
        /// </summary>
        fCode = 0x00000001, // set if internal symbol refers to a code address
        /// <summary>
        /// </summary>
        fFunction = 0x00000002, // set if internal symbol is a function
        /// <summary>
        /// </summary>
        fManaged = 0x00000004, // set if managed code (native or IL)
        /// <summary>
        /// </summary>
        fMSIL = 0x00000008, // set if managed IL code
    };

    /// <summary>
    /// </summary>
    internal struct PubSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_PUB32
        /// <summary>
        /// </summary>
        internal uint flags; // (CV_PUBSYMFLAGS)

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal ushort seg;
    };

    /// <summary>
    /// </summary>
    internal struct ProcSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_GPROC32 or S_LPROC32

        /// <summary>
        /// </summary>
        internal uint dbgEnd; // Debug end offset

        /// <summary>
        /// </summary>
        internal uint dbgStart; // Debug start offset

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this blocks end

        /// <summary>
        /// </summary>
        internal byte flags; // (CV_PROCFLAGS) Proc flags

        /// <summary>
        /// </summary>
        internal uint len; // Proc length

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint next; // pointer to next symbol

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal ushort seg;

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index
    };

    /// <summary>
    /// </summary>
    internal struct ManProcSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_GMANPROC, S_LMANPROC, S_GMANPROCIA64 or S_LMANPROCIA64

        /// <summary>
        /// </summary>
        internal uint dbgEnd; // Debug end offset

        /// <summary>
        /// </summary>
        internal uint dbgStart; // Debug start offset

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this blocks end

        /// <summary>
        /// </summary>
        internal byte flags; // (CV_PROCFLAGS) Proc flags

        /// <summary>
        /// </summary>
        internal uint len; // Proc length

        /// <summary>
        /// </summary>
        internal string name; // optional name field

        /// <summary>
        /// </summary>
        internal uint next; // pointer to next symbol

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal ushort retReg; // Register return value is in (may not be used for all archs)

        /// <summary>
        /// </summary>
        internal ushort seg;

        /// <summary>
        /// </summary>
        internal uint token; // COM+ metadata token for method
    };

    /// <summary>
    /// </summary>
    internal struct ManProcSymMips
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_GMANPROCMIPS or S_LMANPROCMIPS

        /// <summary>
        /// </summary>
        internal uint dbgEnd; // Debug end offset

        /// <summary>
        /// </summary>
        internal uint dbgStart; // Debug start offset

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this blocks end

        /// <summary>
        /// </summary>
        internal uint fpOff; // fp register save offset

        /// <summary>
        /// </summary>
        internal uint fpSave; // fp register save mask

        /// <summary>
        /// </summary>
        internal byte frameReg; // Frame pointer register

        /// <summary>
        /// </summary>
        internal uint intOff; // int register save offset

        /// <summary>
        /// </summary>
        internal uint len; // Proc length

        /// <summary>
        /// </summary>
        internal string name; // optional name field

        /// <summary>
        /// </summary>
        internal uint next; // pointer to next symbol

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal uint regSave; // int register save mask

        /// <summary>
        /// </summary>
        internal byte retReg; // Register return value is in

        /// <summary>
        /// </summary>
        internal ushort seg;

        /// <summary>
        /// </summary>
        internal uint token; // COM+ token type
    };

    /// <summary>
    /// </summary>
    internal struct ThunkSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_THUNK32

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this blocks end

        /// <summary>
        /// </summary>
        internal ushort len; // length of thunk

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint next; // pointer to next symbol

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal byte ord; // THUNK_ORDINAL specifying type of thunk

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal ushort seg;

        /// <summary>
        /// </summary>
        internal byte[] variant; // variant portion of thunk
    };

    /// <summary>
    /// </summary>
    internal enum TRAMP
    {
        // Trampoline subtype
        /// <summary>
        /// </summary>
        trampIncremental, // incremental thunks
        /// <summary>
        /// </summary>
        trampBranchIsland, // Branch island thunks
    };

    /// <summary>
    /// </summary>
    internal struct TrampolineSym
    {
        // Trampoline thunk symbol
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_TRAMPOLINE

        /// <summary>
        /// </summary>
        internal ushort cbThunk; // size of the thunk

        /// <summary>
        /// </summary>
        internal uint offTarget; // offset of the target of the thunk

        /// <summary>
        /// </summary>
        internal uint offThunk; // offset of the thunk

        /// <summary>
        /// </summary>
        internal ushort sectTarget; // section index of the target of the thunk

        /// <summary>
        /// </summary>
        internal ushort sectThunk; // section index of the thunk

        /// <summary>
        /// </summary>
        internal ushort trampType; // trampoline sym subtype
    };

    /// <summary>
    /// </summary>
    internal struct LabelSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_LABEL32

        /// <summary>
        /// </summary>
        internal byte flags; // (CV_PROCFLAGS) flags

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal ushort seg;
    };

    /// <summary>
    /// </summary>
    internal struct BlockSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_BLOCK32

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this blocks end

        /// <summary>
        /// </summary>
        internal uint len; // Block length

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint off; // Offset in code segment

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal ushort seg; // segment of label
    };

    /// <summary>
    /// </summary>
    internal struct WithSym32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_WITH32

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this blocks end

        /// <summary>
        /// </summary>
        internal string expr; // Length-prefixed expression string

        /// <summary>
        /// </summary>
        internal uint len; // Block length

        /// <summary>
        /// </summary>
        internal uint off; // Offset in code segment

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal ushort seg; // segment of label
    };

    /// <summary>
    /// </summary>
    internal struct VpathSym32
    {
        // internal ushort reclen;    // record length
        // internal ushort rectyp;    // S_VFTABLE32

        /// <summary>
        /// </summary>
        internal uint off; // offset of virtual function table

        /// <summary>
        /// </summary>
        internal uint path; // (type index) type index of the path record

        /// <summary>
        /// </summary>
        internal uint root; // (type index) type index of the root of path

        /// <summary>
        /// </summary>
        internal ushort seg; // segment of virtual function table
    };

    /// <summary>
    /// </summary>
    internal struct RegRel32
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_REGREL32
        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint off; // offset of symbol

        /// <summary>
        /// </summary>
        internal ushort reg; // register index for symbol

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or metadata token
    };

    /// <summary>
    /// </summary>
    internal struct AttrRegRel
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_MANREGREL | S_ATTR_REGREL
        /// <summary>
        /// </summary>
        internal ushort flags; // (CV_LVARFLAGS)local var flags

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint off; // offset of symbol

        /// <summary>
        /// </summary>
        internal uint offCod; // first code address where var is live

        /// <summary>
        /// </summary>
        internal ushort reg; // register index for symbol

        /// <summary>
        /// </summary>
        internal ushort segCod;

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index or metadata token
    };

    /// <summary>
    /// </summary>
    internal struct ThreadSym32
    {
        // internal ushort reclen;    // record length
        // internal ushort rectyp;    // S_LTHREAD32 | S_GTHREAD32

        /// <summary>
        /// </summary>
        internal string name; // length prefixed name

        /// <summary>
        /// </summary>
        internal uint off; // offset into thread storage

        /// <summary>
        /// </summary>
        internal ushort seg; // segment of thread storage

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) type index
    };

    /// <summary>
    /// </summary>
    internal struct Slink32
    {
        // internal ushort reclen;    // record length
        // internal ushort rectyp;    // S_SLINK32
        /// <summary>
        /// </summary>
        internal uint framesize; // frame size of parent procedure

        /// <summary>
        /// </summary>
        internal int off; // signed offset where the static link was saved relative to the value of reg

        /// <summary>
        /// </summary>
        internal ushort reg;
    };

    /// <summary>
    /// </summary>
    internal struct ProcSymMips
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_GPROCMIPS or S_LPROCMIPS

        /// <summary>
        /// </summary>
        internal uint dbgEnd; // Debug end offset

        /// <summary>
        /// </summary>
        internal uint dbgStart; // Debug start offset

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this blocks end

        /// <summary>
        /// </summary>
        internal uint fpOff; // fp register save offset

        /// <summary>
        /// </summary>
        internal uint fpSave; // fp register save mask

        /// <summary>
        /// </summary>
        internal byte frameReg; // Frame pointer register

        /// <summary>
        /// </summary>
        internal uint intOff; // int register save offset

        /// <summary>
        /// </summary>
        internal uint len; // Proc length

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint next; // pointer to next symbol

        /// <summary>
        /// </summary>
        internal uint off; // Symbol offset

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal uint regSave; // int register save mask

        /// <summary>
        /// </summary>
        internal byte retReg; // Register return value is in

        /// <summary>
        /// </summary>
        internal ushort seg; // Symbol segment

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index
    };

    /// <summary>
    /// </summary>
    internal struct ProcSymIa64
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_GPROCIA64 or S_LPROCIA64

        /// <summary>
        /// </summary>
        internal uint dbgEnd; // Debug end offset

        /// <summary>
        /// </summary>
        internal uint dbgStart; // Debug start offset

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this blocks end

        /// <summary>
        /// </summary>
        internal byte flags; // (CV_PROCFLAGS) Proc flags

        /// <summary>
        /// </summary>
        internal uint len; // Proc length

        /// <summary>
        /// </summary>
        internal string name; // Length-prefixed name

        /// <summary>
        /// </summary>
        internal uint next; // pointer to next symbol

        /// <summary>
        /// </summary>
        internal uint off; // Symbol offset

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal ushort retReg; // Register return value is in

        /// <summary>
        /// </summary>
        internal ushort seg; // Symbol segment

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index
    };

    /// <summary>
    /// </summary>
    internal struct RefSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_PROCREF_ST, S_DATAREF_ST, or S_LPROCREF_ST

        /// <summary>
        /// </summary>
        internal uint ibSym; // Offset of actual symbol in $$Symbols

        /// <summary>
        /// </summary>
        internal ushort imod; // Module containing the actual symbol

        /// <summary>
        /// </summary>
        internal uint sumName; // SUC of the name

        /// <summary>
        /// </summary>
        internal ushort usFill; // align this record
    };

    /// <summary>
    /// </summary>
    internal struct RefSym2
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_PROCREF, S_DATAREF, or S_LPROCREF

        /// <summary>
        /// </summary>
        internal uint ibSym; // Offset of actual symbol in $$Symbols

        /// <summary>
        /// </summary>
        internal ushort imod; // Module containing the actual symbol

        /// <summary>
        /// </summary>
        internal string name; // hidden name made a first class member

        /// <summary>
        /// </summary>
        internal uint sumName; // SUC of the name
    };

    /// <summary>
    /// </summary>
    internal struct AlignSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_ALIGN
    };

    /// <summary>
    /// </summary>
    internal struct OemSymbol
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_OEM
        /// <summary>
        /// </summary>
        internal Guid idOem; // an oem ID (GUID)

        /// <summary>
        /// </summary>
        internal byte[] rgl; // user data, force 4-byte alignment

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) Type index
    };

    /// <summary>
    /// </summary>
    [Flags]
    internal enum FRAMEPROCSYM_FLAGS : uint
    {
        /// <summary>
        /// </summary>
        fHasAlloca = 0x00000001, // function uses _alloca()
        /// <summary>
        /// </summary>
        fHasSetJmp = 0x00000002, // function uses setjmp()
        /// <summary>
        /// </summary>
        fHasLongJmp = 0x00000004, // function uses longjmp()
        /// <summary>
        /// </summary>
        fHasInlAsm = 0x00000008, // function uses inline asm
        /// <summary>
        /// </summary>
        fHasEH = 0x00000010, // function has EH states
        /// <summary>
        /// </summary>
        fInlSpec = 0x00000020, // function was speced as inline
        /// <summary>
        /// </summary>
        fHasSEH = 0x00000040, // function has SEH
        /// <summary>
        /// </summary>
        fNaked = 0x00000080, // function is __declspec(naked)
        /// <summary>
        /// </summary>
        fSecurityChecks = 0x00000100, // function has buffer security check introduced by /GS.
        /// <summary>
        /// </summary>
        fAsyncEH = 0x00000200, // function compiled with /EHa
        /// <summary>
        /// </summary>
        fGSNoStackOrdering = 0x00000400, // function has /GS buffer checks, but stack ordering couldn't be done
        /// <summary>
        /// </summary>
        fWasInlined = 0x00000800, // function was inlined within another function
    };

    /// <summary>
    /// </summary>
    internal struct FrameProcSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_FRAMEPROC
        /// <summary>
        /// </summary>
        internal uint cbFrame; // count of bytes of total frame of procedure

        /// <summary>
        /// </summary>
        internal uint cbPad; // count of bytes of padding in the frame

        /// <summary>
        /// </summary>
        internal uint cbSaveRegs; // count of bytes of callee save registers

        /// <summary>
        /// </summary>
        internal uint flags; // (FRAMEPROCSYM_FLAGS)

        /// <summary>
        /// </summary>
        internal uint offExHdlr; // offset of exception handler

        /// <summary>
        /// </summary>
        internal uint offPad; // offset (rel to frame) to where padding starts

        /// <summary>
        /// </summary>
        internal ushort secExHdlr; // section id of exception handler
    }

    /// <summary>
    /// </summary>
    internal struct UnamespaceSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_UNAMESPACE
        /// <summary>
        /// </summary>
        internal string name; // name
    };

    /// <summary>
    /// </summary>
    internal struct SepCodSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_SEPCODE

        /// <summary>
        /// </summary>
        internal uint end; // pointer to this block's end

        /// <summary>
        /// </summary>
        internal uint length; // count of bytes of this block

        /// <summary>
        /// </summary>
        internal uint off; // sec:off of the separated code

        /// <summary>
        /// </summary>
        internal uint offParent; // secParent:offParent of the enclosing scope

        /// <summary>
        /// </summary>
        internal uint parent; // pointer to the parent

        /// <summary>
        /// </summary>
        internal uint scf; // (CV_SEPCODEFLAGS) flags

        /// <summary>
        /// </summary>
        internal ushort sec; // (proc, block, or sepcode)

        /// <summary>
        /// </summary>
        internal ushort secParent;
    };

    /// <summary>
    /// </summary>
    internal struct LocalSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_LOCAL

        /// <summary>
        /// </summary>
        internal uint expr; // NI of expression that this temp holds

        /// <summary>
        /// </summary>
        internal ushort flags; // (CV_LVARFLAGS) local var flags

        /// <summary>
        /// </summary>
        internal uint id; // id of the local

        /// <summary>
        /// </summary>
        internal uint idParent; // This is is parent variable - fIsAggregated or fIsAlias

        /// <summary>
        /// </summary>
        internal string name; // Name of this symbol.

        /// <summary>
        /// </summary>
        internal uint offParent; // Offset in parent variable - fIsAggregated

        /// <summary>
        /// </summary>
        internal uint pad0; // pad, must be zero

        /// <summary>
        /// </summary>
        internal uint pad1; // pad, must be zero

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) type index
    }

    /// <summary>
    /// </summary>
    internal struct DefRangeSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_DEFRANGE

        /// <summary>
        /// </summary>
        internal uint id; // ID of the local symbol for which this formula holds

        /// <summary>
        /// </summary>
        internal uint program; // program to evaluate the value of the symbol

        /// <summary>
        /// </summary>
        internal CV_lvar_addr_range range; // Range of addresses where this program is valid
    };

    /// <summary>
    /// </summary>
    internal struct DefRangeSym2
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_DEFRANGE2

        /// <summary>
        /// </summary>
        internal ushort count; // count of CV_lvar_addr_range records following

        /// <summary>
        /// </summary>
        internal uint id; // ID of the local symbol for which this formula holds

        /// <summary>
        /// </summary>
        internal uint program; // program to evaluate the value of the symbol

        /// <summary>
        /// </summary>
        internal CV_lvar_addr_range[] range; // Range of addresses where this program is valid
    };

    /// <summary>
    /// </summary>
    internal struct SectionSym
    {
        // internal ushort reclen     // Record length
        // internal ushort rectyp;    // S_SECTION

        /// <summary>
        /// </summary>
        internal byte align; // Alignment of this section (power of 2)

        /// <summary>
        /// </summary>
        internal byte bReserved; // Reserved.  Must be zero.

        /// <summary>
        /// </summary>
        internal uint cb;

        /// <summary>
        /// </summary>
        internal uint characteristics;

        /// <summary>
        /// </summary>
        internal ushort isec; // Section number

        /// <summary>
        /// </summary>
        internal string name; // name

        /// <summary>
        /// </summary>
        internal uint rva;
    };

    /// <summary>
    /// </summary>
    internal struct CoffGroupSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_COFFGROUP

        /// <summary>
        /// </summary>
        internal uint cb;

        /// <summary>
        /// </summary>
        internal uint characteristics;

        /// <summary>
        /// </summary>
        internal string name; // name

        /// <summary>
        /// </summary>
        internal uint off; // Symbol offset

        /// <summary>
        /// </summary>
        internal ushort seg; // Symbol segment
    };

    /// <summary>
    /// </summary>
    [Flags]
    internal enum EXPORTSYM_FLAGS : ushort
    {
        /// <summary>
        /// </summary>
        fConstant = 0x0001, // CONSTANT
        /// <summary>
        /// </summary>
        fData = 0x0002, // DATA
        /// <summary>
        /// </summary>
        fPrivate = 0x0004, // PRIVATE
        /// <summary>
        /// </summary>
        fNoName = 0x0008, // NONAME
        /// <summary>
        /// </summary>
        fOrdinal = 0x0010, // Ordinal was explicitly assigned
        /// <summary>
        /// </summary>
        fForwarder = 0x0020, // This is a forwarder
    }

    /// <summary>
    /// </summary>
    internal struct ExportSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_EXPORT

        /// <summary>
        /// </summary>
        internal ushort flags; // (EXPORTSYM_FLAGS)

        /// <summary>
        /// </summary>
        internal string name; // name of

        /// <summary>
        /// </summary>
        internal ushort ordinal;
    };

    // Symbol for describing indirect calls when they are using
    // a function pointer cast on some other type or temporary.
    // Typical content will be an LF_POINTER to an LF_PROCEDURE
    // type record that should mimic an actual variable with the
    // function pointer type in question.
    // Since the compiler can sometimes tail-merge a function call
    // through a function pointer, there may be more than one
    // S_CALLSITEINFO record at an address.  This is similar to what
    // you could do in your own code by:
    // if (expr)
    // pfn = &function1;
    // else
    // pfn = &function2;
    // (*pfn)(arg list);

    /// <summary>
    /// </summary>
    internal struct CallsiteInfo
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_CALLSITEINFO

        /// <summary>
        /// </summary>
        internal ushort ect; // section index of call site

        /// <summary>
        /// </summary>
        internal int off; // offset of call site

        /// <summary>
        /// </summary>
        internal ushort pad0; // alignment padding field, must be zero

        /// <summary>
        /// </summary>
        internal uint typind; // (type index) type index describing function signature
    };

    // Frame cookie information

    /// <summary>
    /// </summary>
    internal enum CV_cookietype
    {
        /// <summary>
        /// </summary>
        CV_COOKIETYPE_COPY = 0, 

        /// <summary>
        /// </summary>
        CV_COOKIETYPE_XOR_SP, 

        /// <summary>
        /// </summary>
        CV_COOKIETYPE_XOR_BP, 

        /// <summary>
        /// </summary>
        CV_COOKIETYPE_XOR_R13, 
    };

    // Symbol for describing security cookie's position and type
    // (raw, xor'd with esp, xor'd with ebp).

    /// <summary>
    /// </summary>
    internal struct FrameCookie
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_FRAMECOOKIE

        /// <summary>
        /// </summary>
        internal int cookietype; // (CV_cookietype) Type of the cookie

        /// <summary>
        /// </summary>
        internal byte flags; // Flags describing this cookie

        /// <summary>
        /// </summary>
        internal int off; // Frame relative offset

        /// <summary>
        /// </summary>
        internal ushort reg; // Register index
    };

    /// <summary>
    /// </summary>
    internal enum CV_DISCARDED : uint
    {
        /// <summary>
        /// </summary>
        CV_DISCARDED_UNKNOWN = 0, 

        /// <summary>
        /// </summary>
        CV_DISCARDED_NOT_SELECTED = 1, 

        /// <summary>
        /// </summary>
        CV_DISCARDED_NOT_REFERENCED = 2, 
    };

    /// <summary>
    /// </summary>
    internal struct DiscardedSym
    {
        // internal ushort reclen;    // Record length [SYMTYPE]
        // internal ushort rectyp;    // S_DISCARDED

        /// <summary>
        /// </summary>
        internal byte[] data; // Original record(s) with invalid indices

        /// <summary>
        /// </summary>
        internal uint fileid; // First FILEID if line number info present

        /// <summary>
        /// </summary>
        internal CV_DISCARDED iscarded;

        /// <summary>
        /// </summary>
        internal uint linenum; // First line number
    };

    // V7 line number data types

    /// <summary>
    /// </summary>
    internal enum DEBUG_S_SUBSECTION_TYPE : uint
    {
        /// <summary>
        /// </summary>
        DEBUG_S_IGNORE = 0x80000000, // if this bit is set in a subsection type then ignore the subsection contents

        /// <summary>
        /// </summary>
        DEBUG_S_SYMBOLS = 0xf1, 

        /// <summary>
        /// </summary>
        DEBUG_S_LINES = 0xf2, 

        /// <summary>
        /// </summary>
        DEBUG_S_STRINGTABLE = 0xf3, 

        /// <summary>
        /// </summary>
        DEBUG_S_FILECHKSMS = 0xf4, 

        /// <summary>
        /// </summary>
        DEBUG_S_FRAMEDATA = 0xf5, 
    };

    // Line flags (data present)
    /// <summary>
    /// </summary>
    internal enum CV_LINE_SUBSECTION_FLAGS : ushort
    {
        /// <summary>
        /// </summary>
        CV_LINES_HAVE_COLUMNS = 0x0001, 
    }

    /// <summary>
    /// </summary>
    internal struct CV_LineSection
    {
        /// <summary>
        /// </summary>
        internal uint cod;

        /// <summary>
        /// </summary>
        internal ushort flags;

        /// <summary>
        /// </summary>
        internal uint off;

        /// <summary>
        /// </summary>
        internal ushort sec;
    }

    /// <summary>
    /// </summary>
    internal struct CV_SourceFile
    {
        /// <summary>
        /// </summary>
        internal uint count; // Number of CV_Line records.

        /// <summary>
        /// </summary>
        internal uint index; // Index to file in checksum section.

        /// <summary>
        /// </summary>
        internal uint linsiz; // Size of CV_Line recods.
    }

    /// <summary>
    /// </summary>
    [Flags]
    internal enum CV_Line_Flags : uint
    {
        /// <summary>
        /// </summary>
        linenumStart = 0x00ffffff, // line where statement/expression starts
        /// <summary>
        /// </summary>
        deltaLineEnd = 0x7f000000, // delta to line where statement ends (optional)
        /// <summary>
        /// </summary>
        fStatement = 0x80000000, // true if a statement linenumber, else an expression line num
    };

    /// <summary>
    /// </summary>
    internal struct CV_Line
    {
        /// <summary>
        /// </summary>
        internal uint flags; // (CV_Line_Flags)

        /// <summary>
        /// </summary>
        internal uint offset; // Offset to start of code bytes for line number
    };

    /// <summary>
    /// </summary>
    internal struct CV_Column
    {
        /// <summary>
        /// </summary>
        internal ushort offColumnEnd;

        /// <summary>
        /// </summary>
        internal ushort offColumnStart;
    };

    // File information

    /// <summary>
    /// </summary>
    internal enum CV_FILE_CHECKSUM_TYPE : byte
    {
        /// <summary>
        /// </summary>
        None = 0, 

        /// <summary>
        /// </summary>
        MD5 = 1, 
    };

    /// <summary>
    /// </summary>
    internal struct CV_FileCheckSum
    {
        /// <summary>
        /// </summary>
        internal byte len; // Hash length

        /// <summary>
        /// </summary>
        internal uint name; // Index of name in name table.

        /// <summary>
        /// </summary>
        internal byte type; // Hash type
    }

    /// <summary>
    /// </summary>
    [Flags]
    internal enum FRAMEDATA_FLAGS : uint
    {
        /// <summary>
        /// </summary>
        fHasSEH = 0x00000001, 

        /// <summary>
        /// </summary>
        fHasEH = 0x00000002, 

        /// <summary>
        /// </summary>
        fIsFunctionStart = 0x00000004, 
    };

    /// <summary>
    /// </summary>
    internal struct FrameData
    {
        /// <summary>
        /// </summary>
        internal uint cbBlock;

        /// <summary>
        /// </summary>
        internal uint cbLocals;

        /// <summary>
        /// </summary>
        internal uint cbParams;

        /// <summary>
        /// </summary>
        internal ushort cbProlog;

        /// <summary>
        /// </summary>
        internal ushort cbSavedRegs;

        /// <summary>
        /// </summary>
        internal uint cbStkMax;

        /// <summary>
        /// </summary>
        internal uint flags; // (FRAMEDATA_FLAGS)

        /// <summary>
        /// </summary>
        internal uint frameFunc;

        /// <summary>
        /// </summary>
        internal uint ulRvaStart;
    };

    /// <summary>
    /// </summary>
    internal struct XFixupData
    {
        /// <summary>
        /// </summary>
        internal uint rva;

        /// <summary>
        /// </summary>
        internal uint rvaTarget;

        /// <summary>
        /// </summary>
        internal ushort wExtra;

        /// <summary>
        /// </summary>
        internal ushort wType;
    };

    /// <summary>
    /// </summary>
    internal enum DEBUG_S_SUBSECTION
    {
        /// <summary>
        /// </summary>
        SYMBOLS = 0xF1, 

        /// <summary>
        /// </summary>
        LINES = 0xF2, 

        /// <summary>
        /// </summary>
        STRINGTABLE = 0xF3, 

        /// <summary>
        /// </summary>
        FILECHKSMS = 0xF4, 

        /// <summary>
        /// </summary>
        FRAMEDATA = 0xF5, 
    }
}
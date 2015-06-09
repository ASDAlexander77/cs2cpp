// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

// 

using System;
using System.Reflection;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace System.Reflection
{
    [Serializable]
    internal enum CorElementType : byte 
    {
        End                        = 0x00,
        Void                       = 0x01,
        Boolean                    = 0x02,
        Char                       = 0x03,
        I1                         = 0x04,
        U1                         = 0x05,
        I2                         = 0x06,
        U2                         = 0x07,
        I4                         = 0x08,
        U4                         = 0x09,
        I8                         = 0x0A,
        U8                         = 0x0B,
        R4                         = 0x0C,
        R8                         = 0x0D,
        String                     = 0x0E,
        Ptr                        = 0x0F,
        ByRef                      = 0x10,
        ValueType                  = 0x11,
        Class                      = 0x12,
        Var                        = 0x13,
        Array                      = 0x14,
        GenericInst                = 0x15,
        TypedByRef                 = 0x16,
        I                          = 0x18,
        U                          = 0x19,
        FnPtr                      = 0x1B,
        Object                     = 0x1C,
        SzArray                    = 0x1D,
        MVar                       = 0x1E,
        CModReqd                   = 0x1F,
        CModOpt                    = 0x20,
        Internal                   = 0x21,
        Max                        = 0x22,
        Modifier                   = 0x40,
        Sentinel                   = 0x41,
        Pinned                     = 0x45,
    }

[Serializable]
[Flags()]
    internal enum MdSigCallingConvention: byte
    {
        CallConvMask    = 0x0f,  // Calling convention is bottom 4 bits 

        Default         = 0x00,  
        C               = 0x01,
        StdCall         = 0x02,
        ThisCall        = 0x03,
        FastCall        = 0x04,
        Vararg          = 0x05,  
        Field           = 0x06,  
        LocalSig        = 0x07,
        Property        = 0x08,
        Unmgd           = 0x09,
        GenericInst     = 0x0a,  // generic method instantiation
        
        Generic         = 0x10,  // Generic method sig with explicit number of type arguments (precedes ordinary parameter count)
        HasThis         = 0x20,  // Top bit indicates a 'this' parameter    
        ExplicitThis    = 0x40,  // This parameter is explicitly in the signature
    }


[Serializable]
[Flags()]
    internal enum PInvokeAttributes
    { 
        NoMangle          = 0x0001,


        CharSetMask       = 0x0006,
        CharSetNotSpec    = 0x0000,
        CharSetAnsi       = 0x0002, 
        CharSetUnicode    = 0x0004,
        CharSetAuto       = 0x0006,
        

        BestFitUseAssem   = 0x0000,
        BestFitEnabled    = 0x0010,
        BestFitDisabled   = 0x0020,
        BestFitMask       = 0x0030,
        
        ThrowOnUnmappableCharUseAssem   = 0x0000,
        ThrowOnUnmappableCharEnabled    = 0x1000,
        ThrowOnUnmappableCharDisabled   = 0x2000,
        ThrowOnUnmappableCharMask       = 0x3000,

        SupportsLastError = 0x0040,   

        CallConvMask      = 0x0700,
        CallConvWinapi    = 0x0100,   
        CallConvCdecl     = 0x0200,
        CallConvStdcall   = 0x0300,
        CallConvThiscall  = 0x0400,   
        CallConvFastcall  = 0x0500,

        MaxValue          = 0xFFFF,
    }


[Serializable]
[Flags()]
    internal enum MethodSemanticsAttributes
    {
        Setter          = 0x0001,
        Getter          = 0x0002,
        Other           = 0x0004,
        AddOn           = 0x0008,
        RemoveOn        = 0x0010,
        Fire            = 0x0020,  
    }


    [Serializable]
    internal enum MetadataTokenType
    {
        Module               = 0x00000000,       
        TypeRef              = 0x01000000,                 
        TypeDef              = 0x02000000,       
        FieldDef             = 0x04000000,       
        MethodDef            = 0x06000000,       
        ParamDef             = 0x08000000,       
        InterfaceImpl        = 0x09000000,       
        MemberRef            = 0x0a000000,       
        CustomAttribute      = 0x0c000000,       
        Permission           = 0x0e000000,       
        Signature            = 0x11000000,       
        Event                = 0x14000000,       
        Property             = 0x17000000,       
        ModuleRef            = 0x1a000000,       
        TypeSpec             = 0x1b000000,       
        Assembly             = 0x20000000,       
        AssemblyRef          = 0x23000000,       
        File                 = 0x26000000,       
        ExportedType         = 0x27000000,       
        ManifestResource     = 0x28000000,       
        GenericPar           = 0x2a000000,       
        MethodSpec           = 0x2b000000,       
        String               = 0x70000000,       
        Name                 = 0x71000000,       
        BaseType             = 0x72000000, 
        Invalid              = 0x7FFFFFFF, 
    }

    [Serializable]
    internal struct ConstArray
    {
        public IntPtr Signature { get { return m_constArray; } }
        public int Length { get { return m_length; } }
        public byte this[int index]
        {
            get
            {
                if (index < 0 || index >= m_length)
                    throw new IndexOutOfRangeException();

                unsafe 
                {
                    return ((byte*)m_constArray.ToPointer())[index];
                }
            }
        }

        // Keep the definition in sync with vm\ManagedMdImport.hpp
        internal int m_length;
        internal IntPtr m_constArray;
    }
    
    [Serializable]
    internal struct MetadataToken
    {
        #region Implicit Cast Operators
        public static implicit operator int(MetadataToken token) { return token.Value; }
        public static implicit operator MetadataToken(int token) { return new MetadataToken(token); }
        #endregion

        #region Public Static Members
        public static bool IsTokenOfType(int token, params MetadataTokenType[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if ((int)(token & 0xFF000000) == (int)types[i])
                    return true;
            }

            return false;
        }

        public static bool IsNullToken(int token) 
        { 
            return (token & 0x00FFFFFF) == 0; 
        }
        #endregion

        #region Public Data Members
        public int Value;
        #endregion

        #region Constructor
        public MetadataToken(int token) { Value = token; } 
        #endregion
        
        #region Public Members
        public bool IsGlobalTypeDefToken {  get { return (Value == 0x02000001); } }
        public MetadataTokenType TokenType { get { return (MetadataTokenType)(Value & 0xFF000000); } }
        public bool IsTypeRef { get { return TokenType == MetadataTokenType.TypeRef; } }
        public bool IsTypeDef { get { return TokenType == MetadataTokenType.TypeDef; } }
        public bool IsFieldDef { get { return TokenType == MetadataTokenType.FieldDef; } }
        public bool IsMethodDef { get { return TokenType == MetadataTokenType.MethodDef; } }
        public bool IsMemberRef { get { return TokenType == MetadataTokenType.MemberRef; } }
        public bool IsEvent { get { return TokenType == MetadataTokenType.Event; } }
        public bool IsProperty { get { return TokenType == MetadataTokenType.Property; } }
        public bool IsParamDef { get { return TokenType == MetadataTokenType.ParamDef; } }
        public bool IsTypeSpec { get { return TokenType == MetadataTokenType.TypeSpec; } }
        public bool IsMethodSpec { get { return TokenType == MetadataTokenType.MethodSpec; } }
        public bool IsString { get { return TokenType == MetadataTokenType.String; } }
        public bool IsSignature { get { return TokenType == MetadataTokenType.Signature; } }
        public bool IsModule { get { return TokenType == MetadataTokenType.Module; } }
        public bool IsAssembly { get { return TokenType == MetadataTokenType.Assembly; } }
        public bool IsGenericPar { get { return TokenType == MetadataTokenType.GenericPar; } }
        #endregion

        #region Object Overrides
        public override string ToString() { return String.Format(CultureInfo.InvariantCulture, "0x{0:x8}", Value); }
        #endregion
    }

    internal unsafe struct MetadataEnumResult
    {
        // Keep the definition in sync with vm\ManagedMdImport.hpp
        private int[] largeResult;
        private int length;
        private fixed int smallResult[16];

        public int Length
        {
            get
            {
                return length;
            }
        }

        public int this[int index]
        {
            get
            {
                if (largeResult != null)
                    return largeResult[index];

                fixed (int* p = smallResult)
                    return p[index];
            }
        }
    }
}



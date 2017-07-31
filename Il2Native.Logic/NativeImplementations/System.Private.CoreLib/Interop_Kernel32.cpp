#include "System.Private.CoreLib.h"

namespace CoreLib { 
    namespace _ = ::CoreLib;
    // Method : Interop.Kernel32.CancelIoEx(System.Runtime.InteropServices.SafeHandle, System.Threading.NativeOverlapped*)
    bool Interop_Kernel32::CancelIoEx(::CoreLib::System::Runtime::InteropServices::SafeHandle* handle, ::CoreLib::System::Threading::NativeOverlapped* lpOverlapped)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.CloseHandle(System.IntPtr)
    bool Interop_Kernel32::CloseHandle(::CoreLib::System::IntPtr handle)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.CreateFilePrivate(string, int, System.IO.FileShare, ref Interop.Kernel32.SECURITY_ATTRIBUTES, System.IO.FileMode, int, System.IntPtr)
    ::CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* Interop_Kernel32::CreateFilePrivate_Ref(string* lpFileName, int32_t dwDesiredAccess, ::CoreLib::System::IO::FileShare__enum dwShareMode, _::Interop_Kernel32_SECURITY_ATTRIBUTES& securityAttrs, ::CoreLib::System::IO::FileMode__enum dwCreationDisposition, int32_t dwFlagsAndAttributes, ::CoreLib::System::IntPtr hTemplateFile)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FindClose(System.IntPtr)
    bool Interop_Kernel32::FindClose(::CoreLib::System::IntPtr hFindFile)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FindFirstFileExPrivate(string, Interop.Kernel32.FINDEX_INFO_LEVELS, ref Interop.Kernel32.WIN32_FIND_DATA, Interop.Kernel32.FINDEX_SEARCH_OPS, System.IntPtr, int)
    ::CoreLib::Microsoft::Win32::SafeHandles::SafeFindHandle* Interop_Kernel32::FindFirstFileExPrivate_Ref(string* lpFileName, _::Interop_Kernel32_FINDEX_INFO_LEVELS__enum fInfoLevelId, _::Interop_Kernel32_WIN32_FIND_DATA& lpFindFileData, _::Interop_Kernel32_FINDEX_SEARCH_OPS__enum fSearchOp, ::CoreLib::System::IntPtr lpSearchFilter, int32_t dwAdditionalFlags)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FlushFileBuffers(System.Runtime.InteropServices.SafeHandle)
    bool Interop_Kernel32::FlushFileBuffers(::CoreLib::System::Runtime::InteropServices::SafeHandle* hHandle)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FormatMessage(int, System.IntPtr, uint, int, System.Text.StringBuilder, int, System.IntPtr[])
    int32_t Interop_Kernel32::FormatMessage(int32_t dwFlags, ::CoreLib::System::IntPtr lpSource, uint32_t dwMessageId, int32_t dwLanguageId, ::CoreLib::System::Text::StringBuilder* lpBuffer, int32_t nSize, __array<::CoreLib::System::IntPtr>* arguments)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FreeEnvironmentStrings(char*)
    bool Interop_Kernel32::FreeEnvironmentStrings(char16_t* lpszEnvironmentBlock)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetCPInfo(uint, Interop.Kernel32.CPINFO*)
    int32_t Interop_Kernel32::GetCPInfo(uint32_t codePage, _::Interop_Kernel32_CPINFO* lpCpInfo)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetEnvironmentStrings()
    char16_t* Interop_Kernel32::GetEnvironmentStrings()
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetFileAttributesExPrivate(string, Interop.Kernel32.GET_FILEEX_INFO_LEVELS, ref Interop.Kernel32.WIN32_FILE_ATTRIBUTE_DATA)
    bool Interop_Kernel32::GetFileAttributesExPrivate_Ref(string* name, _::Interop_Kernel32_GET_FILEEX_INFO_LEVELS__enum fileInfoLevel, _::Interop_Kernel32_WIN32_FILE_ATTRIBUTE_DATA& lpFileInformation)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetFileInformationByHandleEx(Microsoft.Win32.SafeHandles.SafeFileHandle, Interop.Kernel32.FILE_INFO_BY_HANDLE_CLASS, out Interop.Kernel32.FILE_STANDARD_INFO, uint)
    bool Interop_Kernel32::GetFileInformationByHandleEx_Out(::CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* hFile, _::Interop_Kernel32_FILE_INFO_BY_HANDLE_CLASS__enum FileInformationClass, _::Interop_Kernel32_FILE_STANDARD_INFO& lpFileInformation, uint32_t dwBufferSize)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetFileType(System.Runtime.InteropServices.SafeHandle)
    int32_t Interop_Kernel32::GetFileType(::CoreLib::System::Runtime::InteropServices::SafeHandle* hFile)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetFullPathNameW(char*, uint, char[], System.IntPtr)
    uint32_t Interop_Kernel32::GetFullPathNameW(char16_t* path, uint32_t numBufferChars, __array<char16_t>* buffer, ::CoreLib::System::IntPtr mustBeZero)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetLongPathNameW(char[], char[], uint)
    uint32_t Interop_Kernel32::GetLongPathNameW(__array<char16_t>* lpszShortPath, __array<char16_t>* lpszLongPath, uint32_t cchBuffer)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetTempFileNameW(string, string, uint, System.Text.StringBuilder)
    uint32_t Interop_Kernel32::GetTempFileNameW(string* tmpPath, string* prefix, uint32_t uniqueIdOrZero, ::CoreLib::System::Text::StringBuilder* tmpFileName)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetTempPathW(int, System.Text.StringBuilder)
    uint32_t Interop_Kernel32::GetTempPathW(int32_t bufferLen, ::CoreLib::System::Text::StringBuilder* buffer)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.LCIDToLocaleName(int, char*, int, uint)
    int32_t Interop_Kernel32::LCIDToLocaleName(int32_t locale, char16_t* pLocaleName, int32_t cchName, uint32_t dwFlags)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.LocaleNameToLCID(string, uint)
    int32_t Interop_Kernel32::LocaleNameToLCID(string* lpName, uint32_t dwFlags)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.LCMapStringEx(string, uint, char*, int, void*, int, void*, void*, System.IntPtr)
    int32_t Interop_Kernel32::LCMapStringEx(string* lpLocaleName, uint32_t dwMapFlags, char16_t* lpSrcStr, int32_t cchSrc, void* lpDestStr, int32_t cchDest, void* lpVersionInformation, void* lpReserved, ::CoreLib::System::IntPtr sortHandle)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FindNLSStringEx(char*, uint, char*, int, char*, int, int*, void*, void*, System.IntPtr)
    int32_t Interop_Kernel32::FindNLSStringEx(char16_t* lpLocaleName, uint32_t dwFindNLSStringFlags, char16_t* lpStringSource, int32_t cchSource, char16_t* lpStringValue, int32_t cchValue, int32_t* pcchFound, void* lpVersionInformation, void* lpReserved, ::CoreLib::System::IntPtr sortHandle)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.CompareStringEx(char*, uint, char*, int, char*, int, void*, void*, System.IntPtr)
    int32_t Interop_Kernel32::CompareStringEx(char16_t* lpLocaleName, uint32_t dwCmpFlags, char16_t* lpString1, int32_t cchCount1, char16_t* lpString2, int32_t cchCount2, void* lpVersionInformation, void* lpReserved, ::CoreLib::System::IntPtr lParam)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.CompareStringOrdinal(char*, int, char*, int, bool)
    int32_t Interop_Kernel32::CompareStringOrdinal(char16_t* lpString1, int32_t cchCount1, char16_t* lpString2, int32_t cchCount2, bool bIgnoreCase)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FindStringOrdinal(uint, char*, int, char*, int, int)
    int32_t Interop_Kernel32::FindStringOrdinal(uint32_t dwFindStringOrdinalFlags, char16_t* lpStringSource, int32_t cchSource, char16_t* lpStringValue, int32_t cchValue, int32_t bIgnoreCase)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.IsNLSDefinedString(int, uint, System.IntPtr, char*, int)
    bool Interop_Kernel32::IsNLSDefinedString(int32_t Function, uint32_t dwFlags, ::CoreLib::System::IntPtr lpVersionInformation, char16_t* lpString, int32_t cchStr)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetUserPreferredUILanguages(uint, out uint, char[], ref uint)
    bool Interop_Kernel32::GetUserPreferredUILanguages_Out_Ref(uint32_t dwFlags, uint32_t& pulNumLanguages, __array<char16_t>* pwszLanguagesBuffer, uint32_t& pcchLanguagesBuffer)
    {
		return ::GetUserPreferredUILanguages(dwFlags, (PULONG) &pulNumLanguages, (PZZWSTR) &(pwszLanguagesBuffer->_data[0]), (PULONG) &pcchLanguagesBuffer);
    }
    
    // Method : Interop.Kernel32.GetLocaleInfoEx(string, uint, void*, int)
    int32_t Interop_Kernel32::GetLocaleInfoEx(string* lpLocaleName, uint32_t LCType, void* lpLCData, int32_t cchData)
    {
		return ::GetLocaleInfoEx(lpLocaleName != nullptr ? (wchar_t*)&(lpLocaleName->FIRST_CHAR_FIELD) : nullptr, LCType, (wchar_t*)lpLCData, cchData);
    }
    
    // Method : Interop.Kernel32.EnumSystemLocalesEx(Interop.Kernel32.EnumLocalesProcEx, uint, void*, System.IntPtr)
    bool Interop_Kernel32::EnumSystemLocalesEx(_::Interop_Kernel32_EnumLocalesProcEx* lpLocaleEnumProcEx, uint32_t dwFlags, void* lParam, ::CoreLib::System::IntPtr reserved)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.ResolveLocaleName(string, char*, int)
    int32_t Interop_Kernel32::ResolveLocaleName(string* lpNameToResolve, char16_t* lpLocaleName, int32_t cchLocaleName)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.EnumTimeFormatsEx(Interop.Kernel32.EnumTimeFormatsProcEx, string, uint, void*)
    bool Interop_Kernel32::EnumTimeFormatsEx(_::Interop_Kernel32_EnumTimeFormatsProcEx* lpTimeFmtEnumProcEx, string* lpLocaleName, uint32_t dwFlags, void* lParam)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetCalendarInfoEx(string, uint, System.IntPtr, uint, System.IntPtr, int, out int)
    int32_t Interop_Kernel32::GetCalendarInfoEx_Out(string* lpLocaleName, uint32_t Calendar, ::CoreLib::System::IntPtr lpReserved, uint32_t CalType, ::CoreLib::System::IntPtr lpCalData, int32_t cchData, int32_t& lpValue)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetCalendarInfoEx(string, uint, System.IntPtr, uint, System.IntPtr, int, System.IntPtr)
    int32_t Interop_Kernel32::GetCalendarInfoEx(string* lpLocaleName, uint32_t Calendar, ::CoreLib::System::IntPtr lpReserved, uint32_t CalType, ::CoreLib::System::IntPtr lpCalData, int32_t cchData, ::CoreLib::System::IntPtr lpValue)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.EnumCalendarInfoExEx(Interop.Kernel32.EnumCalendarInfoProcExEx, string, uint, string, uint, void*)
    bool Interop_Kernel32::EnumCalendarInfoExEx(_::Interop_Kernel32_EnumCalendarInfoProcExEx* pCalInfoEnumProcExEx, string* lpLocaleName, uint32_t Calendar, string* lpReserved, uint32_t CalType, void* lParam)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetNLSVersionEx(int, string, Interop.Kernel32.NlsVersionInfoEx*)
    bool Interop_Kernel32::GetNLSVersionEx(int32_t function, string* localeName, _::Interop_Kernel32_NlsVersionInfoEx* lpVersionInformation)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.LockFile(Microsoft.Win32.SafeHandles.SafeFileHandle, int, int, int, int)
    bool Interop_Kernel32::LockFile(::CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* handle, int32_t offsetLow, int32_t offsetHigh, int32_t countLow, int32_t countHigh)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.UnlockFile(Microsoft.Win32.SafeHandles.SafeFileHandle, int, int, int, int)
    bool Interop_Kernel32::UnlockFile(::CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* handle, int32_t offsetLow, int32_t offsetHigh, int32_t countLow, int32_t countHigh)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.OutputDebugString(string)
    void Interop_Kernel32::OutputDebugString(string* message)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.ReadFile(System.Runtime.InteropServices.SafeHandle, byte*, int, out int, System.IntPtr)
    int32_t Interop_Kernel32::ReadFile_Out(::CoreLib::System::Runtime::InteropServices::SafeHandle* handle, uint8_t* bytes, int32_t numBytesToRead, int32_t& numBytesRead, ::CoreLib::System::IntPtr mustBeZero)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.ReadFile(System.Runtime.InteropServices.SafeHandle, byte*, int, System.IntPtr, System.Threading.NativeOverlapped*)
    int32_t Interop_Kernel32::ReadFile(::CoreLib::System::Runtime::InteropServices::SafeHandle* handle, uint8_t* bytes, int32_t numBytesToRead, ::CoreLib::System::IntPtr numBytesRead_mustBeZero, ::CoreLib::System::Threading::NativeOverlapped* overlapped)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetEndOfFile(Microsoft.Win32.SafeHandles.SafeFileHandle)
    bool Interop_Kernel32::SetEndOfFile(::CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* hFile)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetEnvironmentVariable(string, string)
    bool Interop_Kernel32::SetEnvironmentVariable(string* lpName, string* lpValue)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetThreadErrorMode(uint, out uint)
    bool Interop_Kernel32::SetThreadErrorMode_Out(uint32_t dwNewMode, uint32_t& lpOldMode)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetFilePointerEx(Microsoft.Win32.SafeHandles.SafeFileHandle, long, out long, uint)
    bool Interop_Kernel32::SetFilePointerEx_Out(::CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* hFile, int64_t liDistanceToMove, int64_t& lpNewFilePointer, uint32_t dwMoveMethod)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.WideCharToMultiByte(uint, uint, char*, int, byte*, int, System.IntPtr, System.IntPtr)
    int32_t Interop_Kernel32::WideCharToMultiByte(uint32_t CodePage, uint32_t dwFlags, char16_t* lpWideCharStr, int32_t cchWideChar, uint8_t* lpMultiByteStr, int32_t cbMultiByte, ::CoreLib::System::IntPtr lpDefaultChar, ::CoreLib::System::IntPtr lpUsedDefaultChar)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.WriteFile(System.Runtime.InteropServices.SafeHandle, byte*, int, out int, System.IntPtr)
    int32_t Interop_Kernel32::WriteFile_Out(::CoreLib::System::Runtime::InteropServices::SafeHandle* handle, uint8_t* bytes, int32_t numBytesToWrite, int32_t& numBytesWritten, ::CoreLib::System::IntPtr mustBeZero)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.WriteFile(System.Runtime.InteropServices.SafeHandle, byte*, int, System.IntPtr, System.Threading.NativeOverlapped*)
    int32_t Interop_Kernel32::WriteFile(::CoreLib::System::Runtime::InteropServices::SafeHandle* handle, uint8_t* bytes, int32_t numBytesToWrite, ::CoreLib::System::IntPtr numBytesWritten_mustBeZero, ::CoreLib::System::Threading::NativeOverlapped* lpOverlapped)
    {
        throw 3221274624U;
    }

}

namespace CoreLib { 
    namespace _ = ::CoreLib;
}

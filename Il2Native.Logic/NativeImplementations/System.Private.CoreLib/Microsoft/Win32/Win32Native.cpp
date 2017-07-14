#include "System.Private.CoreLib.h"

namespace CoreLib { namespace Microsoft { namespace Win32 { 
    namespace _ = ::CoreLib::Microsoft::Win32;
    // Method : Microsoft.Win32.Win32Native.FormatMessage(int, System.IntPtr, int, int, System.Text.StringBuilder, int, System.IntPtr)
    int32_t Win32Native::FormatMessage(int32_t dwFlags, ::CoreLib::System::IntPtr lpSource, int32_t dwMessageId, int32_t dwLanguageId, ::CoreLib::System::Text::StringBuilder* lpBuffer, int32_t nSize, ::CoreLib::System::IntPtr va_list_arguments)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.LocalAlloc_NoSafeHandle(int, System.UIntPtr)
    ::CoreLib::System::IntPtr Win32Native::LocalAlloc_NoSafeHandle(int32_t uFlags, ::CoreLib::System::UIntPtr sizetdwBytes)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.LocalFree(System.IntPtr)
    ::CoreLib::System::IntPtr Win32Native::LocalFree(::CoreLib::System::IntPtr handle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.GlobalMemoryStatusExNative(ref Microsoft.Win32.Win32Native.MEMORYSTATUSEX)
    bool Win32Native::GlobalMemoryStatusExNative_Ref(_::Win32Native_MEMORYSTATUSEX& buffer)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.VirtualQuery(void*, ref Microsoft.Win32.Win32Native.MEMORY_BASIC_INFORMATION, System.UIntPtr)
    ::CoreLib::System::UIntPtr Win32Native::VirtualQuery_Ref(void* address, _::Win32Native_MEMORY_BASIC_INFORMATION& buffer, ::CoreLib::System::UIntPtr sizeOfBuffer)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.VirtualAlloc(void*, System.UIntPtr, int, int)
    void* Win32Native::VirtualAlloc(void* address, ::CoreLib::System::UIntPtr numBytes, int32_t commitOrReserve, int32_t pageProtectionMode)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.VirtualFree(void*, System.UIntPtr, int)
    bool Win32Native::VirtualFree(void* address, ::CoreLib::System::UIntPtr numBytes, int32_t pageFreeMode)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.lstrlenA(System.IntPtr)
    int32_t Win32Native::lstrlenA(::CoreLib::System::IntPtr ptr)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.lstrlenW(System.IntPtr)
    int32_t Win32Native::lstrlenW(::CoreLib::System::IntPtr ptr)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.SysAllocStringLen(string, int)
    ::CoreLib::System::IntPtr Win32Native::SysAllocStringLen(string* src, int32_t len)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.SysStringLen(System.IntPtr)
    uint32_t Win32Native::SysStringLen(::CoreLib::System::IntPtr bstr)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.SysFreeString(System.IntPtr)
    void Win32Native::SysFreeString(::CoreLib::System::IntPtr bstr)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.SysAllocStringByteLen(byte[], uint)
    ::CoreLib::System::IntPtr Win32Native::SysAllocStringByteLen(__array<uint8_t>* str, uint32_t len)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.SysStringByteLen(System.IntPtr)
    uint32_t Win32Native::SysStringByteLen(::CoreLib::System::IntPtr bstr)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.SetEvent(Microsoft.Win32.SafeHandles.SafeWaitHandle)
    bool Win32Native::SetEvent(_::SafeHandles::SafeWaitHandle* handle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.ResetEvent(Microsoft.Win32.SafeHandles.SafeWaitHandle)
    bool Win32Native::ResetEvent(_::SafeHandles::SafeWaitHandle* handle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.CreateEventEx(Microsoft.Win32.Win32Native.SECURITY_ATTRIBUTES, string, uint, uint)
    _::SafeHandles::SafeWaitHandle* Win32Native::CreateEventEx(_::Win32Native_SECURITY_ATTRIBUTES* lpSecurityAttributes, string* name, uint32_t flags, uint32_t desiredAccess)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.OpenEvent(uint, bool, string)
    _::SafeHandles::SafeWaitHandle* Win32Native::OpenEvent(uint32_t desiredAccess, bool inheritHandle, string* name)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.CreateMutexEx(Microsoft.Win32.Win32Native.SECURITY_ATTRIBUTES, string, uint, uint)
    _::SafeHandles::SafeWaitHandle* Win32Native::CreateMutexEx(_::Win32Native_SECURITY_ATTRIBUTES* lpSecurityAttributes, string* name, uint32_t flags, uint32_t desiredAccess)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.OpenMutex(uint, bool, string)
    _::SafeHandles::SafeWaitHandle* Win32Native::OpenMutex(uint32_t desiredAccess, bool inheritHandle, string* name)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.ReleaseMutex(Microsoft.Win32.SafeHandles.SafeWaitHandle)
    bool Win32Native::ReleaseMutex(_::SafeHandles::SafeWaitHandle* handle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.CloseHandle(System.IntPtr)
    bool Win32Native::CloseHandle(::CoreLib::System::IntPtr handle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.WriteFile(Microsoft.Win32.SafeHandles.SafeFileHandle, byte*, int, out int, System.IntPtr)
    int32_t Win32Native::WriteFile_Out(_::SafeHandles::SafeFileHandle* handle, uint8_t* bytes, int32_t numBytesToWrite, int32_t& numBytesWritten, ::CoreLib::System::IntPtr mustBeZero)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.CreateSemaphoreEx(Microsoft.Win32.Win32Native.SECURITY_ATTRIBUTES, int, int, string, uint, uint)
    _::SafeHandles::SafeWaitHandle* Win32Native::CreateSemaphoreEx(_::Win32Native_SECURITY_ATTRIBUTES* lpSecurityAttributes, int32_t initialCount, int32_t maximumCount, string* name, uint32_t flags, uint32_t desiredAccess)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.ReleaseSemaphore(Microsoft.Win32.SafeHandles.SafeWaitHandle, int, out int)
    bool Win32Native::ReleaseSemaphore_Out(_::SafeHandles::SafeWaitHandle* handle, int32_t releaseCount, int32_t& previousCount)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.OpenSemaphore(uint, bool, string)
    _::SafeHandles::SafeWaitHandle* Win32Native::OpenSemaphore(uint32_t desiredAccess, bool inheritHandle, string* name)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.GetSystemDirectory(System.Text.StringBuilder, int)
    int32_t Win32Native::GetSystemDirectory(::CoreLib::System::Text::StringBuilder* sb, int32_t length)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.GetStdHandle(int)
    ::CoreLib::System::IntPtr Win32Native::GetStdHandle(int32_t nStdHandle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.FindFirstFile(string, Microsoft.Win32.Win32Native.WIN32_FIND_DATA)
    _::SafeHandles::SafeFindHandle* Win32Native::FindFirstFile(string* fileName, _::Win32Native_WIN32_FIND_DATA* data)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.FindNextFile(Microsoft.Win32.SafeHandles.SafeFindHandle, Microsoft.Win32.Win32Native.WIN32_FIND_DATA)
    bool Win32Native::FindNextFile(_::SafeHandles::SafeFindHandle* hndFindFile, _::Win32Native_WIN32_FIND_DATA* lpFindFileData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.FindClose(System.IntPtr)
    bool Win32Native::FindClose(::CoreLib::System::IntPtr handle)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.GetFileAttributesEx(string, int, ref Microsoft.Win32.Win32Native.WIN32_FILE_ATTRIBUTE_DATA)
    bool Win32Native::GetFileAttributesEx_Ref(string* name, int32_t fileInfoLevel, _::Win32Native_WIN32_FILE_ATTRIBUTE_DATA& lpFileInformation)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.WideCharToMultiByte(uint, uint, char*, int, byte*, int, System.IntPtr, System.IntPtr)
    int32_t Win32Native::WideCharToMultiByte(uint32_t cp, uint32_t flags, char16_t* pwzSource, int32_t cchSource, uint8_t* pbDestBuffer, int32_t cbDestBuffer, ::CoreLib::System::IntPtr null1, ::CoreLib::System::IntPtr null2)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.SetEnvironmentVariable(string, string)
    bool Win32Native::SetEnvironmentVariable(string* lpName, string* lpValue)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.GetEnvironmentVariable(string, System.Text.StringBuilder, int)
    int32_t Win32Native::GetEnvironmentVariable(string* lpName, ::CoreLib::System::Text::StringBuilder* lpValue, int32_t size)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.GetEnvironmentStrings()
    char16_t* Win32Native::GetEnvironmentStrings()
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.FreeEnvironmentStrings(char*)
    bool Win32Native::FreeEnvironmentStrings(char16_t* pStrings)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.GetCurrentProcessId()
    uint32_t Win32Native::GetCurrentProcessId()
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.CoCreateGuid(out System.Guid)
    int32_t Win32Native::CoCreateGuid_Out(::CoreLib::System::Guid& guid)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.CoTaskMemAlloc(System.UIntPtr)
    ::CoreLib::System::IntPtr Win32Native::CoTaskMemAlloc(::CoreLib::System::UIntPtr cb)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.CoTaskMemFree(System.IntPtr)
    void Win32Native::CoTaskMemFree(::CoreLib::System::IntPtr ptr)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.CoTaskMemRealloc(System.IntPtr, System.UIntPtr)
    ::CoreLib::System::IntPtr Win32Native::CoTaskMemRealloc(::CoreLib::System::IntPtr pv, ::CoreLib::System::UIntPtr cb)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegDeleteValue(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string)
    int32_t Win32Native::RegDeleteValue(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegEnumKeyEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, int, char[], ref int, int[], System.Text.StringBuilder, int[], long[])
    int32_t Win32Native::RegEnumKeyEx_Ref(_::SafeHandles::SafeRegistryHandle* hKey, int32_t dwIndex, __array<char16_t>* lpName, int32_t& lpcbName, __array<int32_t>* lpReserved, ::CoreLib::System::Text::StringBuilder* lpClass, __array<int32_t>* lpcbClass, __array<int64_t>* lpftLastWriteTime)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegEnumValue(Microsoft.Win32.SafeHandles.SafeRegistryHandle, int, char[], ref int, System.IntPtr, int[], byte[], int[])
    int32_t Win32Native::RegEnumValue_Ref(_::SafeHandles::SafeRegistryHandle* hKey, int32_t dwIndex, __array<char16_t>* lpValueName, int32_t& lpcbValueName, ::CoreLib::System::IntPtr lpReserved_MustBeZero, __array<int32_t>* lpType, __array<uint8_t>* lpData, __array<int32_t>* lpcbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegOpenKeyEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int, int, out Microsoft.Win32.SafeHandles.SafeRegistryHandle)
    int32_t Win32Native::RegOpenKeyEx_Out(_::SafeHandles::SafeRegistryHandle* hKey, string* lpSubKey, int32_t ulOptions, int32_t samDesired, _::SafeHandles::SafeRegistryHandle*& hkResult)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegQueryInfoKey(Microsoft.Win32.SafeHandles.SafeRegistryHandle, System.Text.StringBuilder, int[], System.IntPtr, ref int, int[], int[], ref int, int[], int[], int[], int[])
    int32_t Win32Native::RegQueryInfoKey_Ref_Ref(_::SafeHandles::SafeRegistryHandle* hKey, ::CoreLib::System::Text::StringBuilder* lpClass, __array<int32_t>* lpcbClass, ::CoreLib::System::IntPtr lpReserved_MustBeZero, int32_t& lpcSubKeys, __array<int32_t>* lpcbMaxSubKeyLen, __array<int32_t>* lpcbMaxClassLen, int32_t& lpcValues, __array<int32_t>* lpcbMaxValueNameLen, __array<int32_t>* lpcbMaxValueLen, __array<int32_t>* lpcbSecurityDescriptor, __array<int32_t>* lpftLastWriteTime)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegQueryValueEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int[], ref int, byte[], ref int)
    int32_t Win32Native::RegQueryValueEx_Ref_Ref(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName, __array<int32_t>* lpReserved, int32_t& lpType, __array<uint8_t>* lpData, int32_t& lpcbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegQueryValueEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int[], ref int, ref int, ref int)
    int32_t Win32Native::RegQueryValueEx_Ref_Ref_Ref(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName, __array<int32_t>* lpReserved, int32_t& lpType, int32_t& lpData, int32_t& lpcbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegQueryValueEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int[], ref int, ref long, ref int)
    int32_t Win32Native::RegQueryValueEx_Ref_Ref_Ref(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName, __array<int32_t>* lpReserved, int32_t& lpType, int64_t& lpData, int32_t& lpcbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegQueryValueEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int[], ref int, char[], ref int)
    int32_t Win32Native::RegQueryValueEx_Ref_Ref(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName, __array<int32_t>* lpReserved, int32_t& lpType, __array<char16_t>* lpData, int32_t& lpcbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegSetValueEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int, Microsoft.Win32.RegistryValueKind, byte[], int)
    int32_t Win32Native::RegSetValueEx(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName, int32_t Reserved, _::RegistryValueKind__enum dwType, __array<uint8_t>* lpData, int32_t cbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegSetValueEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int, Microsoft.Win32.RegistryValueKind, ref int, int)
    int32_t Win32Native::RegSetValueEx_Ref(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName, int32_t Reserved, _::RegistryValueKind__enum dwType, int32_t& lpData, int32_t cbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegSetValueEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int, Microsoft.Win32.RegistryValueKind, ref long, int)
    int32_t Win32Native::RegSetValueEx_Ref(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName, int32_t Reserved, _::RegistryValueKind__enum dwType, int64_t& lpData, int32_t cbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.RegSetValueEx(Microsoft.Win32.SafeHandles.SafeRegistryHandle, string, int, Microsoft.Win32.RegistryValueKind, string, int)
    int32_t Win32Native::RegSetValueEx(_::SafeHandles::SafeRegistryHandle* hKey, string* lpValueName, int32_t Reserved, _::RegistryValueKind__enum dwType, string* lpData, int32_t cbData)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.ExpandEnvironmentStrings(string, System.Text.StringBuilder, int)
    int32_t Win32Native::ExpandEnvironmentStrings(string* lpSrc, ::CoreLib::System::Text::StringBuilder* lpDst, int32_t nSize)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.LocalReAlloc(System.IntPtr, System.IntPtr, int)
    ::CoreLib::System::IntPtr Win32Native::LocalReAlloc(::CoreLib::System::IntPtr handle, ::CoreLib::System::IntPtr sizetcbBytes, int32_t uFlags)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.SendMessageTimeout(System.IntPtr, int, System.IntPtr, string, uint, uint, System.IntPtr)
    ::CoreLib::System::IntPtr Win32Native::SendMessageTimeout(::CoreLib::System::IntPtr hWnd, int32_t Msg, ::CoreLib::System::IntPtr wParam, string* lParam, uint32_t fuFlags, uint32_t uTimeout, ::CoreLib::System::IntPtr lpdwResult)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.QueryUnbiasedInterruptTime(out ulong)
    bool Win32Native::QueryUnbiasedInterruptTime_Out(uint64_t& UnbiasedTime)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.VerifyVersionInfoW(Microsoft.Win32.Win32Native.OSVERSIONINFOEX, uint, ulong)
    bool Win32Native::VerifyVersionInfoW(_::Win32Native_OSVERSIONINFOEX* lpVersionInfo, uint32_t dwTypeMask, uint64_t dwlConditionMask)
    {
        throw 3221274624U;
    }
    
    // Method : Microsoft.Win32.Win32Native.VerSetConditionMask(ulong, uint, byte)
    uint64_t Win32Native::VerSetConditionMask(uint64_t dwlConditionMask, uint32_t dwTypeBitMask, uint8_t dwConditionMask)
    {
        throw 3221274624U;
    }

}}}

namespace CoreLib { namespace Microsoft { namespace Win32 { 
    namespace _ = ::CoreLib::Microsoft::Win32;
}}}

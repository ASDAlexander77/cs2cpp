#include "CoreLib.h"
#if _MSC_VER
#include <windows.h>
#else
#include <unistd.h>
#include <limits.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <fcntl.h>
#endif

#ifdef GC_PTHREADS
#define FILE_TYPE_CHAR 0x0002
#define FILE_TYPE_DISK 0x0001
#define FILE_TYPE_PIPE 0x0003
#define FILE_TYPE_REMOTE 0x8000
#define FILE_TYPE_UNKNOWN 0x0000
#define GENERIC_READ 0x80000000
#define GENERIC_WRITE 0x40000000
#define FILE_ATTRIBUTE_HIDDEN 0x00000002
#define FILE_ATTRIBUTE_SYSTEM 0x00000004
#define FILE_ATTRIBUTE_DIRECTORY 0x00000010
#define FILE_ATTRIBUTE_ARCHIVE 0x00000020
#define FILE_ATTRIBUTE_DEVICE 0x00000040
#define FILE_ATTRIBUTE_NORMAL 0x00000080
#define FILE_FLAG_NO_BUFFERING 0x20000000
#endif


namespace CoreLib { namespace Microsoft { namespace Win32 { 

	// Method : Microsoft.Win32.Win32Native.SetEvent(Microsoft.Win32.SafeHandles.SafeWaitHandle)
	bool Win32Native::SetEvent(CoreLib::Microsoft::Win32::SafeHandles::SafeWaitHandle* handle)
	{
#ifndef GC_PTHREADS
		return ::SetEvent((HANDLE)handle->handle.ToInt32());
#else
#error NOT IMPLEMENTED YET
#endif
	}

	// Method : Microsoft.Win32.Win32Native.ResetEvent(Microsoft.Win32.SafeHandles.SafeWaitHandle)
	bool Win32Native::ResetEvent(CoreLib::Microsoft::Win32::SafeHandles::SafeWaitHandle* handle)
	{
#ifndef GC_PTHREADS
		return ::ResetEvent((HANDLE)handle->handle.ToInt32());
#else
#error NOT IMPLEMENTED YET
#endif
	}

	// Method : Microsoft.Win32.Win32Native.CreateEvent(Microsoft.Win32.Win32Native.SECURITY_ATTRIBUTES, bool, bool, string)
	CoreLib::Microsoft::Win32::SafeHandles::SafeWaitHandle* Win32Native::CreateEvent(CoreLib::Microsoft::Win32::Win32Native_SECURITY_ATTRIBUTES* lpSecurityAttributes, bool isManualReset, bool initialState, string* name)
	{
#ifndef GC_PTHREADS
		auto hEvent = CreateEventW((LPSECURITY_ATTRIBUTES)lpSecurityAttributes,
			(BOOL)isManualReset,
			(BOOL)initialState,
			(LPCWSTR)(name != nullptr ? &name->m_firstChar : nullptr));

		if (hEvent == (HANDLE)-1)
		{
			return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeWaitHandle>(((CoreLib::System::IntPtr)CoreLib::Microsoft::Win32::Win32Native::INVALID_HANDLE_VALUE), false);
		}

		return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeWaitHandle>(__init<CoreLib::System::IntPtr>(hEvent), false);
#else
#error NOT IMPLEMENTED YET
#endif
	}

	// Method : Microsoft.Win32.Win32Native.GetFullPathName(char*, int, char*, System.IntPtr)
	int32_t Win32Native::GetFullPathName(char16_t* path, int32_t numBufferChars, char16_t* buffer, CoreLib::System::IntPtr mustBeZero)
	{
		if (static_cast<void*>(path) == (void*)nullptr)
		{
			throw __new<CoreLib::System::ArgumentNullException>(u"path"_s, u"path"_s);
		}

#ifndef GC_PTHREADS
		return GetFullPathNameW((LPCWSTR)path, numBufferChars, (LPWSTR)buffer, nullptr);
#elif _WIN32 || _WIN64
		return string::wcslen(_wfullpath(buffer, path, numBufferChars));
#else
		auto path_length = string::wcslen(path);
		auto utf8Enc = CoreLib::System::Text::Encoding::get_UTF8();
		auto byteCount = utf8Enc->GetByteCount(path, path_length);
		auto relative_path_utf8 = reinterpret_cast<uint8_t*>(alloca(byteCount + 1));
		auto bytesReceived = utf8Enc->GetBytes(path, path_length, relative_path_utf8, byteCount);
		auto resolved_path_utf8 = reinterpret_cast<uint8_t*>(alloca(numBufferChars));
		auto result = realpath(reinterpret_cast<const char*>(relative_path_utf8), reinterpret_cast<char*>(resolved_path_utf8));
		if (result != 0)
		{
			utf8Enc->GetChars(resolved_path_utf8, numBufferChars, buffer, numBufferChars);
			return static_cast<int32_t>(string::wcslen(buffer));
		}

		return 0;
#endif
	}

	// Method : Microsoft.Win32.Win32Native.GetStdHandle(int)
	CoreLib::System::IntPtr Win32Native::GetStdHandle(int32_t nStdHandle)
	{
		return __init<CoreLib::System::IntPtr>(nStdHandle);
	}

	// Method : Microsoft.Win32.Win32Native.CreateFile(string, int, System.IO.FileShare, Microsoft.Win32.Win32Native.SECURITY_ATTRIBUTES, System.IO.FileMode, int, System.IntPtr)
	CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* Win32Native::CreateFile(string* lpFileName, int32_t dwDesiredAccess, CoreLib::System::IO::enum_FileShare dwShareMode, CoreLib::Microsoft::Win32::Win32Native_SECURITY_ATTRIBUTES* securityAttrs, CoreLib::System::IO::enum_FileMode dwCreationDisposition, int32_t dwFlagsAndAttributes, CoreLib::System::IntPtr hTemplateFile)
	{
#ifndef GC_PTHREADS
		auto hFile = CreateFileW((LPCWSTR)&lpFileName->m_firstChar,    // name of the write
			(int32_t)dwDesiredAccess,						 // open for writing
			(int32_t)dwShareMode,							 // do not share
			(LPSECURITY_ATTRIBUTES)securityAttrs,			 // default security
			(int32_t)dwCreationDisposition,					 // create new file only
			(int32_t)dwFlagsAndAttributes,					 // normal file
			(void*)hTemplateFile);							 // no attr. template

		if (hFile == (HANDLE)-1)
		{
			return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(((CoreLib::System::IntPtr)CoreLib::Microsoft::Win32::Win32Native::INVALID_HANDLE_VALUE), false);
		}

		return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(__init<CoreLib::System::IntPtr>(hFile), false);
#else
		int32_t filed = -1;
		int32_t create_flags = (S_IRUSR | S_IWUSR | S_IRGRP | S_IROTH);
		int32_t open_flags = 0;
		bool fFileExists = false;

		if (lpFileName == nullptr)
		{
			return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(((CoreLib::System::IntPtr)CoreLib::Microsoft::Win32::Win32Native::INVALID_HANDLE_VALUE), false);
		}

		auto path = &lpFileName->m_firstChar;
		auto path_length = string::wcslen(path);
		auto utf8Enc = CoreLib::System::Text::Encoding::get_UTF8();
		auto byteCount = utf8Enc->GetByteCount(path, path_length);
		auto path_urf8 = reinterpret_cast<char*>(alloca(byteCount + 1));
		auto bytesReceived = utf8Enc->GetBytes(path, path_length, (uint8_t*)path_urf8, byteCount);

		switch ((uint32_t)dwDesiredAccess)
		{
		case GENERIC_READ:
			open_flags |= O_RDONLY;
			break;
		case GENERIC_WRITE:
			open_flags |= O_WRONLY;
			break;
		case GENERIC_READ | GENERIC_WRITE:
			open_flags |= O_RDWR;
			break;
		default:
			return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(((CoreLib::System::IntPtr)CoreLib::Microsoft::Win32::Win32Native::INVALID_HANDLE_VALUE), false);
		}

		switch ((CoreLib::System::IO::enum_FileMode)dwCreationDisposition)
		{
		case CoreLib::System::IO::enum_FileMode::c_Create:
			// check whether the file exists
			if (access(path_urf8, F_OK) == 0)
			{
				fFileExists = true;
			}

			open_flags |= O_CREAT | O_TRUNC;
			break;
		case CoreLib::System::IO::enum_FileMode::c_CreateNew:
			open_flags |= O_CREAT | O_EXCL;
			break;
		case CoreLib::System::IO::enum_FileMode::c_Open:
			/* don't need to do anything here */
			break;
		case CoreLib::System::IO::enum_FileMode::c_OpenOrCreate:
			if (access(path_urf8, F_OK) == 0)
			{
				fFileExists = true;
			}

			open_flags |= O_CREAT;
			break;
		case CoreLib::System::IO::enum_FileMode::c_Truncate:
			open_flags |= O_TRUNC;
			break;
		default:
			return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(((CoreLib::System::IntPtr)CoreLib::Microsoft::Win32::Win32Native::INVALID_HANDLE_VALUE), false);
		}

		if ((dwFlagsAndAttributes & FILE_FLAG_NO_BUFFERING) > 0)
		{
			open_flags |= O_DIRECT;
		}

		filed = open(path_urf8, open_flags, (open_flags & O_CREAT) > 0 ? create_flags : 0);
		if (filed < 0)
		{
			return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(__init<CoreLib::System::IntPtr>(0), false);
		}

#if flock
		auto lock_mode = (dwShareMode == 0 /* FILE_SHARE_NONE */) ? LOCK_EX : LOCK_SH;
		if (flock(filed, lock_mode | LOCK_NB) != 0)
		{
			return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(INVALID_HANDLE_VALUE, false);
		}
#endif

#if O_DIRECT
		if ((dwFlagsAndAttributes & FILE_FLAG_NO_BUFFERING) > 0)
		{
#if F_NOCACHE
			if (-1 == fcntl(filed, F_NOCACHE, 1))
			{
				return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(INVALID_HANDLE_VALUE, false);
			}
#else
			////#error Insufficient support for uncached I/O on this platform
#endif
		}
#endif

#if fcntl
		/* make file descriptor close-on-exec; inheritable handles will get
		"uncloseonexeced" in CreateProcess if they are actually being inherited*/
		auto ret = fcntl(filed, F_SETFD, 1);
		if (-1 == ret)
		{
			return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(INVALID_HANDLE_VALUE, false);
		}
#endif

		return __new<CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle>(__init<CoreLib::System::IntPtr>(filed), false);
#endif
	}

	// Method : Microsoft.Win32.Win32Native.CloseHandle(System.IntPtr)
	bool Win32Native::CloseHandle(CoreLib::System::IntPtr handle)
	{
#ifndef GC_PTHREADS
		return ::CloseHandle((HANDLE)handle.ToInt32());
#else
		// TODO: finish it for Event, write wrapper with Destroy function
		close(handle.ToInt32());
		return true;
#endif
	}

	// Method : Microsoft.Win32.Win32Native.GetFileType(Microsoft.Win32.SafeHandles.SafeFileHandle)
	int32_t Win32Native::GetFileType(CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* handle)
	{
		auto stdId = handle->DangerousGetHandle()->ToInt32();
		if (stdId == -11 || stdId == -12)
		{
			return FILE_TYPE_CHAR;
		}

		return FILE_TYPE_DISK;
	}

	// Method : Microsoft.Win32.Win32Native.GetFileSize(Microsoft.Win32.SafeHandles.SafeFileHandle, out int)
	int32_t Win32Native::GetFileSize_Out(CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* hFile, int32_t& highSize)
	{
#ifndef GC_PTHREADS
		return GetFileSize((HANDLE)hFile->DangerousGetHandle()->ToInt32(), (LPDWORD)highSize);
#else
		highSize = 0;
		struct stat data;
		auto returnCode = fstat(hFile->DangerousGetHandle()->ToInt32(), &data);
		if (returnCode != 0)
		{
			return 0;
		}

		return data.st_size;
#endif
	}

	// Method : Microsoft.Win32.Win32Native.ReadFile(Microsoft.Win32.SafeHandles.SafeFileHandle, byte*, int, out int, System.IntPtr)
	int32_t Win32Native::ReadFile_Out(CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* handle, uint8_t* bytes, int32_t numBytesToRead, int32_t& numBytesRead, CoreLib::System::IntPtr mustBeZero)
	{
		auto fd = handle->DangerousGetHandle()->ToInt32();
#if _MSC_VER
		return (int32_t) ::ReadFile((HANDLE)fd, (LPVOID)bytes, numBytesToRead, (LPDWORD)&numBytesRead, nullptr);
#else
		auto r = read(fd, bytes, numBytesToRead);
		if (r == -1)
		{
			numBytesRead = 0;
			return 0;
		}

		numBytesRead = r;
		return 1;
#endif
	}

	// Method : Microsoft.Win32.Win32Native.WriteFile(Microsoft.Win32.SafeHandles.SafeFileHandle, byte*, int, out int, System.IntPtr)
	int32_t Win32Native::WriteFile_Out(CoreLib::Microsoft::Win32::SafeHandles::SafeFileHandle* handle, uint8_t* bytes, int32_t numBytesToWrite, int32_t& numBytesWritten, CoreLib::System::IntPtr mustBeZero)
	{
		auto fd = handle->DangerousGetHandle()->ToInt32();
#ifndef GC_PTHREADS
		return (int32_t) ::WriteFile((HANDLE)fd, (LPCVOID)bytes, numBytesToWrite, (LPDWORD)&numBytesWritten, nullptr);
#else
		if (fd == -11)
		{
			numBytesWritten = write(STDOUT_FILENO, bytes, numBytesToWrite);
			return numBytesWritten < numBytesToWrite ? 0 : 1;
		}
		else if (fd == -12)
		{
			numBytesWritten = write(STDERR_FILENO, bytes, numBytesToWrite);
			return numBytesWritten < numBytesToWrite ? 0 : 1;
		}
		else
		{
			auto r = write(fd, bytes, numBytesToWrite);
			if (r != -1)
			{
				numBytesWritten = r;
				return 1;
			}
		}

		numBytesWritten = 0;
		return 0;
#endif
	}

	// Method : Microsoft.Win32.Win32Native.CreateDirectory(string, Microsoft.Win32.Win32Native.SECURITY_ATTRIBUTES)
	bool Win32Native::CreateDirectory(string* path, CoreLib::Microsoft::Win32::Win32Native_SECURITY_ATTRIBUTES* lpSecurityAttributes)
	{
		throw 3221274624U;
	}

	// Method : Microsoft.Win32.Win32Native.GetCurrentDirectory(int, System.Text.StringBuilder)
	int32_t Win32Native::GetCurrentDirectory(int32_t nBufferLength, CoreLib::System::Text::StringBuilder* lpBuffer)
	{
#ifndef GC_PTHREADS
		auto buffer = reinterpret_cast<char16_t*>(alloca(nBufferLength + 1));
		auto result = ::GetCurrentDirectoryW(nBufferLength, (LPWSTR)buffer);
		if (result != 0)
		{
			lpBuffer->Append(__wchar_t_to_string(buffer));
		}

		return result;
#else
		auto dwDirLen = 0;
		auto dwLastError = 0;

		char  *current_dir;

		/* NULL first arg means getcwd will allocate the string */
		current_dir = getcwd( NULL, PATH_MAX + 1 );

		if ( !current_dir )
		{
			// TODO: finish it
			////dwLastError = ERROR_INTERNAL_ERROR;
			goto done;
		}

		dwDirLen = strlen( current_dir );

		/* if the supplied buffer isn't long enough, return the required
		length, including room for the NULL terminator */
		if ( nBufferLength <= dwDirLen )
		{
			++dwDirLen; /* include space for the NULL */
			goto done;
		}
		else
		{
			lpBuffer->Append(__utf8_to_string(current_dir));
		}

done:
		free( current_dir );

		if ( dwLastError )
		{
			// TODO: finish it
			////SetLastError(dwLastError);
		}

		return dwDirLen;
#endif
	}

	// Method : Microsoft.Win32.Win32Native.GetFileAttributesEx(string, int, ref Microsoft.Win32.Win32Native.WIN32_FILE_ATTRIBUTE_DATA)
	bool Win32Native::GetFileAttributesEx_Ref(string* name, int32_t fileInfoLevel, CoreLib::Microsoft::Win32::Win32Native_WIN32_FILE_ATTRIBUTE_DATA& lpFileInformation)
	{
#ifndef GC_PTHREADS
		return ::GetFileAttributesExW((LPCWSTR)&name->m_firstChar, (GET_FILEEX_INFO_LEVELS)fileInfoLevel, &lpFileInformation);
#else
		auto filename = &name->m_firstChar;
		auto filename_length = string::wcslen(filename);
		auto utf8Enc = CoreLib::System::Text::Encoding::get_UTF8();
		auto byteCount = utf8Enc->GetByteCount(filename, filename_length);
		auto filename_urf8 = reinterpret_cast<char*>(alloca(byteCount + 1));
		auto bytesReceived = utf8Enc->GetBytes(filename, filename_length, (uint8_t*)filename_urf8, byteCount);

		struct stat data;
		auto return_code = stat(filename_urf8, &data);
		if (return_code != 0)
		{
			return false;
		}

		auto fileAttributes = FILE_ATTRIBUTE_NORMAL;

		// if this is folder, return false
		if ((data.st_mode & S_IFMT) == S_IFDIR)
		{
			fileAttributes = FILE_ATTRIBUTE_DIRECTORY;
		}

		lpFileInformation.fileAttributes = (int)fileAttributes;
		lpFileInformation.fileSizeLow = data.st_size;
		lpFileInformation.fileSizeHigh = 0;

		return true;
#endif
	}

	// Method : Microsoft.Win32.Win32Native.RemoveDirectory(string)
	bool Win32Native::RemoveDirectory(string* path)
	{
		throw 3221274624U;
	}

	// Method : Microsoft.Win32.Win32Native.SetCurrentDirectory(string)
	bool Win32Native::SetCurrentDirectory(string* path)
	{
#ifndef GC_PTHREADS
		return ::SetCurrentDirectoryW((LPCWSTR)&path->m_firstChar) > 0;
#else
		auto path_ptr = &path->m_firstChar;
		auto path_length = string::wcslen(path_ptr);
		auto utf8Enc = CoreLib::System::Text::Encoding::get_UTF8();
		auto byteCount = utf8Enc->GetByteCount(path_ptr, path_length);
		auto path_urf8 = reinterpret_cast<char*>(alloca(byteCount + 1));
		auto bytesReceived = utf8Enc->GetBytes(path_ptr, path_length, (uint8_t*)path_urf8, byteCount);

		return chdir(path_urf8) == 0;
#endif
	}
}}}

#include "System.Console.h"

namespace System_Console { 
    namespace _ = ::System_Console;
    // Method : Interop.Kernel32.GetCPInfoExW(uint, uint, Interop.Kernel32.CPINFOEXW*)
    int32_t Interop_Kernel32::GetCPInfoExW(uint32_t CodePage, uint32_t dwFlags, _::Interop_Kernel32_CPINFOEXW* lpCPInfoEx)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.Beep(int, int)
    bool Interop_Kernel32::Beep(int32_t frequency, int32_t duration)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FormatMessage(int, System.IntPtr, uint, int, System.Text.StringBuilder, int, System.IntPtr[])
    int32_t Interop_Kernel32::FormatMessage(int32_t dwFlags, ::CoreLib::System::IntPtr lpSource, uint32_t dwMessageId, int32_t dwLanguageId, ::CoreLib::System::Text::StringBuilder* lpBuffer, int32_t nSize, __array<::CoreLib::System::IntPtr>* arguments)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetConsoleCursorInfo(System.IntPtr, out Interop.Kernel32.CONSOLE_CURSOR_INFO)
    bool Interop_Kernel32::GetConsoleCursorInfo_Out(::CoreLib::System::IntPtr hConsoleOutput, _::Interop_Kernel32_CONSOLE_CURSOR_INFO& cci)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleCursorInfo(System.IntPtr, ref Interop.Kernel32.CONSOLE_CURSOR_INFO)
    bool Interop_Kernel32::SetConsoleCursorInfo_Ref(::CoreLib::System::IntPtr hConsoleOutput, _::Interop_Kernel32_CONSOLE_CURSOR_INFO& cci)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FillConsoleOutputAttribute(System.IntPtr, short, int, Interop.Kernel32.COORD, out int)
    bool Interop_Kernel32::FillConsoleOutputAttribute_Out(::CoreLib::System::IntPtr hConsoleOutput, int16_t wColorAttribute, int32_t numCells, _::Interop_Kernel32_COORD startCoord, int32_t& pNumBytesWritten)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.FillConsoleOutputCharacter(System.IntPtr, char, int, Interop.Kernel32.COORD, out int)
    bool Interop_Kernel32::FillConsoleOutputCharacter_Out(::CoreLib::System::IntPtr hConsoleOutput, char16_t character, int32_t nLength, _::Interop_Kernel32_COORD dwWriteCoord, int32_t& pNumCharsWritten)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetConsoleScreenBufferInfo(System.IntPtr, out Interop.Kernel32.CONSOLE_SCREEN_BUFFER_INFO)
    bool Interop_Kernel32::GetConsoleScreenBufferInfo_Out(::CoreLib::System::IntPtr hConsoleOutput, _::Interop_Kernel32_CONSOLE_SCREEN_BUFFER_INFO& lpConsoleScreenBufferInfo)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetConsoleCP()
    uint32_t Interop_Kernel32::GetConsoleCP()
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetConsoleTitle(System.Text.StringBuilder, int)
    int32_t Interop_Kernel32::GetConsoleTitle(::CoreLib::System::Text::StringBuilder* title, int32_t nSize)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetConsoleMode(System.IntPtr, out int)
    bool Interop_Kernel32::GetConsoleMode_Out(::CoreLib::System::IntPtr handle, int32_t& mode)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleMode(System.IntPtr, int)
    bool Interop_Kernel32::SetConsoleMode(::CoreLib::System::IntPtr handle, int32_t mode)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetConsoleOutputCP()
    uint32_t Interop_Kernel32::GetConsoleOutputCP()
    {
        return ::GetConsoleOutputCP();
    }
    
    // Method : Interop.Kernel32.GetLargestConsoleWindowSize(System.IntPtr)
    _::Interop_Kernel32_COORD Interop_Kernel32::GetLargestConsoleWindowSize(::CoreLib::System::IntPtr hConsoleOutput)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.GetFileType(System.IntPtr)
    uint32_t Interop_Kernel32::GetFileType(::CoreLib::System::IntPtr hFile)
    {
		return ::GetFileType(hFile.INTPTR_VALUE_FIELD);
    }
    
    // Method : Interop.Kernel32.GetStdHandle(int)
    ::CoreLib::System::IntPtr Interop_Kernel32::GetStdHandle(int32_t nStdHandle)
    {
		return  __init<::CoreLib::System::IntPtr>((void*)::GetStdHandle(nStdHandle));
    }
    
    // Method : Interop.Kernel32.MultiByteToWideChar(uint, uint, byte*, int, char*, int)
    int32_t Interop_Kernel32::MultiByteToWideChar(uint32_t CodePage, uint32_t dwFlags, uint8_t* lpMultiByteStr, int32_t cbMultiByte, char16_t* lpWideCharStr, int32_t cchWideChar)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.PeekConsoleInput(System.IntPtr, out Interop.InputRecord, int, out int)
    bool Interop_Kernel32::PeekConsoleInput_Out_Out(::CoreLib::System::IntPtr hConsoleInput, _::Interop_InputRecord& buffer, int32_t numInputRecords_UseOne, int32_t& numEventsRead)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.ReadFile(System.IntPtr, byte*, int, out int, System.IntPtr)
    int32_t Interop_Kernel32::ReadFile_Out(::CoreLib::System::IntPtr handle, uint8_t* bytes, int32_t numBytesToRead, int32_t& numBytesRead, ::CoreLib::System::IntPtr mustBeZero)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.ReadConsole(System.IntPtr, byte*, int, out int, System.IntPtr)
    bool Interop_Kernel32::ReadConsole_Out(::CoreLib::System::IntPtr hConsoleInput, uint8_t* lpBuffer, int32_t nNumberOfCharsToRead, int32_t& lpNumberOfCharsRead, ::CoreLib::System::IntPtr pInputControl)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.ReadConsoleInput(System.IntPtr, out Interop.InputRecord, int, out int)
    bool Interop_Kernel32::ReadConsoleInput_Out_Out(::CoreLib::System::IntPtr hConsoleInput, _::Interop_InputRecord& buffer, int32_t numInputRecords_UseOne, int32_t& numEventsRead)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.ReadConsoleOutput(System.IntPtr, Interop.Kernel32.CHAR_INFO*, Interop.Kernel32.COORD, Interop.Kernel32.COORD, ref Interop.Kernel32.SMALL_RECT)
    bool Interop_Kernel32::ReadConsoleOutput_Ref(::CoreLib::System::IntPtr hConsoleOutput, _::Interop_Kernel32_CHAR_INFO* pBuffer, _::Interop_Kernel32_COORD bufferSize, _::Interop_Kernel32_COORD bufferCoord, _::Interop_Kernel32_SMALL_RECT& readRegion)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleCP(int)
    bool Interop_Kernel32::SetConsoleCP(int32_t codePage)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleCtrlHandler(Interop.Kernel32.ConsoleCtrlHandlerRoutine, bool)
    bool Interop_Kernel32::SetConsoleCtrlHandler(_::Interop_Kernel32_ConsoleCtrlHandlerRoutine* handler, bool addOrRemove)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleCursorPosition(System.IntPtr, Interop.Kernel32.COORD)
    bool Interop_Kernel32::SetConsoleCursorPosition(::CoreLib::System::IntPtr hConsoleOutput, _::Interop_Kernel32_COORD cursorPosition)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleOutputCP(int)
    bool Interop_Kernel32::SetConsoleOutputCP(int32_t codePage)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleScreenBufferSize(System.IntPtr, Interop.Kernel32.COORD)
    bool Interop_Kernel32::SetConsoleScreenBufferSize(::CoreLib::System::IntPtr hConsoleOutput, _::Interop_Kernel32_COORD size)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleTextAttribute(System.IntPtr, short)
    int32_t Interop_Kernel32::SetConsoleTextAttribute(::CoreLib::System::IntPtr hConsoleOutput, int16_t wAttributes)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleTitle(string)
    bool Interop_Kernel32::SetConsoleTitle(string* title)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.SetConsoleWindowInfo(System.IntPtr, bool, Interop.Kernel32.SMALL_RECT*)
    bool Interop_Kernel32::SetConsoleWindowInfo(::CoreLib::System::IntPtr hConsoleOutput, bool absolute, _::Interop_Kernel32_SMALL_RECT* consoleWindow)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.WideCharToMultiByte(uint, uint, char*, int, byte*, int, System.IntPtr, System.IntPtr)
    int32_t Interop_Kernel32::WideCharToMultiByte(uint32_t CodePage, uint32_t dwFlags, char16_t* lpWideCharStr, int32_t cchWideChar, uint8_t* lpMultiByteStr, int32_t cbMultiByte, ::CoreLib::System::IntPtr lpDefaultChar, ::CoreLib::System::IntPtr lpUsedDefaultChar)
    {
		return ::WideCharToMultiByte(CodePage, dwFlags, (LPCWCH)lpWideCharStr, cchWideChar, (LPSTR)lpMultiByteStr, cbMultiByte, (char*)lpDefaultChar.INTPTR_VALUE_FIELD, (int32_t*)lpDefaultChar.INTPTR_VALUE_FIELD);
    }
    
    // Method : Interop.Kernel32.WriteFile(System.IntPtr, byte*, int, out int, System.IntPtr)
    int32_t Interop_Kernel32::WriteFile_Out(::CoreLib::System::IntPtr handle, uint8_t* bytes, int32_t numBytesToWrite, int32_t& numBytesWritten, ::CoreLib::System::IntPtr mustBeZero)
    {
		return ::WriteFile(handle.INTPTR_VALUE_FIELD, bytes, numBytesToWrite, (LPDWORD)&numBytesWritten, (LPOVERLAPPED)mustBeZero.INTPTR_VALUE_FIELD);
    }
    
    // Method : Interop.Kernel32.WriteConsole(System.IntPtr, byte*, int, out int, System.IntPtr)
    bool Interop_Kernel32::WriteConsole_Out(::CoreLib::System::IntPtr hConsoleOutput, uint8_t* lpBuffer, int32_t nNumberOfCharsToWrite, int32_t& lpNumberOfCharsWritten, ::CoreLib::System::IntPtr lpReservedMustBeNull)
    {
        throw 3221274624U;
    }
    
    // Method : Interop.Kernel32.WriteConsoleOutput(System.IntPtr, Interop.Kernel32.CHAR_INFO*, Interop.Kernel32.COORD, Interop.Kernel32.COORD, ref Interop.Kernel32.SMALL_RECT)
    bool Interop_Kernel32::WriteConsoleOutput_Ref(::CoreLib::System::IntPtr hConsoleOutput, _::Interop_Kernel32_CHAR_INFO* buffer, _::Interop_Kernel32_COORD bufferSize, _::Interop_Kernel32_COORD bufferCoord, _::Interop_Kernel32_SMALL_RECT& writeRegion)
    {
        throw 3221274624U;
    }

}

namespace System_Console { 
    namespace _ = ::System_Console;
}

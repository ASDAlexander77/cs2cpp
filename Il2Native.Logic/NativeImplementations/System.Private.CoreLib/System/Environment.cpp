#include "System.Private.CoreLib.h"

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;

	int32_t _exitCode = 0;

    // Method : System.Environment.TickCount.get
    int32_t Environment::get_TickCount()
    {
		std::chrono::steady_clock::time_point t0;
		auto now = std::chrono::steady_clock::now() - t0;
		return now.count() / 100;
	}
    
    // Method : System.Environment._Exit(int)
    void Environment::_Exit(int32_t exitCode)
    {
		std::exit(exitCode);
    }
    
    // Method : System.Environment.ExitCode.get
    int32_t Environment::get_ExitCode()
    {
		return _exitCode;
    }
    
    // Method : System.Environment.ExitCode.set
    void Environment::set_ExitCode(int32_t value)
    {
		_exitCode = value;
    }
    
    // Method : System.Environment.FailFast(string)
    void Environment::FailFast(string* message)
    {
		std::quick_exit(-1);
    }
    
    // Method : System.Environment.FailFast(string, System.Exception)
    void Environment::FailFast(string* message, _::Exception* exception)
    {
		std::quick_exit(-1);
	}
    
    // Method : System.Environment.GetProcessorCount()
    int32_t Environment::GetProcessorCount()
    {
		return std::thread::hardware_concurrency();
    }
    
    // Method : System.Environment.GetCommandLineArgsNative()
    __array<string*>* Environment::GetCommandLineArgsNative()
    {
        throw 3221274624U;
    }
    
    // Method : System.Environment.WinRTSupported()
    bool Environment::WinRTSupported()
    {
        throw 3221274624U;
    }
    
    // Method : System.Environment.HasShutdownStarted.get
    bool Environment::get_HasShutdownStarted()
    {
        throw 3221274624U;
    }
    
    // Method : System.Environment.CurrentProcessorNumber.get
    int32_t Environment::get_CurrentProcessorNumber()
    {
		return std::thread::hardware_concurrency();
    }

}}

namespace CoreLib { namespace System { 
    namespace _ = ::CoreLib::System;
}}

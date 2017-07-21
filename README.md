C# to C++ transpiler (Cs2Cpp)
===========================

The Cs2Cpp repo contains the complete source code implementation for Cs2Cpp. It includes CoreLib, and many other components. It is cross-platform.

Chat Room
---------

Want to chat with other members of the Cs2Cpp community?

[![Join the chat at https://gitter.im/ASDAlexander77/cs2cpp](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/ASDAlexander77/cs2cpp?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Engage, Contribute and Provide Feedback
---------------------------------------

Some of the best ways to contribute are to try things out, file bugs, and join in design conversations.


License
-------

Cs2Cpp is licensed under the MIT license.

Quick Start
-----------

Prerequisite: CMake 2.8+, .NET 4.6.2+, GCC 5.0+ or Microsoft Visual C++ 2017 Community Edition

1) Build Project

```
cd Il2Native
MSBuild Il2Native.sln /p:Configuration=Release /p:Platform="Any CPU"
```

or open Il2Native.sln in Visual Studio 2017 and compile it

2) Build CoreLib (aka mscorlib)

```
cd CoreLib
MSBuild CoreLib.csproj /p:Configuration=Release /p:Platform="AnyCPU"
```

3) Create a temporary folder to build projects/files

```
cd ..\..
mkdir playground
cd playground
```

4) Generating CoreLib C++ project

```
..\Il2Native\Il2Native\bin\Release\Cs2Cpp.exe /release ..\Il2Native\CoreLib\CoreLib.csproj
```

5) Compile it

```
cd CoreLib
build_prerequisite_vs2017_release.bat 
build_vs2017_release.bat
```

Now you have compiled CoreLib (mscorelib)

6) Compile HelloWorld.cs

```
cd ..
```

create file HelloWorld.cs

```C#
using System;

class X {
	public static int Main (string [] args)
	{
		Console.WriteLine ("Hello, World!");
		return 0;
	}
}
```

```
..\Il2Native\Il2Native\bin\Release\Cs2Cpp.exe /release HelloWorld.cs /corelib:..\Il2Native\CoreLib\bin\Release\CoreLib.dll
cd HelloWorld
build_vs2017_release.bat
```

Now you have HelloWorld.exe


How to compile CoreCLR and CoreFX projects
-----------

1) Build Core Lib

```
Cs2Cpp.exe /release <path_to_git>\coreclr\src\mscorlib\System.Private.CoreLib.csproj
```

3) Build any Library project, for example System.Private.Uri

```
Cs2Cpp.exe /release <path_to_git>\corefx\src\System.Private.Uri\src\System.Private.Uri.csproj /corelib:System.Private.CoreLib.dll /p:Configuration=netstandard2.0-Windows_NT-Release
```

3) Compile it

First Core library

```
cd System_Private_CoreLib
build_prerequisite_vs2017_release.bat 
build_vs2017_release.bat
```

Then your targeted library

```
cd System_Private_Uri
build_vs2017_release.bat
```

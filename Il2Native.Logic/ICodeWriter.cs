using Il2Native.Logic.CodeParts;

namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    using PEAssemblyReader;

    public interface ICodeWriter
    {
        void Write(string rawText);

        void Write(OpCodePart ilCode);

        void WriteStart(string moduleName);

        void WriteForwardDeclaration(IType type, int number, int count);

        void WriteTypeStart(IType type, IType genericType);

        void WriteTypeEnd(IType type);

        void WriteBeforeConstructors();

        void WriteConstructorStart(IConstructor ctor);

        void WriteConstructorEnd(IConstructor ctor);

        void WriteAfterConstructors();

        void WriteBeforeFields(int count);

        void WriteFieldStart(IField field, int number, int count);

        void WriteFieldEnd(IField field, int number, int count);

        void WriteAfterFields(int count, bool disablePostDeclarations = false);

        void WriteBeforeMethods();

        void WriteMethodStart(IMethod method);

        void WriteMethodEnd(IMethod method);

        void WriteAfterMethods();

        void WriteEnd();
        
        void Close();
    }
}

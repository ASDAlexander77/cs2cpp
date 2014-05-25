using Il2Native.Logic.CodeParts;

namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public interface ICodeWriter
    {
        void Write(string rawText);

        void Write(OpCodePart ilCode);

        void WriteStart(string moduleName);

        void WriteForwardDeclaration(System.Type type, int number, int count);

        void WriteTypeStart(System.Type type, System.Type genericType);

        void WriteTypeEnd(System.Type type);

        void WriteBeforeConstructors();

        void WriteConstructorStart(System.Reflection.ConstructorInfo ctor);

        void WriteConstructorEnd(System.Reflection.ConstructorInfo ctor);

        void WriteAfterConstructors();

        void WriteBeforeFields(int count);

        void WriteFieldStart(System.Reflection.FieldInfo field, int number, int count);

        void WriteFieldEnd(System.Reflection.FieldInfo field, int number, int count);

        void WriteAfterFields(int count, bool disablePostDeclarations = false);

        void WriteBeforeMethods();

        void WriteMethodStart(System.Reflection.MethodInfo method);

        void WriteMethodEnd(System.Reflection.MethodInfo method);

        void WriteAfterMethods();

        void WriteEnd();
        
        void Close();
    }
}

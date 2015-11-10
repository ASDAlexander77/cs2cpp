namespace Il2Native.Logic.Gencode.InternalMethods.RuntimeTypeHandler
{
    using System;
    using System.Collections.Generic;

    using PEAssemblyReader;

    public static class ConstructNameGen
    {
        public static readonly string Name = "Void System.RuntimeTypeHandle.ConstructName(System.RuntimeTypeHandle, System.TypeNameFormatFlags, System.Runtime.CompilerServices.StringHandleOnStack)";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            var runtimeType = codeWriter.System.System_RuntimeType;

            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgumentAddress(2);
            ilCodeBuilder.LoadFieldAddress(OpCodeExtensions.GetFieldByFieldNumber(codeWriter.ResolveType("System.Runtime.CompilerServices.StringHandleOnStack"), 0, codeWriter));
            ilCodeBuilder.LoadField(OpCodeExtensions.GetFieldByFieldNumber(codeWriter.System.System_IntPtr, 0, codeWriter));
            ilCodeBuilder.Castclass(codeWriter.System.System_String.ToPointerType());

            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.LoadConstant((int)RuntimeTypeInfoGen.TypeNameFormatFlags.FormatBasic);

            var notBasicName = ilCodeBuilder.Branch(Code.Bne_Un, Code.Bne_Un_S);
            // load Name
            ilCodeBuilder.LoadArgumentAddress(0);
            ilCodeBuilder.LoadField(OpCodeExtensions.GetFieldByFieldNumber(codeWriter.System.System_RuntimeTypeHandle, 0, codeWriter));
            ilCodeBuilder.LoadField(OpCodeExtensions.GetFieldByName(runtimeType, RuntimeTypeInfoGen.NameField, codeWriter));
            ilCodeBuilder.SaveIndirect(codeWriter.System.System_String, codeWriter);
            ilCodeBuilder.Return();

            ilCodeBuilder.Add(notBasicName);

            // load FullName
            ilCodeBuilder.LoadArgumentAddress(0);
            ilCodeBuilder.LoadField(OpCodeExtensions.GetFieldByFieldNumber(codeWriter.System.System_RuntimeTypeHandle, 0, codeWriter));
            ilCodeBuilder.LoadField(OpCodeExtensions.GetFieldByName(runtimeType, RuntimeTypeInfoGen.FullNameField, codeWriter));
            ilCodeBuilder.SaveIndirect(codeWriter.System.System_String, codeWriter);
            ilCodeBuilder.Return();

            yield return ilCodeBuilder.Register(Name, codeWriter);
        }
    }
}

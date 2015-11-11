namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class ExchangeGen
    {
        public static readonly string Name32 = "Int32 System.Threading.Interlocked.Exchange(Ref Int32&, Int32)";
        public static readonly string Name64 = "Int64 System.Threading.Interlocked.Exchange(Ref Int64&, Int64)";
        public static readonly string NameF = "Single System.Threading.Interlocked.Exchange(Ref Single&, Single)";
        public static readonly string NameD = "Double System.Threading.Interlocked.Exchange(Ref Double&, Double)";
        public static readonly string Name = "System.Object System.Threading.Interlocked.Exchange(Ref System.Object&, System.Object)";
        public static readonly string NamePtr = "System.IntPtr System.Threading.Interlocked.Exchange(Ref System.IntPtr&, System.IntPtr)";
        public static readonly string NameT = "T System.Threading.Interlocked.Exchange<T>(Ref T&, T)";

        private const string Swap = "swap";

        private const string Location = "location1";
        private const string Value = "_value";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            yield return GetExchangeForType(codeWriter.System.System_Int32, codeWriter).Register(Name32, codeWriter);
            yield return GetExchangeForType(codeWriter.System.System_Int64, codeWriter).Register(Name64, codeWriter);
            yield return GetExchangeForTypeWithCastTo(codeWriter.System.System_Single, codeWriter.System.System_Int32, codeWriter).Register(NameF, codeWriter);
            yield return GetExchangeForTypeWithCastTo(codeWriter.System.System_Double, codeWriter.System.System_Int64, codeWriter).Register(NameD, codeWriter);
            yield return GetExchangeForType(codeWriter.System.System_Object, codeWriter).Register(Name, codeWriter);
            yield return GetExchangeForIntPtrType(codeWriter).Register(NamePtr, codeWriter);

            var method = OpCodeExtensions.GetMethodsByMetadataName(codeWriter.ResolveType("System.Threading.Interlocked"), "Exchange`1", codeWriter).First();
            yield return GetExchangeForType(method.ReturnType, codeWriter).Register(NameT, codeWriter);
        }

        public static IlCodeBuilder GetExchangeForType(IType parameterType, ICodeWriter codeWriter)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.Call(
                new SynthesizedMethodStringAdapter(
                    Swap, null, parameterType, new[] { parameterType.ToPointerType().ToParameter(Location), parameterType.ToParameter(Value) }));
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetExchangeForTypeWithCastTo(IType parameterType, IType castTo, ICodeWriter codeWriter)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Locals.Add(castTo);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(castTo.ToPointerType());
            ilCodeBuilder.LoadArgumentAddress(1);
            ilCodeBuilder.Castclass(castTo.ToPointerType());
            if (castTo.TypeEquals(codeWriter.System.System_Int32))
            {
                ilCodeBuilder.Add(Code.Ldind_I4);
            }
            else
            {
                ilCodeBuilder.Add(Code.Ldind_I8);
            }

            ilCodeBuilder.Call(
                new SynthesizedMethodStringAdapter(
                    Swap, null, castTo, new[] { castTo.ToPointerType().ToParameter(Location), castTo.ToParameter(Value) }));

            ilCodeBuilder.SaveLocal(0);
            ilCodeBuilder.LoadLocalAddress(0);
            ilCodeBuilder.LoadIndirect(parameterType, codeWriter);

            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetExchangeForIntPtrType(ICodeWriter codeWriter)
        {
            var parameterType = codeWriter.System.System_IntPtr;
            var field = OpCodeExtensions.GetFieldByFieldNumber(parameterType, 0, codeWriter);

            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Locals.Add(parameterType);

            // to save for SaveField
            ilCodeBuilder.LoadLocalAddress(0);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadFieldAddress(field);
            ilCodeBuilder.LoadArgumentAddress(1);
            ilCodeBuilder.LoadField(field);
            ilCodeBuilder.Call(
                new SynthesizedMethodStringAdapter(
                    Swap, null, field.FieldType, new[] { field.FieldType.ToPointerType().ToParameter(Location), field.FieldType.ToParameter(Value) }));

            ilCodeBuilder.SaveField(field);

            ilCodeBuilder.LoadLocal(0);
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }
    }
}

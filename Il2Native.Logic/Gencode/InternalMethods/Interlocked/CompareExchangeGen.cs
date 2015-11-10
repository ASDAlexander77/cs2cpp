namespace Il2Native.Logic.Gencode.InternalMethods
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    using OpCodesEmit = System.Reflection.Emit.OpCodes;

    public static class CompareExchangeGen
    {
        public static readonly string Name32 = "Int32 System.Threading.Interlocked.CompareExchange(Ref Int32&, Int32, Int32)";
        public static readonly string Name64 = "Int64 System.Threading.Interlocked.CompareExchange(Ref Int64&, Int64, Int64)";
        public static readonly string NameF = "Single System.Threading.Interlocked.CompareExchange(Ref Single&, Single, Single)";
        public static readonly string NameD = "Double System.Threading.Interlocked.CompareExchange(Ref Double&, Double, Double)";
        public static readonly string Name = "System.Object System.Threading.Interlocked.CompareExchange(Ref System.Object&, System.Object, System.Object)";
        public static readonly string NamePtr = "System.IntPtr System.Threading.Interlocked.CompareExchange(Ref System.IntPtr&, System.IntPtr, System.IntPtr)";
        public static readonly string NameT = "T System.Threading.Interlocked.CompareExchange<T>(Ref T&, T, T)";
        public static readonly string Name32Bool = "Int32 System.Threading.Interlocked.CompareExchange(Ref Int32&, Int32, Int32, Ref Boolean&)";

        private const string CompareAndSwap = "compare_and_swap";
        private const string CompareAndSwapBool = "compare_and_swap_bool";

        private const string Location = "location1";
        private const string Value = "_value";
        private const string Comparand = "comparand";

        public static IEnumerable<Tuple<string, Func<IMethod, IMethod>>> Generate(ICodeWriter codeWriter)
        {
            yield return GetCompareExchangeForType(codeWriter.System.System_Int32, codeWriter).Register(Name32, codeWriter);
            yield return GetCompareExchangeForType(codeWriter.System.System_Int64, codeWriter).Register(Name64, codeWriter);
            yield return GetCompareExchangeForTypeWithCastTo(codeWriter.System.System_Single, codeWriter.System.System_Int32, codeWriter).Register(NameF, codeWriter);
            yield return GetCompareExchangeForTypeWithCastTo(codeWriter.System.System_Double, codeWriter.System.System_Int64, codeWriter).Register(NameD, codeWriter);
            yield return GetCompareExchangeForType(codeWriter.System.System_Object, codeWriter).Register(Name, codeWriter);
            yield return GetCompareExchangeForIntPtrType(codeWriter).Register(NamePtr, codeWriter);
            yield return GetCompareExchangeForTypeWithBool(codeWriter.System.System_Int32, codeWriter).Register(Name32Bool, codeWriter);

            var method = OpCodeExtensions.GetMethodsByMetadataName(codeWriter.ResolveType("System.Threading.Interlocked"), "CompareExchange`1", codeWriter).First();
            yield return GetCompareExchangeForType(method.ReturnType, codeWriter).Register(NameT, codeWriter);
        }

        public static IlCodeBuilder GetCompareExchangeForType(IType parameterType, ICodeWriter codeWriter)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadArgument(2);
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.Call(
                new SynthesizedMethodStringAdapter(
                    CompareAndSwap, null, parameterType, new[] { parameterType.ToPointerType().ToParameter(Location), parameterType.ToParameter(Value), parameterType.ToParameter(Comparand) }));
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetCompareExchangeForTypeWithBool(IType parameterType, ICodeWriter codeWriter)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Locals.Add(parameterType);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadIndirect(parameterType, codeWriter);
            ilCodeBuilder.SaveLocal(0);

            ilCodeBuilder.LoadArgument(3);
            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadArgument(2);
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.Call(
                new SynthesizedMethodStringAdapter(
                    CompareAndSwapBool, null, parameterType, new[] { parameterType.ToPointerType().ToParameter(Location), parameterType.ToParameter(Value), parameterType.ToParameter(Comparand) }));
            ilCodeBuilder.SaveIndirect(parameterType, codeWriter);

            ilCodeBuilder.LoadLocal(0);
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetCompareExchangeForTypeWithCastTo(IType parameterType, IType castTo, ICodeWriter codeWriter)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Locals.Add(castTo);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(castTo.ToPointerType());
            ilCodeBuilder.LoadArgumentAddress(2);
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
                    CompareAndSwap, null, castTo, new[] { castTo.ToPointerType().ToParameter(Location), castTo.ToParameter(Value), castTo.ToParameter(Comparand) }));

            ilCodeBuilder.SaveLocal(0);
            ilCodeBuilder.LoadLocalAddress(0);
            ilCodeBuilder.LoadIndirect(parameterType, codeWriter);

            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetCompareExchangeForIntPtrType(ICodeWriter codeWriter)
        {
            var parameterType = codeWriter.System.System_IntPtr;
            var field = OpCodeExtensions.GetFieldByFieldNumber(parameterType, 0, codeWriter);

            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Locals.Add(parameterType);

            // to save for SaveField
            ilCodeBuilder.LoadLocalAddress(0);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadFieldAddress(field);
            ilCodeBuilder.LoadArgumentAddress(2);
            ilCodeBuilder.LoadField(field);
            ilCodeBuilder.LoadArgumentAddress(1);
            ilCodeBuilder.LoadField(field);
            ilCodeBuilder.Call(
                new SynthesizedMethodStringAdapter(
                    CompareAndSwap, null, field.FieldType, new[] { field.FieldType.ToPointerType().ToParameter(Location), field.FieldType.ToParameter(Value), field.FieldType.ToParameter(Comparand) }));
            
            ilCodeBuilder.SaveField(field);
            
            ilCodeBuilder.LoadLocal(0);
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }
    }
}

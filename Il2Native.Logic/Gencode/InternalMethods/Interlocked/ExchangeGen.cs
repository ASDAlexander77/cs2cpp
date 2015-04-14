namespace Il2Native.Logic.Gencode.InternalMethods
{
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

        public static void Register(ITypeResolver typeResolver)
        {
            GetExchangeForType(typeResolver.System.System_Int32, typeResolver).Register(Name32);
            GetExchangeForType(typeResolver.System.System_Int64, typeResolver).Register(Name64);
            GetExchangeForTypeWithCastTo(typeResolver.System.System_Single, typeResolver.System.System_Int32, typeResolver).Register(NameF);
            GetExchangeForTypeWithCastTo(typeResolver.System.System_Double, typeResolver.System.System_Int64, typeResolver).Register(NameD);
            GetExchangeForType(typeResolver.System.System_Object, typeResolver).Register(Name);
            GetExchangeForIntPtrType(typeResolver).Register(NamePtr);

            var method = typeResolver.ResolveType("System.Threading.Interlocked").GetMethodsByMetadataName("Exchange`1", typeResolver).First();
            GetExchangeForType(method.ReturnType, typeResolver).Register(NameT);
        }

        public static IlCodeBuilder GetExchangeForType(IType parameterType, ITypeResolver typeResolver)
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

        public static IlCodeBuilder GetExchangeForTypeWithCastTo(IType parameterType, IType castTo, ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(castTo.ToPointerType());
            ilCodeBuilder.LoadArgumentAddress(1);
            ilCodeBuilder.Castclass(castTo.ToPointerType());
            if (castTo.TypeEquals(typeResolver.System.System_Int32))
            {
                ilCodeBuilder.Add(Code.Ldind_I4);
            }
            else
            {
                ilCodeBuilder.Add(Code.Ldind_I8);
            }

            ilCodeBuilder.Call(
                new SynthesizedMethodStringAdapter(
                    Swap, null, parameterType, new[] { castTo.ToPointerType().ToParameter(Location), castTo.ToParameter(Value) }));
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetExchangeForIntPtrType(ITypeResolver typeResolver)
        {
            var parameterType = typeResolver.System.System_IntPtr;
            var field = parameterType.GetFieldByFieldNumber(0, typeResolver);

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
                    Swap, null, parameterType, new[] { field.FieldType.ToPointerType().ToParameter(Location), field.FieldType.ToParameter(Value) }));

            ilCodeBuilder.SaveField(field);

            ilCodeBuilder.LoadLocal(0);
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }
    }
}

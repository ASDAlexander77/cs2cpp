namespace Il2Native.Logic.Gencode.InternalMethods
{
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

        public static void Register(ITypeResolver typeResolver)
        {
            GetCompareExchangeForType(typeResolver.System.System_Int32, typeResolver).Register(Name32);
            GetCompareExchangeForType(typeResolver.System.System_Int64, typeResolver).Register(Name64);
            GetCompareExchangeForTypeWithCastTo(typeResolver.System.System_Single, typeResolver.System.System_Int32, typeResolver).Register(NameF);
            GetCompareExchangeForTypeWithCastTo(typeResolver.System.System_Double, typeResolver.System.System_Int64, typeResolver).Register(NameD);
            GetCompareExchangeForType(typeResolver.System.System_Object, typeResolver).Register(Name);
            GetCompareExchangeForIntPtrType(typeResolver).Register(NamePtr);
            GetCompareExchangeForTypeWithBool(typeResolver.System.System_Int32, typeResolver).Register(Name32Bool);

            var method = typeResolver.ResolveType("System.Threading.Interlocked").GetMethodsByMetadataName("CompareExchange`1", typeResolver).First();
            GetCompareExchangeForType(method.ReturnType, typeResolver).Register(NameT);
        }

        public static IlCodeBuilder GetCompareExchangeForType(IType parameterType, ITypeResolver typeResolver)
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

        public static IlCodeBuilder GetCompareExchangeForTypeWithBool(IType parameterType, ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Locals.Add(parameterType);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadIndirect(parameterType, typeResolver);
            ilCodeBuilder.SaveLocal(0);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.LoadArgument(2);
            ilCodeBuilder.LoadArgument(1);
            ilCodeBuilder.LoadArgument(3);
            ilCodeBuilder.Call(
                new SynthesizedMethodStringAdapter(
                    CompareAndSwapBool, null, parameterType, new[] { parameterType.ToPointerType().ToParameter(Location), parameterType.ToParameter(Value), parameterType.ToParameter(Comparand) }));
            ilCodeBuilder.SaveIndirect(parameterType, typeResolver);

            ilCodeBuilder.LoadLocal(0);
            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetCompareExchangeForTypeWithCastTo(IType parameterType, IType castTo, ITypeResolver typeResolver)
        {
            var ilCodeBuilder = new IlCodeBuilder();

            ilCodeBuilder.Locals.Add(castTo);

            ilCodeBuilder.LoadArgument(0);
            ilCodeBuilder.Castclass(castTo.ToPointerType());
            ilCodeBuilder.LoadArgumentAddress(2);
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
                    CompareAndSwap, null, castTo, new[] { castTo.ToPointerType().ToParameter(Location), castTo.ToParameter(Value), castTo.ToParameter(Comparand) }));

            ilCodeBuilder.SaveLocal(0);
            ilCodeBuilder.LoadLocalAddress(0);
            ilCodeBuilder.LoadIndirect(parameterType, typeResolver);

            ilCodeBuilder.Add(Code.Ret);

            return ilCodeBuilder;
        }

        public static IlCodeBuilder GetCompareExchangeForIntPtrType(ITypeResolver typeResolver)
        {
            var parameterType = typeResolver.System.System_IntPtr;
            var field = parameterType.GetFieldByFieldNumber(0, typeResolver);

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

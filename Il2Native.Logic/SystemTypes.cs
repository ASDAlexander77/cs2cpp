namespace Il2Native.Logic
{
    using PEAssemblyReader;

    public class SystemTypes
    {
        public SystemTypes(IModule module)
        {
            this.System_Object = module.ResolveType("System.Object", null);
            this.System_Enum = module.ResolveType("System.Enum", null);
            this.System_MulticastDelegate = module.ResolveType("System.MulticastDelegate", null);
            this.System_Delegate = module.ResolveType("System.Delegate", null);
            this.System_ValueType = module.ResolveType("System.ValueType", null);
            this.System_Void = module.ResolveType("System.Void", null);
            this.System_Boolean = module.ResolveType("System.Boolean", null);
            this.System_Char = module.ResolveType("System.Char", null);
            this.System_SByte = module.ResolveType("System.SByte", null);
            this.System_Byte = module.ResolveType("System.Byte", null);
            this.System_Int16 = module.ResolveType("System.Int16", null);
            this.System_UInt16 = module.ResolveType("System.UInt16", null);
            this.System_Int32 = module.ResolveType("System.Int32", null);
            this.System_UInt32 = module.ResolveType("System.UInt32", null);
            this.System_Int64 = module.ResolveType("System.Int64", null);
            this.System_UInt64 = module.ResolveType("System.UInt64", null);
            this.System_Decimal = module.ResolveType("System.Decimal", null);
            this.System_Single = module.ResolveType("System.Single", null);
            this.System_Double = module.ResolveType("System.Double", null);
            this.System_String = module.ResolveType("System.String", null);
            this.System_IntPtr = module.ResolveType("System.IntPtr", null);
            this.System_UIntPtr = module.ResolveType("System.UIntPtr", null);
            this.System_Array = module.ResolveType("System.Array", null);
            this.System_Collections_IEnumerable = module.ResolveType("System.Collections.IEnumerable", null);
            this.System_Collections_Generic_IEnumerable_T = module.ResolveType("System.Collections.Generic.IEnumerable`1", null);
            this.System_Collections_Generic_IList_T = module.ResolveType("System.Collections.Generic.IList`1", null);
            this.System_Collections_Generic_ICollection_T = module.ResolveType("System.Collections.Generic.ICollection`1", null);
            this.System_Collections_IEnumerator = module.ResolveType("System.Collections.IEnumerator", null);
            this.System_Collections_Generic_IEnumerator_T = module.ResolveType("System.Collections.Generic.IEnumerator`1", null);
            this.System_Collections_Generic_IReadOnlyList_T = module.ResolveType("System.Collections.Generic.IReadOnlyList`1", null);
            this.System_Collections_Generic_IReadOnlyCollection_T = module.ResolveType("System.Collections.Generic.IReadOnlyCollection`1", null);
            this.System_Nullable_T = module.ResolveType("System.Nullable`1", null);
            this.System_DateTime = module.ResolveType("System.DateTime", null);
            this.System_IDisposable = module.ResolveType("System.IDisposable", null);
            this.System_TypedReference = module.ResolveType("System.TypedReference", null);
            ////this.System_ArgIterator = module.ResolveType("System.ArgIterator", null);
            this.System_RuntimeArgumentHandle = module.ResolveType("System.RuntimeArgumentHandle", null);
            this.System_RuntimeFieldHandle = module.ResolveType("System.RuntimeFieldHandle", null);
            this.System_RuntimeMethodHandle = module.ResolveType("System.RuntimeMethodHandle", null);
            this.System_RuntimeTypeHandle = module.ResolveType("System.RuntimeTypeHandle", null);
            this.System_IAsyncResult = module.ResolveType("System.IAsyncResult", null);
            this.System_AsyncCallback = module.ResolveType("System.AsyncCallback", null);
            this.System_ArraySegment_T1 = module.ResolveType("System.ArraySegment`1", null);
            this.System_Exception = module.ResolveType("System.Exception", null);
            this.System_NotSupportedException = module.ResolveType("System.NotSupportedException", null);
            this.System_NotImplementedException = module.ResolveType("System.NotImplementedException", null);
            this.System_NullReferenceException = module.ResolveType("System.NullReferenceException", null);
            this.System_Type = module.ResolveType("System.Type", null);
            this.System_RuntimeType = module.ResolveType("System.RuntimeType", null);
            this.System_RuntimeModule = module.ResolveType("System.Reflection.RuntimeModule", null);
            this.System_RuntimeAssembly = module.ResolveType("System.Reflection.RuntimeAssembly", null);
        }

        public IType System_Object { get; private set; }

        public IType System_Enum { get; private set; }

        public IType System_MulticastDelegate { get; private set; }

        public IType System_Delegate { get; private set; }

        public IType System_ValueType { get; private set; }

        public IType System_Void { get; private set; }

        public IType System_Boolean { get; private set; }

        public IType System_Char { get; private set; }

        public IType System_SByte { get; private set; }

        public IType System_Byte { get; private set; }

        public IType System_Int16 { get; private set; }

        public IType System_UInt16 { get; private set; }

        public IType System_Int32 { get; private set; }

        public IType System_UInt32 { get; private set; }

        public IType System_Int64 { get; private set; }

        public IType System_UInt64 { get; private set; }

        public IType System_Decimal { get; private set; }

        public IType System_Single { get; private set; }

        public IType System_Double { get; private set; }

        public IType System_String { get; private set; }

        public IType System_IntPtr { get; private set; }

        public IType System_UIntPtr { get; private set; }

        public IType System_Array { get; private set; }

        public IType System_Collections_IEnumerable { get; private set; }

        public IType System_Collections_Generic_IEnumerable_T { get; private set; }

        public IType System_Collections_Generic_IList_T { get; private set; }

        public IType System_Collections_Generic_ICollection_T { get; private set; }

        public IType System_Collections_IEnumerator { get; private set; }

        public IType System_Collections_Generic_IEnumerator_T { get; private set; }

        public IType System_Collections_Generic_IReadOnlyList_T { get; private set; }

        public IType System_Collections_Generic_IReadOnlyCollection_T { get; private set; }

        public IType System_Nullable_T { get; private set; }

        public IType System_DateTime { get; private set; }

        public IType System_IDisposable { get; private set; }

        public IType System_TypedReference { get; private set; }

        ////public IType System_ArgIterator { get; private set; }

        public IType System_RuntimeArgumentHandle { get; private set; }

        public IType System_RuntimeFieldHandle { get; private set; }

        public IType System_RuntimeMethodHandle { get; private set; }

        public IType System_RuntimeTypeHandle { get; private set; }

        public IType System_IAsyncResult { get; private set; }

        public IType System_AsyncCallback { get; private set; }

        public IType System_ArraySegment_T1 { get; private set; }

        public IType System_Exception { get; private set; }

        public IType System_NotSupportedException { get; private set; }

        public IType System_NotImplementedException { get; private set; }

        public IType System_NullReferenceException { get; private set; }

        public IType System_Type { get; private set; }

        public IType System_RuntimeType { get; private set; }

        public IType System_RuntimeModule { get; private set; }

        public IType System_RuntimeAssembly { get; private set; }
    }
}

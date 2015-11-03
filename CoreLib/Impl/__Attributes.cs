namespace System
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Constructor | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Field, AllowMultiple = false,
        Inherited = false)]
    public sealed class MergeCodeAttribute : System.Attribute
    {
    }
}

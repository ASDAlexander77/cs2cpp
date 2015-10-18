namespace System
{
    public partial struct RuntimeTypeHandle
    {
        internal static bool CanCastTo(RuntimeType type, RuntimeType target)
        {
            if (type == null || target == null)
                return false;

            if (target == null)
                return false;

            if (type == target)
                return true;

            // If c is a subclass of this class, then c can be cast to this type.
            if (target.IsSubclassOf(type))
                return true;

            if (target.IsInterface)
            {
                return type.ImplementInterface(target);
            }

            return false;
        }
    }
}

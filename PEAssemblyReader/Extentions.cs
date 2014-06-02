namespace PEAssemblyReader
{
    using System.Diagnostics;

    public static class Extentions
    {
        public static bool TypeEquals(this IType type, IType other)
        {
            return type != null && other.CompareTo(type) == 0;
        }

        public static bool TypeNotEquals(this IType type, IType other)
        {
            return !type.TypeEquals(other);
        }

        public static bool IsDerivedFrom(this IType thisType, IType type)
        {
            Debug.Assert((object)type != null);

            if ((object)thisType == (object)type)
            {
                return false;
            }

            var t = thisType.BaseType;
            while ((object)t != null)
            {
                if (type.TypeEquals(t))
                {
                    return true;
                }

                t = t.BaseType;
            }

            return false;
        }
    }
}

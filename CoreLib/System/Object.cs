////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    using System;
    using System.Runtime.CompilerServices;
    /**
     * The <i>Object</i> is the root class for all object in the CLR System. <i>Object</i>
     * is the super class for all other CLR objects and provide a set of methods and low level
     * services to subclasses.  These services include object synchronization and support for clone
     * operations.
     *
     * @see System.ICloneable
     */
    //This class contains no data and does not need to be serializable
    [Serializable()]
    public class Object
    {
        public Object()
        {
        }

        public static bool Equals(Object objA, Object objB)
        {
            if (objA == objB)
            {
                return true;
            }

            if (objA == null || objB == null)
            {
                return false;
            }

            return objA.Equals(objB);
        }

        public static bool ReferenceEquals(Object objA, Object objB)
        {
            return objA == objB;
        }

        public virtual String ToString()
        {
            return GetType().FullName;
        }

        public virtual bool Equals(Object obj)
        {
            throw new NotImplementedException();
        }

        public virtual int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public extern Type GetType();

        ~Object()
        {
        }

        protected extern Object MemberwiseClone();
    }
}



using System.Reflection;
using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, Inherited = false)]
    [System.Runtime.InteropServices.ComVisible(true)]
    public sealed class DecimalConstantAttribute : Attribute
    {
        [CLSCompliant(false)]
        public DecimalConstantAttribute(
            byte scale,
            byte sign,
            uint hi,
            uint mid,
            uint low
        )
        {
            dec = new System.Decimal((int)low, (int)mid, (int)hi, (sign != 0), scale);
        }

        public DecimalConstantAttribute(
            byte scale,
            byte sign,
            int hi,
            int mid,
            int low
        )
        {
            dec = new System.Decimal(low, mid, hi, (sign != 0), scale);
        }


        public System.Decimal Value
        {
            get
            {
                return dec;
            }
        }

        private System.Decimal dec;
    }
}


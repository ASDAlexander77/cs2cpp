// Licensed under the MIT license.

namespace System.Reflection
{
    using System;

    [Serializable]
    public abstract class EventInfo : MemberInfo
    {
        public override MemberTypes MemberType
        {
            get
            {
                return MemberTypes.Event;
            }
        }
    }
}

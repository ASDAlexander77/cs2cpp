////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////namespace System
namespace System
{
    using System.Diagnostics.Contracts;

    [Serializable()]
    public abstract class MulticastDelegate : Delegate
    {
        private MulticastDelegate[] _invocationList;
        private int _invocationCount;

        // This method returns the Invocation list of this multicast delegate.
        [System.Security.SecuritySafeCritical]
        public Delegate[] GetInvocationList()
        {
            Contract.Ensures(Contract.Result<Delegate[]>() != null);

            Delegate[] del;
            Object[] invocationList = _invocationList as Object[];
            if (invocationList == null)
            {
                del = new Delegate[1];
                del[0] = this;
            }
            else
            {
                // Create an array of delegate copies and each
                //    element into the array
                int invocationCount = (int)_invocationCount;
                del = new Delegate[invocationCount];

                for (int i = 0; i < invocationCount; i++)
                    del[i] = (Delegate)invocationList[i];
            }
            return del;
        }

        public static bool operator ==(MulticastDelegate d1, MulticastDelegate d2)
        {
            if ((Object)d1 == null)
                return (Object)d2 == null;

            return d1.Equals(d2);
        }
        
        public static bool operator !=(MulticastDelegate d1, MulticastDelegate d2)
        {
            if ((Object)d1 == null)
                return (Object)d2 != null;

            return !d1.Equals(d2);
        }

        protected override Delegate CombineImpl(Delegate follow)
        {
            if (follow == null)
                return this;

            // Verify that the types are the same...
            if (this.GetType() != follow.GetType())
                throw new ArgumentException("DlgtTypeMis");

            MulticastDelegate dFollow = (MulticastDelegate)follow;
            MulticastDelegate[] resultList;
            int followCount = 1;
            MulticastDelegate[] followList = dFollow._invocationList;
            if (followList != null)
                followCount = (int)dFollow._invocationCount;

            int resultCount;
            MulticastDelegate[] invocationList = _invocationList;
            if (invocationList == null)
            {
                resultCount = 1 + followCount;
                resultList = new MulticastDelegate[resultCount];
                resultList[0] = this;
                if (followList == null)
                {
                    resultList[1] = dFollow;
                }
                else
                {
                    for (int i = 0; i < followCount; i++)
                        resultList[1 + i] = followList[i];
                }
                return NewMulticastDelegate(resultList, resultCount);
            }
            else
            {
                int invocationCount = _invocationCount;
                resultCount = invocationCount + followCount;
                resultList = null;
                if (resultCount <= invocationList.Length)
                {
                    resultList = invocationList;
                    if (followList == null)
                    {
                        if (!TrySetSlot(resultList, invocationCount, dFollow))
                            resultList = null;
                    }
                    else
                    {
                        for (int i = 0; i < followCount; i++)
                        {
                            if (!TrySetSlot(resultList, invocationCount + i, followList[i]))
                            {
                                resultList = null;
                                break;
                            }
                        }
                    }
                }

                if (resultList == null)
                {
                    int allocCount = invocationList.Length;
                    while (allocCount < resultCount)
                        allocCount *= 2;

                    resultList = new MulticastDelegate[allocCount];

                    for (int i = 0; i < invocationCount; i++)
                        resultList[i] = invocationList[i];

                    if (followList == null)
                    {
                        resultList[invocationCount] = dFollow;
                    }
                    else
                    {
                        for (int i = 0; i < followCount; i++)
                            resultList[invocationCount + i] = followList[i];
                    }
                }
                return NewMulticastDelegate(resultList, resultCount);
            }
        }

        protected override sealed Delegate RemoveImpl(Delegate value)
        {
            MulticastDelegate v = value as MulticastDelegate;

            if (v == null)
                return this;
            if (v._invocationList == null)
            {
                MulticastDelegate[] invocationList = _invocationList;
                if (invocationList == null)
                {
                    if (this.Equals(value))
                        return null;
                }
                else
                {
                    int invocationCount = (int)_invocationCount;
                    for (int i = invocationCount; --i >= 0; )
                    {
                        if (value.Equals(invocationList[i]))
                        {
                            if (invocationCount == 2)
                            {
                                // Special case - only one value left, either at the beginning or the end
                                return (Delegate)invocationList[1 - i];
                            }
                            else
                            {
                                MulticastDelegate[] list = DeleteFromInvocationList(invocationList, invocationCount, i, 1);
                                return NewMulticastDelegate(list, invocationCount - 1);
                            }
                        }
                    }
                }
            }
            else
            {
                MulticastDelegate[] invocationList = _invocationList;
                if (invocationList != null)
                {
                    int invocationCount = (int)_invocationCount;
                    int vInvocationCount = (int)v._invocationCount;
                    for (int i = invocationCount - vInvocationCount; i >= 0; i--)
                    {
                        if (EqualInvocationLists(invocationList, v._invocationList, i, vInvocationCount))
                        {
                            if (invocationCount - vInvocationCount == 0)
                            {
                                return null;
                            }
                            else if (invocationCount - vInvocationCount == 1)
                            {
                                return (Delegate)invocationList[i != 0 ? 0 : invocationCount - 1];
                            }
                            else
                            {
                                MulticastDelegate[] list = DeleteFromInvocationList(invocationList, invocationCount, i, vInvocationCount);
                                return NewMulticastDelegate(list, invocationCount - vInvocationCount);
                            }
                        }
                    }
                }
            }

            return this;
        }

        private bool TrySetSlot(MulticastDelegate[] a, int index, MulticastDelegate o)
        {
            if (a[index] == null && System.Threading.Interlocked.CompareExchange(ref a[index], o, null) == null)
                return true;

            if (a[index] != null)
            {
                MulticastDelegate d = o;
                MulticastDelegate dd = a[index];
                if (dd._methodPtr == d._methodPtr &&
                    dd._target == d._target)
                {
                    return true;
                }
            }

            return false;
        }

        private MulticastDelegate[] DeleteFromInvocationList(MulticastDelegate[] invocationList, int invocationCount, int deleteIndex, int deleteCount)
        {
            MulticastDelegate[] thisInvocationList = _invocationList;
            int allocCount = thisInvocationList.Length;
            while (allocCount / 2 >= invocationCount - deleteCount)
                allocCount /= 2;

            MulticastDelegate[] newInvocationList = new MulticastDelegate[allocCount];

            for (int i = 0; i < deleteIndex; i++)
                newInvocationList[i] = invocationList[i];

            for (int i = deleteIndex + deleteCount; i < invocationCount; i++)
                newInvocationList[i - deleteCount] = invocationList[i];

            return newInvocationList;
        }

        private bool EqualInvocationLists(MulticastDelegate[] a, MulticastDelegate[] b, int start, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!(a[start + i].Equals(b[i])))
                    return false;
            }
            return true;
        }

        private MulticastDelegate NewMulticastDelegate(MulticastDelegate[] invocationList, int invocationCount)
        {
            MulticastDelegate result = (MulticastDelegate)this.MemberwiseClone();
            result._invocationList = invocationList;
            result._invocationCount = invocationCount;

            return result;
        }

    }
}



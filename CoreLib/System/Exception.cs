////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Apache License 2.0 (Apache)
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace System
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Reflection;
    using System.Runtime.ExceptionServices;

    [Serializable()]
    public class Exception
    {
        private Exception m_innerException;
        private object m_stackTrace;
        protected int m_HResult;
        protected string _message;

        public UIntPtr IPForWatsonBuckets;

        public Exception()
        {
        }

        public Exception(String message)
        {
            _message = message;
        }

        public Exception(String message, Exception innerException)
        {
            _message = message;
            m_innerException = innerException;
        }

        public virtual String Message
        {
            get
            {
                if (_message == null)
                {
                    return "Exception was thrown: " + this.GetType().FullName;
                }
                else
                {
                    return _message;
                }
            }
        }

        public Exception InnerException
        {
            get { return m_innerException; }
        }

        public virtual Exception GetBaseException()
        {
            Exception inner = InnerException;
            Exception back = this;

            while (inner != null)
            {
                back = inner;
                inner = inner.InnerException;
            }

            return back;
        }

        public virtual String StackTrace
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string RemoteStackTrace
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object WatsonBuckets
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override String ToString()
        {
            String message = _message;
            String s = base.ToString();

            if (message != null && message.Length > 0)
            {
                s += ": " + message;
            }

            return s;
        }

        public void GetStackTracesDeepCopy(out object stackTrace, out object dynamicMethods)
        {
            stackTrace = null;
            dynamicMethods = null;
        }

        public void RestoreExceptionDispatchInfo(ExceptionDispatchInfo exceptionDispatchInfo)
        {
            throw new NotImplementedException();
        }
    }

}



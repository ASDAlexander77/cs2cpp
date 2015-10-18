namespace System.Runtime.InteropServices
{
    partial class SafeHandle
    {
        void InternalFinalize()
        {
        }

        private void InternalDispose()
        {
        }

        public void SetHandleAsInvalid()
        {
            throw new NotImplementedException();
        }

        // Add a reason why this handle should not be relinquished (i.e. have
        // ReleaseHandle called on it). This method has dangerous in the name since
        // it must always be used carefully (e.g. called within a CER) to avoid
        // leakage of the handle. It returns a boolean indicating whether the
        // increment was actually performed to make it easy for program logic to
        // back out in failure cases (i.e. is a call to DangerousRelease needed).
        // It is passed back via a ref parameter rather than as a direct return so
        // that callers need not worry about the atomicity of calling the routine
        // and assigning the return value to a variable (the variable should be
        // explicitly set to false prior to the call). The only failure cases are
        // when the method is interrupted prior to processing by a thread abort or
        // when the handle has already been (or is in the process of being)
        // released.
        public void DangerousAddRef(ref bool success)
        {
            throw new NotImplementedException();
        }

        // Partner to DangerousAddRef. This should always be successful when used in
        // a correct manner (i.e. matching a successful DangerousAddRef and called
        // from a region such as a CER where a thread abort cannot interrupt
        // processing). In the same way that unbalanced DangerousAddRef calls can
        // cause resource leakage, unbalanced DangerousRelease calls may cause
        // invalid handle states to become visible to other threads. This
        // constitutes a potential security hole (via handle recycling) as well as a
        // correctness problem -- so don't ever expose Dangerous* calls out to
        // untrusted code.
        public void DangerousRelease()
        {
            throw new NotImplementedException();
        }
    }
}

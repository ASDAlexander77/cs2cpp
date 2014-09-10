namespace Il2Native.Logic
{
    using System;
    using PEAssemblyReader;
    using System.Collections.Generic;

    public class MethodsWalker
    {
        private IMethod startMethod;

        public MethodsWalker(IMethod startMethod)
        {
            this.startMethod = startMethod;
        }

        public HashSet<IType> DiscoverAllStaticFieldsDependencies()
        {
            var calledMethods = new HashSet<IMethod>();
            var readStaticFields = new HashSet<IField>();

            WalkMethod(this.startMethod, calledMethods, readStaticFields);

            var typesWithStaticFields = new HashSet<IType>();
            foreach (var staticField in readStaticFields)
            {
                if (staticField.DeclaringType.TypeEquals(this.startMethod.DeclaringType))
                {
                    continue;
                }

                typesWithStaticFields.Add(staticField.DeclaringType);
            }

            return typesWithStaticFields;
        }

        private void WalkMethod(IMethod method, HashSet<IMethod> allCalledMethods, HashSet<IField> allReadStaticFields)
        {
            var calledMethods = new HashSet<IMethod>();
            method.DiscoverMethod(null, calledMethods, allReadStaticFields);

            foreach (var nextMethod in calledMethods)
            {
                if (allCalledMethods.Contains(nextMethod))
                {
                    continue;
                }

                allCalledMethods.Add(nextMethod);

                WalkMethod(nextMethod, allCalledMethods, allReadStaticFields);
            }
        }
    }
}

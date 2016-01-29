namespace Il2Native.Logic.DOM.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Microsoft.CodeAnalysis;

    public class AssembliesInfoResolver : IAssembliesInfoResolver
    {
        public IDictionary<string, ITypeSymbol> TypesByName { get; set; }

        public ITypeSymbol GetType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw  new ArgumentNullException();
            }

            ITypeSymbol type;
            if (this.TypesByName.TryGetValue(name, out type))
            {
                return type;
            }

            Debug.Assert(false, string.Format("Type '{0}' can't be found", name));

            return null;
        }
    }
}

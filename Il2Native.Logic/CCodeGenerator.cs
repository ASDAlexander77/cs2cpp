namespace Il2Native.Logic
{
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using DOM;
    using Microsoft.CodeAnalysis;

    public class CCodeGenerator
    {
        private readonly IList<CCodeUnit> _cunits = new List<CCodeUnit>();

        public CCodeGenerator(IAssemblySymbol assembly)
        {
            this.Assembly = assembly;
        }

        protected IAssemblySymbol Assembly { get; set; }

        public IList<CCodeUnit> Build()
        {
            foreach (var type in this.Assembly.Modules.SelectMany(module => module.EnumAllTypes()))
            {
                this._cunits.Add(BuildUnit(type));
            }

            return this._cunits;
        }

        private static CCodeUnit BuildUnit(INamespaceOrTypeSymbol type)
        {
            var unit = new CCodeUnit(type);
            foreach (var method in type.GetMembers().OfType<IMethodSymbol>())
            {
                unit.Declarations.Add(new CCodeMethodDeclaration(method));
                unit.Definitions.Add(new CCodeMethodDefinition(method));
            }

            return unit;
        }
    }
}

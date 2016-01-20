namespace Il2Native.Logic
{
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using DOM;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class CCodeUnitsBuilder
    {
        private readonly IList<CCodeUnit> _cunits = new List<CCodeUnit>();

        internal CCodeUnitsBuilder(IAssemblySymbol assembly, IDictionary<string, BoundStatement> boundBodyByMethodSymbol)
        {
            this.Assembly = assembly;
            this.BoundBodyByMethodSymbol = boundBodyByMethodSymbol;
        }

        internal IDictionary<string, BoundStatement> BoundBodyByMethodSymbol { get; set; }

        protected IAssemblySymbol Assembly { get; set; }
       
        public IList<CCodeUnit> Build()
        {
            foreach (var type in this.Assembly.Modules.SelectMany(module => module.EnumAllTypes()).Where(t => t.ContainingType == null || !t.ContainingType.Name.Contains("<PrivateImplementationDetails>")))
            {
                this._cunits.Add(BuildUnit(type));
            }

            return this._cunits;
        }

        private CCodeUnit BuildUnit(ITypeSymbol type)
        {
            var unit = new CCodeUnit(type);
            foreach (var method in type.GetMembers().OfType<IMethodSymbol>())
            {
                unit.Declarations.Add(new CCodeMethodDeclaration(method));
                unit.Definitions.Add(new CCodeMethodDefinition(method, this.BoundBodyByMethodSymbol[method.ToString()]));
            }

            return unit;
        }
    }
}

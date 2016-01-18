namespace Il2Native.Logic
{
    using System.Collections.Generic;
    using System.Linq;

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
            foreach (var type in this.Assembly.Modules.Cast<IModuleSymbol>().SelectMany(module => module.EnumAllTypes()))
            {
                this._cunits.Add(new CCodeUnit(type.Name, type.ContainingNamespace) { Name = type.Name });
            }

            return this._cunits;
        }
    }
}

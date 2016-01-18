namespace Il2Native.Logic
{
    using System.Collections.Generic;

    public class CCodeUnit
    {
        public CCodeUnit()
        {
            this.Declarations = new List<CCodeDeclaration>();
            this.Definitions = new List<CCodeDefinition>();
        }

        public IList<CCodeDeclaration> Declarations { get; private set; }

        public IList<CCodeDefinition> Definitions { get; private set; }
    }
}

namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;

    using Il2Native.Logic.DOM;

    using Microsoft.CodeAnalysis;

    public class CCodeSerializer
    {
        private string currentFolder;

        public void WriteTo(AssemblyIdentity identity, IList<CCodeUnit> units, string outputFolder)
        {
            if (!Directory.Exists(identity.Name))
            {
                Directory.CreateDirectory(identity.Name);
            }

            this.currentFolder = Path.Combine(outputFolder, identity.Name);

            foreach (var unit in units)
            {
                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetUnitPath(unit))))
                {
                    unit.WriteTo(itw);
                }
            }
        }

        private string GetUnitPath(CCodeUnit unit)
        {
            return Path.Combine(this.currentFolder, unit.Namespace.Replace(".", "\\"), string.Concat(unit.Name, ".cpp"));
        }
    }
}

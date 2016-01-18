namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DOM;
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
                int nestedLevel;
                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetUnitPath(unit, out nestedLevel))))
                {
                    foreach (var definition in unit.Definitions)
                    {
                        definition.WriteTo(itw);
                    }
                }
            }
        }

        private string GetUnitPath(CCodeUnit unit, out int nestedLevel)
        {
            var enumNamespaces = unit.Type.ContainingNamespace.EnumNamespaces().ToList();
            nestedLevel = enumNamespaces.Count();
            var fullDirPath = Path.Combine(this.currentFolder, string.Join("\\", enumNamespaces.Select(n => n.ToString().CleanUpNameAllUnderscore())));
            var fullPath = Path.Combine(fullDirPath, string.Concat(unit.Type.MetadataName.CleanUpNameAllUnderscore(), ".cpp"));
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            return fullPath;
        }
    }
}

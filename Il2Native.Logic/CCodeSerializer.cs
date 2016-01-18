namespace Il2Native.Logic
{
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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
            var enumNamespaces = this.EnumNamespaces(unit.Namespace).ToList();
            nestedLevel = enumNamespaces.Count();
            var fullDirPath = Path.Combine(this.currentFolder, string.Join("\\", enumNamespaces.Select(n => n.CleanUpNameAllUnderscore())));
            var fullPath = Path.Combine(fullDirPath, string.Concat(unit.Name.CleanUpNameAllUnderscore(), ".cpp"));
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            return fullPath;
        }

        private IEnumerable<string> EnumNamespaces(INamespaceSymbol namespaceSymbol)
        {
            if (namespaceSymbol == null)
            {
                yield break;
            }

            if (namespaceSymbol.ContainingNamespace != null)
            {
                foreach (var enumNamespace in this.EnumNamespaces(namespaceSymbol.ContainingNamespace))
                {
                    yield return enumNamespace;
                }
            }

            yield return namespaceSymbol.ToString();
        }
    }
}

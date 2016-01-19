namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata.Ecma335;
    using DOM;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;

    public class CCodeSerializer
    {
        private string currentFolder;

        public static void WriteNamespace(IndentedTextWriter itw, INamespaceSymbol namespaceSymbol)
        {
            foreach (var namespaceNode in namespaceSymbol.EnumNamespaces())
            {
                itw.Write("::");
                itw.Write(namespaceNode.MetadataName);
            }
        }

        public static void WriteName(IndentedTextWriter itw, ISymbol symbol)
        {
            itw.Write(symbol.MetadataName.CleanUpName());
        }

        public static void WriteMethodDeclaration(IndentedTextWriter itw, WriteSettings settings, IMethodSymbol methodSymbol, bool nameOnly)
        {
            // type
            if (methodSymbol.ReturnsVoid)
            {
                itw.Write("void");
            }
            else
            {
                new CCodeType(methodSymbol.ReturnType).WriteTo(itw, settings);
            }

            itw.Write(" ");

            if (settings == WriteSettings.Token)
            {
                // Token
                var peMethodSymbol = methodSymbol as PEMethodSymbol;
                Debug.Assert(peMethodSymbol != null);
                if (peMethodSymbol != null)
                {
                    var token = MetadataTokens.GetToken(peMethodSymbol.Handle);
                    itw.Write("T{0:X}", token);
                }
            }
            else
            {
                if (!nameOnly)
                {
                    WriteNamespace(itw, methodSymbol.ContainingNamespace);
                    itw.Write("::");
                    WriteName(itw, methodSymbol.ReceiverType);
                    itw.Write("::");
                }

                WriteName(itw, methodSymbol);
            }

            itw.Write("(");
            // parameters
            itw.Write(")");

            // post attributes
            // TODO:
        }

        internal static void WriteMethodBody(IndentedTextWriter itw, BoundStatementList boundBody)
        {
            itw.WriteLine();
            itw.WriteLine("{");
            itw.Indent++;

            itw.WriteLine("// Body");

            new CCodeMethodSerializer(itw).Serialize(boundBody);

            itw.Indent--;
            itw.WriteLine("}");
        }

        public void WriteTo(AssemblyIdentity identity, IList<CCodeUnit> units, string outputFolder, WriteSettings settings)
        {
            if (!Directory.Exists(identity.Name))
            {
                Directory.CreateDirectory(identity.Name);
            }

            this.currentFolder = Path.Combine(outputFolder, identity.Name);

            // write header
            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath(identity.Name))))
            {
                itw.WriteLine("#include <cinttypes>");
                itw.WriteLine();

                foreach (var unit in units)
                {
                    var any = false;
                    foreach (var namespaceNode in unit.Type.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Write("namespace ");
                        itw.Write(namespaceNode.MetadataName);
                        itw.Write(" { ");
                        any = true;
                    }

                    if (any)
                    {
                        itw.Indent++;
                        itw.WriteLine();
                    }

                    foreach (var declaration in unit.Declarations)
                    {
                        declaration.WriteTo(itw, settings);
                    }

                    if (any)
                    {
                        itw.Indent--;
                        itw.WriteLine();
                    }

                    foreach (var namespaceNode in unit.Type.ContainingNamespace.EnumNamespaces())
                    {
                        itw.Write("}");
                    }
                }

                itw.Close();
            }

            // write all sources
            foreach (var unit in units)
            {
                int nestedLevel;
                using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath(unit, out nestedLevel))))
                {
                    itw.Write("#include \"");
                    for (int i = 0; i < nestedLevel; i++)
                    {
                        itw.Write("..\\");
                    }

                    itw.WriteLine("{0}.h\"", identity.Name);

                    foreach (var definition in unit.Definitions)
                    {
                        definition.WriteTo(itw, settings);
                    }

                    itw.Close();
                }
            }
        }

        private string GetPath(string name)
        {
            var fullDirPath = this.currentFolder;
            var fullPath = Path.Combine(fullDirPath, String.Concat(name, ".h"));
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            return fullPath;
        }

        private string GetPath(CCodeUnit unit, out int nestedLevel)
        {
            var enumNamespaces = unit.Type.ContainingNamespace.EnumNamespaces().ToList();
            nestedLevel = enumNamespaces.Count();
            var fullDirPath = Path.Combine(this.currentFolder, String.Join("\\", enumNamespaces.Select(n => n.ToString().CleanUpNameAllUnderscore())));
            var fullPath = Path.Combine(fullDirPath, String.Concat(unit.Type.MetadataName.CleanUpNameAllUnderscore(), ".cpp"));
            return fullPath;
        }
    }
}

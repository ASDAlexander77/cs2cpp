// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Cs2CGenerator.cs" company="">
// </copyright>
// <summary>
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace Il2Native.Logic
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Emit;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;

    /// <summary>
    /// </summary>
    public class Cs2CGenerator
    {
        private readonly IDictionary<AssemblyIdentity, AssemblySymbol> cache = new Dictionary<AssemblyIdentity, AssemblySymbol>();

        private readonly IList<UnifiedAssembly<AssemblySymbol>> unifiedAssemblies = new List<UnifiedAssembly<AssemblySymbol>>();

        private readonly IDictionary<string, BoundStatement> boundBodyByMethodSymbol = new ConcurrentDictionary<string, BoundStatement>();

        private readonly IDictionary<string, SourceMethodSymbol> sourceMethodByMethodSymbol = new ConcurrentDictionary<string, SourceMethodSymbol>();

        /// <summary>
        /// </summary>
        public Cs2CGenerator()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="source">
        /// </param>
        /// <param name="args">
        /// </param>
        public Cs2CGenerator(string[] source, string[] args)
            : this()
        {
            this.Sources = source;
            this.FirstSource = this.Sources.First();
            if (this.FirstSource.EndsWith(".csproj"))
            {
                this.LoadProject(this.FirstSource);
            }
            else
            {
                var coreLibPathArg = args != null ? args.FirstOrDefault(a => a.StartsWith("corelib:")) : null;
                this.CoreLibPath = coreLibPathArg != null ? coreLibPathArg.Substring("corelib:".Length) : null;
                this.DefaultDllLocations = Path.GetDirectoryName(Path.GetFullPath(this.FirstSource));
                this.DebugInfo = args != null && args.Contains("debug");
                this.Options = new ProjectProperties();
            }
        }

        private void LoadProject(string firstSource)
        {
            var folder = Path.GetDirectoryName(firstSource);
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            var project = XDocument.Load(firstSource);
            this.Sources =
                project.Root.Elements(ns + "ItemGroup").Elements(ns + "Compile")
                    .Select(element => Path.Combine(folder, element.Attribute("Include").Value))
                    .ToArray();

            var options = new ProjectProperties();
            foreach (var property in project.Root.Elements(ns + "PropertyGroup").Elements())
            {
                options[property.Name.LocalName] = property.Value;
            }

            this.Options = options;
        }

        /// <summary>
        /// </summary>
        public string CoreLibPath { get; set; }

        public bool DebugInfo { get; private set; }

        /// <summary>
        /// </summary>
        public string[] ReferencesList { get; set; }

        /// <summary>
        /// </summary>
        public string DefaultDllLocations { get; private set; }

        /// <summary>
        /// </summary>
        public string SourceFilePath { get; private set; }

        internal IDictionary<string, BoundStatement> BoundBodyByMethodSymbol
        {
            get { return this.boundBodyByMethodSymbol; }
        }

        internal IDictionary<string, SourceMethodSymbol> SourceMethodByMethodSymbol
        {
            get { return this.sourceMethodByMethodSymbol; }
        }

        /// <summary>
        /// </summary>
        protected string FirstSource { get; private set; }

        /// <summary>
        /// </summary>
        protected string[] Sources { get; private set; }

        /// <summary>
        /// </summary>
        protected IDictionary<string, string> Options { get; private set; }

        /// <summary>
        /// A verbose format for displaying symbols (useful for testing).
        /// </summary>
        internal static readonly SymbolDisplayFormat KeyStringFormat =
            new SymbolDisplayFormat(
                globalNamespaceStyle: SymbolDisplayGlobalNamespaceStyle.OmittedAsContaining,
                typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
                propertyStyle: SymbolDisplayPropertyStyle.ShowReadWriteDescriptor,
                localOptions: SymbolDisplayLocalOptions.IncludeType,
                genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters | SymbolDisplayGenericsOptions.IncludeVariance,
                memberOptions:
                    SymbolDisplayMemberOptions.IncludeParameters |
                    SymbolDisplayMemberOptions.IncludeContainingType |
                    SymbolDisplayMemberOptions.IncludeType |
                    SymbolDisplayMemberOptions.IncludeExplicitInterface,
                kindOptions:
                    SymbolDisplayKindOptions.IncludeMemberKeyword,
                parameterOptions:
                    SymbolDisplayParameterOptions.IncludeOptionalBrackets |
                    SymbolDisplayParameterOptions.IncludeDefaultValue |
                    SymbolDisplayParameterOptions.IncludeParamsRefOut |
                    SymbolDisplayParameterOptions.IncludeExtensionThis |
                    SymbolDisplayParameterOptions.IncludeType,
                miscellaneousOptions:
                    SymbolDisplayMiscellaneousOptions.EscapeKeywordIdentifiers |
                    SymbolDisplayMiscellaneousOptions.UseErrorTypeSymbolName,
                compilerInternalOptions:
                    SymbolDisplayCompilerInternalOptions.IncludeScriptType |
                    SymbolDisplayCompilerInternalOptions.UseMetadataMethodNames |
                    SymbolDisplayCompilerInternalOptions.FlagMissingMetadataTypes |
                    SymbolDisplayCompilerInternalOptions.IncludeCustomModifiers);

        public IAssemblySymbol Load()
        {
            var assemblyMetadata = this.CompileWithRoslynInMemory(this.Sources);
            return LoadAssemblySymbol(assemblyMetadata);
        }

        private AssemblyMetadata CompileWithRoslynInMemory(string[] source)
        {
            var srcFileName = Path.GetFileNameWithoutExtension(this.FirstSource);
            var assemblyName = srcFileName;

            var defineSeparators = new[] { ';', ' ' };
            var syntaxTrees =
                source.Select(
                    s =>
                        CSharpSyntaxTree.ParseText(
                        SourceText.From(new FileStream(s, FileMode.Open, FileAccess.Read), Encoding.UTF8),
                            new CSharpParseOptions(
                            LanguageVersion.Experimental,
                            preprocessorSymbols: Options["DefineConstants"].Split(defineSeparators, StringSplitOptions.RemoveEmptyEntries)),
                            s));

            var assemblies = new List<MetadataImageReference>();

            this.LoadReferencesForCompiling(assemblies);

            var options =
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithAllowUnsafe(true)
                                                                                 .WithOptimizations(!this.DebugInfo)
                                                                                 .WithRuntimeMetadataVersion("4.5");

            var compilation = CSharpCompilation.Create(assemblyName, syntaxTrees, assemblies.ToArray(), options);

            var dllStream = new MemoryStream();
            var pdbStream = new MemoryStream();

            PEModuleBuilder.OnMethodBoundBodySynthesizedDelegate peModuleBuilderOnOnMethodBoundBodySynthesized = (symbol, body) =>
            {
                var key = symbol.ToDisplayString(KeyStringFormat);
                Debug.Assert(!boundBodyByMethodSymbol.ContainsKey(key), "Check if method is partial");
                boundBodyByMethodSymbol[key] = body;
            };

            PEModuleBuilder.OnSourceMethodDelegate peModuleBuilderOnSourceMethod = (symbol, sourceMethod) =>
            {
                var key = symbol.ToDisplayString(KeyStringFormat);
                Debug.Assert(!sourceMethodByMethodSymbol.ContainsKey(key), "Check if method is partial");
                sourceMethodByMethodSymbol[key] = sourceMethod;
            };

            PEModuleBuilder.OnMethodBoundBodySynthesized += peModuleBuilderOnOnMethodBoundBodySynthesized;
            PEModuleBuilder.OnSourceMethod += peModuleBuilderOnSourceMethod;

            var result = compilation.Emit(peStream: dllStream, pdbStream: pdbStream);

            PEModuleBuilder.OnMethodBoundBodySynthesized -= peModuleBuilderOnOnMethodBoundBodySynthesized;
            PEModuleBuilder.OnSourceMethod -= peModuleBuilderOnSourceMethod;

            if (result.Diagnostics.Length > 0)
            {
                var errors = result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
                Console.WriteLine(@"Errors: {0}", errors.Count);
                foreach (var diagnostic in errors)
                {
                    Console.WriteLine(diagnostic);
                }

                var diagnostics = result.Diagnostics.Where(d => d.Severity != DiagnosticSeverity.Error);
                Console.WriteLine(@"Warnings/Info: {0}", diagnostics.Count());
                foreach (var diagnostic in diagnostics)
                {
                    Console.WriteLine(diagnostic);
                }
            }

            dllStream.Flush();
            dllStream.Position = 0;

            pdbStream.Flush();
            pdbStream.Position = 0;

            // Successful Compile
            return AssemblyMetadata.CreateFromImageStream(dllStream);
        }

        private void LoadReferencesForCompiling(List<MetadataImageReference> assemblies)
        {
            var added = new HashSet<AssemblyIdentity>();

            if (this.Options["NoStdLib"] != "true")
            {
                this.AddAsseblyReference(
                    assemblies,
                    added,
                    !string.IsNullOrWhiteSpace(this.CoreLibPath) ? this.CoreLibPath : typeof(int).Assembly.Location);
            }

            if (this.ReferencesList != null)
            {
                foreach (var refItem in this.ReferencesList)
                {
                    if (File.Exists(refItem))
                    {
                        AddAsseblyReference(assemblies, added, refItem);
                    }
                    else
                    {
                        this.AddAsseblyReference(assemblies, added, new AssemblyIdentity(refItem));
                    }
                }
            }
        }

        private void AddAsseblyReference(List<MetadataImageReference> assemblies, HashSet<AssemblyIdentity> added, AssemblyIdentity assemblyIdentity)
        {
            var resolvedFilePath = this.ResolveReferencePath(assemblyIdentity);
            if (resolvedFilePath == null)
            {
                return;
            }

            var metadata = AssemblyMetadata.CreateFromImageStream(new FileStream(resolvedFilePath, FileMode.Open, FileAccess.Read));
            if (added.Add(metadata.Assembly.Identity))
            {
                var metadataImageReference = new MetadataImageReference(new FileStream(resolvedFilePath, FileMode.Open, FileAccess.Read));
                assemblies.Add(metadataImageReference);

                // process nested
                foreach (var refAssemblyIdentity in metadata.Assembly.AssemblyReferences)
                {
                    AddAsseblyReference(assemblies, added, refAssemblyIdentity);
                }
            }
        }

        private void AddAsseblyReference(List<MetadataImageReference> assemblies, HashSet<AssemblyIdentity> added, string resolvedFilePath)
        {
            var metadata = AssemblyMetadata.CreateFromImageStream(new FileStream(resolvedFilePath, FileMode.Open, FileAccess.Read));
            var metadataImageReference = new MetadataImageReference(new FileStream(resolvedFilePath, FileMode.Open, FileAccess.Read));

            if (added.Add(metadata.Assembly.Identity))
            {
                assemblies.Add(metadataImageReference);

                // process nested
                foreach (var refAssemblyIdentity in metadata.Assembly.AssemblyReferences)
                {
                    AddAsseblyReference(assemblies, added, refAssemblyIdentity);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyIdentity">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblyMetadata GetAssemblyMetadata(AssemblyIdentity assemblyIdentity)
        {
            var resolveReferencePath = this.ResolveReferencePath(assemblyIdentity);
            if (string.IsNullOrWhiteSpace(resolveReferencePath))
            {
                return null;
            }

            return AssemblyMetadata.CreateFromImageStream(new FileStream(resolveReferencePath, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblySymbol LoadAssemblySymbol(AssemblyIdentity identity)
        {
            AssemblySymbol symbol;
            if (this.cache.TryGetValue(identity, out symbol))
            {
                return symbol;
            }

            var assemblyMetadata = this.GetAssemblyMetadata(identity);
            if (assemblyMetadata != null)
            {
                return this.LoadAssemblySymbol(assemblyMetadata);
            }

            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyMetadata">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblySymbol LoadAssemblySymbol(AssemblyMetadata assemblyMetadata, bool noCache = false)
        {
            AssemblySymbol symbol;
            if (!noCache && this.cache.TryGetValue(assemblyMetadata.Assembly.Identity, out symbol))
            {
                return symbol;
            }

            var assemblySymbol = new PEAssemblySymbol(assemblyMetadata.Assembly, DocumentationProvider.Default, isLinked: false, importOptions: MetadataImportOptions.All);

            this.cache[assemblyMetadata.Assembly.Identity] = assemblySymbol;
            this.unifiedAssemblies.Add(new UnifiedAssembly<AssemblySymbol>(assemblySymbol, assemblyMetadata.Assembly.Identity));

            var moduleReferences = this.LoadReferences(assemblyMetadata);
            foreach (var module in assemblySymbol.Modules)
            {
                module.SetReferences(moduleReferences);
            }

            this.SetCorLib(assemblySymbol);

            return assemblySymbol;
        }

        /// <summary>
        /// </summary>
        /// <param name="identity">
        /// </param>
        /// <returns>
        /// </returns>
        private AssemblySymbol LoadAssemblySymbolOrMissingAssemblySymbol(AssemblyIdentity identity)
        {
            AssemblySymbol symbol;
            if (this.cache.TryGetValue(identity, out symbol))
            {
                return symbol;
            }

            var peAssemblySymbol = this.LoadAssemblySymbol(identity);
            if (peAssemblySymbol != null)
            {
                return peAssemblySymbol;
            }

            return new MissingAssemblySymbol(identity);
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyMetadata">
        /// </param>
        /// <returns>
        /// </returns>
        private ModuleReferences<AssemblySymbol> LoadReferences(AssemblyMetadata assemblyMetadata)
        {
            var peReferences = ImmutableArray.CreateRange(assemblyMetadata.Assembly.AssemblyReferences.Select(this.LoadAssemblySymbolOrMissingAssemblySymbol));

            var moduleReferences = new ModuleReferences<AssemblySymbol>(
                assemblyMetadata.Assembly.AssemblyReferences, peReferences, ImmutableArray.CreateRange(this.unifiedAssemblies));

            return moduleReferences;
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyIdentity">
        /// </param>
        /// <returns>
        /// </returns>
        private string ResolveReferencePath(AssemblyIdentity assemblyIdentity)
        {
            var dllFileName = string.Concat(assemblyIdentity.Name, ".dll");
            if (File.Exists(dllFileName))
            {
                return Path.GetFullPath(dllFileName);
            }

            if (!string.IsNullOrWhiteSpace(this.DefaultDllLocations))
            {
                var dllFullName = Path.Combine(this.DefaultDllLocations, dllFileName);
                if (File.Exists(dllFullName))
                {
                    return dllFullName;
                }
            }

            var windir = Environment.GetEnvironmentVariable("windir");

            var dllFullNameGAC = Path.Combine(windir, string.Format(@"Microsoft.NET\assembly\GAC_MSIL\{0}\{1}_{2}_{3}_{4}", assemblyIdentity.Name, "4.0", assemblyIdentity.Version, assemblyIdentity.CultureName, GetAssemblyHashString(assemblyIdentity)));
            if (File.Exists(dllFullNameGAC))
            {
                return dllFullNameGAC;
            }

            // find first possible
            var dllFullNameGACSearch = Path.Combine(windir, string.Format(@"Microsoft.NET\assembly\GAC_MSIL\{0}", assemblyIdentity.Name));
            try
            {
                foreach (var dll in Directory.EnumerateFiles(dllFullNameGACSearch, "*.dll", SearchOption.AllDirectories))
                {
                    // TODO: filter it here
                    return dll;
                }
            }
            catch (Exception)
            {
            }

            if (assemblyIdentity.Name == "CoreLib")
            {
                return this.CoreLibPath;
            }

            if (assemblyIdentity.Name == "mscorlib")
            {
                if (!string.IsNullOrWhiteSpace(this.CoreLibPath))
                {
                    return this.CoreLibPath;
                }

                Debug.Assert(false, "you are using mscorlib from .NET");

                return typeof(int).Assembly.Location;
            }

            Debug.Fail("Not implemented yet");

            return null;
        }

        private static object GetAssemblyHashString(AssemblyIdentity assemblyIdentity)
        {
            if (!assemblyIdentity.HasPublicKey)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var @byte in assemblyIdentity.PublicKey)
            {
                sb.AppendFormat("{0:N}", @byte);
            }

            return sb.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblySymbol">
        /// </param>
        private void SetCorLib(PEAssemblySymbol assemblySymbol)
        {
            if (!assemblySymbol.Assembly.AssemblyReferences.Any())
            {
                // this is the core lib
                assemblySymbol.SetCorLibrary(assemblySymbol);
                return;
            }

            if (SetCorLib(assemblySymbol, assemblySymbol))
            {
                return;
            }

            Debug.Fail("CoreLib not set");
        }

        private bool SetCorLib(PEAssemblySymbol assemblySymbol, PEAssemblySymbol fromAssemblySymbol)
        {
            var loadedRefAssemblies = from assemblyIdentity in fromAssemblySymbol.Assembly.AssemblyReferences select this.LoadAssemblySymbol(assemblyIdentity);
            foreach (var loadedRefAssemblySymbol in loadedRefAssemblies)
            {
                var peRefAssembly = loadedRefAssemblySymbol as PEAssemblySymbol;
                if (peRefAssembly != null && !peRefAssembly.Assembly.AssemblyReferences.Any())
                {
                    assemblySymbol.SetCorLibrary(loadedRefAssemblySymbol);
                    return true;
                }
            }

            foreach (var loadedRefAssemblySymbol in loadedRefAssemblies)
            {
                if (this.SetCorLib(assemblySymbol, loadedRefAssemblySymbol as PEAssemblySymbol))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
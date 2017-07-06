// Mr Oleksandr Duzhar licenses this file to you under the MIT license.
// If you need the License file, please send an email to duzhar@googlemail.com
// 
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
    using System.Xml.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Emit;
    using Microsoft.CodeAnalysis.CSharp.Symbols;
    using Microsoft.CodeAnalysis.CSharp.Symbols.Metadata.PE;
    using Microsoft.CodeAnalysis.Text;

    using Il2Native.Logic.Project;

    /// <summary>
    /// </summary>
    public class Cs2CGenerator
    {
        private readonly IDictionary<string, BoundStatement> boundBodyByMethodSymbol = new ConcurrentDictionary<string, BoundStatement>();

        private readonly IDictionary<AssemblyIdentity, AssemblySymbol> cache = new Dictionary<AssemblyIdentity, AssemblySymbol>();

        private readonly IDictionary<string, SourceMethodSymbol> sourceMethodByMethodSymbol = new ConcurrentDictionary<string, SourceMethodSymbol>();

        private readonly IList<UnifiedAssembly<AssemblySymbol>> unifiedAssemblies = new List<UnifiedAssembly<AssemblySymbol>>();

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

            DebugOutput = true;
            this.Options = new ProjectProperties();
            if (args != null && args.Contains("release"))
            {
                this.Options["Configuration"] = "Release";
                DebugOutput = false;
            }

            if (this.FirstSource.EndsWith(".csproj"))
            {
                this.LoadProject(this.FirstSource);
            }
            else
            {
                var coreLibPathArg = args != null ? args.FirstOrDefault(a => a.StartsWith("corelib:")) : null;
                this.CoreLibPath = coreLibPathArg != null ? coreLibPathArg.Substring("corelib:".Length) : null;
                this.DefaultDllLocations = Path.GetDirectoryName(Path.GetFullPath(this.FirstSource));
                this.References = args != null ? args.Where(a => a.StartsWith("ref:")).Select(a => a.Substring("ref:".Length)).ToArray() : null;
            }
        }

        /// <summary>
        /// </summary>
        public static bool DebugOutput { get; set; }

        /// <summary>
        /// </summary>
        public ISet<AssemblyIdentity> Assemblies { get; private set; }

        /// <summary>
        /// </summary>
        public string CoreLibPath { get; set; }

        /// <summary>
        /// </summary>
        public string DefaultDllLocations { get; private set; }

        public IDictionary<string, string> Options { get; private set; }

        public bool IsCoreLib
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.CoreLibPath) && (this.References == null || this.References.Length == 0);
            }
        }

        public bool IsLibrary
        {
            get
            {
                return this.Options.ContainsKey("OutputType") && this.Options["OutputType"] == "Library";
            }
        }

        /// <summary>
        /// </summary>
        public string[] References { get; set; }

        /// <summary>
        /// </summary>
        public string[] Errors { get; set; }

        /// <summary>
        /// </summary>
        public string SourceFilePath { get; private set; }

        /// <summary>
        /// C++ files which contain implementations
        /// </summary>
        public string[] Impl { get; private set; }

        /// <summary>
        /// </summary>
        protected string FirstSource { get; private set; }

        /// <summary>
        /// </summary>
        protected string[] Sources { get; private set; }

        internal IDictionary<string, BoundStatement> BoundBodyByMethodSymbol
        {
            get { return this.boundBodyByMethodSymbol; }
        }

        internal IDictionary<string, SourceMethodSymbol> SourceMethodByMethodSymbol
        {
            get { return this.sourceMethodByMethodSymbol; }
        }

        public IAssemblySymbol Load()
        {
            if (this.Errors != null && this.Errors.Any())
            {
                return null;
            }

            var assemblyMetadata = this.CompileWithRoslynInMemory(this.Sources);
            if (assemblyMetadata == null)
            {
                return null;
            }

            return this.LoadAssemblySymbol(assemblyMetadata);
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

        private void AddAsseblyReference(List<MetadataImageReference> assemblies, HashSet<AssemblyIdentity> added, AssemblyIdentity assemblyIdentity)
        {
            if (added.Contains(assemblyIdentity))
            {
                return;
            }

            var resolvedFilePath = this.ResolveReferencePath(assemblyIdentity);
            if (resolvedFilePath == null)
            {
                return;
            }

            var metadata = AssemblyMetadata.CreateFromStream(new FileStream(resolvedFilePath, FileMode.Open, FileAccess.Read));
            if (added.Add(metadata.GetAssembly().Identity))
            {
                assemblies.Add(new MetadataImageReference(metadata, new MetadataReferenceProperties(), null, resolvedFilePath, null));

                this.LoadAssemblySymbol(metadata, true);

                // process nested
                foreach (var refAssemblyIdentity in metadata.GetAssembly().AssemblyReferences)
                {
                    this.AddAsseblyReference(assemblies, added, refAssemblyIdentity);
                }
            }
        }

        private void AddAsseblyReference(List<MetadataImageReference> assemblies, HashSet<AssemblyIdentity> added, string resolvedFilePath)
        {
            var metadata = AssemblyMetadata.CreateFromStream(new FileStream(resolvedFilePath, FileMode.Open, FileAccess.Read));

            if (added.Add(metadata.GetAssembly().Identity))
            {
                assemblies.Add(new MetadataImageReference(metadata, new MetadataReferenceProperties(), null, resolvedFilePath, null));

                this.LoadAssemblySymbol(metadata, true);

                // process nested
                foreach (var refAssemblyIdentity in metadata.GetAssembly().AssemblyReferences)
                {
                    this.AddAsseblyReference(assemblies, added, refAssemblyIdentity);
                }
            }
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
                            LanguageVersion.Latest,
                            preprocessorSymbols: (this.Options["DefineConstants"] ?? string.Empty).Split(defineSeparators, StringSplitOptions.RemoveEmptyEntries)),
                            s));

            var assemblies = new List<MetadataImageReference>();

            this.Assemblies = this.LoadReferencesForCompiling(assemblies);

            var options =
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithAllowUnsafe(true)
                                                                                 .WithOptimizationLevel(DebugOutput ? OptimizationLevel.Debug : OptimizationLevel.Release)
                                                                                 .WithConcurrentBuild(true);

            var compilation = CSharpCompilation.Create(assemblyName, syntaxTrees, assemblies.ToArray(), options);

            var dllStream = new MemoryStream();
            var pdbStream = new MemoryStream();

            PEModuleBuilder.OnMethodBoundBodySynthesizedDelegate peModuleBuilderOnOnMethodBoundBodySynthesized = (symbol, body) =>
            {
                var key = symbol.ToKeyString();
                Debug.Assert(!this.boundBodyByMethodSymbol.ContainsKey(key), "Check if method is partial");
                this.boundBodyByMethodSymbol[key] = body;
            };

            PEModuleBuilder.OnSourceMethodDelegate peModuleBuilderOnSourceMethod = (symbol, sourceMethod) =>
            {
                var key = symbol.ToKeyString();
                Debug.Assert(!this.sourceMethodByMethodSymbol.ContainsKey(key), "Check if method is partial");
                this.sourceMethodByMethodSymbol[key] = sourceMethod;
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

                if (errors.Count > 0)
                {
                    return null;
                }
            }

            dllStream.Flush();
            dllStream.Position = 0;

            pdbStream.Flush();
            pdbStream.Position = 0;

            // Successful Compile
            return AssemblyMetadata.CreateFromStream(dllStream, false);
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

            return AssemblyMetadata.CreateFromStream(new FileStream(resolveReferencePath, FileMode.Open, FileAccess.Read));
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
            if (!noCache && this.cache.TryGetValue(assemblyMetadata.GetAssembly().Identity, out symbol))
            {
                return symbol;
            }

            var assemblySymbol = new PEAssemblySymbol(assemblyMetadata.GetAssembly(), DocumentationProvider.Default, isLinked: false, importOptions: MetadataImportOptions.All);

            this.cache[assemblyMetadata.GetAssembly().Identity] = assemblySymbol;
            this.unifiedAssemblies.Add(new UnifiedAssembly<AssemblySymbol>(assemblySymbol, assemblyMetadata.GetAssembly().Identity));

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

        private void LoadProject(string firstSource)
        {
            var projectLoader = new ProjectLoader(this.Options);
            if (!projectLoader.Load(firstSource))
            {
                this.Errors = projectLoader.Errors.ToArray();

                Console.WriteLine(@"Project Errors: {0}", this.Errors.Count());
                foreach (var diagnostic in this.Errors)
                {
                    Console.WriteLine(diagnostic);
                }

                this.Sources = new string[0];
                this.Impl = new string[0];
                this.References = new string[0];

                return;
            }

            DebugOutput = this.Options["Configuration"] != "Release";

            this.Sources = projectLoader.Sources.ToArray();
            this.Impl = projectLoader.Content.Where(c => c.EndsWith(".c") || c.EndsWith(".cpp") || c.EndsWith(".cxx") || c.EndsWith(".h") || c.EndsWith(".hpp") || c.EndsWith(".hxx")).ToArray();
            this.References = projectLoader.References.ToArray();
        }

        /// <summary>
        /// </summary>
        /// <param name="assemblyMetadata">
        /// </param>
        /// <returns>
        /// </returns>
        private ModuleReferences<AssemblySymbol> LoadReferences(AssemblyMetadata assemblyMetadata)
        {
            var peReferences = ImmutableArray.CreateRange(assemblyMetadata.GetAssembly().AssemblyReferences.Select(this.LoadAssemblySymbolOrMissingAssemblySymbol));

            var moduleReferences = new ModuleReferences<AssemblySymbol>(
                assemblyMetadata.GetAssembly().AssemblyReferences, peReferences, ImmutableArray.CreateRange(this.unifiedAssemblies));

            return moduleReferences;
        }

        private HashSet<AssemblyIdentity> LoadReferencesForCompiling(List<MetadataImageReference> assemblies)
        {
            var added = new HashSet<AssemblyIdentity>();
            if (this.References != null)
            {
                foreach (var refItem in this.References)
                {
                    if (File.Exists(refItem))
                    {
                        this.AddAsseblyReference(assemblies, added, refItem);
                    }
                    else
                    {
                        this.AddAsseblyReference(assemblies, added, new AssemblyIdentity(refItem));
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(this.CoreLibPath))
            {
                this.AddAsseblyReference(assemblies, added, this.CoreLibPath);
            }

            return added;
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

            if (this.SetCorLib(assemblySymbol, assemblySymbol))
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
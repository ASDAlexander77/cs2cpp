namespace Il2Native.Logic
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DOM;
    using DOM.Implementations;
    using DOM.Synthesized;
    using DOM2;
    using Il2Native.Logic.Properties;

    using Microsoft.CodeAnalysis;

    using Roslyn.Utilities;

    public class CCodeFilesGenerator
    {
        private string currentFolder;

        public bool Concurrent { get; set; }

        public void WriteTo(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, IEnumerable<CCodeUnit> units, string outputFolder)
        {
            this.currentFolder = Path.Combine(outputFolder, identity.Name);
            if (!Directory.Exists(this.currentFolder))
            {
                Directory.CreateDirectory(this.currentFolder);
            }

            var includeHeaders = this.WriteTemplateSources(units).Union(this.WriteTemplateSources(units, true));

            this.WriteHeader(identity, references, isCoreLib, units, includeHeaders);

            this.WriteCoreLibSource(identity, isCoreLib);

            this.WriteSources(identity, units);

            this.WriteBuildFiles(identity, references, !isCoreLib);
        }

        public void WriteBuildFiles(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool executable)
        {
            // CMake file helper
            var cmake = @"cmake_minimum_required (VERSION 2.8.10 FATAL_ERROR)

file(GLOB_RECURSE <%name%>_SRC
    ""./src/*.cpp""
)

file(GLOB_RECURSE <%name%>_IMPL
    ""./impl/*.cpp""
)

include_directories(""./"" ""./src"" ""./impl"" <%include%>)

if (MSVC)
link_directories(""./""<%link_msvc%>)
SET(CMAKE_CXX_FLAGS ""${CMAKE_CXX_FLAGS} /Od /GR- /Zi /EHsc"")
else()
link_directories(""./""<%link_other%>)
SET(CMAKE_CXX_FLAGS ""${CMAKE_CXX_FLAGS} -O0 -g -gdwarf-4 -march=native -std=gnu++14 -fno-rtti -fpermissive"")
endif()

add_<%type%> (<%name%> ""${<%name%>_SRC}"" ""${<%name%>_IMPL}"")

<%libraries%>";

            var targetLinkLibraries = @"
if (MSVC)
target_link_libraries (<%name%> {0})
else()
target_link_libraries (<%name%> {0} ""stdc++"")
endif()";

            var type = executable ? "executable" : "library";
            var include = string.Join(" ", references.Select(a => string.Format("\"../{0}/src\" \"../{0}/impl\"", a.Name.CleanUpNameAllUnderscore())));
            var link_msvc = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_win32_debug\"", a.Name.CleanUpNameAllUnderscore())));
            var link_other = string.Join(" ", references.Select(a => string.Format("\"../{0}/__build_mingw32_debug\"", a.Name.CleanUpNameAllUnderscore())));
            var libraries = string.Format(targetLinkLibraries, string.Join(" ", references.Select(a => string.Format("\"{0}\"", a.Name.CleanUpNameAllUnderscore()))));

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("CMakeLists", ".txt"))))
            {
                itw.Write(
                    cmake.Replace("<%libraries%>", executable ? libraries : string.Empty)
                         .Replace("<%type%>", type)
                         .Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore())
                         .Replace("<%include%>", include)
                         .Replace("<%link_msvc%>", link_msvc)
                         .Replace("<%link_other%>", link_other));
                itw.Close();
            }

            // build mingw32 DEBUG .bat
            var buildMinGw32 = @"md __build_mingw32_debug
cd __build_mingw32_debug
cmake -f .. -G ""MinGW Makefiles"" -DCMAKE_BUILD_TYPE=Debug -Wno-dev
mingw32-make -j 8 2>log";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_mingw32_debug", ".bat"))))
            {
                itw.Write(buildMinGw32.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()));
                itw.Close();
            }

            // build Visual Studio .bat
            var buildVS2015 = @"md __build_win32_debug
cd __build_win32_debug
cmake -f .. -G ""Visual Studio 14"" -Wno-dev
call ""%VS140COMNTOOLS%\..\..\VC\vcvarsall.bat"" x86
MSBuild ALL_BUILD.vcxproj /m:8 /p:Configuration=Debug /p:Platform=""Win32"" /toolsversion:14.0";

            using (var itw = new IndentedTextWriter(new StreamWriter(this.GetPath("build_vs2015_debug", ".bat"))))
            {
                itw.Write(buildVS2015.Replace("<%name%>", identity.Name.CleanUpNameAllUnderscore()));
                itw.Close();
            }
        }

        public IList<string> WriteTemplateSources(IEnumerable<CCodeUnit> units, bool stubs = false)
        {
            var headersToInclude = new List<string>();

            // write all sources
            foreach (var unit in units)
            {
                int nestedLevel;
                var root = !stubs ? "src" : "impl";

                if (stubs)
                {
                    var path = this.GetPath(unit, out nestedLevel, ".h", root, doNotCreateFolder: true);
                    if (File.Exists(path))
                    {
                        headersToInclude.Add(path.Substring(string.Concat(root, "\\").Length + this.currentFolder.Length + 1));

                        // do not overwrite an existing file
                        continue;
                    }
                }

                var anyRecord = false;
                var text = new StringBuilder();
                using (var itw = new IndentedTextWriter(new StringWriter(text)))
                {
                    var c = new CCodeWriterText(itw);
                    foreach (var definition in unit.Definitions.Where(d => d.IsGeneric && d.IsStub == stubs))
                    {
                        anyRecord = true;
                        definition.WriteTo(c);
                    }

                    itw.Close();
                }

                if (anyRecord && text.Length > 0)
                {
                    var path = this.GetPath(unit, out nestedLevel, ".h", root);
                    var newText = text.ToString();

                    headersToInclude.Add(path.Substring(string.Concat(root, "\\").Length + this.currentFolder.Length + 1));

                    if (IsNothingChanged(path, newText))
                    {
                        continue;
                    }
                    
                    using (var textFile = new StreamWriter(path))
                    {
                        textFile.Write(newText);
                    }
                }
            }

            return headersToInclude;
        }

        public void WriteSources(AssemblyIdentity identity, IEnumerable<CCodeUnit> units)
        {
            if (this.Concurrent)
            {
                // write all sources
                Parallel.ForEach(
                    units.Where(unit => !((INamedTypeSymbol)unit.Type).IsGenericType),
                    (unit) =>
                    {
                        this.WriteSource(identity, unit);
                        this.WriteSource(identity, unit, true);
                    });
            }
            else
            {
                // write all sources
                foreach (var unit in units.Where(unit => !((INamedTypeSymbol)unit.Type).IsGenericType))
                {
                    this.WriteSource(identity, unit);
                    this.WriteSource(identity, unit, true);
                }                 
            }
        }

        private void WriteSource(AssemblyIdentity identity, CCodeUnit unit, bool stubs = false)
        {
            int nestedLevel;

            if (stubs)
            {
                var path = this.GetPath(unit, out nestedLevel, folder: !stubs ? "src" : "impl", doNotCreateFolder: true);
                if (File.Exists(path))
                {
                    // do not overwrite an existing file
                    return;
                }
            }

            var anyRecord = false;
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                var c = new CCodeWriterText(itw);

                WriteSourceInclude(itw, identity);

                foreach (var definition in unit.Definitions.Where(d => !d.IsGeneric && d.IsStub == stubs))
                {
                    anyRecord = true;
                    definition.WriteTo(c);
                }

                if (unit.MainMethod != null)
                {
                    WriteSourceMainEntry(c, itw, unit.MainMethod);
                }

                itw.Close();
            }

            if (anyRecord && text.Length > 0)
            {
                var path = this.GetPath(unit, out nestedLevel, folder: !stubs ? "src" : "impl");
                var newText = text.ToString();

                if (IsNothingChanged(path, newText))
                {
                    return;
                }

                using (var textFile = new StreamWriter(path))
                {
                    textFile.Write(newText);
                }
            }
        }

        public static void WriteSourceMainEntry(CCodeWriterBase c, IndentedTextWriter itw, IMethodSymbol mainMethod)
        {
            itw.WriteLine();
            itw.WriteLine("auto main() -> int32_t");
            itw.WriteLine("{");
            itw.Indent++;
            c.WriteMethodFullName(mainMethod);
            itw.Write("(");
            if (mainMethod.Parameters.Length > 0)
            {
                itw.Write("nullptr");
            }

            itw.WriteLine(");");
            itw.WriteLine("return 0;");
            itw.Indent--;
            itw.WriteLine("}");
        }

        public static void WriteSourceInclude(IndentedTextWriter itw, AssemblyIdentity identity)
        {
            itw.Write("#include \"");
            itw.WriteLine("{0}.h\"", identity.Name);
        }

        public void WriteHeader(AssemblyIdentity identity, ISet<AssemblyIdentity> references, bool isCoreLib, IEnumerable<CCodeUnit> units, IEnumerable<string> includeHeaders)
        {
            // write header
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                var c = new CCodeWriterText(itw);

                if (isCoreLib)
                {
                    itw.WriteLine(Resources.c_include.Replace("<<%assemblyName%>>", identity.Name));
                }
                else
                {
                    foreach (var reference in references)
                    {
                        itw.WriteLine("#include \"{0}.h\"", reference.Name);
                    }
                }

                // write forward declaration
                foreach (var unit in units)
                {
                    WriteForwardDeclarationForUnit(unit, itw, c);
                }

                itw.WriteLine();

                if (isCoreLib)
                {
                    itw.WriteLine(Resources.c_forward_declarations.Replace("<<%assemblyName%>>", identity.Name));
                }

                itw.WriteLine();

                // write full declaration
                foreach (var unit in units)
                {
                    WriteFullDeclarationForUnit(unit, itw, c);
                }

                if (isCoreLib)
                {
                    itw.WriteLine();
                    itw.WriteLine(Resources.c_declarations.Replace("<<%assemblyName%>>", identity.Name));
                }

                itw.WriteLine();

                foreach (var includeHeader in includeHeaders)
                {
                    itw.WriteLine("#include \"{0}\"", includeHeader);
                }

                itw.Close();
            }

            var path = this.GetPath(identity.Name, subFolder: "src");
            var newText = text.ToString();

            if (IsNothingChanged(path, newText))
            {
                return;
            }

            using (var textFile = new StreamWriter(path))
            {
                textFile.Write(newText);
            }
        }

        private static void WriteForwardDeclarationForUnit(CCodeUnit unit, IndentedTextWriter itw, CCodeWriterText c)
        {
            var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                itw.Write("namespace ");
                c.WriteNamespaceName(namespaceNode);
                itw.Write(" { ");
            }

            if (namedTypeSymbol.IsGenericType)
            {
                c.WriteTemplateDeclaration(namedTypeSymbol);
            }

            itw.Write(namedTypeSymbol.IsValueType ? "struct" : "class");
            itw.Write(" ");
            c.WriteTypeName(namedTypeSymbol, false);
            itw.Write("; ");

            if (namedTypeSymbol.TypeKind == TypeKind.Enum)
            {
                itw.WriteLine();
                itw.Write("enum class ");
                c.WriteTypeName(namedTypeSymbol, false, true);
                itw.Write(" : ");
                c.WriteType(namedTypeSymbol.EnumUnderlyingType);

                c.NewLine();
                c.OpenBlock();

                var any = false;
                foreach (var constValue in namedTypeSymbol.GetMembers().OfType<IFieldSymbol>().Where(f => f.IsConst))
                {
                    if (any)
                    {
                        c.TextSpan(",");
                        c.WhiteSpace();
                    }

                    c.TextSpan("c_");
                    c.WriteName(constValue);
                    if (constValue.ConstantValue != null)
                    {
                        c.TextSpan(" = ");
                        c.TextSpan(constValue.ConstantValue.ToString());
                    }

                    any = true;
                }

                c.EndBlockWithoutNewLine();
                c.EndStatement();
            }

            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                itw.Write("}");
            }

            itw.WriteLine();

            if (namedTypeSymbol.SpecialType == SpecialType.System_Object || namedTypeSymbol.SpecialType == SpecialType.System_String)
            {
                itw.Write("typedef ");
                c.WriteType(namedTypeSymbol, suppressReference: true, allowKeywords: false);
                itw.Write(" ");
                c.WriteTypeName(namedTypeSymbol);
                itw.WriteLine(";");
            }
        }

        private static void WriteFullDeclarationForUnit(CCodeUnit unit, IndentedTextWriter itw, CCodeWriterText c)
        {
            var any = false;
            var namedTypeSymbol = (INamedTypeSymbol)unit.Type;
            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                itw.Write("namespace ");
                c.WriteNamespaceName(namespaceNode);
                itw.Write(" { ");
                any = true;
            }

            if (any)
            {
                itw.Indent++;
                itw.WriteLine();
            }

            if (namedTypeSymbol.IsGenericType)
            {
                c.WriteTemplateDeclaration(namedTypeSymbol);
            }

            itw.Write(namedTypeSymbol.IsValueType ? "struct" : "class");
            itw.Write(" ");
            c.WriteTypeName(namedTypeSymbol, false);
            if (namedTypeSymbol.BaseType != null)
            {
                itw.Write(" : public virtual ");
                c.WriteTypeFullName(namedTypeSymbol.BaseType);
            }

            if (namedTypeSymbol.TypeKind == TypeKind.Interface)
            {
                if (namedTypeSymbol.Interfaces.Any())
                {
                    if (namedTypeSymbol.BaseType == null)
                    {
                        itw.Write(" : ");
                    }
                    else
                    {
                        itw.Write(", ");
                    }

                    var any2 = false;
                    foreach (var item in namedTypeSymbol.Interfaces)
                    {
                        if (any2)
                        {
                            itw.Write(", ");
                        }

                        itw.Write("public virtual ");
                        c.WriteTypeFullName(item, false);

                        any2 = true;
                    }
                }
                else if (namedTypeSymbol.TypeKind == TypeKind.Interface)
                {
                    if (namedTypeSymbol.BaseType == null)
                    {
                        itw.Write(" : ");
                    }

                    itw.Write("public virtual object");
                }
            }

            itw.WriteLine();
            itw.WriteLine("{");
            itw.WriteLine("public:");
            itw.Indent++;

            // base declaration
            if (namedTypeSymbol.BaseType != null)
            {
                itw.Write("typedef ");
                c.WriteTypeFullName(namedTypeSymbol.BaseType, false);
                itw.WriteLine(" base;");
            }

            // declare using to solve issue with overloaded functions in different classes
            foreach (var method in namedTypeSymbol.IterateAllMethodsWithTheSameNames())
            {
                c.TextSpan("using");
                c.WhiteSpace();
                c.WriteType(method.ReceiverType, suppressReference: true, allowKeywords: true);
                c.TextSpan("::");
                c.WriteMethodName(method);
                c.TextSpan(";");
                c.NewLine();
            }

            if (namedTypeSymbol.TypeKind == TypeKind.Enum)
            {
                // value holder for enum
                c.WriteType(namedTypeSymbol);
                itw.WriteLine(" m_value;");
            }

            if (!unit.HasDefaultConstructor)
            {
                c.WriteTypeName(namedTypeSymbol, false);
                itw.WriteLine("() = default;");
            }

            foreach (var declaration in unit.Declarations)
            {
                declaration.WriteTo(c);
            }

            itw.Indent--;
            itw.WriteLine("};");

            foreach (var namespaceNode in namedTypeSymbol.ContainingNamespace.EnumNamespaces())
            {
                itw.Indent--;
                itw.Write("}");
            }

            itw.WriteLine();

            if (namedTypeSymbol.IsPrimitiveValueType() || namedTypeSymbol.TypeKind == TypeKind.Enum)
            {
                itw.WriteLine();
                // write boxing function
                c.TextSpan("inline");
                c.WhiteSpace();
                c.WriteType(namedTypeSymbol, valueTypeAsClass: true);
                c.WhiteSpace();
                c.TextSpan("__box(");
                c.WriteType(namedTypeSymbol);
                c.WhiteSpace();
                c.TextSpan("value)");
                c.NewLine();
                c.OpenBlock();

                var specialTypeConstructorMethod = new CCodeSpecialTypeOrEnumConstructorDeclaration.SpecialTypeConstructorMethod(namedTypeSymbol);
                var objectCreationExpression = new ObjectCreationExpression { Type = namedTypeSymbol, IsReference = true, Method = specialTypeConstructorMethod };
                objectCreationExpression.Arguments.Add(new Parameter { ParameterSymbol = specialTypeConstructorMethod.Parameters.First() });
                new ReturnStatement { ExpressionOpt = objectCreationExpression }.WriteTo(c);

                c.EndBlock();
                itw.WriteLine();
            }
        }

        public void WriteCoreLibSource(AssemblyIdentity identity, bool isCoreLib)
        {
            if (!isCoreLib)
            {
                return;
            }

            // write header
            var text = new StringBuilder();
            using (var itw = new IndentedTextWriter(new StringWriter(text)))
            {
                WriteSourceInclude(itw, identity);

                itw.WriteLine(Resources.c_definitions.Replace("<<%assemblyName%>>", identity.Name));
                itw.Close();
            }

            var newText = text.ToString();
            var path = this.GetPath(identity.Name, subFolder: "src", ext: ".cpp");

            if (IsNothingChanged(path, newText))
            {
                return;
            }

            using (var textFile = new StreamWriter(path))
            {
                textFile.Write(newText);
            }
        }

        private string GetPath(string name, string ext = ".h", string subFolder = "")
        {
            var fullDirPath = Path.Combine(this.currentFolder, subFolder);
            var fullPath = Path.Combine(fullDirPath, String.Concat(name, ext));
            if (!Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            return fullPath;
        }

        private string GetPath(CCodeUnit unit, out int nestedLevel, string ext = ".cpp", string folder = "src", bool doNotCreateFolder = false)
        {
            var fileRelativePath = GetRelativePath(unit, out nestedLevel);
            var fullDirPath = Path.Combine(this.currentFolder, folder, fileRelativePath);
            if (!doNotCreateFolder && !Directory.Exists(fullDirPath))
            {
                Directory.CreateDirectory(fullDirPath);
            }

            var fullPath = Path.Combine(fullDirPath, string.Concat(unit.Type.GetTypeName().CleanUpNameAllUnderscore(), ext));
            return fullPath;
        }

        private static string GetRelativePath(CCodeUnit unit, out int nestedLevel)
        {
            var enumNamespaces = unit.Type.ContainingNamespace.EnumNamespaces().Where(n => !n.IsGlobalNamespace).ToList();
            nestedLevel = enumNamespaces.Count();
            return String.Join("\\", enumNamespaces.Select(n => n.MetadataName.ToString().CleanUpNameAllUnderscore()));
        }

        private static bool IsNothingChanged(string path, string newText)
        {
            // check if file exist and overwrite only if different in size of HashCode
            if (!File.Exists(path))
            {
                return false;
            }

            var fileInfo = new FileInfo(path);
            if (fileInfo.Length != newText.Length)
            {
                return false;
            }

            // sizes the same, check HashValues
            using (var hashAlorithm = new SHA1CryptoServiceProvider())
            using (var textFile = new StreamReader(path))
            {
                var newHash = hashAlorithm.ComputeHash(Encoding.UTF8.GetBytes(newText));
                var originalHash = hashAlorithm.ComputeHash(textFile.BaseStream);
                var isNothingChanged = StructuralComparisons.StructuralEqualityComparer.Equals(newHash, originalHash);
                return isNothingChanged;
            }
        }
    }
}

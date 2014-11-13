// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="Driver.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace PdbReader
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Cci.Pdb;

    /// <summary>
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// </summary>
        private readonly Dictionary<string, SourceFile> files = new Dictionary<string, SourceFile>();

        /// <summary>
        /// </summary>
        private readonly ISymbolWriter symbolWriter;

        /// <summary>
        /// </summary>
        /// <param name="symbolWriter">
        /// </param>
        internal Converter(ISymbolWriter symbolWriter)
        {
            this.symbolWriter = symbolWriter;
        }

        /// <summary>
        /// </summary>
        /// <param name="filename">
        /// </param>
        public static void Convert(string filename)
        {
            var pdb = filename + ".pdb";
            pdb = Path.Combine(Path.GetDirectoryName(filename), pdb);

            using (var stream = File.OpenRead(pdb))
            {
                var funcs = PdbFile.LoadFunctions(stream, true);
                Convert(funcs, null);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="assembly">
        /// </param>
        /// <param name="functions">
        /// </param>
        /// <param name="symbolWriter">
        /// </param>
        internal static void Convert(IEnumerable<PdbFunction> functions, ISymbolWriter symbolWriter)
        {
            var converter = new Converter(symbolWriter);

            foreach (var function in functions)
            {
                converter.ConvertFunction(function);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="function">
        /// </param>
        private void ConvertFunction(PdbFunction function)
        {
            if (function.lines == null)
            {
                return;
            }

            var method = new SourceMethod { Name = function.name, Token = (int)function.token };

            var file = this.GetSourceFile(this.symbolWriter, function);

            var builder = this.symbolWriter.OpenMethod(file.CompilationUnitEntry, 0, method);

            this.ConvertSequencePoints(function, file, builder);

            this.ConvertVariables(function);

            this.symbolWriter.CloseMethod();
        }

        /// <summary>
        /// </summary>
        /// <param name="scope">
        /// </param>
        private void ConvertScope(PdbScope scope)
        {
            this.ConvertSlots(scope.slots);

            foreach (var s in scope.scopes)
            {
                this.ConvertScope(s);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="function">
        /// </param>
        /// <param name="file">
        /// </param>
        /// <param name="builder">
        /// </param>
        private void ConvertSequencePoints(PdbFunction function, SourceFile file, ISourceMethodBuilder builder)
        {
            var last_line = 0;
            foreach (var line in function.lines.SelectMany(lines => lines.lines))
            {
                // 0xfeefee is an MS convention, we can't pass it into mdb files, so we use the last non-hidden line
                var is_hidden = line.lineBegin == 0xfeefee;
                builder.MarkSequencePoint(
                    (int)line.offset, file.CompilationUnitEntry.SourceFile, is_hidden ? last_line : (int)line.lineBegin, (int)line.colBegin, is_hidden);
                if (!is_hidden)
                {
                    last_line = (int)line.lineBegin;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="slots">
        /// </param>
        private void ConvertSlots(IEnumerable<PdbSlot> slots)
        {
            foreach (var slot in slots)
            {
                this.symbolWriter.DefineLocalVariable((int)slot.slot, slot.name);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="function">
        /// </param>
        private void ConvertVariables(PdbFunction function)
        {
            foreach (var scope in function.scopes)
            {
                this.ConvertScope(scope);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="symbolWriter">
        /// </param>
        /// <param name="function">
        /// </param>
        /// <returns>
        /// </returns>
        private SourceFile GetSourceFile(ISymbolWriter symbolWriter, PdbFunction function)
        {
            var name = (from l in function.lines where l.file != null select l.file.name).First();

            SourceFile file;
            if (this.files.TryGetValue(name, out file))
            {
                return file;
            }

            var entry = symbolWriter.DefineDocument(name);
            var unit = symbolWriter.DefineCompilationUnit(entry);

            file = new SourceFile(unit, entry);
            this.files.Add(name, file);
            return file;
        }

        /// <summary>
        /// </summary>
        private class SourceFile : ISourceFile
        {
            /// <summary>
            /// </summary>
            private readonly ICompileUnitEntry compileUnitEntry;

            /// <summary>
            /// </summary>
            private readonly ISourceFileEntry entry;

            /// <summary>
            /// </summary>
            /// <param name="compileUnitEntry">
            /// </param>
            /// <param name="entry">
            /// </param>
            public SourceFile(ICompileUnitEntry compileUnitEntry, ISourceFileEntry entry)
            {
                this.compileUnitEntry = compileUnitEntry;
                this.entry = entry;
            }

            /// <summary>
            /// </summary>
            public ICompileUnitEntry CompilationUnitEntry
            {
                get
                {
                    return this.compileUnitEntry;
                }
            }

            /// <summary>
            /// </summary>
            public ISourceFileEntry Entry
            {
                get
                {
                    return this.entry;
                }
            }
        }

        /// <summary>
        /// </summary>
        private class SourceMethod : ISourceMethod
        {
            /// <summary>
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// </summary>
            public int Token { get; set; }
        }
    }
}
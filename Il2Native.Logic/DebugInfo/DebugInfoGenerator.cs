namespace Il2Native.Logic.DebugInfo
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Il2Native.Logic.Gencode;
    using Il2Native.Logic.Metadata.Model;

    using PdbReader;

    using PEAssemblyReader;

    public class DebugInfoGenerator
    {
        private const string IdentityString = "C# Native compiler";

        private readonly string defaultSourceFilePath;

        private readonly IDictionary<int, int> indexByOffset = new SortedDictionary<int, int>();

        private readonly IList<CollectionMetadata> indexedMetadata = new List<CollectionMetadata>();

        private readonly IDictionary<int, string> nameBySlot = new SortedDictionary<int, string>();

        private readonly IList<NamedMetadata> namedMetadata = new List<NamedMetadata>(3);

        private readonly string pdbFileName;

        private readonly IDictionary<IField, CollectionMetadata> typeMembersMetadataCache = new SortedDictionary<IField, CollectionMetadata>();

        private readonly IDictionary<IType, object> typesMetadataCache = new SortedDictionary<IType, object>();

        private CollectionMetadata currentFunction;

        private CollectionMetadata file;

        private CollectionMetadata fileType;

        private CollectionMetadata globalVariables;

        private IMethod methodDefinition;

        private CollectionMetadata retainedTypes;

        private CollectionMetadata tagExpression;

        private LlvmWriter writer;

        private bool structuresByName = true;

        public DebugInfoGenerator(string pdbFileName, string defaultSourceFilePath)
        {
            this.pdbFileName = pdbFileName;
            this.defaultSourceFilePath = defaultSourceFilePath;

            this.CompileUnit = new CollectionMetadata();
            this.Flags = new CollectionMetadata();
            this.Identity = new CollectionMetadata();

            this.namedMetadata.Add(new NamedMetadata("llvm.dbg.cu", this.CompileUnit));
            this.namedMetadata.Add(new NamedMetadata("llvm.module.flags", this.Flags));
            this.namedMetadata.Add(new NamedMetadata("llvm.ident", this.Identity));

            // add default flags and identity
            this.Identity.Add(new CollectionMetadata(this.indexedMetadata).Add(IdentityString));
            this.Flags.Add(new CollectionMetadata(this.indexedMetadata).Add(2, "Dwarf Version", 4));
            this.Flags.Add(new CollectionMetadata(this.indexedMetadata).Add(2, "Debug Info Version", 2));
        }

        public CollectionMetadata CompileUnit { get; private set; }

        public int? CurrentDebugLine { get; private set; }

        public string DefaultSourceFilePath
        {
            get
            {
                return this.defaultSourceFilePath;
            }
        }

        public CollectionMetadata Flags { get; private set; }

        public CollectionMetadata Identity { get; private set; }

        public IConverter PdbConverter { get; set; }

        public void DefineCompilationUnit(
            CollectionMetadata file,
            out CollectionMetadata enumTypes,
            out CollectionMetadata retainedTypes,
            out CollectionMetadata subprograms,
            out CollectionMetadata globalVariables,
            out CollectionMetadata importedEntities)
        {
            // 4 - C++
            // 12 - C
            var lang = 4;

            var compilationUnit = new CollectionMetadata(this.indexedMetadata).Add(
                string.Format(@"0x11\00{0}\00{1}\000\00\000\00\001", lang, IdentityString),
                // file
                file,
                // Enum Types
                enumTypes = new CollectionMetadata(this.indexedMetadata),
                // Retained Types
                retainedTypes = new CollectionMetadata(this.indexedMetadata),
                // Subprograms
                subprograms = new CollectionMetadata(this.indexedMetadata),
                // Global Variables
                new CollectionMetadata(this.indexedMetadata).Add(globalVariables = new CollectionMetadata(this.indexedMetadata) { NullIfEmpty = true }),
                // Imported entities
                importedEntities = new CollectionMetadata(this.indexedMetadata));

            this.globalVariables = globalVariables;
            this.retainedTypes = retainedTypes;
            this.file = file;
            this.fileType = new CollectionMetadata(this.indexedMetadata).Add("0x29", file);

            this.CompileUnit.Add(compilationUnit);
        }

        public CollectionMetadata DefineFile(ISourceFileEntry entry)
        {
            return new CollectionMetadata(this.indexedMetadata).Add(entry.FileName, PrepareEscape(entry.Directory));
        }

        public void DefineGlobal(IField field)
        {
            if (this.globalVariables == null)
            {
                throw new NullReferenceException("globalVariables");
            }

            var globalType = this.writer.WriteToString(() => field.FieldType.WriteTypePrefix(this.writer.Output)) + "* ";
            var globalName = string.Format("@\"{0}\"", field.GetFullName());

            var line = 0;

            this.retainedTypes.Add(this.DefineType(field.DeclaringType));

            this.globalVariables.Add(
                string.Format(@"0x34\00{0}\00{1}\00{2}\00{3}\000\001", field.Name, field.Name, PrepareEscape(field.GetFullName()), line),
                null,
                this.fileType,
                this.DefineType(field.FieldType),
                new PlainTextMetadata(string.Concat(globalType, " ", globalName)),
                this.DefineMember(field));
        }

        public void DefineLocalVariable(string name, int slot)
        {
            this.nameBySlot[slot] = name;
        }

        public CollectionMetadata DefineMember(IField field, bool create = false, CollectionMetadata structureType = null)
        {
            var line = 0;
            var size = field.FieldType.GetTypeSize(true) * 8;
            var align = LlvmWriter.PointerSize * 8;
            var offset = !field.IsStatic ? field.GetFieldOffset() * 8 : 0;

            // static
            var flags = field.IsStatic ? 4096 : 0;

            CollectionMetadata memberMetadata;
            if (!create && this.typeMembersMetadataCache.TryGetValue(field, out memberMetadata))
            {
                return memberMetadata;
            }

            var fieldMetadata = new CollectionMetadata(this.indexedMetadata);
            var typeMember = fieldMetadata.Add(
                string.Format(@"0xd\00{0}\00{1}\00{2}\00{3}\00{4}\00{5}", field.Name, line, size, align, offset, flags),
                this.file,
                this.structuresByName || structureType == null ? (object)this.DefineType(field.DeclaringType) : (object)structureType,
                this.DefineType(field.FieldType));

            if (this.structuresByName)
            {
                typeMember.Add((object)null);
            }

            typeMembersMetadataCache[field] = typeMember;

            return typeMember;
        }

        public CollectionMetadata DefineMethod(ISourceMethod method, CollectionMetadata file, out CollectionMetadata functionVariables)
        {
            // Flags 256 - definition (as main()), 259 - public (member of a class)
            var flag = 256;

            // Line number of the opening '{' of the function
            var scopeLine = method.LineNumber;

            // find method definition
            this.methodDefinition = this.writer.MethodsByToken[method.Token];

            var methodReferenceType = this.writer.WriteToString(() => this.writer.WriteMethodPointerType(this.writer.Output, this.methodDefinition));
            var methodDefinitionName = this.writer.WriteToString(() => this.writer.WriteMethodDefinitionName(this.writer.Output, this.methodDefinition));

            CollectionMetadata subroutineTypes;
            CollectionMetadata parametersTypes;

            // add compile unit template
            var methodMetadataDefinition =
                new CollectionMetadata(this.indexedMetadata).Add(
                    string.Format(
                        @"0x2e\00{0}\00{1}\00{2}\00{3}\000\001\000\000\00{4}\000\00{5}",
                        method.Name,
                        method.DisplayName,
                        method.LinkageName,
                        method.LineNumber,
                        flag,
                        scopeLine),
                // Source directory (including trailing slash) & file pair
                    file,
                // Reference to context descriptor
                    this.fileType,
                // Subroutine types
                    subroutineTypes = new CollectionMetadata(this.indexedMetadata),
                // indicates which base type contains the vtable pointer for the derived class
                    null,
                // function method reference ex. "i32 ()* @main"                
                    new PlainTextMetadata(string.Concat(methodReferenceType, " ", methodDefinitionName)),
                // Lists function template parameters
                    null,
                // Function declaration descriptor
                    null,
                // List of function variables
                    functionVariables = new CollectionMetadata(this.indexedMetadata));

            // add subrouting type
            subroutineTypes.Add(
                @"0x15\00\000\000\000\000\000\000", null, null, null, parametersTypes = new CollectionMetadata(this.indexedMetadata), null, null, null);

            this.currentFunction = methodMetadataDefinition;

            // add return type
            parametersTypes.Add(
                !this.methodDefinition.ReturnType.IsVoid() && !this.methodDefinition.ReturnType.IsStructureType()
                    ? this.DefineType(this.methodDefinition.ReturnType)
                    : null);
            foreach (var parameter in this.methodDefinition.GetParameters())
            {
                parametersTypes.Add(this.DefineType(parameter.ParameterType));
            }

            return methodMetadataDefinition;
        }

        public CollectionMetadata DefineTagExpression()
        {
            if (this.tagExpression == null)
            {
                this.tagExpression = new CollectionMetadata(this.indexedMetadata).Add("0x102");
            }

            return this.tagExpression;
        }

        public object DefineType(IType type)
        {
            var line = 0;
            var offset = 0;
            var flags = 0;

            object typeMetadata;
            if (this.typesMetadataCache.TryGetValue(type, out typeMetadata))
            {
                return typeMetadata;
            }

            if (type.IsPrimitiveType())
            {
                typeMetadata = this.DefinePrimitiveType(type, line, offset, flags);
                this.typesMetadataCache[type] = typeMetadata;
            }
            else
            {
                var structureType = new CollectionMetadata(this.indexedMetadata);
                var reference = this.structuresByName ? (object)type.FullName : (object)structureType;
                this.typesMetadataCache[type] = reference;
                this.DefineStructureType(type, line, offset, flags, structureType);

                typeMetadata = reference;
            }

            return typeMetadata;
        }

        public CollectionMetadata DefineVariable(string name, IType variableType, DebugVariableType debugVariableType)
        {
            var line = 0;

            var tag = string.Empty;
            switch (debugVariableType)
            {
                case DebugVariableType.Argument:
                    tag = "0x101";
                    line = 16777220;
                    break;
                case DebugVariableType.Auto:
                    tag = "0x100";
                    break;
                default:
                    break;
            }

            var type = this.DefineType(variableType);

            return new CollectionMetadata(this.indexedMetadata).Add(
                string.Format(@"{2}\00{0}\00{1}\000", name, line, tag), this.currentFunction, this.fileType, type);
        }

        public void GenerateFunction(int token)
        {
            this.indexByOffset.Clear();
            this.nameBySlot.Clear();
            this.CurrentDebugLine = null;

            this.PdbConverter.ConvertFunction(token);
        }

        public string GetLocalNameByIndex(int localIndex)
        {
            string name;
            if (this.nameBySlot.TryGetValue(localIndex, out name))
            {
                return name;
            }

            return "local" + localIndex;
        }

        public void ReadAndSetCurrentDebugLine(int offset)
        {
            var newLine = this.GetLineByOffiset(offset);
            if (newLine.HasValue)
            {
                this.CurrentDebugLine = newLine;
            }
        }

        public void SequencePoint(int offset, int lineBegin, int colBegin, CollectionMetadata function)
        {
            var dbgLine = new CollectionMetadata(this.indexedMetadata).Add(lineBegin, colBegin, function, null);
            if (dbgLine.Index.HasValue)
            {
                this.indexByOffset[offset] = dbgLine.Index.Value;
            }
        }

        public void StartGenerating(LlvmWriter writer)
        {
            if (!File.Exists(this.pdbFileName))
            {
                return;
            }

            this.writer = writer;
            this.PdbConverter = Converter.GetConverter(this.pdbFileName, new DebugInfoSymbolWriter.DebugInfoSymbolWriter(this));

            // to force generating CompileUnit info
            this.PdbConverter.ConvertFunction(-1);
        }

        public void WriteTo(TextWriter output)
        {
            output.WriteLine(string.Empty);

            output.WriteLine("; Metadata Entries");
            foreach (var namedMetadataItem in this.namedMetadata)
            {
                namedMetadataItem.WriteTo(output, true);
            }

            output.WriteLine(string.Empty);

            output.WriteLine("; Metadata Items");
            foreach (
                var indexedMetadataItem in this.indexedMetadata.Where(indexedMetadataItem => !indexedMetadataItem.NullIfEmpty || !indexedMetadataItem.IsEmpty))
            {
                output.Write("!{0} = ", indexedMetadataItem.Index);
                indexedMetadataItem.WriteValueTo(output);
                output.WriteLine(string.Empty);
            }
        }

        protected int? GetLineByOffiset(int offset)
        {
            int index;
            if (this.indexByOffset.TryGetValue(offset, out index))
            {
                return index;
            }

            return null;
        }

        private static string PrepareEscape(string globalName)
        {
            var sb = new StringBuilder(globalName.Length);

            foreach (var @char in globalName)
            {
                if (@char == '\\' || @char == '"')
                {
                    sb.Append(@"\");
                    sb.Append(((int)@char).ToString("X"));
                    continue;
                }

                sb.Append(@char);
            }

            return sb.ToString();
        }

        private CollectionMetadata DefineMembers(IType type, CollectionMetadata structureType)
        {
            var members = new CollectionMetadata(this.indexedMetadata);
            foreach (var field in IlReader.Fields(type))
            {
                members.Add(this.DefineMember(field, true, structureType));
            }

            return members;
        }

        private CollectionMetadata DefinePrimitiveType(IType type, int line, int offset, int flags)
        {
            return
                new CollectionMetadata(this.indexedMetadata).Add(
                    string.Format(
                        @"0x24\00{0}\00{1}\00{2}\00{3}\00{4}\00{5}\005",
                        type.FullName,
                        line,
                        type.GetTypeSize(true) * 8,
                        LlvmWriter.PointerSize * 8,
                        offset,
                        flags),
                    null,
                    null);
        }

        private void DefineStructureType(IType type, int line, int offset, int flags, CollectionMetadata structureType)
        {
            structureType.Add(
                    string.Format(
                        @"0x13\00{0}\00{1}\00{2}\00{3}\00{4}\00{5}\000", type.Name, line, type.GetTypeSize(true) * 8, LlvmWriter.PointerSize * 8, offset, flags),
                    this.file,
                    null,
                    null,
                // members
                    this.DefineMembers(type, structureType),
                    null,
                    null,
                    this.structuresByName ? type.FullName : null);

            if (structuresByName)
            {
                this.retainedTypes.Add(structureType);
            }
        }
    }
}
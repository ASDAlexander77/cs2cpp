namespace Il2Native.Logic.DebugInfo
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Gencode;
    using Metadata.Model;
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
        private readonly IDictionary<int, object> subrangeTypeCache = new SortedDictionary<int, object>();
        private readonly IDictionary<string, CollectionMetadata> typeMembersByOffsetMetadataCache =
            new SortedDictionary<string, CollectionMetadata>();

        private readonly IDictionary<IField, CollectionMetadata> typeMembersMetadataCache =
            new SortedDictionary<IField, CollectionMetadata>();

        private readonly IDictionary<IType, object> typesMetadataCache = new SortedDictionary<IType, object>();
        private readonly IDictionary<IType, object> typeStructuresMetadataCache = new SortedDictionary<IType, object>();
        private CollectionMetadata currentFunction;
        private CollectionMetadata file;
        private CollectionMetadata fileType;
        private int functionNumberUsedArgs = -1;
        private CollectionMetadata globalVariables;
        private IMethod methodDefinition;
        private CollectionMetadata retainedTypes;
        private bool structuresByName = true;
        private CollectionMetadata tagExpression;
        private LlvmWriter writer;

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
            get { return this.defaultSourceFilePath; }
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
            if (!this.CompileUnit.IsEmpty)
            {
                var first = this.CompileUnit[0] as CollectionMetadata;

                enumTypes = first[2] as CollectionMetadata;
                retainedTypes = first[3] as CollectionMetadata;
                subprograms = first[4] as CollectionMetadata;
                globalVariables = (first[5] as CollectionMetadata)[0] as CollectionMetadata;
                importedEntities = first[6] as CollectionMetadata;

                this.file = file;
                this.fileType = new CollectionMetadata(this.indexedMetadata).Add("0x29", file);

                return;
            }

            // 4 - C++
            // 12 - C
            // 11 - Java
            var lang = 12;

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
                new CollectionMetadata(this.indexedMetadata).Add(
                    globalVariables = new CollectionMetadata(this.indexedMetadata) { NullIfEmpty = true }),
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

            var globalType = this.writer.WriteToString(() => field.FieldType.WriteTypePrefix(this.writer)) + "* ";
            var globalName = string.Format("@\"{0}\"", field.GetFullName());

            var line = 0;

            this.retainedTypes.Add(this.DefineType(field.DeclaringType));

            this.globalVariables.Add(
                string.Format(
                    @"0x34\00{0}\00{1}\00{2}\00{3}\000\001",
                    field.Name,
                    field.Name,
                    PrepareEscape(field.GetFullName()),
                    line),
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

        public CollectionMetadata DefineMember(
            IField field,
            bool create = false,
            CollectionMetadata structureType = null)
        {
            var line = 0;
            var size = field.FieldType.GetTypeSize(writer, true) * 8;
            var align = LlvmWriter.PointerSize * 8;
            var offset = !field.IsStatic ? field.GetFieldOffset(writer) * 8 : 0;

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
                this.structuresByName || structureType == null
                    ? this.FindStructure(field.DeclaringType)
                    : (object)structureType,
                this.DefineType(field.FieldType));

            if (this.structuresByName)
            {
                typeMember.Add((object)null);
            }

            this.typeMembersMetadataCache[field] = typeMember;

            return typeMember;
        }

        public CollectionMetadata DefineMember(
            string fieldName,
            IType fieldType,
            int offset,
            IType fieldDeclaringType,
            bool create = false,
            CollectionMetadata structureType = null)
        {
            return this.DefineMember(
                fieldName,
                fieldType,
                offset,
                fieldDeclaringType,
                create,
                structureType,
                this.DefineType(fieldType));
        }

        public CollectionMetadata DefineMember(
            string fieldName,
            IType fieldType,
            int offset,
            IType fieldDeclaringType,
            bool create = false,
            CollectionMetadata structureType = null,
            object definedMedataType = null,
            int count = 1)
        {
            var line = 0;
            var size = fieldType.GetTypeSize(this.writer, true) * 8 * count;
            var align = LlvmWriter.PointerSize * 8;

            // static
            var flags = 0;

            var key = string.Concat(fieldType, offset);
            CollectionMetadata memberMetadata;
            if (!create && this.typeMembersByOffsetMetadataCache.TryGetValue(key, out memberMetadata))
            {
                return memberMetadata;
            }

            var fieldMetadata = new CollectionMetadata(this.indexedMetadata);
            var typeMember = fieldMetadata.Add(
                string.Format(@"0xd\00{0}\00{1}\00{2}\00{3}\00{4}\00{5}", fieldName, line, size, align, offset, flags),
                this.file,
                this.structuresByName || structureType == null
                    ? this.FindStructure(fieldDeclaringType)
                    : (object)structureType,
                definedMedataType);

            if (this.structuresByName)
            {
                typeMember.Add((object)null);
            }

            this.typeMembersByOffsetMetadataCache[key] = typeMember;

            return typeMember;
        }

        public CollectionMetadata DefineMethod(
            ISourceMethod method,
            CollectionMetadata file,
            out CollectionMetadata functionVariables)
        {
            // Flags 256 - definition (as main()), 259 - public (member of a class)
            var flag = 256;

            // Line number of the opening '{' of the function
            var scopeLine = method.LineNumber;

            // find method definition
            this.methodDefinition = this.writer.MethodsByToken[method.Token];

            var methodReferenceType =
                this.writer.WriteToString(
                    () => this.writer.WriteMethodPointerType(this.writer.Output, this.methodDefinition));
            var methodDefinitionName =
                this.writer.WriteToString(
                    () => this.writer.WriteMethodDefinitionName(this.writer.Output, this.methodDefinition));

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
                @"0x15\00\000\000\000\000\000\000",
                null,
                null,
                null,
                parametersTypes = new CollectionMetadata(this.indexedMetadata),
                null,
                null,
                null);

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
            else if (type.IsPointer)
            {
                typeMetadata = this.DefinePointerType(type, line, offset);
                this.typesMetadataCache[type] = typeMetadata;
            }
            else if (type.IsByRef)
            {
                typeMetadata = this.DefineByRefType(type, line, offset);
                this.typesMetadataCache[type] = typeMetadata;
            }
            else if (type.IsArray)
            {
                var structureType = new CollectionMetadata(this.indexedMetadata);
                var structureOrStructureRef = this.structuresByName ? type.FullName : (object)structureType;
                var pointer = this.DefinePointerType(structureType, line, offset);

                this.typeStructuresMetadataCache[type] = structureOrStructureRef;
                this.typesMetadataCache[type] = pointer;

                this.DefineStructureType(
                    type,
                    line,
                    offset,
                    flags,
                    structureType,
                    this.DefineArrayMembers(type, structureType));

                typeMetadata = pointer;
            }
            else
            {
                var structureType = new CollectionMetadata(this.indexedMetadata);
                var structureOrStructureRef = this.structuresByName ? type.FullName : (object)structureType;
                object pointer = null;
                if (!type.IsStructureType() && !type.IsEnum)
                {
                    pointer = this.DefinePointerType(structureType, line, offset);
                }

                this.typeStructuresMetadataCache[type] = structureOrStructureRef;
                this.typesMetadataCache[type] = pointer ?? structureOrStructureRef;

                this.DefineStructureType(type, line, offset, flags, structureType);

                typeMetadata = pointer ?? structureOrStructureRef;
            }

            return typeMetadata;
        }

        public CollectionMetadata DefineVariable(
            string name,
            IType variableType,
            DebugVariableType debugVariableType,
            int index = 0)
        {
            var line = 0;

            var tag = string.Empty;
            switch (debugVariableType)
            {
                case DebugVariableType.Argument:
                    tag = "0x101";

                    if (index == 0)
                    {
                        this.functionNumberUsedArgs++;
                    }

                    line = index * 16777216 + 1 + this.functionNumberUsedArgs * 5;
                    break;
                case DebugVariableType.Auto:
                    tag = "0x100";
                    break;
            }

            var type = this.DefineType(variableType);

            return new CollectionMetadata(this.indexedMetadata).Add(
                string.Format(@"{2}\00{0}\00{1}\000", name, line, tag),
                this.currentFunction,
                this.fileType,
                type);
        }

        public object FindStructure(IType type)
        {
            object typeMetadata;
            if (this.typesMetadataCache.TryGetValue(type, out typeMetadata))
            {
                return typeMetadata;
            }

            throw new KeyNotFoundException();
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
            var dbgLine = writer.IsLlvm36
                ? new MDLocation(lineBegin, colBegin, function, this.indexedMetadata)
                : new CollectionMetadata(this.indexedMetadata).Add(lineBegin, colBegin, function, null);
            if (dbgLine.Index.HasValue)
            {
                this.indexByOffset[offset] = dbgLine.Index.Value;
            }
        }

        public bool StartGenerating(LlvmWriter writer)
        {
            if (!File.Exists(this.pdbFileName))
            {
                return false;
            }

            this.structuresByName = true;// !writer.IsLlvm36;

            this.writer = writer;
            this.PdbConverter = Converter.GetConverter(
                this.pdbFileName,
                new DebugInfoSymbolWriter.DebugInfoSymbolWriter(this));

            // to force generating CompileUnit info
            this.PdbConverter.ConvertFunction(-1);

            return true;
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
                var indexedMetadataItem in
                    this.indexedMetadata.Where(
                        indexedMetadataItem => !indexedMetadataItem.NullIfEmpty || !indexedMetadataItem.IsEmpty))
            {
                output.Write("!{0} = ", indexedMetadataItem.Index);
                indexedMetadataItem.WriteValueTo(output, writer.IsLlvm36);
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

        private CollectionMetadata DefineArrayMembers(IType type, CollectionMetadata structureType)
        {
            Debug.Assert(type != null && type.IsArray);

            var members = new CollectionMetadata(this.indexedMetadata);

            var elementsCount = 10;
            var countOffset = 16; // 3 * pointerSize + sizeof(int)
            var dataOffset = 20; // + pointer size

            var countMember = this.DefineMember(
                "count",
                this.writer.ResolveType("System.Int32"),
                countOffset * 8,
                type,
                true,
                structureType);
            members.Add(countMember);
            members.Add(
                this.DefineMember(
                    "array",
                    type,
                    dataOffset * 8,
                    type,
                    true,
                    structureType,
                    this.DefineCArrayType(type.GetElementType(), 0, 0, elementsCount, countMember),
                    0));

            return members;
        }

        private CollectionMetadata DefineByRefType(IType type, int line, int offset)
        {
            Debug.Assert(type != null && type.IsByRef);
            return this.DefinePointerType(this.DefineType(type.GetElementType()), line, offset);
        }

        private CollectionMetadata DefineCArrayType(
            IType type,
            int line,
            int offset,
            int count = 0,
            object countMember = null)
        {
            Debug.Assert(type != null);

            return
                new CollectionMetadata(this.indexedMetadata).Add(
                    string.Format(
                        @"0x1\00\00{0}\00{1}\00{2}\00{3}\000\000",
                        line,
                        count * type.GetTypeSize(this.writer, true) * 8,
                        LlvmWriter.PointerSize * 8,
                        offset),
                    null,
                    null,
                    this.DefineType(type.ToDereferencedType()),
                    new CollectionMetadata(this.indexedMetadata).Add(this.DefineSubrangeType(count, countMember)),
                    null,
                    null,
                    null);
        }

        private CollectionMetadata DefineExpression()
        {
            // 0x97 - DW_OP_push_object_address
            // 0x06 - DW_OP_deref
            return new CollectionMetadata(this.indexedMetadata).Add(@"0x102\0x97\0x06");
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

        private CollectionMetadata DefinePointerType(IType type, int line, int offset)
        {
            Debug.Assert(type != null && type.IsPointer);
            return this.DefinePointerType(this.DefineType(type.ToDereferencedType()), line, offset);
        }

        private CollectionMetadata DefinePointerType(object typeDefinition, int line, int offset)
        {
            return
                new CollectionMetadata(this.indexedMetadata).Add(
                    string.Format(
                        @"0x0f\00\00{0}\00{1}\00{2}\00{3}",
                        line,
                        LlvmWriter.PointerSize * 8,
                        LlvmWriter.PointerSize * 8,
                        offset),
                    null,
                    null,
                    typeDefinition);
        }

        private CollectionMetadata DefinePrimitiveType(IType type, int line, int offset, int flags)
        {
            int typeCode;
            switch (type.FullName)
            {
                case "System.Boolean":
                    typeCode = 2;
                    break;
                case "System.Single":
                case "System.Double":
                    typeCode = 4;
                    break;
                case "System.Char":
                    typeCode = 7;
                    break;
                default:

                    if (type.IsUnsignedType())
                    {
                        typeCode = 7;
                    }
                    else if (type.IsSignedType())
                    {
                        typeCode = 5;
                    }
                    else if (type.IsPointer)
                    {
                        typeCode = 1;
                    }
                    else
                    {
                        typeCode = 0;
                    }

                    break;
            }

            return
                new CollectionMetadata(this.indexedMetadata).Add(
                    string.Format(
                        @"0x24\00{0}\00{1}\00{2}\00{3}\00{4}\00{5}\00{6}",
                        type.FullName,
                        line,
                        type.GetTypeSize(this.writer, true) * 8,
                        LlvmWriter.PointerSize * 8,
                        offset,
                        flags,
                        typeCode),
                    null,
                    null);
        }

        private void DefineStructureType(IType type, int line, int offset, int flags, CollectionMetadata structureType)
        {
            this.DefineStructureType(type, line, offset, flags, structureType, this.DefineMembers(type, structureType));
        }

        private void DefineStructureType(
            IType type,
            int line,
            int offset,
            int flags,
            CollectionMetadata structureType,
            CollectionMetadata members)
        {
            structureType.Add(
                string.Format(
                    @"0x13\00{0}\00{1}\00{2}\00{3}\00{4}\00{5}\000",
                    type.Name,
                    line,
                    type.GetTypeSize(this.writer, true) * 8,
                    LlvmWriter.PointerSize * 8,
                    offset,
                    flags),
                this.file,
                null,
                null,
                members,
                null,
                null,
                this.structuresByName ? type.FullName : null);

            if (this.structuresByName)
            {
                this.retainedTypes.Add(structureType);
            }
        }

        private object DefineSubrangeType(int count, object countMember = null)
        {
            ////if (countMember != null)
            ////{
            ////    count = -1;
            ////}

            object subrangeType;
            if (this.subrangeTypeCache.TryGetValue(count, out subrangeType))
            {
                return subrangeType;
            }

            ////if (countMember == null)
            ////{
            subrangeType = new CollectionMetadata(this.indexedMetadata).Add(string.Format(@"0x21\000\00{0}", count));
            this.subrangeTypeCache[count] = subrangeType;
            ////}
            ////else
            ////{
            ////    // next line should work but not working.
            ////    //subrangeType = new CollectionMetadata(this.indexedMetadata).Add(@"0x21\000", countMember);
            ////    //subrangeType = new CollectionMetadata(this.indexedMetadata).Add(@"0x21", this.DefineExpression(), this.DefineExpression());
            ////}

            return subrangeType;
        }
    }
}
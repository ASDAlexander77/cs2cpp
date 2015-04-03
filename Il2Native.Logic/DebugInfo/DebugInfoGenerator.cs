namespace Il2Native.Logic.DebugInfo
{
    using System.Collections.Generic;
    using System.IO;

    using PdbReader;

    public class DebugInfoGenerator
    {
        private readonly string sourceFilePath;

        private readonly IDictionary<int, KeyValuePair<int, int>> indexByOffset = new SortedDictionary<int, KeyValuePair<int, int>>();

        private readonly IDictionary<int, string> nameBySlot = new SortedDictionary<int, string>();

        private readonly string pdbFileName;

        public DebugInfoGenerator(string pdbFileName, string defaultSourceFilePath)
        {
            this.pdbFileName = pdbFileName;
            if (!string.IsNullOrEmpty(defaultSourceFilePath))
            {
                this.sourceFilePath = defaultSourceFilePath.Replace('\\', '/');
                SourceFilePathChanged = true;
            }
        }

        public int? CurrentDebugLine { get; set; }

        public bool CurrentDebugLineNew { get; set; }

        public string SourceFilePath
        {
            get
            {
                return this.sourceFilePath;
            }
        }

        public bool SourceFilePathChanged { get; set; }

        public IConverter PdbConverter { get; set; }

        public void DefineLocalVariable(string name, int slot)
        {
            var index = 0;
            var currentName = name;
            for (var i = 0; i < slot; i++)
            {
                string existingName;
                if (this.nameBySlot.TryGetValue(i, out existingName) && existingName == currentName)
                {
                    index++;
                    currentName = string.Concat(name, "_", index);
                }
            }

            this.nameBySlot[slot] = (index > 0 ? currentName : name).CleanUpName();
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
                if (newLine != this.CurrentDebugLine)
                {
                    this.CurrentDebugLineNew = true;
                }

                this.CurrentDebugLine = newLine;
            }
        }

        public void SequencePoint(int offset, int lineBegin, int colBegin)
        {
            this.indexByOffset[offset] = new KeyValuePair<int, int>(lineBegin, colBegin);
        }

        public bool StartGenerating()
        {
            if (!File.Exists(this.pdbFileName))
            {
                return false;
            }

            // to allow to write header with first line
            this.CurrentDebugLine = 1;

            this.PdbConverter = Converter.GetConverter(this.pdbFileName, new DebugInfoSymbolWriter.DebugInfoSymbolWriter(this));

            // to force generating CompileUnit info
            this.PdbConverter.ConvertFunction(-1);

            return true;
        }

        protected int? GetLineByOffiset(int offset)
        {
            KeyValuePair<int, int> index;
            if (this.indexByOffset.TryGetValue(offset, out index))
            {
                return index.Key;
            }

            return null;
        }
    }
}
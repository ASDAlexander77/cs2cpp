namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IlCodeBuilder
    {
        private readonly List<object> parts = new List<object>();

        public void Add(Code code)
        {
            this.parts.Add(code);
        }

        public void Add(int valueOrToken)
        {
            var value = BitConverter.GetBytes(valueOrToken);
            this.parts.AddRange(value.Cast<object>());
        }

        public void Add(long valueOrToken)
        {
            var value = BitConverter.GetBytes(valueOrToken);
            this.parts.AddRange(value.Cast<object>());
        }

        public void Add(uint valueOrToken)
        {
            var value = BitConverter.GetBytes(valueOrToken);
            this.parts.AddRange(value.Cast<object>());
        }

        public void Add(ulong valueOrToken)
        {
            var value = BitConverter.GetBytes(valueOrToken);
            this.parts.AddRange(value.Cast<object>());
        }

        public void Add(Code code, int token)
        {
            this.Add(code);
            this.Add(token);
        }

        public void Add(BranchNode branchNode)
        {
            this.parts.Add(branchNode);
        }

        public void Add(Label label)
        {
            this.parts.Add(label);
        }

        public Label Branch(Code code, Code codeShort)
        {
            var branch = new BranchNode(code, codeShort);
            this.Add(branch);
            return branch.Label;
        }

        public void Branch(Code code, Code codeShort, Label label)
        {
        }

        public Label CreateLabel()
        {
            return new Label();
        }

        public byte[] GetCode()
        {
            var bytes = new List<byte>();

            foreach (var @byte in this.IterateBytes())
            {
                bytes.Add(@byte);
            }

            return bytes.ToArray();
        }

        public void LoadArgument(int number)
        {
        }

        // helpers
        public void LoadConstant(int @const)
        {
        }

        public void LoadLocal(int number)
        {
        }

        public void SaveArgument(int number)
        {
        }

        public void SaveLocal(int number)
        {
        }

        private IEnumerable<byte> IterateBytes()
        {
            foreach (var codeItem in this.parts)
            {
                if (codeItem is Code)
                {
                    var @byte = (byte)(Code)codeItem;
                    if (@byte >= 0xE1)
                    {
                        yield return 0xFE;
                        yield return (byte)(@byte - 0xE1);
                    }
                    else
                    {
                        yield return @byte;
                    }
                }
                else if (codeItem is Label)
                {
                    
                }
                else
                {
                    yield return (byte)codeItem;
                }
            }
        }

        public class BranchNode
        {
            private Code opCode;
            private Code opCodeShort;

            public BranchNode(Code opCode, Code opCodeShort)
            {
                this.opCode = opCode;
                this.opCodeShort = opCodeShort;
                this.Label = new Label();
            }

            public Label Label { get; private set; }
        }

        public class Label
        {
            public Label()
            {
            }
        }
    }
}
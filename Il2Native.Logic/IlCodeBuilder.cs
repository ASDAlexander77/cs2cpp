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
            var branch = new BranchNode(code, codeShort, label);
            this.Add(branch);
        }

        public Label CreateLabel()
        {
            return new Label();
        }

        public byte[] GetCode()
        {
            return this.IterateBytes().ToArray();
        }

        public void LoadArgument(int number)
        {
            throw new NotImplementedException();
        }

        // helpers
        public void LoadConstant(int @const)
        {
            throw new NotImplementedException();
        }

        public void LoadLocal(int number)
        {
            throw new NotImplementedException();
        }

        public void SaveArgument(int number)
        {
            throw new NotImplementedException();
        }

        public void SaveLocal(int number)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<byte> IterateBytes()
        {
            var address = 0;

            foreach (var codeItem in this.parts)
            {
                if (codeItem is Code)
                {
                    var @byte = (byte)(Code)codeItem;
                    if (@byte >= 0xE1)
                    {
                        yield return 0xFE;
                        address++;
                        yield return (byte)(@byte - 0xE1);
                        address++;
                    }
                    else
                    {
                        yield return @byte;
                        address++;
                    }

                    continue;
                }

                var label = codeItem as Label;
                if (label != null)
                {
                    label.Address = address;
                    continue;
                }

                var branch = codeItem as BranchNode;
                if (branch != null)
                {
                    foreach (var branchByte in branch.GetBytes())
                    {
                        yield return branchByte;
                    }

                    continue;
                }

                yield return (byte)codeItem;
                address++;
            }
        }

        public class BranchNode
        {
            private Code opCode;
            private Code opCodeShort;
            private int _address;

            public BranchNode(Code opCode, Code opCodeShort)
            {
                this.opCode = opCode;
                this.opCodeShort = opCodeShort;
                this.Label = new Label(this);
            }

            public BranchNode(Code opCode, Code opCodeShort, Label label)
            {
                this.opCode = opCode;
                this.opCodeShort = opCodeShort;
                this.Label = label;
                this.Label.BranchNode = this;
            }

            public int Address
            {
                get { return this._address; }
                set
                {
                    this._address = value;
                    this.AddressSet = true;
                }
            }

            public bool AddressSet { get; private set; }

            public Label Label { get; private set; }

            public bool IsLong 
            {
                get
                {
                    if (!this.AddressSet || this.Label == null || !this.Label.AddressSet)
                    {
                        return true;
                    }

                    var diff = this.Label.Address - this.Address;
                    return diff < SByte.MinValue || diff > SByte.MaxValue;
                }
            }

            public IEnumerable<byte> GetBytes()
            {
                // default size if 5 bytes (OpCode + 4 bytes address)
                if (this.Label == null || !this.Label.AddressSet || this.IsLong)
                {
                    yield return (byte)this.opCode;
                    foreach (var addressByte in BitConverter.GetBytes(this.Address))
                    {
                        yield return addressByte;
                    }
                    yield break;
                }

                yield return (byte)this.opCodeShort;
                yield return (byte)(this.Label.Address - this.Address);
            }
        }

        public class Label
        {
            private int _address;

            public Label()
            {
            }

            public Label(BranchNode branchNode)
            {
                this.BranchNode = branchNode;
            }

            public BranchNode BranchNode { get; set; }

            public int Address
            {
                get { return this._address; }
                set
                {
                    this._address = value;
                    this.AddressSet = true;
                }
            }

            public bool AddressSet { get; private set; }
        }
    }
}
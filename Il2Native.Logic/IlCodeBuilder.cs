namespace Il2Native.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Il2Native.Logic.Gencode;
    using Il2Native.Logic.Gencode.SynthesizedMethods;

    using PEAssemblyReader;

    public class IlCodeBuilder
    {
        private readonly List<object> parts = new List<object>();

        private List<IParameter> _parameters;

        private List<object> _tokenResolutions;

        private List<IType> _locals;

        public List<IParameter> Parameters
        {
            get
            {
                return this._parameters ?? (this._parameters = new List<IParameter>());
            }

            set
            {
                _parameters = value;
            }
        }

        public List<object> TokenResolutions
        {
            get
            {
                return this._tokenResolutions ?? (this._tokenResolutions = new List<object>());
            }

            set
            {
                _tokenResolutions = value;
            }
        }

        public List<IType> Locals
        {
            get
            {
                return this._locals ?? (this._locals = new List<IType>());
            }

            set
            {
                _locals = value;
            }
        }

        public void Register(string fullMethodName)
        {
            MethodBodyBank.Register(fullMethodName, this.GetCode(), _tokenResolutions, _locals, _parameters);
        }

        public IMethod GetMethod(IMethod originalMethod)
        {
            return MethodBodyBank.GetMethodDecorator(originalMethod, this.GetCode(), _tokenResolutions, _locals, _parameters);
        }

        public IMethodBody GetMethodBody(IMethodBody originalOpt = null)
        {
            return new SynthesizedMethodBodyDecorator(originalOpt, _locals, this.GetCode());
        }

        public IList<IParameter> GetParameters()
        {
            return _parameters;
        }

        public IList<object> GetTokenResolutions()
        {
            return _tokenResolutions;
        }

        public void Add(Code code)
        {
            this.parts.Add(code);
        }

        public void RemoveLast()
        {
            this.parts.RemoveAt(this.parts.Count - 1);
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

        public void Add(Code code, byte value)
        {
            this.Add(code);
            this.parts.Add(value);
        }

        public void Add(BranchNode branchNode)
        {
            this.parts.Add(branchNode);
        }

        public void Add(SwitchNode switchNode)
        {
            this.parts.Add(switchNode);
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

        public SwitchNode Switch()
        {
            var @switch = new SwitchNode();
            this.Add(@switch);
            return @switch;
        }

        public Label CreateLabel()
        {
            var label = new Label();
            this.Add(label);
            return label;
        }

        public byte[] GetCode()
        {
            var maxCount = 10;
            while (this.SetLabelAddresses() && maxCount-- >= 0)
            {
            }

            return this.IterateBytes().ToArray();
        }

        public void LoadArgument(int argIndex)
        {
            switch (argIndex)
            {
                case 0:
                    this.Add(Code.Ldarg_0);
                    break;
                case 1:
                    this.Add(Code.Ldarg_1);
                    break;
                case 2:
                    this.Add(Code.Ldarg_2);
                    break;
                case 3:
                    this.Add(Code.Ldarg_3);
                    break;
                default:
                    if (argIndex <= byte.MaxValue)
                    {
                        this.Add(Code.Ldarg_S, checked((byte)argIndex));
                    }
                    else
                    {
                        this.Add(Code.Ldarg, argIndex);
                    }
                    break;
            }
        }

        public void LoadArgumentAddress(int argIndex)
        {
            if (argIndex <= byte.MaxValue)
            {
                this.Add(Code.Ldarga_S, checked((byte)argIndex));
            }
            else
            {
                this.Add(Code.Ldarga, argIndex);
            }
        }

        public void New(IConstructor @constructor)
        {
            Debug.Assert(@constructor != null, "@constructor is null");
            TokenResolutions.Add(@constructor);
            this.Add(Code.Newobj, (int)TokenResolutions.Count);
        }

        public void NewArray(IType elementType)
        {
            Debug.Assert(elementType != null, "@elementType is null");
            TokenResolutions.Add(elementType);
            this.Add(Code.Newarr, (int)TokenResolutions.Count);
        }

        public void SizeOf(IType type)
        {
            Debug.Assert(type != null, "@type is null");
            TokenResolutions.Add(type);
            this.Add(Code.Sizeof, (int)TokenResolutions.Count);
        }

        public void Call(IMethod method)
        {
            Debug.Assert(method != null, "@method is null");
            TokenResolutions.Add(method);
            this.Add(method.IsVirtual ? Code.Callvirt : Code.Call, (int)TokenResolutions.Count);
        }

        public void Castclass(IType type)
        {
            Debug.Assert(type != null, "@type is null");
            TokenResolutions.Add(type);
            this.Add(Code.Castclass, (int)TokenResolutions.Count);
        }

        public void LoadField(IField field)
        {
            Debug.Assert(field != null, "@field is null");
            TokenResolutions.Add(field);
            this.Add(field.IsStatic ? Code.Ldsfld : Code.Ldfld, (int)TokenResolutions.Count);
        }

        public void LoadFieldAddress(IField field)
        {
            Debug.Assert(field != null, "@field is null");
            TokenResolutions.Add(field);
            this.Add(field.IsStatic ? Code.Ldsflda : Code.Ldflda, (int)TokenResolutions.Count);
        }

        public void SaveField(IField field)
        {
            Debug.Assert(field != null, "@field is null");
            TokenResolutions.Add(field);
            this.Add(field.IsStatic ? Code.Stsfld : Code.Stfld, (int)TokenResolutions.Count);
        }

        public void LoadToken(IType type)
        {
            Debug.Assert(type != null, "@type is null");
            TokenResolutions.Add(type);
            this.Add(Code.Ldtoken, (int)TokenResolutions.Count);
        }

        public void LoadToken(IConstBytes constBytes)
        {
            TokenResolutions.Add(constBytes);
            this.Add(Code.Ldtoken, (int)TokenResolutions.Count);
        }

        public void LoadToken(IField field)
        {
            Debug.Assert(field != null, "@field is null");
            TokenResolutions.Add(field);
            this.Add(Code.Ldtoken, (int)TokenResolutions.Count);
        }

        public void CopyObject(IType type)
        {
            Debug.Assert(type != null, "@type is null");
            TokenResolutions.Add(type);
            this.Add(Code.Cpobj, (int)TokenResolutions.Count);
        }

        public void LoadObject(IType type)
        {
            Debug.Assert(type != null, "@type is null");
            TokenResolutions.Add(type);
            this.Add(Code.Ldobj, (int)TokenResolutions.Count);
        }

        public void SaveObject(IType type)
        {
            Debug.Assert(type != null, "@type is null");
            TokenResolutions.Add(type);
            this.Add(Code.Stobj, (int)TokenResolutions.Count);
        }

        public void Throw()
        {
            this.Add(Code.Throw);
        }

        // helpers
        public void LoadNull()
        {
            this.Add(Code.Ldnull);
        }

        // helpers
        public void Return()
        {
            this.Add(Code.Ret);
        }

        public void LoadConstant(int @const)
        {
            switch (@const)
            {
                case 0:
                    this.Add(Code.Ldc_I4_0);
                    break;
                case 1:
                    this.Add(Code.Ldc_I4_1);
                    break;
                case 2:
                    this.Add(Code.Ldc_I4_2);
                    break;
                case 3:
                    this.Add(Code.Ldc_I4_3);
                    break;
                case 4:
                    this.Add(Code.Ldc_I4_4);
                    break;
                case 5:
                    this.Add(Code.Ldc_I4_5);
                    break;
                case 6:
                    this.Add(Code.Ldc_I4_6);
                    break;
                case 7:
                    this.Add(Code.Ldc_I4_7);
                    break;
                case 8:
                    this.Add(Code.Ldc_I4_8);
                    break;
                case -1:
                    this.Add(Code.Ldc_I4_M1);
                    break;
                default:
                    if (@const >= 0 && @const <= byte.MaxValue)
                    {
                        this.Add(Code.Ldc_I4_S, checked((byte)@const));
                    }
                    else
                    {
                        this.Add(Code.Ldc_I4, @const);
                    }

                    break;
            }
        }

        public void LoadLocal(int number)
        {
            switch (number)
            {
                case 0:
                    this.Add(Code.Ldloc_0);
                    break;
                case 1:
                    this.Add(Code.Ldloc_1);
                    break;
                case 2:
                    this.Add(Code.Ldloc_2);
                    break;
                case 3:
                    this.Add(Code.Ldloc_2);
                    break;
                default:
                    if (number <= byte.MaxValue)
                    {
                        this.Add(Code.Ldloc_S, checked((byte)number));
                    }
                    else
                    {
                        this.Add(Code.Ldloc, number);
                    }

                    break;
            }
        }

        public void LoadLocalAddress(int number)
        {
            if (number <= byte.MaxValue)
            {
                this.Add(Code.Ldloca_S, checked((byte)number));
            }
            else
            {
                this.Add(Code.Ldloca, number);
            }
        }

        public void SaveArgument(int argIndex)
        {
            if (argIndex <= byte.MaxValue)
            {
                this.Add(Code.Starg_S, checked((byte)argIndex));
            }
            else
            {
                this.Add(Code.Starg, argIndex);
            }
        }

        public void SaveLocal(int number)
        {
            switch (number)
            {
                case 0:
                    this.Add(Code.Stloc_0);
                    break;
                case 1:
                    this.Add(Code.Stloc_1);
                    break;
                case 2:
                    this.Add(Code.Stloc_2);
                    break;
                case 3:
                    this.Add(Code.Stloc_2);
                    break;
                default:
                    if (number <= byte.MaxValue)
                    {
                        this.Add(Code.Stloc_S, checked((byte)number));
                    }
                    else
                    {
                        this.Add(Code.Stloc, number);
                    }

                    break;
            }
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

                    continue;
                }

                var label = codeItem as Label;
                if (label != null)
                {
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

                var @switch = codeItem as SwitchNode;
                if (@switch != null)
                {
                    foreach (var switchByte in @switch.GetBytes())
                    {
                        yield return switchByte;
                    }

                    continue;
                }

                yield return (byte)codeItem;
            }
        }

        /// <summary>
        /// </summary>
        /// <returns>true if one more call for SetLabelAddresses required</returns>
        private bool SetLabelAddresses()
        {
            var changed = false;
            var address = 0;

            foreach (var codeItem in this.parts)
            {
                if (codeItem is Code)
                {
                    var @byte = (byte)(Code)codeItem;
                    if (@byte >= 0xE1)
                    {
                        address += 2;
                    }
                    else
                    {
                        address++;
                    }

                    continue;
                }

                var label = codeItem as Label;
                if (label != null)
                {
                    label.Address = address;
                    changed |= label.IsChanged;
                    continue;
                }

                var branch = codeItem as BranchNode;
                if (branch != null)
                {
                    branch.Address = address;
                    address += branch.GetBytes().Count();
                    continue;
                }

                var @switch = codeItem as SwitchNode;
                if (@switch != null)
                {
                    @switch.Address = address;
                    address += @switch.GetBytes().Count();
                    continue;
                }

                address++;
            }

            return changed;
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
                this.Label = new Label();
            }

            public BranchNode(Code opCode, Code opCodeShort, Label label)
            {
                this.opCode = opCode;
                this.opCodeShort = opCodeShort;
                this.Label = label;
            }

            public int Address
            {
                get
                {
                    return this._address;
                }

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

                    var diffShort = this.Label.Address - (this.Address + 2);
                    var diff = this.Label.Address - (this.Address + 5);
                    return diff < sbyte.MinValue || diff > sbyte.MaxValue || diffShort < sbyte.MinValue || diffShort > sbyte.MaxValue;
                }
            }

            public IEnumerable<byte> GetBytes()
            {
                // default size if 5 bytes (OpCode + 4 bytes address)
                if (this.Label == null || !this.Label.AddressSet || this.IsLong)
                {
                    yield return (byte)this.opCode;
                    var address = this.Label.Address - (this.Address + 1 + 4);
                    foreach (var addressByte in BitConverter.GetBytes(address))
                    {
                        yield return addressByte;
                    }

                    yield break;
                }

                yield return (byte)this.opCodeShort;
                yield return (byte)(this.Label.Address - (this.Address + 1 + 1));
            }
        }

        public class SwitchNode
        {
            private int _address;

            public SwitchNode()
            {
                this.Labels = new List<Label>();
            }

            public int Address
            {
                get
                {
                    return this._address;
                }

                set
                {
                    this._address = value;
                    this.AddressSet = true;
                }
            }

            public bool AddressSet { get; private set; }

            public Label Label { get; private set; }

            public IList<Label> Labels { get; private set; }

            public IEnumerable<byte> GetBytes()
            {
                yield return (byte)Code.Switch;

                foreach (var countByte in BitConverter.GetBytes(this.Labels.Count))
                {
                    yield return countByte;
                }

                foreach (var label in this.Labels)
                {
                    var value = label.Address - (this.Address + 1 + 4 + this.Labels.Count * 4);
                    foreach (var addressByte in BitConverter.GetBytes(value))
                    {
                        yield return addressByte;
                    }
                }
            }
        }

        public class Label
        {
            private int _address;

            public Label()
            {
            }

            public int Address
            {
                get
                {
                    return this._address;
                }

                set
                {
                    this.IsChanged = _address != value;
                    this._address = value;
                    this.AddressSet = true;
                }
            }

            public bool AddressSet { get; private set; }

            public bool IsChanged { get; private set; }
        }

        public void EmptyParameters()
        {
            this._parameters = new List<IParameter>();
        }
    }
}
namespace Il2Native.Logic.Gencode.SynthesizedMethods
{
    using PEAssemblyReader;

    public class SynthesizedLocalVariable : ILocalVariable
    {
        public SynthesizedLocalVariable(int index, IType type)
        {
            this.LocalIndex = index;
            this.LocalType = type;
        }

        public int LocalIndex
        {
            get;
            private set;
        }

        public IType LocalType
        {
            get;
            set;
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}

namespace Il2Native.Logic.CodeParts
{
    using System.ComponentModel;
    using System.Linq;
    using PEAssemblyReader;

    /// <summary>
    /// </summary>
    public class RequiredIncomingTypes
    {
        private IType[] types;

        /// <summary>
        /// </summary>
        public RequiredIncomingTypes(int count)
        {
            this.types = new IType[count];
        }

        public IType this[int index]
        {
            get { return this.types[index]; }
            set { this.types[index] = value; }
        }
    }
}
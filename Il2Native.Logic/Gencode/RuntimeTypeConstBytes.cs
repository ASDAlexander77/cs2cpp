namespace Il2Native.Logic
{
    using System.Text;
    using PEAssemblyReader;

    public class RuntimeTypeConstBytes : IConstBytes
    {
        public RuntimeTypeConstBytes(IType type)
        {
            this.Reference = string.Concat(type.FullName.CleanUpName(), "_type_data_");
            this.Data = Encoding.ASCII.GetBytes(type.FullName);
        }

        public string Reference { get; private set; }

        public byte[] Data { get; private set; }
    }
}

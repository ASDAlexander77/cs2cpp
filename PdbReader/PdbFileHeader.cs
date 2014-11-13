// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="PdbFileHeader.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Microsoft.Cci.Pdb
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// </summary>
    internal class PdbFileHeader
    {
        /// <summary>
        /// </summary>
        internal int directoryRoot;

        /// <summary>
        /// </summary>
        internal int directorySize;

        /// <summary>
        /// </summary>
        internal int freePageMap;

        /// <summary>
        /// </summary>
        internal readonly byte[] magic;

        /// <summary>
        /// </summary>
        internal readonly int pageSize;

        /// <summary>
        /// </summary>
        internal int pagesUsed;

        /// <summary>
        /// </summary>
        internal readonly int zero;

        /// <summary>
        /// </summary>
        /// <param name="pageSize">
        /// </param>
        internal PdbFileHeader(int pageSize)
        {
            this.magic = new byte[32] { 0x4D, 0x69, 0x63, 0x72, 0x6F, 0x73, 0x6F, 0x66, // "Microsof"
                                        0x74, 0x20, 0x43, 0x2F, 0x43, 0x2B, 0x2B, 0x20, // "t C/C++ "
                                        0x4D, 0x53, 0x46, 0x20, 0x37, 0x2E, 0x30, 0x30, // "MSF 7.00"
                                        0x0D, 0x0A, 0x1A, 0x44, 0x53, 0x00, 0x00, 0x00 // "^^^DS^^^"
                                      };
            this.pageSize = pageSize;
            this.zero = 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="reader">
        /// </param>
        /// <param name="bits">
        /// </param>
        internal PdbFileHeader(Stream reader, BitAccess bits)
        {
            bits.MinCapacity(56);
            reader.Seek(0, SeekOrigin.Begin);
            bits.FillBuffer(reader, 56);

            this.magic = new byte[32];
            bits.ReadBytes(this.magic); // 0..31
            bits.ReadInt32(out this.pageSize); // 32..35
            bits.ReadInt32(out this.freePageMap); // 36..39
            bits.ReadInt32(out this.pagesUsed); // 40..43
            bits.ReadInt32(out this.directorySize); // 44..47
            bits.ReadInt32(out this.zero); // 48..51
            bits.ReadInt32(out this.directoryRoot); // 52..55
        }

        /// <summary>
        /// </summary>
        internal string Magic
        {
            get
            {
                return this.StringFromBytesUTF8(this.magic);
            }
        }

        //////////////////////////////////////////////////// Helper Functions.
        /// <summary>
        /// </summary>
        /// <param name="bytes">
        /// </param>
        /// <returns>
        /// </returns>
        internal string StringFromBytesUTF8(byte[] bytes)
        {
            return this.StringFromBytesUTF8(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes">
        /// </param>
        /// <param name="offset">
        /// </param>
        /// <param name="length">
        /// </param>
        /// <returns>
        /// </returns>
        internal string StringFromBytesUTF8(byte[] bytes, int offset, int length)
        {
            for (var i = 0; i < length; i++)
            {
                if (bytes[offset + i] < ' ')
                {
                    length = i;
                }
            }

            return Encoding.UTF8.GetString(bytes, offset, length);
        }

        /// <summary>
        /// </summary>
        /// <param name="writer">
        /// </param>
        /// <param name="bits">
        /// </param>
        internal void Write(Stream writer, BitAccess bits)
        {
            bits.MinCapacity(56);
            bits.WriteBytes(this.magic); // 0..31
            bits.WriteInt32(this.pageSize); // 32..35
            bits.WriteInt32(this.freePageMap); // 36..39
            bits.WriteInt32(this.pagesUsed); // 40..43
            bits.WriteInt32(this.directorySize); // 44..47
            bits.WriteInt32(this.zero); // 48..51
            bits.WriteInt32(this.directoryRoot); // 52..55

            writer.Seek(0, SeekOrigin.Begin);
            bits.WriteBuffer(writer, 56);
        }

        ////////////////////////////////////////////////////////////// Fields.
    }
}
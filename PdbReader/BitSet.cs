// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="BitSet.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Microsoft.Cci.Pdb
{
    /// <summary>
    /// </summary>
    internal struct BitSet
    {
        /// <summary>
        /// </summary>
        private readonly int size;

        /// <summary>
        /// </summary>
        private readonly uint[] words;

        /// <summary>
        /// </summary>
        /// <param name="bits">
        /// </param>
        internal BitSet(BitAccess bits)
        {
            bits.ReadInt32(out this.size); // 0..3 : Number of words
            this.words = new uint[this.size];
            bits.ReadUInt32(this.words);
        }

        /// <summary>
        /// </summary>
        /// <param name="size">
        /// </param>
        internal BitSet(int size)
        {
            this.size = size;
            this.words = new uint[size];
        }

        /// <summary>
        /// </summary>
        internal bool IsEmpty
        {
            get
            {
                return this.size == 0;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        internal void Clear(int index)
        {
            var word = index / 32;
            if (word >= this.size)
            {
                return;
            }

            this.words[word] &= ~this.GetBit(index);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <param name="word">
        /// </param>
        /// <returns>
        /// </returns>
        internal bool GetWord(int index, out uint word)
        {
            if (index < this.size)
            {
                word = this.ReverseBits(this.words[index]);
                return true;
            }

            word = 0;
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        internal bool IsSet(int index)
        {
            var word = index / 32;
            if (word >= this.size)
            {
                return false;
            }

            return (this.words[word] & this.GetBit(index)) != 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        internal void Set(int index)
        {
            var word = index / 32;
            if (word >= this.size)
            {
                return;
            }

            this.words[word] |= this.GetBit(index);
        }

        /// <summary>
        /// </summary>
        /// <param name="index">
        /// </param>
        /// <returns>
        /// </returns>
        private uint GetBit(int index)
        {
            return (uint)1 << (index % 32);
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        /// <returns>
        /// </returns>
        private uint ReverseBits(uint value)
        {
            uint o = 0;
            for (var i = 0; i < 32; i++)
            {
                o = (o << 1) | (value & 1);
                value >>= 1;
            }

            return o;
        }
    }
}
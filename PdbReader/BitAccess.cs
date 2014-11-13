// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="BitAccess.cs">
//   
// </copyright>
// <summary>
//   
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace Microsoft.Cci.Pdb
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// </summary>
    internal class BitAccess
    {
        /// <summary>
        /// </summary>
        private byte[] buffer;

        /// <summary>
        /// </summary>
        private int offset;

        /// <summary>
        /// </summary>
        /// <param name="capacity">
        /// </param>
        internal BitAccess(int capacity)
        {
            this.buffer = new byte[capacity];
            this.offset = 0;
        }

        /// <summary>
        /// </summary>
        internal byte[] Buffer
        {
            get
            {
                return this.buffer;
            }
        }

        /// <summary>
        /// </summary>
        internal int Position
        {
            get
            {
                return this.offset;
            }

            set
            {
                this.offset = value;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="alignment">
        /// </param>
        internal void Align(int alignment)
        {
            while ((this.offset % alignment) != 0)
            {
                this.offset++;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="stream">
        /// </param>
        /// <param name="capacity">
        /// </param>
        internal void FillBuffer(Stream stream, int capacity)
        {
            this.MinCapacity(capacity);
            stream.Read(this.buffer, 0, capacity);
            this.offset = 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="capacity">
        /// </param>
        internal void MinCapacity(int capacity)
        {
            if (this.buffer.Length < capacity)
            {
                this.buffer = new byte[capacity];
            }

            this.offset = 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadBString(out string value)
        {
            ushort len;
            this.ReadUInt16(out len);
            value = Encoding.UTF8.GetString(this.buffer, this.offset, len);
            this.offset += len;
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes">
        /// </param>
        internal void ReadBytes(byte[] bytes)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = this.buffer[this.offset++];
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadCString(out string value)
        {
            var len = 0;
            while (this.offset + len < this.buffer.Length && this.buffer[this.offset + len] != 0)
            {
                len++;
            }

            value = Encoding.UTF8.GetString(this.buffer, this.offset, len);
            this.offset += len + 1;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal decimal ReadDecimal()
        {
            var bits = new int[4];
            this.ReadInt32(bits);
            return new decimal(bits);
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal double ReadDouble()
        {
            var result = BitConverter.ToDouble(this.buffer, this.offset);
            this.offset += 8;
            return result;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal float ReadFloat()
        {
            var result = BitConverter.ToSingle(this.buffer, this.offset);
            this.offset += 4;
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="guid">
        /// </param>
        internal void ReadGuid(out Guid guid)
        {
            uint a;
            ushort b;
            ushort c;
            byte d;
            byte e;
            byte f;
            byte g;
            byte h;
            byte i;
            byte j;
            byte k;

            this.ReadUInt32(out a);
            this.ReadUInt16(out b);
            this.ReadUInt16(out c);
            this.ReadUInt8(out d);
            this.ReadUInt8(out e);
            this.ReadUInt8(out f);
            this.ReadUInt8(out g);
            this.ReadUInt8(out h);
            this.ReadUInt8(out i);
            this.ReadUInt8(out j);
            this.ReadUInt8(out k);

            guid = new Guid(a, b, c, d, e, f, g, h, i, j, k);
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadInt16(out short value)
        {
            value = (short)((this.buffer[this.offset + 0] & 0xFF) | (this.buffer[this.offset + 1] << 8));
            this.offset += 2;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadInt32(out int value)
        {
            value = (this.buffer[this.offset + 0] & 0xFF) | (this.buffer[this.offset + 1] << 8) | (this.buffer[this.offset + 2] << 16)
                     | (this.buffer[this.offset + 3] << 24);
            this.offset += 4;
        }

        /// <summary>
        /// </summary>
        /// <param name="values">
        /// </param>
        internal void ReadInt32(int[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                this.ReadInt32(out values[i]);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadInt64(out long value)
        {
            value = (this.buffer[this.offset + 0] & 0xFF) | (this.buffer[this.offset + 1] << 8) | (this.buffer[this.offset + 2] << 16)
                     | (this.buffer[this.offset + 3] << 24) | (this.buffer[this.offset + 4] << 32) | (this.buffer[this.offset + 5] << 40)
                     | (this.buffer[this.offset + 6] << 48) | (this.buffer[this.offset + 7] << 56);
            this.offset += 8;
        }

        /// <summary>
        /// </summary>
        /// <returns>
        /// </returns>
        internal string ReadString()
        {
            var len = 0;
            while (this.offset + len < this.buffer.Length && this.buffer[this.offset + len] != 0)
            {
                len += 2;
            }

            var result = Encoding.Unicode.GetString(this.buffer, this.offset, len);
            this.offset += len + 2;
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadUInt16(out ushort value)
        {
            value = (ushort)((this.buffer[this.offset + 0] & 0xFF) | (this.buffer[this.offset + 1] << 8));
            this.offset += 2;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadUInt32(out uint value)
        {
            value =
                (uint)
                ((this.buffer[this.offset + 0] & 0xFF) | (this.buffer[this.offset + 1] << 8) | (this.buffer[this.offset + 2] << 16)
                 | (this.buffer[this.offset + 3] << 24));
            this.offset += 4;
        }

        /// <summary>
        /// </summary>
        /// <param name="values">
        /// </param>
        internal void ReadUInt32(uint[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                this.ReadUInt32(out values[i]);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadUInt64(out ulong value)
        {
            value =
                (ulong)
                ((this.buffer[this.offset + 0] & 0xFF) | (this.buffer[this.offset + 1] << 8) | (this.buffer[this.offset + 2] << 16)
                 | (this.buffer[this.offset + 3] << 24) | (this.buffer[this.offset + 4] << 32) | (this.buffer[this.offset + 5] << 40)
                 | (this.buffer[this.offset + 6] << 48) | (this.buffer[this.offset + 7] << 56));
            this.offset += 8;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void ReadUInt8(out byte value)
        {
            value = (byte)(this.buffer[this.offset + 0] & 0xFF);
            this.offset += 1;
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void SkipCString(out string value)
        {
            var len = 0;
            while (this.offset + len < this.buffer.Length && this.buffer[this.offset + len] != 0)
            {
                len++;
            }

            this.offset += len + 1;
            value = null;
        }

        /// <summary>
        /// </summary>
        /// <param name="stream">
        /// </param>
        /// <param name="count">
        /// </param>
        internal void WriteBuffer(Stream stream, int count)
        {
            stream.Write(this.buffer, 0, count);
        }

        /// <summary>
        /// </summary>
        /// <param name="bytes">
        /// </param>
        internal void WriteBytes(byte[] bytes)
        {
            for (var i = 0; i < bytes.Length; i++)
            {
                this.buffer[this.offset++] = bytes[i];
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="value">
        /// </param>
        internal void WriteInt32(int value)
        {
            this.buffer[this.offset + 0] = (byte)value;
            this.buffer[this.offset + 1] = (byte)(value >> 8);
            this.buffer[this.offset + 2] = (byte)(value >> 16);
            this.buffer[this.offset + 3] = (byte)(value >> 24);
            this.offset += 4;
        }

        /// <summary>
        /// </summary>
        /// <param name="values">
        /// </param>
        internal void WriteInt32(int[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                WriteInt32(values[i]);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace RadarSensorGesture
{
    internal class CRC8
    {
        /// <summary>
        /// Store CRC table for fast computation
        /// </summary>
        private byte[] crcTable;

        public CRC8()
        {
            crcTable = GenerateTable();
        }

        private byte[] GenerateTable()
        {
            byte[] csTable = new byte[256];
            byte poly = 0x8c;
            for (int i = 0; i < 256; ++i)
            {
                int temp = i;
                for (int j = 0; j < 8; ++j)
                {
                    byte lsb = (byte)(temp & 0x01);
                    temp >>= 1;
                    if (lsb != 0) temp ^= poly;
                }
                csTable[i] = (byte)temp;
            }
            return csTable;
        }

        public byte Crc(byte[] buff, int startIndex, int stopIndex)
        {
            byte c = 0x15;
            for (int j = startIndex; j < stopIndex; ++j)
            {
                c = crcTable[c ^ buff[j]];
            }
            return c;
        }
    }
}

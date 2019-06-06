using System;
using System.IO;

namespace crc
{
    public class CRC
    {
        private ushort[] CRCtable;
        private static ushort[] CRCtableDefault = new ushort[256]
        {
            0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241,
            0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440,
            0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40,
            0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841,
            0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40,
            0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41,
            0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641,
            0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040,
            0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240,
            0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441,
            0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41,
            0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840,
            0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41,
            0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40,
            0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640,
            0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041,
            0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240,
            0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441,
            0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41,
            0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840,
            0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41,
            0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40,
            0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640,
            0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041,
            0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241,
            0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440,
            0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40,
            0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841,
            0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40,
            0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41,
            0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641,
            0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040
        };

        virtual public unsafe ushort Checksum(void *ptr, int blocklen)
        {
            return Update(0, ptr, blocklen);
        }

        public unsafe ushort Update(ushort crc, void *ptr, int blocklen)
        {
            ushort ubCrclo;
            ushort uwCrc = crc ;
            byte* pData = (byte*)ptr;
            uwCrc = 0;
            ubCrclo = 0;

            for (int pD = 0; pD < blocklen; pD++)
            {

                ubCrclo ^= pData[pD];
                uwCrc >>= 8;
                uwCrc ^= CRCtable[ubCrclo];
                ubCrclo = ((ushort)(uwCrc & (ushort)0x00ff));
            }

            return (uwCrc);
        }

        public ushort Checksum(string arg)
        {
            if (arg.Length == 0) return 0;
            unsafe
            {
                byte[] bytarg = System.Text.Encoding.ASCII.GetBytes(arg);

                fixed (void* dptr = &bytarg[0])
                {
                    ushort strcrc = Checksum(dptr, bytarg.Length);
                    return strcrc;
                }
            }

        }

        public ushort HexStringChecksum(string hexstr)
        {
            if (hexstr.Length == 0) return 0;
            if (hexstr.Length % 2 != 0)
            {
                Console.WriteLine("Argument length is odd. Not a valid hex string");
                return 0;
            }
            byte[] bytarg = new byte[hexstr.Length / 2];
            for (int idx = 0; idx < bytarg.Length; idx++)
            {
                string hexdig = hexstr.Substring(idx * 2, 2);
                try
                {
                    byte b = Convert.ToByte(hexdig, 16);
                    bytarg[idx] = b;
                }
                catch
                {
                    Console.WriteLine($"Bad hex digit {hexdig}");
                    return 0;
                }

            }
            unsafe
            {
                fixed (void* dptr = &bytarg[0])
                {
                    ushort hexstrcrc = Checksum(dptr, bytarg.Length);
                    return hexstrcrc;
                }
            }
        }

        public ushort FileChecksum(string filename)
        {
            ushort filechecksum = 0;
            System.IO.BinaryReader file = null;
            try
            {
                file = new System.IO.BinaryReader(File.Open(filename, FileMode.Open));
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error opening {filename}");
                Console.WriteLine(e.Message);
                throw e;
            }
            unsafe
            {
                byte[] buffer = new byte[1024] ;
                {
                    while (true)
                    {
                            int buflen = file.Read(buffer, 0, 1024);
                            if (buflen == 0)
                            {
                                break;
                            }
                            fixed (void* dptr = &buffer[0])
                            {
                                filechecksum = Update(filechecksum, dptr, buflen);
                            }
                    }
                }
            }
            return filechecksum;
        }

        public void ShowTable()
        {
            int words_per_line = 8;
            int lines = 256 / words_per_line;
            int wordidx = 0;
            for (int line=0; line<lines; line++)
            {
                string linenostr = line.ToString("D4");
                Console.Write($" {linenostr} : ");
                for (int word=0; word<words_per_line; word++, wordidx++ )
                {
                    string wordstr = CRCtable[wordidx].ToString("x4");
                    Console.Write($" {wordstr}");
                }
                Console.WriteLine();
            }

        }
        public void Generate(ushort poly)
        {
            ushort[] table = new ushort[256];
            byte b = 0;
            ushort reg;
            ushort augumented;
            while (true)
            {
                augumented = (ushort)(b * 0x0100) ;
                string augstr = augumented.ToString("x4");
                reg = 0;
                bool applyxor;
                for (int i=0; i<24; i++)
                {

                    if ((reg & 0x8000) == 0) applyxor = false;
                    else applyxor = true ;

                    if ((augumented & 0x8000) == 0)
                    {
                        reg *= 2;
                    }
                    else
                    {
                        reg *= 2;
                        reg++;
                    }
                    augumented *= 2;
                    if (applyxor) reg ^= poly;

                }
                table[b] = reg;

                if (b == 0xff) break;
                b++;
            }
            CRCtable = table;
        }

        public CRC()
        {
            CRCtable = CRC.CRCtableDefault;
        }

    }

    public class NoiseCRC : CRC
    {
        public int NoiseLevel = 0;
        Random ByteNumGenerator;
        Random BitNumGenerator;
        public NoiseCRC(int level)
        {
            NoiseLevel = level;
            ByteNumGenerator = new Random(7);
            BitNumGenerator = new Random(11);
        }
        public override unsafe ushort Checksum(void* ptr, int blocklen)
        {
            byte[] noisyblock = new byte[blocklen];
            byte* src = (byte*)ptr;
            for (int i = 0; i < blocklen ; i++)
            {
                noisyblock[i] = src[i];
            }

            for (int nl=0; nl < NoiseLevel; nl++)
            {
                int bindex = ByteNumGenerator.Next(blocklen);
                int bitnum = BitNumGenerator.Next(8);
                byte mask = (byte)(1 << bitnum) ;
                string maskstr = mask.ToString("x2");
                Console.WriteLine($"Adding noise to byte {bindex} with mask 0x{maskstr}");
                noisyblock[bindex] = (byte)(noisyblock[bindex] ^ mask);
            }
            fixed (void* dptr = &noisyblock[0])
            {
                return Update(0, dptr, blocklen);
            }
        }
    }
}

// Copyright © 2019 TOPR llc.

// Permission to use, copy, modify, and/or distribute this software for any purpose with or without fee is 
// hereby granted, provided that the above copyright notice and this permission notice appear in all copies.

// THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES WITH REGARD TO THIS SOFTWARE
// INCLUDING ALL IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS.IN NO EVENT SHALL THE AUTHOR BE LIABLE
// FOR ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES WHATSOEVER RESULTING FROM
// LOSS OF USE, DATA OR PROFITS, WHETHER IN AN ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION,
// ARISING OUT OF OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.

// All questions may please be addressed to contact@toprllc.com

using System;
namespace ipadr
{
    public interface IPAddress
    {
        bool Analyze(string adr);
        bool SetMask(string mask);
        void Show();
    }
    public class IPV4Address : IPAddress
    {


        private int subnet_bits;
        private UInt32 address;
        private IPV4Address subnet_mask;

        public IPV4Address()
        {
            Console.WriteLine("V4 address analysis");
        }
        public bool Analyze(string adr)
        {
            byte[] octets = new byte[4];
            string ip = null;
            string snl = null;
            int slashpos = adr.IndexOf('/');
            if (slashpos >= 0)
            {
                snl = adr.Substring(slashpos + 1);
                ip = adr.Substring(0, slashpos);
                try
                {
                    subnet_bits = int.Parse(snl);
                    if ((subnet_bits < 1) || (subnet_bits > 31))
                    {
                        Console.WriteLine("Invalid value for Subnet Mask Length {0,1}", subnet_bits);
                        return false;
                    }
                    SetSubnetMaskBits(subnet_bits);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Subnet Mask length is invalid {0,1}", snl);
                    return false;
                }
            }
            else
            {
                ip = adr;
            }

            try
            {
                for (int ocn = 0; ocn < 3; ocn++)
                {
                    int dp = ip.IndexOf(".");
                    if (dp < 1)
                    {
                        Console.WriteLine("Invalid IP Address");
                        return false;
                    }
                    string nxt = ip.Substring(0, dp);
                    int ocv = int.Parse(nxt);
                    if ((ocv < 0) || (ocv > 255))
                    {
                        Console.WriteLine("Invalid octet {0,1}", ocv);
                        return false;
                    }
                    octets[ocn] = (byte)ocv;
                    //Console.WriteLine($"Set Octet {ocn} value {ocv}");
                    ip = ip.Substring(dp + 1);
                    if (ocn == 2)
                    {
                        //Console.WriteLine($"Last octet {ip}");
                        ocv = int.Parse(ip);
                        if ((ocv < 0) || (ocv > 255))
                        {
                            Console.WriteLine("Invalid octet {0,1}", ocv);
                            return false;
                        }
                        octets[3] = (byte)ocv;
                        //Console.WriteLine($"Set Octet 3 value {ocv}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Console.WriteLine("Invalid IP Address {0,1}",ip);
                return false;
            }

            Array.Reverse(octets);
            address = BitConverter.ToUInt32(octets);
            return true;
        }
        public void Show()
        {
            Console.WriteLine("Image {0,1}", Image());
            Console.WriteLine("Address Class: {0,1}" , Class());
            if (subnet_mask != null)
            {
                Console.WriteLine("Subnet Mask: ");
                subnet_mask.Show();
                UInt32 network_id, host_id;
                network_id = address & subnet_mask.address;
                host_id = address ^ network_id;
                IPV4Address network = new IPV4Address();
                network.address = network_id;
                Console.WriteLine("Network Id");
                network.Show();
                IPV4Address host = new IPV4Address();
                host.address = host_id;
                Console.WriteLine("Host Id");
                host.Show();
            }
        }

        public string Image()
        {
            byte[] adrbytes = BitConverter.GetBytes(address);
            Array.Reverse(adrbytes);
            string image = adrbytes[0].ToString() + "."
                         + adrbytes[1].ToString() + "."
                         + adrbytes[2].ToString() + "."
                         + adrbytes[3].ToString();
            if (subnet_bits > 0)
            {
                image = image + "/" + subnet_bits.ToString();
            }
            return image;
        }

        public string Class()
        {
            if ((0x80000000 & address) == 0)
            {
                if ((0x10000000 & address) == 0)
                {
                    return "A";
                }
                else
                {
                    return "loopback";
                }
            }

            if ((0x40000000 & address) == 0)
            {
                return "B";
            }

            if ((0x20000000 & address) == 0)
            {
                return "C";
            }

            if ((0x10000000 & address) == 0)
            {
                return "D";
            }

            return "E";
        }

        public bool SetMask(string mask)
        {
            if (subnet_bits != 0)
            {
                Console.WriteLine("Subnet mask already inferred to be {0,1} bits", subnet_bits);
                return false;
            }
            IPV4Address maskadr = new IPV4Address();
            maskadr.Analyze(mask);
            if (maskadr.subnet_bits > 0)
            {
                Console.WriteLine("Syntax error in mask address");
                return false;
            }
            if (!ValidMask(maskadr))
            {
                return false;
            }
            subnet_mask = maskadr;
            return true;
        }

        private void SetSubnetMaskBits(int subnet_bits)
        {
            uint mask = 0;
            uint bit = 0x80000000;
            for (int bitnum=0; bitnum < subnet_bits; bitnum++)
            {
                uint nextbit = bit >> bitnum;
                mask |= nextbit;
            }
            subnet_mask = new IPV4Address();
            subnet_mask.address = mask ;
        }

        public static bool ValidMask(IPV4Address madr)
        {
            uint bit = 0x80000000;
            int bitnum=0;
            for ( ; bitnum < 32; bitnum++)
            {
                uint nextbit = bit >> bitnum;
                if ((nextbit & madr.address) == 0)
                {
                    break;
                }
            }
            if (bitnum >= 31)
            {
                Console.WriteLine("Invalid Mask. No bits left for host id");
                return false;
            }
            Console.WriteLine("Mask length {0,1} bits", bitnum);
            for (bitnum++ ; bitnum < 32; bitnum++)
            {
                uint nextbit = bit >> bitnum;
                if ((nextbit & madr.address) != 0)
                {
                    Console.WriteLine("Invalid network mask. Expecting sequence of 0 bits");
                    return false;
                }
            }
            return true;
        }
    }

    public class IPV6Address : IPAddress
    {
        public IPV6Address()
        {
            Console.WriteLine("V6 address analysis");
        }
        public bool Analyze(string adr)
        {
            return false;
        }
        public bool SetMask(string mask)
        {
            return false;
        }
        public void Show()
        {

        }
    }
}

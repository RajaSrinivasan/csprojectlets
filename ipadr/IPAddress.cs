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
        void AnalyzeWithMask(string adr, string mask);
        void AnalyzeMask(string mask);
        void Show();
    }
    public class IPV4Address : IPAddress
    {


        private int subnet_bits;

        private Int32 address;

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
            address = BitConverter.ToInt32(octets);
            return true;
        }
        public void Show()
        {
            Console.WriteLine("Image {0,1}", Image());
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

        public void AnalyzeWithMask(string adr, string mask)
        {

        }
        public void AnalyzeMask(string mask)
        {

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
        public void AnalyzeWithMask(string adr, string mask)
        {

        }
        public void AnalyzeMask(string mask)
        {

        }
        public void Show()
        {

        }
    }
}

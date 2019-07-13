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
        private byte[] octets = new byte[4] ;
        private int subnet_bits;

        public IPV4Address()
        {
            Console.WriteLine("V4 address analysis");
        }
        public bool Analyze(string adr)
        {
            string ip = null;
            string snl = null;
            int slashpos = adr.IndexOf('/');
            if (slashpos >= 0)
            {
                snl = adr.Substring(slashpos + 1);
                ip = adr.Substring(0, slashpos - 1);
                try
                {
                    subnet_bits = int.Parse(snl);
                    if ((subnet_bits < 1) || (subnet_bits > 31))
                    {
                        Console.WriteLine("Invalid value for Subnet Mask Length {0,1}", subnet_bits);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Subnet Mask length is invalid {0,1}", snl);
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

                    ip = ip.Substring(dp + 1);
                    if (ocn == 2)
                    {
                        ocv = int.Parse(ip);
                        if ((ocv < 0) || (ocv > 255))
                        {
                            Console.WriteLine("Invalid octet {0,1}", ocv);
                            return false;
                        }
                        octets[3] = (byte)ocv;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Invalid IP Address");
                return false;
            }
            Console.Write("IP Address {0,1}", ip);
            Console.Write("Subnet: ");
            if (snl != null)
            {
                Console.WriteLine("Length in bits {0,1}", snl);
            }
            else
            {
                Console.WriteLine("unknown");
            }
            return true;
        }
        public void Show()
        {
            Console.WriteLine("{0,1}.{1,1}.{2,1}.{3,1}", octets[0], octets[1], octets[2], octets[3]);
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

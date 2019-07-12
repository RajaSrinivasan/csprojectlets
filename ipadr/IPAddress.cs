using System;
namespace ipadr
{
    public interface IPAddress
    {
        void Analyze(string adr);
        void AnalyzeWithMask(string adr, string mask);
        void AnalyzeMask(string mask);
    }
    public class IPV4Address : IPAddress
    {
        public IPV4Address()
        {
            Console.WriteLine("V4 address analysis");
        }
        public void Analyze(string adr)
        {

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
        public void Analyze(string adr)
        {

        }
        public void AnalyzeWithMask(string adr, string mask)
        {

        }
        public void AnalyzeMask(string mask)
        {

        }
    }
}

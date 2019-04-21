using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ppm
{
    public class Passbase
    {
        const string pwsalt = "Pack my box with five dozen liquor jugs.";
        private Cli cli;

        public string Hex(byte[] bin)
        {
            StringBuilder sb = new StringBuilder();
            for (int i=0; i < bin.Length; i++)
            {
                sb.Append(bin[i].ToString("x2"));
            }
            return sb.ToString();
        }
        private byte[] NormalizedPassword ;
        public string Password
        {
            get
            {
                return Hex(NormalizedPassword);
            }
        }
        public void SetPassword(string pwd)
        {
            SHA256 digest = SHA256.Create();
            NormalizedPassword = digest.ComputeHash(Encoding.UTF8.GetBytes(pwsalt + pwd));
        }

        private string CreationTime;
        private string FileContents;
        private byte[] EncryptedContents;
        private List<Membership> Memberships;

        public Passbase(Cli _cli)
        {
            cli = _cli;
        }

        public bool Load()
        {

            EncryptedContents = File.ReadAllBytes(cli.Filename);
            Decrypt();
            return true;
        }

        private void Encrypt()
        {
            using (AesManaged aes = new AesManaged())
            {                
                aes.Mode = CipherMode.CBC;
                aes.Key = NormalizedPassword ;
                ICryptoTransform xfrm = aes.CreateEncryptor();
                byte[] iv = aes.IV ;                

            }
        }

        private void Decrypt()
        {

        }

        public bool Save()
        {

            FileContents = FileContents + "\n# " + CreationTime ;
            Encrypt();
            File.WriteAllBytes(cli.Filename, EncryptedContents);
            return true;
        }

        public bool Create()
        {
            if (File.Exists(cli.Filename))
            {
                Console.WriteLine($"{cli.Filename} already exists. Not creating.");
                return true;
            }
            DateTime today = DateTime.Today;
            CreationTime = today.ToString();

            return Save();
        }

        public Membership Lookup(string ctx, string uname)
        {
            return new Membership();
        }
        public bool Add(string ctx, string uname)
        {
            string pwd = cli.GetPassword(uname + "[" + ctx + "]" );
            return true;
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Linq;

namespace ppm
{
    public class Passbase
    {
        const string pwsalt = "Pack my box with five dozen liquor jugs.";
        string dashes = new string('-' , 64);
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
            MD5 digest = MD5.Create();
            NormalizedPassword = digest.ComputeHash(Encoding.UTF8.GetBytes(pwsalt + pwd));
        }

        private string CreationTime;
        private string FileContents;
        private byte[] EncryptedContents;
        private List<Membership> Memberships;
        private byte[] InitializationVector;

        public Passbase(Cli _cli)
        {
            cli = _cli;
        }

        public bool Load()
        {
            BinaryReader br;

            try
            {
                br = new BinaryReader(new FileStream(cli.Filename , FileMode.Open));               
            }
            catch (IOException e)
            {
                Console.WriteLine($"{e.Message} \nError opening {cli.Filename}");
                return false;
            }
            AesManaged aes = new AesManaged();
            InitializationVector = br.ReadBytes(aes.IV.Length);
            FileInfo info = new System.IO.FileInfo(cli.Filename);
            EncryptedContents = br.ReadBytes((int)info.Length);
            return Decrypt();
        }


        private void Encrypt()
        {
            using (AesManaged aes = new AesManaged())
            {                
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;
                aes.Key = NormalizedPassword ;
                ICryptoTransform xfrm = aes.CreateEncryptor();
                byte[] iv = aes.IV ;
                InitializationVector = aes.IV;

                using (MemoryStream ms = new MemoryStream())
                {
                    //ms.Write(aes.IV, 0, aes.IV.Length);
                    Console.WriteLine($"IV Length {aes.IV.Length}");
                    //byte[] lenbytes = BitConverter.GetBytes(FileContents.Length);
                    //ms.Write(lenbytes, 0, lenbytes.Length);
                    using (CryptoStream cs = new CryptoStream(ms, xfrm, CryptoStreamMode.Write))
                    {
                        StreamWriter writer = new StreamWriter(cs);
                        writer.Write(FileContents);
                        cs.Write(Encoding.UTF8.GetBytes(FileContents) , 0 , FileContents.Length);
                        cs.FlushFinalBlock();
                        Console.WriteLine($"Wrote {FileContents}");
                        cs.Close();
                    }
                    ms.Close();
                    EncryptedContents = ms.ToArray();
                }
                aes.Clear();
                Console.WriteLine($"Final length {EncryptedContents.Length}");
            }
        }

        private bool Decrypt()
        {
            using (AesManaged aes = new AesManaged())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.Zeros;
                aes.Key = NormalizedPassword;
                aes.IV = InitializationVector;

                //ArraySegment<byte> enc = new ArraySegment<byte>(EncryptedContents, aes.IV.Length , EncryptedContents.Length - aes.IV.Length - 1) ;
                MemoryStream ms = new MemoryStream(EncryptedContents);
                ICryptoTransform xfrm = aes.CreateDecryptor( aes.Key , aes.IV );
                CryptoStream cs = new CryptoStream(ms, xfrm, CryptoStreamMode.Read);
                StreamReader reader = new StreamReader(cs);

                string conts;
                conts = reader.ReadToEnd();
                aes.Clear();
                Console.WriteLine(conts);
            }
            return true;
        }

        public bool Save()
        {

            FileContents = "# Creation Date " + CreationTime + " by Srini ----------------------------------$" ;
            Encrypt();
            BinaryWriter bw;
            try
            {
                bw = new BinaryWriter(new FileStream( cli.Filename , FileMode.Create));
            }
            catch (IOException e)
            {
                Console.WriteLine($"{e.Message} \n Cannot create file {cli.Filename}");
                return false;
            }
            bw.Write(InitializationVector);
            bw.Write(EncryptedContents);
            bw.Close();
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

        public bool List()
        {
            if (Load())
            {
                Console.WriteLine(FileContents);
                return true;
            }
            return false;
        }

        public bool Test()
        {
            FileContents = "# Creation Date " + CreationTime + " by Srini ----------------------------------$";
            Encrypt();
            Decrypt();
            return true;
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

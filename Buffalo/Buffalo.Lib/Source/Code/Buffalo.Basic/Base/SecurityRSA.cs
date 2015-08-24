using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Buffalo.Basic.Base
{
    public class Base_RSA
    {
        public static string GeneratePrivateKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            return rsa.ToXmlString(true);
        }

        public static string GeneratePublicKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            return rsa.ToXmlString(false);
        }

        public static string EncryptString(string sSource, string sPublicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string plaintext = sSource;
            rsa.FromXmlString(sPublicKey);
            byte[] cipherbytes;
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plaintext), false);
            StringBuilder sbString = new StringBuilder();
            for (int i = 0; i < cipherbytes.Length; i++)
            {
                sbString.Append(cipherbytes[i] + ",");
            }
            return sbString.ToString();
        }

        public static string DecryptString(string sSource, string sPrivateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(sPrivateKey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(sSource), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }

    }
}

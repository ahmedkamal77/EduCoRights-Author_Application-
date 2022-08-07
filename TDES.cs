using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace tribleedes
{
     public class TDES
    {

        private TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
        public TDES (string key)
        {
           
            des.Key = UTF8Encoding.UTF8.GetBytes(key); //encoding type
            des.Mode = CipherMode.ECB;  //electronic code book
            des.Padding = PaddingMode.PKCS7;

        }
        public void encryptfile(string filepath)
        {
            byte[] bytes = File.ReadAllBytes(filepath);
            byte[] enc_byte = des.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            File.WriteAllBytes(filepath, enc_byte);

        }
        public void decfile(string filepath)
        {
            byte[] bytes = File.ReadAllBytes(filepath);
            byte[] dec_byte = des.CreateDecryptor().TransformFinalBlock(bytes, 0, bytes.Length);
            File.WriteAllBytes(filepath, dec_byte);

        }

    }
}


using Microsoft.Owin.Security.DataProtection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GroupBrush.BL.DataProtectors
{
    public class AesDataProtector : IDataProtector
    {
        private byte[] _IV;
        private byte[] _key;
        public AesDataProtector(string password, string salt)
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(password, Encoding.ASCII.GetBytes(salt));
            _key = key.GetBytes(256 / 8);
            _IV = key.GetBytes(128 / 8);
        }
        public byte[] Protect(byte[] userData)
        {
            byte[] encrypted;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor,
                    CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(userData, 0, userData.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }
        public byte[] Unprotect(byte[] protectedData)
        {
            byte[] output = null;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(protectedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        byte[] buffer = new byte[8];
                        using (MemoryStream msOutput = new MemoryStream())
                        {
                            int read;
                            while ((read = csDecrypt.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                msOutput.Write(buffer, 0, read);
                            }
                            output = msOutput.ToArray();
                        }
                    }
                }
            }
            return output;
        }
    }
}

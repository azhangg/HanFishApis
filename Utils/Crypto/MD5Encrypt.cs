using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Utils.Crypto
{
    public static class MD5Encrypt
    {
        public static string EncryptTo32(string encryptText, bool isUpper = false)
        {
            if (string.IsNullOrWhiteSpace(encryptText)) return null;
            MD5 md5 = MD5.Create();
            byte[] hashVal = md5.ComputeHash(Encoding.UTF8.GetBytes(encryptText));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashVal)
            {
                sb.Append(b.ToString("x2"));
            }
            if (isUpper)
                return sb.ToString().ToUpper();
            else
                return sb.ToString().ToLower();
        }
    }
}

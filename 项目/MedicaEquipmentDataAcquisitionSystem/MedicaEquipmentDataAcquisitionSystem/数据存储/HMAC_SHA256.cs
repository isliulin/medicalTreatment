using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MEDAS
{
    public class HMAC_SHA256
    {
        //public static string CreateToken(string message, string secret)
        //{
        //    secret = secret ?? "";
        //    var encoding = new System.Text.ASCIIEncoding();
        //    byte[] keyByte = encoding.GetBytes(secret);
        //    byte[] messageBytes = encoding.GetBytes(message);
        //    using (var hmacsha256 = new HMACSHA256(keyByte))
        //    {
        //        byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
        //        return Convert.ToBase64String(hashmessage);
        //    }
        //}
        public static byte[] CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return hashmessage;
            }
        }
    }
}

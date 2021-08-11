using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace kat.Code
{
    public class HashingExample
    {
        public string GetHashedText_MD5(string valueToHash)
        {
            byte[] valueAsBytes = ASCIIEncoding.ASCII.GetBytes(valueToHash);
            byte[] valueT = MD5.HashData(valueAsBytes);
            string hashedValueAsString = Convert.ToBase64String(valueT);

            return hashedValueAsString;
        }

        public string GetHashedText_BCrypt(string valueToHash)
        {
            string salt = genSalt_BCrypt();

            return BCrypt.Net.BCrypt.HashPassword(valueToHash, salt);
        }

        public string genSalt_BCrypt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(7);
        }

        public bool verifyWithBCrypt(string clearText, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(clearText, hash);
        }
    }
}

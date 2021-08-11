using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kat.Code
{
    public class CryptExample
    {
        public string Encrypt(string payload, IDataProtector _protector)
        {
            return _protector.Protect(payload);
        }
        public string Decrypt(string protectecPayload, IDataProtector _protector)
        {
            return _protector.Unprotect(protectecPayload);
        }
    }
}

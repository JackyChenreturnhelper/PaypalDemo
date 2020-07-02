using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaypalDemo.Infrastructure.Helper
{
    public static class EncryptionHelper
    {
        public static string GetBasicAuthorizationString(string clientID, string secret)
        {
            return ToBase64String($"{clientID}:{secret}");
        }

        private static string ToBase64String(string s)
        {
            return System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace OperateHSL.Core.Helpers
{
    public class EncryptorHelper
    {
        public static string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static long GetCurrentUnixTimestamp()
        {
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - unixEpoch;
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}

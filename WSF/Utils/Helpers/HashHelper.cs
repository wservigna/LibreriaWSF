using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WSF.Utils.Helpers
{
    internal class HashHelper
    {

        public static byte[] Hash(string value) {
            SHA256 myHash = SHA256Managed.Create();
            byte[] byteArray = Encoding.UTF8.GetBytes(value);
            MemoryStream stream = new MemoryStream(byteArray);
            return myHash.ComputeHash(stream);
        }

        public static byte[] Hash(int value)
        {
            SHA256 myHash = SHA256Managed.Create();
            byte[] byteArray = Encoding.UTF8.GetBytes(value.ToString());
            MemoryStream stream = new MemoryStream(byteArray);
            return myHash.ComputeHash(stream);
        }

        public static byte[] Hash(byte value)
        {
            SHA256 myHash = SHA256Managed.Create();
            byte[] byteArray = Encoding.UTF8.GetBytes(value.ToString());
            MemoryStream stream = new MemoryStream(byteArray);
            return myHash.ComputeHash(stream);
        }
    }
}

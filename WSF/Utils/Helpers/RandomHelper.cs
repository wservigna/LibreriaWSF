using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSF.Utils.Helpers
{
    internal class RandomHelper
    {
        private static readonly Random Rnd = new Random();

        private const string CodeChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateCode(int length)
        {
            var codeBuilder = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                codeBuilder.Append(CodeChars[GenerateNumber(0, CodeChars.Length)]);
            }

            return codeBuilder.ToString();
        }

        public static int GenerateNumber(int min, int max)
        {
            lock (Rnd)
            {
                return Rnd.Next(min, max);
            }
        }
    }
}

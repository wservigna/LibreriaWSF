using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WSF.Utils.Helpers
{
    internal class CryptoHelper
    {
        public static string Encrypt(string inputText)

        {

            byte[] inputBytes = Encoding.ASCII.GetBytes(inputText);

            byte[] encripted;

            RijndaelManaged cripto = new RijndaelManaged();

            using (MemoryStream ms =

                new MemoryStream(inputBytes.Length))

            {

                using (CryptoStream objCryptoStream =

                    new CryptoStream(ms,

                           cripto.CreateEncryptor(Encoding.ASCII.GetBytes(WSFConsts.RandomKey), Encoding.ASCII.GetBytes(WSFConsts.Copyright)),

                           CryptoStreamMode.Write))

                {

                    objCryptoStream.Write(inputBytes, 0, inputBytes.Length);

                    objCryptoStream.FlushFinalBlock();

                    objCryptoStream.Close();

                }

                encripted = ms.ToArray();

            }

            return Convert.ToBase64String(encripted);

        }



        public static string Decrypt (string inputText)

        {

            byte[] inputBytes = Convert.FromBase64String(inputText);
            byte[] resultBytes = new byte[inputBytes.Length];
            string textoLimpio = String.Empty;

            RijndaelManaged cripto = new RijndaelManaged();

            using (MemoryStream ms = new MemoryStream(inputBytes))
            {

                using (CryptoStream objCryptoStream =
                new CryptoStream(ms, cripto.CreateDecryptor(Encoding.ASCII.GetBytes(WSFConsts.RandomKey),
                Encoding.ASCII.GetBytes(WSFConsts.Copyright)), CryptoStreamMode.Read))
                                {

                    using (StreamReader sr =                                   
                        new StreamReader(objCryptoStream, true))

                    {
                        textoLimpio = sr.ReadToEnd();
                    }
                }
            }

            return textoLimpio;


        }

    }
}

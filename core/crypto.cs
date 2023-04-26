using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Core
{
    public class Crypto
    {
        public static string Encrypt(string plaintext, string password)
        {
            if (plaintext == null) return String.Empty;
            if (plaintext == String.Empty) return String.Empty;

            if (password == null) return String.Empty;
            if (password == String.Empty) return String.Empty;

            try
            {
                char[] textArray = plaintext.ToCharArray();
                char[] keyArray = password.ToCharArray();
                byte[] encKeys = new byte[textArray.Length];

                int keyIdx = 0;
                byte code;


                for (int idx = 0; idx < textArray.Length; idx++)
                {
                    if (keyIdx >= password.Length) keyIdx = 0;

                    textArray[idx] ^= (char)14;
                    if ((byte)keyArray[keyIdx] > 100)
                        code = (byte)(keyArray[keyIdx] - 100);
                    else
                        code = (byte)keyArray[keyIdx];

                    encKeys[idx] = (byte)(textArray[idx] + code);
                }

                return Convert.ToBase64String(encKeys, 0, encKeys.Length);
            }
            catch (Exception error)
            {
                Debug.WriteLine("Exception Crypto.Encrypt: "+error.Message);
                return String.Empty;
            }
        }

        public static String Decrypt(String plaintext, String password)
        {
            if (plaintext == null) return String.Empty;
            if (plaintext == String.Empty) return String.Empty;

            if (password == null) return String.Empty;
            if (password == String.Empty) return String.Empty;

            try 
            {
                byte[] textArray = Convert.FromBase64String(plaintext);
                char[] keyArray = password.ToCharArray();
                byte[] decKeys = new byte[textArray.Length];

                int keyIdx = 0;
                byte code;
                byte value;


                for (int idx = 0; idx < textArray.Length; idx++)
                {
                    if (keyIdx >= password.Length) keyIdx = 0;

                    if ((byte)keyArray[keyIdx] > 100)
                        code = (byte)(keyArray[keyIdx] - 100);
                    else
                        code = (byte)keyArray[keyIdx];

                    if (code > textArray[idx])
                        value = (byte)(code - textArray[idx]);
                    else
                        value = (byte)(textArray[idx] - code);

                    decKeys[idx] = value;
                    decKeys[idx] ^= (byte)14;
                }

                return System.Text.Encoding.UTF8.GetString(decKeys);
            } 
            catch (Exception error)
            {
                Debug.WriteLine("Exception Crypto.Encrypt: " + error.Message);
                return String.Empty;
            }
        }     


    }
}
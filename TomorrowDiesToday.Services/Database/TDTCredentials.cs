using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TomorrowDiesToday.Services.Database
{
    public class TDTCredentials : AWSCredentials
    {
        private string _keyId = "rsqfh1kp6DLiwIDczn7G4N0Xy5I15/7+";
        private string _key = "CwE8Hr5t/Dc2WiN1ixPbBpQLKL4NrcN2EtAp9TS9mXkclQefxf9a1vS77MZXA0W1";

        public override ImmutableCredentials GetCredentials()
        {
            var keyId = Decrypt(_keyId);
            var key = Decrypt(_key);
            var credentials = new ImmutableCredentials(keyId, key, null);
            return credentials;
        }

        // This is temporary; need to encode AWS DynamoDB Key
        //public static string EncryptString(string inputString)
        //{
        //    MemoryStream memStream = null;

        //    byte[] key = { };
        //    byte[] IV = { 12, 21, 43, 17, 57, 35, 67, 27 };
        //    string encryptKey = "xTl7qB4z"; // MUST be 8 characters
        //    key = Encoding.UTF8.GetBytes(encryptKey);
        //    byte[] byteInput = Encoding.UTF8.GetBytes(inputString);
        //    DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        //    memStream = new MemoryStream();
        //    ICryptoTransform transform = provider.CreateEncryptor(key, IV);
        //    CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
        //    cryptoStream.Write(byteInput, 0, byteInput.Length);
        //    cryptoStream.FlushFinalBlock();

        //    return Convert.ToBase64String(memStream.ToArray());
        //}

        private string Decrypt(string inputString)
        {
            byte[] IV = { 12, 21, 43, 17, 57, 35, 67, 27 };
            string encryptKey = "xTl7qB4z"; // MUST be 8 characters
            byte[] key = Encoding.UTF8.GetBytes(encryptKey);
            byte[] byteInput = Convert.FromBase64String(inputString);
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
            MemoryStream memStream = new MemoryStream();
            ICryptoTransform transform = provider.CreateDecryptor(key, IV);
            CryptoStream cryptoStream = new CryptoStream(memStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(byteInput, 0, byteInput.Length);
            cryptoStream.FlushFinalBlock();

            Encoding encoding = Encoding.UTF8;
            var decryptedData = memStream.ToArray();

            provider.Dispose();
            memStream.Dispose();
            cryptoStream.Dispose();

            return encoding.GetString(decryptedData);
        }
    }
}

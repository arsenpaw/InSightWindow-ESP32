using System;
using System.Security.Cryptography;
using System.Text;

namespace IoT_App.Services
{
    public class AesService : IAesService
    {
        public Aes Aes { get; set; }

        public AesService()
        {
            Aes = new Aes(CipherMode.CBC);

            Aes.Key = _getProperByteData(Encoding.UTF8.GetBytes(AppSettings.AesPasswrod));
            Aes.IV = _getProperByteData(Encoding.UTF8.GetBytes(AppSettings.AesPasswrod));

        }

        private byte[] _getProperByteData(byte[] oldArray)
        {
            if (oldArray.Length % 16 == 0)
            {
                return oldArray;
            }

            var newArray = new byte[oldArray.Length + (16 - oldArray.Length % 16)];
            Array.Copy(oldArray, newArray, oldArray.Length);
            return newArray;
        }

        public byte[] EncryptData(string data)
        {
            var dataByteArray = Encoding.UTF8.GetBytes(data);
            return Aes.Encrypt(_getProperByteData(dataByteArray));

        }
        public string DecryptData(byte[] encryptedData)
        {
            var encoder = new UTF8Encoding();

            var decryptedData = Aes.Decrypt(encryptedData);
            var data = encoder.GetString(decryptedData, 0, decryptedData.Length);
            return (data);
        }
    }
}

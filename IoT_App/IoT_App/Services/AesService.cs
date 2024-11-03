using System;
using System.Security.Cryptography;
using System.Text;

namespace IoT_App.Services
{
    public class AesService : IAesService
    {
        public Aes Aes { get; set; }

        public AesService(string password)
        {
            Aes = new Aes(CipherMode.CBC);
            Aes.Key = _getProperByteData(Encoding.UTF8.GetBytes(password));
            Aes.IV = _getProperByteData(Encoding.UTF8.GetBytes(password));
        }

        private byte[] _getProperByteData(byte[] oldArray)
        {
            if (oldArray.Length % 16 == 0)
                return oldArray;
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
            var decryptedData = Aes.Decrypt(encryptedData);
            return Encoding.UTF8.GetString(decryptedData,0,encryptedData.Length);
        }
    }
}

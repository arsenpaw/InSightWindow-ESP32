using System.Security.Cryptography;

namespace IoT_App.Services
{
    public interface IAesService
    {
        Aes Aes { get; set; }

        string DecryptData(byte[] encryptedData);
        byte[] EncryptData(string data);
    }
}
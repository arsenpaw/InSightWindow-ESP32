using System.Security.Cryptography;

namespace IoT_App.Services.Interfaces
{
    public interface IAesService
    {
        Aes Aes { get; set; }

        string DecryptData(byte[] encryptedData);
        byte[] EncryptData(string data);
    }
}
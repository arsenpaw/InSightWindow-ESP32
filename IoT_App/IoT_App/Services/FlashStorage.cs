using IoT_App.Models;
using nanoFramework.Json;
using System.Diagnostics;
using System.IO;


namespace IoT_App.Services
{
    public class FlashStorage : IFlashStorage
    {
        public const string userSettingsFilePath = "I:\\UserSetting.txt";

        public const string fileInternalPath = "I:\\InternalStorage.txt";

        public FlashStorage()
        {
            if (!File.Exists(userSettingsFilePath) || !File.Exists(fileInternalPath))
            {
                File.Delete(userSettingsFilePath);
                File.Delete(fileInternalPath);
                Debug.WriteLine("+++++ Creating a file +++++");
                File.Create(fileInternalPath);
                var fsUser = File.Create(userSettingsFilePath);
                var userSettings = new UserSetting();
                var dataToWrite = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userSettings));
                fsUser.Write(dataToWrite, 0, dataToWrite.Length);
                fsUser.Dispose();
            }
        }

        public void SetUserSettings(UserSetting data)
        {
            Debug.WriteLine("+++++ Writing to a sample file +++++");
            var fs = new FileStream(userSettingsFilePath, FileMode.Open, FileAccess.Write);
            var userSettings = JsonConvert.SerializeObject(data);
            var dataToWrite = System.Text.Encoding.UTF8.GetBytes(userSettings);
            fs.Write(dataToWrite, 0, dataToWrite.Length);
            fs.Dispose();
        }
        public UserSetting GetUserSettings()
        {
            Debug.WriteLine("+++++ Reading from a sample file +++++");
            var fs = new FileStream(userSettingsFilePath, FileMode.Open, FileAccess.Read);
            byte[] fileContent = new byte[fs.Length];
            fs.Read(fileContent, 0, (int)fs.Length);
            var userSettings = System.Text.Encoding.UTF8.GetString(fileContent, 0, fileContent.Length);
            return (UserSetting)JsonConvert.DeserializeObject(userSettings, typeof(UserSetting));
        }
    }
}

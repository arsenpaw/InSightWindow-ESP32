using IoT_App.Models.Store;
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
                Debug.WriteLine("+++++ Creating a settings file +++++");
                File.Create(fileInternalPath);
                _initUserSettings();
            }
        }
        private void _initUserSettings()
        {
            var fsUser = File.Create(userSettingsFilePath);
            var userSettings = new UserSettings();
            var dataToWrite = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(userSettings));
            fsUser.Write(dataToWrite, 0, dataToWrite.Length);
            fsUser.Dispose();
        }

        public void SetUserSettings(UserSettings data)
        {
            Debug.WriteLine("+++++ Writing to a settings file +++++");
            var fs = new FileStream(userSettingsFilePath, FileMode.Open, FileAccess.Write);
            var userSettings = JsonConvert.SerializeObject(data);
            var dataToWrite = System.Text.Encoding.UTF8.GetBytes(userSettings);
            fs.Write(dataToWrite, 0, dataToWrite.Length);
            fs.Dispose();
        }
        public UserSettings GetUserSettings()
        {
            Debug.WriteLine("+++++ Reading from a settings file +++++");
            var fs = new FileStream(userSettingsFilePath, FileMode.Open, FileAccess.Read);
            byte[] fileContent = new byte[fs.Length];
            fs.Read(fileContent, 0, (int)fs.Length);
            fs.Dispose();
            if (fileContent[fileContent.Length - 1] == fileContent[fileContent.Length - 2])
            {
                fileContent[fileContent.Length - 1] = 0;
            }
            var userSettings = System.Text.Encoding.UTF8.GetString(fileContent, 0, fileContent.Length);
            try
            {
                return (UserSettings)JsonConvert.DeserializeObject(userSettings, typeof(UserSettings));
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine("Error Has Ocured and user settings recreated");
                File.Delete(userSettingsFilePath);
                _initUserSettings();
                return new UserSettings();
            }


        }
    }
}

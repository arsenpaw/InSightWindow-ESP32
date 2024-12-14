using IoT_App.Models.Store;
using IoT_App.Services.Interfaces;
using nanoFramework.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace IoT_App.Services
{
    public class FlashStorage : IFlashStorage
    {
        private const string DefaultUserSettingsFilePath = "I:\\UserSetting.txt";
        private const string DefaultFileInternalPath = "I:\\InternalStorage.txt";

        private readonly string _userSettingsFilePath;
        private readonly string _fileInternalPath;
        private readonly object _writeLock = new();

        public FlashStorage()
        {
            _userSettingsFilePath = DefaultUserSettingsFilePath;
            _fileInternalPath = DefaultFileInternalPath;
            EnsureFileExists(_userSettingsFilePath, () => CreateDefaultUserSettings());
            EnsureFileExists(_fileInternalPath);
        }

        public void SetUserSettings(UserSettings data)
        {
            lock (_writeLock)
            {
                Debug.WriteLine("+++++ Writing to a settings file +++++");
                WriteToFile(_userSettingsFilePath, JsonConvert.SerializeObject(data));
            }
        }

        public UserSettings GetUserSettings()
        {
            Debug.WriteLine("+++++ Reading from a settings file +++++");
            var fileContent = ReadFromFile(_userSettingsFilePath);

            if (string.IsNullOrEmpty(fileContent))
            {
                Debug.WriteLine("User settings file is empty. Recreating...");
                CreateDefaultUserSettings();
                return new UserSettings();
            }

            try
            {
                return (UserSettings)JsonConvert.DeserializeObject(fileContent, typeof(UserSettings));
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine($"Error while deserializing user settings: {ex.Message}");
                Debug.WriteLine("Recreating user settings...");
                File.Delete(_userSettingsFilePath);
                CreateDefaultUserSettings();
                return new UserSettings();
            }
        }

        private void EnsureFileExists(string filePath, Action? initializer = null)
        {
            if (!File.Exists(filePath))
            {
                Debug.WriteLine($"Creating file: {filePath}");
                File.Create(filePath).Dispose();
                initializer?.Invoke();
            }
        }

        private void CreateDefaultUserSettings()
        {
            lock (_writeLock)
            {
                var defaultSettings = new UserSettings();
                WriteToFile(_userSettingsFilePath, JsonConvert.SerializeObject(defaultSettings));
            }
        }

        private void WriteToFile(string filePath, string content)
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            var data = System.Text.Encoding.UTF8.GetBytes(content);
            fs.Write(data, 0, data.Length);
        }

        private string ReadFromFile(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
            return System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
    }
}

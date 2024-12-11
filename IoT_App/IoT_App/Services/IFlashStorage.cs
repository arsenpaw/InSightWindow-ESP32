using IoT_App.Models.Store;

namespace IoT_App.Services
{
    public interface IFlashStorage
    {
        UserSettings GetUserSettings();
        void SetUserSettings(UserSettings data);
    }
}
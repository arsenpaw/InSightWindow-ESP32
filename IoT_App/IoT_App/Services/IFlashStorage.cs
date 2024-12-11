using IoT_App.Models;

namespace IoT_App.Services
{
    public interface IFlashStorage
    {
        UserSetting GetUserSettings();
        void SetUserSettings(UserSetting data);
    }
}
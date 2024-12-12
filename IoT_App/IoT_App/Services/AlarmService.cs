using IoT_App.ChainOfResponcebility;
using IoT_App.Models;

namespace IoT_App.Services
{
    public class AlarmService : DataComposeHandler
    {
        private IFlashStorage _flashStorage;
        public AlarmService(IFlashStorage flashStorage)
        {
            _flashStorage = flashStorage;
        }

        public bool GetAlarmStatus()
        {
            var userSettings = _flashStorage.GetUserSettings();
            return false; //isoemn
        }

        public override object DataCompose(AllSensorData request)
        {
            var userSettings = _flashStorage.GetUserSettings();
            if (userSettings.IsProtected)
            {
                request.IsAlarm = GetAlarmStatus();
            }
            else
            {
                request.IsAlarm = false;
            }
            return base.DataCompose(request);
        }
    }
}

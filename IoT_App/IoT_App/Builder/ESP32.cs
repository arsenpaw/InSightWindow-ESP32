using IoT_App.Sensors;
using System;

using System.Text;

namespace IoT_App.Builder
{
    public class ESP32
    {
        private string _parts = string.Empty;

        public void AddPart(string part)
        {
            _parts += part + "\n";
        }

        public void SaveDataToLocalStorage(ISensor sensor, Type sensorType)
        {
            switch (sensorType)
            {
                case Type type when type == typeof(DHT11):
                    AddPart($"Temparature: {((DHT11)sensor).Temparature}");
                    break;
                default:
                    break;
            }
        }
    }
}

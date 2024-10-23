using HttpWebRequestSample;
using IoT_App.Sensors;
using System;
using System.Text;

namespace IoT_App.Builder
{
    public interface IBuilder
    {
        IBuilder EstablishServerConnection();

        IBuilder ConnectToWifi(string ssid, string password);

        IBuilder AddDht11(DHT11 sensor);

        IBuilder AddWaterSensor(WaterSensor sensor);

        ESP32 Build();
    }
}

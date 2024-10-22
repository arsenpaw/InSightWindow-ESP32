using HttpWebRequestSample;
using System;
using System.Text;

namespace IoT_App.Builder
{
    public interface IBuilder
    {
        IBuilder EstablishServerConnection();

        IBuilder ConnectToWifi(string ssid, string password);

        ESP32 Build();
    }
}

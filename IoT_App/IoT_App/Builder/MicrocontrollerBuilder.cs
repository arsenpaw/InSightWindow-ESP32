using HttpWebRequestSample;
using nanoFramework.Networking;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace IoT_App.Builder
{
    public class MicrocontrollerBuilder : IBuilder
    {
        private ESP32 _product;


        public MicrocontrollerBuilder()
        {
            _product = new ESP32();
        }

        public static MicrocontrollerBuilder Create()
        {
            return new MicrocontrollerBuilder();
        }

        public IBuilder EstablishServerConnection()
        {

            return this;
        }

        public IBuilder ConnectToWifi(string ssid, string password)
        {
            bool success = false;
            CancellationTokenSource cs = new(5000);
            Debug.WriteLine("Connecting to Wifi");
            success = WifiNetworkHelper.ConnectDhcp(ssid, password,requiresDateTime: true);
            if (!success)
            {
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {NetworkHelper.Status}.");
            }
            Debug.WriteLine("Connected to Wifi");
            return this;
        }


        public ESP32 Build()
        {
            return _product;
        }

    }
}

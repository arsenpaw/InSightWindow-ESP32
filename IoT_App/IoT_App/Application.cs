using IoT_App.Builder;
using IoT_App.Sensors;
using IoT_App.Services;
using nanoFramework.SignalR.Client;
using System;

using IoT_App.Builder;
using IoT_App.Models;
using IoT_App.Sensors;
using nanoFramework.Json;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;
using nanoFramework.SignalR.Client;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using IoT_App;
using IoT_App.Services;
#if HAS_WIFI
using System.Device.Wifi;
#endif

namespace IoT_App
{
    public class Application
    {
        public IBuilder _esp32Builder { get; set; }

        public Application(IBuilder esp32Builder)
        {
            _esp32Builder = esp32Builder;

        }
        public void Run()
        {
            

            X509Certificate rootCACert = new X509Certificate(AppSettings.dstRootCAX3);

            var esp32 = _esp32Builder
               .ConnectToWifi("PC", "123456789")
               .EstablishServerConnection(
                AppSettings.HUB_URL,
               new HubConnectionOptions()
               {
                   Certificate = rootCACert,
                   SslVerification = SslVerification.NoVerification,
                   SslProtocol = SslProtocols.Tls12,
                   Reconnect = true,
               }
               )
               .AddDht11(new DHT11())
               .AddWaterSensor(new WaterSensor())
               .Build();

            string t = "testWorTestassfasfassafadg";
            Debug.WriteLine($"Original: {t}");
            var encrypted = esp32.AesService.EncryptData(t);
            Debug.WriteLine($"Byte: {encrypted}");
            var decrypted = esp32.AesService.DecryptData(encrypted);
            Debug.WriteLine($"Encrypted:{decrypted}");

            int i = 0;
            do
            {
                Thread.Sleep(1000);
                var d = esp32.ComposeAllDataInfo();
                Debug.WriteLine(JsonConvert.SerializeObject(d));
                esp32.SendDataFromSensorToServer();
                i++;

            }
            while (i > 25);
        }



    }
}

//
// Copyright (c) .NET Foundation and Contributors
// See LICENSE file in the project root for full license information.
//

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
#if HAS_WIFI
using System.Device.Wifi;
#endif

namespace HttpWebRequestSample
{ 

    public class Program
    {
        public static void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)       
                Power.RebootDevice();                
        }

        public static void Main()
        {
            X509Certificate rootCACert = new X509Certificate(Appsettiings.dstRootCAX3);

            var esp32 = MicrocontrollerBuilder.Create()
               .ConnectToWifi("PC", "123456789")
               .EstablishServerConnection(
                Appsettiings.HUB_URL,
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



            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
            while (true)
            {
                var d = esp32.ComposeAllDataInfo();
                Debug.WriteLine(JsonConvert.SerializeObject(d));
                Thread.Sleep(1000);
            }

        }

    }

 }


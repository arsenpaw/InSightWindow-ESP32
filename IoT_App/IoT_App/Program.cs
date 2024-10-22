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
            {
                Power.RebootDevice();
            }          
        }

        public static void Main()
        {
            var esp32 = MicrocontrollerBuilder.Create()
               .ConnectToWifi("PC", "123456789")
               .EstablishServerConnection()
               .Build();
            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
            while (true)
            {
                foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                {

                    Console.WriteLine($"IP Address: {ni.IPv4Address}");
                    Console.WriteLine($"Subnet Mask: {ni.IPv4SubnetMask}");
                    Console.WriteLine($"Gateway: {ni.IPv4GatewayAddress}");
                    Console.WriteLine($"Status: {ni.NetworkInterfaceType}");
                }
                Thread.Sleep(1000);
            }
            var dht11 = new DHT11();
            esp32.SaveDataToLocalStorage(dht11, typeof(DHT11));
            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
        }

    }

 }


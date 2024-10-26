﻿using HttpWebRequestSample;
using IoT_App.Sensors;
using nanoFramework.Json;
using nanoFramework.Networking;
using nanoFramework.SignalR.Client;
using System;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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

        public IBuilder AddDht11(DHT11 sensor)
        {
            _product.DHT11 = sensor;
            return this;
        }
        public IBuilder AddWaterSensor(WaterSensor sensor)
        {
            _product.WaterSensor = sensor;
            return this;
        }
        private static void HubConnection_Closed(object sender, SignalrEventMessageArgs message)
        {
            Debug.WriteLine($"closed received with message: {message.Message}");
        }
        public IBuilder EstablishServerConnection(string url,HubConnectionOptions options)
        {
            _product.HubConnection = new HubConnection(url, options: options);
            var hubConnection = _product.HubConnection;
            do
            {
               hubConnection.Start();
                Debug.WriteLine(hubConnection.State.ToString());
            } while (hubConnection.State != HubConnectionState.Connected);

            hubConnection.Closed += HubConnection_Closed;
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

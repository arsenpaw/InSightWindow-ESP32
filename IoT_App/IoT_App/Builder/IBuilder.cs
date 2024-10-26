﻿using HttpWebRequestSample;
using IoT_App.Sensors;
using nanoFramework.SignalR.Client;
using System;
using System.Text;

namespace IoT_App.Builder
{
    public interface IBuilder
    {
        IBuilder EstablishServerConnection(string url, HubConnectionOptions options);

        IBuilder ConnectToWifi(string ssid, string password);

        IBuilder AddDht11(DHT11 sensor);

        IBuilder AddWaterSensor(WaterSensor sensor);

        ESP32 Build();
    }
}

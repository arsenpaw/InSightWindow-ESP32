﻿using IoT_App.Sensors;
using IoT_App.Services;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.Networking;
using nanoFramework.SignalR.Client;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Threading;

namespace IoT_App.Builder
{
    public class MicrocontrollerBuilder 
    {
        public readonly IServiceCollection Services;

        public MicrocontrollerBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public static MicrocontrollerBuilder CreateBuilder(IServiceCollection services)
        {
            return new MicrocontrollerBuilder(services);
        }

        public void AddDht11()
        {
            Services.AddSingleton(typeof(DHT11), typeof(DHT11));
        }

        public void AddWaterSensor()
        {
            Services.AddSingleton(typeof(WaterSensor), typeof(WaterSensor));
        }

        public void ConfigureServiceConnection(string url, HubConnectionOptions options)
        {
            var headers = new ClientWebSocketHeaders();
            headers[ClaimTypes.DeviceId] = AppSettings.Id.ToString();
            Services.AddSingleton(typeof(HubConnection), new HubConnection(url,headers,options));
        }

        public void ConnectToWifi(string ssid, string password)
        {
            bool success = false;
            CancellationTokenSource cs = new(5000);
            Debug.WriteLine("Connecting to Wifi");
            success = WifiNetworkHelper.ConnectDhcp(ssid, password, requiresDateTime: true);
            if (!success)
            {
                Debug.WriteLine($"Can't get a proper IP address and DateTime, error: {NetworkHelper.Status}.");
            }
            Debug.WriteLine("Connected to Wifi");
        }

        public ESP32 Build()
        {
            Services.AddSingleton(typeof(ESP32));
            var serviceProvider = Services.BuildServiceProvider();

            return (ESP32)serviceProvider.GetRequiredService(typeof(ESP32));
        }
    }
}

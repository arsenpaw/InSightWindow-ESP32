using IoT_App.Sensors;
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
        private readonly IServiceCollection _services;

        public MicrocontrollerBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public static MicrocontrollerBuilder CreateBuilder(IServiceCollection services)
        {
            return new MicrocontrollerBuilder(services);
        }

        public MicrocontrollerBuilder AddDht11()
        {
            _services.AddSingleton(typeof(DHT11), typeof(DHT11));
            return this;
        }

        public MicrocontrollerBuilder AddWaterSensor()
        {
            _services.AddSingleton(typeof(WaterSensor), typeof(WaterSensor));
            return this;
        }

        public MicrocontrollerBuilder ConfigureServiceConnection(string url, HubConnectionOptions options)
        {
            var headers = new ClientWebSocketHeaders();
            headers[ClaimTypes.DeviceId] = AppSettings.Id.ToString();
            _services.AddSingleton(typeof(HubConnection), new HubConnection(url,headers,options));
            return this;
        }

        public MicrocontrollerBuilder ConnectToWifi(string ssid, string password)
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
            return this;
        }

        public ESP32 Build()
        {
            _services.AddSingleton(typeof(ESP32));
            var serviceProvider = _services.BuildServiceProvider();

            return (ESP32)serviceProvider.GetRequiredService(typeof(ESP32));
        }
    }
}

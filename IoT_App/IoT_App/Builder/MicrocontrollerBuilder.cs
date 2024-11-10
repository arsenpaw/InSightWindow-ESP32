using HttpWebRequestSample;
using IoT_App.Sensors;
using IoT_App.Services;
using nanoFramework.Json;
using nanoFramework.Networking;
using nanoFramework.Runtime.Native;
using nanoFramework.SignalR.Client;
using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Security.Claims;
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
        public IBuilder AddAesEncrypting(IAesService aesService)
        {
            _product.AesService = aesService;
            return this;
        }
        private static void HubConnection_Closed(object sender, SignalrEventMessageArgs message)
        {
            Debug.WriteLine($"closed received with message: {message.Message}");
        }
        public IBuilder EstablishServerConnection(string url,HubConnectionOptions options)
        {
            var headers = new ClientWebSocketHeaders();
            headers[ClaimTypes.DeviceId] = AppSettings.Id.ToString();
            _product.HubConnection = new HubConnection(url, options: options, headers: headers);
            var hubConnection = _product.HubConnection;
            do
            {
                Debug.WriteLine("Try to connect to hub");
               hubConnection.Start();
                Debug.WriteLine(hubConnection.State.ToString());
            } while (hubConnection.State != HubConnectionState.Connected);
            hubConnection.Closed += HubConnection_Closed;
            Debug.WriteLine("Connected to hub");
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
            NetworkChange.NetworkAvailabilityChanged += NetworkAvailabilityChanged;
            return this;
        }
        private void NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (!e.IsAvailable)
                Power.RebootDevice();
        }

        public ESP32 Build()
        {
            return _product;
        }

    }
}

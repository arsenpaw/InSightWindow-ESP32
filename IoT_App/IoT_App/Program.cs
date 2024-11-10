using IoT_App;
using IoT_App.Services;
using IoT_App.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using nanoFramework.SignalR.Client;
using System.Net.Security;
using System.Diagnostics;

namespace HttpWebRequestSample
{
    public class Program
    {
        public static void Main()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            var builder = MicrocontrollerBuilder.CreateBuilder(services);

            builder
                .AddDht11()
                .AddWaterSensor()
                .ConnectToWifi("PC", "123456789")
                .ConfigureServiceConnection(
                    AppSettings.HUB_URL,
                    new HubConnectionOptions()
                    {
                        Certificate = new X509Certificate(AppSettings.dstRootCAX3),
                        SslVerification = SslVerification.NoVerification,
                        SslProtocol = SslProtocols.Tls12,
                        Reconnect = true,
                    });

            var esp32 = builder.Build();
            esp32.StartConnection();
            esp32.SubscribeToServerReceiveData();
            
            var data = esp32.ComposeAllDataInfo();
            esp32.SendDataFromSensorToServer(data);

            Debug.WriteLine("ESP32 is ready to send data to server");

        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IAesService), typeof(AesService));
        }
    }
}

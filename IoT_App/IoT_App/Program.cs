using IoT_App;
using IoT_App.Builder;
using IoT_App.Command;
using IoT_App.Observer;
using IoT_App.Sensors;
using IoT_App.Services;
using Microsoft.Extensions.DependencyInjection;
using nanoFramework.SignalR.Client;
using System.Device.Adc;
using System.Diagnostics;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using static IoT_App.AppSettings;
namespace HttpWebRequestSample
{
    public class Program
    {
        //TODO State pattern to change the state of esp32
        public static void Main()
        {
            var services = new ServiceCollection();

            var builder = MicrocontrollerBuilder.CreateBuilder(services);
            builder.Services.AddSingleton(typeof(AdcController));
            builder.Services.AddSingleton(typeof(IAesService), typeof(AesService));
            services.AddSingleton(typeof(IEventObserver), typeof(EventObserver));
            services.AddSingleton(typeof(MagnetSensor));
            builder.Services.AddCommandServices();
            builder.AddFlashStorage();
            builder.AddDht11();
            builder.AddStepMotor(stepPin1, stepPin2, stepPin3, stepPin4);
            builder.AddWaterSensor();
            builder.AddMotorManager();
            builder.ConnectToWifi("PC", "123456789");
            builder.ConfigureServiceConnection(
                    HUB_URL,
                    new HubConnectionOptions()
                    {
                        Certificate = new X509Certificate(AppSettings.dstRootCAX3),
                        SslVerification = SslVerification.NoVerification,
                        SslProtocol = SslProtocols.Tls12,
                        Reconnect = true,
                    });

            var esp32 = builder.Build();
            esp32.SubscribeOnEvents();
            new Thread(() => esp32.SubscribeToServerReceiveData()).Start();
            esp32.StartConnection();
            Debug.WriteLine("ESP32 is ready to send data to server");
            while (true)
            {
                var data = esp32.ComposeDataFromSensor();
                new Thread(() => esp32.SendDataFromSensorToServer(data)).Start();
                Debug.WriteLine($"Is open:{data.IsOpen}");
                Thread.Sleep(1000);

            }
        }


    }
}

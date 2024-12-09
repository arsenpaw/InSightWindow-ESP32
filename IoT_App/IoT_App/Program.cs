using IoT_App;
using IoT_App.Services;
using IoT_App.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography.X509Certificates;
using nanoFramework.SignalR.Client;
using System.Net.Security;
using System.Diagnostics;
using System.Device.Adc;
using System.Threading;
using IoT_App.Motor;
using static IoT_App.AppSettings;
using IoT_App.Command;
using nanoFramework.Runtime.Events;
using IoT_App.Observer;
using System.Device.Gpio;
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
            services.AddScoped(typeof(GpioController));
            builder.Services.AddCommandServices();
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


            while (true)
            {
                var data = esp32.ComposeDataFromSensor();
                esp32.SendDataFromSensorToServer(data);
                Thread.Sleep(5000);
                Debug.WriteLine("ESP32 is ready to send data to server");
            }
        }


    }
}

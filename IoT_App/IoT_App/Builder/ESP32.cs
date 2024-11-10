using IoT_App.Models;
using IoT_App.Sensors;
using IoT_App.Services;
using nanoFramework.Json;
using nanoFramework.SignalR.Client;
using System;
using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace IoT_App.Builder
{
    public class ESP32
    {


        public HubConnection HubConnection { get; set; }

        public IAesService AesService { get; set; }

        public DHT11 DHT11 { get; set; }

        public WaterSensor WaterSensor { get; set; }

        public ESP32(IAesService aesService, HubConnection hubConnection , DHT11 dHT11, WaterSensor waterSensor)
        {
            AesService = aesService;
            HubConnection = hubConnection;
            WaterSensor = waterSensor;
            DHT11 = dHT11;
        }


        public AllSensorData AllSensorData { get; set; } = new AllSensorData();

        public void StartConnection()
        {
            HubConnection.Start();
        }

        public void SendDataFromSensorToServer(AllSensorData data)
        {
            if (!AllSensorData.IsDataChanged)
            {
                Debug.WriteLine("No data changed");
                return;
            }
            string deserialized = JsonConvert.SerializeObject(data);
            var encryptedBytes = AesService.EncryptData(deserialized);
            var encryptedBase64 = Convert.ToBase64String(encryptedBytes);
            var res = HubConnection.InvokeCoreAsync("ReceiveDataFromEsp32", typeof(int), new object[] { encryptedBase64 });
            Debug.WriteLine($"SendDataFromSensorToServer: {res.Value}");

        }
        public void SubscribeToServerReceiveData()
        {
            HubConnection.On("ReceiveCommand",new Type []{ typeof(Command) }, (sender, args) =>
            {
                var commend = args[0] as Command;
                //do something with the data
            });
        }
        public AllSensorData ComposeAllDataInfo()
        {
            AllSensorData.ResetDataChangedFlag();

            DHT11.SetNext(WaterSensor);
            //continue to add more sensors

            DHT11.DataCompose(AllSensorData);

            return AllSensorData;


        }
    }
}

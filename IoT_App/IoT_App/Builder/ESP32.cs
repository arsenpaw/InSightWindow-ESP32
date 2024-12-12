using IoT_App.Command;
using IoT_App.Models;
using IoT_App.Models.Store;
using IoT_App.Observer;
using IoT_App.Sensors;
using IoT_App.Services.Interfaces;
using nanoFramework.Json;
using nanoFramework.SignalR.Client;
using System;
using System.Diagnostics;

namespace IoT_App.Builder
{
    public class ESP32
    {
        private HubConnection HubConnection { get; set; }

        private IAesService AesService { get; set; }

        private IServiceProvider ServiceProvider { get; set; }

        private IEventObserver EventObserver { get; set; }

        private IFlashStorage flashStorage { get; set; }

        private DHT11 DHT11 { get; set; }

        private WaterSensor WaterSensor { get; set; }

        private MagnetSensor MagnetSensor { get; set; }

        public AllSensorData AllSensorData { get; set; } = new AllSensorData();

        public ESP32(IAesService aesService, HubConnection hubConnection,
            DHT11 dHT11, WaterSensor waterSensor,
            IServiceProvider serviceProvider, IEventObserver eventPublisher,
            IFlashStorage flashStorage, MagnetSensor magnetSensor)
        {
            MagnetSensor = magnetSensor;
            AesService = aesService;
            HubConnection = hubConnection;
            WaterSensor = waterSensor;
            DHT11 = dHT11;
            ServiceProvider = serviceProvider;
            EventObserver = eventPublisher;
            this.flashStorage = flashStorage;
        }

        public void SubscribeOnEvents() => EventObserver.EnableEventHandling();


        public void StartConnection() =>
            HubConnection.Start();

        public void test()
        {
        }

        public void SendDataFromSensorToServer(AllSensorData data)
        {
            if (!AllSensorData.IsDataChanged || HubConnection.State != HubConnectionState.Connected)
            {
                return;
            }
            string deserialized = JsonConvert.SerializeObject(data);
            var encryptedBytes = AesService.EncryptData(deserialized);
            var encryptedBase64 = Convert.ToBase64String(encryptedBytes);
            var res = HubConnection.InvokeCoreAsync("ReceiveDataFromEsp32", typeof(int), new object[] { encryptedBase64 });
            Debug.WriteLine($"SendDataFromSensorToServer: {res.Value}");

        }
        private string _decryptRequest(string rawCryptedRequestBody)
        {
            var str = (rawCryptedRequestBody.Trim('"'));
            var encryptedBytes = Convert.FromBase64String(str);
            var decryptedData = AesService.DecryptData(encryptedBytes);
            return decryptedData;
        }
        public void SubscribeToServerReceiveData()
        {
            HubConnection.On("Settings", new Type[] { typeof(string) }, (sender, args) =>
            {
                var encryptedData = (args[0] as string);
                var userSettings = (UserSettings)JsonConvert.DeserializeObject(_decryptRequest(encryptedData), typeof(UserSettings));
                flashStorage.SetUserSettings(userSettings);
            });
            HubConnection.On("ReceiveCommand", new Type[] { typeof(string) }, (sender, args) =>
            {

                var encryptedData = (args[0] as string);
                var command = (CommandDto)JsonConvert.DeserializeObject(_decryptRequest(encryptedData), typeof(CommandDto));
                if (command == null)
                {
                    Debug.WriteLine("command is null");
                }

                var commandService = ServiceProvider.GetServiceByCommand(command.Command);
                commandService.Execute();
            });
        }

        public AllSensorData ComposeDataFromSensor()
        {
            AllSensorData.ResetDataChangedFlag();

            DHT11.SetNext(WaterSensor);
            DHT11.SetNext(MagnetSensor);
            //continue to add more sensors

            DHT11.DataCompose(AllSensorData);

            return AllSensorData;

        }
    }
}

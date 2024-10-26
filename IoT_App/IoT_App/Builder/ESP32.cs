using IoT_App.Models;
using IoT_App.Sensors;
using nanoFramework.Json;
using nanoFramework.SignalR.Client;
using System;
using System.Collections;
using System.Text;

namespace IoT_App.Builder
{
    public class ESP32
    {
        public DHT11 DHT11 { get; set;}

        public WaterSensor WaterSensor { get; set; }

        public AllSensorData AllSensorData { get; set; } = new AllSensorData();

        public HubConnection HubConnection { get; set; }



        public void SendDataToServer()
        {
            string deserialized = JsonConvert.SerializeObject(AllSensorData);
            AsyncResult dashboardClientConnected = HubConnection.InvokeCoreAsync("SendWidnowStatusToClient", null, new object[] { deserialized });

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

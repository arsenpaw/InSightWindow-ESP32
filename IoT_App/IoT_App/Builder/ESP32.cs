﻿using IoT_App.Models;
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
        public DHT11 DHT11 { get; set;}

        public WaterSensor WaterSensor { get; set; }

        public AllSensorData AllSensorData { get; set; } = new AllSensorData();

        public HubConnection HubConnection { get; set; }

        public IAesService AesService { get; set; }

        public void SendDataToServer()
        {
            string deserialized = JsonConvert.SerializeObject(AllSensorData);
            var t = AesService.EncryptData(deserialized);
            var g = Convert.ToBase64String(t);
         
            var res = HubConnection.InvokeCoreAsync("ReceiveDataFromEsp32", typeof(int), new object[] { g });
            Debug.WriteLine($"SendDataToServer: {res}");

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

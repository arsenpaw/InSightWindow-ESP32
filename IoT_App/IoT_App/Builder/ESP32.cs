using IoT_App.Models;
using IoT_App.Sensors;
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

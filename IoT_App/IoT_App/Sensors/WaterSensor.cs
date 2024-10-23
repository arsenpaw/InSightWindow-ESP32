using IoT_App.ChainOfResponcebility;
using IoT_App.Models;
using System;
using System.Text;

namespace IoT_App.Sensors
{
    public class WaterSensor : DataComposeHandler, ISensor
    {
        public int WaterLevel { get; set; }

        public bool IsRain { get; set; }

        public ISensor ReadData()
        {
            WaterLevel = 50;
            IsRain = true;
            return this;
        }

        public override object DataCompose(AllSensorData request)
        {
            ReadData();
            request.IsRain = IsRain;
            return base.DataCompose(request);
        }
    }
}
